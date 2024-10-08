using Microsoft.Reporting.WebForms;
using PayRollReports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Payroll.Views.Shared
{
    public partial class DataWizardReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string searchText = string.Empty;


                List<Paysheetatrr> dt = Session["dtdatawizard"] as List<Paysheetatrr>;
                DataTable dts = new DataTable();
                CustomerListReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Datawizard.rdlc");
                CustomerListReportViewer.LocalReport.DataSources.Clear();
                ReportDataSource rdc = new ReportDataSource("dtpaysheet", dt);
                CustomerListReportViewer.LocalReport.DataSources.Add(rdc);
              
                CustomerListReportViewer.LocalReport.Refresh();
                CustomerListReportViewer.DataBind();

            }
        }
    }
}