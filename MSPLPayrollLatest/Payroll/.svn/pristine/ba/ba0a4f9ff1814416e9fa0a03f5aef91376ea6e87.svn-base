$("#txtHRComponentName").change(function () {
    $("#tblHRComp tbody tr").each(function () {
        if ($("#txtHRComponentName").val().toLowerCase() == $(this).find("td:nth-child(2)").html().toLowerCase()) {
            $app.showAlert("Already Exist " + $("#txtHRComponentName").val(), 4);
            $("#txtHRComponentName").val('');
            return false;
        }
    });
});
var $HRComponent = {
    canSave: false,
    HRCompId: '',
    HRComptable: 'tblHRComp',
    tableId: 'tblempHRComponent',
    selectedEmployeeId: null,
    LoadHRComponents: function () {
        var dtClientList = $('#tblHRComp').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',

            columns: [
             { "data": "Id" },
                    { "data": "Name" },
                       { "data": "CompanyId" },
                       {
                           "data": null
                       }
            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "sClass": "word-wrap"

         },
         {
             "aTargets": [2],
             "sClass": "nodisp"

         },
      {
          "aTargets": [3],
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  $HRComponent.GetPopupData(oData);
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $HRComponent.DeleteData({ Id: oData.Id });
                  }
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend(b, c);
          }
      }
            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Company/GetHRComponents",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (msg) {
                        var out = msg;
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
                var r = $('#tblHRComp tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblHRComp thead').append(r);
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
        if (!$HRComponent.canSave) {
            return false;
        }
        $HRComponent.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmHrComponent"];
        var data = {
            id: $HRComponent.HRCompId,
            popuplalue: formData.elements["txtHRComponentName"].value,
            type: "HRComponent"
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
                        $('#AddHRComponent').modal('toggle');
                        $HRComponent.LoadHRComponents();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var p = jsonResult.result;
                        companyid = 0;
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
        $HRComponent.canSave = true;
        var formData = document.forms["frmHrComponent"];
        $HRComponent.HRCompId = '';
        formData.elements["txtHRComponentName"].value = "";

    },
    GetPopupData: function (context) {
        
        $.ajax({
            url: $app.baseUrl + "Company/GetPopupData",
            data: JSON.stringify({ id: context.Id, type: "HRComponent" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddHRComponent').modal('toggle');
                        var p = jsonResult.result;
                        $HRComponent.RenderData(p);
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
    DeleteData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Company/DeletePopupData",
            data: JSON.stringify({ id: context.Id, type: "HRComponent" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $HRComponent.LoadHRComponents();
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
        
        $HRComponent.canSave = true;
        var formData = document.forms["frmHrComponent"];
        $HRComponent.HRCompId = data.Id;
        formData.elements["txtHRComponentName"].value = data.popuplalue;
    },
    //Employee related code
    initiateFormEmployee: function (employeeId) {
        $HRComponent.selectedEmployeeId = employeeId.id;
        $companyCom.loadHRComponent({ id: "sltempHRComponent" })
        $HRComponent.createHRComponentGrid();


        $('#btnempHRComponent').on('click', function () {
            $HRComponent.canSave = true;
            var formData = document.forms["frmEmpHRComponent"];
            formData.elements["sltempHRComponent"].value = "00000000-0000-0000-0000-000000000000";
            formData.elements["txtEffDate"].value = "";
            formData.elements["txtEndDate"].value = "";
            formData.elements["txtComments"].value = "";
            $HRComponent.HRCompId = null;
        });
        $("#txtEffDate,#txtEndDate").change(function () {
            var from = new Date($("#txtEffDate").val());
            var to = new Date($("#txtEndDate").val());
            if (from != '' && to != '') {
                if (Date.parse(from) > Date.parse(to)) {
                    $("#txtEffDate").val('');
                    $("#txtEndDate").val('');
                    $app.showAlert("Invalid Date Range!\n End date should be greater than Effective date", 4);
                }
            }
        });
    },
    createHRComponentGrid: function () {
        var gridObject = $HRComponent.HRComponentGridObject();
        var tableid = { id: $HRComponent.HRComptable };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvEmpHRComponent').html(modelContent);
        $HRComponent.LoadHRComponentgrid(gridObject, tableid);
    },
    HRComponentGridObject: function () {
        var gridObject = [
                { tableHeader: "id", tableValue: "empHrComponentid", cssClass: 'nodisp' },
                 { tableHeader: "HRComponentId", tableValue: "empHrComponentHRComponentId", cssClass: 'nodisp' },
                 { tableHeader: "Name ", tableValue: "empHrComponentHRComponentName", cssClass: '' },
                 { tableHeader: "Effective From", tableValue: "empHRCompEffectiveDate", cssClass: '' },
                { tableHeader: "End Date", tableValue: "empHRCompEndDate", cssClass: '' },
                { tableHeader: "Comments", tableValue: "empHRCompComments", cssClass: '' },
                { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }
        ];
        return gridObject;
    },
    LoadHRComponentgrid: function (context, tableId) {
        
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
                                    $HRComponent.GetEmpHRComponentData({ Id: oData.empHrComponentid });
                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    if (confirm('Are you sure ,do you want to delete?')) {
                                        $HRComponent.DeleteEmpHRComponentData({ Id: oData.empHrComponentid });
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
            else if (context[cnt].tableValue == "empHRCompEndDate") {
                columnDef.push(
                       {
                           "aTargets": [cnt],
                           "sClass": "word-wrap",
                           "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                               //   b = $('<label style="background-color:#1ED2C7">' + sData + ' </label>');
                               if (oData.empHRCompEndDate == "01/Jan/1900") {
                                   var b = '';
                                   $(nTd).html(b);
                               }
                               else {
                                   var b = oData.empHRCompEndDate
                                   $(nTd).html(b);
                               }
                              
                           }
                       }

                   ); //for action column
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
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetEmpHrComponent",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ 'employeeId': $HRComponent.selectedEmployeeId }),
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
    GetEmpHRComponentData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpHRComponentData",
            data: JSON.stringify({ id: context.Id, employeeId: $HRComponent.selectedEmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddEmp_HRComponent').modal('toggle');
                        var p = jsonResult.result;
                        $HRComponent.renderempHRComponent(p);
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
    saveEmpHRComponent: function () {
        if ($("#sltempHRComponent").val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert("Please select HR Component", 4);
            return false;
        }
        if ($("#txtEffDate").val() == "") {
            $app.showAlert("Please select Effective Date", 4);
            return false;
        }
        var formData = document.forms["frmEmpHRComponent"];
        var data = {
            empHrComponentid: $HRComponent.HRCompId,
            empHrComponentEmployeeId: $HRComponent.selectedEmployeeId,
            empHrComponentHRComponentId: formData.elements["sltempHRComponent"].value,
            empHRCompEffectiveDate: formData.elements["txtEffDate"].value,
            empHRCompEndDate: formData.elements["txtEndDate"].value,
            empHRCompComments: formData.elements["txtComments"].value
        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpHRComponent",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddEmp_HRComponent').modal('toggle');
                        $HRComponent.createHRComponentGrid();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var p = jsonResult.result;
                        companyid = 0;
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
    renderempHRComponent: function (data) {
        $HRComponent.canSave = true;
        var formData = document.forms["frmEmpHRComponent"];
        $HRComponent.HRCompId = data.empHrComponentid;
        formData.elements["sltempHRComponent"].value = data.empHrComponentHRComponentId;
        formData.elements["txtEffDate"].value = data.empHRCompEffectiveDate;
        formData.elements["txtEndDate"].value = data.empHRCompEndDate;
        formData.elements["txtComments"].value = data.empHRCompComments;

    },
    //need to check is this function required
    edit: function (context) {
        $renderempjoindoc.RenderData(context);

    },
    DeleteEmpHRComponentData: function (data) {
        $HRComponent.HRCompId = data.Id;
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteEmpData",
            data: JSON.stringify({ id: data.Id, empId: $HRComponent.selectedEmployeeId, type: 'HrComponent' }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $HRComponent.createHRComponentGrid();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    }


};