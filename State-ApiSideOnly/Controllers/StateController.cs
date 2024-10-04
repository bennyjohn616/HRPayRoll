using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using State_ApiSideOnly.DAL;
using State_ApiSideOnly.Model;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace State_ApiSideOnly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly StateClass_DAL _stateClass_DAL;
        private readonly IConfiguration _configuration;


        public StateController(StateClass_DAL stateClass_DAL, IConfiguration configuration)
        {
            _stateClass_DAL = stateClass_DAL;
            _configuration = configuration;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            List<State> states = new List<State>();
            try
            {
                states = _stateClass_DAL.GetAllStates();
            }
            catch (Exception ex)
            {
              //  TempData["Error Message"] = "No results to show";
            }
            return Ok(states);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            // For demonstration, we'll accept any user with the username "test" and password "password"

            if (userLogin.Username == "user1" && userLogin.Password == "password123")
            {
                var token = GenerateJwtToken();
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "https://yourdomain.com",
                audience: "https://yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class UserLogin
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        //[HttpGet]
        //public async Task<IEnumerable<State>> GetDistrictByStates(string name)
        //{

        //    string[] myValues = new string[] { name };
        //    string csvString = string.Join(",", myValues);

        //    if(name == "TamilNadu")
        //    {
        //        DataTable dt = new DataTable();
        //        SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //        SqlCommand cmd = new SqlCommand("select statename,DistrictName from dbo.StateDistricts where statename IN ('Tamil Nadu','Kerala')", conn);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.Fill(dt);

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            State stateModel = new State();
        //            stateModel.StateID = Convert.ToInt32(dt.Rows[i]["StateID"]);
        //            stateModel.StateName = dt.Rows[i]["StateName"].ToString();
        //            stateModel.DistrictId = Convert.ToInt32(dt.Rows[i]["DistrictID"]);
        //            stateModel.DistrictName = dt.Rows[i]["DistrictName"].ToString();
        //           // name.Add(stateModel);
        //        }
        //    }

        //    return Ok(name);
        //}

        //[Route("GetDistrictByStates")]
        //[HttpGet]
        //public async Task<IEnumerable<State>> GetDistrictByStates(string name)
        //{

        //    // Split the incoming string value by a comma, removing any empty entries
        //    string[] values = name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


        //    List<State> states = new List<State>();

        //    bool containsTamilNadu = values.Any(v => v.Trim().Equals("Tamil Nadu", StringComparison.OrdinalIgnoreCase));
        //    bool containsKerala = values.Any(v => v.Trim().Equals("Kerala", StringComparison.OrdinalIgnoreCase));

        //    if (containsTamilNadu && containsKerala)
        //    {
        //        DataTable dt = new DataTable();

        //        using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            var queryBuilder = new StringBuilder("SELECT StateName, DistrictName FROM dbo.StateDistricts WHERE StateName IN (");
        //            var parameters = new List<string>();

        //            // string query = "SELECT StateName, DistrictName FROM dbo.StateDistricts WHERE StateName IN @StateName";
        //            using (SqlCommand cmd = new SqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@StateName", name);

        //                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        //                {
        //                    adapter.Fill(dt);
        //                }
        //            }
        //        }

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            State stateModel = new State
        //            {
        //              //  StateID = Convert.ToInt32(dt.Rows[i]["StateID"]),
        //                StateName = dt.Rows[i]["StateName"].ToString(),
        //             //   DistrictId = Convert.ToInt32(dt.Rows[i]["DistrictID"]),
        //                DistrictName = dt.Rows[i]["DistrictName"].ToString()
        //            };
        //            states.Add(stateModel);
        //        }
        //    }

        //    return states;
        //}

        //WOrking Method
        //[Route("GetDistrictByStates")]
        //[HttpGet]
        //public async Task<IEnumerable<State>> GetDistrictByStates(string name)
        //{
        //    // Split the incoming string value by a comma, removing any empty entries
        //    string[] values = name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //    if (values.Length == 0)
        //    {
        //        return Enumerable.Empty<State>();
        //    }

        //    List<State> states = new List<State>();
        //    DataTable dt = new DataTable();

        //    using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        // Build the query dynamically
        //        var queryBuilder = new StringBuilder("SELECT StateName, DistrictName FROM dbo.StateDistricts WHERE StateName IN (");

        //        // Create parameter placeholders
        //        for (int i = 0; i < values.Length; i++)
        //        {
        //            queryBuilder.Append($"@StateName{i}");
        //            if (i < values.Length - 1)
        //            {
        //                queryBuilder.Append(", ");
        //            }
        //        }
        //        queryBuilder.Append(")");

        //        string query = queryBuilder.ToString();

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            // Add parameters to the command
        //            for (int i = 0; i < values.Length; i++)
        //            {
        //                cmd.Parameters.AddWithValue($"@StateName{i}", values[i].Trim());
        //            }

        //            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        //            {
        //                adapter.Fill(dt);
        //            }
        //        }
        //    }

        //    // Processing the results and adding them to the list
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        State stateModel = new State
        //        {
        //            StateName = dt.Rows[i]["StateName"].ToString(),
        //            DistrictName = dt.Rows[i]["DistrictName"].ToString()
        //        };
        //        states.Add(stateModel);
        //    }

        //    return states;
        //}

    }
