$dwCatMasterfield = {

    tableId: 'tblCatMasterfield',
    formData: document.forms["frmDwCatMasterfield"],
    settingFor: null,
    designForm: function (renderDiv) {

        var formrH = '<form id="frmDwCatMasterfield"><center>  <h4>Employee Master Details</h4> </<center> <div class="row">';
        formrH = formrH + ' <div class="col-md-6">'; //first
        formrH = formrH + '<div class="form-horizontal"> <br/> <br/>';//start horizatal div


        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';//row end

        formrH = formrH + '<div class="col-md-12 table-responsive"><div id="dvDwCatMasterfieldTable"></div></div>';//for table
        formrH = formrH + '</form>';//form end
        $('#' + renderDiv).html(formrH);//DwCatMasterfieldHtml
        $dwCatMasterfield.formData = document.forms["frmDwCatMasterfield"];
        $('#dvDwCatMasterfieldTable').css('overflow-y', 'scroll');
        $('#dvDwCatMasterfieldTable').css('height', ($(window).height() - 250));
    },
    loadComponent: function (settingFor) {
        debugger;
        $dwCatMasterfield.settingFor = settingFor;
        var gridObject = $dwCatMasterfield.DwCatMasterfieldGridObject();
        var tableid = { id: $dwCatMasterfield.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvDwCatMasterfieldTable').html(modelContent);
        var data = null;
        $dwCatMasterfield.loadDwCatMasterfieldGrid(data, gridObject, tableid);
    },
    DwCatMasterfieldGridObject: function () {
        var gridObject;
        if ($dwCatMasterfield.settingFor == 'Payslip') {
            gridObject = [
                 { tableHeader: "<input type='checkbox' id='chkMaster'onchange= $Payslipsetting.SelectAll('" + $dwCatMasterfield.tableId + "',this)>", tableValue: "Id", cssClass: 'nodisp' },
                 { tableHeader: "Id ", tableValue: "fieldName", cssClass: 'nodisp' },
                 { tableHeader: "Table Name", tableValue: "tableName", cssClass: 'nodisp' },
                 { tableHeader: "Field Name", tableValue: "fieldName", cssClass: '' },
                 { tableHeader: "Payslip Header Print", tableValue: "hOrder", cssClass: 'displayorder txtHOrder' },
                 { tableHeader: "Payslip Footer Print Order", tableValue: "fOrder", cssClass: 'displayorder txtFOrder' },
                 { tableHeader: "FullAndFinal Header Print", tableValue: "ffhOrder", cssClass: 'displayorder txtFFHOrder' },
                 { tableHeader: "Display as", tableValue: "displayAs", cssClass: 'textbox txtDisplay' },
               //  { tableHeader: "Attribute Id", tableValue: "attributeId", cssClass: 'nodisp attributeId' },
            ];
        }
        else {
            gridObject = [
                        { tableHeader: "", tableValue: "fieldName", cssClass: 'checkbox' },
                        { tableHeader: "fieldName", tableValue: "fieldName", cssClass: 'nodisp' },
                        { tableHeader: "Table Name", tableValue: "tableName", cssClass: 'nodisp' },
                        { tableHeader: "Field Name", tableValue: "fieldName", cssClass: '' },
                        { tableHeader: "Order", tableValue: null, cssClass: 'displayorder txtOrder' },
                        { tableHeader: "Display as", tableValue: "displayAs", cssClass: 'textbox txtDisplay' },
            { tableHeader: "Attribute Id", tableValue: "attributeId", cssClass: 'attributeId nodisp' },
                        //{ tableHeader: "Payslip Header Print", tableValue: "", cssClass: '' },
                        //{ tableHeader: "Payslip Footer Print Order", tableValue: "", cssClass: '' }
            ];
        }
        return gridObject;
    },
    loadDwCatMasterfieldGrid: function (data, context, tableId) {
        debugger;
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {

            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes( 'checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" class="chkMaster"  onchange="$PaySheet.displayAttrOrder(this)" id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            } else if (context[cnt].cssClass.includes( 'textbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn "+context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                         
                        var b = $('<input type="text" class="txtDisplayAs"  value="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            } else if (context[cnt].cssClass.includes( 'displayorder')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        if ($dwCatMasterfield.settingFor == 'Payslip') {
                            var b = $('<input type="text" class="txtOrder" onkeypress="return $validator.IsNumeric(event, this.id)"  id="' + sData + '"/>');
                            $(nTd).html(b);
                        }
                        else {
                            var b = $('<input type="text" class="txtOrder" disabled onkeypress="return $validator.IsNumeric(event, this.id)"  id="' + sData + '"/>');
                            $(nTd).html(b);
                        }
                        
                    }
                });
            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass + " word-wrap", "bSearchable": true });
                } else {
                    columnDef.push({ "aTargets": "", "sClass": context[cnt].cssClass + "word-wrap", "bSearchable": true });
                }
            }

        }
        var dtClientList = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            // 'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            // 'scrollY': '50vh',
            //  'scrollCollapse': true,
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "DataWizard/GetMasterFields",
                    data: JSON.stringify({
                        "tableName": "\'Employee\',\'Emp_Address\',\'Emp_Bank\',\'Emp_Personal\'",
                    }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {
                        
                        $PaySheetGrouping.masterFields = jsonResult;
                        $PaySheetFilter.masterFields = jsonResult;
                        //$dwCatMasterfield.DwCatMasterfieldRenderData(Rdata);
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
                $("#tblCatMasterfield").removeClass("sorting_asc");
                var api = this.api();
                $('#tblCatMasterfield input[type="checkBox"]').on('click', function () {

                    var tr = $(this).closest('tr');

                    if (this.checked) {

                        $(tr).find('.txtOrder').val($PaySheet.displayOrder);

                    } else {
                        $(tr).find('.txtOrder').val('');
                    }
                });
                $('#tblCatMasterfield tbody').on('click', 'td:nth-child(5) input', function () {
                    if ($dwCatMasterfield.settingFor == 'PaySlip') {
                        var tr = $(this).closest('tr');

                        if (this.checked) {

                            $(this).val($Payslipsetting.headerDisplayOrder);

                        } else {
                            $(this).val('');
                        }
                    }
                });
                $('#tblCatMasterfield tbody').on('dblclick', 'td:nth-child(6) input', function () {
                    if ($dwCatMasterfield.settingFor == 'PaySlip') {
                        if ($(this).val() == "") {
                            $Payslipsetting.footerDisplayOrder = $Payslipsetting.footerDisplayOrder + 1;
                            $(this).val($Payslipsetting.footerDisplayOrder);
                        } else {
                            $(this).val('');
                        }
                    }
                });
                $('#tblCatMasterfield tbody').on('dblclick', 'td:nth-child(7) input', function () {
                    debugger;
                     if ($dwCatMasterfield.settingFor == 'Payslip') {
                    if ($(this).val() == "") {
                        $Payslipsetting.fandfDisplayOrder = $Payslipsetting.fandfDisplayOrder + 1;
                        $(this).val($Payslipsetting.fandfDisplayOrder);
                    } else {
                        $(this).val('');
                    }}
                });


            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    DwCatMasterfieldRenderData: function (data) {
        var formData = document.forms["frmDwCatMasterfield"];
    },

    loadMasterGrid: function (data) {
        debugger;
        $dwCatMasterfield.settingFor = 'Payslip';
        var context = $dwCatMasterfield.DwCatMasterfieldGridObject();
        var tableId = { id: $dwCatMasterfield.tableId };
        var modelContent = $screen.createTable(tableId, context);
        $('#dvDwCatMasterfieldTable').html(modelContent);
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
            else if (context[cnt].cssClass.includes( 'displayorder')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn "+context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        if (sData == null) { sData = ''; }
                        var b = $('<input type="text" class="txtOrder"   onkeypress="return $validator.IsNumeric(event, this.id)"  value="' + sData + '" Id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
                } else {
                    columnDef.push({ "aTargets": "", "sClass": "word-wrap", "bSearchable": true });
                }
            }

        }
        var dtClientList = $('#tblCatMasterfield').DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data,
       //  'scrollY': '50vh',
            //'scrollCollapse': true,
            fnInitComplete: function (oSettings, json) {
                var r = $('#' + tableId.id + ' tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + tableId.id + ' thead').append(r);
                $('#search_0').css('text-align', 'center');
            }, fnDrawCallback: function () {
                $("#tblCatMasterfield").removeClass("sorting_asc");
                var api = this.api();
                $('#tblCatMasterfield input[type="checkBox"]').on('click', function () {

                    var tr = $(this).closest('tr');

                    if (this.checked) {

                        $(tr).find('.txtOrder').val($PaySheet.displayOrder);

                    } else {
                        $(tr).find('.txtOrder').val('');
                    }
                });
                $('#tblCatMasterfield tbody').on('click', 'td:nth-child(5) input', function () {
                    
                    if ($dwCatMasterfield.settingFor == 'Payslip') {
                        var tr = $(this).closest('tr');
                        $(this).val($Payslipsetting.headerDisplayOrder++);

                    }
                });
                $('#tblCatMasterfield tbody').on('dblclick', 'td:nth-child(6) input', function () {
                    if ($dwCatMasterfield.settingFor == 'Payslip') {
                        if ($(this).val() == "") {
                            $Payslipsetting.footerDisplayOrder = $Payslipsetting.footerDisplayOrder + 1;
                            $(this).val($Payslipsetting.footerDisplayOrder);
                        } else {
                            $(this).val('');
                        }
                    }
                });
                $('#tblCatMasterfield tbody').on('dblclick', 'td:nth-child(7) input', function () {
                    debugger;
                    if ($dwCatMasterfield.settingFor == 'Payslip') {
                    if ($(this).val() == "") {
                        $Payslipsetting.fandfDisplayOrder = $Payslipsetting.fandfDisplayOrder + 1;
                        $(this).val($Payslipsetting.fandfDisplayOrder);
                    } else {
                        $(this).val('');
                    }}
                });
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    }
}

