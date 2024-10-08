$dwDeductions = {

    tableId: 'tblDeductions',
    formData: document.forms["frmDwDeductions"],
    settingFor: null,
    designForm: function (renderDiv) {

        var formrH = '<form id="frmDwDeductions"><center>  <h4>Employee Deductions Details</h4> </<center> <div class="row">';
        formrH = formrH + ' <div class="col-md-6">'; //first
        formrH = formrH + '<div class="form-horizontal"> <br/> <br/>';//start horizatal div


        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';//row end

        formrH = formrH + '<div class="col-md-12"><div id="dvDwDeductionsTable"></div></div>';//for table
        formrH = formrH + '</form>';//form end
        $('#' + renderDiv).html(formrH);//DwDeductionsHtml
        $dwDeductions.formData = document.forms["frmDwDeductions"];
        $('#dvDwDeductionsTable').css('overflow-y', 'scroll');
        //$('#dvDwDeductionsTable').css('overflow-x', 'scroll');
        $('#dvDwDeductionsTable').css('height', ($(window).height() - 250));
    },
    loadComponent: function (settingFor) {
        $dwDeductions.settingFor = settingFor;
        var gridObject = $dwDeductions.DwDeductionsGridObject();
        var tableid = { id: $dwDeductions.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvDwDeductionsTable').html(modelContent);
        var data = null;
        $dwDeductions.loadDwDeductionsGrid(data, gridObject, tableid);
    },
    DwDeductionsGridObject: function () {
        var gridObject;
        if ($dwDeductions.settingFor == 'Payslip') {
            gridObject = [
                { tableHeader: "<input type='checkbox' id='chkDeductions' onchange=$Payslipsetting.SelectAll('" + $dwDeductions.tableId + "',this)>", tableValue: "fieldName", cssClass: 'nodisp' },
                { tableHeader: "FieldName", tableValue: "fieldName", cssClass: 'nodisp' },

                { tableHeader: "Field Name", tableValue: "displayAs", cssClass: '' },
                { tableHeader: "Header Order", tableValue: 'hOrder', cssClass: 'displayorder txtHOrder' },
                { tableHeader: "Deductions Order", tableValue: 'dOrder', cssClass: 'displayorder txtDOrder' },
                { tableHeader: " Footer Order", tableValue: 'fOrder', cssClass: 'displayorder txtFOrder ' },
                { tableHeader: "Display as", tableValue: "displayAs", cssClass: 'textbox txtDisplay' },
            ];
        } else {
            gridObject = [
                { tableHeader: "", tableValue: "fieldName", cssClass: 'checkbox' },
                { tableHeader: "Id", tableValue: "fieldName", cssClass: 'nodisp' },

                { tableHeader: "Field Name", tableValue: "displayAs", cssClass: '' },
                { tableHeader: "Order", tableValue: null, cssClass: 'displayorder txtOrder' },
                { tableHeader: "Display as", tableValue: "displayAs", cssClass: 'textbox txtDisplay' },
                // { tableHeader: "Payslip Header Print", tableValue: "", cssClass: '' },
                //{ tableHeader: "Deductions Print Order", tableValue: "", cssClass: '' },
                //{ tableHeader: "Payslip Footer Print Order", tableValue: "", cssClass: '' },
            ];
        }
        return gridObject;
    },
    loadDwDeductionsGrid: function (data, context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {

            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" class="chkDeductions" onchange="$PaySheet.displayAttrOrder(this)" id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            } else if (context[cnt].cssClass.includes('textbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="text" class="txtDisplayAs"    value="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            }

            else if (context[cnt].cssClass.includes('displayorder')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        if ($dwDeductions.settingFor == 'Payslip') {
                            if (sData == null) { sData = ''; }
                            var b = $('<input type="text" class="txtOrder"   onkeypress="return $validator.IsNumeric(event, this.id)" value="' + sData + '" id="' + sData + '"/>');
                            $(nTd).html(b);
                        }
                        else {
                            var b = $('<input type="text" class="txtOrder" disabled  onkeypress="return $validator.IsNumeric(event, this.id)"  id="' + sData + '"/>');
                            $(nTd).html(b);
                        }
                    }
                });
            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({
                        "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var b;
                            if (oData.IsincludeGrosspay == true) {
                                b = $('<label class="includegross"  style="background-color:#1ED2C7">' + sData + ' </label>');
                            }
                            else {
                                b = $('<label class="notincludedgross">' + sData + ' </label>');
                            }
                            $(nTd).html(b);
                        }
                    });
                } else {
                    columnDef.push({ "aTargets": "", "sClass": "word-wrap", "bSearchable": true });
                }
            }

        }
        var dtClientList = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            //   'scrollY': '50vh',
            'scrollCollapse': true,
            autowidth: true,
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            "autoWidth": true,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "DataWizard/GetEarningDeductionFields",
                    data: JSON.stringify({
                        'behaviorType': "Deduction",
                        'IsPaysheet': $dwDeductions.settingFor
                    }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {


                        $PaySheetFilter.deductionsFields = jsonResult;
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
            fnInitComplete: function (oSettings, json) {
                var r = $('#' + tableId.id + ' tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + tableId.id + ' thead').append(r);
                $('#search_0').css('text-align', 'center');
            },

            fnDrawCallback: function () {
                $("#tblDeductions").removeClass("sorting_asc");
                var api = this.api();
                $('#tblDeductions input[type="checkBox"]').on('click', function () {

                    var tr = $(this).closest('tr');

                    if (this.checked) {

                        $(tr).find('.txtOrder').val($PaySheet.displayOrder);

                    } else {
                        $(tr).find('.txtOrder').val('');
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
    DwDeductionsRenderData: function (data) {
        var formData = document.forms["frmDwDeductions"];


    },
    loadDeductionsGrid: function (data) {
        $dwDeductions.settingFor = 'Payslip';
        var context = $dwDeductions.DwDeductionsGridObject();
        var tableId = { id: $dwDeductions.tableId };
        var modelContent = $screen.createTable(tableId, context);
        $('#dvDwDeductionsTable').html(modelContent);
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {

            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" class="chkMaster" id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            } else if (context[cnt].cssClass.includes('textbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        if (sData == null) { sData = ''; }
                        var b = $('<input type="text" class="txtDisplayAs"    value="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            }
            else if (context[cnt].cssClass.includes('displayorder')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        if (sData == null) { sData = ''; }
                        var b = $('<input type="text" class="txtOrder"   onkeypress="return $validator.IsNumeric(event, this.id)" value="' + sData + '"  id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({
                        "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var b;
                            if (oData.IsincludeGrosspay == true) {
                                b = $('<label class="includegross"  style="background-color:#1ED2C7">' + sData + ' </label>');
                            }
                            else {
                                b = $('<label class="notincludedgross">' + sData + ' </label>');
                            }
                            $(nTd).html(b);
                        }
                    });
                } else {
                    columnDef.push({ "aTargets": "", "sClass": "word-wrap", "bSearchable": true });
                }
            }

        }
        var dtClientList = $('#tblDeductions').DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            // 'scrollY': '50vh',
            'scrollCollapse': true,
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data,
            fnDrawCallback: function () {
                $('#tblDeductions tbody').on('dblclick', 'td:nth-child(4) input', function () {

                    if ($(this).val() == "") {
                        $Payslipsetting.headerDisplayOrder = $Payslipsetting.headerDisplayOrder + 1;
                        $(this).val($Payslipsetting.headerDisplayOrder);
                    } else {
                        $(this).val('');
                    }
                });
                $('#tblDeductions tbody').on('dblclick', 'td:nth-child(5) input', function () {

                    if ($(this).val() == "") {
                        $Payslipsetting.deductionDisplayOrder = $Payslipsetting.deductionDisplayOrder + 1;
                        $(this).val($Payslipsetting.deductionDisplayOrder);
                    } else {
                        $(this).val('');
                    }
                });
                $('#tblDeductions tbody').on('dblclick', 'td:nth-child(6) input', function () {

                    if ($(this).val() == "") {
                        $Payslipsetting.footerDisplayOrder = $Payslipsetting.footerDisplayOrder + 1;
                        $(this).val($Payslipsetting.footerDisplayOrder);
                    } else {
                        $(this).val('');
                    }
                });
            }
        });
    },
    //created by Maddy
    FlexiPayReport: function () {
        debugger
        var input = $("#sltFromEmpCode").val();
        $.ajax({
            url: $app.baseUrl + "DataWizard/FlxiPayReoprt",
            data: JSON.stringify({ SelectInput: input }),
            type: "Post",
            async: false,
            dataType: "Json",
            contentType: "application/json",
            success: function (JsonResult) {
                debugger
                var result = JsonResult;
                if (JsonResult.Status == true) {
                    if (JsonResult.result.filePath != "" && JsonResult.result.filePath != undefined) {
                        var oData = new Object();
                        oData.filePath = JsonResult.result.filePath;
                        $app.downloadSync('Download/DownloadPaySlip', oData);
                    }
                    else if (JsonResult.result.length !== 0) {
                        debugger
                        $('#empFlex').removeClass('nodisp');
                        var tblData = JsonResult.result;
                        var jsonData = JSON.parse(tblData);
                        if ($.fn.DataTable.isDataTable('#tblempFlex')) {
                            // Destroy the existing DataTable
                            $('#tblempFlex').DataTable().destroy();
                        } 
                            $('#tblempFlex').DataTable({
                                paging: false,       // Disable paging
                                searching: false,    // Disable searching
                                lengthChange: false,  // Disable length change
                                 ordering:false
                            });
                         var tableBody = document.getElementById('tblempFlex').getElementsByTagName('tbody')[0];
                         // Clear all rows from the table body
                        tableBody.innerHTML = '';
                         // Loop through the data and append it to the DataTable
                        $.each(jsonData, function (index, row) {
                            var values = Object.values(row);
                            var rowHtml = '<tr><td>' + values.join('</td><td>') + '</td></tr>';
                            $('#tblempFlex tbody').append(rowHtml);
                        });
                     }
                }
                else {
                    $app.showAlert('No data found ', 1);
                    $app.hideProgressModel();
                }
            }

        })
    }
}