﻿


$('#rbtnStopPaymentYes').on("click", function (e) {
    $("#rbtnPayrollProcessYes").attr('disabled', false);
    $("#rbtnPayrollProcessNo").attr('disabled', false);
});
$('#rbtnStopPaymentNo').on("click", function (e) {
    $("#rbtnPayrollProcessYes").attr('disabled', true);
    $("#rbtnPayrollProcessNo").attr('disabled', true);
    $("#rbtnPayrollProcessYes").prop('checked', 'true');
});

$('#idSendWishes').on("click", function (e) {
    debugger;
    if ($('#txtgreetingcontnet').val() == "") {
        $app.showAlert("Please Enter Your Wishes!!!", 4);
    }
    else {
        $employee.SendGreetingsMail($('#idlblHidnempid').text(), $('#idlblHidnType').text(), $('#txtgreetingcontnet').val());
    }


});
$('#btnBirthdayPrevious').on("click", function (e) {
    var Previous = parseInt($('#idHidenmonth').text()) - 1;
    if (Previous != 0) {
        $employee.ButtonBirthdayRender(Previous);
    }


});
$('#btnBirthdayNext').on("click", function (e) {
    var Next = parseInt($('#idHidenmonth').text()) + 1;
    if (Next != 13) {
        $employee.ButtonBirthdayRender(Next);
    }


});
$('#btnBirthdayToday').on("click", function (e) {
    debugger;
    var buttonname = $('#btnBirthdayToday').text();

    if (buttonname == "Today") {
        $('#btnBirthdayToday').text('Month View');
        $('#btnBirthdayNext').addClass('nodisp');
        $('#btnBirthdayPrevious').addClass('nodisp');
        $('#idMonthBday').addClass('nodisp');
        $('#idTodayBday').removeClass('nodisp');
        $employee.TodayBirthdayRender();
    }
    else {
        $('#btnBirthdayToday').text('Today');
        $('#btnBirthdayNext').removeClass('nodisp');
        $('#btnBirthdayPrevious').removeClass('nodisp');
        $('#idMonthBday').removeClass('nodisp');
        $('#idTodayBday').addClass('nodisp');
        $employee.BirthdayRender();
    }

});

$('#btnServicePrevious').on("click", function (e) {
    var ServicePrevious = parseInt($('#idServiceHidenmonth').text()) - 1;
    if (ServicePrevious != 0) {
        $employee.ButtonServiceRender(ServicePrevious);
    }


});
$('#btnServiceNext').on("click", function (e) {
    var ServiceNext = parseInt($('#idServiceHidenmonth').text()) + 1;
    if (ServiceNext != 13) {
        $employee.ButtonServiceRender(ServiceNext);
    }
});
$("#txtEmail").blur(function () {

    if ($("#txtEmail").val().length > 0)
        $employee.mailcheck();
});
$('#txtDOB').change(function () {

    var date = new Date(Date.parse($('#txtDOB').val()));
    var today = new Date();
    var age = Math.floor((today - date) / (365.25 * 24 * 60 * 60 * 1000));
    $('#txtAge').val(age);


});


var $employee = {
    canSave: true,
    selectedEmployeeId: null,//'E5F95082-E525-41E2-91CF-3887244EF41A',
    formData: document.forms["frmEmployeeSave"],
    formEmpData: document.forms["frmEmployeeDisplay"],
    employeeList: null,
    EmpcodeAutoManual: null,
    bindEvent: function () {

        $('#empNavtabs a').on("click", function (e) {
            $("#tabcontent").removeClass('nodisp');
            $employee.LoadEmpRelated(this);
            $('#empOthers').html('');
        });
        $("#txtDOW").change(function () {
            if ($("#txtDOW").val().trim() != "") {
                var dob = new Date($("#txtDOB").val());
                var dow = new Date($("#txtDOW").val());
                if ((dow < dob)) {
                    $app.showAlert("DOW of Employee age Should be greater than DOB!!", 4);
                    $("#txtDOW").val('');
                }
            }

        });

        $("#txtEmpCode").change(function () {
            $employee.EmployeeCdecheck();
        });

        $("#txtConfirmationPeriod").change(function () {
            var doj = new Date($("#txtDOJ").val());
            confirmationperiod = $("#txtConfirmationPeriod").val();
            if (confirmationperiod.length > 0) {
                doj.setMonth(doj.getMonth() + parseInt(confirmationperiod));
            }

            else {
                doj.setMonth(doj.getMonth());
            }
            $("#txtConfirmationDate").val(doj.getDate() + '/' + $payroll.GetMonthName((doj.getMonth() + 1)) + '/' + doj.getFullYear());
        });
        $('#txtRetirementYears').keypress(function () {

            if ($("#txtDOB").val() == "") {
                $app.hideProgressModel();
                $('#txtRetirementYears').val('');
                $app.showAlert("Please select The Date of birth", 4);
            }


        });
        $('#txtDOJ').focus(function () {



            var empcode = $("#txtEmpCode").val();

            if (empcode != "**New**") {

                $employee.GetPayrollDetails();

            }


        });


        $('#txtRetirementYears,#txtDOB,#txtDOJ').change(function () {


            var empcode = $("#txtEmpCode").val();
            var d = new Date();
            var month = d.getMonth() + 1;
            var day = d.getDate();

            var output = d.getFullYear() + '/' +
                (month < 10 ? '0' : '') + month + '/' +
                (day < 10 ? '0' : '') + day;

            var Entereddate = new Date($("#txtDOB").val());
            var month = Entereddate.getMonth() + 1;
            var day = Entereddate.getDate();

            var Eoutput = Entereddate.getFullYear() + '/' +
                (month < 10 ? '0' : '') + month + '/' +
                (day < 10 ? '0' : '') + day;

            //if (this.id == "txtDOJ") {
            //    if (empcode != "**New**") {

            //        $employee.GetPayrollDetails();

            //    }
            //}

            if ($("#txtDOB").val() != "") {
                if (Eoutput > output || Eoutput == output) {

                    $app.showAlert("Date of Birth should be lesser that Current date", 4);

                    $("#txtDOB").val('');
                    return false;

                } else {
                    $employee.Retirementcheck();
                }
            }
            else {

            }



        });
        $('#btnAddEmployee,#btnBack').on("click", function (e) {
            debugger
            switch (this.id) {
                case "btnAddEmployee":
                    $('#btnAddempHide').click();
                    $('#txtAge').val('');
                    $employee.selectedEmployeeId = null;
                    $employee.GetCodeAutoMan();
                    $employee.addNewClick();
                    $employee.clearControl();

                    if ($employee.EmpcodeAutoManual == "1") {
                        if ($('#tblemployee tr').length != 0) {
                            $($employee.formData).find('#txtEmpCode').val('**New**');
                            $($employee.formData).find('#txtEmpCode').attr('readonly', 'true');
                        }
                    }
                    else {
                        $($employee.formData).find('#txtEmpCode').val('**New**');
                        $($employee.formData).find('#txtEmpCode').removeAttr('readonly');
                    }
                    if ($('#tblemployee tr').length == 0) {
                        $.ajax({
                            url: $app.baseUrl + "Employee/GetEmployeeCodeAutonumber",
                            data: null,
                            dataType: "json",
                            contentType: "application/json",
                            type: "POST",
                            success: function (jsonResult) {
                                switch (jsonResult.Status) {
                                    case true:
                                        //$("#txtEmpCode").val(jsonResult.result);
                                        break;
                                    case false:
                                        $app.showAlert("Please Config Employee Auto Number in App Setting Menu", 3);
                                        break;
                                }
                            }
                        });
                    }
                    break;
                case "btnBack":
                    debugger
                    $employee.LoadEmployee();
                    $('#btnBack').addClass('hide');
                    $employee.selectedEmployeeId = null;
                    break;
            }
        });

        $('#frmEmployeeSave').on("submit", function (e) {
            $employee.save();
            return false;
        });



    },




    GetCodeAutoMan: function () {

        $.ajax({
            url: $app.baseUrl + "Setting/GetcodeAutoManual",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {

                $employee.EmpcodeAutoManual = jsonResult.result;
            },
            complete: function () {
            }
        });
    },
    GetPayrollDetails: function () {

        var empidd = $('#empid').val();
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmployeePayrollDetailsData",
            data: JSON.stringify({ empId: empidd }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:


                        break;
                    case false:

                        $app.showAlert(jsonResult.Message, 4);
                        $('.ui-datepicker-calendar').css("display", "none");
                        $('.ui-datepicker-month').css("display", "none");
                        $('.ui-datepicker-year').css("display", "none");
                        $('.ui-icon.ui-icon-circle-triangle-w').css("display", "none");
                        $('.ui-icon.ui-icon-circle-triangle-e').css("display", "none");


                        $('#txtDOJ').blur();
                        //var dojvalue = $('#txtDOJ').val();

                        //$("#txtDOJ").datepicker("setDate", dojvalue);
                        break;
                }
            },

        });

    },

    loadInitial: function () {
        $employee.LoadEmployee();
        $employee.bindEvent();
        $companyCom.loadCategory({ id: 'ddCategory' });
        $companyCom.loadDepartment({ id: "ddDepartment" });
        $companyCom.loadLocation({ id: "ddLocation" });
        $companyCom.loadESILocation({ id: "ddESILocation" });
        $companyCom.loadESIDespensary({ id: "ddESIDispensary" });
        $companyCom.loadPTLocation({ id: "ddPTLocation" });
        // $companyCom.loadESIDespensary({ id: "ddDepartment" });
        $companyCom.loadDesignation({ id: "ddDesignation" });
        $companyCom.loadCostCentre({ id: "ddCostCentre" });
        $companyCom.loadBranch({ id: "ddBranch" });
        // $companyCom.loadCategory({ id: "ddDepartment" });
        $companyCom.loadGrade({ id: "ddGrade" });
        // $companyCom.loadEsiLocation({ id: "ddDepartment" });
        // $companyCom.loadBloodGroup({ id: "ddDepartment" });
        $companyCom.loadLanguge({ id: "ddEmpLanguageName" });
        $companyCom.loadBank({ id: "ddBank" });
        $companyCom.loadRelationship([{ id: "ddRelationShip" }, { id: "ddNomineeRelationShip" }, { id: "ddEmerRelationShip" }]);
        $payroll.initDatetime();
        $payroll.GetMonthName();

        $('#hlnkCategory').on('click', function () {
            $category.AddInitialize();
        });


    },
    LoadEmployee: function () {
        var dtClientList = $('#tblemployee').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"], [2, "asc"]],

            columns: [
             { "data": "empid" },
                    { "data": "empCode" },
                       { "data": "empFName" },
                       { "data": "empLName" },
                       { "data": "empEmail" },
                       { "data": "empPhone" },
                       { "data": "empDOB" },
                        { "data": "empDOJ" },
                        { "data": "dept" },
                        { "data": "designation" },
                       {
                           "data": null
                       }
            ],

            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "bSearchable": false,
             type: 'natural',
         },
         {
             "aTargets": [6],
             "sClass": "nodisp",
             "bSearchable": false
         },
         {
             "aTargets": [1, 2, 3, 4, 5, 6, 7],
             "sClass": "word-wrap"

         },
      {
          "aTargets": [10],
          // "aaSorting": [[2, "asc"]],
          "sClass": "actionColumn",
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  debugger
                  $employee.getEmployee(oData);
                  $employee.addNewClick();
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete Employee?')) {
                      if (confirm('Are you sure ,do you want to delete User also?')) {
                          $employee.deleteData({ empid: oData.empid, IsDeleteUser: 'Yes' });
                      }
                      else {
                          $employee.deleteData({ empid: oData.empid, IsDeleteUser: 'No' });
                      }

                      return false;
                  }
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend(b, c);
          }
      }
            ], responsive: true,

            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetEmployees",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                var out = jsonResult.result;
                                $employee.employeeList = out;
                                setTimeout(function () {

                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });

                                }, 50);
                                break;
                            case false:
                                $app.showAlert(jsonResult.Message, 4);
                                //alert(jsonResult.Message);
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {

                var r = $('#tblCompany tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblCompany thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

        oTable = $('#tblemployee').DataTable();   //Search Option
        $('#txtSearch').keyup(function () {
            oTable.search($(this).val()).draw();
        })

        // $('#tblemployee').dataTable({ bFilter: true, bInfo: false });

    },
    getEmployee: function (data) {
         
        $employee.selectedEmployeeId = data.empid;
        $('#empid').val(data.empid);
        // $('#btnAddEmployee').hide();
        $('#btnAddempHide').click();
        $employee.renderEmployee(data);
        //$('#employees').css('display','none');
        // $('#AddEmployee').modal('toggle');
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmployeeData",
            data: JSON.stringify({ empId: $employee.selectedEmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        //  $('#AddEmployee').modal('toggle');
                        var p = jsonResult.result;
                        data.EmployeeImage = p.EmployeeImage;

                        $employee.renderEmployee(data);
                        // $employee.renderEmployee(p);
                        break;
                    case false:
                        alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    renderEmployee: function (data) {
        debugger;
        $($employee.formData).find('#txtEmpCode').val(data.empCode);
        $($employee.formData).find('#txtEmpCode').attr('readonly', 'true');
        $($employee.formData).find('#txtFirstName').val(data.empFName);
        $($employee.formData).find('#txtLastName').val(data.empLName);
        $($employee.formData).find('#txtEmail').val(data.empEmail);
        $($employee.formData).find('#txtPhone').val(data.empPhone);
        $($employee.formData).find('#ddDesignation').val(data.empDesign);
        $($employee.formData).find('#empGender').val(data.empGender);
        $($employee.formData).find('#ddDepartment').val(data.empDepart);
        $($employee.formData).find('#ddBranch').val(data.empBranch);
        $($employee.formData).find('#ddLocation').val(data.empLocation);
        $($employee.formData).find('#ddESILocation').val(data.empESILocation);
        $($employee.formData).find('#ddESIDispensary').val(data.empESIDispensary);
        $($employee.formData).find('#ddPTLocation').val(data.empPTLocation);
        $($employee.formData).find('#ddGrade').val(data.empGrade);
        $($employee.formData).find('#ddCostCentre').val(data.empCostCentre);
        $($employee.formData).find('#txtDOB').val(data.empDOB);
        if (data.empDOB != '' && data.empDOB != null) {

            var date = new Date(Date.parse(data.empDOB));
            var today = new Date();
            var age = Math.floor((today - date) / (365.25 * 24 * 60 * 60 * 1000));
            $('#txtAge').val(age);
        }
        $($employee.formData).find('#txtDOJ').val(data.empDOJ);
        $($employee.formData).find('#txtDOW').val(data.empDOW);
        $($employee.formData).find('#txtConfirmationPeriod').val(data.empConfPeriod);
        $($employee.formData).find('#txtConfirmationDate').val(data.empConfDate);
        $($employee.formData).find('#txtSeparationDate').val(data.empSeparationDate);
        $($employee.formData).find('#txtRetirementYears').val(data.empRetYears);
        $($employee.formData).find('#txtRetirementDate').val(data.empRetDate);
        (data.empisMetro == true) ? $($employee.formData).find("#rbtnIsMetroYes").prop('checked', 'true') : $($employee.formData).find("#rbtnIsMetroNo").prop('checked', 'true');
        (data.empStopPayment == true) ? $($employee.formData).find("#rbtnStopPaymentYes").prop('checked', 'true') : $($employee.formData).find("#rbtnStopPaymentNo").prop('checked', 'true');
        (data.empPayrollProcess == true) ? $($employee.formData).find("#rbtnPayrollProcessYes").prop('checked', 'true') : $($employee.formData).find("#rbtnPayrollProcessNo").prop('checked', 'true');
        $($employee.formData).find('#ddStatus').val(data.empStatus);
        $($employee.formData).find('#ddCategory').val(data.categoryId);
        if (data.empStopPayment == true) {
            $("#rbtnPayrollProcessYes").attr('disabled', false);
            $("#rbtnPayrollProcessNo").attr('disabled', false);
        } else {
            $("#rbtnPayrollProcessYes").attr('disabled', true);
            $("#rbtnPayrollProcessNo").attr('disabled', true);
            $("#rbtnPayrollProcessYes").prop('checked', 'true');
        }

        if (data.empPwdMailSend == true)
            $($employee.formData).find("#chkIsMailsend").prop('checked', 'true');
        $("#chkIsMailsend").attr('disabled', true);
        var fileNameEmpImg = data.EmployeeImage;
        if (fileNameEmpImg != null) {
            debugger;
            var temp3 = fileNameEmpImg.split("/");
            jQuery("label[for='myEmpImgName']").html(temp3[temp3.length - 1] + '...');
            $($employee.formData).find('#lblEmpImgName').attr('title', temp3);

        }
        else {
            $('#lblEmpImgName').text("Upload Profile");
            $($employee.formData).find('#lblEmpImgName').attr('title', 'Upload Profile');
        }
        $($employee.formData).find('#EMPimg').attr('src', data.EmployeeImage == null ? 'assets/images/profile.png' : data.EmployeeImage.replace("~/", " "));
        if (data.EmployeeImage != null) {
            $('#EMPimg').removeClass('hide')
        }
    },
    //--- created by Keerthika on 08/06/2017
    getEmployeeData: function () {

        //  $employee.selectedEmployeeId = data.empid;
        // $('#btnAddEmployee').hide();
        // $('#btnAddempHide').click();
        //   $employee.renderEmployee(data);
        //$('#employees').css('display','none');
        // $('#AddEmployee').modal('toggle');
        $.ajax({
            url: $app.baseUrl + "Employee/GetSelectedEmployeeData",
            data: null,

            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        //  $('#AddEmployee').modal('toggle');
                        var p = jsonResult.result;
                        var employee = p.EmployeeDetails;
                        var emppersonal = p.EmpPersonal;
                        var empAcademic = p.EmployeeAcademic;
                        var employeeaddress = p.EmployeeAddress;
                        var employeecomponent = p.EmployeeComponent;
                        var empfamily = p.EmpFamilyList;
                        var empEmergency = p.EmpEmergencyComtact;
                        var empemployeement = p.EmpEmployeement;
                        var empnominee = p.EmpNominee;
                        var emplangknow = p.EmpLangKnow;
                        var empHrcomponent = p.EmpHrComponent;
                        var emptraining = p.EmpTraining;

                        $employee.renderEmpDetails(p);
                        // $employee.renderEmployee(p);
                        break;
                    case false:
                        alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    //---
    renderEmpDetails: function (p) {

        $employee.renderEmployeeData(p.EmployeeDetails);
        $employee.renderEmpAcademicData(p.EmployeeAcademic);
        $employee.renderEmpAddressData(p.EmployeeAddress);
    },

    ToJavaScriptDate: function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        if (dt.getDate() == 1 && dt.getMonth() == 0 && dt.getFullYear() == 1)
        {
            return null;
        }
        return dt;
},
    //-----
    renderEmployeeData: function (employee) {

        var locale = "en-us";
        var dob = $employee.ToJavaScriptDate(employee.DateOfBirth);
        employee.DateOfBirth = dob == null ? null : dob.getDate() + "/" + (dob.getMonth() + 1) + "/" + dob.getFullYear();
        var doj = $employee.ToJavaScriptDate(employee.DateOfJoining);
        employee.DateOfJoining = doj == null ? null : doj.getDate() + "/" + (doj.getMonth() + 1) + "/" + doj.getFullYear();
        var dow = $employee.ToJavaScriptDate(employee.DateOfWedding);
        employee.DateOfWedding = dow == null ? null : dow.getDate() + "/" + (dow.getMonth() + 1) + "/" + dow.getFullYear();
        var doc = $employee.ToJavaScriptDate(employee.ConfirmationDate);
        employee.ConfirmationDate = doc == null ? null : doc.getDate() + "/" + (doc.getMonth() + 1) + "/" + doc.getFullYear();
        var dos = $employee.ToJavaScriptDate(employee.SeparationDate);
        employee.SeparationDate = dos == null ? null : dos.getDate() + "/" + (dos.getMonth + 1) + "/" + dos.getFullYear();
        var dor = $employee.ToJavaScriptDate(employee.RetirementDate);
        employee.RetirementDate = dor == null ? null : dor.getDate() + "/" + (dor.getMonth() + 1) + "/" + dor.getFullYear();
        $($employee.formEmpData).find('#lblEmpCode').text(employee.EmployeeCode == null ? "" : employee.EmployeeCode);
        $($employee.formEmpData).find('#lblFname').text(employee.FirstName == null ? "" :employee.FirstName);
        $($employee.formEmpData).find('#lblLname').text(employee.LastName == null ? "" : employee.LastName );
        $($employee.formEmpData).find('#lblEmail').text(employee.Email == null ? "" :employee.Email);
        $($employee.formEmpData).find('#lblPhone').text(employee.Phone == null ? "" : employee.Phone);
        $($employee.formEmpData).find('#lblDesign').text(employee.DesignationName == null ? "" :employee.DesignationName);
        // $($employee.formEmpData).find('#lblGender').text(employee.Gender);
        // (employee.Gender == 1) ?$($employee.formEmpData).find('#lblGender').text(typeof(GenderEnum))
        if (employee.Gender == 0) $($employee.formEmpData).find('#lblGender').text("");
        if (employee.Gender == 1) $($employee.formEmpData).find('#lblGender').text("Male");
        if (employee.Gender == 2) $($employee.formEmpData).find('#lblGender').text("Female");
        $($employee.formEmpData).find('#lblDept').text(employee.DepartmentName == null ? "" :employee.DepartmentName );
        $($employee.formEmpData).find('#lblBranch').text(employee.BranchName == null ? "" :employee.BranchName);
        $($employee.formEmpData).find('#lblLocation').text(employee.LocationName == null ? "" :employee.LocationName);
        $($employee.formEmpData).find('#lblEsiLoc').text(employee.ESILocationName == null ? "" :employee.ESILocationName);
        $($employee.formEmpData).find('#lblEsiDisp').text(employee.ESIDespensaryName == null ? "" :employee.ESIDespensaryName);
        $($employee.formEmpData).find('#lblPtLoc').text(employee.PTLocationName == null ? "" :employee.PTLocationName);
        $($employee.formEmpData).find('#lblGrade').text(employee.GradeName == null ? "" :employee.GradeName);
        $($employee.formEmpData).find('#lblCostCenter').text(employee.CostCentreName == null ? "" :employee.CostCentreName);
        $($employee.formEmpData).find('#lblDob').text(employee.DateOfBirth == null ? "" :employee.DateOfBirth);
        //  (employee.DateOfBirth != d.MinValue )? $($employee.formEmpData).find('#lblDob').text(employee.DateOfBirth.ToString("dd/MMM/yyyy") ): "";
        //  $($employee.formEmpData).find('#lblDob').text(d);
        $($employee.formEmpData).find('#lblDoj').text(employee.DateOfJoining == null ? "" :employee.DateOfJoining);
        // $($employee.formEmpData).find('#lblDoj').text();
        $($employee.formEmpData).find('#lblDow').text(employee.DateOfWedding == null ? "" :employee.DateOfWedding);
        $($employee.formEmpData).find('#lblConfirmPeriod').text(employee.ConfirmationPeriod == null ? "" :employee.ConfirmationPeriod);
        $($employee.formEmpData).find('#lblConfirmDate').text(employee.ConfirmationDate== null ? "" :employee.ConfirmationDate);
        $($employee.formEmpData).find('#lblSepDat').text(employee.SeparationDate == null ? "" :employee.SeparationDate);
        $($employee.formEmpData).find('#lblRetYears').text(employee.RetirementYears == null ? "" : employee.RetirementYears);

        if (employee.RetirementDate != "NaN/Invalid Date/NaN" || employee.ConfirmationDate != "NaN/Invalid Date/NaN" || employee.DateOfBirth != "NaN/Invalid Date/NaN") {

            $($employee.formEmpData).find('#lblRetDate').text(employee.RetirementDate);
        }
        if (employee.DateOfBirth == "NaN/Invalid Date/NaN") {
            $($employee.formEmpData).find('#lblDob').text('');
        }
        if (employee.DateOfJoining == "NaN/Invalid Date/NaN") {
            $($employee.formEmpData).find('#lblDoj').text('');
        }
        if (employee.DateOfWedding == "NaN/Invalid Date/NaN") {
            $($employee.formEmpData).find('#lblDow').text('');
        }
        if (employee.ConfirmationPeriod == 0) {
            $($employee.formEmpData).find('#lblConfirmPeriod').text('');
        }
        if (employee.ConfirmationDate == "NaN/Invalid Date/NaN") {
            $($employee.formEmpData).find('#lblConfirmDate').text('');
        }
        if (employee.RetirementDate == "NaN/Invalid Date/NaN" || employee.RetirementDate == null) {
            $($employee.formEmpData).find('#lblRetDate').text('');
        }
        if (employee.RetirementYears == 0) {
            $($employee.formEmpData).find('#lblRetYears').text('');
        }
        if (employee.SeparationDate == "NaN/Invalid Date/NaN") {
            $($employee.formEmpData).find('#lblSepDat').text('');
        }
        (employee.isMetro == true) ? $($employee.formEmpData).find("#rbtnIsMetroYes").prop('checked', 'true') : $($employee.formEmpData).find("#rbtnIsMetroNo").prop('checked', 'true');
        (employee.StopPayment == true) ? $($employee.formEmpData).find("#rbtnStopPaymentYes").prop('checked', 'true') : $($employee.formEmpData).find("#rbtnStopPaymentNo").prop('checked', 'true');
        (employee.PayrollProcess == true) ? $($employee.formEmpData).find("#rbtnPayrollProcessYes").prop('checked', 'true') : $($employee.formEmpData).find("#rbtnPayrollProcessNo").prop('checked', 'true');
        // $($employee.formEmpData).find('#lblStatus').text(employee.Status);
        (employee.Status == 1) ? $($employee.formEmpData).find('#lblStatus').text("Active") : $($employee.formEmpData).find('#lblStatus').text("Inactive");
        $($employee.formEmpData).find('#lblCategory').text(employee.CategoryName == null ? "" : employee.CategoryName);
        if (employee.StopPayment == true) {
            $("#rbtnPayrollProcessYes").attr('disabled', false);
            $("#rbtnPayrollProcessNo").attr('disabled', false);
        } else {
            $("#rbtnPayrollProcessYes").attr('disabled', true);
            $("#rbtnPayrollProcessNo").attr('disabled', true);
            $("#rbtnPayrollProcessYes").prop('checked', 'true');
        }
    },
    //----
    renderEmpAcademicData: function (empAcademic) {

        $employee.LoadAcademics(empAcademic);

    },
    //----
    LoadAcademics: function (empAcademic) {

        var dtacademic = $('#tblAcademic').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "EmployeeId" },
                    { "data": "DegreeName" },
                     { "data": "InstitionName" },
                      { "data": "YearOfPassing" },


            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "sClass": "nodisp",
             "bSearchable": false

         },
         {
             "aTargets": [2],
             "sClass": "word-wrap"

         },
            {
                "aTargets": [3],
                "sClass": "word-wrap"

            },
             {
                 "aTargets": [4],
                 "sClass": "word-wrap"

             }



            ],
            ajax: function (data, callback, settings) {

                setTimeout(function () {
                    callback({
                        draw: data.draw,
                        data: empAcademic,
                        recordsTotal: empAcademic.length,
                        recordsFiltered: empAcademic.length
                    });
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblAcademic tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblAcademic thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    //----
    renderEmpAddressData: function (employeeaddress) {

        if (employeeaddress.length > 0) {
            $($employee.formEmpData).find('#lblAddress1').text(employeeaddress[0].AddressLine1);
            $($employee.formEmpData).find('#lblAddress2').text(employeeaddress[0].AddressLine2);
            $($employee.formEmpData).find('#lblCity').text(employeeaddress[0].City);
            $($employee.formEmpData).find('#lblState').text(employeeaddress[0].State);
            $($employee.formEmpData).find('#lblPincode').text(employeeaddress[0].PinCode);
            $($employee.formEmpData).find('#lblCountry').text(employeeaddress[0].Country);
            $($employee.formEmpData).find('#lblPhoneAddress').text(employeeaddress[0].Phone);

        }
    },
    //----
    LoadEmpRelated: function (context) {
        debugger;
        switch (context.id) {
            case "emp_family":
                $family.LoadFamilys({ id: $employee.selectedEmployeeId });
                break;
            case "emp_academic":
                $academic.LoadAcademics({ id: $employee.selectedEmployeeId });
                break;
            case "emp_Training":
                $training.LoadTrainings({ id: $employee.selectedEmployeeId });
                break;
            case "emp_employeement":
                $employeement.LoadEmployeements({ id: $employee.selectedEmployeeId });
                break;
            case "emp_Nominee":
                $nominee.LoadNominees({ id: $employee.selectedEmployeeId });
                break;
            case "emp_Benefit":
                $benefitComponent.LoadbenefitComponents({ id: $employee.selectedEmployeeId });
                break;
            case "emp_EmergencyCon":
                $emergencyContact.LoademergencyContacts({ id: $employee.selectedEmployeeId });
                break;
            case "emp_Language":
                $languageKnown.LoadlanguageKnowns({ id: $employee.selectedEmployeeId });
                break;
            case "emp_ContractDetails":
                $languageKnown.loadEmpContractDetail({ id: $employee.selectedEmployeeId });
                break;
            case "emp_joiningdocument":
                $joinDoc.initiateFormEmployee({ id: $employee.selectedEmployeeId });
                break;
            case "emp_HRcomponent":
                $HRComponent.initiateFormEmployee({ id: $employee.selectedEmployeeId });
                break;
            case "emp_Address":
                $empAddress.get({ id: $employee.selectedEmployeeId });
                break;
            case "emp_bankdetails":
                $empbank.get({ id: $employee.selectedEmployeeId });
                break;
            default:
                break;
        }

    },
    Retirementcheck: function () {

        var dob = new Date($("#txtDOB").val());
        var doj = new Date($("#txtDOJ").val());
        var retirementyears = $("#txtRetirementYears").val().trim();
        dob.setFullYear(dob.getFullYear() + parseInt(retirementyears));
        var Rtrmdt = new Date(dob.getDate() + '/' + $payroll.GetMonthName((dob.getMonth() + 1)) + '/' + dob.getFullYear());
        var retirementdate = (dob.getFullYear() - doj.getFullYear());
        if (Rtrmdt >= doj || retirementyears == '') {
            if (retirementyears != '')
                $("#txtRetirementDate").val(('0' + dob.getDate()).slice(-2) + '/' + $payroll.GetMonthName((dob.getMonth() + 1)) + '/' + dob.getFullYear());
            return true;
        }
        else {
            $app.showAlert("Retirement Date is lesser than Date of Joining!! Average retirement age is 58", 1);
            $("#txtRetirementYears").val('');
            $("#txtRetirementDate").val('');
        };
    },
    mailcheck: function () {

        var email = {
            empid: $employee.selectedEmployeeId,
            empEmail: $($employee.formData).find('#txtEmail').val().trim()

        };
        $.ajax({
            url: $app.baseUrl + "Employee/EmailCheck",
            data: JSON.stringify({ dataValue: email }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $('#txtEmail').val('');
                        $('#txtEmail').focus();
                        break;
                }
            },
            complete: function () {
                $employee.canSave = true;
            }
        });
    },
    EmployeeCdecheck: function () {

        var empcode = $($employee.formData).find('#txtEmpCode').val().trim();
        $.ajax({
            url: $app.baseUrl + "Setting/EmpCodeCheck",
            data: JSON.stringify({ EmployeeCde: empcode }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $('#txtEmpCode').val('');
                        $('#txtEmpCode').focus();
                        break;
                }
            },
            complete: function () {
                $employee.canSave = true;
            }
        });
    },

    save: function () {

        var err = 0
        $(".Reqrd").each(function () {


            if (this.id == "ddCategory" || this.id == "empGender" || this.id == "ddPTLocation") {
                if ($('#' + this.id).val() == "00000000-0000-0000-0000-000000000000") {
                    $app.showAlert(this.id == "ddCategory" ? 'Please Select Category' : this.id == "empGender" ? 'Please Select Gender' : this.id == "ddESILocation" ? 'Please Select ESILocation' : this.id == "ddPTLocation" ? 'Please Select PTLocation' : 'Please Select ESIDispensary', 4);
                    err = 1;
                    $('#' + this.id).focus();
                    return false;
                }
            }
            if (this.id == "empGender") {

                if ($('#' + this.id).val() == "0") {
                    $app.showAlert('Please Select Gender', 4);
                    err = 1;
                    $('#' + this.id).focus();
                    return false;
                }
            }

            if ($('#' + this.id).val() == "") {
                $app.showAlert('Please Enter ' + $(this).attr('placeholder'), 4);
                err = 1;
                $('#' + this.id).focus();
                return false;
            }

        });
        if (err == 0) {



            if (!$employee.canSave) {
                return false;
            }
            $employee.canSave = false;
            $app.showProgressModel();

            var data = $employee.BuilEmployeObject();
            $.ajax({
                url: $app.baseUrl + "Employee/SaveEmployee",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    $app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:
                            $employee.canSave = true;
                            var p = jsonResult.result;
                            $employee.selectedEmployeeId = p.empid;
                            $employee.renderEmployee(p);
                            $employee.addNewClick();
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 2);
                            break;
                        case false:
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 4);
                            $employee.canSave = true;
                            break;
                    }
                },
                complete: function () {
                    $employee.canSave = true;
                }
            });
        }
    },
    BuilEmployeObject: function () {

        var err = 0
        var retObject = {
            empid: $employee.selectedEmployeeId,
            categoryId: $($employee.formData).find('#ddCategory').val(),
            empCode: $($employee.formData).find('#txtEmpCode').val(),
            empFName: $($employee.formData).find('#txtFirstName').val(),
            empLName: $($employee.formData).find('#txtLastName').val(),
            empEmail: $($employee.formData).find('#txtEmail').val(),
            empPhone: $($employee.formData).find('#txtPhone').val(),
            empDesign: $($employee.formData).find('#ddDesignation').val(),
            empDOB: $($employee.formData).find('#txtDOB').val(),
            empDOJ: $($employee.formData).find('#txtDOJ').val(),
            empDOW: $($employee.formData).find('#txtDOW').val(),
            empConfPeriod: $($employee.formData).find('#txtConfirmationPeriod').val(),
            empConfDate: $($employee.formData).find('#txtConfirmationDate').val(),
            empSeparationDate: $($employee.formData).find('#txtSeparationDate').val(),
            empRetYears: $($employee.formData).find('#txtRetirementYears').val(),
            empRetDate: $($employee.formData).find('#txtRetirementDate').val(),
            empGender: $($employee.formData).find('#empGender').val(),
            empDepart: $($employee.formData).find('#ddDepartment').val(),
            empisMetro: $($employee.formData).find('#rbtnIsMetroYes').prop('checked') == true ? true : false,
            empBranch: $($employee.formData).find('#ddBranch').val(),
            empLocation: $($employee.formData).find('#ddLocation').val(),
            EmployeeImage: $('#EMPimg').attr('src'),
            empESILocation: $($employee.formData).find('#ddESILocation').val(),
            empESIDispensary: $($employee.formData).find('#ddESIDispensary').val(),
            empPTLocation: $($employee.formData).find('#ddPTLocation').val(),
            empCostCentre: $($employee.formData).find('#ddCostCentre').val(),
            empGrade: $($employee.formData).find('#ddGrade').val(),
            empStopPayment: $($employee.formData).find('#rbtnStopPaymentYes').prop('checked') == true ? true : false,
            empPayrollProcess: $($employee.formData).find('#rbtnPayrollProcessYes').prop('checked') == true ? true : false,
            empStatus: $($employee.formData).find('#ddStatus').val(),
            empPwdMailSend: $($employee.formData).find('#chkIsMailsend').prop('checked') == true ? true : false,
        };
        return retObject;

    },
    clearControl: function () {
         debugger
        $employee.selectedEmployeeId = null;
        $($employee.formData).find('#txtEmpCode').focus();
        $($employee.formData).find('#txtEmpCode').val('');
        $($employee.formData).find('#txtFirstName').val('');
        $($employee.formData).find('#txtLastName').val('');
        $($employee.formData).find('#txtEmail').val('');
        $($employee.formData).find('#txtPhone').val('');
        $($employee.formData).find('#ddDesignation').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#empGender').val(0);
        $($employee.formData).find('#ddDepartment').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#ddBranch').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#ddLocation').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#ddESILocation').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#ddESIDispensary').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#ddPTLocation').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#ddGrade').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#ddCostCentre').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#txtDOB').val('');
        $($employee.formData).find('#txtDOJ').val('');
        $($employee.formData).find('#txtDOW').val('');
        $($employee.formData).find('#txtConfirmationPeriod').val('');
        $($employee.formData).find('#txtConfirmationDate').val('');
        $($employee.formData).find('#txtSeparationDate').val('');
        $($employee.formData).find('#txtRetirementYears').val('');
        $($employee.formData).find('#txtRetirementDate').val('');
        $($employee.formData).find("#rbtnIsMetroYes").prop('checked', true);
        $($employee.formData).find("#rbtnStopPaymentNo").prop('checked', true);
        $($employee.formData).find("#rbtnPayrollProcessYes").prop('checked', true);
        $($employee.formData).find('#ddStatus').val(1);
        $($employee.formData).find('#ddCategory').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#ddBank').val('00000000-0000-0000-0000-000000000000');
        $($employee.formData).find('#EMPimg').attr('src', 'assets/images/profile.png');
        $('#lblEmpImgName').text("Upload Profile");
        $($employee.formData).find('#lblEmpImgName').attr('title', 'Upload Profile');
        $($employee.formData).find("#chkIsMailsend").prop('checked', false);
        //$("#chkIsMailsend").attr('disabled', true);

    },
    addNewClick: function () {
        //$employee.LoadEmployee();
        $('#btnBack').removeClass('hide');
        if ($employee.selectedEmployeeId == null || $employee.selectedEmployeeId == '') {
            $('.empTab').addClass('hide');
            $('.empTab #empDetails').removeClass('hide');
            $('#empDetails').click();
        }
        else {
            $('.empTab').removeClass('hide');
            $('#empDetails').click();
            //new code
            $employee.empRelateddynamicTab('Employee', $employee.selectedEmployeeId);
        }
    },
    empRelateddynamicTab: function (refTableName, refEntityId) {

        $.ajax({
            url: $app.baseUrl + "Entity/GetEntityModelMap",
            data: JSON.stringify({ refModelName: refTableName, refEntityId: refEntityId, entityModelId: '00000000-0000-0000-0000-000000000000' }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $employee.addAdditionaltab(jsonResult.result);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });

    },
    addAdditionaltab: function (dynamicTab) {

        var tabs = [{ id: 'Increment', display: 'Increment' }, { id: 'StopPayment', display: 'StopPayment' }, { id: 'Loan', display: 'Loan' }];
        for (var cnt = 0; cnt < dynamicTab.length; cnt++) {

            tabs.push({ id: dynamicTab[cnt].entityModelId, display: dynamicTab[cnt].DisplayAs });

        }
        for (var tabCount = 0; tabCount < tabs.length; tabCount++) {
            if ($('#empNavtabs').find('#emp_' + tabs[tabCount].id).length <= 0) {
                var b = $('<li><a href="#empOthers" data-toggle="tab" class="empTab empcssOthers" id="emp_' + tabs[tabCount].id + '">' + tabs[tabCount].display + ' </a> </li>');
                $('#empNavtabs').append(b);
            }
        }
        $('.empcssOthers').on("click", function (e, data) {
            debugger;
            var id = $(this).prop('id').trim().replace('emp_', '');
            var displaytext = $(this).text();
            $("#tabcontent").addClass('nodisp');
            switch (id) {
                case "Increment":
                    $increment.initiateForm($employee.selectedEmployeeId, 'empOthers');
                    break;
                case "StopPayment":
                    $StopPayment.initiateForm($employee.selectedEmployeeId, 'empOthers');
                    break;
                case "Loan":
                    $LoanEntry.initiateFormEmployee($employee.selectedEmployeeId, 'empOthers');
                    break;
                case "":
                    $('#empOthers').html('');
                    break;
                default:
                    $entityMapping.renderForm('Employee', $employee.selectedEmployeeId, id, displaytext);
                    break;
            }

        });

        $('#empNavtabs a').on("click", function (e) {
            $("#tabcontent").removeClass('nodisp');
        });
    },
    deleteData: function (data) {

        $employee.selectedEmployeeId = data.empid;
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteEmployee",
            data: JSON.stringify({ empId: data.empid, IsDeleteUser: data.IsDeleteUser }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $employee.LoadEmployee();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    reloadMasterData: function () {

        switch ($comPopup.selectedComponent) {
            case "branch":
                $companyCom.loadBranch({ id: 'ddBranch' });
                break;
            case "designation":
                $companyCom.loadDesignation({ id: 'ddDesignation' });
                break;
            case "location":
                $companyCom.loadLocation({ id: 'ddLocation' });
                break;
            case "costCentre":
                $companyCom.loadCostCentre({ id: 'ddCostCentre' });
                break;
            case "esiLocation":
                $companyCom.loadESILocation({ id: 'ddESILocation' });
                break;
            case "grade":
                $companyCom.loadGrade({ id: 'ddGrade' });
                break;
            case "esiDespensary":
                $companyCom.loadESIDespensary({ id: 'ddESIDispensary' });
                break;
            case "department":
                $companyCom.loadDepartment({ id: 'ddDepartment' });
                break;
            case "ptlocation":
                $companyCom.loadPTLocation({ id: 'ddPTLocation' });
                break;
            case "bank":
                $companyCom.loadBank({ id: 'ddBank' });
                break;
        }
        $comPopup.selectedComponent = '';
    },
    searchData: function (fnType) {

        if ($('#sltLookup').val() != "0") {
            switch (fnType) {
                case "search":
                    var newDataSet = [];
                    var key = $('#sltLookup').val();
                    var value = $('#txtSearchData').val();
                    if (value != "") {
                        $.each($employee.employeeList, function (index, data) {
                            var re = new RegExp(value.toUpperCase() + ".*");
                            var dataVal = data[key].toUpperCase();
                            if (dataVal.match(re)) {
                                newDataSet.push(data);
                            }
                        });
                        $('#tblemployee').dataTable().fnClearTable();
                        if (newDataSet.length > 0) {
                            $('#tblemployee').dataTable().fnAddData(newDataSet);
                        }
                    } else {
                        $('#tblemployee').dataTable().fnClearTable();
                        $('#tblemployee').dataTable().fnAddData($employee.employeeList);
                    }
                    break;
                case "clear":
                    $('#sltLookup').val(0);
                    $('#txtSearchData').val('');
                    $('#tblemployee').dataTable().fnClearTable();
                    $('#tblemployee').dataTable().fnAddData($employee.employeeList);
                    break;
            }

        }
    },
    BirthdayRender: function () {
        debugger;
        var dtacademic = $('#tblBirthday').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "Imgcode" },
                    { "data": "empName" },
                     {
                         "data": "Date",


                         render: function (data) {
                             debugger;
                             var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                             var dateE = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateE;

                         }
                     },



            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "sClass": "word-wrap",
             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                 var path = oData.Imgcode.replace('~/', '');
                 var imgpath = path == "" ? "" : $rootUrl + path;// path.substring(0, 1) == '/' ? path.substring(1) : path;
                 var b = $('<img id="imgUserProfileView" src="' + imgpath + '" class="img-circle img-inline" style="max-height:50px;max-width:50px;" />');
                 $(nTd).empty();
                 $(nTd).prepend(b);
             }

         },
         {
             "aTargets": [2],
             "sClass": "word-wrap"

         },
            {
                "aTargets": [3],
                "sClass": "word-wrap"

            },

            ],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetPayrolldashboarddatas",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Type: "BirthdayCurrentMonth", Monthvalue: 0 }),
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                debugger;
                                var out = jsonResult.result;
                                $('#iddisplay').text(jsonResult.Message);
                                $('#idHidenmonth').text(jsonResult.StatusCode);

                                setTimeout(function () {

                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });

                                }, 50);
                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    ButtonBirthdayRender: function (monthval) {
        debugger;
        var dtacademic = $('#tblBirthday').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "Imgcode" },
                    { "data": "empName" },
                     {
                         "data": "Date",


                         render: function (data) {
                             debugger;
                             var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                             var dateE = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateE;

                         }
                     },



            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "sClass": "word-wrap",
             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                 var path = oData.Imgcode.replace('~/', '');
                 var imgpath = path == "" ? "" : $rootUrl + path;// path.substring(0, 1) == '/' ? path.substring(1) : path;
                 var b = $('<img id="imgUserProfileView" src="' + imgpath + '" class="img-circle img-inline" style="max-height:50px;max-width:50px;" />');
                 $(nTd).empty();
                 $(nTd).prepend(b);
             }

         },
         {
             "aTargets": [2],
             "sClass": "word-wrap"

         },
            {
                "aTargets": [3],
                "sClass": "word-wrap"

            },

            ],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetPayrolldashboarddatas",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Type: "BirthdayDynamicMonth", Monthvalue: monthval }),
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                debugger;
                                var out = jsonResult.result;
                                $('#iddisplay').text(jsonResult.Message);
                                $('#idHidenmonth').text(jsonResult.StatusCode);

                                setTimeout(function () {

                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });

                                }, 50);
                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    TodayBirthdayRender: function (monthval) {
        debugger;
        var dtacademic = $('#tblTodayBirthday').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "Imgcode" },
                    { "data": "empName" },
                     {
                         "data": "Date",


                         render: function (data) {
                             debugger;
                             var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                             var dateE = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateE;

                         }
                     },
                     {
                         "data": null
                     },

            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "sClass": "word-wrap",
             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                 var path = oData.Imgcode.replace('~/', '');
                 var imgpath = path == "" ? "" : $rootUrl + path;// path.substring(0, 1) == '/' ? path.substring(1) : path;
                 var b = $('<img id="imgUserProfileView" src="' + imgpath + '" class="img-circle img-inline" style="max-height:50px;max-width:50px;" />');
                 $(nTd).empty();
                 $(nTd).prepend(b);
             }

         },
         {
             "aTargets": [2],
             "sClass": "word-wrap"

         },
            {
                "aTargets": [3],
                "sClass": "word-wrap"

            },
             {
                 "aTargets": [4],
                 "sClass": "actionColumn",
                 "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                     var c = $('<a href="#" id="btnAddGreetingmsg" value="Add" class="editeButton" title="Send Wishes" data-toggle="modal" data-target="#Addgreetingtxt"><span aria-hidden="true" font-size:30px" class="glyphicon glyphicon-envelope"></span>');
                     c.button();
                     c.on('click', function () {
                         debugger;

                         $('#idlblHidnType').text('Birthday');
                         $('#idlblHidnempid').text(oData.Id);
                         $('#idgreettitle').text('  Enter your Wishes for  ' + oData.empName);
                         // $('#Addgreetingtxt').showProgressModel();
                         $('#Addgreetingtxt').modal('toggle');
                         $employee.AddInitialize();

                         return false;
                     });
                     $(nTd).empty();
                     $(nTd).prepend(c);
                 }
             }

            ],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetPayrolldashboarddatas",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Type: "TodayBirthday", Monthvalue: 0 }),
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                debugger;
                                var out = jsonResult.result;
                                $('#iddisplay').text(jsonResult.Message);
                                $('#idHidenmonth').text(jsonResult.StatusCode);

                                setTimeout(function () {

                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });

                                }, 50);
                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },

    AddInitialize: function () {

    },
    SendGreetingsMail: function (employeeid, event, GreetingContent) {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Employee/SendGreetingsmail",
            data: JSON.stringify({ empId: employeeid, Event: event, messageGreetingContent: GreetingContent }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $('#Addgreetingtxt').modal('toggle');
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $('#Addgreetingtxt').modal('toggle');
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    ButtonServiceRender: function (monthval) {
        debugger;
        var dtacademic = $('#tblServiceYr').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "Imgcode" },
                    { "data": "empName" },
                     {
                         "data": "Date",

                         render: function (data) {
                             debugger;
                             var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                             var dateE = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateE;

                         }
                     },
                      { "data": "ServiceyrORAge" },


            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "sClass": "word-wrap",
             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                 var path = oData.Imgcode.replace('~/', '');
                 var imgpath = path == "" ? "" : $rootUrl + path;// path.substring(0, 1) == '/' ? path.substring(1) : path;
                 var b = $('<img id="imgUserProfileView" src="' + imgpath + '" class="img-circle img-inline" style="max-height:50px;max-width:50px;" />');
                 $(nTd).empty();
                 $(nTd).prepend(b);
             }

         },
         {
             "aTargets": [2],
             "sClass": "word-wrap"

         },
            {
                "aTargets": [3],
                "sClass": "word-wrap"

            },
             {
                 "aTargets": [4],
                 "sClass": "word-wrap"

             }



            ],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetPayrolldashboarddatas",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Type: "ServiceDynamicMonth", Monthvalue: monthval }),
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                debugger;
                                var out = jsonResult.result;
                                $('#idServicedisplay').text(jsonResult.Message);
                                $('#idServiceHidenmonth').text(jsonResult.StatusCode);

                                setTimeout(function () {

                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });

                                }, 50);
                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },

    ServiceyearRender: function () {

        var dtacademic = $('#tblServiceYr').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "Imgcode" },
                    { "data": "empName" },
                     {
                         "data": "Date",

                         render: function (data) {
                             debugger;
                             var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                             var dateE = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateE;

                         }
                     },
                      { "data": "ServiceyrORAge" },


            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "sClass": "word-wrap",
             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                 var path = oData.Imgcode.replace('~/', '');
                 var imgpath = path == "" ? "" : $rootUrl + path;// path.substring(0, 1) == '/' ? path.substring(1) : path;
                 var b = $('<img id="imgUserProfileView" src="' + imgpath + '" class="img-circle img-inline" style="max-height:50px;max-width:50px;" />');
                 $(nTd).empty();
                 $(nTd).prepend(b);
             }

         },
         {
             "aTargets": [2],
             "sClass": "word-wrap"

         },
            {
                "aTargets": [3],
                "sClass": "word-wrap"

            },
             {
                 "aTargets": [4],
                 "sClass": "word-wrap"

             }



            ],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetPayrolldashboarddatas",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Type: "ServiceCurrentMonth", Monthvalue: 0 }),
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                debugger;
                                var out = jsonResult.result;
                                $('#idServicedisplay').text(jsonResult.Message);
                                $('#idServiceHidenmonth').text(jsonResult.StatusCode);

                                setTimeout(function () {

                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });

                                }, 50);
                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    getAppSetting: function (setting) {
        if ($employee.EmpcodeAutoManual == "1") {
            var categoryid = $('#ddCategory').val();
            $.ajax({
                url: $app.baseUrl + "Employee/GetAppSetting",
                data: JSON.stringify({ categoryId: categoryid, setting: setting }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    switch (jsonResult.Status) {
                        case true:

                            var p = jsonResult.result;
                            break;
                        case false:
                            $('#ddCategory').val('00000000-0000-0000-0000-000000000000');
                            alert(jsonResult.Message);
                            break;
                    }
                },
                complete: function () {

                }


            });
        }
    }
};