$dwEarnings = {
    tableId: 'tbldwEar',
    formData: document.forms["frmDwcategory"],
    designForm: function (renderDiv) {
        debugger;
        var formrH = '<form id="frmDwcategory">   <div class="row"> ';
        formrH = formrH + ' <div class="col-md-6">'; //first
        formrH = formrH + '<div class="form-horizontal">';//start horizatal divF    

        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';//row end

        formrH = formrH + '<div class="col-md-12 table-responsive"><h4>Earnings</h4><div id="dvDwcategoryTable"></div></div>';//for table
        formrH = formrH + '</form>';//form end
        $('#' + renderDiv).html(formrH);//DwcategoryHtml
        $dwEarnings.formData = document.forms["frmDwcategory"];
    },
    loadComponent: function () {

        var gridObject = $dwEarnings.dwcategoryGridObject();
        var tableid = { id: $dwEarnings.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvDwcategoryTable').html(modelContent);
        var data = null;
        $dwEarnings.loadDwcategoryGrid(data, gridObject, tableid);
    },
    dwcategoryGridObject: function () {
        var gridObject = [
                  { tableHeader: "<input type='checkbox' id='chkEarnings' onchange=$Payslipsetting.SelectAll('" + $dwEarnings.tableId + "',this)>", tableValue: "fieldName", cssClass: 'checkbox' },
                  { tableHeader: "Id", tableValue: "fieldName", cssClass: 'nodisp' },
               { tableHeader: "Earnings", tableValue: "displayAs", cssClass: '' },
        ];
        return gridObject;
    }
    ,
    loadDwcategoryGrid: function (data, context, tableId) {
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "bSort": false, "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'checkbox') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "bSort": false,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" class="chkEarnings" id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "word-wrap",
                        "bSearchable": true,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var b;
                            if (oData.IsincludeGrosspay == true) {
                                b = $('<label class="includegross"  style="background-color:#1ED2C7"  value=' + sData + '>' + sData + ' </label>');
                            }
                            else {
                                b = $('<label class="notincludedgross" value=' + sData + '>' + sData + ' </label>');
                            }
                            $(nTd).html(b);
                        }
                    });
                } else {
                    columnDef.push({
                        "aTargets": "", "sClass": "word-wrap",
                        "bSearchable": true
                    });
                }
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({

            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "paging": false,
            //"ordering": false,
            "scrollY": '300px',

            "paging": false,
            "info": false,
            "aoColumnDefs": columnDef,
            "drawCallback": function () { // this gets rid of duplicate headers
                $('.dataTables_scrollBody thead tr').css({ display: 'none' });
            },
            //"aaData": data,
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "DataWizard/GetEarningDeductionFields",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        'behaviorType': "Earning",
                        'IsPaysheet': $dwEarnings.settingFor

                    }),
                    dataType: "json",
                    success: function (jsonResult) {
                        var out = jsonResult;
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length,

                            });

                        }, 50);
                    },
                    error: function (msg) {
                    }
                });
            },


            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            },

        });
        var table = $('#' + tableId.id).DataTable();
        table.columns.adjust().draw();
    },

    dwcategoryRenderData: function (data) {
        var formData = document.forms["frmDwcategory"];
    }
}

$dwDed = {
    tableId: 'tbldwDed',
    formData: document.forms["frmDwded"],
    designForm: function (renderDiv) {
        debugger;
        var formrH = '<form id="frmDwcategory">   <div class="row"> ';
        formrH = formrH + ' <div class="col-md-6">'; //first
        formrH = formrH + '<div class="form-horizontal">';//start horizatal divF    

        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';//row end

        formrH = formrH + '<div class="col-md-12 table-responsive"><h4>Deduction</h4><div id="dvDwdedTable"></div></div>';//for table
        formrH = formrH + '</form>';//form end
        $('#' + renderDiv).html(formrH);//DwcategoryHtml
        $dwDed.formData = document.forms["frmDwded"];
    },
    loadComponent: function () {

        var gridObject = $dwDed.dwcategoryGridObject();
        var tableid = { id: $dwDed.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvDwdedTable').html(modelContent);
        var data = null;
        $dwDed.loadDwcategoryGrid(data, gridObject, tableid);
    },
    dwcategoryGridObject: function () {
        var gridObject = [
                  { tableHeader: "<input type='checkbox' id='chkDeductions' onchange=$Payslipsetting.SelectAll('" + $dwDed.tableId + "',this)>", tableValue: "fieldName", cssClass: 'checkbox' },
                  { tableHeader: "Id", tableValue: "fieldName", cssClass: 'nodisp' },
               { tableHeader: "Deduction", tableValue: "displayAs", cssClass: '' },
        ];
        return gridObject;
    },

    loadDwcategoryGrid: function (data, context, tableId) {
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "bSort": false, "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'checkbox') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn", "bSort": false,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" class="chkDeductions" id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "word-wrap",
                        "bSearchable": true,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var b;
                            if (oData.IsincludeGrosspay == true) {
                                b = $('<label class="includegross"  style="background-color:#1ED2C7"  value=' + sData + '>' + sData + ' </label>');
                            }
                            else {
                                b = $('<label class="notincludedgross" value=' + sData + '>' + sData + ' </label>');
                            }
                            $(nTd).html(b);
                        }
                    });
                } else {
                    columnDef.push({
                        "aTargets": "", "sClass": "word-wrap",
                        "bSearchable": true
                    });
                }
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({

            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "paging": false,
            "info": false,
            "scrollY": "300px",
            "aoColumnDefs": columnDef,
            "drawCallback": function () { // this gets rid of duplicate headers
                $('.dataTables_scrollBody thead tr').css({ display: 'none' });
            },
            //"aaData": data,
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "DataWizard/GetEarningDeductionFields",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        'behaviorType': "Deduction",
                        'IsPaysheet': $dwDed.settingFor

                    }),
                    dataType: "json",
                    success: function (jsonResult) {
                        var out = jsonResult;
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


            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    dwcategoryRenderData: function (data) {
        var formData = document.forms["frmDwded"];
    }
}

$salarysummary = {
    selectedattr: [],
    viewcomponents: function () {
        debugger;
        var attr = [];
        //Get Selected Earnings
        var rowsEar = $("#tbldwEar").dataTable().fnGetNodes();
        for (var i = 0; i < rowsEar.length; i++) {

            var newattr = new Object();
            if ($(rowsEar[i]).find(".chkEarnings").prop("checked")) {
                newattr.fieldName = $(rowsEar[i]).find(":eq(2)").html();
                newattr.type = "Earnings";
                newattr.displayas = $(rowsEar[i]).find(".includegross, .notincludedgross").html().trim();
                attr.push(newattr);
            }

        }

        //Get Selected Deduction
        var rowsDed = $("#tbldwDed").dataTable().fnGetNodes();
        for (var i = 0; i < rowsDed.length; i++) {

            var newattr = new Object();
            if ($(rowsDed[i]).find(".chkDeductions").prop("checked")) {
                newattr.fieldName = $(rowsDed[i]).find(":eq(2)").html();
                newattr.type = "Deduction";
                newattr.displayas = $(rowsDed[i]).find(".includegross, .notincludedgross").html().trim();
                attr.push(newattr);
            }

        }


        selectedattr = attr;
    },

    ViewSalarySummary: function () {

        $.ajax({
            url: $app.baseUrl + "DataWizard/ViewSalarySummary",
            data: JSON.stringify({ paysheetattr: selectedattr, category: $('#sltCategory').val(), EmpId: $('#sltEmployeelist').val(), smonth: $('#sltSMonth').val(), syear: $('#sltSYear').val(), nMonth: $('#sltNMonth').val(), nYear: $('#sltNYear').val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                var out = jsonResult.result;
                $salarysummary.LoadDataTableData(out);
            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },
    LoadDataTableData: function (data) {
        debugger;
        $("#tabletds").html('');
        $("#tabletds").append('<table cellpadding="0" cellspacing="0" border="0" class="" id="tblTdsReport"></table>');
        var dataObject = eval('[{"COLUMNS":[],"DATA":[]}]');
        for (i = 0; i < data.rowheader.length; i++) {
            if (data.rowheader[i]["type"] == "Static") {
                dataObject[0].COLUMNS[i] = { title: data.rowheader[i]["displayAs"], className: "dt-staticColor" }
            }
            else if (data.rowheader[i]["type"] == "Earnings") {
                dataObject[0].COLUMNS[i] = { title: data.rowheader[i]["displayAs"], className: "dt-earningColor" }
            }
            else if (data.rowheader[i]["type"] == "Deduction") {
                dataObject[0].COLUMNS[i] = { title: data.rowheader[i]["displayAs"], className: "dt-deductionColor" }
            }
        }
        for (i = 0; i < data.rows.length; i++) {
            dataObject[0].DATA[i] = data.rows[i];
        }
        dataObject[0].DATA[data.rows.length] = data.rowfooter;
        $('#tblTdsReport').dataTable({
         
            "data": dataObject[0].DATA,
            "columns": dataObject[0].COLUMNS,
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            pagingType: "full_numbers",
            searching: true,
            select: true,
            lengthChange: true,
            pageLength: 10,
            scrollX:true,
            scrollY: "550px",
            scrollCollapse: true,
            processing: true,
            // aaSorting: [[0, 'desc']],
            //"bSort": false,
            //dom: "rtiS",
            //bDestroy: true,
            //"scrollX": true,
        
        });





    },

}