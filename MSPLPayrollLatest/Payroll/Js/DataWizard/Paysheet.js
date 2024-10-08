$(document).ready(function () {

    $('table').removeClass('table-bordered');
});

$("#txtrptTitle").blur(function () {

    if ($("#txtrptTitle").val().length > 0)
        $PaySheet.Descriptioncheck();
});

//$("#btnPSSaveSetting").click(function () {
//    $PaySheet.savePaySettings();
//});

$PaySheet = {
    displayOrder: 0,
    Id: '0',
    Desname: '',
    Titlename: '',
    settingFor: null,
    LoadPaySheetsetting: function (settingFor) {
        $PaySheet.settingFor = settingFor;
        $dwcategory.designForm('DataWizardHtml');
        $dwcategory.loadComponent();
        $dwCatMasterfield.designForm('dvCatMasterField');
        $dwCatMasterfield.loadComponent($PaySheet.settingFor);
        $dwEarnings.designForm('dvEarnings');
        $dwEarnings.loadComponent($PaySheet.settingFor);
        $dwDeductions.designForm('dvDeductions');
        $dwDeductions.loadComponent($PaySheet.settingFor);
        $PaySheet.loadSetting();
        $('#sltRptedit').on('change', function (e) {

            $PaySheet.Id = $('#sltRptedit').val();
            $('#txtrptTitle').val($("#sltRptedit option:selected").html());
            if ($PaySheet.Id != 0) {
                $PaySheet.get();
            }
            else {
                $PaySheet.settingFor = settingFor;
                $dwcategory.designForm('DataWizardHtml');
                $dwcategory.loadComponent();
                $dwCatMasterfield.designForm('dvCatMasterField');
                $dwCatMasterfield.loadComponent($PaySheet.settingFor);
                $dwEarnings.designForm('dvEarnings');
                $dwEarnings.loadComponent($PaySheet.settingFor);
                $dwDeductions.designForm('dvDeductions');
                $dwDeductions.loadComponent($PaySheet.settingFor);
                $('#tblGrouping').dataTable().fnClearTable();
                $('#txtTitle').val('');
            }
            return false;
        });
        $('#sltRpt').on('change', function (e) {
            $PaySheet.Id = $('#sltRpt').val();
            $('#txtrptTitle').val($("#sltRpt option:selected").html());
            if ($PaySheet.Id != 0) {
                $PaySheet.getvalid();
            }
            else {
                $("#sltSMonth").removeAttr("disabled");
                $("#sltSYear").removeAttr("disabled");
                $("#sltNMonth").removeAttr("disabled");
                $("#sltNYear").removeAttr("disabled");
                $('#sltSYear').val('');
                $('#sltNYear').val('');
            }
            return false;
        });
    },
    displayAttrOrder: function (chkRow) {

        var allrows = [];

        allrows.push($("#tblCatMasterfield").dataTable().fnGetNodes())
        allrows.push($("#tblEarnings").dataTable().fnGetNodes())
        allrows.push($("#tblDeductions").dataTable().fnGetNodes())


        for (var j = 0; j < allrows.length; j++) {

            for (var i = 0; i < allrows[j].length; i++) {

                if ($(allrows[j][i]).find(".chkMaster,.chkEarnings,.chkDeductions").prop("checked")) {
                    if (parseInt($(allrows[j][i]).find('.txtOrder input').val()) > $PaySheet.displayOrder) {
                        $PaySheet.displayOrder = parseInt($(allrows[j][i]).find('.txtOrder input').val());
                    }
                }
            }
        }




        if (chkRow.checked == true) {
            $PaySheet.displayOrder = $PaySheet.displayOrder + 1;
            $(chkRow).parent().parent().find('.txtOrder').val($PaySheet.displayOrder);
            $(chkRow).parent().parent().find('.txtOrder').removeAttr("disabled");
        } else {
            $(chkRow).parent().parent().find('.txtOrder').val("");
            $(chkRow).parent().parent().find('.txtOrder').attr("disabled", "disabled");
        }
    },
    generatePaySheet: function () {
        debugger;
        $app.showProgressModel();
        var categories = '';
        var attr = [];
        var groups = [];
        var attrFilter = [];
        var title = '';
        var isDetail = '';

        //if ($('#sltRpt').val() == 0) {
        //    $app.showAlert("Title not selected", 4);
        //    return false;
        //}
        if ($('#sltSYear').val() == "" || $('#sltNYear').val() == "") {
            $app.showAlert("Please enter from & To year", 4);
            $app.hideProgressModel();
            return false;
        }
        var fp = $('#sltSYear').val();
        var fm = Number($('#sltSMonth').val());
        var tp = $('#sltNYear').val();
        var tm = Number($('#sltNMonth').val());
        fp = (fp * 100) + fm;
        tp = (tp * 100) + tm;
        if (tp < fp) { 
            $app.showAlert("Please enter To period Should not be less than from period", 4);
            $app.hideProgressModel();
            return false;
        }
        else
        {
            debugger;
            var paysheetValues = '';
            if ($('#sltRpt').val() != 0) {
                $.ajax({
                    url: $app.baseUrl + "DataWizard/GetPaysheetSettings",
                    data: JSON.stringify({ id: $('#sltRpt').val() }),
                    dataType: "json",
                    contentType: "application/json",
                    async: false,
                    type: "POST",
                    success: function (jsonResult) {
                        switch (jsonResult.Status) {
                            
                            case true:
                                var data = jsonResult.result;
                                debugger;
                                title = data.setting[0].title;
                                isDetail = data.setting[0].isDetail;
                                for (var j = 0; j < data.attrCategory.length; j++) {
                                    categories = categories + data.attrCategory[j] + ',';

                                }
                                for (var j = 0; j < data.attrGroup.length; j++) {
                                    attr.push(data.attrGroup[j]);
                                    groups.push(data.attrGroup[j].displayAs);
                                }
                                for (var j = 0; j < data.attrMaster.length; j++) {
                                    attr.push(data.attrMaster[j]);
                                }
                                for (var j = 0; j < data.attrDetail.length; j++) {
                                    attr.push(data.attrDetail[j]);
                                }
                        }
                    }
                });
            }
            else {
                //   Get Selected Category
                isDetail = $('#rbDeail').prop('checked');
                var rows = $("#tbldwCat").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {
                    var newattr = new Object();
                    if ($(rows[i]).find(".cbCategory").prop("checked")) {
                        categories = categories + $(rows[i]).find(":eq(2)").html() + ',';

                    }
                }

                var rows = $("#tblGrouping").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {

                    var newattr = new Object();

                    if ($(rows[i]).find(".groupField").val() != 0) {

                        newattr.fieldName = $(rows[i]).find(".groupField option:selected").html();
                        newattr.displayAs = $(rows[i]).find(".groupField option:selected").html();
                        newattr.tableName = $(rows[i]).find(".groupField").val();
                        newattr.type = "Group";
                        newattr.isPhysicalTbl = false;
                        newattr.order = 0;

                        attr.push(newattr);
                    }

                }

                var rows = $("#tblCatMasterfield").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {

                    if ($(rows[i]).find(".chkMaster").prop("checked")) {

                        var newattr = new Object();
                        newattr.fieldName = $(rows[i]).find(":eq(2)").html();
                        newattr.tableName = $(rows[i]).find(":eq(3)").html();
                        newattr.order = $(rows[i]).find('.txtOrder input').val();
                        newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
                        newattr.type = "Master";
                        newattr.isPhysicalTbl = true;
                        if (newattr.tableName == "AdditionalInfo") {
                            newattr.fieldName = $(rows[i]).find(".attributeId").html();
                        }
                        attr.push(newattr);
                    }

                }
                var rows = $("#tblEarnings").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {

                    var newattr = new Object();
                    if ($(rows[i]).find(".chkEarnings").prop("checked")) {

                        newattr.fieldName = $(rows[i]).find(":eq(2)").html();
                        newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
                        newattr.tableName = " ";
                        newattr.type = "Detail";
                        newattr.isPhysicalTbl = false;
                        newattr.order = $(rows[i]).find('.txtOrder input').val();

                        attr.push(newattr);
                    }

                }
                var rows = $("#tblDeductions").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {
                    var newattr = new Object();
                    if ($(rows[i]).find(".chkDeductions").prop("checked")) {
                        newattr.fieldName = $(rows[i]).find(":eq(2)").html();
                        newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
                        newattr.tableName = " ";
                        newattr.type = "Detail";
                        newattr.isPhysicalTbl = true;
                        newattr.order = $(rows[i]).find('.txtOrder input').val();
                        attr.push(newattr);
                    }

                }
            }
        }


        var rows = $("#tblFilter").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {
            debugger;
            var newattr = new Object();

            if ($(rows[i]).find(".filterField").val() != 0) {

                newattr.type = $(rows[i]).find('td:eq(4)').text();
                if (newattr.type == "Master") {
                    newattr.fieldName = $(rows[i]).find(".filterField option:selected").html();
                } else {
                    newattr.fieldName = $(rows[i]).find(".filterField").val();
                }
                newattr.displayAs = $(rows[i]).find(".filterField option:selected").html();
                newattr.tableName = $(rows[i]).find(".filterField").val();

                newattr.datatype = $(rows[i]).find('td:eq(3)').text();
                newattr.operation = $(rows[i]).find(".operations").val();
                newattr.order = 0;
                var temp = newattr.displayAs.split(" ");
                if (temp[1] == "Category" || temp[1] == "BankName" || temp[1] == "CostCentre" || temp[1] == "Department" || temp[1] == "Designation" || temp[1] == "ESILocation" || temp[1] == "PTLocation") {
                    newattr.value = $(rows[i]).find(".filterValue option:selected").html();
                }
                else {
                    newattr.value = $(rows[i]).find(".filterValue").val();
                }

                attrFilter.push(newattr);
            }

        }

        //created by suriya
       // if ($('#sltNYear').val() == '') {
            //if ($('#sltSYear').val() == '')
            //{
            //   var a = (new Date()).getMonth();          //currentmonth()
            //   var b = (new Date()).getFullYear();       //currentyear()
            //   $('#sltSMonth').val(a);
            //   $('#sltSYear').val(b);
            //}
        //}

        var month = $('#sltSMonth').val();
        var year = $('#sltSYear').val();
        // $('#sltNMonth').val(month);
        // $('#sltNYear').val(year);

        debugger;

        $.ajax({
            url: $app.baseUrl + "DataWizard/GetPaySheet",
            data: JSON.stringify({ paysheetattr: attr, category: categories, title: title, smonth: $('#sltSMonth').val(), syear: $('#sltSYear').val(), nMonth: $('#sltNMonth').val(), nYear: $('#sltNYear').val(), isDetail: isDetail, groupby: groups, filters: attrFilter }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                debugger;
                if (jsonResult.result.filePath != "") {
                    var oData = new Object();
                    oData.filePath = jsonResult.result.filePath;
                    $app.downloadSync('Download/DownloadPaySlip', oData);
                }
                else {
                    $app.showAlert('No data found.File not downloaded', 1);
                    $app.hideProgressModel();
                }
                //ASPX page URL to load report
                //  var src = $app.baseUrl + "assets/plugins/ckeditor/DataWizardReport.aspx";


                //Create a dynamic iframe here and append this to div tag
                // var iframe = '<iframe id="myReportFrame" width="100%" height="800px" scrolling="no" frameborder="0" src="' + src + '" allowfullscreen></iframe>';
                //  $("#divReport").html(iframe);

            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },
    loadSetting: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "DataWizard/GetPaysheetSettings",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({ id: $PaySheet.Id }),

            success: function (jsonResult) {

                $('#txtTitle').val('');
                var out = jsonResult.result.setting;

                $('#sltRptedit').html('');
                $('#sltRptedit').append($("<option></option>").val('0').html('--Select--'));
                $('#sltRpt').html('');
                $('#sltRpt').append($("<option></option>").val('0').html('--Select--'));
                for (var i = 0; out.length > i; i++) {

                    $PaySheet.Id = out[0].id;
                    $('#sltRptedit').append($("<option value='" + out[i].id + "'>" + out[i].description + "</option>"));
                    $('#sltRpt').append($("<option value='" + out[i].id + "'>" + out[i].description + "</option>"));
                }
                $('#sltRptedit').val(0);
                $('#sltRpt').val(0);
                if (out.length < 0) {
                    document.getElementById('sltRptedit').style.display = 'none';
                    document.getElementById$('txtrptTitle').style.display = 'block';
                } else {
                    document.getElementById('txtrptTitle').style.display = 'none';
                    document.getElementById('sltRptedit').style.display = 'block';
                }


            },
            error: function (msg) {
            }
        });
    },
    get: function () {
        $.ajax({
            url: $app.baseUrl + "DataWizard/GetPaysheetSettings",
            data: JSON.stringify({ id: $PaySheet.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {

                    case true:

                        var data = jsonResult.result;
                        $('#txtrptTitle').val(data.setting[0].description);
                        $('#txtTitle').val(data.setting[0].title);

                        var rowsCategory = $("#tbldwCat").dataTable().fnGetNodes();
                        var rowsMaster = $("#tblCatMasterfield").dataTable().fnGetNodes();
                        var rowsEarnings = $("#tblEarnings").dataTable().fnGetNodes();
                        var rowsDeductions = $("#tblDeductions").dataTable().fnGetNodes();

                        $('#tblGrouping').dataTable().fnClearTable();
                        if (data.setting[0].isDetail) {
                            $('#rbDeail').prop('checked', true);
                        } else {
                            $('#rbconsolidate').prop('checked', true);
                        }
                        for (var i = 0; i < rowsCategory.length; i++) {
                            $(rowsCategory[i]).find(".cbCategory").prop("checked", false);

                            for (var j = 0; j < data.attrCategory.length; j++) {

                                if (data.attrCategory[j] == $(rowsCategory[i]).find(":eq(2)").html()) {

                                    $(rowsCategory[i]).find(".cbCategory").prop("checked", true);
                                }
                            }
                        }
                        for (var j = 0; j < data.attrGroup.length; j++) {
                            $PaySheetGrouping.addRow();
                            var tr = $("#tblGrouping").find("tr").last();

                            tr.find(".groupField option").filter(function () {
                                return this.text == data.attrGroup[j].fieldName;
                            }).attr('selected', true);
                        }

                        for (var i = 0; i < rowsMaster.length; i++) {
                            $(rowsMaster[i]).find(".chkMaster").prop("checked", false);
                            $(rowsMaster[i]).find('.txtOrder').val('');
                            for (var j = 0; j < data.attrMaster.length; j++) {

                                if (data.attrMaster[j].fieldName == $(rowsMaster[i]).find(":eq(2)").html() || data.attrMaster[j].fieldName == $(rowsMaster[i]).find(".attributeId").html()) {
                                    $(rowsMaster[i]).find(".chkMaster").prop("checked", true);
                                    //   $(rowsMaster[i]).find(":eq(3)").html(data.attrMaster[j].tableName);
                                    //  $(rowsMaster[i]).find(":eq(4)").html(data.attrMaster[j].fieldName);
                                    $(rowsMaster[i]).find('.txtOrder input').val(data.attrMaster[j].order);
                                    $(rowsMaster[i]).find('.txtDisplay input').val(data.attrMaster[j].displayAs);

                                }
                            }
                        }
                        for (var i = 0; i < rowsEarnings.length; i++) {
                            $(rowsEarnings[i]).find(".chkEarnings").prop("checked", false);
                            $(rowsEarnings[i]).find('.txtOrder').val('');
                            for (var j = 0; j < data.attrDetail.length; j++) {
                                if (data.attrDetail[j].fieldName == $(rowsEarnings[i]).find(":eq(2)").html()) {
                                    $(rowsEarnings[i]).find(".chkEarnings").prop("checked", true);
                                    $(rowsEarnings[i]).find('.txtOrder input').val(data.attrDetail[j].order);
                                    $(rowsEarnings[i]).find('.txtDisplay input').val(data.attrDetail[j].displayAs);
                                }
                               /* if (data.attrDetail.length == 0) {
                                    $("#sltSMonth").attr("disabled", "disabled");
                                    $("#sltSYear").attr("disabled", "disabled");
                                    $("#sltNMonth").attr("disabled", "disabled");
                                    $("#sltNYear").attr("disabled", "disabled");
                                }
                                else {*/
                                    $("#sltSMonth").removeAttr("disabled");
                                    $("#sltSYear").removeAttr("disabled");
                                    $("#sltNMonth").removeAttr("disabled");
                                    $("#sltNYear").removeAttr("disabled");
                                //}
                            }
                        }
                        for (var i = 0; i < rowsDeductions.length; i++) {
                            $(rowsDeductions[i]).find(".chkDeductions").prop("checked", false);
                            $(rowsDeductions[i]).find('.txtOrder').val('');
                            for (var j = 0; j < data.attrDetail.length; j++) {
                                if (data.attrDetail[j].fieldName == $(rowsDeductions[i]).find(":eq(2)").html()) {
                                    $(rowsDeductions[i]).find(".chkDeductions").prop("checked", true);
                                    $(rowsDeductions[i]).find('.txtOrder input').val(data.attrDetail[j].order);
                                    $(rowsDeductions[i]).find('.txtDisplay input').val(data.attrDetail[j].displayAs);
                                }
                              /*  if (data.attrDetail.length == 0) {
                                    $("#sltSMonth").attr("disabled", "disabled");
                                    $("#sltSYear").attr("disabled", "disabled");
                                    $("#sltNMonth").attr("disabled", "disabled");
                                    $("#sltNYear").attr("disabled", "disabled");
                                }
                                else {*/
                                    $("#sltSMonth").removeAttr("disabled");
                                    $("#sltSYear").removeAttr("disabled");
                                    $("#sltNMonth").removeAttr("disabled");
                                    $("#sltNYear").removeAttr("disabled");
                               // }
                            }
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
        document.getElementById('txtrptTitle').style.display = 'none';
        document.getElementById('sltRptedit').style.display = 'block';
        $PaySheet.displayOrder = 0;
        $dwcategory.loadComponent();
        $dwCatMasterfield.loadComponent($PaySheet.settingFor);
        $dwEarnings.loadComponent($PaySheet.settingFor);
        $dwDeductions.loadComponent($PaySheet.settingFor);
        $('#tblGrouping').dataTable().fnClearTable();
        $('#txtTitle').val('');
        $PaySheet.Id = '0';
        $PaySheet.loadSetting($PaySheet.Id);
        $('#sltRptedit').append($("<option></option>").val('0').html('--Select--'));

        //$(".cbCategory").prop("checked", false);
    },
    newPSSetting: function () {
        document.getElementById('txtrptTitle').style.display = 'block';
        document.getElementById('sltRptedit').style.display = 'none';
        //  setting.description = $('#txtrptTitle').val('');
        $('#sltRptedit').val(0);
        $('#txtTitle').val('');
        $('#txtrptTitle').val('');
        $PaySheet.Id = 0;
        $PaySheet.displayOrder = 0;
        $dwcategory.loadComponent();
        $dwCatMasterfield.loadComponent($PaySheet.settingFor);
        $dwEarnings.loadComponent($PaySheet.settingFor);
        $dwDeductions.loadComponent($PaySheet.settingFor);
        $('#tblGrouping').dataTable().fnClearTable();

    },
    //created by suriya...
    deleteSettings: function () {

        $PaySheet.ID = $('#sltRptedit').val();
        $.ajax({
            url: $app.baseUrl + "DataWizard/DeletepaysheetSetting",
            data: JSON.stringify({ iD: $PaySheet.ID }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert("Deleted Successfully", 2);
                        document.getElementById('txtrptTitle').style.display = 'none';
                        document.getElementById('sltRptedit').style.display = 'block';
                        $PaySheet.displayOrder = '0';
                        $dwcategory.loadComponent();
                        $dwCatMasterfield.loadComponent($PaySheet.settingFor);
                        $dwEarnings.loadComponent($PaySheet.settingFor);
                        $dwDeductions.loadComponent($PaySheet.settingFor);
                        $('#tblGrouping').dataTable().fnClearTable();
                        $('#txtTitle').val('');
                        $PaySheet.Id = '0';
                        $PaySheet.loadSetting($PaySheet.Id);
                        $('#sltRptedit').append($("<option></option>").val('0').html('--Select--'));
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
    //created by suriya...
    Descriptioncheck: function (data) {

        $PaySheet.Id = $('#sltRptedit').val();
        $PaySheet.Desname = $('#txtrptTitle').val();
        $.ajax({
            url: $app.baseUrl + "DataWizard/Descriptioncheck",
            data: JSON.stringify({ ID: $PaySheet.Id, Description: $PaySheet.Desname }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert("Title available", 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $('#txtrptTitle').val('');
                        $('#txtrptTitle').focus();
                        break;
                }
            },
            complete: function () {
                $PaySheet.savePaySettings = true;
            }
        });
    },

    //created by suriya

    getvalid: function () {
        $.ajax({
            url: $app.baseUrl + "DataWizard/GetPaysheetSettings",
            data: JSON.stringify({ id: $PaySheet.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {

                    case true:
                        var data = jsonResult.result;

                        var rowsEarnings = $("#tblEarnings").dataTable().fnGetNodes();
                        var rowsDeductions = $("#tblDeductions").dataTable().fnGetNodes();

                        for (var i = 0; i < rowsEarnings.length; i++) {
                            $(rowsEarnings[i]).find(".chkEarnings").prop("checked", false);
                            $(rowsEarnings[i]).find('.txtOrder').val('');
                            for (var j = 0; j < data.attrDetail.length; j++) {
                                if (data.attrDetail[j].fieldName == $(rowsEarnings[i]).find(":eq(2)").html()) {
                                }
                            }
                           /* if (data.attrDetail.length == 0) {
                                $("#sltSMonth").attr("disabled", "disabled");
                                $("#sltSYear").attr("disabled", "disabled");
                                $("#sltNMonth").attr("disabled", "disabled");
                                $("#sltNYear").attr("disabled", "disabled");
                                $('#sltSYear').val('');
                                $('#sltNYear').val('');
                            }
                            else {*/
                                $("#sltSMonth").removeAttr("disabled");
                                $("#sltSYear").removeAttr("disabled");
                                $("#sltNMonth").removeAttr("disabled");
                                $("#sltNYear").removeAttr("disabled");
                            //}
                        }
                        for (var i = 0; i < rowsDeductions.length; i++) {
                            $(rowsDeductions[i]).find(".chkDeductions").prop("checked", false);
                            $(rowsDeductions[i]).find('.txtOrder').val('');
                            for (var j = 0; j < data.attrDetail.length; j++) {
                                if (data.attrDetail[j].fieldName == $(rowsDeductions[i]).find(":eq(2)").html()) {
                                }
                            }
                            /*if (data.attrDetail.length == 0) {
                                $("#sltSMonth").attr("disabled", "disabled");
                                $("#sltSYear").attr("disabled", "disabled");
                                $("#sltNMonth").attr("disabled", "disabled");
                                $("#sltNYear").attr("disabled", "disabled");
                                $('#sltSYear').val('');
                                $('#sltNYear').val('');
                            }
                            else {*/
                                $("#sltSMonth").removeAttr("disabled");
                                $("#sltSYear").removeAttr("disabled");
                                $("#sltNMonth").removeAttr("disabled");
                                $("#sltNYear").removeAttr("disabled");
                           // }
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
    savePaySetting: function () {

        setting = new Object();
        attr = [];
        categories = '';
        //   Get Selected Category
        rows = $("#tbldwCat").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {
            var newattr = new Object();
            if ($(rows[i]).find(".cbCategory").prop("checked")) {
                categories = categories + $(rows[i]).find(":eq(2)").html() + ',';

            }
        }
        if ($("#txtrptTitle").val() == '') {
            $app.showAlert("Enter the title name", 4);
        }
        else {
            if (categories == '') {
                $app.showAlert("Please select category", 4);

            }
            else {
                setting.description = $('#txtrptTitle').val();
                setting.id = $('#sltRptedit').val();
                setting.category = categories;
                setting.title = $('#txtTitle').val();
                setting.isDetail = $('#rbDeail').prop('checked');
                var rows = $("#tblGrouping").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {

                    var newattr = new Object();

                    if ($(rows[i]).find(".groupField").val() != 0) {

                        newattr.fieldName = $(rows[i]).find(".groupField option:selected").html();
                        newattr.displayAs = $(rows[i]).find(".groupField option:selected").html();
                        newattr.tableName = $(rows[i]).find(".groupField").val();
                        newattr.type = "Group";
                        newattr.isPhysicalTbl = false;
                        newattr.order = 0;

                        attr.push(newattr);
                    }

                }

                var rows = $("#tblCatMasterfield").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {

                    if ($(rows[i]).find(".chkMaster").prop("checked")) {

                        var newattr = new Object();
                        newattr.fieldName = $(rows[i]).find(":eq(2)").html();
                        newattr.tableName = $(rows[i]).find(":eq(3)").html();
                        newattr.order = $(rows[i]).find('.txtOrder input').val();
                        newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
                        newattr.type = "Master";
                        newattr.isPhysicalTbl = true;
                        if (newattr.tableName == "AdditionalInfo") {
                            newattr.fieldName = $(rows[i]).find(".attributeId").html();
                        }
                        attr.push(newattr);
                    }

                }
                var rows = $("#tblEarnings").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {

                    var newattr = new Object();
                    if ($(rows[i]).find(".chkEarnings").prop("checked")) {

                        newattr.fieldName = $(rows[i]).find(":eq(2)").html();
                        newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
                        newattr.tableName = " ";
                        newattr.type = "Detail";
                        newattr.isPhysicalTbl = false;
                        newattr.order = $(rows[i]).find('.txtOrder input').val();

                        attr.push(newattr);
                    }

                }
                var rows = $("#tblDeductions").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {

                    var newattr = new Object();
                    if ($(rows[i]).find(".chkDeductions").prop("checked")) {

                        newattr.fieldName = $(rows[i]).find(":eq(2)").html();
                        newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
                        newattr.tableName = " ";
                        newattr.type = "Detail";
                        newattr.isPhysicalTbl = true;
                        newattr.order = $(rows[i]).find('.txtOrder input').val();


                        attr.push(newattr);
                    }

                }
                $.ajax({
                    url: $app.baseUrl + "DataWizard/savePaysheetSetting",
                    data: JSON.stringify({
                        paysheetattr: attr,
                        jpaysheet: setting
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    success: function (jsonResult) {
                        switch (jsonResult.Status) {
                            case true:

                                $app.showAlert("Saved Successfully", 2);
                                document.getElementById('txtrptTitle').style.display = 'none';
                                document.getElementById('sltRptedit').style.display = 'block';
                                $PaySheet.displayOrder = '0';
                                $dwcategory.loadComponent();
                                $dwCatMasterfield.loadComponent($PaySheet.settingFor);
                                $dwEarnings.loadComponent($PaySheet.settingFor);
                                $dwDeductions.loadComponent($PaySheet.settingFor);
                                $('#tblGrouping').dataTable().fnClearTable();
                                $('#txtTitle').val('');
                                $PaySheet.Id = '0';
                                $PaySheet.loadSetting($PaySheet.Id);
                                $('#sltRptedit').append($("<option></option>").val('0').html('--Select--'));
                                break;

                        }
                    },
                    complete: function () {
                        $app.hideProgressModel();

                    }
                });
            }
        }

    }
}