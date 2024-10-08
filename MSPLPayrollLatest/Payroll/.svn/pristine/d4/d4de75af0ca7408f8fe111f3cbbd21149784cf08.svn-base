$("#ddInputType").change(function () {

    $formulaCreation.type = $("#ddInputType").val();
    $("#txtFormula").val(''); $("#txtBaseValue").val('');
    $("#txtFormula").prop('disabled', false);
    // $("#dvOperator").children().prop('disabled', false);
    $(".operator").prop('disabled', false);
    $(".enternumber").prop('disabled', false);
    if ($formulaCreation.type == 4) {
        //  $('#dvFormuladialog').modal('toggle');
        $formulaCreation.totlalIfCont = 0
        //  $formulaCreation.ifElseHiddenFormula = ''; $formulaCreation.hidenformula = '';      
        $($taxBehavior.formData).find('#dvConditionalFormula').removeClass('hide');
        $($taxBehavior.formData).find('#dvConditionalFormula').show();
        $($taxBehavior.formData).find('#dvRangeFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvNormalFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvMatchingComponent').addClass('hide');

    } else if ($formulaCreation.type == 5) {
        //  $('#dvFormuladialog').modal('toggle');
        $('#txtBaseValue').focus();
        $($taxBehavior.formData).find('#dvRangeFormula').removeClass('hide');
        $($taxBehavior.formData).find('#dvRangeFormula').show();
        $($taxBehavior.formData).find('#dvConditionalFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvNormalFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvMatchingComponent').addClass('hide');
    }
    else if ($formulaCreation.type == 7) {
        //  $('#dvFormuladialog').modal('toggle');
        $('#txtBaseValue').focus();
        $($taxBehavior.formData).find('#dvMatchingComponent').removeClass('hide');
        $($taxBehavior.formData).find('#dvMatchingComponent').show();
        $($taxBehavior.formData).find('#dvRangeFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvConditionalFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvNormalFormula').removeClass('hide');
        //  $("#dvOperator").children().prop('disabled', true);// Projection allow only one component so disable the operators
        $("#txtFormula").prop('disabled', true);
        $(".operator").prop('disabled', true);
        $(".enternumber").prop('disabled', true);
        //  $taxBehavior.RemoveOneTimeComponent($formulaCreation.attributeModelList, "Projection");
    }
    else {
        $($taxBehavior.formData).find('#dvNormalFormula').removeClass('hide');
        $($taxBehavior.formData).find('#dvNormalFormula').show();
        $($taxBehavior.formData).find('#dvConditionalFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvRangeFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvMatchingComponent').addClass('hide');
    }
    //  $taxBehavior.RemoveOneTimeComponent($formulaCreation.attributeModelList, "");
});

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
$taxBehavior = {
    selectedAttributeId: '00000000-0000-0000-0000-000000000000',
    attributeModelList: null,
    selectedAttributeModeltypeId: null,

    formData: document.forms["frmFormula"],
    financeYear: $companyCom.getDefaultFinanceYear(),
    loadInitial: function () {
        $companyCom.loadEarningDeduction({ id: 'sltMatchingComponent' }, 'Earning', true)
    },
    loadAttributeModelList: function () {
        $.ajax({
            url: $app.baseUrl + "Company/GetAttributeModelList",
            data: JSON.stringify({ type: "TaxField" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $taxBehavior.attributeModelList = p;
                        $taxBehavior.renderAttributeModelTree(p);
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
    renderAttributeModelTree: function (data) {
        debugger;
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
        $("#attributeModeltreeTax").fancytree();
        //before load clean the tree nodes
        $("#attributeModeltreeTax").fancytree("destroy");
        fancyTree = jQuery("#attributeModeltreeTax").fancytree({
            extensions: ["contextMenu", "dnd", "glyph", "wide", "filter"],//"childcounter",
            selectMode: 3,
            checkbox: true,
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
                $taxBehavior.selectedAttributeId = data.node.key;
                $formulaCreation.type = $("#sltType").val();
                $taxBehavior.loadTaxBehavior();

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
                        'EditAttributeModel': { 'name': 'Edit', 'icon': 'edit' },

                    }
                },
                actions: function (node, action, options) {
                    if (action === "EditAttributeModel") {
                        $taxBehavior.selectedAttributeModeltypeId = node.parent.key;
                        $taxBehavior.selectedAttributeId = node.key;
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

                        //    $filedCreation.edit({ Id: $taxBehavior.selectedAttributeId, AttributeModelTypeId: $taxBehavior.selectedAttributeModeltypeId, behaviorType: type });
                        $taxBehavior.loadTaxBehavior();
                    }

                }
            }

        });

        $("#attributeModeltreeTax").fancytree("getRootNode").visit(function (node) {
            node.toggleExpanded();
        });
    },
    loadTaxBehavior: function () {



        $.ajax({
            url: $app.baseUrl + "TaxEntity/GetTaxBehavior",
            data: JSON.stringify({ financeYearId: $taxBehavior.financeYear.id, attributeid: $taxBehavior.selectedAttributeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;

                        $taxBehavior.edit(p);
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
    save: function () {
        debugger;
        var status = false;

        if ($formulaCreation.type == 5) {
            status = validateFormulea('txtBaseValue');
        } else {
            status = validateFormulea('txtFormula');
        }
        if (status) {


            var hiddenFormula = '';
            if ($formulaCreation.type == 1) {
                hiddenFormula = $taxBehavior.formData.elements["txtFormula"].value;
            }
            if ($formulaCreation.type == 2) {
                hiddenFormula = '';
            }
            else if ($formulaCreation.type == 3) {
                //first character
                var chkoperater = $formulaCreation.hidenformula.charAt(0);
                if (chkoperater == "+" && chkoperater == "-" && chkoperater == "*" && chkoperater == "/") {
                    $app.showAlert('Invalid syntax at begining of the formula', 4);
                    return;
                }
                //last character
                chkoperater = $formulaCreation.hidenformula.charAt($formulaCreation.hidenformula.length - 1);
                if (chkoperater == "+" && chkoperater == "-" && chkoperater == "*" && chkoperater == "/") {
                    $app.showAlert('Invalid syntax at end of the formula', 4);
                    alert("");
                    return;
                }

                if ($formulaCreation.hidenformula.split('(').length != $formulaCreation.hidenformula.split(')').length) {
                    $app.showAlert('Invalid parenthesis used on formula', 4);
                    return;
                }

                hiddenFormula = $formulaCreation.hidenformula;
            }
            else if ($("#sltEntityType").val() == 4) {
                hiddenFormula = $formulaCreation.ifElseHiddenFormula;
                //hiddenFormula = $formulaCreation.hidenformula == "" ? $formulaCreation.ifElseHiddenFormula : $formulaCreation.hidenformula;
            }

            $app.showProgressModel();
            debugger
            var data = $taxBehavior.BuildObject();
            $("#txtFormula").val(data.value);

            $.ajax({
                url: $app.baseUrl + "TaxEntity/SaveTaxBehavior",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                async: false,
                success: function (jsonResult) {

                    switch (jsonResult.Status) {
                        case true:
                            var p = jsonResult.result;
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 2);
                            return false;
                            break;
                        case false:
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 4);
                            return false;
                            break;
                    }
                    return false;
                },
                complete: function () {
                    $app.hideProgressModel();
                    return false;
                }
            });

        }
    },
    BuildObject: function () {
        debugger;
        var formula;
        var val;
        var baseValue;
        var baseFormula;
        if ($formulaCreation.type == 4) {
            formula = $formulaCreation.hidenformula;
            $formulaCreation.buildIfElseFormula();
            //  formula = $formulaCreation.hidenformula == "" ? $formulaCreation.ifElseHiddenFormula : $formulaCreation.hidenformula;
            formula = formula == "" ? $formulaCreation.ifElseHiddenFormula : formula;
            val = $formulaCreation.ifElseTextFormula;
        } else if ($formulaCreation.type == 5) {
            $formulaCreation.buildRange($($taxBehavior.formData).find('#txtBaseValue').val(), $formulaCreation.hiddenFormula);// $($taxBehavior.formData).find('#hdnBaseValue').val());
            formula = $formulaCreation.rangeHiddenFormula;
            val = $formulaCreation.rangeTextFormula;
            baseValue = $($taxBehavior.formData).find('#txtBaseValue').val();
            validateFormulea('txtBaseValue');
            baseFormula = $formulaCreation.hidenformula;
        }
            //else if ($formulaCreation.type == 7) {
            //    val = $($taxBehavior.formData).find('#sltMatchingComponent').val();
            //}
        else {
            validateFormulea('txtFormula');
            val = $($taxBehavior.formData).find('#txtFormula').val();
            formula = $formulaCreation.hidenformula;
        }
        var retObject = {
            id: $taxBehavior.id,
            financeyearId: $taxBehavior.financeYear.id,
            value: val,
            formula: formula,
            inputtype: $($taxBehavior.formData).find('#ddInputType').val(),
            fieldtype: $($taxBehavior.formData).find('#ddType').val(),
            category: $($taxBehavior.formData).find('#ddSlabCategory').val(),
            fieldfor: $($taxBehavior.formData).find('#ddFieldFor').val(),
            attributeId: $taxBehavior.selectedAttributeId,
            baseValue: baseValue,
            baseFormula: baseFormula
        };
        return retObject;

    },
    edit: function (data) {

        $taxBehavior.clear();

        if (data.length > 0) {
            $formulaCreation.attributeCode = data[0].attributeId;
            $formulaCreation.hiddenEligibilityFormula = '';
            $formulaCreation.formulaFocusedElemt = '';
            $formulaCreation.ifFocusedElement = '';
            $($formulaCreation.formData).find('#dvIfElseCondition').html('');
            $($formulaCreation.formData).find('#dvRanges').html('');
            $($taxBehavior.formData).find('#ddSlabCategory').val(data[0].category);
            $($taxBehavior.formData).find('#ddFieldFor').val(data[0].fieldfor);
            $($taxBehavior.formData).find('#ddType').val(data[0].fieldtype);
            $($taxBehavior.formData).find('#ddInputType').val(data[0].inputtype);
            $formulaCreation.type = data[0].inputtype;

            $($taxBehavior.formData).find('#txtFormula').val(data[0].value);
            $($taxBehavior.formData).find('#txtBaseValue').val(data[0].baseValue);
            if ($formulaCreation.type == 4) {
                //  $('#dvFormuladialog').modal('toggle');
                $($taxBehavior.formData).find('#dvConditionalFormula').removeClass('hide');
                $($taxBehavior.formData).find('#dvConditionalFormula').show();
                $($taxBehavior.formData).find('#dvRangeFormula').addClass('hide');
                $($taxBehavior.formData).find('#dvNormalFormula').addClass('hide');
                $formulaCreation.ifElseHiddenFormula = data[0].formula;
                $formulaCreation.editIfElse();
            } else if ($formulaCreation.type == 5) {
                //  $('#dvFormuladialog').modal('toggle');
                $($taxBehavior.formData).find('#dvRangeFormula').removeClass('hide');
                $($taxBehavior.formData).find('#dvRangeFormula').show();
                $($taxBehavior.formData).find('#dvConditionalFormula').addClass('hide');
                $($taxBehavior.formData).find('#dvNormalFormula').addClass('hide');
                $($taxBehavior.formData).find('#txtBaseValue').val(data[0].baseValue);
                $formulaCreation.rangeHiddenFormula = data[0].formula;
                $formulaCreation.editRange();
            }
                //else if ($formulaCreation.type == 7) {
                //    $($taxBehavior.formData).find('#dvConditionalFormula').addClass('hide');
                //    $($taxBehavior.formData).find('#dvRangeFormula').addClass('hide');
                //    $($taxBehavior.formData).find('#dvNormalFormula').addClass('hide');
                //    $($taxBehavior.formData).find('#sltMatchingComponent').val(data[0].baseValue);
                //}
            else {
                $($taxBehavior.formData).find('#dvNormalFormula').removeClass('hide');
                $($taxBehavior.formData).find('#dvNormalFormula').show();
                $($taxBehavior.formData).find('#dvConditionalFormula').addClass('hide');
                $($taxBehavior.formData).find('#dvRangeFormula').addClass('hide');
            }
        } else {
            $formulaCreation.attributeCode = $taxBehavior.selectedAttributeId;
        }
    },
    clear: function () {
        $($taxBehavior.formData).find('#ddSlabCategory').val(0);
        $($taxBehavior.formData).find('#txtBaseValue').val('');
        $($taxBehavior.formData).find('#txtFormula').val('');
        $($taxBehavior.formData).find('#dvNormalFormula').removeClass('hide');
        $($taxBehavior.formData).find('#dvNormalFormula').show();
        $($taxBehavior.formData).find('#dvConditionalFormula').addClass('hide');
        $($taxBehavior.formData).find('#dvRangeFormula').addClass('hide');
        $($taxBehavior.formData).find('#ddType').val(1);
        $($taxBehavior.formData).find('#ddFieldFor').val("ITAX");
        //  $formulaCreation.ifElseHiddenFormula = '';
        // $formulaCreation.ifElseTextFormula = '';
        // $formulaCreation.hiddenEligibilityFormula = '';
        // $formulaCreation.formulaFocusedElemt = '';
        // $formulaCreation.ifFocusedElement = '';
        //  $($formulaCreation.formData).find('#dvIfElseCondition').html('');
        // $($formulaCreation.formData).find('#dvRanges').html('');
    },
    RemoveOneTimeComponent: function (data, type) {

        var temp = data;
        if (Type = "Projection") {
            var treeData = [];
            for (var cnt = 0; cnt < temp.length; cnt++) {
                if (temp[cnt].AttributeModelList.length <= 0) {
                    treeData.push(temp[cnt]);
                }
                else {

                    for (var childcnt = 0; childcnt < temp[cnt].AttributeModelList.length; childcnt++) {
                        if (temp[cnt].AttributeModelList[childcnt].TaxDeductionMode != "ONETIME") {
                            treeData.push(temp[cnt].AttributeModelList[childcnt]);
                        }
                    }
                }
                $formulaCreation.renderAttributeModelTree(treeData);
            }
        }
        else {
            $formulaCreation.renderAttributeModelTree(data);
        }
    },
}