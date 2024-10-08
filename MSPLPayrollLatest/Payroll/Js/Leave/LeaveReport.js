


$("#btnReportExport").on('click', function () {

    if ($("#txtReportFromDate").val() == "" || $("#txtReportToDate").val() == "") {
        $app.showAlert('Please Select the Dates!', 4);
    }
    else {
        $LeaveReport.Excelreportexport();
    }

});


$("#idcrdemployeeCode").change(function () {
    debugger;
    if ($('#idcrdemployeeCode').val() != "00000000-0000-0000-0000-000000000000")
    {
        if($('#idcrdemployeeCode').val() != "11111111-1111-1111-1111-111111111111")
        {
            if($('#idcrdemployeeCode').val() != "22222222-2222-2222-2222-222222222222")
        {
            $("#idcrdmanager").removeClass("nodisp");
            $LeaveReport.LoadCreditLeave($('#idcrdemployeeCode').val(), "tblRptCreditmanager");
            }
            else {
                $("#idcrdmanager").addClass("nodisp")
            }
        }
        else {
            $("#idcrdmanager").addClass("nodisp")
        }
    }
    else
    {
        $("#idcrdmanager").addClass("nodisp")
    }
});
$("#idHrcrdemployeeCode").change(function () {

    if ($('#idHrcrdemployeeCode').find('option:selected').text() != "--Select--") {
        $("#idcrdHr").removeClass("nodisp");
        $LeaveReport.LoadCreditLeave($('#idHrcrdemployeeCode').val(), "tblRptCredithr");
    } else {
        $("#idcrdHr").addClass("nodisp")
    }
});


$("#iddebemployeeCode").change(function () {
    if ($('#iddebemployeeCode').val() != "00000000-0000-0000-0000-000000000000") {
        if ($('#iddebemployeeCode').val() != "11111111-1111-1111-1111-111111111111") {
            if ($('#iddebemployeeCode').val() != "22222222-2222-2222-2222-222222222222") {
                $("#iddebmanager").removeClass("nodisp");
                $LeaveReport.LoadCreditLeave($('#iddebemployeeCode').val(), "tblRptDebitmanager");
            }
            else {
                $("#iddebmanager").addClass("nodisp")
            }
        }
        else {
            $("#iddebmanager").addClass("nodisp")
        }
    }
    else
    {
        $("#iddebmanager").addClass("nodisp")
    }
});

$("#idHrdebemployeeCode").change(function () {

    if ($('#idHrdebemployeeCode').find('option:selected').text() != "--Select--") {
        $("#iddebHr").removeClass("nodisp");
        $LeaveReport.LoadDebitLeave($('#idHrdebemployeeCode').val(), "tblRptDebithr");
    } else {
        $("#iddebHr").addClass("nodisp")
    }
});
$('#ApCaLeNavtabs a').on("click", function (e) {
    debugger;
    $("#tabcontent").removeClass('nodisp');
    $LeaveReport.LoadApCaLeRelatedReport(this);
});

var $LeaveReport = {
    LoadData: function (tblName) {

        var dtClientList = $('#'+tblName+'').DataTable({
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

            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetEmpLeaveReport",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: $('#hdnEmpId').val(), status: $('#status').val() }),
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
    LoadDebitLeave: function (EMPid, Tblname) {
         debugger;
        $payroll.initDatetime();

        var dtClientList = $('#' + Tblname).DataTable({
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
                    data: JSON.stringify({ EMPID: EMPid }),
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

     LoadCreditLeave: function (Empid,Tblname) {
         debugger;
         $payroll.initDatetime();

         var dtClientList = $('#' + Tblname).DataTable({
             'iDisplayLength': 10,
             'bPaginate': true,
             'sPaginationType': 'full',
             'sDom': '<"top">rt<"bottom"ip><"clear">',
             columns: [
              { "data": "id" },
              { "data": "EmployeeCode" },
              { "data": "LeaveDate" },
              { "data": "LeaveType" },
              { "data": "Credit" },
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
                     url: $app.baseUrl + "Leave/GetCreditLeave",
                     contentType: "application/json; charset=utf-8",
                     data: JSON.stringify({ EMPID: Empid }),
                     dataType: "json",
                     success: function (jsonResult) {

                         var out = jsonResult.result;
                         $app.clearSession(jsonResult);
                         switch (jsonResult.Status) {
                             case true:
                                 debugger;
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
     ExcelExport: function (person,type,userid,emptype) {
         $.ajax({
             type: 'POST',
             cache: false,
             url: $app.baseUrl + "Leave/ExportDebitcreditReport",

             contentType: "application/json; charset=utf-8",
             data: JSON.stringify({ Person: person, Type: type, UserId: userid }),
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
     HRmanagerExcelExport: function (person, type, userid,Finyear,screen,emptype) {
         $.ajax({
             type: 'POST',
             cache: false,
             url: $app.baseUrl + "Leave/HrmangerExportReport",

             contentType: "application/json; charset=utf-8",
             data: JSON.stringify({ Person: person, Type: type, UserId: userid, Finyr: Finyear, Report: screen,Emptype:emptype }),
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
     DownloadGrid: function (Gridname){
        debugger;
        var tab_text = "<table border='2px'><tr>";
        var textRange; var j = 0;
        tab = document.getElementById(Gridname); // id of table

        for (j = 0 ; j < tab.rows.length ; j++) {
            tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
            //tab_text=tab_text+"</tr>";
        }

        tab_text = tab_text + "</table>";
        tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
        tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
        tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
        {
            txtArea1.document.open("txt/html", "replace");
            txtArea1.document.write(tab_text)
            txtArea1.document.close();
            txtArea1.focus();
            sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
        }
        else                 //other browser not tested on IE 11
            sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

        return (sa);
    },


    PendingLoadData: function (tblName) {
        debugger;
        var dtClientList = $('#' + tblName).DataTable({
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
                       debugger;
                       $LeaveReport.LoadPendingStatEmployee(sData);

                   });
                   if ($('#hidnrepId').text() == 'Appleavestatusreport')
                   {
                       var c = $('<a href="#" id="btnViewStat"   data-target="#AddAppcancelReason" data-toggle="modal"  class="deleteButton" title="Cancel Approved Leave"><span aria-hidden="true" class="glyphicon glyphicon-edit"></span></button>');
                       c.button();
                       c.on('click', function () {
                           debugger;
                           $LeaveReport.intializereason(oData);
                       });
                   }
                  

                  
                   
                   $(nTd).empty();
                   if ($('#hidnrepId').text() == 'Appleavestatusreport')
                   {
                       $(nTd).prepend(b, c);
                   }
                   else
                   {
                       $(nTd).prepend(b);
                   }
                  
               }
           }],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetEmpLeaveReport",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: $('#hdnEmpId').val(), status: $('#status').val() }),
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
   
    ApprovedCancelReportLoadData: function () {

        var dtClientList = $('#tblLeaveApprovedcancelReport').DataTable({
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
                   //var c = $('<a href="#" id="btnViewStat"   data-target="#AddAppcancelReason" data-toggle="modal"  class="deleteButton" title="Cancel Approved Leave"><span aria-hidden="true" class="glyphicon glyphicon-edit"></span></button>');

                   b.button();
                   b.on('click', function () {
                       $LeaveReport.LoadApprovecancelStatusGrid(sData);

                   });
                   //c.button();
                   //c.on('click', function () {
                   //    debugger;
                   //    $LeaveReport.intializereason(oData);
                   //});
                   $(nTd).empty();
                   $(nTd).prepend(b);
               }
           }],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetAppcanlevrepdata",
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
    Approvedcancelreqsave: function () {
        debugger;
        $.ajax({
            type: 'POST',
            cache: false,
            url: $app.baseUrl + "Leaverequest/SaveApprovedLeaveCancelRequest",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ LeaveRequestId: $("#txtHiddenid").val(), CancelReason: $("#txtRejreason").val() }),
            async: false,
            dataType: "json",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var oData = new Object();
                        oData.filePath = jsonResult.result;
                        $app.hideProgressModel();
                        $LeaveReport.PendingLoadData();
                        $app.showAlert(jsonResult.Message, 2);
                        $('#AddAppcancelReason').modal('toggle');
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        $app.hideProgressModel();
                        $('#AddAppcancelReason').modal('toggle');
                }

            },
            error: function (msg) {
            }
        });

    },
    intializereason: function (datavalue) {
        debugger;
        $LeaveReport.designForm('divApprovedCancelReason');
        var formData = document.forms["frmAppcancel"];
        formData.elements["txtRejreason"].value = "";
        formData.elements["txtHiddenid"].value = datavalue.Id;
    },
    designForm: function (renderDiv) {
        debugger;
        var formHr = '<div id="AddAppcancelReason" class="modal fade" role="dialog" aria-hidden="true" data-backdrop="static"data-keyboard="true">';
        formHr = formHr + '<form id="frmAppcancel" data-toggle="validator" role="form" >';
        formHr = formHr + '<div class="modal-dialog">';
        formHr = formHr + '<div class="modal-content">';
        formHr = formHr + '<div class="modal-header">';
        formHr = formHr + '<h4 class="modal-title" id="H4">';
        formHr = formHr + 'Reason for Approved leave cancellation </h4>';
        formHr = formHr + '</div>';
        formHr = formHr + ' <div class="col-sm-12">';
        formHr = formHr + ' <div class="form-group">';
        formHr = formHr + '</br>';
        formHr = formHr + '<label class="control-label col-md-4">';
        formHr = formHr + 'Reason <label style="color:red;font-size: 13px">*</label>  </label>';
        formHr = formHr + '<div class="col-md-7">';
        formHr = formHr + '<input type="text" id="txtRejreason" class="form-control " placeholder="Enter the Reason"/>'
        formHr = formHr + '<input type="text" id="txtHiddenid" class="form-control nodisp" placeholder="Enter the Reason"/>'
        formHr = formHr + '</div>';
        formHr = formHr + '</div>';
        formHr = formHr + '</div>';
        formHr = formHr + '<div class="modal-footer">';
        formHr = formHr + '<label hidden class="control-label col-md-4" id="hddnlbl" >';
        formHr = formHr + '</label>';
        formHr = formHr + '<button type="button" id="btnAppcancelreasonSave" class="btn custom-button" onclick="$LeaveReport.Approvedcancelreqsave()">Save</button>';
        formHr = formHr + '<button type="button" class="btn custom-button" data-dismiss="modal" >Close </button> </div>';
        formHr = formHr + '</div> ';
        formHr = formHr + '</div>';
        formHr = formHr + '</div>';
        formHr = formHr + '</form>';
        formHr = formHr + '</div>';

        $('#' + renderDiv).html(formHr);//transHtml
        //$leave.formData = document.forms["frmRejectReason"];
    },
   
    Excelreportexport: function () {


        $.ajax({
            type: 'POST',
            cache: false,
            url: $app.baseUrl + "Leave/DownloadLeaveReport",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EmployeeId: $('#hdnEmpId').val(), status: $('#status').val() }),
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
    loadEmployeeUnderassignmanager: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/GetEmployeeUnderAssignManager",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('All Employee'));
                $('#' + dropControl.id).append($("<option></option>").val('11111111-1111-1111-1111-111111111111').html('Active Employee'));
                $('#' + dropControl.id).append($("<option></option>").val('22222222-2222-2222-2222-222222222222').html('InActive Employee'));
                $.each(msg.result, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.EmployeeCode));
                });
            },
            error: function (msg) {
            }
        });
    },
    loadEmployeeUnderassignmanagerforlevbalrept: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/GetEmployeeUnderAssignManager",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.EmployeeCode));
                });
            },
            error: function (msg) {
            }
        });
    },
    LoadPendingStatPopup: function (context) {
        debugger;
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
                    url: $app.baseUrl + "LeaveRequest/GetPendingStatReport",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Id: context.ChildId }),
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
    LoadPendingStatEmployee: function (context) {
        debugger;
        var dtClientList = $('#tbllevstatusDetail').DataTable({
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
                    url: $app.baseUrl + "LeaveRequest/GetPendingStatReport",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Id: context.ChildId }),
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
    LoadApprovecancelStatusGrid: function (context) {

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
                    url: $app.baseUrl + "LeaveRequest/GetPendingStatReport",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ Id: context.Id }),
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
    LoadApCaLeRelatedReport: function (context) {

    switch (context.id) {
        case "ApCaLePd":
            $("#lblhiddenNavtabvalue").text("ApCaLePd");
            $("#lblHederAppCanText").text("Approved Cancel Pending List");
            $('#status').val('4');
            $LeaveReport.ApprovedCancelReportLoadData();
            break;
        case "ApCaLeAc":
            $("#lblhiddenNavtabvalue").text("ApCaLeAc");
            $("#lblHederAppCanText").text("Approved Cancel Accepted List");
            $LeaveReport.ApprovedCancelReportLoadData();
            $('#status').val('5');
            break;
        case "ApCaLeRj":
            $("#lblhiddenNavtabvalue").text("ApCaLeRj");
            $("#lblHederAppCanText").text("Approved Cancel Rejected List");
            $LeaveReport.ApprovedCancelReportLoadData();
            $('#status').val('2');
            break;
        default:
            break;
    }
}
}