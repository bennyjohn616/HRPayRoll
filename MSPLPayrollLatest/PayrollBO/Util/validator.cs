using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using TraceError;

namespace PayrollBO
{
    public class validator
    {
        public static DateTime ValidateDate(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            DateTime dDate = new DateTime();
            // input= DateTime.Now.ToString("dd/MM/yyyy").Replace('.', '/');
            if (!string.IsNullOrEmpty(input))
            {
                DateTime minValue = DateTime.MinValue;
                DateTime maxValue = DateTime.MaxValue;
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (!DateTime.TryParse(input, new CultureInfo("en-GB"), DateTimeStyles.None, out dDate))
                {

                    error.Add("Please provide valid date(dd/MM/yyyy) format at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                    //Please to give valid date at row 1 and column 1 in the  sheet of 1.
                }

                if (minValue > dDate || maxValue < dDate)
                {

                    error.Add("Please provide valid date at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
                // if (DateTime.TryParse(input, provider,DateTimeStyles.None,out dDate)==false)
                //{
                //     error.Add("Please provide valid date at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                //     //Please to give valid date at row 1 and column 1 in the  sheet of 1.
                // }
            }
            return dDate;
        }
        //created By Ajithpanner on 8/12/17
        public static string ValidateString(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {

            if (!string.IsNullOrEmpty(input))
            {
                if (Regex.IsMatch(input, @"\d"))
                {
                    error.Add("Please provide valid string at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
            }
            return input;
        }

        public static int ValidateNumber(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(input))
            {
                if (!Int32.TryParse(input, out num))
                {
                    error.Add("Please provide valid number at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
            }
            return num;
        }

        public static long ValidateEsiNumber(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            long varOut = 0;
            if (!string.IsNullOrEmpty(input))
            {
                if (!long.TryParse(input, out varOut))
                {
                    error.Add("Please provide valid number at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
            }
            return varOut;
        }

        public static string ValidatePhoneNumber(string input, ref List<string> error, string sheet, int rowNo, string colmn, string len)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (!Regex.Match(input, @"\d{" + len + "}").Success)
                {
                    error.Add("Please provide valid " + colmn + " at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
            }
            return input;
        }
        public static string ValidateExtentionNumber(string input, ref List<string> error, string sheet, int rowNo, string colmn, string minlen, string maxlen)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Length < Convert.ToInt32(minlen) && input.Length > Convert.ToInt32(minlen))
                {
                    error.Add("Please provide valid " + colmn + " at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
            }
            return input;
        }
        public static string Validate12dgtno(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (!Regex.Match(input, @"\d{12}").Success)
                {
                    error.Add("Please provide valid number at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
            }
            return input;
        }

        public static decimal ValidateMoney(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            decimal num = 0;
            if (!string.IsNullOrEmpty(input))
            {
                if (!decimal.TryParse(input, out num))
                {
                    error.Add("Please provide valid money at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
            }
            return num;

        }
        public static double ValidateLop(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            double num = 0;
            if (!string.IsNullOrEmpty(input))
            {
                if (!double.TryParse(input, out num))
                {
                    error.Add("Please provide valid number at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
            }
            return num;

        }
        public static string ValidateEmail(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (!string.IsNullOrEmpty(input))
            {
                if (!re.IsMatch(input))
                    error.Add("Please provide valid email at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
            }
            return input;
        }
        public static string ValidatePAN(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            //string strRegex = "/^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/";
            string strRegex = "^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$";
            Regex re = new Regex(strRegex);
            if (!string.IsNullOrEmpty(input))
            {
                if (!re.IsMatch(input))
                    error.Add("Please provide valid PAN Number at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
            }
            return input;
        }
        public static string ValidateAlphaNum(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            //string strRegex = "/^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/";
            string strRegex = "^[a-zA-Z0-9]*$";
            Regex re = new Regex(strRegex);
            if (!string.IsNullOrEmpty(input))
            {
                if (!re.IsMatch(input))
                    error.Add("Please provide valid Alpha numeric at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
            }
            return input;
        }

        public static string ValidateAlphaOnly(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            //string strRegex = "/^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/";
            string strRegex = "^[a-zA-Z]*$";
            Regex re = new Regex(strRegex);
            if (!string.IsNullOrEmpty(input))
            {
                if (!re.IsMatch(input))
                    error.Add("Please provide valid Alphabetic only at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
            }
            return input;
        }

        public static string ValidateWeb(string input, ref List<string> error)
        {
            return input;
        }

        public static void ValidateRequired(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            if (string.IsNullOrEmpty(input))
            {
                rowNo = rowNo + 1;
                error.Add("Please provide valid data at row " + (rowNo) + " and column " + colmn + " in the sheet of " + sheet);
            }
        }

        public static void ValidateCompared(string input1, string input2, ref List<string> error, string sheet, int rowNo, string colmn)
        {

        }

        public static void ValidateMaxLength(string input, string maxVal, ref List<string> error, string sheet, int rowNo, string colmn)
        {

            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(maxVal))
            {
                int max = 1000;
                if (Int32.TryParse(maxVal, out max))
                {
                    if (input.Length > max)
                    {
                        error.Add("Please provide valid length of data at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                    }
                }
            }
        }

        public static void ValidateMaxValue(string input, string maxVal, ref List<string> error, string sheet, int rowNo, string colmn)
        {

            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(maxVal))
            {
                int max = 0;
                int inp = 0;
                if (Int32.TryParse(maxVal, out max) && Int32.TryParse(input, out inp))
                {
                    if (inp > max)
                    {
                        error.Add("Please provide valid data(<" + max + ") at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                    }
                }
            }
        }


        public static void ValidateMinValue(string input, string minVal, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(minVal))
            {
                int min = 0;
                int inp = 0;
                if (Int32.TryParse(minVal, out min) && Int32.TryParse(input, out inp))
                {
                    if (inp < min)
                    {
                        error.Add("Please provide valid data(>" + min + ") at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                    }
                }
            }
        }

        public static int GetStatus(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            input = input.ToUpper();
            if (input.Contains("ACTIVE") || input.StartsWith("A"))
                return 1;
            else if (input.Contains("INACTIVE") || input.StartsWith("I"))
                return 2;
            else
            {
                error.Add("Please provide valid Flag(Active/InActive) at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                //error.Add("There is no specified Status");
                return 0;
            }


        }

        public static int GetGender(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            input = input.Trim().ToUpper();
            if (input.Contains("FEMALE") || input.StartsWith("F") || input.Contains("Female"))
                return 2;
            else if (input.Contains("MALE") || input.StartsWith("M") || input.Contains("Male"))
                return 1;
            else
            {
                error.Add("Please provide valid Gender(Male/Female) at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                //error.Add("There is no specified Gender");
                return 0;
            }


        }
        public static int GetAddressType(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            input = input.ToUpper();
            if (input.Contains("PERMANENT") || input.StartsWith("P") || input.Contains("PERMANENT ADDRESS"))
                return 1;
            else if (input.Contains("COMMUNICATION") || input.StartsWith("C") || input.Contains("COMMUNICATION ADDRESS"))
                return 2;
            else
            {
                error.Add("Please provide valid Addres Type(Permanent/Communication) at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                return 0;
            }


        }
        public static int GetMonth(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            input = input.ToUpper();
            if (input.Contains("JANUARY") || input.StartsWith("JAN") || input.Contains("1"))
                return 1;
            else if (input.Contains("February") || input.StartsWith("FEB") || input.Contains("2"))
                return 2;
            else if (input.Contains("MARCH") || input.StartsWith("MAR") || input.Contains("3"))
                return 3;
            else if (input.Contains("APRIL") || input.StartsWith("APR") || input.Contains("4"))
                return 4;
            else if (input.Contains("MAY") || input.StartsWith("MAY") || input.Contains("5"))
                return 5;
            else if (input.Contains("JUNE") || input.StartsWith("JUN") || input.Contains("6"))
                return 6;
            else if (input.Contains("JULY") || input.StartsWith("JUL") || input.Contains("7"))
                return 7;
            else if (input.Contains("AUGUST") || input.StartsWith("AUG") || input.Contains("8"))
                return 8;
            else if (input.Contains("SEPTEMBER") || input.StartsWith("SEP") || input.Contains("9"))
                return 9;
            else if (input.Contains("OCTOBER") || input.StartsWith("OCT") || input.Contains("10"))
                return 10;
            else if (input.Contains("NOVEMBER") || input.StartsWith("NOV") || input.Contains("11"))
                return 11;
            else if (input.Contains("DECEMBER") || input.StartsWith("DEC") || input.Contains("12"))
                return 12;

            else
            {
                error.Add("Please provide valid Month(Januay to December) at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                return 0;
            }


        }

        public static int ValidateYear(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(input))
            {
                if (!Int32.TryParse(input, out num))
                {
                    error.Add("Please provide valid Year at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                }
                else
                {
                    if (num > (DateTime.Now.Year + 1) || num < (DateTime.Now.Year) - 1)
                    {
                        error.Add("Please provide valid Year at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                    }
                }
            }
            return num;
        }


        public static bool GetTruORFalse(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            input = input.ToUpper();
            if (input.Contains("TRUE") || input.StartsWith("T") || input.StartsWith("M") || input.StartsWith("Y") || input.Contains("YES"))
                return true;
            else if (input.Contains("FALSE") || input.StartsWith("F") || input.StartsWith("N") || input.Contains("NO") || string.IsNullOrEmpty(input))
                return false;
            else
            {
                error.Add("Please provide valid Flag(Yes/No) at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                //error.Add("There is no specified Value");
                return false;
            }


        }

        public static int GetRelationShip(string input, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            input = input.ToUpper();
            if (input.Contains("FATHER") || input.StartsWith("F"))
                return 1;
            if (input.Contains("MOTHER") || input.StartsWith("M"))
                return 2;
            if (input.Contains("SON") || input.StartsWith("SON"))
                return 3;
            if (input.Contains("DAUGHTER") || input.StartsWith("D"))
                return 4;
            if (input.Contains("SISTER") || input.StartsWith("SIS"))
                return 5;
            if (input.Contains("BROTHER") || input.StartsWith("B"))
                return 6;
            if (input.Contains("SPOUSE") || input.StartsWith("S"))
                return 7;
            if (input.Contains("Others") || input.StartsWith("O"))
                return 8;
            else
            {
                error.Add("Please provide valid RelationShip at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                // error.Add("There is no specified relationship");
                return 0;
            }


        }

        public static Guid ValidateLocation(string input, LocationList locations, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = locations.Where(u => u.LocationName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                if (input != "")
                    error.Add("Please provide valid Location at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddLocation' class='btn btn - default AddPopup location^" + input + "'>Add</button>");
                // error.Add("There is no specified location");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }
        public static Guid ValidateEsiLocation(string input, EsiLocationList esiLocations, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = esiLocations.Where(u => u.LocationName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                if (input != "")
                    error.Add("Please provide valid ESI Location at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddEsiLocation' class='btn btn - default AddPopup esiLocation^" + input + "'>Add</button>");
                // error.Add("There is no specified ESI Location");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }
        public static Guid ValidateEsiDespensaryList(string input, ESIDespensaryList esiDespensaryList, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = esiDespensaryList.Where(u => u.ESIDespensary.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                if (input != "")
                    error.Add("Please provide valid ESI Despensary at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddEsiDespensary' class='btn btn - default AddPopup esiDespensary^" + input + "'>Add</button>");
                // error.Add("There is no specified ESI Location");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }
        public static Guid ValidateCategory(string input, ref CategoryList categorys, ref List<string> error, string sheet, int rowNo, string colmn, int userid)
        {
            var tmp = categorys.Where(u => u.Name.ToLower().Trim() == (input.ToLower().Trim())).FirstOrDefault();
            if (tmp == null)
            {
                if (input == "")
                {
                    error.Add("Please provide valid Category at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is Empty. Plz Refer Excel");
                }
                else
                {
                    bool isSaved = false;
                    Category data = new Category();
                    data.Name = input;
                    var disorder = categorys.Count != 0 ? categorys.Select(k => k.DisOrder).ToList().Max() : 0;
                    data.DisOrder = disorder + 1;
                    var cat = categorys.CompanyId;
                    data.CompanyId = cat;
                    data.CreaateBy = userid;
                    data.ModifiedBy = data.CreaateBy;
                    data.IsDeleted = false;
                    isSaved = data.Save();
                    CategoryList CategroyList = new CategoryList(categorys.CompanyId);
                    categorys = CategroyList;
                    tmp = categorys.Where(u => u.Name.ToLower().Trim() == (input.ToLower().Trim())).FirstOrDefault();
                }
            }


            if (object.ReferenceEquals(tmp, null))
            {
                error.Add("Please provide valid Category at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddCategory' class='btn btn - default AddPopup category^" + input + "'>Add</button>");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }
        public static Guid ValidateBank(string input, BankList banks, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = banks.Where(u => u.BankName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                error.Add("Please provide valid Bank Name at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list.Do you want to add? <button id='btnAddBank' class='btn btn - default AddPopup bank^" + input + "'>Add</button>");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }

        public static Guid ValidateDesignation(string input, DesignationList designationList, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = designationList.Where(u => u.DesignationName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                if (input != "")
                    error.Add("Please provide valid Designation at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddDesignation' class='btn btn - default AddPopup designation^" + input + "'>Add</button>");
                // error.Add("There is no specified Designation");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }

        public static Guid ValidateDepartment(string input, DepartmentList departmentList, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = departmentList.Where(u => u.DepartmentName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                if (input != "")
                    error.Add("Please provide valid Department at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddDepartment' class='btn btn - default AddPopup department^" + input + "'>Add</button>");
                // error.Add("There is no specified Department Name");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }

        public static Guid ValidateCostCentre(string input, CostCentreList costCentreList, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = costCentreList.Where(u => u.CostCentreName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                if (input != "")
                    error.Add("Please provide valid Cost Centre at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddCostCentre' class='btn btn - default AddPopup costCentre^" + input + "'>Add</button>");
                //   error.Add("There is no specified Cost Centre");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }

        public static Guid ValidatePTLocation(string input, PTLocationList pTLocationList, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = pTLocationList.Where(u => u.PTLocationName.Trim().ToLower() == input.Trim().ToLower()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                if (input != "")
                    error.Add("Please provide valid PT Location at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddPTLocation' class='btn btn - default AddPopup ptlocation^" + input + "'>Add</button>");
                //  error.Add("There is no specified PT Location");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }
        //public static int validatebloodgroup(string input, BloodGroupList bldgrp, ref List<string> error, string sheet, int rowNo, string colmn)
        //{
        //    var tmp = bldgrp.Where(u => u.BloodGroupName.Contains(input)).FirstOrDefault();
        //    if (object.ReferenceEquals(tmp, null))
        //    {
        //        error.Add("Please provide valid Blood Group at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list.");
        //        // error.Add("There is no specified Category");
        //        return 0;
        //    }
        //    else
        //    {
        //        return tmp.Id;
        //    }
        //}
        public static Guid ValidateGrade(string input, GradeList gradeList, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = gradeList.Where(u => u.GradeName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null) || string.IsNullOrEmpty(input))
            {
                error.Add("Please provide valid Grade at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddGrade' class='btn btn - default AddPopup grade^" + input + "'>Add</button>");
                // error.Add("There is no specified PT Location");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }
        public static int ValidateLanguage(string input, ref List<Language> languages, ref List<string> error, string sheet, int rowNo, string colmn, int compId, int userId)
        {
            var tmp = languages.Where(u => u.Name.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {

                //error.Add("Please provide valid Language at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                //// error.Add("There is no specified Language");
                //return 0;
                if (input == "")
                {
                    error.Add("Please provide valid Language at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                    tmp.LangId = 0;
                }
                else
                {
                    Language language = new Language();
                    language.Name = input;
                    language.CreatedOn = DateTime.Now;
                    language.ModifiedOn = DateTime.Now;
                    language.CreatedBy = userId.ToString();
                    language.ModifiedBy = language.CreatedBy.ToString();
                    language.IsActive = false;
                    language.CompanyId = compId;
                    language.Save();
                    languages = language.LanguagesList(compId);
                    tmp = languages.Where(u => u.Name.Trim() == (input).Trim()).FirstOrDefault();
                }
                return tmp.LangId;
            }
            else
            {
                return tmp.LangId;
            }
        }
        public static int ValidateBloodGroup(string input, List<BloodGroup> bloodGroup, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = bloodGroup.Where(u => u.BloodGroupName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                if (input != "")
                    error.Add("Please provide valid BloodGroup at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list.");
                // error.Add("There is no specified Language");
                return 0;
            }
            else
            {
                return tmp.Id;
            }
        }
        public static Guid ValidateBranch(string input, BranchList branch, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = branch.Where(u => u.BranchName.ToLower().Trim() == (input.ToLower().Trim())).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                error.Add("Please provide valid Branch at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is not in the list. Do you want to add? <button id='btnAddBranch' class='btn btn - default AddPopup branch^" + input + "'>Add</button>");
                // error.Add("There is no specified Language");
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }

        public static Guid ValidateBenefitComponent(string input, List<keyValueItem> benefitComponet, ref List<string> error, string sheet, int rowNo, string colmn)
        {

            var tmp = benefitComponet.Where(u => u.DisplayName.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (object.ReferenceEquals(tmp, null))
            {
                error.Add("Please provide valid Benefit component at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet);
                return Guid.Empty;
            }
            else
            {
                return tmp.Id;
            }
        }

        public static Guid ValidateLoanMaster(string input, AttributeModelList attributeModelList, ref List<string> error, string sheet, int rowNo, string colmn)
        {
            var tmp = attributeModelList.Where(u => u.Name.ToLower().Trim() == (input).ToLower().Trim()).FirstOrDefault();
            if (!object.ReferenceEquals(tmp, null))
            {
                error.Add("Please provide valid Loan Code at row " + (rowNo + 1) + " and column " + colmn + " in the sheet of " + sheet + " " + input + " is Already Exist.");
                // error.Add("There is no specified ESI Location");
                return tmp.Id;
            }
            else
            {
                return Guid.Empty;

            }
        }
        public void WriteLog(string strContent)
        {
            try
            {
                using (StreamWriter w = File.AppendText("C:\\temp\\ " + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt"))
                {
                    w.WriteLine(strContent + " - " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
    public class keyValueItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}
