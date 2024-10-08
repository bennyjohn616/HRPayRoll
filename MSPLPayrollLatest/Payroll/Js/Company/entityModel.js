

$("#frmEntity").on('submit', function (event) {
    if ($app.requiredValidate('frmEntity', event)) {
        $entitymodel.saveEntityModel('frmEntity');
        return false;
    }
});
$(document).ready(function () {
    loadComponentData()
});

function loadComponentData() {

}


$("#frmField").on('submit', function (event) {
    debugger
    if ($app.requiredValidate('frmField', event)) {
        $filedCreation.save('save');
        return false;
    }
});
$("#btnSaveClose").on('click', function (event) {
    if ($app.requiredValidate('frmField', event)) {
        $filedCreation.save('saveclose');
        return false;
    }
});

$("#frmEntityAttribute").on('submit', function (event) {
    if ($app.requiredValidate('frmEntityAttribute', event)) {
        $entitymodel.saveEntityAttribute();
        return false;
    }
});
///Modified By:Sharmila
$("input[name=search]").keyup(function (e) {

    var tree = $("#attributeModeltree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("button#btnResetSearch").click();
        return;
    }
    var n = tree.applyFilter(match);
    $("button#btnResetSearch").attr("disabled", false);
}).focus();

$("button#btnResetSearch").click(function (e) {

    var tree = $("#attributeModeltree").fancytree("getTree");
    $("input[name=search]").val("");
    tree.clearFilter();
}).attr("disabled", true);

$("input#hideMode").change(function (e) {

    var tree = $("#attributeModeltree").fancytree("getTree");
    tree.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    tree.clearFilter();
    $("input[name=search]").keyup();
    tree.render();
});
$("input[name=searchTables]").keyup(function (e) {

    var treeTable = $("#entityModeltree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("button#btnResetSearchTables").click();
        return;
    }
    var n = treeTable.applyFilter(match);
    $("button#btnResetSearchTables").attr("disabled", false);
}).focus();

$("button#btnResetSearchTables").click(function (e) {

    var treeTable = $("#entityModeltree").fancytree("getTree");
    $("input[name=searchTables]").val("");
    treeTable.clearFilter();
}).attr("disabled", true);

$("input#hideModeTables").change(function (e) {

    var treeTable = $("#entityModeltree").fancytree("getTree");
    treeTable.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    treeTable.clearFilter();
    $("input[name=searchTables]").keyup();
    // treeTable.render();
});

$("input[name=search1]").keyup(function (e) {

    var saltree = $("#entityAttributeModeltree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("button#btnResetSearch1").click();
        return;
    }
    var x = saltree.applyFilter(match);
    $("button#btnResetSearch1").attr("disabled", false);
}).focus();

$("button#btnResetSearch1").click(function (e) {

    var saltree = $("#entityAttributeModeltree").fancytree("getTree");
    $("input[name=search1]").val("");
    saltree.clearFilter();
}).attr("disabled", true);

$("input#hideMode1").change(function (e) {

    var saltree = $("#entityAttributeModeltree").fancytree("getTree");
    saltree.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    saltree.clearFilter();
    $("input[name=search1]").keyup();
    // saltree.render();
});
var $entitymodel = {
    canSave: false,
    lastEntityAttributeModelId: '',
    selectedType: 'All',
    selectedTableCategoryId: '',
    selectedEntityModelId: '',
    selectedEntityAttributeModelId: '',
    selectedAttributemodelId: '',
    selectedAttributeModeltypeId: '',
    entityModeltargetForm: 'frmEntity',
    entityAttributetargetForm: 'frmEntityAttribute',
    attributeModelList: null,
    entityModelIdForEntityAttribute: null,
    entityModelDatas: [],

    saveEntityModel: function (target) {

        if (!$entitymodel.canSave) {
            return false;
        }
        $entitymodel.canSave = false;
        $app.showProgressModel();
        var formdata = {
            Id: $entitymodel.selectedEntityModelId,
            Name: $("#" + target + " #txtName").val(),
            DisplayAs: $("#" + target + " #txtDisplayAs").val(),
            IsPhysicalTable: false,
            TableCategoryId: $entitymodel.selectedTableCategoryId
        };
        var entiyMaping = {
            RefEntityModelName: $("#" + target + " #sltEntityRelation").val(),
            EntityTableName: $entitymodel.selectedEntityModelId
        };
        $.ajax({

            url: $app.baseUrl + "Entity/SaveEntityModel",
            data: JSON.stringify({ dataValue: formdata, entitymap: entiyMaping }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $('#AddEntity').modal('toggle');
                        var target = $entitymodel.entityAttributetargetForm;
                        $app.clearControlValues(target);
                        $entitymodel.loadTableCategory();
                        $app.hideProgressModel();
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
    clearEntityModel: function () {

        $entitymodel.selectedEntityModelId = null;
        $app.clearControlValues($entitymodel.entityModeltargetForm);
        if ($entitymodel.selectedTableCategoryId == '' || $entitymodel.selectedTableCategoryId == null) {
            $app.showAlert('Please select the category to add entity', 4);
            return false;
        }
        else {
            $entitymodel.canSave = true;
            $('#AddEntity').modal('toggle');
        }
    },
    renderEntityModel: function (data) {

        if ($entitymodel.selectedEntityModelId == null || $entitymodel.selectedEntityModelId == '') {
            alert('Please select the entity to edit');
            return false;
        }
        var target = $entitymodel.entityModeltargetForm;
        $app.clearControlValues(target);
        $('#AddEntity').modal('toggle');
        $entitymodel.canSave = true;
        $entitymodel.selectedEntityModelId = data.entityModel.Id;
        $("#" + target + " #txtName").val(data.entityModel.Name);
        $("#" + target + " #txtDisplayAs").val(data.entityModel.DisplayAs);
        $("#" + target + " #sltEntityRelation").val(data.entityMap.RefEntityModelName);
        var Defaultlist = ["Salary", "AdditionalInfo"];
        for (var cnt = 0; cnt < Defaultlist.length; cnt++) {
            if (Defaultlist[cnt] == data.entityModel.Name) {
                document.getElementById("txtName").readOnly = true;
                return true
            }
            else {
                document.getElementById("txtName").readOnly = false;
            }
        }
    },
    renderAttributeModelTree: function (data) {
        debugger
        var treeData = [];
        for (var cnt = 0; cnt < data.length; cnt++) {
            if (data[cnt].AttributeModelList.length <= 0) {
                treeData.push({ key: data[cnt].Id, title: data[cnt].DisplayAs });
            }
            else {
                var treeChildData = [];
                for (var childcnt = 0; childcnt < data[cnt].AttributeModelList.length; childcnt++) {
                    treeChildData.push({ key: data[cnt].AttributeModelList[childcnt].Id, title: data[cnt].AttributeModelList[childcnt].Name + "[" + data[cnt].AttributeModelList[childcnt].DisplayAs + "]" });
                }
                treeData.push({ key: data[cnt].Id, title: data[cnt].DisplayAs, children: treeChildData });
            }
        }
        //initialize the tree
        $("#attributeModeltree").fancytree();
        //before load clean the tree nodes
        $("#attributeModeltree").fancytree("destroy");
        fancyTree = jQuery("#attributeModeltree").fancytree({
            extensions: ["contextMenu", "dnd", "glyph", "wide", "filter"],//"childcounter",
            quicksearch: true,
            selectMode: 3,
            checkbox: true,
            autoScroll: true,
            strings: {
                loading: "Loading..."
            },
            filter: {
                //autoApply: true,   // Re-apply last filter if lazy data is loaded
                //autoExpand: false, // Expand all branches that contain matches while filtered
                //counter: true,     // Show a badge with number of matching child nodes near parent icons
                //fuzzy: false,      // Match single characters in order, e.g. 'fb' will match 'FooBar'
                //hideExpandedCounter: true,  // Hide counter badge if parent is expanded
                //hideExpanders: false,       // Hide expanders if all child nodes are hidden by filter
                //highlight: true,   // Highlight matches by wrapping inside <mark> tags
                //leavesOnly: false, // Match end nodes only
                //nodata: true,      // Display a 'no data' status node if result is empty
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
            source: treeData,
            //childcounter: {
            //    deep: true,
            //    hideZeros: true,
            //    hideExpanded: true
            //},
            minExpandLevel: 1,
            glyph: $payroll.getTreeglyphOpts(),
            wide: {
                iconWidth: "1em",     // Adjust this if @fancy-icon-width != "16px"
                iconSpacing: "0.5em", // Adjust this if @fancy-icon-spacing != "3px"
                levelOfs: "1.5em"     // Adjust this if ul padding != "16px"
            },
            icon: function (event, data) {
                if (data.node.isFolder()) {
                    return "glyphicon glyphicon-book";
                }
                else { return ""; }
            }, initHelper: function (node, data) {
                // Helper was just created: modify markup
                var helper = data.ui.helper,
                    sourceNodes = data.tree.getSelectedNodes();

                // Store a list of active + all selected nodes
                if (!node.isSelected()) {
                    sourceNodes.unshift(node);
                }
                helper.data("sourceNodes", sourceNodes);
                // Mark selected nodes also as drag source (active node is already)
                $(".fancytree-active,.fancytree-selected", tree.$container)
                    .addClass("fancytree-drag-source");
                // Add a counter badge to helper if dragging more than one node
                if (sourceNodes.length > 1) {
                    helper.append($("<span class='fancytree-childcounter'/>")
                        .text("+" + (sourceNodes.length - 1)));
                }
                // Prepare an indicator for copy-mode
                helper.prepend($("<span class='fancytree-dnd-modifier'/>")
                    .text("+").hide());
            },
            updateHelper: function (node, data) {
                // Mouse was moved or key pressed: update helper according to modifiers

                // NOTE: pressing modifier keys stops dragging in jQueryUI 1.11
                // http://bugs.jqueryui.com/ticket/14461
                var event = data.originalEvent,
                    tree = node.tree,
                    copyMode = event.ctrlKey || event.altKey;

                // Adjust the drop marker icon
                //          data.dropMarker.toggleClass("fancytree-drop-copy", copyMode);

                // Show/hide the helper's copy indicator (+)
                data.ui.helper.find(".fancytree-dnd-modifier").toggle(copyMode);
                // tree.debug("1", $(".fancytree-active,.fancytree-selected", tree.$container).length)
                // tree.debug("2", $(".fancytree-active,.fancytree-selected").length)
                // Dim the source node(s) in move-mode
                $(".fancytree-drag-source", tree.$container)
                    .toggleClass("fancytree-drag-remove", !copyMode);
                // data.dropMarker.toggleClass("fancytree-drop-move", !copyMode);
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
                    if (node.parent.title == 'root') {
                        return false;
                    }
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
                ///testing///

                var activeNodes = $("#attributeModeltree").fancytree('getTree').getSelectedNodes();
                if (data.node.selected == true && activeNodes.length > 0) {
                    $('#btnAddMapping').removeAttr('disabled');
                }
                else if (data.node.selected == false && activeNodes.length == 0) {
                    $('#btnAddMapping').attr('disabled', 'disabled');
                }

                var s = data.tree.getSelectedNodes();

            },
            expand: function (node, data) {

                data.node.getLastChild().scrollIntoView(true, { topNode: data.node })
            },

            contextMenu: {
                menu: function (node) {
                    if (node.parent.title == 'root') {
                        return {
                            'AddAttributeModel': { 'name': 'Add', 'icon': 'new', 'disabled': ((node.data.FileType === "2") ? true : false) }
                        }
                    }
                    else {
                        return {
                            'EditAttributeModel': { 'name': 'Edit', 'icon': 'edit' },
                            'DeleteAttributeModel': { 'name': 'Delete', 'icon': 'delete' }
                        }
                    }
                },
                actions: function (node, action, options) {

                    if (action === "AddAttributeModel") {
                        $entitymodel.selectedAttributeModeltypeId = node.key;
                        $entitymodel.selectedAttributemodelId = null;
                        var type = node.title;
                        if (type.indexOf('Earning') >= 0) {
                            type = 'Earning';
                        }
                        else if (type.indexOf('Deduction') >= 0) {
                            type = 'Deduction';
                        }
                        else if (type.indexOf('Master') >= 0) {
                            type = 'Master';
                        }
                        else if (type.indexOf('Tax') >= 0) {
                            type = 'Tax';
                        }
                        else {
                            return false;
                        }
                        $filedCreation.addNew({ Id: $entitymodel.selectedAttributeModeltypeId, behaviorType: type });
                    }
                    else if (action === "EditAttributeModel") {
                        $entitymodel.selectedAttributeModeltypeId = node.parent.key;
                        $entitymodel.selectedAttributemodelId = node.key;
                        var type = node.parent.title;
                        if (type.indexOf('Earning') >= 0) {
                            type = 'Earning';
                        }
                        else if (type.indexOf('Deduction') >= 0) {
                            type = 'Deduction';
                        }
                        else if (type.indexOf('Master') >= 0) {
                            type = 'Master';
                        } else if (type.indexOf('Tax') >= 0) {
                            type = 'Tax';
                        }
                        else {
                            return false;
                        }

                        $filedCreation.edit({ Id: $entitymodel.selectedAttributemodelId, AttributeModelTypeId: $entitymodel.selectedAttributeModeltypeId, behaviorType: type });
                    }
                    else if (action === "DeleteAttributeModel") {
                        if (confirm('Are you sure,do you want to delete')) {
                            $entitymodel.selectedAttributeModeltypeId = node.parent.key;
                            var type = node.parent.title;
                            $entitymodel.selectedAttributemodelId = node.key;
                            $entitymodel.deleteAttributeModel({ 'AttributeModelId': node.key });
                        }
                    }
                }

            }


        });


        $("#attributeModeltree").fancytree("getRootNode").visit(function (node) {

            node.toggleExpanded();
        });
    },
    AddFields: function () {
        debugger;
        var activeNodes = $("#attributeModeltree").fancytree('getTree').getSelectedNodes();
        $.each(activeNodes, function (index, node) {
            var formdata = {
                Id: $entitymodel.selectedEntityAttributeModelId,
                EntityModelId: $entitymodel.entityModelIdForEntityAttribute,
                AttributeModelId: node.key,
                DisplayOrder: 0,
                IsHidden: false,
                DefaultValue: null,
                IsActive: true
            };
            $.ajax({
                url: $app.baseUrl + "Entity/SaveEntityAttributeModel",
                data: JSON.stringify({ dataValue: formdata }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",

                success: function (jsonResult) {

                    switch (jsonResult.Status) {
                        case true:

                            var p = jsonResult.result;
                            var rootNode = $("#entityAttributeModeltree").fancytree("getRootNode").children[0];
                            var childNode = rootNode.addChildren({
                                title: node.title,
                                tooltip: node.title,
                                folder: false,
                                key: p.Id,
                            });
                            // node.addNode({ key: p.Id, title: node.title });
                            $app.showAlert(jsonResult.Message, 2);
                            var target = $entitymodel.entityAttributetargetForm;
                            $app.clearControlValues(target);
                            $app.showAlert('Please update dynamic group after component creations ', 1);
                            break;
                        case false:
                            $app.showAlert(jsonResult.Message, 4);
                            break;
                    }
                },
                complete: function () {

                }
            });
        });

    },
    renderEntityModelTree: function (data) {
        debugger
        var treeData = [];
        for (var cnt = 0; cnt < data.length; cnt++) {
            if (data[cnt].EntityModelList.length <= 0) {
                treeData.push({ key: data[cnt].Id, title: data[cnt].Description });
            }
            else {
                var treeChildData = [];
                $entitymodel.entityModelDatas = [];
                for (var childcnt = 0; childcnt < data[cnt].EntityModelList.length; childcnt++) {
                    treeChildData.push({ key: data[cnt].EntityModelList[childcnt].Id, title: data[cnt].EntityModelList[childcnt].Name });
                    $entitymodel.entityModelDatas.push({ Id: data[cnt].EntityModelList[childcnt].Id, Name: data[cnt].EntityModelList[childcnt].Name });
                }
                treeData.push({ key: data[cnt].Id, title: data[cnt].Description, children: treeChildData });
            }
        }
        //initialize the tree
        $("#entityModeltree").fancytree();
        //before load clean the tree nodes
        $("#entityModeltree").fancytree("destroy");
        fancyTree = jQuery("#entityModeltree").fancytree({
            extensions: ["contextMenu", "glyph", "wide", "filter"],//"childcounter",
            selectMode: 3,
            strings: {
                loading: "Loading..."
            },
            source: treeData,
            //childcounter: {
            //    deep: true,
            //    hideZeros: true,
            //    hideExpanded: true
            //},
            filter: {
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
            minExpandLevel: 1,
            glyph: $payroll.getTreeglyphOpts(),
            wide: {
                iconWidth: "1em",     // Adjust this if @fancy-icon-width != "16px"
                iconSpacing: "0.5em", // Adjust this if @fancy-icon-spacing != "3px"
                levelOfs: "1.5em"     // Adjust this if ul padding != "16px"
            },
            icon: function (event, data) {
                if (data.node.isFolder()) {
                    return "glyphicon glyphicon-book";
                }
                else { return ""; }
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

                    return false; //true;
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
                    if (node.parent.title == 'root') {
                        return {
                            'AddEntityModel': { 'name': 'Add Table', 'icon': 'folder', 'disabled': ((node.data.FileType === "2") ? true : false) }
                        }
                    }
                    else {
                        return {
                            'ViewFields': { 'name': 'View Fields', 'icon': 'view' },
                            'EditEntityModel': { 'name': 'Edit', 'icon': 'edit' },
                            'DeleteEntityModel': { 'name': 'Delete', 'icon': 'delete' }
                        }
                    }
                },
                actions: function (node, action, options) {

                    if (action === "AddEntityModel") {
                        $entitymodel.selectedTableCategoryId = node.key;
                        $entitymodel.selectedEntityModelId = null;
                        $entitymodel.clearEntityModel();
                    }
                    else if (action === "EditEntityModel") {
                        $entitymodel.selectedTableCategoryId = node.parent.key;
                        $entitymodel.selectedEntityModelId = node.key;
                        $entitymodel.loadEntityModel({ TableCategoryId: $entitymodel.selectedTableCategoryId, Id: $entitymodel.selectedEntityModelId });
                    }
                    else if (action === "DeleteEntityModel") {
                        if (confirm('Are you sure,do you want to delete')) {
                            $entitymodel.selectedEntityModelId = node.key;
                            $entitymodel.deleteEntityModel({ EntityModelId: node.key });
                        }
                    }
                    else if (action === "ViewFields") {
                        if (node.parent.title == 'root') {
                            $entitymodel.selectedEntityModelId = null;
                            $entitymodel.selectedTableCategoryId = node.Key;

                        }
                        else {
                            $entitymodel.selectedTableCategoryId = node.parent.key;
                            $entitymodel.selectedEntityModelId = node.key;
                            $entitymodel.entityModelIdForEntityAttribute = node.key;
                            $entitymodel.loadEntityAttributeModel({ id: $entitymodel.entityModelIdForEntityAttribute });

                        }
                    }
                }
            }

        });
        $("#entityModeltree").fancytree("getRootNode").visit(function (node) {

            node.toggleExpanded();
        });
    },
    renderEntityAttributeModelTree: function (data) {
        debugger
        var treeDefaultChildData = []
        treeDefaultChildData.push({ key: "DefaultId", title: "Id" });
        treeDefaultChildData.push({ key: "DefaultName", title: "Name" });
        var treeData = [];
        if (data.EntityAttributeModelList.length <= 0) {
            treeData.push({ key: data.Id, title: data.DisplayAs, children: treeDefaultChildData });
        }
        else {
            var treeChildData = [];
            treeChildData = treeDefaultChildData;
            for (var childcnt = 0; childcnt < data.EntityAttributeModelList.length; childcnt++) {
                treeChildData.push({ key: data.EntityAttributeModelList[childcnt].Id, title: data.EntityAttributeModelList[childcnt].AttributeModel.DisplayAs });
                $entitymodel.lastEntityAttributeModelId = data.EntityAttributeModelList[childcnt].Id;
            }
            treeData.push({ key: data.Id, title: data.DisplayAs, children: treeChildData });
        }
        //initialize the tree
        $("#entityAttributeModeltree").fancytree();
        //before load clean the tree nodes
        $("#entityAttributeModeltree").fancytree("destroy");
        fancyTree = jQuery("#entityAttributeModeltree").fancytree({
            extensions: ["contextMenu", "dnd", "glyph", "wide", "filter"],//"childcounter",
            selectMode: 3,
            checkbox: false,
            render: true,
            strings: {
                loading: "Loading..."
            },
            source: treeData,
            //childcounter: {
            //    deep: true,
            //    hideZeros: true,
            //    hideExpanded: true
            //},
            filter: {
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
            minExpandLevel: 1,
            glyph: $payroll.getTreeglyphOpts(),
            wide: {
                iconWidth: "1em",     // Adjust this if @fancy-icon-width != "16px"
                iconSpacing: "0.5em", // Adjust this if @fancy-icon-spacing != "3px"
                levelOfs: "1.5em"     // Adjust this if ul padding != "16px"
            },
            icon: function (event, data) {
                if (data.node.isFolder()) {
                    return "glyphicon glyphicon-book";
                }
                else { return ""; }
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
                    //                    if (node.isFolder() === false) {
                    //                        return false;
                    //                    }

                    return true;
                },
                dragDrop: function (node, data) {
                    debugger;
                    if (!data.otherNode) {
                        // It's a non-tree draggable
                        var title = $(data.draggable.element).text() + " (" + (count)++ + ")";
                        node.addNode({ title: title }, data.hitMode);
                        return;
                    }
                    else {
                        if ((data.draggable.element[0]).id == "attributeModeltree") {
                            var title = $(data.draggable.element).text();
                            // if (node.key == "1") {
                            // node.addNode({ title: title }, data.hitMode);
                            $entitymodel.selectedEntityAttributeModelId = null;
                            var formdata = {
                                Id: $entitymodel.selectedEntityAttributeModelId,
                                EntityModelId: $entitymodel.entityModelIdForEntityAttribute,
                                AttributeModelId: data.otherNode.key,
                                DisplayOrder: 0,
                                IsHidden: false,
                                DefaultValue: null,
                                IsActive: true
                            };
                            $.ajax({
                                url: $app.baseUrl + "Entity/SaveEntityAttributeModel",
                                data: JSON.stringify({ dataValue: formdata }),
                                dataType: "json",
                                contentType: "application/json",
                                type: "POST",
                                success: function (jsonResult) {

                                    switch (jsonResult.Status) {
                                        case true:
                                            var p = jsonResult.result;
                                            //node.addNode({ key: data.otherNode.key, title: data.otherNode.title });
                                            node.addNode({ key: p.Id, title: data.otherNode.title });
                                            var target = $entitymodel.entityAttributetargetForm;
                                            $app.clearControlValues(target);
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
                        debugger;
                        data.otherNode.moveTo(node, data.hitMode);
                        $entitymodel.updown({ EntityAttributeModelId: node.key }, data);
                    }

                }
            },
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
                    if (node.parent.title == 'root' || node.key == "DefaultId" || node.key == "DefaultName") {
                        return;
                    }
                    else if ($entitymodel.lastEntityAttributeModelId == node.key) {
                        return {
                            'EditEntityAttribute': { 'name': 'Edit', 'icon': 'edit' },
                            'DeleteEntityAttribute': { 'name': 'Delete', 'icon': 'delete' }
                        }
                    }
                    else {
                        return {
                            'DownAttribute': { 'name': 'Down', 'icon': 'down' },
                            'EditEntityAttribute': { 'name': 'Edit', 'icon': 'edit' },
                            'DeleteEntityAttribute': { 'name': 'Delete', 'icon': 'delete' }
                        }

                    }
                },
                actions: function (node, action, options) {
                    $entitymodel.selectedEntityAttributeModelId = node.key;
                    if (action === "EditEntityAttribute") {
                        $entitymodel.renderEntityAttribute(node.data, node.title);
                    }
                    else if (action == "DownAttribute") {
                        $entitymodel.updown({ EntityAttributeModelId: node.key }, data);
                    }

                    else if (action === "DeleteEntityAttribute") {
                        if (confirm('Are you sure,do you want to delete')) {
                            $entitymodel.deleteEntityAttribute({ EntityAttributeModelId: node.key });
                        }

                    }

                }
            }

        });
        $("#entityAttributeModeltree").fancytree("getRootNode").visit(function (node) {

            node.toggleExpanded();
        });
    },
    loadRelationtable: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetEntityRelationtable",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (msg) {
                var out = msg.result;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                $.each(out, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Name).html(blood.DisplayAs));
                });
            },
            error: function (msg) {
            }
        });
    },
    loadFlexiPay: function () {
        $.ajax({
            url: $app.baseUrl + "Company/GetAttributeModelList",
            data: JSON.stringify({ type: $entitymodel.selectedType }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
                        $entitymodel.attributeModelList = p;
                        $entitymodel.renderAttributeModelTree(p);
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
    loadTableCategory: function () {
        debugger
        $entitymodel.loadRelationtable({ id: 'sltEntityRelation' });
        $entitymodel.loadAttributeModelList();
        $entitymodel.loadFlexiModelList();
        //$entitymodel.loadFlexiModelList()
        $.ajax({
            url: $app.baseUrl + "Company/GetTableCategory",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $entitymodel.renderEntityModelTree(p);
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
    loadEntityModel: function (data) {

        $.ajax({
            url: $app.baseUrl + "Entity/GetEntityModel",
            data: JSON.stringify({ tablecategoryId: data.TableCategoryId, id: data.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                debugger
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $entitymodel.renderEntityModel(p);
                        $entitymodel.loadFlexiModelList();
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
    loadEntityAttributeModel: function (data) {

        $.ajax({
            url: $app.baseUrl + "Company/GetEntityAttributeModel",
            data: JSON.stringify({ tablecategoryId: $entitymodel.selectedTableCategoryId, enityModelId: data.id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
                        $entitymodel.renderEntityAttributeModelTree(p);
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
    loadAttributeModelList: function () {

        $.ajax({
            url: $app.baseUrl + "Company/GetAttributeModelList",
            data: JSON.stringify({ type: $entitymodel.selectedType }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
                        $entitymodel.attributeModelList = p;
                        $entitymodel.renderAttributeModelTree(p);
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
    renderFlexiFields: function (data) {
        debugger;
        var modal = $('<div>', {
            id: 'ShowFlexiPay',
            'data-toggle': 'validator',
            class: 'modal'
        });

        var modalDialog = $('<div>', { class: 'modal-dialog' });
        var modalContent = $('<div>', { class: 'modal-content' });

        var modalHeader = $('<div>', { class: 'modal-header' });
        modalHeader.append($('<button>', {
            type: 'button',
            class: 'close',
            'data-dismiss': 'modal',
            text: '×'
        }));
        modalHeader.append($('<h4>', {
            class: 'modal-title',
            id: 'H4',
            text: 'Add/Edit Field'
        }));

        var modalBody = $('<div>', { class: 'modal-body' });
        var formHorizontal = $('<div>', { class: 'form-horizontal' });
        var formGroup = $('<div>', { class: 'form-group col-md-10' });

        formGroup.append($('<label>', {
            id: 'fieldName', // Corrected the id attribute
            text: 'ComponentName' // Use "text" to set label text
        }));


        formGroup.append($('<input>', {
            id: 'FlexiComponent',
            class: 'form-control',

        }));
        formGroup.append($('<input>', {
            id: 'hideId',
            class: 'nodisp',

        }));
        var radioContainerDiv = $('<div>', { class: 'form-group' });
        radioContainerDiv.append($('<lable>', {
            text: "BasicPay",
            class: "control-label col-md-3"
        }))
        // Create the "Yes" radio button and label
        var radioYesLabel = $('<label>', { class: 'radio-inline' });
        var radioYes = $('<input>', {
            type: 'radio',
            id: 'BasicPayYes',
            name: 'BasicPay',
            value: 'True'
        }).appendTo(radioYesLabel);
        radioYesLabel.append('Yes');

        // Create the "No" radio button and label
        var radioNoLabel = $('<label>', { class: 'radio-inline' });
        var radioNo = $('<input>', {
            type: 'radio',
            id: 'BasicPayNo',
            name: 'BasicPay',
            checked: 'checked',
            value: 'False'
        }).appendTo(radioNoLabel);
        radioNoLabel.append('No');

        // Append the labels and radio buttons to the container div
        radioContainerDiv.append(radioYesLabel, radioNoLabel);

        // Append the container div to the form group
        formGroup.append(radioContainerDiv);
        var radioContainerDiv2 = $('<div>', { class: 'form-group' });
        radioContainerDiv2.append($('<lable>', {
            text: "FlexiPay",
            class: "control-label col-md-3"
        }))
        // Create the "Yes" radio button and label
        var radioYesLabel2 = $('<label>', { class: 'radio-inline' });
        var radioYes2 = $('<input>', {
            type: 'radio',
            id: 'FlexiPayYes',
            name: 'FlexiPay',
            value: 'True'
        }).appendTo(radioYesLabel2);
        radioYesLabel2.append('Yes');

        // Create the "No" radio button and label
        var radioNoLabel2 = $('<label>', { class: 'radio-inline' });
        var radioNo2 = $('<input>', {
            type: 'radio',
            id: 'FlexiPayNo',
            name: 'FlexiPay',
            checked: 'checked',
            value: 'False'
        }).appendTo(radioNoLabel2);
        radioNoLabel2.append('No');

        // Append the labels and radio buttons to the container div
        radioContainerDiv2.append(radioYesLabel2, radioNoLabel2);

        // Append the container div to the form group
        formGroup.append(radioContainerDiv2);

        var modalFooter = $('<div>', { class: 'modal-footer' });
        modalFooter.append($('<button>', {
            type: 'button',
            class: 'btn btn-secondary',
            text: 'Cancel',
            'data-dismiss': 'modal',
            click: function () {
                modal.modal('hide');
                $("#showFlexiComponent").val("");
            }
        }))
        modalFooter.append($('<button>', {
            type: 'button',
            class: 'btn btn-primary',
            id: 'btnSave', // Add an ID to the button for easier selection
            text: 'Save',
            click: function () {
                debugger;
                var formdata = {
                    Name: $("#FlexiComponent").val(),
                    Basicpay: $('#BasicPayYes').prop("checked") ? true : false,
                    FlxiPay: $('#FlexiPayYes').prop("checked") ? true : false ,
                }
                if (result !== null && result !== undefined && result !== "") {
                    $.ajax({
                        url: $app.baseUrl + "Company/SaveFlexiPayComponent",
                        data: JSON.stringify({ Component: formdata }),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json", // Corrected content type
                        success: function (jsonResult) {
                            if (jsonResult.Status) {
                                debugger;
                                $app.showAlert(jsonResult.Message, 1);
                                $("#hideId").val("");
                               
                                $("#ShowFlexiPay").remove();
                                $entitymodel.loadFlexiModelList();
                            } else {
                                $app.showAlert(jsonResult.Message, 4);
                            }
                        }
                    });
                }
            }
        }));

        // Construct the modal structure
        formHorizontal.append(formGroup);
        modalBody.append(formHorizontal);
        modalContent.append(modalHeader, modalBody, modalFooter);
        modalDialog.append(modalContent);
        modal.append(modalDialog);

        // Add the modal to the DOM
        $('body').append(modal);

        // Activate the modal
        modal.modal();
    },

    loadFlexiModelList: function () {
        debugger
        $.ajax({
            url: $app.baseUrl + "Company/GetFlexiPayComponentSelect",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger
                        var p = jsonResult.result;

                        $entitymodel.renderFlexiAttributeModelTree(p);
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
    renderFlexiAttributeModelTree: function (data) {
        debugger
        var treeDefaultChildData = [];


        var treeData = [];
        var treeChildData = [];
        treeChildData = treeDefaultChildData;

        $.each(data, function (index, item) {
            debugger;
            treeChildData.push({ key: item.Id, title: item.Name });
        });

        treeData.push({ key: "", title: "FlexiPay", children: treeChildData });

        //if (data.EntityAttributeModelList.length <= 0) {
        //    treeData.push({ key: data.Id, title: data.DisplayAs, children: treeDefaultChildData });
        //}
        //else {
        //}
        //initialize the tree
        $("#Flexipaycom").fancytree();
        //before load clean the tree nodes
        $("#Flexipaycom").fancytree("destroy");
        fancyTree = jQuery("#Flexipaycom").fancytree({
            extensions: ["contextMenu", "dnd", "glyph", "wide", "filter"],//"childcounter",
            selectMode: 3,
            checkbox: false,
            render: true,
            strings: {
                loading: "Loading..."
            },
            source: treeData,
            //childcounter: {
            //    deep: true,
            //    hideZeros: true,
            //    hideExpanded: true
            //},
            filter: {
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
            minExpandLevel: 1,
            glyph: $payroll.getTreeglyphOpts(),
            wide: {
                iconWidth: "1em",     // Adjust this if @fancy-icon-width != "16px"
                iconSpacing: "0.5em", // Adjust this if @fancy-icon-spacing != "3px"
                levelOfs: "1.5em"     // Adjust this if ul padding != "16px"
            },
            icon: function (event, data) {
                if (data.node.isFolder()) {
                    return "glyphicon glyphicon-book";
                }
                else { return ""; }
            },
            // ... other settings ...
            contextMenu: {
                menu: {
                    edit: { name: "Edit", icon: "edit" },
                    delete: { name: "Delete", icon: "delete" },
                },
                actions: function (node, action, options) {
                    if (action === "edit") {
                        // Handle edit action
                        alert("Edit clicked for node with key: " + node.key);
                    } else if (action === "delete") {
                        // Handle delete action
                        alert("Delete clicked for node with key: " + node.key);
                    }
                },
            },
        });
    },
    renderEntityAttribute: function (data, attributename) {

        if ($entitymodel.selectedEntityAttributeModelId == null || $entitymodel.selectedEntityAttributeModelId == '') {
            alert('Please select the field to edit');
            return false;
        }
        $.ajax({
            url: $app.baseUrl + "Entity/GetEntityAttributeModel",
            data: JSON.stringify({ id: $entitymodel.selectedEntityAttributeModelId, entityModelId: $entitymodel.selectedEntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $entitymodel.canSave = true;
                        data = jsonResult.result;
                        var target = $entitymodel.entityAttributetargetForm;
                        $app.clearControlValues(target);
                        $('#AddEntityAttribute').modal('toggle');
                        $("#" + target + " #txtName").val(attributename);
                        $("#" + target + " #txtDefaultvalue").val(data.DefaultValue);
                        $("#" + target + " #hdnDisplayOrder").val(data.DisplayOrder);
                        data.IsHidden == true ? $("#" + target + " #rdHiddenYes").prop('checked', true) : $("#" + target + " #rdHiddenNo").prop('checked', true);
                        data.IsActive == true ? $("#" + target + " #rdActiveYes").prop('checked', true) : $("#" + target + " #rdActiveNo").prop('checked', true);
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
    saveEntityAttribute: function () {

        if (!$entitymodel.canSave) {
            return false;
        }
        $entitymodel.canSave = false;
        $app.showProgressModel();
        var target = $entitymodel.entityAttributetargetForm;
        var formdata = {
            Id: $entitymodel.selectedEntityAttributeModelId,
            EntityModelId: $entitymodel.entityModelIdForEntityAttribute,
            AttributeModelId: null,
            DisplayOrder: $("#" + target + " #hdnDisplayOrder").val(),
            IsHidden: $("#" + target + " #rdHiddenYes").prop('checked') ? true : false,
            DefaultValue: $("#" + target + " #txtDefaultvalue").val(),
            IsActive: $("#" + target + " #rdActiveYes").prop('checked') ? true : false
        };
        $.ajax({
            url: $app.baseUrl + "Entity/SaveEntityAttributeModel",
            data: JSON.stringify({ dataValue: formdata }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $('#AddEntityAttribute').modal('toggle');
                        var target = $entitymodel.entityAttributetargetForm;
                        $app.clearControlValues(target);
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
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
    },
    updown: function (context, treedata) {

        $.ajax({
            url: $app.baseUrl + "Entity/InterChangeOrder",
            data: JSON.stringify({ entityModelId: $entitymodel.selectedEntityModelId, entityAttributeModelId: context.EntityAttributeModelId, actionVal: "Down" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        treedata = jsonResult.result;
                        $entitymodel.renderEntityAttributeModelTree(treedata);
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
    deleteEntityAttribute: function (context) {
        $.ajax({
            url: $app.baseUrl + "Entity/DeleteEntityAttributeModel",
            data: JSON.stringify({ entityModelId: $entitymodel.selectedEntityModelId, entityAttributeModelId: context.EntityAttributeModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var treedata = jsonResult.result;
                        $entitymodel.renderEntityAttributeModelTree(treedata);
                        $app.showAlert(jsonResult.Message, 2);
                        $entitymodel.selectedEntityAttributeModelId = '';
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
    deleteAttributeModel: function (context) {
        $.ajax({
            url: $app.baseUrl + "Entity/DeleteAttributeModel",
            data: JSON.stringify({ attributeModelId: context.AttributeModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                //alert(jsonResult.Status);
                switch (jsonResult.Status) {
                    case true:
                        var treedata = jsonResult.result;
                        $entitymodel.attributeModelList = treedata;
                        $entitymodel.renderAttributeModelTree(treedata);
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
    deleteEntityModel: function (context) {
        $.ajax({
            url: $app.baseUrl + "Entity/DeleteEntityModel",
            data: JSON.stringify({ entityModelId: context.EntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $entitymodel.renderEntityModelTree(p);
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
    //radio yes click evet
    showFlexiPay: function () {
        debugger;
        // Create the modal element
        $entitymodel.createModel();
        $entitymodel.loadComponentData();
    },

    createModel: function () {
        // Check if the modal already exists
        debugger;
        var modal = $('#ShowFlexiPayComponent');

        if (modal.length === 0) {
            // If the modal doesn't exist, create it
            modal = $('<div>', {
                id: 'ShowFlexiPayComponent',
                'data-toggle': 'validator',
                class: 'modal'
            });

            var modalDialog = $('<div>', { class: 'modal-dialog' });
            var modalContent = $('<div>', { class: 'modal-content' });

            var modalHeader = $('<div>', { class: 'modal-header' });
            modalHeader.append($('<button>', {
                type: 'button',
                class: 'close',
                'data-dismiss': 'modal',
                text: '×'
            }));
            modalHeader.append($('<h4>', {
                class: 'modal-title',
                id: 'H4',
                text: 'Add/Edit Field'
            }));

            var modalBody = $('<div>', { class: 'modal-body' });
            var formHorizontal = $('<div>', { class: 'form-horizontal' });
            var formGroup = $('<div>', { class: 'form-group' });

            formGroup.append($('<label>', {
                text: 'FlexiPay Item',
                class: "control-label"
            }));
            formGroup.append($('<select>', {
                id: 'showFlexiComponent',
                class: 'form-control',
            }));

            formGroup.append($('<label>', {
                text: 'Display Order',
                class: "control-label"
            }));
            formGroup.append($('<input>', {
                id: 'displayOreder',
                type: 'text',
                class: 'form-control',
            }));

            formGroup.append($('<label>', {
                text: "Fixed Amount",
                class: "control-label"
            }));
            formGroup.append($('<input>', {
                type: "text",
                id: "txtFixedAm",
                class: "form-control"
            }));

            

            // Create a container div for radio buttons
            var radioContainerDiv = $('<div>', { class: 'form-group' });
            radioContainerDiv.append($('<lable>', {
                text: "ReadOnly",
                class: "control-label col-md-2"
            }))
            // Create the "Yes" radio button and label
            var radioYesLabel = $('<label>', { class: 'radio-inline' });
            var radioYes = $('<input>', {
                type: 'radio',
                id: 'ReadOnlyYes',
                name: 'ReadOnly',
                value: 'True'
            }).appendTo(radioYesLabel);
            radioYesLabel.append('Yes');

            // Create the "No" radio button and label
            var radioNoLabel = $('<label>', { class: 'radio-inline' });
            var radioNo = $('<input>', {
                type: 'radio',
                id: 'ReadOnlyNo',
                name: 'ReadOnly',
                checked: 'checked',
                value: 'False'
            }).appendTo(radioNoLabel);
            radioNoLabel.append('No');

            // Append the labels and radio buttons to the container div
            radioContainerDiv.append(radioYesLabel, radioNoLabel);

            // Append the container div to the form group
            formGroup.append(radioContainerDiv);

            // Construct the modal structure
            formHorizontal.append(formGroup);
            modalBody.append(formHorizontal);

            // Modal footer with Save and Cancel buttons
            var modalFooter = $('<div>', { class: 'modal-footer' });
            modalFooter.append($('<button>', {
                type: 'button',
                class: 'btn btn-primary',
                text: 'Save',
                click: function () {
                    // Add your save logic here
                    debugger
                    $entitymodel.saveFlexiPayMap();
                    modal.modal('hide');
                }
            }));
            modalFooter.append($('<button>', {
                type: 'button',
                class: 'btn btn-secondary',
                text: 'Cancel',
                'data-dismiss': 'modal',
                click: function () {
                    modal.modal('hide');
                    $("#showFlexiComponent").val("");
                }
            }));

            modalContent.append(modalHeader, modalBody, modalFooter);
            modalDialog.append(modalContent);
            modal.append(modalDialog);

            // Add the modal to the DOM
            $('body').append(modal);
        }

        // Reset the select value to '--Select--' when reopening the modal
        $("#showFlexiComponent").val('00000000-0000-0000-0000-000000000000');

        // Show the modal
        modal.modal();
    }
,

    loadComponentData: function () {
        $.ajax({
            url: $app.baseUrl + "Company/GetFlexiPayComponentSelect",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (msg) {
                debugger
                var out = msg.result;
                var selector = $('#showFlexiComponent');
                // Clear existing options
                selector.empty();
                selector.append($("<option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                var selectedItem = out.find(function (item) {
                    return item.MasterCompentId === $filedCreation.selectedId;
                });

                $.each(out, function (index, item) {
                    selector.append($("<option>").val(item.Id).html(item.Name));
                });
                debugger
                if (selectedItem) {
                    selector.val(selectedItem.Id);
                    $("#displayOreder").val(selectedItem.DisplayOrder);
                    $("#txtFixedAm").val(selectedItem.FixedAmount);
                    (selectedItem.IsReadOnly == true) ? $("#ReadOnlyYes").prop('checked', true) : $("#ReadOnlyNo").prop("checked", true);

                }
                else {
                    selector.val('00000000-0000-0000-0000-000000000000')
                    $("#displayOreder").val("");
                    $("#txtFixedAm").val("");
                    $("#ReadOnlyNo").prop("checked", true);
                }

            },
            complete: function () {

            }
        });
    },
    saveFlexiPayMap: function () {
        debugger
        var formdata = {
            Id: $filedCreation.selectedId,
            SelectorId: $("#showFlexiComponent").val(),
            DisplayOrder: $("#displayOreder").val(),
            FixedAmount: $("#txtFixedAm").val(),
            Name: $("#showFlexiComponent option:selected").text(),
            IsReadOnly: $("#ReadOnlyYes").prop('checked') ? true : false,
        }

        $.ajax({
            url: $app.baseUrl + "Company/SaveFlexiPayOrder",
            data: JSON.stringify({ dataValue: formdata }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger
                var status = jsonResult.Status;
                if (status) {
                    $app.showAlert(jsonResult.Message, 2)
                }
                else {
                    $app.showAlert(jsonResult.Message, 4)
                }
            },
            complete: function () {

            }

        })
    }


};
