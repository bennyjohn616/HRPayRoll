
// -----------------------------------------------------------------------
// <copyright file="TXEmployeeSectionList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To handle the TXEmployeeSectionList
    /// </summary>
    public class TXEmployeeSectionList : List<TXEmployeeSection>
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public TXEmployeeSectionList()
        {

        }
        public TXEmployeeSectionList(Guid employeeId, DateTime effectiveDate)
        {
            TXEmployeeSection txEmployeeSection = new TXEmployeeSection();
            txEmployeeSection.EmployeeId = employeeId;
            txEmployeeSection.EffectiveDate = effectiveDate;
            DataTable dtValue = txEmployeeSection.GetDeclareValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXEmployeeSection txEmployeeSectionTemp = new TXEmployeeSection();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txEmployeeSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionId"])))
                        txEmployeeSectionTemp.SectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txEmployeeSectionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    txEmployeeSectionTemp.DeclaredValue = Convert.ToString(dtValue.Rows[rowcount]["DeclaredValue"]);
                    txEmployeeSectionTemp.ApprovedValue = Convert.ToString(dtValue.Rows[rowcount]["ApprovedValue"]);
                    txEmployeeSectionTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EffectiveDate"])))
                        txEmployeeSectionTemp.EffectiveDate = (Convert.ToDateTime(dtValue.Rows[rowcount]["EffectiveDate"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDocument"])))
                        txEmployeeSectionTemp.IsDocument = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDocument"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txEmployeeSectionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txEmployeeSectionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txEmployeeSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txEmployeeSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txEmployeeSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txEmployeeSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HasPan"])))
                        txEmployeeSectionTemp.HasPan = Convert.ToBoolean(dtValue.Rows[rowcount]["HasPan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PanCardNumber"])))
                        txEmployeeSectionTemp.PanNumber = Convert.ToString(dtValue.Rows[rowcount]["PanCardNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HasDeclaration"])))
                        txEmployeeSectionTemp.HasDeclaration = Convert.ToBoolean(dtValue.Rows[rowcount]["HasDeclaration"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LandLordName"])))
                        txEmployeeSectionTemp.LandLordName = Convert.ToString(dtValue.Rows[rowcount]["LandLordName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LandLordAddress"])))
                        txEmployeeSectionTemp.LandLordAddress = Convert.ToString(dtValue.Rows[rowcount]["LandLordAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalRent"])))
                        txEmployeeSectionTemp.TotalRent = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalRent"]);
                    this.Add(txEmployeeSectionTemp);
                }
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public TXEmployeeSectionList(Guid employeeId,DateTime effectiveDate,bool byProof)
        {
            
            TXEmployeeSection txEmployeeSection = new TXEmployeeSection();
            txEmployeeSection.EmployeeId = employeeId;
            txEmployeeSection.EffectiveDate = effectiveDate;
            txEmployeeSection.Proof = byProof;
            DataTable dtValue = txEmployeeSection.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    TXEmployeeSection txEmployeeSectionTemp = new TXEmployeeSection();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        txEmployeeSectionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SectionId"])))
                        txEmployeeSectionTemp.SectionId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["SectionId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        txEmployeeSectionTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    txEmployeeSectionTemp.DeclaredValue = Convert.ToString(dtValue.Rows[rowcount]["DeclaredValue"]);
                    txEmployeeSectionTemp.ApprovedValue = Convert.ToString(dtValue.Rows[rowcount]["ApprovedValue"]);
                    txEmployeeSectionTemp.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EffectiveDate"])))
                        txEmployeeSectionTemp.EffectiveDate =(Convert.ToDateTime(dtValue.Rows[rowcount]["EffectiveDate"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDocument"])))
                        txEmployeeSectionTemp.IsDocument = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDocument"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        txEmployeeSectionTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        txEmployeeSectionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        txEmployeeSectionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        txEmployeeSectionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        txEmployeeSectionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        txEmployeeSectionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HasPan"])))
                        txEmployeeSectionTemp.HasPan = Convert.ToBoolean(dtValue.Rows[rowcount]["HasPan"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PanCardNumber"])))
                        txEmployeeSectionTemp.PanNumber = Convert.ToString(dtValue.Rows[rowcount]["PanCardNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["HasDeclaration"])))
                        txEmployeeSectionTemp.HasDeclaration = Convert.ToBoolean(dtValue.Rows[rowcount]["HasDeclaration"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LandLordName"])))
                        txEmployeeSectionTemp.LandLordName = Convert.ToString(dtValue.Rows[rowcount]["LandLordName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["LandLordAddress"])))
                        txEmployeeSectionTemp.LandLordAddress = Convert.ToString(dtValue.Rows[rowcount]["LandLordAddress"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["TotalRent"])))
                        txEmployeeSectionTemp.TotalRent = Convert.ToDecimal(dtValue.Rows[rowcount]["TotalRent"]);
                    this.Add(txEmployeeSectionTemp);
                }

            }
        }

        #endregion

        #region property

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public Guid EmployeeId { get; set; }



        #endregion

        #region Public methods

        /// <summary>
        /// Save the Tax txEmployeeSection and add to the list
        /// </summary>
        /// <param name="txEmployeeSection"></param>
        public void AddNew(TXEmployeeSection txEmployeeSection)
        {
            if (txEmployeeSection.Save())
            {
                this.Add(txEmployeeSection);
            }
        }

        /// <summary>
        /// delete the tax finance year data
        /// </summary>
        /// <param name="txEmployeeSection"></param>

        public void DeleteExist(TXEmployeeSection txEmployeeSection)
        {
            if (txEmployeeSection.Delete())
            {
                this.Remove(txEmployeeSection);
            }
        }


        #endregion

    }
}
