﻿using MailKit.Net.Smtp;
using MimeKit;
using lxrhotel.API.Services;

public class EmailService : IEmailService
{
    // Cấu hình email và mật khẩu ứng dụng của bạn
    private const string EmailUsername = "your-email@gmail.com";
    private const string EmailPassword = "your-app-password";
    public async Task SendBookingEmailAsync(string toEmail, string hoTen, int maDon, decimal soTien)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("LXR Hotel Support", EmailUsername));
        message.To.Add(new MailboxAddress(hoTen, toEmail));
        message.Subject = $"[LXR Hotel] Xác nhận đặt phòng thành công - Mã đơn #{maDon}";

        message.Body = new TextPart("html")
        {
            Text = $@"
                <h3>Cảm ơn {hoTen} đã tin tưởng Luxury Hotel!</h3>
                <p>Chúng tôi đã nhận được khoản thanh toán đặt cọc <b>{soTien:N0} VND</b> cho đơn đặt phòng số <b>#{maDon}</b>.</p>
                <p>Thông tin của bạn đã được chuyển đến bộ phận lễ tân. Hẹn gặp bạn tại khách sạn!</p>
                <br>
                <p>Thân mến,<br>Đội ngũ LXR Hotel.</p>"
        };

        using (var client = new SmtpClient())
        {
            
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls); // Máy chủ SMTP của Gmail
            await client.AuthenticateAsync(EmailUsername, EmailPassword); // Xác thực
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public async Task SendNewPasswordAsync(string toEmail, string customerName, string newPassword)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("LXR Hotel Support", EmailUsername));
        message.To.Add(new MailboxAddress(customerName, toEmail));
        message.Subject = "[LXR Hotel] Yêu cầu cấp lại mật khẩu";

        message.Body = new TextPart("html")
        {
            Text = $@"
                <h3>Xin chào {customerName},</h3>
                <p>Chúng tôi đã nhận được yêu cầu cấp lại mật khẩu cho tài khoản của bạn.</p>
                <p>Mật khẩu mới của bạn là: <b>{newPassword}</b></p>
                <p>Vui lòng đăng nhập lại và đổi mật khẩu để đảm bảo an toàn.</p>
                <br>
                <p>Thân mến,<br>Đội ngũ LXR Hotel.</p>"
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(EmailUsername, EmailPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}