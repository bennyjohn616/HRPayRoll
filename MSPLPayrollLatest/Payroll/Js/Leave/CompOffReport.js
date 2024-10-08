$('#compoffgainNavtabs a').on("click", function (e) {
    debugger;
    $("#tabcontent").removeClass('nodisp');
    $CompoffReport.TabChangeEvent(this);
});
$('#btnManagercompgainrptview').on("click", function (e) {
    debugger;
   
    if ($('#FinYears').val() == '00000000-0000-0000-0000-000000000000') {
        $("#managercomgainrpt").addClass('nodisp');
        $app.showAlert('Please Select the Financial Year', 4);
    }
    else
    {
       
            $("#managercomgainrpt").removeClass('nodisp');
            $CompoffReport.NonEmployeeRoleCompoffgainReportLoadData();
        
      
    }
});
$("#btnCompOffGainExport").on('click', function () {
    $CompoffReport.Excelreportexport($("#ReportType").val());
});
$("#btnCompOffGainView").on('click', function () {
    $CompoffReport.EmployeeCompOffHistroy($("#ReportType").val());

});
$('#managerCompoffgainReptExport').on("click", function (e) {
    debugger;

    if ($('#FinYears').val() == '00000000-0000-0000-0000-000000000000') {
        $("#managercomgainrpt").addClass('nodisp');
        $app.showAlert('Please Select the Financial Year', 4);
    }
    else {

        $("#managercomgainrpt").addClass('nodisp');
        var type = "";
        var finyear = $('#FinYears').val();
        if ($('#ddlcomogggailstatus').val() == "Compgainpending")
        {
            type = "0";
        }
        else if ($('#ddlcomogggailstatus').val() == "Compgainapproved")
        {
            type = "1";
        }
        else if ($('#ddlcomogggailstatus').val() == "Compgainrejected")
        {
            type = "2";
        }
        else if ($('#ddlcomogggailstatus').val() == "Compgaincancelled") {
            type = "3";
        }
        else
        {
            type = "99";
        }
        $LeaveReport.HRmanagerExcelExport("Manager", "Gainreport", $('#hdnEmpId').val(), finyear, type, $('#ddlmanagercompoffempcode').val());
    }
});
$("#ddlmanagercompoffempcode").change(function () {
    debugger;
    if ($('#ddlmanagercompoffempcode').val() == '00000000-0000-0000-0000-000000000000' || $('#ddlmanagercompoffempcode').val() == '11111111-1111-1111-1111-111111111111' || $('#ddlmanagercompoffempcode').val() == '22222222-2222-2222-2222-222222222222')
        {
        $("#managerCompoffgainReptExport").removeClass("nodisp");
        $("#btnManagercompgainrptview").addClass("nodisp");
    }
    else {
        $("#managerCompoffgainReptExport").addClass("nodisp");
        $("#btnManagercompgainrptview").removeClass("nodisp");
    }
});

var $CompoffReport = {
    NonEmployeeRoleCompoffgainReportLoadData: function () {
        debugger;
        var dtClientList = $('#tblManagercompoffgainlReport').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                     { "data": "Id" },
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
                             var dateE = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateE;

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

                     { "data": "Reason" },

                     { "data": null }

            ],

            "aoColumnDefs": [
                 {
                     "aTargets": [0],
                     "sClass": "nodisp"
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
                        "sClass": ""
                    },
                    {
                        "aTargets": [5],
                        "sClass": "word-wrap"
                    },
                    {
                        "aTargets": [6],
                        "sClass": "word-wrap"
                    },
                    {
                        "aTargets": [7],
                        "sClass": "word-wrap"
                    },
                    {
                        "aTargets": [8],
                        "sClass": "word-wrap"
                    },

           {
               "aTargets": [9],
               "sClass": "actionColumn",

               "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                   var b = $('<a href="#" id="btnViewStat"  data-target="#ViewStatusPopup" data-toggle="modal" style="padding-right:50%" class="editeButton" title="View Status"><span aria-hidden="true" class="glyphicon glyphicon-info-sign"></span></button>');
                   b.button();
                   b.on('click', function () {
                       $LeaveReport.LoadApprovecancelStatusGrid(sData);

                   });
                   $(nTd).empty();
                   $(nTd).prepend(b);
               }
           }],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/NonemployeeroleGetCompoffGainReport",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: $('#ddlmanagercompoffempcode').val(), type: $('#ddlcomogggailstatus').val(), Finyr: $('#FinYears').val() }),
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
     CompoffgainReportLoadData: function () {

         var dtClientList = $('#tblcompoffgainlReport').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                     { "data": "Id" },
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
                             var dateE = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateE;

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

                     { "data": "Reason" },

                     { "data": null }

            ],

            "aoColumnDefs": [
                 {
                     "aTargets": [0],
                     "sClass": "nodisp"
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
                        "sClass": ""
                    },
                    {
                        "aTargets": [4],
                        "sClass": "word-wrap"
                    },
                    {
                        "aTargets": [5],
                        "sClass": "word-wrap"
                    },
                    {
                        "aTargets": [6],
                        "sClass": "word-wrap"
                    },
                    {
                        "aTargets": [7],
                        "sClass": "word-wrap"
                    },

           {
               "aTargets": [8],
               "sClass": "actionColumn",

               "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                   var b = $('<a href="#" id="btnViewStat"  data-target="#ViewStatusPopup" data-toggle="modal" style="padding-right:50%" class="editeButton" title="View Status"><span aria-hidden="true" class="glyphicon glyphicon-info-sign"></span></button>');
                   b.button();
                   b.on('click', function () {
                       $LeaveReport.LoadApprovecancelStatusGrid(sData);

                   });
                   $(nTd).empty();
                   $(nTd).prepend(b);
               }
           }],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetCompoffGainReport",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: $('#hdnEmpId').val(), type: $("#lblhiddenNavtabvalue").text() }),
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

  
     TabChangeEvent: function (context) {

         switch (context.id) {
             case "Compgainpending":
                 $("#lblhiddenNavtabvalue").text("Compgainpending");
                 $("#lblHederCompgainText").text("Compoff Gain Pending List");
                 $('#status').val('4');
                 $CompoffReport.CompoffgainReportLoadData();
                 break;
             case "Compgainapproved":
                 $("#lblhiddenNavtabvalue").text("Compgainapproved");
                 $("#lblHederCompgainText").text("Compoff Gain Approved List");
                 $CompoffReport.CompoffgainReportLoadData();
                 $('#status').val('5');
                 break;
             case "Compgainrejected":
                 $("#lblhiddenNavtabvalue").text("Compgainrejected");
                 $("#lblHederCompgainText").text("Compoff Gain Rejected List");
                 $CompoffReport.CompoffgainReportLoadData();
                 $('#status').val('2');
                 break;
             default:
                 break;
         }
     },
    loadFinYrs: function () {
    debugger;
    $.ajax({
        type: 'POST',
        url: $app.baseUrl + "Leave/GetFinanceYears",
        contentType: "application/json; charset=utf-8",
        data: null,
        dataType: "json",
        async: false,
        success: function (msg) {
            debugger;
            var out = msg.result;
            console.log(out);
            $('#FinYears').html('');
            $('#FinYears').append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
            $.each(out, function (index, finyr) {
                $('#FinYears').append($("<option></option>").val(finyr.id).html(finyr.startDate + ' TO ' + finyr.EndDate));
            });
        },
        error: function (msg) {
        }
    });
    },

    EmployeeCompOffHistroy: function (Type) {
        var employeeId = '';var finyrId='';
        if (Type=="Employee") {
            employeeId = $('#hdnEmpId').val();
            FinYrId = '00000000-0000-0000-0000-000000000000';
        }
        else {
            employeeId = $("#ddlmanagercompoffempcode").val();
            FinYrId = $("#FinYears").val();
        }
        var dtClientList = $('#tblCompOffTrackingReport').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                     { "data": "Id" }, { "data": "EmployeeCode" }, { "data": "EmployeeName" },
                     { "data": "WorkedDate", },
                     {
                         "data": "ValidDate",

                     }, { "data": "CreditDays" },
                     {
                         "data": "LeaveDate",
                         //render: function (data) {
                         //    debugger;
                         //    if (data == null) {
                         //        return "Not Used";
                         //    }

                         //}
                     },
                     { "data": "UsedLeave" }, { "data": "AvaliableDays" }, { "data": "Status" }
            ],
            "aoColumnDefs": [
                 { "aTargets": [0], "sClass": "nodisp" },
                 { "aTargets": [1], "sClass": "word-wrap" },
                 { "aTargets": [2], "sClass": "word-wrap" },
                 { "aTargets": [3], "sClass": "word-wrap" },
                 { "aTargets": [4], "sClass": "word-wrap" },
                 { "aTargets": [5], "sClass": "word-wrap" },
                 { "aTargets": [6], "sClass": "word-wrap" },
                 { "aTargets": [7], "sClass": "word-wrap" },
                 { "aTargets": [8], "sClass": "word-wrap" },
                 { "aTargets": [9], "sClass": "nodisp" },
                // {"aTargets": [10],"sClass": "actionColumn"}

            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetCompOffLeaveTracking",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: employeeId, FinYrId: FinYrId, Type: Type }),
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
                        var out = JSON.parse(jsonResult);
                        $app.clearSession(jsonResult);
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length

                            });

                        }, 50);

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
 
    Excelreportexport: function (Type) {
        debugger;
        var employeeId = ''; var finyrId = '';
        if (Type == "Employee") {
            employeeId = $('#hdnEmpId').val();
            FinYrId = '00000000-0000-0000-0000-000000000000';
        }
        else {
            employeeId = $("#ddlmanagercompoffempcode").val();
            FinYrId = $("#FinYears").val();
        }
        $.ajax({
            type: 'POST',
            cache: false,
            url: $app.baseUrl + "Leave/DownloadCompOffGainHistoryReport",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EmployeeId: employeeId, FinYrId: FinYrId, Type: Type }),
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
}