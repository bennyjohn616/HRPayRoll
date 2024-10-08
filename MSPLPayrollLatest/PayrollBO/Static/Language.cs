using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class Language
    {
        #region Properties

        public Guid Id { get; set; }
        public string Name { get; set; }

        public int CompanyId { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsActive { get; set; }
        public int LangId { get; set; }
        #endregion

        private static List<Language> _languages;
        public Language()
        {
            if (object.ReferenceEquals(_languages, null))
            {
                _languages = new List<Language>();
                _languages.Add(new Language() { LangId = 1, Name = "English" });
                _languages.Add(new Language() { LangId = 2, Name = "Tamil" });
                _languages.Add(new Language() { LangId = 3, Name = "Hindi" });
                _languages.Add(new Language() { LangId = 4, Name = "French" });

            }

        }

        public static Language Get(int id)
        {
            if (object.ReferenceEquals(_languages, null))
            {
                Language tmp = new Language();
            }
            var ret = _languages.Where(u => u.LangId == id).FirstOrDefault();
            if (object.ReferenceEquals(ret, null))
                ret = new Language();
            return ret;
        }
        public List<Language> GetLanguages()
        {
            if (object.ReferenceEquals(_languages, null))
            {
                Language tmp = new Language();
            }
            return _languages;
        }

        public List<Language> LanguagesList(int companyId)
        {
            List<Language> languagesList = new List<PayrollBO.Language>();
            Language languages = new Language();
            DataTable dtValue = languages.GetTableValues(Guid.Empty, companyId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Language language = new Language();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        language.Id =new Guid (Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    language.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    language.LangId = Convert.ToInt32(dtValue.Rows[rowcount]["LangId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        language.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        language.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);

                    languagesList.Add(language);
                }
            }
            return languagesList;
        }


        public Language(Guid id, int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, companyId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
            }
        }


        #region Public methods


        /// <summary>
        /// Save the Bank
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Language_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.LangId = Convert.ToInt32(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the Bank
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Language_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Bank
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("Language_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
    }
}
