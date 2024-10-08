
$("#btnReportView").on('click', function () {

    if ($("#txtReportFromDate").val() == "" || $("#txtReportToDate").val() == "") {
        $app.showAlert('Please Select the Dates!', 4);
    }
    else {
        $("#reportdivid").removeClass('nodisp')
        $EmployeeLeaveReport.LoadEmployeeLeaveReport();
        //reportdivid
    }

});
$("#btnReportExport").on('click', function () {

    if ($("#txtReportFromDate").val() == "" || $("#txtReportToDate").val() == "") {
        $app.showAlert('Please Select the Dates!', 4);
    }
    else {
        $EmployeeLeaveReport.Excelreportexport();
    }

});
$("#txtReportToDate,#txtReportFromDate").change(function () {
    var datefrom = new Date($("#txtReportFromDate").val());
    var dateto = new Date($("#txtReportToDate").val());
    if ($("#txtReportFromDate").val() != '' && $("#txtReportToDate").val() != '') {
        if (dateto >= datefrom) {

        }
        else {

            $app.showAlert('End Date should not be less than Start Date !', 3);
            $("#txtReportToDate").val('');
        }


    }
});


$('#empLeaveNavtabs a').on("click", function (e) {
    debugger;
    $("#tabcontent").removeClass('nodisp');
    $EmployeeLeaveReport.LoadLeaveRelated(this);
});


var $EmployeeLeaveReport = {

    Fromdate: $("#txtReportFromDate").val(),
    Todate: $("#txtReportToDate").val(),
    LoadHRViewReport: function () {



        var dtClientList = $('#tblHRViewReport').DataTable({
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

                 var c = $('<input type="button" id="btnViewStat" value="View Status" data-target="#ViewStatusPopup" data-toggle="modal"  class="btn custom-button marginbt7"  />');
                 c.button();
                 c.on('click', function () {
                     $LeaveReport.LoadPendingStatwithoutfinyearPopup(sData);


                 });

                 $(nTd).empty();
                 $(nTd).prepend(c);
             }
         }

            ],


            ajax: function (data, callback, settings) {


                var fdate = $("#txtReportFromDate").val();
                var Tdate = $("#txtReportToDate").val();
                var EmpId = $("#sltmgrviewrptEmpCode").val();
                var LevStat = $("#sltleavestatus").val();
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "LeaveRequest/GetHRviewReport",

                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ FromDate: fdate, ToDate: Tdate, EmployeeId: EmpId, LeaveStat: LevStat }),
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
                                //$EmployeeLeaveReport.renderReporttitle(out);
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

    LoadManagerViewReport: function () {



        var dtClientList = $('#tblAssignleaveViewReport').DataTable({
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

                 var c = $('<input type="button" id="btnViewStat" value="View Status" data-target="#ViewStatusPopup" data-toggle="modal"  class="btn custom-button marginbt7"  />');
                 c.button();
                 c.on('click', function () {
                     $LeaveReport.LoadPendingStatwithoutfinyearPopup(sData);


                 });

                 $(nTd).empty();
                 $(nTd).prepend(c);
             }
         }

            ],


            ajax: function (data, callback, settings) {


                var fdate = $("#txtReportFromDate").val();
                var Tdate = $("#txtReportToDate").val();
                var EmpId = $("#sltmgrviewrptEmpCode").val();
                var LevStat = $("#sltleavestatus").val();
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "LeaveRequest/GetAssignManagerIssue",

                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ FromDate: fdate, ToDate: Tdate, EmployeeId: EmpId, LeaveStat: LevStat }),
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
                                //$EmployeeLeaveReport.renderReporttitle(out);
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
    LoadEmployeeLeaveReport: function () {




        var dtClientList = $('#tblemployeeleaveViewReport').DataTable({
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

                 var c = $('<input type="button" id="btnViewStat" value="View Status" data-target="#ViewStatusPopupFull" data-toggle="modal"  class="btn custom-button marginbt7"  />');
                 c.button();
                 c.on('click', function () {

                     $EmployeeLeaveReport.LoadPendingStatwithoutfinyearPopup(sData);


                 });

                 $(nTd).empty();
                 $(nTd).prepend(c);
             }
         }

            ],


            ajax: function (data, callback, settings) {


                var fdate = $("#txtReportFromDate").val();
                var Tdate = $("#txtReportToDate").val();
                var LevStat = $("#sltleavestatus").val();
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "LeaveRequest/GetLeaveReport",

                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ FromDate: fdate, ToDate: Tdate, LeaveStat: LevStat }),
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
                                //$EmployeeLeaveReport.renderReporttitle(out);
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

    renderReporttitle: function (data) {


        jQuery("label[for='myvalue']").html(data[0].LeaveTitle)

    },
    Employeeselect: function () {


        if ($("#sltemployeecode").val() == "00000000-0000-0000-0000-000000000000") {
            $("#IdRptlevbal").addClass('nodisp');
        }
        else {
            $("#IdRptlevbal").removeClass('nodisp');
            $EmployeeLeaveReport.LoadEmployeeLeaveBalanceReport();
        }

    },

    Excelreportexport: function (data) {

        var fdate, Tdate, LevStat, Empid
        if (data == "HrReport") {
            fdate = $("#txtReportFromDate").val();
            Tdate = $("#txtReportToDate").val();
            LevStat = $("#sltleavestatus").val();
            Empid = $("#sltmgrviewrptEmpCode").val();
        }
        else if (data == "MyReport") {
            fdate = $("#txtReportFromDate").val();
            Tdate = $("#txtReportToDate").val();
            LevStat = $("#sltleavestatus").val();
            Empid = '';
        }
        else if (data == "AssignReport") {
            fdate = $("#txtReportFromDate").val();
            Tdate = $("#txtReportToDate").val();
            LevStat = $("#sltleavestatus").val();
            Empid = $("#sltmgrviewrptEmpCode").val();
        }
        $.ajax({
            type: 'POST',
            cache: false,
            url: $app.baseUrl + "LeaveRequest/ExportLeaveReport",

            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ FromDate: fdate, ToDate: Tdate, LeaveStatus: LevStat, Employeeid: Empid, ReportType: data }),
            async: false,
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var oData = new Object();
                        oData.filePath = jsonResult.result;

                        //$app.downloadSync('Download/DownloadExcel', oData);
                        $app.downloadSync('Download/DownloadPaySlip', oData);
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                }

            },
            error: function (msg) {
            }
        });

    },
    ExcelBalancereportexport: function () {
        $.ajax({
            type: 'POST',
            cache: false,
            url: $app.baseUrl + "LeaveRequest/ExportLeaveBalanceForAllEmployee",

            contentType: "application/json; charset=utf-8",
            data: null,
            async: false,
            dataType: "json",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var oData = new Object();
                        oData.filePath = jsonResult.result;
                        $app.downloadSync('Download/DownloadPaySlip', oData);
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                }

            },
            error: function (msg) {
            }
        });

    },
    LoadPendingStatwithoutfinyearPopup: function (context) {

        var dtClientList = $('#tblLeavePendingStat').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                     { "data": "MgrEmpCode" },
                     { "data": "MgrEmpName" },
                     { "data": "MgrPriority" },
                     { "data": "LevStatus" },
            ],

            "aoColumnDefs": [
                 {
                     "aTargets": [0],
                     "sClass": "word-wrap"
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


            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetPendingStatReportwithoutfinyear",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ParentId: context.Id }),
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

    LoadEmployeeLeaveBalanceReport: function () {



        var dtClientList = $('#tblHRbalReport').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "LeaveTitle" },
                    { "data": "TotalDays" },
                    { "data": "UsedDays" },
                    { "data": "AvailableDays" },


            ],
            "aoColumnDefs": [


         {
             "aTargets": [0, 1, 2, 3],
             "sClass": "word-wrap"

         },

            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "Leave/GetLeaveBalanceReport",

                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Empid: $("#sltemployeecode").val() }),

                    async: false,
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        //console.log(jsonresult.result);
                        switch (jsonResult.Status) {
                            case true:

                                var out = jsonResult.result;
                                var namemessage = jsonResult.Message;
                                setTimeout(function () {
                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length,

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
                var r = $('#tblEmpLevDetails tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblEmpLevDetails thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });


    },

    LoadLeaveRelated: function (context) {
        debugger;
    switch (context.id) {
        case "FullLeaveReport":
         
            break;
        case "PendingReport":
            $('#status').val("0");
            $LeaveReport.PendingLoadData("tblPendingLeaveReport");
            break;
        case "ApprovedReport":
            $('#status').val("1");
            $LeaveReport.PendingLoadData("tblApprovedLeaveReport");
            break;
        case "RejectedReport":
            $('#status').val("2");
            $LeaveReport.PendingLoadData("tblRejectedLeaveReport");
            break;
        case "CancelledReport":
            $('#status').val("3");
            $LeaveReport.LoadData("tblCancelledLeaveReport");
            break;
        default:
            $LeaveReport.PendingLoadData();
            break;
        }
  
},
}