
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
