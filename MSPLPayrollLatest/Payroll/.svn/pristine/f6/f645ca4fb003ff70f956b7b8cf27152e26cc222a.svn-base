
$('#EmpCode').change(function () {
    $BAupd.LoadData();
});


$('#btn-add').click(function () {
    $BAupd.UpdateDropDown();
});





var $BAupd = {
    canSave: false,
    BACompId: '',
    selectedEmployeeId: null,
    selectlist: null,
    templist1: [],
    templist2: [],
    templist3: [],
    EmployeeId: null,
    payhistory: null,
    financeYear: null,
    INPdate: null,
    cutoffdate: null,
    datalist: null,
    TotBallw: null,
    newtable: null,
    arrHead: null,
    finalBallw: null,
    rowref: null,
    EntityId: null,
    EntityModelId: null,
    InitRtn: null,
    serverDate: null,
    serverDate1: null,
    cutoffdateDisp: null,
    serverDate2:null,


    LoadInitial: function () {
        var date1 = new Date($BAupd.serverDate);
        $BAupd.serverDate1 = (date1.getFullYear() * 10000) + ((date1.getMonth() + 1) * 100) + date1.getDate();
        $BAupd.serverDate2 = date1.getDate();
        $BAupd.GetInpDate();
        $BAupd.GetDefaultfinyear();
        $BAupd.GetEmployee();
    },


    LoadBAComponents: function () {
        debugger;
        $('#data1').addClass('nodisp');
        $('#data').addClass('nodisp');
        $('#OptSave').addClass('nodisp');
        $('#Table1').html('');
        $('#Table1').val('');
        $('#name').html('');
        $BAupd.newtable = "Y";
        $('#txtId1Amt').removeAttr('disabled');
        $('#UpdComp1').removeAttr('disabled');
        $('#btn-add').removeAttr('disabled');
        $BAupd.finalBallw = 0;
    },

    UpdateDropDown: function () {
        ele1 = $("#UpdComp1");
        $BAupd.FillDropdown(ele1, $BAupd.templist1, $BAupd.templist2);
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

    LoadData: function () {
        debugger;
        var emp = $('#EmpCode option:selected').val();
        $BAupd.GetPayHistory(emp);
        $BAupd.checkTempData(emp);
        $BAupd.EntityId = $BAupd.datalist.EntityId;
        $BAupd.EntityModelId = $BAupd.datalist.EntityModelId;
        $BAupd.selectlist = $BAupd.datalist.AttributeModels;
        $BAupd.newtable = "Y";
        $BAupd.TotBallw = 0;
        $BAupd.InitRtn = "Y";
        var data2 = $BAupd.selectlist;
        $BAupd.templist1 = []; $BAupd.templist2 = [];
        if (data2.length > 0) {
            for (var i in data2) {
                $BAupd.templist1.push(data2[i].Id)
                $BAupd.templist2.push(data2[i].DisplayAs);
            }
        }
        debugger;
        if ($BAupd.cutoffdate < $BAupd.serverDate1) {
            $('#OptSave').hide();
            var msg = "!Cannot Enter Data after cutoff date </br>" + " CUTOFF DATE : " + $BAupd.cutoffdateDisp
            this.warnmsg(msg);
            return false;
        }



        $('#msg').html('');
        if ($BAupd.payhistory.length > 0) {
            $('#OptSave').hide();
            var msg = "!Salary Already processed for this Employee, Cannot Update Data";
            this.warnmsg(msg);
            $('#txtId1Amt').attr('disabled', true);
            $('#UpdComp1').attr('disabled', true);
        }
       else {
            debugger;
            var name = "Name : " + $BAupd.datalist.emp.FirstName + "  " + $BAupd.datalist.emp.LastName;
            $('#txtId1Amt').removeAttr('disabled');
            $('#UpdComp1').removeAttr('disabled');
            $('#name').text(name);
            var Details = $('<table border="0" style=width:auto></table>');
            Details.attr('id', 'Table2');
            $('#lblComp1').html('');
            $('#lblComp1').html(Details);
            var arrHead = new Array();
            arrHead = ['ID', 'Value','Ref'];
            for (var h = 0; h < arrHead.length; h++) {
                if (h == 2) {
                    var tdarea = tdarea + "<td class='nodisp'>" + arrHead[h] + "</td>";
                }
                else {
                    var tdarea = tdarea + "<td>" + arrHead[h] + "</td>";
                }
            }
            var th = "<tr>" + tdarea + "</tr>";
            $('#Table2').append(th);
            if ($BAupd.datalist.entTemp.length > 0) {
                var data3 = $BAupd.datalist.entTemp;
                var total = 0;
                for (i = 0; i < data3.length; i++) {
                    var text1 = ""; var index = "";
                    for (k = 0; k < $BAupd.templist1.length; k++) {
                        if ($BAupd.templist1[k] == data3[i].CompId) {
                            text1 = $BAupd.templist2[k];
                            index = k;
                        }
                        if (text1 == "Basket Allowance") {
                            $BAupd.TotBallw = Number(data3[i].Value);
                        }
                    }
                    debugger;
                    if (index != -1) {
                        $BAupd.templist1.splice(index, 1);
                        $BAupd.templist2.splice(index,1);
                    }
                    total = Number(total) + Number(data3[i].Value);
                    var ele1 = data3[i].CompId;
                    var td = "<td class='nodisp'>" + ele1 + "</td>";
                    var th = "<tr><td>" + text1 + "</td>" + "<td>" + data3[i].Value + "</td>" + td + "</tr>"
                    $('#Table2').append(th);
                }
                var th = "<tr><td>" + "TOTAL" + "</td>" + "<td>" + total + "</td></tr>"
                $('#Table2').append(th);
            }
            else {
                var msg = "DATA NOT AVAILABLE";
                this.warnmsg(msg);
                return false;
            }

            $('#data').removeClass('nodisp');
            $('#data1').removeClass('nodisp');

            if ($BAupd.datalist.entityMasterValues.length > 0) {
                $('#TableArea').html('');
                var name = "Name : " + $BAupd.datalist.emp.FirstName + "  " + $BAupd.datalist.emp.LastName;
                $('#txtId1Amt').removeAttr('disabled');
                $('#UpdComp1').removeAttr('disabled');
                $BAupd.InitRtn = "Y";
                $('#name').text(name);
                if ($BAupd.datalist.entityMasterValues.length > 0) {
                    var data3 = $BAupd.datalist.entityMasterValues;
                    var data2 = $BAupd.selectlist;
                    for (i = 0; i < data3.length; i++) {
                        if (data3[i].Value != 0) {
                            if ($BAupd.newtable == "Y") {
                                $BAupd.CreateTable();
                            }
                            $('#UpdComp1').html('');
                            var text1 = "";
                            for (k = 0; k < $BAupd.templist1.length; k++) {
                                if ($BAupd.templist1[k] == data3[i].AttributeModelId) {
                                    text1 = $BAupd.templist2[k];
                                    break;
                                }
                            }
                            if (text1 != "") {
                                $('#UpdComp1').append($("<option selected></option>").val(data3[i].AttributeModelId).html(text1));
                                $('#txtId1Amt').val(data3[i].Value);
                                $BAupd.AddTable();
                            }
                        }
                    }
                }
            }
        }
        debugger;
        $BAupd.InitRtn = "";
        $BAupd.UpdateDropDown();
    },

    GetDefaultfinyear: function () {
        debugger;
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() == "EMPLOYEE") {
            date2 = new Date($BAupd.cutoffdate);
            $BAupd.financeYear = $companyCom.EmployeeDefaultFinanceYear(date2);
        }
        else {
            $BAupd.financeYear = $companyCom.getDefaultFinanceYear();
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
                        $BAupd.INPdate = jsonResult.result.TDSdays;
                        var locale = "en-us";
                        var cudate = new Date(parseInt(jsonResult.result.cutoffdate.replace(/(^.*\()|([+-].*$)/g, '')));
                        $BAupd.cutoffdate = (cudate.getFullYear() * 10000) + ((cudate.getMonth() + 1) * 100) + cudate.getDate();
                        $BAupd.cutoffdateDisp = cudate.getDate() + "/" + (cudate.getMonth() + 1) + "/" + cudate.getFullYear();
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



    GetEmployee: function () {
        debugger;
        var condi = "Ballw.";
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



    checkTempData: function (emp) {
        debugger;
        $app.showProgressModel();
        $BAupd.datalist = '';
        var financeyear = $BAupd.financeYear.id;
        $.ajax({
            async: false,
            url: $app.baseUrl + "Entity/GetEntityTempData",
            data: JSON.stringify({
                refEntityId: emp, financeyear: financeyear
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $BAupd.GetData(jsonResult.result);
                        $app.hideProgressModel();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        $app.hideProgressModel();
                        break;

                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },

    GetData: function (result) {
        debugger;
        $BAupd.datalist = '';
        if (result.AttributeModels.length > 0) {
            $BAupd.datalist = result;
        }

    },

    closeMsg: function () {
        $.confirm({
            icon: "fa fa-info-circle",
            title: 'Confirm! ',
            content: 'Quit Without Saving Data!',
            type: 'green',
            buttons: {
                Yes: function () {
                    $BAupd.LoadBAComponents();
                },
                No: function () {
                },
            }
        });
    },

    Getopt: function () {
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if ($BAupd.cutoffdate < $BAupd.serverDate1) {
                alert("You can update Entries only upto " + $BAupd.cutoffdateDisp);
                $('#OptSave').addClass('nodisp');
            }
        }
    },

    month_name: function (dt) {
        mlist = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        return mlist[dt.getMonth()];
    },


    save: function () {
        debugger;
        var emp = $('#EmpCode option:selected').val();
        var array1 = [];
        var total = 0;
        var tableData = document.getElementById('Table2');
        if (tableData.rows.length > 0) {
            var rows = tableData.rows.length;
            for (i = 1; i < rows - 1; i++) {
                var currentrow = tableData.rows[i];
                if (currentrow.cells[0].innerText != "Basket Allowance") {
                    var data1 = {
                        id: currentrow.cells[2].innerText,
                        name: currentrow.cells[0].innerText,
                        value: Number(currentrow.cells[1].innerText),
                    }
                    array1.push(data1);
                }
            }
        }

        var tableData = document.getElementById('Table1');
        if (tableData.rows.length > 0) {
            var rows = tableData.rows.length;
            for (i = 0; i < rows; i++) {
                var currentrow = tableData.rows[i];
                total = total + Number(currentrow.cells[1].innerText);
                var data1 = {
                    id: currentrow.cells[3].innerText,
                    name: currentrow.cells[0].innerText,
                    value: Number(currentrow.cells[1].innerText),
                }
                array1.push(data1);
            }
        }
        var finalBAllw = $BAupd.TotBallw;
        var total1 = Number(finalBAllw);
        if (total1 != total) {
            var msg = "Basket Allowance Breakup Not Tally, Cannot Update Data";
            var msg1 = "BreakUp Total : " + total + " " + "Basket Allowance : " + total1;
            this.warnmsg(msg +"\n" + msg1);
            return false;
        }
        $('#UpdComp1 option').each(function () {
             var data1 = {
                  id: $(this).val(),
                  name: $(this).text(),
                  value: 0,
             }
                array1.push(data1);
        });

        var data = {
            entityId: $BAupd.EntityId,
            entityModelId: $BAupd.EntityModelId,
            EntityKeyValues: array1,
        };

        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Entity/SaveEntityMasterValue",
            data: JSON.stringify({ dataValue: data, refEntityId: emp, refEntityModel: 'Employee' }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $('#data').addClass('nodisp');
                        $('#data1').addClass('nodisp');
                        $('#OptSave').addClass('nodisp');
                        $BAupd.Getopt();
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

    GetPayHistory: function (emp) {
        $app.showProgressModel();
        var fromyear = new Date($BAupd.financeYear.startDate).getFullYear();
        var frommonth = new Date($BAupd.financeYear.startDate).getMonth() + 1;
        var toyear = new Date($BAupd.financeYear.EndDate).getFullYear();
        var tomonth = new Date($BAupd.financeYear.EndDate).getMonth() + 1;
        var EmployeeId = emp;
        $.ajax({
            async: false,
            url: $app.baseUrl + "TaxDeclaration/GetEmpPayhistory",
            data: JSON.stringify({ smonth: frommonth, syear: fromyear, nmonth: tomonth, nyear: toyear, EmployeeId: EmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $BAupd.GetHistory(jsonResult.result);
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
        $BAupd.payhistory = '';
        if (result.length > 0) {
            $BAupd.payhistory = result;
        }
    },

    FillDropdown: function (selector, selectlist1, selectlist2) {
        debugger;
        if (selectlist2.length > 0) {
             var vItems_1 = [];
             for (var i in selectlist2) {
                 if ($.trim(selectlist2[i]) != $.trim($BAupd.extext1) && ($.trim(selectlist2[i]) != $.trim($BAupd.extext2))) {
                     vItems_1.push("<option " + "value = " + selectlist1[i] + ">" + selectlist2[i] + "</option>");
                 }
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

    AddTable: function () {
        debugger;
        val1 = $('#UpdComp1 option:selected').text();
        val3 = Number($('#txtId1Amt').val());
        if ($.trim(val1) == "" || Number(val3) == NaN || Number(val3) == 0) {
            var msg = "Entry not Valid \n" + " Value : " + val3;
            this.warnmsg(msg);
            return false;
        }

        var amt3 = $BAupd.TotBallw;
        var total = 0;

        var trcount = $('#Table1 tr').length;
        if (trcount > 0) {
            var table = document.getElementById('Table1');
            if (table.rows.length > 0) {
                var rows = table.rows.length;
                for (i = 0; i < rows; i++) {
                    var currentRow = table.rows[i];
                    var total = Number(total) + Number(currentRow.cells[1].innerText);
                }
            }
        }
        $BAupd.finalBallw = 0;
        if ($BAupd.InitRtn != "Y") {
            if (amt3 < (Number(total) + Number(val3))) {
                var msg = "Total amount Exceeds Basket Allowance, cannot ADD";
                this.warnmsg(msg);
                return false;
            }
            else {
                $BAupd.finalBallw = Number(Number(amt3) - (Number(total) + Number(val3)));
            }
        }

        if ($BAupd.newtable == "Y") {
            $BAupd.CreateTable();
        }
        debugger;
        $BAupd.AddRow();
        $BAupd.finalBallw = Number(Number(amt3) - (Number(total) + Number(val3)));
        $('#txtId1Amt').val($BAupd.finalBallw);
        if ($BAupd.finalBallw == 0) {
            $('#txtId1Amt').attr('disabled', true);
            $('#UpdComp1').attr('disabled', true);
            $('#btn-add').attr('disabled', true);
            $('#btn-add').hide();
            $('#OptSave').removeClass('nodisp');
            $BAupd.Getopt();
        }
        else {
            $('#txtId1Amt').removeAttr('disabled');
            $('#UpdComp1').removeAttr('disabled');
            $('#btn-add').removeAttr('disabled');
            $('#btn-add').show();
        }
        
    },

    CreateTable: function () {
        $BAupd.newtable = "";
        $BAupd.rowref = 0;
        $BAupd.arrHead = new Array();
        $BAupd.arrHead = ['ID', 'Value','Option'];

        var Details =  $('<table border="1" padding= "10px" width="auto"></table>');
        Details.attr('id', 'Table1');
        $('#TableArea').html(Details);

        var tdarea = "";
        for (var h = 0; h < $BAupd.arrHead.length; h++) {
            td = "<td style=text-align:center>" + $BAupd.arrHead[h] + "</td>";
            tdarea = tdarea + td;
        }
        debugger;
        var th = tdarea;
        $('#Table1').append(th);
    },

    AddRow: function () {
        debugger;
        var tdarea = "";
        $BAupd.rowref = $BAupd.rowref + 1;
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
                var button = "<input type=button class='btn-group btn-corner' style='background-color:green;color:white' onclick='$BAupd.RemoveRow(" + $BAupd.rowref + ")' value = 'Delete'/>"
                td = "<td style='text-align:center;margin:10px'>" + button + "</td>";
            }

            if (c == 3) {
                var ele1 = "<label>" + val2 + "</label>";
                td = "<td class='nodisp' style=text-align:center>" + ele1 + "</td>";
            }

            tdarea = tdarea + td;
        }
        tr = "<tr id=" + $BAupd.rowref + ">" + tdarea + "</tr>";
        $('#Table1').append(tr);
        if ($('#Table1 tr').length > 0) {
            $('#Table1').show();
        }


    },

    RemoveRow: function (rowno) {
        debugger;
        $('#' + rowno).remove();
        $BAupd.UpdateDropDown();
        if ($('#Table1 tr').length == 0) {
            $('#Table1').hide();
        }

        var amt3 = $BAupd.TotBallw;
        var total = 0;
        $BAupd.finalBallw = 0;

        var trcount = $('#Table1 tr').length;
        if (trcount > 0) {
            var table = document.getElementById('Table1');
            if (table.rows.length > 0) {
                var rows = table.rows.length;
                for (i = 0; i < rows; i++) {
                    var currentRow = table.rows[i];
                    var total = total + Number(currentRow.cells[1].innerText);
                    $BAupd.finalBallw = Number(Number(amt3) - (Number(total)));
                }
            }
        }
        
        
        $('#txtId1Amt').val($BAupd.finalBallw);
        if ($BAupd.finalBallw == 0) {
            $('#txtId1Amt').attr('disabled', true);
            $('#UpdComp1').attr('disabled', true);
            $('#btn-add').attr('disabled', true);
            $('#btn-add').hide();
            $BAupd.Getopt();
        }
        else {
            $('#txtId1Amt').removeAttr('disabled');
            $('#UpdComp1').removeAttr('disabled');
            $('#btn-add').removeAttr('disabled');
            $('#btn-add').show();
            $BAupd.Getopt();
        }
    },

    warnmsg: function (msg) {
        $.alert({
            icon: 'fa fa-exclamation-triangle',
            boxWidth: '600px',
            useBootstrap:false,
            title: "Warning!",
            content: msg,
            type: 'red',
            typeAnimated: true,
        });
    },

};
