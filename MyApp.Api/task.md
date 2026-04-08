# Tiến độ công việc - 08/04/2026

## 1. Backend (API & Application)
- [x] **Cấu hình AutoMapper**:
    - [x] Đăng ký AutoMapper trong `Program.cs`.
    - [x] Loại bỏ package `AutoMapper.Extensions.Microsoft.DependencyInjection` bị dư thừa gây lỗi build.
- [x] **Cấu hình Mapping Profile**:
    - [x] Mapping `Product_DTO` -> `Product` (Bổ sung gán `RecordStatus = "1"` mặc định).
    - [x] Mapping `ProductUpdateDto` -> `Product` (Sửa lỗi Missing mapping).
    - [x] Mapping `CategoryUpdateDto` -> `Category`.
    - [x] **Fix Error**: Sửa lỗi `AutoMapperMappingException` cho `Category -> Category_DTO` bằng cách thêm `.ReverseMap()`.
- [x] **Refactor Service**:
    - [x] Cập nhật `ProductService.AddProduct` sử dụng `_mapper.Map`.
    - [x] Refactor `CategoryService`: Sử dụng `IMapper` cho tất cả các phương thức (`GetListCategoryForUI`, `AddCategory`, `UpdateCategory`).
    - [x] Refactor `ProductService`: Chuyển đổi từ `Anonymous Object` sang sử dụng `ProductDetailDTO` cho các phương thức trả về dữ liệu.
- [x] **Mô hình DTO**:
    - [x] Hoàn thiện `ProductDetailDTO.cs` để phục vụ hiển thị chi tiết sản phẩm.
    - [x] Cập nhật `Category_DTO.cs`: Bổ sung trường `Id` để đồng bộ dữ liệu với UI.
- [x] **Tối ưu Repository**:
    - [x] `CategoryRepository.GetAllCategoriesAsync`: Trả về Entity gốc thay vì anonymous object để mapping chính xác.
    - [x] `ProductRepository.GetByCategoryIdAsync`: Bổ sung `.Include(p => p.Category)` để lấy dữ liệu danh mục liên quan (Eager Loading).

## 2. Frontend (Giao diện & Logic)
- [x] **Giao diện chi tiết (Product Detail)**:
    - [x] Thiết kế layout `ProductDetail.html`.
    - [x] Đồng bộ phong cách CSS trong `Index.css`.
- [x] **Xử lý dữ liệu**:
    - [x] Viết Script `ProductDetail.js` để fetch dữ liệu từ API và hiển thị lên UI.

## 3. Các vấn đề cần giải quyết (Pending/Fixing)
- [ ] Kiểm tra lại toàn bộ luồng Update Product để đảm bảo các trường `UpdatedAt` được cập nhật chính xác.
- [ ] Tối ưu hóa việc hiển thị hình ảnh từ đường dẫn API.
- [ ] Xử lý lỗi khóa file DLL khi build trong lúc ứng dụng đang chạy.
- [ ] Kiểm tra tính nhất quán giữa frontend và backend sau khi refactor sang DTO.