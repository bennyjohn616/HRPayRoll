using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollReports
{
    public class GenerateDataView
    {
        #region "Public Variables"
        public DataTable PaySheetDataView { get; set; }
        #endregion
        public GenerateDataView()
        {
            PaySheetDataView = new DataTable();
        }
        public GenerateDataView(List<Paysheetatrr> attr, bool isdetailrpt)
        {
            PaySheetDataView = new DataTable();


            bool MasterColumn = false;

            attr.ForEach(a =>
            {
                if (a.Type == "Detail" && !MasterColumn)
                {
                    MasterColumn = true;
                    if (isdetailrpt)
                    {

                        //PaySheetDataView.Columns.Add("Emp Code", typeof(string));

                        //PaySheetDataView.Columns.Add("Emp Name", typeof(string));

                    }
                }
                //   if (a.FieldName.ToLower() != "employeecode" && a.FieldName.ToLower() != "firstname")
                //   {
                if (!PaySheetDataView.Columns.Contains(a.DisplayAs == null ? a.FieldName : a.DisplayAs))
                {
                    PaySheetDataView.Columns.Add(a.DisplayAs == null ? a.FieldName : a.DisplayAs, typeof(string));
                }
                // }
            });//attr End;


        }

        public void AddDataRow(List<Paysheetatrr> empdata, bool isDetail,string[] grp)
        {
            DataRow dr = PaySheetDataView.NewRow();

            empdata.ForEach(d =>
            {

                if (isDetail&&object.ReferenceEquals(grp,null))
                {
                    
                    dr["EmployeeCode"] = d.EmpCode;
                  
                        dr["FirstName"] = d.EmpName;
                }

                if (!string.IsNullOrEmpty(d.DisplayAs == null ? d.FieldName : d.DisplayAs))
                {
                    if (d.DisplayAs == "EmployeeCode")
                        dr[d.DisplayAs == null ? d.FieldName : d.DisplayAs] = d.Value == null ? string.Empty : "'" + d.Value;
                    else
                    {
                        if (isDetail)
                        {
                            dr[d.DisplayAs == null ? d.FieldName : d.DisplayAs] = d.Value == null ? string.Empty : d.Value;
                        }
                        else if(!isDetail && (d.Type.ToLower() == "master" || d.Type.ToLower() == "group"))
                        {
                            dr[d.DisplayAs == null ? d.FieldName : d.DisplayAs] = d.Value == null ? string.Empty : d.Value;
                        }
                        else
                        {
                            dr[d.DisplayAs == null ? d.FieldName : d.DisplayAs] = empdata.Where(x => x.DisplayAs == d.DisplayAs).ToList().Sum(y =>Convert.ToDecimal(y.Value));
                        }
                      
                    }
                       
                }

            });//empdata End;
            PaySheetDataView.Rows.Add(dr);
        }
        public class NTuple<T> : IEquatable<NTuple<T>>
        {
            public NTuple(IEnumerable<T> values)
            {
                Values = values.ToArray();
            }

            public readonly T[] Values;

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj == null)
                    return false;
                return Equals(obj as NTuple<T>);
            }

            public bool Equals(NTuple<T> other)
            {
                if (ReferenceEquals(this, other))
                    return true;
                if (other == null)
                    return false;
                var length = Values.Length;
                if (length != other.Values.Length)
                    return false;
                for (var i = 0; i < length; ++i)
                    if (!Equals(Values[i], other.Values[i]))
                        return false;
                return true;
            }

            public override int GetHashCode()
            {
                var hc = 17;
                foreach (var value in Values)
                    hc = hc * 37 + (!ReferenceEquals(value, null) ? value.GetHashCode() : 0);
                return hc;
            }
        }
        public void GropingData(string[] group, bool isDetail, List<Paysheetatrr> attr)
        {
            DataTable dtTemp = new DataTable();



            string groupExpr = string.Empty;

            if (group != null)
            {
                for (int i = 0; i < group.Count(); i++)
                {
                    groupExpr += group[i] + ",";
                }
                groupExpr = groupExpr.TrimEnd(',');
                if (!isDetail) //Consolidation
                {
                    DataView dtView = new DataView(this.PaySheetDataView);
                    DataTable tmppay = this.PaySheetDataView.Clone();
                    DataColumn[] keys = new DataColumn[group.Count()];

                    for (int index = 0; index < group.Count(); index++)
                    {
                        DataColumn column = new DataColumn();
                        column.ColumnName = group[index];
                        keys[index] = column;
                        dtTemp.Columns.Add(column);
                    }

                    attr.ForEach(a =>
                    {

                        if (!dtTemp.Columns.Contains(a.DisplayAs == null ? a.FieldName : a.DisplayAs) && a.Type == "Detail")
                        {
                            dtTemp.Columns.Add(a.DisplayAs == null ? a.FieldName : a.DisplayAs, typeof(string));
                            tmppay.Columns[a.DisplayAs == null ? a.FieldName : a.DisplayAs].DataType = typeof(decimal);
                        }
                    });

                    dtTemp.PrimaryKey = keys;
                    tmppay = this.PaySheetDataView;
                    DataTable dtgroup = dtView.ToTable(true, group);
                    foreach (DataRow dr in dtgroup.Rows)
                    {
                        DataRow drow = dtTemp.NewRow();
                        bool goadd = true;
                        for (int index = 0; index < group.Count(); index++)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(dr[group[index]])))
                            {
                                goadd = false;
                            }
                            drow[group[index]] = dr[group[index]];
                        }
                        if (goadd)
                        {
                            dtTemp.Rows.Add(drow);
                        }
                    }
                    dtTemp.Columns.Add("No.OF.Employees");
                    attr.ForEach(a =>
                    {

                        if (tmppay.Columns.Contains(a.DisplayAs == null ? a.FieldName : a.DisplayAs) && a.Type == "Detail")
                        {
                            IEnumerable<string> columnsToGroupBy = group;
                            string columnToAggregate = a.DisplayAs == null ? a.FieldName : a.DisplayAs;


                            foreach (var g in tmppay.AsEnumerable().GroupBy(r => new NTuple<object>(from column in columnsToGroupBy select r[column])))
                            {
                                if (dtTemp.Rows.Find(g.Key.Values) != null)
                                {
                                    dtTemp.Rows.Find(g.Key.Values)[a.DisplayAs == null ? a.FieldName : a.DisplayAs] = (g.Sum(x => x.IsNull(columnToAggregate) ? 0.0m : Convert.ToDecimal(x[columnToAggregate])));
                                    dtTemp.Rows.Find(g.Key.Values)["No.OF.Employees"] = g.Count();
                                }

                            }
                        }
                    });

                    this.PaySheetDataView = dtTemp;
                }
                this.PaySheetDataView.DefaultView.Sort = groupExpr.TrimEnd(',');

                this.PaySheetDataView = this.PaySheetDataView.DefaultView.ToTable(); //dtTemp;
            }
        }
    }
}
