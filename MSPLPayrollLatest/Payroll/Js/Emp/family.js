//    dateNow = new Date();
//    var years = dateNow.getFullYear() - birthdayDate.getFullYear();
//    var months = dateNow.getMonth() - birthdayDate.getMonth();
//    var days = dateNow.getDate() - birthdayDate.getDate();
$("#txtDateOfBirth").change(function () {
 
    $('#txtAge').attr('readonly', true);
    $('#txtAge').addClass('input-disabled');
    var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    var d = new Date();
    var dob = new Date($("#txtDateOfBirth").val());
    var currentdate = new Date(d.getDate() + '/' + $payroll.GetMonthName((d.getMonth() + 1)) + '/' + d.getFullYear());
    var totaldays = Math.round(Math.abs((dob.getTime() - currentdate.getTime()) / (oneDay)));
    var age = totaldays / 365;
    $("#txtAge").val(age.toFixed(0));
});
// created by AjithPanner on 13/11/17
$("#btnSavefamily").on('click', function () {
   
    
    $family.canSave = true;
    $family.saveFamily();
    
});
var $family = {
    canSave: false,
    selectedFamilyId: '',
    selectedEmployeeId: '',
    formData: document.forms["frmEmpFamily"],
    bindEvent: function () {
   
        $('#btnAddFamily').on('click', function () {
            $family.addInitialize();
        });

        //$('#frmEmpFamily').on('submit', function () {
        //    $family.saveFamily();
        //});
    },
    LoadFamilys: function (employee) {
 
        $family.bindEvent();
        $family.selectedEmployeeId = employee.id;
        var dtClientList = $('#tblEmpFamily').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "empFamilyid" },
                    { "data": "empFamilyEmployeeId" },
                    { "data": "empFamilyName" },
                     { "data": "empFamilyAddress" },
                      { "data": "relation.name" },
                       { "data": "empFamilyDOB" },
                        { "data": "empFamilyAge" },
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
             
                  $family.renderFamily(oData);
                  return false;
              });
              c.button();
              c.on('click', function () {
           
                  if (confirm('Are you sure ,do you want do delete?'))
                      $family.deleteData({ Id: oData.empFamilyid, empId: $family.selectedEmployeeId, type: "family" });
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend(b, c);


          }
      }],
            ajax: function (data, callback, settings) {
            
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetFamilys",
                    contentType: "application/json; charset=utf-8",
                    data: "{'employeeId':'" + $family.selectedEmployeeId + "'}",
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

    },
    addInitialize: function () {
    
        $family.canSave = true;
        $family.selectedFamilyId = null;
        $($family.formData).find('#txtName').val('');
        $($family.formData).find('#txtAddress').val('');
        $($family.formData).find('#txtDateOfBirth').val('');
        $($family.formData).find('#ddRelationShip').val(0);
        $($family.formData).find('#txtAge').val('');
    },
    renderFamily: function (data) {
   
        $family.canSave = true;
        $family.selectedFamilyId = data.empFamilyid;
        $('#AddFamily').modal('toggle');
        var formData = document.forms["frmEmpFamily"];
        $($family.formData).find('#txtName').val(data.empFamilyName);
        $($family.formData).find('#txtAddress').val(data.empFamilyAddress);
        $($family.formData).find('#txtDateOfBirth').val(data.empFamilyDOB);
        $($family.formData).find('#ddRelationShip').val(data.relation.id);
        $($family.formData).find('#txtAge').val(data.empFamilyAge);
    },
    getFamily: function () {
  
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpData",
            data: JSON.stringify({ id: $family.selectedFamilyId, type: "employee" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddFamily').modal('toggle');
                        var p = jsonResult.result;
                        $employee.renderEmployee(p);
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
    saveFamily: function () {      
        var isValidForm = document.forms['frmEmpFamily'].checkValidity();
        if (!isValidForm) {
            $app.showAlert('Please fill required field', 4);
            return false;
        }
        if ($("#ddRelationShip").val() == "0" || $("#txtDateOfBirth").val()=="") {
            $app.showAlert('Please fill required field', 4);
            return false;
        }
        $family.canSave = false;
        $app.showProgressModel();
        var data = {
            empFamilyid: $family.selectedFamilyId,
            empFamilyEmployeeId: $family.selectedEmployeeId,
            empFamilyName: $($family.formData).find('#txtName').val(),
            empFamilyAddress: $($family.formData).find('#txtAddress').val(),
            empFamilyDOB: $($family.formData).find('#txtDateOfBirth').val(),
            empFamilyAge: $($family.formData).find('#txtAge').val(),
            relation: { id: $($family.formData).find('#ddRelationShip').val(), name: "" }
        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpFamily",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var p = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $('#AddFamily').modal('toggle');
                        $family.LoadFamilys({ id: $family.selectedEmployeeId });
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert('Update failed', 4);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },
    deleteData: function (data) {
   
        $family.selectedFamilyId = data.Id;
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
                        $family.LoadFamilys($family.selectedEmployeeId);
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