

$("#btnRevert,#btncloserevert").on('click', function () {
    
    if (this.id == "btnRevert")
    {
        $Holiday.bStatus = 1;
    }
    else {
        $Holiday.bStatus = 2;
    }
        $Holiday.Revertfunc($Holiday.holidaydatess,$Holiday.leaveRevertdates);
    
    
       
    

});
$("#btnSave").on('click', function () {
    
    if ($("#txtFromDate").val() == "")
    {
        $app.showAlert('Please Select the Holiday Date!', 4);
    }
    else {
        $Holiday.save();
    }
 
});

//$("#txtToDate,#txtFromDate").change(function () {
//    var datefrom = new Date($("#txtFromDate").val());
//    var dateto = new Date($("#txtToDate").val());
//    if ($("#txtFromDate").val() != '' && $("#txtToDate").val() != '') {
//        if (dateto >= datefrom) {

//        }
//        else {

//            $app.showAlert('End Date should not be less than Start Date !', 3);
//            $("#txtToDate").val('');
//        }
//    }
//});



var $Holiday = {
    HolidayId: "00000000-0000-0000-0000-000000000000",
    canSave: false,
    dttblCess: null,
    holidaydatess: null,
    leaveRevertdates: null,
    bStatus : null,
   

    LoadHoliday: function () {
        debugger
        $payroll.initDatetime();

        var dtClientList = $('#tblHoliday').DataTable({
            
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "title": "Component", "data": "ComponentName" },
                    {
                        "data": "HolidayDate",
                         
                         render: function (data) {
                             
                             var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                             var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                             return dateF;
                             
                         }
                    
                    },
                    { "data": "Reason" },
                    { "data": "Type" },
                    {"data": null}
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
          "sClass": "actionColumn",

          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  // $levFinanceYear.GetFinanceYear(oData);
                  $Holiday.HolidayId = oData.Id;
                  $Holiday.AddInitialize();
                  $Holiday.RenderData(oData);
                  $Holiday.canSave = true;
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $Holiday.DeleteData(oData);
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
                    url: $app.baseUrl + "Leave/GetHolidayempty",
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
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },

    renderrevertholiday: function (Employeelistdata) {
        
        var dtClientList = $('#tblHolidayRevert').DataTable({

            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                   
                    {
                        "data": "date",

                        render: function (data) {
                            
                            var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                            var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                            return dateF;

                        }

                    },
                    { "data": "empcode" },
                    { "data": "empname" }
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
          "sClass": "word-wrap",

      }
            ],
            ajax: function (data, callback, settings) {
                
                setTimeout(function () {
                    callback({
                        draw: data.draw,
                        data: Employeelistdata,
                        recordsTotal: Employeelistdata.length,
                        recordsFiltered: Employeelistdata.length
                    });
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblAcademic tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblAcademic thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
 
    },

    save: function () {
        debugger;
        $Holiday.canSave = false;
        $app.showProgressModel();
        var holidaydatelise = null;
        var Employeelist = null;
        var data = {
            id:$Holiday.HolidayId,
            HolidayFromdate: $('#txtFromDate').val(),
            //HolidayTodate: $('#txtToDate').val(),
            HolidayTodate: $('#txtFromDate').val(),
            Reason: $('#txtReason').val(),
            Component: document.getElementById('lblHDSdynamic').innerText,
            ComponentValue: $('#ddlHDSDynamic').val(),
            Type: $('#ddlHDStype').val()
        };
        debugger;
        $.ajax({
            url: $app.baseUrl + "Leave/SaveHoliday",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $('#AddHoliday').modal('toggle');
                        $Holiday.LoadHoliday();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var p = jsonResult.result;
                        companyid = 0;
                        break;
                    case false:
                        debugger;
                        if (jsonResult.result.statuserror == 1) {
                            $Holiday.holidaydatess = jsonResult.result.jsondataRevHolidates;
                            Employeelist = jsonResult.result.jsondataholiday;
                            $Holiday.leaveRevertdates = jsonResult.result.jsondataRevertdates;
                            $('#AddHoliday').modal('hide');
                            $('#Revertholidayleave').modal('toggle');
                            $Holiday.renderrevertholiday(Employeelist);
                        }
                        else {
                            $app.hideProgressModel();
                              $app.showAlert(jsonResult.Message, 4);
                             $('#txtFromDate').val('');
                             //$('#txtToDate').val('');
                            $('#txtReason').val('');
                        }
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }

        });
    },
   
    Revertfunc: function (holidaydatess, leaveRevertdates) {
        console.log(holidaydatess);
        console.log(leaveRevertdates);
        //for (i = 0; i > holidaydatess.length; i++) {
        //    var dA = new Date(holidaydatess[i].Holidaydate);

        //    //var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
        //    //var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
        //    //return dateF;
        //}
       
        var bStatus = $Holiday.bStatus;
        
        $.ajax({
            url: $app.baseUrl + "Leave/RevertLeaveholiday",
            data: JSON.stringify({ Holidaydates: holidaydatess, Revertdate: leaveRevertdates, bStatus }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $Holiday.LoadHoliday();
                        $('#Revertholidayleave').modal('hide');
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

    Revertfuncsave: function () {
        

        $.ajax({
            url: $app.baseUrl + "Leave/RevertLeaveholidaySave",
            data: JSON.stringify({ Holidaydates:holidaydatess}),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $Holiday.LoadHoliday();
                        $('#Revertholidayleave').modal('hide');
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
        debugger;
        $.ajax({
            url: $app.baseUrl + "Leave/GetDynamicHolidaysettings",
            data: null,
            dataType: "json",
            contentType: "json",
            type: "POST",
            async: false,
            success: function (msg) {
                debugger;
                var out = msg.result;
                switch (msg.Status) {
                    case true:
                        debugger;
                        $('#ddlHDSDynamic').empty();
                        //Commented in order to display select in ddl during loading.
                        //$('#ddlHDSDynamic').append($("<option></option>").val('select').html('--Select--'));
                        $('#ddlHDSDynamic').append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                        $.each(out, function (index, blood) {
                            $('#ddlHDSDynamic').append($("<option></option>").val(blood.Id).html(blood.name));
                        });
                        $('#lblHDSdynamic').html(msg.Message);
                        if (msg.Message == "COMPANYWISE") {
                            $('#divHDSdynamicload').addClass("nodisp");
                        }
                        break;
                    case false:

                        break;
                }
            },
            error: function () {
                debugger;
            }
        });
        var formData = document.forms["frmHolidaySettings"];
        $Holiday.HolidayId = "00000000-0000-0000-0000-000000000000",
        $("label[for = FromDate]").text("From Date");
        $('#divTodate').removeClass('nodisp');
        $('#txtFromDate').val(''),
         //$('#txtToDate').val(''),
        $('#txtReason').val(''),


        $Holiday.canSave = true;

    },



     GetHoliday: function (context) {
        $.ajax({
            url: $app.baseUrl + "Leave/GetFinanceYear",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddHoliday').modal('toggle');
                        var p = jsonResult.result;
                        $Holiday.RenderData(p);
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
   
    DeleteData: function (context) {
        
        $.ajax({
            url: $app.baseUrl + "Leave/DeleteHoliday",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $Holiday.LoadHoliday();
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

    RenderData: function (data) {
        debugger;
        $('#AddHoliday').modal('toggle');
        var formData = document.forms["frmHolidaySettings"];
        $Holiday.financeYearId = data.id;
       
        var date = new Date(parseInt(data.HolidayDate.replace(/(^.*\()|([+-].*$)/g, '')));
        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
        $("label[for = FromDate]").text("Holiday Date"); 
        $('#divTodate').addClass('nodisp');
        formData.elements["txtFromDate"].value = dateF;
        formData.elements["txtReason"].value = data.Reason;
        $('#ddlHDSDynamic').val(data.ComponentValue);     
        $('#lblHDSdynamic').html(data.Component);
        if (data.Component == "COMPANYWISE") {
            $('#divHDSdynamicload').addClass("nodisp");
        }
    },
   
};