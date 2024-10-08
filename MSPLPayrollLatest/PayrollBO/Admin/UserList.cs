

// -----------------------------------------------------------------------
// <copyright file="UserList.cs" company="Microsoft">
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

    public class UserList : List<User>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public UserList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public UserList(int Id)
        {
            //this.CompanyId = companyId;
            User User = new User();
            DataTable dtValue = User.GetTableValues(Id,0);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    User UserTemp = new User();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        UserTemp.Id = (Convert.ToInt32(dtValue.Rows[rowcount]["Id"]));
                    UserTemp.Username = Convert.ToString(dtValue.Rows[rowcount]["Username"]);
                    UserTemp.Password = Convert.ToString(dtValue.Rows[rowcount]["Password"]);
                    UserTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    UserTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    UserTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    UserTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    UserTemp.UserRole = Convert.ToInt32(dtValue.Rows[rowcount]["UserRole"]);//--
                    UserTemp.ProfileImage = Convert.ToString(dtValue.Rows[rowcount]["ProfileImage"]);
                    //UserTemp.RoleName = Convert.ToString(dtValue.Rows[rowcount]["RoleName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        UserTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                    //    UserTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        UserTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        UserTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        UserTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        UserTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        UserTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        UserTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsForget"])))
                        UserTemp.IsForget = Convert.ToBoolean(dtValue.Rows[rowcount]["IsForget"]);
                    this.Add(UserTemp);
                }
            }
        }

        public UserList(Guid empId)
        {
            //this.CompanyId = companyId;
            User User = new User();
           
            DataTable dtValue = User.GetTableValues(empId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    User UserTemp = new User();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        UserTemp.Id = (Convert.ToInt32(dtValue.Rows[rowcount]["Id"]));
                    UserTemp.Username = Convert.ToString(dtValue.Rows[rowcount]["Username"]);
                    UserTemp.Password = Convert.ToString(dtValue.Rows[rowcount]["Password"]);
                    UserTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    UserTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    UserTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    UserTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    UserTemp.UserRole = Convert.ToInt32(dtValue.Rows[rowcount]["UserRole"]);//--
                    UserTemp.ProfileImage = Convert.ToString(dtValue.Rows[rowcount]["ProfileImage"]);
                    //UserTemp.RoleName = Convert.ToString(dtValue.Rows[rowcount]["RoleName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        UserTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                    //    UserTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        UserTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        UserTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        UserTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        UserTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        UserTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        UserTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsForget"])))
                        UserTemp.IsForget = Convert.ToBoolean(dtValue.Rows[rowcount]["IsForget"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DBString"])))
                        UserTemp.DBString = Convert.ToString(dtValue.Rows[rowcount]["DBString"]);
                    UserTemp.DBConnectionId = Convert.ToInt32(dtValue.Rows[rowcount]["DBConnectionId"]);

                    this.Add(UserTemp);
                }
            }
        }

        public UserList(int roleid,int temp)
        {
            //this.CompanyId = companyId;
            User User = new User();

            DataTable dtValue = User.GetUsertblValues(roleid);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    User UserTemp = new User();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        UserTemp.Id = (Convert.ToInt32(dtValue.Rows[rowcount]["Id"]));
                    UserTemp.Username = Convert.ToString(dtValue.Rows[rowcount]["Username"]);
                    UserTemp.Password = Convert.ToString(dtValue.Rows[rowcount]["Password"]);
                    UserTemp.FirstName = Convert.ToString(dtValue.Rows[rowcount]["FirstName"]);
                    UserTemp.LastName = Convert.ToString(dtValue.Rows[rowcount]["LastName"]);
                    UserTemp.Email = Convert.ToString(dtValue.Rows[rowcount]["Email"]);
                    UserTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    UserTemp.UserRole = Convert.ToInt32(dtValue.Rows[rowcount]["UserRole"]);//--
                    UserTemp.ProfileImage = Convert.ToString(dtValue.Rows[rowcount]["ProfileImage"]);
                    //UserTemp.RoleName = Convert.ToString(dtValue.Rows[rowcount]["RoleName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        UserTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                    //    UserTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        UserTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        UserTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        UserTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        UserTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        UserTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        UserTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsForget"])))
                        UserTemp.IsForget = Convert.ToBoolean(dtValue.Rows[rowcount]["IsForget"]);
                    this.Add(UserTemp);
                }
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }



        #endregion

        #region Public methods

        /// <summary>
        /// Save the Category and add to the list
        /// </summary>
        /// <param name="category"></param>
        public void AddNew(User User)
        {
            if (User.Save())
            {
                this.Add(User);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="category"></param>
        public void DeleteExist(User User)
        {
            if (User.Delete())
            {
                this.Remove(User);
            }
        }


        #endregion

        #region private methods




        #endregion
    }
}
