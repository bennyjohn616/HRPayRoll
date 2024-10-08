$("#btnDeclarationUpload").on('click', function (event) {

    $DeclarationTaximport.uploadfile();

    return false;
});
//master report  generation
$("#btnSampleExcel").on('click', function (event) {
    if ($('#sltImportFile').val() != "0") {
        $DeclarationTaximport.ExportTemplate();
    }
    else {
        $app.showAlert("Please Select Import File", 3);
    }
    return false;
});
$('#fDeclarationUpload').on('change', function (e) {

    $('#xlSheetdrop').html('');
    $('#dvTxtbox').html('');
    $('#dvColoumn').html('');
    $('#DeclarationentityModeltree').html('');
    $('#DeclarationrangeFrom,#DeclarationrangeTo').attr('readonly', false);

    var files = e.target.files;
    if (files.length > 0) {
        var file = this.files[0];
        fileName = file.name;
        var re = /\..+$/;
        var ext = fileName.match(re);
        $("#btnDeclarationUpload").removeClass('hide');
        $("#btnDeclarationUpload").hide();
        size = file.size;

        if (ext[0] == '.xls') {
            if (size > 5242880) {
                $('#fDeclarationUpload').val('');
                $app.showAlert('Invalid file size, the maximum file size is 5 MB', 4);
                $DeclarationTaximport.fileData = null;
                return false;
            }
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append(files[x].name, files[x]);
                }
                $DeclarationTaximport.fileData = data;
                $("#btnDeclarationUpload").show();
            }
            else {
                $('#fDeclarationUpload').val('');
                $app.showAlert("This browser doesn't support HTML5 file uploads!", 4);
                $DeclarationTaximport.fileData = null;
                return false;
            }
        }
        else {
            $('#fDeclarationUpload').val('');
            $app.showAlert('Invalid file, please select ".xls" file', 4);
            $DeclarationTaximport.fileData = null;
            return false;
        }
    }
});
//Created by Babu.R as on 12-May-2018
$('#btnDeclarationImport').on('click', function (e) {
    
    if ($('#sltImportFile').val() != "0") {
        if ($('#ddTaxImportMonth').val() != "0") {
            if ($('#txtTaxImportYear').val() != "") {
                if ($('#xlSheetdrop').val() != null) {
                    if ($('#xlSheetdrop').val() != "0") {
                        if ($('#srartRow').val() != "" && $('#endRow').val() != "") {
                            $DeclarationTaximport.importProcess();
                        }
                        else {
                            $app.showAlert("Please enter Start Row and End Row ", 3);
                        }
                    }
                    else {
                        $app.showAlert("Please Select sheet ", 3);
                    }
                }
                else {
                    $app.showAlert("Please Upload file ", 3);
                }
            }
            else {
                $app.showAlert("Please Select Effective Month ", 3);
            }
        }
        else {
            $app.showAlert("Please Enter Effective Year ", 3);
        }
    }
    else {
        $app.showAlert("Please Select Import File", 3);
    }
    return false;
});
$('#btnDeclarationCloseError').on('click', function (e) {

    // $DeclarationTaximport.importProcess();
    $('#DvDeclarationErrorPopup').addClass('hide');
    return false;

});

$('#sltDeclarationImportTemplate').on('change', function (e) {

    $DeclarationTaximport.selectedTemplateId = $('#sltDeclarationImportTemplate').val();
    if (confirm('Do you want to refresh the mapping based on the template?')) {
        $DeclarationTaximport.loadTemplateDetail();
    }
    return false;

});
$('#btnDeclarationEdit').on('click', function (e) {

    // $('#AddDeclarationTemplate').modal('toggle');
    $DeclarationTaximport.canTemplateSave = true;
    $('#txtDeclarationTemplateName').val($('#sltDeclarationImportTemplate').find('option:selected').text());
    $('#chkDeclarationCurrentSetting').prop('checked', false);

});
$('#btnDeclarationAddNew').on('click', function (e) {

    $DeclarationTaximport.canTemplateSave = true;
    $DeclarationTaximport.selectedTemplateId = '';
    $('#txtDeclarationTemplateName').val('');
    $('#chkDeclarationCurrentSetting').prop('checked', false);
    // return false;

});
$("input[name=DeclarationsearchTables]").keyup(function (e) {

    var treeT = $("#DeclarationentityModeltree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("button#btnDeclarationResetSearchTables").click();
        return;
    }
    var n = treeT.applyFilter(match);
    $("button#btnDeclarationResetSearchTables").attr("disabled", false);
}).focus();

$("button#btnDeclarationResetSearchTables").click(function (e) {

    var treeT = $("#DeclarationentityModeltree").fancytree("getTree");
    $("input[name=DeclarationsearchTables]").val("");
    treeT.clearFilter();
}).attr("disabled", true);

$("input#DeclarationhideModeTables").change(function (e) {

    var treeT = $("#DeclarationentityModeltree").fancytree("getTree");
    treeT.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    treeT.clearFilter();
    $("input[name=DeclarationsearchTables]").keyup();
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
        $DeclarationTaximport.loadColumnName();
    }

});
$("#selectValue").change(function () {

    var selectedText = $(this).find("option:selected").text();
    var selectedValue = $(this).val();
    alert("Selected Text: " + selectedText + " Value: " + selectedValue);
});


$('#frmDeclarationTemplate').on('submit', function (e) {

    $DeclarationTaximport.retriveData();
    if (!$DeclarationTaximport.canTemplateSave)
        return false;
    $DeclarationTaximport.canTemplateSave = false;
    var saveObj = {
        id: $DeclarationTaximport.selectedTemplateId,
        name: $('#txtDeclarationTemplateName').val()
    };
    var jsonImportTemplateDetails = [];
    if ($DeclarationTaximport.fileResponseData != null) {
        $.each($DeclarationTaximport.fileResponseData[1], function (ind, item) {
            if (item.MappedSheet != '' && item.MappedSheet != null) {
                $.each(item.ImportColumns, function (chiInd, childItem) {
                    var tempDetails = {
                        id: '',
                        importTemplateId: $DeclarationTaximport.selectedTemplateId,
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
    $app.showProgressModel();
    $.ajax({
        type: 'POST',
        url: $app.baseUrl + "TaxUtil/SaveImportTemplate",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ importTemplate: saveObj, currentSetting: $('#chkDeclarationCurrentSetting').prop('checked') }),
        dataType: "json",
        success: function (jsonResult) {
            $app.clearSession(jsonResult);
            switch (jsonResult.Status) {
                case true:
                    $DeclarationTaximport.loadTemplate({ id: 'sltDeclarationImportTemplate' });
                    $('#AddDeclarationTemplate').modal('toggle');
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

$DeclarationTaximport = {
    canTemplateSave: false,
    fileData: null,
    fileResponseData: null,
    selectedModel: '',
    selectedSheet: '',
    selectedTemplateId: '',
    uploadfile: function () {

        $('#dvDeclarationErrorMsg').html('');
        if ($('#DeclarationrangeFrom').val() != "" && $('#DeclarationrangeTo').val() != "") {
            $DeclarationTaximport.fileData.append("fromRange", $("#DeclarationrangeFrom").val().toUpperCase())
            $DeclarationTaximport.fileData.append("toRange", $("#DeclarationrangeTo").val().toUpperCase());
        }
        else {

            $app.showAlert("Please enter FromRange and ToRange ", 3);
            return false;
        }

        $.ajax({
            type: "POST",
            url: $app.baseUrl + 'TaxUtil/UploadFile',
            contentType: false,
            processData: false,
            data: $DeclarationTaximport.fileData,

            success: function (result) {

                $DeclarationTaximport.fileResponseData = result;
                $DeclarationTaximport.renderModelTree(result[1]);
                // $DeclarationTaximport.renderSheetTree(result[0].XlSheet);
                $DeclarationTaximport.xlSheetdrop(result[0].XlSheet);
                $DeclarationTaximport.renderAttributeTree(null);
                $DeclarationTaximport.renderXlColumnTree(null);
                $DeclarationTaximport.loadTemplate({ id: 'sltDeclarationImportTemplate' });
                $('#DeclarationrangeFrom,#DeclarationrangeTo').attr('readonly', true);
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
            }
        });
    },
    renderModelTree: function (data) {

        var treeData = [];
        for (var cnt = 0; cnt < data.length; cnt++) {
            treeData.push({ key: data[cnt].Name, title: data[cnt].MappedSheet != null ? data[cnt].Name + '-' + data[cnt].MappedSheet : data[cnt].Name });
            if (data[cnt].MappedSheet != null) {
                $("#xlSheetdrop").val(data[cnt].MappedSheet)
                $DeclarationTaximport.selectedModel = data[cnt].Name;
            }
        }
        //initialize the tree
        $("#DeclarationentityModeltree").fancytree();
        //before load clean the tree nodes
        $("#DeclarationentityModeltree").fancytree("destroy");
        fancyTree = jQuery("#DeclarationentityModeltree").fancytree({
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
                    //for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                    //    if ($DeclarationTaximport.fileResponseData[1][cnt].Name == node.key) {
                    //        $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet = data.otherNode.key;
                    //    }
                    //}

                    if (!$DeclarationTaximport.CheckMapped(true, data.otherNode)) {
                        $DeclarationTaximport.map(true, node, data.otherNode);
                        $DeclarationTaximport.renderModelTree($DeclarationTaximport.fileResponseData[1]);
                    }
                    else {
                        $app.showAlert('You have already mapped this sheet with other table', 3);
                        return false;
                    }
                }
            },
            // checkbox: (("1" === "1") ? true : false),
            click: function (event, data) {

                $DeclarationTaximport.selectedModel = data.node.key;
                //$DeclarationTaximport.loadColumn();
                $DeclarationTaximport.loadColumnName();
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
                    var name = $DeclarationTaximport.getMappedName(true, node);
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
                        $DeclarationTaximport.unmap(true, node);
                    }
                }
            }

        });

        $('#DeclarationentityModeltree ul li .fancytree-title').each(function () {
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
        $DeclarationTaximport.selectedSheet = '';
        for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
            if ($DeclarationTaximport.fileResponseData[1][cnt].Name == $DeclarationTaximport.selectedModel) {
                attribute = $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns;
                $DeclarationTaximport.selectedSheet = $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet;
            }
        }
        $DeclarationTaximport.renderAttributeTree(attribute);
        for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
            if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == $DeclarationTaximport.selectedSheet) {
                xlColumns = $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns;
            }
        }
        $DeclarationTaximport.renderXlColumnTree(xlColumns);
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
    //alreadyMapped: function (selectId) {

    //    var find = '#' + selectId + ' option:selected'
    //    var currentVal = $(".column").find(find).val();
    //    var count = 0;
    //    var rows = $("#tbl_tblImportColumn").dataTable().fnGetNodes();
    //    var column = [];
    //    for (var i = 0; i < rows.length; i++) {
    //        column[i] = $(rows[i]).find(".column option:selected").val();

    //    }

    //    for (var j = 0 ; j < column.length; j++) {

    //        if (currentVal == column[j]) {

    //            count++;
    //            if (count > 1) {
    //                $app.showAlert('Already mapped', 3);
    //                $(".column").find('#' + selectId).val("0");
    //            }
    //        }

    //    }

    //},
    retriveData: function () {
        var rows = $("#tbl_tblImportColumn").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();

            if ($(rows[i]).find(".column option:selected").val() != 0) {
                newattr.sheetcol = $(rows[i]).find(".sheetCol").text().trim();
                newattr.xlcol = $(rows[i]).find(".column option:selected").val();
                for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                    if ($DeclarationTaximport.fileResponseData[1][cnt].MappedSheet == $DeclarationTaximport.selectedSheet) {
                        for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                            if ($DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].Name == newattr.sheetcol) {
                                $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = newattr.xlcol;
                            }

                        }
                    }
                }
                for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
                    if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == $DeclarationTaximport.selectedSheet) {
                        for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                            if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == newattr.xlcol) {
                                $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = true;
                            }

                        }
                    }
                }
            }
        }
    },
    //created by Babu.R as on 12-May-2018
    loadColumnName: function () {

        var attribute = [];
        $DeclarationTaximport.selectedSheet = '';
        for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
            if ($DeclarationTaximport.fileResponseData[1][cnt].Name == $DeclarationTaximport.selectedModel) {
                attribute = $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns;
                $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet = $('#xlSheetdrop').val();
                $DeclarationTaximport.selectedSheet = $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet;
            }
            else {
                $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet = null;
            }
        }
        $DeclarationTaximport.renderModelTree($DeclarationTaximport.fileResponseData[1]);

        for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
            if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == $DeclarationTaximport.selectedSheet) {
                $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].isMapped = true;
            }
            else {
                $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].isMapped = false;
            }
        }
        //var gridObject = [];
        //gridObject.push({ tableHeader: "Column", tableValue: 'Column' });
        //gridObject.push({ tableHeader: "Excel Column", tableValue: 'Excel Column' });
        //$DeclarationTaximport.renderFieldGrid(gridObject, { id: "tblImportColumn" });
        $DeclarationTaximport.renderFieldGrid();
        //if ($("#sltDeclarationImportTemplate").val() != "00000000-0000-0000-0000-000000000000") {
        //    $DeclarationTaximport.LoadAttributeModelsTemplate(attribute);
        //}
        //else {
        //    $DeclarationTaximport.LoadAttributeModels(attribute);
        //}
    },
    renderFieldGrid: function () {

        $("#dvTxtbox").html('');
        $("#dvTxtbox").html('<div class="col-md-6"><lable class="font-weight: bold">StartRow:  </lable><input type="text" id="startRow" onkeypress="return $validator.IsNumeric(event, this.id)"></div><div class="col-md-6  "><lable class="font-weight: bold">EndRow:  </lable><input type="text" id="endRow" onkeypress="return $validator.IsNumeric(event, this.id)"></div>');
        //var grid = '<table id="tbl_' + tableprop.id + '" class="table table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer" style="display:none">'
        //    + '<thead>'
        //            + '<tr>'
        //for (var cnt = 0; cnt < context.length; cnt++) {
        //    grid = grid + '<th>' + context[cnt].tableHeader + '</th>'
        //}
        //grid = grid + '</tr></thead>';
        //grid = grid + '<tbody><tr>'
        //for (var cnt = 0; cnt < context.length; cnt++) {//for action td 
        //    grid = grid + '<td></td>';
        //}
        //grid = grid + '</tr></tbody></table>';


        //$("#dvColoumn").html(grid);
    },
    //LoadAttributeModelsTemplate: function (datas) {

    //    var dtClientList = $('#tbl_tblImportColumn').DataTable({
    //        'iDisplayLength': 20,
    //        'bPaginate': true,
    //        'sPaginationType': 'full',
    //        'sDom': '<"top">rt<"bottom"ip><"clear">',
    //        columns: [
    //         { "data": "Name" },
    //         { "data": "MappedColumnName" }

    //        ],
    //        "aoColumnDefs": [

    //     {
    //         "aTargets": [0],
    //         "sClass": "word-wrap sheetCol"

    //     },
    //     {
    //         "aTargets": [1],
    //         "sClass": "column",

    //         "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

    //             var b = $($DeclarationTaximport.xlSheetColumn(oData.Name));
    //             $(nTd).empty();
    //             $(nTd).prepend(b);
    //             if (sData == "" || sData == null) {
    //                 $(b).val("0");
    //             }
    //             else {
    //                 $(b).val(sData);
    //             }

    //         }

    //     }

    //        ],
    //        ajax: function (data, callback, settings) {
    //            setTimeout(function () {
    //                callback({
    //                    draw: data.draw,
    //                    data: datas,
    //                    recordsTotal: datas.length,
    //                    recordsFiltered: datas.length
    //                });
    //            }, 50);

    //        },
    //        fnInitComplete: function (oSettings, json) {

    //        },
    //        dom: "rtiS",
    //        "bDestroy": true,
    //        scroller: {
    //            loadingIndicator: true
    //        }
    //    });



    //},
    //LoadAttributeModels: function (datas) {

    //    var dtClientList = $('#tbl_tblImportColumn').DataTable({
    //        'iDisplayLength': 20,
    //        'bPaginate': true,
    //        'sPaginationType': 'full',
    //        'sDom': '<"top">rt<"bottom"ip><"clear">',
    //        columns: [
    //         { "data": "Name" },
    //         { "data": null }

    //        ],
    //        "aoColumnDefs": [

    //     {
    //         "aTargets": [0],
    //         "sClass": "word-wrap sheetCol"

    //     },
    //     {
    //         "aTargets": [1],
    //         "sClass": "column",

    //         "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

    //             var b = $($DeclarationTaximport.xlSheetColumn(sData.Name));
    //             $(nTd).empty();
    //             $(nTd).prepend(b);
    //         }

    //     }

    //        ],
    //        ajax: function (data, callback, settings) {
    //            setTimeout(function () {
    //                callback({
    //                    draw: data.draw,
    //                    data: datas,
    //                    recordsTotal: datas.length,
    //                    recordsFiltered: datas.length
    //                });
    //            }, 50);

    //        },
    //        fnInitComplete: function (oSettings, json) {

    //        },
    //        dom: "rtiS",
    //        "bDestroy": true,
    //        scroller: {
    //            loadingIndicator: true
    //        }
    //    });



    //},
    xlSheetColumn: function (selectId) {

        var selec = selectId;
        selec = selec.split(" ").join("");
        var data = null;
        for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
            if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == $DeclarationTaximport.selectedSheet) {
                data = $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns;
            }
        }

        var select = '<select id=' + selec + ' onchange="$DeclarationTaximport.alreadyMapped(this.id)"><option value=0>--select--</option>'
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

                    //for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                    //    if ($DeclarationTaximport.fileResponseData[1][cnt].Name == $DeclarationTaximport.selectedModel) {
                    //        for (var chld = 0; chld < $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns.length; chld++) {
                    //            if ($DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[chld].Name == node.key) {
                    //                $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[chld].MappedColumnName = data.otherNode.key;
                    //            }

                    //        }

                    //    }
                    //}
                    if (!$DeclarationTaximport.CheckMapped(false, data.otherNode)) {
                        $DeclarationTaximport.map(false, node, data.otherNode);
                        $DeclarationTaximport.loadColumn();
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
                    var name = $DeclarationTaximport.getMappedName(false, node);
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
                        $DeclarationTaximport.unmap(false, node);
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
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[1][cnt].Name == node.key) {
                    name = $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[1][cnt].Name == $DeclarationTaximport.selectedModel) {
                    for (var chld = 0; chld < $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns.length; chld++) {
                        if ($DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[chld].Name == node.key) {
                            name = $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[chld].MappedColumnName;
                        }
                    }
                }
            }
        }
        return name;
    },
    unmap: function (isSheet, node) {

        var name = $DeclarationTaximport.getMappedName(isSheet, node);
        $DeclarationTaximport.unMapSheet(isSheet, name);
        if (isSheet) {
            $DeclarationTaximport.selectedModel = null;
            $DeclarationTaximport.renderModelTree($DeclarationTaximport.fileResponseData[1]);
            $DeclarationTaximport.loadColumnName();
        }
        else {
            $DeclarationTaximport.loadColumn();
        }

    },
    unMapSheet: function (isSheet, name) {

        if (isSheet) {
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == name) {
                    $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].isMapped = false;
                    for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
                    }
                }
            }
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[1][cnt].MappedSheet == name) {
                    $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet = null;
                    for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                        $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
                    }
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == $DeclarationTaximport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == name) {
                            $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
                        }
                    }
                }
            }
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[1][cnt].MappedSheet == $DeclarationTaximport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                        if ($DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName == name) {
                            $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
                        }
                    }
                }
            }
        }
    },
    map: function (isSheet, node, othernode) {
        if (isSheet) {
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == othernode.key) {
                    $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].isMapped = true;
                }
            }
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[1][cnt].Name == node.key) {
                    $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet = othernode.key;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == $DeclarationTaximport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == othernode.key) {
                            $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = true;
                        }
                    }
                }
            }
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[1][cnt].MappedSheet == $DeclarationTaximport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                        if ($DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].Name == node.key) {
                            $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = othernode.key;
                        }
                    }
                }
            }
        }
    },
    CheckMapped: function (isSheet, node) {
        var isMaped = false;
        if (isSheet) {
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == node.key) {

                    isMaped = $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].isMapped;
                    break;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == $DeclarationTaximport.selectedSheet) {
                    for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == node.key) {
                            isMaped = $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped;
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
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "TaxUtil/ProcessImport",
            data: JSON.stringify({
                xlImport: $DeclarationTaximport.fileResponseData[0], table: $('#xlSheetdrop').val(), startRow: $('#startRow').val(), endRow: $('#endRow').val(), fromRange: $("#DeclarationrangeFrom").val().toUpperCase(), toRange: $("#DeclarationrangeTo").val().toUpperCase(), EffMonth: $('#ddTaxImportMonth').val(), EffYear: $('#txtTaxImportYear').val(), ImportFile: $("#sltImportFile  :selected").text().trim()
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        if (jsonResult.result.length <= 0) {
                            $('#dvDeclarationErrorMsg').html('');
                            $app.showAlert(jsonResult.Message, 2);
                        }
                        else {
                            $app.showAlert('There is some error while importing.Please refer error messages below.', 3);
                            $DeclarationTaximport.showImportError(jsonResult.result);
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

                    var files = event.target.className.split(/\s+/).pop();
                    $DeclarationTaximport.savePopup(files, this.id);
                    return false;
                });
            }
        });
    },
    showImportError: function (Errordata) {
        $('#dvDeclarationErrorMsg').html('');
        $.each(Errordata, function (index, elmt) {
            $('#dvDeclarationErrorMsg').append('<p>' + elmt + '</p>')
        });
        $('#DvDeclarationErrorPopup').removeClass('hide');

    },
    //Template related work
    loadTemplate: function (dropControl) {
        $('#dvTemplate').removeClass('hide');
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxUtil/GetImportTemplates",
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
                if ($DeclarationTaximport.selectedTemplateId == '' || $DeclarationTaximport.selectedTemplateId == '00000000-0000-0000-0000-000000000000') {
                    $('#' + dropControl.id).val('00000000-0000-0000-0000-000000000000')
                }
                else {
                    $('#' + dropControl.id).val($DeclarationTaximport.selectedTemplateId);
                }
            },
            error: function (msg) {
            }
        });
    },
    loadTemplateDetail: function () {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxUtil/GetImportTemplate",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ templateId: $DeclarationTaximport.selectedTemplateId }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                var out = jsonResult.result;
                $DeclarationTaximport.unMapAll();
                $DeclarationTaximport.TemplateMap(out);

            },
            error: function (msg) {
            }
        });
    },
    TemplateMap: function (data) {

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

            for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
                if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].sheetName == item.mappedSheetName) {
                    $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].isMapped = true;
                    var colms = $DeclarationTaximport.getSheetColumn(data, item.tableName);
                    $.each(colms, function (colInd, col) {
                        for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                            if ($DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == col.mappedSheetColumn) {
                                $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = true;
                            }
                        }

                    });

                    //Table mapped
                    for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
                        if ($DeclarationTaximport.fileResponseData[1][cnt].Name == item.tableName) {
                            $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet = item.mappedSheetName;
                            $.each(colms, function (colInd, col) {
                                for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                                    var colVal = $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].Name
                                    if ($DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId != '' && $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId != '00000000-0000-0000-0000-000000000000') {
                                        colVal = $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId;
                                    }
                                    if (colVal.toUpperCase() == col.tableColumn.toUpperCase()) {
                                        $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = col.mappedSheetColumn;
                                    }
                                }
                            });
                        }
                    }
                    //

                }
            }

        });
        $DeclarationTaximport.renderModelTree($DeclarationTaximport.fileResponseData[1]);
        $DeclarationTaximport.loadColumnName();
        // $DeclarationTaximport.loadColumn();
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

        for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[0].XlSheet.length; cnt++) {
            $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].isMapped = false;
            for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                $DeclarationTaximport.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
            }
        }
        for (var cnt = 0; cnt < $DeclarationTaximport.fileResponseData[1].length; cnt++) {
            $DeclarationTaximport.fileResponseData[1][cnt].MappedSheet = null;
            for (var tCnt = 0; tCnt < $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                $DeclarationTaximport.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
            }

        }

    },
    ExportTemplate: function (data) {

        $.ajax({
            url: $app.baseUrl + "TaxUtil/GetSampleExcel",
            data: JSON.stringify({ ImportFile: $("#sltImportFile  :selected").text().trim() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        var oData = new Object();
                        oData.filePath = jsonResult.result;
                        $app.downloadSync('Download/DownloadPaySlip', oData);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.result, 'danger'); k
                        break;
                }


            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
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
