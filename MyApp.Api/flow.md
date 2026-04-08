# Tài liệu chi tiết Luồng Đa ngôn ngữ (Localization)

Tài liệu này giải thích chi tiết cách triển khai tính năng đa ngôn ngữ cho hệ thống Validation trong project Clean Architecture của bạn.

---

## 1. Kiến trúc tổng thể
Chúng ta triển khai Localization theo mô hình **Shared Resources** (Tài nguyên dùng chung). Thay vì mỗi Class có một file ngôn ngữ riêng, chúng ta tập hợp tất cả thông báo vào một nơi để dễ quản lý.

-   **Marker Class**: `SharedResource.cs` đóng vai trò là "mỏ neo" để .NET biết nơi bắt đầu tìm kiếm các file `.resx`.
-   **Resource Files**: Các file `.resx` (XML) chứa các cặp Key-Value cho từng ngôn ngữ.
-   **DTOs**: Sử dụng các "Key" thay vì chuỗi văn bản cố định.

---

## 2. Các bước triển khai chi tiết

### Bước 1: Tạo Marker Class và Resource Files
Chúng ta đặt tài nguyên tại tầng **Application** vì đây là nơi chứa các DTO và Logic nghiệp vụ cần thông báo lỗi.

-   **File**: `MyApp.Application/Resources/SharedResource.cs`
-   **Nhiệm vụ**: Class này phải là `public` và trống (không cần code). Nó chỉ dùng để lấy thông tin về `Namespace` và `Assembly`.
-   **Các file ngôn ngữ**: `SharedResource.vi.resx` (Tiếng Việt) và `SharedResource.en.resx` (Tiếng Anh). Những file này phải nằm **cùng thư mục** với marker class.

### Bước 2: Cấu hình trong Service (Program.cs)
Tại project Web API, chúng ta cần đăng ký các dịch vụ cần thiết:

1.  **AddLocalization**: Đăng ký dịch vụ bản địa hóa cơ bản.
    ```csharp
    builder.Services.AddLocalization();
    ```
    *Lưu ý: Không cần truyền `ResourcesPath` nếu các file `.resx` đã nằm đúng cấu trúc thư mục/namespace của marker class.*

2.  **AddDataAnnotationsLocalization**: Cấu hình để các thuộc tính như `[Required]`, `[StringLength]` trong DTO biết tìm thông báo ở đâu.
    ```csharp
    .AddDataAnnotationsLocalization(op => {
        op.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });
    ```
    *Dòng này cực kỳ quan trọng: Nó ép tất cả các lỗi Validation phải đi qua file `SharedResource` để tìm chuỗi dịch.*

### Bước 3: Cấu hình Middleware (Xử lý Request)
Để hệ thống biết người dùng muốn dùng ngôn ngữ nào, chúng ta dùng `RequestLocalizationOptions`:

-   **Default Culture**: `vi-VN` (Tiếng Việt).
-   **Supported Cultures**: Danh sách các ngôn ngữ hệ thống trợ giúp (`vi-VN`, `en-US`).
-   **Middleware**: `app.UseRequestLocalization(localizationOptions);`
    *Middleware này sẽ kiểm tra Query String (`?culture=...`), Cookie, hoặc Accept-Language Header để quyết định ngôn ngữ.*


## 3. Luồng hoạt động khi có Request (Flow)

1.  **Request gửi đến**: Người dùng gọi `POST /api/Categories?culture=en-US`.
2.  **Nhận diện ngôn ngữ**: Middleware `RequestLocalization` đọc query `culture=en-US` và thiết lập `CultureInfo.CurrentCulture` của luồng xử lý thành tiếng Anh.
3.  **Validation**: ASP.NET Core kiểm tra DTO. Nó thấy thuộc tính `[Required(ErrorMessage = "NameRequired")]`.
4.  **Tìm kiếm chuỗi dịch**:
    -   Hệ thống gọi đến `SharedResource`.
    -   Dựa trên `en-US`, nó tìm đến file `SharedResource.en.resx`.
    -   Nó tìm Key `NameRequired` và lấy ra giá trị: `"Name is required"`.
5.  **Trả về kết quả**: Nếu có lỗi, API trả về mã 400 kèm thông báo đã được dịch.

---

## 4. Các lưu ý quan trọng để không bị lỗi

> [!WARNING]
> **Namespace và Thư mục**: Namespace của `SharedResource.cs` phải khớp hoàn toàn với cấu trúc thư mục chứa file `.resx`. Ví dụ: Folder là `Resources` thì Namespace phải kết thúc bằng `.Resources`.

> [!IMPORTANT]
> **Dữ liệu Database**: Localization của .NET **không** dịch dữ liệu trong Database. Nó chỉ dịch các chuỗi "tĩnh" mà bạn đã định nghĩa trong file `.resx`.

> [!TIP]
> **Rebuild**: Mỗi khi bạn thêm hoặc sửa file `.resx`, hãy **Rebuild** lại Project để đảm bảo các tài nguyên được nhúng (Embed) vào file DLL mới nhất.
