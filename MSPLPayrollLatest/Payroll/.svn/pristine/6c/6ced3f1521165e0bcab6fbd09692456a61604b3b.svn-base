
/*$("#btnAddNew").click(function () {

    if ($dyanmicEntity.selectedEntityModelId == '' || $dyanmicEntity.selectedEntityModelId == null) {
        $app.showAlert('Please select the Model', 4);
    }
    else {
        $dyanmicEntity.GetEntity({ Id: '00000000-0000-0000-0000-000000000000', EntityModelId: $dyanmicEntity.selectedEntityModelId });
        $dyanmicEntity.canSave = true;
    }
});
$("#btnCopy").click(function () {

    var tab = $('#tbl_' + $dyanmicEntity.selectedEntityModelId + ' tbody .selected td');
    var sourceentityid = $(tab[0]).text();
    if (sourceentityid != '' && sourceentityid != null) {
        $dyanmicEntity.copyEntity({ Id: sourceentityid, EntityModelId: $dyanmicEntity.selectedEntityModelId })
        return false;
    }
    else {
        $app.showAlert('Please select any Row before copy', 4);
        return false;
    }
});*/

$BABehavior = {
    canSave: false,
    selectedEntityModelId: null,
    selectedEntityModelname: null,
    refEntityList: null,
    entity: null,
    selectedEntityId: null,
    LoadEntityModelDrop: function () {

        $dyanmicEntity.LoadEntityModel({ id: 'BAsltEntityModel' });
        $payroll.initDatetime();
    },
    LoadEntityModel: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetEntityModels",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {

                    case true:
                        var out = jsonResult.result;
                        $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                        $.each(out, function (index, object) {

                            if (object.Name.trim() != "AdditionalInfo") {

                                $('#' + dropControl.id).append($("<option></option>").val(object.Id).html(object.Name));
                            }
                        });
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
        $('#' + dropControl.id).change(function () {

            if ($('#' + dropControl.id).val() == 0) {
                $("#BAdvDynamicEntity").html('');
                $("#BAdvTitle").html('');
                $("#btnCopy").addClass('nodisp');
                $("#btnAddNew").addClass('nodisp');

            }
            else {
                $("#btnCopy").removeClass('nodisp');
                $("#btnAddNew").removeClass('nodisp');
                $dyanmicEntity.selectedEntityModelname = $('#' + dropControl.id).find('option:selected').text();
                $dyanmicEntity.selectedEntityModelId = $('#' + dropControl.id).val();
                $dyanmicEntity.LoadGridData();
            }
        });
    },
    renderFieldGrid: function (context, tableprop) {

        $("#BAdvTitle").html('');
        $("#BAdvTitle").html('<h4>' + $dyanmicEntity.selectedEntityModelname + '</h4>');
        var grid = '<table id="tbl_' + tableprop.id + '" class="table table-responsive table-striped table-hover table-condensed userTablehand" width:100%>'
            + '<thead>'
            + '<tr>'
            + '<th class="nodisp">'
            + '</th><th>Action</th>'
        for (var cnt = 1; cnt < context.length; cnt++) {
            grid = grid + '<th class="word-wrap">' + context[cnt].tableHeader + '</th>'

        }
        grid = grid + '</tr></thead>';
        //grid = grid + '<th>Action</th></tr></thead>';
        grid = grid + '<tbody><tr>'
        for (var cnt = 0; cnt <= context.length; cnt++) {//for action td 
            grid = grid + '<td></td>';
        }
        grid = grid + '</tr></tbody></table>';
        $("#BAdvDynamicEntity").html('');
        $("#BAdvDynamicEntity").html(grid);

    },
    renderGrid: function (context) {

        var gridObject = [];
        gridObject.push({ tableHeader: "Id", tableValue: 'Id' });
        //gridObject.push({ tableHeader: "Action", tableValue: 'null' });
        gridObject.push({ tableHeader: "Name", tableValue: 'Name' });
        for (var cont = 0; cont < context[0].EntityAttributeModelList.length; cont++) {
            if (context[0].EntityAttributeModelList[cont].IsHidden == false && context[0].EntityAttributeModelList[cont].AttributeModel.ContributionType == 1) {
                gridObject.push({
                    tableHeader: context[0].EntityAttributeModelList[cont].AttributeModel.DisplayAs,
                    tableValue: 'EntityAttributeModelList.' + cont + '.EntityAttributeValue.Value'
                });
            }
        }
        if (context.length == 1 && context[0].Id == '00000000-0000-0000-0000-000000000000') {
            context = null;
        }
        $dyanmicEntity.renderFieldGrid(gridObject, { id: $dyanmicEntity.selectedEntityModelId });
        $dyanmicEntity.LoadAttributeModels(context, gridObject, { id: $dyanmicEntity.selectedEntityModelId });
        //$app.applyseletedrow();

    },
    LoadGridData: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetBABehavior",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: $dyanmicEntity.selectedEntityModelId }),
            dataType: "json",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result[0];
                        $dyanmicEntity.refEntityList = jsonResult.result[1];
                        $dyanmicEntity.renderGrid(out);
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
    LoadAttributeModels: function (data, context, tableprop) {

        var columnsValue = [];
        //if (data != null) {
        //    columnsValue.push({ "data": null }); //for action column
        //} else {
        //    columnsValue.push({ "data": '' }); //for action column
        //}

        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (cnt == 0) {
                columnsValue.push({ "data": null }); //for action column
            }
        }


        var columnDef = [];
        columnDef.push({ "aTargets": [0], "sClass": "nodisp", "bSearchable": false }); //for id column
        if (data != null) {
            for (var cnt1 = 1; cnt1 <= context.length; cnt1++) {
                if (cnt1 == 1) {
                    columnDef.push(
                        {
                            "aTargets": [cnt1],
                            "sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var b = $('<div style="margin-top:10px;"<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button><br/></div>');
                                var d = $('<div style="margin-top:10px;"<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button><br/></div>');
                                var c = $('<div style="margin-top:10px;"<a href="#" class="editeButton" title="Copy"><span aria-hidden="true" class="glyphicon glyphicon-copyright-mark"></span></button><br/></div>');

                                b.button();
                                b.on('click', function () {

                                    $dyanmicEntity.GetEntity({ Id: oData.Id, EntityModelId: $dyanmicEntity.selectedEntityModelId });
                                    $dyanmicEntity.canSave = true;
                                    return false;
                                });
                                d.button();
                                d.on('click', function () {

                                    $dyanmicEntity.EntityMappingCheck({ Id: oData.Id, EntityModelId: $dyanmicEntity.selectedEntityModelId, Name: oData.Name });
                                    //if (confirm('Are you sure,do you want to delete?'))
                                    //{
                                    //    $dyanmicEntity.deleteEntity({ Id: oData.Id });
                                    //    return false;
                                    //}
                                    //return false;
                                });
                                c.button();
                                c.on('click', function () {

                                    if (confirm('Are you sure,do you want to Copy this?')) {
                                        $dyanmicEntity.CopyDynamicGroupEntity({ Id: oData.Id, EntityModelId: $dyanmicEntity.selectedEntityModelId, Name: oData.Name });
                                    }

                                });
                                $(nTd).empty();
                                $(nTd).prepend(c, b, d);
                            }
                        }

                    ); //for action column
                }
                else {
                    columnDef.push({ "aTargets": [cnt1], "sClass": "word-wrap" }); //for action column
                }
            }
        }

        if (tableprop.id != null) {
            var dtClientList = $('#tbl_' + tableprop.id).DataTable({

                'iDisplayLength': 10,
                'bPaginate': true,
                'sPaginationType': 'full',
                'sDom': '<"top">rt<"bottom"ip><"clear">',
                columns: columnsValue,
                "aoColumnDefs": columnDef,
                "aaData": data,
                scrollY: "300px",
                //scrollX: true,
                // scrollCollapse: true,
                paging: true,
                fixedColumns: {
                    leftColumns: 3
                },
                fnInitComplete: function (oSettings, json) {

                    var r = $('#tblFormula tfoot tr');
                    r.find('th').each(function () {
                        $(this).css('padding', 8);
                    });
                    $('#tblFormula thead').append(r);
                    $('#search_0').css('text-align', 'center');
                },
                dom: "rtiS",
                destroy: true,
                scroller: {
                    loadingIndicator: true
                }
            });
        }
    },
    //Created By :Sharmila
    //Created On :3.05.17
    EntityMappingCheck: function (context) {

        $dyanmicEntity.selectedEntityId = context.Id;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetEntityMapping",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ entitymodelId: context.EntityModelId, entityId: context.Id, Name: context.Name }),
            dataType: "json",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert(jsonResult.Message, 4);
                        // var out = jsonResult.Message;
                        //$dyanmicEntity.RenderForm(out);
                        break;
                    case false:
                        if (confirm('Are you sure,do you want to delete?')) {
                            $dyanmicEntity.deleteEntity({ Id: context.Id });
                        }

                        //$app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },

    GetEntity: function (context) {

        $dyanmicEntity.selectedEntityId = context.Id;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetEntity",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: context.Id, entitymodelId: context.EntityModelId }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $dyanmicEntity.RenderForm(out);
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
    RenderForm: function (data) {
        $dyanmicEntity.selectedEntityId = data.Id;
        var formAttribute = [];
        formAttribute.push({
            type: "text",
            displayedAs: "Name",
            attributeName: "Name",
            attributeId: "Name",
            attributeModelId: '',
            behaviorType: 'Master',
            minLength: "5",
            maxLength: "100",
            attributeValue: (data.Name == null) ? "" : data.Name,
            required: 1,
            readOnly: '',
            FdataType: '',
            monthlyInput: false,
            attrbuteBehavior: {},
            refEntityModelId: '',
        });
        for (var cnt = 0; cnt < data.EntityAttributeModelList.length; cnt++) {
            if (data.EntityAttributeModelList[cnt].AttributeModel.ContributionType == 2) {
                continue;
            }
            var valuetype = 3;
            var isShowFormula = false;
            var isHideFormula = false;
            var name = data.EntityAttributeModelList[cnt].AttributeModel.Name;
            //|| name == 'LD' || name == 'MD' || name == 'TDS' || name == 'PTAX' || name == 'ESI' || name == 'PF'
            if (data.EntityAttributeModelList[cnt].AttributeModel.IsMonthlyInput || data.EntityAttributeModelList[cnt].AttributeModel.IsInstallment || (data.EntityAttributeModelList[cnt].AttributeModel.IsSetting && data.EntityAttributeModelList[cnt].AttributeModel.Name == "MD")) {
                isShowFormula = true;
                valuetype = 2;
            }

            var percentage = "";
            if (data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue == "") {
                data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue = data.EntityAttributeModelList[cnt].DefaultValue;
            }
            else {

                var defaultpercent = [];
                defaultpercent = data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue.split('%');
                if (defaultpercent.count > 1) {
                    data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue = defaultpercent[0];
                }
                else {
                    data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue = defaultpercent[0];
                    percentage = defaultpercent[1];
                }
            }


            if (data.EntityAttributeModelList[cnt].AttributeModel.Name == "PF") {
                valuetype = 3;
                percentage = 100;
            }
            //created by mubarak
            //inorder to hide the formula button to supplementary days and lop credit days

            if (data.EntityAttributeModelList[cnt].AttributeModel.Name == "SUPPDAYS" || data.EntityAttributeModelList[cnt].AttributeModel.Name == "LOPCREDITDAYS") {
                data.EntityAttributeModelList[cnt].AttributeModel.IsMonthlyInput = true;
                isShowFormula = null;
                valuetype = 2;

            }

            formAttribute.push({

                type: "text",
                displayedAs: data.EntityAttributeModelList[cnt].AttributeModel.DisplayAs,
                attributeName: data.EntityAttributeModelList[cnt].AttributeModel.Name,
                attributeId: data.EntityAttributeModelList[cnt].Id,
                attributeModelId: data.EntityAttributeModelList[cnt].AttributeModelId,
                behaviorType: data.EntityAttributeModelList[cnt].AttributeModel.BehaviorType,//IsMasterField,
                monthlyInput: isShowFormula,
                minLength: "5",
                maxLength: data.EntityAttributeModelList[cnt].AttributeModel.DataSize,
                attributeValue: ((data.EntityAttributeModelList[cnt].EntityAttributeValue.Value == null) || ((data.EntityAttributeModelList[cnt].EntityAttributeValue.Value == "NULL"))) ? ((data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue == null) || (data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue == "NULL")) ? "" : data.EntityAttributeModelList[cnt].AttributeModel.DefaultValue : data.EntityAttributeModelList[cnt].EntityAttributeValue.ValueCode == '' ? data.EntityAttributeModelList[cnt].EntityAttributeValue.Value : data.EntityAttributeModelList[cnt].EntityAttributeValue.ValueCode,
                required: data.EntityAttributeModelList[cnt].AttributeModel.IsMandatory,
                readOnly: data.EntityAttributeModelList[cnt].AttributeModel.BehaviorType == 'Master' ? '' : 'readonly',
                //readOnly: data.EntityAttributeModelList[cnt].AttributeModel.BehaviorType == 'Master' ? '' : '',
                FdataType: data.EntityAttributeModelList[cnt].AttributeModel.DataType,
                attrbuteBehavior: {
                    installment: data.EntityAttributeModelList[cnt].AttributeModel.IsInstallment,
                    increment: data.EntityAttributeModelList[cnt].AttributeModel.IsIncrement,
                    monthlyInput: data.EntityAttributeModelList[cnt].AttributeModel.IsMonthlyInput,
                    includeForGrossPay: data.EntityAttributeModelList[cnt].AttributeModel.IsIncludeForGrossPay,
                },
                refEntityModelId: data.EntityAttributeModelList[cnt].AttributeModel.RefEntityModelId,
                percentage: percentage,
                valueType: valuetype
            });
        }
        //
        var returnval = '<form role="form" id="' + data.Id + '"><div class="modal-dialog"> '
            + '<div class="modal-content"> '
            + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
            + '  &times;</button>'
            + '  <h4 class="modal-title" id="H4">'
            + '    Add/Edit ' + $dyanmicEntity.selectedEntityModelname + '</h4>'
            + ' </div>'
            + ' <div class="modal-body"> <div class="form-horizontal"> {formelemnt}  </div></div>';
        var formelemnt = '';
        var earningElmt = '';
        var dedutElmt = '';
        var dedFormula = '';
        var earFormula = '';
        for (var cnt = 0; cnt < formAttribute.length; cnt++) {

            if (formAttribute[cnt].behaviorType == 'Earning' && formAttribute[cnt].attrbuteBehavior.includeForGrossPay == true) {

                earFormula = earFormula + formAttribute[cnt].attributeName + "+";
            } else if (formAttribute[cnt].behaviorType == 'Deduction' && formAttribute[cnt].attrbuteBehavior.includeForGrossPay == true) {
                dedFormula = dedFormula + formAttribute[cnt].attributeName + "+";
            }
        }
        earFormula = earFormula.replace(/(^[+\s]+)|([+\s]+$)/g, '');
        dedFormula = dedFormula.replace(/(^[+\s]+)|([+\s]+$)/g, '');


        for (var cnt = 0; cnt < formAttribute.length; cnt++) {


            if (formAttribute[cnt].attributeValue == null) { formAttribute[cnt].attributeValue = ''; }
            var maxlenth = '';
            var req = ''
            var reqLbl = ''
            if (formAttribute[cnt].monthlyInput == true) {

            } else
                if ((formAttribute[cnt].required == 1 && formAttribute[cnt].monthlyInput != false)) {

                    req = "required";
                    reqLbl = '<label style="color:red;font-size: 13px">*</label>';
                }
            var addhighlight = '';
            if (formAttribute[cnt].attributeValue == null) { formAttribute[cnt].attributeValue = ''; }
            if (formAttribute[cnt].attributeValue != null && formAttribute[cnt].attributeValue != '' && formAttribute[cnt].attributeValue != "NULL") {
                addhighlight = 'style = "color: blue;"';

            }
            var temp = '<div class="form-group">'
                + ' <label class="control-label col-md-4"' + addhighlight + ' id="lbl' + formAttribute[cnt].attributeId + '">' + formAttribute[cnt].displayedAs + ' ' + reqLbl + '</label>'
                + '<div class="col-md-6">'
            if (formAttribute[cnt].refEntityModelId != '' && formAttribute[cnt].refEntityModelId != '00000000-0000-0000-0000-000000000000')//dropdown
            {
                temp = temp + '<select class="form-control" ' + formAttribute[cnt].readOnly + ' id="' + formAttribute[cnt].attributeId + '" placeholder="' + formAttribute[cnt].displayedAs + '" >'
                for (var refCnt = 0; refCnt < $dyanmicEntity.refEntityList.length; refCnt++) {
                    if ($dyanmicEntity.refEntityList[refCnt].refEntityModelId == formAttribute[cnt].refEntityModelId) {
                        for (var refEntitycnt = 0; refEntitycnt < $dyanmicEntity.refEntityList[refCnt].refEntityList.length; refEntitycnt++) {
                            var seleted = '';
                            if ($dyanmicEntity.refEntityList[refCnt].refEntityList[refEntitycnt].entityId == formAttribute[cnt].attributeValue) {
                                seleted = 'selected="selected"';
                            }
                            temp = temp + '<option value="' + $dyanmicEntity.refEntityList[refCnt].refEntityList[refEntitycnt].entityId + '" ' + seleted + '> ' + $dyanmicEntity.refEntityList[refCnt].refEntityList[refEntitycnt].entityName + '</option>'
                        }
                        break;
                    }
                }
                temp = temp + '</select>'
            }
            else if (formAttribute[cnt].FdataType == 'Date') {
                temp = temp + '<input type="' + formAttribute[cnt].type + '" class="form-control datepicker" ' + formAttribute[cnt].readOnly + ' id="' + formAttribute[cnt].attributeId + '" value="' + formAttribute[cnt].attributeValue
                temp = temp + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + ' readonly />'
            }
            else if (formAttribute[cnt].FdataType == 'Bool') {
                temp = temp + '<input type="checkbox"' + formAttribute[cnt].readOnly + ' id="' + formAttribute[cnt].attributeId + '"';
                if (formAttribute[cnt].attributeValue == "Yes")
                    temp = temp + 'checked="checked"';
                temp = temp + '/>';
            }
            else {
                if (formAttribute[cnt].behaviorType != 'Earning' && formAttribute[cnt].behaviorType != 'Deduction') {
                    maxlenth = 'maxlength="' + formAttribute[cnt].maxLength + '"';
                }
                if (formAttribute[cnt].displayedAs.toLowerCase() == "total deduction" || formAttribute[cnt].attributeName.toLowerCase() == "totded") {
                    formAttribute[cnt].attributeValue = dedFormula;
                } if (formAttribute[cnt].displayedAs.toLowerCase() == "gross pay" || formAttribute[cnt].attributeName.toLowerCase() == "eg") {
                    formAttribute[cnt].attributeValue = earFormula;
                }
                if (formAttribute[cnt].displayedAs.toLowerCase() == "net pay" || formAttribute[cnt].attributeName.toLowerCase() == "netpay") {

                    formAttribute[cnt].attributeValue = 'EG-TOTDED';
                }
                var attvallen = formAttribute[cnt].attributeValue.length;
                if (attvallen < 20) {
                    attvallen = 20;
                }
                else if (100 > attvallen && attvallen > 20) {
                    attvallen = 50;
                }
                else if (150 > attvallen && attvallen > 100) {
                    attvallen = 100;
                }
                else {
                    attvallen = 150;
                }

                temp = temp + '<textarea type="' + formAttribute[cnt].type + '" class="form-control" ' + formAttribute[cnt].readOnly + ' id="' + formAttribute[cnt].attributeId + '" value="' + formAttribute[cnt].attributeValue
                temp = temp + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + maxlenth + ' ' + req + "style=min-height:" + attvallen + "px ! important" + ' >' + formAttribute[cnt].attributeValue + '</textarea>'
            }
            temp = temp + ' </div>'
            //new code for formula
            //if (formAttribute[cnt].attributeModelId != '' && formAttribute[cnt].isMasterField == false) {

            if ((formAttribute[cnt].behaviorType == 'Earning' || formAttribute[cnt].behaviorType == 'Deduction') && formAttribute[cnt].monthlyInput == false) {
                temp = temp + '<div class="col-md-2"><button type="button" id="' + formAttribute[cnt].attributeModelId + '" class="btn custom-button-resize  formula"> Formula</button></div>';
            }

            if ((formAttribute[cnt].behaviorType == 'Earning' || formAttribute[cnt].behaviorType == 'Deduction') && formAttribute[cnt].monthlyInput == true) {
                temp = temp + '<div class="col-md-2"><button type="button" id="' + formAttribute[cnt].attributeModelId + '" class="btn custom-button-resize formula"> Rounding</button></div>';
            }
            //end 
            // else {
            temp = temp + '</div>';
            //}
            if (formAttribute[cnt].behaviorType == 'Earning') {
                earningElmt = earningElmt + temp;
            }
            else if (formAttribute[cnt].behaviorType == 'Deduction') {
                dedutElmt = dedutElmt + temp;
            }
            else {
                formelemnt = formelemnt + temp;
            }
        }
        returnval = returnval + ' <div class="modal-footer">'
            + ' <button type="submit" id="btnBranchSave" class="btn custom-button">'
            + '  Save</button>'
            + ' <button type="button" class="btn custom-button" data-dismiss="modal">'
            + '   Close</button>'
            + ' </div> </div>';
        returnval = returnval + '</form>'
        if (earningElmt != '') {
            earningElmt = '<h4>Earning\'s</h5><hr/>' + earningElmt;
        }
        if (dedutElmt != '') {
            dedutElmt = '<h4>Deduction\'s</h5><hr/>' + dedutElmt;
        }
        formelemnt = formelemnt + earningElmt + dedutElmt;

        returnval = returnval.replace('{formelemnt}', formelemnt);
        document.getElementById("BArenderId").innerHTML = returnval;
        $payroll.initDatetime();
        $('#BAAddData').modal('toggle');
        $("#" + data.Id).on('submit', function (event) {
            if ($app.requiredValidate(data.Id, event)) {
                $dyanmicEntity.save({ id: data.Id, entityModelId: data.EntityModelId });
                return false;
            }
            else {
                return false;
            }
        });
        //new code formula
        $(".formula").on('click', function (event) {
            //$('#AddData').modal('toggle');  
            if (data.Id == '' || data.Id == null) {
                $app.showAlert('Please save the form before formula creation', 4);
                return false;
            }
            else {
                var atrributeId = $(this).parent().parent().find('textarea')[0].id;
                $formulaCreation.loadEntityBehavior({ AttributeModelId: this.id, EntityId: $dyanmicEntity.selectedEntityId, EntityModelId: data.EntityModelId, data: $dyanmicEntity.getAttributeModel(data, this.id) }, atrributeId, document.getElementById(atrributeId).value);
            }
        });
        //end


    },
    getAttributeModel: function (datas, attrbuteModelId) {

        for (var cnt = 0; cnt < datas.EntityAttributeModelList.length; cnt++) {
            if (datas.EntityAttributeModelList[cnt].AttributeModelId == attrbuteModelId) {
                return datas.EntityAttributeModelList[cnt];
            }
        }

    },
    save: function (context) {
        debugger;
        if (!$dyanmicEntity.canSave) {
            return false;
        }
        $dyanmicEntity.canSave = false;
        $app.showProgressModel();
        var keyvalues = [];
        $("form#" + context.id + " :input").each(function (int, input) {
            // var input = $(this); // This is the jquery object of the input, do what you will            
            try {
                if (input.tagName == "INPUT" && input.type == "text") {
                    keyvalues.push({ id: input.id, value: input.value });//name: input.name,
                }
                else if (input.tagName == "INPUT" && input.type == "checkbox") {
                    if ($('#' + input.id).is(":checked") == true) {
                        keyvalues.push({ id: input.id, value: 'Yes' });//name: input.name,
                    } else {
                        keyvalues.push({ id: input.id, value: 'No' });//name: input.name,
                    }

                }
                else if (input.tagName == "SELECT") {
                    keyvalues.push({ id: input.id, name: $(input).find('option:selected').text(), value: input.value });
                }
                else if (input.tagName == "BUTTON") {
                    // continue;
                    //$('#' + dropControl.id).find('option:selected').text()
                }
                if (input.tagName == "TEXTAREA" && input.type == "textarea") {
                    keyvalues.push({ id: input.id, value: input.value });//name: input.name,
                }
            }
            catch (sx) {
            }

        });
        var formdata = {
            entityId: $dyanmicEntity.selectedEntityId,//context.id,
            entityModelId: context.entityModelId,
            EntityKeyValues: keyvalues
        };
        $.ajax({
            url: $app.baseUrl + "Entity/SaveBABehavior",
            data: JSON.stringify({ dataValue: formdata }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $('#BAAddData').modal('toggle');
                        $dyanmicEntity.LoadGridData();
                        $app.clearControlValues(context.id);
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

    deleteEntity: function (data) {

        $.ajax({
            url: $app.baseUrl + "Entity/DeleteBABehavior",
            data: JSON.stringify({ id: data.Id, entityModelId: $dyanmicEntity.selectedEntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $dyanmicEntity.LoadGridData();
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
};
