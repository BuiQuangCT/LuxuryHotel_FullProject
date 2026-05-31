# LUXURY HOTEL - SYSTEM ARCHITECTURE & VISUAL DIAGRAMS

---

## I. SYSTEM ARCHITECTURE DIAGRAM

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         CLIENT TIER (Frontend)                          │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐                 │
│  │  index.html  │  │  login.html  │  │ register.html│                 │
│  │  (Homepage)  │  │  (Login)     │  │  (Register)  │                 │
│  └──────────────┘  └──────────────┘  └──────────────┘                 │
│                                                                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐                 │
│  │  detail.html │  │booking-confirm│  │ profile.html │                 │
│  │(Room Details)│  │   .html       │  │ (Profile)    │                 │
│  │              │  │  (Booking)    │  │              │                 │
│  └──────────────┘  └──────────────┘  └──────────────┘                 │
│                                                                         │
│  ┌──────────────────────────────────────────────────────┐              │
│  │        admin.html (Admin Dashboard)                  │              │
│  └──────────────────────────────────────────────────────┘              │
│                                                                         │
│  Technologies: HTML5, CSS3, JavaScript, LocalStorage                  │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
                                  │
                     HTTP/HTTPS (Fetch API)
                                  ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                    APPLICATION TIER (Backend API)                       │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────────────────────────────────────────────┐               │
│  │            ASP.NET Core Web API (Program.cs)        │               │
│  │  - JWT Authentication                              │               │
│  │  - CORS Configuration                              │               │
│  │  - Swagger Documentation                           │               │
│  └─────────────────────────────────────────────────────┘               │
│                          │                                              │
│        ┌─────────────────┼─────────────────┐                           │
│        │                 │                 │                           │
│    ┌───▼──────────┐ ┌───▼──────────┐ ┌───▼──────────┐                │
│    │ Controllers  │ │  Services    │ │   Models     │                │
│    ├──────────────┤ ├──────────────┤ ├──────────────┤                │
│    │ - KhachHang  │ │- EmailService│ │- KhachHang   │                │
│    │ - DatPhong   │ │- VNPayLib    │ │- DatPhong    │                │
│    │ - ThanhToan  │ │- Encryption  │ │- Phong       │                │
│    │ - Phong      │ │- Validation  │ │- HoaDon      │                │
│    │ - Admin      │ │              │ │- DatCoc      │                │
│    │              │ │              │ │- GiaoDich    │                │
│    │              │ │              │ │- DanhGium    │                │
│    │              │ │              │ │- HinhAnh     │                │
│    └──────────────┘ └──────────────┘ └──────────────┘                │
│                                                                         │
│  ┌─────────────────────────────────────────────────────┐               │
│  │         Entity Framework Core (Data Access)         │               │
│  │  - DbContext: LuxuryHotelContext                   │               │
│  │  - LINQ Queries                                    │               │
│  │  - ORM Mapping                                     │               │
│  └─────────────────────────────────────────────────────┘               │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
                                  │
                    SQL Queries (SQL Server)
                                  ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                      DATA TIER (Database)                               │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌────────────────────────────────────────────────────────┐            │
│  │       Microsoft SQL Server - LuxuryHotel Database      │            │
│  ├────────────────────────────────────────────────────────┤            │
│  │                                                        │            │
│  │  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐ │            │
│  │  │ KhachHang    │  │ DatPhong     │  │ Phong       │ │            │
│  │  │ (Customers)  │  │ (Bookings)   │  │ (Rooms)     │ │            │
│  │  └──────────────┘  └──────────────┘  └─────────────┘ │            │
│  │                                                        │            │
│  │  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐ │            │
│  │  │ HoaDon       │  │ DatCoc       │  │ GiaoDich    │ │            │
│  │  │ (Invoices)   │  │ (Deposits)   │  │ (Trans.)    │ │            │
│  │  └──────────────┘  └──────────────┘  └─────────────┘ │            │
│  │                                                        │            │
│  │  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐ │            │
│  │  │ HinhAnh      │  │ DanhGium     │  │ KhachSan    │ │            │
│  │  │ (Images)     │  │ (Reviews)    │  │ (Hotels)    │ │            │
│  │  └──────────────┘  └──────────────┘  └─────────────┘ │            │
│  │                                                        │            │
│  └────────────────────────────────────────────────────────┘            │
│                                                                         │
│  Server: LAPTOP-3S88H9S1                                              │
│  Connection: Trusted Connection, TrustServerCertificate              │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
                                  │
                      External Services
                                  ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                      EXTERNAL SERVICES TIER                             │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌──────────────────────┐              ┌──────────────────────┐       │
│  │    VNPay Gateway     │              │   Gmail SMTP Server  │       │
│  │  (Payment Process)   │              │  (Email Service)     │       │
│  │  - Sandbox: URL      │              │  - Send Emails       │       │
│  │  - Payment IPN       │              │  - Notifications     │       │
│  │  - Signature Verify  │              │  - Confirmations     │       │
│  └──────────────────────┘              └──────────────────────┘       │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘

```

---

## II. DETAILED COMPONENT ARCHITECTURE

### **2.1 Frontend Components Structure**

```
Frontend/
│
├── Public Pages (No Authentication Required)
│   ├── index.html
│   │   ├── Header Component
│   │   │   ├── Logo/Brand
│   │   │   ├── Navigation Menu
│   │   │   └── User Section (Login/Logout)
│   │   ├── Hero Banner Section
│   │   ├── Room Showcase
│   │   ├── Featured Amenities
│   │   └── Footer
│   │
│   ├── login.html
│   │   └── Login Form
│   │       ├── Email Input
│   │       ├── Password Input
│   │       ├── Submit Button
│   │       └── Register Link
│   │
│   ├── register.html
│   │   └── Registration Form
│   │       ├── Name Input
│   │       ├── Phone Input
│   │       ├── CCCD Input
│   │       ├── Email Input
│   │       ├── Password Input
│   │       ├── Submit Button
│   │       └── Login Link
│   │
│   ├── forgot-password.html
│   │   └── Password Recovery
│   │       ├── Email Input
│   │       ├── Verification Code
│   │       ├── New Password
│   │       └── Submit Button
│   │
│   └── detail.html
│       ├── Room Gallery
│       ├── Room Information
│       │   ├── Room Type
│       │   ├── Price
│       │   ├── Area
│       │   ├── Capacity
│       │   └── Amenities
│       └── Booking Form
│           ├── Check-in Picker
│           ├── Check-out Picker
│           ├── Guest Count
│           └── Book Button
│
├── Protected Pages (Authentication Required)
│   ├── profile.html
│   │   ├── User Info Display
│   │   ├── Edit Profile Form
│   │   ├── Booking History Section
│   │   ├── Payment History Section
│   │   └── Logout Button
│   │
│   ├── booking-confirm.html
│   │   ├── Booking Details Card
│   │   │   ├── Booking ID
│   │   │   ├── Customer Info
│   │   │   ├── Room Details
│   │   │   └── Dates
│   │   ├── Payment Summary
│   │   │   ├── Total Amount
│   │   │   ├── Deposit
│   │   │   ├── Remaining
│   │   │   └── Payment Method
│   │   └── Action Buttons
│   │       ├── Confirm Payment
│   │       └── Back Button
│   │
│   └── admin.html
│       ├── Sidebar Menu
│       │   ├── Dashboard
│       │   ├── Room Management
│       │   ├── Booking Management
│       │   ├── Customer Management
│       │   ├── Payment Management
│       │   ├── Reports
│       │   └── Logout
│       │
│       └── Main Content Area
│           ├── Tab Panes
│           │   ├── Dashboard Tab
│           │   │   ├── Statistics Cards
│           │   │   ├── Charts
│           │   │   └── Recent Activities
│           │   │
│           │   ├── Rooms Tab
│           │   │   ├── Rooms Table
│           │   │   ├── Add Room Button
│           │   │   ├── Edit/Delete Options
│           │   │   └── Room Status
│           │   │
│           │   ├── Bookings Tab
│           │   │   ├── Bookings Table
│           │   │   ├── Filter Options
│           │   │   ├── Status Updates
│           │   │   └── Cancel Options
│           │   │
│           │   ├── Customers Tab
│           │   │   ├── Customers Table
│           │   │   ├── Search Function
│           │   │   ├── View Details
│           │   │   └── Contact Options
│           │   │
│           │   ├── Payments Tab
│           │   │   ├── Transaction History
│           │   │   ├── Status Filters
│           │   │   ├── Export Option
│           │   │   └── Revenue Summary
│           │   │
│           │   └── Reports Tab
│           │       ├── Custom Reports
│           │       ├── Date Range Picker
│           │       ├── Export Options
│           │       └── Analytics Graphs
│           │
│           └── Loading Spinner
│               ├── For Form Submissions
│               ├── For Data Loading
│               └── For Payment Processing

```

### **2.2 Backend API Structure**

```
ASP.NET Core API
│
├── Controllers/
│   │
│   ├── KhachHangController.cs
│   │   ├── [HttpPost] dang-ky
│   │   │   └── Register new customer
│   │   │
│   │   ├── [HttpPost] dang-nhap
│   │   │   ├── Validate credentials
│   │   │   ├── Generate JWT token
│   │   │   └── Return user info
│   │   │
│   │   ├── [HttpGet] thong-tin
│   │   │   └── Get authenticated user profile
│   │   │
│   │   ├── [HttpPut] cap-nhat-profile
│   │   │   └── Update user information
│   │   │
│   │   └── [HttpPost] cap-nhat-password
│   │       └── Change password with hash
│   │
│   ├── DatPhongController.cs
│   │   ├── [HttpPost] tao-don
│   │   │   └── Create new booking
│   │   │
│   │   ├── [HttpGet] lich-su/{maKh}
│   │   │   └── Get customer booking history
│   │   │
│   │   ├── [HttpGet] chi-tiet/{maDatPhong}
│   │   │   └── Get booking details
│   │   │
│   │   ├── [HttpPut] huy-don/{maDatPhong}
│   │   │   └── Cancel booking
│   │   │
│   │   └── [HttpPut] cap-nhat/{maDatPhong}
│   │       └── Update booking status
│   │
│   ├── ThanhToanController.cs
│   │   ├── [HttpGet] tao-url
│   │   │   ├── Generate VNPay payment URL
│   │   │   └── Add payment parameters
│   │   │
│   │   ├── [HttpGet] vnpay-ipn
│   │   │   ├── Verify payment signature
│   │   │   ├── Update transaction status
│   │   │   └── Update booking status
│   │   │
│   │   ├── [HttpGet] lich-su/{maKh}
│   │   │   └── Get customer payment history
│   │   │
│   │   └── [HttpPost] hoan-tien
│   │       └── Process refund
│   │
│   ├── PhongController.cs
│   │   ├── [HttpGet] danh-sach
│   │   │   ├── Get all rooms
│   │   │   ├── Filter by type, price
│   │   │   └── Pagination
│   │   │
│   │   ├── [HttpGet] chi-tiet/{maPhong}
│   │   │   └── Get room details with images
│   │   │
│   │   ├── [HttpGet] kiem-tra-trong/{maPhong}
│   │   │   ├── Check room availability
│   │   │   ├── Date range validation
│   │   │   └── Return availability status
│   │   │
│   │   ├── [HttpPost] them-phong
│   │   │   └── Admin: Create new room
│   │   │
│   │   ├── [HttpPut] cap-nhat/{maPhong}
│   │   │   └── Admin: Update room details
│   │   │
│   │   └── [HttpDelete] xoa/{maPhong}
│   │       └── Admin: Delete room
│   │
│   └── AdminController.cs
│       ├── [HttpGet] dashboard
│       │   ├── Get statistics
│       │   ├── Total customers
│       │   ├── Total bookings
│       │   ├── Total revenue
│       │   └── Available rooms
│       │
│       ├── [HttpGet] bao-cao/doanh-thu
│       │   ├── Revenue report
│       │   └── Date range filtering
│       │
│       ├── [HttpGet] bao-cao/dat-phong
│       │   ├── Booking statistics
│       │   └── Status breakdown
│       │
│       └── [HttpGet] bao-cao/khach-hang
│           ├── Customer statistics
│           └── User activity report
│
├── Services/
│   │
│   ├── EmailService.cs
│   │   ├── SendBookingEmailAsync()
│   │   │   └── Send confirmation emails
│   │   │
│   │   ├── SendPaymentReceiptAsync()
│   │   │   └── Send payment receipts
│   │   │
│   │   ├── SendCancellationEmailAsync()
│   │   │   └── Send cancellation notices
│   │   │
│   │   └── BuildEmailTemplate()
│   │       └── Create HTML email body
│   │
│   └── VnPayLibrary.cs (Custom)
│       ├── AddRequestData()
│       ├── AddResponseData()
│       ├── CreateRequestUrl()
│       ├── ValidateSignature()
│       └── HmacSHA512()
│
├── Models/
│   ├── KhachHang.cs
│   ├── DatPhong.cs
│   ├── Phong.cs
│   ├── HoaDon.cs
│   ├── DatCoc.cs
│   ├── GiaoDich.cs
│   ├── HinhAnh.cs
│   ├── DanhGium.cs
│   ├── KhachSan.cs
│   └── LuxuryHotelContext.cs
│
└── Program.cs
    ├── Service Configuration
    ├── Database Setup
    ├── JWT Configuration
    ├── CORS Configuration
    └── Middleware Setup

```

---

## III. DATA FLOW DIAGRAMS

### **3.1 Complete User Registration Flow**

```
START
  │
  ▼
┌─────────────────────────────┐
│ User fills registration form│
│ - Name                      │
│ - Phone                     │
│ - CCCD                      │
│ - Email                     │
│ - Password                  │
└─────────────────────────────┘
  │
  ▼
┌─────────────────────────────┐
│ Frontend: Validate inputs   │
│ - Email format              │
│ - Password length >= 6      │
│ - Phone format              │
└─────────────────────────────┘
  │
  ├─ Validation Failed
  │   └──> Display Error Message ──> Back to form
  │
  ▼
┌─────────────────────────────┐
│ Send POST /dang-ky to API   │
└─────────────────────────────┘
  │
  ▼
┌─────────────────────────────┐
│ Backend: Check if email     │
│ or phone already exists     │
└─────────────────────────────┘
  │
  ├─ Email/Phone exists
  │   └──> Return 400 BadRequest
  │       └──> Display error on frontend
  │
  ▼
┌─────────────────────────────┐
│ Hash password with BCrypt   │
│ Generate salt + hash        │
└─────────────────────────────┘
  │
  ▼
┌─────────────────────────────┐
│ Create KhachHang object     │
│ Set TrangThai = "active"    │
└─────────────────────────────┘
  │
  ▼
┌─────────────────────────────┐
│ Save to Database            │
│ INSERT into KhachHang       │
└─────────────────────────────┘
  │
  ├─ Save Failed
  │   └──> Return 500 Error
  │
  ▼
┌─────────────────────────────┐
│ Return Success Response     │
│ 200 OK + Message            │
└─────────────────────────────┘
  │
  ▼
┌─────────────────────────────┐
│ Frontend: Show success msg  │
│ Redirect to Login page      │
└─────────────────────────────┘
  │
  ▼
END

```

### **3.2 Complete Booking & Payment Flow**

```
START
  │
  ▼
┌──────────────────────────────┐
│ User selects room details    │
│ Picks check-in & check-out   │
│ Selects number of guests     │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Frontend: Calculate total    │
│ amount (price × nights)      │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ User clicks "Book Now"       │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Frontend: Verify auth token  │
├──────────────────────────────┤
│ Token exists?                │
└──────────────────────────────┘
  │
  ├─ No Token ──> Redirect to Login
  │
  ▼
┌──────────────────────────────┐
│ POST /tao-don to Backend     │
│ - MaKh                       │
│ - MaPhong                    │
│ - NgayNhan                   │
│ - NgayTra                    │
│ - TongTien                   │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Backend: Verify room exists  │
│ Check room availability      │
└──────────────────────────────┘
  │
  ├─ Room not found / not available
  │   └──> Return 400 BadRequest
  │
  ▼
┌──────────────────────────────┐
│ Create DatPhong record       │
│ Status = "Pending"           │
│ Generate confirmation code   │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Save to Database             │
│ Generate MaDatPhong          │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Return booking confirmation  │
│ Return MaDatPhong            │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Frontend: Display confirmation│
│ Show booking details         │
│ Show payment summary         │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ User reviews booking info    │
│ Clicks "Confirm Payment"     │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ GET /tao-url?maDatPhong=X    │
│ &tongTien=Y                  │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Backend: Configure VNPay     │
│ Add payment parameters       │
│ Generate HMAC hash           │
│ Create payment URL           │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Return VNPay payment URL     │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Frontend: Redirect to VNPay  │
│ User enters card info        │
│ Completes payment on VNPay   │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ VNPay processes payment      │
│ Redirects to return URL      │
│ Sends IPN callback           │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Backend: GET /vnpay-ipn      │
│ Receive IPN parameters       │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Extract payment parameters   │
│ - vnp_TxnRef (Booking ID)   │
│ - vnp_ResponseCode           │
│ - vnp_SecureHash             │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Verify HMAC signature        │
│ Calculate expected hash      │
│ Compare with vnp_SecureHash  │
└──────────────────────────────┘
  │
  ├─ Hash mismatch
  │   └──> Return RspCode: 97
  │       └──> Payment unauthorized
  │
  ▼
┌──────────────────────────────┐
│ Find booking in database     │
│ Check if status = "Pending"  │
└──────────────────────────────┘
  │
  ├─ Status not Pending
  │   └──> Return RspCode: 02
  │       └──> Already processed
  │
  ▼
┌──────────────────────────────┐
│ Check vnp_ResponseCode       │
│ If = "00" (Success)          │
└──────────────────────────────┘
  │
  ├─ Response code not 00
  │   └──> Return RspCode: 01
  │       └──> Payment failed
  │
  ▼
┌──────────────────────────────┐
│ Update booking status        │
│ Status = "Success"           │
│ Update DatCoc (Deposit paid) │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Create GiaoDich record       │
│ Log transaction details      │
│ Save transaction to HoaDon   │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Prepare confirmation email   │
│ Send via EmailService        │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Return RspCode: 00           │
│ Message: "Success"           │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Frontend: Show success page  │
│ Display confirmation details │
│ Booking complete!           │
└──────────────────────────────┘
  │
  ▼
END

```

### **3.3 Admin Dashboard Data Flow**

```
START
  │
  ▼
┌──────────────────────────────┐
│ Admin logs in                │
│ Email + Password             │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Backend: POST /dang-nhap     │
│ Verify admin role            │
│ Generate JWT token           │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Frontend: Store token        │
│ Redirect to admin.html       │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Load Admin Dashboard         │
│ Initialize sidebar menu      │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ GET /api/Admin/dashboard     │
│ (with JWT token)             │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Backend: Query statistics    │
│ - Count all customers        │
│ - Count all bookings         │
│ - Sum revenue                │
│ - Count available rooms      │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Return statistics JSON       │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Frontend: Display cards with │
│ statistics                   │
└──────────────────────────────┘
  │
  ▼
┌──────────────────────────────┐
│ Admin clicks menu items      │
│ - Room Management            │
│ - Booking Management         │
│ - Customer Management        │
│ - Payment History            │
└──────────────────────────────┘
  │
  ├─ Room Management
  │   │
  │   ▼
  │   GET /api/Phong/danh-sach
  │   │
  │   ▼
  │   Display rooms table
  │   with actions:
  │   - Edit
  │   - Delete
  │   - View bookings
  │
  ├─ Booking Management
  │   │
  │   ▼
  │   GET /api/DatPhong/lich-su
  │   │
  │   ▼
  │   Display bookings table
  │   with filters & actions
  │
  ├─ Customer Management
  │   │
  │   ▼
  │   GET /api/KhachHang/danh-sach
  │   │
  │   ▼
  │   Display customers table
  │   Search & filter options
  │
  └─ Payment History
      │
      ▼
      GET /api/ThanhToan/lich-su
      │
      ▼
      Display transactions table
      Revenue breakdown
  │
  ▼
END
```

---

## IV. SECURITY FLOW DIAGRAMS

### **4.1 JWT Authentication Flow**

```
┌─────────────────┐
│  User Login     │
│  credentials    │
└─────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ POST /dang-nhap                 │
│ Email: user@example.com         │
│ Password: user_password         │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Find user by email     │
└─────────────────────────────────┘
        │
        ├─ Not found ──> Return 404
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Check if account       │
│ status is "active"              │
└─────────────────────────────────┘
        │
        ├─ Status locked ──> Return 401
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Verify password        │
│ BCrypt.Verify(input, hash)      │
└─────────────────────────────────┘
        │
        ├─ Mismatch ──> Return 400
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Create Claims list     │
│ - MaKh (User ID)                │
│ - Email                         │
│ - Role (Admin/KhachHang)        │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Create JWT Token       │
│ - Header: Algorithm (HS256)     │
│ - Payload: Claims + Exp (2h)    │
│ - Signature: HMAC-SHA256        │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Return Response        │
│ {                               │
│   token: JWT_TOKEN,             │
│   maKh: user_id,                │
│   vaiTro: "Admin"               │
│ }                               │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Frontend: Store token in        │
│ LocalStorage                    │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Frontend: Attach token to       │
│ Authorization header            │
│ "Bearer {token}"                │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Request to Protected API        │
│ GET /api/KhachHang/thong-tin    │
│ Headers: {                      │
│   Authorization: Bearer ...     │
│ }                               │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Extract token from     │
│ Authorization header            │
│ Parse JWT token                 │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Validate token         │
│ - Check signature               │
│ - Check expiry                  │
│ - Check issuer/audience         │
└─────────────────────────────────┘
        │
        ├─ Invalid ──> Return 401
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Extract claims from    │
│ token                           │
│ Get user ID and role            │
└─────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Check authorization    │
│ against endpoint requirements   │
└─────────────────────────────────┘
        │
        ├─ Unauthorized ──> Return 403
        │
        ▼
┌─────────────────────────────────┐
│ Backend: Execute endpoint logic │
│ Return protected data           │
└─────────────────────────────────┘
```

### **4.2 Payment Security Flow (VNPay)**

```
┌──────────────────────────────┐
│ User ready to pay            │
│ MaDatPhong & Amount          │
└──────────────────────────────┘
        │
        ▼
┌──────────────────────────────┐
│ GET /tao-url                 │
│ ?maDatPhong=X&tongTien=Y     │
└──────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────────────┐
│ Backend: Prepare payment request             │
│ vnp_Version: "2.1.0"                         │
│ vnp_Command: "pay"                           │
│ vnp_TmnCode: Merchant code                   │
│ vnp_Amount: Amount * 100 (in VND x100)      │
│ vnp_CreateDate: Now (yyyyMMddHHmmss)         │
│ vnp_CurrCode: "VND"                          │
│ vnp_Locale: "vn"                             │
│ vnp_OrderInfo: Booking reference             │
│ vnp_ReturnUrl: Return URL after payment      │
│ vnp_TxnRef: Transaction ID                   │
└──────────────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────────────┐
│ Backend: Create signature                    │
│ 1. Sort parameters alphabetically            │
│ 2. Build query string                        │
│ 3. Generate HMAC-SHA512 hash                 │
│    - Input: query string + HashSecret        │
│    - Output: 128-char hex string             │
│ 4. Append signature to URL                   │
└──────────────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────┐
│ Backend: Return payment URL  │
│ https://sandbox.vnpayment... │
└──────────────────────────────┘
        │
        ▼
┌──────────────────────────────┐
│ Frontend: Redirect to VNPay  │
└──────────────────────────────┘
        │
        ▼
┌──────────────────────────────┐
│ User at VNPay gateway        │
│ Enters card info             │
│ Completes 3DS verification   │
└──────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────────────┐
│ VNPay processes payment                      │
│ - Connects to bank                           │
│ - Transfers funds                            │
│ - Generates response                         │
│ vnp_ResponseCode: "00" (Success)            │
│ vnp_TransactionNo: Payment ID                │
│ vnp_OrderInfo: Booking reference             │
│ vnp_PayDate: Payment date/time               │
│ vnp_SecureHash: Response hash (128 char)     │
└──────────────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────────────┐
│ VNPay sends IPN callback to backend          │
│ GET /vnpay-ipn?vnp_TxnRef=X&...             │
│                  &vnp_SecureHash=...         │
└──────────────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────────────┐
│ Backend: Verify IPN callback                 │
│ 1. Extract vnp_SecureHash from params        │
│ 2. Build query string (sorted params)        │
│ 3. Generate HMAC-SHA512 hash                 │
│    - Same algorithm as payment URL           │
│ 4. Compare hashes                            │
│    Received vs Calculated                    │
└──────────────────────────────────────────────┘
        │
        ├─ Hash mismatch ──> Return 97 (Invalid)
        │                    └──> Log suspicious activity
        │
        ▼
┌──────────────────────────────┐
│ Backend: Extract parameters  │
│ - vnp_TxnRef: Booking ID     │
│ - vnp_ResponseCode: Status   │
└──────────────────────────────┘
        │
        ▼
┌──────────────────────────────┐
│ Backend: Find booking        │
│ Check status = "Pending"     │
└──────────────────────────────┘
        │
        ├─ Status != Pending ──> Return 02
        │                        (Already processed)
        │
        ▼
┌──────────────────────────────┐
│ Backend: Check response code │
│ vnp_ResponseCode = "00"?     │
└──────────────────────────────┘
        │
        ├─ Not "00" ──> Payment failed
        │              └──> Return 01
        │
        ▼
┌──────────────────────────────────────────────┐
│ Backend: Update booking & transaction        │
│ 1. Set DatPhong.TrangThai = "Success"        │
│ 2. Create GiaoDich record                    │
│    - Amount, DateTime, Method, Status        │
│ 3. Update DatCoc (mark deposit as paid)      │
│ 4. Create HoaDon if not exists               │
│ 5. Send confirmation email                  │
│ 6. Return RspCode: 00 (Success)              │
└──────────────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────┐
│ Frontend: Show success page  │
│ Display confirmation details │
└──────────────────────────────┘
```

---

## V. DATABASE RELATIONSHIPS DIAGRAM

```
┌─────────────────────────────────────────────────────────────────┐
│                    DATABASE SCHEMA                              │
│                  (SQL Server)                                   │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌──────────────────┐          ┌──────────────────┐            │
│  │ KhachHang (PK)   │ 1 ─────N │ DatPhong (PK)    │            │
│  ├──────────────────┤          ├──────────────────┤            │
│  │ MaKh (INT)       │          │ MaDatPhong (INT) │            │
│  │ HoTen (NVARCHAR) │          │ MaKh (INT, FK)   │            │
│  │ Email (VARCHAR)  │          │ MaPhong (VARCHAR)│            │
│  │ MatKhau (VARCHAR)│          │ NgayNhan (DATE)  │            │
│  │ SoDienThoai      │          │ NgayTra (DATE)   │            │
│  │ Cmnd (VARCHAR)   │          │ SoNguoi (INT)    │            │
│  │ NgayTao (DATE)   │          │ TongTien (DECIMAL)           │
│  │ TrangThai (VARCHAR)         │ TrangThai        │            │
│  │ VaiTro (VARCHAR) │          │ MaXacNhan        │            │
│  └──────────────────┘          └──────────────────┘            │
│         │                               │ 1                    │
│         │                               │                      │
│         │                    ┌──────────┴─────────┐             │
│         │                    │                   │              │
│         │                    ▼ 1                 ▼ 1            │
│         │              ┌──────────────┐   ┌──────────────┐      │
│         │              │ DatCoc (PK)  │   │ HoaDon (PK)  │      │
│         │              ├──────────────┤   ├──────────────┤      │
│         │              │ MaDatCoc     │   │ MaHd (INT)   │      │
│         │              │ MaDatPhong   │   │ MaDatPhong   │      │
│         │              │ (FK, UNIQUE) │   │ (FK)         │      │
│         │              │ SoTienCoc    │   │ TongTien     │      │
│         │              │ NgayDatCoc   │   │ SoTienDaCoc  │      │
│         │              │ TrangThai    │   │ SoTienConLai │      │
│         │              └──────────────┘   │ NgayXuatHd   │      │
│         │                                 │ TrangThaiTt  │      │
│         │                                 └──────────────┘      │
│         │                                       │ 1             │
│         │                                       │               │
│         │                                       ▼ N             │
│         │                              ┌──────────────────┐     │
│         │                              │ GiaoDich (PK)    │     │
│         │                              ├──────────────────┤     │
│         │                              │ MaGd (INT)       │     │
│         │                              │ MaHd (INT, FK)   │     │
│         │                              │ SoTien (DECIMAL) │     │
│         │                              │ NgayGd (DATETIME)│     │
│         │                              │ PhuongThuc       │     │
│         │                              │ TrangThai        │     │
│         │                              └──────────────────┘     │
│         │                                                       │
│         │                                                       │
│         │          ┌────────────────────────────────┐           │
│         │          │                                │           │
│         ▼ N        ▼ 1                              ▼ 1        │
│  ┌──────────────────────────────┐   ┌──────────────────────┐    │
│  │  DanhGium (Rating/Review)    │   │  KhachSan (Hotel)    │    │
│  ├──────────────────────────────┤   ├──────────────────────┤    │
│  │ MaDg (INT, PK)               │   │ MaKs (VARCHAR, PK)   │    │
│  │ MaKh (INT, FK)               │   │ TenKs (NVARCHAR)     │    │
│  │ MaKs (VARCHAR, FK)           │   │ DiaChi (NVARCHAR)    │    │
│  │ DiemSo (INT) [1-5]           │   │ SoDienThoai (VARCHAR)│    │
│  │ NoiDung (NTEXT)              │   │ NgayThanhLap (DATE)  │    │
│  │ ThoiGian (DATETIME)          │   │ Email (VARCHAR)      │    │
│  │ TrangThai (VARCHAR)          │   └──────────────────────┘    │
│  └──────────────────────────────┘         │ 1                   │
│                                           │                     │
│                                           ▼ N                   │
│                        ┌─────────────────────────────┐           │
│                        │   Phong (Room)              │           │
│                        ├─────────────────────────────┤           │
│                        │ MaPhong (VARCHAR, PK)       │           │
│                        │ MaKs (VARCHAR, FK)          │           │
│                        │ LoaiPhong (NVARCHAR)        │           │
│                        │ Gia (DECIMAL)               │           │
│                        │ DienTich (INT)              │           │
│                        │ TienNghi (NVARCHAR)         │           │
│                        │ SucChua (INT)               │           │
│                        │ TrangThai (VARCHAR)         │           │
│                        └─────────────────────────────┘           │
│                                │ 1                              │
│                                │                                │
│                                ▼ N                              │
│                        ┌─────────────────────────────┐           │
│                        │   HinhAnh (Image)           │           │
│                        ├─────────────────────────────┤           │
│                        │ MaHa (INT, PK)              │           │
│                        │ MaPhong (VARCHAR, FK)       │           │
│                        │ DuongDanHinh (NVARCHAR)     │           │
│                        │ ThuTuHinh (INT)             │           │
│                        │ NgayTai (DATETIME)          │           │
│                        └─────────────────────────────┘           │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## VI. API DOCUMENTATION SUMMARY

### **Base URL**

```
Development: http://localhost:7182/api
Production: https://api.luxuryhotel.com/api
```

### **Authentication Header**

```
Authorization: Bearer {JWT_TOKEN}
Content-Type: application/json
```

### **Response Format**

```json
{
  "data": {},
  "message": "Success message",
  "statusCode": 200,
  "errors": []
}
```

### **Common HTTP Status Codes**

- `200 OK` - Request successful
- `201 Created` - Resource created
- `400 Bad Request` - Invalid parameters
- `401 Unauthorized` - Missing/invalid token
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

---

**Document Generated:** 2026-05-14  
**Project:** Luxury Hotel Management System  
**Architecture Version:** 1.0
