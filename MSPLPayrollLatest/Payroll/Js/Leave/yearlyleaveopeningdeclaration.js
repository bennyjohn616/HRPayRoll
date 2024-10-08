

$("#btnyrlylevopndecSave").on('click', function () {
    debugger;
    if ($("#categoryYLOD").val() == "--Select--")
    {
        $app.showAlert('Please Select the Category!', 4);
    }
    else if ($("#leaveTypeYLOD").val() == "--Select--")
        {
        $app.showAlert('Please Select the Leave Type!', 4);
    }
    else if (($("#txtNoOfDaysYLOD").val() == "") || ($("#txtNoOfDaysYLOD").val() == "0"))
    {
        $app.showAlert('Please Enter the Opening Days!', 4);
    }
    else {
        $Yrlylevopendec.save();
    }
 
});



var $Yrlylevopendec = {

    levopndecidId: "00000000-0000-0000-0000-000000000000",

    save: function () {
        debugger;
        $app.showProgressModel();
        var holidaydatelise = null;
        var Employeelist = null;
        var data = {
            levopndecID: $Yrlylevopendec.levopndecidId,
            Category: $('#categoryYLOD').val(),
            levtype: $('#leaveTypeYLOD').val(),
            opendays: $('#txtNoOfDaysYLOD').val(),
        };
        $.ajax({
            url: $app.baseUrl + "Leave/yrlylevopndec",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#Addlevopnyrlydec').modal('toggle');
                        $Yrlylevopendec.Loadyrlylevopndec();
                        break;
                    case false:
                        debugger;
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
    //AddInitialize: function () {
    //    debugger;
    //    var formData = document.forms["Yearlyleaveopeningdeclaration"];
    //    $Holiday.HolidayId = "00000000-0000-0000-0000-000000000000",
    //    $("label[for = FromDate]").text("From Date");
    //    $('#divTodate').removeClass('nodisp');
    //    $('#txtFromDate').val(''),
    //    //$('#txtToDate').val(''),
    //    $('#txtReason').val(''),


    //    $Holiday.canSave = true;

    //},


    LoadYearlylevopndecPopup: function () {
        $companyCom.loadLeaveType({ id: 'leaveTypeYLOD' });
        $companyCom.loadCategory({ id: 'categoryYLOD' });
        $("#txtNoOfDaysYLOD").val("");

    },
    Loadyrlylevopndec: function () {
        debugger
        $payroll.initDatetime();

        var dtClientList = $('#tblYrlylevopndec').DataTable({

            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "levopndecID" },
                    { "data": "Category" },
                    { "data": "CATIDNAME" },
                    { "data": "levtype" },
                     { "data": "LEVIDNAME" },
                    { "data": "opendays" },
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
             "sClass": "nodisp"

         },
         {
             "aTargets": [2],
             "sClass": "word-wrap"

         },
         {
             "aTargets": [3],
             "sClass": "nodisp"

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
          "sClass": "actionColumn",

          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  debugger;
                  // $levFinanceYear.GetFinanceYear(oData);
                  $Yrlylevopendec.levopndecidId = oData.levopndecID;
                  $Yrlylevopendec.RenderData(oData);
                  $Holiday.canSave = true;
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $Yrlylevopendec.DeleteData(oData);
                  }
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend(b, c);
          }
      }
            ],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetYearlylevopeningsdec",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
                        var out = jsonResult.result;
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
    RenderData: function (data) {
        debugger;
        $('#Addlevopnyrlydec').modal('toggle');
        var formData = document.forms["Yearlyleaveopeningdeclaration"];
        $Yrlylevopendec.LoadYearlylevopndecPopup();
        formData.elements["categoryYLOD"].value = data.Category;
        formData.elements["leaveTypeYLOD"].value = data.levtype;
        formData.elements["txtNoOfDaysYLOD"].value = data.opendays;
    },
    DeleteData: function (context) {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Leave/DeleteYearlyopeningDeclaration",
            data: JSON.stringify({ id: context.levopndecID }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $Yrlylevopendec.Loadyrlylevopndec();
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
}
