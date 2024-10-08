// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Microsoft">
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
    using SQLDBperation;

    /// <summary>
    /// To handle the User
    /// </summary>
    public class User
    {

        #region private variable

        private UserCompanymappingList _userId;
        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public User()
        {

        }

        //public void ForEach(Func<object, object> p)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public User(int Id)
        {
            this.Id = Id;
            DataTable dtValue = this.GetTableValues(this.Id, 0);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                this.Username = Convert.ToString(dtValue.Rows[0]["Username"]);
                this.Password = Convert.ToString(dtValue.Rows[0]["Password"]);
                this.FirstName = Convert.ToString(dtValue.Rows[0]["FirstName"]);
                this.LastName = Convert.ToString(dtValue.Rows[0]["LastName"]);
                this.Email = Convert.ToString(dtValue.Rows[0]["Email"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                this.ProfileImage = Convert.ToString(dtValue.Rows[0]["ProfileImage"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["UserRole"])))
                    this.UserRole = Convert.ToInt32(dtValue.Rows[0]["UserRole"]);//--

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsForget"])))
                    this.IsForget = Convert.ToBoolean(dtValue.Rows[0]["IsForget"]);
            }
        }
        public User(Guid Id)
        {
            this.EmployeeId = Id;
            DataTable dtValue = this.GetTableValues(this.EmployeeId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                this.Username = Convert.ToString(dtValue.Rows[0]["Username"]);
                this.Password = Convert.ToString(dtValue.Rows[0]["Password"]);
                this.FirstName = Convert.ToString(dtValue.Rows[0]["FirstName"]);
                this.LastName = Convert.ToString(dtValue.Rows[0]["LastName"]);
                this.Email = Convert.ToString(dtValue.Rows[0]["Email"]);
                this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                this.ProfileImage = Convert.ToString(dtValue.Rows[0]["ProfileImage"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["UserRole"])))
                    this.UserRole = Convert.ToInt32(dtValue.Rows[0]["UserRole"]);//--

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsForget"])))
                    this.IsForget = Convert.ToBoolean(dtValue.Rows[0]["IsForget"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Get or Set the Password
        /// </summary>
        public string Password { get; set; }

        public Guid EmployeeId { get; set; }
        /// <summary>
        /// Get or Set the FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get or Set the LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get or Set the Email
        /// </summary>
        public string Email
        {
            get; set;
        }

        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the Phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Get or Set the ProfileImage
        /// </summary>
        public string ProfileImage { get; set; }
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// Get or Set the UserRole
        /// </summary>
        public int UserRole { get; set; }
        public string RoleName { get; set; } //--

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public bool IsForget { get; set; }

        public string DBString { get; set; }

        public int DBConnectionId { get; set; }
        public string compCode { get; set; }
        public UserCompanymappingList userCompanyMappingset
        {
            get; set;
        }
        public UserCompanymappingList userCompanyMapping
        {
            get
            {
                if (object.ReferenceEquals(_userId, null))
                {
                    if (this.Id != 0)
                    {
                        _userId = new UserCompanymappingList(this.Id);
                    }
                    else
                        _userId = new UserCompanymappingList();
                }
                return _userId;

            }
            set
            {
                _userId = value;
            }
        }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the User
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("User_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Username", this.Username);
            sqlCommand.Parameters.AddWithValue("@Password", this.Password);
            sqlCommand.Parameters.AddWithValue("@FirstName", this.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", this.LastName);
            sqlCommand.Parameters.AddWithValue("@Email", this.Email);
            sqlCommand.Parameters.AddWithValue("@Phone", this.Phone);
            sqlCommand.Parameters.AddWithValue("@ProfileImage", this.ProfileImage);
            sqlCommand.Parameters.AddWithValue("@UserRole", this.UserRole);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@IsForget", this.IsForget);
            sqlCommand.Parameters.AddWithValue("@DBConnectionId", this.DBConnectionId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            LoginDBOperation dbOperation = new LoginDBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = Convert.ToInt32(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the User
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("User_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        public string ValidateLogin(string username, string password)
        {

            DataTable dtValue = this.GetTableValues(username, password);


            if (dtValue.Rows.Count > 0)
            {
                string CHKUsername = Convert.ToString(dtValue.Rows[0]["Username"]);
                string CHKPassword = Convert.ToString(dtValue.Rows[0]["Password"]);

                if (CHKUsername.Trim().ToUpper() == username.Trim().ToUpper() && CHKPassword.Trim() == password.Trim())
                {

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                        this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                    this.Username = Convert.ToString(dtValue.Rows[0]["Username"]);
                    this.Password = Convert.ToString(dtValue.Rows[0]["Password"]);
                    this.FirstName = Convert.ToString(dtValue.Rows[0]["FirstName"]);
                    this.LastName = Convert.ToString(dtValue.Rows[0]["LastName"]);
                    this.Email = Convert.ToString(dtValue.Rows[0]["Email"]);
                    this.Phone = Convert.ToString(dtValue.Rows[0]["Phone"]);
                    this.ProfileImage = Convert.ToString(dtValue.Rows[0]["ProfileImage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                        this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["UserRole"])))
                        this.UserRole = Convert.ToInt32(dtValue.Rows[0]["UserRole"]);//--
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                        this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                        this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                        this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                        this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                        this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                        this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsForget"])))
                        this.IsForget = Convert.ToBoolean(dtValue.Rows[0]["IsForget"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DBString"])))
                        this.DBString = Convert.ToString(dtValue.Rows[0]["DBString"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DBConnectionId"])))
                        this.DBConnectionId = Convert.ToInt32(dtValue.Rows[0]["DBConnectionId"]);
                    this.CompanyId= dtValue.Rows[0]["CompanyId"]==DBNull.Value ?0:Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["compCode"])))
                        this.compCode = Convert.ToString(dtValue.Rows[0]["compCode"]);
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            else
            {
                return "Failed";
            }



        }

        public bool CheckUserExist(string userName)
        {
            return CheckUserName(userName);
        }
        #endregion

        #region private methods


        /// <summary>
        /// Select the User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int id, int temp)
        {

            SqlCommand sqlCommand = new SqlCommand("User_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetUsertblValues(int roleid)
        {

            SqlCommand sqlCommand = new SqlCommand("RoleDelete_Check");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@RoleId", roleid);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable GetTableValues(Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("User_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public DataTable GetUserDBconnectionValues(int id)
        {

            SqlCommand sqlCommand = new SqlCommand("USER_DBCONNECTION");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@DBConnectionId", id);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public DataTable GetProductDetails(string username)
        {
            SqlCommand sqlCommand = new SqlCommand("ProductDetailsSelect");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Username", username);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        /// <summary>
        /// Select the User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(string username, string password)
        {

            SqlCommand sqlCommand = new SqlCommand("User_Login");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@username", username);
            sqlCommand.Parameters.AddWithValue("@password", password);
            LoginDBOperation dbOperation = new LoginDBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected bool CheckUserName(string userName)
        {
            bool IsExist = false;
            SqlCommand sqlCommand = new SqlCommand("select Username from [User] where Username='" + userName + "'");
            // sqlCommand.Parameters.AddWithValue("@UserNameorEmail", userName);
            sqlCommand.CommandType = CommandType.Text;
            LoginDBOperation dbOperation = new LoginDBOperation();
            DataTable dt = dbOperation.GetTableData(sqlCommand);
            IsExist = dt.Rows.Count > 0 ? true : false;
            return IsExist;
        }
        #endregion

    }
}

