
var $academic = {
    canSave: false,
    selectedAcademicId: '',
    seletedEmployeeId: '',
    bindEvent: function () {
        $('#btnAddAcademic').on('click', function () {
            $academic.addInitialize();
        });
        //$('#btnAcademicSend').on('click', function () {
        //    $academic.save();
        //});
    },

    LoadAcademics: function (employee) {
        $academic.bindEvent();
        $academic.seletedEmployeeId = employee.id;
        var dtacademic = $('#tblAcademic').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "empAcademicid" },
                    { "data": "empAcademicEmployeeId" },
                    { "data": "empAcademicDegreeName" },
                     { "data": "empAcademicInstitionName" },
                      { "data": "empAcademicYearOfPassing" },

                    { "data": null }
            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [1],
             "sClass": "nodisp",
             "bSearchable": false

         },
         {
             "aTargets": [2, 3, 4],
             "sClass": "word-wrap"

         },
      {
          "aTargets": [5],
          "sClass": "actionColumn",
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  $academic.render(oData);
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure,do you want to delete?')) {
                      $academic.deleteData({ Id: oData.empAcademicid, empId: $academic.seletedEmployeeId, type: "academic" });
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
                    url: $app.baseUrl + "Employee/GetEmpAcademics",
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
                var r = $('#tblAcademic tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblAcademic thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    addInitialize: function () {
        $academic.canSave = true;
        var formData = document.forms["frmAcademic"];
        formData.elements["txtDegreeName"].value = "";
        formData.elements["txtInistitionName"].value = "";
        formData.elements["txtYearofPassing"].value = "";
        $academic.selectedAcademicId = '';
    },
    render: function (data) {
        $academic.canSave = true;
        $('#AddAcademic').modal('toggle');
        var formData = document.forms["frmAcademic"];
        formData.elements["txtDegreeName"].value = data.empAcademicDegreeName;
        formData.elements["txtInistitionName"].value = data.empAcademicInstitionName;
        formData.elements["txtYearofPassing"].value = data.empAcademicYearOfPassing;
        $academic.selectedAcademicId = data.empAcademicid;
    },
    get: function () {
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpData",
            data: JSON.stringify({ id: $academic.seletedEmployeeId, type: "academic" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddFamily').modal('toggle');
                        var p = jsonResult.result;
                        $academic.render(p);
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
    save: function () {
        var isValidForm = document.forms['frmAcademic'].checkValidity();
        if (!isValidForm) {
            $app.showAlert('Please fill required field', 4);
            return false;
        }
        $academic.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmAcademic"];
        var data = {
            empAcademicid: $academic.selectedAcademicId,
            empAcademicEmployeeId: $academic.seletedEmployeeId,
            empAcademicDegreeName: formData.elements["txtDegreeName"].value,
            empAcademicInstitionName: formData.elements["txtInistitionName"].value,
            empAcademicYearOfPassing: formData.elements["txtYearofPassing"].value

        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpAcademic",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var p = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $('#AddAcademic').modal('toggle');
                        $academic.LoadAcademics({ id: $academic.seletedEmployeeId });
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

            }
        });
    },
    deleteData: function (data) {
        $academic.selectedAcademicId = data.Id;
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteEmpData",
            data: JSON.stringify({ Id: data.Id, empId: data.empId, type: data.type }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $academic.LoadAcademics({ id: $academic.seletedEmployeeId });
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

};