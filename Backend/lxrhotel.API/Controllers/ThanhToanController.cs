﻿using Microsoft.AspNetCore.Mvc;
using lxrhotel.API.Models;
using lxrhotel.API.Services;
using System;
using System.Threading.Tasks;

namespace lxrhotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly LuxuryHotelContext _context;
        private readonly IEmailService _emailService;

        public ThanhToanController(LuxuryHotelContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet("tao-url")]
        public IActionResult TaoUrlThanhToan(int maDatPhong, decimal tongTien)
        {
            // 1. Cấu hình VNPay 
            string vnp_TmnCode = "JLQ3O2EL"; 
            string vnp_HashSecret = "Y1UDIWP635I8SD7R7SI43AIE591F5ZUM"; 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; 

            // Lấy URL của API để tạo ReturnUrl động
            var request = HttpContext.Request;
            // Chú ý: Cần đảm bảo địa chỉ này có thể được VNPay truy cập.
            // Đối với môi trường local, bạn có thể cần sử dụng ngrok hoặc cấu hình tương tự.
            // Tạm thời, nếu chạy local, ta có thể giả định một URL mà VNPay có thể gọi được.
            // Ở đây, tôi sẽ xây dựng URL dựa trên request đến, nhưng có thể cần chỉnh sửa
            // cho phù hợp với môi trường triển khai của bạn.
            string vnp_Returnurl = $"{request.Scheme}://{request.Host}/api/ThanhToan/vnpay-return";

            // Link để VNPay đá về sau khi thanh toán xong (ĐÃ SỬA)
            // string vnp_Returnurl = "http://127.0.0.1:5500/index.html";

            // 2. Gọi thư viện VnPayLibrary
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (tongTien * 100).ToString()); 
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh_toan_don_" + maDatPhong); 
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", maDatPhong.ToString());

            // 3. Tạo link
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return Ok(new { url = paymentUrl });
        }

        [HttpGet("vnpay-return")]
        public IActionResult VNPayReturn()
        {
            string vnp_HashSecret = "Y1UDIWP635I8SD7R7SI43AIE591F5ZUM";
            var vnpay = new VnPayLibrary();
            var queryDictionary = HttpContext.Request.Query;

            foreach (var kv in queryDictionary)
            {
                if (!string.IsNullOrEmpty(kv.Key) && kv.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(kv.Key, kv.Value.ToString());
                }
            }

            string vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_SecureHash = queryDictionary["vnp_SecureHash"].ToString();

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            // URL của trang frontend để chuyển hướng về
            string frontend_url = "http://127.0.0.1:5500/booking-confirm.html";

            if (!checkSignature)
            {
                // Chuyển hướng về trang frontend với thông báo lỗi
                return Redirect($"{frontend_url}?status=error&message=Invalid+signature&orderId={vnp_TxnRef}");
            }

            if (vnp_ResponseCode == "00")
            {
                // Thanh toán thành công
                // Chuyển hướng về trang frontend với trạng thái thành công
                return Redirect($"{frontend_url}?status=success&orderId={vnp_TxnRef}&message=Payment+Success");
            }
            else
            {
                // Thanh toán thất bại
                // Chuyển hướng về trang frontend với trạng thái thất bại
                return Redirect($"{frontend_url}?status=failed&orderId={vnp_TxnRef}&message=Payment+Failed+-+Code:+{vnp_ResponseCode}");
            }
        }


        [HttpGet("vnpay-ipn")]
        public async Task<IActionResult> VNPayIPN()
        {
           
            string vnp_HashSecret = "Y1UDIWP635I8SD7R7SI43AIE591F5ZUM";

            var vnpay = new VnPayLibrary();

           
            var queryDictionary = HttpContext.Request.Query;
            foreach (var kv in queryDictionary)
            {
                if (!string.IsNullOrEmpty(kv.Key) && kv.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(kv.Key, kv.Value.ToString());
                }
            }

            // Đảm bảo không bị lỗi nếu VNPay không trả về mã GD
            string vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            if (string.IsNullOrEmpty(vnp_TxnRef))
            {
                return Ok(new { RspCode = "99", Message = "Missing TxnRef" });
            }

            // Lấy các tham số quan trọng
            int orderId = Convert.ToInt32(vnp_TxnRef); // Đã ép kiểu về int để khớp với int maDatPhong
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_SecureHash = queryDictionary["vnp_SecureHash"].ToString();

            // 2. Xác thực chữ ký
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            if (!checkSignature)
            {
                return Ok(new { RspCode = "97", Message = "Invalid signature" });
            }

            // 3. Tìm đơn hàng trong Database
            var donDatPhong = await _context.DatPhongs.FindAsync(orderId);
            if (donDatPhong == null)
            {
                return Ok(new { RspCode = "01", Message = "Order not found" });
            }

            // 4. Kiểm tra trạng thái đơn hàng (Chỉ cập nhật nếu đang Pending)
            if (donDatPhong.TrangThai != "Pending")
            {
                return Ok(new { RspCode = "02", Message = "Order already confirmed" });
            }

            // 5. Kiểm tra mã phản hồi từ VNPay (00 là thành công)
            if (vnp_ResponseCode == "00")
            {
                donDatPhong.TrangThai = "Success";

                // --- MODULE 06: Ghi nhận vào bảng Đặt cọc ---
                var datCoc = new DatCoc
                {
                    MaDatPhong = donDatPhong.MaDatPhong,
                    SoTienCoc = donDatPhong.TongTien, // Giả sử đặt cọc 100% tiền đơn
                    TrangThai = "Đã thanh toán",
                    NgayDatCoc = DateTime.Now
                };
                _context.DatCocs.Add(datCoc);

                // --- MODULE 07: Gửi Email tự động ---
                var khachHang = await _context.KhachHangs.FindAsync(donDatPhong.MaKh);
                if (khachHang != null)
                {
                    // Chạy ngầm việc gửi email để không làm chậm phản hồi của IPN
                    _ = _emailService.SendBookingEmailAsync(khachHang.Email, khachHang.HoTen, donDatPhong.MaDatPhong, donDatPhong.TongTien);
                }
            }
            else
            {
                donDatPhong.TrangThai = "Failed";
            }

            // Lưu vào DB
            await _context.SaveChangesAsync();

            // 6. Báo cáo lại cho VNPay là Server đã xử lý xong
            return Ok(new { RspCode = "00", Message = "Confirm Success" });
        }
    }
}