using System.Net;
using System.Net.Mail;
using System.Text;
using AltV.Net;

namespace TerraTex_RolePlay_AltV_Server.Lib.BaseSystem;

public class Email
{
    public bool Enabled { get; }
    public string? Host { get; }
    public int Port { get; }
    public string? User { get; }
    private string? Password { get; }
    public bool EnableSsl { get; }
    public string? FromEmail { get; }
    public string? FromName { get; }
    public string Subject { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string ToEmail { get; set; } = string.Empty;

    public Email(string toEmail, string subject, string text) : this()
    {
        Subject = subject;
        Text = text;
        ToEmail = toEmail;
    }

    public Email()
    {
        var mailConfig = Alt.Core.GetServerConfig().Get("TerraTex").Get("Email");

        Enabled = mailConfig.Get("enabled").GetBoolean().GetValueOrDefault(false);
        if (Enabled)
        {
            Host = mailConfig.Get("host").GetString()!;
            Port = mailConfig.Get("port").GetInt()!.Value;
            User = mailConfig.Get("user").GetString()!;
            Password = mailConfig.Get("password").GetString()!;
            EnableSsl = mailConfig.Get("enableSSL").GetBoolean().GetValueOrDefault(true);
            FromEmail = mailConfig.Get("fromEmail").GetString()!;
            FromName = mailConfig.Get("fromName").GetString()!;
        }
    }

    public void Send()
    {
        if (Enabled)
        {
            SmtpClient client = new SmtpClient
            {
                Port = Port,
                Host = Host!,
                EnableSsl = EnableSsl,
                Timeout = 30000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };

            MailMessage mm =
                new MailMessage(new MailAddress(FromEmail!, FromName), new MailAddress(ToEmail))
                {
                    Subject = Subject,
                    Body = Text,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    // DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };

            try
            {
                client.Send(mm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex);
            }
        }
        else
        {
            Console.WriteLine("MAIL DISBALED - DID NOT SEND MAIL - Subject: " + Subject + " - Content: " + Text + " - To: " + ToEmail);
        }
    }
}