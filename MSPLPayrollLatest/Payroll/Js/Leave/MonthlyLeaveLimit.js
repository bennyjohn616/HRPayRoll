function isNumberKey(evt) {
    
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
        return false;
    else {
        var len = document.getElementById("txtNoOfDays").value.length;
        var index = document.getElementById("txtNoOfDays").value.indexOf('.');

        if (index > 0 && charCode == 46) {
            return false;
        }
        if (index > 0) {charCode
            var CharAfterdot = (len + 1) - index;
            if (CharAfterdot > 2) {
                return false;
            }
            if (charCode!=53) {
                return false;
            }
        }
        
    }
    return true;
}

function MonthlylimitVal()
{
    if ($("#txtNoOfDays").val() > 31) {
        $app.showAlert('Invalid Data!', 4);
        $("#txtNoOfDays").val('');
        return false;
    }
} 
function MonthlylimitVal() {
    if ($("#txtcrdays").val() > 31) {
        $app.showAlert('Invalid Data!', 4);
        $("#txtcrdays").val('');
        return false;
    }
}
$("#btnSave").on('click', function () {
    
    if ($('#ddLeaveType').val() == "00000000-0000-0000-0000-000000000000") {
        $app.showAlert('Please Select Leave Type!', 4) ;
    }
    else if ($("#txtNoOfDays").val() == "") {
        $app.showAlert('Please Fill No of days!', 4);
    }
    else {
        $MonthlyLeaveLimit.save();
    }

});

$('#ddLeaveType').change(function () {
    
    if ($('#ddLeaveType').find('option:selected').text() == "LOSS OF PAY") {
        $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
        $app.showAlert('You cannot set Monthly leave Limit for LOSS OF PAY DAYS', 4);
    }
    //if ($('#ddLeaveType').find('option:selected').text() == "ONDUTY") {
    //    $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
    //    $app.showAlert('You cannot set Monthly leave Limit for ONDUTY', 4);
    //}

});
var $MonthlyLeaveLimit = {
    LoadMonthlyLeaveLimit: function () {
        var dtMothlyLeaveLimit = $('#tblMothlyLeaveLimit').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            "oLanguage": {
                "sEmptyTable":     "No Data Avaliable"
            },
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    {
                        "data": "LeaveType"},
                    { "data": "MaxDays" },
                     { "data": "CrDays" },
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
             "sClass": "word-wrap"

         },
         {
             "aTargets": [2],
             "sClass": "word-wrap"

         },
                  {
                      "aTargets": [3],
                      "sClass": "word-wrap"

                  },

      {
          "aTargets": [4],
          "sClass": "actionColumn",

          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {             
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $MonthlyLeaveLimit.DeleteData(oData);
                  }
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend(c);
          }
      }
            ],
            ajax: function (data, callback, settings) {
                
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetMonthlyLeaveLimit",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        
                        var out = jsonResult.result;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
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

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            //fnInitComplete: function (oSettings, json) {

            //},
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    save: function () {
        $app.showProgressModel();
        if ($('#txtcrdays').val() == "")
        {
            $('#txtcrdays').val("0");
        }
        var data = {
            LeaveType: $('#ddLeaveType').find('option:selected').text(),
            LeaveTypeId: $('#ddLeaveType').val(),            
            MaxDays: $('#txtNoOfDays').val(),            
            CrDays: $('#txtcrdays').val(),
        };
        $.ajax({
            url: $app.baseUrl + "Leave/SaveMonthlyLeaveLimit",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddMonthlyLeaveLimit').modal('toggle');
                        $MonthlyLeaveLimit.LoadMonthlyLeaveLimit();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                       
                        break;
                    case false:
                        
                        if (jsonResult.result.statuserror == 1) {                           
                            $('#AddMonthlyLeaveLimit').modal('hide');                     
                        }
                        else {
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 4);
                            AddInitialize();
                        }

                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }

        });
    },
    DeleteData: function (context) {
        
        $.ajax({
            url: $app.baseUrl + "Leave/DeleteMonthlyLeaveLimit",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.result) {
                    case true:
                        $MonthlyLeaveLimit.LoadMonthlyLeaveLimit();
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
    },
    AddInitialize: function () {
        
        var formData = document.forms["frmMonthlyLeaveSettings"];        
        $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000'),
        //$('#txtToDate').val(''),
        $('#txtNoOfDays').val(''),
        $('#txtcrdays').val('')
    },
};