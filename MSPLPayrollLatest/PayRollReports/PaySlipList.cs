using PayrollBO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollReports
{
    public class PaySlipList : List<Payattr>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PaySlipList()
        { }
        public PaySlipList(Guid Id)
        {
            PaySlipAttributeList attr = new PaySlipAttributeList(Id);
            PaySlipAttributeList hederSection = new PaySlipAttributeList();
            attr.ForEach(a =>
            {
                if (a.HeaderDisplayOrder > 0)
                {
                    Payattr newattr = new Payattr();
                    newattr.ConfigurationId = Id;
                    newattr.TableName = a.TableName;
                    newattr.FieldName = a.FieldName;
                    newattr.DisplayAs = a.DisplayAs;
                    newattr.Section = "Header";

                    newattr.DisplayOrder =(int) a.HeaderDisplayOrder;
                    this.Add(newattr);
                }
                if (a.FooterDisplayOrder > 0)
                {
                    Payattr newattr = new Payattr();
                    newattr.ConfigurationId = Id;
                    newattr.TableName = a.TableName;
                    newattr.FieldName = a.FieldName;
                    newattr.DisplayAs = a.DisplayAs;
                    newattr.Section = "Footer";

                    newattr.DisplayOrder = (int)a.FooterDisplayOrder;
                    this.Add(newattr);
                }
                if (a.FandFHeaderDisplayOrder>0)
                {
                    Payattr newattr = new Payattr();
                    newattr.ConfigurationId = Id;
                    newattr.TableName = a.TableName;
                    newattr.FieldName = a.FieldName;
                    newattr.DisplayAs = a.DisplayAs;
                    newattr.Section = "FandFHeader";

                    newattr.DisplayOrder = (int)a.FandFHeaderDisplayOrder;
                    this.Add(newattr);
                }
                if (a.EarningDisplayOrder > 0)
                {
                    Payattr newattr = new Payattr();
                    newattr.ConfigurationId = Id;
                    newattr.TableName = a.TableName;
                    newattr.FieldName = a.FieldName;
                    newattr.DisplayAs = a.DisplayAs;
                    newattr.Section = "Earnings";
                    newattr.MatchingId = a.MatchingId;
                    newattr.DisplayOrder = (int)a.EarningDisplayOrder;
                    this.Add(newattr);
                }
                if (a.DeductionDisplayOrder > 0)
                {
                    Payattr newattr = new Payattr();
                    newattr.ConfigurationId = Id;
                    newattr.TableName = a.TableName;
                    newattr.FieldName = a.FieldName;
                    newattr.DisplayAs = a.DisplayAs;
                    newattr.Section = "Deductions";

                    newattr.DisplayOrder = (int) a.DeductionDisplayOrder;
                    this.Add(newattr);
                }


            });
        }

       

     
        #endregion
        #region Public methods

     

    

        #endregion

        #region private methods


        #endregion
    }
}
