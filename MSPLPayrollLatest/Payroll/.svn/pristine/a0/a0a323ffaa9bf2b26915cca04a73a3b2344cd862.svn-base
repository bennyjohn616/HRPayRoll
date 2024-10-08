
$("#ddlLTSLeaveType").change(function () {
    debugger;
    var RowsDateCheck = $("#tblLeavetypesettings").dataTable().fnGetNodes();
    for (i = 0; i < RowsDateCheck.length; i++) {
        if ($(RowsDateCheck[i]).find("td:nth-child(2)").html().trim().toLowerCase() == $("#ddlLTSLeaveType option:selected").text().trim().toLowerCase()) {
            $app.showAlert("Already Exist " + $("#ddlLTSLeaveType option:selected").text()+",For Updation please delete the existing settings!!!", 4);
            $("#ddlLTSLeaveType").val("00000000-0000-0000-0000-000000000000");
            $("#ddlLTSLeaveType").focus();
            return false;
        }
    }
});
var $leaveSettings = {

    LoadLeaveTypesettings: function () {
        debugger;
        var dtMothlyLeaveLimit = $('#tblLeavetypesettings').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            "oLanguage": {
                "sEmptyTable": "No Data Avaliable"
            },
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "Id" },
                    { "data": "leaveparameter" },
                    { "data": "LeaveTypeDesc" },
                    { "data": "LevopenReq" },
                    { "data": "LevEncashAvail" },
                    { "data": "usedlevName" },
                    { "data": "LevTypeActive" },
                    { "data": null },

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
              "sClass": "word-wrap"

          },
            {
             "aTargets": [7],
            "sClass": "actionColumn",

            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                c.button();
                c.on('click', function () {
                    if (confirm('Are you sure ,do you want to delete?')) {
                        $leaveSettings.DeleteleavetypesettingsData(oData);
                    }
                    return false;
                });
                $(nTd).empty();
                $(nTd).prepend(c);
            }
        }
            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetLeaveTypeSettingsSave",
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
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    Checkleavemastersettingexisting: function (dropControl) {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Leave/getleavemastersettings",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
                success: function (jsonResult) {
                    var p = jsonResult.result;
                    if (dropControl.id1 == "Compoffalert") {
                        var date = new Date(parseInt(parseInt(p[0].FinYearEnd.replace(/(^.*\()|([+-].*$)/g, ''))));
                        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                        $("#dtpLVD").val(dateF);
                    }
                    debugger;
                    switch (jsonResult.Status) {

                    case true:
                        $('#' + dropControl.id1).addClass("nodisp");
                        $('#' + dropControl.id2).removeClass("nodisp");
                        break;
                    case false:
                        $('#' + dropControl.id1).removeClass("nodisp");
                        $('#' + dropControl.id2).addClass("nodisp");
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    DeleteleavetypesettingsData: function (context) {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Leave/DeleteLeaveTypeSetting",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:                       
                        $app.showAlert("Leave Type setting Deleted succesfully", 2);
                        var table = $("#tblLeavetypesettings").DataTable();
                        table.ajax.reload();
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
    CheckLeaveMasterSettings: function () {
        debugger;
        var leavemastersaveflagMax = true;
        var leavemastersaveflagMin = true;
    var checkval=$("#ddlminmaxval").val();
    if (checkval == "C")
    {
        if ($('#IdMaxValue').prop('checked')) {
            leavemastersaveflagMax = false;
            if ($("#txtMaxValue").val() != "") {
                if (parseFloat($("#txtMaxValue").val()) != 0) {
                    if ($("#txtMaxTimes").val() != "")
                    {
                        if (parseFloat($("#txtMaxTimes").val()) != 0) {
                            if ($("#ddlMaxsetperiod").val() == 0) {
                                leavemastersaveflagMax = false;
                                $app.showAlert("Please select Maximum setting period", 4);
                            }
                            else {
                                if ($("#ddlMaxsetperiod").val() == "M") {
                                    if ($("#ddlMaxdeviation").val() == 0) {
                                        $app.showAlert("Please select Allow deviation in previous months quota not used ", 4);
                                    }
                                    else {
                                        leavemastersaveflagMax = true;
                                    }
                                }
                                else {
                                    leavemastersaveflagMax = true;
                                }
                            }
                        }
                        else {
                            leavemastersaveflagMax = false;
                            $app.showAlert("Maximun : Maximum Times Allowed value cannot be Zero(0)", 4);
                            $("#txtMaxTimes").val("");
                        }
                    }
                    else {
                        if ($("#ddlMaxsetperiod").val() == 0 && $("#ddlMaxdeviation").val() == 0)
                        {
                            leavemastersaveflagMax = true;
                        }
                        else {
                            
                            $app.showAlert("You can't select Maximum setting period and deviations with out entering Maximum Times Allowed", 4);
                            leavemastersaveflagMax = false;
                        }
                      
                    }
                }
                else {
                    leavemastersaveflagMax = false;
                    $app.showAlert("Maximun No of Days can be taken at a time value cannot be Zero(0)", 4);
                    $("#txtMaxValue").val("");
                }
            }
            else {
                leavemastersaveflagMax = false;
                $app.showAlert("Please enter Maximun No of Days can be taken at a time", 4);
            }

        }


        if ($('#IdMinValue').prop('checked')) {
            leavemastersaveflagMin = false;
            if ($("#txtMinValue").val() != "") {
                if (parseFloat($("#txtMinValue").val()) != 0) {
                    if ($("#txtMinTimes").val() != "") {
                        if (parseFloat($("#txtMinTimes").val()) != 0) {
                            if ($("#ddlMinsetperiod").val() == 0) {
                                leavemastersaveflagMin = false;
                                $app.showAlert("Please select Minimum setting period", 4);
                            }
                            else {
                                if ($("#ddlMinsetperiod").val() == "M") {
                                    if ($("#ddlMindeviation").val() == 0) {
                                        $app.showAlert("Please select Allow deviation in previous months quota not used ", 4);
                                    }
                                    else {
                                        leavemastersaveflagMin = true;
                                    }
                                }
                                else {
                                    leavemastersaveflagMin = true;
                                }
                            }
                        }
                        else {
                            leavemastersaveflagMin = false;
                            $app.showAlert("Minimum : Maximum Times Allowed value cannot be Zero(0)", 4);
                            $("#txtMinTimes").val("");
                        }
                    }
                    else {
                        if ($("#ddlMinsetperiod").val() != 0 && $("#ddlMindeviation").val() != 0) {
                            $app.showAlert("You can't select Maximum setting period and deviations with out entering Maximum Times Allowed", 4);
                            leavemastersaveflagMin = false;
                        }
                        else {
                            leavemastersaveflagMin = true;
                        }
                    }
                }
                else {
                    leavemastersaveflagMin = false;
                    $app.showAlert("Maximun No of Days can be taken at a time value cannot be Zero(0)", 4);
                    $("#txtMinValue").val("");
                }
            }
            else {
                leavemastersaveflagMin = false;
                $app.showAlert("Please enter Maximun No of Days can be taken at a time", 4);
            }
        }



        if (leavemastersaveflagMin == true && leavemastersaveflagMax == true) {
            $leaveSettings.SaveLeaveMasterSettings();
        }
    }
    else
    {
        $leaveSettings.SaveLeaveMasterSettings();
    }
    },
    SaveLeaveMasterSettings: function () {
        debugger;
        $app.showProgressModel();
        var formData = {
            leaveparameter: $("#ddlLevSet").val(),
            Holidayparameter: $("#ddlHolSet").val(),
            Compoffparameter: $("#ddlCPoffSet").val(),
            Weekoffparameter: $("#ddlWKoffSet").val(),
            Weekoffentryvalid: $("#ddlWKoffEntrySet").val(),
            Minormaxparameter: $("#ddlminmaxval").val(),
            mindays: $("#txtMinValue").val(),
            maxmintimes: $("#txtMinTimes").val(),
            minperiod: $("#ddlMinsetperiod").val(),
            mindeviation: $("#ddlMindeviation").val(),
            maxdays: $("#txtMaxValue").val(),
            maxmaxtimes: $("#txtMaxTimes").val(),
            maxperiod: $("#ddlMaxsetperiod").val(),
            maxdeviation: $("#ddlMaxdeviation").val(),
            RpComp1: $("#ddlCOMP1").val(),
            RpComp2: $("#ddlCOMP2").val(),
            RpComp3: $("#ddlCOMP3").val(),
            RpComp4: $("#ddlCOMP4").val(),
            RpComp5: $("#ddlCOMP5").val(),
            leavecreditparameter: $("#ddlLevCreditSet").val(),
            encashmentparameter: $("#ddlEncashmentSet").val(),
        };
        $.ajax({
            url: $app.baseUrl + "Leave/leavemastersettingssave",
            data: JSON.stringify({ LeaveSettingData: formData }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
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
    },
    SaveLeaveTypeSettings: function () {
        debugger;
        $app.showProgressModel();
        var formData = {
            LeaveTypeId: $("#ddlLTSLeaveType").val(),
            LeaveTypeDesc: $("#txtLTSLeaveTypeDesc").val(),
            LevopenReq: $("#ddlLTSOpnReq").val(),
            LevEncashAvail: $("#ddlLTSEncashavail").val(),
            //Openingbal: $("#ddlLTSOBPayrollMatch").val(),
            //avalbal: $("#ddlLTSCBPayrollMatch").val(),
            usedlev: $("#ddlLTSULPayrollMatch").val(),
            LevTypeActive: $("#ddlLTSActive").val(),
        };
        $.ajax({
            url: $app.baseUrl + "Leave/LeaveTypeSettingsSave",
            data: JSON.stringify({ LeaveTypeData: formData }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $('#AddLeavetypesettings').modal('toggle');
                        $app.showAlert(jsonResult.Message, 2);
                        $leaveSettings.LoadLeaveTypesettings();
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
    SaveLeaveConfiguration: function () {
        debugger;
        $app.showProgressModel();
        var com;
        var compval;
        var formData = {
            LeaveTypeId: $("#ddLCLeaveType").val(),
            DynamicComponentName: $("#lblLCdynamic").text(),
            DynamicComponentValue: $("#ddlLCDynamic").val(),
            ConfigEffectiveDate: $("#DPLCEffdate").val(),
            MaxDayMonth: $("#txtLCMaxMonthDays").val(),
            AllowDevisionMonth: $("#ddlLCMonthDeviation").val(),
            overallMax: $("#txtLCMaxdays").val(),
            carryLimit: $("#txtLCCaryFwrdLmt").val(),
            InvHoliday: $("#ddlLCInterVening").val(),
            InvHolidaysubparameter: $("#ddlIntervApplic").val(),
            Compoffallow: $("#ddlLcCompoffAllow").val(),
            Isattachreq:$("#ddlLCAttachmentreq").val(),
            Attachreqmaxdays:$("#txtLCAttachreqdays").val(),
            mindays:$("#txtPMinValue").val(),
            maxmintimes:$("#txtPMinTimes").val(),
            minperiod:$("#ddlPMinsetperiod").val(),
            mindeviation:$("#ddlPMindeviation").val(),
            maxdays:$("#txtPMaxValue").val(),
            maxmaxtimes:$("#txtPMaxTimes").val(),
            maxperiod:$("#ddlPMaxsetperiod").val(),
            maxdeviation: $("#ddlPMaxdeviation").val(),

        }; 
        $.ajax({
           
            url: $app.baseUrl + "Leave/LeaveConfigurationSave",
            data: JSON.stringify({ LeaveConfigData: formData }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                     //   $increment.loadIncMaster();
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
    SaveLeaveEncashment: function () {
        debugger;
        $app.showProgressModel();
       
        var formData = {
            LeaveTypeId:   $("#ddlEncashlevtype").val(),
            Encashparameter: $("#ddlencashmentParameter").val(),
            EncashLimit: $("#txtEncashLimit").val(),
            Encashcomponent: $("#ddlEncashcomp").val(),
        };
        $.ajax({

            url: $app.baseUrl + "Leave/LeaveEncashmentsave",
            data: JSON.stringify({ LeaveEncashment: formData }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                      //  $increment.loadIncMaster();
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
    CheckSaveLeaveEncashment: function () {
        debugger;
        $app.showProgressModel();

       
        $.ajax({

            url: $app.baseUrl + "Leave/GetLeaveEncashment",
            data: JSON.stringify({EncashComp:$("#ddlencashmentParameter").val(), LeaveTypeId: $("#ddlEncashlevtype").val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        var p = jsonResult.result;
                        $("#ddlEncashlevtype").val(p[0].LeaveTypeId);
                        $("#ddlEncashcomp").val(p[0].Encashcomponent);
                        $("#txtEncashLimit").val(p[0].EncashLimit);
                        $app.showAlert("Existing data Retrived", 3);
                       
                        break;
                    case false:
                        $app.hideProgressModel();
                        
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },
    AddInitialize: function () {

        var formData = document.forms["frmLeaveTypeSettings"];

        $("#ddlLTSLeaveType").val("00000000-0000-0000-0000-000000000000");
        $("#txtLTSLeaveTypeDesc").val("");
        $("#ddlLTSOpnReq").val("select");
        $("#ddlLTSEncashavail").val("select");
        $("#ddlLTSULPayrollMatch").val("select");
        $("#ddlLTSActive").val("Y");
    },
}