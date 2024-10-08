//$(document).ready(function () {
//    $payrollHistroy.LoadEntityModelDrop();
//});
$("#btnSave").click(function () {

    $payrollHistroy.save();
});
$("#btnDelete").click(function () {
    debugger;
    var rows = $("#tbl_" + $payrollHistroy.selectedCategoryId).dataTable().fnGetNodes();
    var count = 0;
    for (var i = 0; i < rows.length; i++) {
        if ($(rows[i]).find(":eq(3)").html().trim() == "Not Process") {
            count++;
        }
    }
    if (rows.length != count) {
        if (confirm("Are you sure to delete the payroll process?")) {

            $taxHistory.selectedCategoryId = $('#sltCategory').val() == null ? "00000000-0000-0000-0000-000000000000" : $('#sltCategory').val();
            $taxHistory.year = $('#sltYear').val();
            $taxHistory.month = $('#sltMonth').val();
            $taxHistory.selectedpayrollType = $('#sltPayrollType').val();
            $taxHistory.financeYear;
            if ($('#chkincludeTax').prop('checked')) {
                $taxHistory.taxHistoryValidate(rows.length);
            }
            else {
                $payrollHistroy.deletePayroll();
            }


        }

    }

});
$('#sltMonth').change(function () {

    $payrollHistroy.LoadLock();
    $payrollHistroy.month = $('#sltMonth').val();
    $payrollHistroy.LoadPayrolltype();
    $payrollHistroy.LoadGridData();
});
$('#sltYear').change(function () {
    $payrollHistroy.year = $('#sltYear').val();
    $payrollHistroy.LoadPayrolltype();
    $payrollHistroy.LoadGridData();
    $payrollHistroy.LoadLock();
});
$('#sltPayrollType').change(function () {
    debugger;
    if ($('#sltPayrollType').val() == "0") {
        $('#dvPayrollprocess').addClass('nodisp');

    } else {
        $('#dvPayrollprocess').removeClass('nodisp')
        $("#dvCat").removeClass('nodisp');
        //  $payrollHistroy.selectedCategoryId = $('#sltPayrollType').val() != 'All Employees' ? $('#sltCategory').val() : '00000000-0000-0000-0000-000000000000';
        $payrollHistroy.selectedCategoryId = $('#sltPayrollType').val() != 'All Employees' ? $('#sltCategory').val() != null ? $('#sltCategory').val() : '00000000-0000-0000-0000-000000000000' : '00000000-0000-0000-0000-000000000000';
        $payrollHistroy.selectedpayrollType = $('#sltPayrollType').val();
        $('#lblPayrollprocess').text($payrollHistroy.selectedpayrollType);
        $payrollHistroy.LoadPayrolltype();
        $payrollHistroy.LoadGridData();
        $payrollHistroy.LoadLock();
    }
});
$('#sltCategory').change(function () {

    if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000") {
        $payrollHistroy.selectedEntityModelname = $('#sltCategory').find('option:selected').text();
        $payrollHistroy.selectedCategoryId = $('#sltCategory').val();
        $payrollHistroy.LoadGridData();
    } else {
        $("#dvDynamicEntity").html('');
        $("#dvTitle").html('');

    }
});


$payrollHistroy = {
    selectedEntityModelname: null,
    selectedCategoryId: null,
    selectedpayrollType: $('#sltPayrollType').val(),
    month: $('#sltMonth').val(),
    year: $('#sltYear').val(),
    LoadPayrolltype: function () {

        if ($('#sltPayrollType').find('option:selected').text() == "Category") {
            $companyCom.loadCategory({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "CostCentre") {
            $companyCom.loadCostCentre({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Designation") {
            $companyCom.loadDesignation({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Branch") {
            $companyCom.loadBranch({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Department") {
            $companyCom.loadDepartment({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Location") {
            $companyCom.loadLocation({ id: 'sltCategory' });
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "Single Employee" && $('#sltYear').val() != null && $('#sltMonth').val() != null) {
            $companyCom.loadSelectiveEmployee({ id: 'sltCategory', condi: 'empDOJ.' + $('#sltYear').val() + '.' + $('#sltMonth').val() });
            $payrollHistroy.selectedpayrollType = "Single Employee";
        }
        else if ($('#sltPayrollType').find('option:selected').text() == "All Employees" && $('#sltYear').val() != null && $('#sltMonth').val() != null) {
            $payrollHistroy.selectedEntityModelname = $('#sltPayrollType').find('option:selected').text() + ' for the month of ' + $('#sltMonth').find('option:selected').text() + '-' + $('#sltYear').find('option:selected').text();
            $payrollHistroy.selectedpayrollType = "All";
            $("#dvCat").addClass('nodisp');
        }




    },
    LoadEntityModelDrop: function () {

        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltPayrollType', condi: 'Payroll' }) //Load Payroll type
        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltMonth', condi: 'Month' });
        $companyCom.loadPreviousPayrollProcessMonthYear({ id: 'sltYear', condi: 'Year' });
    },
    renderFieldGrid: function (context, tableprop) {

        $("#dvTitle").html('');
        $("#dvTitle").html('<h4>' + $payrollHistroy.selectedEntityModelname + '</h4>');
        var grid = '<table id="tbl_' + tableprop.id + '" class="table table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer">'
            + '<thead>'
                    + '<tr>'
                    + '<th class="nodisp">'
                    + '</th>'
        for (var cnt = 1; cnt < context.length; cnt++) {
            grid = grid + '<th>' + context[cnt].tableHeader + '</th>'
        }
        grid = grid + '</tr></thead>';
        grid = grid + '<tbody><tr>'
        for (var cnt = 0; cnt < context.length; cnt++) {//for action td 
            grid = grid + '<td></td>';
        }
        grid = grid + '</tr></tbody></table>';

        if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000" || $payrollHistroy.selectedpayrollType == "All") {
            $("#dvDynamicEntity").html(grid);
        }
        else {
            $("#dvTitle").html('');
            $("#dvDynamicEntity").html('');
        }

    },
    renderGrid: function (context) {

        var gridObject = [];
        gridObject.push({ tableHeader: "Id", tableValue: 'employeeId' });
        gridObject.push({ tableHeader: "Employee Name", tableValue: 'employeeName' });
        gridObject.push({ tableHeader: "Employee Code", tableValue: 'employeeCode' });
        gridObject.push({ tableHeader: "Status", tableValue: 'status' });
        $payrollHistroy.renderFieldGrid(gridObject, { id: $payrollHistroy.selectedCategoryId });
        $payrollHistroy.LoadAttributeModels(context, gridObject, { id: $payrollHistroy.selectedCategoryId });


    },
    LoadGridData: function () {

        $payrollHistroy.month = $('#sltMonth').val();
        $payrollHistroy.year = $('#sltYear').val();
        if ($payrollHistroy.month == 0) {
            $app.showAlert('Please select Month', 4);
            return false;
        }
        else {
            $.ajax({
                type: 'POST',
                url: $app.baseUrl + "Entity/GetPayrollHistory",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    selectedId: $payrollHistroy.selectedCategoryId,
                    month: $payrollHistroy.month,
                    year: $payrollHistroy.year,
                    type: $payrollHistroy.selectedpayrollType
                }),
                dataType: "json",
                success: function (jsonResult) {
                    $app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:
                            var out = jsonResult.result;
                            $payrollHistroy.renderGrid(out);
                            break;
                        case false:
                            $app.showAlert(jsonResult.Message, 4);
                            //alert(jsonResult.Message);
                            break;
                    }

                },
                error: function (msg) {
                }
            });
        }
    },

    LoadLock: function () {

        $payrollHistroy.month = $('#sltMonth').val();
        $payrollHistroy.year = $('#sltYear').val();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetLockLoad",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                month: $payrollHistroy.month,
                year: $payrollHistroy.year
            }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        if (out.PayrollLock == 1) {
                            $("#btnSave").hide();
                            $("#btnDelete").hide();
                        }
                        else {
                            $("#btnSave").show();
                            $("#btnDelete").show();
                        }
                        break;
                    case false:
                        $("#btnSave").show();
                        $("#btnDelete").show();
                        break;
                }

            },
            error: function (msg) {
            }
        });

    },
    LoadAttributeModels: function (data, context, tableprop) {

        var columnsValue = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
        }
        var columnDef = [];
        columnDef.push({ "aTargets": [0], "sClass": "nodisp", "bSearchable": false }); //for id column
        if (data != null) {
            for (var cnt1 = 1; cnt1 < context.length; cnt1++) {
                if (cnt1 == 1) {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.status == 'Processed' || oData.status == 'Imported') {
                                    var b = $('<a href="#" class="editeButton"><span>' + oData.employeeName + '</span></button>');
                                    var c = $('<a href="#" class="printButton"> <img src="assets/plugins/TableTools-master/images/pdf.png"></button>');
                                    b.button();
                                    b.on('click', function () {
                                        $payrollHistroy.viewHistory(oData.employeeId);
                                        return false;
                                    });
                                    c.button();
                                    c.on('click', function () {
                                        $payrollHistroy.printHistory(oData.employeeId);
                                    });
                                    $(nTd).empty();
                                    $(nTd).prepend(b, c);

                                }
                                else { return }
                            }
                        }

                    );
                }
                else if (cnt1 == 3) {
                    columnDef.push(
                       {
                           "aTargets": [cnt1],
                           "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                               if (oData.status != 'Processed' && oData.status != 'Imported' && oData.status != 'Not Process') {
                                   var b = $('<a href="#" class="editeButton"><span>Error</span></button>');
                                   b.button();
                                   b.on('click', function () {
                                       var sts = oData.status.split('&');
                                       var err = '';
                                       $('#dvErrorMsg').html('');
                                       $.each(sts, function (ind, item) {
                                           $('#dvErrorMsg').append('<p>' + item + '</p>')
                                       });
                                       // $('#dvErrorMsg').append('<p>' + sts + '</p>')
                                       $('#dvError').modal('toggle');
                                       return false;
                                   });
                                   $(nTd).empty();
                                   $(nTd).prepend(b);

                               }
                               else { return }
                           }
                       }

                   );
                }
                else if (cnt1 == 2) {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            "type": 'natural',
                            //"sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            }
                        }

                    ); //for action column
                }
                else {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            //"sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            }
                        }

                    ); //for action column
                }
            }
        }
        var dtClientList = $('#tbl_' + tableprop.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data,
            fnInitComplete: function (oSettings, json) {

                var r = $('#tblFormula tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblFormula thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "aaSorting": [[2, "asc"]],
            scroller: {
                loadingIndicator: true
            }
        });
        //$('#tbl_' + tableprop.id).on('click', 'tbody td:not(:first-child)', function (e) {
        //    dtClientList.inline(this);
        //});
    },
    save: function (context) {

        $app.showProgressModel();
        //var keyvalues = [];
        //$('#tbl_' + $payrollHistroy.selectedCategoryId + ' tbody tr').each(function (index, data) {
        //    var dtValues = [];
        //    var cnt = 0;
        //    $(data).find('td').each(function (ind, tmp) {

        //        if (cnt == 0) {
        //            dtValues.push({
        //                'EmployeeId': $(tmp).text()
        //            });
        //        }
        //        else {
        //            var input = $(tmp).find('input');
        //            if (input != null) {
        //                var id = $(input).prop('id');
        //                if (id != null) {
        //                    id = id.replace('txt_', '');
        //                    dtValues.push({
        //                        'Id': id, 'value': $(input).val()
        //                    });
        //                }
        //            }
        //        }
        //        cnt = cnt + 1;
        //    });
        //    var rowVal = [];
        //    for (var t1 = 1; t1 < dtValues.length; t1++) {
        //        rowVal.push({ 'AttributeModId': dtValues[t1].Id, 'MIValue': dtValues[t1].value });
        //    }
        //    keyvalues.push({ 'EmployeeId': dtValues[0].EmployeeId, 'Id': $payrollHistroy.selectedCategoryId, 'Month': $payrollHistroy.month, 'year': $payrollHistroy.year, MonthlyInputAttributes: rowVal });
        //});
        $.ajax({
            url: $app.baseUrl + "Entity/ProcessPayroll",
            data: JSON.stringify({ selectedId: $payrollHistroy.selectedCategoryId, year: $payrollHistroy.year, month: $payrollHistroy.month, type: $payrollHistroy.selectedpayrollType, includeTax: $('#chkincludeTax').prop('checked') }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: true,
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $payrollHistroy.LoadGridData();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, jsonResult.Message == "No Record(s) Processed" ? 1 : 2);
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
    deletePayroll: function (context) {
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Entity/DeletePayroll",
            data: JSON.stringify({ selectedId: $payrollHistroy.selectedCategoryId, year: $payrollHistroy.year, month: $payrollHistroy.month, type: $payrollHistroy.selectedpayrollType }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $payrollHistroy.LoadGridData();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, jsonResult.Message == "No Record(s) Processed" ? 1 : 2);
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
    viewHistory: function (employeeid) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetPayrollProcessdHistory",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ employeeId: employeeid, month: $payrollHistroy.month, year: $payrollHistroy.year }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        //   $entityMapping.createForm(out);
                        $payrollHistroy.renderDynamicForm(out, false);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    // Modified By Keerthika on 19/04/2016 error msg
    printHistory: function (employeeid) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/PrintPayrollProcessdHistory",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ employeeId: employeeid, month: $payrollHistroy.month, year: $payrollHistroy.year }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        var oData = new Object();
                        oData.employeeId = employeeid;
                        oData.filePath = jsonResult.result.filePath;
                        $app.downloadSync('Download/DownloadPaySlip', oData);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
                // return false;
            },
            error: function (msg) {
            }
        });


    },
    convertPDF: function (strhtml) {

        $.fileDownload($app.baseUrl + 'Download/ConvertPDF', {
            //preparingMessageHtml: "We are preparing your report, please wait...",
            //failMessageHtml: "There was a problem generating your report, please try again.",
            httpMethod: "POST",

            data: JSON.stringify({ strhtml: strhtml })//$(this).serialize()
          , successCallback: function (url) {
              $app.hideProgressModel();
          },
            failCallback: function (responseHtml, url) {
                $app.hideProgressModel();
                $app.showAlert('File not found', 4);

            }
        });
    },
    renderDynamicForm: function (data) {
        var formAttribute = [];
        formAttribute.push({
            type: "text",
            displayedAs: "Name",
            attributeName: "Name",
            attributeId: "Name",
            attributeModelId: 'Name',
            behaviorType: 'Master',
            isMasterField: '',
            minLength: "5",
            maxLength: "100",
            attributeValue: (data.Name == null) ? "" : data.Name,
            required: 1,
            readOnly: ' readonly="true"'
        });
        for (var cnt = 0; cnt < data.EntityAttributeModelList.length; cnt++) {
            formAttribute.push({
                type: "text",
                displayedAs: data.EntityAttributeModelList[cnt].AttributeModel.DisplayAs,
                attributeName: data.EntityAttributeModelList[cnt].AttributeModel.Name,
                attributeId: data.EntityAttributeModelList[cnt].Id,
                attributeModelId: data.EntityAttributeModelList[cnt].AttributeModelId,
                behaviorType: data.EntityAttributeModelList[cnt].AttributeModel.BehaviorType,
                isMasterField: data.EntityAttributeModelList[cnt].IsMasterField,
                minLength: "5",
                maxLength: data.EntityAttributeModelList[cnt].AttributeModel.DataSize,
                attributeValue: (data.EntityAttributeModelList[cnt].EntityAttributeValue.Value == null) ? (data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue == null) ? "" : data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue : data.EntityAttributeModelList[cnt].EntityAttributeValue.Value,
                required: data.EntityAttributeModelList[cnt].AttributeModel.IsMandatory,
                readOnly: ' readonly="true"'
            });
        }

        //
        var returnval = '<form role="form" id="' + data.Id + '"><div class="modal-dialog"> '
        + '<div class="modal-content"> '
        + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
              + '  &times;</button>'
          + '  <h4 class="modal-title" id="H4">'
            + $('#lbl_' + data.EntityModelId).text().toLowerCase() + ' Details </h4>'
       + ' </div>'
       + ' <div class="modal-body"> <div class="form-horizontal"> {formelemnt}  </div></div>';
        var formelemnt = '';
        var earningElmt = '';
        var dedutElmt = '';
        var netpayelemnt = '';
        var totDedelemnt = '';
        var earnedelemnt = '';
        for (var cnt = 0; cnt < formAttribute.length; cnt++) {
            var req = ''
            if (formAttribute[cnt].required == 1) {
                req = "required";
            }
            var temp = '<div class="form-group">'
                     + ' <label class="control-label col-md-4">' + formAttribute[cnt].displayedAs + '</label>'
                     + '<div class="col-md-7">'
                     + '<input type="' + formAttribute[cnt].type + '" class="form-control" id="' + formAttribute[cnt].attributeModelId + '" value="' + formAttribute[cnt].attributeValue
                     + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + formAttribute[cnt].readOnly + '/>'
            temp = temp + '</div></div>';
            if (formAttribute[cnt].behaviorType == 'Earning') {
                if (formAttribute[cnt].displayedAs == 'Net Pay') {
                    netpayelemnt = temp;
                }
                else if (formAttribute[cnt].displayedAs == 'Earned Gross') {
                    earnedelemnt = temp;
                }
                else {
                    earningElmt = earningElmt + temp;
                }

            }
            else if (formAttribute[cnt].behaviorType == 'Deduction') {
                if (formAttribute[cnt].displayedAs == 'Total Deduction') {
                    totDedelemnt = temp;
                }
                else {
                    dedutElmt = dedutElmt + temp;
                }

            }
            else {
                formelemnt = formelemnt + temp;
            }
            // formelemnt = formelemnt + temp;
        }
        returnval = returnval + '</form>'
        if (earningElmt != '') {
            earningElmt = '<h4>Earning\'s</h5><hr/>' + earningElmt;
        }
        if (dedutElmt != '') {
            dedutElmt = '<h4>Deduction\'s</h5><hr/>' + dedutElmt;
        }
        formelemnt = formelemnt + earningElmt + dedutElmt + '<hr/>' + earnedelemnt + totDedelemnt + netpayelemnt;

        returnval = returnval.replace('{formelemnt}', formelemnt);
        document.getElementById("dynamicProcessForm").innerHTML = returnval;
        $('#dynamicProcessForm').modal('toggle');

    }
    , GetFormString: function (data) {
        var formAttribute = [];
        formAttribute.push({
            type: "text",
            displayedAs: "Name",
            attributeName: "Name",
            attributeId: "Name",
            attributeModelId: 'Name',
            isMasterField: '',
            minLength: "5",
            maxLength: "100",
            attributeValue: (data.Name == null) ? "" : data.Name,
            required: 1,
            readOnly: ' readonly="true"'
        });
        for (var cnt = 0; cnt < data.EntityAttributeModelList.length; cnt++) {
            formAttribute.push({
                type: "text",
                displayedAs: data.EntityAttributeModelList[cnt].AttributeModel.DisplayAs,
                attributeName: data.EntityAttributeModelList[cnt].AttributeModel.Name,
                attributeId: data.EntityAttributeModelList[cnt].Id,
                attributeModelId: data.EntityAttributeModelList[cnt].AttributeModelId,
                isMasterField: data.EntityAttributeModelList[cnt].IsMasterField,
                minLength: "5",
                maxLength: data.EntityAttributeModelList[cnt].AttributeModel.DataSize,
                attributeValue: (data.EntityAttributeModelList[cnt].EntityAttributeValue.Value == null) ? (data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue == null) ? "" : data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue : data.EntityAttributeModelList[cnt].EntityAttributeValue.Value,
                required: data.EntityAttributeModelList[cnt].AttributeModel.IsMandatory,
                readOnly: ' readonly="true"'
            });
        }

        //
        var returnval = '<form role="form" id="' + data.Id + '"><div class="modal-dialog"> '
        + '<div class="modal-content"> '
        + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
              + '  &times;</button>'
          + '  <h4 class="modal-title" id="H4">'
            + $('#lbl_' + data.EntityModelId).text().toLowerCase() + ' Details </h4>'
       + ' </div>'
       + ' <div class="modal-body"> <div class="form-horizontal"> {formelemnt}  </div></div>';
        var formelemnt = '';
        for (var cnt = 0; cnt < formAttribute.length; cnt++) {
            var req = ''
            if (formAttribute[cnt].required == 1) {
                req = "required";
            }
            var temp = '<div class="form-group">'
                     + ' <label class="control-label col-md-4">' + formAttribute[cnt].displayedAs + '</label>'
                     + '<div class="col-md-7">'
                     + '<input type="' + formAttribute[cnt].type + '" class="form-control" id="' + formAttribute[cnt].attributeModelId + '" value="' + formAttribute[cnt].attributeValue
                     + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + formAttribute[cnt].readOnly + '/>'
            temp = temp + '</div></div>';
            formelemnt = formelemnt + temp;
        }

        returnval = returnval + '</form>'
        returnval = returnval.replace('{formelemnt}', formelemnt);
        return returnval;

    }
};