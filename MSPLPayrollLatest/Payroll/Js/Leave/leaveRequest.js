﻿$("#sltEmpCode").change(function () {
    debugger
    //LoadFields($("#sltEmpCode").val(), $('#sltEmpCode').find('option:selected').text());
    if ($('#sltEmpCode').find('option:selected').text() != "--Select--") {
        $leave.MLAssMgrDetails = [];
        $("#idassigntable").removeClass("nodisp");
        $leave.LoadAssignmanager();

        $leave.CheckforAssignedmanagereditordelete();

    }
    else {
        $leave.MLAssMgrDetails = [];
        $("#idassigntable").addClass("nodisp")

    }

});
$("#sltManager").change(function () {
    debugger
    var status = true;
    if ($('#sltManager').find('option:selected').text() != "--Select--") {
        for (i = 0; i < $leave.MLAssMgrDetails.length; i++) {
            var selectAssEmpid = $("#sltManager").val();
            if ($leave.MLAssMgrDetails[i].AssEmpId == selectAssEmpid) {
                status = false
            }
        }
        if (status == true) {
            $leave.managernameshow();
        }
        else {
            $app.showAlert("This Employee is already assigned", 4);
            var formData = document.forms["frmAssMgrPopup"];
            formData.elements["sltManager"].value = "00000000-0000-0000-0000-000000000000";
            formData.elements["txtMgrName"].value = "";
        }

    }
    else {


    }


});

$("#txtPrioNumb").change(function () {
    debugger
    var status = true;

    for (i = 0; i < $leave.MLAssMgrDetails.length; i++) {
        var PriorityNumb = $("#txtPrioNumb").val();
        if ($leave.MLAssMgrDetails[i].MgrPriority == PriorityNumb) {
            status = false
        }
    }
    if (status == true) {
        //$leave.managernameshow();
    }
    else {
        $app.showAlert("This Priority Number is already Exist", 4);
        var formData = document.forms["frmAssMgrPopup"];
        formData.elements["txtPrioNumb"].value = "";

    }





});

$("#btnMLAssMgrSave").on('click', function () {
    debugger
    var savestatus = 0;
    var managercode = $("#sltManager").val();
    var appstatus = $("#sltAppStat").val();
    var prioritynum = $("#txtPrioNumb").val();
    var appcancelrights = $("#sltAppCanRigh").val();
    if (managercode == "00000000-0000-0000-0000-000000000000") {
        $app.showAlert('Please Select Manager Id', 4);
        savestatus = 1;
    }
    if (appstatus == "0") {
        $app.showAlert('Please Select Approval status', 4);
        savestatus = 1;

    }

    if (prioritynum == "") {
        $app.showAlert('Please Enter Priority number ', 4);
        savestatus = 1;

    }
    if (prioritynum == "0") {
        $app.showAlert('Priority number cannot be Zero', 4);
        savestatus = 1;

    }
    if (appcancelrights == "0") {
        $app.showAlert('Please Select Approval cancel rights ', 4);
        savestatus = 1;

    }
    if (savestatus == "0") {
        $leave.saveAssignmanager();
    }
});



$('#btnsavecompoffrequest').on('click', function () {
    debugger;
    var err = 0;

    var HRorUser = $("#pgeTitle").text();
    var datefrom = new Date($("#txtFromDate").val());
    var dateto = new Date($("#txtToDate").val());
    var datefrom1 = Date.parse($("#txtFromDate").val());
    var dateto1 = Date.parse($("#txtToDate").val());
    var fromday = $("#FsltDay").val();
    var today = $("#TsltDay").val();
    $(".Reqrd").each(function () {

        var id = this.id;
        if (this.id == "sltLevType") {
            if ($('#' + this.id).val() == "00000000-0000-0000-0000-000000000000") {
                $app.showAlert('Please Select Leave  Type', 4);
                err = 1;
                return false;
            }
        }
        else if (document.getElementById(this.id).value == "" && this.id != 'txtContact') {
            $app.showAlert('Please Enter ' + $(this).attr('placeholder'), 4);
            err = 1;
            return false;
        }
        if ($("#sltLevType").val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert('Please Select Leave  Type', 4);
            err = 1;
            return false;
        }
        if (datefrom1 != dateto1) {
            if (today == 2) {
                $app.showAlert("Please Select FULLDAY in the Todate instead of SECOND-HALF!!!", 4);
                $("#TsltDay").focus();
                err = 1;
                return false;
            }


            if (fromday == 1) {
                $app.showAlert("Please Select FULLDAY in the Fromdate instead of FIRST-HALF!!!", 4);
                $("#FsltDay").focus();
                err = 1;
                return false;
            }

        }

        if (datefrom1 == dateto1) {
            if (today != fromday) {
                $app.showAlert("Single Date cannot have different day type", 4);
                $("#FsltDay").focus();
                err = 1;
                return false;
            }
        }

    });
    if (err == 0) {
        $leave.saveCompoffRequest();
    }
});

$('#btnsaveleaverequest').on('click', function () {
    debugger;
    var err = 0;

    var HRorUser = $("#pgeTitle").text();
    var datefrom = new Date($("#txtFromDate").val());
    var dateto = new Date($("#txtToDate").val());
    var datefrom1 = Date.parse($("#txtFromDate").val());
    var dateto1 = Date.parse($("#txtToDate").val());
    var fromday = $("#FsltDay").val();
    var today = $("#TsltDay").val();
    $(".Reqrd").each(function () {

        var id = this.id;
        if (this.id == "sltLevType") {
            if ($('#' + this.id).val() == "00000000-0000-0000-0000-000000000000") {
                $app.showAlert('Please Select Leave  Type', 4);
                err = 1;
                return false;
            }
        }
        else if (document.getElementById(this.id).value == "" && this.id != 'txtContact') {
            $app.showAlert('Please Enter ' + $(this).attr('placeholder'), 4);
            err = 1;
            return false;
        }
        if ($("#sltLevType").val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert('Please Select Leave  Type', 4);
            err = 1;
            return false;
        }
        if (datefrom1 != dateto1) {
            if (today == 2) {
                $app.showAlert("Please Select FULLDAY in the Todate instead of SECOND-HALF!!!", 4);
                $("#TsltDay").focus();
                err = 1;
                return false;
            }


            if (fromday == 1) {
                $app.showAlert("Please Select FULLDAY in the Fromdate instead of FIRST-HALF!!!", 4);
                $("#FsltDay").focus();
                err = 1;
                return false;
            }

        }

        if (datefrom1 == dateto1) {
            if (today != fromday) {
                $app.showAlert("Single Date cannot have different day type", 4);
                $("#FsltDay").focus();
                err = 1;
                return false;
            }
        }
    });
    if (err == 0) {
        $leave.saveRequest(data = 'CONFORMSAVE');
    }
});

$('#btnaddleaverequest').on('click', function () {
    debugger;
    var err = 0;

    var datefrom = new Date($("#txtFromDate").val());
    var dateto = new Date($("#txtToDate").val());
    var datefrom1 = Date.parse($("#txtFromDate").val());
    var dateto1 = Date.parse($("#txtToDate").val());
    var fromday = $("#FsltDay").val();
    var today = $("#TsltDay").val();
    $(".Reqrd").each(function () {

        if (this.id == "sltLevType") {
            if ($('#' + this.id).val() == "00000000-0000-0000-0000-000000000000") {
                $app.showAlert('Please Select Leave  Type', 4);
                err = 1;
                return false;
            }
        }
        else if (document.getElementById(this.id).value == "") {
            $app.showAlert('Please Enter ' + $(this).attr('placeholder'), 4);
            err = 1;
            return false;
        }
        if ($("#sltLevType").val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert('Please Select Leave  Type', 4);
            err = 1;
            return false;
        }
        ////if ($("#sltLevType").val() != "199F5DB2-14B7-46D3-A0E4-715D56682277") {
        //if ($("#sltLevType").text() != "COMPOFF") {
        //    if ($('#lblBalance').text().trim() < ((Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1)) {
        //        $app.showAlert("You don't have enough leave!!!", 4);
        //        err = 1;
        //        return false;
        //    }
        //}
        if (datefrom1 != dateto1) {
            if (today == 2) {
                $app.showAlert("Please Select FULLDAY in the Todate instead of SECOND-HALF!!!", 4);
                $("#TsltDay").focus();
                err = 1;
                return false;
            }


            if (fromday == 1) {
                $app.showAlert("Please Select FULLDAY in the Fromdate instead of FIRST-HALF!!!", 4);
                $("#FsltDay").focus();
                err = 1;
                return false;
            }

        }

        if (datefrom1 == dateto1) {
            if (today != fromday) {
                $app.showAlert("Single Date cannot have different day type", 4);
                $("#FsltDay").focus();
                err = 1;
                return false;
            }
        }



    });
    if (err == 0) {
        $leave.saveRequest(data = 'ADD');
    }
});




$("#txtToDate,#txtFromDate").change(function () {
    var datefrom = new Date($("#txtFromDate").val());
    var dateto = new Date($("#txtToDate").val());
    if ($("#txtFromDate").val() != '' && $("#txtToDate").val() != '') {
        if (dateto >= datefrom) {

        }
        else {

            $app.showAlert('End Date should not be less than Start Date !', 3);
            $("#txtToDate").val('');
        }
    }
});

//$("#txtToDate,#txtFromDate,#TsltDay,#FsltDay").change(function () {
//    
//    if ($("#FsltDay").val() == 2 || $("#TsltDay").val() == 2) {
//        var datefrom = new Date($("#txtFromDate").val());
//        var dateto = new Date($("#txtToDate").val());
//        if ($("#txtFromDate").val() != $("#txtToDate").val())
//        {
//            alert('Unable to chose');
//            $("#TsltDay").val('0');
//        } else if($("#txtFromDate").val() == $("#txtToDate").val()) {
//            if($("#FsltDay").val() != 2)
//            {
//                alert('Unable to chose');
//                $("#TsltDay").val('0');
//            }
//        }
//    }
//});


$("#txtFromDate,#txtDateJoin").change(function () {
    var datefrom = new Date($("#txtDateJoin").val());
    var dateto = new Date($("#txtFromDate").val());
    if ($("#txtDateJoin").val() != '' && $("#txtFromDate").val() != '') {
        if (dateto >= datefrom) {

        }
        else {

            $app.showAlert('From Date should not be less than Date of Joining !', 3);
            $("#txtFromDate").val('');
        }
    }
});


$("#sltLevType").change(function () {
    debugger
    var levvaluetext = $('#sltLevType').find('option:selected').text();
    var levtypvalue = $("#sltLevType").val();
    // if (levtypvalue != "199F5DB2-14B7-46D3-A0E4-715D56682277")
    if (levvaluetext.trim() != "LOSS OF PAY DAYS" && levvaluetext.trim() != "ONDUTY" && levvaluetext.trim() != "WORK FROM HOME" && levvaluetext.trim() != "--Select--")//199f5db2-14b7-46d3-a0e4-715d56682277
    {
        $("#lblBalance").removeClass('nodisp');
        $leave.GetLeavebalance();

    }
    else {

        $("#lblBalance").addClass('nodisp');
    }
    if (levvaluetext.trim() == "COMPOFF") {
        $leave.GetCompOffgainHistory();
        $('#CompOffGainHistory').modal('toggle');

    }
    else {

    }

});
$('#RequestNavtabs a').on("click", function (e) {
    $("#tabcontent").removeClass('nodisp');
    $leave.LoadLeaveRelated(this);
});

$('#lveNavtabs a').on("click", function (e) {
    $("#tabcontent").removeClass('nodisp');
    $('#txtEmpCode').val('');
    $('#txtEmpName').val('');
    $('#txtDateJoin').val('');
    $('#txtLevel').val('');
    $('#sltLevType').val('00000000-0000-0000-0000-000000000000');
    $leave.clearLeaveEntry();
    $leave.LoadLeaveRelated(this);
});

$leave = {
    enteredBy: null,
    // EmpId: 'A9081EB2-5EE6-4108-BF95-1FE448C33313',
    Id: null,
    EmployeeDATAid: null,
    leaveDATAid: null,
    LeaveTypeDATAid: null,
    FromDateDATA: null,
    TodateDATA: null,
    ReasonDATA: null,
    ContactDATA: null,
    noofdaysDATA: null,
    HalfFullDayDATA: null,
    FromDayDATA: null,
    DataCancel: null,
    ToDayDATA: null,
    AssignMgrId: null,
    rejtable: 'tblreject',
    MLAssMgrDetails: [],
    CompOff: false,
    formData: document.forms["frmRejectReason"],
    selectedCompOffGainId: [],
    CompOffleaveCount:0,
    designForm: function (renderDiv) {

        var formHr = '<div id="AddReason" class="modal fade" role="dialog" aria-hidden="true" data-backdrop="static"data-keyboard="true">';
        formHr = formHr + '<form id="frmRejectReason" data-toggle="validator" role="form" >';
        formHr = formHr + '<div class="modal-dialog">';
        formHr = formHr + '<div class="modal-content">';
        formHr = formHr + '<div class="modal-header">';
        formHr = formHr + '<h4 class="modal-title" id="H4">';
        formHr = formHr + 'Reason For Reject/Cancel </h4>';
        formHr = formHr + '</div>';
        formHr = formHr + ' <div class="col-sm-12">';
        formHr = formHr + ' <div class="form-group">';
        formHr = formHr + '</br>';
        formHr = formHr + '<label class="control-label col-md-4">';
        formHr = formHr + 'Reason <label style="color:red;font-size: 13px">*</label>  </label>';
        formHr = formHr + '<div class="col-md-7">';
        formHr = formHr + '<input type="text" id="txtRejreason" class="form-control " placeholder="Enter the Reason"/>'
        formHr = formHr + '</div>';
        formHr = formHr + '</div>';
        formHr = formHr + '</div>';
        formHr = formHr + '<div class="modal-footer">';
        formHr = formHr + '<label hidden class="control-label col-md-4" id="hddnlbl" >';
        formHr = formHr + '</label>';
        formHr = formHr + '<button type="button" id="btnSave" class="btn custom-button" onclick="$leave.RejectCancel()">Save</button>';
        formHr = formHr + '<button type="button" class="btn custom-button" data-dismiss="modal" >Close </button> </div>';
        formHr = formHr + '</div> ';
        formHr = formHr + '</div>';
        formHr = formHr + '</div>';
        formHr = formHr + '</form>';
        formHr = formHr + '</div>';

        $('#' + renderDiv).html(formHr);//transHtml
        //$leave.formData = document.forms["frmRejectReason"];
    },
    formData: document.forms["frmHRRejectReason"],
    designFormHRREJReson: function (renderDiv) {

        var formHr1 = '<div id="AddHRReason" class="modal fade" role="dialog" aria-hidden="true" data-backdrop="static"data-keyboard="true">';
        formHr1 = formHr1 + '<form id="frmHRRejectReason" data-toggle="validator" role="form" >';
        formHr1 = formHr1 + '<div class="modal-dialog">';
        formHr1 = formHr1 + '<div class="modal-content">';
        formHr1 = formHr1 + '<div class="modal-header">';
        formHr1 = formHr1 + '<h4 class="modal-title" id="H4">';
        formHr1 = formHr1 + 'Reason For Reject</h4>';
        formHr1 = formHr1 + '</div>';
        formHr1 = formHr1 + ' <div class="col-sm-12">';
        formHr1 = formHr1 + ' <div class="form-group">';
        formHr1 = formHr1 + '</br>';
        formHr1 = formHr1 + '<label class="control-label col-md-4">';
        formHr1 = formHr1 + 'Reason <label style="color:red;font-size: 13px">*</label>  </label>';
        formHr1 = formHr1 + '<div class="col-md-7">';
        formHr1 = formHr1 + '<input type="text" id="txtHRRejreason" class="form-control " placeholder="Enter the Reason"/>'
        formHr1 = formHr1 + '</div>';
        formHr1 = formHr1 + '</div>';
        formHr1 = formHr1 + '</div>';
        formHr1 = formHr1 + '<div class="modal-footer">';
        formHr1 = formHr1 + '<label hidden class="control-label col-md-4" id="hddnlbl" >';
        formHr1 = formHr1 + '</label>';
        formHr1 = formHr1 + '<button type="button" id="btnHRSave" class="btn custom-button" onclick="$leave.HRRejectCancel()">Save</button>';
        formHr1 = formHr1 + '<button type="button" class="btn custom-button" data-dismiss="modal" >Close </button> </div>';
        formHr1 = formHr1 + '</div> ';
        formHr1 = formHr1 + '</div>';
        formHr1 = formHr1 + '</div>';
        formHr1 = formHr1 + '</form>';
        formHr1 = formHr1 + '</div>';

        $('#' + renderDiv).html(formHr1);//transHtml
        //$leave.formData = document.forms["frmRejectReason"];
    },
    LoadData: function (Reqtype) {
        debugger;

        var dtClientList = $('#tblLeaveList').DataTable({
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
                             //$("#EndDate").val(data.getDate() + '/' + $payroll.GetMonthName((data.getMonth() + 1)) + '/' + data.getFullYear());
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
                     {
                         "data": "Status",
                         render: function (data) {

                             var statustype;
                             if (data == 0) {
                                
                                 statustype = "Pending";
                             }
                             else if (data == 1) {
                                 statustype = "Approved";
                             }
                             else if (data == 2) {
                                 statustype = "Rejected";
                             }
                             else if (data == 4) {
                                 statustype = "Added";
                             }
                             else {
                                 statustype = "Cancelled";
                             }
                             return statustype;
                             //$("#EndDate").val(data.getDate() + '/' + $payroll.GetMonthName((data.getMonth() + 1)) + '/' + data.getFullYear());
                         }
                     },

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
                       "sClass": "word-wrap"
                   },
                   {
                       "aTargets": [5],
                       "sClass": "word-wrap"
                   },
                    {
                        "aTargets": [6],
                        "sClass": ""
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


                  var b = $('<a href="#" class="editeButton" title="View"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                  //var c=$('<input type="button" value="Cancel"  class="btn custom-button marginbt7" data-target="#AddReason" data-toggle="modal" />')
                  var c = $('<input type="button" id="btnCancel" value="Cancel" data-target="#AddReason" data-toggle="modal"  class="btn custom-button marginbt7"  />');
                  var d = $('<a href="#" title="Delete" ><span aria-hidden="true" class="glyphicon glyphicon-remove" ></span></button>');
                  //var c = $('<a href="#" title="Cancel" data-target="#AddReason" data-toggle="modal" ><span aria-hidden="true" class="glyphicon glyphicon-remove" ></span></button>');
                  //var d=$('<input type="button" value="Cancel"  class="btn custom-button marginbt7" data-target="#AddReason" data-toggle="modal" />')
                  b.button();
                  b.on('click', function () {

                      $leave.GetDetail(oData);

                      return false;

                  });
                  //if ({ "data": "LeaveTypeName" } == 'FLOATING HOLIDAY') { //maddy
                  //    $('#btnCancel').hide()
                  //}
                  c.button();
                  c.on('click', function () {
                      $leave.DataCancel = sData;
                      $leave.intializereason(data = 'Cancel');

                  });
                  debugger
                  if (oData.LeaveTypeName == 'FLOATING HOLIDAY') {
                      debugger
                      c.hide();
                  }
                  d.on('click', function () {

                      if (confirm('Are you sure ,do you want to delete?')) {
                          $leave.DeleteRequest(oData);
                      }

                  });
                  if (sData.Status == 0) {
                      $(nTd).empty();
                      $(nTd).prepend(c);
                  }
                  else if (sData.Status == 4) {
                      $(nTd).empty();
                      $(nTd).prepend(b, d);
                  }
                  else {
                      $(nTd).empty();
                  }
              }
          }],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetEmpLeaveRequest",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: $('#hdnEmpId').val(), Reqtype: Reqtype }),
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

    Cancel: function () {
        debugger;
        var data = $leave.DataCancel;
        EmployeeDATAid = data.EmployeeId;
        leaveDATAid = data.Id;
        LeaveTypeDATAid = data.LeaveType;
        FromDateDATA = data.FromDate;
        TodateDATA = data.EndDate;
        ReasonDATA = data.Reason;

        HalfFullDayDATA = data.HalfFullDay;
        ContactDATA = data.Contact;
        FromDayDATA = data.FromDay;
        ToDayDATA = data.ToDay;

        var date = new Date(parseInt(FromDateDATA.replace(/(^.*\()|([+-].*$)/g, '')));
        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
        var date1 = new Date(parseInt(TodateDATA.replace(/(^.*\()|([+-].*$)/g, '')));
        var dateE = date1.getDate() + '/' + $payroll.GetMonthName((date1.getMonth() + 1)) + '/' + date1.getFullYear();

        var noofdaysDATA = (Math.abs(((new Date(dateE) - new Date(dateF)) / (1000 * 60 * 60 * 24))) + 1);
        if (dateF != dateE) {
            if (FromDayDATA != 0) {
                noofdaysDATA = noofdaysDATA - 0.5;
            }
            if (ToDayDATA != 0) {
                noofdaysDATA = noofdaysDATA - 0.5;
            }
        }
        else if ($("#txtFromDate").val() == $("#txtToDate").val() && ($("#FsltDay").val() && $("#TsltDay").val()) != "0") {
            noofdaysDATA = noofdaysDATA - 0.5;
        }
        else {
            noofdaysDATA = noofdaysDATA;
        }

        var dataValue = {
            Id: leaveDATAid,

            EmployeeId: EmployeeDATAid,
            FromDate: dateF,
            EndDate: dateE,
            Reason: ReasonDATA,
            LeaveType: LeaveTypeDATAid,
            NavTabStatus: $('#lblhiddenNavtab').text(),//"leaveReq",
            LeavingStation: $('#sltLeavingStation').val(),
            Contact: ContactDATA,
            HalfFullDay: HalfFullDayDATA,
            NoOfDays: noofdaysDATA,
            EnteredBy: EmployeeDATAid,
            Status: 3,
            Rejectreason: $('#txtRejreason').val(),
            FirstLvlContact: $('#txt1stLevel').val(),
            FirstLvlContactData: $('#txt1stlvlData').val(),
            prioritynumber: $('#lblpriority').val(),
            AssignmanagerId: $('#lblASSMGR').val(),
            ApporRejorcancStat: 3
        }

        debugger;
        $leave.RejectLeaveStatus(dataValue);

        return false;

    },
    saveCompoffRequest: function () {

        $app.showProgressModel();
        var err = 0
        var dataValue = {
            Id: $leave.Id,
            EmployeeId: $('#sltEmpCode').val(),
            FromDate: $('#txtFromDate').val(),
            EndDate: $('#txtToDate').val(),
            Reason: $('#txtReason').val(),
            LeaveType: $('#sltLevType').val(),
            LeavingStation: $('#sltLeavingStation').val(),
            Contact: $('#txtContact').val(),
            HalfFullDay: $('#sltDay').val(),
            fromHFDAY: $("#FsltDay").val(),
            ToHFDAY: $("#TsltDay").val(),
            NoOfDays: (Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1,
            // NoOfDays: (daydiff(parseDate($('#txtFromDate').val()), parseDate($('#txtToDate').val()))),

            EnteredBy: $('#lblEnteredBy').val(),
            FirstLvlContact: $('#txt1stLevel').val(),
            FirstLvlContactData: $('#txt1stlvlData').val()
        }

        var fromday = $('#FsltDay').val();
        var CurrStatus;
        var HRorUserflag;
        var today = $('#TsltDay').val();
        var noofdays = (Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1;
        if ($("#txtFromDate").val() != $("#txtToDate").val()) {
            if (fromday != 0) {
                noofdays = noofdays - 0.5;
            }
            if (today != 0) {
                noofdays = noofdays - 0.5;
            }
        }
        else if ($("#txtFromDate").val() == $("#txtToDate").val() && ($("#FsltDay").val() && $("#TsltDay").val()) != "0") {
            noofdays = noofdays - 0.5;
        }
        else {
            noofdays = noofdays;
        }
        if ($("#pgeTitle").text() == "HR Leave Entry") {
            HRorUserflag = 1;
        }
        else {
            HRorUserflag = 0;
        }
        var dataValue = {

            Id: $leave.Id,
            EmployeeId: $('#sltEmpCode').val(),
            FromDate: $('#txtFromDate').val(),
            EndDate: $('#txtToDate').val(),
            Reason: $('#txtReason').val(),
            LeaveType: $('#sltLevType').val(),
            LeavingStation: $('#sltLeavingStation').val(),
            Contact: $('#txtContact').val(),
            FromDay: fromday,
            ToDay: today,
            //balanceLeave: $('#lblBalance').text().trim(),
            balanceLeave: $('#lblBalanceLevHide').text().trim(),
            NoOfDays: noofdays,
            // NoOfDays: (daydiff(parseDate($('#txtFromDate').val()), parseDate($('#txtToDate').val()))),
            EnteredBy: $('#lblEnteredBy').val(),
            FirstLvlContact: $('#txt1stLevel').val(),
            FirstLvlContactData: $('#txt1stlvlData').val(),
            fromHFDAY: $("#FsltDay").val(),
            ToHFDAY: $("#TsltDay").val(),
            Status: CurrStatus,
            HRentrystatus: HRorUserflag

        }
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/CompoffEntrySave",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dataValue: dataValue }),
            dataType: "json",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $leave.LoadData("compoff");
                        //$leave.clearData();
                        $('#txtFromDate').val('');
                        $('#txtToDate').val('');
                        $('#txtReason').val('');
                        $('#txtContact').val('');
                        $("#lblBalance").empty();
                        $("#lblBalanceLevHide").empty();
                        $("#lblBalance").append("");
                        $("#lblBalanceLevHide").append("");
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $leave.LoadData("compoff");
                        //$leave.clearData();
                        $('#txtFromDate').val('');
                        $('#txtToDate').val('');
                        $('#txtReason').val('');
                        $('#txtContact').val('');
                        $("#lblBalance").empty();
                        $("#lblBalanceLevHide").empty();
                        $("#lblBalance").append("");
                        $("#lblBalanceLevHide").append("");
                        break;
                }

            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },
    saveRequest: function (data) {
        debugger;
        $app.showProgressModel();
        var err = 0
        var dataValue = {
            Id: $leave.Id,
            EmployeeId: $('#sltEmpCode').val(),
            FromDate: $('#txtFromDate').val(),
            EndDate: $('#txtToDate').val(),
            Reason: $('#txtReason').val(),
            LeaveType: $('#sltLevType').val(),
            LeavingStation: $('#sltLeavingStation').val(),
            Contact: $('#txtContact').val(),
            HalfFullDay: $('#sltDay').val(),
            fromHFDAY: $("#FsltDay").val(),
            ToHFDAY: $("#TsltDay").val(),
            NoOfDays: (Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1,
            EnteredBy: $('#lblEnteredBy').val(),
            FirstLvlContact: $('#txt1stLevel').val(),
            FirstLvlContactData: $('#txt1stlvlData').val()
        }

        var fromday = $('#FsltDay').val();
        var CurrStatus;
        var HRorUserflag;
        var today = $('#TsltDay').val();
        var noofdays = (Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1;
        if ($("#txtFromDate").val() != $("#txtToDate").val()) {
            if (fromday != 0) {
                noofdays = noofdays - 0.5;
            }
            if (today != 0) {
                noofdays = noofdays - 0.5;
            }
        }
        else if ($("#txtFromDate").val() == $("#txtToDate").val() && ($("#FsltDay").val() && $("#TsltDay").val()) != "0") {
            noofdays = noofdays - 0.5;
        }
        else {
            noofdays = noofdays;
        }

        if (data == 'CONFORMSAVE') {
            CurrStatus = 0;
        }
        else if (data == 'ADD') {
            CurrStatus = 4;
        }
        if ($("#pgeTitle").text() == "HR Leave Entry") {
            HRorUserflag = 1;
        }
        else {
            HRorUserflag = 0;
        }
        var dataValue = {

            Id: $leave.Id,
            EmployeeId: $('#sltEmpCode').val(),
            FromDate: $('#txtFromDate').val(),
            EndDate: $('#txtToDate').val(),
            Reason: $('#txtReason').val(),
            LeaveType: $('#sltLevType').val(),
            LeavingStation: $('#sltLeavingStation').val(),
            Contact: $('#txtContact').val(),
            FromDay: fromday,
            ToDay: today,
            //balanceLeave: $('#lblBalance').text().trim(),
            balanceLeave: $('#lblBalanceLevHide').text().trim(),
            NoOfDays: noofdays,
            // NoOfDays: (daydiff(parseDate($('#txtFromDate').val()), parseDate($('#txtToDate').val()))),
            EnteredBy: $('#lblEnteredBy').val(),
            FirstLvlContact: $('#txt1stLevel').val(),
            FirstLvlContactData: $('#txt1stlvlData').val(),
            fromHFDAY: $("#FsltDay").val(),
            ToHFDAY: $("#TsltDay").val(),
            Status: CurrStatus,
            HRentrystatus: HRorUserflag,
            IsAttachReq: $("#idattach").val(),
            Tempid: $('#attachtempID').text(),
            Imgpath: $('#fullpathID').text(),
            HRorUser: $("#pgeTitle").text(),

        }

        if ($('#sltLevType').find('option:selected').text().trim() == "COMPOFF") {
            $leave.CompOffBalanceSelect();
        }
        $.ajax({

            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/LeaveEntry",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dataValue: dataValue, CompOffEntry: $leave.selectedCompOffGainId }),
            dataType: "json",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $leave.clearData();
                        $leave.LoadData();
                        //$leave.clearData();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $leave.clearData();
                        $leave.LoadData();
                        //$leave.clearData();
                        break;
                }

            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },
    CheckforAssignedmanagereditordelete: function () {


        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/CheckforAssignedmanagereditordelete",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EmployeeId: $("#sltEmpCode").val() }),
            dataType: "json",
            success: function (jsonResult) {

                var out = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:

                        document.getElementById('iddisplaystatus').innerHTML = '';
                        $("#btnAddPopup").removeClass("nodisp");
                        break;
                    case false:

                        document.getElementById('iddisplaystatus').innerHTML = 'You cannot Edit or Delete,This Employee is UnderProcess!!!';
                        $("#btnAddPopup").addClass("nodisp");
                        break;
                }

            },
            error: function (msg) {
            }
        });



    },
    //----------------------------------------------


    BTNrejbymail: function () {

        debugger
        var reason = $("#txtRejreasonbymail").val();
        if (reason == "") {
            $app.showAlert('Please Enter the Reason for Rejection', 4);
        }
        else {
            $leave.EmailupdateLeaveStatus();
        }



    },
    intializereason: function (data) {


        var formData = document.forms["frmRejectReason"];
        $('#hddnlbl').val(data);
        formData.elements["txtRejreason"].value = "";
        // }



    },

    HRintializereason: function (data) {




        var formData = document.forms["frmHRRejectReason"];
        $('#hddnlbl').val(data);
        formData.elements["txtHRRejreason"].value = "";






    },
    HRleaveentryEmpselect: function () {


        if ($('#sltEmpCode').val() != "00000000-0000-0000-0000-000000000000") {
            $leave.clearData();
            $("#hdnEmpId").val($("#sltEmpCode").val());

            $leave.getManager({ Email: 'txt1stLevel' });
            $leave.GetEmployeDetail($("#hdnEmpId").val());
            $leave.LoadData();
            $leave.designForm('divCancelReason');
            $("#idHRentrytbl").removeClass('nodisp');
        }
        else {
            $("#idHRentrytbl").addClass('nodisp');
            $leave.clearData();
            $("#txtEmpName").val('');
            $("#txt1stLevel").val('');
            $('#txt1stlvlData').val('');
            $("#txtDateJoin").val('');


        }



    },
    HRselectiontype: function (svalue) {

        $("#txtSelectiontype").val(svalue);
        $("#sltEmployeeCode").removeClass('nodisp');
        $('#sltEmployeeCode').find('option:selected').text('--Select--');
        $("#txtEmpCode").val('');
        $("#txt1stLevel").val('');
        $('#txt1stlvlData').val('');
        $("#txtDateJoin").val('');
        $("#txtLevel").val('');
        $("#txtFromDate").val('');
        $("#txtReason").val('');
        $("#txtSelectedempcode").val('');
        $("#txtEmpName").val('');
        $("#txtToDate").val('');
        $("#txtContact").val('');
        // $('#sltLevType').find('option:selected').text('--Select--');
        $companyCom.loadLeaveTypeforleaverequest({ id: 'sltLevType' });
        $('#FsltDay').find('option:selected').text('Full Day');
        $('#TsltDay').find('option:selected').text('Full Day');
        $("#idhrapprejtable").addClass("nodisp");
        sltEmployeeCode.clearData;
        $companyCom.loadEmployee({ id: 'sltEmployeeCode' });
    },

    HRselectionemployee: function () {

        $('#txtEmpCode').val('');
        $('#txtEmpName').val('');
        $('#txtDateJoin').val('');
        $('#txtLevel').val('');
        $('#sltLevType').val('00000000-0000-0000-0000-000000000000');
        $('#FsltDay').val('0');
        $('#TsltDay').val('0');

        $leave.clearLeaveEntry();


        if ($('#sltEmployeeCode').find('option:selected').text() == "--Select--") {
            $("#idhrapprejtable").addClass("nodisp");
        }
        else {
            $("#idhrapprejtable").removeClass("nodisp");
            $("#txtSelectedempcode").val($('#sltEmployeeCode').find('option:selected').text());
            $leave.HRLeaveRequestList();
        }


    },

    RejectCancel: function () {

        if ($('#txtRejreason').val() != "") {
            if ($('#hddnlbl').val() == "Reject") {
                document.getElementById("#btnSave").onclick = $leave.reject();
            }
            else if ($('#hddnlbl').val() == "Cancel") {
                document.getElementById("#btnSave").onclick = $leave.Cancel();
            }
            return false;
        }
        else {
            $app.showAlert("Please Enther the Reason", 4);
        }
    },
    HRRejectCancel: function () {

        if ($('#txtRejreason').val() != "") {
            if ($('#hddnlbl').val() == "Reject") {
                document.getElementById("#btnHRSave").onclick = $leave.HRreject();
            }
            else if ($('#hddnlbl').val() == "Cancel") {
                document.getElementById("#btnHRSave").onclick = $leave.Cancel();
            }
            return false;
        }
        else {
            $app.showAlert("Please Enther the Reason", 4);
        }
    },
    GetLeavebalance: function (data) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/GetLeaveBalance",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EmployeeId: $("#sltEmpCode").val(), LeaveType: $("#sltLevType").val() }),
            dataType: "json",
            success: function (jsonResult) {

                var out = jsonResult.result;
                if (data == null) {
                    data = 0;
                }
                var balanceleavetotal = Number(out.balanceLeave) + Number(data);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $('#lblBalance').text("       " + balanceleavetotal + "      ");
                        $('#lblBalanceLevHide').text("       " + balanceleavetotal + "      ");
                        if (out.IsAttachReq == "Y") {
                            $('#divattachment').removeClass("nodisp");
                        }
                        else {
                            $('#divattachment').addClass("nodisp");
                        }
                        if (out.LeaveType == '6ddf6b2f-c2b4-4c25-869b-834871ef3b28') { //Maddy 
                            $app.showAlert("You Cann't  Request Floating Leave", 3);
                            $('#btnsaveleaverequest').hide();
                            return;
                        }
                        $('#btnsaveleaverequest').show();
                        break;
                    case false:

                        break;
                }

            },
            error: function (msg) {
            }
        });
    },

    GetCompOffgainHistory: function (data) {

        var dtClientList = $('#compoffGainhistoryTable').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'responsive': true,
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            columns: [
                 {
                     "data": "ID",
                 },
                 {
                     "data": "ID",
                     "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                         var b = $('<input type="checkbox" class="chkcompOffBal" id="' + sData + '"/>');
                         $(nTd).html(b);
                     }
                 },
                 { "data": "CompOffDateReq", },
                 { "data": "ValidDate", },
                 { "data": "AvaliableDays" },

            ],
            "aoColumnDefs": [
                {
                    "aTargets": [0],
                    "sClass": "nodisp",
                    "bSearchable": false
                },
            {
                "aTargets": [1],
                "sClass": "compoffGainId",
                "bSearchable": false
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
                 "sClass": "word-wrap AvaliableDays"
             }

            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetBalanceCompOff",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: $("#sltEmpCode").val() }),
                    dataType: "json",
                    success: function (msg) {
                        debugger;

                        var out = JSON.parse(msg);
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

                var r = $('#compoffGainhistoryTable tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#compoffGainhistoryTable thead').append(r);

            },
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            dom: "rtiS",
            "bDestroy": true,

            scroller: {
                loadingIndicator: true
            }
        });

    },
    CompOffBalanceSelect: function () {
        debugger;
        CompOffleaveCount = 0;
        var attr = [];
        //Get Selected Earnings
        var rowsEar = $("#compoffGainhistoryTable").dataTable().fnGetNodes();
        for (var i = 0; i < rowsEar.length; i++) {

            var newattr = new Object();
            if ($(rowsEar[i]).find(".chkcompOffBal").prop("checked")) {
                newattr.CompOffGainId = $(rowsEar[i]).find(":eq(0)").html();
                newattr.AvaliableDays = $(rowsEar[i]).find(".AvaliableDays").html().trim();
                CompOffleaveCount = CompOffleaveCount + parseFloat(newattr.AvaliableDays);
                attr.push(newattr);
            }
        }

        $('#lblBalance').text("       " + CompOffleaveCount + "      ");
        $('#lblBalanceLevHide').text("       " + CompOffleaveCount + "      ");
        $leave.selectedCompOffGainId = attr;
    },

    //-------------
    clearData: function () {
        debugger;
        $('#txtFromDate').val('');
        $('#txtToDate').val('');
        $('#txtReason').val('');
        $('#txtContact').val('');

        $('#sltLevType').val('00000000-0000-0000-0000-000000000000');
        $("#lblBalance").empty();
        $("#lblBalanceLevHide").empty();
        $("#lblBalance").append("");
        $("#lblBalanceLevHide").append("");
        //$("#divattachment").addClass("nodisp");
        $("#attachtempID").text("");
        $("#fullpathID").text("");
        $("#lblattachfrmt").text("");
        $("#divattachment").addClass("nodisp");

    },
    //--------
    // Created by Keerthika on 24/06/2017
    cancelRequest: function () {
        $app.showProgressModel();
        var dataValue = {
            Id: $leave.Id,
            EmployeeId: $('#sltEmpCode').val(),
            FromDate: $('#txtFromDate').val(),
            EndDate: $('#txtToDate').val(),
            Reason: $('#txtReason').val(),
            LeaveType: $('#sltLevType').val(),
            LeavingStation: $('#sltLeavingStation').val(),
            Contact: $('#txtContact').val(),
            FromDay: $('#FsltDay').val(),
            ToDay: $('#TsltDay').val(),

            NoOfDays: (Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1,
            // NoOfDays: (daydiff(parseDate($('#txtFromDate').val()), parseDate($('#txtToDate').val()))),

            EnteredBy: $('#lblEnteredBy').val(),
            FirstLvlContact: $('#txt1stLevel').val(),
            FirstLvlContactData: $('#txt1stlvlData').val()
        }
        $.ajax({

            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/LeaveCancel",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dataValue: dataValue }),
            dataType: "json",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $leave.LoadData();
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                }

            },
            complete: function () {
                $app.hideProgressModel();
            }
        });

    },

    DeleteRequest: function (data) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/DeleteLeaveRequests",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ GID: data.Id }),
            dataType: "json",
            success: function (jsonResult) {

                var out = jsonResult.result;

                switch (jsonResult.Status) {

                    case true:

                        $app.showAlert(jsonResult.Message, 2);
                        $leave.LoadData();
                        $leave.clearData();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        $leave.LoadData();
                        $leave.clearData();
                        break;
                }

            },
            error: function (msg) {
            }
        });

    },

    LeaveRequestList: function (Reqtype) {
        debugger;
        var dtClientList = $('#tblLeaveList').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [{ "data": "Id" },
                { "data": "Empcode" },
         {
             "data": "FromDate",
             render: function (data) {

                 var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')))
                 // return date.getDate() + '/' + date.getMonth() + '/' + date.getYear();
                 //var month = date.getMonth() + 1;
                 //var day = date.getDate();
                 //var year = date.getFullYear();
                 //var dateF = day + "/" + month + "/" + year;
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
        var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')))
        // return date.getDate() + '/' + date.getMonth() + '/' + date.getYear();
        //var month = date.getMonth() + 1;
        //var day = date.getDate();
        //var year = date.getFullYear();
        //var dateE = day + "/" + month + "/" + year;
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
{ "data": "Reason" },

{ "data": null }],

            "aoColumnDefs": [
                             {
                                 "aTargets": [0],
                                 "sClass": "nodisp"
                             },
                      {
                          "aTargets": [8],
                          "sClass": "actionColumn",

                          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                              var b = $('<a href="#" class="editeButton" title="View"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                              //var c = $('<a href="#" title="Reject"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                              b.button();
                              b.on('click', function () {
                                  debugger;
                                  $leave.GetDetail(oData);
                                  return false;
                              });
                              //c.button();
                              //c.on('click', function () {
                              //    if (confirm('Are you sure ,do you want to delete?')) {
                              //        $leave.Reject(oData);
                              //    }
                              //    return false;
                              //});
                              $(nTd).empty();
                              $(nTd).prepend(b);
                          }
                      }],


            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetEmpLeaveRequest",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: '00000000-0000-0000-0000-000000000000', Reqtype: Reqtype }),
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
    HRLeaveRequestList: function () {

        var dtClientList = $('#tblLeaveList').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [{ "data": "Id" },
                { "data": "Empcode" },
         {
             "data": "FromDate",
             render: function (data) {

                 var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')))
                 var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                 return dateF;


             }
         },
{
    "data": "EndDate",
    render: function (data) {
        var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')))
        // return date.getDate() + '/' + date.getMonth() + '/' + date.getYear();
        //var month = date.getMonth() + 1;
        //var day = date.getDate();
        //var year = date.getFullYear();
        //var dateE = day + "/" + month + "/" + year;
        var dateE = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
        return dateE;

    }
},
//{ "data": "LeaveType" },
{ "data": "Reason" },

{ "data": null }],

            "aoColumnDefs": [
                             {
                                 "aTargets": [0],
                                 "sClass": "nodisp"
                             },
                      {
                          "aTargets": [5],
                          "sClass": "actionColumn",

                          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                              var b = $('<a href="#" class="editeButton" title="View"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                              //var c = $('<a href="#" title="Reject"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                              b.button();
                              b.on('click', function () {

                                  $leave.GetDetail(oData);
                                  return false;
                              });
                              //c.button();
                              //c.on('click', function () {
                              //    if (confirm('Are you sure ,do you want to delete?')) {
                              //        $leave.Reject(oData);
                              //    }
                              //    return false;
                              //});
                              $(nTd).empty();
                              $(nTd).prepend(b);
                          }
                      }],


            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/HRGetEmpLeaveRequest",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: $("#sltEmployeeCode").val(), Selectiontype: $("#txtSelectiontype").val() }),
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
    GetDetail: function (data) {

        var priority = data.prioritynumber;
        var assigmgr = data.AssignmanagerId;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/GetLeaveRequest",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ LeaveReqId: data.Id }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {

                var out = jsonResult.result;

                switch (jsonResult.Status) {

                    case true:
                        debugger;
                        $leave.Id = out.Id;
                        $('#sltEmpCode').val(out.EmployeeId);
                        $('#txtEmpName').val(out.emp.empFName);
                        $('#txtDateJoin').val(out.emp.empDOJ);
                        $('#txtFromDate').val(out.FromDate);
                        $('#txtLevel').val(out.emp.empEmail);
                        $('#txtToDate').val(out.EndDate);
                        $('#txtReason').val(out.Reason);
                        $('#sltLevType').val(out.LeaveType);
                        $('#sltLeavingStation').val(out.LeavingStation);
                        $('#txtContact').val(out.Contact);
                        $('#FsltDay').val(out.FromDay);
                        $('#TsltDay').val(out.ToDay);
                        $('#txtEmpCode').val(out.emp.empCode);
                        $('#lblpriority').val(priority);
                        $('#lblASSMGR').val(assigmgr);

                        if (out.Imgpath != null) {
                            $('#divattachid').removeClass("nodisp");
                            $('#lblhiddenattachpath').text(out.Imgpath);
                        }
                        else {
                            $('#divattachid').addClass("nodisp");
                            $('#lblhiddenattachpath').text("");
                        }
                        //NoOfDays: Math.abs(new Date(EndDate) - new Date(FromDate)) / 1000 / 60 / 60 / 24,
                        if (out.LeaveType != out.DefaultLOPid)//199f5db2-14b7-46d3-a0e4-715d56682277
                        {
                            $leave.GetLeavebalance(out.NoOfDays);
                        }
                        else {
                            $('#sltLevType').val(out.DefaultLOPid);//199f5db2-14b7-46d3-a0e4-715d56682277
                        }
                        $leave.CompOff.val(out.CompOff);
                        break;
                    case false:

                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    GetEmpDetail: function (data) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/EmpLeaveDetails",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ employeeId: $("#sltEmpCode").val(), leaveType: $("#sltLevType").val() }),
            dataType: "json",
            success: function (jsonResult) {

                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:
                       
                        $('#btnsaveleaverequest').show()
                        $leave.EmpId = out.employeeId;
                        $leave.enteredBy = out.enteredBy;
                        $('#sltEmpCode').val(out.employeeId);
                        $('#txtEmpName').val(out.empName);
                        $('#txtDateJoin').val(out.empDOJ);
                        $('#txtLevel').val(out.firstlevelContact);
                        $('#lblBalance').text(out.balanceLeave);
                        $('#lblBalanceLevHide').text(out.balanceLeave);
                        break;
                    case false:

                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    //---- Created by Keerthika on 20/06/2017
    GetParameterValues: function (param) {

        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    },
    RejectLeave: function () {

        var EmployeeId = $leave.GetParameterValues('empid');
        var LeaveId = $leave.GetParameterValues('id');
        var text = $('#txtReject').val();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/LeaveRejectMail",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ employeeId: EmployeeId, id: LeaveId, text: text }),
            dataType: "json",
            success: function (jsonResult) {
                switch (jsonResult.Status) {

                    case true:
                        $('#txtReject').hide();
                        $('#reason').hide();
                        $('#btnReject').hide();
                        $('#msg').text("Leave Rejected Successfully");
                        //  alert("Leave Rejected Successfully");

                    case false:
                        alert("Leave not Rejected ");
                        //  $app.showAlert(jsonResult.Message, 4);
                }
                //  var out = jsonResult.result;



            },
            error: function (msg) {
            }
        });
    },
    //-------------- created by keerthika on 15/06/2017
    GetEmployeeDetail: function (data) {

        var sltEmpCode = $('#sltEmpCode').val();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetEmployeeData",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ empId: sltEmpCode }),
            dataType: "json",
            success: function (jsonResult) {

                // var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:
                        var p = jsonResult.result;
                        $('#txtEmpName').val(p.empFName).prop('disabled', true);

                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    GetEmployeDetail: function (data) {

        var sltEmpCode = $('#sltEmpCode').val();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetEmployeeData",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ empId: sltEmpCode }),
            dataType: "json",
            success: function (jsonResult) {

                // var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:

                        var p = jsonResult.result;
                        var empName = p.empFName + " " + p.empLName;
                        // $('#txtEmailId').val(p.empEmail).prop('disabled', true);
                        // $('sltEmpCode').val(p.empCode).prop('disabled', true);
                        $('#txtDateJoin').val(p.empDOJ).prop('disabled', true);
                        $('#txtEmpName').val(empName).prop('disabled', true);
                        break;

                    case false:

                        $('#txtDateJoin').val('').prop('disabled', true);
                        $('#txtEmpName').val('').prop('disabled', true);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    //-----
    getEmpAssignManager: function () {

        var sltEmpCode = $('#sltEmpCode').val();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/GetEmpAssignManager",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EmployeeId: sltEmpCode }),
            dataType: "json",
            success: function (jsonResult) {

                // var out = jsonResult.result;

                var p = jsonResult;
                $('#txtFirstLevel').val(p.AssEmpId);





            },
            error: function (msg) {
            }
        });
    },

    allUser: function () {
        $(".date-picker").datepicker({
            dateFormat: 'dd/mm/yy'
        });

        $(".date-picker").each(function () {
            $(this).add($(this).next()).wrapAll('<div class="imageInputWrapper"></div>');
        });

        $('#txtToDate').on('change', function (e) {
            $("#fDate").removeClass("red");
            $("#tDate").removeClass("red");
            var fromDate = $("#fDate").datepicker('getDate');
            var toDate = $("#tDate").datepicker('getDate');

            if (toDate <= fromDate) { //here only checks the day not month and year
                $("#fDate").addClass("red");
                $("#tDate").addClass("red");
                errors++;
            }

            if (errors) e.preventDefault();
        });
    },
    //-----

    //-----
    save: function () {

        var err = 0;
        if ($('#txtFirstLevel').val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert("Please select the Assign Employee Code", 4);
            err = 1;
        }
        else if ($('#sltEmpCode').val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert("Please select the  Employee Code", 4);
            err = 1;
        }
        var data = {

            EmployeeId: $('#sltEmpCode').val(),
            AssEmpId: $('#txtFirstLevel').val(),
            CompanyId: $('#hdnCompId').val()
        };
        //  LastName: $($User.formData).find('#txtLastName').val(),
        console.log(data);
        if (err == 0) {
            $.ajax({
                url: $app.baseUrl + "LeaveRequest/SaveAssignManager",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {

                    $app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:


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
                    $app.hideProgressModel();
                }
            });
            return false;
        }

    },

    //--------
    getManager: function (context) {


        $.ajax({
            url: $app.baseUrl + "LeaveRequest/GetAssignManager",
            data: JSON.stringify({ EMPLOYEEID: $("#hdnEmpId").val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var out = jsonResult.result;

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        $('#txt1stLevel').val(out[0].Email);
                        $('#txt1stlvlData').val(out[0].firstlevelData);
                        break;
                    case false:
                        $('#txt1stLevel').val('');
                        $('#txt1stlvlData').val('');
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });

    },





    //----
    clearLeaveEntry: function (data) {

        $('#sltEmpCode').val();
        $('#txtFromDate').val('');
        $('#txtToDate').val('');
        $('#txtReason').val('');
        $('#sltLevType').val('');
        $('#sltLeavingStation').val();
        $('#txtContact').val('');
        $('#sltDay').val(0);
        $('#lblBalance').text('');
        $('#lblBalanceLevHide').text('');
        $('#lblpriority').val('');
    },
    reject: function () {
        debugger;
        var sltEmpCode =
           {
               EmployeeId: $('#txtEmpCode').val(),
               LeaveType: $('#sltLevType').val()
           }
        $leave.GetEmpDetail(sltEmpCode);




        var fromday = $('#FsltDay').val();
        var today = $('#TsltDay').val();
        var noofdays = (Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1;
        if ($("#txtFromDate").val() != $("#txtToDate").val()) {
            if (fromday != 0) {
                noofdays = noofdays - 0.5;
            }
            if (today != 0) {
                noofdays = noofdays - 0.5;
            }
        }
        else if ($("#txtFromDate").val() == $("#txtToDate").val() && ($("#FsltDay").val() && $("#TsltDay").val()) != "0") {
            noofdays = noofdays - 0.5;
        }
        else {
            noofdays = noofdays;
        }

        var dataValue = {
            Id: $leave.Id,
            //EmployeeId: EmpDetail.Id,
            EmployeeId: $('#sltEmpCode').val(),
            NavTabStatus: $('#lblhiddenNavtab').text(),
            FromDate: $('#txtFromDate').val(),
            EndDate: $('#txtToDate').val(),
            Reason: $('#txtReason').val(),
            LeaveType: $('#sltLevType').val(),
            LeavingStation: $('#sltLeavingStation').val(),
            Contact: $('#txtContact').val(),
            HalfFullDay: $('#sltDay').val(),
            NoOfDays: noofdays,
            EnteredBy: $('#lblEnteredBy').val(),
            Status: 2,
            Rejectreason: $('#txtRejreason').val(),
            FirstLevelcontact: $('#txtLevel').val(),
            prioritynumber: $('#lblpriority').val(),
            AssignmanagerId: $('#lblASSMGR').val(),
            ApporRejorcancStat: 2
        }
        var employee = {
            EmployeeId: $('#sltEmpCode').val(),
            Status: 2,
        }

        $leave.RejectLeaveStatus(dataValue);
    },
    HRreject: function () {

        var sltEmpCode =
           {
               EmployeeId: $('#txtEmpCode').val(),
               LeaveType: $('#sltLevType').val()
           }
        $leave.GetEmpDetail(sltEmpCode);




        var fromday = $('#FsltDay').val();
        var today = $('#TsltDay').val();
        var noofdays = (Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1;
        if ($("#txtFromDate").val() != $("#txtToDate").val()) {
            if (fromday != 0) {
                noofdays = noofdays - 0.5;
            }
            if (today != 0) {
                noofdays = noofdays - 0.5;
            }
        }
        else if ($("#txtFromDate").val() == $("#txtToDate").val() && ($("#FsltDay").val() && $("#TsltDay").val()) != "0") {
            noofdays = noofdays - 0.5;
        }
        else {
            noofdays = noofdays;
        }

        var dataValue = {
            Id: $leave.Id,
            //EmployeeId: EmpDetail.Id,
            EmployeeId: $('#sltEmpCode').val(),
            FromDate: $('#txtFromDate').val(),
            EndDate: $('#txtToDate').val(),
            Reason: $('#txtReason').val(),
            LeaveType: $('#sltLevType').val(),
            LeavingStation: $('#sltLeavingStation').val(),
            Contact: $('#txtContact').val(),
            HalfFullDay: $('#sltDay').val(),
            NoOfDays: noofdays,
            EnteredBy: $('#lblEnteredBy').val(),
            Status: 2,
            Rejectreason: $('#txtHRRejreason').val(),
            FirstLevelcontact: $('#txtLevel').val(),
            prioritynumber: $('#lblpriority').val(),
            AssignmanagerId: $('#lblASSMGR').val(),
            ApporRejorcancStat: 2,
            Selectiontype: $('#txtSelectiontype').val(),
        }
        var employee = {
            EmployeeId: $('#sltEmpCode').val(),
            Status: 2,
        }


        if ($('#txtEmpCode').val() == "") {
            $app.showAlert("Please Select a Leave Request to Reject!!!", 4);
        }
        else {
            $leave.HRRejectLeaveStatus(dataValue);
        }

    },
    approve: function () {
        debugger;
        var dataValue = {

            Id: $leave.Id,
            //EmployeeId: EmpDetail.Id,
            EmployeeId: $('#sltEmpCode').val(),
            NavTabStatus: $('#lblhiddenNavtab').text(),
            //FromDate: $('#txtFromDate').val(),
            //EndDate: $('#txtToDate').val(),
            //Reason: $('#txtReason').val(),
            LeaveType: $('#sltLevType').val(),
            //LeavingStation: $('#sltLeavingStation').val(),
            //Contact: $('#txtContact').val(),
            //HalfFullDay: $('#sltDay').val(),
            //NoOfDays: noofdays,
            //EnteredBy: $('#lblEnteredBy').val(),
            Status: 1,
            Rejectreason: $('#txtRejreason').val(),
            EmailId: $('#txtLevel').val(),
            prioritynumber: $('#lblpriority').val(),
            AssignmanagerId: $('#lblASSMGR').val(),
            ApporRejorcancStat: 1
        }
        var employee = {
            EmployeeId: $('#sltEmpCode').val(),
            Status: 1,
        }
        $leave.updateLeaveStatus(dataValue);
    },
    HRapprove: function () {


        if ($('#txtEmpCode').val() == "") {
            $app.showAlert("Please Select a Leave Request to Approve!!!", 4);
        }
        else {
            var sltEmpCode =
                {
                    EmployeeCode: $('#txtEmpCode').val(),
                    LeaveType: $('#sltLevType').val()
                }


            $leave.GetEmpDetail(sltEmpCode);

            var fromday = $('#FsltDay').val();
            var today = $('#TsltDay').val();
            var noofdays = (Math.abs((new Date($('#txtToDate').val()) - new Date($('#txtFromDate').val())) / (1000 * 60 * 60 * 24))) + 1;
            if ($("#txtFromDate").val() != $("#txtToDate").val()) {
                if (fromday != 0) {
                    noofdays = noofdays - 0.5;
                }
                if (today != 0) {
                    noofdays = noofdays - 0.5;
                }
            }
            else if ($("#txtFromDate").val() == $("#txtToDate").val() && ($("#FsltDay").val() && $("#TsltDay").val()) != "0") {
                noofdays = noofdays - 0.5;
            }
            else {
                noofdays = noofdays;
            }

            var dataValue = {

                Id: $leave.Id,
                //EmployeeId: EmpDetail.Id,
                EmployeeId: $('#sltEmpCode').val(),
                FromDate: $('#txtFromDate').val(),
                EndDate: $('#txtToDate').val(),
                Reason: $('#txtReason').val(),
                LeaveType: $('#sltLevType').val(),
                LeavingStation: $('#sltLeavingStation').val(),
                Contact: $('#txtContact').val(),
                HalfFullDay: $('#sltDay').val(),
                NoOfDays: noofdays,
                EnteredBy: $('#lblEnteredBy').val(),
                Status: 1,
                Rejectreason: $('#txtRejreason').val(),
                EmailId: $('#txtLevel').val(),
                prioritynumber: $('#lblpriority').val(),
                AssignmanagerId: $('#lblASSMGR').val(),
                ApporRejorcancStat: 1,
                Selectiontype: $('#txtSelectiontype').val(),
            }
            var employee = {
                EmployeeId: $('#sltEmpCode').val(),
                Status: 1,
            }
            $leave.HRupdateLeaveStatus(dataValue);
        }
    },
    managernameshow: function () {

        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/Showmanagername",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EMPID: $("#sltManager").val() }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:

                        $app.hideProgressModel();
                        $("#txtMgrName").val(out);
                        break;
                    case false:
                        // $('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    EmailupdateLeaveStatus: function () {

        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/UpdateLeaveStatusThroughMail",
            contentType: "application/json; charset=utf-8",

            data: JSON.stringify({ ApporRejorcancStat: $('#hdnAorRstat').val(), prioritynumber: $('#hdnprioritynum').val(), AssignmanagerId: $('#hdnassgnmngrid').val(), Id: $('#hdnLeaveid').val(), dataValueRejectreason: $('#txtRejreasonbymail').val(), dataValueStatus: $('#hdnstatus').val() }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:

                        //$('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $("#idrejectiondiv").addClass("nodisp");
                        document.getElementById('idsucessstatus').innerHTML = 'Leave Request Rejected Succesfully!!!';
                        break;
                    case false:
                        // $('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    updateLeaveStatus: function (data) {
        debugger;
        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/UpdateLeaveStatus",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:
                        //$('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message + "Accepted Successfully", 2);
                        $('#txtEmpCode').val('');
                        $('#txtEmpName').val('');
                        $('#txtDateJoin').val('');
                        //$('#txtDate').val('');
                        $('#txtLevel').val('');
                        $('#sltLevType').val('00000000-0000-0000-0000-000000000000');

                        $leave.LeaveRequestList();
                        //$leave.LeaveRequestList();
                        $leave.clearLeaveEntry();
                        break;
                    case false:
                        // $('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    HRupdateLeaveStatus: function (data) {

        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/HRUpdateLeaveStatus",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:

                        //$('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message + "Accepted Successfully", 2);
                        $('#txtEmpCode').val('');
                        $('#txtEmpName').val('');
                        $('#txtDateJoin').val('');
                        // $('#txtDate').val('');
                        $('#txtLevel').val('');
                        $('#sltLevType').val('00000000-0000-0000-0000-000000000000');
                        $('#txtFromDate').val('');
                        $('#txtToDate').val('');
                        $('#txtReason').val('');
                        $('#txtContact').val('');
                        $leave.HRselectionemployee();
                        $leave.clearLeaveEntry();
                        //$('#txtSelectedempcode').val('');
                        //$('#sltEmployeeCode').find('option:selected').text('--Select--');

                        // $leave.LeaveRequestList();
                        //$companyCom.loadEmployee({ id: 'sltEmployeeCode' });
                        //$leave.LeaveRequestList();

                        break;
                    case false:
                        // $('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },

    RejectLeaveStatus: function (data) {
        $app.showProgressModel();
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/UpdateLeaveStatus",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $('#AddReason').modal('toggle');
                        if (out.Status == 2) {
                            $leave.LeaveRequestList();
                            $('#txtEmpCode').val('');
                            $('#txtEmpName').val('');
                            $('#txtDateJoin').val('');
                            $('#txtDate').val('');
                            $('#txtLevel').val('');
                            $('#sltLevType').val('00000000-0000-0000-0000-000000000000');
                            $app.showAlert(jsonResult.Message + " is Rejected", 2);

                        }
                        else if (out.Status == 3) {
                            $leave.LoadData();
                            $app.showAlert(jsonResult.Message + " is Cancelled", 2);

                        }


                        //$leave.LeaveRequestList();
                        $leave.clearLeaveEntry();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $('#AddReason').modal('toggle');
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },


    HRRejectLeaveStatus: function (data) {
        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/HRUpdateLeaveStatus",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $('#AddHRReason').modal('toggle');
                        if (out.Status == 2) {
                            $app.showAlert(jsonResult.Message + " is Rejected", 2);
                            $('#txtEmpCode').val('');
                            $('#txtEmpName').val('');
                            $('#txtDateJoin').val('');
                            // $('#txtDate').val('');
                            $('#txtLevel').val('');
                            $('#sltLevType').val('00000000-0000-0000-0000-000000000000');
                            $('#txtFromDate').val('');
                            $('#txtToDate').val('');
                            $('#txtReason').val('');
                            $('#txtContact').val('');
                            $leave.HRselectionemployee();
                            $leave.clearLeaveEntry();
                        }
                        else if (out.Status == 3) {
                            $leave.LoadData();
                            $app.showAlert(jsonResult.Message + " is Cancelled", 2);

                        }


                        //$leave.LeaveRequestList();
                        $leave.clearLeaveEntry();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $('#AddHRReason').modal('toggle');
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },

    saveAssignmanager: function () {
        debugger
        $app.showProgressModel();
       
        var SaveAssignMgrValue = {
            Id: $leave.AssignMgrId,
            AssEmpId: $("#sltManager").val(),
            ApprovMust: $("#sltAppStat").val(),
            MgrPriority: $("#txtPrioNumb").val(),
            AppCancelRights: $("#sltAppCanRigh").val(),
            EmployeeId: $("#sltEmpCode").val()
        }

        $.ajax({
            url: $app.baseUrl + "LeaveRequest/SaveAssignManagerML",

            data: JSON.stringify({ SaveAssignMgrValue: SaveAssignMgrValue }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddAssignpopup').modal('toggle');
                        $leave.LoadAssignmanager();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $employee.canSave = true;
                        break;
                }
            },
            complete: function () {

            }
        });

    },




    LoadAssignmanager: function () {


        var dtClientList = $('#tblAssignedmanager1').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "MgrEmpCode" },
                     { "data": "MgrEmpName" },
                      { "data": "ApprovMustString" },
                       { "data": "MgrPriority" },
                        { "data": "AppCancelRightString" },
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
             "sClass": "word-wrap"

         },
         {
             "aTargets": [5],
             "sClass": "word-wrap"

         },
      {
          "aTargets": [6],
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {

                  if ($("#iddisplaystatus").text() == "") {
                      $leave.getAssignMgrData(oData);
                      return false;
                  }
                  else {
                      $app.showAlert("You Can't Edit this Manager,This Employee is UnderProcess", 4);
                  }


              });
              c.button();
              c.on('click', function () {

                  if ($("#iddisplaystatus").text() == "") {
                      if (confirm('Are you sure ,do you want do delete?')) {
                          $leave.deleteAssingMgr(oData);
                          return false;
                      }
                  }
                  else {
                      $app.showAlert("You Can't delete this Manager,This Employee is UnderProcess", 4);
                  }



              });
              $(nTd).empty();
              $(nTd).prepend(b, c);


          }
      }],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "LeaveRequest/GetMULTILEVELAssignManager",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EMPID: $("#sltEmpCode").val() }),
                    dataType: "json",
                    async: false,
                    success: function (jsonResult) {

                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {

                            case true:
                                $leave.MLAssMgrDetails = [];
                                console.log(jsonResult.result);
                                var out = jsonResult.result;
                                for (i = 0; i < out.length; i++) {


                                    var AssMgrObj = new Object();

                                    AssMgrObj.Id = out[i].Id;
                                    AssMgrObj.AssEmpId = out[i].AssEmpId;
                                    AssMgrObj.EmployeeId = out[i].EmployeeId;
                                    AssMgrObj.FinYear = out[i].FinYear;
                                    AssMgrObj.MgrEmpCode = out[i].MgrEmpCode;
                                    AssMgrObj.MgrEmpName = out[i].MgrEmpName;
                                    AssMgrObj.MgrPriority = out[i].MgrPriority;
                                    AssMgrObj.ApprovMust = out[i].ApprovMust;
                                    AssMgrObj.AppCancelRights = out[i].AppCancelRights;
                                    AssMgrObj.CompanyId = out[i].CompanyId;
                                    $leave.MLAssMgrDetails.push(AssMgrObj);
                                }
                                setTimeout(function () {
                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });

                                }, 500);
                                break;
                            case false:
                                $leave.MLAssMgrDetails = [];
                                $app.showAlert(jsonResult.Message, 4);
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblAssignedmanager tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblAssignedmanager thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });


    },
    deleteAssingMgr: function (context) {

        $.ajax({
            url: $app.baseUrl + "LeaveRequest/DeleteAssignMgrData",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $leave.LoadAssignmanager();
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

    getAssignMgrData: function (data) {

        $.ajax({
            url: $app.baseUrl + "LeaveRequest/GetAssMgrSelectedData",
            data: JSON.stringify({ Id: data.Id, EmpId: data.EmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
                        $leave.renderAssignMgr(data);
                        break;
                    case false:
                        alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    renderAssignMgr: function (data) {

        $('#AddAssignpopup').modal('toggle');
        var formData = document.forms["frmAssMgrPopup"];
        $leave.AssignMgrId = data.Id;
        formData.elements["sltManager"].value = data.AssEmpId;
        formData.elements["txtMgrName"].value = data.MgrEmpName;
        formData.elements["sltAppStat"].value = data.ApprovMust;
        formData.elements["txtPrioNumb"].value = data.MgrPriority;
        formData.elements["sltAppCanRigh"].value = data.AppCancelRights;


    },
    AddInitializeAssMgr: function () {


        var formData = document.forms["frmAssMgrPopup"];
        $leave.AssignMgrId = ''
        formData.elements["sltManager"].value = "00000000-0000-0000-0000-000000000000";
        formData.elements["txtMgrName"].value = "";
        formData.elements["sltAppStat"].value = 0;
        formData.elements["txtPrioNumb"].value = "";
        formData.elements["sltAppCanRigh"].value = 0;

    },

    LoadLeaveRelated: function (context) {

        switch (context.id) {
            case "leaveAprRejDetails":
                $('#divattachid').addClass('nodisp');
                $('#lblHederText').text("Leave Request");
                $('#lblhiddenNavtab').text("leaveReq");
                $leave.LeaveRequestList("leaveReq");
                break;
            case "compoffAprRejDetails":
                $('#divattachid').addClass('nodisp');
                $('#lblHederText').text("Comp-Off Gain Request");
                $('#lblhiddenNavtab').text("compoff");
                $leave.LeaveRequestList("compoff");
                break;
            case "ApprovedLevCancelAprRejDetails":
                $('#divattachid').addClass('nodisp');
                $('#lblHederText').text("Approved Leave Cancel Request");
                $('#lblhiddenNavtab').text("AppCanReq");
                $leave.LeaveRequestList("AppCanReq");
                break;
            case "Leave_Request":
                $('#lblhiddenNavtab').text("leaveReq");
                $leave.LoadData("leaveReq");
                break;
            case "Compoff_Request":
                debugger;
                $('#lblhiddenNavtab').text("compoffGainReq");
                $leave.LoadData("compoff");
                break;
            default:
                break;
        }
    },
}