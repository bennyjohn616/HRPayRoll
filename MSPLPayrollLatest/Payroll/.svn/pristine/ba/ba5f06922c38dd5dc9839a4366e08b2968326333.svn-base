$('#sltlopcategory,#sltsplmntrycategory').change(function () {
    $PremiumSetting.GetlopComponent();
});
$PremiumSetting = {
    canSave: true,
    selectedCategoryId: null,
    selectedComponent: null,
    ComponentloadData: '',
    tableId: 'tbllopcategorygrid',
    suplmtrytableId: 'tblsplmtrycategorygrid',
    lopcategoryId: 'sltlopcategory',
    suplmtryCatId: 'sltsplmntrycategory',
    formData: document.forms["premiumsetting"],
    loadpremiumsetting: function (data, context, tableId) {
        $companyCom.loadlopcrdyscmpnent({ id: "sltlopcreditdays" });
        $companyCom.loadlopcrdyscmpnent({ id: "sltsplmntrydays" });
        $companyCom.loadCategory({ id: "sltlopcategory" });
        $companyCom.loadCategory({ id: "sltsplmntrycategory" });
        var dtCategoryList = $('#' + $PremiumSetting.tableId).DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [


              { "data": "Id" },
                {
                    "data": null
                },
              { "data": "displayAs" }
            ],
            "aoColumnDefs": [
                {
                    "aTargets": [0],
                    "sClass": "nodisp",
                    "bSearchable": false
                },
            {
                "aTargets": [1],
                "sClass": "actionColumn",
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    var b = $('<input type="checkbox" class="cbComponent" id="' + sData + '"/>');
                    $(nTd).html(b);
                }

            },
            {
                "aTargets": [2],
                "sClass": "word-wrap"

            }, ]
            ,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "PremiumSetting/GetComponents",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        var Rdata = jsonResult.result;
                        var out = Rdata;
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
                var r = $('#' + $PremiumSetting.tableId + 'tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + $PremiumSetting.tableId + 'thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
        $PremiumSetting.loadsplmntrypremiumsetting();
    },
    loadsplmntrypremiumsetting: function (data, context, suplmtrytableId) {
        var dtCategoryList = $('#' + $PremiumSetting.suplmtrytableId).DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [


              { "data": "Id" },
                {
                    "data": null
                },
              { "data": "displayAs" }
            ],

            "aoColumnDefs": [
                {
                    "aTargets": [0],
                    "sClass": "nodisp",
                    "bSearchable": false
                },
            {
                "aTargets": [1],
                "sClass": "actionColumn",
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    var b = $('<input type="checkbox" class="cbComponent" id="' + sData + '"/>');
                    $(nTd).html(b);
                }

            },
            {
                "aTargets": [2],
                "sClass": "word-wrap"

            }, ]
            ,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "PremiumSetting/GetComponents",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        var Rdata = jsonResult.result;
                        var out = Rdata;
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
                var r = $('#' + $PremiumSetting.suplmtrytableId + 'tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + $PremiumSetting.suplmtrytableId + 'thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    SaveComponent: function () {
        tblid = $('#settingTab li.active').text().trim() == 'LOP Credit Setting' ? $PremiumSetting.tableId : $PremiumSetting.suplmtrytableId;
        catId = $('#settingTab li.active').text().trim() == 'LOP Credit Setting' ? $PremiumSetting.lopcategoryId : $PremiumSetting.suplmtryCatId;
        var categories = '';
        //Get Selected Category
        var rows = $("#" + tblid).dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {
            if ($(rows[i]).find(".cbComponent").prop("checked")) {
                categories = categories + $(rows[i]).find(":eq(0)").html() + ',';
            }
        }
        $app.showProgressModel();
        var formData = document.forms["premiumsetting"];
        var data = {
            PAttrId: categories,
            PCategory: $("#" + catId).val(),
            PType: $('#settingTab li.active').text().trim()
        };
        $.ajax({
            url: $app.baseUrl + "PremiumSetting/SavePremiumSettingComponent",
            data: JSON.stringify({ datavalue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $PremiumSetting.canSave = true;
                        var p = jsonResult.result;
                        $PremiumSetting.selectedCategoryId = $("#" + catId).val();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $PremiumSetting.canSave = true;
                        break;
                }
            },
            complete: function () {
                $PremiumSetting.canSave = true;
            }
        });
    },
    savelopdays: function () {

        if (!$PremiumSetting.canSave) {
            return false;
        }
        $PremiumSetting.canSave = false;
        $app.showProgressModel();
        var data = {
            PComponent: $($PremiumSetting.formData).find('#sltlopcreditdays').val(),
            PBackMonth: $($PremiumSetting.formData).find('#txtBackMonth').val(),
            PType: $('#settingTab li.active').text().trim()

        }; //$PremiumSetting.BuildaysmatchingObject();
        $.ajax({
            url: $app.baseUrl + "PremiumSetting/SavePremiumSetting",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $PremiumSetting.canSave = true;
                        var p = jsonResult.result;
                        $PremiumSetting.selectedComponent = p.component;
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $PremiumSetting.canSave = true;
                        break;
                }
            },
            complete: function () {
                $PremiumSetting.canSave = true;
            }
        });
    },
    BuildaysmatchingObject: function () {
        var retObject = {
            component: $($PremiumSetting.formData).find('#sltlopcreditdays').val(),
            empCode: $($PremiumSetting.formData).find('#txtBackMonth').val(),
        };
        return retObject;
    },
    GetlopComponent: function () {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "PremiumSetting/GetSavedComponents",
            contentType: "application/json",
            data: JSON.stringify({ CategoryId: $('#settingTab li.active').text().trim() == 'LOP Credit Setting' ? $('#sltlopcategory').val() : $('#sltsplmntrycategory').val(), Type: $('#settingTab li.active').text().trim() }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $PremiumSetting.RenderLopComponent(out);
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
    RenderLopComponent: function (catdata) {
        var tbl = $('#settingTab li.active').text().trim() == 'LOP Credit Setting' ? $PremiumSetting.tableId : $PremiumSetting.suplmtrytableId
        $("#" + tbl + " tbody tr").each(function () {
            $(this).find(".cbComponent").prop('checked', false)
        });

        $(catdata).each(function (i, obj) {
            var $this = $(this);
            $("#" + tbl + " tbody tr").each(function () {
                if ($(this).find(":eq(0)").html() == $this[0].PCategory) {
                    $(this).find(".cbComponent").prop('checked', true);
                    return false;
                }
            });
        });
    },
    savesupdays: function () {
        if (!$PremiumSetting.canSave) {
            return false;
        }
        $PremiumSetting.canSave = false;
        $app.showProgressModel();
        var data = {
            PComponent: $($PremiumSetting.formData).find('#sltsplmntrydays').val(),
            PBackMonth: $($PremiumSetting.formData).find('#txtBackMonthsplmntry').val(),
            PType: $('#settingTab li.active').text().trim()

        }; //$PremiumSetting.BuildaysmatchingObject();
        $.ajax({
            url: $app.baseUrl + "PremiumSetting/SavePremiumSetting",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $PremiumSetting.canSave = true;
                        var p = jsonResult.result;
                        $PremiumSetting.selectedComponent = p.component;
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $PremiumSetting.canSave = true;
                        break;
                }
            },
            complete: function () {
                $PremiumSetting.canSave = true;
            }
        });
    },
    BuildaysmatchingObject: function () {
        var retObject = {
            component: $($PremiumSetting.formData).find('#sltsplmntrydays').val(),
            empCode: $($PremiumSetting.formData).find('#txtBackMonthsplmntry').val(),
        };
        return retObject;
    },
};
