﻿ 

$entityMapping = {
    canSave: false,
    refEntityId: null,
    refEntityModel: null,
    entityId: null,
    entitymodelId: null,
    displaytext: '',
    refEntityList: null,
    oldEntityId: null,
    mappingShow: null,
    newEntityId: null,
    saveMappingId: null,
    renderForm: function (refTableName, refEntityId, entityModelId, displaytext) {//new function
        debugger
        if (entityModelId == '') {
            $app.showAlert('Please contact administrator', 4);
            return false;
        }
        else {
            $entityMapping.entitymodelId = entityModelId;
            $entityMapping.refEntityId = refEntityId;
            $entityMapping.refEntityModel = refTableName;
            $entityMapping.displaytext = displaytext;
            context = { EntityModelId: $entityMapping.entitymodelId, refEntityId: $entityMapping.refEntityId, refEntityModel: $entityMapping.refEntityModel }
            $entityMapping.getEntityMapping(context);
        }
    },
    getEntityMapping: function (context) {
        debugger
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetEntityForEmployee",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ entitymodelId: context.EntityModelId, refEntityId: $entityMapping.refEntityId, refEntityModel: $entityMapping.refEntityModel, employeeId: $employee.selectedEmployeeId }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        if ($entityMapping.displaytext.trim() == "AdditionalInfo") {
                            $entityMapping.createAdditionalInfoForm(out);
                        } else {
                            if ($entityMapping.displaytext.trim() == "Salary") {
                                $employee.getAppSetting("Salary");
                            }
                            $entityMapping.createForm(out);

                        }
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    /* Modified By Keerthika
    Modified On 11/04/2017  */
    createForm: function (data) {//new function

        $entityMapping.entityId = data.Id;
        var isMaster = false;

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
                readOnly: (isMaster == true) ? '' : ' readonly="true"'
            });
        }
        var colms = 1;
        if (formAttribute.length > 12)
            colms = 3;
        else if (formAttribute.length > 6)
            colms = 2;
        else
            colms = 1;
        //col-md-offset-8
        var dd1 = new Date();
        var mm = dd1.getMonth() + 1;
        var yy = dd1.getFullYear();
        var tmp = '<div id="AddData" class="modal"  role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false"><div id="dynamicProcessForm"></div></div>'
        tmp = tmp + '<div class="row"><form id="frmEmpOthers"> <h4>' + $entityMapping.displaytext + ' ' + mm + '/' + yy + '</h4>';

       
        tmp = tmp + ' <div class="col-md-12 text-right " id="masterValueHide"><button type="button" id="btnChange"  class="btn custom-button marginbt4" data-toggle="modal" data-target="#AddCompany">Change ' + $entityMapping.displaytext + ' </button>';
        tmp = tmp + ' <button type="button" id="btnChangeEmpMaster"  class="btn custom-button marginbt4" data-toggle="modal" data-target="#AddCompany">Change Master</button> </div>';
         tmp = tmp + '<div class="col-md-12"> <div class="form-horizontal">'
        for (var cnt = 0; cnt < colms; cnt++) {
            tmp = tmp + '<div class="col-md-' + 12 / colms + '">{' + cnt + 'formelemnt} </div>';
        }
        tmp = tmp + ' </div></div></form></div>';
        var colElements = [];
        var formelemnt = '';
        for (var cnt = 0; cnt < formAttribute.length; cnt++) {
            var req = ''
           debugger
            var maxlength = '';

            // value = formAttribute[cnt].maxLength;
            if (formAttribute[cnt].maxLength != null && formAttribute[cnt].maxLength != "") {
                maxlength = "" + formAttribute[cnt].maxLength;
            }
            if (formAttribute[cnt].required == 1) {
                req = "required";
            }
            var temp = '<div class="form-group">'
                + ' <label class="control-label col-md-4">' + formAttribute[cnt].displayedAs + '</label>'
                + '<div class="col-md-7">'
                + '<input type="' + formAttribute[cnt].type + '" class="form-control" id="' + formAttribute[cnt].attributeModelId + '" value="' + formAttribute[cnt].attributeValue
                + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + formAttribute[cnt].readOnly + maxlength + '/>'
            temp = temp + '</div></div>';
           
            // formelemnt = formelemnt + temp;
            var modVal = (cnt) % colms;
            if (colElements[modVal] == undefined) {
                colElements[modVal] = '';
            }
            colElements[modVal] = colElements[modVal] + temp;
        }
        for (var ct = 0; ct < colElements.length; ct++) {
            tmp = tmp.replace('{' + ct + 'formelemnt}', colElements[ct]);
        }
        $("#empOthers").html(tmp);
        var role = $('#Rollid').val();
        if (role !== "1") {
            $('#masterValueHide').hide();
        }
        $('#btnChange').on('click', function (event) {

            $entityMapping.render($entityMapping.refEntityModel, $entityMapping.refEntityId);
        });
        $('#btnChangeEmpMaster').on('click', function (event) {
            debugger;
            if ($entityMapping.entityId == null || $entityMapping.entityId == undefined || $entityMapping.entityId == '' || $entityMapping.entityId == '00000000-0000-0000-0000-000000000000') {
                $app.showAlert('There is no ' + $entityMapping.displaytext + ' mapped with this employee.', 4);
                return false;
            }
            else {

                $entityMapping.checkMasterValue();
                return false;

            }

        });
    },
    //saveDate: function () {
    //    debugger
    //    var getdate = $('#selectedDate').val();
    //    var empId = $('#empid').val();
    //    $.ajax({
    //        url: $app.baseUrl + "Entity/SaveIncrementDate",
    //        data: JSON.stringify({ IncrementDate: getdate, Emplid: empId}),
    //        dataType: "json",
    //        contentType: "application/json",
    //        type: "POST",
    //        success: function (jsonResult) {
    //            $app.clearSession(jsonResult);
    //        },
    //        complete: function () {

    //        }
    //    });
    //},
    //renderIncreDate: function () {
    //    var empId = $('#empid').val();
    //    $.ajax({
    //        url: $app.baseUrl + "Entity/SelectIncrementDate",
    //        data: JSON.stringify({  Emplid: empId }),
    //        dataType: "json",
    //        contentType: "application/json",
    //        type: "POST",
    //        success: function (jsonResult) {
    //            $app.clearSession(jsonResult);
    //            var temp = '  <div class="modal" id="setIncrement" tabindex="-1" role="dialog" aria-labelledby="dateModalLabel" aria-hidden="true">'
    //            temp = temp + '    <div class="modal-dialog" role="document">'
    //            temp = temp + '    <div class="modal-content">'
    //            temp = temp + '         <div class="modal-header">'
    //            temp = temp + '             <h5 class="modal-title" id="dateModalLabel">Select Date</h5>'
    //            temp = temp + '            <button type="button" class="close" data-dismiss="modal" aria-label="Close">'
    //            temp = temp + '            <span aria-hidden="true">&times;</span>'
    //            temp = temp + '            </button>'
    //            temp = temp + '        </div>'
    //            temp = temp + '         <div class="modal-body">'
    //            temp = temp + '             <!-- Date input -->'
    //            temp = temp + '               <label for="selectedDate">Select Date:</label>'
    //            temp = temp + '               <input type="date" id="selectedDate" class="form-control">'
    //            temp = temp + '   </div>'
    //            temp = temp + '              <div class="modal-footer">'
    //            temp = temp + '  <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>'
    //            temp = temp + '                  <button type="button" class="btn btn-primary" onclick="$entityMapping.saveDate()">Save</button>'
    //            temp = temp + '                </div>'
    //            temp = temp + '            </div>'
    //            temp = temp + '        </div>'
    //            temp = temp + '      </div>'
    //            $("#empOthers").append(temp);
    //        },
    //        complete: function () {

    //        }
    //    }); },
          
         
   
    checkMasterValue: function (contxt) {

        $.ajax({
            url: $app.baseUrl + "Entity/GetEntityMasterValue",
            data: JSON.stringify({
                id: $entityMapping.entityId, entitymodelId: $entityMapping.entitymodelId,
                refEntityModel: $entityMapping.refEntityModel, refEntityId: $entityMapping.refEntityId
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        if (jsonResult.result.EntityAttributeModelList.length > 0) {
                            $entityMapping.renderDynamicForm(jsonResult.result, true);
                            return false;
                        }
                        else {
                            $app.showAlert('There is no master value for this employee.', 3);
                            return false;
                        }
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

    changeMasterValue: function () {
        debugger;
        $app.showProgressModel();
        $entityMapping.saveMappingId = 'true';
        $.ajax({
            url: $app.baseUrl + "Entity/ChangeEntityMasterValue",
            async: false,
            data: JSON.stringify({
                id: $entityMapping.newEntityId, entitymodelId: $entityMapping.entitymodelId,
                refEntityModel: $entityMapping.refEntityModel, refEntityId: $entityMapping.refEntityId, oldEntityId: $entityMapping.oldEntityId
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        if (jsonResult.result.EntityAttributeModelList.length > 0) {
                            $app.hideProgressModel();
                            $entityMapping.showForm(jsonResult.result);
                            // $entityMapping.renderDynamicForm(jsonResult.result, true);
                            return false;
                        }
                        else {
                            $app.hideProgressModel();
                            $app.showAlert('There is no master value for this employee.', 3);
                            return false;
                        }
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            },
        });
    },

    showForm: function (data) {
        $entityMapping.renderDynamicForm(data, true);
    },

    render: function (refTableName, refEntityId) {

        $entityMapping.refEntityId = refEntityId;
        $entityMapping.refEntityModel = refTableName;
        $.ajax({
            url: $app.baseUrl + "Entity/GetEntityModelMap",
            data: JSON.stringify({ refModelName: $entityMapping.refEntityModel, refEntityId: $entityMapping.refEntityId, entityModelId: $entityMapping.entitymodelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $entityMapping.buildPage(jsonResult.result);
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

    buildPage: function (data) {
        $entityMapping.oldEntityId = '';
        var returnval = '<form role="dialog" id="frm_' + $entityMapping.entitymodelId + '"><div class="modal-dialog"> '
            + '<div class="modal-content"> '
            + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
            + '  &times;</button>'
            + '  <h4 class="modal-title" id="H4">'
            + $entityMapping.displaytext + ' </h4>'
            + ' </div>'
            + ' <div class="modal-body"> <div class="form-horizontal"> {formelemnt}  </div></div>';
        var tmp = ''
        for (var cnt = 0; cnt < data.length; cnt++) {
            var t1 = '<div class="form-group">'
                + '<label class="control-label col-md-4" id="lbl_' + data[cnt].entityModelId.toLowerCase() + '">'
                + data[cnt].DisplayAs + '</label>'
                + ' <div class="col-md-6">'
                + '<select id="' + data[cnt].entityModelId + '" class="form-control" required></select>'
                + '</div>'
                + '</div>'
            tmp = tmp + t1;
        }
        returnval = returnval.replace('{formelemnt}', tmp);
        returnval = returnval + ' <div class="modal-footer">'
            + ' <button type="submit" id="btnBranchSave" class="btn custom-button">'
            + '  Save</button>'
            + ' <button type="button" class="btn custom-button" data-dismiss="modal">'
            + '   Close</button>'
            + ' </div> </div>';
        returnval = returnval + '</form>'
        $("#dynamicProcessForm").html(returnval);
        $('#AddData').modal('toggle');
        for (var cnt = 0; cnt < data.length; cnt++) {
            $('#' + data[cnt].entityModelId).append($("<option></option>").val('00000000 - 0000 - 0000 - 0000 - 000000000000').html('--Select--'));
            $.each(data[cnt].entityCollection, function (index, entity) {
                $('#' + data[cnt].entityModelId).append($("<option></option>").val(entity.entityId.toLowerCase()).html(entity.entityName));
            });

            $.each(data[cnt].seletedEntity, function (index, entity) {
                if (entity.entityId != null) {
                    $('#' + data[cnt].entityModelId).val(entity.entityId.toLowerCase());
                    $entityMapping.oldEntityId = entity.entityId;
                }
            });
        }
        $('#frm_' + $entityMapping.entitymodelId).on('submit', function () {
            $entityMapping.checkSave();
        });
    },

    checkSave: function () {
        $entityMapping.mappingShow = '';
        $entityMapping.newEntityId = '';
        var input = '';
        var curentity = ''
        $('form#frm_' + $entityMapping.entitymodelId + ' :input').each(function () {
            input = $(this);
            if (input[0].tagName == "BUTTON") {
                // continue;
            }
            else {
                curentity = input[0].value;
            }
        });

        debugger;
        $entityMapping.newEntityId = curentity;
        if ($entityMapping.oldEntityId != "" && $entityMapping.oldEntityId != curentity) {
            $entityMapping.mappingShow = "NO";
            $('#frm_' + $entityMapping.entitymodelId).modal('hide');
            $entityMapping.changeMasterValue();
        }
        else {
            $entityMapping.SaveMapping();
            $('#frm_' + $entityMapping.entitymodelId).modal('hide');
            $('body').removeClass('modal-open');
            return false;
        }
    },

    save: function (context) {
        debugger;
        if (!$entityMapping.canSave) {
            return false;
        }
        $entityMapping.canSave = false;
        $app.showProgressModel();
        var keyvalues = [];
        debugger;

        $("form#" + context.id + " :input").each(function () {
            var input = $(this); // This is the jquery object of the input, do what you will
            if (input[0].tagName == "BUTTON") {
                // continue;
            }
            else {
                keyvalues.push({ id: input[0].id, name: input[0].name, value: input[0].value });
            }
        });
        var formdata = {
            entityId: context.id,
            entityModelId: context.entityModelId,
            EntityKeyValues: keyvalues
        };
        $.ajax({
            url: $app.baseUrl + "Entity/SaveEntityMasterValue",
            data: JSON.stringify({ dataValue: formdata, refEntityId: $entityMapping.refEntityId, refEntityModel: $entityMapping.refEntityModel, saveMappingId: $entityMapping.saveMappingId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        //$('#frm_' + context.id).modal('toggle');
                        $app.clearControlValues(context.id);
                        $entityMapping.renderForm($entityMapping.refEntityModel, $entityMapping.refEntityId, $entityMapping.entitymodelId, $entityMapping.displaytext);
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
        $entityMapping.saveMappingId = '';
    },

    renderDynamicForm: function (data, isMaster) {
        debugger;
        $entityMapping.canSave = true;
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
            readOnly: ' readonly="true"',
            valueType: "Name"
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
                readOnly: (isMaster == true) ? '' : ' readonly="true"',
                valueType: data.EntityAttributeModelList[cnt].ValueType
            });
        }
        debugger;
        var returnval = '<form role="dialog" id="' + data.Id + '"><div class="modal-dialog" data-backdrop = "static"> '
            + '<div class="modal-content"> '
            + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
            + '  &times;</button>'
            + '  <h4 class="modal-title" id="H4">'
            + $('#lbl_' + data.EntityModelId).text().toLowerCase() + ' Details </h4>'
            + ' </div>'
            + ' <div class="modal-body"> <div class="form-horizontal"><label class="labelvalTypeMaster"> Masterval</label>'
            + '<label class="labelvalTypeMonthly"> Monthlyval</label>'
            + '<label class="labelvalTypePercentage"> Percentage</label>{formelemnt}  </div></div>';

        var formelemnt = '';
        for (var cnt = 0; cnt < formAttribute.length; cnt++) {
            var req = '';
            var disabled = '';
            var bgclr = 'style=background-color:white';// percentage type color
            var monthbgClr = 'style=background-color:lightskyblue';
            var temp = '';
            if (formAttribute[cnt].required == 1) {
                req = "required";
            }
            bgclr = formAttribute[cnt].valueType == "2" ? monthbgClr : bgclr;
            disabled = formAttribute[cnt].valueType == "2" ? 'disabled' : '';
            if (formAttribute[cnt].valueType != "1" && formAttribute[cnt].valueType != "Name" && formAttribute[cnt].valueType != "2") {
                bgclr = 'style=background-color:lightgreen';// percentage type color
                temp = '<div class="form-group">'
                    + ' <label class="control-label col-md-4">' + formAttribute[cnt].displayedAs + '</label>'
                    + '<div class="col-md-7">'
                    + '<input type="' + formAttribute[cnt].type + '"' + bgclr + ' class="form-control" id="' + formAttribute[cnt].attributeModelId + '"onkeypress="return $validator.checkDecimal(event, 2)" onkeyup="return $validator.Percentagevalidate(this.id)"  value="' + formAttribute[cnt].attributeValue
                    + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + formAttribute[cnt].readOnly + '/>'
            }
            else {
                temp = '<div class="form-group">'
                    + ' <label class="control-label col-md-4">' + formAttribute[cnt].displayedAs + '</label>'
                    + '<div class="col-md-7">'
                    + '<input type="' + formAttribute[cnt].type + '"' + bgclr + ' class="form-control" id="' + formAttribute[cnt].attributeModelId + '" onkeypress="return $validator.checkDecimal(event, 2)"  value="' + formAttribute[cnt].attributeValue
                    + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + disabled + formAttribute[cnt].readOnly + '/>'
            }

            temp = temp + '</div></div>';
            formelemnt = formelemnt + temp;
        }
        if (isMaster) {
            returnval = returnval + ' <div class="modal-footer">'
                + ' <button type="submit" id="btnBranchSave" class="btn custom-button">'
                + '  Save</button>'
                + ' <button type="button" class="btn custom-button" data-dismiss="modal">'
                + '   Close</button>'
                + ' </div> </div>';
            returnval = returnval + '</form>'
        }
        else {
            returnval = returnval + '</form>'
        }
        debugger;
        returnval = returnval.replace('{formelemnt}', formelemnt);
        document.getElementById("dynamicProcessForm").innerHTML = returnval;
        $('#AddData').modal('show');
        $("#" + data.Id).on('submit', function (event) {
            if ($app.requiredValidate(data.Id, event)) {
                $entityMapping.save({ id: data.Id, entityModelId: data.EntityModelId });
                $entityMapping.saveMappingId = '';
                $('#' + data.Id).modal('hide');
                $('body').removeClass('modal-open');
                return false;
            }
            else {
                return false;
            }
        });
    },

    SaveMapping: function () {
        debugger;
        $app.showProgressModel();
        var keyvalues = [];
        $('form#frm_' + $entityMapping.entitymodelId + ' :input').each(function () {
            var input = $(this); // This is the jquery object of the input, do what you will
            if (input[0].tagName == "BUTTON") {
                // continue;
            }
            else {
                keyvalues.push({ entityModelId: input[0].id, entityId: input[0].value, refEntityId: $entityMapping.refEntityId, refEntitymodelId: $entityMapping.refEntityModel });

            }
        });

        $.ajax({
            url: $app.baseUrl + "Entity/SaveEntityMap",
            data: JSON.stringify({ dataValue: keyvalues }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        if ($entityMapping.mappingShow != "NO") {
                            $entityMapping.renderForm($entityMapping.refEntityModel, $entityMapping.refEntityId, $entityMapping.entitymodelId, $entityMapping.displaytext);
                        }
                        $entityMapping.mappingShow = '';
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
        return false;

    },
    createAdditionalInfoForm: function (data) {//new function
        debugger;
        $entityMapping.entityId = data.Id;
        var isMaster = false;

        var formAttribute = [];

        //formAttribute.push({
        //    type: "text",
        //    displayedAs: "Name",
        //    attributeName: "Name",
        //    attributeId: "Name",
        //    attributeModelId: 'Name',
        //    isMasterField: '',
        //    minLength: "5",
        //    maxLength: "100",
        //    attributeValue: (data.Name == null) ? "" : data.Name,
        //    required: 1,
        //    readOnly: ' readonly="true"'
        //});
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
                refEntityModelId: data.EntityAttributeModelList[cnt].AttributeModel.RefEntityModelId,
                FdataType: data.EntityAttributeModelList[cnt].AttributeModel.DataType,
                //   readOnly: (isMaster == true) ? '' : ' readonly="true"'
            });
        }

        var returnval = '<form role="form" id="' + data.Id + '">'
        returnval = returnval + '<div id="AddData" class="modal" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false"><div id="dynamicProcessForm"></div></div>'
        returnval = returnval + '<div class="row"><form id="frmEmpOthers"> <h4>' + $entityMapping.displaytext + '</h4>';
        returnval = returnval + ' <div class="col-md-12 text-right "><button type="submit" id="btnChange"  class="btn custom-button marginbt7">Save </button>';
        returnval = returnval + ' </div>';
        returnval = returnval + '<div class="col-md-8"> <div class="form-horizontal">'
        returnval = returnval + ' <div class="form-horizontal"> {formelemnt}  </div></div>';
        var formelemnt = '';
        for (var cnt = 0; cnt < formAttribute.length; cnt++) {
            var req = ''
            debugger
            if (formAttribute[cnt].required == 1) {
                req = "required";
            }
            var temp = '<div class="form-group">'
                + ' <label class="control-label col-md-4">' + formAttribute[cnt].displayedAs + '</label>'
                + '<div class="col-md-7">'
            if (formAttribute[cnt].refEntityModelId != '' && formAttribute[cnt].refEntityModelId != '00000000-0000-0000-0000-000000000000')//dropdown
            {

                $entityMapping.LoadEntityList(formAttribute[cnt].refEntityModelId);
                temp = temp + '<select class="form-control" ' + formAttribute[cnt].readOnly + ' id="' + formAttribute[cnt].attributeModelId + '" placeholder="' + formAttribute[cnt].displayedAs + '" >'

                for (var refCnt = 0; refCnt < $entityMapping.refEntityList.length; refCnt++) {
                    var seleted = '';
                    if ($entityMapping.refEntityList[refCnt].Id == formAttribute[cnt].attributeValue) {

                        seleted = 'selected="selected"';
                    }
                    temp = temp + '<option value="' + $entityMapping.refEntityList[refCnt].Id + '" ' + seleted + '> ' + $entityMapping.refEntityList[refCnt].Name + '</option>'

                }
                temp = temp + '</select>'
            }
             
            else if (formAttribute[cnt].FdataType == 'Date') {
                debugger
                temp = temp + '<input type="' + formAttribute[cnt].type + '" class="form-control datepicker" ' + formAttribute[cnt].readOnly + ' id="' + formAttribute[cnt].attributeModelId + '" value="' + formAttribute[cnt].attributeValue
                temp = temp + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + ' readonly />'
            }
            else if (formAttribute[cnt].FdataType == 'Bool') {

                temp = temp + '<input type="checkbox"' + formAttribute[cnt].readOnly + ' id="' + formAttribute[cnt].attributeModelId + '"';
                if (formAttribute[cnt].attributeValue == "True")
                    temp = temp + 'checked="checked"';
                temp = temp + '/>';
            } else {

                var maxlength = ' maxlength = "' + formAttribute[cnt].maxLength + '" ';
                var kyp = "";
                if (formAttribute[cnt].FdataType == "Number") {

                    kyp = ' onkeypress = "return $validator.IsNumeric(event, \'' + formAttribute[cnt].attributeModelId + '\')" ';
                }
                var isnumeric = formAttribute[cnt].type;
                temp = temp + '<input type="' + formAttribute[cnt].type + '" class="form-control " ' + kyp + ' ' + maxlength + formAttribute[cnt].readOnly + ' id="' + formAttribute[cnt].attributeModelId + '" value="' + formAttribute[cnt].attributeValue
                temp = temp + '" placeholder="' + formAttribute[cnt].displayedAs + '" ' + req + ' />';
            }
            temp = temp + '</div></div>';
            formelemnt = formelemnt + temp;
        }

        returnval = returnval.replace('{formelemnt}', formelemnt);
        $("#empOthers").html(returnval);
        $(".datepicker").datepicker();
        debugger;
        $("#" + data.Id).on('submit', function (event) {
            if ($app.requiredValidate(data.Id, event)) {
                $entityMapping.saveAdditionalInfo();
                event.preventDefault();
                // $entityMapping.save({ id: data.Id, entityModelId: data.EntityModelId });
                // return false;
            }
            else {
                return false;
            }
        });//-===========





    },
    // modified by Ajithpanner on 22/11/17
    saveAdditionalInfo: function () {
        $app.showProgressModel();
        var keyvalues = [];
        debugger;

        $('form#' + $entityMapping.entityId + ' :input').each(function () {
            var input = $(this); // This is the jquery object of the input, do what you will
            if (input[0].tagName == "BUTTON") {
                // continue;
            }
            else {

                var refentity;
                var value = input[0].value;
                if (input[0].type == "checkbox") {
                    value = input[0].checked;
                }
                if (input[0].tagName == 'SELECT') {
                    refentity = input[0].value;
                    value = $(input[0]).find('option:selected').text();
                }
                keyvalues.push({ attributeId: input[0].id, value: value, entityModelId: $entityMapping.entitymodelId, employeeId: $employee.selectedEmployeeId, refEntityId: refentity });

            }
        });
        debugger;
        $.ajax({
            url: $app.baseUrl + "Entity/SaveEntityAdditionalInfo",
            data: JSON.stringify({ dataValue: keyvalues }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $entityMapping.renderForm($entityMapping.refEntityModel, $entityMapping.refEntityId, $entityMapping.entitymodelId, $entityMapping.displaytext);
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
        return false;
    },
    LoadEntityList: function (selectedEntityModelId) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetEntityAttributeValue",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: selectedEntityModelId }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //var out = jsonResult.result[0];
                        $entityMapping.refEntityList = jsonResult.result;

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
};

