// -----------------------------------------------------------------------
// <copyright file="GoforMail.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NotificationEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net.Mail;
    using System.Net;
    using System.Configuration;
    using System.Net.Configuration;
    using System.IO;
    using System.Web;
    using TraceError;
    using System.Data.SqlClient;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;



    /// <summary>
    /// To handle the smtp and mail message for mail services
    /// </summary>
    public class PayRoleMail
    {
        #region private variable

        private List<string> _mailTo;
        private List<string> _attachement;
        private List<string> _mailCC;
        public string password { get; set; }
        public string ssl { get; set; }
        public string TOmail { get; set; }
        public int author { get; set; }
        public string smtpHost { get; set; }
        public int smtpPortNo { get; set; }
        public string fromusermail { get; set; }

        #endregion

        #region constructors

        public PayRoleMail()
        {

        }

        public PayRoleMail(string mailTo, string subject, string message, List<string> attachement)
        {
            this.MailTo.Add(mailTo);
            this.Subject = subject;
            this.Message = message;
            this.Attachment = attachement;

        }

        public PayRoleMail(string mailTo, string subject, string message, string attachement)
        {
            this.MailTo.Add(mailTo);
            this.Subject = subject;
            this.Message = message;
            this.Attachment.Add(attachement);

        }

        //created by Keerthika on 06/06/2017
        public PayRoleMail(string mailTo, string subject, string message)
        {
            this.MailTo.Add(mailTo);
            this.Subject = subject;
            this.Message = message;


        }
        public PayRoleMail(string mailTo, string[] mailcc, string subject, string message, string CCMessage)
        {
            this.MailTo.Add(mailTo);
            this.Subject = subject;
            this.Message = message;
            this.CCMessage = CCMessage;
            for (int i = 0; i < mailcc.Length; i++)
            {
                if (!string.IsNullOrEmpty(mailcc[i]))
                    this.MailCC.Add(mailcc[i].Trim());

            }


        }

        public PayRoleMail(List<string> mailTo, string subject, string message, List<string> attachement = null)
        {
            this.MailTo = mailTo;
            this.Subject = subject;
            this.Message = message;
            this.Attachment = attachement;
        }

        public PayRoleMail(List<string> mailTo, List<string> mailCC, string subject, string message, string Name, string issueNotes, List<string> attachement = null)
        {
            this.MailTo = mailTo;
            this.MailCC = mailCC;
            this.Subject = subject;
            if (message != string.Empty)
                this.Message = message;
            else
                this.Message = GetContent(subject, Name, issueNotes);
            this.Attachment = attachement;
        }


        #endregion

        #region property

        /// <summary>
        /// get or set the From
        /// </summary>
        public string From { get; private set; }

        /// <summary>
        /// get or set the mail to address
        /// </summary>
        public List<string> MailTo
        {
            get
            {
                if (_mailTo == null)
                {
                    _mailTo = new List<string>();
                }
                return _mailTo;
            }
            set
            {
                _mailTo = value;
            }
        }

        public List<string> MailCC
        {
            get
            {
                if (_mailCC == null)
                {
                    _mailCC = new List<string>();
                }
                return _mailCC;
            }
            set
            {
                _mailCC = value;
            }
        }

        /// <summary>
        /// get or set the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// get or set the message body
        /// </summary>
        public string Message { get; set; }
        public string CCMessage { get; set; }

        /// <summary>
        /// get or set the attachement
        /// </summary>
        public List<string> Attachment
        {
            get
            {
                if (_attachement == null)
                {
                    _attachement = new List<string>();
                }
                return _attachement;
            }
            set
            {
                _attachement = value;
            }
        }

        #endregion

        #region public methods
        /// <summary>
        ///  Sending Email Method rewritten by K. Tamilvanan on 04-10-2022
        /// </summary>
        /// <returns></returns>
        public bool Send()
        {

            if (this.MailTo == null)
                return false;
            if (this.MailTo.Count <= 0)
                return false;
            MailMessage message = new MailMessage();
            this.From = Convert.ToString(message.From);
            for (int count = 0; count < this.MailTo.Count; count++)
            {
                message.To.Add(new MailAddress(this.MailTo[count]));
            }
            for (int count = 0; count < this.Attachment.Count; count++)
            {
                if (this.Attachment[count] != null)
                    message.Attachments.Add(new Attachment(this.Attachment[count]));
            }
            for (int count = 0; count < this.MailCC.Count; count++)
            {
                if (this.MailCC[count] != null)
                    message.CC.Add(new MailAddress(this.MailCC[count]));
            }
            try
            {
                /*
                message.Subject = this.Subject;
                message.Body = this.Message;
                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString(), int.Parse(ConfigurationManager.AppSettings["Port"].ToString()));
                //  bool mailsend = false;
                ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.Send(message);
                message.Attachments.Dispose();
                message.To.Clear();
                message.Dispose();
                smtp.Dispose();
                return true;
                */

                message.Subject = this.Subject;
                message.Body = this.Message;
                message.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString(), int.Parse(ConfigurationManager.AppSettings["Port"].ToString()));


                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SenderMail"].ToString(), ConfigurationManager.AppSettings["Password"].ToString()) as ICredentialsByHost;
 
                //  bool mailsend = false;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  

                smtp.Send(message);
                message.Attachments.Dispose();
                message.To.Clear();
                message.Dispose();
                smtp.Dispose();
                return true;
            }

            catch (Exception e)
            {
                ErrorLog.Log(e);
                return false;
            } 

        }

        /// <summary>
        /// Sending Greeting Email Method rewritten by K. Tamilvanan on 04-10-2022
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="portNo"></param>
        /// <param name="frommail"></param>
        /// <param name="frompassword"></param>
        /// <param name="sslen"></param>
        /// <returns></returns>
        public bool SendGreetingmail(string smtpServer, int portNo, string frommail, string frompassword, bool sslen)
        {

            if (this.MailTo == null)
                return false;

            if (this.MailTo.Count <= 0)
                return false;


            MailMessage mail = new MailMessage();

            for (int count = 0; count < this.MailTo.Count; count++)
            {
                mail.To.Add(new MailAddress(this.MailTo[count]));
            }
            for (int count = 0; count < this.Attachment.Count; count++)
            {
                if (this.Attachment[count] != null)
                    mail.Attachments.Add(new Attachment(this.Attachment[count]));
            }

            SmtpClient SmtpServer = new SmtpClient();
            mail.From = new MailAddress(frommail);
            mail.Subject = this.Subject;
            mail.IsBodyHtml = true;
            mail.Body = this.Message;
            SmtpServer.Host = smtpServer;
            SmtpServer.Port = portNo;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.Credentials = new System.Net.NetworkCredential(frommail, frompassword) as ICredentialsByHost;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //ktv 20220908 YYYYMMDD 

            try
            {
                SmtpServer.Send(mail);
                return true;
            }

            catch (Exception ex)
            {
                ErrorLog.LogInFile(ex);
                ErrorLog.Log(ex);
                return false;
            }
        }

        public bool OLDSendGreetingmail(string smtpServer, int portNo, string frommail, string frompassword, bool sslen)
        {

            if (this.MailTo == null)
                return false;

            if (this.MailTo.Count <= 0)
                return false;


            MailMessage mail = new MailMessage();

            for (int count = 0; count < this.MailTo.Count; count++)
            {
                mail.To.Add(new MailAddress(this.MailTo[count]));
            }
            for (int count = 0; count < this.Attachment.Count; count++)
            {
                if (this.Attachment[count] != null)
                    mail.Attachments.Add(new Attachment(this.Attachment[count]));
            }

            SmtpClient SmtpServer = new SmtpClient();
            mail.From = new MailAddress(frommail);
            mail.Subject = this.Subject;
            mail.IsBodyHtml = true;
            mail.Body = this.Message;
            SmtpServer.Host = smtpServer;
            SmtpServer.Port = portNo;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(frommail, frompassword);
            ServicePointManager.ServerCertificateValidationCallback =
                  delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            try
            {
                SmtpServer.Send(mail);
                return true;
            }

            catch (Exception ex)
            {
                ErrorLog.LogInFile(ex);
                ErrorLog.Log(ex);
                return false;
            }
        }
        /// <summary>
        ///  SendTestMail Method rewritten by K. Tamilvanan on 04-10-2022
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="portNo"></param>
        /// <param name="frommail"></param>
        /// <param name="frompassword"></param>
        /// <param name="sslen"></param>
        /// <returns></returns>
        public bool SendTestMail(string smtpServer, int portNo, string frommail, string frompassword, bool sslen)
        {

            if (this.MailTo == null)
                return false;

            if (this.MailTo.Count <= 0)
                return false;

            ////MailMessage message = new MailMessage();
            MailMessage mail = new MailMessage();
            ////this.From = Convert.ToString(message.From);
            for (int count = 0; count < this.MailTo.Count; count++)
            {
                mail.To.Add(new MailAddress(this.MailTo[count]));
            }
            for (int count = 0; count < this.Attachment.Count; count++)
            {
                if (this.Attachment[count] != null)
                    mail.Attachments.Add(new Attachment(this.Attachment[count]));
            }

            SmtpClient SmtpServer = new SmtpClient();
            mail.From = new MailAddress(frommail);
            mail.Subject = this.Subject;
            mail.IsBodyHtml = true;
            mail.Body = this.Message;
            SmtpServer.Host = smtpServer;
            SmtpServer.Port = portNo;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.Credentials = new System.Net.NetworkCredential(frommail, frompassword) as ICredentialsByHost;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //ktv 20220908 YYYYMMDD 

            try
            {
                SmtpServer.Send(mail);
                if (this.MailCC.Count != 0)
                {
                    for (int count = 0; count < this.MailCC.Count; count++)
                    {
                        mail.To.Clear();
                        if (this.MailCC[count] != null)
                            mail.To.Add(new MailAddress(this.MailCC[count]));
                        mail.Body = this.CCMessage;
                        SmtpServer.Send(mail);
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                ErrorLog.LogInFile(ex);
                ErrorLog.Log(ex);
                return false;
            }

        }
         

        public bool OLDSendTestMail(string smtpServer, int portNo, string frommail, string frompassword, bool sslen)
        {
           
            if (this.MailTo == null)              
                return false;
            
            if (this.MailTo.Count <= 0)
                return false; 
            
            MailMessage mail = new MailMessage();
            
            for (int count = 0; count < this.MailTo.Count; count++)
            {               
                mail.To.Add(new MailAddress(this.MailTo[count]));
            }
            for (int count = 0; count < this.Attachment.Count; count++)
            {              
                if (this.Attachment[count] != null)
                    mail.Attachments.Add(new Attachment(this.Attachment[count]));
            }
           
            SmtpClient SmtpServer = new SmtpClient();
            mail.From = new MailAddress(frommail);
            mail.Subject = this.Subject;
            mail.IsBodyHtml = true;
            mail.Body = this.Message;
            SmtpServer.Host = smtpServer;
            SmtpServer.Port = portNo;
            SmtpServer.EnableSsl = true;
            
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(frommail, frompassword); 
           
            ServicePointManager.ServerCertificateValidationCallback =
                  delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
         
            try
            {
                SmtpServer.Send(mail);               
                if (this.MailCC.Count != 0)
                {
                    for (int count = 0; count < this.MailCC.Count; count++)
                    {
                        mail.To.Clear();
                        if (this.MailCC[count] != null)
                            mail.To.Add(new MailAddress(this.MailCC[count]));
                        mail.Body = this.CCMessage;
                        SmtpServer.Send(mail);
                    }
                }
                return true;
            }

            catch (Exception ex)
            {              
                ErrorLog.LogInFile(ex);
                ErrorLog.Log(ex);
                return false;
            } 
        }


        private void unwanted()
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string username1 = smtpSection.Network.UserName;

            if (smtpSection != null)
            {

                int port = smtpSection.Network.Port;
                string host = smtpSection.Network.Host;
                string password = smtpSection.Network.Password;
                string username = smtpSection.Network.UserName;

                //smtp.Host = "excelenciaconsulting.com";
                //smtp.EnableSsl = false;
                //smtp.Port = 25;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Credentials = new NetworkCredential("pandiyan@excelenciaconsulting.com", "Excel.123!");
                //MailAddress fromAddress = new MailAddress("pandiyan@excelenciaconsulting.com");
                //message.From = fromAddress;
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Created By  :   Abirami M
        /// Created On  :   May 27 2015
        /// Description :   Get mail content based on mail type
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="name"></param>
        /// <param name="issueNotes"></param>
        /// <returns></returns>
        public string GetContent(string subject, string name, string issueNotes)
        {
            string TemplateFilePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string FilePath = "";
            StreamReader file;
            string EmailTemplate = "";
            switch (subject)
            {
                case "Permit order has been received":
                    FilePath = TemplateFilePath + "EmailTemplates\\PermitConfirmation.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    break;
                case "Issue with Permit Job":
                    FilePath = TemplateFilePath + "EmailTemplates\\IssueWithPermitJob.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    EmailTemplate = EmailTemplate.Replace("{IssueNotes}", issueNotes);
                    break;
                case "Email citing license expiration soon":
                    FilePath = TemplateFilePath + "EmailTemplates\\LicenseExpiration.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    break;
                case "Email citing license has expired":
                    FilePath = TemplateFilePath + "EmailTemplates\\LicenseExpired.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    break;
                case "Email requesting account replenishment":
                    FilePath = TemplateFilePath + "EmailTemplates\\AccountReplenishment.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    EmailTemplate = EmailTemplate.Replace("{amount}", issueNotes);
                    break;
                case "Email citing auth letter running out":
                    FilePath = TemplateFilePath + "EmailTemplates\\AuthLetterRunning.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    break;
                case "Email citing auth letter has run out":
                    FilePath = TemplateFilePath + "EmailTemplates\\AuthLetterRunOut.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    break;
                case "Email citing insurance expiration soon":
                    FilePath = TemplateFilePath + "EmailTemplates\\InsuranceExpiration.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    break;
                case "Email citing insurance has expired":
                    FilePath = TemplateFilePath + "EmailTemplates\\InsuranceExpired.htm";
                    file = File.OpenText(FilePath);
                    EmailTemplate = file.ReadToEnd();
                    file.Close();
                    EmailTemplate = EmailTemplate.Replace("{name}", name);
                    break;
            }
            return EmailTemplate;

        }
        #endregion


    }
}
