
# Tiến độ công việc - 08/04/2026 (Cập nhật 17:40 PM)

## 1. Backend (API & Application) - [HOÀN THÀNH]
- [x] **AutoMapper & DTO**:
    - [x] Đăng ký AutoMapper assembly trong `Program.cs`.
    - [x] Sửa lỗi `MappingException` (Category -> Category_DTO) bằng `.ReverseMap()`.
    - [x] Bổ sung trường `Id` vào `Category_DTO`.
    - [x] Đồng bộ hóa `ProductDetailDTO`.
- [x] **Đa ngôn ngữ (Localization) - Giai đoạn 2**:
    - [x] Tích hợp `IStringLocalizer<SharedResource>` vào `ProductService` và `CategoryService`.
    - [x] Chuyển đổi toàn bộ logic `throw new Exception` sang sử dụng Resource Keys (ví dụ: `CategoryRequired`, `DuplicateProductCode`).
    - [x] Quốc tế hóa Validation Messages trong các DTO (`Product_DTO`, `ProductUpdateDto`, `Category_DTO`, `CategoryUpdateDto`).
- [x] **Cải thiện API & Repository**:
    - [x] Chuẩn hóa lại Route API cho tường minh hơn (Product & Category).
    - [x] Refactor `ICategoryRepository`: Trả về `IEnumerable<Category>` thay vì `dynamic`.
    - [x] Cập nhật logic lọc `RecordStatus` trong Repository để đồng nhất dữ liệu.

## 2. Frontend (Giao diện & Tiện ích) - [ĐANG TRIỂN KHAI]
- [x] **Tính năng đa ngôn ngữ trên UI**:
    - [x] Triển khai Bộ chọn ngôn ngữ (VI/EN) trên Header.
    - [x] Tích hợp tham số `culture` vào Fetch API.
- [/] **Đồng bộ hóa Route mới**:
    - [x] Cần cập nhật lại các hàm `fetch()` trong JavaScript để khớp với các Endpoint mới.
- [ ] **Xử lý thông báo Validation**:
    - [ ] Hiển thị thông báo lỗi từ API (đã được localize) lên UI thay vì dùng `alert`.

 ###############################################################################

    # Tiến độ công việc - 10/04/2026 (Cập nhật 17:15 PM)

## 1. Chuyển đổi sang Stored Procedures (Category) - [HOÀN THÀNH]
- [x] **Thiết kế & Tài liệu**:
    - [x] Khởi tạo tài liệu [Stored Procedures.md](file:///d:/Clean_Architecture/MyApp.Api/MyApp.Api/Stored%20Procedures.md) quản lý tập trung mã SQL.
    - [x] Triển khai đầy đủ bộ SP CRUD: `sp_GetAllCategories`, `sp_GetCategoryById`, `sp_InsertCategory`, `sp_UpdateCategory`, `sp_DeleteCategory`.
- [x] **Logic Validation tại Database**:
    - [x] Tích hợp kiểm tra dữ liệu (trống tên, trùng mã, không tồn tại ID) trực tiếp trong SQL Server.
    - [x] Sử dụng `RAISERROR` và `THROW` để quăng thông báo lỗi chủ động từ Store.
    - [x] Triển khai **Soft Delete** (Xóa mềm) bằng cách cập nhật `RecordStatus = '0'`.
- [x] **Tối ưu hóa**:
    - [x] Chốt phương án gán `RecordStatus = '1'` mặc định trong Store thay vì dùng Trigger (đơn giản, dễ kiểm soát).

## 2. Kiến trúc & Backend - [HOÀN THÀNH]
- [x] **Store Repository Pattern**:
    - [x] Tách biệt hoàn toàn logic SP sang `ICategoryRepository_store` và `CategoryRepository_store` (nằm trong thư mục riêng `Repositories_Store`).
    - [x] Tích hợp **Dapper** để thực thi Stored Procedures hiệu quả hơn EF Core.
    - [x] Xây dựng class **`SpResponse`** (Domain) để chuẩn hóa việc nhận thông báo lỗi/thành công từ Database.
- [x] **API & Controller**:
    - [x] Phát triển `CategoryStoreController` với bộ Route mới (`/Rotev2/...`).
    - [x] Tích hợp **AutoMapper** để ánh xạ dữ liệu từ DTO sang Entity một cách tự động và an toàn (có cấu hình `Ignore Id` để bảo vệ dữ liệu).
- [x] **Fix lỗi hệ thống**:
    - [x] Khắc phục lỗi Duplicate Interface `IProductService`.
    - [x] Sửa lỗi Runtime Dependency Injection cho `IMapper` (chuyển từ class `Mapper` sang interface `IMapper`).
    - [x] Giải quyết triệt để lỗi "Cập nhật nhưng lại thêm mới" của EF Core bằng cách dùng Store.

## 3. Các vấn đề tồn đọng & Task tiếp theo
- [ ] **Mở rộng cho Product**: Chuyển đổi các Repository của Product sang sử dụng Stored Procedures tương tự như Category.
- [ ] **Trigger & Function**: Triển khai thêm các Trigger tự động hóa dữ liệu phức tạp và các Function xử lý logic định dạng.
- [ ] **Giao diện Frontend**: Nhúng các Endpoint `/Rotev2` mới vào giao diện Admin để kiểm thử thực tế.
- [/] **Lỗi khóa file DLL**: Đang gặp lỗi file `.dll` bị process `MyApp.Api` chiếm dụng khi build lại nhanh.

---
## Tiến độ công việc - 13/04/2026 (Cập nhật 15:35 PM)

### 1. CQRS & MediatR - [HOÀN THÀNH]
- [x] **Cấu trúc Feature-based**: Triển khai Command & Query cho Category (Create, Update, Delete, GetList).
- [x] **Phát triển Command Handlers**: Xử lý logic nghiệp vụ tách biệt hoàn toàn khỏi Service truyền thống.
- [x] **MediatR Integration**: Đăng ký và cấu hình MediatR trong `Program.cs` để quản lý các Request/Response.

### 2. Logging & Hệ thống (Serilog) - [HOÀN THÀNH]
- [x] **Cấu hình Serilog MSSQL Sink**: Tích hợp ghi log trực tiếp vào SQL Server thông qua bảng `AppLogs`.
- [x] **Tự động hóa Database**: Kích hoạt `autoCreateSqlTable: true` đảm bảo hệ thống tự khởi tạo bảng log khi bắt đầu.
- [x] **Hệ thống Debugging (SelfLog)**: Bật Serilog SelfLog để bắt các lỗi nội bộ (kết nối SQL, lỗi tham số) vào file `Logs/selflog.txt`.
- [x] **Sửa lỗi cấu hình**: Khắc phục lỗi sai chính tả trong `appsettings.json` cho Sink MSSQL.

### 3. Fix Bug & Tối ưu hóa Database Layer - [HOÀN THÀNH]
- [x] **Sửa lỗi NullReferenceException**: Xử lý triệt để lỗi crash tại `DeleteCategory` khi ID không tồn tại hoặc ID=1000.
- [x] **Đảm bảo Guard Clauses**: Bổ sung các lệnh `return` hoặc `throw` ngay tại đầu hàm Service để tránh code chạy "lố".
- [x] **Stored Procedure Parameter DTOs**:
    - [x] Khởi tạo `CategoryUpdateParams` và `CategoryCreateParams` (Domain layer).
    - [x] Tối ưu hóa mapping trong `Products_mapper` để chuyển đổi tự động sang các class tham số này.
    - [x] Khắc phục lỗi "Too many arguments" của Dapper khi gọi Store.

### 4. Các công việc tiếp theo
- [ ] **Phát triển UI Admin**: Nhúng các Endpoint `/Rotev2` (Dapper/MediatR) vào giao diện React/Vue để kiểm thử đầu-cuối.
- [ ] **Chuyển đổi Product**: Tiếp tục áp dụng mô hình CQRS & MediatR cho module Sản phẩm (Product).
- [ ] **Security**: Kiểm tra lại các trường nhạy cảm trong DTO để tránh lộ dữ liệu không cần thiết lên Swagger.

---
## Tiến độ công việc - 14/04/2026 (Cập nhật 17:15 PM)

### 1. Đa ngôn ngữ (Localization) cho Stored Procedures - [HOÀN THÀNH]
- [x] **Category Module**:
    - [x] Cập nhật `sp_InsertCategory` & `sp_UpdateCategory` để trả về Resource Keys thay vì chuỗi tiếng Việt cứng.
    - [x] Tích hợp `IStringLocalizer` vào `CategoryRepository_store` để dịch thông báo từ Database.
    - [x] Bổ sung bộ Resource Keys: `DuplicateCategoryCode`, `InvalidCreatedDate`, `InsertCategorySuccess`, `UpdateCategorySuccess`.

### 2. Hệ thống Định danh & Bảo mật (Identity) - [HOÀN THÀNH]
- [x] **Sửa lỗi hệ thống**:
    - [x] Fix lỗi khai báo `IJwtService` (sai kiểu class/interface và access modifier).
    - [x] Sửa lỗi cú pháp JSON trong `appsettings.json` làm ứng dụng không khởi động được.
    - [x] Khắc phục lỗi Dependency Injection cho `IJwtRepository` trong `Program.cs`.
- [x] **Tính năng Đăng ký (Register)**:
    - [x] Triển khai `sp_RegisterUser` (validate trùng Username/Email, quản lý Transaction).
    - [x] Tích hợp mã hóa mật khẩu `BCrypt` chuẩn tại tầng Application.
    - [x] Xây dựng `RegisterRequest` DTO và hoàn thiện endpoint API.
- [x] **Refactor Đăng nhập (Login)**:
    - [x] Chốt phương án sử dụng Stored Procedure `sp_GetUserByUsername` kết hợp Dapper.
    - [x] Tích hợp kiểm tra trạng thái tài khoản (`RecordStatus`) và trả về lỗi đa ngôn ngữ (`InvalidCredentials`, `AccountLocked`).

---
## Tiến độ công việc - 15/04/2026 (Cập nhật 17:40 PM)

### 1. Hạ tầng & Helper Generic (StoreHelper) - [HOÀN THÀNH]
- [x] **Xây dựng `StoreHelper`**: 
    - [x] Triển khai Interface `IStoreHelper` và lớp `StoreHelper` dựa trên Dapper & Reflection.
    - [x] Tự động hóa việc map tham số từ Object C# sang Parameters của Stored Procedure.
    - [x] Đăng ký Dependency Injection tập trung trong `Program.cs`.
- [x] **Refactor Category Module**:
    - [x] Chuyển đổi `CategoryRepository_store` sang sử dụng `StoreHelper` (giảm 70% lượng code lặp lại).

### 2. Module Sản phẩm (Product CRUD via Stored Procedures) - [HOÀN THÀNH]
- [x] **Thiết kế Database (SQL)**:
    - [x] Hoàn thiện bộ SP: `sp_GetProductsByCategoryId`, `sp_GetProductById`, `sp_InsertProduct`, `sp_UpdateProduct`, `sp_DeleteProduct`.
    - [x] Triển khai logic **Soft Delete** (`RecordStatus <> '0'`) và **Validation** (trùng mã, trống tên, sai logic danh mục).
- [x] **Phát triển Backend Layer**:
    - [x] Triển khai `ProductRepository_store` tích hợp `StoreHelper`.
    - [x] Xây dựng `ProductStoreService` quản lý logic nghiệp vụ và mapping DTO.
    - [x] Hoàn thiện `ProductStoreController` với prefix `/Rotev2/`.
- [x] **Đa ngôn ngữ & Localization**:
    - [x] Tích hợp `IStringLocalizer` để dịch các thông báo lỗi trả về từ SQL `RAISERROR`.

### 3. Fix Bug & Tối ưu hóa (Crucial Fixes) - [HOÀN THÀNH]
- [x] **Sửa lỗi SQL Logic**: Đồng bộ hóa toàn bộ logic kiểm tra bản ghi hoạt động sang điều kiện `RecordStatus <> '0'`.
- [x] **Sửa lỗi Column Mapping**: 
    - [x] Fix lỗi `sp_InsertProduct` gán nhầm mã Code vào cột trạng thái.
    - [x] Fix lỗi `sp_GetProductById` thiếu cột `Id` và `CategoryId` dẫn đến mapping sai trên UI.
- [x] **Tối ưu AutoMapper**: 
    - [x] Cấu hình lại `Products_mapper` để xử lý chuyển đổi `string` (dd/MM/yyyy) sang `DateTime` một cách an toàn.
    - [x] Khắc phục lỗi "String '32' is not a valid DateTime" do mapping nhầm trường CategoryId.

### 4. Công việc tiếp theo
- [ ] **Mở rộng Module**: Áp dụng mô hình StoreHelper cho các module còn lại (Order, Customer).
- [ ] **Frontend**: Nhúng Endpoint Product `/Rotev2` vào giao diện quản trị.
- [ ] **Unit Test**: Viết test case cho `StoreHelper` để đảm bảo mapping tham số luôn chính xác.

