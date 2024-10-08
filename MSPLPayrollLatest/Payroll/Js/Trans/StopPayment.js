$("#sltEmployeelist").change(function () {
    if ($("#sltEmployeelist").val() == "00000000-0000-0000-0000-000000000000") {
        // $("#dvemp").html('');
        $("#dvStopPaymentAdd").addClass('nodisp');
        $("#dvStopPaymentHistory").addClass('nodisp');

    }
    else {
        $StopPayment.StopPaymentEmpId = $("#sltEmployeelist").val();
        $StopPayment.renderStopPaymentfromTran();
        $("#dvStopPaymentAdd").removeClass('nodisp');
        $("#dvStopPaymentHistory").removeClass('nodisp');
        //$StopPayment.loadGrid();
    }
});



var $StopPayment = {
    StopPaymentId: '',
    StopPaymentEmpId: '',
    tableId: 'tblStopPayment',
    formData: document.forms["frmStopPayment"],
    designForm: function (renderDiv) {

        var formrH = ' <div class="row"> <div id="dvStopPaymentAdd" class ="col-md-12 text-right"><input type="button" id="btnAddStopPayment" value="Add" class="btn custom-button marginbt7" data-toggle="modal" data-target="#AddStopPayment" onclick="$StopPayment.AddInitialize();"/></div>';
        formrH = formrH + '<div class="col-md-12 table-responsive"><div id="dvStopPaymentHistory"></div></div>';//for table
        //popup
        formrH = formrH + '<div id="AddStopPayment" class="modal fade" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">'

        formrH = formrH + '<form id="frmStopPayment"><div class="modal-dialog"> <div class="modal-content"><div class="modal-header">'
        formrH = formrH + '<button type="button" class="close" data-dismiss="modal">'
        formrH = formrH + '&times;'
        formrH = formrH + '</button>'
        formrH = formrH + '<h4 class="modal-title" id="H4">'
        formrH = formrH + 'Add/Edit StopPayment'
        formrH = formrH + '</h4>'
        formrH = formrH + '</div>'
        formrH = formrH + '<div class="modal-body">'

        formrH = formrH + '<div class="form-horizontal">';//start horizatal div
        //Stop Payment Month
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Stop Payment Month <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><select id="sltSPayMonth" class="form-control" placeholder="Enter the Stop Payment Month" required ></select> </div>';
        formrH = formrH + '</div>';
        //Stop Payment Year
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Stop Payment Year <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><select id="txtSPayYear"  class="form-control" autofocus></select> </div>';
        formrH = formrH + '</div>';
        //Stop Payment Type
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Stop Payment Type <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><select id="sltSPayType" class="form-control" placeholder="Enter the Stop Payment Type" required ></select> </div>';
        formrH = formrH + '</div>';
        //Remarks
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Remarks <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><input type="text" id="txtReason" class="form-control" placeholder="Enter the Remarks" required /> </div>';
        formrH = formrH + '</div>';

        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';
        //button row start
        formrH = formrH + '<div class="modal-footer">';
        formrH = formrH + '<button type="submit" id="btnSave" class="btn custom-button"> Save</button>';
        formrH = formrH + '<button type="button" class="btn custom-button" data-dismiss="modal">Close </button> </div>';
        //button row end
        formrH = formrH + '</form>';//form end
        formrH = formrH + '</div></div>';
        $('#' + renderDiv).html(formrH);//transHtml
        $StopPayment.formData = document.forms["frmStopPayment"];
        $("#frmStopPayment").on('submit', function (event) {
            if ($app.requiredValidate("frmStopPayment", event)) {
                $StopPayment.save();
            }
            return false;
        });

    },
    initiateForm: function (employeeId, renderDiv) {
        
        $StopPayment.StopPaymentEmpId = employeeId;
        $StopPayment.designForm(renderDiv);
        $companyCom.loadYear({ id: "txtSPayYear" });
        $payroll.loadMonth({ id: 'sltSPayMonth' });
        $StopPayment.loadSPayType({ id: 'sltSPayType' });
        $StopPayment.loadGrid();
        //$('#frmStopPayment').on('submit', function (event) {
        //    if ($app.requiredValidate('frmStopPayment', event)) {
        //        $StopPayment.save();
        //        return false;
        //    }
        //    else {
        //        return false;
        //    } 
        //});
        $payroll.initDatetime();
    },
    loadGrid: function () {
        var gridObject = $StopPayment.createGridObject();
        var tableid = { id: $StopPayment.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $("#dvStopPaymentHistory").html('');
        $("#dvStopPaymentHistory").html(modelContent);

        $StopPayment.LoadGridData(gridObject, tableid);

    },

    loadInitial: function () {
        $companyCom.loadEmployee({ id: "sltEmployeelist" });
        $payroll.loadMonth({ id: 'sltSPayMonth' });
        $StopPayment.loadSPayType({ id: 'sltSPayType' });
        $("#dvStopPaymentAdd").addClass('nodisp');
        $payroll.GetFullMonthName();

    },
    loadSPayType: function (dropControl) {
        var msg = [];
        msg.push({ id: 1, name: 'HOLD FOR F&F' });
        msg.push({ id: 2, name: 'OTHERS' });
        msg.push({ id: 3, name: 'RELEASE' });
        $.each(msg, function (index, SP) {
            $('#' + dropControl.id).append($("<option></option>").val(SP.id).html(SP.name));
        });
    },
    LoadStopPayment: function (context) {
        $.ajax({
            url: $app.baseUrl + "Transaction/GetStopPaymentData",
            data: JSON.stringify({ SPayEmpId: $StopPayment.StopPaymentEmpId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddStopPayment').modal('toggle');
                        var p = jsonResult.result;
                        $StopPayment.RenderData(p);
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
    RenderData: function (data) {
        $('#AddStopPayment').modal('toggle');
        var formData = document.forms["frmStopPayment"];
        $StopPayment.StopPaymentId = data.SPayid;
        formData.elements["sltSPayMonth"].value = data.SPayMonth;
        formData.elements["txtSPayYear"].value = data.SPayYear;
        formData.elements["sltSPayType"].value = data.SPayType;
        formData.elements["txtReason"].value = data.SPayRemarks;
    },

    renderStopPaymentfromTran: function () {
        $StopPayment.designForm('dvstpPayment');
        $companyCom.loadYear({ id: "txtSPayYear" });
        $payroll.loadMonth({ id: 'sltSPayMonth' });
        $StopPayment.loadSPayType({ id: 'sltSPayType' });
        $payroll.initDatetime();
        $StopPayment.loadGrid();
    },

    AddInitialize: function () {

        var formData = document.forms["frmStopPayment"];
        $StopPayment.StopPaymentId = "";
        formData.elements["sltSPayMonth"].value = "";
        formData.elements["txtSPayYear"].value = "";
        formData.elements["sltSPayType"].value = "";
        formData.elements["txtReason"].value = "";
    },

    save: function () {
        $app.showProgressModel();
        var formData = document.forms["frmStopPayment"];
        var data = {
            SPayEmpId: $StopPayment.StopPaymentEmpId,
            SPayMonth: formData.elements["sltSPayMonth"].value,
            SPayYear: formData.elements["txtSPayYear"].value,
            SPayType: formData.elements["sltSPayType"].value,
            SPayRemarks: formData.elements["txtReason"].value,
            SPayid: $StopPayment.StopPaymentId
        };
        $.ajax({
            url: $app.baseUrl + "Transaction/SaveStopPaymentData",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:                        
                        $StopPayment.loadGrid();
                        $('#AddStopPayment').modal('toggle');
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

    createGridObject: function () {
        var gridObject = [];
        gridObject.push({ tableHeader: "Id", tableValue: 'SPayEmpId', cssClass: 'nodisp' });
        gridObject.push({ tableHeader: "Name", tableValue: 'SPayEmpName' });
        gridObject.push({ tableHeader: "Stop Payment Month", tableValue: 'SPayMonth' });
        gridObject.push({ tableHeader: "Stop Payment Year", tableValue: 'SPayYear' });
        gridObject.push({ tableHeader: "Remarks", tableValue: 'SPayRemarks' });
        gridObject.push({ tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' });
        return gridObject;


    },
    LoadGridData: function (gridobject, tableprop) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Transaction/GetStopPayment",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                SPayEmpId: $StopPayment.StopPaymentEmpId
            }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;                        
                        $StopPayment.renderGridData(out, gridobject, tableprop);
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

    renderGridData: function (data, context, tableId) {

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
                                    $StopPayment.edit(oData);
                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    if (confirm('Are you sure ,do you want to delete?')) {
                                        $StopPayment.Delete(oData);
                                    }
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
                        var b = $('<input type="text" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
                        $(nTd).html(b);
                    }
                });

            }
            else {
                if (cnt == 2) {

                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "word-wrap",
                        "bSearchable": true,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            MonthName = $payroll.GetFullMonthName(oData.SPayMonth);
                            $(nTd).html(MonthName);
                        }


                    });
                }
                else {
                    columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
                }
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



        //var columnsValue = [];
        //var columnDef = [];
        //for (var cnt = 0; cnt < context.length; cnt++) {
        //    columnsValue.push({ "data": context[cnt].tableValue });
        //    if (context[cnt].cssClass == 'nodisp') {
        //        columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
        //    }
        //    else if (context[cnt].cssClass == 'actionColumn') {
        //        columnDef.push(
        //                {
        //                    "aTargets": [cnt],
        //                    "sClass": "actionColumn",
        //                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        //                        var b = $('<a href="#" class="editeButton"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
        //                        var c = $('<a href="#" class="deleteButton"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
        //                        b.button();
        //                        b.on('click', function () {
        //                            $StopPayment.edit(oData);
        //                            return false;
        //                        });
        //                        c.button();
        //                        c.on('click', function () {
        //                            if (confirm('Are you sure ,do you want to delete?')) {
        //                                $StopPayment.Delete(oData);
        //                            }
        //                            return false;
        //                        });
        //                        $(nTd).empty();
        //                        $(nTd).prepend(b, c);
        //                    }
        //                }

        //            ); //for action column
        //    }
        //    else if (context[cnt].cssClass == 'edit') {
        //        columnDef.push({
        //            "aTargets": [cnt],
        //            "sClass": "actionColumn",
        //            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        //                var b = $('<input type="text" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
        //                $(nTd).html(b);
        //            }
        //        });

        //    }
        //    else {
        //        if (cnt == 2) {

        //            columnDef.push({
        //                "aTargets": [cnt],
        //                "sClass": "word-wrap",
        //                "bSearchable": true,
        //                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        //                    MonthName = $payroll.GetFullMonthName(oData.SPayMonth);
        //                    $(nTd).html(MonthName);
        //                }


        //            });
        //        }
        //        else {
        //            columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
        //        }
        //    }
        //}
        //var dtClientList = $('#tbl_' + tableprop.id).DataTable({
        //    'iDisplayLength': 10,
        //    'bPaginate': true,
        //    'sPaginationType': 'full',
        //    'sDom': '<"top">rt<"bottom"ip><"clear">',
        //    columns: columnsValue,
        //    "aoColumnDefs": columnDef,
        //    "aaData": data,
        //    fnInitComplete: function (oSettings, json) {
        //        var r = $('#tblFormula tfoot tr');
        //        r.find('th').each(function () {
        //            $(this).css('padding', 8);
        //        });
        //        $('#tblFormula thead').append(r);
        //        $('#search_0').css('text-align', 'center');
        //    },
        //    dom: "rtiS",
        //    scroller: {
        //        loadingIndicator: true
        //    }
        //});
        //$('#tbl_' + tableprop.id).on('click', 'tbody td:not(:first-child)', function (e) {
        //    dtClientList.inline(this);
        //});
    },

    edit: function (context) {
        $StopPayment.RenderData(context);

    },
    Delete: function (context) {
        $StopPayment.StopPaymentId = context.SPayid;
        $.ajax({
            url: $app.baseUrl + "Transaction/DeleteStopPaymentData",
            data: JSON.stringify({ id: context.SPayid }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $StopPayment.loadGrid();
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
    }
}

