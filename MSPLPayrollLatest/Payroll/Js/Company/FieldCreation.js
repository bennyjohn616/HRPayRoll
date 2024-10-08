﻿$("#txtFieldName").keydown(function (e) {
    
    var regex = new RegExp("^[a-zA-Z]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 189 && e.keyCode !=9) {
        if (!regex.test(key)) {
            e.preventDefault();
            return false;
        }
    }
});
$("#txtDisplayAs").keydown(function (e) {

    var regex = new RegExp("^[a-zA-Z]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);k
    if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 32 && e.keyCode != 9) {
        if (!regex.test(key)) {
            e.preventDefault();
            return false;
        }
    }
});
$("#sltFieldType").on('change', function (event) {
    if ($("#sltFieldType").val() == "Direct") {
        $('#dvEntityModelDrop').addClass('nodisp');
    }
    else {
        $filedCreation.loadEntityModelDropDown({ id: 'sltRefEntityModel' });
        $('#dvEntityModelDrop').removeClass('nodisp');
    }
});
$('#txtFieldName').change(function () {
    
    this.value = this.value.split(" ").join("");
    this.value = this.value.toUpperCase();
    if ($("#txtFieldName").val() !== "") {
        var data = $filedCreation.IsExistFieldName($("#txtFieldName").val(), true);
        switch (data.isexist) {
            case true:
                $app.showAlert("The Field " + $("#txtFieldName").val() + " Already Exist", 4);
                $("#txtFieldName").val('');
                $("#txtFieldName").focus();
                break;
            case false:
                $app.showAlert("The Field " + $("#txtFieldName").val() + " Available", 2);
                //alert(jsonResult.Message);
                break;
        }
    }
    else {
    }
});

// modified by Ajithpanner on 11/22/17
$("#txtDataType").on('change', function (event) {
    
    if ($("#txtDataType").val() == "String" || $("#txtDataType").val() == "Number") {
        $('#dvDataSize').removeClass('nodisp');
    }
    else {
        $('#dvDataSize').addClass('nodisp');
    }
});


$('#rdIncludeGrossYes').on('change', function (event) {
    $($filedCreation.formData).find('#rdReimbursementYes').attr('disabled', true);
    $($filedCreation.formData).find('#rdReimbursementNo').attr('disabled', true);
});
$('#rdIncludeGrossNo').on('change', function (event) {
    $($filedCreation.formData).find('#rdReimbursementYes').attr('disabled', true);
    $($filedCreation.formData).find('#rdReimbursementNo').attr('disabled', true);
});

$('#rdMonthlyInputYes').on('change', function (event) {
    if ($($filedCreation.formData).find('#rdMonthlyInputYes').prop('checked') == true) {
        $($filedCreation.formData).find('#rdIncrementNo').prop('checked', true);
        $($filedCreation.formData).find('#rdIncrementNo').attr('disabled', true);
        $($filedCreation.formData).find('#rdIncrementYes').attr('disabled', true);
        $($filedCreation.formData).find('#rdFinalSettlementNo').attr('disabled', false);
        $($filedCreation.formData).find('#rdFinalSettlementYes').attr('disabled', false);

    }
    else {
        $($filedCreation.formData).find('#rdFinalSettlementNo').prop('checked', true);
        $($filedCreation.formData).find('#rdFinalSettlementNo').attr('disabled', true);
        $($filedCreation.formData).find('#rdFinalSettlementYes').attr('disabled', true);
        $($filedCreation.formData).find('#rdIncrementNo').attr('disabled', false);
        $($filedCreation.formData).find('#rdIncrementYes').attr('disabled', false);
    }
});
$('#rdMonthlyInputNo').on('change', function (event) {
    if ($($filedCreation.formData).find('#rdMonthlyInputYes').prop('checked') == true) {
        $($filedCreation.formData).find('#rdIncrementNo').prop('checked', true);
        $($filedCreation.formData).find('#rdIncrementNo').attr('disabled', true);
        $($filedCreation.formData).find('#rdIncrementYes').attr('disabled', true);
        $($filedCreation.formData).find('#rdFinalSettlementNo').attr('disabled', false);
        $($filedCreation.formData).find('#rdFinalSettlementYes').attr('disabled', false);

    }
    else {
       // $($filedCreation.formData).find('#rdFinalSettlementNo').prop('checked', true);
     //   $($filedCreation.formData).find('#rdFinalSettlementNo').attr('disabled', true);
      //  $($filedCreation.formData).find('#rdFinalSettlementYes').attr('disabled', true);
        $($filedCreation.formData).find('#rdIncrementNo').attr('disabled', false);
        $($filedCreation.formData).find('#rdIncrementYes').attr('disabled', false);
    }
});
$('#rdMonthlyInputDeductionYes').on('change', function (event) {
    if ($($filedCreation.formData).find('#rdMonthlyInputDeductionYes').prop('checked') == true) {
        $($filedCreation.formData).find('#rdInstallmentNo').prop('checked', true);
        $($filedCreation.formData).find('#rdInstallmentNo').attr('disabled', true);
        $($filedCreation.formData).find('#rdInstallmentYes').attr('disabled', true);
    }
    else {
        $($filedCreation.formData).find('#rdInstallmentNo').attr('disabled', false);
        $($filedCreation.formData).find('#rdInstallmentYes').attr('disabled', false);
    }
});
$('#rdMonthlyInputDeductionNo').on('change', function (event) {
    if ($($filedCreation.formData).find('#rdMonthlyInputDeductionYes').prop('checked') == true) {
        $($filedCreation.formData).find('#rdInstallmentNo').prop('checked', true);
        $($filedCreation.formData).find('#rdInstallmentNo').attr('disabled', true);
        $($filedCreation.formData).find('#rdInstallmentYes').attr('disabled', true);
    }
    else {
        $($filedCreation.formData).find('#rdInstallmentNo').attr('disabled', false);
        $($filedCreation.formData).find('#rdInstallmentYes').attr('disabled', false);
    }
});
var $filedCreation = {
    canSave: false,
    type: 'master',
    selectedId: '',
    selectedCompanyId: '',
    selectedAttributeModelTypeId: null,
    formData: document.forms["frmField"],
    loadEntityModelDropDown: function (dropControl) {
        $('#' + dropControl.id).html('');
        $.each($entitymodel.entityModelDatas, function (index, blood) {
            $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.Name));
        });
    },
    save: function (btntext) {
        debugger;
        if (!$filedCreation.canSave) {
            return false;
        }
        $filedCreation.canSave = false;
        $app.showProgressModel();
        var data = $filedCreation.buildObject();
        $.ajax({
            url: $app.baseUrl + "Entity/SaveAttributeModel",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var p = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        if (btntext == 'saveclose') {
                            $filedCreation.rdsetFalse();
                            $('#AddField').modal('toggle');
                        }
                        else {
                            $filedCreation.initializeForm();
                        }
                        $entitymodel.loadAttributeModelList();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert('Plan update failed', 4);
                        break;
                }
            },
            complete: function () {

            }
        });
        return false;
    },

    IsExistFieldName(txtFieldName, includmaster) {
  
        var isexist = new Object();
        $.ajax({
            url: $app.baseUrl + "Company/GetAttributeModelData_Field",
            data: JSON.stringify({ id: txtFieldName, incudeMaster: includmaster }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        isexist = jsonResult.result;
                        break;
                    case false:
                        isexist = jsonResult.result;
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {
            }
        });
        return isexist;

    }
    ,
    deleteData: function () {
        
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpAcademic",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var p = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        // $('#AddAcademic').modal('toggle');
                        $academic.LoadAcademics({ id: $academic.seletedEmployeeId });
                        break;
                    case false:
                        $app.showAlert('Plan update failed', 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    addNew: function (context) {
        
        $filedCreation.selectedAttributeModelTypeId = context.Id;
        $filedCreation.selectedId = null;

        if ($filedCreation.selectedAttributeModelTypeId == null || $filedCreation.selectedAttributeModelTypeId == '')
        {
            $app.showAlert('Please select Field Type', 4);
            return false;
        }
        else
        {
            $('#AddField').modal('toggle');
        }
        if (context.behaviorType == 'Earning')//$filedCreation.selectedAttributeModelTypeId.toUpperCase() == '550A5131-D822-4293-B54B-19FDEC09B1E6') {
        {
            $filedCreation.type = 'Earning';
        }
        else if (context.behaviorType == 'Deduction')//$filedCreation.selectedAttributeModelTypeId.toUpperCase() == 'D6E14542-8215-4A24-85FE-302F186D6AB9') {
        {
            $filedCreation.type = 'Deduction';
        }
        else if (context.behaviorType == 'Tax')//$filedCreation.selectedAttributeModelTypeId.toUpperCase() == 'D6E14542-8215-4A24-85FE-302F186D6AB9') {
        {
            $filedCreation.type = 'Tax';
        }
        else
            $filedCreation.type = 'Master';
        $filedCreation.initializeForm();
    },
    edit: function (context) {
 
        if (context.behaviorType == 'Earning' || context.behaviorType == 'Deduction') {
            $('#txtDataType > option').each(function () {
                if ($(this).val != "Number") {

                    $(this).addClass("hidden");
                }

            });
        }
        else {
            $('#txtDataType').attr("disabled", false);
            $('#txtDataType > option').each(function () {

                    $(this).removeClass("hidden");

            });
        }
        $filedCreation.selectedId = context.Id;
        $filedCreation.selectedAttributeModelTypeId = context.AttributeModelTypeId;
        if ($filedCreation.selectedId == null || $filedCreation.selectedId == '') {
            $app.showAlert('Please select the Field to edit', 4);
            return false;
        }
        if (context.behaviorType == 'Earning') {
            $filedCreation.type = 'Earning';
        }
        else if (context.behaviorType == 'Deduction') {
            $filedCreation.type = 'Deduction';
        }
        else if (context.behaviorType == 'Tax') {
            $filedCreation.type = 'Tax';
        }
        else {
            $filedCreation.type = 'Master';
            $filedCreation.loadEntityModelDropDown({ id: 'sltRefEntityModel' });
        }
        $filedCreation.LoadField({ Id: $filedCreation.selectedId });

    },
    rdsetFalse: function () {
        $($filedCreation.formData).find('#rdTaxableYes').prop('checked', false);
        $($filedCreation.formData).find('#rdTaxableNo').prop('checked', true);
        $($filedCreation.formData).find('#rdIncrementYes').prop('checked', false);
        $($filedCreation.formData).find('#rdIncrementNo').prop('checked', true);
        $($filedCreation.formData).find('#rdReimbursementYes').prop('checked', false);
        $($filedCreation.formData).find('#rdReimbursementNo').prop('checked', true);
        $($filedCreation.formData).find('#rdFinalSettlementYes').prop('checked', false);
        $($filedCreation.formData).find('#rdFinalSettlementNo').prop('checked', true);
        $($filedCreation.formData).find('#rdInstallmentYes').prop('checked', false);
        $($filedCreation.formData).find('#rdInstallmentNo').prop('checked', true);
        $($filedCreation.formData).find('#rdInstallmentNo').attr('disabled', false);
        $($filedCreation.formData).find('#rdInstallmentYes').attr('disabled', false);
    },
    initializeForm: function () {
        
        $filedCreation.canSave = true;
        $('#frmField #H4').text(' Add/Edit ' + $filedCreation.type + ' Field');
        $filedCreation.selectedId = null;
        $($filedCreation.formData).find('#txtFieldName').val('');
        $($filedCreation.formData).find('#txtDisplayAs').val('');
        $($filedCreation.formData).find('#txtDataType').val('');
        $($filedCreation.formData).find('#txtDataSize').val('');
        $($filedCreation.formData).find('#txtOrderNo').val(0);
       
        
        if ($filedCreation.type != 'Master') {
            $($filedCreation.formData).find('#txtDataType').val('Number');
            $('#dvDataSize').addClass('nodisp');
            $($filedCreation.formData).find('#txtDataType').attr("disabled", true);
        } else {
            $('#dvDataSize').removeClass('nodisp');
            $($filedCreation.formData).find('#txtDataType').attr("disabled", false);
        }



        $($filedCreation.formData).find('#txtDefaultValue').val('');
        $($filedCreation.formData).find('#rdMandatoryYes').prop('checked', false);
        $($filedCreation.formData).find('#rdMandatoryNo').prop('checked', true);
        $($filedCreation.formData).find('#sltFieldType').val('Direct');
        // $($filedCreation.formData).find('#txtOrder').val() = '';
        $($filedCreation.formData).find('#rdTransactionYes').prop('checked', false);
        $($filedCreation.formData).find('#rdTransactionNo').prop('checked', true);
        // $($filedCreation.formData).find('#rdFilterYes').prop('checked',true);
        //$($filedCreation.formData).find('#rdFilterNo').val() = false;
        //hide the behavior of the field

        $('#frmField #dvMasterFieldBehavior').css('display', 'none');
        $('#frmField #dvEarningFieldBehavior').css('display', 'none');
        $('#frmField #dvDeductionFieldBehavior').css('display', 'none');
        $('#frmField #dvTaxBehaviour').css('display', 'none');

        if ($filedCreation.type == "Earning") {
            $('#frmField #dvEarningFieldBehavior').css('display', 'block');
            //$($filedCreation.formData).find('#rdIncludeGrossYes').prop('checked', true);
            //$($filedCreation.formData).find('#rdIncludeGrossNo').prop('checked', false);
            //$($filedCreation.formData).find('#rdMonthlyInputYes').prop('checked', false);
            //$($filedCreation.formData).find('#rdMonthlyInputNo').prop('checked', true);
        }
        else if ($filedCreation.type == "Deduction") {
            $('#frmField #dvDeductionFieldBehavior').css('display', 'block');
            //$($filedCreation.formData).find('#rdIncludeGrossDeductionYes').prop('checked', true);
            //$($filedCreation.formData).find('#rdIncludeGrossDeductionNo').prop('checked', false);
            //$($filedCreation.formData).find('#rdMonthlyInputDeductionYes').prop('checked', false);
            //$($filedCreation.formData).find('#rdMonthlyInputDeductionNo').prop('checked', true);           

        }
        else if ($filedCreation.type == "Tax") {
            $('#frmField #dvTaxBehaviour').css('display', 'block');
        }
        else {
            $('#frmField #dvMasterFieldBehavior').css('display', 'block');
        }
        //$($filedCreation.formData).find('#rdTaxableYes').prop('checked', false);
        //$($filedCreation.formData).find('#rdTaxableNo').prop('checked', true);
        //$($filedCreation.formData).find('#rdIncrementYes').prop('checked', false);
        //$($filedCreation.formData).find('#rdIncrementNo').prop('checked', true);
        //$($filedCreation.formData).find('#rdReimbursementYes').prop('checked', false);
        //$($filedCreation.formData).find('#rdReimbursementNo').prop('checked', true);
        //$($filedCreation.formData).find('#rdFinalSettlementYes').prop('checked', false);
        //$($filedCreation.formData).find('#rdFinalSettlementNo').prop('checked', true);
        //$($filedCreation.formData).find('#rdInstallmentYes').prop('checked', false);
        //$($filedCreation.formData).find('#rdInstallmentNo').prop('checked', true);
        //$($filedCreation.formData).find('#rdInstallmentNo').attr('disabled', false);
        //$($filedCreation.formData).find('#rdInstallmentYes').attr('disabled', false);
        $('#txtFieldName').attr('readonly', false);
    },
    renderFrom: function (data) {
        debugger;
        if (data.Name == "SUPPDAYS" || data.Name == "LOPCREDITDAYS")
        {
            $filedCreation.canSave = true;
            $('#AddField').modal('toggle');
            $filedCreation.selectedId = data.Id;
            $filedCreation.selectedCompanyId = data.CompanyId;
            $($filedCreation.formData).find('#txtFieldName').val(data.Name);
            $($filedCreation.formData).find('#txtDisplayAs').val(data.DisplayAs);
            $($filedCreation.formData).find('#txtDataType').val(data.DataType);
            $($filedCreation.formData).find('#txtDataSize').val(data.DataSize);
            $($filedCreation.formData).find('#txtDefaultValue').val(data.DefaultValue);
            $($filedCreation.formData).find('#txtOrderNo').val(data.OrderNumber);
            $('#txtFieldName').attr('readonly', true);
            $('#dvMasterFieldBehavior').css('display', 'none');
                $('#frmField #dvEarningFieldBehavior').css('display', 'none');
                $('#dvDeductionFieldBehavior').css('display', 'none');
                $('#dvTaxBehaviour').css('display', 'none');
                return false;
        }
            
           
           
        $filedCreation.canSave = true;
        $('#AddField').modal('toggle');
        $filedCreation.selectedId = data.Id;
        $filedCreation.selectedCompanyId = data.CompanyId;
        // $filedCreation.type = data.AttributeModelType;
        $($filedCreation.formData).find('#txtFieldName').val(data.Name);
        $($filedCreation.formData).find('#txtDisplayAs').val(data.DisplayAs);
        $($filedCreation.formData).find('#txtDataType').val(data.DataType);
        $($filedCreation.formData).find('#txtDataSize').val(data.DataSize);
        $($filedCreation.formData).find('#txtDefaultValue').val(data.DefaultValue);
        $($filedCreation.formData).find('#txtOrderNo').val(data.OrderNumber);
        (data.IsMandatory == true) ? $($filedCreation.formData).find('#rdMandatoryYes').prop('checked', true) : $($filedCreation.formData).find('#rdMandatoryNo').prop('checked', true);
        (data.IsMonthlyInput == true) ? $($filedCreation.formData).find('#rdInstallmentYes,#rdInstallmentNo').attr('disabled', true) : $($filedCreation.formData).find('#rdInstallmentYes,#rdInstallmentNo').attr('disabled', false);

        //
        if (data.RefEntityModelId != '' && data.RefEntityModelId != '00000000-0000-0000-0000-000000000000') {
            $($filedCreation.formData).find('#sltRefEntityModel').val(data.RefEntityModelId);
            $('#dvEntityModelDrop').removeClass('nodisp');
            $($filedCreation.formData).find('#sltFieldType').val('PopUp');
        }
        else {
            $('#dvEntityModelDrop').addClass('nodisp');
        }
        // $($filedCreation.formData).find('#txtOrder').val( data.OrderNumber;
        (data.IsTransaction == true) ? $($filedCreation.formData).find('#rdTransactionYes').prop('checked', true) : $($filedCreation.formData).find('#rdTransactionNo').prop('checked', true);
        // (data.IsFilter == true) ? $($filedCreation.formData).find('#rdFilterYes').val( true : $($filedCreation.formData).find('#rdFilterNo').val( false;
        //hide the behavior of the field

        $('#frmField #dvMasterFieldBehavior').css('display', 'none');
        $('#frmField #dvEarningFieldBehavior').css('display', 'none');
        $('#frmField #dvDeductionFieldBehavior').css('display', 'none');
         $('#frmField #dvTaxBehaviour').css('display', 'none');

        $('#frmField #H4').text(' Add/Edit ' + $filedCreation.type + ' Field');
        if ($filedCreation.type == "Earning") {
            $('#frmField #dvEarningFieldBehavior').css('display', 'block');
            (data.IsIncludeForGrossPay == true) ? $($filedCreation.formData).find('#rdIncludeGrossYes').prop('checked', true) : $($filedCreation.formData).find('#rdIncludeGrossNo').prop('checked', true);
            (data.IsMonthlyInput == true) ? $($filedCreation.formData).find('#rdMonthlyInputYes').prop('checked', true) : $($filedCreation.formData).find('#rdMonthlyInputNo').prop('checked', true);
        }
        else if ($filedCreation.type == "Deduction") {
            $('#frmField #dvDeductionFieldBehavior').css('display', 'block');
            (data.IsIncludeForGrossPay == true) ? $($filedCreation.formData).find('#rdIncludeGrossDeductionYes').prop('checked', true) : $($filedCreation.formData).find('#rdIncludeGrossDeductionNo').prop('checked', true);
            (data.IsMonthlyInput == true) ? $($filedCreation.formData).find('#rdMonthlyInputDeductionYes').prop('checked', true) : $($filedCreation.formData).find('#rdMonthlyInputDeductionNo').prop('checked', true);

        } else if ($filedCreation.type == "Tax") {
            $('#frmField #dvTaxBehaviour').css('display', 'block');
        }
        else {
            $('#frmField #dvMasterFieldBehavior').css('display', 'block');
        }


        //$($filedCreation.formData).find('#txtDegreeName"].value=data.VisualLength;
        //$($filedCreation.formData).find('#txtDegreeName"].value=data.VisualBoxLines;
        //   IsIncludeForGrossPay: grosspay,
        // IsMonthlyInput: monthlyinput,
        (data.IsTaxable == true) ? $($filedCreation.formData).find('#rdTaxableYes').prop('checked', true) : $($filedCreation.formData).find('#rdTaxableNo').prop('checked', true);
        (data.IsIncrement == true) ? $($filedCreation.formData).find('#rdIncrementYes').prop('checked', true) : $($filedCreation.formData).find('#rdIncrementNo').prop('checked', true);
        (data.IsReimbursement == true) ? $($filedCreation.formData).find('#rdReimbursementYes').prop('checked', true) : $($filedCreation.formData).find('#rdReimbursementNo').prop('checked', true);
        (data.FullAndFinalSettlement == true) ? $($filedCreation.formData).find('#rdFinalSettlementYes').prop('checked', true) : $($filedCreation.formData).find('#rdFinalSettlementNo').prop('checked', true);
        (data.IsInstallment == true) ? $($filedCreation.formData).find('#rdInstallmentYes').prop('checked', true) : $($filedCreation.formData).find('#rdInstallmentNo').prop('checked', true);
        (data.IsFlexiPay == true) ? $($filedCreation.formData).find('#rdFlexiPayYes').prop('checked', true) : $($filedCreation.formData).find('#rdFlexiPayNo').prop('checked', true);
        //if (data.Name == "SUPPDAYS") {
        //    $('#txtFieldName').attr('readonly', true);
        //    $('#dvMasterFieldBehavior').addClass('nodisp');
        //    $('#dvEarningFieldBehavior').addClass('nodisp');
        //    $('#dvDeductionFieldBehavior').addClass('nodisp');
        //    $('#dvTaxBehaviour').addClass('nodisp');
        //    return false;

        //}
        if ($('#txtFieldName').val() != "") {
            $('#txtFieldName').attr('readonly', true);
        }
     
    },
    buildObject: function () {
        var grosspay = false;
        var monthlyinput = false;
        if ($filedCreation.type == "Earning") {
            grosspay = $($filedCreation.formData).find('#rdIncludeGrossYes').prop('checked') == true ? true : false;
            monthlyinput = $($filedCreation.formData).find('#rdMonthlyInputYes').prop('checked') == true ? true : false;
        }
        else if ($filedCreation.type == "Deduction") {
            grosspay = $($filedCreation.formData).find('#rdIncludeGrossDeductionYes').prop('checked') == true ? true : false;
            monthlyinput = $($filedCreation.formData).find('#rdMonthlyInputDeductionYes').prop('checked') == true ? true : false;
        }
        var refEntityMod = '00000000-0000-0000-0000-000000000000';
        if ($($filedCreation.formData).find('#sltFieldType').val() == 'PopUp') {
            refEntityMod = $($filedCreation.formData).find('#sltRefEntityModel').val();
        }

        var retrunobject = {
            Id: $filedCreation.selectedId,
            CompanyId: $filedCreation.selectedCompanyId,
            AttributeModelTypeId: $filedCreation.selectedAttributeModelTypeId,
            Name: $($filedCreation.formData).find('#txtFieldName').val(),
            DisplayAs: $($filedCreation.formData).find('#txtDisplayAs').val(),
            DataType: $($filedCreation.formData).find('#txtDataType').val(),
            DataSize: $($filedCreation.formData).find('#txtDataSize').val(),
            DefaultValue: $($filedCreation.formData).find('#txtDefaultValue').val(),
            IsMandatory: $($filedCreation.formData).find('#rdMandatoryYes').prop('checked') ? true : false,
            FieldType: $($filedCreation.formData).find('#sltFieldType').val(),
            OrderNumber: $($filedCreation.formData).find('#txtOrderNo').val(),
            IsTransaction: $($filedCreation.formData).find('#rdTransactionYes').prop('checked') ? true : false,
            RefEntityModelId: refEntityMod,
            IsFilter: true,// $($filedCreation.formData).find('#rdFilterYes').value == true  ? true : false,
            VisualLength: 1,
            VisualBoxLines: 1,
            IsIncludeForGrossPay: grosspay,
            IsMonthlyInput: monthlyinput,
            IsTaxable: $($filedCreation.formData).find('#rdTaxableYes').prop('checked') ? true : false,
            IsIncrement: $($filedCreation.formData).find('#rdIncrementYes').prop('checked') ? true : false,
            IsFlexiPay: $($filedCreation.formData).find('#rdFlexiPayYes').prop('checked') ? true : false,
            IsReimbursement: $($filedCreation.formData).find('#rdReimbursementYes').prop('checked') ? true : false,
            FullAndFinalSettlement: $($filedCreation.formData).find('#rdFinalSettlementYes').prop('checked') ? true : false,
            IsInstallment: $($filedCreation.formData).find('#rdInstallmentYes').prop('checked') ? true : false,
            BehaviorType: $filedCreation.type

        };
        if ($filedCreation.type == "Tax") {
            IsMandatory: $($filedCreation.formData).find('#rdtxMandatoryYes').prop('checked') ? true : false;
        }
        return retrunobject;

    },
    LoadField: function (data) {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Company/GetAttributeModelData",
            data: JSON.stringify({ id: data.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $entitymodel.loadAttributeModelList();
                        $filedCreation.renderFrom(p);
                       
                        //$entitymodel.renderAttributeModelTree(p);
                       
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    }
};