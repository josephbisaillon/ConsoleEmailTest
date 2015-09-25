using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailTester
{
    class Program
    {
        static SmtpClient client = new SmtpClient();

        static void Main(string[] args)
        {
            client.SendCompleted += client_SendCompleted;


            // Passing through array of emails, title and body to send an email 
            SendMail(PrivateEmailCredentials.EmailRecips, "SSO HAS SOLD!", "JK Testing");

            while (true)
            {
                Thread.Sleep(100);
            }
        }

        /// <summary>
        ///  Pass the string of recipients and the message to send, send is done ASYNC
        /// </summary>
        /// <param name="recipients">String array of email recipients</param>
        /// <param name="Title">Subject line of the email to be sent</param>
        /// <param name="Body">Body of the email to be sent</param>
        static void SendMail(string[] recipients, string Title, string Body)
        {
            try
            {
                // Command line argument must the the SMTP host.
                client.Port = 25;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(PrivateEmailCredentials.FromEmail, PrivateEmailCredentials.PW);
              
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("tradealertmmm@gmail.com");
                mm.Subject = Title;
                mm.Body = Body;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                foreach (string email in recipients)
                {
                    mm.To.Add(new MailAddress(email));
                }
                client.SendAsync(mm, "Text Message");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
         
        }

        // Gives us some feedback.
        static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                Console.WriteLine("{0} FAILED TO SEND!", e.UserState.ToString());
            else
                Console.WriteLine("{0} Sent Successfully!", e.UserState.ToString());
        }
    }
}
