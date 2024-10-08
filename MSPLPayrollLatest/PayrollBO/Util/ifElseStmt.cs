using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PayrollBO
{
    public class ifElseStmt
    {
        public int id { get; set; }
        public int level { get; set; }
        public string leftVal { get; set; }
        public string rightVal { get; set; }
        public string oprtVal { get; set; }
        public string thenVal { get; set; }
        public string thenStment { get; set; }
        public string ifStment { get; set; }

        public bool ifExecutedVal { get; set; }
        public bool CorrectExecuteionBlock { get; set; }

        private ifElseStmt convert(string input, string thenStmt, int levl, List<ifElseStmt> ifCollection)
        {
            ifElseStmt stemt = new ifElseStmt();
            if (input.IndexOf("if") >= 0|| input.IndexOf("If") >= 0 || input.IndexOf("ElseIf") >= 0)
            {
                if (input.ToLower().IndexOf("elseif") >= 0)
                {
                    stemt.ifStment = "ElseIf";
                    input = input.ToLower().Replace("elseif", "").Trim();
                }
                if (input.IndexOf("if") >= 0|| input.IndexOf("If") >= 0)
                {
                    stemt.ifStment = "If";
                    input = input.Replace("if", "").Trim();
                    input = input.Replace("If", "").Trim();
                }
                string strOp = GetOpertor(input);
                stemt.oprtVal = strOp;
                string[] lftRgts = Regex.Split(input, strOp);
                stemt.leftVal = lftRgts[0].Trim();
                if (thenStmt == "THEN")//if it the we need do split with then value
                {
                    string[] rightVals = Regex.Split(lftRgts[1].ToUpper(), "THEN");
                    stemt.rightVal = rightVals[0];
                    stemt.thenVal = rightVals[1];

                }
                else
                {
                    stemt.rightVal = lftRgts[1].Trim();
                }
                stemt.thenStment = thenStmt;
            }
            else if (input.ToLower().IndexOf("else") >= 0)
            {
                stemt.ifStment = "Else";
                input = input.ToLower().Replace("else", "").Trim();
                stemt.thenVal = input.ToLower().Replace("then", "").Trim();
                stemt.thenStment = "THEN";
            }
            stemt.level = levl;
            if (object.ReferenceEquals(ifCollection, null) || ifCollection.Count <= 0)
            {
                stemt.id = 1;
            }
            else
            {
                int max = ifCollection.Max(p => p.id);
                stemt.id = max + 1;
            }
            return stemt;
        }

        private string GetOpertor(string input)
        {
            string strOpertr = string.Empty;
            if (input.IndexOf(">=") >= 0)
                strOpertr = ">=";
            else if (input.IndexOf("<=") >= 0)
                strOpertr = "<=";
            else if (input.IndexOf(">") >= 0)
                strOpertr = ">";
            else if (input.IndexOf("<") >= 0)
                strOpertr = "<";
            else if (input.IndexOf("!=") >= 0)
                strOpertr = "!=";
            else if (input.IndexOf("=") >= 0)
                strOpertr = "=";
            return strOpertr;
        }

        public List<ifElseStmt> GetifElse(string rawData)
        {
            List<ifElseStmt> output = new List<ifElseStmt>();
            string[] ifConti = Regex.Split(rawData, ":");
            for (int ifcnt = 0; ifcnt < ifConti.Length; ifcnt++)
            {
                string str = ifConti[ifcnt].Trim();
                if (str.IndexOf("AND") >= 0 || str.IndexOf("OR") >= 0)
                {
                    string[] inputs = Regex.Split(str, "AND");
                    string thenStemt = "AND";
                    if (inputs.Length <= 1)
                    {
                        inputs = Regex.Split(str, "OR");
                        thenStemt = "OR";
                    }
                    for (int i = 0; i < inputs.Length; i++)
                    {
                        string parrt1 = inputs[i];
                        if (parrt1.IndexOf("OR") >= 0)
                        {
                            string[] strOr = Regex.Split(parrt1, "OR");
                            if (strOr.Length > 0)
                            {
                                for (int j = 0; j < strOr.Length; j++)
                                {
                                    if (j == strOr.Length - 1)
                                    {
                                        if (strOr[j].IndexOf("THEN") >= 0)
                                            output.Add(convert(strOr[j], "THEN", ifcnt, output));
                                        else
                                            output.Add(convert(strOr[j], thenStemt, ifcnt, output));
                                    }
                                    else
                                    {
                                        output.Add(convert(strOr[j], "OR", ifcnt, output));
                                    }
                                }
                            }
                            else
                            {
                                if (i == inputs.Length - 1)
                                {
                                    if (inputs[i].IndexOf("THEN") >= 0)
                                        output.Add(convert(inputs[i], "THEN", ifcnt, output));
                                    else
                                        output.Add(convert(inputs[i], thenStemt, ifcnt, output));
                                }
                                else
                                {
                                    output.Add(convert(inputs[i], thenStemt, ifcnt, output));
                                }
                            }
                        }
                        else
                        {
                            if (i == inputs.Length - 1)
                            {
                                if (inputs[i].IndexOf("THEN") >= 0)
                                    output.Add(convert(inputs[i], "THEN", ifcnt, output));
                                else
                                    output.Add(convert(inputs[i], thenStemt, ifcnt, output));
                            }
                            else
                            {
                                output.Add(convert(inputs[i], thenStemt, ifcnt, output));
                            }
                        }

                    }
                }
                else
                {
                    output.Add(convert(str, "THEN", ifcnt, output));
                }

            }
            return output;
        }

        public List<ifElseStmt> GetCorrectExecution(List<ifElseStmt> output)
        {
            int maxlevel = output.Max(p => p.level);
            bool correctedVal = false;
            for (int lvl = 0; lvl <= maxlevel; lvl++)
            {
                if (correctedVal)
                    break;
                List<ifElseStmt> lvlObjects = output.Where(p => p.level == lvl).ToList();
                // string preOpr = "OR";
                bool preiFexeVal = true;
                for (int cnt = 0; cnt < lvlObjects.Count; cnt++)
                {
                    if (correctedVal)
                        break;
                    ifElseStmt u = lvlObjects[cnt];
                    if (u.ifStment == "ElseIf" || u.ifStment == "if"|| u.ifStment == "If")
                    {
                        bool ret = ExecuteIf(u);
                        if (u.thenStment == "AND")
                        {
                            if (preiFexeVal && ret)
                            {
                                preiFexeVal = true;
                            }
                        }
                        else if (u.thenStment == "OR")
                        {
                            if (preiFexeVal || ret)
                            {
                                preiFexeVal = true;
                            }
                        }
                        else if (u.thenStment == "THEN")
                        {
                            if (preiFexeVal && ret)
                            {
                                correctedVal = true;
                                output.Where(q => q.id == u.id).FirstOrDefault().CorrectExecuteionBlock = true;
                            }
                        }
                    }
                    else if (u.ifStment.ToLower() == "else")
                    {

                        correctedVal = true;
                        output.Where(q => q.id == u.id).FirstOrDefault().CorrectExecuteionBlock = true;

                    }
                }
            }
            return output;
        }

        private bool ExecuteIf(ifElseStmt u)
        {
            double val = 0;
            if (double.TryParse(u.rightVal, out val))
            {
                switch (u.oprtVal)
                {
                    case "<":
                        if (Convert.ToDouble(u.leftVal) < Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case ">":
                        if (Convert.ToDouble(u.leftVal) > Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "<=":
                        if (Convert.ToDouble(u.leftVal) <= Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            Console.WriteLine("less than or equal " + u.thenVal);
                            return true;
                        }
                        break;
                    case ">=":
                        if (Convert.ToDouble(u.leftVal) >= Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "!=":
                        if (Convert.ToDouble(u.leftVal) != Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "=":
                        if (Convert.ToDouble(u.leftVal) == Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    default:
                        return false;
                }
            }
            else
            {
                switch (u.oprtVal)
                {
                    case "<":
                        if (Convert.ToDouble(u.leftVal) < Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case ">":
                        if (Convert.ToDouble(u.leftVal) > Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "<=":
                        if (Convert.ToDouble(u.leftVal) <= Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            Console.WriteLine("less than or equal " + u.thenVal);
                            return true;
                        }
                        break;
                    case ">=":
                        if (Convert.ToDouble(u.leftVal) >= Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "!=":
                        if (u.leftVal.ToLower().Trim() != u.rightVal.ToLower().Trim())
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "=":
                        if (u.leftVal.ToLower().Trim() == u.rightVal.ToLower().Trim())
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    default:
                        return false;
                }
            }
            return false;

        }


        public List<ifElseStmt> MatchGetifElse(string rawData)
        {
            List<ifElseStmt> output = new List<ifElseStmt>();
            string[] ifConti = Regex.Split(rawData, ":");
            for (int ifcnt = 0; ifcnt < ifConti.Length; ifcnt++)
            {
                string str = ifConti[ifcnt].Trim();
                if (str.IndexOf(" AND ") >= 0 || str.IndexOf(" OR ") >= 0)
                {
                    string[] inputs = Regex.Split(str, " AND ");
                    string thenStemt = "AND";
                    if (inputs.Length <= 1)
                    {
                        inputs = Regex.Split(str, " OR ");
                        thenStemt = "OR";
                    }
                    for (int i = 0; i < inputs.Length; i++)
                    {
                        string parrt1 = inputs[i];
                        if (parrt1.IndexOf(" OR ") >= 0)
                        {
                            string[] strOr = Regex.Split(parrt1, " OR ");
                            if (strOr.Length > 0)
                            {
                                for (int j = 0; j < strOr.Length; j++)
                                {
                                    if (j == strOr.Length - 1)
                                    {
                                        if (strOr[j].IndexOf(" THEN ") >= 0)
                                            output.Add(convert(strOr[j], "THEN", ifcnt, output));
                                        else
                                            output.Add(convert(strOr[j], thenStemt, ifcnt, output));
                                    }
                                    else
                                    {
                                        output.Add(convert(strOr[j], "OR", ifcnt, output));
                                    }
                                }
                            }
                            else
                            {
                                if (i == inputs.Length - 1)
                                {
                                    if (inputs[i].IndexOf("THEN") >= 0)
                                        output.Add(convert(inputs[i], "THEN", ifcnt, output));
                                    else
                                        output.Add(convert(inputs[i], thenStemt, ifcnt, output));
                                }
                                else
                                {
                                    output.Add(convert(inputs[i], thenStemt, ifcnt, output));
                                }
                            }
                        }
                        else
                        {
                            if (i == inputs.Length - 1)
                            {
                                if (inputs[i].IndexOf("THEN") >= 0)
                                    output.Add(convert(inputs[i], "THEN", ifcnt, output));
                                else
                                    output.Add(convert(inputs[i], thenStemt, ifcnt, output));
                            }
                            else
                            {
                                output.Add(convert(inputs[i], thenStemt, ifcnt, output));
                            }
                        }

                    }
                }
                else
                {
                    output.Add(convert(str, "THEN", ifcnt, output));
                }

            }
            return output;
        }

        public List<ifElseStmt> MatchExecution(List<ifElseStmt> output)
        {
            int maxlevel = output.Max(p => p.level);
            bool correctedVal = false;
            for (int lvl = 0; lvl <= maxlevel; lvl++)
            {
                if (correctedVal)
                    break;
                List<ifElseStmt> lvlObjects = output.Where(p => p.level == lvl).ToList();
                // string preOpr = "OR";
                bool preiFexeVal = true;
                for (int cnt = 0; cnt < lvlObjects.Count; cnt++)
                {
                    if (correctedVal)
                        break;
                    ifElseStmt u = lvlObjects[cnt];
                    if (u.ifStment == "ElseIf" || u.ifStment == "if" || u.ifStment == "If")
                    {
                        bool ret = MatchIf(u);
                        if (u.thenStment == "AND")
                        {
                            if (preiFexeVal && ret)
                            {
                                preiFexeVal = true;
                            }
                        }
                        else if (u.thenStment == "OR")
                        {
                            if (preiFexeVal || ret)
                            {
                                preiFexeVal = true;
                            }
                        }
                        else if (u.thenStment == "THEN")
                        {
                            if (preiFexeVal && ret)
                            {
                                correctedVal = true;
                                output.Where(q => q.id == u.id).FirstOrDefault().CorrectExecuteionBlock = true;
                            }
                        }
                    }
                    else if (u.ifStment.ToLower() == "else")
                    {

                        correctedVal = true;
                        output.Where(q => q.id == u.id).FirstOrDefault().CorrectExecuteionBlock = true;

                    }
                }
            }
            return output;
        }

        private bool MatchIf(ifElseStmt u)
        {
            double val = 0;
            if (double.TryParse(u.rightVal, out val))
            {
                switch (u.oprtVal)
                {
                    case "<":
                        if (Convert.ToDouble(u.leftVal) < Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case ">":
                        if (Convert.ToDouble(u.leftVal) > Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "<=":
                        if (Convert.ToDouble(u.leftVal) <= Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            Console.WriteLine("less than or equal " + u.thenVal);
                            return true;
                        }
                        break;
                    case ">=":
                        if (Convert.ToDouble(u.leftVal) >= Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "!=":
                        if (Convert.ToDouble(u.leftVal) != Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "=":
                        if (Convert.ToDouble(u.leftVal) == Convert.ToDouble(u.rightVal))
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    default:
                        return false;
                }
            }
            else
            {
                int s = string.Compare(u.leftVal.Trim().ToLower(), u.rightVal.Trim().ToLower());
                switch (u.oprtVal)
                {
                    case "<":
                        if (s < 0)
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                            break;
                    case ">":
                        if (s > 0)
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "<=":
                        if (s == 0 || s < 0)
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case ">=":
                        if (s == 0 || s > 0)
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "!=":
                        if (s != 0)
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    case "=":
                        if (s == 0)
                        {
                            u.ifExecutedVal = true;
                            return true;
                        }
                        break;
                    default:
                        return false;
                }
            }
            return false;

        }


    }
}
