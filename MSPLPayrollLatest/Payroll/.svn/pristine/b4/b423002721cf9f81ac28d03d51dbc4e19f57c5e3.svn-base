
$('#txtEffectiveDate').change(function () {
    var doj = new Date($($employee.formData).find('#txtDOJ').val());
    var EffectiveDate = new Date($("#txtEffectiveDate").val());
    if ((doj > EffectiveDate)) {
        $app.showAlert("EffectiveDate should not be lesser than DOJ", 4);
        $("#txtEffectiveDate").val('');
        return false;
    }

});






var $benefitComponent = {
    canSave: false,
    selectedBencomponentId: '',
    selectedEmployeeId: '',
    bindEvent: function () {
        $('#btnAddBenefitComponent').on('click', function () {
            $benefitComponent.addInitialize();
        });
        $('#frmEmpBenefitComponent').on('submit', function () {
            $benefitComponent.save();
        });
    },
    LoadbenefitComponents: function (employee) {
        $benefitComponent.loadbenefitDopDown({ id: 'sltBenefitComponent' });
        $benefitComponent.bindEvent();
        $benefitComponent.selectedEmployeeId = employee.id;
        var dtBenefitComponent = $('#tblBenefitComponent').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "empBenefitCompid" },
                    { "data": "empBenefitCompEmployeeId" },
                  //   { "data": "empBenefitComponentId" },
                    { "data": "empBenefitComponentName" },
                     { "data": "empBenefitCompAmt" },
                      { "data": "empBenefitCompEffDate" },
                    { "data": null }
            ],
            "aoColumnDefs": [
        {
            "aTargets": [0, 1],
            "sClass": "nodisp",
            "bSearchable": false
        },
         {
             "aTargets": [2, 3, 4],
             "sClass": "word-wrap"

         },
      {
          "aTargets": [5],
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  $benefitComponent.render(oData);
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure,do you want to delete?')) {
                      $benefitComponent.deleteData({ Id: oData.empBenefitCompid, empId: $benefitComponent.selectedEmployeeId, type: "benefitComponent" });
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
                    url: $app.baseUrl + "Employee/GetEmpBenefitcomponents",
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
                var r = $('#tblEmpFamily tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblCompany thead').append(r);
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
        $benefitComponent.canSave = true;
        var formData = document.forms["frmEmpBenefitComponent"];
        //formData.elements["txtEmpComponentName"].value = "";
        formData.elements["sltBenefitComponent"].value = "";
        formData.elements["txtAmount"].value = "";
        formData.elements["txtEffectiveDate"].value = "";
        $benefitComponent.selectedBencomponentId = null;
    }
    ,
    render: function (data) {
        $benefitComponent.canSave = true;
        $('#AddBenefitComponent').modal('toggle');
        var formData = document.forms["frmEmpBenefitComponent"];
        // formData.elements["txtEmpComponentName"].value = data.empBenefitComponentName;
        formData.elements["sltBenefitComponent"].value = data.empBenefitComponentId;
        formData.elements["txtAmount"].value = data.empBenefitCompAmt;
        formData.elements["txtEffectiveDate"].value = data.empBenefitCompEffDate;
        $benefitComponent.selectedBencomponentId = data.empBenefitCompid;
    },
    save: function () {
        if (!$benefitComponent.canSave) {
            return false;
        }
        $benefitComponent.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmEmpBenefitComponent"];
        var data = {
            empBenefitCompid: $benefitComponent.selectedBencomponentId,
            empBenefitCompEmployeeId: $benefitComponent.selectedEmployeeId,
            empBenefitComponentName: '',//formData.elements["txtEmpComponentName"].value,
            empBenefitComponentId: formData.elements["sltBenefitComponent"].value,
            empBenefitCompAmt: formData.elements["txtAmount"].value,
            empBenefitCompEffDate: formData.elements["txtEffectiveDate"].value
        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpBenfitComponent",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var p = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $benefitComponent.LoadbenefitComponents({ id: $benefitComponent.selectedEmployeeId });
                        $('#AddBenefitComponent').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    },

    get: function () {
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpData",
            data: JSON.stringify({ id: $benefitComponent.selectedEmployeementID, type: "benefitComponent" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $benefitComponent.render(p);
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
        $benefitComponent.selectedBencomponentId = data.Id;
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
                        $benefitComponent.LoadbenefitComponents({ id: $benefitComponent.selectedEmployeeId });
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
    loadbenefitDopDown: function (dropControl) {
        $.ajax({
            url: $app.baseUrl + "Employee/GetBenefitComponent",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $('#' + dropControl.id).html('');
                        $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                        $.each(p, function (index, blood) {
                            $('#' + dropControl.id).append($("<option></option>").val(blood.id).html(blood.displayName));
                        });
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