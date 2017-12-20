
using System;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using log4net;
using System.Net.Mime;
using System.IO;

namespace TmsWebApp.HelpingUtilities
{
    public enum EmailType { Support = 1, Reporting = 2 }

    public class EmailSender
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static void SendErrorEmail(string message)

        {
            try
            {
                string localhost = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                string toEmail = "";
                
                toEmail = ConfigurationManager.AppSettings["DevSupportEmailAddress"];

                SmtpClient Client = new SmtpClient();
                Client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"], ConfigurationManager.AppSettings["SMTPPassword"]);
                Client.Host = ConfigurationManager.AppSettings["SMTPHost"];
                Client.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                Client.EnableSsl = ConfigurationManager.AppSettings["SMTPEnableSSL"].ToLower() == "true";
                Client.Send(ConfigurationManager.AppSettings["SMTPSupport"],
                    toEmail, 
                    "Money Router Error Message, Deployment Environment - " + ConfigurationManager.AppSettings["Environment"] + " " + localhost, 
                    message);
                Client.Dispose();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                //throw ex;
                throw new Exception("Internal Message: " + message, ex);
            }
        }

        public static void SendSupportEmail(string message, string receiverAddress,string attachmentFilename = null)
        {
            try
            {
                SmtpClient Client = new SmtpClient(ConfigurationManager.AppSettings["SMTPHost"]);
                System.Net.Mail.MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.Body = message;
                msg.Subject = "Injaz Support Message ";
                msg.From = new MailAddress(ConfigurationManager.AppSettings["SMTPSupport"]);
                msg.To.Add(receiverAddress);

                if (attachmentFilename != null)
                {
                    Attachment attachment = new Attachment(attachmentFilename);
                    
                    msg.Attachments.Add(attachment);
                }

                Client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"], ConfigurationManager.AppSettings["SMTPPassword"]);
                //Client.Host = ConfigurationManager.AppSettings["SMTPHost"];
                Client.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                Client.EnableSsl = ConfigurationManager.AppSettings["SMTPEnableSSL"].ToLower() == "true";
                
                Client.Send(msg);
                Client.Dispose();
               
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        private static void Send(MailMessage Email)
        {
            try
            {
                SmtpClient Client = new SmtpClient();
                Client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"], ConfigurationManager.AppSettings["SMTPPassword"]);
                Client.Host = ConfigurationManager.AppSettings["SMTPHost"];
                Client.Send(Email);
                Client.Dispose();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}

