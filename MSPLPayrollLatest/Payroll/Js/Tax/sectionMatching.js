$sectionMatching = {
    tableid: 'tblSectionMatching',
    financeYear: $companyCom.getDefaultFinanceYear(),
    sectionId: null,
    formData: document.forms["frmFormula"],
    loadInitial: function () {
        if($sectionMatching.financeYear!=null)
        {
            var gridObject = $sectionMatching.getGridObject();
            $("#dvprojection").hide();
            var tableid = { id: $sectionMatching.tableId };
            var modelContent = $screen.createTable(tableid, gridObject);
            $('#divSectionMatchingDetail').html(modelContent);
            $sectionMatching.loadSection(gridObject, tableid);
        }
        else {
            $app.showAlert("Set default Financial Year", 4);
        }
      
    },
    addInitialize: function () {

        $('#sltProjection').val("No");
        $('#txtFormula').val('');
        $('#txtTxSecName').val('');
        $('#ddTxSection').val($('#sltTaxSection').val());
        $('#txtSectionLimit').val('');
        $('#txtTxSecNumber').val('');
        $('#ddSecExemptionType').val('0');
        //$('#myCheckbox').attr('checked', 'checked');
        $('#chkBillReq').removeAttr('checked');
        $('#chkEligible').removeAttr('checked');
    },
    getGridObject: function () {
        var gridObject;
        gridObject = [
                { tableHeader: "Parent Section", tableValue: "parentSection", cssClass: '' },
                { tableHeader: "Section", tableValue: "name", cssClass: '' },
                { tableHeader: "Formula", tableValue: 'value', cssClass: '' },
                { tableHeader: " Projection", tableValue: 'projection', cssClass: '' },
                { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' },
                { tableHeader: "orderNo", tableValue: 'disorderNo', cssClass: 'nodisp' },
        ];
        return gridObject;
    },
    loadSection: function (context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'actionColumn') {
                columnDef.push(
                        {
                            "aTargets": [cnt],
                            "sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                b.button();
                                b.on('click', function () {
                                    $sectionMatching.sectionId = oData.id;
                                    $sectionMatching.renderSection(oData);
                                    return false;
                                });
                                $(nTd).empty();
                                if (oData.parentId != "00000000-0000-0000-0000-000000000000") {
                                    $(nTd).prepend(b);
                                }
                            }
                        }

                    ); //for action column
            }

            else if (context[cnt].cssClass == 'edit') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="text" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
                        $(nTd).html(b);
                    }
                });
            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaSorting": [[5, "asc"]],
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxSection/GetTaxSection",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ financialyearId: $sectionMatching.financeYear.id, parentId: '00000000-0000-0000-0000-000000000000', type: 'All' }),
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:

                                var out = jsonResult.result;
                                setTimeout(function () {
                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });
                                }, 50);
                                break;
                            case false:
                                $app.showAlert(jsonResult.Message, 4);
                                //alert(jsonResult.Message);
                        }
                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#' + tableId.id + ' tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + tableId.id + ' thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    renderSection: function (data) {
        $("#ddInputType option[value='7']").remove();
        $('#MatchingMaster').modal('toggle');
        $('#sltProjection').val(data.projection);
        $('#txtFormula').val(data.value);
        $($formulaCreation.formData).find('#dvIfElseCondition').html('');
        $($formulaCreation.formData).find('#dvRanges').html('');
        $('#ddInputType').val(data.formulaType);
        $formulaCreation.type = data.formulaType;
        $($sectionMatching.formData).find('#txtFormula').val(data.value);
        $($sectionMatching.formData).find('#txtBaseValue').val(data.formula);
        $($sectionMatching.formData).find('#dvMatchingComponent').addClass('hide');

        if (data.exemptionType != "Yearly") {
            $("#ddInputType option[value='7']").remove();
        }
        else {
            $("#ddInputType").append('<option value="7">Projection</option>');
        }
        
        if ($formulaCreation.type == 4) {
            //  $('#dvFormuladialog').modal('toggle');
            $($sectionMatching.formData).find('#dvConditionalFormula').removeClass('hide');
            $($sectionMatching.formData).find('#dvConditionalFormula').show();
            $($sectionMatching.formData).find('#dvRangeFormula').addClass('hide');
            $($sectionMatching.formData).find('#dvNormalFormula').addClass('hide');
            $formulaCreation.ifElseHiddenFormula = data.formula;
            $formulaCreation.editIfElse();
        } else if ($formulaCreation.type == 5) {
            //  $('#dvFormuladialog').modal('toggle');
            $($sectionMatching.formData).find('#dvRangeFormula').removeClass('hide');
            $($sectionMatching.formData).find('#dvRangeFormula').show();
            $($sectionMatching.formData).find('#dvConditionalFormula').addClass('hide');
            $($sectionMatching.formData).find('#dvNormalFormula').addClass('hide');
            $($sectionMatching.formData).find('#txtBaseValue').val(data.baseValue);
            $formulaCreation.rangeHiddenFormula = data.formula;
            $formulaCreation.editRange();
        }
        else if ($formulaCreation.type == 7) {
            //  $('#dvFormuladialog').modal('toggle');
            $('#ddInputType').val(data.formulaType);
            $($sectionMatching.formData).find('#dvRangeFormula').addClass('hide');
            $($sectionMatching.formData).find('#dvMatchingComponent').removeClass('hide');
            $($sectionMatching.formData).find('#dvMatchingComponent').show();
            $($sectionMatching.formData).find('#dvConditionalFormula').addClass('hide');
            $($sectionMatching.formData).find('#sltMatchingComponent').val(data.MatchingComponent);
           // $("#dvOperator").children().prop('disabled', true);
            $("#txtFormula").prop('disabled', true);
            $(".operator").prop('disabled', true);
            $(".enternumber").prop('disabled', true);
        }
        else {
            $($sectionMatching.formData).find('#dvNormalFormula').removeClass('hide');
            $($sectionMatching.formData).find('#dvNormalFormula').show();
            $($sectionMatching.formData).find('#dvConditionalFormula').addClass('hide');
            $($sectionMatching.formData).find('#dvRangeFormula').addClass('hide');
        }
    },
    save: function () {
        
        var matchingComponentVal = $($txMatchingList.formData).find('#sltMatchingComponent').val();
        if ($($sectionMatching.formData).find('#ddInputType').val() == "0") {
            alert("Plz select the Input type field");
        }
        else if ($($sectionMatching.formData).find('#ddInputType').val() == "7" &&
            (matchingComponentVal == "00000000-0000-0000-0000-000000000000" || ($($txMatchingList.formData).find('#txtFormula').val() == ""))) {
            alert("Plz Fill value and select the Matching component field");
        }
        else {
            $app.showProgressModel();
            var status = false;
            if ($formulaCreation.type == 5) {
                status = validateFormulea('txtBaseValue');
            } else {
                status = validateFormulea('txtFormula');
            }

            if (status) {
                var hiddenFormula = '';
                if ($formulaCreation.type == 1) {
                    hiddenFormula = $sectionMatching.formData.elements["txtFormula"].value;
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
                   // hiddenFormula = $formulaCreation.hidenformula;
                }
                var formula;
                var val;
                var baseValue;
                var baseFormula;
                var matchingComponent;
                if ($formulaCreation.type == 4) {
                    $formulaCreation.buildIfElseFormula();
                    formula = $formulaCreation.ifElseHiddenFormula;
                    val = $formulaCreation.ifElseTextFormula;
                } else if ($formulaCreation.type == 5) {
                    $formulaCreation.buildRange($($sectionMatching.formData).find('#txtBaseValue').val(), $formulaCreation.hiddenFormula);// $($sectionMatching.formData).find('#hdnBaseValue').val());
                    formula = $formulaCreation.rangeHiddenFormula;
                    val = $formulaCreation.rangeTextFormula;
                    baseValue = $($sectionMatching.formData).find('#txtBaseValue').val();
                    validateFormulea('txtBaseValue');
                    baseFormula = $formulaCreation.hidenformula;
                }
                else if ($formulaCreation.type == 7) {
                    matchingComponent = $($txMatchingList.formData).find('#sltMatchingComponent').val();
                    validateFormulea('txtFormula');
                    val = $($sectionMatching.formData).find('#txtFormula').val();
                    formula = $formulaCreation.hidenformula;
                }

                else {
                    validateFormulea('txtFormula');
                    val = $($sectionMatching.formData).find('#txtFormula').val();
                    formula = $formulaCreation.hidenformula;
                }
                
                $.ajax({
                    url: $app.baseUrl + "TaxSection/SaveTaxSectionMatching",
                    data: JSON.stringify({
                        Id: $sectionMatching.sectionId, projection: $($sectionMatching.formData).find('#sltProjection').val(), formula: formula, value: val,
                        formulaType: $($sectionMatching.formData).find('#ddInputType').val(), baseFormula: baseFormula, baseValue: baseValue, matchingComponent: matchingComponent
                    }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {
                        switch (jsonResult.Status) {
                            case true:
                                
                                var p = jsonResult.result;
                                $app.showAlert(jsonResult.Message, 2);
                                $app.hideProgressModel();
                                $sectionMatching.sectionId = p.id;
                                $sectionMatching.loadInitial();
                                $('#MatchingMaster').modal('toggle');
                                break;
                            case false:
                                $app.hideProgressModel();
                                $app.showAlert(jsonResult.Message, 4);
                                break;
                        }
                    },
                    complete: function () {
                    }
                });
            }
        }
    },

};

