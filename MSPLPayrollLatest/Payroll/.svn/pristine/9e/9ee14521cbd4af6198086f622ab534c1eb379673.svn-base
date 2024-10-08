
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Payroll.Models.Common
{
    public class BirthdayGreeting
    {

        public string Birthdaygreetings(string baseurl,string contents, string Name)
        {

            string Messagecontent = string.Empty;
            baseurl = "src=" + "\""+baseurl;
           
            Messagecontent = "<html><head><title> happy_birthday </title><meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\"></head>";
            Messagecontent += "<body bgcolor = \"#FFFFFF\" leftmargin = \"0\" topmargin = \"0\" marginwidth = \"0\" marginheight = \"0\">";
            Messagecontent += "<table id = \"Table_01\" width = \"600\" border = \"0\" cellpadding = \"0\" cellspacing = \"0\"><tr><td bgcolor = \"#FFFFFF\">";
            Messagecontent += "<img style = \"display:block\" border = \"0\" src=images/img_01.png\" width = \"125\" height = \"160\" alt = \"\"></td><td bgcolor = \"#FFFFFF\">";
            Messagecontent += " <img style =\"display:block\" border = \"0\" src=images/img_02.png\" width = \"400\" height = \"160\" alt = \"\"></td><td bgcolor = \"#FFFFFF\">";
            Messagecontent += "<img style = \"display:block\" border = \"0\" src=images/img_03.png\" width = \"75\" height = \"160\" alt = \"\"></td></tr><tr><td bgcolor = \"#FFFFFF\">&nbsp;</td>";
            Messagecontent += "<td style = \"border -left-style: solid; border-left-color: #898989; border-left-width: 1px; border-right-style: solid; border-right-color: #898989; border-right-width: 1px\" bgcolor = \"#F4F4F4\">";
            Messagecontent += "<table border = \"0\" width = \"100%\" cellspacing = \"10\" cellpadding = \"0\"><tr><td colspan = \"3\" align = \"center\"><b><i>";
            Messagecontent += "<font size = \"5\" color = \"#800080\">";
            Messagecontent += " Happy Birthday " + Name;
            Messagecontent +="</font></i></b></td></tr><tr><td width = \"80\"> &nbsp;</td><td><div>";
            Messagecontent += contents;
            Messagecontent += "</div></td><td width = \"20\"> &nbsp;</td></tr></table>";
            Messagecontent += "</td><td bgcolor = \"#FFFFFF\">&nbsp;</td></tr><tr><td bgcolor = \"#FFFFFF\"><img style = \"display:block\" border = \"0\" src=images/img_07.png\" width = \"125\" height = \"206\" alt = \"\"></td>";                 
            Messagecontent += "<td bgcolor = \"#FFFFFF\"><img style = \"display:block\" border = \"0\" src=images/img_08.png\" width = \"400\" height = \"206\" alt =\"\"></td><td bgcolor = \"#FFFFFF\"> ";                               
            Messagecontent += "<img style = \"display:block\" border = \"0\" src=images/img_09.png\" width = \"75\" height = \"206\" alt = \"\"></td></tr><tr><td bgcolor = \"#FFFFFF\" colspan = \"3\">";
            Messagecontent += "</td></tr></table></body></html>";
            Messagecontent = Messagecontent.Replace("src=", baseurl);
            return Messagecontent;
        }


    }
}