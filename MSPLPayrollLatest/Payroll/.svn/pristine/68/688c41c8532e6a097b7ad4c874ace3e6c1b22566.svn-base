var $emergencyContact = {
    canSave: false,
    selectedEmployeeId: '',
    selectedEmergency: '',
    formData: document.forms["frmEmpEmergencyContact"],
    bindEvent: function () {
        $('#btnAddEmergencyContact').on('click', function () {
            $emergencyContact.addInitialize();
        });
        $('#frmEmpEmergencyContact').on('submit', function () {
            $emergencyContact.save();
        });
    },
    LoademergencyContacts: function (employee) {
        $emergencyContact.bindEvent();
        $emergencyContact.selectedEmployeeId = employee.id;
        var dtEmergencyContact = $('#tblEmergencyContact').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "empEmrgContid" },
                    { "data": "empEmrgContEmployeeId" },
                    { "data": "empEmrgContName" },
                     { "data": "empEmrgContNumber" },
                      { "data": "relation.name" },
                       { "data": "empEmrgContAddress" },
                    { "data": null }
            ],
            "aoColumnDefs": [
        {
            "aTargets": [0, 1],
            "sClass": "nodisp",
            "bSearchable": false
        },

         {
             "aTargets": [2, 3, 4, 5],
             "sClass": "word-wrap"

         },
      {
          "aTargets": [6],
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  $emergencyContact.render(oData);
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure,do you want to delete?')) {
                      $emergencyContact.deleteData({ Id: oData.empEmrgContid, empId: $emergencyContact.selectedEmployeeId, type: "emergencyContact" });
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
                    url: $app.baseUrl + "Employee/GetEmpEmergencyContacts",
                    contentType: "application/json; charset=utf-8",
                    data: "{'employeeId':'" + employee.id + "'}",
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
                var r = $('#tblEmergencyContact tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblEmergencyContact thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    }
    ,
    addInitialize: function () {
        $emergencyContact.canSave = true;
        $($emergencyContact.formData).find('#txtEmerContactName').val('');
        $($emergencyContact.formData).find('#txtEmerContactNumber').val('');
        $($emergencyContact.formData).find('#txtEmerAddress').val('');
        $($emergencyContact.formData).find('#ddEmerRelationShip').val(0);
        $emergencyContact.selectedEmergency = null;
    }
    ,
    render: function (data) {
        $emergencyContact.canSave = true;
        $('#AddEmergencyContact').modal('toggle');
        $($emergencyContact.formData).find('#txtEmerContactName').val(data.empEmrgContName);
        $($emergencyContact.formData).find('#txtEmerContactNumber').val(data.empEmrgContNumber);
        $($emergencyContact.formData).find('#txtEmerAddress').val(data.empEmrgContAddress);
        $($emergencyContact.formData).find('#ddEmerRelationShip').val(data.relation.id);
        $emergencyContact.selectedEmergency = data.empEmrgContid;
    },
    save: function () {
        if (!$emergencyContact.canSave) {
            return false;
        }
        $emergencyContact.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmEmpEmergencyContact"];
        var data = {
            empEmrgContid: $emergencyContact.selectedEmergency,
            empEmrgContEmployeeId: $emergencyContact.selectedEmployeeId,
            empEmrgContName: $($emergencyContact.formData).find('#txtEmerContactName').val(),
            empEmrgContNumber: $($emergencyContact.formData).find('#txtEmerContactNumber').val(),
            empEmrgContAddress: $($emergencyContact.formData).find('#txtEmerAddress').val(),
            relation: { id: $($emergencyContact.formData).find('#ddEmerRelationShip').val(), name: "" }
        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpEmergencyContact",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var p = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $emergencyContact.LoademergencyContacts({ id: $emergencyContact.selectedEmployeeId });
                        $('#AddEmergencyContact').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert('Plan update failed', 4);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },
    get: function () {
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpData",
            data: JSON.stringify({ id: $emergencyContact.selectedEmergency, type: "emergencyContact" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $emergencyContact.render(p);
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
    deleteData: function (data) {
        $emergencyContact.selectedEmergency = data.Id;
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteEmpData",
            data: JSON.stringify({ id: data.Id, empId: data.empId, type: data.type }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $emergencyContact.LoademergencyContacts({ id: $emergencyContact.selectedEmployeeId });
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