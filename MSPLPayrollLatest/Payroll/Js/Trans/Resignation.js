$("#txtLastWorkingDate,#txtDOJ").change(function () {

    var datefrom = new Date($("#txtDOJ").val());
    var dateto = new Date($("#txtLastWorkingDate").val());

    //var TODATE = dateto.getDate() + "/" + monthNames[(dateto.getMonth())] + "/" + dateto.getFullYear();

    var dateMonthlyInput = new Date($("#txtMonthlyLast").val());
    var datePayrollInput = new Date($("#txtPayrollLast").val());

    var dateMonthlyInputAdd = new Date($("#txtMonthly").val());
    var datePayrollInputAdd = new Date($("#txtPayroll").val());

    var previouspayrollinput = new Date(datePayrollInputAdd);
    var newdate = new Date(previouspayrollinput);
    newdate.setDate(newdate.getMonth() - 1); // minus the date
    var lastDay = new Date(newdate.getFullYear(), newdate.getMonth() + 1, 0);  //last date 
    var PPI = new Date(lastDay);

    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    $Resignation.renderForm($("#sltResgCategorylist").val());
    var MonthlyInput = dateMonthlyInput.getDate() + "/" + monthNames[(dateMonthlyInput.getMonth())] + "/" + dateMonthlyInput.getFullYear();
    var PayrollInput = datePayrollInput.getDate() + "/" + monthNames[(datePayrollInput.getMonth())] + "/" + datePayrollInput.getFullYear();
    var MonthlyInputAdd = dateMonthlyInputAdd.getDate() + "/" + monthNames[(dateMonthlyInputAdd.getMonth())] + "/" + dateMonthlyInputAdd.getFullYear();
    var PayrollInputAdd = datePayrollInputAdd.getDate() + "/" + monthNames[(datePayrollInputAdd.getMonth())] + "/" + datePayrollInputAdd.getFullYear();

    if (dateto < datefrom) {
        $app.showAlert('Last working date doesnt grater than DOJ', 4);
        $("#txtLastWorkingDate").val('');
    }
    else {
        if ($("#txtDOJ").val() != '' && $("#txtLastWorkingDate").val() != '' && $("#txtMonthlyLast").val() != '' && $("#txtPayrollLast").val() != '') {
            if ($Resignation.monthdays == "MonthDay" || $Resignation.monthdays == "StaticDay" || $Resignation.monthdays == "MonthlyInput") {
                if (dateto > PPI || dateto == PPI) {
                    if (dateto < datePayrollInputAdd) {
                        if (confirm('Already Payroll is processed for this month!Do you want to continue')) {
                            $("#txtLastWorkingDate").val();
                        }
                        else {
                            $("#txtLastWorkingDate").val('');
                        }
                    }
                    else if (dateto < dateMonthlyInputAdd) {
                        if (confirm('Monthly input is saved.Do you want to continue')) {
                            $("#txtLastWorkingDate").val();
                        }
                        else {
                            $("#txtLastWorkingDate").val('');
                        }
                    }
                    else {
                        $("#txtLastWorkingDate").val();
                    }
                }
                else if (dateto < PPI) {
                    $app.showAlert('Payroll is processed for this month!', 4);
                    $("#txtLastWorkingDate").val('');
                }
            }
            if ($Resignation.monthdays == "StartDay") {
                var StartdayPayroll = $Resignation.MonthDayOrStartDay + "/" + monthNames[(datePayrollInputAdd.getMonth())] + "/" + datePayrollInputAdd.getFullYear();
                var previouspayrollinput = new Date(StartdayPayroll);
                var newdate = new Date(previouspayrollinput);
                newdate.setDate(newdate.getDate() - 1); // minus the date
                var Startdaypayrollinput = new Date(newdate);        // current month payslip

                var newdate1 = new Date(previouspayrollinput);
                newdate1.setDate(newdate1.getMonth() - 1); // minus the month
                var startdatepayrollmonth = new Date(newdate1);

                var StartdayPayrollI = $Resignation.MonthDayOrStartDay + "/" + monthNames[(startdatepayrollmonth.getMonth())] + "/" + startdatepayrollmonth.getFullYear();
                var previouspayrollinputI = new Date(StartdayPayrollI);
                var newdate2 = new Date(previouspayrollinputI);
                newdate2.setDate(newdate2.getDate() - 1); // minus the date
                var PreviousStartdaypayrollinput = new Date(newdate2);  //previous month payslip

                var StartdayMonthlyinputI = $Resignation.MonthDayOrStartDay + "/" + monthNames[(dateMonthlyInputAdd.getMonth())] + "/" + dateMonthlyInputAdd.getFullYear();
                var previousMonthlyinput = new Date(StartdayMonthlyinputI);
                var newdate3 = new Date(previousMonthlyinput);
                newdate3.setDate(newdate3.getDate() - 1); // minus the date
                var StartdayMonthlyinput = new Date(newdate3);        //last monthly input

                if (dateto > PreviousStartdaypayrollinput || dateto == PreviousStartdaypayrollinput) {
                    if (dateto < Startdaypayrollinput) {
                        if (confirm('Already Payroll is processed for this month!Do you want to continue')) {
                            $("#txtLastWorkingDate").val();
                        }
                        else {
                            $("#txtLastWorkingDate").val('');
                        }
                    }
                    else if (dateto < StartdayMonthlyinput) {
                        if (confirm('Monthly input is saved.Do you want to continue')) {
                            $("#txtLastWorkingDate").val();
                        }
                        else {
                            $("#txtLastWorkingDate").val('');
                        }
                    }
                    else {
                        $("#txtLastWorkingDate").val();
                    }
                }
                else if (dateto < PreviousStartdaypayrollinput) {
                    $app.showAlert('Payroll is processed for this month!', 4);
                    $("#txtLastWorkingDate").val('');
                }
            }
        }
        else if (dateMonthlyInputAdd == "" && datePayrollInputAdd == "") {
            if (dateto < datefrom) {
                $app.showAlert('Last working date doesnt grater than DOJ', 4);
                $("#txtLastWorkingDate").val('');
            }
            else {
                $("#txtLastWorkingDate").val();
            }
        }
        else if (dateMonthlyInputAdd != "") {
            if (dateto < datefrom) {
                $app.showAlert('Last working date doesnt grater than DOJ', 4);
                $("#txtLastWorkingDate").val('');
            }
            else {
                $("#txtLastWorkingDate").val();
            }
        }
        else {
            if (dateto < datefrom) {
                $app.showAlert('Last working date doesnt grater than DOJ', 4);
                $("#txtLastWorkingDate").val('');
            }
            else {
                $("#txtLastWorkingDate").val();
            }
        }
    }

});

$("#sltResgCategorylist").change(function () {

    if ($("#sltResgCategorylist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#dvemp").addClass('nodisp');
        $("#dvdetails").addClass('nodisp');
    }
    else {
        $Resignation.SelectedCatId = $("#sltResgCategorylist").val();
        $companyCom.loadSelectiveEmployee({ id: 'sltResgEmployeelist', condi: 'Category.' + $("#sltResgCategorylist").val() });
        $("#dvemp").removeClass('nodisp');
        $("#sltResgEmployeelist").val("00000000-0000-0000-0000-000000000000");
        $("#dvdetails").addClass('nodisp');
    }
});



$("#sltResgEmployeelist").change(function () {

    if ($("#sltResgEmployeelist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#dvdetails").addClass('nodisp');
    }
    else {
        $Resignation.SelectedCatId = $("#sltResgCategorylist").val();
        $Resignation.SeparationEmpId = $("#sltResgEmployeelist").val();
        $("#dvdetails").removeClass('nodisp');
        $Resignation.LoadSeparation({ Id: $Resignation.SeparationEmpId, CatId: $Resignation.SelectedCatId });
        $("#txtLastWorkingDate").val('');
        $("#txtReason").val('');
    }
});


var $Resignation = {
    UserId: '',
    SeparationEmpId: '',
    SelectedCatId: '',
    monthdays: '',
    MonthDayOrStartDay: '',
    loadInitial: function () {
        $payroll.initDatetime();
        $companyCom.loadCategory({ id: "sltResgCategorylist" });
        $("#dvemp").addClass('nodisp');
        $Resignation.BindResignationEmployees();
    },
    renderForm: function (categoryid) {
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Setting/GetSeperationSetting",
            data: JSON.stringify({ category: categoryid }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $Resignation.monthdays = jsonResult.result[0].MonthDayProcess;
                        $Resignation.MonthDayOrStartDay = jsonResult.result[0].MonthDayOrStartDay;
                        console.log(jsonResult.result);
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
    loadTypeOfSep: function (dropControl) {
        var msg = [];
        msg.push({ id: 1, name: 'SEPARATION' });
        msg.push({ id: 2, name: 'SUSPENSION' });
        //   msg.push({ id: 3, name: 'RESIGNATION' });
        msg.push({ id: 4, name: 'OTHER REASON' });
        $.each(msg, function (index, sep) {
            $('#' + dropControl.id).append($("<option></option>").val(sep.id).html(sep.name));
        });
    },
    LoadSeparation: function (context) {
        $.ajax({
            url: $app.baseUrl + "Employee/GetSeparationData",
            data: JSON.stringify({ SepCatid: context.CatId, SepEmpId: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $Resignation.RenderData(p);
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
    RenderData: function (data) {
        debugger;
        var formData = document.forms["Resignation"];
        console.log(data);
        debugger;
        $Resignation.SelectedCatId = data.SepCatid;
        $Resignation.SeparationEmpId = data.SepEmpId;

        formData.elements["txtEmpName"].value = data.SepEmpName;
        formData.elements["txtDOJ"].value = data.SepDOJ;
        formData.elements["txtMonthlyLast"].value = data.SepMonthlyLastWorkingDate;
        formData.elements["txtPayrollLast"].value = data.SepPayrollLastWorkingDate;
        formData.elements["txtMonthly"].value = data.SepMonthlyDate;
        formData.elements["txtPayroll"].value = data.SepPayrollDate;


    },
    EditRenderData: function (data) {
        $("#dvemp").removeClass('nodisp');
        $("#dvdetails").removeClass('nodisp');
        var formData = document.forms["Resignation"];
       
        $Resignation.SelectedCatId = data.categoryId;
        $Resignation.SeparationEmpId = data.empid;
        $Resignation.LoadSeparation({ Id: $Resignation.SeparationEmpId, CatId: $Resignation.SelectedCatId });
        $("#dvemp").removeClass('nodisp');
        $("#sltResgCategorylist").hide();
        $("#sltResgEmployeelist").hide();
        $("#sltResgCategorylist").val(data.categoryId);
        $("#sltResgEmployeelist").val(data.empid);
        $("#lblCategory").show();
        $("#lblempcode").show();
        $("#lblCategory").text(data.category);
        $("#lblempcode").text(data.empCode);
  
        formData.elements["txtLastWorkingDate"].value = data.lastworkingDate;
        formData.elements["txtResignationDate"].value = data.resignationDate;
        formData.elements["txtReason"].value = data.reason;

    },
    CheckPayrollProcees: function () {
        debugger;
        if ($("#txtLastWorkingDate").val() == "") {
            alert("Plz select the Last working date");
        }
        else {
            $app.showProgressModel();
            var formData = document.forms["Resignation"];
            var data = {
                SepCatid: formData.elements["sltResgCategorylist"].value,
                SepEmpId: formData.elements["sltResgEmployeelist"].value == "" ? $Resignation.SeparationEmpId : formData.elements["sltResgEmployeelist"].value,
                SepDOJ: formData.elements["txtDOJ"].value,
                SepLWDate: formData.elements["txtLastWorkingDate"].value,
                SepReason: formData.elements["txtReason"].value,
            };
            var months = ["", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            var SplitSDate = data.SepLWDate.split("/");
            var MonthSplit = SplitSDate[1];
            var Month = months.indexOf(MonthSplit);
            var Year = SplitSDate[2];
            var Type = "Single Employee";
            if (Month == "") {
                $app.showAlert('Please select Month', 4);
                return false;
            }
            else {
                $.ajax({
                    url: $app.baseUrl + "Entity/GetPayrollHistory",
                    data: JSON.stringify({
                        selectedId: data.SepEmpId,
                        month: Month,
                        year: Year,
                        type: Type
                    }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {

                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                //for (var i = 0; i < jsonResult.result.length; i++) {
                                //    var result = jsonResult.result[i].status;
                                //    if (result == "Not Process") {
                                $Resignation.save();
                                //    }
                                //    else if (result == "Processed") {
                                //        alert("Save failed! This Month is already Processed");
                                //    }
                                //}
                                break;
                            case false:
                                $Resignation.save();
                                break;
                        }
                    },
                    complete: function () {
                        $app.hideProgressModel();
                    }
                });
            }
        }
    },
    save: function () {
        $app.showProgressModel();
        var formData = document.forms["Resignation"];
        var data = {
            SepCatid: formData.elements["sltResgCategorylist"].value,
            SepEmpId: formData.elements["sltResgEmployeelist"].value == "" ? $Resignation.SeparationEmpId : formData.elements["sltResgEmployeelist"].value,
            SepDOJ: formData.elements["txtDOJ"].value,
            SepLWDate: formData.elements["txtLastWorkingDate"].value,
            SepReason: formData.elements["txtReason"].value,
            SepResgDate: formData.elements["txtResignationDate"].value,
        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveResignation",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddUser').modal('toggle');
                        //$User.LoadUser();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        //alert(jsonResult.Message);
                        var p = jsonResult.result;
                        companyid = 0;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
        //   $companyCom.loadCategory({ id: "sltResgCategorylist" });
        //    $Resignation.loadTypeOfSep({ id: "sltTypeOfSeparation" });
        $("#dvemp").addClass('nodisp');
        $("#dvdetails").addClass('nodisp');
        var table = $('#tblResignationEmployee').DataTable();
        table.ajax.reload();
        $('#AddResignation').modal('toggle');

    },

    BindResignationEmployees: function () {

        $app.applyseletedrow();
        var dtClientList = $('#tblResignationEmployee').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'responsive': true,
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            columns: [
                 { "data": "Id" },
                 { "data": "empid" },
                 { "data": "categoryId" },
                 { "data": "category" },
                 { "data": "empCode" },
                 { "data": "lastworkingDate" },
                 { "data": "resignationDate" },
                 { "data": "reason" },
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
                 "sClass": "nodisp",
                 "bSearchable": false
             },
              {
                  "aTargets": [2],
                  "sClass": "nodisp",
                  "bSearchable": false
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
                  var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                  var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                  b.button();
                  c.button();
                  b.on('click', function () {
                      $('#AddResignation').modal('toggle');
                      $Resignation.EditRenderData(oData);
                      return false;
                  });
                  c.on('click', function () {
                      debugger;
                      $.confirm({
                          title: 'Confirm!',
                          content: 'Do you want delete?',
                          buttons: {
                              YES: function () {
                                  $Resignation.Delete(oData);
                              },
                              NO: function () {
                                  //return false;
                              }

                          }
                      });
                     
                      return false;
                  });
                  $(nTd).empty();
                  $(nTd).append(b,c);

              }
          }


            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetResignationEmployees",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (msg) {
                        debugger;
                        var out = msg.result;
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

                var r = $('#tblResignationEmployee tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblResignationEmployee thead').append(r);
                $('#search_0').css('text-align', 'center');

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

        //var table = $('#tblSeparationEmployee').DataTable();
        //$('#myInput').keyup(function () {
        //    table.search($(this).val()).draw();
        //})

    },
    Delete: function (data) {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteResignationData",
            data: JSON.stringify({ id: data.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert(jsonResult.Message, 1);
                        var dtClientList = $('#tblResignationEmployee').DataTable();
                        dtClientList.ajax.reload();
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

//Close Popup inputs are clear.
$('#AddResignation').on('hidden.bs.modal', function () {
    $("#dvemp").addClass('nodisp');
    $("#dvdetails").addClass('nodisp');
    $("#sltResgCategorylist").show();
    $("#sltResgEmployeelist").show();
    $("#lblCategory").hide();
    $("#lblempcode").hide();
    $(this).find('form').trigger('reset');


})