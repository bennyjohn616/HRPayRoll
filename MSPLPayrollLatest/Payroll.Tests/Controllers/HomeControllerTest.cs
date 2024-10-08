using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payroll;
using Payroll.Controllers;
using PayrollBO;

namespace Payroll.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            // ViewResult result = controller.About() as ViewResult;

            // Assert
            //  Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            //ViewResult result = controller.Contact() as ViewResult;

            // Assert
            //Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ComputeTax()
        {
            TXFinanceYearList txFinanceYear = new TXFinanceYearList(28);
            TXFinanceYear financeYear = txFinanceYear.Where(f => f.IsActive == true).FirstOrDefault();
            if (!ReferenceEquals(financeYear,null))
            {
                TXSectionList allSection = new TXSectionList(28, financeYear.Id, Guid.Empty);
                TXSectionList section = new TXSectionList();
                TXSectionList subSections = new TXSectionList();
                TXSectionList otherSections = new TXSectionList();

                section.AddRange(allSection.Where(s => s.ParentId != Guid.Empty).ToList().OrderBy(s=>s.OrderNo));
                subSections.AddRange(allSection.Where(s => s.ParentId == Guid.Empty && s.SectionType != "Others").OrderBy(s => s.OrderNo));
                otherSections.AddRange(allSection.Where(s => s.SectionType == "Others").OrderBy(s => s.OrderNo));



                TaxComputationInfo taxinfo = new TaxComputationInfo();
                taxinfo.CompanyId = 12;
                taxinfo.FinanceYear = financeYear;
                taxinfo.FinanceYearId = financeYear.Id;
                taxinfo.ApplyMonth = 6;
                taxinfo.ApplyYear = 2016;
                taxinfo.UserId = 1;
                taxinfo.Employees.AddRange(new EmployeeList(12, 1,Guid.Empty).Where(e => e.EmployeeCode == "3526S").ToList());
                taxinfo.Sections = section;
                taxinfo.SubSections = subSections;
                taxinfo.OtherIncomeHeads = otherSections;

                ITax.ComputeTax(taxinfo);
            }
        }
    }
}
