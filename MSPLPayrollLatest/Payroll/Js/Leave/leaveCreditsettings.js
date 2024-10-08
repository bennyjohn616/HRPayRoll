function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
        return false;
    else {
        var len = document.getElementById("txtNoOfDays").value.length;
        var index = document.getElementById("txtNoOfDays").value.indexOf('.');

        if (index > 0 && charCode == 46) {
            return false;
        }
        if (index > 0) {
            charCode
            var CharAfterdot = (len + 1) - index;
            if (CharAfterdot > 2) {
                return false;
            }
            if (charCode != 53) {
                return false;
            }
        }

    }
    return true;
}

function MonthlylimitVal() {
    if ($("#txtNoOfDays").val() > 31) {
        $app.showAlert('Invalid Data!', 4);
        $("#txtNoOfDays").val('');
        return false;
    }
}
function MonthlylimitVal() {
    if ($("#txtcrdays").val() > 31) {
        $app.showAlert('Invalid Data!', 4);
        $("#txtcrdays").val('');
        return false;
    }
}

$("#sltRCategorylist").change(function () {
    debugger;
    if ($('#sltRCategorylist').val() == "00000000-0000-0000-0000-000000000000") {
        $('#ddLeaveTypeCP').val('00000000-0000-0000-0000-000000000000');
    }
    else {
        $LeaveCreditsettings.loadLeaveType('ddLeaveTypeCP');
    }


});
$("#sltCategory").change(function () {
    debugger;
    var CatNameRowsDateCheck = $("#tblCreditleavesettings").dataTable().fnGetNodes();
    for (i = 0; i < CatNameRowsDateCheck.length; i++) {
        if ($(CatNameRowsDateCheck[i]).find("td:nth-child(5)").html().trim().toLowerCase() == $('#ddLeaveType').find('option:selected').text().trim().toLowerCase() && $(CatNameRowsDateCheck[i]).find("td:nth-child(3)").html().trim().toLowerCase() == $('#sltCategory').find('option:selected').text().trim().toLowerCase()) {
            $app.showAlert(" This category (" + $('#sltCategory').find('option:selected').text().trim() + ") and Leave Type(" + $('#ddLeaveType').find('option:selected').text().trim() + ") Combination Already Exist ", 4);
            $('#sltCategory').val('00000000-0000-0000-0000-000000000000');
            $("#sltCategory").focus();
            return false;
        }
    }
});
$("#ddLeaveType").change(function () {
    debugger;
    if ($('#ddLeaveType').find('option:selected').text() == "LOSS OF PAY DAYS") {
        $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
        $app.showAlert('You cannot set Leave Credits for LOSS OF PAY DAYS', 4);
        return false;
    }
    if ($('#ddLeaveType').find('option:selected').text() == "ONDUTY") {
        $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
        $app.showAlert('You cannot set Leave Credits for ONDUTY', 4);
        return false;
    }
    var CatNameRowsDateCheck = $("#tblCreditleavesettings").dataTable().fnGetNodes();
    for (i = 0; i < CatNameRowsDateCheck.length; i++) {
        if ($(CatNameRowsDateCheck[i]).find("td:nth-child(5)").html().trim().toLowerCase() == $('#ddLeaveType').find('option:selected').text().trim().toLowerCase() && $(CatNameRowsDateCheck[i]).find("td:nth-child(3)").html().trim().toLowerCase() == $('#sltCategory').find('option:selected').text().trim().toLowerCase()) {
            $app.showAlert(" This category (" + $('#sltCategory').find('option:selected').text().trim() + ") and Leave Type(" + $('#ddLeaveType').find('option:selected').text().trim() + ") Combination Already Exist ", 4);
            $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
            $("#ddLeaveType").focus();
            return false;
        }
    }


});

$("#chkDefaultLEVCR").on('click', function () {
    debugger;

    var Montprocesschk = document.getElementById("chkDefaultLEVCR").checked;
    if (Montprocesschk == true) {
        document.getElementById('txtNoOfDays').readOnly = true;
        $("#chkDefaultLEVCR").val(0);
    }
    else {
        document.getElementById('txtNoOfDays').readOnly = false;
        $("#chkDefaultLEVCR").val("");
    }
});
$("#btnProcess").on('click', function () {
    debugger;
    var flg = true;
    if ($('#sltRCategorylist').val() == "00000000-0000-0000-0000-000000000000") {
        $app.showAlert('Please Select Category!!!', 4);
        flg = false;
    }
    if ($('#ddLeaveTypeCP').val() == "00000000-0000-0000-0000-000000000000") {
        flg = false;
        $app.showAlert('Please Select Leave Type!', 4);
    }
    if ($("#txtProcessDate").val() == "") {
        flg = false;
        $app.showAlert('Please Select the Process date!!!', 4);
    }
    else {
        var NXTDT = new Date($("#NextProcessDate").val());
        var LSTDT = new Date($("#txtLastProcessDate").val());
        var CURDT = new Date($("#txtProcessDate").val());
        if (NXTDT > CURDT) {
            flg = false;
            $app.showAlert('Please Select the Valid ProcessDate!!!', 4);
        }
    }
    if (flg == true) {

        $LeaveCreditsettings.CreditProcess();

    }
});
$("#btnCreditsave").unbind().on('click', function () {
    debugger;
    var flg = true;
    if ($('#sltCategory').val() == "00000000-0000-0000-0000-000000000000") {
        $app.showAlert('Please Select Category!!!', 4);
        flg = false;
    }

    if ($('#ddLeaveType').val() == "00000000-0000-0000-0000-000000000000") {
        flg = false;
        $app.showAlert('Please Select Leave Type!', 4);
    }



    if ($("#ddLeaveCreditType").val() == "days") {
        if ($("#txtNoOfDays").val() == "") {
            flg = false;
            $app.showAlert('Please Enter the Leave Credit calculation after(Days)!!!', 4);
        }
        else {
            var txtval = parseInt($("#txtNoOfDays").val(), 10);
            if (txtval == "0") {
                flg = false;
                $app.showAlert('Please Enter the Leave Credit calculation after(Days),Zero is not valid!!!', 4);
            }
        }

    }

    if ($("#txtcrdays").val() == "") {
        flg = false;
        $app.showAlert('Please Enter the Credit Days!!!', 4);
    }
    else {
        var txtval1 = parseInt($("#txtcrdays").val(), 10);
        if (txtval1 == "0") {
            flg = false;
            $app.showAlert('Please Enter the Credit Days,Zero is not valid!!!', 4);
        }
    }
    if ($("#txtmidmontdate").val() == "") {
        flg = false;
        $app.showAlert('Please Enter the Credit for employees joining in the mid of month will be calculated before !!!', 4);
    }
    else {
        var txtval2 = parseInt($("#txtmidmontdate").val(), 10);
        if (txtval2 == "0") {
            flg = false;
            $app.showAlert('Please Enter the Credit for employees joining in the mid of month will be calculated before,Zero is not valid!!!', 4);
        }
    }
    if ($("#txtEffectiveDate").val() == "") {
        flg = false;
        $app.showAlert('Please Select the Credit Effective from date!!!', 4);
    }



    if (flg == true) {

        $LeaveCreditsettings.Creditsettingsave();

    }

});

//$("#ddlLCDynamic").on('click', function () {
//    debugger;
//    if ($("#ddLCLeaveType").val() == "00000000-0000-0000-0000-000000000000")
//    {
//        $("#ddlLCDynamic").val("select");
//    }

//});

$('#ddLeaveType').change(function () {

    if ($('#ddLeaveType').find('option:selected').text() == "LOSS OF PAY") {
        $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
        $app.showAlert('You cannot set Monthly leave Limit for LOSS OF PAY DAYS', 4);
    }
    //if ($('#ddLeaveType').find('option:selected').text() == "ONDUTY") {
    //    $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
    //    $app.showAlert('You cannot set Monthly leave Limit for ONDUTY', 4);
    //}

});

var $LeaveCreditsettings = {

    LoadCreditLeavesettings: function () {
        var dtMothlyLeaveLimit = $('#tblCreditleavesettings').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            "oLanguage": {
                "sEmptyTable": "No Data Avaliable"
            },
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "CategoryTypeId" },
                    { "data": "CategoryType" },
                    { "data": "LeaveTypeId" },
                    { "data": "LeaveType" },
                    {

                        "data": "Effectivedate",
                        render: function (data) {

                            var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                            var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                            return dateF;
                        }
                    },
                    { "data": "Rotationdays" },
                    { "data": "CrDays" },
                    { "data": "midmontdate" },
                    { "data": null }
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
              "sClass": "nodisp",
              "bSearchable": false
          },
         {
             "aTargets": [4],
             "sClass": "word-wrap"

         },
         {
             "aTargets": [5],
             "sClass": "word-wrap"

         },
          {
              "aTargets": [6],
              "sClass": "word-wrap"

          },
           {
               "aTargets": [7],
               "sClass": "word-wrap"

           },
           {
               "aTargets": [8],
               "sClass": "word-wrap"

           },

         {
             "aTargets": [9],
             "sClass": "actionColumn",

             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                 var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                 c.button();
                 c.on('click', function () {
                     if (confirm('Are you sure ,do you want to delete?')) {
                         $LeaveCreditsettings.DeleteData(oData);
                     }
                     return false;
                 });
                 $(nTd).empty();
                 $(nTd).prepend(c);
             }
         }
            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetCreditdayssettings",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {

                        var out = jsonResult.result;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
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
            //fnInitComplete: function (oSettings, json) {

            //},
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },


    weekoffScreenSetting: function () {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Leave/getleavemastersettings",
            data: null,
            dataType: "json",
            contentType: "application/json",
            //contentType: "json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                //$app.clearSession(jsonResult);
                var p = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:
                        debugger;

                        document.getElementById('lblWeekparam').innerHTML = p[0].Weekoffparameter;
                        document.getElementById('lblWeekEntval').innerHTML = p[0].Weekoffentryvalid;

                        if (p[0].Weekoffentryvalid == "C") //CUTOFF DATE
                        {
                            debugger;
                            if (p[0].Weekoffparameter == "companywise")//CUTOFF DATE WISE WITH  companywise
                            {
                                $("#divComponent").addClass("nodisp");
                                $("#divCutoff").removeClass("nodisp");
                                $("#divmonthddl").addClass("nodisp");
                                $("#Empweekoffgrid").removeClass("nodisp");
                                $("#Companyweekoffgrid").addClass("nodisp");

                            }
                            else if (p[0].Weekoffparameter == "employeewise")//CUTOFF DATE WISE WITH  employeewise
                            {
                                $("#divComponent").removeClass("nodisp");
                                $("#divCutoff").removeClass("nodisp");
                                $("#divmonthddl").addClass("nodisp");
                                $("#Empweekoffgrid").removeClass("nodisp");
                                $("#Companyweekoffgrid").addClass("nodisp");
                                $("#WeekoffCom").val(p[0].Weekoffparameter);
                                $("#WeekoffCom").attr("disabled", "true");
                            }
                            else//CUTOFF DATE WISE WITH  Otherfields
                            {
                                $("#divComponent").removeClass("nodisp");
                                $("#divCutoff").removeClass("nodisp");
                                $("#divmonthddl").addClass("nodisp");
                                $("#Empweekoffgrid").removeClass("nodisp");
                                $("#Companyweekoffgrid").addClass("nodisp");
                                $("#WeekoffCom").val(p[0].Weekoffparameter);
                                $("#WeekoffCom").attr("disabled", "true");
                            }



                        }
                        else if (p[0].Weekoffentryvalid == "M") //MONTH WISE
                        {
                            debugger;
                            if (p[0].Weekoffparameter == "companywise")//MONTH WISE WITH  companywise
                            {
                                $("#divCutoff").addClass("nodisp");
                                $("#divmonthddl").removeClass("nodisp");
                                $("#divComponent").addClass("nodisp");
                                $("#Empweekoffgrid").addClass("nodisp");
                                $("#Companyweekoffgrid").removeClass("nodisp");
                            }
                            else if (p[0].Weekoffparameter == "employeewise")//MONTH WISE WITH  employeewise
                            {
                                $("#divCutoff").addClass("nodisp");
                                $("#divmonthddl").removeClass("nodisp");
                                $("#WeekoffCom").val(p[0].Weekoffparameter);
                                $("#WeekoffCom").attr("disabled", "true");
                                $("#Empweekoffgrid").removeClass("nodisp");
                                $("#Companyweekoffgrid").addClass("nodisp");
                            }
                            else//MONTH WISE WITH  Otherfields
                            {
                                $("#divCutoff").addClass("nodisp");
                                $("#divmonthddl").removeClass("nodisp");
                                $("#Empweekoffgrid").addClass("nodisp");
                                $("#WeekoffCom").val(p[0].Weekoffparameter);
                                $("#WeekoffCom").attr("disabled", "true");
                            }
                        }
                        else//YEAR WISE
                        {
                            debugger;
                            if (p[0].Weekoffparameter == "companywise")//YEAR WISE WITH  companywise
                            {
                                $("#divCutoff").addClass("nodisp");
                                $("#divmonthddl").addClass("nodisp");
                                $("#Empweekoffgrid").addClass("nodisp");
                                $("#Companyweekoffgrid").removeClass("nodisp");
                                $("#divComponent").addClass("nodisp");

                            }
                            else if (p[0].Weekoffparameter == "employeewise")//YEAR WISE WITH  employeewise
                            {
                                $("#divCutoff").addClass("nodisp");
                                $("#divmonthddl").addClass("nodisp");
                                $("#WeekoffCom").val(p[0].Weekoffparameter);
                                $("#WeekoffCom").attr("disabled", "true");
                                $("#Empweekoffgrid").removeClass("nodisp");
                                $("#Companyweekoffgrid").addClass("nodisp");
                            }
                            else//YEAR WISE WITH  Otherfields
                            {
                                $("#divCutoff").addClass("nodisp");
                                $("#divmonthddl").addClass("nodisp");
                                $("#WeekoffCom").val(p[0].Weekoffparameter);
                                $("#WeekoffCom").attr("disabled", "true");
                                $("#Empweekoffgrid").addClass("nodisp");
                                $("#Companyweekoffgrid").removeClass("nodisp");
                            }
                        }

                        break;
                    case false:
                        debugger;

                        break;
                }
            },
            complete: function () {

            }
        });
    },
    LoadEncashmentlevDDL: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Leave/GetEncashmentlevtypeDropdown",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var empid = $('#hdnEmployeeId').val();
                var sessionempid = msg.Message;
                var out = msg.result;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('select').html('--Select--'));
                $.each(out, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.name));
                });
            },
            error: function (msg) {
            }
        });

    },




    LoadDropdowns: function (dropdownIds) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Leave/GetLeavemasterDropdown",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var empid = $('#hdnEmployeeId').val();
                var sessionempid = msg.Message;
                var out = msg.result;
                $.each(dropdownIds, function (i, id) {
                    $('#' + id).html('');
                    $('#' + id).append($("<option></option>").val('companywise').html('Company Wise'));
                    if (id == "ddlWKoffSet") {
                        $('#' + id).append($("<option></option>").val('employeewise').html('Employee Wise'));
                    }
                    $('#' + id).append($("<option></option>").val('category').html('Category'));
                    $('#' + id).append($("<option></option>").val('branch').html('Branch'));
                    $('#' + id).append($("<option></option>").val('designation').html('Designation'));
                    $('#' + id).append($("<option></option>").val('costCentre').html('Cost Centre'));
                    $('#' + id).append($("<option></option>").val('esiLocation').html('ESI Location'));
                    $('#' + id).append($("<option></option>").val('grade').html('Grade'));
                    $('#' + id).append($("<option></option>").val('esiDespensary').html('ESI Dispensary'));
                    $('#' + id).append($("<option></option>").val('department').html('Department'));
                    $('#' + id).append($("<option></option>").val('location').html('Location'));
                    $('#' + id).append($("<option></option>").val('ptlocation').html('PT Location'));
                    $('#' + id).append($("<option></option>").val('bank').html('Bank'));
                    $('#' + id).append($("<option></option>").val('leaveType').html('Leave Type'));
                    $('#' + id).append($("<option></option>").val('languagesknown').html('Languages Known'));
                    $.each(out, function (index, blood) {
                        $('#' + id).append($("<option></option>").val(blood.name).html(blood.name));
                    });
                });
            },
            error: function (msg) {
            }
        });

    },

    LoadREPORTDropdowns: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetLeavemasterDropdown",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var empid = $('#hdnEmployeeId').val();
                var sessionempid = msg.Message;
                var out = msg.result;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('select').html('--Select--'));
                $('#' + dropControl.id).append($("<option></option>").val('branch').html('Branch'));
                $('#' + dropControl.id).append($("<option></option>").val('designation').html('Designation'));
                $('#' + dropControl.id).append($("<option></option>").val('costCentre').html('Cost Centre'));
                $('#' + dropControl.id).append($("<option></option>").val('esiLocation').html('ESI Location'));
                $('#' + dropControl.id).append($("<option></option>").val('grade').html('Grade'));
                $('#' + dropControl.id).append($("<option></option>").val('esiDespensary').html('ESI Dispensary'));
                $('#' + dropControl.id).append($("<option></option>").val('department').html('Department'));
                $('#' + dropControl.id).append($("<option></option>").val('location').html('Location'));
                $('#' + dropControl.id).append($("<option></option>").val('ptlocation').html('PT Location'));
                $('#' + dropControl.id).append($("<option></option>").val('bank').html('Bank'));
                $('#' + dropControl.id).append($("<option></option>").val('leaveType').html('Leave Type'));
                $('#' + dropControl.id).append($("<option></option>").val('languagesknown').html('Languages Known'));
                $.each(out, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.name).html(blood.name));
                });
            },
            error: function (msg) {
            }
        });

    },

    LoadLeaveMasterSettings: function () {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Leave/getleavemastersettings",
            data: null,
            dataType: "json",
            contentType: "json",
            //contentType: "application/json",
            type: "POST",
            async: false,
            success: function (msg) {
                debugger;
                var p = msg.result;
                switch (msg.Status) {
                    case true:
                        debugger;
                        $("#ddlLevSet").val(p[0].leaveparameter),
                        $("#ddlHolSet").val(p[0].Holidayparameter),
                        $("#ddlCPoffSet").val(p[0].Compoffparameter),
                        $("#ddlWKoffSet").val(p[0].Weekoffparameter),
                        $("#ddlWKoffEntrySet").val(p[0].Weekoffentryvalid),
                        $("#ddlCOMP1").val(p[0].RpComp1),
                        $("#ddlCOMP2").val(p[0].RpComp2),
                        $("#ddlCOMP3").val(p[0].RpComp3),
                        $("#ddlCOMP4").val(p[0].RpComp4),
                        $("#ddlCOMP5").val(p[0].RpComp5),
                        $("#ddlLevCreditSet").val(p[0].leavecreditparameter),
                        $("#ddlEncashmentSet").val(p[0].encashmentparameter)
                        if (p[0].btnSaveEnable == false) {
                            $("#LeaveMastersetdiv").addClass("nodisp");
                            $("#dvMasterSettings *").prop('disabled', true);
                        }
                        if (p[0].Minormaxparameter == "C") {
                            $("#ddlminmaxval").val("C");
                            $("#idchkmaxmin").removeClass("nodisp");
                            if (p[0].mindays == 0 && p[0].maxmintimes == 0) //MIN SETTINGS RETRIVE
                            {
                                $('#IdMinValue').prop('checked', false);
                                $("#divminid").addClass("nodisp");
                            }
                            else {
                                $('#IdMinValue').prop('checked', true);
                                $("#divminid").removeClass("nodisp");
                                $("#txtMinValue").val(p[0].mindays);
                                if (p[0].maxmintimes == 0) {
                                    $("#txtMinTimes").val("");
                                }
                                else {
                                    $("#txtMinTimes").val(p[0].maxmintimes);
                                    $("#ddlMinsetperiod").val(p[0].minperiod);

                                    if (p[0].minperiod == "M") {
                                        $("#idmintimesallowdeviation").removeClass("nodisp");
                                        $("#ddlMindeviation").val(p[0].mindeviation);
                                    }



                                }
                            }

                            if (p[0].maxdays == 0 && p[0].maxmaxtimes == 0) //MAX SETTINGS RETRIVE
                            {
                                $('#IdMaxValue').prop('checked', false);
                                $("#divmaxid").addClass("nodisp");
                            }
                            else {
                                $('#IdMaxValue').prop('checked', true);
                                $("#divmaxid").removeClass("nodisp");
                                $("#txtMaxValue").val(p[0].maxdays);
                                if (p[0].maxmaxtimes == 0) {
                                    $("#txtMaxTimes").val("");
                                }
                                else {
                                    $("#txtMaxTimes").val(p[0].maxmaxtimes);
                                    $("#ddlMaxsetperiod").val(p[0].maxperiod);
                                    if (p[0].maxperiod == "M") {
                                        $("#idmaxtimesallowdeviation").removeClass("nodisp");
                                        $("#ddlMaxdeviation").val(p[0].maxdeviation);
                                    }

                                }
                            }
                        }
                        else {
                            $("#ddlminmaxval").val("P");
                            $("#idchkmaxmin").addClass("nodisp");
                            $("#divminid").addClass("nodisp");
                            $("#divmaxid").addClass("nodisp");
                        }

                        break;
                    case false:

                        break;
                }
            },
            error: function () {
                debugger;
            }
        });

    },

    LoadDYNAMICConfiguration: function (dropControl) {
        debugger;
        //var type = 'LeaveParameter';
        var type = $('#lblhiddenparameter').text();
        $.ajax({
            url: $app.baseUrl + "Leave/GetDYNAMICconfigurationsettings",
            data: JSON.stringify({ parametertype: type }),
            dataType: "json",
            //contentType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (msg) {
                debugger;
                var out = msg.result;
                switch (msg.Status) {
                    case true:
                        debugger;
                        $('#' + dropControl.id1).empty();
                        $('#' + dropControl.id1).append($("<option></option>").val('select').html('--Select--'));
                        $.each(out, function (index, blood) {
                            $('#' + dropControl.id1).append($("<option></option>").val(blood.Id).html(blood.name));
                        });

                        if (dropControl.id1 == "ddlLCDynamic") {
                            $('#lblLCdynamic').html(msg.Message);
                            var date1 = new Date(parseInt(out[0].FinYearStart.replace(/(^.*\()|([+-].*$)/g, '')))
                            var dateF = date1.getDate() + '/' + $payroll.GetMonthName((date1.getMonth() + 1)) + '/' + date1.getFullYear();
                            var date2 = new Date(parseInt(out[0].FinYearEnd.replace(/(^.*\()|([+-].*$)/g, '')))
                            var dateT = date2.getDate() + '/' + $payroll.GetMonthName((date2.getMonth() + 1)) + '/' + date2.getFullYear();
                            $('#DPLCfinstartdate').val(dateF);
                            $('#DPLCfinenddate').val(dateT);
                        }
                        else if (dropControl.id1 == "ddlCreditParameter") {
                            $('#lblCreditParameter').html(msg.Message);
                        }
                        else if (dropControl.id1 == "ddlencashmentParameter") {
                            $('#lblencashmentParameter').html(msg.Message);
                        }
                        else {
                            $('#idcompoffdynlabel').html(msg.Message);
                        }
                        if (msg.Message == "COMPANYWISE") {
                            $('#' + dropControl.id2).addClass("nodisp");
                        }

                        if (out[0].Minormaxparameter == "C") {
                            $('#dividminormaxsettingtab').addClass("nodisp");

                        }
                        else {
                            $('#dividminormaxsettingtab').removeClass("nodisp");
                        }



                        break;
                    case false:

                        break;
                }
            },
            error: function () {
                debugger;
            }
        });

    },

    LoadComponentvalueforWeekoff: function (dropControl) {
        debugger;
        var type = 'WeekoffParameter';
        $.ajax({
            url: $app.baseUrl + "Leave/GetDYNAMICconfigurationsettings",
            data: JSON.stringify({ parametertype: type }),
            dataType: "json",
            //contentType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (msg) {
                debugger;
                var out = msg.result;
                switch (msg.Status) {
                    case true:
                        debugger;
                        $('#' + dropControl.id).empty();
                        $('#' + dropControl.id).append($("<option></option>").val('select').html('--Select--'));
                        $.each(out, function (index, blood) {
                            $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.name));
                        });
                        break;
                    case false:

                        break;
                }
            },
            error: function () {
                debugger;
            }
        });

    },
    LoadComponentmatchingFields: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Leave/GetComponentmatchingforleaveconfig",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var empid = $('#hdnEmployeeId').val();
                var sessionempid = msg.Message;
                var out = msg.result;
                $('#' + dropControl.id1).empty();
                $('#' + dropControl.id2).empty();
                $('#' + dropControl.id3).empty();
                $('#' + dropControl.id1).append($("<option></option>").val('select').html('--Select--'));
                $('#' + dropControl.id2).append($("<option></option>").val('select').html('--Select--'));
                $('#' + dropControl.id3).append($("<option></option>").val('select').html('--Select--'));
                $.each(out, function (index, blood) {
                    $('#' + dropControl.id1).append($("<option></option>").val(blood.Id).html(blood.name));
                    $('#' + dropControl.id2).append($("<option></option>").val(blood.Id).html(blood.name));
                    $('#' + dropControl.id3).append($("<option></option>").val(blood.Id).html(blood.name));
                });
            },
            error: function (msg) {
            }
        });

    },
    EncashmentComponentmatchingFields: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Leave/GetComponentmatchingforleaveconfig",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var empid = $('#hdnEmployeeId').val();
                var sessionempid = msg.Message;
                var out = msg.result;
                $('#' + dropControl.id).empty();
                $('#' + dropControl.id).append($("<option></option>").val('select').html('--Select--'));
                $.each(out, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.name));
                });
            },
            error: function (msg) {
            }
        });

    },
    LoadLeaveconfigdetails: function (Leaveid, compvalueid) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Leave/Getleaveconfigdetails",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ LeaveTypeId: Leaveid, DynamicTypeId: compvalueid }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        var p = jsonResult.result;
                        $("#ddLCLeaveType").val(p[0].LeaveTypeId);
                        $("#ddlLCDynamic").val(p[0].DynamicComponentValue);
                        var date = new Date(parseInt(p[0].ConfigEffectiveDate.replace(/(^.*\()|([+-].*$)/g, '')))
                        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                        $("#DPLCEffdate").val(dateF);
                        $("#txtLCMaxMonthDays").val(p[0].MaxDayMonth);
                        $("#ddlLCMonthDeviation").val(p[0].AllowDevisionMonth);
                        $("#txtLCMaxdays").val(p[0].overallMax);
                        $("#txtLCCaryFwrdLmt").val(p[0].carryLimit);

                        $("#ddlLcCompoffAllow").val(p[0].Compoffallow);
                        $("#btnSaveLeaveconfigSettings").addClass("nodisp");

                        //Intervening Holiday checking
                        $("#ddlLCInterVening").val(p[0].InvHoliday);
                        if (p[0].InvHoliday == "Y") {
                            $("#divIntervApplic").removeClass("nodisp");
                            $("#ddlIntervApplic").val(p[0].InvHolidaysubparameter);
                        }
                        else {
                            $("#divIntervApplic").addClass("nodisp");
                        }

                        //File attachement checking
                        $("#ddlLCAttachmentreq").val(p[0].Isattachreq);
                        if (p[0].Isattachreq == "Y") {
                            $("#txtLCAttachreqdays").val(p[0].Attachreqmaxdays);
                            $("#divAttachNoofdays").removeClass("nodisp");
                        }
                        else {
                            $("#txtLCAttachreqdays").val("");
                            $("#divAttachNoofdays").addClass("nodisp");
                        }

                        //MIN SETTINGS RETRIVE
                        if (p[0].mindays == 0 && p[0].maxmintimes == 0) {
                            $('#IdPMinValue').prop('checked', false);
                            $("#divPminid").addClass("nodisp");
                        }
                        else {
                            $('#IdPMinValue').prop('checked', true);
                            $("#divPminid").removeClass("nodisp");
                            $("#txtPMinValue").val(p[0].mindays);
                            if (p[0].maxmintimes == 0) {
                                $("#txtPMinTimes").val("");
                            }
                            else {
                                $("#txtPMinTimes").val(p[0].maxmintimes);
                                $("#ddlPMinsetperiod").val(p[0].minperiod);

                                if (p[0].minperiod == "M") {
                                    $("#idPmintimesallowdeviation").removeClass("nodisp");
                                    $("#ddlPMindeviation").val(p[0].mindeviation);
                                }
                            }
                        }

                        //MAX SETTINGS RETRIVE
                        if (p[0].maxdays == 0 && p[0].maxmaxtimes == 0) {
                            $('#IdPMaxValue').prop('checked', false);
                        }
                        else {
                            $('#IdPMaxValue').prop('checked', true);
                            $("#divPmaxid").removeClass("nodisp");
                            $("#txtPMaxValue").val(p[0].maxdays);
                            if (p[0].maxmaxtimes == 0) {
                                $("#txtPMaxTimes").val("");
                            }
                            else {
                                $("#txtPMaxTimes").val(p[0].maxmaxtimes);
                                $("#ddlPMaxsetperiod").val(p[0].maxperiod);
                                if (p[0].maxperiod == "M") {
                                    $("#idPmaxtimesallowdeviation").removeClass("nodisp");
                                    $("#ddlPMaxdeviation").val(p[0].maxdeviation);
                                }

                            }
                        }

                        break;
                    case false:
                        $LeaveCreditsettings.ClearLeaveConfig();
                        $("#btnSaveLeaveconfigSettings").removeClass("nodisp");
                        $("#txtLCAttachreqdays").val("");
                        $("#divAttachNoofdays").addClass("nodisp");
                        $("#ddlLCAttachmentreq").val("select");
                        $('#IdPMaxValue').prop('checked', false);
                        $('#IdPMinValue').prop('checked', false);
                        $("#ddlPMaxsetperiod").val("0");
                        $("#ddlPMinsetperiod").val("0");
                        $("#ddlPMaxdeviation").val("0");
                        $("#ddlPMindeviation").val("0");
                        $("#txtPMaxTimes").val("");
                        $("#txtPMinTimes").val("");
                        $("#txtPMaxValue").val("");
                        $("#txtPMinValue").val("");
                        $("#divPmaxid").addClass("nodisp");
                        $("#divPminid").addClass("nodisp");
                        $("#divIntervApplic").addClass("nodisp");
                        $("#ddlLCInterVening").val("select");
                        break;
                }
            },
            error: function (msg) {
            }
        });

    },

    CreditProcess: function () {
        $app.showProgressModel();
        debugger;
        $.ajax({
            url: $app.baseUrl + "Leave/LeaveCreditProcess",
            data: JSON.stringify({ CategoryTypeId: $('#sltRCategorylist').val(), LeaveTypeId: $('#ddLeaveTypeCP').val(), ProcessDate: $('#txtProcessDate').val(), LastProcessDate: $('#txtLastProcessDate').val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddUser').modal('toggle');
                        //$User.LoadUser();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        //alert(jsonResult.Message);
                        var p = jsonResult.result;
                        companyid = 0;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });

    },
    saveCompoffsettings: function () {
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Leave/SaveCompOffSettings",
            data: JSON.stringify({ compOffparameter: $('#ddlCompDynamicfield').val(), Compoffdays: $('#txtCoCVD').val(), Compoffdate: $('#dtpLVD').val(), Compoffvalidtype: $('#ddlCosCompoffFor').val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var table = $("#tblCompOffSettings").DataTable();
                        table.ajax.reload();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        AddInitialize();

                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }

        });
    },
    SelectCompoffsettings: function () {
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Leave/SelectCompOffSettings",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        break;
                    case false:
                        $app.hideProgressModel();


                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }

        });
    },
    Creditsettingsave: function () {
        debugger;
        $app.showProgressModel();
        var data = {
            CategoryTypeId: $('#ddlCreditParameter').val(),
            CategoryType: $('#ddlCreditParameter').find('option:selected').text(),
            LeaveTypeId: $('#ddLeaveType').val(),
            LeaveType: $('#ddLeaveType').find('option:selected').text(),
            Effectivedate: $('#txtEffectiveDate').val(),
            Rotationdays: $('#txtNoOfDays').val(),
            CrDays: $('#txtcrdays').val(),
            midmontdate: $('#txtmidmontdate').val(),
            // Monthflag: document.getElementById("chkDefaultLEVCR").checked,
            LeaveCreditType: $('#ddLeaveCreditType').val(),
            leaveCreditAffect: $('#ddLeaveCreditAffect').val(),
        };
        $.ajax({
            url: $app.baseUrl + "Leave/CreditLeaveSettings",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $('#AddLeaveCreditSettings').modal('toggle');
                        $LeaveCreditsettings.LoadCreditLeavesettings();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);

                        break;
                    case false:
                        debugger;
                        if (jsonResult.result.statuserror == 1) {
                            $('#AddLeaveCreditSettings').modal('hide');
                        }
                        else {
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 4);
                            AddInitialize();
                        }

                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }

        });
    },

    DeleteData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Leave/DeleteCreditLeaveSetting",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        // $LeaveCreditsettings.LoadCreditLeavesettings();
                        var tablecrdt = $("#tblCreditleavesettings").DataTable();
                        tablecrdt.ajax.reload();
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
    loadCreditprocessDates: function (catid, levid) {

        $.ajax({
            url: $app.baseUrl + "Leave/loadCreditprocessDates",
            data: JSON.stringify({ Cattype: $('#sltRCategorylist').val(), levid: $('#ddLeaveTypeCP').val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;

                var out = jsonResult.result;

                var date = new Date(parseInt(out[0].LastProcessDate.replace(/(^.*\()|([+-].*$)/g, '')));
                var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                $("#txtLastProcessDate").val(dateF);
                var date1 = new Date(parseInt(out[0].NEXTProcessDate.replace(/(^.*\()|([+-].*$)/g, '')));
                var dateF1 = date1.getDate() + '/' + $payroll.GetMonthName((date1.getMonth() + 1)) + '/' + date1.getFullYear();
                $("#NextProcessDate").val(dateF1);

            },
            error: function (msg) {
            }
        });
    },
    loadLeaveType: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Leave/loadCreditLeavetype",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Cattype: $('#sltRCategorylist').val() }),
            dataType: "json",
            async: false,
            success: function (msg) {
                debugger;
                var sessionempid = msg.Message;
                var out = msg.result;

                if (sessionempid == "00000000-0000-0000-0000-000000000000") {
                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                    $.each(out, function (index, blood) {
                        $('#' + dropControl.id).append($("<option></option>").val(blood.LeaveTypeId).html(blood.LeaveType));
                    });
                }
                else {
                    $.each(out, function (index, blood) {
                        $('#' + dropControl.id).append($("<option></option>").val(blood.LeaveTypeId).html(blood.LeaveType));
                    });
                }

            },
            error: function (msg) {
            }
        });

    },
    loadCategory: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Leave/loadCreditCategory",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                debugger;
                var sessionempid = msg.Message;
                var out = msg.result;

                if (sessionempid == "00000000-0000-0000-0000-000000000000") {
                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                    $.each(out, function (index, blood) {
                        $('#' + dropControl.id).append($("<option></option>").val(blood.CategoryTypeId).html(blood.CategoryType));
                    });
                }
                else {
                    $.each(out, function (index, blood) {
                        $('#' + dropControl.id).append($("<option></option>").val(blood.CategoryTypeId).html(blood.CategoryType));
                    });
                }

            },
            error: function (msg) {
            }
        });

    },

    LoadLeaveCreditTypes: function (dropControl) {

        $('#' + dropControl.id).html('');
        $('#' + dropControl.id).append($("<option></option>").val('0').html('Select'));
        $('#' + dropControl.id).append($("<option></option>").val('yearly').html('Yearly'));
        $('#' + dropControl.id).append($("<option></option>").val('monthly').html('Monthly'));
        $('#' + dropControl.id).append($("<option></option>").val('quaterly').html('Quarterly'));
        $('#' + dropControl.id).append($("<option></option>").val('halfly').html('Halfly'));
        $('#' + dropControl.id).append($("<option></option>").val('days').html('Days'));
    },

    ClearLeaveConfig: function () {



        $("#DPLCEffdate").val("");
        $("#txtLCMaxMonthDays").val("");
        $("#ddlLCMonthDeviation").val("select");
        $("#ddlLCOpnReq").val("select");
        $("#txtLCMaxdays").val("");
        $("#txtLCCaryFwrdLmt").val("");
        $("#ddlLCInterVening").val("select");
        $("#txtLCMinNoOfDays").val("");
        $("#txtLCMaxNoOfDays").val("");
        $("#ddlLcCompoffAllow").val("select");
        //$("#ddlLcOBPayrollMatch").val("select");
        //$("#ddlLcCBPayrollMatch").val("select");
        $("#ddlLcULPayrollMatch").val("select");
    },
    AddInitialize: function () {

        var formData = document.forms["frmLeaveCreditSettings"];
        $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000'),
         $('#sltCategory').val('00000000-0000-0000-0000-000000000000'),
        $('#txtmidmontdate').val(''),
        $('#txtEffectiveDate').val(''),
        $('#txtNoOfDays').val(''),
        $('#txtcrdays').val('')
    },
    LoadLeaveRelated: function (context) {

        switch (context.id) {
            case "LevCompoff_Settings":
                $leaveSettings.Checkleavemastersettingexisting({ id1: 'Compoffalert', id2: 'Compoffdetails' });
                $('#lblhiddenparameter').text('CompoffParameter');
                $LeaveCreditsettings.LoadDYNAMICConfiguration({ id1: 'ddlCompDynamicfield', id2: 'idDivCompoffDyncomp' });
                //$LeaveCreditsettings.SelectCompoffsettings();
                $LeaveCreditsettings.LoadCompOffSettings();
                break;
            case "LevWeekoff_Settings":
                debugger;
                $leaveSettings.Checkleavemastersettingexisting({ id1: 'weekoffalert', id2: 'weekoffzdetails' });
                $Weekoff.weekoffExistingcheck();
                var dropdownIds = [];
                dropdownIds.push('WeekoffCom');
                $LeaveCreditsettings.LoadDropdowns(dropdownIds);
                $LeaveCreditsettings.weekoffScreenSetting();
                debugger;
                if (!$('#divComponent').hasClass("nodisp")) {
                    $LeaveCreditsettings.LoadComponentvalueforWeekoff({ id: 'WeekoffComVal' });
                }

                break;
            case "LevMaster_Settings":
                var dropdownIds = [];
                dropdownIds.push('ddlLevSet'); dropdownIds.push('ddlWKoffSet');
                dropdownIds.push('ddlHolSet'); dropdownIds.push('ddlCPoffSet');
                dropdownIds.push('ddlCOMP1'); dropdownIds.push('ddlCOMP2');
                dropdownIds.push('ddlCOMP3'); dropdownIds.push('ddlCOMP4');
                dropdownIds.push('ddlCOMP5'); dropdownIds.push('ddlLevCreditSet');
                dropdownIds.push('ddlEncashmentSet');

                $LeaveCreditsettings.LoadDropdowns(dropdownIds);
                $LeaveCreditsettings.LoadLeaveMasterSettings();
                break;
            case "LeaveEncashment_Settings":
                $('#lblhiddenparameter').text('encashmentParameter');
                $LeaveCreditsettings.LoadDYNAMICConfiguration({ id1: 'ddlencashmentParameter', id2: 'divencashmentParameter' });
                $("#ddlencashmentParameter").val("select");
                $leaveSettings.Checkleavemastersettingexisting({ id1: 'encashmentalert', id2: 'encashmentzdetails' });
                $LeaveCreditsettings.LoadEncashmentlevDDL({ id: 'ddlEncashlevtype' });
                $LeaveCreditsettings.EncashmentComponentmatchingFields({ id: 'ddlEncashcomp' });
                $("#txtEncashLimit").val("");
                break;
            case "Levave_Config":
                debugger;
                $leaveSettings.Checkleavemastersettingexisting({ id1: 'levconfigalert', id2: 'levconfigdetails' });
                $companyCom.loadLeaveType({ id: 'ddLCLeaveType' });
                $('#lblhiddenparameter').text('LeaveParameter');
                $LeaveCreditsettings.LoadDYNAMICConfiguration({ id1: 'ddlLCDynamic', id2: 'divLCdynamicload' });
                $("#ddLCLeaveType").val("00000000-0000-0000-0000-000000000000");
                $("#ddlLCDynamic").val("select");
                $LeaveCreditsettings.ClearLeaveConfig();

                break;
            case "Leavetype_Settings":
                $LeaveCreditsettings.LoadComponentmatchingFields({ id1: 'ddlLTSOBPayrollMatch', id2: 'ddlLTSCBPayrollMatch', id3: 'ddlLTSULPayrollMatch' });
                $leaveSettings.LoadLeaveTypesettings();
                $companyCom.loadMasterLeaveType({ id: 'ddlLTSLeaveType' });
                break;

            case "LevCredit_Settings":
                $leaveSettings.Checkleavemastersettingexisting({ id1: 'creditalert', id2: 'Creditzdetails' });
                $('#lblhiddenparameter').text('LeaveCreditParameter');
                $LeaveCreditsettings.LoadDYNAMICConfiguration({ id1: 'ddlCreditParameter', id2: 'divCreditParameter' });
                $LeaveCreditsettings.LoadLeaveCreditTypes({ id: 'ddLeaveCreditType' });
                $("#ddlLCDynamic").val("select");
                break;
            default:
                break;
        }
    },

    LoadCompOffSettings: function () {
        var dtClientList = $('#tblCompOffSettings').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                     { "data": "Id" }, { "data": "DynamicCompName" }, { "data": "CreditvalidityType" },
                     { "data": "Creditvaliditydays" }, { "data": "CreditvalidityDate" }, { "data": null }
            ],
            "aoColumnDefs": [
                 { "aTargets": [0], "sClass": "nodisp" },
                 { "aTargets": [1], "sClass": "word-wrap" },
                 { "aTargets": [2], "sClass": "word-wrap" },
                 { "aTargets": [3], "sClass": "word-wrap" },
                 { "aTargets": [4], "sClass": "word-wrap" },
                 {
                     "aTargets": [5], "sClass": "actionColumn",
                     "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                         var b = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                         b.button();
                         b.on('click', function () {
                             if (confirm("Are you sure, do you want to Delete?"))
                                 $LeaveCreditsettings.DeleteData(oData);
                             return false;
                         });
                         $(nTd).empty();
                         $(nTd).append(b);

                     }
                 }

            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetCompOffsettings",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
                        var out = JSON.parse(jsonResult);
                        $app.clearSession(jsonResult);
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length

                            });

                        }, 50);

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    DeleteData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Leave/DeleteComOffSettings",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var tblcompOffSettings = $('#tblCompOffSettings').DataTable();
                        tblcompOffSettings.ajax.reload();
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
};