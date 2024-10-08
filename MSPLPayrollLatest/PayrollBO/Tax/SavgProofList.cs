using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace PayrollBO
{
    public class SavgProofList : List<SavgProof>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public SavgProofList()
        {

        }
        #endregion
        public SavgProofList(Guid financeyearId, Guid employeeId, int companyId)
        {
            SavgProof SavgTemp = new SavgProof();
            SavgTemp.financeyearId = financeyearId;
            SavgTemp.EmployeeId = employeeId;
            SavgTemp.companyId = companyId;
            this.EmployeeId = employeeId;
            this.companyId = companyId;
            DataTable dtValue = SavgTemp.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    SavgProof SavgTemp1 = new SavgProof();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["companyId"])))
                        SavgTemp1.companyId = Convert.ToInt32(dtValue.Rows[rowcount]["companyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"])))
                        SavgTemp1.financeyearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmpId"])))
                        SavgTemp1.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmpId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SerialNo"])))
                        SavgTemp1.SerialNo = Convert.ToInt32(dtValue.Rows[rowcount]["SerialNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Description"])))
                        SavgTemp1.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Uploaddate"])))
                        SavgTemp1.Uploaddate = Convert.ToDateTime(dtValue.Rows[rowcount]["Uploaddate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        SavgTemp1.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (SavgTemp1.Status != null && SavgTemp1.Status != "")
                    {
                        if (SavgTemp1.Status.ToUpper() == "A")
                        {
                            SavgTemp1.Status = "Accepted";
                        }
                        if (SavgTemp1.Status.ToUpper() == "R")
                        {
                            SavgTemp1.Status = "Rejected";
                        }
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Remarks"])))
                        SavgTemp1.Remarks = Convert.ToString(dtValue.Rows[rowcount]["Remarks"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["filename"])))
                        SavgTemp1.Filename = Convert.ToString(dtValue.Rows[rowcount]["filename"]);
                    this.Add(SavgTemp1);
                }
            }
        }


        public SavgProofList(int companyId)
        {
            SavgProof SavgTemp = new SavgProof();
            SavgTemp.companyId = companyId;
            SavgTemp.EmployeeId = Guid.Empty;
            this.companyId = companyId;
            this.EmployeeId = Guid.Empty;
            DataTable dtValue = SavgTemp.GetTableValues();

            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    SavgProof SavgTemp1 = new SavgProof();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["companyId"])))
                        SavgTemp1.companyId = Convert.ToInt32(dtValue.Rows[rowcount]["companyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"])))
                        SavgTemp1.financeyearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["employeeId"])))
                        SavgTemp1.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["employeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SerialNo"])))
                        SavgTemp1.SerialNo = Convert.ToInt32(dtValue.Rows[rowcount]["SerialNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Description"])))
                        SavgTemp1.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        SavgTemp1.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Remarks"])))
                        SavgTemp1.Remarks = Convert.ToString(dtValue.Rows[rowcount]["Remarks"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["filename"])))
                        SavgTemp1.Filename = Convert.ToString(dtValue.Rows[rowcount]["filename"]);
                    this.Add(SavgTemp1);
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


        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Tax txEmployeeSection and add to the list
        /// </summary>
        public void AddNew(SavgProof SavgTemp)
        {
            if (SavgTemp.Save(SavgTemp))
            {
                this.Add(SavgTemp);
            }
        }

        #endregion

    }

    public class JsonSavgProofList
    {
        public SavgProofList SavgProofList;
        public Employee emp;
    }
}
