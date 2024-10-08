var $training = {
    canSave: false,
    selectedTrainingId: '',
    selectedEmployeeId: '',
    bindEvent: function () {
        $("#txtTrainingTo,#txtTrainingFrom").change(function () {
            var datefrom = new Date($("#txtTrainingFrom").val());
            var dateto = new Date($("#txtTrainingTo").val());
            if ($("#txtTrainingFrom").val() != '' && $("#txtTrainingTo").val() != '') {
                if (dateto >= datefrom) {

                }
                else {

                    $app.showAlert('End Date should not be less than Start Date !', 3);
                    $("#txtTrainingTo").val('');
                }
            }
        });
        $('#btnAddTraining').on('click', function () {
            $training.addInitialize();
        });
        $('#frmEmp_Training').on('submit', function () {
            $training.save();
        });
    },
    LoadTrainings: function (employee) {
        $training.bindEvent();
        $training.selectedEmployeeId = employee.id;
        var dtTraining = $('#tblTraining').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "empTrainingNameid" },
                    { "data": "empTrainingEmployeeId" },
                    { "data": "empTrainingName" },
                     { "data": "empTrainingInstitute" },
                      { "data": "empTrainingCertfNo" },
                      { "data": "empTrainingFDate" },
                      { "data": "empTrainingTDate" },

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
             "aTargets": [2, 3, 4, 5, 6],
             "sClass": "word-wrap"

         },
      {
          "aTargets": [7],
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  $training.render(oData);
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure,you want to delete?')) {
                      $training.deleteData({ id: oData.empTrainingNameid, empId: $training.selectedEmployeeId, type: "training" });
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
                    url: $app.baseUrl + "Employee/GetEmpTraining",
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
                var r = $('#tblTraining tfoot tr');
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
        $training.canSave = true;
        var formData = document.forms["frmEmp_Training"];
        formData.elements["txTariningName"].value = "";
        formData.elements["txtTrainingFrom"].value = "";
        formData.elements["txtTrainingTo"].value = "";
        formData.elements["txtTrainingCertificate"].value = "";
        formData.elements["txtTrainingInstitute"].value = "";
        $training.selectedTrainingId = null;
    },
    render: function (data) {
        $training.canSave = true;
        $('#AddTraining').modal('toggle');
        var formData = document.forms["frmEmp_Training"];
        formData.elements["txTariningName"].value = data.empTrainingName;
        formData.elements["txtTrainingFrom"].value = data.empTrainingFDate;
        formData.elements["txtTrainingTo"].value = data.empTrainingTDate;
        formData.elements["txtTrainingCertificate"].value = data.empTrainingCertfNo;
        formData.elements["txtTrainingInstitute"].value = data.empTrainingInstitute;
        $training.selectedTrainingId = data.empTrainingNameid;
    },
    save: function () {
        if (!$training.canSave) {
            return false;
        }
        $training.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmEmp_Training"];
        var data = {
            empTrainingNameid: $training.selectedTrainingId,
            empTrainingEmployeeId: $training.selectedEmployeeId,
            empTrainingName: formData.elements["txTariningName"].value,
            empTrainingFDate: formData.elements["txtTrainingFrom"].value,
            empTrainingTDate: formData.elements["txtTrainingTo"].value,
            empTrainingCertfNo: formData.elements["txtTrainingCertificate"].value,
            empTrainingInstitute: formData.elements["txtTrainingInstitute"].value

        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpTraining",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var p = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $('#AddTraining').modal('toggle');
                        $training.LoadTrainings({ id: $training.selectedEmployeeId });
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
            data: JSON.stringify({ id: $training.selectedTrainingId, type: "training" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddTraining').modal('toggle');
                        var p = jsonResult.result;
                        $training.renderTraining(p);
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
    deleteData: function (data) {
        $training.selectedTrainingId = data.id;
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteEmpData",
            data: JSON.stringify({ id: data.id, empId: data.empId, type: data.type }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $training.LoadTrainings({ id: $training.selectedEmployeeId });
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