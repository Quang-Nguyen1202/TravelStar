using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Interfaces;
public interface IEmailService
{
    Task SendConfirmBookingEmailAsync(string to, string subject, string confirmCode);
    Task SendInfoBookingSuccessEmailAsync(string to, string subject);
}