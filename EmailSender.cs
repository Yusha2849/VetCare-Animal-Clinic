using VetCare_Animal_Clinic;
using System.Net;
using System.Net.Mail;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("vetclinic94@gmail.com", "rhhj cxbz guvv wzme")
        };

        return client.SendMailAsync(
            new MailMessage(from: "vetclinic94@gmail.com",
                            to: email,
                            subject,
                            message
                            ));
    }
}