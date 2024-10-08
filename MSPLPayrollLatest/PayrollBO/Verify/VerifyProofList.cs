using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace PayrollBO
{
    public class VerifyProofList : List<VerifyProof>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public VerifyProofList()
        {

        }
        #endregion
        public VerifyProofList(Guid financeyearId, Guid employeeId)
        {
            VerifyProof vtemp = new VerifyProof();
            vtemp.financeyearId = financeyearId;
            vtemp.EmployeeId = employeeId;
            DataTable dtValue = vtemp.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    VerifyProof vtemp1 = new VerifyProof();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["companyId"])))
                        vtemp1.companyId = Convert.ToInt32(dtValue.Rows[rowcount]["companyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"])))
                        vtemp1.financeyearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmpId"])))
                        vtemp1.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmpId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SerialNo"])))
                        vtemp1.SerialNo = Convert.ToInt32(dtValue.Rows[rowcount]["SerialNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Description"])))
                        vtemp1.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Uploaddate"])))
                        vtemp1.Uploaddate = Convert.ToDateTime(dtValue.Rows[rowcount]["Uploaddate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        vtemp1.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (vtemp1.Status != null && vtemp1.Status != "")
                    {
                        if (vtemp1.Status.ToUpper() == "A")
                        {
                            vtemp1.Status = "Accepted";
                        }
                        if (vtemp1.Status.ToUpper() == "R")
                        {
                            vtemp1.Status = "Rejected";
                        }
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Remarks"])))
                        vtemp1.Remarks = Convert.ToString(dtValue.Rows[rowcount]["Remarks"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["filename"])))
                        vtemp1.Filename = Convert.ToString(dtValue.Rows[rowcount]["filename"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Mailsent"])))
                        vtemp1.Mailsent = Convert.ToString(dtValue.Rows[rowcount]["Mailsent"]);
                    this.Add(vtemp1);
                }
            }
        }


        public Employee Employee { get; set; }

        public int companyId { get; set; }

        public Guid financeyearId { get; set; }
        public Guid EmployeeId { get; set; }
        public int SerialNo { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Filename { get; set; }
        public DateTime Uploaddate { get; set; }

        public string Mailsent { get; set; }


        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Tax txEmployeeSection and add to the list
        /// </summary>
        public void AddNew(VerifyProof vtemp)
        {
            if (vtemp.Save(vtemp))
            {
                this.Add(vtemp);
            }
        }

        #endregion

    }

    public class JsonVerifyProofList
    {
        public VerifyProofList VerifyProofList;
    }

}
