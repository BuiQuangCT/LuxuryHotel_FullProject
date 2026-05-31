# LUXURY HOTEL MANAGEMENT SYSTEM - PROJECT DOCUMENTATION

## Extraction of GUI, Architecture & Flow Diagrams

---

## I. GRAPHICAL USER INTERFACE (GUI) - INTERFACE DESIGN

### 1. Main Interface Pages

#### A. **Homepage (index.html)**

- **Purpose:** Main landing page for customers
- **Design Elements:**
  - Sticky Header Navigation (positioned at top)
  - Logo: "Luxury Hotel" with Gold color (#d4af37)
  - Hero Banner: Full-width background image (60vh height)
  - Navigation Links with hover effects
  - User Login/Logout Section
- **Color Scheme:**
  - Primary: #2c3e50 (Dark Blue)
  - Accent: #d4af37 (Gold)
  - Background: #f9f9f9 (Light Gray)
  - Text: #333 (Dark Gray)

- **Key Features:**
  - Responsive sticky navigation
  - Hero section with gradient overlay
  - User section with login button
  - Professional luxury hotel aesthetic

#### B. **Login Page (login.html)**

- **Design Pattern:** Glassmorphism (frosted glass effect)
- **Elements:**
  - Centered login form
  - Background: Hotel image with dark overlay
  - Backdrop blur effect for form container
  - Input fields with transparent background
  - Email and password inputs
  - Submit button (Gold color)
  - Link to registration page

- **Features:**
  - Responsive form (400px width on desktop)
  - Error message display
  - Form validation feedback

#### C. **Registration Page (register.html)**

- **Design Pattern:** Similar to Login (Glassmorphism)
- **Form Fields:**
  - Họ và Tên (Full Name)
  - Số điện thoại (Phone Number)
  - Căn cước công dân / CMND (ID Card)
  - Email
  - Password (min 6 chars)

- **Features:**
  - Client-side validation
  - API call to backend
  - Success/Error message display
  - Link to login page

#### D. **Booking Confirmation Page (booking-confirm.html)**

- **Design:** Card-based layout
- **Components:**
  - Header: Fixed with logo
  - Container: Max-width 800px, centered
  - Payment Summary Card with gold background
  - Info rows with labels and values

- **Information Displayed:**

  ```
  - Booking ID
  - Customer Name
  - Room Details
  - Check-in Date
  - Check-out Date
  - Number of Guests
  - Total Amount
  - Deposit Amount
  - Remaining Amount
  ```

- **Buttons:**
  - Confirm Payment Button (Gold)
  - Back Button (Gray)
  - Loading Spinner animation

#### E. **Admin Dashboard (admin.html)**

- **Layout:** Two-column sidebar layout
- **Sidebar Navigation:**
  - Menu items for different admin functions
  - Active state highlighting
  - Logout button at bottom

- **Main Content Area:**
  - Tab-based interface
  - Cards for statistics display
  - Tables for data management
  - Fade-in animations for tab switching

- **Tab Functions:**
  - Dashboard Overview
  - Room Management
  - Booking Management
  - Customer Management
  - Payment Management
  - User Reports

#### F. **Customer Profile Page (profile.html)**

- **Design:** Card-based layout
- **Sections:**
  - User information display
  - Booking history
  - Payment history
  - Profile edit form
  - Logout option

#### G. **Room Details Page (detail.html)**

- **Components:**
  - Room image gallery
  - Room specifications
  - Amenities list
  - Price information
  - Booking date picker
  - Number of guests selector
  - Booking confirmation form

#### H. **Password Recovery Page (forgot-password.html)**

- **Flow:**
  - Email input for account recovery
  - Verification code input
  - New password form
  - Success message

---

## II. UI/UX DESIGN SPECIFICATIONS

### 1. **Typography**

- Font Family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif
- Font Sizes:
  - Logo: 24px (bold)
  - Headings: 20-22px
  - Body text: 14-16px
  - Small text: 13-14px

### 2. **Color Palette**

```
Primary Colors:
  - Dark Blue: #2c3e50
  - Gold (Accent): #d4af37
  - Light Gold: #b5952f (hover state)
  - Light Background: #f9f9f9
  - White: #ffffff

Secondary Colors:
  - Gray: #7f8c8d, #95a5a6
  - Light Gray: #eee, #f4f7f6
  - Error Red: #e74c3c
  - Warning Yellow: #fff9e6
```

### 3. **Component Design**

- **Buttons:**
  - Primary Button: Gold background, white text
  - Secondary Button: Gray background, white text
  - Danger Button: Red background
  - Hover states with color transition (0.3s)

- **Cards:**
  - White background
  - 10px border radius
  - Box shadow: 0 4px 15px rgba(0,0,0,0.1)
  - Padding: 20-30px

- **Input Fields:**
  - 10px border radius
  - Padding: 10px
  - Border: none
  - Transparent background (0.2 alpha) for login/register
  - Outline: none (custom focus removed)

- **Forms:**
  - 15-20px margin between groups
  - Labels above inputs
  - Placeholder text in lighter color

### 4. **Spacing & Layout**

- Container max-width: 800px-1200px
- Padding: 15-50px on containers
- Gap between flex items: 15-20px
- Responsive breakpoints for mobile

### 5. **Effects & Animations**

- Hover transitions: 0.3s ease
- Fade-in animations for tab switching
- Loading spinner: CSS animation (spin 1s linear infinite)
- Box shadows for depth
- Backdrop blur for glassmorphism effect

---

## III. BACKEND ARCHITECTURE & DESIGN PATTERNS

### 1. **Architecture Overview**

```
Luxury Hotel API
├── Program.cs (Main Configuration)
├── Controllers/
│   ├── KhachHangController (Customer Management)
│   ├── DatPhongController (Room Booking)
│   ├── ThanhToanController (Payment Processing)
│   ├── PhongController (Room Management)
│   └── AdminController (Admin Operations)
├── Models/
│   ├── KhachHang (Customer)
│   ├── Phong (Room)
│   ├── DatPhong (Booking)
│   ├── HoaDon (Invoice)
│   ├── DatCoc (Deposit)
│   ├── GiaoDich (Transaction)
│   ├── HinhAnh (Image)
│   ├── DanhGium (Review/Rating)
│   ├── KhachSan (Hotel)
│   └── LuxuryHotelContext (Database Context)
└── Services/
    └── EmailService (Email Notifications)
```

### 2. **Design Patterns Used**

#### A. **Entity Framework Pattern (ORM)**

- Uses EF Core for database operations
- Database Context: `LuxuryHotelContext`
- DbSet for each entity
- LINQ queries for data retrieval

#### B. **Repository Pattern (Implicit)**

- CRUD operations through DbContext
- Separation of data access logic

#### C. **Service Pattern**

- `EmailService`: Handles email notifications using MailKit
- Decouples business logic from controllers

#### D. **JWT Authentication Pattern**

- Token-based authentication for Admin users
- Claims-based authorization
- Token expiry: 2 hours

#### E. **Password Hashing Pattern**

- BCrypt.Net library for secure password hashing
- Password verification using BCrypt.Verify()
- Never store plain text passwords

#### F. **API Controller Pattern**

- RESTful API endpoints
- Route attributes for clean URLs
- JSON request/response bodies

### 3. **Core Services**

#### A. **Authentication & Authorization**

```csharp
- JWT Token Generation
- Role-based Access Control (Admin, KhachHang)
- Password Hashing with BCrypt
- Email verification capability
```

#### B. **Email Service**

```csharp
- Async email sending
- SMTP configuration (Gmail)
- HTML email templates
- Booking confirmation emails
```

#### C. **Payment Processing**

```csharp
- VNPay Integration
- Secure payment URL generation
- HMAC-SHA512 signature validation
- Transaction tracking
```

### 4. **Database Connection**

- Provider: SQL Server
- Connection String: Server=LAPTOP-3S88H9S1;Database=LuxuryHotel
- Trusted Connection with certificate trust
- Unicode support for Vietnamese text

---

## IV. DATABASE MODELS & RELATIONSHIPS

### 1. **Entity Relationship Diagram (ERD)**

```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│    KhachHang (Customer)                                    │
│    ─────────────────────                                   │
│    • MaKh (PK)                                             │
│    • HoTen                                                 │
│    • Email                                                 │
│    • MatKhau                                               │
│    • SoDienThoai                                           │
│    • Cmnd                                                  │
│    • NgayTao                                               │
│    • TrangThai                                             │
│    • VaiTro                                                │
│    ──────────────────────────────────────────────────────│
│            ↓ 1:N                                           │
│    ┌────────────────────────────────────────┐              │
│    │                                        │              │
│    DatPhong (Booking)                       │              │
│    ──────────────────────                   │              │
│    • MaDatPhong (PK)                        │              │
│    • MaKh (FK)                              │              │
│    • MaPhong (FK)                           │              │
│    • NgayNhan                               │              │
│    • NgayTra                                │              │
│    • SoNguoi                                │              │
│    • TongTien                               │              │
│    • TrangThai                              │              │
│    • NgayDat                                │              │
│    • MaXacNhan                              │              │
│    ──────────────────────────────────────────┘              │
│         ↓ 1:1                   ↓ 1:1                      │
│    ┌─────────────┐         ┌──────────────┐                │
│    │ DatCoc      │         │ HoaDon       │                │
│    │ (Deposit)   │         │ (Invoice)    │                │
│    └─────────────┘         │              │                │
│                            │ • GiaoDich 1:N              │
│                            └──────────────┘                │
│                                                            │
│    ↑ N:1                                                   │
│    │                                                       │
│    Phong (Room)                                            │
│    ────────────                                            │
│    • MaPhong (PK)                                          │
│    • MaKs (FK)                                             │
│    • LoaiPhong                                             │
│    • Gia                                                   │
│    • DienTich                                              │
│    • TienNghi                                              │
│    • SucChua                                               │
│    • TrangThai                                             │
│    ────────────────────────────────────────────────────────│
│         ↓ 1:N                                              │
│    HinhAnh (Image)                                         │
│    • MaHa (PK)                                             │
│    • MaPhong (FK)                                          │
│    • DuongDanHinh                                          │
│    ─────────────────────────────────────────────────────   │
│                                                            │
│    KhachSan (Hotel)                                        │
│    • MaKs (PK)                                             │
│    • TenKs                                                 │
│    • DiaChi                                                │
│    • SoDienThoai                                           │
│    ────────────────────────────────────────────────────────│
│         ↓ 1:N                                              │
│    DanhGium (Review/Rating)                                │
│    • MaDg (PK)                                             │
│    • MaKh (FK)                                             │
│    • MaKs (FK)                                             │
│    • DiemSo                                                │
│    • NoiDung                                               │
│    • ThoiGian                                              │
│    • TrangThai                                             │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 2. **Data Models Details**

#### **KhachHang (Customer)**

```csharp
public partial class KhachHang
{
    public int MaKh { get; set; }                    // Primary Key
    public string VaiTro { get; set; }              // Role: Admin, KhachHang
    public string HoTen { get; set; }               // Full Name
    public string Email { get; set; }               // Email (Unique)
    public string MatKhau { get; set; }             // Hashed Password
    public string SoDienThoai { get; set; }         // Phone Number
    public string Cmnd { get; set; }                // ID Card
    public DateTime NgayTao { get; set; }           // Created Date
    public string TrangThai { get; set; }           // Status: active, locked

    // Relationships
    public virtual ICollection<DanhGium> DanhGia { get; set; }
    public virtual ICollection<DatPhong> DatPhongs { get; set; }
}
```

#### **DatPhong (Booking)**

```csharp
public partial class DatPhong
{
    public int MaDatPhong { get; set; }             // Primary Key
    public int MaKh { get; set; }                   // Foreign Key (Customer)
    public string MaPhong { get; set; }             // Foreign Key (Room)
    public DateTime NgayNhan { get; set; }          // Check-in Date
    public DateTime NgayTra { get; set; }           // Check-out Date
    public int SoNguoi { get; set; }                // Number of Guests
    public decimal TongTien { get; set; }           // Total Amount
    public string TrangThai { get; set; }           // Status: Pending, Success, Đã hủy
    public DateTime NgayDat { get; set; }           // Booking Date
    public string MaXacNhan { get; set; }           // Confirmation Code

    // Relationships
    public virtual DatCoc DatCoc { get; set; }
    public virtual HoaDon HoaDon { get; set; }
    public virtual KhachHang MaKhNavigation { get; set; }
    public virtual Phong MaPhongNavigation { get; set; }
}
```

#### **Phong (Room)**

```csharp
public partial class Phong
{
    public string MaPhong { get; set; }             // Primary Key
    public string MaKs { get; set; }                // Foreign Key (Hotel)
    public string LoaiPhong { get; set; }           // Room Type
    public decimal Gia { get; set; }                // Price per Night
    public int DienTich { get; set; }               // Area (sqm)
    public string TienNghi { get; set; }            // Amenities
    public int SucChua { get; set; }                // Capacity
    public string TrangThai { get; set; }           // Status: available, occupied

    // Relationships
    public virtual ICollection<DatPhong> DatPhongs { get; set; }
    public virtual ICollection<HinhAnh> HinhAnhs { get; set; }
    public virtual KhachSan MaKsNavigation { get; set; }
}
```

#### **HoaDon (Invoice)**

```csharp
public partial class HoaDon
{
    public int MaHd { get; set; }                   // Primary Key
    public int MaDatPhong { get; set; }             // Foreign Key (Booking)
    public decimal TongTien { get; set; }           // Total Amount
    public decimal SoTienDaCoc { get; set; }        // Deposit Paid
    public decimal SoTienConLai { get; set; }       // Remaining Amount
    public DateTime NgayXuatHd { get; set; }        // Invoice Date
    public string TrangThaiTt { get; set; }         // Payment Status

    // Relationships
    public virtual ICollection<GiaoDich> GiaoDiches { get; set; }
    public virtual DatPhong MaDatPhongNavigation { get; set; }
}
```

#### **DatCoc (Deposit)**

```csharp
public partial class DatCoc
{
    public int MaDatCoc { get; set; }               // Primary Key
    public int MaDatPhong { get; set; }             // Foreign Key (Booking)
    public decimal SoTienCoc { get; set; }          // Deposit Amount
    public DateTime NgayDatCoc { get; set; }        // Deposit Date
    public string TrangThai { get; set; }           // Status: Chờ TT, Đã TT, Hủy
}
```

#### **GiaoDich (Transaction)**

```csharp
public partial class GiaoDich
{
    public int MaGd { get; set; }                   // Primary Key
    public int MaHd { get; set; }                   // Foreign Key (Invoice)
    public decimal SoTien { get; set; }             // Amount
    public DateTime NgayGd { get; set; }            // Transaction Date
    public string PhuongThuc { get; set; }          // Payment Method: VNPay, Tiền mặt
    public string TrangThai { get; set; }           // Status: Success, Failed
}
```

---

## V. API ENDPOINTS & FLOW

### 1. **Customer (KhachHang) Endpoints**

#### **1.1 Registration**

```
POST /api/KhachHang/dang-ky
Request Body:
{
  "hoTen": "string",
  "email": "string",
  "matKhau": "string",
  "soDienThoai": "string",
  "cmnd": "string"
}
Response:
{
  "thongBao": "Đăng ký tài khoản thành công!"
}
```

#### **1.2 Login**

```
POST /api/KhachHang/dang-nhap
Query Parameters:
  - email: string
  - matKhauGoc: string

Response:
{
  "thongBao": "Đăng nhập thành công!",
  "token": "JWT_TOKEN",
  "maKh": int,
  "vaiTro": "string"
}
```

#### **1.3 Get User Profile**

```
GET /api/KhachHang/thong-tin
Headers:
  - Authorization: Bearer {token}

Response:
{
  "email": "string",
  "hoTen": "string",
  "soDienThoai": "string",
  "cmnd": "string"
}
```

### 2. **Booking (DatPhong) Endpoints**

#### **2.1 Create Booking**

```
POST /api/DatPhong/tao-don
Request Body:
{
  "maKh": int,
  "maPhong": "string",
  "ngayNhan": "datetime",
  "ngayTra": "datetime",
  "tongTien": decimal
}
Response:
{
  "message": "Tạo đơn đặt phòng thành công!",
  "maDatPhong": int
}
```

#### **2.2 Get Booking History**

```
GET /api/DatPhong/lich-su/{maKh}
Response:
[
  {
    "maDatPhong": int,
    "maPhong": "string",
    "ngayNhan": "datetime",
    "ngayTra": "datetime",
    "tongTien": decimal,
    "trangThai": "string"
  }
]
```

#### **2.3 Cancel Booking**

```
PUT /api/DatPhong/huy-don/{maDatPhong}
Response:
{
  "message": "Đã hủy đơn đặt phòng thành công."
}
```

### 3. **Payment (ThanhToan) Endpoints**

#### **3.1 Create Payment URL (VNPay)**

```
GET /api/ThanhToan/tao-url?maDatPhong=int&tongTien=decimal
Response:
{
  "url": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?..."
}
```

#### **3.2 Payment Callback (IPN)**

```
GET /api/ThanhToan/vnpay-ipn?vnp_TxnRef=...&vnp_ResponseCode=...
Response:
{
  "RspCode": "00",
  "Message": "Payment successful"
}
```

---

## VI. SYSTEM FLOW DIAGRAMS

### 1. **User Registration & Login Flow**

```
┌─────────────────────────────────────────────────────────────────┐
│                   REGISTRATION FLOW                             │
└─────────────────────────────────────────────────────────────────┘

User                        Frontend              Backend (API)
 │                             │                       │
 │ Fill Registration Form       │                       │
 ├────────────────────────────>│                        │
 │                             │ POST /dang-ky          │
 │                             │ (Email, Password...)   │
 │                             ├───────────────────────>│
 │                             │                        │
 │                             │  ✓ Validate Email      │
 │                             │  ✓ Hash Password       │
 │                             │  ✓ Save to DB          │
 │                             │                        │
 │                             │    Success Response    │
 │                             │<───────────────────────┤
 │  Registration Success       │                        │
 │<────────────────────────────┤                        │
 │                             │                        │


┌─────────────────────────────────────────────────────────────────┐
│                    LOGIN FLOW                                   │
└─────────────────────────────────────────────────────────────────┘

User                        Frontend              Backend (API)
 │                             │                       │
 │ Enter Email & Password      │                       │
 ├────────────────────────────>│                        │
 │                             │ POST /dang-nhap        │
 │                             │ (Email, Password)      │
 │                             ├───────────────────────>│
 │                             │                        │
 │                             │  ✓ Find User by Email  │
 │                             │  ✓ Verify Password     │
 │                             │  ✓ Generate JWT Token  │
 │                             │                        │
 │                             │ JWT Token Response     │
 │                             │<───────────────────────┤
 │  Store Token in Storage     │                        │
 │  Redirect to Homepage       │                        │
 │<────────────────────────────┤                        │
 │                             │                        │
```

### 2. **Room Booking & Payment Flow**

```
┌──────────────────────────────────────────────────────────────────┐
│                    BOOKING FLOW                                  │
└──────────────────────────────────────────────────────────────────┘

User                    Frontend            Backend (API)       Database
 │                         │                    │                  │
 │ View Room Details       │                    │                  │
 ├────────────────────────>│                    │                  │
 │ Select Dates & Guests   │ GET /api/Phong     │                  │
 │ Fill Booking Form       ├──────────────────>│                  │
 │                         │                    │ Query Rooms      │
 │                         │  Room Details      │                  │
 │                         │<──────────────────>│                  │
 │ Click Book Button       │                    │                  │
 ├────────────────────────>│                    │                  │
 │                         │ POST /tao-don      │                  │
 │                         │ (MaKh, MaPhong..)  │                  │
 │                         ├──────────────────>│                  │
 │                         │                    │ Create DatPhong  │
 │                         │                    ├─────────────────>│
 │                         │                    │                  │
 │                         │  Booking Response  │ Save Booking     │
 │                         │<──────────────────>│<─────────────────┤
 │ Show Booking Details    │                    │                  │
 │<────────────────────────┤                    │                  │
 │                         │                    │                  │


┌──────────────────────────────────────────────────────────────────┐
│                  PAYMENT FLOW (VNPay)                            │
└──────────────────────────────────────────────────────────────────┘

User                Frontend          Backend (API)        VNPay Gateway
 │                    │                    │                    │
 │ Click Pay          │                    │                    │
 ├───────────────────>│                    │                    │
 │                    │ GET /tao-url       │                    │
 │                    │ (MaDatPhong, Amount)                    │
 │                    ├──────────────────>│                    │
 │                    │                    │                    │
 │                    │  ✓ Config VNPay    │                    │
 │                    │  ✓ Add Request Data│                    │
 │                    │  ✓ Generate Hash   │                    │
 │                    │                    │ Create URL         │
 │                    │  Payment URL       │                    │
 │                    │<──────────────────>│                    │
 │ Redirect to VNPay  │                    │                    │
 │<───────────────────┤                    │                    │
 │                    │                    │                    │
 │ Enter Card Info    │                    │                    │
 │ Complete Payment   ├───────────────────────────────────────>│
 │                    │                    │                    │
 │                    │                    │<─ Payment Result ──┤
 │                    │                    │ GET /vnpay-ipn     │
 │                    │ IPN Callback       │ (TxnRef, ResponseCode...)
 │                    │<─────────────────────────────────────────┤
 │                    │                    │ Verify Signature   │
 │                    │                    │ Update Status      │
 │                    │                    ├──────────────────>│
 │                    │                    │ Save Transaction   │
 │                    │                    │<──────────────────┤
 │ Success Page       │                    │ Success Response   │
 │<───────────────────┤<──────────────────>│                    │
 │                    │                    │                    │
```

### 3. **Admin Dashboard Flow**

```
┌──────────────────────────────────────────────────────────────────┐
│                 ADMIN FLOW                                       │
└──────────────────────────────────────────────────────────────────┘

Admin                  Frontend          Backend (API)        Database
 │                       │                    │                  │
 │ Login with Admin Role │                    │                  │
 ├──────────────────────>│                    │                  │
 │                       │ POST /dang-nhap    │                  │
 │                       ├──────────────────>│                  │
 │                       │                    │ Verify Admin     │
 │                       │  JWT Token + Role  │                  │
 │                       │<──────────────────>│                  │
 │ Redirect to Dashboard │                    │                  │
 │<──────────────────────┤                    │                  │
 │                       │                    │                  │
 │ View Dashboard Stats  │                    │                  │
 │ - Total Bookings      │ GET /api/Admin/... │                  │
 │ - Revenue             ├──────────────────>│                  │
 │ - Customer Count      │                    │ Query Data       │
 │ - Available Rooms     │                    │                  │
 │                       │                    ├─────────────────>│
 │                       │    Stats Data      │                  │
 │                       │<──────────────────>│<─────────────────┤
 │ Display Stats Cards   │                    │                  │
 │<──────────────────────┤                    │                  │
 │                       │                    │                  │
 │ Click Room Management │                    │                  │
 │ - View All Rooms      │ GET /api/Phong     │                  │
 │ - Edit Room Details   ├──────────────────>│                  │
 │ - Change Status       │                    │ Fetch Rooms      │
 │                       │                    │                  │
 │                       │  Room List         ├─────────────────>│
 │                       │<──────────────────>│<─────────────────┤
 │ Display Rooms Table   │                    │                  │
 │<──────────────────────┤                    │                  │
 │                       │                    │                  │
 │ Click Booking Mgmt    │ GET /api/DatPhong/ │                  │
 │ - View All Bookings   ├──────────────────>│                  │
 │ - Check Status        │                    │ Fetch Bookings   │
 │ - Process Payment     │                    │                  │
 │                       │                    ├─────────────────>│
 │                       │  Booking List      │                  │
 │                       │<──────────────────>│<─────────────────┤
 │ Display Bookings      │                    │                  │
 │<──────────────────────┤                    │                  │
 │                       │                    │                  │
```

### 4. **Email Notification Flow**

```
┌──────────────────────────────────────────────────────────────────┐
│              EMAIL NOTIFICATION SYSTEM                           │
└──────────────────────────────────────────────────────────────────┘

Event (Payment Success)    Backend Service        Gmail SMTP       User Email
         │                        │                    │                │
         │ Trigger Event          │                    │                │
         ├───────────────────────>│                    │                │
         │                        │ Prepare Email      │                │
         │                        │ - To: customer@    │                │
         │                        │ - Subject: Confirm │                │
         │                        │ - Body: HTML       │                │
         │                        │                    │                │
         │                        │ Connect SMTP       │                │
         │                        ├───────────────────>│                │
         │                        │                    │ Authenticate   │
         │                        │                    │ Send Email     │
         │                        │                    ├──────────────>│
         │                        │                    │                │
         │                        │  Email Sent        │ Receive Email  │
         │                        │<───────────────────┤                │
         │                        │                    │ Customer Views │
         │                        │                    │<───────────────┤
         │                        │                    │                │
```

### 5. **Password Hashing & Security Flow**

```
┌──────────────────────────────────────────────────────────────────┐
│           PASSWORD SECURITY FLOW (Using BCrypt)                 │
└──────────────────────────────────────────────────────────────────┘

REGISTRATION:
User enters password     "MyPassword123"
         │
         ├─> BCrypt.HashPassword()
         │   - Generate random salt
         │   - Hash with salt + algorithm
         │   - Return hash string
         │
Hash stored: "$2a$11$NXw4e4Mpt6J2H8P3x..."
         │
         └─> Save to Database


LOGIN:
User enters password     "MyPassword123"
         │
         ├─> User found in DB
         │   Hash from DB: "$2a$11$NXw4e4Mpt6J2H8P3x..."
         │
         ├─> BCrypt.Verify(inputPassword, dbHash)
         │   - Hash input with same algorithm
         │   - Compare with DB hash
         │
         ├─> If Match ✓
         │   Generate JWT Token
         │   Return token + roles
         │
         └─> If No Match ✗
             Return "Sai mật khẩu!"
```

---

## VII. SECURITY IMPLEMENTATION

### 1. **Authentication**

- JWT Token-based authentication
- Claims-based authorization
- Token expiry: 2 hours
- Roles: Admin, KhachHang

### 2. **Password Security**

- BCrypt hashing with salt
- Never store plain passwords
- Verification on login

### 3. **Payment Security**

- HMAC-SHA512 signature validation
- VNPay integration with secure credentials
- Transaction verification
- IPN callback validation

### 4. **API Security**

- CORS configuration
- HTTPS connection (Sandbox testing on HTTP)
- Input validation
- SQL injection prevention through EF Core

---

## VIII. CONFIGURATION & SETTINGS

### **Database Configuration**

```csharp
Server: LAPTOP-3S88H9S1
Database: LuxuryHotel
Authentication: Trusted_Connection=True
TrustServerCertificate: True
```

### **JWT Configuration**

```csharp
Key: "LXR_Hotel_Super_Secret_Key_At_Least_32_Chars_Long_2026!!!"
Issuer: "LuxuryHotelAPI"
Audience: "LuxuryHotelClients"
Expiry: 2 hours
Algorithm: HmacSha256Signature
```

### **VNPay Configuration**

```csharp
TMN Code: JLQ3O2EL
HashSecret: Y1UDIWP635I8SD7R7SI43AIE591F5ZUM
URL: https://sandbox.vnpayment.vn/paymentv2/vpcpay.html
Return URL: http://127.0.0.1:5500/index.html
```

### **Email Configuration**

```csharp
Provider: Gmail SMTP
Server: smtp.gmail.com
Port: 587
Security: StartTls
From Email: your-email@gmail.com
```

---

## IX. TECHNOLOGY STACK

### **Frontend**

- HTML5
- CSS3 (with Flexbox, Grid, Animations)
- JavaScript (Vanilla)
- Local Storage for token management
- Fetch API for HTTP requests

### **Backend**

- C# (.NET)
- ASP.NET Core Web API
- Entity Framework Core (ORM)
- JWT Authentication
- BCrypt.Net (Password Hashing)
- MailKit (Email Service)
- VNPay Payment Gateway

### **Database**

- Microsoft SQL Server 2019+
- 9 main entities with relationships

### **Tools & Libraries**

- Swagger/OpenAPI for API documentation
- CORS middleware
- Dependency Injection

---

## X. FUNCTIONAL MODULES

### **1. Customer Management**

- Registration
- Login/Logout
- Profile Management
- Password Recovery
- Account Deactivation

### **2. Room Management**

- View Room Catalog
- Room Filtering (by type, price, amenities)
- Room Availability Check
- Room Details & Images

### **3. Booking System**

- Create Booking
- Check Availability
- Booking Confirmation
- Booking History
- Cancel Booking

### **4. Payment System**

- Deposit Payment (VNPay)
- Final Payment
- Payment History
- Transaction Verification

### **5. Admin Panel**

- Dashboard Overview
- Customer Management
- Room Management
- Booking Management
- Payment Management
- Reports & Statistics

### **6. Notification System**

- Email Confirmations
- Booking Notifications
- Payment Receipts

### **7. Rating & Review**

- Submit Reviews
- Rate Hotels/Rooms
- View Reviews

---

## XI. DATA VALIDATION RULES

### **Customer Registration**

- Email: Valid email format, must be unique
- Password: Minimum 6 characters
- Phone: Valid Vietnamese phone format
- CMND: Valid 9 or 12 digits
- Name: Non-empty

### **Booking Creation**

- Check-in date must be after today
- Check-out date must be after check-in date
- Number of guests must be positive
- Room must be available for dates
- Total amount must be positive

### **Payment**

- Amount must match booking total
- Transaction ID must be unique
- Payment status validation

---

**Document Generated:** 2026-05-14  
**Project:** Luxury Hotel Management System  
**Status:** Complete Documentation
