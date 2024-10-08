// -----------------------------------------------------------------------
// <copyright file="Company.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
    using System.Data.OleDb;
    using System.Configuration;
    using TraceError;
    /// <summary>
    /// To handle the Company
    /// </summary>
    public class Company
    {

        #region private variable

        // private AttributeModelList _attributeModellist;
        private BranchList _branchList;
        private CategoryList _categorylist;
        private CostCentreList _costCenterlist;
        private DepartmentList _departmentlist;
        private DesignationList _designationList;
        private ESIDespensaryList _esiDespensarylist;
        private EsiLocationList _esiLocationList;
        private GradeList _gradelist;
        private JoiningDocumentList _joiningDocumentlist;


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Company()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Company(int id, int userId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, userId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                this.CompanyName = Convert.ToString(dtValue.Rows[0]["CompanyName"]);
                this.AddressLine1 = Convert.ToString(dtValue.Rows[0]["AddressLine1"]);
                this.AddressLine2 = Convert.ToString(dtValue.Rows[0]["AddressLine2"]);
                this.City = Convert.ToString(dtValue.Rows[0]["City"]);
                this.State = Convert.ToString(dtValue.Rows[0]["State"]);
                this.Country = Convert.ToString(dtValue.Rows[0]["Country"]);
                this.PinCode = Convert.ToString(dtValue.Rows[0]["PinCode"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                this.EMail = Convert.ToString(dtValue.Rows[0]["EMail"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PrimaryCompanyId"])))
                    this.PrimaryCompanyId = Convert.ToInt32(dtValue.Rows[0]["PrimaryCompanyId"]);
                this.PFBankName = Convert.ToString(dtValue.Rows[0]["PFBankName"]);
                this.PFBankAddress = Convert.ToString(dtValue.Rows[0]["PFBankAddress"]);
                this.GroupCode = Convert.ToString(dtValue.Rows[0]["GroupCode"]);
                this.PFEmployeerCode = Convert.ToString(dtValue.Rows[0]["PFEmployeerCode"]);
                this.PensionFundAcNo = Convert.ToString(dtValue.Rows[0]["PensionFundAcNo"]);
                this.EPFAcNo = Convert.ToString(dtValue.Rows[0]["EPFAcNo"]);
                this.AdminChargeAcNo = Convert.ToString(dtValue.Rows[0]["AdminChargeAcNo"]);
                this.InspectionChargeAcNo = Convert.ToString(dtValue.Rows[0]["InspectionChargeAcNo"]);
                this.EDLIAcNo = Convert.ToString(dtValue.Rows[0]["EDLIAcNo"]);
                this.ESIEmployeerContribution = Convert.ToString(dtValue.Rows[0]["ESIEmployeerContribution"]);
                this.PayrollProcessBy = Convert.ToString(dtValue.Rows[0]["PayrollProcessBy"]);
                this.VPFProjectionRequired = (dtValue.Rows[0]["VPFProjectionRequired"] == null || dtValue.Rows[0]["VPFProjectionRequired"] is DBNull) ? false : Convert.ToBoolean(dtValue.Rows[0]["VPFProjectionRequired"]);
                this.VPFProjection = (dtValue.Rows[0]["VPFProjection"] == null || dtValue.Rows[0]["VPFProjection"] is DBNull) ? false : Convert.ToBoolean(dtValue.Rows[0]["VPFProjection"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TDSdays"])))
                    this.TDSdays = Convert.ToInt32(dtValue.Rows[0]["TDSdays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ServiceYearMonth"])))
                    this.ServiceYearMonth = Convert.ToInt32(dtValue.Rows[0]["ServiceYearMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyLogo"])))
                    this.Companylogo = Convert.ToString(dtValue.Rows[0]["CompanyLogo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["cutoffdate"])))
                    this.cutoffdate = Convert.ToDateTime(dtValue.Rows[0]["cutoffdate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["proofcutoffdate"])))
                    this.proofcutoffdate = Convert.ToDateTime(dtValue.Rows[0]["proofcutoffdate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["proofrscutoffdate"])))
                    this.proofrscutoffdate = Convert.ToDateTime(dtValue.Rows[0]["proofrscutoffdate"]); //maddy
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsTwoStep"])))
                //    this.IsTwoStep = Convert.ToBoolean(dtValue.Rows[0]["IsTwoStep"]);

            }
        }

        public Company(int id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                this.CompanyName = Convert.ToString(dtValue.Rows[0]["CompanyName"]);
                this.AddressLine1 = Convert.ToString(dtValue.Rows[0]["AddressLine1"]);
                this.AddressLine2 = Convert.ToString(dtValue.Rows[0]["AddressLine2"]);
                this.City = Convert.ToString(dtValue.Rows[0]["City"]);
                this.State = Convert.ToString(dtValue.Rows[0]["State"]);
                this.Country = Convert.ToString(dtValue.Rows[0]["Country"]);
                this.PinCode = Convert.ToString(dtValue.Rows[0]["PinCode"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                this.EMail = Convert.ToString(dtValue.Rows[0]["EMail"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PrimaryCompanyId"])))
                    this.PrimaryCompanyId = Convert.ToInt32(dtValue.Rows[0]["PrimaryCompanyId"]);
                this.PFBankName = Convert.ToString(dtValue.Rows[0]["PFBankName"]);
                this.PFBankAddress = Convert.ToString(dtValue.Rows[0]["PFBankAddress"]);
                this.GroupCode = Convert.ToString(dtValue.Rows[0]["GroupCode"]);
                this.PFEmployeerCode = Convert.ToString(dtValue.Rows[0]["PFEmployeerCode"]);
                this.PensionFundAcNo = Convert.ToString(dtValue.Rows[0]["PensionFundAcNo"]);
                this.EPFAcNo = Convert.ToString(dtValue.Rows[0]["EPFAcNo"]);
                this.AdminChargeAcNo = Convert.ToString(dtValue.Rows[0]["AdminChargeAcNo"]);
                this.InspectionChargeAcNo = Convert.ToString(dtValue.Rows[0]["InspectionChargeAcNo"]);
                this.EDLIAcNo = Convert.ToString(dtValue.Rows[0]["EDLIAcNo"]);
                this.ESIEmployeerContribution = Convert.ToString(dtValue.Rows[0]["ESIEmployeerContribution"]);
                this.PayrollProcessBy = Convert.ToString(dtValue.Rows[0]["PayrollProcessBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TDSdays"])))
                    this.TDSdays = Convert.ToInt32(dtValue.Rows[0]["TDSdays"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ServiceYearMonth"])))
                    this.ServiceYearMonth = Convert.ToInt32(dtValue.Rows[0]["ServiceYearMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["usercreation"])))
                    this.Usercreations = Convert.ToBoolean(dtValue.Rows[0]["usercreation"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyLogo"])))
                    this.Companylogo = Convert.ToString(dtValue.Rows[0]["CompanyLogo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["VPFProjectionRequired"])))
                    this.VPFProjectionRequired = Convert.ToBoolean(dtValue.Rows[0]["VPFProjectionRequired"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["VPFProjection"])))
                    this.VPFProjection = Convert.ToBoolean(dtValue.Rows[0]["VPFProjection"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["cutoffdate"])))
                    this.cutoffdate = Convert.ToDateTime(dtValue.Rows[0]["cutoffdate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["proofcutoffdate"])))
                    this.proofcutoffdate = Convert.ToDateTime(dtValue.Rows[0]["proofcutoffdate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["proofrscutoffdate"])))
                    this.proofrscutoffdate = Convert.ToDateTime(dtValue.Rows[0]["proofrscutoffdate"]);

            }
        }

        public Company singleCompanyDetails(int id)
        {

            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id);
            if (dtValue.AsEnumerable().Where(r => r.Field<int>("Id") == id).Count() > 0)
            {
                dtValue = dtValue.AsEnumerable().Where(r => r.Field<int>("Id") == id)
                                  .CopyToDataTable();
                if (dtValue.Rows.Count > 0)
                {

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                        this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                    this.CompanyName = Convert.ToString(dtValue.Rows[0]["CompanyName"]);
                    this.AddressLine1 = Convert.ToString(dtValue.Rows[0]["AddressLine1"]);
                    this.AddressLine2 = Convert.ToString(dtValue.Rows[0]["AddressLine2"]);
                    this.City = Convert.ToString(dtValue.Rows[0]["City"]);
                    this.State = Convert.ToString(dtValue.Rows[0]["State"]);
                    this.Country = Convert.ToString(dtValue.Rows[0]["Country"]);
                    this.PinCode = Convert.ToString(dtValue.Rows[0]["PinCode"]);
                    this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                    this.EMail = Convert.ToString(dtValue.Rows[0]["EMail"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                        this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                        this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                        this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                        this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                        this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                        this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PrimaryCompanyId"])))
                        this.PrimaryCompanyId = Convert.ToInt32(dtValue.Rows[0]["PrimaryCompanyId"]);
                    this.PFBankName = Convert.ToString(dtValue.Rows[0]["PFBankName"]);
                    this.PFBankAddress = Convert.ToString(dtValue.Rows[0]["PFBankAddress"]);
                    this.GroupCode = Convert.ToString(dtValue.Rows[0]["GroupCode"]);
                    this.PFEmployeerCode = Convert.ToString(dtValue.Rows[0]["PFEmployeerCode"]);
                    this.PensionFundAcNo = Convert.ToString(dtValue.Rows[0]["PensionFundAcNo"]);
                    this.EPFAcNo = Convert.ToString(dtValue.Rows[0]["EPFAcNo"]);
                    this.AdminChargeAcNo = Convert.ToString(dtValue.Rows[0]["AdminChargeAcNo"]);
                    this.InspectionChargeAcNo = Convert.ToString(dtValue.Rows[0]["InspectionChargeAcNo"]);
                    this.EDLIAcNo = Convert.ToString(dtValue.Rows[0]["EDLIAcNo"]);
                    this.ESIEmployeerContribution = Convert.ToString(dtValue.Rows[0]["ESIEmployeerContribution"]);
                    this.PayrollProcessBy = Convert.ToString(dtValue.Rows[0]["PayrollProcessBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TDSdays"])))
                        this.TDSdays = Convert.ToInt32(dtValue.Rows[0]["TDSdays"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ServiceYearMonth"])))
                        this.ServiceYearMonth = Convert.ToInt32(dtValue.Rows[0]["ServiceYearMonth"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["usercreation"])))
                        this.Usercreations = Convert.ToBoolean(dtValue.Rows[0]["usercreation"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyLogo"])))
                        this.Companylogo = Convert.ToString(dtValue.Rows[0]["CompanyLogo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["VPFProjectionRequired"])))
                        this.VPFProjectionRequired = Convert.ToBoolean(dtValue.Rows[0]["VPFProjectionRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["VPFProjection"])))
                        this.VPFProjection = Convert.ToBoolean(dtValue.Rows[0]["VPFProjection"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["cutoffdate"])))
                        this.cutoffdate = Convert.ToDateTime(dtValue.Rows[0]["cutoffdate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["proofcutoffdate"])))
                        this.proofcutoffdate = Convert.ToDateTime(dtValue.Rows[0]["proofcutoffdate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["proofrscutoffdate"])))
                        this.proofrscutoffdate = Convert.ToDateTime(dtValue.Rows[0]["proofrscutoffdate"]);
                }
            }
            return this;
        }
        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the CompanyName
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Get or Set the AddressLine1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Get or Set the AddressLine2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Get or Set the City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or Set the State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or Set the Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Get or Set the PinCode
        /// </summary>
        public string PinCode { get; set; }

        /// <summary>
        /// Get or Set the PinCode
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Get or Set the PinCode
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        /// 
        public bool chk_sw { get; set;}
        public DateTime cutoffdate { get; set; }

        public DateTime proofcutoffdate { get; set; }
        public DateTime proofrscutoffdate { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsTwoStep { get; set; }
        public bool Usercreations { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// get or set the primary Company Id
        /// </summary>
        public int PrimaryCompanyId { get; set; }
        public int COMDELSTAT { get; set; }

        /// <summary>
        /// get or set the PF Bank Name
        /// </summary>
        public string PFBankName { get; set; }

        /// <summary>
        /// get or set the PF Bank Address
        /// </summary>
        public string PFBankAddress { get; set; }

        /// <summary>
        /// get or set the Group Code
        /// </summary>
        public string GroupCode { get; set; }
        /// <summary>
        /// get or set the PF Employee
        /// </summary>
        public string PFEmployeerCode { get; set; }

        /// <summary>
        /// get or set the Pension Fund Account no
        /// </summary>
        public string PensionFundAcNo { get; set; }

        /// <summary>
        /// get or set the EPF Ac no
        /// </summary>
        public string EPFAcNo { get; set; }

        /// <summary>
        /// Get or set the Admin charge Account no
        /// </summary>
        public string AdminChargeAcNo { get; set; }

        /// <summary>
        /// Get or set the inspection Charge Ac no
        /// </summary>
        public string InspectionChargeAcNo { get; set; }

        /// <summary>
        /// get or set the Edli Account Number
        /// </summary>
        public string EDLIAcNo { get; set; }

        /// <summary>
        /// Get or set the ESIEmployeerContribution
        /// </summary>
        public string ESIEmployeerContribution { get; set; }

        /// <summary>
        /// get or set the PayrollProcessBy
        /// </summary>
        public string PayrollProcessBy { get; set; }

        public bool VPFProjectionRequired { get; set; }

        public int TDSdays { get; set; }

        public int ServiceYearMonth { get; set; }

        public string Companylogo { get; set; }
        public bool VPFProjection { get; set; }
        public string CompanyAddress
        {
            get
            {
                string CompanyAddress = !string.IsNullOrEmpty(this.AddressLine1) ? this.AddressLine1 + (!string.IsNullOrEmpty(this.AddressLine2) ? "*" + this.AddressLine2 : "")
                                       : !string.IsNullOrEmpty(this.AddressLine2) ? this.AddressLine2 + "*" : "";
                CompanyAddress = !string.IsNullOrEmpty(CompanyAddress) ? this.City + "*" : "";
                CompanyAddress = this.CompanyName + "*" + CompanyAddress;
                return CompanyAddress;
            }
        }


        /// <summary>
        /// get or set the attribute model list
        /// </summary>
        //public AttributeModelList AttributeModelList
        //{
        //    get
        //    {
        //        if (object.ReferenceEquals(_attributeModellist, null))
        //        {
        //            if (this.Id > 0)
        //            {
        //                _attributeModellist = new AttributeModelList(this.Id);
        //            }
        //            else
        //            {
        //                _attributeModellist = new AttributeModelList();
        //            }
        //        }
        //        return _attributeModellist;
        //    }
        //    set
        //    {
        //        _attributeModellist = value;
        //    }
        //}

        /// <summary>
        /// get or set the branch list
        /// </summary>
        public BranchList BranchList
        {
            get
            {
                if (object.ReferenceEquals(_branchList, null))
                {
                    if (this.Id > 0)
                    {
                        _branchList = new BranchList(this.Id);

                    }
                    else
                    {
                        _branchList = new BranchList();
                    }
                }
                return _branchList;
            }
            set
            {
                _branchList = value;
            }
        }

        /// <summary>
        /// get or set the categorylsit
        /// </summary>
        public CategoryList CategoryList
        {
            get
            {
                if (object.ReferenceEquals(_categorylist, null))
                {
                    if (this.Id > 0)
                    {
                        _categorylist = new CategoryList(this.Id);
                    }
                    else
                        _categorylist = new CategoryList();
                }
                return _categorylist;
            }
            set
            {
                _categorylist = value;
            }
        }

        /// <summary>
        /// get or set the cost center list
        /// </summary>
        public CostCentreList CostCentreList
        {
            get
            {
                if (object.ReferenceEquals(_costCenterlist, null))
                {
                    if (this.Id > 0)
                    {
                        _costCenterlist = new CostCentreList(this.Id);
                    }
                    else
                        _costCenterlist = new CostCentreList();
                }
                return _costCenterlist;
            }
            set
            {
                _costCenterlist = value;
            }
        }

        /// <summary>
        /// get or set the department list
        /// </summary>
        public DepartmentList DepartmentList
        {
            get
            {
                if (object.ReferenceEquals(_departmentlist, null))
                {
                    if (this.Id > 0)
                    {
                        _departmentlist = new DepartmentList(this.Id);
                    }
                    else
                        _departmentlist = new DepartmentList();
                }
                return _departmentlist;
            }
            set
            {
                _departmentlist = value;
            }
        }

        /// <summary>
        /// get or set the designation list
        /// </summary>
        public DesignationList DesignationList
        {
            get
            {
                if (object.ReferenceEquals(_designationList, null))
                {
                    if (this.Id > 0)
                    {
                        _designationList = new DesignationList(this.Id);
                    }
                    else
                        _designationList = new DesignationList();
                }
                return _designationList;
            }
            set
            {
                _designationList = value;
            }
        }

        /// <summary>
        /// get or set the Esi despensary list
        /// </summary>
        public ESIDespensaryList ESIDespensaryList
        {
            get
            {
                if (object.ReferenceEquals(_esiDespensarylist, null))
                {
                    if (this.Id > 0)
                    {
                        _esiDespensarylist = new ESIDespensaryList(this.Id);
                    }
                    else
                        _esiLocationList = new EsiLocationList();
                }
                return _esiDespensarylist;
            }
            set
            {
                _esiDespensarylist = value;
            }
        }

        /// <summary>
        /// get or set the esi location list
        /// </summary>
        public EsiLocationList EsiLocationList
        {
            get
            {
                if (object.ReferenceEquals(_esiLocationList, null))
                {
                    if (this.Id > 0)
                    {
                        _esiLocationList = new EsiLocationList(this.Id);
                    }
                    else
                        _esiLocationList = new EsiLocationList();
                }
                return _esiLocationList;
            }
            set
            {
                _esiLocationList = value;
            }
        }

        /// <summary>
        /// get or set the garde list
        /// </summary>
        public GradeList GradeList
        {
            get
            {
                if (object.ReferenceEquals(_gradelist, null))
                {
                    if (this.Id > 0)
                    {
                        _gradelist = new GradeList(this.Id);
                    }
                    else
                        _gradelist = new GradeList();
                }
                return _gradelist;
            }
            set
            {
                _gradelist = value;
            }
        }

        /// <summary>
        /// get or set the JoiningDocumentList
        /// </summary>
        public JoiningDocumentList JoiningDocumentList
        {
            get
            {
                if (object.ReferenceEquals(_joiningDocumentlist, null))
                {
                    if (this.Id > 0)
                    {
                        _joiningDocumentlist = new JoiningDocumentList(this.Id);
                    }
                    else
                        _joiningDocumentlist = new JoiningDocumentList();

                }
                return _joiningDocumentlist;
            }
            set
            {
                _joiningDocumentlist = value;
            }
        }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the Company
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Company_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyName", this.CompanyName);
            sqlCommand.Parameters.AddWithValue("@AddressLine1", this.AddressLine1);
            sqlCommand.Parameters.AddWithValue("@AddressLine2", this.AddressLine2);
            sqlCommand.Parameters.AddWithValue("@City", this.City);
            sqlCommand.Parameters.AddWithValue("@State", this.State);
            sqlCommand.Parameters.AddWithValue("@Country", this.Country);
            sqlCommand.Parameters.AddWithValue("@PinCode", this.PinCode);
            sqlCommand.Parameters.AddWithValue("@Phone", this.Phone);
            sqlCommand.Parameters.AddWithValue("@EMail", this.EMail);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@cutoffdate", this.cutoffdate);
            sqlCommand.Parameters.AddWithValue("@proofcutoffdate", this.proofcutoffdate);
            sqlCommand.Parameters.AddWithValue("@proofrscutoffdate", this.proofrscutoffdate);
            sqlCommand.Parameters.AddWithValue("@CompanyLogo", this.Companylogo);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = Convert.ToInt32(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the Company
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Company_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            //return dbOperation.DeleteData(sqlCommand);
            sqlCommand.Parameters.Add("@Comdelstat", SqlDbType.Int).Direction = ParameterDirection.Output;
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@Comdelstat");
            if (status)
            {
                if (Convert.ToInt32(outValue) == 0)
                {
                    this.COMDELSTAT = 1;
                }
                else
                {
                    this.COMDELSTAT = 2;
                }
            }
            return status;
        }

        public void InsertDefaultForNewCompany(int companyId, int userId, string filepath)
        {
            DataSet datas = readXL(filepath);

            InsertTableCategory(companyId, userId, datas);
            InsertAttributeModelType(companyId, userId, datas);
            InsertSetting(companyId, userId, datas);
            //Commented in order to restrict creation of role during new company creation.
            //InsertRole(companyId, datas);
            InsertEmployeeRoleForm(companyId, userId, datas);
            InsertTableLeave(companyId, userId, datas);
            EntityModel entiModel = new EntityModel(ComValue.SalaryTable, companyId);
            AttributeModelList attrlist = new AttributeModelList(companyId);
            int oder = 0;
            attrlist.ForEach(u =>
            {
                //TO ADD DEFAULT FIELD IN THE SALARY FIELD

                if (u.Name == "PD" || u.Name == "LD" || u.Name == "MD" || u.Name == "LWF" || u.Name == "SUPPDAYS" || u.Name == "LOPCREDITDAYS")
                {
                    oder = oder + 1;
                    EntityAttributeModel entityAttr = new EntityAttributeModel();
                    entityAttr.AttributeModelId = u.Id;
                    entityAttr.DisplayOrder = oder + 1;
                    entityAttr.EntityModelId = entiModel.Id;
                    entityAttr.IsActive = true;
                    entityAttr.IsHidden = false;
                    entityAttr.IsMasterField = false;
                    entityAttr.ModifiedBy = userId;
                    entityAttr.CreatedBy = userId;
                    entityAttr.Save();
                }
            });

        }

        public bool SaveCompanySetting()
        {

            SqlCommand sqlCommand = new SqlCommand("Company_SaveSetting");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@PFBankName", this.PFBankName);
            sqlCommand.Parameters.AddWithValue("@PFBankAddress", this.PFBankAddress);
            sqlCommand.Parameters.AddWithValue("@GroupCode", this.GroupCode);
            sqlCommand.Parameters.AddWithValue("@PFEmployeerCode", this.PFEmployeerCode);
            sqlCommand.Parameters.AddWithValue("@PensionFundAcNo", this.PensionFundAcNo);
            sqlCommand.Parameters.AddWithValue("@EPFAcNo", this.EPFAcNo);
            sqlCommand.Parameters.AddWithValue("@AdminChargeAcNo", this.AdminChargeAcNo);
            sqlCommand.Parameters.AddWithValue("@InspectionChargeAcNo", this.InspectionChargeAcNo);
            sqlCommand.Parameters.AddWithValue("@EDLIAcNo", this.EDLIAcNo);
            sqlCommand.Parameters.AddWithValue("@ESIEmployeerContribution", this.ESIEmployeerContribution);
            sqlCommand.Parameters.AddWithValue("@PayrollProcessBy", this.PayrollProcessBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@VPFProjectionRequired", this.VPFProjectionRequired);
            sqlCommand.Parameters.AddWithValue("@VPFProjection", this.VPFProjection);
            sqlCommand.Parameters.AddWithValue("@TDSdays", this.TDSdays);
            sqlCommand.Parameters.AddWithValue("@ServiceYearMonth", this.ServiceYearMonth);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "");
            return status;
        }

        public void Initialize()
        {
            this.BranchList = new BranchList();
            this.CategoryList = new CategoryList();
            this.CostCentreList = new CostCentreList();
            this.DepartmentList = new DepartmentList();
            this.DesignationList = new DesignationList();
            this.ESIDespensaryList = new ESIDespensaryList();
            this.EsiLocationList = new EsiLocationList();
            this.GradeList = new GradeList();
            this.JoiningDocumentList = new JoiningDocumentList();

        }
        #endregion

        #region private methods


        /// <summary>
        /// Select the Company
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int id, int userId)
        {

            SqlCommand sqlCommand = new SqlCommand("Company_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@UserId", userId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(int id)
        {

            SqlCommand sqlCommand = new SqlCommand("SingleCompany_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            //sqlCommand.Parameters.AddWithValue("@UserId", userId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }





        private DataSet readXL(string srcfile)
        {
            DataSet datas = new DataSet();
            try
            {

                //string sourceFile = srcfile + "\\StaticData\\Company Import.xls";
                string sourceFile = srcfile + ConfigurationManager.AppSettings["StaticCompanyData"].ToString();

                string strConn = ComValue.GetXLOldebConnection(sourceFile);

                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                //if (sourceFile.Contains(".xlsx"))
                //    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

                OleDbConnection conn;
                OleDbCommand cmd;
                conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable Sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                try
                {
                    for (int i = 0; i < Sheets.Rows.Count; i++)
                    {

                        string worksheets = Sheets.Rows[i]["TABLE_NAME"].ToString();

                        string sqlQuery = String.Format("SELECT * FROM [{0}]", worksheets);
                        Console.WriteLine(worksheets);
                        cmd = new OleDbCommand(sqlQuery, conn);
                        cmd.CommandType = CommandType.Text;
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dt.TableName = worksheets;
                        datas.Tables.Add(dt);

                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.LogInFile(ex);
                    ErrorLog.Log(ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();

                }

            }
            catch (Exception ex)
            {

                ErrorLog.Log(ex);
                throw ex;
            }
            return datas;

        }

        private void InsertTableCategory(int companyId, int userId, DataSet ds)
        {
            DataTable dtValue = ds.Tables["TableCategory$"];
            for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
            {
                TableCategory tblCat = new TableCategory();
                tblCat.CompanyId = companyId;
                tblCat.CreatedBy = userId;
                tblCat.ModifiedBy = userId;
                tblCat.Description = Convert.ToString(dtValue.Rows[cnt]["Description"]);
                tblCat.Name = Convert.ToString(dtValue.Rows[cnt]["Name"]);
                tblCat.IsDeleted = false;
                tblCat.IsActive = true;
                if (tblCat.Save())
                {
                    if (tblCat.Name == "Payroll")
                    {
                        InsertEntityModel(companyId, userId, tblCat.Id, ds);
                    }
                }

            }
        }
        private void InsertTableLeave(int companyId, int userId, DataSet ds)
        {
            DataTable dtValue = ds.Tables["TableLeave$"];
            for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
            {
                Tableleave tblLev = new Tableleave();
                tblLev.CompanyId = companyId;
                tblLev.CreatedBy = userId.ToString();
                tblLev.colour = Convert.ToString(dtValue.Rows[cnt]["colour"]);
                tblLev.LeaveType = Convert.ToString(dtValue.Rows[cnt]["LeaveType"]);
                //tblLev.Id = new Guid(dtValue.Rows[cnt]["Id"].ToString());

                tblLev.IsDeleted = false;

                tblLev.Save();


            }
        }

        #region properties
        /// <summary>
        /// ModifiedBy:sharmila
        /// ModifiedOn:18.04.17
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="ds"></param>
        //private void InsertTaxSection(int companyId, int userId, DataSet ds)
        //{
        //    try
        //    {
        //        AttributeModelList Modelobj = new AttributeModelList(companyId);

        //        DataTable dtValue = ds.Tables["AttributeModel$"];
        //        for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
        //        {
        //            AttributeModel attrMod = new AttributeModel();
        //            attrMod.Name = Convert.ToString(dtValue.Rows[cnt]["Name"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[cnt]["Id"])) && Convert.ToString(dtValue.Rows[cnt]["Id"]) != "NULL")
        //                attrMod.Id = new Guid(Convert.ToString(dtValue.Rows[cnt]["Id"]));
        //            Modelobj.Add(attrMod);
        //        }
        //        TXSectionList TSLObj = new TXSectionList();
        //        DataTable dttblvalue = ds.Tables["TaxSections$"];
        //        for (int count = 0; count < dttblvalue.Rows.Count; count++)
        //        {
        //            TXSection TSObj = new TXSection();
        //            TSObj.Id = Guid.NewGuid();
        //            if (!string.IsNullOrEmpty(Convert.ToString(dttblvalue.Rows[count]["Parent Section"])) && Convert.ToString(dttblvalue.Rows[count]["Parent Section"]) != "NULL")
        //            {
        //                var ParrentName = Convert.ToString(dttblvalue.Rows[count]["Parent Section"]);

        //                var currentTSL = TSLObj.Where(d => d.Name == ParrentName).FirstOrDefault();
        //                TSObj.ParentId = currentTSL.Id;
        //            }
        //            TSObj.CompanyId = companyId;
        //            if (!string.IsNullOrEmpty(Convert.ToString(dttblvalue.Rows[count]["Financial YearId"])) && Convert.ToString(dttblvalue.Rows[count]["Financial YearId"]) != "NULL")
        //            {
        //                var FinancialId = Convert.ToString(dttblvalue.Rows[count]["Financial YearId"]);
        //                TSObj.FinancialYearId = new Guid(FinancialId); 

        //            }
        //            //TSObj.FinancialYearId = new Guid(Convert.ToString(companyId));
        //            TSObj.Name = Convert.ToString(dttblvalue.Rows[count]["Name"]);
        //            TSObj.DisplayAs = Convert.ToString(dttblvalue.Rows[count]["Name"]);
        //            TSObj.OrderNo = Convert.ToInt32(dttblvalue.Rows[count]["Order"]);
        //            var lmt = (dttblvalue.Rows[count]["Limit"].ToString());
        //            if ((!string.IsNullOrEmpty(Convert.ToString(lmt))) && lmt != "Null")
        //                TSObj.Limit = Convert.ToDecimal(lmt);
        //            var Examp = (dttblvalue.Rows[count]["Examption Method"].ToString());
        //            if (!string.IsNullOrEmpty(Convert.ToString(Examp)))
        //            {
        //                if (Examp == "Monthly")
        //                    TSObj.ExemptionType = Convert.ToInt32("0");
        //                if (Examp == "Yearly")
        //                    TSObj.ExemptionType = Convert.ToInt32("1");
        //                if (Examp == "")
        //                    TSObj.ExemptionType = Convert.ToInt32("");

        //            }
        //            var GrossDeduct = Convert.ToString(dttblvalue.Rows[count]["IsGrossDeductable"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(GrossDeduct))) && GrossDeduct != "Null")
        //                TSObj.IsGrossDeductable = Convert.ToBoolean(GrossDeduct);

        //            var DocRequired = Convert.ToString(dttblvalue.Rows[count]["IsDocumentRequired"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(DocRequired))) && DocRequired != "Null")
        //                TSObj.IsDocumentRequired = Convert.ToBoolean(DocRequired);

        //            var ApprovelReq = Convert.ToString(dttblvalue.Rows[count]["IsApprovelRequired"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(ApprovelReq))) && ApprovelReq != "Null")
        //                TSObj.IsApprovelRequired = Convert.ToBoolean(ApprovelReq);

        //            var Active = Convert.ToString(dttblvalue.Rows[count]["IsActive"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(Active))) && Active != "Null")
        //            TSObj.IsActive = false;
        //            TSObj.CreatedOn = DateTime.Now;
        //            TSObj.CreatedBy = userId;
        //            TSObj.ModifiedOn = DateTime.Now;
        //            TSObj.ModifiedBy = TSObj.CreatedBy;
        //            TSObj.IsDeleted = false;

        //            TSObj.Formula = Convert.ToString(dttblvalue.Rows[count]["Formula"]);
        //            string formulatoken = Convert.ToString(dttblvalue.Rows[count]["Formula"]); 
        //            char[] delimiterChars = { ',',' ','[',']', '*', '-', '(', ')' };
        //            string[] words = formulatoken.Split(delimiterChars,System.StringSplitOptions.RemoveEmptyEntries);

        //            if (words.Length != 0)
        //            {
        //                for (int c = 0; c < words.Length; c++)
        //                {
        //                    AttributeModel newmod = new AttributeModel();
        //                    newmod = Modelobj.Where(d => d.Name == words[c]).FirstOrDefault();
        //                    if (newmod != null)
        //                    {
        //                        string FetchedID =Convert.ToString(string.Concat("{"+ newmod.Id+"}"));
        //                        formulatoken = formulatoken.Replace(words[c], FetchedID);
        //                        TSObj.Value = Convert.ToString(formulatoken);
        //                    }   

        //                }
        //            }
        //            var projection = Convert.ToString(dttblvalue.Rows[count]["Projection"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(projection))) && projection != "Null")
        //                TSObj.Projection = Convert.ToString(projection);

        //            var DocReq = Convert.ToString(dttblvalue.Rows[count]["SectionType"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(DocReq))) && DocReq != "Null")
        //                TSObj.SectionType = Convert.ToString(DocReq);

        //            //TSObj.IncomeType = Convert.ToInt32(dttblvalue.Rows[count]["IncomeType"]);
        //            var Formulatype = Convert.ToString(dttblvalue.Rows[count]["FormulaType"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(Formulatype))) && Formulatype != "Null")
        //                TSObj.FormulaType = Convert.ToInt32(Formulatype);

        //            var baseformula = Convert.ToString(dttblvalue.Rows[count]["BaseFormula"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(baseformula))) && baseformula != "Null")
        //                TSObj.BaseFormula = Convert.ToString(baseformula);

        //            var basevalue = Convert.ToString(dttblvalue.Rows[count]["BaseValue"]);
        //            if ((!string.IsNullOrEmpty(Convert.ToString(basevalue))) && basevalue != "Null")
        //                TSObj.BaseValue = Convert.ToString(basevalue);

        //            TSLObj.Add(TSObj);

        //        }
        //        TSLObj.ForEach(l =>
        //        {
        //            l.Save();
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        #endregion

        private void InsertAttributeModelType(int companyId, int userId, DataSet ds)
        {
            AttributeModelTypeList retobj = new AttributeModelTypeList();
            DataTable dtValue = ds.Tables["AttributeModelType$"];
            for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
            {
                AttributeModelType attrModtype = new AttributeModelType();
                attrModtype.CompanyId = companyId;
                attrModtype.CreatedBy = userId;
                attrModtype.ModifiedBy = userId;
                attrModtype.DisplayAs = Convert.ToString(dtValue.Rows[cnt]["DisplayAs"]);
                attrModtype.Name = Convert.ToString(dtValue.Rows[cnt]["Name"]);
                attrModtype.IsActive = true;
                string strType = Convert.ToString(dtValue.Rows[cnt]["Id"]);
                attrModtype.AttributeModelList = InsertAttributeModelModel(companyId, userId, strType, ds);
                retobj.Add(attrModtype);
                // attrModtype.Save();
            }
            retobj.ForEach(u =>
            {
                if (u.Save())
                {
                    u.AttributeModelList.ForEach(p =>
                    {
                        if (p.Name == "ESIEMPR")
                        {
                            string str = "";
                            String adad = str;
                        }
                        p.AttributeModelTypeId = u.Id;
                        Guid oldid = p.Id;
                        var child = u.AttributeModelList.Where(s => s.ParentId == oldid).ToList();
                        if (p.ParentId == Guid.Empty)
                        {
                            if (p.Save())
                            {
                                Guid newid = p.Id;
                                child.ForEach(s =>
                                {
                                    var tmp = u.AttributeModelList.Where(q => q.Id == s.Id).FirstOrDefault();
                                    tmp.AttributeModelTypeId = u.Id;
                                    tmp.ParentId = newid;
                                    tmp.ContributionType = 2;
                                    tmp.Save();

                                });
                            }
                        }

                    });
                }

            });
        }

        private AttributeModelList InsertAttributeModelModel(int companyId, int userId, string attrTypeId, DataSet ds)
        {
            AttributeModelList retObj = new AttributeModelList();
            DataTable dtValue = ds.Tables["AttributeModel$"];
            DataRow[] rows = dtValue.Select("AttributeModelTypeId='" + attrTypeId + "'");
            for (int cnt = 0; cnt < rows.Length; cnt++)
            {
                AttributeModel attrModtype = new AttributeModel();
                if (!string.IsNullOrEmpty(Convert.ToString(rows[cnt]["Id"])) && Convert.ToString(rows[cnt]["Id"]) != "NULL")
                    attrModtype.Id = new Guid(Convert.ToString(rows[cnt]["Id"]));
                // attrModtype.Id = Guid.NewGuid();
                attrModtype.AttributeModelTypeId = new Guid(attrTypeId);
                attrModtype.CompanyId = companyId;
                attrModtype.CreatedBy = userId;
                attrModtype.ModifiedBy = userId;
                attrModtype.DisplayAs = Convert.ToString(rows[cnt]["DisplayAs"]);
                attrModtype.Name = Convert.ToString(rows[cnt]["Name"]);
                attrModtype.DataType = Convert.ToString(rows[cnt]["DataType"]);
                attrModtype.DataSize = Convert.ToInt32(rows[cnt]["DataSize"]);
                if (!string.IsNullOrEmpty(Convert.ToString(rows[cnt]["OrderNumber"])) && Convert.ToString(rows[cnt]["OrderNumber"]).ToLower() != "null")
                    attrModtype.OrderNumber = Convert.ToInt32(rows[cnt]["OrderNumber"]);

                attrModtype.IsMandatory = true;
                attrModtype.DefaultValue = Convert.ToString(rows[cnt]["DefaultValue"]);
                if (Convert.ToString(rows[cnt]["IsMandatory"]) == "0")
                    attrModtype.IsMandatory = false;
                attrModtype.IsMonthlyInput = false;
                if (Convert.ToString(rows[cnt]["IsMonthlyInput"]) == "1")
                    attrModtype.IsMonthlyInput = true;
                attrModtype.IsDefault = true;
                if (Convert.ToString(rows[cnt]["IsDefault"]) == "0")
                    attrModtype.IsDefault = false;
                attrModtype.BehaviorType = Convert.ToString(rows[cnt]["BehaviorType"]);
                attrModtype.IsSetting = true;
                if (Convert.ToString(rows[cnt]["IsSetting"]) == "0")
                    attrModtype.IsSetting = false;
                attrModtype.IsIncludeForGrossPay = false;
                if (Convert.ToString(rows[cnt]["IsIncludeForGrossPay"]) == "1")
                    attrModtype.IsIncludeForGrossPay = true;
                attrModtype.IsTaxable = false;
                if (Convert.ToString(rows[cnt]["IsTaxable"]) == "1")
                    attrModtype.IsTaxable = true;
                attrModtype.IsIncrement = false;
                if (Convert.ToString(rows[cnt]["IsIncrement"]) == "1")
                    attrModtype.IsIncrement = true;
                attrModtype.IsReimbursement = false;
                if (Convert.ToString(rows[cnt]["IsReimbursement"]) == "1")
                    attrModtype.IsReimbursement = true;
                attrModtype.FullAndFinalSettlement = false;
                if (Convert.ToString(rows[cnt]["FullAndFinalSettlement"]) == "1")
                    attrModtype.FullAndFinalSettlement = true;
                attrModtype.IsInstallment = false;
                if (Convert.ToString(rows[cnt]["IsInstallment"]) == "1")
                    attrModtype.IsInstallment = true;
                if (!string.IsNullOrEmpty(Convert.ToString(rows[cnt]["ParentId"])) && Convert.ToString(rows[cnt]["ParentId"]) != "NULL")
                    attrModtype.ParentId = new Guid(Convert.ToString(rows[cnt]["ParentId"]));
                attrModtype.ContributionType = Convert.ToInt32(rows[cnt]["ContributionType"]);
                attrModtype.IsDeleted = false;
                retObj.Add(attrModtype);
            }
            return retObj;
        }

        private void InsertEntityModel(int companyId, int userId, Guid tableCatId, DataSet ds)
        {

            DataTable dtValue = ds.Tables["EntityModel$"];
            for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
            {
                EntityModel attrModtype = new EntityModel();
                attrModtype.CompanyId = companyId;
                attrModtype.CreatedBy = userId;
                attrModtype.ModifiedBy = userId;
                attrModtype.IsPhysicalTable = false;
                attrModtype.DisplayAs = Convert.ToString(dtValue.Rows[cnt]["DisplayAs"]);
                attrModtype.Name = Convert.ToString(dtValue.Rows[cnt]["Name"]);
                attrModtype.TableCategoryId = tableCatId;
                attrModtype.IsActive = true;

                if (attrModtype.Save())
                {
                    EntityModelMapping entiymap = new EntityModelMapping();
                    entiymap.EntityTableName = attrModtype.Id.ToString();
                    entiymap.RefEntityModelName = "Employee";
                    entiymap.CompanyId = companyId;
                    entiymap.IsDeleted = false;
                    entiymap.Save();

                }
            }

        }

        private void InsertSetting(int companyId, int userId, DataSet ds)
        {

            DataTable dtValue = ds.Tables["Setting$"];
            for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
            {
                Setting attrModtype = new Setting();
                attrModtype.CompanyId = companyId;
                attrModtype.CreatedBy = userId;
                attrModtype.ModifiedBy = userId;
                attrModtype.ParentId = 0;//need to check
                attrModtype.Id = Convert.ToInt32(dtValue.Rows[cnt]["Id"]);
                attrModtype.DisplayAs = Convert.ToString(dtValue.Rows[cnt]["DisplayAs"]);
                attrModtype.Name = Convert.ToString(dtValue.Rows[cnt]["Name"]);
                attrModtype.IsActive = true;
                SettingDefinitionList settngDef = InsertSettingDefinition(companyId, userId, attrModtype.Id, ds);
                if (attrModtype.Save())
                {

                    settngDef.ForEach(u =>
                    {
                        int parent = u.ParentId;
                        u.SettingId = attrModtype.Id;
                        if (u.Save())
                        {
                            settngDef.Where(p => p.ParentId == parent).ToList().ForEach(q =>
                            {
                                settngDef.Where(s => s.Id == q.Id).FirstOrDefault().ParentId = u.ParentId;
                            });
                        }
                    });
                }
            }

        }

        public void InsertRole(int companyId, DataSet ds)
        {
            DataTable dtValue = ds.Tables["RoleInsert$"];
            RoleList roleList = new RoleList();
            for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
            {
                Role role = new Role();

                role.Name = Convert.ToString(dtValue.Rows[cnt]["Name"]);
                role.DisplayAs = Convert.ToString(dtValue.Rows[cnt]["DisplayAs"]);
                role.Description = Convert.ToString(dtValue.Rows[cnt]["Description"]);
                role.CompanyId = companyId;
                roleList.Add(role);
            }
            roleList.ForEach(r =>
            {
                r.Save();
            });
        }

        //---- Created by Keerthike on 26/05/2017
        public void InsertEmployeeRoleForm(int companyId, int userId, DataSet ds)
        {
            DataTable dtValue = ds.Tables["EmployeeRole$"];
            RoleFormCommandList roleFormCommandlist = new RoleFormCommandList();
            FormCommandList formCommandList = new FormCommandList(true);
            // RoleList role = new RoleList();
            int rId = 0;
            RoleList role = new RoleList(rId, companyId);
            for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
            {

                RoleFormCommand roleFormCommand = new RoleFormCommand();
                roleFormCommand.CompanyId = companyId;
                roleFormCommand.CreatedBy = userId;
                roleFormCommand.ModifiedBy = userId;

                formCommandList.ForEach(f =>
                {
                    var cn = Convert.ToString(dtValue.Rows[cnt]["CommandName"]);
                    var commandName = formCommandList.Where(f1 => cn.Trim() == f1.CommandName.Trim()).FirstOrDefault();
                    if (commandName != null)
                    {
                        roleFormCommand.FormCommandId = commandName.Id;
                    }

                    var roleId = role.Where(r => r.Name == "Employee").FirstOrDefault();
                    if (roleId != null)
                    {
                        roleFormCommand.RoleId = roleId.Id;

                    }
                });


                roleFormCommand.IsRead = Convert.ToBoolean(dtValue.Rows[cnt]["IsRead"]);
                roleFormCommand.IsWrite = Convert.ToBoolean(dtValue.Rows[cnt]["IsWrite"]);

                roleFormCommand.IsRequired = Convert.ToBoolean(dtValue.Rows[cnt]["IsRequired"]);

                roleFormCommand.IsPayrollTransaction = Convert.ToBoolean(dtValue.Rows[cnt]["IsPayrollTransaction"]);
                roleFormCommand.IsApproval = Convert.ToBoolean(dtValue.Rows[cnt]["IsApproval"]);
                roleFormCommand.ReadMessage = Convert.ToString(dtValue.Rows[cnt]["ReadMessage"]);
                roleFormCommand.WriteMessage = Convert.ToString(dtValue.Rows[cnt]["WriteMessage"]);
                roleFormCommand.RequiredMessage = Convert.ToString(dtValue.Rows[cnt]["RequiredMessage"]);
                roleFormCommand.TransactionMessage = Convert.ToString(dtValue.Rows[cnt]["TransactionMessage"]);
                roleFormCommand.ApprovalMessage = Convert.ToString(dtValue.Rows[cnt]["ApprovalMessage"]);
                roleFormCommandlist.Add(roleFormCommand);
            }
            // return roleFormCommandlist;
            roleFormCommandlist.ForEach(r =>
            {
                r.Save();
            });

        }
        private SettingDefinitionList InsertSettingDefinition(int companyId, int userId, int settingid, DataSet ds)
        {
            SettingDefinitionList settngDef = new SettingDefinitionList();
            DataTable dtValue = ds.Tables["SettingDefinition$"];
            DataRow[] rows = dtValue.Select("SettingId=" + settingid);
            for (int cnt = 0; cnt < rows.Length; cnt++)
            {
                SettingDefinition attrModtype = new SettingDefinition();
                attrModtype.CompanyId = companyId;
                attrModtype.CreatedBy = userId;
                attrModtype.ModifiedBy = userId;
                attrModtype.Id = Convert.ToInt32(Convert.ToString(rows[cnt]["Id"]));
                attrModtype.ParentId = Convert.ToInt32(Convert.ToString(rows[cnt]["ParentId"]));
                attrModtype.DisplayAs = Convert.ToString(rows[cnt]["DisplayAs"]);
                attrModtype.Name = Convert.ToString(rows[cnt]["Name"]);
                attrModtype.SettingId = Convert.ToInt32(Convert.ToString(rows[cnt]["SettingId"]));
                attrModtype.ControlType = Convert.ToString(rows[cnt]["ControlType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(rows[cnt]["Value"])) && Convert.ToString(rows[cnt]["Value"]) != "NULL")
                    attrModtype.Value = Convert.ToString(rows[cnt]["Value"]);
                if (!string.IsNullOrEmpty(Convert.ToString(rows[cnt]["RefEntityModelId"])) && Convert.ToString(rows[cnt]["RefEntityModelId"]) != "NULL")
                    attrModtype.RefEntityModelId = Convert.ToString(rows[cnt]["RefEntityModelId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(rows[cnt]["RadioGroupName"])) && Convert.ToString(rows[cnt]["RadioGroupName"]) != "NULL")
                    attrModtype.RadioGroupName = Convert.ToString(rows[cnt]["RadioGroupName"]);
                attrModtype.IsDeleted = false;
                attrModtype.IsActive = true;
                attrModtype.IsUniqueConstraint = false;
                settngDef.Add(attrModtype);
            }
            return settngDef;

        }

        #endregion

    }
}

