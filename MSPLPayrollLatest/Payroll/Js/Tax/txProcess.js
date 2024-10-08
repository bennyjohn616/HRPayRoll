﻿//$(document).ready(function () {
//    $taxHistory.LoadEntityModelDrop();


//});
$('#sltMonth').change(function () {

    var month = $('#sltMonth').val();
    var sd = new Date($taxHistory.financeYear.startDate);
    var ed = new Date($taxHistory.financeYear.EndDate);
    if (ed.getMonth() + 1 >= month) {
        $('#sltYear').val(ed.getFullYear());
    }
    else {
        $('#sltYear').val(sd.getFullYear());
    }
});
$('#closepreview').click(function () {

    $taxHistory.deleteEmployeePreviewWorkSheet($("#preViewSrc").attr('src'))

});

$taxHistory = {
    selectedEntityModelname: null,
    selectedCategoryId: null,
    selectedEmpcode: null,
    currentMonthYesOrNo: null,
    selectedpayrollType: $('#sltPayrollType').val(),
    financeYear: $companyCom.getDefaultFinanceYear(),
    month: $('#sltMonth').val(),
    year: $('#sltYear').val(),
    LastLock: null,
    emplock: null,
    actual: null,
    timeinterval: null,
    processtype: null,
    EmpId: null,



    LoadPayrolltype: function () {
        debugger
        if ($('#sltPayrollType').find('option:selected').text() == "Category") {
            $companyCom.loadCategory({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "CostCentre") {
            $companyCom.loadCostCentre({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Designation") {
            $companyCom.loadDesignation({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Branch") {
            $companyCom.loadBranch({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Department") {
            $companyCom.loadDepartment({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Location") {
            $companyCom.loadLocation({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Single Employee" && $('#sltYear').val() != null && $('#sltMonth').val() != null) {
            $companyCom.loadSelectiveEmployee({ id: 'sltCategory', condi: 'empDOJ.' + $('#sltYear').val() + '.' + $('#sltMonth').val() });
            $taxHistory.selectedpayrollType = "Single Employee";
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "All Employees" && $('#sltYear').val() != null && $('#sltMonth').val() != null) {
            $taxHistory.selectedEntityModelname = $('#sltPayrollType').find('option:selected').text() + ' for the month of ' + $('#sltMonth').find('option:selected').text() + '-' + $('#sltYear').find('option:selected').text();
            $taxHistory.selectedpayrollType = "All";
            $("#dvCat").addClass('nodisp');
        }


    },

    GetDefaultfinyear: function () {
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() == "EMPLOYEE" || $taxHistory.processtype == "employee") {
            var date1 = $('#EntryDate').val();
            date2 = new Date(date1);
            $taxHistory.financeYear = $companyCom.EmployeeDefaultFinanceYear(date2);
        }
        else {
            $taxHistory.financeYear = $companyCom.getDefaultFinanceYear();
        }
    },


    GetLockLoad: function () {
        $taxHistory.LastLock = "";
        var month = $('#sltMonth').val();
        var year = $('#sltYear').val();
        var sd = new Date($taxHistory.financeYear.startDate);
        var ed = new Date($taxHistory.financeYear.EndDate);
        if (ed.getMonth() + 1 >= $taxHistory.month) {
            $('#sltYear').val(ed.getFullYear());
        }
        else {
            $('#sltYear').val(sd.getFullYear());
        }
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxDeclaration/GetLockLoad",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                month: month,
                year: year
            }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        if (out.PayrollLock == true) {
                            $taxHistory.LastLock = "Y";
                            $taxHistory.empCheck();
                            break;
                        }
                        else {
                            $taxHistory.LastLock = "";
                            $taxHistory.empCheck();
                            break;
                        }
                        $taxHistory.empCheck();
                        break;
                    case false:
                        $taxHistory.LastLock = "";
                        $taxHistory.empCheck();
                        break;
                }
            },
            error: function () {
                $taxHistory.LastLock = "";
            }
        });
    },

    empCheck: function () {
        debugger;
        if ($taxHistory.LastLock == "Y") {
            $('#btnarea').hide();
        }
        else {
            $taxHistory.emplock = "";
            $taxHistory.empCheck_1();
            if ($taxHistory.emplock == "Y") {
                $("#btnarea").hide();
            }
            else {
                $("#btnarea").show();
            }
        }

    },

    empCheck_1: function () {
        debugger;

        if ($('#hdnRoleName').val().toUpperCase() == "EMPLOYEE" || $taxHistory.processtype == "employee") {
            var edate1 = new Date($('#EntryDate').val());
            var edate = edate1.getDate();
            var emonth = edate1.getMonth() + 1;
            var eyear = edate1.getFullYear();

            var curdate = new Date();
            if (eyear == $taxHistory.year && emonth == $taxHistory.month && edate > curdate.getDate()) {
                $taxHistory.emplock = "";
            }
            else {
                $taxHistory.emplock = "Y";
            }
        }


    },


    LoadEntityModelDrop: function () {
        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltPayrollType', condi: 'Incometax' }) //Load Payroll type
        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltMonth', condi: 'Month' });
        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltYear', condi: 'Year' });
    },


    renderFieldGrid: function (context, tableprop) {
        $("#dvTitle").html('');
        $("#dvTitle").html('<h4>' + $taxHistory.selectedEntityModelname + '</h4>');
        var grid = '<table id="tbl_' + tableprop.id + '" class="table table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer">'
            + '<thead>'
            + '<tr>'
            + '<th class="nodisp">'
            + '</th>'
        for (var cnt = 1; cnt < context.length; cnt++) {
            grid = grid + '<th>' + context[cnt].tableHeader + '</th>'
        }
        grid = grid + '</tr></thead>';
        grid = grid + '<tbody><tr>'
        for (var cnt = 0; cnt < context.length; cnt++) {//for action td 
            grid = grid + '<td></td>';
        }
        grid = grid + '</tr></tbody></table>';

        if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000" || $taxHistory.selectedpayrollType == "All") {
            $("#dvDynamicEntity").html(grid);
        }
        else {
            $("#dvTitle").html('');
            $("#dvDynamicEntity").html('');
        }

    },
    renderGrid: function (context) {
        var gridObject = [];
        gridObject.push({ tableHeader: "Id", tableValue: 'employeeId' });
        gridObject.push({ tableHeader: "Employee Name", tableValue: 'employeeName' });
        gridObject.push({ tableHeader: "Month", tableValue: 'month' });
        gridObject.push({ tableHeader: "Year", tableValue: 'year' });
        gridObject.push({ tableHeader: "Employee Code", tableValue: 'employeeCode' });
        gridObject.push({ tableHeader: "Status", tableValue: 'status' });
        $taxHistory.renderFieldGrid(gridObject, { id: $taxHistory.selectedCategoryId });
        $taxHistory.LoadAttributeModels(context, gridObject, { id: $taxHistory.selectedCategoryId });


    },
    taxHistoryValidate: function (count) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxProcess/GetTaxHistory",
            contentType: "application/json; charset=utf-8",
            async: false,
            data: JSON.stringify({
                selectedId: $taxHistory.selectedCategoryId,
                month: $taxHistory.month,
                year: $taxHistory.year,
                type: $taxHistory.selectedpayrollType,
                financeyearId: $taxHistory.financeYear.id
            }),
            dataType: "json",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        var counts = 0;
                        out.forEach(function (item) {

                            if (item.status.trim() == "Not Process") {
                                counts++;
                            }
                        });

                        if (count != counts) {
                            // if (confirm("Would you like to delete income tax process too?")) {

                            $taxHistory.DeleteValidation();
                            $payrollHistroy.deletePayroll();
                            //}
                            //else {
                            //    $payrollHistroy.deletePayroll();
                            //}
                        }
                        else {
                            $payrollHistroy.deletePayroll();
                        }
                        break;
                    case false:

                        break;
                }

            },
            error: function (msg) {
            }
        });


    },
    LoadGridData: function () {
        debugger
        $taxHistory.month = $('#sltMonth').val();
        $taxHistory.year = $('#sltYear').val();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxProcess/GetTaxHistory",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                selectedId: $taxHistory.selectedCategoryId,
                month: $taxHistory.month,
                year: $taxHistory.year,
                type: $taxHistory.selectedpayrollType,
                financeyearId: $taxHistory.financeYear.id,
                processtype: $taxHistory.processtype
            }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $taxHistory.renderGrid(out);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });

    },
    LoadAttributeModels: function (data, context, tableprop) {
        var columnsValue = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
        }
        var columnDef = [];
        columnDef.push({ "aTargets": [0], "sClass": "nodisp", "bSearchable": false }); //for id column
        if (data != null) {
            for (var cnt1 = 1; cnt1 < context.length; cnt1++) {
                if (cnt1 == 1) {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.status == 'Processed' || oData.status == 'Imported') {
                                    var b = $('<a href="#"  data-toggle="modal" data-target="#workSheetpreview" class="editeButton"><span>' + oData.employeeName + '</span></button>');
                                    var c = $('<a href="#" class="printButton"> <img src="assets/plugins/TableTools-master/images/pdf.png"></button>');
                                    var d = $('<a href="#" class="printButton"> <img src="assets/plugins/TableTools-master/images/pdf.png"></button>');
                                    b.button();
                                    b.on('click', function () {
                                        $taxHistory.getEmployeePreviewWorkSheet(oData.employeeId);
                                        return false;
                                    });
                                    c.button();
                                    c.on('click', function () {
                                        if ($taxHistory.processtype == "employee") {
                                            $taxHistory.currentMonthYesOrNo = false;
                                            $taxHistory.getEmployeePrintWorkSheet(oData.employeeId);
                                        }
                                        else {
                                            $.confirm({
                                                title: 'TDS Sheet',
                                                content: 'Do you wish to process for current month',
                                                buttons: {
                                                    YES: function () {
                                                        $taxHistory.currentMonthYesOrNo = true;

                                                        $taxHistory.getEmployeePrintWorkSheet(oData.employeeId);
                                                    },
                                                    NO: function () {
                                                        $taxHistory.currentMonthYesOrNo = false;
                                                        $taxHistory.getEmployeePrintWorkSheet(oData.employeeId);
                                                    },

                                                }
                                            });
                                        }

                                    });
                                    d.button();
                                    d.on('click', function () {
                                        $taxHistory.getEmployeePrintWorkSheetAP(oData.employeeId);

                                    });
                                    $(nTd).empty();
                                    if ($('#hdnRoleName').val().toUpperCase() == "EMPLOYEE" || $taxHistory.processtype == "employee") {
                                        $(nTd).prepend(b, c);
                                    }
                                    else {
                                        $(nTd).prepend(b, c, d);
                                    }


                                }
                                else { return }
                            }
                        }

                    );
                }
                else if (cnt1 == 3) {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.status != 'Processed' && oData.status != 'Imported' && oData.status != 'Not Process') {
                                    var b = $('<a href="#" class="editeButton"><span>Error</span></button>');
                                    b.button();
                                    b.on('click', function () {
                                        var sts = oData.status.split('&');
                                        var err = '';
                                        $('#dvErrorMsg').html('');
                                        $.each(sts, function (ind, item) {
                                            $('#dvErrorMsg').append('<p>' + item + '</p>')
                                        });
                                        // $('#dvErrorMsg').append('<p>' + sts + '</p>')
                                        $('#dvError').modal('toggle');
                                        return false;
                                    });
                                    $(nTd).empty();
                                    $(nTd).prepend(b);

                                }
                                else { return }
                            }
                        }

                    );
                }
                else if (cnt1 == 2) {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            "type": 'natural',
                            //"sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            }
                        }

                    ); //for action column
                }
                else {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            //"sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            }
                        }

                    ); //for action column
                }
            }
        }
        var dtClientList = $('#tbl_' + tableprop.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data,
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblFormula tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblFormula thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "aaSorting": [[2, "asc"]],
            scroller: {
                loadingIndicator: true
            }
        });
        //$('#tbl_' + tableprop.id).on('click', 'tbody td:not(:first-child)', function (e) {
        //    dtClientList.inline(this);
        //});
    },
    save: function (context) {
        //  $app.showAlert("Income tax process has been started", 1);
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "TaxProcess/TaxProcess",
            data: JSON.stringify({ selectedId: $taxHistory.selectedCategoryId, year: $taxHistory.year, month: $taxHistory.month, type: $taxHistory.selectedpayrollType, financeYearId: $taxHistory.financeYear.id, processtype: $taxHistory.processtype }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: true,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $taxHistory.LoadGridData();
                        $app.showAlert(jsonResult.Message, jsonResult.Message == "No Record(s) Processed" ? 1 : 2);
                        $app.hideProgressModel();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
                $taxHistory.LoadGridData();
                $app.hideProgressModel();
            },
            complete: function () {
            }
        });
    },
    DeleteValidation: function (context) {
        debugger;
        if ($taxHistory.selectedCategoryId == null) {
            $taxHistory.selectedCategoryId = "00000000-0000-0000-0000-000000000000";
        }
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "TaxProcess/DeleteValidation",
            data: JSON.stringify({ selectedId: $taxHistory.selectedCategoryId, year: $taxHistory.year, month: $taxHistory.month, type: $taxHistory.selectedpayrollType, financeyearId: $taxHistory.financeYear.id, processtype: $taxHistory.processtype }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $taxHistory.LoadGridData();
                        if (jsonResult.result.notEligible.length > 0) {
                            var noteligible = new Array();
                            jsonResult.result.notEligible
                            for (i = 0; i < jsonResult.result.notEligible.length; i++) {
                                noteligible[i] = jsonResult.result.notEligible[i].EmployeeCode;
                            }
                            $app.showAlert('Cannot Delete The Tax Process For Employee   ' + noteligible.join(","), 3);
                        }
                        if (jsonResult.result.eligible.length > 0) {
                            var eligible = new Array();
                            var id = new Array()

                            jsonResult.result.eligible
                            for (i = 0; i < jsonResult.result.eligible.length; i++) {
                                eligible[i] = jsonResult.result.eligible[i].EmployeeCode;
                                id[i] = jsonResult.result.eligible[i].Id;

                            }
                            var con = confirm('Are you sure to delete the tax process for the employee  ' + eligible.join(","));
                            if (con == true) {

                                $taxHistory.DeleteTaxProcess(id);
                            }
                        }
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
    DeleteTaxProcess: function (context) {

        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "TaxProcess/DeleteTaxProcess",
            data: JSON.stringify({ selectedId: $taxHistory.selectedCategoryId, year: $taxHistory.year, month: $taxHistory.month, type: $taxHistory.selectedpayrollType, financeyearId: $taxHistory.financeYear.id, empid: context, processtype: $taxHistory.processtype }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $taxHistory.LoadGridData();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, jsonResult.Message == "No Record(s) Processed" ? 1 : 2);
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
    viewHistory: function (employeeid) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetPayrollProcessdHistory",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ employeeId: employeeid, month: $taxHistory.month, year: $taxHistory.year }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        //   $entityMapping.createForm(out);
                        $taxHistory.renderDynamicForm(out, false);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    deleteEmployeePreviewWorkSheet: function (path) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxProcess/DeleteWorksheet",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ path: path }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {


                $("#workSheetpreview").modal('hide');
                return false;
            }
        });


    },

    getEmployeePreviewWorkSheet: function (employeeid) {
        $app.showProgressModel();
        $taxHistory.actual = 'NO';
        if ($taxHistory.processtype == "employee") {
            $taxHistory.currentMonthYesOrNo = false;
        }
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmployeeData",
            data: JSON.stringify({ empId: employeeid }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $taxHistory.selectedCategoryId = jsonResult.result.categoryId;
                        $taxHistory.selectedEmpcode = jsonResult.result.empCode;
                        $.ajax({
                            type: 'POST',
                            url: $app.baseUrl + "TaxProcess/GetWorksheet",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ categories: $taxHistory.selectedCategoryId, month: $taxHistory.month, year: $taxHistory.year, empCode: $taxHistory.selectedEmpcode, financeyearId: $taxHistory.financeYear.id, actual: $taxHistory.actual, processtype: $taxHistory.processtype, IsCurrentmonth: $taxHistory.currentMonthYesOrNo }),
                            dataType: "json",
                            async: false,
                            success: function (jsonResult) {
                                debugger;
                                jsonResult.result.filePath;
                                var input = jsonResult.result.filePath;

                                var start = input.indexOf("tempfiles");
                                var end = jsonResult.result.filePath.length;
                                //$("#preViewSrc").attr('src', jsonResult.result.filePath);
                                $("#preView").attr('src', "");
                                $("#preViewSrc").attr('src', "");
                                $("#preView").attr('src', input.substring(start, end));
                                $("#preViewSrc").attr('src', input);
                                //$("#workSheetpreview").load();
                                $("#workSheetpreview").modal('show');
                                return false;
                            }
                        });
                        break;
                    case false:
                        $app.showAlert("There is an error while retriving employee data", 4);
                        break;
                }

            },
            complete: function () {
                $app.hideProgressModel();
            }

        });

    },


    getEmployeeLoginWorkSheet: function (employeeid) {

        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmployeeData",
            data: JSON.stringify({ empId: employeeid }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $taxHistory.selectedCategoryId = jsonResult.result.categoryId;
                        $taxHistory.selectedEmpcode = jsonResult.result.empCode;
                        $.ajax({
                            type: 'POST',
                            url: $app.baseUrl + "TaxProcess/GetEmployeeLoginWorksheet",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ month: $("#sltMonth").val(), year: $("#ltYear").val(), empCode: $taxHistory.selectedEmpcode, processtype: $taxHistory.processtype }),
                            dataType: "json",
                            async: false,
                            success: function (jsonResult) {
                                var oData = new Object();
                                oData.filePath = jsonResult.result.filePath;
                                $app.downloadSync('Download/DownloadPaySlip', oData);
                                return false;
                            }
                        });
                        break;
                    case false:
                        $app.showAlert("There is an error while retriving employee data", 4);
                        break;
                }

            },
            complete: function () {
                $app.hideProgressModel();
            }

        });

    },
    getEmployeePrintWorkSheetAP: function (employeeid) {
        debugger;

        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmployeeData",
            data: JSON.stringify({ empId: employeeid }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $taxHistory.selectedCategoryId = jsonResult.result.categoryId;
                        $taxHistory.selectedEmpcode = jsonResult.result.empCode;
                        $.ajax({
                            type: 'POST',
                            url: $app.baseUrl + "TaxProcess/GetWorksheetAP",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ categories: $taxHistory.selectedCategoryId, month: $taxHistory.month, year: $taxHistory.year, empCode: $taxHistory.selectedEmpcode, financeyearId: $taxHistory.financeYear.id, processtype: $taxHistory.processtype }),
                            dataType: "json",
                            async: false,
                            success: function (jsonResult) {
                                var oData = new Object();
                                oData.filePath = jsonResult.result.filePath;
                                $app.downloadSync('Download/DownloadPaySlip', oData);
                                return false;
                            }
                        });
                        break;
                    case false:
                        $app.showAlert("There is an error while retriving employee data", 4);
                        break;
                }

            },
            complete: function () {
                $app.hideProgressModel();
            }

        });

    },
    getEmployeePrintWorkSheet: function (employeeid) {
        debugger;
        //if (confirm("Exclude Current Month Value")) {
        //    $taxHistory.currentMonthYesOrNo = false;
        //}
        //else {
        //    $taxHistory.currentMonthYesOrNo = true;
        //} 
        $taxHistory.actual = 'NO';
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmployeeData",
            data: JSON.stringify({ empId: employeeid }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $taxHistory.selectedCategoryId = jsonResult.result.categoryId;
                        $taxHistory.selectedEmpcode = jsonResult.result.empCode;
                        $.ajax({
                            type: 'POST',
                            url: $app.baseUrl + "TaxProcess/GetWorksheet",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ categories: $taxHistory.selectedCategoryId, month: $taxHistory.month, year: $taxHistory.year, empCode: $taxHistory.selectedEmpcode, financeyearId: $taxHistory.financeYear.id, actual: $taxHistory.actual, IsCurrentmonth: $taxHistory.currentMonthYesOrNo, processtype: $taxHistory.processtype }),
                            dataType: "json",
                            async: false,
                            success: function (jsonResult) {
                                var oData = new Object();
                                oData.filePath = jsonResult.result.filePath;
                                $app.downloadSync('Download/DownloadPaySlip', oData);
                                return false;
                            }
                        });
                        break;
                    case false:
                        $app.showAlert("There is an error while retriving employee data", 4);
                        break;
                }

            },
            complete: function () {
                $app.hideProgressModel();
            }

        });

    },


    convertPDF: function (strhtml) {
        $.fileDownload($app.baseUrl + 'Download/ConvertPDF', {
            //preparingMessageHtml: "We are preparing your report, please wait...",
            //failMessageHtml: "There was a problem generating your report, please try again.",
            httpMethod: "POST",

            data: JSON.stringify({ strhtml: strhtml })//$(this).serialize()
            , successCallback: function (url) {
                $app.hideProgressModel();
            },
            failCallback: function (responseHtml, url) {
                $app.hideProgressModel();
                $app.showAlert('File not found', 4);

            }
        });
    },
    renderDynamicForm: function (data) {
        var formAttribute = [];
        formAttribute.push({
            type: "text",
            displayedAs: "Name",
            attributeName: "Name",
            attributeId: "Name",
            attributeModelId: 'Name',
            behaviorType: 'Master',
            isMasterField: '',
            minLength: "5",
            maxLength: "100",
            attributeValue: (data.Name == null) ? "" : data.Name,
            required: 1,
            readOnly: ' readonly="true"'
        });
        for (var cnt = 0; cnt < data.EntityAttributeModelList.length; cnt++) {
            formAttribute.push({
                type: "text",
                displayedAs: data.EntityAttributeModelList[cnt].AttributeModel.DisplayAs,
                attributeName: data.EntityAttributeModelList[cnt].AttributeModel.Name,
                attributeId: data.EntityAttributeModelList[cnt].Id,
                attributeModelId: data.EntityAttributeModelList[cnt].AttributeModelId,
                behaviorType: data.EntityAttributeModelList[cnt].AttributeModel.BehaviorType,
                isMasterField: data.EntityAttributeModelList[cnt].IsMasterField,
                minLength: "5",
                maxLength: data.EntityAttributeModelList[cnt].AttributeModel.DataSize,
                attributeValue: (data.EntityAttributeModelList[cnt].EntityAttributeValue.Value == null) ? (data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue == null) ? "" : data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue : data.EntityAttributeModelList[cnt].EntityAttributeValue.Value,
                required: data.EntityAttributeModelList[cnt].AttributeModel.IsMandatory,
                readOnly: ' readonly="true"'
            });
        }

        //
        var returnval = '<form role="form" id="' + data.Id + '"><div class="modal-dialog"> '
            + '<div class="modal-content"> '
            + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
            + '  &times;</button>'
            + '  <h4 class="modal-title" id="H4">'
            + $('#lbl_' + data.EntityModelId).text().toLowerCase() + ' Details </h4>'
            + ' </div>'
            + ' <div class="modal-body"> <div class="form-horizontal"> {formelemnt}  </div></div>';
        var formelemnt = '';
        var earningElmt = '';
        var dedutElmt = '';
        for (var cnt = 0; cnt < formAttribute.length; cnt++) {
            var req = ''
            if (formAttribute[cnt].required == 1) {
                req = "required";
            }
            var temp = '<div class="form-group">'
                + ' <label class="control-label col-md-4">' + formAttribute[cnt].displayedAs + '</label>'
                + '<div class="col-md-7">'
                + '<input type="' + formAttribute[cnt].type + '" class="form-control" id="' + formAttribute[cnt].attributeModelId + '" value="' + formAttribute[cnt].attributeValue
                + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + formAttribute[cnt].readOnly + '/>'
            temp = temp + '</div></div>';
            if (formAttribute[cnt].behaviorType == 'Earning') {
                earningElmt = earningElmt + temp;
            }
            else if (formAttribute[cnt].behaviorType == 'Deduction') {
                dedutElmt = dedutElmt + temp;
            }
            else {
                formelemnt = formelemnt + temp;
            }
            // formelemnt = formelemnt + temp;
        }
        returnval = returnval + '</form>'
        if (earningElmt != '') {
            earningElmt = '<h4>Earning\'s</h5><hr/>' + earningElmt;
        }
        if (dedutElmt != '') {
            dedutElmt = '<h4>Deduction\'s</h5><hr/>' + dedutElmt;
        }
        formelemnt = formelemnt + earningElmt + dedutElmt;

        returnval = returnval.replace('{formelemnt}', formelemnt);
        document.getElementById("dynamicProcessForm").innerHTML = returnval;
        $('#dynamicProcessForm').modal('toggle');

    }
    , GetFormString: function (data) {
        var formAttribute = [];
        formAttribute.push({
            type: "text",
            displayedAs: "Name",
            attributeName: "Name",
            attributeId: "Name",
            attributeModelId: 'Name',
            isMasterField: '',
            minLength: "5",
            maxLength: "100",
            attributeValue: (data.Name == null) ? "" : data.Name,
            required: 1,
            readOnly: ' readonly="true"'
        });
        for (var cnt = 0; cnt < data.EntityAttributeModelList.length; cnt++) {
            formAttribute.push({
                type: "text",
                displayedAs: data.EntityAttributeModelList[cnt].AttributeModel.DisplayAs,
                attributeName: data.EntityAttributeModelList[cnt].AttributeModel.Name,
                attributeId: data.EntityAttributeModelList[cnt].Id,
                attributeModelId: data.EntityAttributeModelList[cnt].AttributeModelId,
                isMasterField: data.EntityAttributeModelList[cnt].IsMasterField,
                minLength: "5",
                maxLength: data.EntityAttributeModelList[cnt].AttributeModel.DataSize,
                attributeValue: (data.EntityAttributeModelList[cnt].EntityAttributeValue.Value == null) ? (data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue == null) ? "" : data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue : data.EntityAttributeModelList[cnt].EntityAttributeValue.Value,
                required: data.EntityAttributeModelList[cnt].AttributeModel.IsMandatory,
                readOnly: ' readonly="true"'
            });
        }

        //
        var returnval = '<form role="form" id="' + data.Id + '"><div class="modal-dialog"> '
            + '<div class="modal-content"> '
            + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
            + '  &times;</button>'
            + '  <h4 class="modal-title" id="H4">'
            + $('#lbl_' + data.EntityModelId).text().toLowerCase() + ' Details </h4>'
            + ' </div>'
            + ' <div class="modal-body"> <div class="form-horizontal"> {formelemnt}  </div></div>';
        var formelemnt = '';
        for (var cnt = 0; cnt < formAttribute.length; cnt++) {
            var req = ''
            if (formAttribute[cnt].required == 1) {
                req = "required";
            }
            var temp = '<div class="form-group">'
                + ' <label class="control-label col-md-4">' + formAttribute[cnt].displayedAs + '</label>'
                + '<div class="col-md-7">'
                + '<input type="' + formAttribute[cnt].type + '" class="form-control" id="' + formAttribute[cnt].attributeModelId + '" value="' + formAttribute[cnt].attributeValue
                + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + formAttribute[cnt].readOnly + '/>'
            temp = temp + '</div></div>';
            formelemnt = formelemnt + temp;
        }

        returnval = returnval + '</form>'
        returnval = returnval.replace('{formelemnt}', formelemnt);
        return returnval;

    },
    LoadWorkSheetColumn: function () {

        var dtClientList = $('#tblWorksheetXlReport').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [

                { "data": "Id" },
                { "data": "Order" },
                { "data": "Section" },
                { "data": "Description" },
                { "data": null }
            ],

            "aoColumnDefs": [

                {
                    "aTargets": [0],
                    "sClass": "nodisp"
                },
                {
                    "aTargets": [1],
                    "sClass": "nodisp"
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

                        var c = $('<input type="checkbox" class="selectxlColumn">');
                        c.button();
                        c.on('click', function () {
                            $LeaveReport.LoadPendingStatPopup(sData);
                            //$leave.intializereason(data = 'Cancel');

                        });

                        $(nTd).empty();
                        $(nTd).prepend(c);
                    }
                }],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxProcess/GetWorksheetColumn",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ financialyearId: $taxHistory.financeYear.id }),
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
                                $app.showAlert(jsonResult.Message, jsonResult.StatusCode);
                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            // "order": [[1, "asc"], [2, 'asc']],
            "order": [[1, "asc"]],
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    SelectAll: function (tblid, chkid) {

        var checkboxCount = $('#' + tblid + ' tbody tr').length;
        var isCheckAll;
        var rows = $("#" + tblid + "").dataTable().fnGetNodes();
        if (chkid.checked == true) {
            isCheckAll = true;
        } else {
            isCheckAll = false;
        }
        for (var i = 0; i < rows.length; i++) {
            $(rows[i]).find("." + chkid.id).prop("checked", isCheckAll);
        }
    },
    getXLWorkSheet: function () {

        var datum = new Object();

        datum.listid = [];
        datum.listfield = [];
        var rows = $("#tblWorksheetXlReport").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {


            if ($(rows[i]).find(".selectxlColumn").prop("checked")) {

                datum.listid.push($(rows[i]).find(":eq(0)").html());
                datum.listfield.push($(rows[i]).find(":eq(3)").html());
            }
        }
        if ($('#sltMonth').find('option:selected').text() == "--Select---") {
            $app.showAlert("please select the effective month", 4);
            return;
        }
        if (datum.listid.length > 0) { } else {
            $app.showAlert("please select atleast one column", 4);
            return;
        }
        var month = $('#sltMonth').val();
        var year = $('#sltYear').val();

        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "TaxProcess/GetWorksheetXL",
            data: JSON.stringify({ categories: null, month: month, year: year, empCode: "", financeyearId: $taxHistory.financeYear.id, data: datum }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                if (jsonResult.Message == "Failure") {
                    $app.showAlert(jsonResult.result, 4)
                } else {
                    $app.hideProgressModel();
                    var oData = new Object();

                    oData.filePath = jsonResult.result;
                    $app.downloadSync('Download/DownloadPaySlip', oData);
                }
            },
            complete: function () {
                $app.hideProgressModel();

            }

        });


    },
    TaxpreviewForm: function () {
        var formPreview = '';

        formPreview = formPreview + "<div class='col-sm-12'>"
        formPreview = formPreview + "<div class='formgroup'>";
        formPreview = formPreview + "<label class='control-label'></label>";

        formPreview = formPreview + "</div>";
        formPreview = formPreview + "</div>"

    },
    getWorkSheet: function () {
        var categories = '';
        $taxHistory.actual = 'NO';
        var rows = $("#tbldwCat").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            if ($(rows[i]).find(".cbCategory").prop("checked")) {
                categories += "" + $(rows[i]).find(":eq(2)").html() + ",";
            }
            categories = categories.trim(',');
            var month = $('#sltMonth').val();
            var year = $('#sltYear').val();

        }

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxProcess/GetWorksheet",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ categories: categories, month: month, year: year, empCode: $('#txtEmpcode').val(), financeyearId: $taxHistory.financeYear.id, actual: $taxHistory.actual, processtype: $taxHistory.processtype }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                var oData = new Object();
                oData.filePath = jsonResult.result.filePath;
                $app.downloadSync('Download/DownloadPaySlip', oData);
                return false;
            }
        })

    },

};

