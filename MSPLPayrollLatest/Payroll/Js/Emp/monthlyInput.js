﻿//$(document).ready(function () {
//    $monthlyInput.LoadEntityModelDrop();
//});
$("#btnSave").click(function () {
    $monthlyInput.save();
    $monthlyInput.InitScreen();
    
});
$("#btnDelete").click(function () {
    $monthlyInput.Delete();
    $monthlyInput.InitScreen();

});
$("#btnReset").click(function () {
   
    if (confirm("Are you sure, do you want to Reset?")) {
        var RowsDateCheck = $('#tbl_' + $monthlyInput.selectedEntityId).dataTable().fnGetNodes();
        $(RowsDateCheck).each(function (index, data) {
            $(data).find('td').each(function (ind, tmp) {
                $(tmp).find('input').val("0.00");
            });
        });
        $monthlyInput.InitScreen();
    }
});

$('#sltCategory').change(function () {
    $monthlyInput.selectedcategoryId = $('#sltCategory').val();

});
$('#sltMonth').change(function () {
    $monthlyInput.month = $('#sltMonth').val();

});
$('#sltYear').change(function () {
    $monthlyInput.year = $('#sltYear').val();
});

$('#btnClose').click(function () {
    $monthlyInput.currentMonthYesOrNo = '';
    $monthlyInput.currentMonthYesOrNo = confirm('Quit Without Saving Data');
    if ($monthlyInput.currentMonthYesOrNo == true) {
        $monthlyInput.InitScreen();
    }
});


$('#btnView').click(function () {
    $monthlyInput.selectedcategoryId = $('#sltCategory').val();
    $monthlyInput.month = $('#sltMonth').val();
    $monthlyInput.year = $('#sltYear').val();
    $monthlyInput.Checkdata();
});

$('.resetmonthlyButton').click(function () {
    debugger;
    table.row('.selected').remove().draw(false);
});


$(function () {
    debugger;
    $(".context-menu-one").contextMenu({
        selector: 'th',
        callback: function (key, options) {
            debugger;
            var content = $(this).text();  
            if (confirm("Are you sure, do you want to copy all ?")) {
                var attribute = content;
               
                // Get row data                
                var RowsDateCheck = $('#tbl_' + $monthlyInput.selectedEntityId).dataTable().fnGetNodes();
                var updateVal='';
                $(RowsDateCheck).each(function (index, data) {
                    var dtValues = [];
                    var cnt = 0;
                    $(data).find('td').each(function (ind, tmp) {
                        if (cnt == 0) {
                            dtValues.push({
                                'ClsName': attribute
                            });
                        }
                        else {
                            var input = $(tmp).find('input');
                            if (input != null) {
                                var id = $(input).prop('id');
                                var cls = $(input).prop('className');
                                if (id != null && attribute == cls) {
                                    dtValues.push({
                                        'Id': id, 'value': $(input).val(), 'cls': cls
                                    });
                                }
                            }
                        }
                        cnt = cnt + 1;                     
                      
                    });
                    if (attribute == dtValues[0].ClsName) {                       
                        for (var t1 = 1; t1 < dtValues.length; t1++) {
                            if (dtValues[t1].cls == attribute) {
                                if (updateVal=='') {
                                    updateVal = dtValues[t1].value;
                                }
                                $(data).find('#' + dtValues[t1].Id).val(updateVal);
                            }       
                        }
                    }
                    
                });
            }
        },
        items: {
            "copy": {
                name: "Copy", icon: "edit",
                disabled: function (key, opt) {
                    var col = $(this).text();
                    if (col == "Code" || col=="Action" || col=="Name") return true;
                },               
            },
            //"delete": { name: "Delete", icon: "delete" },
        }
    });

    $('.context-menu-one').on('click', function (e) {
        console.log('clicked', this);
    })
});
$monthlyInput = {
    currentMonthYesOrNo: '',
    selectedEntityModelId: null,
    selectedEntityModelname: null,
    selectedcategoryId: null,
    selectedEntityId: null,
    month: $('#sltMonth').val(),
    year: $('#sltYear').val(),
    FFFlag: false,
    result:'',
    Alignment: function () {

        var a = $('.dataTables_scrollHeadInner table').first().css("margin-left");
        var b = $('.dataTables_scrollHeadInner table').first().css("width");
        $(".dataTables_scrollBody table").css({ 'margin-left': a, "width": b });
    },
    LoadEntityModelDrop: function () {
        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltMonth', condi: 'Month' });
        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltYear', condi: 'Year' });
        $monthlyInput.LoadEntityModel({ id: 'sltEntityModel' });
        $companyCom.loadCategory({ id: 'sltCategory' });
        $monthlyInput.selectedcategoryId = $('#sltCategory').val();
    },
    LoadEntityModel: function (dropControl) {
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
                            $monthlyInput.selectedEntityModelId = object.EntityModelId;
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
            if ($('#' + dropControl.id).val() == 0) {
                $("#dvDynamicEntity").html('');
            }
            else {
                $monthlyInput.selectedEntityModelname = $('#' + dropControl.id).find('option:selected').text();
                $monthlyInput.selectedEntityId = $('#' + dropControl.id).val();
                //$monthlyInput.LoadGridData();
            }
        });
    },
    renderFieldGrid: function (context, tableprop) {

        $("#dvTitle").html('');
        $("#dvTitle").html('<h4>' + $monthlyInput.selectedEntityModelname + '</h4>');
        var grid = '<table id="tbl_' + tableprop.id + '" class="table position-fixed table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer">'
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
        grid = grid + '<tbody class="context-menu-one"><tr>'
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
        gridObject.push({ tableHeader: "Action", tableValue: 'EmployeeId' });
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
        $monthlyInput.renderFieldGrid(gridObject, { id: $monthlyInput.selectedEntityId });
        $monthlyInput.LoadAttributeModels(context, gridObject, { id: $monthlyInput.selectedEntityId });

    },

    Checkdata: function () {
        debugger;
        $monthlyInput.month = $('#sltMonth').val();
        $monthlyInput.year = $('#sltYear').val();
        $monthlyInput.selectedcategoryId = $('#sltCategory option:selected').val();
        $monthlyInput.selectedEntityId = $('#sltEntityModel option:selected').val();
        debugger

        if ($monthlyInput.selectedEntityId != 0 && $monthlyInput.month != 0 && $monthlyInput.year != 0 && $monthlyInput.selectedcategoryId != '00000000-0000-0000-0000-000000000000' && $monthlyInput.selectedEntityId != '--Select--') {
            $monthlyInput.result = '';
            $monthlyInput.LoadGridData();
        }
        else {
            alert("Fill Category,Month,Year to View");
            $('#Input').show();
        }
    },

    InitScreen: function () {
        $('#Input').show();
        $('#mmyy-text').html('');
        $("#dvTitle").html('');
        $("#dvDynamicEntity").html('');
        $('#btnSave').addClass('nodisp');
        $('#btnReset').addClass('nodisp');
        $('#btnDelete').addClass('nodisp');
        $('#btnClose').addClass('nodisp');
    },

    CheckInit: function () {
        alert($monthlyInput.currentMonthYesOrNo);
        if ($monthlyInput.currentMonthYesOrNo == true) {
            $monthlyInput.InitScreen();
        }
    },


    LoadGridData: function () {
        debugger;
        $monthlyInput.month = $('#sltMonth').val();
        $monthlyInput.year = $('#sltYear').val();
        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetMonthlyInput",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                entitymodelId: $monthlyInput.selectedEntityModelId,
                entityId: $monthlyInput.selectedEntityId,
                categoryId: $monthlyInput.selectedcategoryId,
                month: $monthlyInput.month,
                year: $monthlyInput.year,
                employeeId: "00000000-0000-0000-0000-000000000000"
            }),
            dataType: "json",
            success: function (jsonResult) {
                debugger;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        if (out.length == 0) {
                            alert("NO RECORDS");
                            break;
                        }
                        else {
                            $('#Input').hide();
                            $('#mmyy-text').html("Month / Year : " + $monthlyInput.month + "/" + $monthlyInput.year);
                            $('#btnSave').removeClass('nodisp');
                            $('#btnReset').removeClass('nodisp');
                            $('#btnDelete').removeClass('nodisp');
                            $('#btnClose').removeClass('nodisp');
                            $monthlyInput.renderGrid(out);
                            break;
                        }
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


    LoadAttributeModels: function (data, context, tableprop) {
        debugger;
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
        columnDef.push(
                       {
                           "aTargets": [1],
                           //"sClass": "actionColumn",
                           "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                
                               if (oData.editStatus == 'CanEdit') {
                                   //var b = $('<input type="text"  onkeyup="return $validator.moneyvalidation(this.id)" onkeypress="return $validator.moneyvalidation(this.id)" id="txt_' + oData.MonthlyInputAttributes[iCol - 3].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 3].MIValue + '" />');
                                   //$(nTd).html(b);
                                   var r = $('<a href="#" class="resetmonthlyButton" title="Reset"><span aria-hidden="true" class="glyphicon glyphicon-refresh"></span></button>');
                                   r.button();
                                   r.on('click', function () {
                                       var ids = $(this).parent();
                                       if (confirm("Are you sure, do you want to Reset?")) {
                                           var empId = oData.EmployeeId;
                                           var keyvalues = [];
                                           // Get row data
                                           var tables = $('#tbl_' + $monthlyInput.selectedEntityId).dataTable();
                                           var RowsDateCheck = $('#tbl_' + $monthlyInput.selectedEntityId).dataTable().fnGetNodes();
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
                                               if (empId == dtValues[0].EmployeeId) {
                                                   var rowVal = [];
                                                   for (var t1 = 1; t1 < dtValues.length; t1++) {
                                                       $(data).find('#txt_' + dtValues[t1].Id).val("0.00");
                                                   }
                                               }
                                           });
                                       }
                                       return false;
                                   });
                                   $(nTd).empty();
                                   $(nTd).append(r);
                               }
                               else {
                                   $(nTd).empty();
                               }
                           }
                       }

                   ); //for action column
        if (data != null) {
            
            for (var cnt1 = 4; cnt1 < context.length; cnt1++) {
                if (context[cnt1].MIdisplay == "LD") {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            //"sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                if (oData.editStatus == 'CanEdit') {
                                    //var b = $('<input type="text"  onkeyup="return $validator.moneyvalidation(this.id)" onkeypress="return $validator.moneyvalidation(this.id)" id="txt_' + oData.MonthlyInputAttributes[iCol - 3].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 3].MIValue + '" />');
                                    //$(nTd).html(b);
                                    var b = $('<input type="text"  onblur="return $validator.minmax(this.id)" onchange="return $validator.minmax(this.id)" class="' + oData.MonthlyInputAttributes[iCol - 4].Name + '" id="txt_' + oData.MonthlyInputAttributes[iCol - 4].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 4].MIValue + '" />');
                                   
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
                                    //  var b = $('<input type="text" maxlength="15"   onkeyup="return $monthlyInput.MonthlyInputvalidation("' + oData.Id + '" , "' + oData.MonthlyInputAttributes[iCol - 4].AttributeModId + '")"  class="' + oData.MonthlyInputAttributes[iCol - 4].Name + '" id="txt_' + oData.MonthlyInputAttributes[iCol - 4].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 4].MIValue + '" />');
                                    var b = $('<input type="text" maxlength="15"  onkeypress = "return $validator.checkDecimal(event, 2)"  class="' + oData.MonthlyInputAttributes[iCol - 4].Name + '" id="txt_' + oData.MonthlyInputAttributes[iCol - 4].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 4].MIValue + '" />');
                                    if (oData.MonthlyInputAttributes[iCol - 4].status == "sne") {
                                        b = $('<input type="text" maxlength="15"  onkeypress = "return $validator.checkDecimal(event, 2)"  class="' + oData.MonthlyInputAttributes[iCol - 4].Name + '" id="txt_' + oData.MonthlyInputAttributes[iCol - 4].AttributeModId + '" value="' + oData.MonthlyInputAttributes[iCol - 4].MIValue + '" style="border:none;" readonly/>');
                                    }

                                    if ($monthlyInput.FFFlag) {
                                        if (oData.MonthlyInputAttributes[iCol - 4].MIDisplay == "NPAYDAYS") {
                                            debugger;
                                        var date1 = new Date($('#txtResignationDate').val());
                                        var date2 = new Date($('#txtLastWorkingDate').val());
                                        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                                        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24))+1;
                                        diffDays = $('#txtNoticePeriod').val() - diffDays;
                                        diffDays = diffDays < 0 ? 0 : diffDays;
                                        b = $('<input type="text" maxlength="15"  onkeypress = "return $validator.checkDecimal(event, 2)"  class="' + oData.MonthlyInputAttributes[iCol - 4].Name + '" id="txt_' + oData.MonthlyInputAttributes[iCol - 4].AttributeModId + '" value="' + diffDays + '" />');
                                        }
                                    }
                                    $(nTd).html(b);
                                }

                            }
                        }

                    ); //for action column
                }
            }
        }
        debugger;
        var dtClientList = $('#tbl_' + tableprop.id).DataTable({

            'iDisplayLength': 10,
            'bPaginate': true,
            'bSearchable': true,
            'sPaginationType': 'full',
            //  'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            fixedHeader: true,
            "aaData": data,
            "bFilter": true,
            "sSearch": "Search:",
            // scrollY: "900px",
            scrollX: true,
            scrollCollapse: true,
            paging: true,
            fixedColumns: {
                //heightMatch: 'auto',
                leftColumns: 4
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

        //new $.fn.dataTable.FixedColumns(dtClientList);
        //$('#tbl_' + tableprop.id).on('click', 'tbody td:not(:first-child)', function (e) {
        //    debugger;
        //    dtClientList.inline(this);
        //});
        // var table = $('#tbl_' + tableprop.id).DataTable();
        //table.on('select', function (e, dt, type, indexes) {

        //    if (type === 'row') {
        //        var data = table.rows(indexes).data().pluck('id');
        //        debugger;
        //        // do something with the ID of the selected items
        //    }
        //});

    },
    Delete: function (context) {
        debugger;
        $app.showProgressModel();
        var keyvalues = [];
        var RowsDateCheck = $('#tbl_' + $monthlyInput.selectedEntityId).dataTable().fnGetNodes();
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
            keyvalues.push({ 'EmployeeId': dtValues[0].EmployeeId, 'Id': $monthlyInput.selectedEntityId, 'Month': $monthlyInput.month, 'year': $monthlyInput.year, MonthlyInputAttributes: rowVal });
        });
        $.ajax({
            url: $app.baseUrl + "Entity/DeleteMonthlyInput",
            data: JSON.stringify({ dataValue: keyvalues, entitymodeId: $monthlyInput.selectedEntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                        //$monthlyInput.LoadGridData();
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
    save: function (context) {
        $app.showProgressModel();
        var keyvalues = [];
        var RowsDateCheck = $('#tbl_' + $monthlyInput.selectedEntityId).dataTable().fnGetNodes();
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
            keyvalues.push({ 'EmployeeId': dtValues[0].EmployeeId, 'Id': $monthlyInput.selectedEntityId, 'Month': $monthlyInput.month, 'year': $monthlyInput.year, MonthlyInputAttributes: rowVal });
        });
        $.ajax({
            url: $app.baseUrl + "Entity/SaveMonthlyInput",
            data: JSON.stringify({ dataValue: keyvalues, entitymodeId: $monthlyInput.selectedEntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                       // $monthlyInput.LoadGridData();
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
    deleteEntity: function (data) {
        $.ajax({
            url: $app.baseUrl + "Entity/DeleteEntity",
            data: JSON.stringify({ id: data.Id, entityModelId: $monthlyInput.selectedEntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $monthlyInput.LoadGridData();
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


};