$("#ddcomponent").change(function () {

    LoadFields($("#ddcomponent").val(), $('#ddcomponent').find('option:selected').text());
});

$(".closePopup").click(function () {
    $('#AddPopup').modal('hide');
});
function LoadFields(component, text) {

    $comPopup.selectedComponent = component;
    $('#PopupTitle').text('Add/Edit ' + text);
    $comPopup.SetVisible($comPopup.selectedComponent);
    $comPopup.createPopupGrid();
    $comPopup.selectedId = '';

}

$("#txtName").blur(function () {

    var RowsDateCheck = $("#tblPopup").dataTable().fnGetNodes();
    for (i = 0; i < RowsDateCheck.length; i++) {
        if ($(RowsDateCheck[i]).find("td:nth-child(2)").html().trim().toLowerCase() == $("#txtName").val().trim().toLowerCase()) {

            if ($(RowsDateCheck[0]).find("td:nth-child(1)").html() != $comPopup.selectedId) {
                $app.showAlert("Already Exist " + $("#txtName").val(), 4);
                $("#txtName").val('');
                $("#txtName").focus();
                return false;
            }
            else {
                return true;
            }

        }
    }
});

var $comPopup = {

    canSave: false,
    selectedId: '',
    selectedComponent: 'branch',
    popuptable: 'tblPopup',
    init: function () {
        $comPopup.createPopupGrid();
        $('#btnAddPopup').addClass('nodisp');
        $('#dvPopupRender').addClass('nodisp');
    },
    SetVisible: function (selectedData) {
        $('#btnAddPopup').removeClass('nodisp');
        $('#dvPopupRender').removeClass('nodisp');
        if (selectedData == 'esiLocation') {
            $('#ESIpopup').removeClass('nodisp');
        }
        else if (selectedData != '0') {
            $('#ESIpopup').addClass('nodisp');
        }
        else if (selectedData == '0') {
            $('#btnAddPopup').addClass('nodisp');
            $('#dvPopupRender').addClass('nodisp');
        }

    },

    createPopupGrid: function () {
        var isEsi = false;
        if ($comPopup.selectedComponent == 'esiLocation') {
            isEsi = true;
        }
        var gridObject = $comPopup.gridObject(isEsi);
        var tableid = { id: $comPopup.popuptable };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvPopupRender').html(modelContent);
        $comPopup.LoadPopup(gridObject, tableid);
    },
    gridObject: function (isEsi) {
        var gridObject = [
                { tableHeader: "id", tableValue: "Id", cssClass: 'nodisp' },
                { tableHeader: $('#ddcomponent').find('option:selected').text() + ' Name', tableValue: "popuplalue", cssClass: '' }]
        if (isEsi) {
            gridObject.push({ tableHeader: "Applicable", tableValue: "isApplicable", cssClass: '' });
            gridObject.push({ tableHeader: "Employer Code", tableValue: "employerCode", cssClass: '' });
        }
        gridObject.push({ tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' });


        return gridObject;
    },
    LoadPopup: function (context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "aaSorting": [[1, "asc"]], "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'actionColumn') {
                columnDef.push(
                        {
                            "aTargets": [cnt],
                            "sClass": "actionColumn",
                            "aaSorting": [[1, "asc"]],
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');

                                b.button();
                                b.on('click', function () {

                                    if (oData.type == "leaveType" && oData.popuplalue == "LOSS OF PAY DAYS") {
                                        $app.showAlert("You can't edit LOSS OF PAY DAYS", 4);
                                        return false;
                                    }
                                    else if (oData.type == "leaveType" && oData.popuplalue == "ONDUTY") {
                                        $app.showAlert("You can't edit ONDUTY", 4);
                                        return false;
                                    }
                                    else {
                                        $comPopup.GetPopupData(oData);
                                        return false;
                                    }

                                });
                                c.button();
                                c.on('click', function () {

                                    if (oData.type == "leaveType" && oData.popuplalue == "LOSS OF PAY DAYS") {
                                        $app.showAlert("You can't Delete LOSS OF PAY DAYS", 4);
                                        return false;
                                    }
                                    else if (oData.type == "leaveType" && oData.popuplalue == "ONDUTY") {
                                        $app.showAlert("You can't Delete ONDUTY", 4);
                                        return false;
                                    }
                                    else {
                                        if (confirm('Are you sure ,do you want to delete?')) {
                                            $comPopup.DeleteData(oData);
                                        }
                                        return false;
                                    }
                                });


                                $(nTd).empty();
                                $(nTd).prepend(b, c);
                            }
                        }

                    ); //for action column
            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#tblPopup').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"]],
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Company/GetPopUpDatas",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ type: $comPopup.selectedComponent }),
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                var out = jsonResult.result;
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
                                $app.showAlert(jsonResult.Message, 4);
                                //alert(jsonResult.Message);
                        }

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
    save: function () {

        if (!$comPopup.canSave) {
            return false;
        }
        $comPopup.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmPopup"];
        var data = {
            id: $comPopup.selectedId,
            popuplalue: formData.elements["txtName"].value,
            isApplicable: $(formData).find('#chkisApplicable').prop("checked"),
            employerCode: formData.elements["txtEmployerCode"].value,
            type: $comPopup.selectedComponent

        };
        $.ajax({
            url: $app.baseUrl + "Company/SavePopup",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddPopup').modal('toggle');
                        $comPopup.createPopupGrid();
                        $comPopup.selectedId = '';
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        if (data.type == "leaveType") {
                            $companyCom.loadLeaveType({ id: 'ddlLTSLeaveType' });
                        }
                        else if ($employee) {
                            $employee.reloadMasterData();
                        }
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

        $comPopup.canSave = true;
        var formData = document.forms["frmPopup"];
        $comPopup.selectedId = '';
        formData.elements["txtName"].value = "";
        $(formData).find('#chkisApplicable').prop("checked", 0);
        formData.elements["txtEmployerCode"].value = "";

    },

    GetPopupData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Company/GetPopupData",
            data: JSON.stringify({ id: context.Id, type: $comPopup.selectedComponent }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddPopup').modal('toggle');
                        var p = jsonResult.result;
                        $comPopup.RenderData(p);
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
    DeleteData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Company/DeletePopupData",
            data: JSON.stringify({ id: context.Id, type: $comPopup.selectedComponent }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $comPopup.createPopupGrid();
                        $app.showAlert(jsonResult.Message, 2);
                        //alert(jsonResult.Message);
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
        $comPopup.canSave = true;
        var formData = document.forms["frmPopup"];
        $comPopup.selectedId = data.Id;
        formData.elements["txtName"].value = data.popuplalue;
        $(formData).find('#chkisApplicable').prop("checked", data.isApplicable);
        formData.elements["txtEmployerCode"].value = data.employerCode;
        $comPopup.SetVisible($comPopup.selectedComponent);
    }

};

$("#ddEMailTemplate").change(function () {
    if ($("#ddEMailTemplate").val() == "0") {
        $("#dvTemplateForm").addClass('hide').removeClass('show');
    }
    else {
        $("#dvTemplateForm").addClass('show').removeClass('hide');
        LoadTemplates($("#ddEMailTemplate").val(), $('#ddEMailTemplate').find('option:selected').text());
    }

});
function LoadTemplates(component, text) {



}