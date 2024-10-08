using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;
namespace PayrollBO
{
    public class RoleList : List<Role>
    {        
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public RoleList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        //<param name = "id" ></ param >

        //--Modified by Keerthika on 02/05/2017
        public RoleList(int Id,int companyId)
        {
            this.Id = Id;
            this.Name = Name;
            this.CompanyId = companyId;
          //  this.Id =Id;
         //   this.Id = Id;
            Role Role = new Role();
            DataTable dtValue = Role.GetTableValues(Id, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Role RoleTemp = new Role();


                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Name"])))
                        RoleTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                  //  RoleTemp.Id = Convert.ToString(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"])))
                        RoleTemp.DisplayAs = Convert.ToString(dtValue.Rows[rowcount]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Description"])))
                        RoleTemp.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"]))) // Modified
                       RoleTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"]))) //--Modified 
                        
                        RoleTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    this.Add(RoleTemp);
                }
            }
        }


    #endregion

    #region property


    /// <summary>
    /// Get or Set the CompanyId
    /// </summary>
    public string Name { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }



    #endregion

    #region Public methods

    /// <summary>
    /// Save the Category and add to the list
    /// </summary>
    /// <param name="category"></param>
    public void AddNew(Role Role)
    {
        if (Role.Save())
        {
            this.Add(Role);
        }
    }

   

    /// <summary>
    /// Delete the Category and remove from the list
    /// </summary>
    /// <param name="category"></param>
    public void DeleteExist(Role Role)
    {
        if (Role.Delete())
        {
            this.Remove(Role);
        }
    }

   


    #endregion

    #region private methods




    #endregion
}
}
