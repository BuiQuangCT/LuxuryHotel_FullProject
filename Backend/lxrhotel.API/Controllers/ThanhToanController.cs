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

            
            var request = HttpContext.Request;
           
            string vnp_Returnurl = $"{request.Scheme}://{request.Host}/api/ThanhToan/vnpay-return";

            
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

          
            string frontend_url = "http://127.0.0.1:5500/booking-confirm.html";

            if (!checkSignature)
            {
               
                return Redirect($"{frontend_url}?status=error&message=Invalid+signature&orderId={vnp_TxnRef}");
            }

            if (vnp_ResponseCode == "00")
            {
                
                return Redirect($"{frontend_url}?status=success&orderId={vnp_TxnRef}&message=Payment+Success");
            }
            else
            {
               
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

         
            string vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            if (string.IsNullOrEmpty(vnp_TxnRef))
            {
                return Ok(new { RspCode = "99", Message = "Missing TxnRef" });
            }

            
            int orderId = Convert.ToInt32(vnp_TxnRef); 
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_SecureHash = queryDictionary["vnp_SecureHash"].ToString();

            
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            if (!checkSignature)
            {
                return Ok(new { RspCode = "97", Message = "Invalid signature" });
            }

           
            var donDatPhong = await _context.DatPhongs.FindAsync(orderId);
            if (donDatPhong == null)
            {
                return Ok(new { RspCode = "01", Message = "Order not found" });
            }

           
            if (donDatPhong.TrangThai != "Pending")
            {
                return Ok(new { RspCode = "02", Message = "Order already confirmed" });
            }

            
            if (vnp_ResponseCode == "00")
            {
                donDatPhong.TrangThai = "Success";

                
                var datCoc = new DatCoc
                {
                    MaDatPhong = donDatPhong.MaDatPhong,
                    SoTienCoc = donDatPhong.TongTien, 
                    TrangThai = "Đã thanh toán",
                    NgayDatCoc = DateTime.Now
                };
                _context.DatCocs.Add(datCoc);

              
                var khachHang = await _context.KhachHangs.FindAsync(donDatPhong.MaKh);
                if (khachHang != null)
                {
                    
                    _ = _emailService.SendBookingEmailAsync(khachHang.Email, khachHang.HoTen, donDatPhong.MaDatPhong, donDatPhong.TongTien);
                }
            }
            else
            {
                donDatPhong.TrangThai = "Failed";
            }

            
            await _context.SaveChangesAsync();

            
            return Ok(new { RspCode = "00", Message = "Confirm Success" });
        }
    }
}