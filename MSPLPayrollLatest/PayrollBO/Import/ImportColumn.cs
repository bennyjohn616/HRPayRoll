using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class ImportColumn
    {
        public Guid OtherTableUniqueId { get; set; }
        public string Name { get; set; }

        public string DisplayAs { get; set; }
        public string MappedColumnName { get; set; }

        public string MinVal { get; set; }

        public string MaxLength { get; set; }

        public bool IsRequired { get; set; }
        public string IsRequiredstr { get; set; }
        public string TableName { get; set; }
        public static List<ImportColumn> GetEmployeeColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();

            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "10", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "First Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "40", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Date Of Joining", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Category", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Last Name", IsRequired = false, IsRequiredstr = "", MaxLength = "40", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Email", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Phone", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Branch", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Designation", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Date Of Birth", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Date Of Wedding", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Confirmation Period", IsRequired = false, IsRequiredstr = "", MaxLength = "24", MinVal = "1" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Confirmation Date", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Separation Date", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Retirement Years", IsRequired = false, IsRequiredstr = "", MaxLength = "60", MinVal = "18" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Retirement Date", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Gender", IsRequired = false, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Department", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Metro", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Location", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "CostCentre", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Grade", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Stop Payment", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Payroll Process", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "Status", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "ESI Location", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "PT Location", IsRequired = false, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee", Name = "ESI Despensary", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeTrainingColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Training", Name = "Training Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "25", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Training", Name = "Training From", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Training", Name = "Training To", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Training", Name = "Certificate Number", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Training", Name = "Institute", IsRequired = false, IsRequiredstr = "", MaxLength = "60", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeFamilyColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Family", Name = "Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "150", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Family", Name = "Address", IsRequired = false, IsRequiredstr = "", MaxLength = "60", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Family", Name = "RelationShip", IsRequired = false, IsRequiredstr = "", MaxLength = "20", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Family", Name = "Date Of Birth", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Family", Name = "Age", IsRequired = false, IsRequiredstr = "", MaxLength = "100", MinVal = "18" });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeAcademicColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Academic", Name = "Degree Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "25", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Academic", Name = "Institution Name", IsRequired = false, IsRequiredstr = "", MaxLength = "60", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Academic", Name = "Year Of Passing", IsRequired = false, IsRequiredstr = "", MaxLength = "3000", MinVal = "1960" });
            return retobj;
        }
        public static List<ImportColumn> GetEmployeeLanguageColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Language", Name = "Language Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Language", Name = "Can Speak", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Language", Name = "Can Read", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Language", Name = "Can Write", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeAddressColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Address", Name = "Address Type", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Address", Name = "Address Line1", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "600", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Address", Name = "Address Line2", IsRequired = false, IsRequiredstr = "", MaxLength = "600", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Address", Name = "City", IsRequired = false, IsRequiredstr = "", MaxLength = "60", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Address", Name = "State", IsRequired = false, IsRequiredstr = "", MaxLength = "60", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Address", Name = "Country", IsRequired = false, IsRequiredstr = "", MaxLength = "60", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Address", Name = "Pin Code", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Address", Name = "Phone", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });            
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeNomineeColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Nominee", Name = "Nominee Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "150", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Nominee", Name = "Address", IsRequired = false, IsRequiredstr = "", MaxLength = "100", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Nominee", Name = "Relation Ship", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Nominee", Name = "DateOfBirth", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Nominee", Name = "Amount Percentage", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Nominee", Name = "Age", IsRequired = false, IsRequiredstr = "", MaxLength = "100", MinVal = "18" });
            retobj.Add(new ImportColumn() { TableName = "Employee Nominee", Name = "Name Of Guardian And Address", IsRequired = false, IsRequiredstr = "", MaxLength = "80", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetEmployeeSeparationColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Separation", Name = "Type Of Separation", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "150", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Separation", Name = "Last Working Date", IsRequired = false, IsRequiredstr = "", MaxLength = "100", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Separation", Name = "Reason", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetEmployeeReleaseColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Release", Name = "Release Date", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "150", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetEmployeeBenefitComponentColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Benefit Component", Name = "Component Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "40", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Benefit Component", Name = "Amount", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Benefit Component", Name = "Effective Date", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeEmergencyContactColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Emergency Contact", Name = "Contact Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "40", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Emergency Contact", Name = "Contact Number", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Emergency Contact", Name = "Relation Ship", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Emergency Contact", Name = "Address", IsRequired = false, IsRequiredstr = "", MaxLength = "80", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeEmployeementColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Employeement", Name = "Previous Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Employeement", Name = "Company Name", IsRequired = false, IsRequiredstr = "", MaxLength = "50", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Employeement", Name = "Position Held", IsRequired = false, IsRequiredstr = "", MaxLength = "20", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Employeement", Name = "Work From", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Employeement", Name = "Work To", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeePersonalColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Personal Mobile No", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Office Mobile No", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Extension No", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Personal Email", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Office Email", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Blood Group", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Print Cheque", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Senior Citizen", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "PaySlip Remarks", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "PAN Number", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "PF Number", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "ESI Number", IsRequired = false, IsRequiredstr = "", MaxLength = "10", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Marital Status", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "No of children", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "PF Confirmation Date", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "AADHAR Number", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "PF UAN", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Father Name", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Personal", Name = "Spouse Name", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetCategoryColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Category", Name = "Category Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "20", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Category", Name = "Display Order", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "20", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetBranchColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Branch", Name = "Branch Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetCostCentreColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Cost Centre", Name = "Cost Centre", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetDepartmentColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Department", Name = "Department Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetDesignationColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Designation", Name = "Designation", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "100", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetESIDespensaryColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "ESI Dispensary", Name = "ESI Despensary", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetESILocationColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "ESI Location", Name = "ESI Location", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "ESI Location", Name = "Applicable", IsRequired = false, IsRequiredstr = "" });
            retobj.Add(new ImportColumn() { TableName = "ESI Location", Name = "Employer Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetGradeColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Grade", Name = "Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "20", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetHRComponentColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "HR Component", Name = "Component Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetJoiningDocumentColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Joining Document", Name = "Document Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetLocationColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Location", Name = "Location", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetPTLocationColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "PT Location", Name = "PT Location", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetMonthlyInputColumn(int companyId)
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Monthly Input", Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "10", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Monthly Input", Name = "Month", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Monthly Input", Name = "Year", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            EntityModel entitymodel = new EntityModel(ComValue.SalaryTable, companyId);
            entitymodel.EntityAttributeModelList.ForEach(p =>
            {
                if (p.AttributeModel.IsMonthlyInput)
                {
                    retobj.Add(new ImportColumn() { TableName = "Monthly Input", Name = p.AttributeModel.Name, DisplayAs = p.AttributeModel.DisplayAs, OtherTableUniqueId = p.AttributeModel.Id, IsRequired = true, IsRequiredstr = "", MaxLength = "", MinVal = "" });
                }

            });
            EntityList entityList = new EntityList(entitymodel.Id);
            entityList.ForEach(p =>
            {
                EntityBehaviorList entityBehavior = new EntityBehaviorList(p.Id, p.EntityModelId);
                //var result = peopleList2.Where(p => !peopleList1.Any(p2 => p2.ID == p.ID));
                List<EntityBehavior> seletedEntityBehavir = entityBehavior.Where(u => u.ValueType == 2 && !retobj.Any(u2 => u2.OtherTableUniqueId == u.AttributeModelId)).ToList();//Monthly Input
                seletedEntityBehavir.ForEach(q =>
                {
                    var tmp = entitymodel.EntityAttributeModelList.Where(s => s.AttributeModel.Id == q.AttributeModelId).FirstOrDefault();
                    if (!object.ReferenceEquals(tmp, null))
                        retobj.Add(new ImportColumn() { TableName = "Monthly Input", Name = tmp.AttributeModel.Name, DisplayAs = tmp.AttributeModel.DisplayAs, OtherTableUniqueId = q.AttributeModelId, IsRequired = true, IsRequiredstr = "", MaxLength = "", MinVal = "" });
                });

            });


            return retobj;
        }
        public static List<ImportColumn> GetSalaryColumn(int companyId)
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "10", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Salary", Name = "Month", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Salary", Name = "Year", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            EntityModel entitymodel = new EntityModel(ComValue.SalaryTable, companyId);
            entitymodel.EntityAttributeModelList.ForEach(p =>
            {
                retobj.Add(new ImportColumn() { TableName = "Salary", Name = p.AttributeModel.Name, OtherTableUniqueId = p.AttributeModel.Id, IsRequired = true, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeAdditionalInfoColumn(int companyId)
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "10", MinVal = "" });
            EntityModel entitymodel = new EntityModel(ComValue.EMPADDLINFOTable, companyId);
            entitymodel.EntityAttributeModelList.ForEach(p =>
            {
                retobj.Add(new ImportColumn() { TableName = "Employee AdditionalInfo", Name = p.AttributeModel.Name, OtherTableUniqueId = p.AttributeModel.Id, IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeBankInfoColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "10", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Bank Details", Name = "Bank Account No", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Bank Details", Name = "Bank Name", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "50", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Bank Details", Name = "Bank Branch", IsRequired = false, IsRequiredstr = "", MaxLength = "50", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Bank Details", Name = "Bank Address", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Bank Details", Name = "IFSC Number", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Bank Details", Name = "City", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Bank Details", Name = "State", IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetIncrementColumn(int companyId)
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "10", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Increment", Name = "Effective Date", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Increment", Name = "Apply Month", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Increment", Name = "Apply Year", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Increment", Name = "Before LOP", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Increment", Name = "After LOP", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            EntityModel entitymodel = new EntityModel(ComValue.SalaryTable, companyId);
            bool isReq = false;
            entitymodel.EntityAttributeModelList.ForEach(p =>
            {
                if (p.AttributeModel.IsIncrement && isReq == false)
                {
                    isReq = true;
                    retobj.Add(new ImportColumn() { TableName = "Increment", Name = p.AttributeModel.Name, DisplayAs = p.AttributeModel.DisplayAs, OtherTableUniqueId = p.AttributeModel.Id, IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
                }
                else if (p.AttributeModel.IsIncrement)
                {
                    retobj.Add(new ImportColumn() { TableName = "Increment", Name = p.AttributeModel.Name, DisplayAs = p.AttributeModel.DisplayAs, OtherTableUniqueId = p.AttributeModel.Id, IsRequired = true, IsRequiredstr = "", MaxLength = "", MinVal = "" });
                }

            });

            return retobj;
        }

        public static List<ImportColumn> GetSalaryMasterColumn(int companyId)
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "10", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Salary Master", Name = "Salary Grade", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            EntityModel entitymodel = new EntityModel(ComValue.SalaryTable, companyId);
            EntityList entityList = new EntityList(entitymodel.Id);
            entityList.ForEach(p =>
            {
                EntityBehaviorList entityBehavior = new EntityBehaviorList(p.Id, p.EntityModelId);
                //var result = peopleList2.Where(p => !peopleList1.Any(p2 => p2.ID == p.ID));
                List<EntityBehavior> seletedEntityBehavir = entityBehavior.Where(u => u.ValueType == 1 && !retobj.Any(u2 => u2.OtherTableUniqueId == u.AttributeModelId)).ToList();//master Input
                seletedEntityBehavir.ForEach(q =>
                {
                    var tmp = entitymodel.EntityAttributeModelList.Where(s => s.AttributeModel.Id == q.AttributeModelId).FirstOrDefault();
                    if (!object.ReferenceEquals(tmp, null))
                        retobj.Add(new ImportColumn() { TableName = "Salary Master", Name = tmp.AttributeModel.Name, DisplayAs = tmp.AttributeModel.DisplayAs, OtherTableUniqueId = q.AttributeModelId, IsRequired = false, IsRequiredstr = "", MaxLength = "", MinVal = "" });
                });

            });
            return retobj;
        }
        public static List<ImportColumn> GetLoanMasterColumn(int companyId)
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Loan Master", Name = "Loan Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Master", Name = "Loan Description", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "50", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Master", Name = "IsInterest", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Master", Name = "Interest Percentage", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetLoanEntryColumn(int companyId)
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "50", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Entry", Name = "LoanMaster Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Entry", Name = "Loan Date", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Entry", Name = "Apply Date", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Entry", Name = "Loan Amount", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Entry", Name = "NoOf Months", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetLoanTransactionColumn(int companyId)
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "50", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Transaction", Name = "LoanMaster Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Transaction", Name = "Loan Date", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Loan Transaction", Name = "Month of Installment", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            return retobj;
        }
        public static List<ImportColumn> GetTaxDeclarationColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { TableName = "Tax Declaration", Name = "EMPLOYEE ID", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "50", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Tax Declaration", Name = "SECTION ID", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "30", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Tax Declaration", Name = "YEARLY VALUE", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Tax Declaration", Name = "MONTH", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Tax Declaration", Name = "YEAR", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            return retobj;
        }

        public static List<ImportColumn> GetEmployeeCodeChangeColumn()
        {
            List<ImportColumn> retobj = new List<ImportColumn>();
            retobj.Add(new ImportColumn() { Name = "Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "", MinVal = "" });
            retobj.Add(new ImportColumn() { TableName = "Employee Code Change", Name = "New Employee Code", IsRequired = true, IsRequiredstr = "Yes", MaxLength = "150", MinVal = "" });
            return retobj;
        }
    }
}
