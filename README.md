# 🏨 Hệ thống Quản lý Khách sạn (Luxury Hotel Management System)

Luxury Hotel là một hệ thống quản lý khách sạn Full-stack được phát triển với kiến trúc 3-tier, mang đến trải nghiệm đặt phòng mượt mà cho khách hàng và bộ công cụ quản lý toàn diện cho ban quản trị.

---

## 🚀 Tính năng nổi bật

### 👤 Phân hệ Khách hàng (Customer)

- **Đăng ký & Đăng nhập:** Xác thực bảo mật với JWT Token và mã hóa mật khẩu BCrypt.
- **Khám phá & Đặt phòng:** Tìm kiếm phòng trống theo ngày, số lượng người và hạng phòng.
- **Thanh toán trực tuyến:** Tích hợp cổng thanh toán **VNPay**. Hỗ trợ đặt cọc (30%, 50%) hoặc thanh toán toàn bộ (100%).
- **Quản lý hồ sơ:** Xem lịch sử đặt phòng, lịch sử giao dịch và hủy đơn đặt phòng linh hoạt.
- **Thông báo tự động:** Nhận email xác nhận đặt phòng và hóa đơn điện tử tự động (MailKit).

### 👨‍💼 Phân hệ Quản trị viên (Admin)

- **Bảng điều khiển (Dashboard):** Thống kê doanh thu, số lượng đơn hàng, số phòng đang sử dụng theo thời gian thực.
- **Quản lý Trạng thái Phòng:** Cập nhật tình trạng phòng (Sẵn sàng, Đang dọn dẹp, Bảo trì).
- **Quản lý Đơn đặt phòng:** Duyệt đơn, kiểm tra trạng thái thanh toán, hủy đơn.
- **Quản lý Khách hàng:** Theo dõi danh sách tài khoản, khóa/mở khóa tài khoản khi cần thiết.

---

## 🛠 Công nghệ sử dụng

**Frontend:**

- HTML5, CSS3, Vanilla JavaScript.
- Thiết kế giao diện Glassmorphism hiện đại, Responsive UI.
- Fetch API để giao tiếp với Backend.

**Backend:**

- ASP.NET Core Web API.
- Entity Framework Core (Code-First/Database-First).
- LINQ Queries, RESTful Architecture.

**Cơ sở dữ liệu:**

- Microsoft SQL Server.

**Thư viện & Tiện ích mở rộng:**

- **BCrypt.Net:** Băm & bảo mật mật khẩu.
- **JWT (JSON Web Tokens):** Phân quyền & phiên đăng nhập.
- **MailKit:** Gửi email tự động qua SMTP (Gmail).
- **VNPay SDK:** Xử lý IPN callback & thanh toán điện tử.

---

## 📂 Cấu trúc dự án

```text
LuxuryHotel_FullProject/
│
├── Backend/                    # Thư mục mã nguồn ASP.NET Core API
│   └── lxrhotel.API/
│       ├── Controllers/        # KhachHang, DatPhong, Phong, ThanhToan, Admin
│       ├── Models/             # Entity models (LuxuryHotelContext, KhachHang, DatPhong...)
│       ├── Services/           # EmailService, VnPayLibrary
│       ├── Program.cs          # File khởi chạy & cấu hình dịch vụ, CORS, JWT
│       └── appsettings.json    # Cấu hình chuỗi kết nối DB & Secret Keys
│
└── Frontend/                   # Thư mục giao diện HTML/CSS/JS thuần
    ├── index.html              # Trang chủ
    ├── login.html              # Trang đăng nhập
    ├── register.html           # Trang đăng ký
    ├── detail.html             # Chi tiết phòng & chọn mức cọc
    ├── admin.html              # Bảng điều khiển quản trị viên
    └── profile.html            # Trang hồ sơ khách hàng
```

---

## 🔑 Tài khoản Demo (Dành cho Giảng viên chấm bài)

Để thuận tiện cho việc kiểm tra và trải nghiệm toàn bộ luồng nghiệp vụ của hệ thống mà không cần đăng ký mới, giảng viên có thể sử dụng các tài khoản đã được thiết lập sẵn dưới đây:

**1. Tài khoản Quản trị viên (Admin)**

- **Email:** `admin@luxuryhotel.com`
- **Mật khẩu:** `123456`

**2. Tài khoản Khách hàng (Customer)**

- **Email:** `quang36@gmail.com`
- **Mật khẩu:** `36363636`

> **💡 Mẹo test luồng:** Giảng viên có thể dùng tài khoản **Customer** để tạo một đơn đặt phòng (có thể thử thanh toán VNPay Sandbox), sau đó đăng xuất và dùng tài khoản **Admin** để kiểm tra thống kê doanh thu và quản lý đơn hàng đó.

---

## ⚙️ Hướng dẫn cài đặt & Khởi chạy

### 1. Yêu cầu hệ thống

- [.NET SDK 8.0/10.0](https://dotnet.microsoft.com/download) trở lên.
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) & SQL Server Management Studio (SSMS).
- Trình duyệt web hiện đại (Chrome, Edge, Firefox).
- Extension "Live Server" (nếu dùng VS Code) để chạy Frontend.

### 2. Thiết lập CSDL (Database)

1. Mở SSMS, chạy file script `database.sql` (nếu có) hoặc sử dụng Entity Framework Core Migrations để tạo database.
2. Mở file `Backend/lxrhotel.API/appsettings.json`.
3. Cập nhật `DefaultConnection` cho khớp với Server Name của SQL Server trên máy bạn:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=LuxuryHotel;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

### 3. Cấu hình các dịch vụ bên thứ 3 (Email & Thanh toán)

- Trong các controller, đảm bảo cập nhật thông tin **VNPay (TmnCode, HashSecret)** và **Email SMTP (Password App Gmail)** bằng tài khoản thật hoặc sandbox của bạn để test tính năng thanh toán, gửi mail.

### 4. Khởi chạy hệ thống

**Chạy Backend (API):**

1. Mở thư mục `Backend/lxrhotel.API/` bằng Visual Studio.
2. Chạy ứng dụng bằng nút `Start` (IIS Express hoặc Kestrel).
3. API sẽ chạy ở địa chỉ mặc định (VD: `https://localhost:7182`). Kiểm tra Swagger UI để xem các Endpoints.

**Chạy Frontend:**

1. Mở thư mục `Frontend/` bằng Visual Studio Code.
2. Nhấp chuột phải vào file `index.html` và chọn **"Open with Live Server"**.
3. Cập nhật lại đường dẫn API URL trong mã JS Frontend nếu port mạng backend của máy bạn chạy khác `7182`.

---

## 🛡 Vấn đề Bảo mật (Security)

- Đảm bảo các `Secret Key` (JWT, VNPay) trong tương lai được đưa vào `appsettings.json` thay vì hard-code trong Controllers.
- Nên sử dụng cấu hình CORS chỉ cho phép các domain được ủy quyền ở môi trường Production.

---

_Dự án Luxury Hotel Management System._
