$slaryView = {
    LoadSalaryList: function () {
        $.ajax({
            url: $app.baseUrl + "Employee/GetSalaryList",
            data: "",
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);

                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;

                        $slaryView.renderSalary(p,"salary");
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

    renderSalary: function (data,type) {
        var treeData = [];
        for (var cnt = 0; cnt < data.length; cnt++) {
            
            if (data[cnt].length <= 0) {
                treeData.push({ key: data[cnt].year, title: data[cnt].Year });
            }
            else {
                var treeChildData = [];
                for (var childcnt = 0; childcnt < data[cnt].filelist.length; childcnt++) {

                    treeChildData.push({ key: data[cnt].filelist[childcnt].filepath, title: data[cnt].filelist[childcnt].month });

                }
                treeData.push({ key: data[cnt].year, title: data[cnt].year, children: treeChildData });
            }
        }

        if (data.length === 0) {
            treeData.push({ key: 0, title: "No Data Available" });
        }


        //initialize the tree
        $("#payslipTree").fancytree();
        //before load clean the tree nodes
        $("#payslipTree").fancytree("destroy");
        fancyTree = jQuery("#payslipTree").fancytree({
            extensions: ["contextMenu", "dnd", "glyph", "wide", "filter"],//"childcounter",
            selectMode: 3,
            checkbox: false,
            strings: {
                loading: "Loading..."
            },
            source: treeData,
            filter: {
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },
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
                debugger;
                if (!data.node.hasChildren()) {
                    $slaryView.downloadPayslip(data.node.key,type)
                }

            },
            init: function (event, ctx) {
                ctx.tree.debug("init");
                ctx.tree.rootNode.fixSelection3FromEndNodes();
            },
            loadchildren: function (event, ctx) {
                ctx.tree.debug("loadchildren");
                ctx.node.fixSelection3FromEndNodes();
            }
            
           

        });

        $("#payslipTree").fancytree("getRootNode").visit(function (node) {
              node.toggleExpanded();
        });
    }

    ,
    downloadPayslip: function (fileid,type) {

        var url = "";
        url = fileid;
        var ajaxUrl = "";
        if (type=="salary") {
            ajaxUrl = $app.baseUrl + "Employee/LoadPayslip";
        }
        else if (type == "form16")
        {
            ajaxUrl = $app.baseUrl + "Employee/LoadForm16";
        }
        $.ajax({
            type: 'POST',
            url: ajaxUrl,//$app.baseUrl + "Employee/LoadPayslip",
            contentType: "application/json",
            data: JSON.stringify({ Payslipfile: url }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                debugger;
                if (jsonResult.Status) {
                    var oData = new Object();
                    oData.filePath = jsonResult.result.filePath;
                    $app.downloadSync('Download/DownLevOpenTemp', oData);
                    return false;
                }
                else
                {
                    
                    if (jsonResult.result.filePath!=null) {
                        var oData = new Object();
                        oData.filePath = jsonResult.result.filePath;
                        $app.downloadSync('Download/DownLevOpenTemp', oData);
                        return false;
                    }
                    else {
                        $app.showAlert(jsonResult.Message, 4);
                    }
                   
                }
            }
        });
    },
    downloadWorkSheet: function (fileid) {

        var url = "";
        url = fileid;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/downloadWorkSheet",
            contentType: "application/json",
            data: "",
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        var oData = new Object();

                        oData.filePath = jsonResult.result.filePath;
                        $app.downloadSync('Download/DownLevOpenTemp', oData);
                        return false;
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);

                        break;
                }
               
            }
        });
    },

    downloadForm16: function () {      
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/downloadForm16",
            contentType: "application/json",
            data: "",
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        
                        var oData = new Object();
                        oData.filePath = jsonResult.result.filePath;
                        $app.downloadSync('Download/DownLevOpenTemp', oData);
                        return false;
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);

                        break;
                }
               
            }
        });
    },
    //File name based 
    LoadForm16List: function (formName) {
        $.ajax({
            url: $app.baseUrl + "Employee/GetTDSFormList",
            data: JSON.stringify({formName: formName }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);

                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;

                        $slaryView.renderSalary(p,"form16");
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

}