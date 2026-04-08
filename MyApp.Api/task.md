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
    - [x] Chuẩn hóa lại Route API cho tường minh hơn:
        - Product: `/lay/Product`, `/layPdtheoId/Product{id}`, `/them/Product`, `/sua/Product{id}`.
        - Category: `/them/category`, `/sua/category{id}`.
    - [x] Refactor `ICategoryRepository`: Trả về `IEnumerable<Category>` thay vì `dynamic` để đảm bảo type-safety.
    - [x] Cập nhật logic lọc `RecordStatus` trong Repository để đồng nhất dữ liệu.

## 2. Frontend (Giao diện & Tiện ích) - [ĐANG TRIỂN KHAI]
- [x] **Tính năng đa ngôn ngữ trên UI**:
    - [x] Triển khai Bộ chọn ngôn ngữ (VI/EN) trên Header.
    - [x] Tích hợp tham số `culture` vào Fetch API.
- [/] **Đồng bộ hóa Route mới**:
    - [ ] Cần cập nhật lại các hàm `fetch()` trong JavaScript để khớp với các Endpoint mới vừa đổi tên (ví dụ: `/lay/Product` thay vì `/api/Products`).
- [ ] **Xử lý thông báo Validation**:
    - [ ] Hiển thị thông báo lỗi từ API (đã được localize) lên UI một cách thân thiện thay vì dùng `alert`.

## 3. Các vấn đề tồn đồn & Task tiếp theo
- [ ] **Khắc phục lỗi khóa file DLL**: Cần tìm giải pháp triệt để khi Build/Rebuild mà không cần tắt VS Code/Terminal.
- [ ] **Tối ưu hóa hình ảnh**: Cấu hình lưu trữ và trả về đường dẫn ảnh thực thế thay vì dùng placeholder.
- [ ] **Kiểm tra luồng Update**: Review kỹ lại logic `UpdatedAt` và map dữ liệu trong `UpdateProduct`.
- [ ] **Unit Test**: Bổ sung test case cho logic Localization để đảm bảo không mất Key khi thêm DTO mới.