//$("#sltSetting").change(function () {

//    if ($("#sltSetting option:selected").text() == "Employee Code Segment")
//    {
//        if($("input[id$='_PrefixEmpCode']").val().trim()!="")
//            $('#btnSave').prop('disabled', true);
//    }
//    else {
//        $('#btnSave').prop('disabled', false);
//    }

//});








$setting = {
    categories: null,
    selectedEntityModelId: null,
    selectedEntityModelname: null,
    selectedSetting: '',
    PaysheetSettingId: '',
    PaysheetLockId: '',
    loadInitial: function () {
        $setting.pfEsiSetting.renderForm();
        $setting.bindEvent();
        var options = '';
        for (i = 1; i <= 31; i++) {
            options += '<option value="' + i + '">' + i + '</option>';
        }
        $('#DDdays').append(options);
    },
    bindEvent: function () {
        $('#settingTab a').on("click", function (e) {
            switch (e.target.id) {
                case "setPFESI":
                    $setting.pfEsiSetting.renderForm();
                    break;
                case "setStatutory":
                    $setting.pfEsiStatutorySetting.loadcategory();
                    break;
                case "setEmpCode":
                    $Empcodesetting.loadInitial();
                    break;
                default:
                    break;
            }
        });
    },

    LoadEntityModelDrop: function () {
        $setting.LoadSetting({ id: 'sltSetting' });
    },
    LoadSetting: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Setting/GetSetting",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                $.each(msg, function (index, Setting) {
                    $('#' + dropControl.id).append($("<option></option>").val(Setting.Id).html(Setting.DisplayAs));
                });
            },
            error: function (msg) {
            }
        });
        $('#' + dropControl.id).change(function () {
            if ($('#' + dropControl.id).val() == 0) {
                // $("#dvSetting").html('');
                $("#renderId").html('');
                $("#dvTitle").html('');

            }
            else {
                $setting.pfEsiSetting.renderForm();
                //$setting.selectedEntityModelname = $('#' + dropControl.id).find('option:selected').text();
                //$setting.selectedEntityModelId = $('#' + dropControl.id).val();
                //$setting.selectedSetting = $("#sltSetting").val();
                //$setting.GetSettingEntity({ Id: $setting.selectedSetting });
                //$("#dvTitle").html('<h2>' + $setting.selectedEntityModelname + '</h2>');
            }
        });
    },
    GetSettingEntity: function (context) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Setting/GetSettingForm",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $setting.RenderForm(out);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },

    popuplockrelease: function () {
        $("#ddLockMonth").val('0'),
        $("#txtPaysheetLock").val(''),
        $("#txtPaysheetAdminPass").val('')
        $('#chkPaysheetLock').prop('checked', false);
    },
    savelockrelease: function () {
        var err = 0;
        $(".Reqrd").each(function () {
            if (document.getElementById(this.id).value == "0") {
                $app.showAlert(this.id == "ddLockMonth" ? 'Please Select paysheet month' : '', 2);
                err = 1;
                return false;
            }
            else if (this.id == "txtPaysheetLock" || this.id == "txtPaysheetAdminPass") {
                if (document.getElementById(this.id).value == "") {
                    $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                    err = 1;
                    return false;
                }
            }
        });
        if (err == 1) {
            $app.hideProgressModel();
            return false;
        }
        var PLPaySheetLockid = $setting.PaysheetLockId == "" ? '00000000-0000-0000-0000-000000000000' : $setting.PaysheetLockId;
        var PLPayrollMonth = $("#ddLockMonth").val();
        var PLPayrollYear = $("#txtPaysheetLock").val();
        var PLPayrollLock = $("#chkPaysheetLock").is(":checked");
        var PLAdminPassword = $("#txtPaysheetAdminPass").val();
        $.ajax({
            url: $app.baseUrl + "Setting/SavelockReleaseSetting",
            data: JSON.stringify({ PaySheetLockid: PLPaySheetLockid, AdminPassword: PLAdminPassword, PayrollMonth: PLPayrollMonth, PayrollYear: PLPayrollYear, PayrollLock: PLPayrollLock }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        if (jsonResult.result == "Update") {
                            $app.showAlert(jsonResult.Message, 1);
                        }
                        else {
                            $app.showAlert(jsonResult.Message, 2);
                        }
                        $('#LockandRelease').modal('toggle');
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
    RenderForm: function (data) {

        var returnval = '<form role="form" id="frm_' + $setting.selectedSetting + '"> '
            + '<div class="col-md-7">'
                   + '<div class="form-horizontal" id="dv_0"> </div>'

        returnval = returnval + ' <div class="form-group">'
           + ' <button type="submit" id="btnSave" class="btn custom-button">'
              + '  Save</button>'
       + ' </div> </div>'
        returnval = returnval + '</form>'
        $('#renderId').html(returnval);
        $("#frm_" + $setting.selectedSetting).on('submit', function (event) {
            if ($app.requiredValidate('frm_' + $setting.selectedSetting, event)) {
                $setting.save({ id: 'frm_' + $setting.selectedSetting, SettingId: $setting.selectedSetting });
                return false;
            }
            else {
                return false;
            }
        });
        $setting.addControl(data, 0);

    },
    addControl: function (data, parentId) {
        for (var cnt = 0; cnt < data.length; cnt++) {
            if (data[cnt].ParentId != parentId) {
                continue;
            }
            if ($('#dv_' + data[cnt].Id).length > 0) {
                continue;
            }
            var req = 'required'
            var temp = '';
            if (data[cnt].ControlType == "text") {
                var setValue = (data[cnt].SettingValue.Value == null) ? data[cnt].Value : data[cnt].SettingValue.Value;
                temp = temp + '<div class="form-group" id="dv_' + data[cnt].Id + '">';
                temp = temp + ' <label class="control-label col-md-5">' + data[cnt].DisplayAs + '</label> <div class="col-md-5">';
                temp = temp + '<input type="' + data[cnt].ControlType + '" class="form-control"  id="' + data[cnt].Id + '_' + data[cnt].Name
                 + '" value="' + setValue + '" placeholder="' + data[cnt].DisplayAs + '" ' + req + '> </div> </div>'
            }
            else if (data[cnt].ControlType == "dropdown") {
                temp = temp + '<div class="form-group" id="dv_' + data[cnt].Id + '">';
                temp = temp + ' <label class="control-label col-md-5">' + data[cnt].DisplayAs + '</label> <div class="col-md-5">';
                var cntName = data[cnt].Id + '_' + data[cnt].Name;
                temp = temp + '<select id="' + cntName + '" class="form-control">{options}</select> </div> </div>'
                var msg = data[cnt].SettingDropDown;
                var options = '';
                $.each(msg, function (index, blood) {
                    options = options + '<option value="' + blood.id + '">' + blood.name + '</option>';
                });
                temp = temp.replace('{options}', options);
            }
            else if (data[cnt].ControlType == "radio") {
                var setValue = (data[cnt].SettingValue.Value == null) ? data[cnt].Value : data[cnt].SettingValue.Value;
                setValue = setValue.toUpperCase();
                var cheked = '';
                if (setValue == 'TRUE' || setValue == 'ON') {
                    cheked = 'checked';
                }
                temp = temp + '<div class="form-group" id="dv_' + data[cnt].Id + '">';
                temp = temp + ' <label class="control-label col-md-5">' + data[cnt].DisplayAs + '</label> <div class="col-md-5">';
                temp = temp + '<input type="radio" id="' + data[cnt].Id + '_' + data[cnt].Name + '" name="' + data[cnt].RadioGroupName + '" ' + cheked + ' /></div></div>'
            }
            else if (data[cnt].ControlType == "panel") {
                temp = temp + '<div class="form-group" >';
                temp = temp + '<div class="panel panel-primary"> <div class="panel-heading"><h3 class="panel-title"> ' + data[cnt].DisplayAs + '</h3></div>';
                temp = temp + ' <div class="panel-body" id="dv_' + data[cnt].Id + '"> </div> </div>';
            }
            else if (data[cnt].ControlType == "lbl") {
                temp = temp + '<div class="form-group" id="dv_' + data[cnt].Id + '"> <label class="control-label col-md-5">' + data[cnt].DisplayAs + '</label></div>'
            }
            $('#dv_' + parentId).append(temp);
            var chi = [];
            $.each(data, function (ind, obj) {
                if (obj.ParentId == data[cnt].Id) {
                    chi.push(obj);
                }
            });
            if (chi.length > 0) {
                $setting.addControl(chi, data[cnt].Id);
            }
            //  formelemnt = formelemnt + temp;
        }
    },
    // prevElemt: null,
    save: function (context) {
        $app.showProgressModel();
        var keyvalues = [];
        $("form#" + context.id + " :input").each(function () {
            var input = $(this); // This is the jquery object of the input, do what you will
            if (input[0].tagName == "BUTTON") {
                // continue;
            }
            else {
                if (input[0].type == 'text' || input[0].type == 'radio' || input[0].type == 'select-one') {
                    var idVal = input[0].id;
                    if (input[0].type == 'radio') {
                        var radioVal = $('#' + idVal).prop('checked') == true ? 'TRUE' : 'FALSE';
                        keyvalues.push({ settingDefid: idVal, value: radioVal, settingid: context.SettingId });
                    }
                    else {
                        keyvalues.push({ settingDefid: idVal, value: $('#' + idVal).val(), settingid: context.SettingId });
                    }
                }
            }
        });
        var formdata = {
            settingId: context.id,
            settingKeyValues: keyvalues
        };

        $.ajax({
            url: $app.baseUrl + "Setting/SaveSetting",
            data: JSON.stringify({ dataValue: formdata }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.clearControlValues(context.id);
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $setting.selectedSetting = '';
                        $("#sltSetting").val(0);
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
    deleteEntity: function (data) {
        $.ajax({
            url: $app.baseUrl + "Entity/DeleteEntity",
            data: JSON.stringify({ id: data.Id, entityModelId: $setting.selectedEntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        //   $setting.LoadGridData();
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
    loadRelatedDropDown: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetLanguages",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.id).html(blood.name));
                });
            },
            error: function (msg) {
            }
        });

    },
    pfEsiSetting: {
        formName: 'frm_pfEsi',
        formData: document.forms['frm_pfEsi'],
        renderForm: function () {
            $setting.pfEsiSetting.bindEvent();
            $.ajax({
                url: $app.baseUrl + "Setting/GetCompSetting",
                data: null,
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    switch (jsonResult.Status) {
                        case true:
                            $app.hideProgressModel();
                            $setting.pfEsiSetting.assignValueControl(jsonResult.result);
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
        assignValueControl: function (datas) {
            debugger;
            var tmp = datas[1];
            $($setting.pfEsiSetting.formData).find('#sltPayrollProcess').html('');
            $.each(tmp, function (int, item) {
                $($setting.pfEsiSetting.formData).find('#sltPayrollProcess').append($("<option></option>").val(item).html(item));
            });
            var data = datas[0];
            $($setting.pfEsiSetting.formData).find('#txtPFBankAddress').val(data.PFBankAddress);
            $($setting.pfEsiSetting.formData).find('#txtPFBankName').val(data.PFBankName),
            $($setting.pfEsiSetting.formData).find('#txtGroupCode').val(data.GroupCode),
            $($setting.pfEsiSetting.formData).find('#txtPFEmployerCode').val(data.PFEmployeerCode),
            //$($setting.pfEsiSetting.formData).find('#txtPensionFundAc').val(data.PensionFundAcNo),
            //$($setting.pfEsiSetting.formData).find('#txtEPFAc').val(data.EPFAcNo),
            $($setting.pfEsiSetting.formData).find('#txtAdminChargeAc').val(data.AdminChargeAcNo),
            $($setting.pfEsiSetting.formData).find('#txtInspectionChargesAc').val(data.InspectionChargeAcNo),
            $($setting.pfEsiSetting.formData).find('#txtEDLIAc').val(data.EDLIAcNo),
            //$($setting.pfEsiSetting.formData).find('#txtESIEmployersContribution').val(data.ESIEmployeerContribution),
            $($setting.pfEsiSetting.formData).find('#sltPayrollProcess').val(data.PayrollProcessBy),
            $("#serviceYearMonth").val(data.ServiceYearMonth)
            if (data.VPFProjectionRequired == true) {
                $($setting.pfEsiSetting.formData).find('#chkbVPFProjReq').prop('checked', true);
                $($setting.pfEsiSetting.formData).find('#chkbVPFProjection').prop('disabled', false);
            }
            else {
                $($setting.pfEsiSetting.formData).find('#chkbVPFProjReq').prop('checked', false);
                $($setting.pfEsiSetting.formData).find('#chkbVPFProjection').prop('disabled', true);

            }
            $($setting.pfEsiSetting.formData).find('#chkbVPFProjection').prop('checked', data.VPFProjection);
            $($setting.pfEsiSetting.formData).find('#DDdays').val(data.TDSdays)
            var d = new Date();

            if (data.TDSdays >= d.getDate()) {
                $('#btnCarryForward').hide();
            }
            else {
                $('#btnCarryForward').show();
            }
        },
        bindEvent: function () {
            $('#' + $setting.pfEsiSetting.formName).on('submit', function () {
                $setting.pfEsiSetting.save();
                return false;
            });
        },
        save: function () {
            var VPFProjReq = false; var VPFProjection = false;
            if ($($setting.pfEsiSetting.formData).find('#chkbVPFProjReq').prop('checked') == true) {
                VPFProjReq = true;
            }
            if ($($setting.pfEsiSetting.formData).find('#chkbVPFProjection').prop('checked') == true) {
                VPFProjection = true;
            }

            var formdata = {
                PFBankName: $($setting.pfEsiSetting.formData).find('#txtPFBankName').val(),
                PFBankAddress: $($setting.pfEsiSetting.formData).find('#txtPFBankAddress').val(),
                GroupCode: $($setting.pfEsiSetting.formData).find('#txtGroupCode').val(),
                PFEmployeerCode: $($setting.pfEsiSetting.formData).find('#txtPFEmployerCode').val(),
                //PensionFundAcNo: $($setting.pfEsiSetting.formData).find('#txtPensionFundAc').val(),
                //EPFAcNo: $($setting.pfEsiSetting.formData).find('#txtEPFAc').val(),
                AdminChargeAcNo: $($setting.pfEsiSetting.formData).find('#txtAdminChargeAc').val(),
                InspectionChargeAcNo: $($setting.pfEsiSetting.formData).find('#txtInspectionChargesAc').val(),
                EDLIAcNo: $($setting.pfEsiSetting.formData).find('#txtEDLIAc').val(),
                //ESIEmployeerContribution: $($setting.pfEsiSetting.formData).find('#txtESIEmployersContribution').val(),
                PayrollProcessBy: $($setting.pfEsiSetting.formData).find('#sltPayrollProcess').val(),
                VPFProjectionRequired: VPFProjReq,
                VPFProjection: VPFProjection,
                TDSdays: $($setting.pfEsiSetting.formData).find('#DDdays').val(),
                ServiceYearMonth: $("#serviceYearMonth").val(),
            };
            $.ajax({
                url: $app.baseUrl + "Setting/SaveCompSetting",
                data: JSON.stringify({ dataValue: formdata }),
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
        }
    },

    CarryForward: function () {
        if ($('#carryFromDays').val() == $('#carryToDays').val()) {
            $app.showAlert("From & To Month Could Not Same", 4);
            return true;
        }
        $.ajax({
            url: $app.baseUrl + "Setting/DeclarationCarryForward",
            data: JSON.stringify({ StartMonth: $('#carryFromDays').val(), EndMonth: $('#carryToDays').val() }),
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

    pfEsiStatutorySetting: {
        formName: 'frm_statutory',
        formData: document.forms['frm_statutory'],
        loadcategory: function () {
            $companyCom.loadCategory({ id: 'sltSaturyCategory' });
            $setting.pfEsiStatutorySetting.bindEvent();
        },
        renderForm: function (categoryid) {
            $app.showProgressModel();
            $.ajax({
                url: $app.baseUrl + "Setting/GetCategorySetting",
                data: JSON.stringify({ category: categoryid }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    switch (jsonResult.Status) {
                        case true:
                            $app.hideProgressModel();
                            $setting.pfEsiStatutorySetting.assignValueControl(jsonResult.result);
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
        assignValueControl: function (datas) {
            var tmp = datas[1];
            var data = datas[0];
            $($setting.pfEsiStatutorySetting.formData).find('#txtpflimit').val(data.PFLimit);
            $($setting.pfEsiStatutorySetting.formData).find('#rdpfLimit').prop('checked', true);
            if (data.PFProcess == 'Limit') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpfLimit').prop('checked', true);
            }
            else if (data.PFProcess == 'Gross') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpfNoLimit').prop('checked', true);
            }
            else if (data.PFProcess == 'Both') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpfBoth').prop('checked', true);
            }
            $($setting.pfEsiStatutorySetting.formData).find('#txtpfrounding').val(data.PFRounding);

            $($setting.pfEsiStatutorySetting.formData).find('#rdpFPFlimt').prop('checked', true);
            if (data.PFFPFProcess == 'Limit') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpFPFlimt').prop('checked', true);
            }
            else if (data.PFFPFProcess == 'Gross') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpFPFGrs').prop('checked', true);
            }

            $($setting.pfEsiStatutorySetting.formData).find('#txtAdminlimit').val(data.PFAdminLimit);
            $($setting.pfEsiStatutorySetting.formData).find('#rdpfAdminChargelimt').prop('checked', true);
            if (data.PFAdminChargeProcess == 'Limit') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpfAdminChargelimt').prop('checked', true);
            }
            else if (data.PFAdminChargeProcess == 'Gross') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpfAdminChargeGrs').prop('checked', true);
            }
            $($setting.pfEsiStatutorySetting.formData).find('#txtEDLIlimit').val(data.PFEdliLimit);
            $($setting.pfEsiStatutorySetting.formData).find('#rdpfEDLIChargelimt').prop('checked', true);
            if (data.PFEdliChargeProcess == 'Limit') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpfEDLIChargelimt').prop('checked', true);
            }
            else if (data.PFEdliChargeProcess == 'Gross') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdpfEDLIChargeGrs').prop('checked', true);
            }

            //Esi seeting
            $($setting.pfEsiStatutorySetting.formData).find('#txtEsilimit').val(data.ESILimit);
            $($setting.pfEsiStatutorySetting.formData).find('#rdesiLimit').prop('checked', true);
            if (data.ESIProcess == 'Limit') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdesiLimit').prop('checked', true);
            }
            else if (data.ESIProcess == 'Gross') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdesiNoLimit').prop('checked', true);
            }
            $($setting.pfEsiStatutorySetting.formData).find('#txtesirounding').val(data.ESIRounding);
            $($setting.pfEsiStatutorySetting.formData).find('#txtInspectionlimit').val(data.PFInspectionLimit);
            $($setting.pfEsiStatutorySetting.formData).find('#rdEsiInsChargelimt').prop('checked', true);
            if (data.PFInspectionChargeProcess == 'Limit') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdEsiInsChargelimt').prop('checked', true);
            }
            else if (data.PFInspectionChargeProcess == 'Gross') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdEsiInsChargeGrs').prop('checked', true);
            }
            //Month day setting
            $($setting.pfEsiStatutorySetting.formData).find('#dvMonthStartDay').hide();
            $($setting.pfEsiStatutorySetting.formData).find('#rdmonthDay').prop('checked', true);
            if (data.MonthDayProcess == 'MonthDay') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdmonthDay').prop('checked', true);
            }
            else if (data.MonthDayProcess == 'StaticDay') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdStaticDay').prop('checked', true);
                $($setting.pfEsiStatutorySetting.formData).find('#dvMonthStartDay').show();
            }
            else if (data.MonthDayProcess == 'StartDay') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdStartingDay').prop('checked', true);
                $($setting.pfEsiStatutorySetting.formData).find('#dvMonthStartDay').show();
            }
            else if (data.MonthDayProcess == 'MonthlyInput') {
                $($setting.pfEsiStatutorySetting.formData).find('#rdMonthInput').prop('checked', true);
            }
            $($setting.pfEsiStatutorySetting.formData).find('#txtmonthdayorstartday').val(data.MonthDayOrStartDay);
        },
        bindEvent: function () {
            $('#' + $setting.pfEsiStatutorySetting.formName).on('submit', function () {
                $setting.pfEsiStatutorySetting.save();
                return false;
            });
            $('#sltSaturyCategory').on('change', function () {
                $('#btnStatutorySave').show();
                if ($($setting.pfEsiStatutorySetting.formData).find('#sltSaturyCategory').val() == '' || $($setting.pfEsiStatutorySetting.formData).find('#sltSaturyCategory').val() == '00000000 - 0000 - 0000 - 0000 - 000000000000') {
                    $('#btnStatutorySave').hide();
                }
                $setting.pfEsiStatutorySetting.renderForm($($setting.pfEsiStatutorySetting.formData).find('#sltSaturyCategory').val());
                return false;
            });
            $('#rdmonthDay,#rdStaticDay,#rdStartingDay,#rdMonthInput').on('change', function (e) {
                switch (e.target.id) {
                    case "rdmonthDay":
                        $($setting.pfEsiStatutorySetting.formData).find('#dvMonthStartDay').hide();
                        break;
                    case "rdStaticDay":
                        $($setting.pfEsiStatutorySetting.formData).find('#dvMonthStartDay').show();
                        break;
                    case "rdStartingDay":
                        $($setting.pfEsiStatutorySetting.formData).find('#dvMonthStartDay').show();
                        break;
                    case "rdMonthInput":
                        $($setting.pfEsiStatutorySetting.formData).find('#dvMonthStartDay').hide();
                        break;
                }
                return false;
            });
            $("#txtmonthdayorstartday").change(function () {
                if (($("#txtmonthdayorstartday").val() > '31') || $("#txtmonthdayorstartday").val() < '0') {
                    $app.showAlert("Please provide validate month date", 4);
                }
            });
        },
        save: function () {

            var pFProcess = 'Limit';
            if ($($setting.pfEsiStatutorySetting.formData).find('#rdpfNoLimit').prop('checked') == true) {
                pFProcess = 'Gross';
            }
            else if ($($setting.pfEsiStatutorySetting.formData).find('#rdpfBoth').prop('checked') == true) {
                pFProcess = 'Both';
            }
            var pFFPFProcess = 'Limit';
            if ($($setting.pfEsiStatutorySetting.formData).find('#rdpFPFGrs').prop('checked') == true) {
                pFFPFProcess = 'Gross';
            }
            var pFAdminChargeProcess = 'Limit';
            if ($($setting.pfEsiStatutorySetting.formData).find('#rdpfAdminChargeGrs').prop('checked') == true) {
                pFAdminChargeProcess = 'Gross';
            }
            var pFEdliChargeProcess = 'Limit';
            if ($($setting.pfEsiStatutorySetting.formData).find('#rdpfEDLIChargeGrs').prop('checked') == true) {
                pFEdliChargeProcess = 'Gross';
            }
            var eSIProcess = 'Limit';
            if ($($setting.pfEsiStatutorySetting.formData).find('#rdesiNoLimit').prop('checked') == true) {
                eSIProcess = 'Gross';
            }
            var monthDayProcess = 'MonthDay';
            if ($($setting.pfEsiStatutorySetting.formData).find('#rdStaticDay').prop('checked') == true) {
                monthDayProcess = 'StaticDay';
            }
            else if ($($setting.pfEsiStatutorySetting.formData).find('#rdStartingDay').prop('checked') == true) {
                monthDayProcess = 'StartDay';
            }
            else if ($($setting.pfEsiStatutorySetting.formData).find('#rdMonthInput').prop('checked') == true) {
                monthDayProcess = 'MonthlyInput';
            }
            var eSIInspectionChargeProcess = 'Limit'
            if ($($setting.pfEsiStatutorySetting.formData).find('#rdEsiInsChargeGrs').prop('checked') == true) {
                eSIInspectionChargeProcess = 'Gross';
            }
            var formdata = {
                Id: $($setting.pfEsiStatutorySetting.formData).find('#sltSaturyCategory').val(),
                PFLimit: $($setting.pfEsiStatutorySetting.formData).find('#txtpflimit').val(),
                PFProcess: pFProcess,
                PFRounding: $($setting.pfEsiStatutorySetting.formData).find('#txtpfrounding').val(),
                PFFPFProcess: pFFPFProcess,
                PFAdminChargeProcess: pFAdminChargeProcess,
                PFAdminLimit: $($setting.pfEsiStatutorySetting.formData).find('#txtAdminlimit').val(),
                PFEdliChargeProcess: pFEdliChargeProcess,
                PFEdliLimit: $($setting.pfEsiStatutorySetting.formData).find('#txtEDLIlimit').val(),
                ESILimit: $($setting.pfEsiStatutorySetting.formData).find('#txtEsilimit').val(),
                ESIProcess: eSIProcess,
                PFInspectionChargeProcess: eSIInspectionChargeProcess,
                PFInspectionLimit: $($setting.pfEsiStatutorySetting.formData).find('#txtInspectionlimit').val(),
                ESIRounding: $($setting.pfEsiStatutorySetting.formData).find('#txtesirounding').val(),
                MonthDayProcess: monthDayProcess,
                MonthDayOrStartDay: $($setting.pfEsiStatutorySetting.formData).find('#txtmonthdayorstartday').val()
            };
            $app.showProgressModel();
            $.ajax({
                url: $app.baseUrl + "Setting/SaveCategorySetting",
                data: JSON.stringify({ dataValue: formdata }),
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
        }
    }


};