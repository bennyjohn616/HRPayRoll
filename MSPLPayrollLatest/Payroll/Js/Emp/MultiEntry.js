//$(document).ready(function () {
//    $multiEntryInput.LoadEntityModelDrop();
//});


$("#btnSave").click(function () {
    $multiEntryInput.save();
});

$('#sltCategory').change(function () {
    if ($('#sltEntityModel').val() != 0) {
        $multiEntryInput.selectedcategoryId = $('#sltCategory').val();
        $multiEntryInput.LoadGridData();
    }

});

$multiEntryInput = {
    selectedEntityModelId: null,
    selectedEntityModelname: null,
    selectedcategoryId: null,
    selectedEntityId: null,
    Alignment: function () {
        var a = $('.dataTables_scrollHeadInner table').first().css("margin-left");
        var b = $('.dataTables_scrollHeadInner table').first().css("width");
        $(".dataTables_scrollBody table").css({ 'margin-left': a, "width": b });
    },
    loadCategory: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetCategories",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var empid = $('#hdnEmployeeId').val();
                var sessionempid = msg.Message;
                var out = msg.result;

                if (sessionempid == "00000000-0000-0000-0000-000000000000") {
                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--AllCategory--'));
                    $.each(out, function (index, blood) {
                        $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.Name));
                    });
                }
                else {
                    $.each(out, function (index, blood) {
                        $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.Name));
                    });
                }

            },
            error: function (msg) {
            }
        });

    },
    LoadEntityModelDrop: function () {

        $multiEntryInput.LoadEntityModel({ id: 'sltEntityModel' });
        $multiEntryInput.loadCategory({ id: 'sltCategory' });
        $multiEntryInput.selectedcategoryId = $('#sltCategory').val();
    },
    LoadEntityModel: function (dropControl) {
        $("#clrcode").hide();
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetMonthlyEntityList",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                        $.each(out, function (index, object) {
                            $('#' + dropControl.id).append($("<option></option>").val(object.Id).html(object.Name));
                            $multiEntryInput.selectedEntityModelId = object.EntityModelId;
                        });
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
        $('#' + dropControl.id).change(function () {
            debugger;
            if (dropControl.id != "sltMasterEntityModel") {


                if ($('#' + dropControl.id).val() == 0) {
                    $("#dvDynamicEntity").html('');
                }
                else {
                    $multiEntryInput.selectedcategoryId = $('#sltCategory').val();
                    $multiEntryInput.selectedEntityModelname = $('#' + dropControl.id).find('option:selected').text();
                    $multiEntryInput.selectedEntityId = $('#' + dropControl.id).val();
                    $multiEntryInput.LoadGridData();
                }
            }
        });
    },
    renderFieldGrid: function (context, tableprop) {

        $("#dvTitle").html('');
        $("#dvTitle").html('<h4>' + $multiEntryInput.selectedEntityModelname + '</h4>');
        var grid = '<table id="tbl_' + tableprop.id + '" class="table table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer">'
            + '<thead>'
                    + '<tr>'
                    + '<th class="nodisp">'
                    + '</th>'
        for (var cnt = 1; cnt < context.length; cnt++) {
            if (context[cnt].fieldType == "earning") {
                grid = grid + '<th style="background-color:#1ED2C7">' + context[cnt].tableHeader + '</th>'
            }
            else if (context[cnt].fieldType == "deduction") {
                grid = grid + '<th style="background-color:#C1751A">' + context[cnt].tableHeader + '</th>'
            }
                //else if (context[cnt].fieldType == "others") {
                //    grid = grid + '<th style="background-color:#E04646">' + context[cnt].tableHeader + '</th>'
                //}
            else {
                grid = grid + '<th>' + context[cnt].tableHeader + '</th>'
            }
        }
        grid = grid + '</tr></thead>';
        grid = grid + '<tbody><tr>'
        for (var cnt = 0; cnt < context.length; cnt++) {//for action td 
            grid = grid + '<td></td>';
        }
        grid = grid + '</tr></tbody></table>';
        $("#dvDynamicEntity").html('');
        $("#dvDynamicEntity").html(grid);
    },
    renderGrid: function (context) {
        debugger;
        if (context.length <= 0) {
            $("#dvTitle").html('');
            $("#dvDynamicEntity").html('');
            context = null;
            return false;
        }
        var gridObject = [];
        gridObject.push({ tableHeader: "Id", tableValue: 'EmployeeId' });
        gridObject.push({ tableHeader: "Code", tableValue: 'EmployeeCode' });
        gridObject.push({ tableHeader: "Name", tableValue: 'EmployeeName' });
        for (var cont = 0; cont < context[0].MonthlyInputAttributes.length; cont++) {// context.jsonMonthlyInput.length
            gridObject.push({
                tableHeader: context[0].MonthlyInputAttributes[cont].Name,//"Employee Id",
                tableValue: 'MonthlyInputAttributes.' + cont + '.MIValue',
                fieldType: context[0].MonthlyInputAttributes[cont].AttributeBehaviorType,
                MIdisplay: context[0].MonthlyInputAttributes[cont].MIDisplay
            });
        }
        $multiEntryInput.renderFieldGrid(gridObject, { id: $multiEntryInput.selectedEntityId });
        $multiEntryInput.LoadAttributeModels(context, gridObject, { id: $multiEntryInput.selectedEntityId });
        $multiEntryInput.Alignment();
    },
    LoadGridData: function () {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetMasterInput",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                entitymodelId: $multiEntryInput.selectedEntityModelId,
                entityId: $multiEntryInput.selectedEntityId,
                categoryId: $multiEntryInput.selectedcategoryId,
                employeeId: "00000000-0000-0000-0000-000000000000"
            }),
            dataType: "json",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $multiEntryInput.renderGrid(out);
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
        //if (data != null) {
        //    columnsValue.push({ "data": null }); //for action column
        //} else {
        //    columnsValue.push({ "data": '' }); //for action column
        //}
        var columnDef = [];
        columnDef.push({ "aTargets": [0], "sClass": "nodisp", "bSearchable": false }); //for id column
        if (data != null) {
            for (var cnt1 = 3; cnt1 < context.length; cnt1++) {
                if (context[cnt1].MIdisplay == "LD") {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            //"sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                if (oData.editStatus == 'CanEdit') {
                                    //var b = $('<input type="text"  onkeyup="return $validator.moneyvalidation(this.id)" onkeypress="return $validator.moneyvalidation(this.id)" id="txt_' + oData.MonthlyInputAttributes[iCol - 3].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 3].MIValue + '" />');
                                    //$(nTd).html(b);
                                    var b = $('<input type="text"  onblur="return $validator.minmax(this.id)" onchange="return $validator.minmax(this.id)" class="' + oData.MonthlyInputAttributes[iCol - 3].Name + '" id="txt_' + oData.MonthlyInputAttributes[iCol - 3].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 3].MIValue + '" />');
                                    $(nTd).html(b);
                                }

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

                                if (oData.editStatus == 'CanEdit') {
                                    //var b = $('<input type="text"  onkeyup="return $validator.moneyvalidation(this.id)" onkeypress="return $validator.moneyvalidation(this.id)" id="txt_' + oData.MonthlyInputAttributes[iCol - 3].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 3].MIValue + '" />');
                                    //$(nTd).html(b);
                                    var b = $('<input type="text"  onkeyup="return $validator.moneyvalidation(this.id)" onkeypress="return $validator.moneyvalidation(this.id)" class="' + oData.MonthlyInputAttributes[iCol - 3].Name + '" id="txt_' + oData.MonthlyInputAttributes[iCol - 3].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 3].MIValue + '" />');
                                    $(nTd).html(b);
                                }

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
            // 'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            fixedHeader: true,
            "aoColumnDefs": columnDef,
            "aaData": data,
            "bFilter": true,
            "sSearch": "Search:",
            scrollX: true,
            scrollCollapse: true,
            paging: true,
            fixedColumns: {
                //heightMatch: 'auto',
                leftColumns: 3
            },
            autoFill: true,
            select: {
                style: 'os',
                selector: 'td:first-child',
                blurable: true
            },
            fnInitComplete: function (oSettings, json) {

                var r = $('#tblFormula tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblFormula thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            "drawCallback": function (settings) {

                //var a = $('.dataTables_scrollHeadInner table').first().css("margin-left");
                //var b = $('.dataTables_scrollHeadInner table').first().css("width");
                //$(".dataTables_scrollBody table").css({ 'margin-left': a, "width": b });
                //$(".dataTables_scrollBody").css({ 'overflow-y': 'hidden' });
                //$(".DTFC_LeftBodyLiner").css({ 'overflow': 'hidden' });
                //  $('.dataTables_scrollHeadInner table').clear();

            },
            // dom: "rtiS",

            scroller: {
                loadingIndicator: true
            }

        });


    },
    save: function (context) {

        $app.showProgressModel();
        var keyvalues = [];
        var RowsDateCheck = $('#tbl_' + $multiEntryInput.selectedEntityId).dataTable().fnGetNodes();
        $(RowsDateCheck).each(function (index, data) {
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
            keyvalues.push({ 'EmployeeId': dtValues[0].EmployeeId, 'Id': $multiEntryInput.selectedEntityId, MonthlyInputAttributes: rowVal });
        });
        $.ajax({
            url: $app.baseUrl + "Entity/SaveMultiEntryMasterInput",
            data: JSON.stringify({ dataValue: keyvalues, entitymodeId: $multiEntryInput.selectedEntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                        $multiEntryInput.LoadGridData();
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
};

$masterEarnDedSettings = {
    selectedEntityId: null,
    tableId: 'tbldwCat',
    formData: document.forms["frmDwcategory"],

    loadComponent: function () {

        var gridObject = $masterEarnDedSettings.mastersettingsGridObject();
        var tableid = { id: $masterEarnDedSettings.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvMasterEarnDedSettingsTable').html(modelContent);
        var data = null;
        $masterEarnDedSettings.loadMasterSettingsGrid(data, gridObject, tableid);
    },

    mastersettingsGridObject: function () {
        var gridObject = [
                { tableHeader: "Id", tableValue: "AttributeId", cssClass: 'nodisp' },
                { tableHeader: "Components Name", tableValue: "AttributeName", cssClass: '' },
                  { tableHeader: "Visible", tableValue: "AttributeId", cssClass: 'checkbox' },
        ];
        return gridObject;
    }
    ,
    loadMasterSettingsGrid: function (data, context, tableId) {

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
                    "sClass": "actionColumn ",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        debugger;
                        var ischecked = '';
                        if (oData.IsVisible) {
                            ischecked = 'Checked';
                        }
                        var b = $('<input type="checkbox" class="cbCategory"' + ischecked + '  id="' + sData + '"/>');
                        if (oData.AttributeInputType == "1") {
                            { $(nTd).css('background-color', '#ffff0099'); }
                        }
                        else if (oData.AttributeInputType == "2") {
                            { $(nTd).css('background-color', '#001fff7a'); }
                        }
                        else if (oData.AttributeInputType == "3") {
                            { $(nTd).css('background-color', 'white'); }
                        }
                        else if (oData.AttributeInputType == "4") {
                            { $(nTd).css('background-color', '#ff000091'); }
                        }
                        else if (oData.AttributeInputType == "5") {
                            { $(nTd).css('background-color', '#db5bd161'); }
                        }
                        else {

                        }


                        $(nTd).html(b);
                    },

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
            "paging": true,
            //"ordering": false,
            "info": false,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Entity/GetMasEarnDedCompSettingList",
                    data: JSON.stringify({ id: $masterEarnDedSettings.selectedEntityId }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (jsonResult) {
                        debugger;
                        var Rdata = jsonResult.result;
                        var out = Rdata;
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


    saveSettings: function () {
        debugger;
        var EntityId = $('#sltMasterEntityModel').find('option:selected').val();
        var attributesId = '';
        var rows = $("#tbldwCat").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            if ($(rows[i]).find(".cbCategory").prop("checked")) {
                attributesId += "" + $(rows[i]).find(":eq(0)").html() + ",";
            }
            attributesId = attributesId.trim(',');
        }

        $.ajax({
            url: $app.baseUrl + "Entity/SaveMasterInputSettings",
            data: JSON.stringify({ entityId: EntityId, attributesIds: attributesId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert("Saved Successfully", 1);

                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);

                        break;
                }

            },
            complete: function () {
                $app.hideProgressModel();

            }
        });
    }
};

$('#sltMasterEntityModel').change(function () {
    if ($('#sltMasterEntityModel').find('option:selected').val() != "0") {
        $("#dvMasterEarnDedSettingsTable").show();
        $("#clrcode").show();
        $masterEarnDedSettings.selectedEntityId = $('#sltMasterEntityModel').find('option:selected').val();
        $masterEarnDedSettings.loadComponent();
    }
    else {
        $("#dvMasterEarnDedSettingsTable").hide();
        $("#clrcode").hide();

    }

});