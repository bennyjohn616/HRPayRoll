﻿
$("#employeeCode").change(function () {
    
    if ($('#employeeCode').find('option:selected').text() != "--Select--") {
        $("#iddebittable").removeClass("nodisp");
        $DebitLeave.LoadDebitLeave();
    } else {
        $("#iddebittable").addClass("nodisp")
    }
});
$("#leaveType").change(function () {
    
    $DebitLeave.LoadLeaveBalance();

});
$("#txtNoOfDays,#leaveBalance").change(function () {
    
   // var lb = parseFloat($("#leaveBalance").val());
    var lb = parseFloat($("#leaveBalance").val());
    var nd = parseFloat($("#txtNoOfDays").val());
    if (nd > lb) {
        $app.showAlert("No Of Days is not valid", 4);
        $("#txtNoOfDays").val("");
    }

});
$("#txtNoOfDays,#leaveBalance").blur(function () {
    
    var lb = parseFloat($("#leaveBalance").val());
    var nd =parseFloat($("#txtNoOfDays").val());
    if (nd > lb) {
        $app.showAlert("No Of Days is not valid", 4);
        $("#txtNoOfDays").val("");
    }

});




var $DebitLeave = {
    save: function () {
        
        if ($('#employeeCode').find('option:selected').text() == "--Select--") {
            $app.showAlert("please select the leave type", 4);
            return;
        }
        if ($('#entryDate').datepicker('getDate') == null)
        {
            $app.showAlert("please enter the Debit date", 4);
            return;
        }
        var dayscount = parseFloat($('#txtNoOfDays').val());
        if (dayscount==0) {
            $app.showAlert("Zero is not a valid Debit days", 4);
            return;
        }
        if ($('#idDebitReason').val() == "") {
            $app.showAlert("please enter the Debit Reason", 4);
            return;
        }
        var data = [];
        var value = new Object();
        value.EmployeeId = $("#employeeCode").val(),
        value.DebitLeaveEntryDate = $('#entryDate').datepicker('getDate');
        value.LeaveType = $("#leaveType").val(),
        value.NoOfDays = $("#txtNoOfDays").val(),
         value.Reason = $('#idDebitReason').val(),
        data.push(value);
        $.ajax({
            url: $app.baseUrl + "Leave/SaveDebitleave",
            data: JSON.stringify({ datavalue: data}),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                
                switch (jsonResult.Status) {
                    case true:
                        $('#addDebitLeave').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $DebitLeave.LoadDebitLeave();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },

        });
        },
    LoadPopup: function () {
        $("#entryDate").val("");
        $("#leaveBalance").val("");
        $("#txtNoOfDays").val("");
        $companyCom.loadLeaveType({ id: 'leaveType' })
       
    },
    LoadLeaveBalance: function () {
        
        $.ajax({
            type: 'POST',
            cache: false,
            url: $app.baseUrl + "Leave/BalanceLeave",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ empid: $("#employeeCode").val(), leaveType: $("#leaveType").val() }),
            async: false,
            dataType: "json",
            success: function (jsonResult) {
                
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $("#leaveBalance").val(out.AvailableDays)
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                }

            },
            error: function (msg) {
            }
        });
    },  
    LoadDebitLeave: function () {
        
        $payroll.initDatetime();

        var dtClientList = $('#tblDebitLeave').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "id" },
             { "data": "EmployeeCode" },
             { "data": "LeaveDate" },
             { "data": "LeaveType" },
             { "data": "Debit" },
             {
               "data": "Reason"
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
             "sClass": "word-wrap"

         },
         {
             "aTargets": [3],
             "sClass": "word-wrap"

         },
          {
              "aTargets": [4],
              "sClass": "word-wrap"

          },
      {
          "aTargets": [5],
          "sClass": "word-wrap"

      }

            ],
            ajax: function (data, callback, settings) {
                
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetDebitLeave",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EMPID: $("#employeeCode").val() }),
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
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    DeleteData: function (context) {
        
        $.ajax({
            url: $app.baseUrl + "Leave/DeleteDebitLeave",
            data: JSON.stringify({ finId: context.id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $DebitLeave.LoadDebitLeave();
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
}