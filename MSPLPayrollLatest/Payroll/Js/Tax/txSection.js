$('#sltTaxSection').on('change', function () {
    $txSection.selectedSectionparentId = $('#sltTaxSection').val();
    $('#ddTxSection').val($('#sltTaxSection').val());
    var gridObject = $txSection.getGridObject();
    $txSection.loadSection(gridObject, { id: $txSection.tableId });
});
$("#txtTxSecNumber").change(function () {
    
    $txSection.checkOrderNo();
});
$("#txtTxSecNumber").blur(function () {
    
    $txSection.checkOrderNo();
});

$txSection = {
    canSave: true,
    formData: document.forms["frmTxSection"],
    tableId: 'tblTxSection',
    selectedSectionId: '00000000-0000-0000-0000-000000000000',
    selectedSectionparentId: '',
    taxSection:'',
    financeYear: $companyCom.getDefaultFinanceYear(),
    checkOrderNo:function(){
        $($txSection.taxSection).each(function (index) {
            
            if (parseInt($txSection.taxSection[index].orderNo) == parseInt($("#txtTxSecNumber").val().trim().toLowerCase())) {
                $app.showAlert("Already Exist " + $("#txtTxSecNumber").val(), 4);
                $("#txtTxSecNumber").val('');
                return false;
            }
        });
    },
    bindEvent: function () {
        $('#btnAddSection').on("click", function (e) {
            $txSection.addInitialize();
        });
        $('#frmTxSection').on("submit", function (e) {
            $txSection.save();
            return false
        });
    },
    loadInitial: function () {
        
        var gridObject = $txSection.getGridObject();
        var tableid = { id: $txSection.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvSection').html(modelContent);
        $txSection.loadSection(gridObject, tableid);
    },

    getGridObject: function () {
        
        var gridObject;
        if ($txSection.selectedSectionparentId == '') {
            gridObject = [
                    { tableHeader: "id", tableValue: "id", cssClass: 'nodisp' },
                    { tableHeader: "financialyearId", tableValue: "financialYearId", cssClass: 'nodisp' },
                    { tableHeader: "Name", tableValue: "name", cssClass: '' },
                    { tableHeader: "Limit", tableValue: "limit", cssClass: '' },
                    { tableHeader: "orderNo", tableValue: "orderNo", cssClass: 'nodisp' },
                    { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }];
        } else {
            
            gridObject = [
                        { tableHeader: "id", tableValue: "id", cssClass: 'nodisp' },
                    { tableHeader: "parentid", tableValue: "parentId", cssClass: 'nodisp' },
                    { tableHeader: "financialyearId", tableValue: "financialYearId", cssClass: 'nodisp' },
                    { tableHeader: "Name", tableValue: "name", cssClass: '' },
                      { tableHeader: "Limit", tableValue: "limit", cssClass: '' },
                 //   { tableHeader: "Display As", tableValue: "displayAs", cssClass: '' },
                 //   { tableHeader: "Order No", tableValue: "orderNo", cssClass: '' },
                     { tableHeader: "Bill Req", tableValue: "documentReq", cssClass: '' },
                     { tableHeader: "Exemption Type", tableValue: "exemptionType", cssClass: '' },
                       { tableHeader: "Eligible Under New Tax Scheme", tableValue: "Eligible", cssClass: '' },
                //     { tableHeader: "Gross Deductable", tableValue: "grossDeductable", cssClass: '' },
                   { tableHeader: "orderNo", tableValue: "orderNo", cssClass: 'nodisp' },
                    { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }
            ];
        }
        return gridObject;
    },
    loadSection: function (context, tableId) {
        
        var columnsValue = [];
        var columnDef = [];
        var colmorder = 0;
        if ($txSection.selectedSectionparentId == '')
        {
            colmorder = 4;
        }
        else {
            colmorder = 7;
        }
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
                                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');


                                b.button();
                                b.on('click', function () {
                                    $txSection.renderSection(oData);
                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    if (confirm('Are you sure ,do you want to delete?')) {
                                        $txSection.deleteData(oData.id);
                                    }
                                    return false;
                                });


                                $(nTd).empty();
                                $(nTd).prepend(b, c);
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
            "order": [[colmorder, "asc"]],
            //"aaData": data,
            ajax: function (data, callback, settings) {
                debugger;
                var type = "Section";
                if ($txSection.selectedSectionparentId == '' )
                {
                    type = "Section";
                } else
                {
                    if (!$txSection.selectedSectionparentId) {
                        type = "Section";
                    }
                    type = "SubSection";
                }
                var FinancialYrId = $txSection.financeYear;
                if (FinancialYrId != null)
                {
                    $.ajax({
                        type: 'POST',
                        // url: $app.baseUrl + "TaxSection/GetTaxSection",
                        url: $app.baseUrl + "TaxSection/GetTaxSection",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ financialyearId: $txSection.financeYear.id, parentId: $txSection.selectedSectionparentId, type: type }),
                        dataType: "json",
                        success: function (jsonResult) {
                            
                            $app.clearSession(jsonResult);
                            switch (jsonResult.Status) {
                                case true:
                                    $txSection.taxSection = jsonResult.result;
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
                }
                if (FinancialYrId == null)
                {
                    $app.showAlert("Set default Financial Year", 4);
                }

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

    
        $('#AddSection').modal('toggle');
        var formData = document.forms["frmTxSection"];
        $txSection.selectedSectionId = data.id;
        formData.elements["txtTxSecName"].value = data.name;
        formData.elements["txtTxSecNumber"].value = data.orderNo;
        formData.elements["txtSectionLimit"].value = data.limit;
        
        console.log(data.documentReq);
        if (data.documentReq == 'Yes') {
            $('#chkBillReq').prop('checked', true);
        }
        if (data.documentReq == 'No') {
            $('#chkBillReq').prop('checked', false);
        }

        if (data.Eligible == 'Yes') {
            $('#chkEligible').prop('checked', true);
        }
        if (data.Eligible == 'No') {
            $('#chkEligible').prop('checked', false);
        }


        if ($txSection.selectedSectionparentId != '') {
            $($txSection.formData).find('#ddTxSection').val(data.parentId);
            formData.elements["ddSecExemptionType"].value = data.exemptionType;
        }
        if (data.exemptionType == "Yearly")
        {
            formData.elements["ddSecExemptionType"].value=1;
        }
        else if (data.exemptionType == "Monthly") {
            formData.elements["ddSecExemptionType"].value=2;
        }
        else {
            formData.elements["ddSecExemptionType"].value=0;
        }
    },

    save: function () {
        
        if (!$txSection.canSave) {
            return false;
        }
        $txSection.canSave = false;
        $app.showProgressModel();
        var data = $txSection.selectedSectionparentId == null || $txSection.selectedSectionparentId == '' ? $txSection.BuildSectionObject() : $txSection.BuildSubSectionObject();
        $.ajax({
            url: $app.baseUrl + "TaxSection/SaveTaxSection",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                
                switch (jsonResult.Status) {
                    case true:
                        
                        $txSection.canSave = true;
                        var p = jsonResult.result;
                        $txSection.selectedSectionId = p.id;
                        $app.hideProgressModel();
                        $txSection.loadInitial();
                        $app.showAlert(jsonResult.Message, 2);
                        if ($txSection.selectedSectionparentId == '') {
                            $txSection.addInitializeSection();
                        } else {
                            $txSection.addInitializeSubSection();
                        }

                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $txSection.canSave = true;
                        break;
                }
                return false;
            },
            complete: function () {
                $txSection.canSave = true;
            }
        });
    },
    clearControl: function () {

    },
    addInitializeSection: function () {
        
        // $('#AddSection').modal('toggle');
        $txSection.selectedSectionId = '';
        $txSection.selectedSectionparentId = '';
        $($txSection.formData).find('#txtTxSecName').val('');
        //$($txSection.formData).find('#txtSectionDisplayAs').val('');
        $($txSection.formData).find('#txtSectionLimit').val('');
        $($txSection.formData).find('#txtTxSecNumber').val('');
        //  $($txSection.formData).find('#ddSecExemptionType').val(1);
        //  $($txSection.formData).find('#chkSecGrossDeductable').prop('checked', false);
        //  $($txSection.formData).find('#chkSecDocumentReq').prop('checked', false);
        //   $($txSection.formData).find('#chkSecApprovelReq').prop('checked', false);
    },
    addInitializeSubSection: function () {
        //   $('#AddSection').modal('toggle');
        $txSection.selectedSectionId = '00000000-0000-0000-0000-000000000000';
        $txSection.selectedSectionparentId = $('#sltTaxSection').val();

        $($txSection.formData).find('#ddTxSection').val($('#sltTaxSection').val());
        $($txSection.formData).find('#txtTxSecName').val('');
        //   $($txSection.formData).find('#txtSectionDisplayAs').val('');
        $($txSection.formData).find('#txtSectionLimit').val('');
        $($txSection.formData).find('#ddSecExemptionType').val(1);
        $($txSection.formData).find('#txtTxSecNumber').val('');
        // $($txSection.formData).find('#chkSecGrossDeductable').prop('checked', false);
        //     $($txSection.formData).find('#chkSecGrossDeductable').prop('checked', false);
        //    $($txSection.formData).find('#chkSecDocumentReq').prop('checked', false);
        //     $($txSection.formData).find('#chkSecApprovelReq').prop('checked', false);
    },
    deleteData: function (data) {
        
        var id = data;
        $.ajax({

            url: $app.baseUrl + "TaxSection/DeleteTaxSection",
            data: JSON.stringify({ sectionId: id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function () {
                $app.showAlert("deleted successfuly", 2);
                $txSection.loadInitial();
                $txOtherIncome.loadInitial();

            }
        })
    },
    BuildSectionObject: function () {
    
        var retObject = {
            id: $txSection.selectedSectionId,
            financialYearId: $txSection.financeYear.id,
            name: $($txSection.formData).find('#txtTxSecName').val(),
            displayAs: $($txSection.formData).find('#txtTxSecName').val(),
            orderNo: $($txSection.formData).find('#txtTxSecNumber').val(),
            limit: $($txSection.formData).find('#txtSectionLimit').val(),
            status: "Yes",

        };
        return retObject;
    },
    BuildSubSectionObject: function () {
    
        var retObject = {
            id: $txSection.selectedSectionId,
            parentId: $($txSection.formData).find('#ddTxSection').val(),
            financialYearId: $txSection.financeYear.id,
            name: $($txSection.formData).find('#txtTxSecName').val(),
            displayAs: $($txSection.formData).find('#txtTxSecName').val(),
            limit: $($txSection.formData).find('#txtSectionLimit').val(),
            status: "Yes",
            exemptionType: $($txSection.formData).find('#ddSecExemptionType').val(),
            documentReq: $($txSection.formData).find('#chkBillReq').prop('checked') == true ? 'Yes' : 'No',
            orderNo: $($txSection.formData).find('#txtTxSecNumber').val(),
            Eligible: $($txSection.formData).find('#chkEligible').prop('checked')== true ? 'Yes' : 'No'
            //grossDeductable: $($txSection.formData).find('#chkSecGrossDeductable').prop('checked') == true ? 'Yes' : 'No',
            //documentReq: $($txSection.formData).find('#chkSecDocumentReq').prop('checked') == true ? 'Yes' : 'No',
            //approvelReq: $($txSection.formData).find('#chkSecApprovelReq').prop('checked') == true ? 'Yes' : 'No',
        };
        return retObject;
    },
    saveTaxSection: function () {
    
    if (!$txSection.canSave) {
        return false;
    }
    $txSection.canSave = false;
    $app.showProgressModel();
    var id = $txSection.selectedSectionId;
    if (id == null || id=="")
    {
        id = '00000000-0000-0000-0000-000000000000';
    }
    var financialYear= $txSection.financeYear.id;
    var  name= $($txSection.formData).find('#txtTxSecName').val();
    var displayAs= $($txSection.formData).find('#txtTxSecName').val();
    var orderNo= $($txSection.formData).find('#txtTxSecNumber').val();
    var limit= $($txSection.formData).find('#txtSectionLimit').val();
    var status = "Yes";
    $.ajax({
        url: $app.baseUrl + "TaxSection/SaveSection",
        data: JSON.stringify({ ID: id, financialYearID: financialYear, Name: name, Displayas: displayAs, Limit: limit, Status: status, OrderNO: orderNo }),
        dataType: "json",
        contentType: "application/json",
        type: "POST",
        async: false,
        success: function (jsonResult) {
            
            switch (jsonResult.Status) {
                case true:
                    
                    $txSection.canSave = true;
                    var p = jsonResult.result;
                    $txSection.selectedSectionId = p.id;
                    $app.hideProgressModel();
                    $txSection.loadInitial();
                    $app.showAlert(jsonResult.Message, 2);
                    $txSection.addInitializeSection();
                    break;
                case false:
                    $app.hideProgressModel();
                    $app.showAlert(jsonResult.Message, 4);
                    $txSection.canSave = true;
                    break;
            }
            return false;
        },
        complete: function () {
            $txSection.canSave = true;
        }
    });
    },
    saveTaxSubSection: function () {
        $app.showProgressModel();
        var id = $txSection.selectedSectionId;
        var parentId = $($txSection.formData).find('#ddTxSection').val();
        if (parentId == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert('Please select the section', 4);
            $app.hideProgressModel();
        }
        else {
            var financialYearId = $txSection.financeYear.id;
            var name = $($txSection.formData).find('#txtTxSecName').val();
            var displayAs = $($txSection.formData).find('#txtTxSecName').val();
            var limit = $($txSection.formData).find('#txtSectionLimit').val();
            var exemptionType = $($txSection.formData).find('#ddSecExemptionType').val();
            var documentReq = $($txSection.formData).find('#chkBillReq').prop('checked') == true ? 'Yes' : 'No';
            var Eligible = $($txSection.formData).find('#chkEligible').prop('checked') == true ? 'Yes' : 'No';
            var orderNo = $($txSection.formData).find('#txtTxSecNumber').val();
            $.ajax({
                url: $app.baseUrl + "TaxSection/SaveSubSection",
                data: JSON.stringify({ ID: id, parentId: parentId, FinancialYearId: financialYearId, Name: name, displayAs: displayAs, limit: limit, exemptionType: exemptionType, documentReq: documentReq, orderNo: orderNo,Eligible: Eligible }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                async: false,
                success: function (jsonResult) {
                    
                    switch (jsonResult.Status) {
                        case true:
                            
                            var p = jsonResult.result;
                            $txSection.selectedSectionId = p.id;
                            $app.hideProgressModel();
                            $txSection.loadInitial();
                            $app.showAlert(jsonResult.Message, 2);
                            $txSection.addInitializeSubSection();
                            break;
                        case false:
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 4);
                            $txSection.canSave = true;
                            break;
                    }
                    return false;
                },
                complete: function () {
                    $txSection.canSave = true;
                }
            });
        }
    }
};