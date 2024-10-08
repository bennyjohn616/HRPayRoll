﻿$("#txtResignationDate").change(function () {

    var LWD = new Date($("#txtLastWorkingDate").val());
    var ResignDate = new Date($("#txtResignationDate").val());
    var DOJ = new Date($("#txDoj").val());
    if (ResignDate > LWD) {
        $app.showAlert('Resignation Date should not be Grater Than Last Working Date', 4);
        $("#txtResignationDate").val('');
    }
    else if (ResignDate == DOJ) {
        $("#txtResignationDate").val();
    }
    else if (ResignDate < DOJ) {
        $app.showAlert('Resignation Date should not be Less Than Date of joining', 4);
        $("#txtResignationDate").val('');
    }
    else {
        $("#txtResignationDate").val();
    }
});
$("#txtSettlementDate").change(function () {
    var LWD = new Date($("#txtLastWorkingDate").val());
    var Settlementdate = new Date($("#txtSettlementDate").val());
    if (Settlementdate < LWD) {
        $app.showAlert('Settlement Date should not be less Than Last Working Date', 4);
        $("#txtSettlementDate").val('');
    }
    else if (Settlementdate == LWD) {
        $("#txtSettlementDate").val();
    }
    else {
        $("#txtSettlementDate").val();
    }
});
$("#sltCategorylist").change(function () {
    $FullFinalSettlement.dropdownchangeEvent();
});

$("#FullsltMonth").change(function () {
    debugger;
    var month = $('#FullsltMonth').val();
    var sd = new Date($FullFinalSettlement.financeYear.startDate);
    var ed = new Date($FullFinalSettlement.financeYear.EndDate);
    var d = new Date();
    var Currentyear = parseInt(d.getFullYear());
    $('#FullsltYear').html('');
    $('#FullsltYear').append($("<option></option>").val(Currentyear - 2).html(Currentyear - 3));
    $('#FullsltYear').append($("<option></option>").val(Currentyear - 2).html(Currentyear - 2));
    $('#FullsltYear').append($("<option></option>").val(Currentyear - 1).html(Currentyear - 1));
    $('#FullsltYear').append($("<option></option>").val(Currentyear).html(Currentyear));
    $('#FullsltYear').append($("<option></option>").val(Currentyear + 1).html(Currentyear + 1));
/*    if (ed.getMonth() + 1 >= month) {
        $('#FullsltYear').append('<option value='+ ed.getFullYear() +'>'+ed.getFullYear() +'</option>')
    }
    else {
        $('#FullsltYear').append('<option value=' + sd.getFullYear() + '>' + sd.getFullYear() + '</option>')
    }*/
    $FullFinalSettlement.dropdownchangeEvent();
});


$("#FullsltYear").change(function () {
    $FullFinalSettlement.dropdownchangeEvent();
});


$("#sltEmployeelist").change(function () {
    if ($("#sltEmployeelist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#dvdetails").addClass('nodisp');
        $("#dvsave").addClass('nodisp');
    }
    else {

        $FullFinalSettlement.SelectedCatId = $("#sltCategorylist").val();
        $FullFinalSettlement.FullFinalSettlementEmpId = $("#sltEmployeelist").val();
        $("#dvdetails").removeClass('nodisp');
        $("#dvsave").removeClass('nodisp');
        $FullFinalSettlement.LoadFullFinalSettlement({ Id: $FullFinalSettlement.FullFinalSettlementEmpId });
        $FullFinalSettlement.empEntity();

    }
});

$("#btnRefreshProcess").click(function () {
     $("#sltEmployeelist").val("00000000-0000-0000-0000-000000000000") 
        $("#dvdetails").addClass('nodisp');
        $("#dvsave").addClass('nodisp');
     
   
});
//$("#sltMonth, #sltYear").change(function () {

//    var CheckResignationDate = new Date($("#txtResignationDate").val());   
//    if (CheckResignationDate.getMonth() + 1 <= $("#sltMonth").val() && CheckResignationDate.getFullYear() <= $("#sltYear").val()) {
//        $FullFinalSettlement.loadComponent();
//    }
//    else {

//        $('#dvFullMonthly').html('');
//        $app.showAlert("Invalid Selection of Month & Year", 3);

//    }

//});

$("#btnAdd").on('click', function () {
    $FullFinalSettlement.save();
    $FullFinalSettlement.saveMI();

});

$("#btnAddFFProcess").on('click', function () {

    //var Lastwday = $("#txtLastWorkingDate").val();
    //var LW = Lastwday.replace(new RegExp("/", "g"), ' ')
    //var LWday = new Date(LW);
    //var month = LWday.getMonth() + 1;
    //var year = LWday.getFullYear();
    if ($("#txtResignationDate").val() == "" || $("#txtSettlementDate").val() == "") {
        $app.showAlert('Please enter ResignationDate/SettlementDate ', 4);
        return false;
    }
    var month = $("#FullsltMonth").val();
    var year = $("#FullsltYear").val();
    var inputmonth1 = month + "/" + "01" + "/" + year;
    var inputmonth = new Date(inputmonth1);
    var LastWorkingMonth = new Date($("#txtLastWorkingDate").val());
    var LWDMonth = new Date(LastWorkingMonth).getMonth() + 1;
    var LWDYear = new Date(LastWorkingMonth).getFullYear();
    var LastMonthlyInput = LWDMonth + "/" + "01" + "/" + LWDYear;
    var LWM = new Date(LastMonthlyInput);
    LWM.setDate(LWM.getDate() - 1);
    if (inputmonth == LWM || inputmonth > LWM) {
        $FullFinalSettlement.LoadGridData();
    }
    else {
        $app.showAlert('Your Input Month and Year should be grater than Last Working Month', 4);
        return false;
    }
    //  $companyCom.loadCategory({ id: "sltCategory" });


});
$("#btnSend").on('click', function () {

    // $FullFinalSettlement.save();
});


var $FullFinalSettlement = {
    FafId: '',
    FullFinalSettlementEmpId: '',
    SelectedCatId: '',
    fullMonthlyTblId: 'tblFullMonthly',
    fullFinalTblId: 'tblFullFinal',
    entitymodelId: null,
    entityId: null,
    financeYear: $companyCom.getDefaultFinanceYear(),
    loadInitial: function () {
        $payroll.initDatetime();
        $companyCom.loadCategory({ id: "sltCategorylist" });
        //$companyCom.loadEmployee({ id: "sltEmployeelist" });
        $payroll.loadMonth({ id: 'sltFafMonth' });
        //$Separation.loadTypeOfSep({ id: "sltTypeOfSeparation" });
        $("#dvemp").addClass('nodisp');
    },
    LoadFullFinalSettlement: function (context) {

        debugger;
        $.ajax({
            url: $app.baseUrl + "Transaction/GetFullAndFinalData",
            data: JSON.stringify({ FafEmpId: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
                        $FullFinalSettlement.RenderData(p);
                        $FullFinalSettlement.getGridData();
                        //   $FullFinalSettlement.loadFullFinal(p.FullFinalDetails);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });

    },
    PreviewFFProcess: function () {
        $.ajax({
            url: $app.baseUrl + "Transaction/GetSettlementOutput",
            data: JSON.stringify({ dataValue: { Fafid: $FullFinalSettlement.FafId, FafEmpId: $('#sltEmployeelist').val() } }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        var oData = new Object();

                        oData.filePath = p.filePath;
                        $app.downloadSync('Download/DownloadPaySlip', oData);
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
    DeleteFFProcess: function () {
        $.ajax({
            url: $app.baseUrl + "Transaction/DeleteFFProcess",
            data: JSON.stringify({ empid: $('#sltEmployeelist').val(), includeTax: $('#chkincludeTax').prop('checked') }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $('#dvFullFinal').html('')
                        $FullFinalSettlement.LoadFullFinalSettlement({ Id: $FullFinalSettlement.FullFinalSettlementEmpId });
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
    RenderData: function (data) {

        var formData = document.forms["FullFinalSettlement"];
        $FullFinalSettlement.FafId = data.Fafid
        $FullFinalSettlement.FafEmpId = data.FafEmpId;
        //formData.elements["sltMonth"].value = data.FafapplyMonth;
        //formData.elements["sltYear"].value = data.FafapplyYear;
        formData.elements["txtResignationDate"].value = data.FafresignationDate;
        formData.elements["txtLastWorkingDate"].value = data.FaflastWorkingDate;
        formData.elements["txtRelievingDate"].value = data.FaflastWorkingDate;
        formData.elements["txDoj"].value = data.FafDateOfJoining;
        formData.elements["txtSettlementDate"].value = data.FafsettlementDate;
        formData.elements["txtNoticePeriod"].value = data.FafnoticePeriodToBeServed;
        // formData.elements["txtLOPDays"].value = data.FaflopDays;
        formData.elements["txtSalaryDays"].value = data.FafsalaryDays;
        formData.elements["txtReportName"].value = data.Fafreportname;
        formData.elements["txtMonthDays"].value = data.FafmonthDays;
        formData.elements["txtNotes"].value = data.Fafnotes;
        $("#txtLoanAmount").val(data.Fafloanamount);

    },
    //--------
    LoadGridData: function () {
        debugger;
        $app.showProgressModel();
        $monthlyInput.FFFlag = true;
        var Lastwday = $("#txtLastWorkingDate").val();
        var LW = Lastwday.replace(new RegExp("/", "g"), ' ')
        var LWday = new Date(LW);
        month = LWday.getMonth() + 1;
        year = LWday.getFullYear();
        //month = $('#sltMonth').val();
        //year = $('#sltYear').val();
        employeeId = $('#sltEmployeelist').val();
        categoryId = $('#sltCategorylist').val();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetMonthlyInput",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                entitymodelId: $FullFinalSettlement.entitymodelId,
                entityId: $FullFinalSettlement.entityId,
                categoryId: categoryId,
                month: month,
                year: year,
                employeeId: employeeId
            }),
            dataType: "json",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $monthlyInput.selectedEntityId = $FullFinalSettlement.entityId;
                        $monthlyInput.selectedEntityModelId = $FullFinalSettlement.entitymodelId;
                        $monthlyInput.selectedCategoryId = categoryId;
                        console.log(out);
                        $monthlyInput.renderGrid(out);
                        $app.hideProgressModel();
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
        $app.hideProgressModel();

    },
    //-----
    empEntity: function () {

        employeeId = $('#sltEmployeelist').val();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetEmpEntity",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                entitymodelId: "00000000-0000-0000-0000-000000000000",
                entityId: "00000000-0000-0000-0000-000000000000",
                employeeId: employeeId
            }),
            dataType: "json",
            success: function (jsonResult) {
                debugger;
                if (jsonResult.Status == true) {

                    debugger;
                    var p = jsonResult.result;
                    $FullFinalSettlement.entityId = p.EntityId
                    $FullFinalSettlement.entitymodelId = p.EntityModelId;
                }
                else {
                    $app.showAlert('Dynamic group not mapped', 1);

                }
            },
            error: function (msg) {
            }
        });
    },
    saveMI: function () {

        var Lastwday = $("#txtLastWorkingDate").val();
        var LW = Lastwday.replace(new RegExp("/", "g"), ' ')
        var LWday = new Date(LW);
        month = LWday.getMonth() + 1;
        year = LWday.getFullYear();
        //month = $('#sltMonth').val();
        //year = $('#sltYear').val();
        employeeId = $('#sltEmployeelist').val();
        categoryId = $('#sltCategorylist').val();
        $monthlyInput.selectedEntityId = $FullFinalSettlement.entityId;
        $monthlyInput.selectedEntityModelId = $FullFinalSettlement.entitymodelId;
        $monthlyInput.selectedcategoryId = categoryId;
        $payrollHistroy.selectedCategoryId = employeeId;
        $payrollHistroy.selectedpayrollType = "FandF";
        $payrollHistroy.year = year;
        $payrollHistroy.month = month;
        $app.showProgressModel();
        var keyvalues = [];
        $('#tbl_' + $monthlyInput.selectedEntityId + ' tbody tr').each(function (index, data) {

            var dtValues = [];
            var cnt = 0;
            $(data).find('td').each(function (ind, tmp) {

                if (cnt == 0) {
                    dtValues.push({
                        'EmployeeId': $(tmp).text()
                    });
                }
                else {
                    var input = $(tmp).find('input');
                    if (input != null) {
                        var id = $(input).prop('id');
                        if (id != null) {
                            id = id.replace('txt_', '');
                            dtValues.push({
                                'Id': id, 'value': $(input).val()
                            });
                        }
                    }
                }
                cnt = cnt + 1;
            });
            var rowVal = [];
            for (var t1 = 1; t1 < dtValues.length; t1++) {
                rowVal.push({ 'AttributeModId': dtValues[t1].Id, 'MIValue': dtValues[t1].value });
            }
            keyvalues.push({ 'EmployeeId': dtValues[0].EmployeeId, 'Id': $FullFinalSettlement.entityId, 'Month': month, 'year': year, MonthlyInputAttributes: rowVal });
        });
        $.ajax({
            url: $app.baseUrl + "Entity/SaveMonthlyInput",
            data: JSON.stringify({ dataValue: keyvalues, entitymodeId: $FullFinalSettlement.entitymodelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:

                      //  $monthlyInput.selectedEntityId = $FullFinalSettlement.entityId;
                      //  $monthlyInput.selectedEntityModelId = $FullFinalSettlement.entitymodelId;
                      //  $monthlyInput.selectedcategoryId = categoryId;
                      //  $monthlyInput.month = month;
                      //  $monthlyInput.year = year;
                        //  $monthlyInput.LoadGridData();
                       // $FullFinalSettlement.LoadGridData();
                        // $payrollHistroy.save();
                        $FullFinalSettlement.savePayroll();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
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
    //-----
    savePayroll: function (context) {

        $app.showProgressModel();
        debugger;
        $.ajax({
            url: $app.baseUrl + "Entity/ProcessPayroll",
            data: JSON.stringify({ selectedId: $payrollHistroy.selectedCategoryId, year: $payrollHistroy.year, month: $payrollHistroy.month, type: $payrollHistroy.selectedpayrollType, includeTax: $('#chkincludeTax').prop('checked') }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {

                    case true:
                        // $payrollHistroy.LoadGridData();
                        $app.hideProgressModel();
                        $app.showAlert("Payroll Processed", jsonResult.Message == "No Record(s) Processed" ? 1 : 2);
                        $FullFinalSettlement.getGridData();
                        $('.modal').hide();
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
    //------
    getGridData: function () {
        debugger;
        month = $('#FullsltMonth').val();
        year = $('#FullsltYear').val();
        employeeId = $('#sltEmployeelist').val();
        categoryId = $('#sltCategorylist').val();
        $.ajax({
            url: $app.baseUrl + "Transaction/GetGridData",
            data: JSON.stringify({ employeeId: employeeId, month: month, year: year }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $FullFinalSettlement.loadFullFinal(p);
                        $app.hideProgressModel();
                        //$app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },
    //------
    save: function () {

        $app.showProgressModel();
        var data = $FullFinalSettlement.buildObject();
        $.ajax({
            url: $app.baseUrl + "Transaction/SaveFullAndFinal",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            async: false,
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
                        $FullFinalSettlement.getGridData();
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
    buildObject: function () {

        var formData = document.forms["FullFinalSettlement"];
        var data = {
            //SepCatid: formData.elements["sltCategorylist"].value,
            FafEmpId: formData.elements["sltEmployeelist"].value,
            FafapplyMonth: formData.elements["FullsltMonth"].value,
            FafapplyYear: formData.elements["FullsltYear"].value,
            FafresignationDate: formData.elements["txtResignationDate"].value,
            FaflastWorkingDate: formData.elements["txtLastWorkingDate"].value,
            FafrelievingDate: formData.elements["txtRelievingDate"].value,
            FafsettlementDate: formData.elements["txtSettlementDate"].value,
            FafnoticePeriodToBeServed: formData.elements["txtNoticePeriod"].value,
            // FaflopDays: formData.elements["txtLOPDays"].value,
            FafsalaryDays: formData.elements["txtSalaryDays"].value,
            Fafreportname: formData.elements["txtReportName"].value,
            FafmonthDays: formData.elements["txtMonthDays"].value,
            Fafnotes: formData.elements["txtNotes"].value,

        };
        return data;
    },
    loadComponent: function () {

        var gridObject = $FullFinalSettlement.fullMonthlyObject();
        var tableid = { id: $FullFinalSettlement.fullMonthlyTblId };
        var modelContent = $screen.createTable(tableid, gridObject);
        // $('#dvFullMonthly').html(modelContent);
        var data = null;
        //  $FullFinalSettlement.loadData(gridObject, tableid);
    },
    fullMonthlyObject: function () {

        var gridObject = [
                { tableHeader: "id", tableValue: "id", cssClass: 'nodisp' },
                { tableHeader: "Component Name", tableValue: "name", cssClass: '' },
                { tableHeader: "Value", tableValue: "componentVal", cssClass: '' }
        ];
        return gridObject;
    },
    loadData: function (context, tableId) {

        $.ajax({
            url: $app.baseUrl + "Transaction/GetFullAndFinalMonthlyComponent",
            data: JSON.stringify({ empId: $FullFinalSettlement.FullFinalSettlementEmpId, month: $("#FullsltMonth").val(), year: $("#FullsltYear").val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $FullFinalSettlement.loadfullMonthlyGrid(p, context, tableId);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });

    },
    loadfullMonthlyGrid: function (data, context, tableId) {


        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'actionColumn') {
                columnDef.push(
                        {
                            "aTargets": [cnt],
                            "sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                                b.button();
                                b.on('click', function () {
                                    $formulaCreation.edit(oData);
                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    DeleteClientRecord(oData.Id);
                                    return false;
                                });
                                $(nTd).empty();
                                $(nTd).prepend(b, c);
                            }
                        }

                    ); //for action column
            }
            else if (context[cnt].cssClass == 'edit') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="text" onkeyup="return $validator.moneyvalidation(this.id)" onkeypress="return $validator.moneyvalidation(this.id)" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
                        $(nTd).html(b);
                    }
                });

            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data,
            fnInitComplete: function (oSettings, json) {
                var r = $('#' + tableId.id + ' tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + tableId.id + ' thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    loadFullFinal: function (data) {

        var gridObject = [
               { tableHeader: "Pay Month", tableValue: "payMonth", cssClass: '' },
               { tableHeader: "Pay Year", tableValue: "payYear", cssClass: '' },
               { tableHeader: "Gross", tableValue: "gross", cssClass: '' },
               { tableHeader: "TDS", tableValue: "tds", cssClass: '' },
               { tableHeader: "NetPay", tableValue: "netPay", cssClass: '' }
        ];
        var tableid = { id: $FullFinalSettlement.fullFinalTblId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvFullFinal').html(modelContent);
        $FullFinalSettlement.loadfullFinalGrid(data, gridObject, tableid);
    },
    loadfullFinalGrid: function (data, context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'actionColumn') {
                columnDef.push(
                        {
                            "aTargets": [cnt],
                            "sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                                b.button();
                                b.on('click', function () {
                                    $formulaCreation.edit(oData);
                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    DeleteClientRecord(oData.Id);
                                    return false;
                                });
                                $(nTd).empty();
                                $(nTd).prepend(b, c);
                            }
                        }

                    ); //for action column
            }
            else if (context[cnt].cssClass == 'edit') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="text" onkeyup="return $validator.moneyvalidation(this.id)" onkeypress="return $validator.moneyvalidation(this.id)" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
                        $(nTd).html(b);
                    }
                });

            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data,
            fnInitComplete: function (oSettings, json) {
                var r = $('#' + tableId.id + ' tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + tableId.id + ' thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    saveFullFinal: function () {

        $app.showProgressModel();
        var data = $FullFinalSettlement.buildObject();
        var keyvalues = [];
        $('#' + $FullFinalSettlement.fullMonthlyTblId + ' tbody tr').each(function (index, data) {
            var dtValues = {};
            var cnt = 0;
            $(data).find('td').each(function (ind, tmp) {
                if (cnt == 0) {
                    dtValues.id = $(tmp).text();
                }
                if (cnt == 2) {
                    dtValues.componentVal = $(tmp).text();
                }
                cnt = cnt + 1;
            });
            dtValues.month = $("#FullsltMonth").val();
            dtValues.year = $("#FullsltYear").val();
            keyvalues.push(dtValues);
        });
        $.ajax({
            url: $app.baseUrl + "Transaction/AddFullAndFinal",
            data: JSON.stringify({ dataValue: keyvalues, data: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var p = jsonResult.result;
                        $FullFinalSettlement.RenderData(p);
                        companyid = 0;

                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },

    dropdownchangeEvent: function () {
        if ($("#sltCategorylist").val() == "00000000-0000-0000-0000-000000000000") {
            $("#dvemp").addClass('nodisp');
            $("#dvdetails").addClass('nodisp');
            $("#dvsave").addClass('nodisp');
        }
        else {

            $companyCom.loadSelectiveEmployee({ id: 'sltEmployeelist', condi: 'Category-FF.' + $("#sltCategorylist").val() + '.' + $("#FullsltMonth").val() + '.' + $("#FullsltYear").val() });
            $FullFinalSettlement.SelectedCatId = $("#sltCategorylist").val();
            $("#dvemp").removeClass('nodisp');
            $("#sltEmployeelist").val("00000000-0000-0000-0000-000000000000");
            $("#dvdetails").addClass('nodisp');
        }
    }

}