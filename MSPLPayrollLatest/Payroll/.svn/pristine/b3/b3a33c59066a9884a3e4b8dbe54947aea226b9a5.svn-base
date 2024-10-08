$("#sltProjection").change(function () {
    
    if ($("#sltProjection").val() == "No") {
        $("#sltTaxDeductionmode").val("ONETIME");
        $('#sltTaxDeductionmode').attr('disabled', false);
        $("#sltMatchingComponent").val("00000000-0000-0000-0000-000000000000");
        $("#sltMatchingComponent").attr('disabled', true);
    }
    else {
        $("#sltTaxDeductionmode").val("Normal");
        $('#sltTaxDeductionmode').attr('disabled', true);
        $("#sltMatchingComponent").attr('disabled', false);
    }
});
$("#sltProjection").blur(function () {
    
    if ($("#sltProjection").val() == "No") {
        $("#sltTaxDeductionmode").val("ONETIME");
        $('#sltTaxDeductionmode').attr('disabled', false);
        $("#sltMatchingComponent").val("00000000-0000-0000-0000-000000000000");
    }
    else {
        $("#sltTaxDeductionmode").val("Normal");
        $('#sltTaxDeductionmode').attr('disabled', true);
    }
});
var $txMatchingList = {
    tableid: 'tblIncomeMatching',
    financeYear: $companyCom.getDefaultFinanceYear(),
    attributeId: null,
    formData: document.forms["frmFormula"],
    popupstate: null,
    loadInitial: function () {
        
        var gridObject = $txMatchingList.getGridObject();
        var tableid = { id: $txMatchingList.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvIncomeMatching').html(modelContent);
        $txMatchingList.loadSection(gridObject, tableid);
        $companyCom.loadEarningDeduction({ id: 'sltMatchingComponent' }, 'Earning',false);
        
        $txMatchingList.loadSections({ id: 'sltExamptionComponent' }, '', 'SubSection');
        $companyCom.loadOtherExamption({ id: 'sltGrossSection' }, "GROSSSECTION");
        $('#inputTypelabel').hide();
        $('#ddInputType').hide();
        $('#ddInputType').val('3');
        $("#sltProjection").val("Yes");
        $("#sltTaxDeductionmode").val("Normal");
        $('#sltTaxDeductionmode').attr('disabled', true);        
    },
    loadSections: function (dropControl, parentId, type) {
        var FinancialYrId =$companyCom.getDefaultFinanceYear();
        if (FinancialYrId != null) {
            $.ajax({
                type: 'POST',
                async: false,
                url: $app.baseUrl + "TaxSection/GetTaxSection",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ financialyearId: $companyCom.getDefaultFinanceYear().id, parentId: parentId, type: type }),
                dataType: "json",
                success: function (result) {

                    var out = result;
                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                    $.each(result.result, function (index, value) {

                        if (value.formulaType == 6 || value.formulaType == 2) {
                            $('#' + dropControl.id).append($("<option></option>").val(value.id).html(value.name));
                        }
                    });
                },
                error: function (msg) {
                }
            });
        }
        else {
            $app.showAlert("Set default Financial Year", 4);
        }
    },
    getGridObject: function () {

        var gridObject;

        gridObject = [
                { tableHeader: "id", tableValue: "attributeId", cssClass: 'nodisp' },
                { tableHeader: "Earnings", tableValue: "name", cssClass: 'Name' },
                { tableHeader: "Mactching Component", tableValue: 'matchingCompName', cssClass: '' },
                { tableHeader: "Exemption Component", tableValue: 'examptionCompName', cssClass: '' },
                { tableHeader: "Operator", tableValue: 'operators', cssClass: 'operators' },
                { tableHeader: "Other Component", tableValue: 'otherComponent', cssClass: '' },
                { tableHeader: "Gross Section", tableValue: 'grossSection', cssClass: '' },
                { tableHeader: "Order No", tableValue: 'orderno', cssClass: '' },
                { tableHeader: "Projection", tableValue: 'projection', cssClass: '' },
                { tableHeader: "Tax Deduction", tableValue: 'taxDeductionmode', cssClass: '' },
                { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }

        ];
        return gridObject;

    },
    loadSection: function (context, tableId) {
        
        if ($txMatchingList.financeYear != null) {
            var columnsValue = [];
            var columnDef = [];
            for (var cnt = 0; cnt < context.length; cnt++) {
                columnsValue.push({ "data": context[cnt].tableValue });
                if (context[cnt].cssClass == 'nodisp') {
                    columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
                }
                else if (context[cnt].tableValue == "grossSection") {
                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "actionColumn",
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {


                            if (oData.grossSection == 1) {
                                $(nTd).html('GrossIncomn17(1)');
                            }
                            if (oData.grossSection == 2) {
                                $(nTd).html('GrossIncomn17(2)');
                            }
                            if (oData.grossSection == 3) {
                                $(nTd).html('GrossIncomn17(3)');
                            }
                            if (oData.displayAs == $txMatchingList.popupstate) {
                                $(nTd).css('background-color', '#96e0d0');
                            }
                        },
                    });

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
                                        debugger;
                                        $txMatchingList.popupstate = oData.displayAs;
                                        $txMatchingList.attributeId = oData.Id;
                                        $txMatchingList.rendermatch(oData);
                                        return false;
                                    });

                                    $(nTd).empty();
                                    $(nTd).prepend(b);
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
                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "word-wrap",
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.displayAs == $txMatchingList.popupstate) {
                                $(nTd).css('background-color', '#96e0d0');
                            }
                        },
                        "bSearchable": true
                    });
                }
            }

            var dtClientList = $('#' + tableId.id).DataTable({

                'iDisplayLength': 100,
                'bPaginate': true,
                'sPaginationType': 'full',
                'sDom': '<"top">rt<"bottom"ip><"clear">',
                columns: columnsValue,
                "aoColumnDefs": columnDef,
                //"aaData": data,
                ajax: function (data, callback, settings) {

                    $.ajax({
                        type: 'POST',
                        url: $app.baseUrl + "FinanceYear/GetTaxable",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ financeYearid: $txMatchingList.financeYear.id }),
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
        }
        else {
            $app.showAlert("Set default Financial Year", 4);
        }
        },
    rendermatch: function (data) {
        
        $('#AddIncomMatching').modal('toggle');
        $txMatchingList.attributeId = data.attributeId;
        $($txMatchingList.formData).find('#txtFieldName').val(data.name);
        $($txMatchingList.formData).find('#txtDescription').val(data.displayAs);
        $($txMatchingList.formData).find('#sltProjection').val(data.projection);
        $('#txtFormula').val(data.otherComponent);
        $($txMatchingList.formData).find('#sltExamptionComponent').val(data.examptionComponent);
        $($txMatchingList.formData).find('#sltMatchingComponent').val(data.matchingComponent);       
        data.operators==""? $($txMatchingList.formData).find('#sltOperator').val('00000000-0000-0000-0000-000000000000'):$($txMatchingList.formData).find('#sltOperator').val(data.operators);
        $($txMatchingList.formData).find('#sltTaxDeductionmode').val(data.taxDeductionmode);
        data.grossSection==0? $($txMatchingList.formData).find('#sltGrossSection').val('00000000-0000-0000-0000-000000000000'): $($txMatchingList.formData).find('#sltGrossSection').val(data.grossSection);
        $($txMatchingList.formData).find('#txtOrderNo').val(data.orderno);
        if ($($txMatchingList.formData).find('#sltProjection').val() == "No") {        
            $("#sltMatchingComponent").attr('disabled', true);
            $("#sltTaxDeductionmode").attr('disabled', false);
            
        }
        else {      
            $("#sltMatchingComponent").attr('disabled', false);
        }
    },
    addInitialize: function () {


        $($txMatchingList.formData).find('#sltProjection').val();
        $('#txtFormula').val('');
        $($txMatchingList.formData).find('#sltExamptionComponent').val('00000000-0000-0000-0000-000000000000');
        $($txMatchingList.formData).find('#sltMatchingComponent').val('00000000-0000-0000-0000-000000000000');
        $($txMatchingList.formData).find('#ddSecExemptionType').val('00000000-0000-0000-0000-000000000000');
        $($txMatchingList.formData).find('#sltOperator').val('00000000-0000-0000-0000-000000000000');
        $($txMatchingList.formData).find('#sltTaxDeductionmode').val('');
        $($txMatchingList.formData).find('#sltGrossSection').val('00000000-0000-0000-0000-000000000000');
        $($txMatchingList.formData).find('#txtOrderNo').val('');




    },
    saveMatching: function () {
        
        if ($($txMatchingList.formData).find('#sltProjection').val() == "Yes" && $($txMatchingList.formData).find('#sltMatchingComponent').val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert("MatchingComponent is required", 4);
            return false;
        }
        var data = $txMatchingList.BuildSubSectionObject();
   
        $.ajax({
            url: $app.baseUrl + "TaxSection/SaveIncomeMatching",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                
                switch (jsonResult.Status) {
                    case true:
                        
                        var p = jsonResult.result;
                        $txMatchingList.selectedSectionId = p.id;
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $txMatchingList.addInitialize();
                        $txMatchingList.loadInitial();
                        $('#AddIncomMatching').modal('toggle');

                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);

                        break;
                }
            },
            complete: function ()
            {
                $app.hideProgressModel();
            }
        });
    },
    BuildSubSectionObject: function () {
        validateFormulea('txtFormula');
        var retObject = {
            attributeId: $txMatchingList.attributeId,
            financeYearid: $txMatchingList.financeYear.id,
            projection: $($txMatchingList.formData).find('#sltProjection').val(),
            formula:  $formulaCreation.hidenformula,
            examptionComponent: $($txMatchingList.formData).find('#sltExamptionComponent').val(),
            matchingComponent: $($txMatchingList.formData).find('#sltMatchingComponent').val(),
            otherComponent: $($txMatchingList.formData).find('#txtFormula').val(),
            operators: $($txMatchingList.formData).find('#sltOperator').val(),
            taxDeductionmode: $($txMatchingList.formData).find('#sltTaxDeductionmode').val(),
            grossSection: $($txMatchingList.formData).find('#sltGrossSection').val(),
            orderno: $($txMatchingList.formData).find('#txtOrderNo').val(),
        };
        return retObject;
    }
};
















