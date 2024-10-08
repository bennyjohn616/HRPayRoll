$("#btnUpload").on('click', function (event) {

    $Employeefimport.uploadfile();
    $('#xlSheetdrop').html('');
    return false;
});

$('#fUpload').on('change', function (e) {

    $('#xlSheetdrop').html('');
    $('#dvTxtbox').html('');
    $('#dvColoumn').html('');
    $('#entityModeltree').html('');
    $('#rangeFrom,#rangeTo').attr('readonly', false);

    var files = e.target.files;
    if (files.length > 0) {
        var file = this.files[0];
        fileName = file.name;
        var re = /\..+$/;
        var ext = fileName.match(re);
        $("#btnUpload").removeClass('hide');
        $("#btnUpload").hide();
        size = file.size;

        //if (ext[0] == '.xls') {
            if (size > 5242880) {
                $('#fUpload').val('');
                $app.showAlert('Invalid file size, the maximum file size is 5 MB', 3);
                $Employeefimport.fileData = null;
                return false;
            }
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append(files[x].name, files[x]);
                }
                $Employeefimport.fileData = data;
                $("#btnUpload").show();
            }
            else {
                $('#fUpload').val('');
                $app.showAlert("This browser doesn't support HTML5 file uploads!", 3);
                $Employeefimport.fileData = null;
                return false;
            }
        //}
        //else {
        //    $('#fUpload').val('');
        //    $app.showAlert('Invalid file, please select ".xls" file', 4);
        //    $Employeefimport.fileData = null;
        //    return false;
        //}
    }
});
//Created by AjithPanner on 14/12/17
$('#btnImport').on('click', function (e) {
    debugger;
    $Employeefimport.retriveData();

    if ($Employeefimport.selectedModel != null && $Employeefimport.selectedSheet != null && $Employeefimport.selectedSheet != 0) {
        if ($('#srartRow').val() != "" && $('#endRow').val() != "") {
            $Employeefimport.importProcess();
        }
        else {
            $app.showAlert("Please enter Start Row and End Row ", 3);
        }
    }
    return false;

});
$('#btnCloseError').on('click', function (e) {

    // $Employeefimport.importProcess();
    $('#DvErrorPopup').addClass('hide');
    return false;

});

$('#sltImportTemplate').on('change', function (e) {
    if ($Employeefimport.ViewLoadedColumns == true) {
        $Employeefimport.selectedTemplateId = $('#sltImportTemplate').val();
        if (confirm('Do you want to refresh the mapping based on the template?')) {
            //LoadColumnNames
            $Employeefimport.loadTemplateDetail();
            //$Employeefimport.xlSheetdrop($Employeefimport.selectedSheet);
        }
    }
    else {
        $("#sltImportTemplate").val("00000000-0000-0000-0000-000000000000");
        $app.showAlert("Please load the columns", 4);
        return false;
    }
    return false;

});
$('#btnEdit').on('click', function (e) {

    // $('#AddTemplate').modal('toggle');
    $Employeefimport.canTemplateSave = true;
    $('#txtTemplateName').val($('#sltImportTemplate').find('option:selected').text());
    $('#chkCurrentSetting').prop('checked', false);

});
$('#btnAddNew').on('click', function (e) {

    $Employeefimport.canTemplateSave = true;
    $Employeefimport.selectedTemplateId = '';
    $('#txtTemplateName').val('');
    $('#chkCurrentSetting').prop('checked', false);
    // return false;

});
$("input[name=searchTables]").keyup(function (e) {

    var treeT = $("#entityModeltree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("button#btnResetSearchTables").click();
        return;
    }
    var n = treeT.applyFilter(match);
    $("button#btnResetSearchTables").attr("disabled", false);
}).focus();

$("button#btnResetSearchTables").click(function (e) {

    var treeT = $("#entityModeltree").fancytree("getTree");
    $("input[name=searchTables]").val("");
    treeT.clearFilter();
}).attr("disabled", true);

$("input#hideModeTables").change(function (e) {

    var treeT = $("#entityModeltree").fancytree("getTree");
    treeT.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    treeT.clearFilter();
    $("input[name=searchTables]").keyup();
    treeT.render();
});

$("input[name=searchSheet]").keyup(function (e) {

    var treeS = $("#xlSheettree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("button#btnResetSearchSheet").click();
        return;
    }
    var n = treeS.applyFilter(match);
    $("button#btnResetSearchSheet").attr("disabled", false);
}).focus();

$("button#btnResetSearchSheet").click(function (e) {

    var treeS = $("#xlSheettree").fancytree("getTree");
    $("input[name=searchSheet]").val("");
    treeS.clearFilter();
}).attr("disabled", true);

$("input#hideModeSheet").change(function (e) {

    var treeS = $("#xlSheettree").fancytree("getTree");
    treeS.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    treeS.clearFilter();
    $("input[name=searchSheet]").keyup();
    //tree.render();
});

$("input[name=searchColumn]").keyup(function (e) {

    var treeC = $("#attributetree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("button#btnResetSearchColumn").click();
        return;
    }
    var n = treeC.applyFilter(match);
    $("button#btnResetSearchColumn").attr("disabled", false);
}).focus();

$("button#btnResetSearchColumn").click(function (e) {

    var treeC = $("#attributetree").fancytree("getTree");
    $("input[name=searchColumn]").val("");
    treeC.clearFilter();
}).attr("disabled", true);

$("input#hideModeColumn").change(function (e) {

    var treeC = $("#attributetree").fancytree("getTree");
    treeC.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    treeC.clearFilter();
    $("input[name=searchColumn]").keyup();
    // tree.render();
});
$("input[name=searchExcel]").keyup(function (e) {

    var treeX = $("#xlColumntree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("button#btnResetSearchExcel").click();
        return;
    }
    var n = treeX.applyFilter(match);
    $("button#btnResetSearchExcel").attr("disabled", false);
}).focus();

$("button#btnResetSearchExcel").click(function (e) {

    var treeX = $("#xlColumntree").fancytree("getTree");
    $("input[name=searchExcel]").val("");
    treeX.clearFilter();
}).attr("disabled", true);

$("input#hideModeExcel").change(function (e) {

    var treeX = $("#xlColumntree").fancytree("getTree");
    treeX.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    treeX.clearFilter();
    $("input[name=searchExcel]").keyup();
    //treeX.render();
});
$('#xlSheetdrop').change(function () {
    if ($('#xlSheetdrop').val() != "0") {
        $Employeefimport.selectedSheet = $('#xlSheetdrop').val();
    }
});
$("#selectValue").change(function () {

    var selectedText = $(this).find("option:selected").text();
    var selectedValue = $(this).val();
    alert("Selected Text: " + selectedText + " Value: " + selectedValue);
});


$('#frmTemplate').on('submit', function (e) {


    $Employeefimport.retriveData();
    if (!$Employeefimport.canTemplateSave)
        return false;
    $Employeefimport.canTemplateSave = false;
    var saveObj = {
        id: $Employeefimport.selectedTemplateId,
        name: $('#txtTemplateName').val()
    };
    var jsonImportTemplateDetails = [];
    if ($Employeefimport.fileResponseData != null) {
        $.each($Employeefimport.fileResponseData[1], function (ind, item) {
            if (item.MappedSheet != '' && item.MappedSheet != null) {
                $.each(item.ImportColumns, function (chiInd, childItem) {
                    var tempDetails = {
                        id: '',
                        importTemplateId: $Employeefimport.selectedTemplateId,
                        tableName: item.Name,
                        mappedSheetName: item.MappedSheet,
                        tableColumn: childItem.OtherTableUniqueId == '00000000-0000-0000-0000-000000000000' ? childItem.Name : childItem.OtherTableUniqueId,
                        mappedSheetColumn: childItem.MappedColumnName
                    }
                    jsonImportTemplateDetails.push(tempDetails);
                });
            }
        });
    }
    saveObj.jsonImportTemplateDetails = jsonImportTemplateDetails;
    debugger;
    $app.showProgressModel();
    $.ajax({
        type: 'POST',
        url: $app.baseUrl + "Util/SaveImportTemplate",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ importTemplate: saveObj, currentSetting: $('#chkCurrentSetting').prop('checked') }),
        dataType: "json",
        success: function (jsonResult) {
            $app.clearSession(jsonResult);
            switch (jsonResult.Status) {
                case true:
                    $Employeefimport.loadTemplate({ id: 'sltImportTemplate' });
                    $('#AddTemplate').modal('toggle');
                    $app.hideProgressModel();
                    $app.showAlert(jsonResult.Message, 2);
                    break;
                case false:
                    $app.hideProgressModel();
                    $app.showAlert(jsonResult.Message, 4);
                    break;
            }
        },
        error: function (msg) {
        }
    });

    return false;

});

$Employeefimport = {
    canTemplateSave: false,
    fileData: null,
    fileResponseData: null,
    selectedModel: '',
    selectedSheet: '',
    selectedTemplateId: '',
    selectedloadColumnName: '',
    ViewLoadedColumns: false,
    uploadfile: function () {

        $('#dvErrorMsg').html('');
        if ($('#rangeFrom').val() != "" && $('#rangeTo').val() != "") {

            $Employeefimport.fileData.set("fromRange", $("#rangeFrom").val().toUpperCase())
            $Employeefimport.fileData.set("toRange", $("#rangeTo").val().toUpperCase());
        }
        else {

            $app.showAlert("Please enter FromRange and ToRange ", 3);
            return false;
        }
        $('#dvDrodp').show();
        $('#dvViewp').show();

        $.ajax({
            type: "POST",
            url: $app.baseUrl + 'Util/UploadFile',
            contentType: false,
            processData: false,
            data: $Employeefimport.fileData,

            success: function (result) {

                $Employeefimport.LoadOptFldVals();
                $Employeefimport.fileResponseData = result;
                $Employeefimport.renderModelTree(result[1]);
                // $Employeefimport.renderSheetTree(result[0].XlSheet);
                $Employeefimport.xlSheetdrop(result[0].XlSheet);
                $Employeefimport.renderAttributeTree(null);
                $Employeefimport.renderXlColumnTree(null);
                $Employeefimport.loadTemplate({ id: 'sltImportTemplate' });
                $('#rangeFrom,#rangeTo').attr('readonly', true);
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
            }
        });
    },
    SelectAll: function (tblid, chkid) {

        var checkboxCount = $('#' + tblid + ' tbody tr').length;
        var isCheckAll;
        var rows = $("#" + tblid + "").dataTable().fnGetNodes();
        if (chkid.checked == true) {
            isCheckAll = true;
        } else {
            isCheckAll = false;
        }
        for (var i = 0; i < rows.length; i++) {
            $(rows[i]).find(".chktablenames").prop("checked", isCheckAll);
        }
    },
    LoadOptFldVals: function () {
        $('#dvTableNames').show();
        var dtClientList = $('#tblOptinalFieldValue').DataTable({
            'iDisplayLength': 18,
            'sDom': '<"top">',
            columns: [
             { "data": "view" },
             { "data": "tablename" },
            ],
            "aoColumnDefs": [
         {
             "aTargets": [0],
             render: function (data, type, row) {
                 return '<input type="checkbox" class="chktablenames" id=' + row.tablenameid + '>';
             }

         },
         {
             "aTargets": [1],
             "sClass": "word-wrap"
         },
            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    url: $app.baseUrl + 'Util/TableNames',
                    //data: null,
                    data: JSON.stringify({ importTableName: $payroll.ImportTableName }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (msg) {

                        var out = msg.result[0];
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
            },
            scroller: {
                loadingIndicator: true
            }
        });
    },
    LoadColumnNames: function () {
        debugger;
        var err = 0
        var rows = $('#tblOptinalFieldValue').dataTable().fnGetNodes();
        var OptionalFds = '';
        for (var i = 0; i < rows.length; i++) {
            if ($(rows[i]).find(".chktablenames").prop("checked") == true) {
                var optfdv = $(rows[i]).find(".chktablenames")[0].id;
                OptionalFds = OptionalFds + optfdv + ',';
            }
        }
        OptionalFds = OptionalFds.replace(/,\s*$/, "");
        if ($("#xlSheetdrop").val() == "0") {
            $app.showAlert("Please select the sheet name ", 4);
            err = 1;
        }
        if (OptionalFds == "" || OptionalFds == null) {
            $app.showAlert("Please select the table name ", 4);
            err = 1;
        }
        if (err == 1) {
            $app.hideProgressModel();
            return false;
        }
        $.ajax({
            url: $app.baseUrl + 'Util/ColumnNames',
            data: JSON.stringify({ ColumnName: OptionalFds }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            timeout: 30000,
            success: function (jsonResult) {
                var out = jsonResult.result[0];
                selectedloadColumnName = out;
                $Employeefimport.loadColumnName(out);
                $Employeefimport.ViewLoadedColumns = true;
            },
            complete: function () {
            }
        });
    },
    renderModelTree: function (data) {

        var treeData = [];
        for (var cnt = 0; cnt < data.length; cnt++) {
            treeData.push({ key: data[cnt].Name, title: data[cnt].MappedSheet != null ? data[cnt].Name + '-' + data[cnt].MappedSheet : data[cnt].Name });
            if (data[cnt].MappedSheet != null) {
                $("#xlSheetdrop").val(data[cnt].MappedSheet)
                $Employeefimport.selectedModel = data[cnt].Name;
            }
        }
        //initialize the tree
        $("#entityModeltree").fancytree();
        //before load clean the tree nodes
        $("#entityModeltree").fancytree("destroy");
        fancyTree = jQuery("#entityModeltree").fancytree({
            extensions: ["contextMenu", "dnd", "filter"],
            selectMode: 3,
            strings: {
                loading: "Loading..."
            },
            source: treeData,
            filter: {
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
            autoScroll: true,
            dnd: {
                preventVoidMoves: true, // Prevent dropping nodes 'before self', etc.
                preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
                autoExpandMS: 400,
                draggable: {
                    //                        zIndex: 1000,
                    //                        appendTo: "body",
                    scroll: false,
                    revert: "invalid"
                },
                dragStart: function (node, data) {
                    return true;
                },
                dragEnter: function (node, data) {
                    return true;
                },
                dragDrop: function (node, data) {
                    if (data.otherNode.tree.$div[0].id != 'xlSheettree') {
                        return false;
                    }
                    //for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                    //    if ($Employeefimport.fileResponseData[1][cnt].Name == node.key) {
                    //        $Employeefimport.fileResponseData[1][cnt].MappedSheet = data.otherNode.key;
                    //    }
                    //}

                    if (!$Employeefimport.CheckMapped(true, data.otherNode)) {

                        $Employeefimport.map(true, node, data.otherNode);

                        $Employeefimport.renderModelTree($Employeefimport.fileResponseData[1]);
                    }
                    else {
                        $app.showAlert('You have already mapped this sheet with other table', 3);
                        return false;
                    }
                }
            },
            // checkbox: (("1" === "1") ? true : false),
            click: function (event, data) {

                $Employeefimport.selectedModel = data.node.key;
                //$Employeefimport.loadColumn();
                $Employeefimport.loadColumnName();
            },
            init: function (event, ctx) {
                ctx.tree.debug("init");
                ctx.tree.rootNode.fixSelection3FromEndNodes();
            },
            loadchildren: function (event, ctx) {
                ctx.tree.debug("loadchildren");
                ctx.node.fixSelection3FromEndNodes();
            },
            select: function (event, data) {

            },
            contextMenu: {
                menu: function (node) {
                    var name = $Employeefimport.getMappedName(true, node);
                    if (name != '' && name != null) {
                        return {
                            'UnMap': { 'name': 'UnMap Sheet', 'icon': 'view' }
                        }
                    }
                    else {
                        return {};
                    }
                },
                actions: function (node, action, options) {
                    if (action === "UnMap") {
                        $Employeefimport.unmap(true, node);
                    }
                }
            }

        });

        $('#entityModeltree ul li .fancytree-title').each(function () {
            if ($(this).text().indexOf("-") >= 0) {
                $(this).css('background-color', "#1fb5ac")
            } else {
                $(this).css('background-color', "white")
            }
        });
    },
    loadColumn: function () {

        var attribute = [];
        var xlColumns = [];
        for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
            if ($Employeefimport.fileResponseData[1][cnt].Name == $Employeefimport.selectedModel) {
                attribute = $Employeefimport.fileResponseData[1][cnt].ImportColumns;
                $Employeefimport.selectedSheet = $Employeefimport.fileResponseData[1][cnt].MappedSheet;
            }
        }
        $Employeefimport.renderAttributeTree(attribute);
        for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
            if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == $Employeefimport.selectedSheet) {
                xlColumns = $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns;
            }
        }
        $Employeefimport.renderXlColumnTree(xlColumns);
    },
    xlSheetdrop: function (data) {

        var dropData = [];
        for (var cnt = 0; cnt < data.length; cnt++) {
            dropData.push({ key: data[cnt].sheetName, title: data[cnt].sheetName });
        }
        $('#xlSheetdrop').append($("<option></option>").val(0).html('--Select--'));
        $.each(dropData, function (index, object) {

            $('#xlSheetdrop').append($("<option></option>").val(object.key).html(object.title));

        });
    },
    alreadyMapped: function (selectId) {
        debugger;
        var find = '#' + selectId + ' option:selected'
        var currentVal = $(".column").find(find).val();
        var count = 0;
        var rows = $("#tbl_tblImportColumn").dataTable().fnGetNodes();
        var column = [];
        for (var i = 0; i < rows.length; i++) {
            column[i] = $(rows[i]).find(".column option:selected").val();
        }
        if (currentVal != 0) {
            for (var j = 0 ; j < column.length; j++) {

                if (currentVal == column[j]) {

                    count++;
                    if (count > 1) {
                        $app.showAlert('Already mapped', 3);
                        $(".column").find('#' + selectId).val("0");
                    }
                }

            }
        }

    },
    retriveData: function () {
        debugger;
        var rows = $("#tbl_tblImportColumn").dataTable().fnGetNodes();

        for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
            for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
            }
        }

        for (var cnt = 0; cnt < selectedloadColumnName.length; cnt++) {
            selectedloadColumnName[cnt].MappedColumnName = null;
        }
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();

            if ($(rows[i]).find(".column option:selected").val() != 0) {
                debugger;
                newattr.sheetcol = $(rows[i]).find(".sheetCol").text().trim();
                newattr.xlcol = $(rows[i]).find(".column option:selected").val();
                for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                    if ($Employeefimport.fileResponseData[1][cnt].MappedSheet == $Employeefimport.selectedSheet) {
                        for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                            if ($Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].Name == newattr.sheetcol) {
                                $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = newattr.xlcol;
                            }
                        }
                    }
                }
                for (var cnt = 0; cnt < selectedloadColumnName.length; cnt++) {
                    if (selectedloadColumnName[cnt].Name == newattr.sheetcol) {
                        selectedloadColumnName[cnt].MappedColumnName = newattr.xlcol;
                    }
                }
                for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
                    if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == $Employeefimport.selectedSheet) {
                        for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                            if ($Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == newattr.xlcol) {
                                $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = true;
                            }

                        }
                    }
                }
            }
        }
    },
    loadColumnName: function (ImportClumns) {
        debugger;
        var attribute = [];
        $Employeefimport.selectedSheet = '';

        var rows = $('#tblOptinalFieldValue').dataTable().fnGetNodes();
        var OptionalFds = '';
        for (var i = 0; i < rows.length; i++) {
            if ($(rows[i]).find(".chktablenames").prop("checked") == true) {
                var optfdv = $(rows[i]).find(".chktablenames")[0].id;
                optfdv = optfdv.replace("#", " ");
                optfdv = optfdv.replace("#", " ");
                for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                    debugger;
                    if ($Employeefimport.fileResponseData[1][cnt].Name == optfdv) {
                        $Employeefimport.selectedSheet = $('#xlSheetdrop').val();
                        $Employeefimport.fileResponseData[1][cnt].MappedSheet = $('#xlSheetdrop').val();
                    }
                    //else {
                    //    $Employeefimport.fileResponseData[1][cnt].MappedSheet = null;
                    //}
                }
            }
        }
        for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
            //if ($Employeefimport.fileResponseData[1][cnt].Name == $Employeefimport.selectedModel) {
            //attribute = $Employeefimport.fileResponseData[1][cnt].ImportColumns;
            attribute = ImportClumns;
            //$Employeefimport.fileResponseData[1][cnt].MappedSheet = $('#xlSheetdrop').val();
            $Employeefimport.selectedSheet = $('#xlSheetdrop').val(); //$Employeefimport.fileResponseData[1][cnt].MappedSheet;
            //}
            //else {
            //    $Employeefimport.fileResponseData[1][cnt].MappedSheet = null;
            //}
        }
        $Employeefimport.renderModelTree($Employeefimport.fileResponseData[1]);

        for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
            if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == $Employeefimport.selectedSheet) {
                $Employeefimport.fileResponseData[0].XlSheet[cnt].isMapped = true;
            }
            else {
                $Employeefimport.fileResponseData[0].XlSheet[cnt].isMapped = false;
            }
        }
        var gridObject = [];
        gridObject.push({ tableHeader: "Table", tableValue: 'TableName' });
        gridObject.push({ tableHeader: "Column", tableValue: 'Column' });
        gridObject.push({ tableHeader: "Display Name", tableValue: 'Display Name' });
        gridObject.push({ tableHeader: "*", tableValue: '*' });
        gridObject.push({ tableHeader: "Excel Column", tableValue: 'Excel Column' });
        $Employeefimport.renderFieldGrid(gridObject, { id: "tblImportColumn" });
        if ($("#sltImportTemplate").val() != "00000000-0000-0000-0000-000000000000") {
            $Employeefimport.LoadAttributeModelsTemplate(attribute);
        }
        else {
            $Employeefimport.LoadAttributeModels(attribute);
        }
    },
    renderFieldGrid: function (context, tableprop) {

        $("#dvTxtbox").html('');
        $("#dvTxtbox").html('<div class="col-md-6"><lable class="font-weight: bold">StartRow:  </lable><input type="text" id="startRow" onkeypress="return $validator.IsNumeric(event, this.id)"></div><div class="col-md-6  "><lable class="font-weight: bold">EndRow:  </lable><input type="text" id="endRow" onkeypress="return $validator.IsNumeric(event, this.id)"></div>');
        var grid = '<table id="tbl_' + tableprop.id + '" class="table table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer" style="width: 700px;">'
            + '<thead>'
                    + '<tr>'
        for (var cnt = 0; cnt < context.length; cnt++) {
            grid = grid + '<th>' + context[cnt].tableHeader + '</th>'
        }
        grid = grid + '</tr></thead>';
        grid = grid + '<tbody><tr>'
        for (var cnt = 0; cnt < context.length; cnt++) {//for action td 
            grid = grid + '<td></td>';
        }
        grid = grid + '</tr></tbody></table>';


        $("#dvColoumn").html(grid);
    },
    LoadAttributeModelsTemplate: function (datas) {

        var dtClientList = $('#tbl_tblImportColumn').DataTable({
            'iDisplayLength': 18,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "TableName" },
             { "data": "Name" },
             { "data": "DisplayAs" },
             { "data": "IsRequiredstr" },
             { "data": "MappedColumnName" }

            ],
            "aoColumnDefs": [

         {
             "aTargets": [0],
             "sClass": "word-wrap"

         }, {
             "aTargets": [1],
             "sClass": "word-wrap sheetCol"

         }, {
             "aTargets": [2],
             "sClass": "word-wrap "

         }, {
             "aTargets": [3],
             "sClass": "red"

         },
         {
             "aTargets": [4],
             "sClass": "column",

             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                 var b = $($Employeefimport.xlSheetColumn(oData.Name));
                 $(nTd).empty();
                 $(nTd).prepend(b);
                 if (sData == "" || sData == null) {
                     $(b).val("0");
                 }
                 else {
                     $(b).val(sData);
                 }

             }

         }

            ],
            ajax: function (data, callback, settings) {
                setTimeout(function () {
                    callback({
                        draw: data.draw,
                        data: datas,
                        recordsTotal: datas.length,
                        recordsFiltered: datas.length
                    });
                }, 50);

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
    LoadAttributeModels: function (datas) {

        var dtClientList = $('#tbl_tblImportColumn').DataTable({
            'iDisplayLength': 18,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "TableName" },
             { "data": "Name" },
             { "data": "DisplayAs" },
             { "data": "IsRequiredstr" },
             { "data": null }

            ],
            "aoColumnDefs": [
         {
             "aTargets": [0],
             "sClass": "word-wrap"

         },
         {
             "aTargets": [1],
             "sClass": "word-wrap sheetCol"

         },
          {
              "aTargets": [2],
              "sClass": "word-wrap"

          }, {
              "aTargets": [3],
              "sClass": "red"

          },
         {
             "aTargets": [4],
             "sClass": "column",

             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                 var b = $($Employeefimport.xlSheetColumn(sData.Name));
                 $(nTd).empty();
                 $(nTd).prepend(b);
             }

         }

            ],
            ajax: function (data, callback, settings) {
                setTimeout(function () {
                    callback({
                        draw: data.draw,
                        data: datas,
                        recordsTotal: datas.length,
                        recordsFiltered: datas.length
                    });
                }, 50);

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
    xlSheetColumn: function (selectId) {
        debugger;
        var selec = selectId;
        selec = selec.split(" ").join("");
        var data = null;
        for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
            if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == $Employeefimport.selectedSheet) {
                data = $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns;
            }
        }

        var select = '<select id=' + selec + ' onchange="$Employeefimport.alreadyMapped(this.id)"><option value=0>--select--</option>'
        for (var cnt = 0; cnt < data.length; cnt++) {
            select = select + '<option value="' + data[cnt].mapColumn + '">' + data[cnt].mapColumn + '</option>'
        }
        select = select + '</select>';
        return select;
    },

    renderSheetTree: function (data) {

        var treeData = [];
        for (var cnt = 0; cnt < data.length; cnt++) {
            treeData.push({ key: data[cnt].sheetName, title: data[cnt].sheetName });
        }
        //initialize the tree
        $("#xlSheettree").fancytree();
        //before load clean the tree nodes
        $("#xlSheettree").fancytree("destroy");
        fancyTree = jQuery("#xlSheettree").fancytree({
            extensions: ["contextMenu", "dnd", "filter"],
            selectMode: 3,
            strings: {
                loading: "Loading..."
            },
            autoScroll: true,
            source: treeData,
            filter: {
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
            dnd: {
                preventVoidMoves: true, // Prevent dropping nodes 'before self', etc.
                preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
                autoExpandMS: 400,
                draggable: {
                    //                        zIndex: 1000,
                    //                        appendTo: "body",
                    scroll: false,
                    revert: "invalid"
                },
                dragStart: function (node, data) {
                    return true;
                },
                dragEnter: function (node, data) {
                    return true;
                },
                dragDrop: function (node, data) {
                    return false;
                }
            },
            // checkbox: (("1" === "1") ? true : false),
            click: function (event, data) {

            },
            init: function (event, ctx) {
                ctx.tree.debug("init");
                ctx.tree.rootNode.fixSelection3FromEndNodes();
            },
            loadchildren: function (event, ctx) {
                ctx.tree.debug("loadchildren");
                ctx.node.fixSelection3FromEndNodes();
            },
            select: function (event, data) {

            },
            contextMenu: {
                menu: function (node) {

                },
                actions: function (node, action, options) {

                }
            }

        });
    },
    renderAttributeTree: function (data) {

        var treeData = [];
        if (data != null) {
            for (var cnt = 0; cnt < data.length; cnt++) {
                treeData.push({ key: data[cnt].Name, title: data[cnt].MappedColumnName != null && data[cnt].MappedColumnName.length > 0 ? data[cnt].Name + '-' + data[cnt].MappedColumnName : data[cnt].Name });
            }
        }
        //initialize the tree
        $("#attributetree").fancytree();
        //before load clean the tree nodes
        $("#attributetree").fancytree("destroy");
        fancyTree = jQuery("#attributetree").fancytree({
            extensions: ["contextMenu", "dnd", "filter"],
            selectMode: 3,
            strings: {
                loading: "Loading..."
            },
            autoScroll: true,
            source: treeData,
            filter: {
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
            dnd: {
                preventVoidMoves: true, // Prevent dropping nodes 'before self', etc.
                preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
                autoExpandMS: 400,
                draggable: {
                    //                        zIndex: 1000,
                    //                        appendTo: "body",
                    scroll: false,
                    revert: "invalid"
                },
                dragStart: function (node, data) {
                    return true;
                },
                dragEnter: function (node, data) {
                    return true;
                },
                dragDrop: function (node, data) {
                    if (data.otherNode.tree.$div[0].id != 'xlColumntree') {
                        return false;
                    }

                    //for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                    //    if ($Employeefimport.fileResponseData[1][cnt].Name == $Employeefimport.selectedModel) {
                    //        for (var chld = 0; chld < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; chld++) {
                    //            if ($Employeefimport.fileResponseData[1][cnt].ImportColumns[chld].Name == node.key) {
                    //                $Employeefimport.fileResponseData[1][cnt].ImportColumns[chld].MappedColumnName = data.otherNode.key;
                    //            }

                    //        }

                    //    }
                    //}
                    if (!$Employeefimport.CheckMapped(false, data.otherNode)) {
                        $Employeefimport.map(false, node, data.otherNode);
                        $Employeefimport.loadColumn();
                    }
                    else {
                        $app.showAlert('You have already mapped this column with other column', 3);
                        return false;
                    }
                }
            },
            // checkbox: (("1" === "1") ? true : false),
            click: function (event, data) {
            },
            init: function (event, ctx) {
                ctx.tree.debug("init");
                ctx.tree.rootNode.fixSelection3FromEndNodes();
            },
            loadchildren: function (event, ctx) {

                ctx.tree.debug("loadchildren");
                ctx.node.fixSelection3FromEndNodes();
            },
            select: function (event, data) {

            },
            contextMenu: {
                menu: function (node) {
                    var name = $Employeefimport.getMappedName(false, node);
                    if (name != '' && name != null) {
                        return {
                            'UnMap': { 'name': 'UnMap Column', 'icon': 'view' }
                        }
                    }
                    else {
                        return {};
                    }

                },
                actions: function (node, action, options) {
                    if (action === "UnMap") {
                        $Employeefimport.unmap(false, node);
                    }

                }
            }

        });

        $('#attributetree ul li .fancytree-title').each(function () {
            if ($(this).text().indexOf("-") >= 0) {
                $(this).css('background-color', "#1fb5ac")
            } else {
                $(this).css('background-color', "white")
            }
        });
    },
    renderXlColumnTree: function (data) {

        var treeData = [];
        if (data != null) {
            for (var cnt = 0; cnt < data.length; cnt++) {
                treeData.push({ key: data[cnt].mapColumn, title: data[cnt].mapColumn });
            }
        }
        //initialize the tree
        $("#xlColumntree").fancytree();
        //before load clean the tree nodes
        $("#xlColumntree").fancytree("destroy");
        fancyTree = jQuery("#xlColumntree").fancytree({
            extensions: ["contextMenu", "dnd", "filter"],
            selectMode: 3,
            strings: {
                loading: "Loading..."
            },
            autoScroll: true,
            source: treeData,
            filter: {
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
            dnd: {
                preventVoidMoves: true, // Prevent dropping nodes 'before self', etc.
                preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
                autoExpandMS: 400,
                draggable: {
                    //                        zIndex: 1000,
                    //                        appendTo: "body",
                    scroll: false,
                    revert: "invalid"
                },
                dragStart: function (node, data) {
                    return true;
                },
                dragEnter: function (node, data) {
                    return false;
                },
                dragDrop: function (node, data) {
                    return false;
                }
            },
            // checkbox: (("1" === "1") ? true : false),
            click: function (event, data) {
            },
            init: function (event, ctx) {
                ctx.tree.debug("init");
                ctx.tree.rootNode.fixSelection3FromEndNodes();
            },
            loadchildren: function (event, ctx) {
                ctx.tree.debug("loadchildren");
                ctx.node.fixSelection3FromEndNodes();
            },
            select: function (event, data) {

            },
            contextMenu: {
                menu: function (node) {
                    return false;
                },
                actions: function (node, action, options) {
                    return false;
                }
            }

        });
    },
    getMappedName: function (isSheet, node) {

        var name = '';
        if (isSheet) {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                if ($Employeefimport.fileResponseData[1][cnt].Name == node.key) {
                    name = $Employeefimport.fileResponseData[1][cnt].MappedSheet;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                if ($Employeefimport.fileResponseData[1][cnt].Name == $Employeefimport.selectedModel) {
                    for (var chld = 0; chld < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; chld++) {
                        if ($Employeefimport.fileResponseData[1][cnt].ImportColumns[chld].Name == node.key) {
                            name = $Employeefimport.fileResponseData[1][cnt].ImportColumns[chld].MappedColumnName;
                        }
                    }
                }
            }
        }
        return name;
    },
    unmap: function (isSheet, node) {

        var name = $Employeefimport.getMappedName(isSheet, node);
        $Employeefimport.unMapSheet(isSheet, name);
        if (isSheet) {
            $Employeefimport.selectedModel = null;
            $Employeefimport.renderModelTree($Employeefimport.fileResponseData[1]);
            $Employeefimport.loadColumnName();
        }
        else {
            $Employeefimport.loadColumn();
        }

    },
    unMapSheet: function (isSheet, name) {

        if (isSheet) {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == name) {
                    $Employeefimport.fileResponseData[0].XlSheet[cnt].isMapped = false;
                    for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
                    }
                }
            }
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                if ($Employeefimport.fileResponseData[1][cnt].MappedSheet == name) {
                    $Employeefimport.fileResponseData[1][cnt].MappedSheet = null;
                    for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {

                        $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
                    }
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == $Employeefimport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == name) {
                            $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
                        }
                    }
                }
            }
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                if ($Employeefimport.fileResponseData[1][cnt].MappedSheet == $Employeefimport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                        if ($Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName == name) {

                            $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
                        }
                    }
                }
            }
        }
    },
    map: function (isSheet, node, othernode) {

        if (isSheet) {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == othernode.key) {
                    $Employeefimport.fileResponseData[0].XlSheet[cnt].isMapped = true;
                }
            }
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                if ($Employeefimport.fileResponseData[1][cnt].Name == node.key) {
                    $Employeefimport.fileResponseData[1][cnt].MappedSheet = othernode.key;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == $Employeefimport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == othernode.key) {
                            $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = true;
                        }
                    }
                }
            }
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {

                if ($Employeefimport.fileResponseData[1][cnt].MappedSheet == $Employeefimport.selectedSheet) {

                    for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {

                        if ($Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].Name == node.key) {

                            $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = othernode.key;
                        }
                    }
                }
            }
        }
    },
    CheckMapped: function (isSheet, node) {

        var isMaped = false;
        if (isSheet) {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == node.key) {

                    isMaped = $Employeefimport.fileResponseData[0].XlSheet[cnt].isMapped;
                    break;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == $Employeefimport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == node.key) {
                            isMaped = $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped;
                            break;
                        }
                    }
                }
            }
        }
        return isMaped;
    },
    importProcess: function () {
        debugger;
        var err = 0;
        var IsAmem = false;
        if ($("#ddUImportType").val() == "2") {
            IsAmem = true;
        }
        var errorlistm = "Please map those columns : ";
        for (var cnt = 0; cnt < selectedloadColumnName.length; cnt++) {
            if (selectedloadColumnName[cnt].IsRequiredstr == "Yes" && (selectedloadColumnName[cnt].MappedColumnName == "" || selectedloadColumnName[cnt].MappedColumnName == null)) {                
                if ($("#ddUImportType").val() == "2" && (selectedloadColumnName[cnt].Name == "Employee Code" || selectedloadColumnName[cnt].Name == "Month" || selectedloadColumnName[cnt].Name == "Year" || selectedloadColumnName[cnt].Name == "Salary Grade")) {
                    err = 1;
                    errorlistm = errorlistm + selectedloadColumnName[cnt].Name + ",";
                }
                else if ($("#ddUImportType").val() == "1") {
                    err = 1;
                    errorlistm = errorlistm + selectedloadColumnName[cnt].Name + ",";
                }
            }
        }
        debugger;
        errorlistm = errorlistm.replace(/,\s*$/, "");

        if ($("#ddUImportType").val() == "0") {
            $app.showAlert("Please select the Type", 4);
            err = 2;
        }
        if (err == 1) {
            $app.showAlert(errorlistm, 4);
            $app.hideProgressModel();
            return false;
        }
        if (err == 2) {
            $app.hideProgressModel();
            return false;
        }
       
        debugger;
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Util/ProcessImport",
            data: JSON.stringify({
                xlImport: $Employeefimport.fileResponseData[0], table: $Employeefimport.fileResponseData[1], IsAmendment: IsAmem,
                addMaster: false, startRow: $('#startRow').val(),
                endRow: $('#endRow').val(), fromRange: $("#rangeFrom").val().toUpperCase(), toRange: $("#rangeTo").val().toUpperCase()
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        // $app.clearControlValues(context.id);
                        $app.hideProgressModel();
                        if (jsonResult.result.length <= 0) {
                            $('#dvErrorMsg').html('');
                            $app.showAlert(jsonResult.Message, 2);
                        }
                        else {
                            $app.showAlert('There is some error while importing.Please refer error messages below.', 3);
                            $Employeefimport.showImportError(jsonResult.result);
                        }

                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
                $('.AddPopup').on('click', function (event) {

                    var filecat = event.target.className.split('^');
                    var files = filecat[0].split(/\s+/).pop();
                    var files00 = files + '^' + filecat[1];
                    $Employeefimport.savePopup(files00, this.id);
                    return false;
                });
            }
        });
    },
    showImportError: function (Errordata) {
        $('#dvErrorMsg').html('');
        $.each(Errordata, function (index, elmt) {
            $('#dvErrorMsg').append('<p>' + elmt + '</p>')
        });
        $('#DvErrorPopup').removeClass('hide');

    },
    //Template related work
    loadTemplate: function (dropControl) {
        $('#dvTemplate').removeClass('hide');
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Util/GetImportTemplates",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                var out = jsonResult.result;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(out, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.id).html(blood.name));
                });
                if ($Employeefimport.selectedTemplateId == '' || $Employeefimport.selectedTemplateId == '00000000-0000-0000-0000-000000000000') {
                    $('#' + dropControl.id).val('00000000-0000-0000-0000-000000000000')
                }
                else {
                    $('#' + dropControl.id).val($Employeefimport.selectedTemplateId);
                }
            },
            error: function (msg) {
            }
        });
    },
    loadTemplateDetail: function () {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Util/GetImportTemplate",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ templateId: $Employeefimport.selectedTemplateId }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                var out = jsonResult.result;
                $Employeefimport.unMapAll();
                $Employeefimport.TemplateMap(out);

            },
            error: function (msg) {
            }
        });
    },
    TemplateMap: function (data) {
        debugger;
        var ExistsObj = [];
        var buildObj = [];
        $.each(data.jsonImportTemplateDetails, function (ind, item) {
            if ($.inArray(item.tableName, ExistsObj) > -1) {
                //do something    
            }
            else {
                ExistsObj.push(item.tableName);
                buildObj.push({ mappedSheetName: item.mappedSheetName, tableName: item.tableName });
            }

        });

        //data.jsonImportTemplateDetails
        $.each(buildObj, function (ind, item) {
            for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
                //if ($Employeefimport.fileResponseData[0].XlSheet[cnt].sheetName == item.mappedSheetName) {
                $Employeefimport.fileResponseData[0].XlSheet[cnt].isMapped = true;
                var colms = $Employeefimport.getSheetColumn(data, item.tableName);
                $.each(colms, function (colInd, col) {
                    for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == col.mappedSheetColumn) {

                            $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = true;
                        }
                    }

                });

                //Table mapped
                for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
                    if ($Employeefimport.fileResponseData[1][cnt].Name == item.tableName) {
                        //$Employeefimport.fileResponseData[1][cnt].MappedSheet = item.mappedSheetName;
                        $.each(colms, function (colInd, col) {
                            for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                                var colVal = $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].Name
                                if ($Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId != '' && $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId != '00000000-0000-0000-0000-000000000000') {
                                    colVal = $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId;
                                }
                                debugger;
                                if (colVal.toUpperCase() == col.tableColumn.toUpperCase()) {
                                    $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = col.mappedSheetColumn;
                                }
                            }
                        });
                    }
                    //}
                    //
                }
                //Table mapped
                for (var cnt = 0; cnt < selectedloadColumnName.length; cnt++) {
                    //if ($Employeefimport.fileResponseData[1][cnt].Name == item.tableName) {
                    //$Employeefimport.selectedloadColumnName[cnt].MappedSheet = item.mappedSheetName;
                    $.each(colms, function (colInd, col) {
                        //for (var tCnt = 0; tCnt < $Employeefimport.selectedloadColumnName[cnt].length; tCnt++) {
                        var colVal = selectedloadColumnName[cnt].Name
                        if (selectedloadColumnName[cnt].OtherTableUniqueId != '' && selectedloadColumnName[cnt].OtherTableUniqueId != '00000000-0000-0000-0000-000000000000') {
                            colVal = selectedloadColumnName[cnt].OtherTableUniqueId;
                        }
                        debugger;
                        if (colVal.toUpperCase() == col.tableColumn.toUpperCase()) {
                            selectedloadColumnName[cnt].MappedColumnName = col.mappedSheetColumn;
                        }
                        //}
                    });
                    //}
                    //}
                    //
                }
            }

        });
        $Employeefimport.renderModelTree($Employeefimport.fileResponseData[1]);
        $Employeefimport.loadColumnName(selectedloadColumnName);
        // $Employeefimport.loadColumn();
    },
    getSheetColumn: function (data, table) {
        var retObj = [];
        $.each(data.jsonImportTemplateDetails, function (ind, item) {
            if (item.tableName == table) {
                retObj.push({ tableColumn: item.tableColumn, mappedSheetColumn: item.mappedSheetColumn });
            }

        });
        return retObj;
    },
    unMapAll: function () {

        for (var cnt = 0; cnt < $Employeefimport.fileResponseData[0].XlSheet.length; cnt++) {
            $Employeefimport.fileResponseData[0].XlSheet[cnt].isMapped = false;
            for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                $Employeefimport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
            }
        }
        for (var cnt = 0; cnt < $Employeefimport.fileResponseData[1].length; cnt++) {
            $Employeefimport.fileResponseData[1][cnt].MappedSheet = null;
            for (var tCnt = 0; tCnt < $Employeefimport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                $Employeefimport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
            }
        }

    },

    //Add Popup Values
    savePopup: function (data, btnId) {

        $app.showProgressModel();
        var formData = document.forms["frmPopup"];
        var dataValue = data.split('^');

        var data = {
            id: '',
            popuplalue: dataValue[1],
            isApplicable: '',
            employerCode: '',
            type: dataValue[0]

        };
        $.ajax({
            url: $app.baseUrl + "Company/SavePopup",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        $('#' + btnId).html(dataValue[1] + ' Added');
                        $('#' + btnId).attr('disabled', 'disabled');
                        //$comPopup.createPopupGrid();
                        //$comPopup.selectedId = '';
                        $app.hideProgressModel();
                        $app.showAlert(dataValue[1] + " Saved in " + dataValue[0], 2);
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
};
