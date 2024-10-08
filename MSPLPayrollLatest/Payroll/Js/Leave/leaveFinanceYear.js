﻿
//--FOR FINANCE YEAR DATE CALCULATION--//
$('#txtStartDate').change(function () {
    
    var SDate = new Date($('#txtStartDate').val());
    var TempDate = new Date(SDate.setMonth(SDate.getMonth() + 12));
    var EDate = new Date(TempDate.setDate(TempDate.getDate() - 1));
   // console.log(EDate);
    $("#txtEndDate").val(EDate.getDate() + '/' + $payroll.GetMonthName((EDate.getMonth() + 1)) + '/' + EDate.getFullYear());

});
//----//
var $levFinanceYear = {

    financeYearId: "00000000-0000-0000-0000-000000000000",
    canSave: false,
    dttblCess: null,
    LoadfinanceYears: function () {
        
        $payroll.initDatetime();
       
        var dtClientList = $('#tblFinanceYear').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "id" },
                    { "data": "startDate" },
                    { "data": "EndDate" },
                      
                        { "data": "defaultyear" },
                       {
                           "data": null
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
          "sClass": "actionColumn",

          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  // $levFinanceYear.GetFinanceYear(oData);
                  $levFinanceYear.RenderData(oData);
                  $levFinanceYear.canSave = true;
                  $("#txtStartDate").datepicker("option", "disabled", true); 
                  $('.ui-datepicker-title').css("display", "none");
                  $('.ui-datepicker-week-end').css("display", "none");
                  $('.ui-datepicker-header.ui-widget-header.ui-helper-clearfix.ui-corner-all').css("display", "none");
                  //$('.ui-icon.ui-icon-circle-triangle-w').css("display", "none");
                  //$('.ui-icon.ui-icon-circle-triangle-e').css("display", "none");
                  //$('.ui-datepicker-header.ui-widget-header.ui-helper-clearfix.ui-corner-all').css("display", "none");


                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $levFinanceYear.DeleteData(oData);
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
                    url: $app.baseUrl + "Leave/GetFinanceYears",
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
    }
        ,
    save: function () {
        
        debugger
        if (!$levFinanceYear.canSave) {
            return false;
        }
        $levFinanceYear.canSave = false;
        $app.showProgressModel();
   
        var data = {
            id: $levFinanceYear.financeYearId,
            startDate: $('#txtStartDate').val(),
            EndDate: $('#txtEndDate').val(),
           
            defaultyear: $('#chkDefault').is(':checked')
            //,otherExemption: otherExemption
        };
        $.ajax({
            url: $app.baseUrl + "Leave/SaveFinanceYear",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddFinancialYear').modal('toggle');
                        $levFinanceYear.LoadfinanceYears();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var p = jsonResult.result;
                        companyid = 0;
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
    },

    AddInitialize: function () {
        var formData = document.forms["frmFinancialYear"];
        $levFinanceYear.financeYearId = '00000000-0000-0000-0000-000000000000';
        $('#txtStartDate').val(''),
        $('#txtEndDate').val(''),
        $("#txtStartDate").datepicker("option", "disabled", false);
        $("#chkDefault").attr('disabled', false);
        //$('.ui-datepicker-title').css("display",true);
        //$('.ui-datepicker-week-end').css("display", true);
        //$('.ui-datepicker-header.ui-widget-header.ui-helper-clearfix.ui-corner-all').css("display", true);


      
      
        $levFinanceYear.canSave = true;
     
    },

    GetFinanceYear: function (context) {
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
                        $('#AddfinanceYear').modal('toggle');
                        var p = jsonResult.result;
                        $levFinanceYear.RenderData(p);
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
        
        //$levFinanceYear.financeYearId = context.id;
        $.ajax({
            url: $app.baseUrl + "Leave/DeleteFinanceYear",
            data: JSON.stringify({ finId: context.id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $levFinanceYear.LoadfinanceYears();
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
        
        $('#AddFinancialYear').modal('toggle');
        var formData = document.forms["frmFinancialYear"];
        $levFinanceYear.financeYearId = data.id;
        formData.elements["txtStartDate"].value = data.startDate;
        formData.elements["txtEndDate"].value = data.EndDate;
        if (data.defaultyear == true) {
            $("#chkDefault").prop('checked', data.defaultyear);
            $("#chkDefault").attr('disabled', true);
        }
        else {
            $("#chkDefault").prop('checked', data.defaultyear);
            $("#chkDefault").attr('disabled', false);
        }
        
    

    },
   
  
  
    
};