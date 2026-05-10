using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    public async Task SendBookingEmailAsync(string toEmail, string hoTen, int maDon, decimal soTien)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("LXR Hotel Support", "your-email@gmail.com"));
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
            
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("your-email@gmail.com", "your-app-password");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}