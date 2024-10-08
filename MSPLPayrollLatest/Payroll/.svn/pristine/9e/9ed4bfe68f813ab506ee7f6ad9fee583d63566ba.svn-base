$dwEarnings = {

    tableId: 'tblEarnings',
    formData: document.forms["frmDwEarnings"],
    settingFor: null,
    designForm: function (renderDiv) {

        var formrH = '<form id="frmDwEarnings"><center>  <h4>Employee Earnings Details</h4> </<center> <div class="row">';
        formrH = formrH + ' <div class="col-md-6">'; //first
        formrH = formrH + '<div class="form-horizontal"> <br/> <br/>';//start horizatal div


        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';//row end

        formrH = formrH + '<div class="col-md-12 table-responsive"><div id="dvDwEarningsTable"></div></div>';//for table
        formrH = formrH + '</form>';//form end
        $('#' + renderDiv).html(formrH);//DwEarningsHtml
        $dwEarnings.formData = document.forms["frmDwEarnings"];
        $('#dvDwEarningsTable').css('overflow-y', 'scroll');
        $('#dvDwEarningsTable').css('height', ($(window).height() - 250));
    },
    loadComponent: function (settingFor) {
        debugger;
        $dwEarnings.settingFor = settingFor;
        var gridObject = $dwEarnings.DwEarningsGridObject();
        var tableid = { id: $dwEarnings.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvDwEarningsTable').html(modelContent);
        var data = null;
        $dwEarnings.loadDwEarningsGrid(data, gridObject, tableid);
    },
    DwEarningsGridObject: function () {

        var gridObject = [];
        if ($dwEarnings.settingFor == 'Payslip') {
            gridObject = [

                    { tableHeader: "<input type='checkbox' id='chkEarnings' onchange=$Payslipsetting.SelectAll('" + $dwEarnings.tableId + "',this)>", tableValue: "fieldName", cssClass: 'nodisp' },
                    { tableHeader: "FieldName", tableValue: "fieldName", cssClass: 'nodisp' },
                       { tableHeader: "MatchingId", tableValue: "MatchingId", cssClass: 'MatchId' },
                    { tableHeader: "Field Name", tableValue: "displayAs", cssClass: '' },
                    { tableHeader: "Header Order", tableValue: 'hOrder', cssClass: 'displayorder txtHOrder' },
                    { tableHeader: "Earnings Order", tableValue: 'eOrder', cssClass: 'displayorder txtEOrder' },
                    { tableHeader: "Footer Order", tableValue: 'fOrder', cssClass: 'displayorder txtFOrder' },
                     { tableHeader: "Display As", tableValue: 'displayAs', cssClass: 'textbox txtDisplay' },

            ];
        } else {
            gridObject = [

                { tableHeader: "", tableValue: "fieldName", cssClass: 'checkbox' },
                { tableHeader: "Id", tableValue: "fieldName", cssClass: 'nodisp' },
                { tableHeader: "Field Name", tableValue: "displayAs", cssClass: '' },
                { tableHeader: "Order", tableValue: null, cssClass: 'displayorder txtOrder' },
                 { tableHeader: "Display As", tableValue: 'displayAs', cssClass: 'textbox txtDisplay' },
                //{ tableHeader: "Payslip Header Print", tableValue: "", cssClass: '' },
                //{ tableHeader: "Earnings Print Order", tableValue: "", cssClass: '' },
                //{ tableHeader: "Payslip Footer Print Order", tableValue: "", cssClass: '' },
            ];
        }
        return gridObject;
    },
    loadDwEarningsGrid: function (data, context, tableId) {
        //  debugger;
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {

            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
           else if (context[cnt].cssClass == 'MatchId') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass+" nodisp", "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" class="chkEarnings"  onchange="$PaySheet.displayAttrOrder(this)" id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            } else if (context[cnt].cssClass.includes('textbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        //debugger;
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
                        if ($dwEarnings.settingFor == 'Payslip') {
                            debugger;
                            if (sData == null) { sData = ''; }
                            var b = $('<input type="text" class="txtOrder"   onkeypress="return $validator.IsNumeric(event, this.id)" value="' + sData + '"  id="' + sData + '"/>');
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
                            var cls='';
                            if (oData.IsincludeGrosspay == true) {
                                b = $('<label class="includegross" style="background-color:#1ED2C7" value=' + sData + '>' + sData + ' </label>');
                            }
                            else {
                                b = $('<label class="notincludedgross" value=' + sData + '>' + sData + ' </label>');
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
            //    'scrollY': '50vh',
            'scrollCollapse': true,
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "DataWizard/GetEarningDeductionFields", //Get Attributemodel List for Field values
                    data: JSON.stringify({
                        'behaviorType': "Earning",
                        'IsPaysheet': $dwEarnings.settingFor

                    }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {


                        $PaySheetFilter.allowanceFields = jsonResult;
                        //$dwEarnings.DwEarningsRenderData(Rdata);
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
                $("#tblEarnings").removeClass("sorting_asc");
                var api = this.api();
                $('#tblEarnings input[type="checkBox"]').on('click', function () {

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
    DwEarningsRenderData: function (data) {
        var formData = document.forms["frmDwEarnings"];


    },
    loadEarningsGrid: function (data) {
        //debugger;
        $dwEarnings.settingFor = 'Payslip';
        var context = $dwEarnings.DwEarningsGridObject();
        var tableId = { id: $dwEarnings.tableId };
        var modelContent = $screen.createTable(tableId, context);
        $('#dvDwEarningsTable').html(modelContent);
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {

            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'MatchId') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass + " nodisp", "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" class="chkMaster" onchange="$PaySheet.displayAttrOrder(this)" id="' + sData + '"/>');
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
                        "aTargets": [cnt],
                        "sClass": "word-wrap",
                        "bSearchable": true,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var b;
                            if (oData.IsincludeGrosspay == true) {
                                b = $('<label class="includegross"  style="background-color:#1ED2C7"  value='+sData+'>' + sData + ' </label>');
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
        var dtClientList = $('#tblEarnings').DataTable({
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
                $('#tblEarnings tbody').on('dblclick', 'td:nth-child(4) input', function () {

                    if ($(this).val() == "") {
                        $Payslipsetting.headerDisplayOrder = $Payslipsetting.headerDisplayOrder + 1;
                        $(this).val($Payslipsetting.headerDisplayOrder);
                    } else {
                        $(this).val('');
                    }
                });
                $('#tblEarnings tbody').on('dblclick', 'td:nth-child(5) input', function () {
                    if ($(this).val() == "") {

                        $Payslipsetting.earningDisplayOrder = $Payslipsetting.earningDisplayOrder + 1;
                        $(this).val($Payslipsetting.earningDisplayOrder);
                    } else {
                        $(this).val('');
                    }
                });
                $('#tblEarnings tbody').on('dblclick', 'td:nth-child(6) input', function () {

                    if ($(this).val() == "") {
                        $Payslipsetting.footerDisplayOrder = $Payslipsetting.footerDisplayOrder + 1;
                        $(this).val($Payslipsetting.footerDisplayOrder);
                    } else {
                        $(this).val('');
                    }
                });
            }
        });
    }
}