# Giải thích cơ chế JWT (JSON Web Token) cho Thực tập sinh (Intern)

Chào em, với tư cách là một Senior, anh sẽ giải thích cho em về cách cơ chế xác thực rà soát (Authentication/Authorization) hoạt động trong dự án `MyApp.Api` của chúng ta sử dụng JWT nhé. Cách giải thích này sẽ đi từ cơ bản đến cách nó áp dụng thực tế vào codebase hiện tại.

## 1. JWT là gì?
**JWT (JSON Web Token)** là một tiêu chuẩn mở (RFC 7519) quy định cách truyền tải thông tin an toàn giữa các bên dưới dạng JSON object. Thông tin này có thể được xác minh và đáng tin cậy vì nó có chứa chữ ký số (digital signature).

Hãy tưởng tượng JWT giống như một **"Chiếc thẻ ra vào" (VIP Pass)**. Bạn đưa thông tin cá nhân (Username/Password) cho bảo vệ kiểm tra, nếu đúng, bảo vệ cấp cho bạn một cái Thẻ. Từ đó về sau, bạn muốn vào các phòng trong tòa nhà (gọi API), bạn chỉ cần đưa Thẻ này ra thay vì cứ phải khai báo lại Username/Password.

## 2. Cấu trúc của một Token JWT
Một JWT thực chất là một chuỗi string khá dài gồm 3 phần, được phân cách bởi dấu chấm `.`:
`Header.Payload.Signature`

- **Header**: Chứa thông tin về loại token (là JWT) và thuật toán mã hóa (VD: HMAC SHA256).
- **Payload (Data)**: Chứa các "Claims" (những thông tin mình muốn đính kèm vào token). Ví dụ: `UserId`, `Role`, `UserName`. (Lưu ý: Không bỏ mật khẩu hay dữ liệu nhạy cảm vào đây vì ai cũng có thể giải mã phần này trên Frontend).
- **Signature (Chữ ký)**: Đây là phần quan trọng nhất. Nó được tạo ra bằng cách gộp Header, Payload và 1 **SecretKey** (Chìa khóa bí mật chỉ Server biết) rồi mã hóa bằng thuật toán khai báo ở Header. Giúp Server xác nhận xem Token này có bị giả mạo trên đường truyền hay không.

## 3. Cấu hình JWT trong `MyApp.Api`
Nếu em mở file `appsettings.json`, em sẽ thấy cấu hình JWT của dự án chúng ta:

```json
"JwtSettings": {
  "SecretKey": "MySuperSecretKey_2026_!@#_VerySecure_123456789",
  "Issuer": "MyApp_Backend",
  "Audience": "MyApp_Frontend",
  "AccessTokenExpiration": 15,    // Token sống 15 phút
  "RefreshTokenExpiration": 43200 // Refresh token sống 30 ngày (tính bằng phút)
}
```

- **SecretKey**: Là khóa bí mật để ký Token (giống như con dấu của bảo vệ). Nếu ai/hacker có được chìa khóa này, họ có thể vào vai Server phát hành Token giả để chiếm toàn quyền. Do đó, phải giữ tuyệt mật tuyệt đối!
- **Issuer (Người cấp)**: Tên của hệ thống phát hành ra cái token này, ở đây là `MyApp_Backend`.
- **Audience (Người nhận)**: Tên của hệ thống dùng token này, ở đây quy ước là `MyApp_Frontend`.
- **AccessTokenExpiration**: Thời gian sống của Access Token (15 phút). Vì thẻ này có quyền lực cao, dùng trực tiếp để gọi API nên lỡ bị trộm thì cũng chỉ xài được 15 phút rồi hết hạn, tăng tính bảo mật.
- **RefreshTokenExpiration**: Thời gian sống của Refresh Token (thường lâu hơn, VD: 30 ngày). Khi Access Token hết hạn, mình dùng Refresh Token này để xin Backend cấp lại Access Token mới mà không bắt User phải gõ lại Password.

## 4. Luồng hoạt động (Flow) trong dự án

### Bước 1: Đăng nhập (Login)
1. **Client (Frontend/Postman)** gửi Request POST lên Endpoint `/api/auth/login` gồm `Username` và `Password`.
2. **Backend (MyApp.Api)** nhận request, kiểm tra trong DB (Database SQL Server qua repo Dapper) xem User này có tồn tại và đúng Password (đã hash mã hóa) hay không.
3. Nếu đúng => Backend dùng `SecretKey` để tạo ra 2 giá trị là:
   - `AccessToken` (mang thông tin User, Role... sống 15 phút).
   - `RefreshToken` (1 chuỗi random string lưu vào DB để cho phép làm mới Access Token).
4. Backend trả về JSON chứa cặp token này cho Client.

### Bước 2: Gọi các API được bảo vệ (Call Secured API)
1. Để lấy danh sách Category hay Product (các API bị khóa, yêu cầu đăng nhập, thường có gắn attribute `[Authorize]`), **Client** phải gắn `AccessToken` lấy được từ Bước 1 vào **Header** của Request HTTP theo cú pháp:
   ```http
   Authorization: Bearer <chuỗi_access_token>
   ```
2. **Backend** nhận được Request. Middleware `JwtBearer` của ASP.NET Core sẽ "chặn" Request này lại trước khi cho vào Controller để kiểm tra:
   - Token có đúng định dạng không?
   - Token hết hạn chưa (còn trong 15 phút không)?
   - Chữ ký (Signature) có đúng được tạo ra từ `SecretKey` của Server không? Bị sửa đổi giữa chừng không?
   - `Issuer` và `Audience` có đúng như cài đặt trong config không?
3. Nếu tất cả đều OK (Hợp lệ) => Cho phép Request đi tiếp vào `Controller`. Khi đó trong code, em có thể lấy thông tin User đang Request qua biến `HttpContext.User`. Ngược lại, nếu sai rớt ở bất cứ điều khoản nào => Trả về lỗi Http Status Code `401 Unauthorized` ngay lập tức.

### Bước 3: Làm mới Token (Refresh Token) 
Khi `AccessToken` bị hết thời hạn 15 phút:
1. Client cố gắng gọi vào API => Backend trả về lỗi `401 Unauthorized`.
2. Lúc này Frontend sẽ ngầm (không để User biết) gọi lên Endpoint API ví dụ: `/api/auth/refresh-token` gửi cái chuỗi `RefreshToken` (đã lưu ở phía FE).
3. Backend kiểm tra `RefreshToken` này trong Database xem: có đúng mặt mã này không? bị vô hiệu hóa (revoked) chưa? còn hạn (trong vòng 30 ngày) không?
4. Nếu hợp lệ, hệ thống tạo ra cặp `AccessToken` và `RefreshToken` mới tinh và trả ngược lại cho Client (đồng thời vô hiệu hóa bộ cũ).
5. Frontend lưu lại cái mới, và dùng cái `AccessToken` mới đó tự động gọi lại cái API ban nãy bị `401`. Toàn bộ luồng này User không hề hay biết và họ không phải trải qua cảnh cứ 15 phút bị văng ra bắt nhập lại mật khẩu.

---

## 5. Giải thích mã nguồn (.cs) trong thực tế (AuthService.cs)

Bây giờ anh em mình dòm thẳng vào file `MyApp.Application/Services/AuthService.cs` nhé, anh sẽ phân tích cho em đoạn code Đăng ký & Đăng nhập hoạt động như thế nào, và mật mã được an toàn ra sao.

### 5.1. Khi người dùng Đăng ký (RegisterAsync)

```csharp
public async Task<SpResponse> RegisterAsync(RegisterRequest request)
{
    var user = new User
    {
        Username = request.Username,
        // Dùng thư viện BCrypt để mã hóa (Hash) Password!
        PasswordHass = BCrypt.Net.BCrypt.HashPassword(request.Password),
        FullName = request.FullName,
        Email = request.Email,
        Role = "User"
    };

    return await _userRepo.RegisterAsync(user);
}
```

**Thuật toán mã hóa BCrypt là gì và tại sao lại dùng nó?**
- Trong dự án này (và mọi dự án nghiêm túc), mật khẩu **không bao giờ** được lưu dưới dạng chữ bình thường (plaintext) như chữ `123456`. 
- Đoạn code `BCrypt.HashPassword` sẽ dùng thuật toán mã hóa một chiều **BCrypt** để biến chuỗi `123456` thành một chuỗi mã hóa (Hash String) như vầy: `$2a$11$N9lkxyz...`.
- Lưu ý: Đây là mã hóa **MỘT CHIỀU**, tức là từ cái chuỗi `$2a$11...` đó, **KHÔNG MỘT AI** (kể cả Tech Lead hay Hacker hack được Database) có thể dịch ngược lại ra chữ `123456` được! Điều này bảo vệ an toàn tuyệt đối cho người dùng nếu DB bị lộ.

### 5.2. Khi người dùng Đăng nhập (LoginAsync)

Vậy khi User nhập chữ "123456" để đăng nhập, làm sao Backend biết đó là đúng nếu đã không thể giải ngược mã?

```csharp
public async Task<LoginResponse> LoginAsync(LoginRequest request)
{
    // Bước 1: Lấy tài khoản từ DB lên theo Username
    var user = await _userRepo.GetUserByUsernameAsync(request.Username);

    // Bước 2: Dùng BCrypt.Verify để kiểm tra Mật khẩu
    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHass))
    {
        throw new Exception(_localizer["InvalidCredentials"]); // Lỗi: Sai Username / Password!
    }

    // ... (Code check tài khoản khoá)

    // Bước 3: Nếu đúng, gen ra cặp Token
    var accessToken = _jwtService.GenerateAccessToken(user);
    var refreshToken = _jwtService.GenerateRefreshToken();

    // Bước 4: Cập nhật RefreshToken mới vô Table User, hạn 7 ngày
    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
    await _userRepo.UpdateUserRefreshTokenAsync(user);

    // Trả về cho bên Frontend
    return new LoginResponse
    {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        Username = user.Username
    };
}
```

**Logic kì diệu nằm ở dòng lệnh `BCrypt.Verify(...)`**:
1. Hàm `Verify()` sẽ tự động mang chuỗi mật khẩu thô ráp `123456` của người dùng nhập vào.
2. Nó dùng đúng "gia vị băm" (Salt) nằm lẫn trong chuỗi `$2a$11...` của DB để băm con `123456` kia lại 1 lần nữa.
3. Chạy thuật toán xong, nếu kết quả trả ra GIỐNG HỆT với chuỗi băm nằm dưới DB => Có nghĩa người dùng đã nhập **đúng** mật khẩu thô ban đầu => **Xác thực thành công!**.
4. Lúc này Server mới tự tin nhờ `_jwtService` vẽ bùa ra cái `AccessToken` và trả về cho Client chép vào tay! Đồng thời lưu cái `RefreshToken` vào DB với thời hạn là 7 ngày (`AddDays(7)`) để sau dùng cho luồng Làm mới Token!

---

## 6. Giải thích chi tiết Code tạo Token (JwtRepository.cs)

Bây giờ anh em mình cùng "mổ xẻ" file `MyApp.Infrastructure/Services/JwtService.cs` (nơi chứa class `JwtRepository`) để xem các bước cụ thể khi Server "vẽ bùa" ra cái Token nhé.

### 6.1. Hàm tạo Access Token (`GenerateAccessToken`)

Đây là hàm quan trọng nhất, nơi biến thông tin User thành chuỗi JWT có chữ ký bảo mật.

```csharp
public string GenerateAccessToken(User user)
{
    // Bước 1: Tạo danh sách các "Claims" (Thông tin đính kèm)
    // Claim giống như những "nhãn dán" thông tin dán lên tấm thẻ
    var claims = new List<Claim>
    {
         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ID của User (để biết là ai)
         new Claim(ClaimTypes.Name, user.Username),               // Tên đăng nhập
         new Claim(ClaimTypes.Role, user.Role ?? "User")          // Quyền hạn (để check Permission)
    };

    // Bước 2: Lấy SecretKey từ appsettings.json và tạo Khóa bảo mật (Symmetric Key)
    // Encoding.UTF8.GetBytes biến chuỗi text bí mật thành mảng byte để máy tính tính toán
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));

    // Bước 3: Khai báo Thuật toán mã hóa & Chữ ký (HmacSha256)
    // SigningCredentials là sự kết hợp giữa Chìa khóa và Thuật toán để "đóng dấu"
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    // Bước 4: Tạo đối tượng Token với đầy đủ thông tin (Metadata)
    var token = new JwtSecurityToken(
        issuer: _config["JwtSettings:Issuer"],   // Server phát hành (MyApp_Backend)
        audience: _config["JwtSettings:Audience"], // Nơi sử dụng (MyApp_Frontend)
        claims: claims,                            // Thông tin User vừa tạo ở Bước 1
        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtSettings:AccessTokenExpiration"])), // Thời hạn 15p
        signingCredentials: creds                  // Chữ ký bảo mật tạo ở Bước 3
    );

    // Bước 5: Chuyển đối tượng Token (Object) thành chuỗi String (Chuỗi 3 phần)
    // Hàm WriteToken này sẽ gộp Header.Payload.Signature lại cho mình
    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

**Tại sao phải làm nhiều bước vậy?**
- **Claims**: Giống như thông tin in trên thẻ. Khi Client gửi Token lên các API sau này, Server chỉ việc "bóc" cái nhãn này ra là biết ngay "Ông này là User ID 10, Role là Admin", khỏi cần vào DB tìm lại cho mất công.
- **SigningCredentials**: Đây là linh hồn của bảo mật. Nhờ có `SecretKey` phối hợp với thuật toán, nếu hacker cố tình sửa Role từ `User` thành `Admin` trong Token, chữ ký Signature sẽ sai ngay lập tức vì hacker không có `SecretKey` của mình.

### 6.2. Hàm tạo Refresh Token (`GenerateRefreshToken`)

Khác với Access Token (là 1 JWT phức tạp), Refresh Token ở dự án mình đơn giản chỉ là một chuỗi "mã số bí mật" ngẫu nhiên.

```csharp
public string GenerateRefreshToken()
{
    // Tạo một chuỗi ngẫu nhiên (GUID) kết hợp với mốc thời gian (Ticks)
    // Kết quả: "a1b2c3d4... + 6384883..." (Cực kỳ khó đoán và duy nhất)
    return Guid.NewGuid().ToString() + DateTime.Now.Ticks;
}
```

**Tại sao Refresh Token lại đơn giản hơn Access Token?**
- Vì bản thân chuỗi này không mang dữ liệu gì cả. Nó chỉ đóng vai trò là một **"Mã số đối soát"**.
- Mã này được lưu cứng vào Database (cột `RefreshToken`). 
- Khi thẻ Access Token hết hạn, Client đưa cái mã này lên. Server chỉ việc vào DB tìm: "Có ai đang cầm mã bí mật này không?". Nếu thấy User A đang cầm mã này và mã vẫn còn hạn (7 ngày), Server mới tin tưởng và cấp Access Token mới cho User A.

---

## 7. Giải thích cấu hình Middleware (Program.cs)

Đây là phần "Trái tim" của hệ thống xác thực. Nếu thiếu phần này, Server sẽ không biết cách đọc Token em gửi lên, cũng như không biết cái Token đó có hợp lệ hay không. Em dòm vào file `Program.cs` của dự án mình nhé.

### 7.1. Đăng ký dịch vụ Xác thực (Service Registration)

Đoạn code này nằm ở phần đầu file `Program.cs`, dùng để khai báo với ASP.NET Core rằng: "Này, dự án tôi dùng JWT để kiểm soát ra vào đấy nhé!".

```csharp
builder.Services.AddAuthentication(options => {
    // 1. Quy định kiểu xác thực mặc định là JwtBearer (Mã vạch JWT)
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    // 2. Cấu hình các quy tắc để kiểm tra cái Token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,           // Kiểm tra xem có đúng "Nơi cấp" (Issuer) mình quy định không
        ValidateAudience = true,         // Kiểm tra xem có đúng "Người nhận" (Audience) mình quy định không
        ValidateLifetime = true,         // Kiểm tra xem Token còn hạn dùng không (Expiration)
        ValidateIssuerSigningKey = true, // Quan trọng nhất: Kiểm tra "Dấu mộc/Chữ ký" xem có khớp với SecretKey không

        // Lấy các giá trị từ file appsettings.json để đối soát
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});
```

**Giải thích cho Intern dễ hiểu:**
- `AddAuthentication`: Giống như việc công ty lắp đặt một cái **Máy Quẹt Thẻ** ở cổng chính.
- `TokenValidationParameters`: Chính là **Bộ quy tắc** nạp vào máy. Ví dụ: Thẻ phải có logo công ty (Issuer), Thẻ phải còn hạn (Lifetime), và quan trọng nhất là tấm hình trên thẻ phải có độ phân giải và mã hóa đúng kiểu của máy (Signature). Nếu thiếu một trong các yếu tố này, máy sẽ báo "Tít tít" (Lỗi 401) và không mở cửa.

### 7.2. Kích hoạt Middleware trong Pipeline (Request Pipeline)

Sau khi mua máy về (Đăng ký dịch vụ), mình phải cắm điện và đặt nó đúng vị trí trong hành lang đi lại của Request.

```csharp
// ... (Các middleware khác như CORS, Localization)

app.UseAuthentication(); // 1. Xác thực: "Anh là ai?" (Kiểm tra thẻ)

app.UseAuthorization();  // 2. Phân quyền: "Anh được vào đây không?" (Kiểm tra Role Admin/User trên thẻ)

// ... (Sau đó mới tới Controllers)
app.MapControllers();
```

**Lưu ý cực kỳ quan trọng về Thứ tự (Order):**
- **`app.UseAuthentication()` phải nằm TRƯỚC `app.UseAuthorization()`**. Vì em phải biết họ là ai cái đã, rồi mới quyết định xem họ có quyền vào phòng Admin hay không.
- Cả hai cái này phải nằm **TRƯỚC `app.MapControllers()`**. Để khi request chạy tới các hàm xử lý của em (Controller), nó đã mang sẵn thông tin "Thân phận" (`User.Identity`) rồi.

---

**Tóm lại cho Intern dễ nhớ:**
- JWT giống như cái **Thẻ Nhân Viên** để vô công ty. Lúc rải CV đăng ký thì cần viết **Mã nhân viên / Pass**. 
- Để bảo đảm an toàn sổ sách, bảo vệ công ty không chép Pass của em ra sổ, mà dùng cối xay thịt **BCrypt** băm (hash) Pass đó ra và lưu vào sổ dưới dạng cục thịt băm (`PasswordHass`). 
- Lúc đến cổng ngày đầu, em báo Mật khẩu. Ông bảo vệ lại băm thử nghiệm đúng cái con Mật khẩu em vừa báo. Khớp cục thịt băm trong sổ (`BCrypt.Verify()`) thì ok cho đi làm cái Thẻ mộc đỏ (Gen Token `AccessToken`).
- Có Thẻ rồi, trưa đi ăn đi dạo các phòng ban (gọi API) em khỏi đọc Pass, cứ đưa Thẻ (Access Token) cho **Máy quẹt thẻ (Middleware)** quẹt cái "Tít".
- Máy soát thẻ (JWT Middleware - `AddJwtBearer`) sẽ tự động coi dấu mộc trên Cục thẻ có đúng của trường (Issuer) và không bị sửa chữa không (dùng `SecretKey` trên appsetting), cũng như là thẻ có bị hết hạn 15p (Expiration) hay chưa.
- Nếu thẻ OK, nó sẽ cho phép em đi tiếp qua cửa **Phân quyền (Authorization)** để xem em là Nhân viên hay Sếp (Role) rồi mới cho vào phòng tương ứng.
- Nếu Thẻ hết hạn, hãy dùng **Giấy xác nhận (Refresh Token)** cầm lên phòng Hành chính để họ đối chiếu sổ sách Database, nếu khớp họ sẽ in cho em cái Thẻ mới!

Là lập trình viên .NET, em chỉ cần học làm quen với thư viện `System.IdentityModel.Tokens.Jwt` (cho JWT), `BCrypt.Net-Next` (cho Hashing) và cách cấu hình Middleware trong `Program.cs` là có thể cứng cáp phần xác thực Authentication này rồi nhé. Code vui nha!
