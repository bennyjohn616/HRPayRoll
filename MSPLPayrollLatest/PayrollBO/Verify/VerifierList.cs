using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollBO
{
    public class VerifierList : List<Verifier>
    {
        public VerifierList()
        {

        }

        public VerifierList(Guid finyear, Guid empid)
        {
            Verifier verifier = new Verifier();
            DataTable dtValue = verifier.GetTableValues(finyear,empid);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Verifier Verifiertemp = new Verifier();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["VerifierID"])))
                       Verifiertemp.VerifierID = new Guid(Convert.ToString(dtValue.Rows[rowcount]["VerifierID"]));
                   Verifiertemp.finyear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYear"]));
                   Verifiertemp.CompanyName = Convert.ToString(dtValue.Rows[rowcount]["CompanyName"]);
                   Verifiertemp.MailID = Convert.ToString(dtValue.Rows[rowcount]["MailID"]);
                   Verifiertemp.DBConnectionId = Convert.ToInt32(dtValue.Rows[rowcount]["DBConnectionID"]);
                   Verifiertemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                   this.Add(Verifiertemp);
                }
            }
        }

        public VerifierList(Guid finyear)
        {
            Verifier verifier = new Verifier();
            DataTable dtValue = verifier.GetTableValues(finyear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Verifier Verifiertemp = new Verifier();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["VerifierID"])))
                        Verifiertemp.VerifierID = new Guid(Convert.ToString(dtValue.Rows[rowcount]["VerifierID"]));
                    Verifiertemp.finyear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinanceYear"]));
                    Verifiertemp.CompanyName = Convert.ToString(dtValue.Rows[rowcount]["CompanyName"]);
                    Verifiertemp.MailID = Convert.ToString(dtValue.Rows[rowcount]["MailID"]);
                    Verifiertemp.DBConnectionId = Convert.ToInt32(dtValue.Rows[rowcount]["DBConnectionID"]);
                    this.Add(Verifiertemp);
                }
            }
        }

        public Guid finyear { get; set; }
        public Guid VerifierID { get; set; }
        public string CompanyName { get; set; }
        public int DBConnectionId { get; set; }
        public string MailID { get; set; }
        public string FirstName { get; set; }



    }
}
