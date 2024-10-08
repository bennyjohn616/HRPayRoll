$('#btnSaveWeekoffSettings').on('click', function () {

    debugger;

    if ($('#Companyweekoffgrid').hasClass("nodisp")) {          //FOR DATE WISE GRID
        debugger;
        var weekOffDates = [];

        var DatewisegridDataCheck = $("#tblEmpWeekoff").dataTable().fnGetNodes();
        for (i = 0; i < DatewisegridDataCheck.length; i++) {
            //if (($(DatewisegridDataCheck[i]).find(".StatusValues option:selected").val()).toLowerCase() != "working")
            //{
            var tempdays = new Object();
            tempdays.weekoffdate = ($(DatewisegridDataCheck[i]).find(".datevalues").text());
            tempdays.dayname = ($(DatewisegridDataCheck[i]).find(".datename").text());
            tempdays.weekoff = ($(DatewisegridDataCheck[i]).find(".StatusValues option:selected").val());
            weekOffDates.push(tempdays);
            //}
        }
        if (weekOffDates.length != 0) {
            $Weekoff.save(weekOffDates);
        }
    }
    else {   //FOR MONTH WISE GRID

        var weekoffdays = [];
        var CatNameRowsDateCheck = $("#tblWeekoffsetting").dataTable().fnGetNodes();
        for (i = 0; i < CatNameRowsDateCheck.length; i++) {
            if ($(CatNameRowsDateCheck[i]).find("td .WeekValue").val().trim().toLowerCase() != "working") {
                var tempdays = new Object();
                tempdays.weekoff = CatNameRowsDateCheck[i].innerText.trim();
                tempdays.weekType = $(CatNameRowsDateCheck[i]).find("td .WeekValue").val().trim();
                tempdays.weekONE = $(CatNameRowsDateCheck[i]).find("td .FirstWeek").val().trim();
                tempdays.weekTWO = $(CatNameRowsDateCheck[i]).find("td .SecondWeek").val().trim();
                tempdays.weekTHREE = $(CatNameRowsDateCheck[i]).find("td .ThirdWeek").val().trim();
                tempdays.weekFOUR = $(CatNameRowsDateCheck[i]).find("td .FourthWeek").val().trim();
                tempdays.weekFIVE = $(CatNameRowsDateCheck[i]).find("td .FifthWeek").val().trim();
                weekoffdays.push(tempdays);
            }
        }
        if (weekoffdays.length != 0) {
            $Weekoff.save(weekoffdays);
        }
    }
});


$("#ddlWKOFmonth,#txtCutoffFromDate,#txtCutoffToDate,#WeekoffComVal").change(function () {
    debugger;
    var names = [];
    var parameter = $('#lblWeekparam').text();
    var Entryval = $('#lblWeekEntval').text();

    var Gridtype;

    if (parameter != "employeewise" && Entryval != "C") {
        Gridtype = "MG"
    }
    else {
        Gridtype = "DG"
    }
    if (Entryval == "C") {
        if (parameter == "companywise") {
            if ($('#txtCutoffFromDate').val() != "" & $('#txtCutoffToDate').val() != "") {
                if (Gridtype == "DG") {
                    $Weekoff.Loaddatewisegrid(names, "Leave/GetWeekoffExistingcheck");
                }
                else {

                }
                //$Weekoff.weekoffExistingcheck();
            }
        }
        else {
            if ($('#txtCutoffFromDate').val() != "" & $('#txtCutoffToDate').val() != "" & $('#WeekoffComVal').val() != "select") {
                if (Gridtype == "DG") {
                    $Weekoff.Loaddatewisegrid(names, "Leave/GetWeekoffExistingcheck");
                }
                else {

                }
                //$Weekoff.weekoffExistingcheck();
            }

        }

    }
    else if (Entryval == "M") {

        if (parameter == "companywise") {
            if ($('#ddlWKOFmonth').val() != "select") {
                if (Gridtype == "DG") {
                    $Weekoff.Loaddatewisegrid(names, "Leave/GetWeekoffExistingcheck");
                }
                else {
                    $Weekoff.weekoffExistingcheck();
                }
                //$Weekoff.weekoffExistingcheck();
            }

        }
        else {
            if ($('#ddlWKOFmonth').val() != "select" & $('#WeekoffComVal').val() != "select") {
                if (Gridtype == "DG") {
                    $Weekoff.Loaddatewisegrid(names, "Leave/GetWeekoffExistingcheck");
                }
                else {
                      $Weekoff.weekoffExistingcheck();
                }
                // $Weekoff.weekoffExistingcheck();
            }

        }
    }
    else {

        if (parameter == "companywise") {
            if (Gridtype == "DG") {
                $Weekoff.Loaddatewisegrid(names, "Leave/GetWeekoffExistingcheck");
            }
            else {
                // $Weekoff.weekoffExistingcheck();
            }
        }
        else {
            if (Gridtype == "DG") {
                $Weekoff.Loaddatewisegrid(names, "Leave/GetWeekoffExistingcheck");
            }
            else {
                $Weekoff.weekoffExistingcheck();
            }
        }
    }
});




$('#btnweekoffGetdates').on('click', function () {
    debugger;
    var checked = $("input[type=checkbox]:checked").length;

    if (!checked) {
        alert("You must check at least one checkbox.");
        return false;
    }
    else {
        var names = [];
        $('#chkboxdiv input:checked').each(function () {
            names.push(this.name);
        });
    }
    var fromdate = $('#txtCutoffFromDate').val();
    var Todate = $('#txtCutoffToDate').val();
    $Weekoff.Loaddatewisegrid(names, "Leave/GetDatesforweekoff");
});

$("day-control select").on('change', function () {
    alert("Works");
})



var $Weekoff = {


    noneditablerender: function (gridvalues) {
        var WeekoffDAYS = ["Working", "Full", "Half", "Varible"];
        var DAYS = ["Working", "Full", "Half"];

        var dtClientList = $("#tblWeekoffsetting").DataTable({

            columns: [
                null,
                {
                    "className": 'details-control',
                    render: function (d, t, r) {

                        var $select = $("<select></select>", {
                            "id": r[0],
                            "class": "WeekValue",
                            "value": d,
                            "onchange": "$Weekoff.WeekoffSetting(this)"
                        });
                        $.each(WeekoffDAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {

                        var $select = $("<select></select>", {
                            "id": r[0] + "Frstart",
                            "class": "FirstWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {
                        var $select = $("<select></select>", {
                            "id": r[0] + "Sstart",
                            "class": "SecondWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {

                        var $select = $("<select></select>", {
                            "id": r[0] + "Tstart",
                            "class": "ThirdWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {
                        var $select = $("<select></select>", {
                            "id": r[0] + "Fstart",
                            "class": "FourthWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {
                        var $select = $("<select></select>", {
                            "id": r[0] + "Fvstart",
                            "class": "FifthWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                }

            ],
            //"aoColumnDefs": [
            //    { "bSortable": false, "aTargets": [  ] }, 
            //    { "bSearchable": false, "aTargets": [ 0, 1, 2, 3,4,5 ] }
            //                ],
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,

            scroller: {
                loadingIndicator: true
            }

        });








    },

    render: function () {

        $.ajax({
            url: $app.baseUrl + "Leave/GetWeekoffsetting",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
                        for (var j = 0; j < p.length; j++) {
                            $("#" + p[j].dayname).prop("checked", true);
                        }
                        break;
                    case false:
                        //$app.hideProgressModel();
                        //$app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }

        });

        var WeekoffDAYS = ["Working", "Full", "Half", "Varible"];
        var DAYS = ["Working", "Full", "Half"];


        var dtClientList = $("#tblWeekoffsetting").DataTable({

            columns: [
                null,
                {
                    "className": 'details-control',
                    render: function (d, t, r) {

                        var $select = $("<select></select>", {
                            "id": r[0],
                            "class": "WeekValue",
                            "value": d,
                            "onchange": "$Weekoff.WeekoffSetting(this)"
                        });
                        $.each(WeekoffDAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {

                        var $select = $("<select></select>", {
                            "id": r[0] + "Frstart",
                            "class": "FirstWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {
                        var $select = $("<select></select>", {
                            "id": r[0] + "Sstart",
                            "class": "SecondWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {

                        var $select = $("<select></select>", {
                            "id": r[0] + "Tstart",
                            "class": "ThirdWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {
                        var $select = $("<select></select>", {
                            "id": r[0] + "Fstart",
                            "class": "FourthWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                },
                {
                    render: function (d, t, r) {
                        var $select = $("<select></select>", {
                            "id": r[0] + "Fvstart",
                            "class": "FifthWeek",
                            "value": d,
                            "disabled": "disabled"
                        });
                        $.each(DAYS, function (k, v) {
                            var $option = $("<option></option>", {
                                "text": v,
                                "value": v
                            });
                            if (d === v) {
                                $option.attr("selected", "selected")
                            }
                            $select.append($option);
                        });
                        return $select.prop("outerHTML");
                    }
                }

            ],
            //"aoColumnDefs": [
            //    { "bSortable": false, "aTargets": [  ] }, 
            //    { "bSearchable": false, "aTargets": [ 0, 1, 2, 3,4,5 ] }
            //                ],
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,

            scroller: {
                loadingIndicator: true
            }

        });
        $('#tblWeekoffsetting tbody').on('click', 'td.details-control', function () {
            var tr = $(this).closest('tr');
            var row = dtClientList.row(tr);
        });


    },

    WeekoffSetting: function (data) {
        debugger;
        if (data.value == "Varible") {
            $('#' + data.id + 'Frstart,#' + data.id + 'Sstart,#' + data.id + 'Tstart,#' + data.id + 'Fstart,#' + data.id + 'Fvstart').prop('disabled', false);
            return true;
        }
        else {
            $('#' + data.id + 'Frstart,#' + data.id + 'Sstart,#' + data.id + 'Tstart,#' + data.id + 'Fstart,#' + data.id + 'Fvstart').val(data.value);
            $('#' + data.id + 'Frstart,#' + data.id + 'Sstart,#' + data.id + 'Tstart,#' + data.id + 'Fstart,#' + data.id + 'Fvstart').prop('disabled', true);
        }

    },

    Loaddatewisegrid: function (names, URL) {
        debugger;
        $('#btnweekoffGetdates').removeClass('nodisp');
        var dtClientList = $('#tblEmpWeekoff').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            "oLanguage": {
                "sEmptyTable": "No Data Avaliable"

            },
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    {
                        "data": "dates",

                        render: function (data) {

                            var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                            var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                            return dateF;

                        }

                    },
                    { "data": "datesname" },
                    { "data": null }
            ],
            "aoColumnDefs": [

         {
             "aTargets": [0],
             "sClass": "word-wrap datevalues"

         },
         {
             "aTargets": [1],
             "sClass": "word-wrap datename"

         },

      {
          "aTargets": [2],
          "sClass": "word-wrap StatusValues",

          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              debugger;
              var b;
              if (oData.Weekoff == "") {
                  b = $("<select id= \"daystatus\"class = \"StatusValue\"><option value=\"Full\">Full</option><option value=\"Half\">Half</option><option value=\"Working\">Working</option></select>");
                  $('#btnSaveWeekoffSettings').removeClass('nodisp');
                  $('#btnweekoffGetdates').removeClass('nodisp');
              }
              else {
                  b = oData.Weekoff;
                  $('.' + oData.datesname).prop('checked', true);
                  $('#btnSaveWeekoffSettings').addClass('nodisp');
                  $('#btnweekoffGetdates').addClass('nodisp');
              }



              $(nTd).empty();
              $(nTd).prepend(b);
          }
      }
            ],
            ajax: function (data, callback, settings) {
                var Commonvalues = {
                    CutoffFrom: $("#txtCutoffFromDate").val(),
                    CutoffTo: $("#txtCutoffToDate").val(),
                    Month: $("#ddlWKOFmonth").val(),
                    ComponentName: $("#WeekoffCom").val(),
                    ComponentValue: $("#WeekoffComVal").val(),
                };
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + URL,
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ weekdaynames: names, CommonDataValues: Commonvalues }),
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
                        var out = jsonResult.result;
                        if (jsonResult.result.length != 0) {
                            if (out.Weekoff == "") {
                                $('.Sunday').prop('checked', false);
                                $('.Monday').prop('checked', false);
                                $('.Tuesday').prop('checked', false);
                                $('.Wednesday').prop('checked', false);
                                $('.Thursday').prop('checked', false);
                                $('.Friday').prop('checked', false);
                                $('.Saturday').prop('checked', false);
                            }
                        }
                        else {
                            $('.Sunday').prop('checked', false);
                            $('.Monday').prop('checked', false);
                            $('.Tuesday').prop('checked', false);
                            $('.Wednesday').prop('checked', false);
                            $('.Thursday').prop('checked', false);
                            $('.Friday').prop('checked', false);
                            $('.Saturday').prop('checked', false);
                        }
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
    checkvalues: function () {


        var checkedValue = $('.weekoff:checked').val();
        var checkboxes = document.getElementsByName('Weekoffday');

        var numberofcheckedValue = checkboxes.length;
        var s1 = [];
        var row = [];
        for (var i = 0, n = checkboxes.length; i < n; i++) {
            if (checkboxes[i].checked) {
                var day = new Object();
                day.dayname = checkboxes[i].value;
                row.push(day);
            }
        }
        if (row.length == 0) {
            $app.showAlert('Please Select the Weekoff Day!', 4);
        }
        else {
            $Weekoff.save(row);
        }

        //var numberofcheckedValue = $('[name="Weekoffday"]:checked').length;

    },


    loadSelect: function (Name) {
        debugger;
        //Name = Name.replace(/ +/g, "");

        //var data = $payslipMatching.Data;
        //var select = '<select id="dataid" </option>'
        ////var select = '<select id="' + Name + '" onchange="$payslipMatching.alreadyMapped(this.id)"><option value="00000000-0000-0000-0000-000000000000">--select--</option>'

        //var WeekoffDAYS = ["Working", "Full", "Half"];

        //    for (var cnt = 0; cnt < WeekoffDAYS.length; cnt++) {
        //    debugger;
        //    var sel = "";
        //    if (WeekoffDAYS[cnt] == Name.Weekoff) {
        //        sel = "selected";
        //    }
        //    select = select + '<option value="' + WeekoffDAYS[cnt] + '">' + WeekoffDAYS[cnt]+ sel + '</option>'
        //     }

        //select = select + '</select>';
        return Name.Weekoff;
    },

    save: function (weekoffday) {
        var CommonObject = {
            CutoffFrom: $("#txtCutoffFromDate").val(),
            CutoffTo: $("#txtCutoffToDate").val(),
            Month: $("#ddlWKOFmonth").val(),
            ComponentName: $("#WeekoffCom").val(),
            ComponentValue: $("#WeekoffComVal").val(),
        };




        $.ajax({
            url: $app.baseUrl + "Leave/SaveWeekoffsetting",
            data: JSON.stringify({ dataValue: weekoffday, CommonDatas: CommonObject }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddHoliday').modal('toggle');
                        //$Holiday.LoadHoliday();
                        //$app.hideProgressModel();
                        debugger
                        $('#btnSaveWeekoffSettings').addClass('nodisp');
                        $('#btnweekoffGetdates').addClass('nodisp');
                        $app.showAlert(jsonResult.Message, 2);
                        //var p = jsonResult.result;
                        //companyid = 0;
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



    weekoffExistingcheck: function () {
        debugger
        $("#btnSaveWeekoffSettings").show();
        var names = [];
        var Commonvalues = {
            CutoffFrom: $("#txtCutoffFromDate").val(),
            CutoffTo: $("#txtCutoffToDate").val(),
            Month: $("#ddlWKOFmonth").val(),
            ComponentName: $("#WeekoffCom").val(),
            ComponentValue: $("#WeekoffComVal").val(),
        };
        $.ajax({
            url: $app.baseUrl + "Leave/GetWeekoffExistingcheck",
            data: JSON.stringify({ weekdaynames: names, CommonDataValues: Commonvalues }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        var out = jsonResult.result;
                        if (jsonResult.Message == "Data already Existing!!!") {
                            $("#btnSaveWeekoffSettings").hide();
                            $Weekoff.existValueRender(out);
                        }
                        else {
                            $Weekoff.noneditablerender(out);
                        }

                        break;
                    case false:
                        debugger;
                        $Weekoff.render();
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    existValueRender: function (gridvalues) {
        debugger;
        $('#tblWeekoffsetting').DataTable({
            data: gridvalues,         
            "columns" : [
           { "data": "weekoff" },
           { "data": "weekType" },
           { "data": "weekONE" },
           { "data": "weekTWO" },
           { "data": "weekTHREE" },
           { "data": "weekFOUR" },
           { "data": "weekFIVE" }
            ],
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    }
}