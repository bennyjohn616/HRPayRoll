//$('#ddMonth').change(function () {

//    var month = $('#ddMonth').val();
//    var sd = new Date($declaractionEntry.financeYear.startDate);
//    var ed = new Date($declaractionEntry.financeYear.EndDate);
//    if (ed.getMonth() + 1 >= month) {
//        $('#txtYear').val(ed.getFullYear());
//    }
//    else {
//        $('#txtYear').val(sd.getFullYear());
//    }

//});

$('#btnAppr').on('click', function () {
    debugger;
    var appcanreason = $('#txtAppCanRes').val();
    if(appcanreason=="")
    {
        $app.showAlert('Please Enter the cancel Reason', 4);
        return false;
    }
    else
    {
        debugger;
        $HRApprovalCancel.Cancelreason = appcanreason;
        $('#ViewApprovedcancelDetail').modal('toggle');
        $HRApprovalCancel.SaveApprovalcancel();
       
    }


});
$('#BtnApprovedCancelview').on('click', function () {
    debugger;
    var savestat = 0;
    if ($('#sltEmployeelist').val() == "00000000-0000-0000-0000-000000000000") {
        $app.showAlert('Please Select Employee Code', 4);
        savestat = 1;
      
    }
    if ($('#sltAppCanMonth').find('option:selected').text() == "--Select--") {
        $app.showAlert('Please Select Month', 4);
        savestat = 1;
    }
    if ($('#sltYear').find('option:selected').text() == "") {
        $app.showAlert('Please Select Year', 4);
        savestat = 1;
       
    }
    if (savestat == 0) {
        $('#idappcanbyhr').removeClass('nodisp');
        $HRApprovalCancel.GetApprovedcancelrecods();
    }
    else {
       
        $('#idappcanbyhr').addClass('nodisp');
        return false;
    }
});

var $HRApprovalCancel = {

    Empid: null,
    levType: null,
    fromdate: null,
    todate: null,
    levid: null,
    Cancelreason:null,
    loadInitial: function () {
        $companyCom.loadEmployee({ id: "sltEmployeelist" });
        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltYear', condi: 'Year' });
    },


    GetApprovedcancelrecods: function () {
        debugger;



        var dtClientList = $('#tblHRapprovedcancellist').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "Empcode" },
                    { "data": "EmployeeName" },
                    {
                        "data": "FromDate",
                        render: function (data) {
                            debugger;
                            var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                            var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                            return dateF;
                        }
                    },
                    {
                        "data": "FromDay",
                        render: function (data) {

                            var FromDayType;
                            if (data == 0) {
                                FromDayType = "Full";
                            }
                            else if (data == 1) {
                                FromDayType = "First Half";
                            }

                            else {
                                FromDayType = "Second Half";
                            }
                            return FromDayType;

                        }
                    },
                    {
                        "data": "EndDate",
                        render: function (data) {
                            debugger;
                            var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                            var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                            return dateF;
                        }
                    },
                    {
                        "data": "ToDay",
                        render: function (data) {

                            var ToDayType;
                            if (data == 0) {
                                ToDayType = "Full";
                            }
                            else if (data == 1) {
                                ToDayType = "First Half";
                            }

                            else {
                                ToDayType = "Second Half";
                            }
                            return ToDayType;

                        }
                    },
                    { "data": "NoOfDays" },
                    { "data": "LeaveTypeName" },
                    { "data": "LeaveStatus" },
                    { "data": null },


            ],
            "aoColumnDefs": [
       {
           "aTargets": [0],
           "sClass": "nodisp"
       },

         {
             "aTargets": [1, 2, 3, 4, 5, 6,7,8,9],
             "sClass": "word-wrap"

         },
         {
             "aTargets": [10],
             "sClass": "actionColumn",

             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                 debugger;
                 var c = $('<input type="button" id="btnHRCancelApproval" value="Cancel Approval" data-target="#ViewApprovedcancelDetail" data-toggle="modal"  class="btn custom-button marginbt7"  />');
                 c.button();
                 c.on('click', function () {
                    
                     debugger;
                     $("#txtAppCanRes").val('');
                     $HRApprovalCancel.Empid = sData.EmployeeId;
                     $HRApprovalCancel.levType = sData.LeaveType;
                     var date = new Date(parseInt(sData.FromDate.replace(/(^.*\()|([+-].*$)/g, '')));
                     var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                     var date1 = new Date(parseInt(sData.EndDate.replace(/(^.*\()|([+-].*$)/g, '')));
                     var dateT = date1.getDate() + '/' + $payroll.GetMonthName((date1.getMonth() + 1)) + '/' + date1.getFullYear();
                     $HRApprovalCancel.fromdate = dateF;
                     $HRApprovalCancel.todate = dateT;
                     $HRApprovalCancel.levid = sData.Id;
                 });
                 $(nTd).empty();
                 $(nTd).prepend(c);
             }
         }

            ],


            ajax: function (data, callback, settings) {
                debugger;

                var Year = $("#sltYear").val();
                var Month = $("#sltAppCanMonth").val();
                var Empid = $("#sltEmployeelist").val();
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "LeaveRequest/PayrollProcessedCheckforapprovalleavecancel",

                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Employeeid: Empid, month: Month, year: Year }),
                    async: false,
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
                                $('#idappcanbyhr').addClass('nodisp');
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblCalDetails tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblCalDetails thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });


        

    },
    SaveApprovalcancel: function () {
        debugger;
        $app.showProgressModel();
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/SaveHRApprovedCancel",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EmployeeId: $HRApprovalCancel.Empid, leaveType: $HRApprovalCancel.levType, fromdate: $HRApprovalCancel.fromdate, todate: $HRApprovalCancel.todate, levreqid: $HRApprovalCancel.levid, Cancelreson: $HRApprovalCancel.Cancelreason }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $app.hideProgressModel();
                $app.showAlert(out.Message, 2);
                $HRApprovalCancel.GetApprovedcancelrecods();
            },
            error: function (msg) {
            }
        });

    },

}  