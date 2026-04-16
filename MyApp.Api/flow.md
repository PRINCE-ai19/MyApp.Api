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

**Tóm lại cho Intern dễ nhớ:**
- JWT giống như cái **Thẻ Nhân Viên** để vô công ty. Lúc xin thì cần **Mã nhân viên / Pass**. 
- Có thẻ rồi, mỗi lần qua cổng cứ quẹt thẻ. Máy (JWT Middleware) sẽ tự động coi dấu mộc (Signature - verify bằng `SecretKey`) có đúng do công ty cấp không, thẻ hết hạn (Expiration) chưa.
- Thẻ xài tầm 15 phút là bị hết hạn (do Rule công ty khắc nghiệt =]]). Khi đó phải dùng **Giấy xác nhận gia hạn (Refresh token)** mang tới phòng HCĐN để xin cấp cái Thẻ (Access Token) mới chứ không phải nộp lại Form xin việc (User/Pass).

Là lập trình viên .NET, em chỉ cần học làm quen với thư viện `System.IdentityModel.Tokens.Jwt` là có thể tự tay Generate token rồi nhé. Nếu có chỗ nào chưa rõ khi coi code `AuthService` hay `Program.cs` thì nhắn anh. Code vui nha!
