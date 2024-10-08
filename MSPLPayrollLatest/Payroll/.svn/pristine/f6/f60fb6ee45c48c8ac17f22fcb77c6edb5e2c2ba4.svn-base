$(function () {
    $('#Jstree1').jstree();
    $test.LoadJsTree();
    $test.loadFancyAttributeModels();
    $test.loadFancyAttributeModels2();



});

var $test = {
    LoadJsTree: function () {
        // $('#Jstree1').jstree("destroy").empty();
        var treeData = [];
        treeData.push({ "id": 1, "parent": "#", "text": 'Test' });
        for (var childcnt = 2; childcnt < 4; childcnt++) {
            treeData.push({ "id": childcnt, "parent": 1, "text": "test" + childcnt });
        }
        $('#Jstree1').jstree({
            'core': {
                'data': treeData
            }
        });
    },
    loadFancyAttributeModels: function () {
        var treeData = [];
        var treeChildData = [];
        for (var childcnt = 0; childcnt < 2; childcnt++) {
            treeChildData.push({ key: childcnt, title: "test0" + childcnt });
        }
        treeData.push({ key: 1, title: "Test0", children: treeChildData });
        fancyTree = jQuery("#fancyTree1").fancytree({
            extensions: ["contextMenu", "dnd"],
            checkbox: false,
            selectMode: 3,
            strings: {
                loading: "Loading..."
            },
            source: treeData,

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
                    if (node.isFolder() === false) {
                        // return false;
                    }

                    return true;
                },
                dragDrop: function (node, data) {
                    if (!data.otherNode) {
                        // It's a non-tree draggable
                        var title = $(data.draggable.element).text() + " (" + (count)++ + ")";
                        node.addNode({ title: title }, data.hitMode);
                        return;
                    }
                    else {
                        var title = $(data.draggable.element).text();
                        if (node.key == "1") {
                            // node.addNode({ title: title }, data.hitMode);
                            alert(node.key);
                            node.addNode({ key: data.otherNode.key, title: data.otherNode.title });
                        }
                    }
                    // data.otherNode.moveTo(node, data.hitMode);
                    //alert("drag drop");
                }
            },
            //checkbox: (("1" === "1") ? true : false),
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
                    //if(node.pare)
                    return {
                        'AddEntity': { 'name': 'Add Table', 'icon': 'folder', 'disabled': ((node.data.FileType === "2") ? true : false) },
                        'Edit': { 'name': 'Edit', 'icon': 'rename' },
                        'Delete': { 'name': 'Delete', 'icon': 'delete' }
                    }
                },
                actions: function (node, action, options) {
                    if (action === "AddEntity") {
                        alert("Add");
                    }
                    if (action === "Edit") {
                        alert("Edit");
                    }
                }
            }
            ,
            select: function (event, data) {

            },
            dblclick: function (event, data) {
            }

        });
    },
    loadFancyAttributeModels2: function () {

        var treeData = [];

        var treeChildData = [];
        for (var childcnt = 0; childcnt < 2; childcnt++) {
            treeChildData.push({ key: childcnt, title: "test" + childcnt });
        }
        treeData.push({ key: 1, title: "Test", children: treeChildData });
        fancyTree = jQuery("#fancyTree2").fancytree({
            extensions: ["contextMenu", "dnd"],
            selectMode: 3,
            strings: {
                loading: "Loading..."
            },
            source: treeData,

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
                    // alert("drag start");
                    return true;
                },
                dragEnter: function (node, data) {
                    //alert("drag Enter");
                    if (node.isFolder() === false) {
                        return false;
                    }

                    return true;
                },
                dragDrop: function (node, data) {
                }
            },
            checkbox: (("1" === "1") ? true : false),
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
                    return {
                        'AddEntity': { 'name': 'Add Table', 'icon': 'folder', 'disabled': ((node.data.FileType === "2") ? true : false) },
                        'Edit': { 'name': 'Edit', 'icon': 'rename' },
                        'Delete': { 'name': 'Delete', 'icon': 'delete' }
                    }
                },
                actions: function (node, action, options) {
                    if (action === "AddEntity") {
                        $app.showAlert(Add, 4);
                    }
                    if (action === "Edit") {
                        $app.showAlert(Edit, 4);
                    }
                }
            }
            ,
            select: function (event, data) {

            },
            dblclick: function (event, data) {
            }

        });
    }
}