using Payroll.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PayrollBO.RoleSetting
{
    public class ApplyRoleSetting
    {

        public void CheckSave(string tableName)
        {

        }

        public void CheckDelete(string tableName)
        {

        }
        public void ApplyApprove(string tableName,object inputObj)
        {
            if (HttpContext.Current.Session["RoleSetting"] == null)
            {
                FormCommandList formCommands = new FormCommandList();
                formCommands = (FormCommandList)HttpContext.Current.Session["FormCommand"];
                var cmds = formCommands.Where(u => u.TableName == tableName).ToList();
                List<jsonRoleFormCommand> roleFormCommand = new List<jsonRoleFormCommand>();
                roleFormCommand = (List<jsonRoleFormCommand>)HttpContext.Current.Session["RoleSetting"];
                var tmp = roleFormCommand.Where(u => formCommands.Any(p1 => p1.Id == u.formCommandId)).ToList();

                    //    var result = formCommandlist.Where(p => !roleFormCommand.Any(p2 => p2.FormCommandId == p.Id)).ToList();
            }

        }
    }
}
