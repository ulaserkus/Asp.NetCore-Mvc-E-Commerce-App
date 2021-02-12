using System.Threading.Tasks;

namespace App.webui.EmailService
{
    public interface IEmailSender
    {
        //smtp
        //api => sendgrid
       

       Task SendEmailAsync(string email,string subject,string htmlMessage);

       
    }
}