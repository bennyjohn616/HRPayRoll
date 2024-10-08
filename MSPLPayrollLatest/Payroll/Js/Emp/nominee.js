



$("#txtAmountPercentage").change(function () {

    var per = parseFloat($("#txtAmountPercentage").val());
    if (per != 0) {
        var TotAmt = 0.0;
        $("#tblNominee tbody tr").each(function () {
            TotAmt = TotAmt + parseFloat($(this).find("td:nth-child(8)").html());
        });
        var percntage = $("#txtAmountPercentage").val();
        if (percntage > 100) {
            $("#txtAmountPercentage").val('');
            $app.showAlert("Percentage should not be greater than 100!!", 4);
        }
        TotAmt = TotAmt - parseFloat($nominee.selectPerAmt);
        if ((parseFloat(TotAmt) + parseFloat($("#txtAmountPercentage").val())) > 100.1) {
            $("#txtAmountPercentage").val('');
            $app.showAlert("Total Percentage should not be greater than 100!!", 4);
        }
    }
    else {
        $app.showAlert(" Percentage should not be 0!!", 4);
        $("#txtAmountPercentage").focus();
        $("#txtAmountPercentage").val('');
    }
});


$('#txtNDateOfBirth').change(function () {

    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();

    var output = d.getFullYear() + '/' +
        (month < 10 ? '0' : '') + month + '/' +
        (day < 10 ? '0' : '') + day;

    var Entereddate = new Date($("#txtNDateOfBirth").val());
    var month = Entereddate.getMonth() + 1;
    var day = Entereddate.getDate();

    var Eoutput = Entereddate.getFullYear() + '/' +
        (month < 10 ? '0' : '') + month + '/' +
        (day < 10 ? '0' : '') + day;

    if (Entereddate != "") {
        if (Eoutput > output || Eoutput == output) {
            $app.showAlert("Date of Birth should be lesser that Current date", 4);

            $("#txtNDateOfBirth").val('');
            return false;
        }
        else {

        }
    }

});



$("#ddNomineeRelationShip").change(function () {
    $("#tblNominee tbody tr").each(function () {
        if ($("#ddNomineeRelationShip option:selected").text().trim().toLowerCase() == $(this).find("td:nth-child(5)").html().trim().toLowerCase() && ($("#ddNomineeRelationShip").val() == 1 || $("#ddNomineeRelationShip").val() == 2)) {
            $app.showAlert($("#ddNomineeRelationShip option:selected").text() + "  Already Exist ", 4);
            $("#ddNomineeRelationShip").val(0);
            return false;
        }
    });
});






var $nominee = {
    canSave: false,
    selectPerAmt: 0,
    selectedEmployeeId: '',
    selectedNomineeId: '',
    formData: document.forms["frmEmpNominee"],
    bindEvent: function () {

        $("#txtNDateOfBirth").change(function () {
            $('#txtNAge').attr('readonly', true);
            $('#txtNAge').addClass('input-disabled');
            var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
            var r = new Date();
            var dob = new Date($("#txtNDateOfBirth").val());
            var currentdate = new Date(r.getDate() + '/' + $payroll.GetMonthName((r.getMonth() + 1)) + '/' + r.getFullYear());
            var totaldays = Math.round(Math.abs((dob.getTime() - currentdate.getTime()) / (oneDay)));
            var Age = totaldays / 365;
            $("#txtNAge").val(Age.toFixed(0));

        });
        $('#btnAddNominee').on('click', function () {
            $nominee.addInitialize();

        });
        $('#frmEmpNominee').on('submit', function (e) {
            $nominee.save();
            e.preventDefault();
        });
    },
    LoadNominees: function (employee) {

        $nominee.bindEvent();
        $nominee.selectedEmployeeId = employee.id;
        var dtnominee = $('#tblNominee').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "empNomineeid" },
                    { "data": "empNomineeEmployeeId" },
                    { "data": "empNomineeName" },
                     { "data": "empNomineeAddress" },
                      { "data": "relation.name" },
                       { "data": "empNomineeDOB" },
                        { "data": "empNomineeAge" },
                        { "data": "empNomineeAmtPercent" },
                        { "data": "empGuardianAddr" },
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
             "aTargets": [2, 3, 4, 5, 6, 7, 8],
             "sClass": "word-wrap"

         },
      {
          "aTargets": [9],
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  $nominee.render(oData);
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure,do you want to delete?')) {
                      $nominee.deleteData({ Id: oData.empNomineeid, empId: $nominee.selectedEmployeeId, type: "nominee" });
                  }
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend(b, c);


          }
      }],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetEmpNominees",
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
                var r = $('#tblNominee tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblNominee thead').append(r);
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
        $nominee.canSave = true;
        $($nominee.formData).find('#txtName').val('');
        $($nominee.formData).find('#txtAddress').val('');
        $($nominee.formData).find('#ddNomineeRelationShip').val(0);
        $($nominee.formData).find('#txtNDateOfBirth').val('');
        $($nominee.formData).find('#txtNAge').val('');
        $($nominee.formData).find('#txtAmountPercentage').val('');
        $($nominee.formData).find('#txtGuardianNameAddress').val('');
        $nominee.selectedNomineeId = null;
        $nominee.selectPerAmt = 0;
    },
    render: function (data) {

        $nominee.canSave = true;
        $('#AddNominee').modal('toggle');
        $($nominee.formData).find('#txtName').val(data.empNomineeName);
        $($nominee.formData).find('#txtAddress').val(data.empNomineeAddress);
        $($nominee.formData).find('#ddNomineeRelationShip').val(data.relation.id);
        $($nominee.formData).find('#txtNDateOfBirth').val(data.empNomineeDOB);
        $($nominee.formData).find('#txtNAge').val(data.empNomineeAge);
        $($nominee.formData).find('#txtAmountPercentage').val(data.empNomineeAmtPercent);
        $($nominee.formData).find('#txtGuardianNameAddress').val(data.empGuardianAddr);
        $nominee.selectedNomineeId = data.empNomineeid;
        $nominee.selectPerAmt = data.empNomineeAmtPercent;
    },





    save: function () {

        $(".control").keypress(function (evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
              ((evt.which) ? evt.which : 0));
            if (charCode > 32 && (charCode == 9 || charCode == 40 || charCode == 39 || charCode == 37 || charCode == 27 || charCode < 65 || charCode > 90) &&
              (charCode < 97 || charCode > 122)) {
                alert("Enter Only Alphabets Value");
                return false;
            }
            else {
                return true;
            }
        });
        if ($($nominee.formData).find('#txtNDateOfBirth').val() != "" && $($nominee.formData).find('#ddNomineeRelationShip').val() != 0) {

            if (!$nominee.canSave) {
                return false;
            }
            $nominee.canSave = false;
            $app.showProgressModel();
            var data = {
                empNomineeid: $nominee.selectedNomineeId,
                empNomineeEmployeeId: $nominee.selectedEmployeeId,
                empNomineeName: $($nominee.formData).find('#txtName').val(),
                empNomineeAddress: $($nominee.formData).find('#txtAddress').val(),
                empNomineeDOB: $($nominee.formData).find('#txtNDateOfBirth').val(),
                empNomineeAge: $($nominee.formData).find('#txtNAge').val(),
                empNomineeAmtPercent: $($nominee.formData).find('#txtAmountPercentage').val(),
                empGuardianAddr: $($nominee.formData).find('#txtGuardianNameAddress').val(),
                relation: { id: $($nominee.formData).find('#ddNomineeRelationShip').val(), name: "" }

            };
            $.ajax({
                url: $app.baseUrl + "Employee/SaveEmpNominee",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    var p = jsonResult.result;
                    switch (jsonResult.Status) {
                        case true:
                            $nominee.LoadNominees({ id: $nominee.selectedEmployeeId });
                            $('#AddNominee').modal('toggle');
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
        }
        else {
            $app.showAlert($($nominee.formData).find('#txtNDateOfBirth').val() == "" ? 'Please Select the date of Birth' : 'Please Select the Relationship', 4);

        }
    },

    get: function () {
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpData",
            data: JSON.stringify({ id: $nominee.selectedNomineeId, type: "nominee" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $nominee.render(p);
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

        $nominee.selectedNomineeId = data.Id;
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteEmpData",
            data: JSON.stringify({ id: $nominee.selectedNomineeId, empId: $nominee.selectedEmployeeId, type: "nominee" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //nominee.LoadNominees({ id: data.Id, empId: data.empId, type: data.type });
                        $nominee.LoadNominees({ id: $nominee.selectedEmployeeId });
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