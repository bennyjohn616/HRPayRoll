$txOtherIncome = {
    tableid: 'tblOtherSection',
    financeYear: $companyCom.getDefaultFinanceYear(),
    formData: document.forms["frmTxSection"],
    selectedSectionId:null,
    loadInitial: function () {
        if ($txOtherIncome.financeYear!=null) {
            var gridObject = $txOtherIncome.getGridObject();
            var tableid = { id: $txOtherIncome.tableId };
            var modelContent = $screen.createTable(tableid, gridObject);
            $('#dvOtherSection').html(modelContent);
            $txOtherIncome.loadSection(gridObject, tableid);
        }
        else {
            $app.showAlert("Set default Financial Year", 4);
        }
        
    },
    clear: function () {
        
        $($txOtherIncome.formData).find('#txtTxSecName').val('');
        $($txOtherIncome.formData).find('#txtSectionLimit').val('');
        $($txOtherIncome.formData).find('#txtTxotherSecNumber').val('');
        $($txOtherIncome.formData).find('#sltIncomeType').val('00000000-0000-0000-0000-000000000000');
    },
    getGridObject: function () {

        var gridObject;

        gridObject = [
                    { tableHeader: "id", tableValue: "id", cssClass: 'nodisp' },
                    { tableHeader: "financialyearId", tableValue: "financialYearId", cssClass: 'nodisp' },
                    { tableHeader: "Name", tableValue: "name", cssClass: 'name' },
                    { tableHeader: "Limit", tableValue: "limit", cssClass: 'limit' },
                    { tableHeader: "Income Type", tableValue: "IncomeTypeName", cssClass: 'incomeType' },
                    { tableHeader: "orderNo", tableValue: "orderNo", cssClass: 'nodisp' },
                    { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }
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
                                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');


                                b.button();
                                b.on('click', function () {
                                    $txOtherIncome.renderSection(oData);
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
            "order": [[5, "asc"]],
            //"aaData": data,
            ajax: function (data, callback, settings) {
                


                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxSection/GetTaxSection",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ financialyearId: $txOtherIncome.financeYear.id, parentId: '', type: 'Others' }),
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
    save: function () {
        
        $app.showProgressModel();
        var id = $txOtherIncome.selectedSectionId;
        if (id == null)
        {
            id = '00000000-0000-0000-0000-000000000000';
        }
        var financialYearId= $txOtherIncome.financeYear.id;
        var name= $($txOtherIncome.formData).find('#txtTxSecName').val();
        var displayAs= $($txOtherIncome.formData).find('#txtTxSecName').val();
        var orderNo= $($txOtherIncome.formData).find('#txtTxotherSecNumber').val();//$($txSection.formData).find('#txtEmail').val(),
        var limit= $($txOtherIncome.formData).find('#txtSectionLimit').val();
        var incomeType= $($txOtherIncome.formData).find('#sltIncomeType').val();
        var sectionType = "Others";
        $.ajax({
            url: $app.baseUrl + "TaxSection/SaveOtherTaxSection",
            data: JSON.stringify({ ID: id, Financeyear: financialYearId, Name: name, Displayas: displayAs, OrderNo: orderNo, limit: limit, IncomeType: incomeType, sectionType: sectionType }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $txOtherIncome.selectedSectionId = p.id;
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);                   
                        $txOtherIncome.addInitializeSection();
                        $txOtherIncome.loadInitial();
                       

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
    },
    addInitializeSection: function () {
        
        $('#AddOtherIncomeHeads').modal('toggle');
        $txOtherIncome.selectedSectionId = null;
        
        $($txOtherIncome.formData).find('#txtTxSecName').val('');
      
        $($txOtherIncome.formData).find('#txtSectionLimit').val('');
     
        
    },
    BuildSectionObject: function () {
        
        var retObject = {
            id: $txOtherIncome.selectedSectionId,
            financialYearId: $txOtherIncome.financeYear.id,
            name: $($txOtherIncome.formData).find('#txtTxSecName').val(),
            displayAs: $($txOtherIncome.formData).find('#txtTxSecName').val(),
            orderNo: $($txOtherIncome.formData).find('#txtTxotherSecNumber').val(),//$($txSection.formData).find('#txtEmail').val(),
            limit: $($txOtherIncome.formData).find('#txtSectionLimit').val(),
            status: "Yes",
            incomeType: $($txOtherIncome.formData).find('#sltIncomeType').val(),
            sectionType:"Others"
        };
        return retObject;
    },
    renderSection: function (data) {

        $('#AddOtherIncomeHeads').modal('toggle');
        var formData = document.forms["frmTxSection"];
        $txOtherIncome.selectedSectionId = data.id;
        formData.elements["txtTxSecName"].value = data.name;
        formData.elements["txtSectionLimit"].value = data.limit;
        formData.elements["txtTxotherSecNumber"].value = data.orderNo;
        formData.elements["sltIncomeType"].value = data.incomeType;
    },
};

