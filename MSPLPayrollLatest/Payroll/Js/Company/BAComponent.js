
$('#EmpCode').change(function () {
    $BAComponent.LoadData();
});


$('BAbtnSave').click(function () {
    debugger;
    $BAComponent.save();
});


$('#UpdComp1').focus(function () {
    debugger;
    $BAComponent.templist1 = []; $BAComponent.templist2 = [];
    var selval = "";
    if ($('#UpdComp1 option:selected').text().length > 0) {
        selval = $('#UpdComp1 option:selected').val();
    }
    debugger;
    ele1 = $("#UpdComp1");
    var data = $BAComponent.selectlist;
    if (data.length > 0) {
        for (var i in data) {
            $BAComponent.templist1.push(data[i].Id)
            $BAComponent.templist2.push(data[i].DisplayAs);
        }
    }
    $BAComponent.UpdateDropDown();
    $('#UpdComp1').val(selval);
    return true;
});



$Dropdown = {
    FillDropdown: function (selector, selectlist1, selectlist2) {
        debugger;
        if (selectlist2.length > 0) {
            var vItems_1 = [];
            for (var i in selectlist2) {
               vItems_1.push("<option " + "value = " + selectlist1[i] + ">" + selectlist2[i] + "</option>");
            }
            $(selector).empty();
            $(selector).append(vItems_1.join(''));
            return true;
        }
        else {
            $(selector).empty();
            return false;
        }
    },

};





var $BAComponent = {
    canSave: false,
    BACompId: '',
    BAComptable: 'tblBAComp',
    tableId: 'tblempBAComponent',
    selectedEmployeeId: null,
    selectlist: null,
    templist1: [],
    templist2: [],
    EmployeeId: null,
    payhistory: null,
    financeYear: null,
    EffectiveDate: null,
    Applymmyy: null,
    appmmyy:null,
    INPdate: null,
    cutoffdate: null,
    newtable: null,
    finalBallw: null,
    rowref: null,
    arrHead: null,
    EntityId: null,
    EntityModelId: null,
    serverDate: null,
    serverDate1: null,
    cutoffdateDisp: null,
    serverDate2:null,




    LoadBAComponents: function (data) {
        debugger;
        var date1 = new Date($BAComponent.serverDate);
        $BAComponent.serverDate1 = (date1.getFullYear() * 10000) + ((date1.getMonth() + 1) * 100) + date1.getDate();
        $BAComponent.serverDate2 = date1.getDate();
        $BAComponent.GetInpDate();
        $BAComponent.GetDefaultfinyear();
        $BAComponent.AddInitialize();
        $BAComponent.GetEmployee();
    },

    LoadData: function () {
        debugger;
        var emp = $('#EmpCode option:selected').val();
        var empcode = $('#EmpCode option:selected').text();
        $BAComponent.appmmyy = $.trim($('#appmmyy').val());
        var appmm = $BAComponent.appmmyy.substring(0, 2);
        var appyy = $BAComponent.appmmyy.substring(3);
        var finyy = $('#txtYear').val();
        if (appmm < 01 || appmm > 12 || appyy < 2021 || appyy > (finyy+1)) {
            var msg = "Enter Valid Month & year"
            this.warnmsg(msg);
            exit;
        }

        $BAComponent.GetPayHistory(emp);
        $BAComponent.checkTempData(emp);
        $BAComponent.EntityId = $BAComponent.datalist.EntityId;
        $BAComponent.EntityModelId = $BAComponent.datalist.EntityModelId;
        $BAComponent.selectlist = $BAComponent.datalist.AttributeModels;
        $BAComponent.newtable = "Y";
        var data2 = $BAComponent.selectlist;
        $BAComponent.templist1 = []; $BAComponent.templist2 = [];
        if (data2.length > 0) {
            for (var i in data2) {
                $BAComponent.templist1.push(data2[i].Id)
                $BAComponent.templist2.push(data2[i].DisplayAs);
            }
        }

        $('#msg').html('');

        /*if ($BAComponent.datalist.entityMasterValues.length > 0) {
            $('#btnSave').hide();
            var msg = "Basket Allowance Breakup Details for  " + empcode + " Entered, Cannot Update"
            this.warnmsg(msg);
            $('#txtId1Amt').attr('disabled', true);
            $('#UpdComp1').attr('disabled', true);
            return false;
        }*/

       /* if ($BAComponent.cutoffdate < $BAComponent.serverDate1) {
            $('#btnSave').hide();
            var msg = "!Cannot Enter Data after cutoff date </br>" + " CUTOFF DATE : " + $BAComponent.cutoffdateDisp
            this.warnmsg(msg);
            $('#txtId1Amt').attr('disabled', true);
            $('#UpdComp1').attr('disabled', true);
            return false;
        }*/

        if ($BAComponent.payhistory.length > 0) {
            $('#btnSave').hide();
            var msg = "!Salary Already processed for this Employee, Cannot Update Data";
            this.warnmsg(msg);
            $('#txtId1Amt').attr('disabled', true);
            $('#UpdComp1').attr('disabled', true);
            return false;
        }
        else {
            debugger;
            $('#TableArea').html('');
            var name = "Name : " + $BAComponent.datalist.emp.FirstName + "  " + $BAComponent.datalist.emp.LastName;
            $('#txtId1Amt').removeAttr('disabled');
            $('#UpdComp1').removeAttr('disabled');
            $('#name').text(name);
            if ($BAComponent.datalist.entTemp.length > 0) {
                var data3 = $BAComponent.datalist.entTemp;
                var data2 = $BAComponent.selectlist;
                for (i = 0; i < data3.length; i++) {
                    if ($BAComponent.newtable == "Y") {
                        $BAComponent.CreateTable();
                    }
                    $('#UpdComp1').html('');
                    var text1 = "";
                    for (k = 0; k < $BAComponent.templist1.length; k++) {
                        if ($BAComponent.templist1[k] == data3[i].CompId) {
                            text1 = $BAComponent.templist2[k];
                        }
                    }
                    $('#UpdComp1').append($("<option selected></option>").val(data3[i].CompId).html(text1));
                    $('#txtId1Amt').val(data3[i].Value);
                    $BAComponent.AddTable();
                }
            }
        }
        $('#data').removeClass('nodisp');
        $('#data1').removeClass('nodisp');
        $BAComponent.UpdateDropDown();
    },




    checkTempData: function (emp) {
        debugger;
        var financeyear = $BAComponent.financeYear.id;
        $.ajax({
            url: $app.baseUrl + "Entity/GetEntityTempData",
            async: false,
            data: JSON.stringify({refEntityId: emp, financeyear: financeyear}),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                var data = jsonResult.result;
                $BAComponent.GetTempData(data);
            },
            complete: function () {

            }
        });

    },

    GetTempData: function (data) {
        $BAComponent.datalist = "";
        $BAComponent.datalist = data;

    },

    Initialize: function () {
        $('#TableArea').html('');
        $('#data').addClass('nodisp');
        $('#data1').addClass('nodisp');
        $('#OptSave').addClass('nodisp');
    },

    closeMsg: function () {
        $.confirm({
            icon:"fa fa-info-circle",
            title: 'Confirm! ',
            content: 'Quit Without Saving Data!',
            type:'green',
            buttons: {
                Yes: function () {
                    $BAComponent.Initialize();
                },
                No: function () {
                },
            }
        });
    },


    save: function () {
        debugger;
        $app.showProgressModel();
        var data = '';
        var array1 = [];
        var emp = $('#EmpCode option:selected').val();
        var tableData = document.getElementById('Table1');
        if (tableData.rows.length > 0) {
            var rows = tableData.rows.length;
            for (i = 0; i < rows; i++) {
                var currentrow = tableData.rows[i];
                var data1 = {
                    financeyearId: $BAComponent.financeYear.id,
                    EffectiveDate: $BAComponent.EffectiveDate,
                    Applymmyy: $BAComponent.Applymmyy,
                    EmployeeId: emp,
                    CompId: currentrow.cells[3].innerText,
                    Value: Number(currentrow.cells[1].innerText),
                }
                array1.push(data1);
            }
        }
        data = array1;
        $.ajax({
            url: $app.baseUrl + "Entity/SaveEntityTempMaster",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $('#empBAComponent').modal('hide');
                        /* $BAComponent.checkTempData();*/
                        $app.showAlert(jsonResult.Message, 2);
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
        $BAComponent.Initialize();
    },

    GetDefaultfinyear: function () {
        debugger;
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() == "EMPLOYEE") {
            date2 = new Date($BAComponent.cutoffdate);
            $BAComponent.financeYear = $companyCom.EmployeeDefaultFinanceYear(date2);
            $('#txtYear').attr('readonly', true);
        }
        else {
            $BAComponent.financeYear = $companyCom.getDefaultFinanceYear();
        }
    },

    GetInpDate: function () {
        $.ajax({
            url: $app.baseUrl + "Setting/GetTDSdays",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $BAComponent.INPdate = jsonResult.result.TDSdays;
                        var locale = "en-us";
                        var cudate = new Date(parseInt(jsonResult.result.cutoffdate.replace(/(^.*\()|([+-].*$)/g, '')));
                        //cudate.setMonth(cudate.getMonth() + 1);
                        $BAComponent.cutoffdate = (cudate.getFullYear() * 10000) + ((cudate.getMonth() + 1) * 100) + cudate.getDate();
                        $BAComponent.cutoffdateDisp = cudate.getDate() + "/" + (cudate.getMonth() + 1) + "/" + cudate.getFullYear();
                        $app.hideProgressModel();
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



    AddInitialize: function () {
        $BAComponent.canSave = true;
        var formData = document.forms["frmBAComponent"];
        formData.elements["UpdComp1"].value = '';
        formData.elements["txtId1Amt"].value = '';
    },





    GetPayHistory: function (emp) {
        $app.showProgressModel();
        var appmm = $BAComponent.appmmyy.substring(0, 2);
        var appyy = $BAComponent.appmmyy.substring(3);
        var fromyear = appyy;
        var frommonth = appmm;
        if (appmm > 3) {
            var toyear = fromyear + 1;
        }
        else {
            var toyear = fromyear;
        }
        var tomonth = 03;
        var EmployeeId = emp;
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/GetEmpPayhistory",
            data: JSON.stringify({smonth : frommonth, syear : fromyear, nmonth : tomonth, nyear : toyear, EmployeeId: EmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $BAComponent.GetHistory(jsonResult.result);
                        $app.hideProgressModel();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            }
        })
    },


    GetHistory: function (result) {
        $BAComponent.payhistory = '';
        if (result.length > 0) {
            $BAComponent.payhistory = result;
        }
    },

    GetEmployee: function () {
        debugger;
        var condi = "Ballw" + ".";
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetSelectiveEmployees",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                Condition: condi
            }),
            dataType: "json",
            async: false,
            success: function (data) {
                debugger;
                var msg = data.result.Jsondata;
                $('#EmpCode').html('');
                $.each(msg, function (index, blood) {
                    $('#EmpCode').append($("<option></option>").val(blood.empid).html(blood.empCode));
                });

            },
            error: function (msg) {
            }
        });
    },

    UpdateDropDown: function () {
        ele1 = $("#UpdComp1");
        $Dropdown.FillDropdown(ele1, $BAComponent.templist1, $BAComponent.templist2);
        debugger;
        var table = document.getElementById('Table1');
        if (table.rows.length > 0) {
            var rows = table.rows.length;
            for (i = 0; i < rows; i++) {
                var currentRow = table.rows[i];
                var col1_value = currentRow.cells[3].innerText;
                var list = document.getElementById('UpdComp1');
                var opcount = document.getElementById('UpdComp1').length;
                var index = -1;
                for (k = 0; k < opcount; k++) {
                    if (list.options[k].value == col1_value) {
                        index = list.options[k].index;
                    }
                }
                if (index > -1) {
                    list.remove(index);
                }

            }
        }
        return true;
    },

    AddTable: function () {
        debugger;
        val1 = $('#UpdComp1 option:selected').text();
        val3 = Number($('#txtId1Amt').val());
        if ($.trim(val1) == "" || Number(val3) == NaN || Number(val3) == 0) {
            var msg = "Entry Not Valid";
            this.warnmsg(msg);
            return false;
        }

        if ($BAComponent.newtable == "Y") {
            $BAComponent.CreateTable();
        }

        $BAComponent.AddRow();

        var total = 0;
        var opt_sw = "";
        var trcount = $('#Table1 tr').length;
        if (trcount > 0) {
            var table = document.getElementById('Table1');
            if (table.rows.length > 0) {
                var rows = table.rows.length;
                for (i = 0; i < rows; i++) {
                    var currentRow = table.rows[i];
                    var total = total + Number(currentRow.cells[1].innerText);
                    if (currentRow.cells[0].innerText == "Basket Allowance") {
                        opt_sw = "Y";
                    }

                }
            }
        }
        debugger;
        if (opt_sw == "Y") {
            $('#OptSave').removeClass('nodisp');
        }
        else {
            $('#OptSave').addClass('nodisp');
        }

        $('#total').val(total);
        $('#txtId1Amt').val('');
    },

    CreateTable: function () {
        $BAComponent.newtable = "";
        $BAComponent.rowref = 0;
        $BAComponent.arrHead = new Array();
        $BAComponent.arrHead = ['ID', 'Value', 'Option'];

        var Details = $('<table border="1" padding= "10px" width="auto"></table>');
        Details.attr('id', 'Table1');
        $('#TableArea').html(Details);

        var tdarea = "";
        for (var h = 0; h < $BAComponent.arrHead.length; h++) {
            td = "<td style=text-align:center>" + $BAComponent.arrHead[h] + "</td>";
            tdarea = tdarea + td;
        }
        debugger;
        var th = tdarea;
        $('#Table1').append(th);
    },

    AddRow: function () {
        debugger;
        var tdarea = "";
        $BAComponent.rowref = $BAComponent.rowref + 1;
        val1 = $('#UpdComp1 option:selected').text();
        val2 = $('#UpdComp1 option:selected').val();
        for (var c = 0; c < 4; c++) {
            if (c == 0) {
                var ele = "<label>" + val1 + "</label>";
                td = "<td style=text-align:center>" + ele + "</td>";
            }

            if (c == 1) {
                val3 = $('#txtId1Amt').val();
                var ele = "<label>" + val3 + "</label>";
                td = "<td style=text-align:center>" + ele + "</td>";
            }

            if (c == 2) {
                var button = "<input type=button class='btn-group btn-corner' style='background-color:green;color:white' onclick='$BAComponent.RemoveRow(" + $BAComponent.rowref + ")' value = 'Delete'/>"
                td = "<td style='text-align:center;margin:10px'>" + button + "</td>";
            }

            if (c == 3) {
                var ele1 = "<label>" + val2 + "</label>";
                td = "<td class='nodisp' style=text-align:center>" + ele1 + "</td>";
            }

            tdarea = tdarea + td;
        }
        tr = "<tr id=" + $BAComponent.rowref + ">" + tdarea + "</tr>";
        $('#Table1').append(tr);
        if ($('#Table1 tr').length > 0) {
            $('#Table1').show();
        }
        else {
            $('#Table1').hide();
        }

        $BAComponent.UpdateDropDown();
    },

    RemoveRow: function (rowno) {
        debugger;
        $('#' + rowno).remove();
        $BAComponent.UpdateDropDown();
        if ($('#Table1 tr').length == 0) {
            $('#Table1').hide();
        }

        var total = 0;
        var opt_sw = "";
        var trcount = $('#Table1 tr').length;
        if (trcount > 0) {
            var table = document.getElementById('Table1');
            if (table.rows.length > 0) {
                var rows = table.rows.length;
                for (i = 0; i < rows; i++) {
                    var currentRow = table.rows[i];
                    var total = total + Number(currentRow.cells[1].innerText);
                    if (currentRow.cells[0].innerText == "Basket Allowance") {
                        opt_sw = "Y";
                    }
                }
            }
        }

        if (opt_sw == "Y") {
            $('#OptSave').removeClass('nodisp');
        }
        else {
            $('#OptSave').addClass('nodisp');
        }

        $('#total').val(total);
    },

    warnmsg: function (msg) {
        $.alert({
            boxWidth: '600px',
            useBootstrap: false,
            icon: 'fa fa-warning',
            title: "Warning!",
            content: msg,
            type: 'red',
            typeAnimated: true,
        });
    },
}
