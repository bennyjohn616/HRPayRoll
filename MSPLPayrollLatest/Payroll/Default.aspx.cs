using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Payroll
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportBinding();
            }
        }
        public void ReportBinding()
        {
            //Data for binding to the Report
            DataTable table1 = new DataTable("Employees");
            table1.Columns.Add("First Name");
            table1.Columns.Add("Last Name");
            table1.Columns.Add("Department");
            table1.Columns.Add("Salary");
            table1.Columns.Add("Zip Code");

            table1.Rows.Add("Nava", "Sivasakthi", "IT", "$10000", "50325");
            table1.Rows.Add("Babu", "Raman", "IT", "$7500", "50325");
            table1.Rows.Add("Frank", "Zamar", "IT", "$7000", "50325");

            DataSet ds = new DataSet();
            ds.Tables.Add(table1);

            //Report Binding

            rptDemo.ReportTitle = "Reporting Demo";
            rptDemo.ReportName = "Demo";
            rptDemo.DataBind(ds);
            rptDemo.Visible = true;

        }
    }
}
