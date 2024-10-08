
$(document).ready(function () {


});

$Payslipsetting = {
    headerDisplayOrder: 0,
    fandfDisplayOrder: 0,
    footerDisplayOrder: 0,
    earningDisplayOrder: 0,
    deductionDisplayOrder: 0,
    ConfigurationId: '00000000-0000-0000-0000-000000000000',
    settingFor: null,
    HeaderContent: null,
    FooterContent: null,
    LoadPayslipsetting: function (settingFor) {
        debugger;
        $Payslipsetting.settingFor = settingFor;
        $dwcategory.designForm('DataWizardHtml');
        $dwcategory.loadComponent();
        $dwCatMasterfield.designForm('dvCatMasterField');
        $dwCatMasterfield.loadComponent($Payslipsetting.settingFor);
        $dwEarnings.designForm('dvEarnings');
        $dwEarnings.loadComponent($Payslipsetting.settingFor);
        $dwDeductions.designForm('dvDeductions');
        $dwDeductions.loadComponent($Payslipsetting.settingFor);
        $Payslipsetting.loadSetting();

        $('#sltconfiguration').on('change', function (e) {
            debugger;
            $Payslipsetting.ConfigurationId = $('#sltconfiguration').val();
            $('#txtDescription').val($("#sltconfiguration option:selected").html());
            $Payslipsetting.get();
            if ($Payslipsetting.ConfigurationId == "00000000-0000-0000-0000-000000000000") {
                $("#txtTitle").val("");
            }
            return false;

        });
        $('#tblEarnings tbody').on('dblclick', 'td:nth-child(4) input', function () {

            if ($(this).val() == "") {
                $Payslipsetting.headerDisplayOrder = $Payslipsetting.headerDisplayOrder + 1;
                $(this).val($Payslipsetting.headerDisplayOrder);
            } else {
                $(this).val('');
            }
        });
        $('#tblEarnings tbody').on('dblclick', 'td:nth-child(5) input', function () {
            if ($(this).val() == "") {

                $Payslipsetting.earningDisplayOrder = $Payslipsetting.earningDisplayOrder + 1;
                $(this).val($Payslipsetting.earningDisplayOrder);
            } else {
                $(this).val('');
            }
        });
        $('#tblEarnings tbody').on('dblclick', 'td:nth-child(6) input', function () {

            if ($(this).val() == "") {
                $Payslipsetting.footerDisplayOrder = $Payslipsetting.footerDisplayOrder + 1;
                $(this).val($Payslipsetting.footerDisplayOrder);
            } else {
                $(this).val('');
            }
        });
        $('#tblDeductions tbody').on('dblclick', 'td:nth-child(4) input', function () {

            if ($(this).val() == "") {
                $Payslipsetting.headerDisplayOrder = $Payslipsetting.headerDisplayOrder + 1;
                $(this).val($Payslipsetting.headerDisplayOrder);
            } else {
                $(this).val('');
            }
        });
        $('#tblDeductions tbody').on('dblclick', 'td:nth-child(5) input', function () {

            if ($(this).val() == "") {
                $Payslipsetting.deductionDisplayOrder = $Payslipsetting.deductionDisplayOrder + 1;
                $(this).val($Payslipsetting.deductionDisplayOrder);
            } else {
                $(this).val('');
            }
        });
        $('#tblDeductions tbody').on('dblclick', 'td:nth-child(6) input', function () {

            if ($(this).val() == "") {
                $Payslipsetting.footerDisplayOrder = $Payslipsetting.footerDisplayOrder + 1;
                $(this).val($Payslipsetting.footerDisplayOrder);
            } else {
                $(this).val('');
            }
        });
        $('#tblCatMasterfield tbody').on('dblclick', 'td:nth-child(5) input', function () {

            if ($(this).val() == "") {
                $Payslipsetting.headerDisplayOrder = $Payslipsetting.headerDisplayOrder + 1;
                $(this).val($Payslipsetting.headerDisplayOrder);
            } else {
                $(this).val('');
            }
        });
        $('#tblCatMasterfield tbody').on('dblclick', 'td:nth-child(6) input', function () {
            debugger;
            if ($(this).val() == "") {
                $Payslipsetting.footerDisplayOrder = $Payslipsetting.footerDisplayOrder + 1;
                $(this).val($Payslipsetting.footerDisplayOrder);
            } else {
                $(this).val('');
            }
        });
        $('#tblCatMasterfield tbody').on('dblclick', 'td:nth-child(7) input', function () {
            debugger;
            if ($(this).val() == "") {
                $Payslipsetting.fandfDisplayOrder = $Payslipsetting.fandfDisplayOrder + 1;
                $(this).val($Payslipsetting.fandfDisplayOrder);
            } else {
                $(this).val('');
            }
        });

    }, SelectAll: function (tblid, chkid) {
        debugger;
        var checkboxCount = $('#' + tblid + ' tbody tr').length;
        var isCheckAll;
        var rows = $("#" + tblid + "").dataTable().fnGetNodes();
        if (chkid.checked == true) {
            isCheckAll = true;
        } else {
            isCheckAll = false;
        }
        for (var i = 0; i < rows.length; i++) {
            $(rows[i]).find("." + chkid.id).prop("checked", isCheckAll);
        }
    },
    loadSetting: function () {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "DataWizard/GetSettings",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({ id: $Payslipsetting.ConfigurationId }),
            success: function (jsonResult) {
                $('#txtTitle').val('');
                var out = jsonResult.result.setting;
                $('#sltconfiguration').html('');
                $('#sltconfiguration').append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                for (var i = 0; out.length > i; i++) {
                    $Payslipsetting.ConfigurationId = out[0].CofigurationId;
                    $('#sltconfiguration').append($("<option value='" + out[i].CofigurationId + "'>" + out[i].description + "</option>"));
                }
                $('#sltconfiguration').val('00000000-0000-0000-0000-000000000000');
                //if ($Payslipsetting.ConfigurationId == '' || $Payslipsetting.ConfigurationId == '00000000-0000-0000-0000-000000000000') {
                //    $('#sltconfiguration').val('00000000-0000-0000-0000-000000000000');
                //}
                //else {
                //    $('#sltconfiguration').val($Payslipsetting.ConfigurationId);
                //}
                if (out.length < 0) {
                    document.getElementById('sltconfiguration').style.display = 'none';
                    document.getElementById$('txtDescription').style.display = 'block';
                } else {
                    document.getElementById('txtDescription').style.display = 'none';
                    document.getElementById('sltconfiguration').style.display = 'block';
                }
            },
            error: function (msg) {
            }
        });
    },
    get: function () {
        debugger;
        $.ajax({
            url: $app.baseUrl + "DataWizard/GetSettings",
            data: JSON.stringify({ id: $Payslipsetting.ConfigurationId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                switch (jsonResult.Status) {
                    case true:
                        $('#txtTitle').val(jsonResult.result.setting[0].title);
                        $('#sltCumlativeMonth').val(jsonResult.result.setting[0].cumulativeMonth);
                        $Payslipsetting.HeaderContent = jsonResult.result.setting[0].Header;
                        $Payslipsetting.FooterContent = jsonResult.result.setting[0].Footer;
                        $('#tblCatMasterfield').dataTable().fnClearTable();
                        $('#tblEarnings').dataTable().fnClearTable();
                        $('#tblDeductions').dataTable().fnClearTable();
                        var $radios = $('input:radio[name=Matching]');
                        var MatchingSettingsFor = jsonResult.result.setting[0].MatchingSettingsFor;
                        if (MatchingSettingsFor == "Cumulative") {
                            $radios.filter('[value=Cumulative]').prop('checked', true);
                            $("#dvMatching").removeClass('show').addClass('hide');
                            $("#dvCumulative").removeClass('hide').addClass('show');
                        }
                        else if (MatchingSettingsFor == "Matching") {
                            $radios.filter('[value=Matching]').prop('checked', true);
                            $("#dvMatching").removeClass('hide').addClass('show');
                            $("#dvCumulative").removeClass('show').addClass('hide');
                        }
                        else {
                            $radios.filter('[value=Nill]').prop('checked', true);
                            $("#dvCumulative").removeClass('show').addClass('hide');
                            $("#dvMatching").removeClass('show').addClass('hide');
                        }

                        var data = jsonResult.result;
                        var rows = $("#tbldwCat").dataTable().fnGetNodes();

                        for (var i = 0; i < rows.length; i++) {
                            $(rows[i]).find(".cbCategory").prop("checked", false);
                            for (var j = 0; j < data.attrCategory.length; j++) {
                                if (data.attrCategory[j].fieldName == $(rows[i]).find(":eq(2)").html()) {
                                    $(rows[i]).find(".cbCategory").prop("checked", true);
                                }
                            }
                        }
                        for (var j = 0; j < data.attrMaster.length; j++) {

                            if (data.attrMaster[j].tableName == "AdditionalInfo") {
                                data.attrMaster[j].fieldName = data.attrMaster[j].displayAs;
                            }
                        }
                        $dwCatMasterfield.loadMasterGrid(data.attrMaster);
                        $dwEarnings.loadEarningsGrid(data.attrEarnings);
                        $dwDeductions.loadDeductionsGrid(data.attrDeductions);
                        if (MatchingSettingsFor == "Matching") {
                            $payslipMatching.renderFieldGrid();
                            $payslipMatching.LoadAttributeModels();
                        }

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
    cancelSetting: function () {


        document.getElementById('txtDescription').style.display = 'none';
        document.getElementById('sltconfiguration').style.display = 'block';
        $(".cbCategory").prop("checked", false);

        $Payslipsetting.ConfigurationId = '00000000-0000-0000-0000-000000000000',
         $Payslipsetting.LoadPayslipsetting('Payslip');
        $Payslipsetting.loadSetting();

    },
    newPSSetting: function () {

        //$('#txtDescription').val('');
        document.getElementById('txtDescription').style.display = 'block';
        $('#txtDescription').val('');
        document.getElementById('sltconfiguration').style.display = 'none';
        //   $('#tbldwCat').dataTable().fnClearTable();
        var rows = $("#tbldwCat").dataTable().fnGetNodes();

        for (var i = 0; i < rows.length; i++) {
            $(rows[i]).find(".cbCategory").prop("checked", false);
        }
        $Payslipsetting.ConfigurationId = '00000000-0000-0000-0000-000000000000';

        //$Payslipsetting.headerDisplayOrder = 0;
        //$Payslipsetting.footerDisplayOrder = 0;
        //$Payslipsetting.earningDisplayOrder = 0;
        //$Payslipsetting.deductionDisplayOrder = 0;
        // $Payslipsetting.fandfDisplayOrder = 0;

        $('#txtTitle').val('');
        $('#sltCumlativeMonth').val(0);
    },
    //Created by Keerthika on 14/06/2017
    deleteSettings: function () {

        $Payslipsetting.ConfigurationId = $('#sltconfiguration').val();
        $.ajax({
            url: $app.baseUrl + "DataWizard/DeleteSetting",
            data: JSON.stringify({ configId: $Payslipsetting.ConfigurationId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert(jsonResult.Message, 2);
                        $('#txtTitle').val('');
                        $Payslipsetting.ConfigurationId = '00000000-0000-0000-0000-000000000000',
                        $Payslipsetting.LoadPayslipsetting('Payslip');
                        $Payslipsetting.loadSetting();
                        break;
                    case false:
                        $app.showAlert("Data is not deleted Properly", 4);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },

    GetAllFields: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "DataWizard/GetSettings",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({ id: $Payslipsetting.ConfigurationId }),

            success: function (jsonResult) {
                var out = jsonResult.result.setting;
            },
            error: function (msg) {
            }
        });
    },

    saveSettings: function () {
        debugger;
        $app.showProgressModel();
        $Payslipsetting.GetAllFields();

        var setting = new Object();
        setting.CofigurationId = $Payslipsetting.ConfigurationId;
        setting.description = $('#txtDescription').val();
        setting.title = $('#txtTitle').val();
        setting.cumulativeMonth = $('#sltCumlativeMonth').val();
        setting.Header = $Payslipsetting.HeaderContent;
        setting.Footer = $Payslipsetting.FooterContent;
        setting.MatchingSettingsFor = $('input[name=Matching]:checked').val();


        var attr = [];
        var tempvalidation = [];
        var matchingComp = [];
        //Get Selected Category
        var rows = $("#tbldwCat").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            if ($(rows[i]).find(".cbCategory").prop("checked")) {
                newattr.fieldName = $(rows[i]).find(":eq(2)").html();

                newattr.tableName = "Category";
                newattr.type = "Category";
                newattr.isPhysicalTbl = true;

                attr.push(newattr);
            }

        }

        var rows = $("#tblCatMasterfield").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            newattr.fieldName = $(rows[i]).find(":eq(3)").html();
            newattr.tableName = $(rows[i]).find(":eq(2)").html();
            newattr.type = "Master";
            newattr.isPhysicalTbl = true;
            newattr.hOrder = $(rows[i]).find(".txtHOrder input").val();
            newattr.fOrder = $(rows[i]).find(".txtFOrder input").val();
            newattr.ffhOrder = $(rows[i]).find(".txtFFHOrder input").val();
            newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
            newattr.eOrder = 0;
            newattr.dOrder = 0;
            if (newattr.tableName == "AdditionalInfo") {

                newattr.fieldName = $(rows[i]).find(".attributeId").html();
            }
            attr.push(newattr);


        }
        var rows = $("#tblEarnings").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            newattr.fieldName = $(rows[i]).find(":eq(1)").html();
            // newattr.tableName = $(rows[i]).find(":eq(3)").html();
            newattr.type = "Earnings";
            newattr.isPhysicalTbl = true;
            newattr.hOrder = $(rows[i]).find(".txtHOrder input").val();
            newattr.fOrder = $(rows[i]).find(".txtFOrder input").val();
            newattr.eOrder = $(rows[i]).find(".txtEOrder input").val();
            newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
            newattr.includeGross = $(rows[i]).find(":eq(3) .includegross").html();
            newattr.dOrder = 0;
            if (newattr.includeGross != undefined && (newattr.eOrder == "" || newattr.eOrder == "0" || newattr.eOrder == 0)) {
                tempvalidation.push(newattr);
            }
            attr.push(newattr);

        }

        var rows = $("#tblDeductions").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            newattr.fieldName = $(rows[i]).find(":eq(1)").html();
            //  newattr.tableName = $(rows[i]).find(":eq(3)").html();
            newattr.type = "Deductions";
            newattr.isPhysicalTbl = true;
            newattr.hOrder = $(rows[i]).find(".txtHOrder input").val();
            newattr.fOrder = $(rows[i]).find(".txtFOrder input").val();
            newattr.eOrder = 0;
            newattr.dOrder = $(rows[i]).find(".txtDOrder input").val();
            newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
            if (newattr.includeGross != undefined && (newattr.dOrder == "" || newattr.dOrder == "0" || newattr.dOrder == 0)) {
                tempvalidation.push(newattr);
            }
            attr.push(newattr);

        }
        if (tempvalidation.length == 0) {
            if (setting.MatchingSettingsFor == "Matchings" || setting.MatchingSettingsFor == "Matching")
                $payslipMatching.saveMatchingComp();
            matchingComp = $payslipMatching.MatchingVal;
            $.ajax({
                url: $app.baseUrl + "DataWizard/saveSetting",
                data: JSON.stringify({ setting: setting, attr: attr, matchingComp: matchingComp }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {

                    switch (jsonResult.Status) {
                        case true:
                            $app.showAlert("Saved Successfully", 1);
                            document.getElementById('txtDescription').style.display = 'none';
                            document.getElementById('sltconfiguration').style.display = 'block';
                            $(".cbCategory").prop("checked", false);
                            $('#txtTitle').val('');
                            $Payslipsetting.ConfigurationId = '00000000-0000-0000-0000-000000000000',
                             $Payslipsetting.LoadPayslipsetting('Payslip');
                            $Payslipsetting.loadSetting();
                            break;
                        case false:
                            $app.showAlert(jsonResult.Message, 4);
                            $Payslipsetting.ConfigurationId = '00000000-0000-0000-0000-000000000000',
                            $Payslipsetting.LoadPayslipsetting('Payslip');
                            $Payslipsetting.loadSetting();
                            break;
                    }
                    $app.hideProgressModel();
                },
                complete: function () {
                    $app.hideProgressModel();

                }
            });
        }
        else {
            $app.hideProgressModel();
            alert("Settings not Available for " & tempvalidation[0].displayAs);
            $app.showAlert("Please fill earnings and deductions order", 1);
            
        }
    },

    generatePaySlip: function (empcode, type) {
        debugger;
        $app.showProgressModel();
        var categories = '';
        var rows = $("#tbldwCat").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            if ($(rows[i]).find(".cbCategory").prop("checked")) {
                categories += "'" + $(rows[i]).find(":eq(2)").html() + "',";
            }
            categories = categories.trim(',');
            var month = $('#psMonth').val();
            var year = $('#psYear').val();


        }
        $.ajax({
            url: $app.baseUrl + "DataWizard/GetPayrollHistory",
            data: JSON.stringify({ categories: categories, month: month, year: year, empCode: empcode, type: type, singlePDF: $('#singlePDF').prop('checked') }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                var oData = new Object();
                if (jsonResult.Status == true) {
                    if (type == "GeneratePDF") {
                        oData.filePath = jsonResult.result.filePath;
                        $app.downloadSync('Download/DownloadPaySlip', oData);
                    }
                    else if (type == "SendMail") {
                        $app.showAlert(jsonResult.Message, 2);
                    }

                } else {
                    $app.showAlert(jsonResult.Message, 1);
                }


            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },
    generateForm16PartB: function (empcode) {
        $app.showProgressModel();
        debugger;
        var categories = '';
        var FinYrId = '';
        var FinYrText = '';
        var rows = $("#tbldwCat").dataTable().fnGetNodes();

        for (var i = 0; i < rows.length; i++) {
            var newattr = new Object();
            if ($(rows[i]).find(".cbCategory").prop("checked")) {
                categories += "'" + $(rows[i]).find(":eq(2)").html() + "',";
            }

            categories = categories.trim(',');
            FinYrId = $('#FinYears').val();
            FinYrText = $('#FinYears :selected').text();

        }

        $.ajax({
            url: $app.baseUrl + "DataWizard/GetForm16PartB",
            data: JSON.stringify({ categories: categories, empCode: empcode, FinYrId: FinYrId, FinYrText: FinYrText }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                if (jsonResult.result.empcodes != "")
                    $app.showAlert(jsonResult.result.empcodes + " Employees Form 16 Part B Not generated", 1);
                var oData = new Object();
                oData.filePath = jsonResult.result.filepath;
                if (jsonResult.result.filepath != "")
                    $app.downloadSync('Download/DownloadPaySlip', oData);
            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },

    loadFinYrs: function () {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "FinanceYear/GetFinanceYears",
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
                $.each(out, function (index, blood) {
                    $('#FinYears').append($("<option></option>").val(blood.Id).html(blood.Name));
                });
            },
            error: function (msg) {
            }
        });
    },
}
