using Microsoft.AspNetCore.Mvc;
using State_ApiSideOnly.Model;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.ComponentModel;

namespace State_ApiSideOnly.DAL
{
    public class StateClass_DAL : Controller
    {
        private readonly IConfiguration _configuration;

        public StateClass_DAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("api/[controller]/GetAllStates")] // Example route, adjust to fit your needs
        [HttpGet]
        public List<State> GetAllStates()
        {
            List<State> states = new List<State>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from dbo.StateDistricts", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                State stateModel = new State();
                stateModel.StateID = Convert.ToInt32(dt.Rows[i]["StateID"]);
                stateModel.StateName = dt.Rows[i]["StateName"].ToString();
                stateModel.DistrictId = Convert.ToInt32(dt.Rows[i]["DistrictID"]);
                stateModel.DistrictName = dt.Rows[i]["DistrictName"].ToString();
                states.Add(stateModel);
            }
            return states;
        }

        [Route("api/[controller]/Login")]
        [HttpGet]
        public async Task<Login> UserLogin(string username, string password)
        {
            Login login = null; // Initialize login as null to indicate no user found by default
            DataTable dt = new DataTable();

            string query = "SELECT username, password FROM dbo.UserLogins WHERE username = @username AND password = @password";

            // Ensure the connection is disposed of correctly
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                // Ensure the command is disposed of correctly
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);  // Use @ in the parameter name
                    cmd.Parameters.AddWithValue("@password", password);  // Use @ in the parameter name

                    try
                    {
                        await conn.OpenAsync(); // Open the connection asynchronously

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) // Use SqlDataReader to read data
                        {
                            if (reader.HasRows)
                            {
                                login = new Login();
                                while (await reader.ReadAsync()) // Read data asynchronously
                                {
                                    // Assuming Login object has properties Username and Password
                                    login.UserName = reader["username"].ToString();
                                    login.Password = reader["password"].ToString();
                                }
                                Console.WriteLine("Sucess");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception (consider using a logging framework)
                        Console.WriteLine(ex.Message);
                        // Optionally, handle exceptions and return appropriate error response
                    }
                }
            }

            return login;
        }




        //WOrking Method
        //[Route("GetDistrictByStates")]
        [Route("api/[controller]/GetDistrictByStates")]
        [HttpGet]
        public async Task<IEnumerable<State>> GetDistrictByStates(string name)
        {
            // Split the incoming string value by a comma, removing any empty entries
            string[] values = name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 0)
            {
                return Enumerable.Empty<State>();
            }

            List<State> states = new List<State>();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                // Build the query dynamically
                var queryBuilder = new StringBuilder("SELECT StateName, DistrictName FROM dbo.StateDistricts WHERE StateName IN (");

                // Create parameter placeholders
                for (int i = 0; i < values.Length; i++)
                {
                    queryBuilder.Append($"@StateName{i}");
                    if (i < values.Length - 1)
                    {
                        queryBuilder.Append(", ");
                    }
                }
                queryBuilder.Append(")");

                string query = queryBuilder.ToString();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the command
                    for (int i = 0; i < values.Length; i++)
                    {
                        cmd.Parameters.AddWithValue($"@StateName{i}", values[i].Trim());
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            // Processing the results and adding them to the list
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                State stateModel = new State
                {
                    StateName = dt.Rows[i]["StateName"].ToString(),
                    DistrictName = dt.Rows[i]["DistrictName"].ToString()
                };
                states.Add(stateModel);
            }

            return states;
        }
    }
}
