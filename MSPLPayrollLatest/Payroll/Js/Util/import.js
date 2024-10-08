$("#btnUpload").on('click', function (event) {

    $import.uploadfile();
    return false;
});

$('#fUpload').on('change', function (e) {

    var files = e.target.files;
    if (files.length > 0) {
        var file = this.files[0];
        fileName = file.name;
        var re = /\..+$/;
        var ext = fileName.match(re);
        $("#btnUpload").removeClass('hide');
        $("#btnUpload").hide();
        size = file.size;

        if (ext[0] == '.xls') {
            if (size > 5242880) {
                $('#fUpload').val('');
                $app.showAlert('Invalid file size, the maximum file size is 5 MB', 3);
                $import.fileData = null;
                return false;
            }
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append(files[x].name, files[x]);
                }
                $import.fileData = data;
                $("#btnUpload").show();
            }
            else {
                $('#fUpload').val('');
                $app.showAlert("This browser doesn't support HTML5 file uploads!", 3);
                $import.fileData = null;
                return false;
            }
        }
        else {
            $('#fUpload').val('');
            $app.showAlert('Invalid file, please select ".xls" file', 3);
            $import.fileData = null;
            return false;
        }
    }
});

$('#btnImport').on('click', function (e) {

    $import.importProcess();
    return false;

});
$('#btnCloseError').on('click', function (e) {

    // $import.importProcess();
    $('#DvErrorPopup').addClass('hide');
    return false;

});

$('#sltImportTemplate').on('change', function (e) {

    $import.selectedTemplateId = $('#sltImportTemplate').val();
    if (confirm('Do you want to refresh the mapping based on the template?')) {
        $import.loadTemplateDetail();
    }
    return false;

});
$('#btnEdit').on('click', function (e) {

    // $('#AddTemplate').modal('toggle');
    $import.canTemplateSave = true;
    $('#txtTemplateName').val($('#sltImportTemplate').find('option:selected').text());
    $('#chkCurrentSetting').prop('checked', false);

});
$('#btnAddNew').on('click', function (e) {

    $import.canTemplateSave = true;
    $import.selectedTemplateId = '';
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

$('#frmTemplate').on('submit', function (e) {

    if (!$import.canTemplateSave)
        return false;
    $import.canTemplateSave = false;
    var saveObj = {
        id: $import.selectedTemplateId,
        name: $('#txtTemplateName').val()
    };
    var jsonImportTemplateDetails = [];
    if ($import.fileResponseData != null) {
        $.each($import.fileResponseData[1], function (ind, item) {
            if (item.MappedSheet != '' && item.MappedSheet != null) {
                $.each(item.ImportColumns, function (chiInd, childItem) {
                    var tempDetails = {
                        id: '',
                        importTemplateId: $import.selectedTemplateId,
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
        url: $app.baseUrl + "Util/SaveImportTemplate",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ importTemplate: saveObj, currentSetting: $('#chkCurrentSetting').prop('checked') }),
        dataType: "json",
        success: function (jsonResult) {
            $app.clearSession(jsonResult);
            switch (jsonResult.Status) {
                case true:
                    $import.loadTemplate({ id: 'sltImportTemplate' });
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



/*
function upld() {

    var formData = new FormData($('#fUpload')[0]);
    $.ajax({
        url: $app.baseUrl + 'Util/UploadFile',  //Server script to process data
        type: 'POST',
        xhr: function () {  // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                alert('upload');
                //   myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // For handling the progress of the upload
            }
            return myXhr;
        },
        data: formData,
        //Options to tell jQuery not to process data or worry about content-type.
        cache: false,
        contentType: false,
        processData: false
    });
}
function progressHandlingFunction(e) {
    if (e.lengthComputable) {
        $("#divAddInforamtionDialog").show();
        $("#txtuploadedMsgAdd").text("  " + fileName + " uploaded successfully");
    }
}
*/

$import = {
    canTemplateSave: false,
    fileData: null,
    fileResponseData: null,
    selectedModel: '',
    selectedSheet: '',
    selectedTemplateId: '',
    uploadfile: function () {
        $('#dvErrorMsg').html('');
        $.ajax({
            type: "POST",
            url: $app.baseUrl + 'Util/UploadFile',
            contentType: false,
            processData: false,
            data: $import.fileData,
            async: false,
            success: function (result) {
                $import.fileResponseData = result;
                $import.renderModelTree(result[1]);
                $import.renderSheetTree(result[0].XlSheet);
                $import.renderAttributeTree(null);
                $import.renderXlColumnTree(null);
                $import.loadTemplate({ id: 'sltImportTemplate' });
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
                    //for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                    //    if ($import.fileResponseData[1][cnt].Name == node.key) {
                    //        $import.fileResponseData[1][cnt].MappedSheet = data.otherNode.key;
                    //    }
                    //}

                    if (!$import.CheckMapped(true, data.otherNode)) {
                        $import.map(true, node, data.otherNode);
                        $import.renderModelTree($import.fileResponseData[1]);
                    }
                    else {
                        $app.showAlert('You have already mapped this sheet with other table', 3);
                        return false;
                    }
                }
            },
            // checkbox: (("1" === "1") ? true : false),
            click: function (event, data) {
                $import.selectedModel = data.node.key;
                $import.loadColumn();
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
                    var name = $import.getMappedName(true, node);
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
                        $import.unmap(true, node);
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
        $import.selectedSheet = '';
        for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
            if ($import.fileResponseData[1][cnt].Name == $import.selectedModel) {
                attribute = $import.fileResponseData[1][cnt].ImportColumns;
                $import.selectedSheet = $import.fileResponseData[1][cnt].MappedSheet;
            }
        }
        $import.renderAttributeTree(attribute);
        for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
            if ($import.fileResponseData[0].XlSheet[cnt].sheetName == $import.selectedSheet) {
                xlColumns = $import.fileResponseData[0].XlSheet[cnt].xlColumns;
            }
        }
        $import.renderXlColumnTree(xlColumns);
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

                    //for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                    //    if ($import.fileResponseData[1][cnt].Name == $import.selectedModel) {
                    //        for (var chld = 0; chld < $import.fileResponseData[1][cnt].ImportColumns.length; chld++) {
                    //            if ($import.fileResponseData[1][cnt].ImportColumns[chld].Name == node.key) {
                    //                $import.fileResponseData[1][cnt].ImportColumns[chld].MappedColumnName = data.otherNode.key;
                    //            }

                    //        }

                    //    }
                    //}
                    if (!$import.CheckMapped(false, data.otherNode)) {
                        $import.map(false, node, data.otherNode);
                        $import.loadColumn();
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
                    var name = $import.getMappedName(false, node);
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
                        $import.unmap(false, node);
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
            for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                if ($import.fileResponseData[1][cnt].Name == node.key) {
                    name = $import.fileResponseData[1][cnt].MappedSheet;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                if ($import.fileResponseData[1][cnt].Name == $import.selectedModel) {
                    for (var chld = 0; chld < $import.fileResponseData[1][cnt].ImportColumns.length; chld++) {
                        if ($import.fileResponseData[1][cnt].ImportColumns[chld].Name == node.key) {
                            name = $import.fileResponseData[1][cnt].ImportColumns[chld].MappedColumnName;
                        }
                    }
                }
            }
        }
        return name;
    },
    unmap: function (isSheet, node) {
        var name = $import.getMappedName(isSheet, node);
        $import.unMapSheet(isSheet, name);
        if (isSheet) {
            $import.renderModelTree($import.fileResponseData[1]);
            $import.loadColumn();
        }
        else {
            $import.loadColumn();
        }

    },
    unMapSheet: function (isSheet, name) {
        if (isSheet) {
            for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
                if ($import.fileResponseData[0].XlSheet[cnt].sheetName == name) {
                    $import.fileResponseData[0].XlSheet[cnt].isMapped = false;
                    for (var tCnt = 0; tCnt < $import.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        $import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
                    }
                }
            }
            for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                if ($import.fileResponseData[1][cnt].MappedSheet == name) {
                    $import.fileResponseData[1][cnt].MappedSheet = null;
                    for (var tCnt = 0; tCnt < $import.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                        $import.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
                    }
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
                if ($import.fileResponseData[0].XlSheet[cnt].sheetName == $import.selectedSheet) {
                    for (var tCnt = 0; tCnt < $import.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == name) {
                            $import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
                        }
                    }
                }
            }
            for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                if ($import.fileResponseData[1][cnt].MappedSheet == $import.selectedSheet) {
                    for (var tCnt = 0; tCnt < $import.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                        if ($import.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName == name) {
                            $import.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
                        }
                    }
                }
            }
        }
    },
    map: function (isSheet, node, othernode) {
        if (isSheet) {
            for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
                if ($import.fileResponseData[0].XlSheet[cnt].sheetName == othernode.key) {
                    $import.fileResponseData[0].XlSheet[cnt].isMapped = true;
                }
            }
            for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                if ($import.fileResponseData[1][cnt].Name == node.key) {
                    $import.fileResponseData[1][cnt].MappedSheet = othernode.key;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
                if ($import.fileResponseData[0].XlSheet[cnt].sheetName == $import.selectedSheet) {
                    for (var tCnt = 0; tCnt < $import.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == othernode.key) {
                            $import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = true;
                        }
                    }
                }
            }
            for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                if ($import.fileResponseData[1][cnt].MappedSheet == $import.selectedSheet) {
                    for (var tCnt = 0; tCnt < $import.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                        if ($import.fileResponseData[1][cnt].ImportColumns[tCnt].Name == node.key) {
                            $import.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = othernode.key;
                        }
                    }
                }
            }
        }
    },
    CheckMapped: function (isSheet, node) {
        var isMaped = false;
        if (isSheet) {
            for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
                if ($import.fileResponseData[0].XlSheet[cnt].sheetName == node.key) {

                    isMaped = $import.fileResponseData[0].XlSheet[cnt].isMapped;
                    break;
                }
            }
        }
        else {
            for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
                if ($import.fileResponseData[0].XlSheet[cnt].sheetName == $import.selectedSheet) {
                    for (var tCnt = 0; tCnt < $import.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                        if ($import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == node.key) {
                            isMaped = $import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped;
                            break;
                        }
                    }
                }
            }
        }
        return isMaped;
    },
    importProcess: function () {

        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Util/ProcessImport",
            data: JSON.stringify({ xlImport: $import.fileResponseData[0], table: $import.fileResponseData[1], IsAmendment: $('input[name="IsAmendment"]').prop('checked'), addMaster: $('input[name="AddMasterValue"]').prop('checked') }),
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
                            $import.showImportError(jsonResult.result);
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
                    $import.savePopup(files, this.id);
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
                if ($import.selectedTemplateId == '' || $import.selectedTemplateId == '00000000-0000-0000-0000-000000000000') {
                    $('#' + dropControl.id).val('00000000-0000-0000-0000-000000000000')
                }
                else {
                    $('#' + dropControl.id).val($import.selectedTemplateId);
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
            data: JSON.stringify({ templateId: $import.selectedTemplateId }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                var out = jsonResult.result;
                $import.unMapAll();
                $import.TemplateMap(out);

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

            for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
                if ($import.fileResponseData[0].XlSheet[cnt].sheetName == item.mappedSheetName) {
                    $import.fileResponseData[0].XlSheet[cnt].isMapped = true;
                    var colms = $import.getSheetColumn(data, item.tableName);
                    $.each(colms, function (colInd, col) {
                        for (var tCnt = 0; tCnt < $import.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                            if ($import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].mapColumn == col.mappedSheetColumn) {
                                $import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = true;
                            }
                        }

                    });

                    //Table mapped
                    for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
                        if ($import.fileResponseData[1][cnt].Name == item.tableName) {
                            $import.fileResponseData[1][cnt].MappedSheet = item.mappedSheetName;
                            $.each(colms, function (colInd, col) {
                                for (var tCnt = 0; tCnt < $import.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                                    var colVal = $import.fileResponseData[1][cnt].ImportColumns[tCnt].Name
                                    if ($import.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId != '' && $import.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId != '00000000-0000-0000-0000-000000000000') {
                                        colVal = $import.fileResponseData[1][cnt].ImportColumns[tCnt].OtherTableUniqueId;
                                    }
                                    if (colVal.toUpperCase() == col.tableColumn.toUpperCase()) {
                                        $import.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = col.mappedSheetColumn;
                                    }
                                }
                            });
                        }
                    }
                    //

                }
            }

        });
        $import.renderModelTree($import.fileResponseData[1]);
        $import.loadColumn();
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
        for (var cnt = 0; cnt < $import.fileResponseData[0].XlSheet.length; cnt++) {
            $import.fileResponseData[0].XlSheet[cnt].isMapped = false;
            for (var tCnt = 0; tCnt < $import.fileResponseData[0].XlSheet[cnt].xlColumns.length; tCnt++) {
                $import.fileResponseData[0].XlSheet[cnt].xlColumns[tCnt].isMapped = false;
            }
        }
        for (var cnt = 0; cnt < $import.fileResponseData[1].length; cnt++) {
            $import.fileResponseData[1][cnt].MappedSheet = null;
            for (var tCnt = 0; tCnt < $import.fileResponseData[1][cnt].ImportColumns.length; tCnt++) {
                $import.fileResponseData[1][cnt].ImportColumns[tCnt].MappedColumnName = null;
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
