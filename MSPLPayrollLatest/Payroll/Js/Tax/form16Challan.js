$('#sltCeMonth').change(function () {
    debugger;
    $form16Challan.remove();
});

$('#btnCeView').click(function () {
    debugger;
    if ($form16Challan.valiDation("view")) {
        $form16Challan.loadCompon();
    }
});
$('#btnCeSave').click(function () {
    if ($form16Challan.valiDation("save")) {
        $form16Challan.saveChallanEntry();
    }
});
$('#btnEditChallanEntry').click(function () {
    if ($form16Challan.valiDation("update")) {
        $form16Challan.UpdateChallanEntry();
    }
});


$form16Challan = {
    tableId: 'tbldwEmp',
    Id: '',
    FinanceYearId:'',
    setAtt: function () {
        debugger;
        var rows = $("#tbldwCat").dataTable().fnGetNodes();
        rows.forEach(function (item, index) {
            debugger;
            rows[index].onchange = function () { $form16Challan.remove(); }
            //$(rows[index]).attr("onchange","$form16Challan.remove();");
        });

    },
    remove: function () {
        debugger;
        var rows = $("#tbldwEmp").dataTable().fnGetNodes();
        rows.forEach(function (item, index) {
            $(rows[index]).find(".cbEmployee").prop("checked", false);
        });
        $("#tbldwEmp tr").remove();
    },
    financeYear: $companyCom.getDefaultFinanceYear(),
    valiDation: function (name) {
        debugger;
        if (name == "view") {
            var categoryID = new Array();
            var rows = $("#tbldwCat").dataTable().fnGetNodes();
            rows.forEach(function (item, index) {
                debugger;
                if ($(rows[index]).find(".cbCategory").prop("checked")) {
                    categoryID.push($(rows[index]).find(":eq(2)").html());
                }
            });
            if (!(categoryID.length > 0)) {
                $app.showAlert("Please Select Atleast One Category", 4)
                return false;
            }
        }
        if (name == "save") {
            var categoryID = new Array();
            var rows = $("#tbldwEmp").dataTable().fnGetNodes();
            rows.forEach(function (item, index) {
                debugger;
                if ($(rows[index]).find(".cbEmployee").prop("checked")) {
                    categoryID.push($(rows[index]).find(":eq(2)").html());
                }
            });
            if (!(categoryID.length > 0)) {
                $app.showAlert("Please Click View and select atleast One Employee", 4)
                return false;
            }
        }
        if ($('#sltCeMonth').val() == 0) {
            $app.showAlert("Please Select Month", 4)
            return false;
        }
        if ($('#txtChallanDate').datepicker('getDate') == null) {
            $app.showAlert("Please Select Challan Date", 4)
            return false;
        }
        if ($('#txtChallanNo').val() == "") {
            $app.showAlert("Please Enter Challan No", 4)
            return false;
        }
        if ($('#ddBank').val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert("Please Select Bank", 4)
            return false;
        }
        if ($('#txtCheque').val() == "") {
            $app.showAlert("Please Enter Check/DD", 4)
            return false;
        }
        if ($('#txtBSRCode').val() == "") {
            $app.showAlert("Please Enter BSRCode", 4)
            return false;
        }
        if ($('#bookEntry').val() == 0) {
            $app.showAlert("Please Select Book Entry", 4)
            return false;
        }
        else {
            return true;
        }

    },
    clearData: function () {
        var emprows = $("#tbldwEmp").dataTable().fnGetNodes();
        var catrows = $("#tbldwCat").dataTable().fnGetNodes();
        $('#cbEmployee').prop("checked", false);
        $('#cbCategory').prop("checked", false);
        for (var i = 0; i < emprows.length; i++) {
            $(emprows[i]).find(".cbEmployee").prop("checked", false);
        }
        for (var i = 0; i < catrows.length; i++) {
            $(catrows[i]).find(".cbCategory").prop("checked", false);
        }
        $('#sltCeMonth').val(0);
        $('#txtChallanDate').val(null);
        $('#txtChallanNo').val("");
        $('#ddBank').val("00000000-0000-0000-0000-000000000000");
        $('#txtCheque').val("");
        $('#txtBSRCode').val("");
        $('#bookEntry').val(0);


    },
    loadCompon: function () {
        debugger;
        var gridObject = $form16Challan.dwcategoryGridObject();
        var tableid = { id: $form16Challan.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvFromEmployeeTable').html(modelContent);
        var data = null;
        $form16Challan.loadDwcategoryGrid(data, gridObject, tableid);
    },
    dwcategoryGridObject: function () {
        debugger;
        var gridObject = [
                { tableHeader: "<input type='checkbox' id='cbEmployee' onchange=$Payslipsetting.SelectAll('" + $form16Challan.tableId + "',this)>", tableValue: "Id", cssClass: 'checkbox' },
                { tableHeader: "Id", tableValue: "Id", cssClass: 'nodisp' },
                { tableHeader: "Code", tableValue: "EmployeeCode", cssClass: '' },
                { tableHeader: "Employee Name", tableValue: "Name", cssClass: '' },
                { tableHeader: "Tax Amount", tableValue: "TaxAmount", cssClass: '' },
        ];
        return gridObject;
    },
    loadDwcategoryGrid: function (data, context, tableId) {
        debugger;
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'checkbox') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" class="cbEmployee" id="' + sData + '"/>');
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
            //"aaData": data,
            ajax: function (data, callback, settings) {
                var dataView = new Object();
                dataView.categoryID = new Array();
                var rows = $("#tbldwCat").dataTable().fnGetNodes();
                rows.forEach(function (item, index) {
                    debugger;
                    if ($(rows[index]).find(".cbCategory").prop("checked")) {
                        dataView.categoryID.push($(rows[index]).find(":eq(2)").html());
                    }
                });
                dataView.month = $('#sltCeMonth').val();
                dataView.challanDate = $('#txtChallanDate').datepicker('getDate');
                dataView.challanNo = $('#txtChallanNo').val();
                dataView.bankName = $('#ddBank').val();
                dataView.checkdd = $('#txtCheque').val();
                dataView.BSRCode = $('#txtBSRCode').val();
                dataView.bookEntry = $('#bookEntry').val() == 1 ? true : $('#bookEntry').val() == 2 ? false : null;
                dataView.financialYear = $form16Challan.financeYear.id;

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxDeclaration/GetChallanEmployeeEntry",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ data: dataView }),
                    dataType: "json",
                    success: function (jsonResult) {
                        var Rdata = jsonResult.result;
                        var out = Rdata;
                        $form16Challan.setAtt();
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

    saveChallanEntry: function () {
        $app.showProgressModel();
        var dataView = new Object();
        dataView.employeeID = new Array();
        var rows = $("#tbldwEmp").dataTable().fnGetNodes();
        rows.forEach(function (item, index) {
            debugger;
            if ($(rows[index]).find(".cbEmployee").prop("checked")) {
                dataView.employeeID.push($(rows[index]).find(":eq(2)").html());
            }
        });
        dataView.month = $('#sltCeMonth').val();
        dataView.challanDate = $('#txtChallanDate').datepicker('getDate');
        dataView.challanNo = $('#txtChallanNo').val();
        dataView.bankName = $('#ddBank').val();
        dataView.checkdd = $('#txtCheque').val();
        dataView.BSRCode = $('#txtBSRCode').val();
        dataView.bookEntry = $('#bookEntry').val() == 1 ? true : $('#bookEntry').val() == 2 ? false : null;
        dataView.financialYear = $form16Challan.financeYear.id;

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxDeclaration/SaveChallanEntry",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ data: dataView }),
            dataType: "json",
            success: function (jsonResult) {

                var Rdata = jsonResult.result;
                var out = Rdata;
                $form16Challan.clearData();
                $app.hideProgressModel();
                $app.showAlert(jsonResult.Message, 2);

            },
            error: function (msg) {
            }
        });
    },
    LoadViewData: function () {
        debugger;
        var dtClientList = $('#tblChallanView').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [

                     {
                         "data": "challanDate",
                         render: function (data) {
                             debugger;
                             var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                             var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateF;

                         }
                     },

                     { "data": "bank" },

                     { "data": "challanAmount" },

                     { "data": "PayrollMonth" },

                     { "data": null }

            ],

            "aoColumnDefs": [
                 {
                     "aTargets": [0],
                     "sClass": "word-wrap"
                 },
                  {
                      "aTargets": [1],
                      "sClass": "word-wrap"
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
                   debugger;
                   var c = $('<input type="button" id="btnViewStat" value="View" data-target="#challanDateList" data-toggle="modal"  class="btn custom-button marginbt7"  />');
                   c.button();
                   c.on('click', function () {
                       debugger;
                       $form16Challan.LoadViewDataList(sData);

                   });

                   $(nTd).empty();
                   $(nTd).prepend(c);
               }
           }],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxDeclaration/GetChallanEmployeeView",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ finId: $form16Challan.financeYear.id }),
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
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
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },

    LoadViewDataList: function (inp) {
        var date = new Date(parseInt(inp.challanDate.replace(/(^.*\()|([+-].*$)/g, '')));
        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
        debugger;
        var dtClientList = $('#tblChallanViewList').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [

                     {
                         "data": "EmployeeCode",

                     },

                     { "data": "Name" },

                     { "data": "TaxAmount" },

                      { "data": "challanNo" },

                      { "data": "checkdd" },
                      { "data": null }
            ],

            "aoColumnDefs": [
                 {
                     "aTargets": [0],
                     "sClass": "word-wrap"
                 },
                  {
                      "aTargets": [1],
                      "sClass": "word-wrap"
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
                      },
                      {
                          "aTargets": [5],
                          "sClass": "actionColumn",
                          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                              b.button();
                              b.on('click', function () {
                                  $('#challenEntryEdit').modal('toggle');
                                  $form16Challan.RenderData(oData);
                                 
                                  return false;
                              });
                              c.button();
                              c.on('click', function () {
                                  if (confirm("Are you sure, do you want to Delete?"))
                                      $form16Challan.DeleteData(oData);
                                  return false;
                              });
                              $(nTd).empty();
                              $(nTd).append(b, c);

                          }
                      }


            ],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxDeclaration/GetChallanEmployeeListView",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ finId: $form16Challan.financeYear.id, challanDate: dateF, bankId: inp.bankName }),
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
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
        debugger;
        var date = new Date(parseInt(context.ApplyDate.replace(/(^.*\()|([+-].*$)/g, '')));
        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
        context.ApplyDate = dateF;
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/DeleteEmployeeChallanEntry",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ data: context }),
            dataType: "json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                     
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
        debugger;
        var date = new Date(parseInt(data.ApplyDate.replace(/(^.*\()|([+-].*$)/g, '')));
        var challandate = new Date(parseInt(data.challanDate.replace(/(^.*\()|([+-].*$)/g, '')));
        var datech = challandate.getDate() + '/' + $payroll.GetMonthName((challandate.getMonth() + 1)) + '/' + challandate.getFullYear();
        data.challanDate = datech;
        var formData = document.forms["frmEditChallanEntry"];
        $form16Challan.Id = data.Id;
        $form16Challan.FinanceYearId= data.FinanceYearId;
        formData.elements["sltCeMonth"].value = date.getMonth() + 1;
        formData.elements["txtChallanDate"].value = data.challanDate;
        formData.elements["txtChallanNo"].value = data.challanNo;
        formData.elements["ddBank"].value = data.bankId;
        formData.elements["txtCheque"].value = data.checkdd;
        formData.elements["bookEntry"].value = data.bookEntry==true?"1":"2";
        formData.elements["txtBSRCode"].value = data.BSRCode;
    },
    UpdateChallanEntry: function () {
        $app.showProgressModel();
        var dataView = new Object();       
        dataView.month = $('#sltCeMonth').val();
        dataView.challanDate = $('#txtChallanDate').val();
        dataView.challanNo = $('#txtChallanNo').val();
        dataView.bankName = $('#ddBank').val();
        dataView.checkdd = $('#txtCheque').val();
        dataView.BSRCode = $('#txtBSRCode').val();
        dataView.bookEntry = $('#bookEntry').val() == 1 ? true : $('#bookEntry').val() == 2 ? false : null;
        dataView.financialYear = $form16Challan.FinanceYearId;
        dataView.Id = $form16Challan.Id;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxDeclaration/UpdateChallanEntry",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ data: dataView }),
            dataType: "json",
            success: function (jsonResult) {

                var Rdata = jsonResult.result;
                var out = Rdata;
                $form16Challan.clearData();
                $app.hideProgressModel();
                $app.showAlert(jsonResult.Message, 2);
                $('#challenEntryEdit').modal('toggle');
                var table = $('#tblChallanViewList').DataTable();
                table.ajax.reload();
            },
            error: function (msg) {
            }
        });
        $form16Challan.FinanceYearId = '';
        $form16Challan.Id='';
    },

}


