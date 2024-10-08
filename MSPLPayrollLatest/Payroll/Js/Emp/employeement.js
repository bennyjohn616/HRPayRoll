$("#txtEmployeementWorkTo,#txtEmployeementWorkFrom").change(function () {

    var datefrom = new Date($("#txtEmployeementWorkFrom").val());
    var dateto = new Date($("#txtEmployeementWorkTo").val());
    var datedoj = new Date($("#txtDOJ").val());
    if ($("#txtEmployeementWorkFrom").val() != '' && $("#txtEmployeementWorkTo").val() != '') {
        if (dateto > datefrom) {

        }
        else {

            $app.showAlert('EmployeementWorkFrom Date should not be greater than EmployeementWorkTo Date !', 3);
            $("#txtEmployeementWorkTo").val('');
        }



    } else {

    } if (dateto > datedoj) {
        $app.showAlert('EmployeementWork Date should not be greater than Joining Date !', 3);
        $("#txtEmployeementWorkTo").val('');
    }
});
var $employeement = {
    canSave: false,
    selectedEmployeementID: '',
    selectedEmployeeId: '',
    EditWorkFromDt: '',
    EditWorkToDt: '',
    bindEvent: function () {
        $('#btnAddEmployeement').on('click', function () {
            $employeement.addInitialize();
        });
        $('#frmEmployeement').on('submit', function () {
            $employeement.save();
        });

        $("#txtEmployeementWorkFrom,#txtEmployeementWorkTo").change(function () {

            var workfrom = new Date($("#txtEmployeementWorkFrom").val());
            var workto = new Date($("#txtEmployeementWorkTo").val());
            if (workfrom != '' && workto != '') {
                if (Date.parse(workfrom) > Date.parse(workto)) {
                    $("#txtEmployeementWorkFrom").val('');
                    $("#txtEmployeementWorkTo").val('');
                    $app.showAlert("Invalid Date Range!\n End date should be greater than Start date", 4);
                }
                else {
                    var Wt = 0;
                    $("#tblEmployeement tbody tr").each(function () {

                        var GWorkfrom = new Date($(this).find("td:nth-child(6)").html());
                        var GWorkto = new Date($(this).find("td:nth-child(7)").html());
                        var EditWfrDt = new Date($employeement.EditWorkFromDt);
                        var EditWtoDt = new Date($employeement.EditWorkToDt);
                        if ($employeement.EditWorkFromDt == $(this).find("td:nth-child(6)").html() && $employeement.EditWorkToDt == $(this).find("td:nth-child(7)").html()) {
                        }
                        else if (GWorkfrom != "Invalid Date" && GWorkto != "Invalid Date" && workfrom != "Invalid Date" && workto != "Invalid Date") {
                            if (GWorkfrom <= workto && GWorkto >= workfrom) {
                                Wt = 1;
                            }
                            if ((GWorkfrom > workfrom && GWorkto > workto) || (GWorkfrom < workfrom && GWorkto < workto)) {
                            }
                            else {
                                Wt = 1;
                            }
                        }

                    });
                    if (Wt == 1) {
                        $app.showAlert("Invalid Employeement Date", 4);
                        $("#txtEmployeementWorkFrom").val('');
                        $("#txtEmployeementWorkTo").val('');
                        return false;
                    }
                    return false;
                }
            }


        });
    },





    LoadEmployeements: function (employee) {
        $employeement.bindEvent();
        $employeement.selectedEmployeeId = employee.id;
        var dtemployeement = $('#tblEmployeement').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                        { "data": "empEmployeementid" },
                        { "data": "empEmployeementEmployeeId" },
                        { "data": "empEmployeementEmpCode" },
                        { "data": "empEmployeementCompanyName" },
                        { "data": "empEmployeementPositionHeld" },
                        { "data": "empEmployeementWorkFrom" },
                        { "data": "empEmployeementWorkTo" },
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
                  $employeement.render(oData); // EditClientRecord(oData.Id, oData.CompanyName);
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure,do you want to delete?')) {
                      $employeement.deleteData({ Id: oData.empEmployeementid, empId: $employeement.selectedEmployeeId, type: "employeement" });
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
                    url: $app.baseUrl + "Employee/GetEmpEmployeement",
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
                var r = $('#tblEmployeement tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblEmployeement thead').append(r);
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
        $employeement.canSave = true;
        var formData = document.forms["frmEmployeement"];
        formData.elements["txtEmployeementEmpCode"].value = "";
        formData.elements["txtEmployeementCompanyName"].value = "";
        formData.elements["txtEmployeementPosition"].value = "";
        formData.elements["txtEmployeementWorkFrom"].value = "";
        formData.elements["txtEmployeementWorkTo"].value = "";
        $employeement.selectedEmployeementID = null;
        $employeement.EditWorkFromDt = '';
        $employeement.EditWorkToDt = '';

    },
    render: function (data) {
        $employeement.canSave = true;
        $('#AddEmployeement').modal('toggle');
        var formData = document.forms["frmEmployeement"];
        formData.elements["txtEmployeementEmpCode"].value = data.empEmployeementEmpCode;
        formData.elements["txtEmployeementCompanyName"].value = data.empEmployeementCompanyName;
        formData.elements["txtEmployeementPosition"].value = data.empEmployeementPositionHeld;
        formData.elements["txtEmployeementWorkFrom"].value = data.empEmployeementWorkFrom;
        formData.elements["txtEmployeementWorkTo"].value = data.empEmployeementWorkTo;
        $employeement.selectedEmployeementID = data.empEmployeementid;
        $employeement.EditWorkFromDt = data.empEmployeementWorkFrom;
        $employeement.EditWorkToDt = data.empEmployeementWorkTo;

    },
    //ToDatecheck: function () {
    //    
    //    var formData = document.forms["frmEmployeement"];
    //    var wrktodate = {
    //        empEmployeementEmployeeId: $employeement.selectedEmployeeId,
    //        empEmployeementWorkTo:formData.elements["txtEmployeementWorkTo"].value,
    //    };
    //    $.ajax({
    //        url: $app.baseUrl + "Employee/WrktodateCheck",
    //        data: JSON.stringify({ dataValue: wrktodate }),
    //        dataType: "json",
    //        contentType: "application/json",
    //        type: "POST",
    //        success: function (jsonResult) {
    //            $app.clearSession(jsonResult);
    //            switch (jsonResult.Status) {
    //                case true:

    //                    break;
    //                case false:
    //                    $app.hideProgressModel();
    //                    $app.showAlert(jsonResult.Message, 4);
    //                    $('#txtEmployeementWorkTo').val('');
    //                    $('#txtEmployeementWorkTo').focus();
    //                    break;
    //            }
    //        },

    //    });
    //},





    save: function () {
        if (!$employeement.canSave) {
            return false;
        }
        $employeement.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmEmployeement"];
        var data = {
            empEmployeementid: $employeement.selectedEmployeementID,
            empEmployeementEmployeeId: $employeement.selectedEmployeeId,
            empEmployeementEmpCode: formData.elements["txtEmployeementEmpCode"].value,
            empEmployeementCompanyName: formData.elements["txtEmployeementCompanyName"].value,
            empEmployeementPositionHeld: formData.elements["txtEmployeementPosition"].value,
            empEmployeementWorkFrom: formData.elements["txtEmployeementWorkFrom"].value,
            empEmployeementWorkTo: formData.elements["txtEmployeementWorkTo"].value,

        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpEmployeement",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var p = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $employeement.LoadEmployeements({ id: $employeement.selectedEmployeeId });
                        $('#AddEmployeement').modal('toggle');
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
            data: JSON.stringify({ id: $employeement.selectedEmployeementID, type: "employeement" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddFamily').modal('toggle');
                        var p = jsonResult.result;
                        $employeement.render(p);
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
        $employeement.selectedEmployeementID = data.Id;
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
                        $employeement.LoadEmployeements({ id: $employeement.selectedEmployeeId });
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