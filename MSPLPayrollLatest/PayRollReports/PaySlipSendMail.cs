using NotificationEngine;
using PayrollBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollReports
{
    public class PaySlipSendMail
    {


        public void PaySlipSend(Company compDetails, Employee emp, int month, int year, MailConfig mailconfig,string filepath)
        {
            string subject = "Pay slip for the month of" + (MonthEnum)month+" _"+ year;
            string message = "<p>Dear "+emp.FirstName+" ,<br/> Please find attached your pay slip for the month of "+ (MonthEnum)month + " "+year+ ". <br/><br/>Thanks and Regrads<br/>HR Team</p>";

            PayRoleMail payrolemail = new PayRoleMail(emp.Email, subject, message, filepath);

            Task SendPayslipMail = new Task(delegate
            {
                payrolemail.SendTestMail(mailconfig.IPAddress, mailconfig.PortNo, mailconfig.FromEmail, mailconfig.MailPassword, mailconfig.EnableSSL);
            });
            SendPayslipMail.Start();


        }

    }
}
