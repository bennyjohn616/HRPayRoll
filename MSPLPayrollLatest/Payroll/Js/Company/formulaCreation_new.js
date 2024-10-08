﻿//$('#btnFormulaSave').on('click', function () {
//    var err = 0;
//    
//    if ($("#sltEntityType").val() == 3) {
//        var txt = $("#txtFormula").val(); // load the content
//        var splchar = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
//        var result;

//        var l = txt.length; // length of the original string
//        var lastChar = txt.substring(l - 1, l); // get the last char of the original string
//        if (splchar.test(lastChar)) { // if the last char is dot, remove the last char
//            //result = txt.substring(0, l - 1);

//            //$('#txtFormula').val(result);
//            $app.showAlert('Enter the correct formula', 3);
//            err = 1;
//            $('#txtFormula').focus();
//        }
//        else { // otherwise do nothing
//            result = txt;
//            validateFormulea('txtFormula');
//        }

//    }

//    if (err == 0) {
//        $formulaCreation.save();
//    }
//});




$("input[name=searchF]").keyup(function (e) {

    var TreeF = $("#attributeModeltree").fancytree("getTree");
    var match = $(this).val();
    if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
        $("input[name=searchF]").val("");
        TreeF.clearFilter();
        return;
    }
    var n = TreeF.applyFilter(match);
    $("button#btnResetSearch").attr("disabled", false);
}).focus();

$("button#btnResetSearch").click(function (e) {

    var TreeF = $("#attributeModeltree").fancytree("getTree");
    $("input[name=searchF]").val("");
    TreeF.clearFilter();
    return;
}).attr("disabled", true);

$("input#hideMode").change(function (e) {

    var TreeF = $("#attributeModeltree").fancytree("getTree");
    TreeF.options.filter.mode = $(this).is(":checked") ? "hide" : "dimm";
    TreeF.clearFilter();
    $("input[name=searchF]").keyup();
    return;
    // tree.render();
});
$('.operator').on("click", function (e) {
    debugger;
    if ($("#txtFieldName").val() != "PTAX") {
        $formulaCreation.buildFormula(e, null);
    }
});
$('.remove').on("click", function (e) {
    if ($("#txtFieldName").val() != "PTAX") {
        $formulaCreation.removeLastChar();
    }
});
$('.enternumber').on("click", function (e) {

    var EnterNum = prompt("Enter Number", "0");
    if (EnterNum != null) {
        debugger;
        if ($("#txtFieldName").val() != "PTAX") {
            $formulaCreation.buildFormula(null, null, EnterNum);
        }
    }

});
function sortmyway(data_A, data_B) {
    return (data_A - data_B);
}

$("#sltCategory").change(function () {

    $formulaCreation.selectedCategoryId = $("#sltCategory").val();
    $formulaCreation.renderGrid();
});
$("#sltType").change(function () {

    $formulaCreation.type = $("#sltType").val();
    $formulaCreation.renderGrid();
});
///testing
$(".hasclear").keyup(function () {
    var t = $(this);
    t.next('span').toggle(Boolean(t.val()));
});

$(".clearer").hide($(this).prev('input').val());

$(".clearer").click(function () {
    $(this).prev('input').val('').focus();
    $(this).hide();
});

$("#sltEntityType").ready(function () {
    $("#txtArrearMapField").removeAttr("disabled");
});

$("#renderId").on(function () {
    if ($("#sltEntityType").val() == 3 || $("#sltEntityType").val() == 7) {
        $("#txtArrearMapField").attr("disabled", "disabled");
    }
});



$("#renderId").ready(function () {
    debugger;
    if ($("#sltEntityType").val() == 1) {
        $("#lblfrmla").text("Value").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#dvArrearMap").attr("placeholder", "Enter the Value");
        $("#txtArrearMapField").removeAttr("disabled");

    }
    else if ($("#sltEntityType").val() == 2) {
        $("#lblfrmla").text("Value").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Value");
        $("#txtArrearMapField").attr("disabled", "disabled");
    }
    else if ($("#sltEntityType").val() == 3) {
        $("#lblfrmla").text("Formula").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Formula");
        $("#txtArrearMapField").attr("disabled", "disabled");
    }
    else {
        $("#lblfrmla").text("Formula").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Formula");
        $("#txtArrearMapField").attr("disabled", "disabled");
    }


});



$("#sltEntityType").change(function () {
    debugger;
    //var ph;
    //var man;
    //man = '*';

    if ($("#sltEntityType").val() == 1) {
        $("#lblfrmla").text("Value").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#dvArrearMap").attr("placeholder", "Enter the Value");
        $("#txtArrearMapField").removeAttr("disabled");

    }
    else if ($("#sltEntityType").val() == 2) {
        $("#lblfrmla").text("Value").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Value");
        $("#txtArrearMapField").attr("disabled", "disabled");
    }
    else if ($("#sltEntityType").val() == 3) {
        $("#lblfrmla").text("Formula").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Formula");
        $("#txtArrearMapField").attr("disabled", "disabled");
    }
    else {
        $("#lblfrmla").text("Formula").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Formula");
        $("#txtArrearMapField").attr("disabled", "disabled");
    }


});



$("#sltEntityType").change(function () {
    debugger;
    //var ph;
    //var man;
    //man = '*';
    $($formulaCreation.formData).find('#dvRangeFormula').addClass('hide');
    if ($("#sltEntityType").val() == 1) {
        $("#lblfrmla").text("Value").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#dvArrearMap").attr("placeholder", "Enter the Value");
        $("#txtArrearMapField").removeAttr("disabled");

    }
    else if ($("#sltEntityType").val() == 2) {

        if ($formulaCreation.attrModelIsincre === true) {
            $app.showAlert("This component has increment as yes ", 3);
            $("#sltEntityType").val('1');
            $("#txtArrearMapField").removeAttr("disabled");
        }
        $("#lblfrmla").text("Value").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Value");
    }
    else if ($("#sltEntityType").val() == 3 || $("#sltEntityType").val() == 7) {
        $("#lblfrmla").text("Formula").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Formula");
        $("#txtArrearMapField").attr("disabled", "disabled");
    }
    else {
        $("#lblfrmla").text("Formula").append('<label id="lblman" style="color:red;font-size: 13px"> * </label>');
        $("#txtFormula").attr("placeholder", "Enter the Formula");
        $("#txtArrearMapField").attr("disabled", "disabled");
    }
    $formulaCreation.type = $("#sltEntityType").val();
    $($formulaCreation.formData).find('#txtFormula').val(''); //clear Textbox of formula
    $formulaCreation.hidenformula = ''; //clear Value of formula
    $($formulaCreation.formData).find('#btnViewIf').addClass('hide');
    $($formulaCreation.formData).find('#btnViewRange').addClass('hide');
    $("#frmFormula .modal-footer").removeClass('hide');


    if ($formulaCreation.type == 1) {
        $($formulaCreation.formData).find('#txtFormula').prop('readonly', false);

        if ($($formulaCreation.formData).find('#txtFormula').val() == '') {
            $($formulaCreation.formData).find('#txtFormula').val('0');
        }
    }

    if ($formulaCreation.type == 2) {
        //  $($formulaCreation.formData).find('#txtFormula').prop('readonly', true);
    }
    if ($formulaCreation.type == 3 || $formulaCreation.type == 4 || $formulaCreation.type == 5 || $formulaCreation.type == 7) {
        $($formulaCreation.formData).find('#txtFormula').prop('readonly', false);
        if ($($formulaCreation.formData).find('#txtPercentage').val() == '') {
            $($formulaCreation.formData).find('#txtPercentage').val('100');
            $("#txtArrearMapField").attr("disabled", "disabled");
        }
    }
    if ($formulaCreation.type == 4) {
        $($formulaCreation.formData).find('#dvConditionalFormula').removeClass('hide');
        $($formulaCreation.formData).find('#dvConditionalFormula').show();
        $($formulaCreation.formData).find('#dvNormalFormula').hide();
        $($formulaCreation.formData).find('#btnViewIf').removeClass('hide');

        $("#frmFormula .modal-footer").addClass('hide');
    }
    if ($formulaCreation.type == 5) {
        $('#txtBaseValue').focus();
        $formulaCreation.formulaFocusedElemt = 'txtFormula';
        $($formulaCreation.formData).find('#dvRangeFormula').removeClass('hide');
        $($formulaCreation.formData).find('#dvRangeFormula').show();
        $($formulaCreation.formData).find('#dvNormalFormula').hide();
        $($formulaCreation.formData).find('#btnViewRange').removeClass('hide');
        $("#frmFormula .modal-footer").addClass('hide');
    }
    if ($('#txtFieldName').val() == "VPF") {
        $app.showAlert("This component has set only Master ", 3);
        $("#sltEntityType").val('1'); $("#txtFormula").val('0');
        return false;
    }
});



$("#frmFormula").on('submit', function (event) {

    $formulaCreation.save();
    return false;
});

$("#btnIfAdd").on('click', function (event) {
    
    $formulaCreation.BuildIfElseSegment(false);
    return false;
});

$("#btnRangeAdd").on('click', function (event) {

    $formulaCreation.BuildRangeSegment();
    return false;
});
$("#btnIfBuildFormula").on('click', function (event) {

    $formulaCreation.buildIfElseFormula();
    $('#txtIfFormula').val($formulaCreation.ifElseTextFormula.replace('', ''));
    return false;
});
$("#btnIfClose").on('click', function (event) {

    $("#frmFormula .modal-footer").removeClass('hide');
    $($formulaCreation.formData).find('#dvConditionalFormula').hide();
    $($formulaCreation.formData).find('#dvNormalFormula').show();
    $('#txtIfFormula').val($formulaCreation.ifElseTextFormula.replace('', ''));
    return false;
});
$("#btnIfSaveClose").on('click', function (event) {

    $("#frmFormula .modal-footer").removeClass('hide');
    $formulaCreation.buildIfElseFormula();
    $($formulaCreation.formData).find('#dvConditionalFormula').hide();
    $($formulaCreation.formData).find('#dvNormalFormula').show();
    $('#txtIfFormula').val($formulaCreation.ifElseTextFormula.replace('', ''));
    $('#txtFormula').val($formulaCreation.ifElseTextFormula.replace('', ''));
    return false;
});
$("#btnRangeSaveClose").on('click', function (event) {

    $("#frmFormula .modal-footer").removeClass('hide');
    $formulaCreation.buildRange();
    $($formulaCreation.formData).find('#dvRangeFormula').hide();
    $($formulaCreation.formData).find('#dvNormalFormula').show();
    $('#txtIfFormula').val($formulaCreation.rangeTextFormula);
    $('#txtFormula').val($formulaCreation.rangeTextFormula);
    return false;
});
$("#btnRangeClose").on('click', function (event) {

    $("#frmFormula .modal-footer").removeClass('hide');
    $formulaCreation.buildRange();
    $($formulaCreation.formData).find('#dvRangeFormula').hide();
    $($formulaCreation.formData).find('#dvNormalFormula').show();
    $('#txtIfFormula').val($formulaCreation.rangeTextFormula.replace('', ''));
    $('#txtFormula').val($formulaCreation.rangeTextFormula.replace('', ''));
    return false;
});

$('#btnViewIf').on('click', function () {
    $("#frmFormula .modal-footer").addClass('hide');
    $($formulaCreation.formData).find('#dvConditionalFormula').removeClass('hide');
    $($formulaCreation.formData).find('#dvConditionalFormula').show();
    $($formulaCreation.formData).find('#dvNormalFormula').hide();
    $formulaCreation.editIfElse();
    return false;
});

$('#btnViewRange').on('click', function () {
    $('#txtBaseValue').focus();
    $formulaCreation.formulaFocusedElemt = 'txtFormula';
    $($formulaCreation.formData).find('#dvRangeFormula').removeClass('hide');
    $($formulaCreation.formData).find('#dvRangeFormula').show();
    $($formulaCreation.formData).find('#dvNormalFormula').hide();
    $($formulaCreation.formData).find('#btnViewRange').removeClass('hide');
    $("#frmFormula .modal-footer").addClass('hide');
    $formulaCreation.editRange();

    return false;
});

$('#txtEligibilityFormula').focus(function () {
    $formulaCreation.formulaFocusedElemt = 'txtEligibilityFormula';
});
$('#txtFormula').focus(function () {

    $formulaCreation.formulaFocusedElemt = 'txtFormula';
    if ($("#sltEntityType").val() == 1) {
        $("#txtFormula").unbind("keypress"); // release the keypress event
        $("#txtFormula").keypress(function (evt) { // allow only numeric values
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
                //alert("Wow; Its Work!.")
            }
            return true;
        });
    }
    else if ($("#sltEntityType").val() == 3 || $("#sltEntityType").val() == 7) {
        $("#txtFormula").unbind("keypress");
        $('#txtFormula').keyup(function () { // convert lower to upper case while typing the characters
            this.value = this.value.toUpperCase();
        });

    }
});
// Created By AjithPanner on 3/11/2017
$('#txtArrearMapField').focus(function () {

    if ($("#sltEntityType").val() == 3 || $("#sltEntityType").val() == 7) {
        $("#txtArrearMapField").removeAttr("disabled");
    }
    $('.operator').attr('disabled', 'disabled');
    $formulaCreation.formulaFocusedElemt = 'txtArrearMapField';
    $("#txtArrearMapField").unbind("keypress");
    $('#txtArrearMapField').keyup(function () { // convert lower to upper case while typing the characters
        this.value = this.value.toUpperCase();
    });
});
//$('#txtFormula').on('change', function () {
//    
//    if ($("#sltEntityType").val() == 3) {
//        var txt = $("#txtFormula").val(); // load the content
//        var splchar = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
//        var result;

//        var l = txt.length; // length of the original string
//        var lastChar = txt.substring(l - 1, l); // get the last char of the original string
//        if (lastChar == splchar) { // if the last char is dot, remove the last char
//            result = txt.substring(0, l - 1);
//        }
//        else { // otherwise do nothing
//            result = txt;
//        }
//        alert(result);
//    }
//});
//$('#txtFormula').mouseover(function () {//need to validate
//    
//   validateFormulea('txtFormula');
//});
$('#txtFormula').change(function () {//need to validate

    validateFormulea('txtFormula');
});
// Created by AjithPanner on 3/11/2017
$('#txtArrearMapField').change(function () {

    validateFormulea('txtArrearMapField');
});
$('#txtBaseValue').change(function () {

    validateFormulea('txtBaseValue');
});
function validateFormulea(field) {
    debugger;
  //  var fields = document.getElementById(field).value.replace(/\+/g, ',').replace(/\-/g, ',').replace(/\*/g, ',').replace(/\//g, ',').replace(/\)/g, ',').replace(/\(/g, ',').replace(new RegExp('If', 'gi'), ',').replace(new RegExp('THEN', 'gi'), ',').replace(new RegExp('Else', 'gi'), ',').replace(new RegExp('Max', 'gi'), ',').replace(new RegExp('Min', 'gi'), ',').replace(/\[/g, ',').replace(/\]/g, ',').replace(new RegExp('>', 'gi'), ',').replace(new RegExp('<', 'gi'), ',').replace(new RegExp('=', 'gi'), ',').replace(new RegExp(' OR ', 'gi'), ',').replace(new RegExp(' AND ', 'gi'), ',').replace(new RegExp('!', 'gi'), ',').replace(new RegExp(':', 'gi'), ',').split(',');
    var fields = document.getElementById(field).value.replace(/\+/g, ',').replace(/\-/g, ',').replace(/\*/g, ',').replace(/\//g, ',').replace(/\)/g, ',').replace(/\(/g, ',').replace('Max[', ',').replace('Min[', ',').replace(/\[/g, ',').replace(/\]/g, ',').replace(new RegExp('>', 'gi'), ',').replace(new RegExp('<', 'gi'), ',').replace(new RegExp('=', 'gi'), ',').replace(new RegExp('!', 'gi'), ',').replace(new RegExp(':', 'gi'), ',').split(',');
    if($("#sltEntityType").val()==4){
        var fields = document.getElementById(field).value.replace(/\+/g, ',').replace(/\-/g, ',').replace(/\*/g, ',').replace(/\//g, ',').replace(/\)/g, ',').replace(/\(/g, ',').replace(new RegExp('ElseIf ', 'gi'), ',').replace(new RegExp('If ', 'gi'), ',').replace(new RegExp('THEN ', 'gi'), ',').replace(new RegExp('Else ', 'gi'), ',').replace(/\[/g, ',').replace(/\]/g, ',').replace(new RegExp('>', 'gi'), ',').replace(new RegExp('<', 'gi'), ',').replace(new RegExp('=', 'gi'), ',').replace(new RegExp(' OR ', 'gi'), ',').replace(new RegExp(' AND ', 'gi'), ',').replace(new RegExp('!', 'gi'), ',').replace(new RegExp(':', 'gi'), ',').split(',');
    }
    if ($("#sltEntityType").val() == 5) {
        var fields = document.getElementById(field).value.replace(/\+/g, ',').replace(/\-/g, ',').replace(/\*/g, ',').replace(/\//g, ',').replace(/\)/g, ',').replace(/\(/g, ',').replace(new RegExp('ElseIf ', 'gi'), ',').replace(new RegExp('If ', 'gi'), ',').replace(new RegExp('THEN', 'gi'), ',').replace(new RegExp('Else ', 'gi'), ',').replace(/\[/g, ',').replace(/\]/g, ',').replace(new RegExp('>', 'gi'), ',').replace(new RegExp('<', 'gi'), ',').replace(new RegExp('=', 'gi'), ',').replace(new RegExp(' OR ', 'gi'), ',').replace(new RegExp(' AND ', 'gi'), ',').replace(new RegExp('!', 'gi'), ',').replace(new RegExp(':', 'gi'), ',').split(',');
    }
    if ($("#ddInputType").val() == 4) {
        var fieldInput = document.getElementById(field).value;
        var regex = /[\+\-\*\/()\[\]><=,]|(ElseIf|If|THEN|Else|OR|AND|!|:)/gi;
        var fields = fieldInput.replace(regex, ',').split(',');
    }
    if ($("#ddInputType").val() == 5) {
        var fields = document.getElementById(field).value.replace(/\+/g, ',').replace(/\-/g, ',').replace(/\*/g, ',').replace(/\//g, ',').replace(/\)/g, ',').replace(/\(/g, ',').replace(new RegExp('ElseIf ', 'gi'), ',').replace(new RegExp('If ', 'gi'), ',').replace(new RegExp('THEN', 'gi'), ',').replace(new RegExp('Else ', 'gi'), ',').replace(/\[/g, ',').replace(/\]/g, ',').replace(new RegExp('>', 'gi'), ',').replace(new RegExp('<', 'gi'), ',').replace(new RegExp('=', 'gi'), ',').replace(new RegExp(' OR ', 'gi'), ',').replace(new RegExp(' AND ', 'gi'), ',').replace(new RegExp('!', 'gi'), ',').replace(new RegExp(':', 'gi'), ',').split(',');
    }
    var goloop = true;
    var textVal = document.getElementById(field).value;
    var chktextval = textVal;
    if (fields.length != 0) {
        chktextval = fields[fields.length - 1];
    }
    $formulaCreation.hidenformula = textVal;

    var check = true;//$formulaCreation.validateFormula(field, chktextval);
    if (check) {

        $.each(fields, function (index, value) {
             
            value = value.trim();
            var data = $filedCreation.IsExistFieldName(value, false);
            if (!data.isexist && !$.isNumeric(value)) {
                if (value !== '') {
                    $app.showAlert("The Field " + value + " is not Exist", 3);
                    if (field == "txtArrearMapField") {
                        $('#txtArrearMapField').focus();
                    }
                    else if (field == "txtFormula") {
                        $('#txtFormula').focus();
                    }
                    goloop = false;
                    return false;
                }
            } else {
                if (!$.isNumeric(value)) {
                    var newformula = $formulaCreation.hidenformula.replace((new RegExp('\\b' + data.name + '\\b', 'gi')), '{' + data.id + '}');
                    $formulaCreation.hidenformula = newformula;
                    //if ($formulaCreation.type == 4)
                    // $formulaCreation.ifElseHiddenFormula = newformula;
                }
            }
            if ($formulaCreation.attributeCode == value) {
                $app.showAlert('cannot create the formula using same field', 4);
                goloop = false;
                return false;
            }
        });
    } else {
        goloop = false;
    }
    if (goloop) {
        $app.showAlert(document.getElementById(field).value, 1);
        return true;
    } else {
        return false;
    }

}

$('#txtFormula').blur(function () {//need to validate
    //
    //if ($("#sltEntityType").val() == 3) {
    //            var txt = $("#txtFormula").val(); // load the content
    //            var splchar = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
    //            var result;

    //            var l = txt.length; // length of the original string
    //            var lastChar = txt.substring(l - 1, l); // get the last char of the original string
    //            if (splchar.test(lastChar)) { // if the last char is dot, remove the last char
    //                  result = txt.substring(0, l - 1);

    //                $('#txtFormula').val(result);
    //                $app.showAlert('Enter the correct formula',3);
    //              $('#txtFormula').focus();
    //            }
    //            else { // otherwise do nothing
    //                result = txt;
    //            }

    //        }
    //validateFormulea('txtFormula');

});

//created by AjithPanner on 1/11/2017
$('#txtArrearMapField').blur(function () {//need to validate

    $('.operator').removeAttr('disabled');

    //  validateFormulea('txtArrearMapField');

    //$("#txtArrearMapField").removeAttr("disabled");
});
//$(document).ready(function () {
//    $("#txtArrearMapField").attr("disabled", "disabled");

//})
//$('#txtArrearMapField').ready(function () {
//    $("#txtArrearMapField").attr("disabled", "disabled");
//});

var $formulaCreation = {
    
    canSave: false,
    attrModelIsincre: false,
    formulaFocusedElemt: '',
    totlalIfCont: 0,
    ifFocusedElement: '',
    ifElseHiddenFormula: '',
    ifElseTextFormula: '',
    rangeTextFormula: '',
    rangeHiddenFormula: '',
    hidenformula: '',
    hiddenEligibilityFormula: '',
    attributeModelList: null,
    attributeModelId: '',
    attributeCode: '',
    entityModelId: '',
    entityId: '',
    targetControlId: '',
    formData: document.forms["frmFormula"],
    entityAttributeModel: null,
    arrearHiddenField: '',
    ArrearMapCheck: false,
    BaseFormula:'',
    removeLastChar: function () {
        debugger;
        var txtField = '';
        var valHiddenFiled = '';
        if ($formulaCreation.formulaFocusedElemt == 'txtEligibilityFormula') {
            txtField = 'txtEligibilityFormula';
        }
        else {
            if ($formulaCreation.type == 4) {
                if ($formulaCreation.ifFocusedElement.indexOf('txtIfValue_') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtIfValue_', 'hdIfValue_');
                }
                else {
                    return false;
                }
            }
            if ($formulaCreation.type == 5) {
                if ($formulaCreation.ifFocusedElement.indexOf('txtBaseValue') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtBaseValue', 'hdnBaseValue');
                } else if ($formulaCreation.ifFocusedElement.indexOf('txtRangeValue_') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtRangeValue_', 'hdRangeValue_');
                }
                else {
                    return false;
                }
            }
            if ($formulaCreation.type == 3 || $formulaCreation.type == 7) {
                txtField = 'txtFormula';
                $("#txtArrearMapField").attr("disabled", "disabled");
            }
            if ($formulaCreation.type == 1) {
                $('#txtArrearMapField').val('');
            }
        }
        if (txtField == '') {
            return false;
        }//txtFormula
        $('#' + txtField).val(
    function (index, value) {

        var textVal = value.substr(value.length - 1);
        var hiddenVal = '';
        if (txtField == 'txtEligibilityFormula') {
            hiddenVal = $formulaCreation.hiddenEligibilityFormula.substr($formulaCreation.hiddenEligibilityFormula.length - 1);
        }
        else if (txtField.indexOf('txtIfValue_') >= 0) {
            hiddenVal = $formulaCreation.formData.elements[valHiddenFiled].value.substr($formulaCreation.formData.elements[valHiddenFiled].value.length - 1);
        }
        else if (txtField == 'txtFormula') {
            debugger;
            hiddenVal = $formulaCreation.hidenformula.substr($formulaCreation.hidenformula.length - 1); 
            //hiddenVal = $formulaCreation.hidenformula.slice(-1);
        }

        if (textVal == "+" || textVal == "-" || textVal == "*" || textVal == "/" || textVal == "(" || textVal == ")" || !isNaN(textVal)) {
            if (txtField == 'txtEligibilityFormula') {
                $formulaCreation.hiddenEligibilityFormula = $formulaCreation.hiddenEligibilityFormula.substr(0, $formulaCreation.hiddenEligibilityFormula.length - 1);
            }
            else if (txtField.indexOf('txtIfValue_') >= 0) {
                $formulaCreation.formData.elements[valHiddenFiled].value = $formulaCreation.formData.elements[valHiddenFiled].value.substr(0, $formulaCreation.formData.elements[valHiddenFiled].value.length - 1);
            }
            else if (txtField == 'txtFormula') {
                debugger;
                $formulaCreation.hidenformula = $formulaCreation.hidenformula.substr(0, $formulaCreation.hidenformula.length - 1);
            }
            return value.substr(0, value.length - 1);
        }
        else {
            if (txtField == 'txtEligibilityFormula') {
                $formulaCreation.hiddenEligibilityFormula = $formulaCreation.hiddenEligibilityFormula.substr(0, $formulaCreation.hiddenEligibilityFormula.length - $formulaCreation.hiddenEligibilityFormula.split("").reverse().join("").indexOf('{') - 1);
            }
            else if (txtField.indexOf('txtIfValue_') >= 0) {
                $formulaCreation.formData.elements[valHiddenFiled].value = $formulaCreation.formData.elements[valHiddenFiled].value.substr(0, $formulaCreation.formData.elements[valHiddenFiled].value.length - $formulaCreation.formData.elements[valHiddenFiled].value.split("").reverse().join("").indexOf('{') - 1);
            }
            else if (txtField == 'txtFormula') {
                debugger;
                $formulaCreation.hidenformula = $formulaCreation.hidenformula.substr(0, $formulaCreation.hidenformula.length - $formulaCreation.hidenformula.split("").reverse().join("").indexOf('{') - 1);
            }

            var arrVal = [];
            arrVal.push(value.split("").reverse().join("").indexOf('+'));
            arrVal.push(value.split("").reverse().join("").indexOf('-'));
            arrVal.push(value.split("").reverse().join("").indexOf('*'));
            arrVal.push(value.split("").reverse().join("").indexOf('/'));
            arrVal.push(value.split("").reverse().join("").indexOf('('));
            arrVal.push(value.split("").reverse().join("").indexOf('('));
            var sortList = arrVal.slice();
            var smlval = 0;
            sortList.sort(sortmyway);
            for (var i in sortList) {
                if (sortList[i] != -1) {
                    smlval = sortList[i];
                    break;
                }
            }
            smlval = smlval == 0 ? value.length : smlval;
            return value.substr(0, value.length - smlval);
        }
    })
    },
    //-------
    numberCheck: function isNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    },


    // checkFormula:
    buildFormula: function (e, data, entnumber) {
        debugger;
        var textVal = '';
        var idVal = '';
        var EntityModelId = $formulaCreation.entityModelId;
        var AttributeModelId = $formulaCreation.entityId;
        var EntityId = $formulaCreation.attributeModelId;
        var EntityAttributeModelID;
        var textFieldName = $formulaCreation.formulaFocusedElemt;


        if (entnumber != null) {
            textVal = entnumber;
            idVal = entnumber;
        }
        else {
            if (data == null) {
                textVal = e.target.innerText;
                idVal = e.target.innerText;
            }
            else {
                textVal = data.node.title;
                idVal = data.node.key;
            }
        }
        $formulaCreation.checkFormula(EntityModelId, textVal, AttributeModelId, EntityId, textFieldName);// Created By:sharmila
        if ($formulaCreation.attributeModelId == idVal) {
            $app.showAlert('You cannot create the formula using same field', 4);
            return;
        }
        var value = $formulaCreation.hidenformula;
        var check = true;
        var valHiddenFiled = '';
        var txtField = '';

        if ($formulaCreation.formulaFocusedElemt == 'txtArrearMapField') {

            if (entnumber == null) {
                if (ArrearMapCheck == true) {
                    ArrearMapval = textVal.substr(0, textVal.indexOf(' '));
                    $($formulaCreation.formData).find('#txtArrearMapField').val(ArrearMapval);
                }
                else {
                    $($formulaCreation.formData).find('#txtArrearMapField').val('');
                }
                $formulaCreation.arrearHiddenField = idVal;
                check = false;
                return false;
            }
            else {
                check = false;
                $app.showAlert('You are not allowed to enter number in Arrear map field', 3);
            }
        }
        if ($formulaCreation.formulaFocusedElemt == 'txtEligibilityFormula') {
            txtField = 'txtEligibilityFormula';
            var chktextval = textVal;
            chktextval = textVal.substr(0, textVal.indexOf(' '));
            check = $formulaCreation.validateFormula(txtField, chktextval.length > 0 ? textVal.substr(0, textVal.indexOf(' ')) : textVal); //arul textVal
        }
        else {
            if ($formulaCreation.type == 4) {

                if ($formulaCreation.ifFocusedElement.indexOf('txtIfLeft_') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    $formulaCreation.formData.elements[$formulaCreation.ifFocusedElement].value = '';
                    $formulaCreation.formData.elements[$formulaCreation.ifFocusedElement.replace('txtIfLeft_', 'hdIfLeft_')].value = '';
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtIfLeft_', 'hdIfLeft_');
                    validateFormulea(txtField);
                    check = true;
                }
                else if ($formulaCreation.ifFocusedElement.indexOf('txtIfRight_') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    $formulaCreation.formData.elements[$formulaCreation.ifFocusedElement].value = '';
                    $formulaCreation.formData.elements[$formulaCreation.ifFocusedElement.replace('txtIfRight_', 'hdIfRight_')].value = '';
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtIfRight_', 'hdIfRight_');
                    validateFormulea(txtField);
                    check = true;
                }
                else if ($formulaCreation.ifFocusedElement.indexOf('txtIfValue_') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    //  check = $formulaCreation.validateFormula($formulaCreation.ifFocusedElement, textVal);  

                    var chktextval = textVal;
                    chktextval = textVal.substr(0, textVal.indexOf(' '));
                    check = $formulaCreation.validateFormula(txtField, chktextval.length > 0 ? textVal.substr(0, textVal.indexOf(' ')) : textVal); //arul textVal
                    if (check) validateFormulea(txtField);
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtIfValue_', 'hdIfValue_');
                }
                else {
                    check = false;
                    return false;
                }
            }
            if ($formulaCreation.type == 5) {
                if ($formulaCreation.ifFocusedElement.indexOf('txtRangeFrom_') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    $formulaCreation.formData.elements[$formulaCreation.ifFocusedElement].value = '';
                    $formulaCreation.formData.elements[$formulaCreation.ifFocusedElement.replace('txtRangeFrom_', 'hdRangeFrom_')].value = '';
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtRangeFrom_', 'hdRangeFrom_');
                    check = true;
                }
                else if ($formulaCreation.ifFocusedElement.indexOf('txtRangeTo_') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    $formulaCreation.formData.elements[$formulaCreation.ifFocusedElement].value = '';
                    $formulaCreation.formData.elements[$formulaCreation.ifFocusedElement.replace('txtRangeTo_', 'hdRangeTo_')].value = '';
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtRangeTo_', 'hdRangeTo_');
                    check = true;
                }
                else if ($formulaCreation.ifFocusedElement.indexOf('txtRangeValue_') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    check = $formulaCreation.validateFormula($formulaCreation.ifFocusedElement, textVal);
                    valHiddenFiled = $formulaCreation.ifFocusedElement.replace('txtRangeValue_', 'hdRangeValue_');
                } else if ($formulaCreation.ifFocusedElement.indexOf('txtBaseValue') >= 0) {
                    txtField = $formulaCreation.ifFocusedElement;
                    check = true;
                    valHiddenFiled = 'hdnBaseValue';
                }
                else {
                    check = false;
                    return false;
                }
            }
            if ($formulaCreation.type == 3 || $formulaCreation.type == 1 || $formulaCreation.type == 7) {
                $formulaCreation.formulaFocusedElemt = 'txtFormula';
                txtField = 'txtFormula';
                var chktextval = textVal;
                chktextval = textVal.substr(0, textVal.indexOf(' '));
                check = $formulaCreation.validateFormula(txtField, chktextval.length > 0 ? textVal.substr(0, textVal.indexOf(' ')) : textVal); //arul textVal
                //txtField=
            }
        }
        if (check) {
            var chktextval = textVal;
            chktextval = textVal.substr(0, textVal.indexOf(' '));
            textVal = chktextval.length > 0 ? textVal.substr(0, textVal.indexOf(' ')) : textVal;  //arul textVal           
            idVal = idVal.trim();
            if (txtField != '') {
                $formulaCreation.formData.elements[txtField].value = $formulaCreation.formData.elements[txtField].value + textVal;
            }
            if (idVal == "+" || idVal == "-" || idVal == "*" || idVal == "/" || idVal == "(" || idVal == ")") {
                if (txtField == 'txtEligibilityFormula') {
                    $formulaCreation.hiddenEligibilityFormula = $formulaCreation.hiddenEligibilityFormula + idVal;
                }
                else if (txtField == 'txtBaseValue') {
                    $formulaCreation.BaseFormula = $formulaCreation.BaseFormula + idVal;
                }
                else {
                    debugger
                    if ($formulaCreation.type == 3 || $formulaCreation.type == 1 && $formulaCreation.type == 7) {
                        $formulaCreation.hidenformula = $formulaCreation.hidenformula + idVal;
                    }
                    else if ($formulaCreation.type == 4) {
                        $formulaCreation.formData.elements[valHiddenFiled].value = $formulaCreation.formData.elements[valHiddenFiled].value + idVal;
                    }
                }
            }
            else {

                if (entnumber != null) {
                    if (txtField == 'txtEligibilityFormula') {
                        $formulaCreation.hiddenEligibilityFormula = $formulaCreation.hiddenEligibilityFormula + idVal;
                    }
                   else if (txtField == 'txtBaseValue') {
                       $formulaCreation.BaseFormula = $formulaCreation.BaseFormula + idVal;
                    }
                    else {
                        if ($formulaCreation.type == 3 || $formulaCreation.type == 7) {
                            $formulaCreation.hidenformula = $formulaCreation.hidenformula + idVal;
                        }
                        else if ($formulaCreation.type == 4) {
                            $formulaCreation.formData.elements[valHiddenFiled].value = $formulaCreation.formData.elements[valHiddenFiled].value + idVal;
                        }
                    }
                }
                else {
                    if (txtField == 'txtEligibilityFormula') {
                        $formulaCreation.hiddenEligibilityFormula = $formulaCreation.hiddenEligibilityFormula + '{' + idVal + '}';
                    }
                    else if (txtField == 'txtBaseValue') {
                        $formulaCreation.BaseFormula = $formulaCreation.BaseFormula + '{' + idVal + '}';
                    }
                    else {
                        if ($formulaCreation.type == 3 || $formulaCreation.type == 1 || $formulaCreation.type == 7) {
                            $formulaCreation.hidenformula = $formulaCreation.hidenformula + '{' + idVal + '}';
                        }
                        else if ($formulaCreation.type == 4) {
                            $formulaCreation.formData.elements[valHiddenFiled].value = $formulaCreation.formData.elements[valHiddenFiled].value + '{' + idVal + '}';
                        }
                    }
                }
            }
            $app.showAlert($formulaCreation.formData.elements[txtField].value, 1);
        }

    },
    ///
    //CreatedBy:Sharmila
    //CreatedOn:24.04.17
    // checkFormula: function
    ///
    checkFormula: function (EntityModelId, textVal, AttributeModelId, EntityId, textFieldName) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetEntityValueList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                EntityModelId: EntityModelId, textValue: textVal, EntityAttModelId: AttributeModelId, EntityId: EntityId, textName: textFieldName, AttributeCode: $formulaCreation.attributeCode, AttributeId: $formulaCreation.attributeModelId
            }),
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        ArrearMapCheck = true;
                        break;
                    case false:
                        // Modified by AjithPanner on 2/11/2017
                        $app.showAlert(jsonResult.Message, 4);
                        var msgs = jsonResult.Message;
                        var var1 = "Only Earning master component allowed for Arrear Mapping";
                        var var2 = "Arrear component not yet saved";
                        var var3 = "Already Mapped";
                        var var4 = "Not as Master";
                        var var5 = $("#txtArrearMapField").val();
                        var var6 = var4 + var5;
                        var var7 = var3 + var5;
                        var var8 = var2 + var5;
                        var var9 = var1;
                        if (msgs.trim() == var9.trim()) {
                            $("#txtArrearMapField").val('');
                            $formulaCreation.arrearHiddenField = '';
                        }
                        if (msgs.trim() == var8.trim()) {
                            $("#txtArrearMapField").val('');
                            $formulaCreation.arrearHiddenField = '';
                        }
                        else if (msgs.trim() == var6.trim()) {
                            $("#txtArrearMapField").val('');
                            $formulaCreation.arrearHiddenField = '';
                        }
                        else if (msgs.trim() == var7.trim()) {
                            $("#txtArrearMapField").val('');
                            $formulaCreation.arrearHiddenField = '';
                        }
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    validateFormula: function (controlElement, textVal) {

        var controlval = $formulaCreation.formData.elements[controlElement].value;
        var check = true;

        if (controlval != null && controlval != "") {
            var lastChar = controlval[controlval.length - 1];
            if ((lastChar == "+" || lastChar == "-" || lastChar == "*" || lastChar == "/") && (
                textVal == "+" || textVal == "-" || textVal == "*" || textVal == "/" || textVal == ")")
                ) {
                check = false;
                $app.showAlert('There is an error in expression', 4);
            }
            else if (lastChar == "(" && (textVal == "+" || textVal == "-" || textVal == "*" || textVal == "/" || textVal == ")")) {
                check = false;
                $app.showAlert('There is an error in expression', 4);
            }
            else if (lastChar == ")" && (textVal != "+" && textVal != "-" && textVal != "*" && textVal != "/" && textVal != ")")) {
                check = false;
                $app.showAlert('There is an error in expression', 4);
            }
            else if (!isNaN(lastChar) && (textVal != "+" && textVal != "-" && textVal != "*" && textVal != "/" && textVal != ")")) {
                check = false;
                $app.showAlert('There is an error in expression', 4);
            }
            else if ((lastChar != "+" && lastChar != "-" && lastChar != "*" && lastChar != "/" && lastChar != "(" && lastChar != ")")
    && (textVal != "+" && textVal != "-" && textVal != "*" && textVal != "/" && textVal != "(" && textVal != ")")
    ) {
                check = false;
                $app.showAlert('There is an error in expression', 4);
            }
            else if ((lastChar != "+" && lastChar != "-" && lastChar != "*" && lastChar != "/" && lastChar != "(" && lastChar != ")")
    && (textVal == "(")) {
                check = false;
                $app.showAlert('There is an error in expression', 4);
            }
        }
        else {
            if (textVal == "+" || textVal == "-" || textVal == "*" || textVal == "/") {
                $app.showAlert('You can not enter operator with empty preceeded value.', 4);
                check = false;
            }
                
        }

        return check;
    },
    setArrearMatchField: function (data) {

        if ($formulaCreation.attributeModelId == data.key) {
            $app.showAlert('You cannot create the formula using same field', 4);
            return;
        }
        debugger;
        $formulaCreation.buildFormula(event, data);
        $($formulaCreation.formData).find('#txtArrearMapField').val(data.title);
        $formulaCreation.arrearHiddenField = data.key;
    },
    renderFieldGrid: function (context) {
        var grid = '<table id="tblFormula" class="">'
            + '<thead>'
                    + '<tr>'
                    + '<th class="nodisp">'
                    + '</th>'
        for (var cnt = 1; cnt < context.length; cnt++) {
            grid = grid + '<th>' + context[cnt].tableHeader + '</th>'
        }
        grid = grid + '<th>Action</th></tr></thead>';
        grid = grid + '<tbody><tr>'
        for (var cnt = 0; cnt <= context.length; cnt++) {//for action td 
            grid = grid + '<td></td>';
        }
        grid = grid + '</tr></tbody></table>';
        $("#dvFormula").html('');
        $("#dvFormula").html(grid);
    },
    renderGrid: function () {
        var gridObject = [
                                                { tableHeader: "", tableValue: "FormulaId" },
                                                { tableHeader: "Field Name", tableValue: "name" },
                                                { tableHeader: "Display As", tableValue: "displayAs" },
                                                { tableHeader: "Type", tableValue: "type" },
                                                { tableHeader: "Percentage", tableValue: "Percentage" },
                                                { tableHeader: "Formula", tableValue: "Formula" },
                                                { tableHeader: "Rounding", tableValue: "rounding" },
                                                { tableHeader: "Maximum", tableValue: "Maximum" }
        ];
        $formulaCreation.renderFieldGrid(gridObject);
        $formulaCreation.LoadAttributeModels(gridObject);
        $formulaCreation.loadAttributeModelList();


    },
    LoadAttributeModels: function (context) {

        var columnsValue = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
        }
        columnsValue.push({ "data": null }); //for action column
        var columnDef = [];
        columnDef.push({ "aTargets": [0], "sClass": "nodisp", "bSearchable": false }); //for id column
        for (var cnt1 = 1; cnt1 <= context.length; cnt1++) {
            if (cnt1 == context.length) {
                columnDef.push(
                                                {
                                                    "aTargets": [cnt1],
                                                    "sClass": "actionColumn",
                                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                                        var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                                        var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                                                        b.button();
                                                        b.on('click', function () {
                                                            $formulaCreation.edit(oData);
                                                            return false;
                                                        });
                                                        c.button();
                                                        c.on('click', function () {
                                                            DeleteClientRecord(oData.Id);
                                                            return false;
                                                        });
                                                        $(nTd).empty();
                                                        $(nTd).prepend(b, c);
                                                    }
                                                }

                    ); //for action column
            }
            else {
                columnDef.push({ "aTargets": [cnt1], "sClass": "word-wrap" }); //for action column
            }
        }
        var dtClientList = $('#tblFormula').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Entity/GetFormulas",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ categoryId: $formulaCreation.selectedCategoryId, type: $formulaCreation.type }),
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
                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
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
            scroller: {
                loadingIndicator: true
            }
        });
    },
    edit: function (data) {
        debugger;
        $('#dvFormulaCreationAttribute').removeClass('hide').addClass('show');
        $('#hdMonthlyInput').removeClass('hide').addClass('show');
        $formulaCreation.attributeCode = data.name;
        $formulaCreation.canSave = true;
        $formulaCreation.loadAttributeModelList();
        $formulaCreation.hiddenEligibilityFormula = '';
        $formulaCreation.formulaFocusedElemt = '';
        $formulaCreation.ifFocusedElement = '';
        $formulaCreation.BaseFormula = '';
        $formulaCreation.rangeHiddenFormula ='';
        $($formulaCreation.formData).find('#btnViewIf').addClass('hide');
        $($formulaCreation.formData).find('#btnViewRange').addClass('hide');
        $($formulaCreation.formData).find('#dvNormalFormula').show();
        $($formulaCreation.formData).find('#dvConditionalFormula').hide();
        $('#AddFormula').modal('toggle');
        $formulaCreation.hidenformula = data.hiddenform != null ? data.hiddenform : '';
        $formulaCreation.arrearHiddenField = data.arrearMatchField != null ? data.arrearMatchField : '';
        $($formulaCreation.formData).find('#txtArrearMapField').val(data.arrearMatchfieldName);
        $($formulaCreation.formData).find('#txtFieldName').val(data.name);
        $($formulaCreation.formData).find('#lblFormulatitle').text("Add/Edit " + data.Displayname);  //title change
        $($formulaCreation.formData).find('#sltEntityType').val(data.valueType == 0 ? 1 : data.valueType);
        $($formulaCreation.formData).find('#txtFormula').val(data.formula);
        $($formulaCreation.formData).find('#txtPercentage').val(data.percentage);
        $($formulaCreation.formData).find('#txtMaximum').val(data.maximum);
        $($formulaCreation.formData).find('#sltRounding').val(data.roundingId == 0 ? 1 : data.roundingId);
        $formulaCreation.type = data.valueType;
        $($formulaCreation.formData).find('#txtEligibilityFormula').val(data.eligibilityFormula != null ? data.eligibilityFormula : '');//hiddenEligibilityFormula
        $formulaCreation.hiddenEligibilityFormula = data.hiddenEligibilityFormula != null ? data.hiddenEligibilityFormula : '';
        $formulaCreation.BaseFormula = data.baseFormula;
        $formulaCreation.rangeHiddenFormula = data.formula;
        $($formulaCreation.formData).find('#txtBaseValue').val(data.baseValue);
        if (data.name.toLowerCase() == "eg" || data.name.toLowerCase() == "totded" || data.name.toLowerCase() == "netpay")
        {
            debugger;
            $($formulaCreation.formData).find('#txtFormula').prop('disabled', true);
            $($formulaCreation.formData).find('#sltEntityType').val(3);
            $($formulaCreation.formData).find('#sltEntityType').prop('disabled', true);
        }
        else {
            debugger;
            $($formulaCreation.formData).find('#txtFormula').prop('disabled', false);
            $($formulaCreation.formData).find('#sltEntityType').prop('disabled', false);
            $($formulaCreation.formData).find('#sltEntityType').val($formulaCreation.type);
        }

        // $($formulaCreation.formData).find('#sltEntityType').prop('readonly', false);
        //   $($formulaCreation.formData).find('#sltEntityType').prop('disabled', false);
        if ($formulaCreation.type == 1) {//master input
            $($formulaCreation.formData).find('#txtFormula').prop('readonly', false);
            $($formulaCreation.formData).find('#txtFormula').prop('onkeypress', 'return $validator.IsNumeric(event, this.id);');
        }
        if ($formulaCreation.type == 2 && data.IsMonthlyInput) {//monthly input
            $('#dvFormulaCreationAttribute').removeClass('show').addClass('hide');
            $('#hdMonthlyInput').removeClass('show').addClass('hide');           
            $($formulaCreation.formData).find('#sltEntityType').attr("disabled", true); 
        }
        if ($formulaCreation.type == 3 || $formulaCreation.type == 4 || $formulaCreation.type == 5 || $formulaCreation.type == 7) {//formula
            // $($formulaCreation.formData).find('#txtFormula').prop('readonly', true);
            $("#txtArrearMapField").attr("disabled", "disabled");
            if ($formulaCreation.type == 4) {
                $formulaCreation.ifElseHiddenFormula = data.hiddenform != null ? data.hiddenform : '';
                $formulaCreation.ifElseTextFormula = data.formula;
                $($formulaCreation.formData).find('#btnViewIf').removeClass('hide');

            }
            if ($formulaCreation.type == 5) {
                $($formulaCreation.formData).find('#btnViewRange').removeClass('hide');
            }
        }
        if ($formulaCreation.entityAttributeModel.AttributeModel.IsIncrement == true) {
            $($formulaCreation.formData).find('#dvArrearMap').removeClass('nodisp');
            $($formulaCreation.formData).find('#sltEntityType').val($formulaCreation.type); //-----(1)
            $($formulaCreation.formData).find('#sltEntityType').prop('readonly', true);

            //     $($formulaCreation.formData).find('#sltEntityType').prop('disabled', true);
        }
        else {
            $($formulaCreation.formData).find('#dvArrearMap').addClass('nodisp');
        }
        if (data.name == 'PF') {
            $("#sltEntityType option[value='4']").each(function () {
                $(this).hide();
            });
            $("#sltEntityType option[value='5']").each(function () {
                $(this).hide();
            });
        }
        else {
            $("#sltEntityType option[value='4']").each(function () {
                $(this).removeAttr('style');
            });
            $("#sltEntityType option[value='5']").each(function () {
                $(this).removeAttr('style');
            });
        }
        if (data.name == 'PTAX' || data.name == 'ESI' || data.name == 'PF') {

            $($formulaCreation.formData).find('#dvEligibityFromula').removeClass('nodisp');
        }
        else {
            $($formulaCreation.formData).find('#dvEligibityFromula').addClass('nodisp');
        }
        if (data.name == 'PTAX') {
            $('#txtFormula').attr('readonly', true);
        }
        else {
            $('#txtFormula').attr('readonly', false);
        }
        $formulaCreation.renderChildBehav(data.childBehavior);

    },
    save: function () {
        debugger;
        if (validateFormulea('txtFormula')) {
            if (!$formulaCreation.canSave) {
                return false;
            }
            $formulaCreation.canSave = false;
            $app.showProgressModel();
            var hiddenFormula = '';
            if ($("#sltEntityType").val() == 1) {
                hiddenFormula = $formulaCreation.formData.elements["txtFormula"].value;
            }
            if ($("#sltEntityType").val() == 2) {
                hiddenFormula = '';
                $("#txtFormula").val("0");
            }
            else if ($("#sltEntityType").val() == 3 || $("#sltEntityType").val() == 7) {
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
                //hiddenFormula = $formulaCreation.ifElseHiddenFormula;
                hiddenFormula = $formulaCreation.hidenformula;
            }

            var data = {
                attrubuteModelId: $formulaCreation.attributeModelId,
                entityModelId: $formulaCreation.entityModelId,
                entityId: $formulaCreation.entityId,
                hiddenform: hiddenFormula,
                name: $formulaCreation.formData.elements["txtFieldName"].value,
                valueType: $("#sltEntityType").val(),
                type: $formulaCreation.formData.elements["sltEntityType"].value,
                formula: $formulaCreation.formData.elements["txtFormula"].value,
                percentage: $formulaCreation.formData.elements["txtPercentage"].value,
                maximum: $formulaCreation.formData.elements["txtMaximum"].value,
                roundingId: $formulaCreation.formData.elements["sltRounding"].value,
                arrearMatchField: $formulaCreation.arrearHiddenField,
                hiddenEligibilityFormula: $formulaCreation.hiddenEligibilityFormula,
                //arrearMatchfieldName: $($formulaCreation.formData).find('#txtArrearMapField').val()
                baseFormula: $formulaCreation.BaseFormula,
                basevalue: $formulaCreation.formData.elements["txtBaseValue"].value,
            };
            var keyvalues = [];
            $('#dvChildBehav :input').each(function (int, input) {
                if (input.tagName == "INPUT") {
                    keyvalues.push({ attrubuteModelId: input.id.replace('txtChild_', ''), percentage: input.value });//name: input.name,
                }
            });
            //Created by AjithPanner on 11/11/17
            var textBox = $('#txtArrearMapField').val();
            if (textBox == "") {
                data.arrearMatchField = null;
            }
            data.childBehavior = keyvalues;
            $.ajax({
                url: $app.baseUrl + "Entity/SaveEntityBehavior",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {

                    $app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:
                            debugger;
                            $('#' + $formulaCreation.targetControlId).val($($formulaCreation.formData).find('#txtFormula').val());
                            document.getElementById('lbl' + $formulaCreation.targetControlId).style.color = "Blue";
                            $('#AddFormula').modal('toggle');
                            $formulaCreation.entityId = jsonResult.result.EntityId;
                            $dyanmicEntity.selectedEntityId = jsonResult.result.EntityId;
                            return false;
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 2);
                            break;
                        case false:
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 4);
                            break;
                    }
                },
                complete: function () {
                    $app.hideProgressModel();
                }
            });
            return false;
        }
    },
    renderChildBehav: function (data) {


        $('#dvChildBehav').html('');
        if (data.length > 0) {
            $('#dvChildBehav').append('<h4>Employer Contribution(In Percentage)</h4><hr />');
        }
        for (var cnt = 0; cnt < data.length; cnt++) {
            //onkeypress="return $validator.IsNumeric(event, this.id);"

            var strAge = data[cnt].name == "PFSNRCITZAGE" ? "Enter the Senior Citizen Age" : "Enter the Percentage"
            var txtValue = data[cnt].percentage == null ? "" : data[cnt].percentage;
            if (txtValue == "") {
                txtValue = data[cnt].defaultValue;
            }
            var Keypress = data[cnt].name == "PFSNRCITZAGE" ? ' " onkeypress="return $validator.IsNumeric(event, this.id);" maxlength="2" ' : '" onkeypress="return $validator.checkDecimal(event, 2)"';
            var temp = '<div class="form-group"><label class="control-label col-md-4">' + data[cnt].Displayname + '</label>'
                             + '<div class="col-md-7">'
    + '<input type="text" id="txtChild_' + data[cnt].attrubuteModelId + Keypress + ' class="form-control" placeholder="' + strAge + '" value="' + txtValue + '" required/>'
                               + '</div></div>';
            $('#dvChildBehav').append(temp);

        }
    },
    renderAttributeModelTree: function (data) {

        var treeData = [];
        for (var cnt = 0; cnt < data.length; cnt++) {
            if (data[cnt].AttributeModelList.length <= 0) {
                treeData.push({ key: data[cnt].Id, title: data[cnt].DisplayAs });
            }
            else {
                var treeChildData = [];
                for (var childcnt = 0; childcnt < data[cnt].AttributeModelList.length; childcnt++) {
                    treeChildData.push({ key: data[cnt].AttributeModelList[childcnt].Id, title: data[cnt].AttributeModelList[childcnt].Name + ' [' + data[cnt].AttributeModelList[childcnt].DisplayAs + ']' });
                }
                treeData.push({ key: data[cnt].Id, title: data[cnt].DisplayAs, children: treeChildData });
            }
        }
        //initialize the tree
        $("#attributeModeltree").fancytree();
        //before load clean the tree nodes
        $("#attributeModeltree").fancytree("destroy");
        fancyTree = jQuery("#attributeModeltree").fancytree({
            extensions: ["contextMenu", "filter"],
            selectMode: 3,
            strings: {
                loading: "Loading..."
            },
            source: treeData,
            dblclick: function (event, data) {

                debugger;
                $formulaCreation.buildFormula(event, data);
            },
            filter: {
                // mode: "dimm"    
                mode: "hide"  // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
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
            }
            , contextMenu: {
                menu: function (node) {
                    if (node.parent.title == 'root') {
                        return {
                            // 'AddFormula': { 'name': 'Add to Formula', 'icon': 'new', 'disabled': ((node.data.FileType === "2") ? true : false) }
                        }
                    }
                    else if ($formulaCreation.entityAttributeModel.AttributeModel.IsIncrement) {
                        return {
                            'AddFormula': { 'name': 'Add to Formula', 'icon': 'add' },
                            'SetArrear': { 'name': 'Set Arrear Field', 'icon': 'add' }
                        }
                    }
                    else {
                        return {
                            'AddFormula': { 'name': 'Add to Formula', 'icon': 'add' }

                        }
                    }
                },
                actions: function (node, action, options) {
                    if (action === "AddFormula") {
                        debugger;
                        $formulaCreation.buildFormula(null, { node: node }, null);
                    }
                    else if (action === "SetArrear") {
                        $formulaCreation.setArrearMatchField(node);
                    }
                    else {
                        return false;
                    }
                }
            }
        });
    },
    loadAttributeModelList: function (isTaxable) {
        if (isTaxable != "Tax") {
            isTaxable = "";
        }
        $.ajax({
            //url: $app.baseUrl + "Entity/GetFormulaAttributeModelList",
            //data: JSON.stringify({ entityModelId: $formulaCreation.entityModelId }),
            url: $app.baseUrl + "Entity/GetPayrollmodels",
            data: JSON.stringify({ name: 'Salary', isTaxable: isTaxable }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $formulaCreation.attributeModelList = p;
                        $formulaCreation.renderAttributeModelTree(p);
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
    loadEntityBehavior: function (data, targetId, staticFormula) {

        $formulaCreation.attrModelIsincre = data.data.AttributeModel.IsIncrement;
        $formulaCreation.attributeModelId = data.AttributeModelId;
        $formulaCreation.entityId = data.EntityId;
        $formulaCreation.entityModelId = data.EntityModelId;
        $formulaCreation.targetControlId = targetId;
        $formulaCreation.entityAttributeModel = data.data;
        $.ajax({
            url: $app.baseUrl + "Entity/GetEntityBehavior",
            data: JSON.stringify({ attributeModelId: data.AttributeModelId, entityId: data.EntityId, entityModelId: data.EntityModelId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        var p = jsonResult.result;
                        if (staticFormula && p.Displayname == "PF") {

                            //  p.percentage = p.percentage;

                            p.percentage = p.percentage;
                        }
                        else {
                            p.formula = staticFormula;
                           

                        }
                        $formulaCreation.hidenformula = p.formula;
                        $formulaCreation.edit(p);
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
    }
    /* if else block*/
        ,
    BuildIfElseSegment: function (isifOnly) {
        debugger;
        if ($('#slt_' + $formulaCreation.totlalIfCont).length > 0) {
            if ($('#slt_' + $formulaCreation.totlalIfCont).val() == 'Else') {
                alert('You can not add the condition after else block');
                return;
            }
        }
        $formulaCreation.totlalIfCont = $formulaCreation.totlalIfCont + 1;
        var condCnt = $formulaCreation.totlalIfCont;
        var readonly = condCnt == 1 ? 'readonly' : '';
        var ifItems = '';
        if (condCnt == 1 || isifOnly) {
            ifItems = '<option value="If">If</option>';
            readonly = 'readonly';
        }
        else {
            ifItems = '<option value="ElseIf">Else If</option><option value="Else">Else</option>';
        }
        //readonly="readonly"
        var template = ' <div class="" id="dvCondition_' + condCnt + '">'
        + '<div class="col-md-2 paddingcustomize"><select class="form-control paddingcustomize" id="slt_' + condCnt + '" ' + readonly + '>' + ifItems + '</select></div>'
        + '<div class="col-md-3 paddingcustomize" id="dvleft_' + condCnt + '"><input type="hidden" id="hdIfLeft_' + condCnt + '" /><input type="text" id="txtIfLeft_' + condCnt + '" class="form-control" onblur="validateFormulea(this.id)"  placeholder="Enter the Condition" readonly /></div>'
        + '<div class="col-md-2 paddingcustomize" id="dvOperator_' + condCnt + '"><select class="form-control paddingcustomize" id="sltOperator_' + condCnt + '"><option value="=">=</option><option value=">">></option><option value="<"><</option><option value="<="><=</option><option value=">=">>=</option><option value="!=">!=</option></select></div>'
        + '<div class="col-md-3 paddingcustomize" id="dvRight_' + condCnt + '"><input type="hidden" id="hdIfRight_' + condCnt + '" /><input type="text" id="txtIfRight_' + condCnt + '" onblur="validateFormulea(this.id)"  class="form-control"  placeholder="Enter the Condition" readonly /></div>'
        + '<div class="col-md-2 paddingcustomize" id=dvThen_"' + condCnt + '"><select class="form-control paddingcustomize" id="sltThen_' + condCnt + '"><option value="THEN">THEN</option><option value="AND">AND</option><option value="OR">OR</option></select></div>'
        + '<div class="col-md-10 marginbt7 paddingcustomize" id="dvIfVal_' + condCnt + '"><input type="hidden" id="hdIfValue_' + condCnt + '" /><input type="text" id="txtIfValue_' + condCnt + '" class="form-control" onchange="validateFormulea(this.id)" placeholder="Enter the value"/></div>'
        + '<div class="col-md-1 marginbt7 paddingcustomize" id="dvIfClr_' + condCnt + '"><span id="searchclear_' + condCnt + '" class="glyphicon glyphicon-refresh" style="color:#6495ED"></span></div>'
        + '<div class="col-md-1 marginbt7 paddingcustomize" id="dvIfRemove_' + condCnt + '"><span id="searchConRemove_' + condCnt + '" class="glyphicon glyphicon-remove" style="color:#FF0000"></span></div>'
        + '</div>  <hr id="hr_' + condCnt + '" />'
        $('#dvIfElseCondition').append(template);
        $('#slt_' + condCnt).on('change', function () {
            var sltIf = $('#slt_' + condCnt).val();
            $('#dvleft_' + condCnt).hide();
            $('#dvOperator_' + condCnt).hide();
            $('#dvRight_' + condCnt).hide();
            $('#dvThen_' + condCnt).hide();
            $('#sltThen_' + condCnt).prop('readonly', '');

            if (sltIf == 'If' || sltIf == 'ElseIf') {
                $('#dvleft_' + condCnt).show();
                $('#dvOperator_' + condCnt).show();
                $('#dvRight_' + condCnt).show();
                $('#dvThen_' + condCnt).show();
            }
            else if (sltIf == 'Else') {
                $('#dvThen_' + condCnt).show();
                $('#sltThen_' + condCnt).val('THEN');
                $('#sltThen_' + condCnt).prop('readonly', 'readonly');
                if (condCnt < $formulaCreation.totlalIfCont) {
                    $formulaCreation.removeUnWantedIfElse(condCnt);
                }

            }
        });
        $('#sltThen_' + condCnt).on('change', function () {
            $('#dvIfVal_' + condCnt).hide();
            var sltthen = $('#sltThen_' + condCnt).val();
            if (sltthen == 'AND' || sltthen == 'OR') {
                if (condCnt == $formulaCreation.totlalIfCont) {
                    $formulaCreation.BuildIfElseSegment(true);
                }
                else if (condCnt < $formulaCreation.totlalIfCont) {
                    $formulaCreation.removeUnWantedIfElse(condCnt);
                    $formulaCreation.BuildIfElseSegment(true);
                }
                $('#dvIfVal_' + condCnt).hide();

            }
            else if (sltthen == 'THEN') {
                $('#dvIfVal_' + condCnt).show();
                $formulaCreation.removeUnWantedIfElse(condCnt);
            }
        });

        $('#txtIfLeft_' + condCnt).focus(function () {
            $formulaCreation.ifFocusedElement = 'txtIfLeft_' + condCnt;

        });
        $('#txtIfRight_' + condCnt).focus(function () {
            $formulaCreation.ifFocusedElement = 'txtIfRight_' + condCnt;
        });
        $('#txtIfValue_' + condCnt).focus(function () {
            $formulaCreation.ifFocusedElement = 'txtIfValue_' + condCnt;
        });

        $(document).ready(function () {

            $('#searchclear_' + condCnt).click(function () {

                $('#txtIfLeft_' + condCnt).val('').focus();
                $('#txtIfRight_' + condCnt).val('').focus();
                $('#txtIfValue_' + condCnt).val('').focus();

            });
            $('#searchConRemove_' + condCnt).click(function () {

                $('#dvCondition_' + condCnt).val('').remove();
                $('#hr_' + condCnt).val('').remove();
            });
        });
        return false;

    },
    removeUnWantedIfElse: function (currentCount) {
        var det = 0;
        var cnt = currentCount + 1;
        for (; cnt <= $formulaCreation.totlalIfCont; cnt++) {
            $('#dvCondition_' + cnt).remove();
            $('#hr_' + cnt).remove();
            det = det + 1;
        }
        $formulaCreation.totlalIfCont = $formulaCreation.totlalIfCont - det;
    },
    buildIfElseFormula: function () {

        var formulaText = '';
        var formulaVal = '';
        for (var dvCount = 1; dvCount <= $formulaCreation.totlalIfCont; dvCount++) {
            if ($('#dvCondition_' + dvCount).length > 0) {
                var ifSelectVal = $('#slt_' + dvCount).val();
                var ifSelectText = $('#slt_' + dvCount).val();
                var ifLeftVal = $('#hdIfLeft_' + dvCount).val() == '' ? 0 : $('#hdIfLeft_' + dvCount).val();
                var ifLeftText = $('#txtIfLeft_' + dvCount).val() == '' ? 0 : $('#txtIfLeft_' + dvCount).val();
                var ifOperatorVal = $('#sltOperator_' + dvCount).val();
                var ifRightVal = $('#hdIfRight_' + dvCount).val() == '' ? 0 : $('#hdIfRight_' + dvCount).val();
                var ifRightText = $('#txtIfRight_' + dvCount).val() == '' ? 0 : $('#txtIfRight_' + dvCount).val();
                var ifSelectThen = $('#sltThen_' + dvCount).val();
                var ifAssignedVal = $('#hdIfValue_' + dvCount).val() == '' ? 0 : $('#hdIfValue_' + dvCount).val();
                var ifAssignedText = $('#txtIfValue_' + dvCount).val() == '' ? 0 : $('#txtIfValue_' + dvCount).val();
                if (ifSelectVal == 'Else') {
                    ifOperatorVal = '';
                    ifLeftVal = '';
                    ifLeftText = '';
                    ifRightText = '';
                    ifRightVal = '';
                }
                var statementEnd = '';
                if (ifSelectThen == 'THEN') {
                    statementEnd = ':';
                }
                if (ifSelectThen != 'THEN') {
                    ifAssignedText = '';
                    ifAssignedVal = '';
                }
                formulaText = formulaText + ifSelectText + ' ' + ifLeftText + ' ' + ifOperatorVal + ' ' + ifRightText + ' ' + ifSelectThen + ' ' + ifAssignedText + statementEnd + '';
                formulaVal = formulaVal + ifSelectVal + ' ' + ifLeftVal + ' ' + ifOperatorVal + ' ' + ifRightVal + ' ' + ifSelectThen + ' ' + ifAssignedVal + statementEnd + '';

            }

        }

        $formulaCreation.ifElseTextFormula = formulaText;
        $formulaCreation.ifElseHiddenFormula = formulaVal;

    },
    buildRange: function (basevalue, hdnBasevalue) {
        debugger;
        var formulaText = '';
        var formulaVal = '';

        for (var dvCount = 1; dvCount <= $formulaCreation.totlalIfCont; dvCount++) {
            if ($('#dvRangeCondition_' + dvCount).length > 0) {

                var rangefromVal = $('#hdRangeFrom_' + dvCount).val() == '' ? 0 : $('#hdRangeFrom_' + dvCount).val();
                var rangefromText = $('#txtRangeFrom_' + dvCount).val() == '' ? 0 : $('#txtRangeFrom_' + dvCount).val();

                var rangetoVal = $('#hdRangeTo_' + dvCount).val() == '' ? 0 : $('#hdRangeTo_' + dvCount).val();
                var rangetoText = $('#txtRangeTo_' + dvCount).val() == '' ? 0 : $('#txtRangeTo_' + dvCount).val();
                validateFormulea('txtRangeValue_' + dvCount);
                //var rangeVal = $('#hdRangeValue_' + dvCount).val() == '' ? 0 : $('#hdRangeValue_' + dvCount).val();
                var rangeVal = $formulaCreation.hidenformula;
                var rangeText = $('#txtRangeValue_' + dvCount).val() == '' ? 0 : $('#txtRangeValue_' + dvCount).val();

                if (dvCount == 1) {
                    start = "Else";
                } else if ($formulaCreation.totlalIfCont == dvCount) {
                    start = "If";
                } else {
                    start = "ElseIf";
                }
                // formulaText = formulaText + start + basevalue + ' > ' + rangefromText  + ' AND ' + basevalue + ' < ' + rangetoText + ' THEN ' + rangeText + ':';
                //  formulaVal = formulaText + start + hdnBasevalue + ' > ' + rangefromVal + ' AND ' + hdnBasevalue + ' < ' + rangetoVal + ' THEN ' + rangeVal + ':';
                formulaText = formulaText + rangefromText + '-' + rangetoText + 'THEN' + rangeText + ':';
                formulaVal = formulaText + rangefromVal + '-' + rangetoVal + 'THEN' + rangeVal + ':';

            }

        }
        $formulaCreation.rangeTextFormula = formulaText;
        // $formulaCreation.rangeHiddenFormula = formulaVal;
        $formulaCreation.rangeHiddenFormula = formulaText;

    },
    editIfElse: function () {
        debugger;
        var formula = $formulaCreation.ifElseHiddenFormula;//ifElseTextFormula;//'if ad = ewr AND if  b = b OR if c = c THEN 20:ElseIf x = x THEN 1:Else    THEN I:';
        var lst = formula.split(':');
        var ifObjects = [];
        for (var pCont = 0; pCont < lst.length; pCont++) {

            var part = lst[pCont].trim();

            if (part == '') {
                continue;
            }
            if (part.indexOf('AND') >= 0 || part.indexOf('OR') >= 0) {
                var tmpThenStmt = 'AND';
                var partAnd = part.split('AND');
                if (partAnd.length <= 0) {
                    partAnd = part.split('OR');
                    tmpThenStmt = 'OR';
                }
                if (partAnd.length > 0) {
                    for (var aCnt = 0; aCnt < partAnd.length; aCnt++) {
                        if (partAnd[aCnt] != '') {
                            if (partAnd[aCnt].indexOf('OR') > 0) {
                                var partOr = partAnd[aCnt].split('OR');
                                if (partOr.length > 0) {
                                    for (var oCnt = 0; oCnt < partOr.length; oCnt++) {
                                        if (partOr[oCnt] != '') {
                                            if (oCnt == partOr.length - 1) {
                                                ifObjects.push($formulaCreation.makeIfElseObject(partOr[oCnt], 'THEN'));
                                            }
                                            else {
                                                ifObjects.push($formulaCreation.makeIfElseObject(partOr[oCnt], 'OR'));
                                            }
                                        }
                                    }
                                }
                                else {
                                    if (partAnd[aCnt] != '') {
                                        if (aCnt == partAnd.length - 1) {
                                            ifObjects.push($formulaCreation.makeIfElseObject(partAnd[aCnt], 'THEN'));
                                        }
                                        else {
                                            ifObjects.push($formulaCreation.makeIfElseObject(partAnd[aCnt], tmpThenStmt));
                                        }
                                    }
                                }
                            }
                            else {
                                if (partAnd[aCnt] != '') {
                                    if (aCnt == partAnd.length - 1) {
                                        ifObjects.push($formulaCreation.makeIfElseObject(partAnd[aCnt], 'THEN'));
                                    }
                                    else {
                                        ifObjects.push($formulaCreation.makeIfElseObject(partAnd[aCnt], tmpThenStmt));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else {
                ifObjects.push($formulaCreation.makeIfElseObject(part, 'THEN'));
            }

        }
        $formulaCreation.totlalIfCont = 0;
        $($formulaCreation.formData).find('#dvIfElseCondition').html('');
        var isifOnly = false;

        for (var objCount = 1; objCount <= ifObjects.length; objCount++) {
            $formulaCreation.BuildIfElseSegment(isifOnly);
            isifOnly = false;
            $('#slt_' + objCount).val();
            var obj = ifObjects[objCount - 1];
            $('#slt_' + objCount).val(obj.ifStatement);
            $('#hdIfLeft_' + objCount).val(obj.leftValue);
            $('#txtIfLeft_' + objCount).val(obj.leftText);
            $('#sltOperator_' + objCount).val(obj.operator);
            $('#hdIfRight_' + objCount).val(obj.rightValue);
            $('#txtIfRight_' + objCount).val(obj.rightText);
            $('#sltThen_' + objCount).val(obj.thenStatement);
            $('#hdIfValue_' + objCount).val(obj.thenValue);
            $('#txtIfValue_' + objCount).val(obj.thenText);
            if (obj.ifStatement == 'Else') {
                $('#slt_' + objCount).trigger('change');
            }
            if (obj.thenStatement == 'AND' || obj.thenStatement == 'OR') {
                isifOnly = true;
                $('#hdIfValue_' + objCount).hide();
                $('#txtIfValue_' + objCount).hide();
            }
        }
    },
    getOpertor: function (input) {

        var strOpertr = '=';
        if (input.indexOf(">=") >= 0) {
            strOpertr = ">=";
        }
        else if (input.indexOf("<=") >= 0) {
            strOpertr = "<=";
        }
        else if (input.indexOf(">") >= 0) {
            strOpertr = ">";
        }
        else if (input.indexOf("<") >= 0) {
            strOpertr = "<";
        }
        else if (input.indexOf("!=") >= 0) {
            strOpertr = "!=";
        }
        else if (input.indexOf("=") >= 0) {
            strOpertr = "=";
        }

        return strOpertr;
    },
    makeIfElseObject: function (part, thenStmt) {

        var obj = {};

        if (part.indexOf('If') >= 0 || part.indexOf('ElseIf') >= 0) {
            if (part.indexOf('If') >= 0 && part.indexOf('ElseIf') == -1) {
                obj.ifStatement = 'If';
                part = part.replace('If', '').trim();
            }
            if (part.indexOf('ElseIf') >= 0) {
                obj.ifStatement = 'ElseIf';
                part = part.replace('ElseIf', '').trim();
            }
            obj.operator = $formulaCreation.getOpertor(part);//'=';
            var condition = part.split(obj.operator);
            obj.leftValue = condition[0].trim();
            obj.leftText = $formulaCreation.getIfElseText(condition[0].trim());
            if (thenStmt == 'THEN') {
                var rightsvals = condition[1].split('THEN');
                obj.rightValue = rightsvals[0].trim();
                obj.rightText = $formulaCreation.getIfElseText(rightsvals[0].trim());
                obj.thenValue = rightsvals[1].trim();
                obj.thenText = $formulaCreation.getIfElseText(rightsvals[1].trim());
            }
            else {
                obj.rightValue = condition[1].trim();
                obj.rightText = $formulaCreation.getIfElseText(condition[1].trim());
            }
            obj.thenStatement = thenStmt;
        }
        else if (part.indexOf('Else') >= 0) {
            obj.ifStatement = 'Else';
            part = part.replace('Else', '').trim();
            obj.thenValue = part.replace('THEN', '').trim();
            obj.thenText = $formulaCreation.getIfElseText(obj.thenValue);
            obj.thenStatement = 'THEN';
        }
        return obj;

    },

    editRange: function () {
        debugger;
        var formula = $formulaCreation.rangeHiddenFormula;//RangeTextFormula;//'from-to then value:from-to then value;
        var lst = formula.split(':');
        var ifObjects = [];
        for (var pCont = 0; pCont < lst.length; pCont++) {

            var Range = lst[pCont].trim();

            if (Range == '') {
                continue;
            }
            var data = new Object();
            if (Range.indexOf('-') >= 0) {

                var parts = Range.split('-');

                if (parts.length > 0 && parts.length <= 2) {

                    data.from = parts[0];
                    parts = parts[1].split('THEN');
                    data.to = parts[0];
                    data.then = parts[1];

                }
            }

            ifObjects.push(data);


        }
        $formulaCreation.totlalIfCont = 0;
        $($formulaCreation.formData).find('#dvRanges').html('');
        var isifOnly = false;
        for (var objCount = 1; objCount <= ifObjects.length; objCount++) {
            $formulaCreation.BuildRangeSegment();


            var obj = ifObjects[objCount - 1];


            $('#txtRangeFrom_' + objCount).val(obj.from);
            $('#txtRangeTo_' + objCount).val(obj.to);
            $('#txtRangeValue_' + objCount).val(obj.then);



        }
    },

    getIfElseText: function (tmp) {
        tmp = tmp.toUpperCase();
        do {
            if (tmp.indexOf('{') >= 0) {
                var startIndex = tmp.indexOf('{');
                //  var endIndex = tmp.indexOf('}');
                //  var id = tmp.substring(startIndex + 1, endIndex - (startIndex));
                var id = tmp.substring(startIndex + 1, (startIndex + 37));
                var name = '';
                name = $formulaCreation.getAttributeName(id);
                tmp = tmp.replace(new RegExp('{' + id + '}', 'gi'), name);
            }
        }
        while (tmp.indexOf('{') >= 0);

        return tmp;

    },
    getAttributeName: function (findId) {
        var retObject = '';
        $.each($formulaCreation.attributeModelList, function (index, item) {
            $.each(item.AttributeModelList, function (childIndex, child) {
                if (child.Id.toUpperCase() == findId.toUpperCase()) {
                    retObject = child.Name;
                }
            });
        });
        return retObject;
    },
    BuildRangeSegment: function () {


        $formulaCreation.totlalIfCont = $formulaCreation.totlalIfCont + 1;
        var condCnt = $formulaCreation.totlalIfCont;
        var readonly = condCnt == 1 ? 'readonly' : '';

        //readonly="readonly"

        ///testing
        var template = ' <div class="col-md-12" id="dvRangeCondition_' + condCnt + '">'
        + '<div class="col-md-1 paddingcustomize" id="dvRangeF' + condCnt + '">From</div>'
        + '<div class="col-md-2 paddingcustomize" id="dvleft_' + condCnt + '"><input type="hidden" id="hdRangeFrom_' + condCnt + '" /><input type="text" id="txtRangeFrom_' + condCnt + '" class="form-control hasclear" readonly/> </div>'
        + '<div class="col-md-1 paddingcustomize" id="dvRangeT' + condCnt + '">To</div>'
        + '<div class="col-md-2 paddingcustomize" id="dvRight_' + condCnt + '"><input type="hidden" id="hdRangeTo_' + condCnt + '" /><input type="text" id="txtRangeTo_' + condCnt + '" class="form-control hasclear"  placeholder="" readonly/ ></div>'
        + '<div class="col-md-1 paddingcustomize" id="dvRangeTH' + condCnt + '">Then</div>'
        + '<div class="col-md-3 paddingcustomize" id="dvIfVal_' + condCnt + '"><input type="hidden" id="hdRangeValue_' + condCnt + '" /><input type="text" id="txtRangeValue_' + condCnt + '" class="form-control hasclear"  placeholder="Enter the value" readonly/></div>'
        + '<div class="col-md-1 paddingcustomize" id=dvspanClr_"' + condCnt + '"><span id="searchclearvalue_' + condCnt + '" class="glyphicon glyphicon-refresh"  style="color:#6495ED"></span></div>'
        + '<div class="col-md-1 paddingcustomize" id=dvspanRmv_"' + condCnt + '"><span id="searchclearRemove_' + condCnt + '" class="glyphicon glyphicon-remove" style="color:#FF0000"></span></div>'
        + '</div>  <hr id="hr_' + condCnt + '" />'
        $('#dvRanges').append(template);
        $('#txtRangeFrom_' + condCnt).focus(function () {
            $formulaCreation.ifFocusedElement = 'txtRangeFrom_' + condCnt;
        });
        $('#txtRangeTo_' + condCnt).focus(function () {
            $formulaCreation.ifFocusedElement = 'txtRangeTo_' + condCnt;
        });
        $('#txtRangeValue_' + condCnt).focus(function () {
            $formulaCreation.ifFocusedElement = 'txtRangeValue_' + condCnt;
        });
        $('#txtBaseValue').focus(function () {
            $formulaCreation.ifFocusedElement = 'txtBaseValue';
        });

        $(document).ready(function () {

            $('#searchclearvalue_' + condCnt).click(function () {

                $('#txtRangeFrom_' + condCnt).val('').focus();
                $('#txtRangeTo_' + condCnt).val('').focus();
                $('#txtRangeValue_' + condCnt).val('').focus();
            });
            $('#searchclearRemove_' + condCnt).click(function () {

                $('#dvRangeCondition_' + condCnt).val('').remove();
                $('#hr_' + condCnt).val('').remove();
            });
        });
        return false;

    },


}