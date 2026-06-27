namespace lxrhotel.API.Services
{
    public interface IEmailService
    {
        Task SendNewPasswordAsync(string toEmail, string customerName, string newPassword);
        Task SendBookingEmailAsync(string toEmail, string customerName, int bookingId, decimal amount);
    }
}