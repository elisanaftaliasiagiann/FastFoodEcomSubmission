using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace FastFood.Web.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implementasi dummy (belum kirim email sungguhan)
            return Task.CompletedTask;
        }
    }
}
