// created by Madhavan 22/07/2023
var $FlexiPay = {
    selectDefaultValue: function (id, text) {
        var splAnnum = parseFloat($("#annuSpl").val());
        var splCalcu = parseFloat($("#splCalcu").val());
        var perMonth = parseFloat($("#perMonthSpl").val());
        var peyearSpl = parseFloat($("#annuSpl").val());
        var result = text.toUpperCase();
        if (isNaN(perMonth)) {
            perMonth = 0;
        }
        if (isNaN(splAnnum)) {
            splAnnum = 0;
        }
        if (isNaN(splCalcu)) {
            splCalcu = 0;
        }
        if (id == "giftSelec") {
            var giftAmount = parseFloat(4500);
            if (result == "YES") {
                if (peyearSpl < giftAmount) {
                    $app.showAlert(" Gift Voucher Less Then Special Allowance =" + peyearSpl, 3);
                    return false;
                }
                $("#perAnnumGift").val(giftAmount);
                $("#annuGift").val(giftAmount);
                $("#perMonthGift").val(giftAmount / 12);
                $("#giftSpan").text("Valid Input");
                $("#giftSpan").css("background-color", "green");
                var Splvalue = splAnnum - giftAmount
                var loadSplForMonth = (Splvalue / 12).toFixed(2);
                $("#annuSpl").val(Math.round(Splvalue));
                $("#perAnnumSpl").val(Math.round(Splvalue));
                $("#perMonthSpl").val(Math.round(loadSplForMonth));
            }
            if (result == "NO") {
                $("#perAnnumGift").val("");
                $("#annuGift").val("");
                $("#perMonthGift").val("");
                var Total = splAnnum + giftAmount;
                var loadSplForMonth = (Total / 12).toFixed(2);
                $("#annuSpl").val(Math.round(Total));
                $("#perAnnumSpl").val(Math.round(Total));
                $("#perMonthSpl").val(Math.round(loadSplForMonth));
                $("#giftSpan").text("");
                $("#giftSpan").css("background-color", "lightgray");
            }
        }
        if (id == "mealSelec") {
            var meal = parseFloat(26400);
            if (result == "YES") {
                if (peyearSpl < meal) {
                    $app.showAlert(" Meal Voucher Less Then Special Allowance =" + peyearSpl, 3);
                    return false;
                }
                $("#perAnnumMeal").val(meal);
                $("#annuMeal").val(meal);
                $("#perMonthMeal").val(meal / 12);
                $("#mealSpan").text("Valid Input");
                $("#mealSpan").css("background-color", "green");
                var Splvalue = splAnnum - meal
                var loadSplForMonth = (Splvalue / 12).toFixed(2);
                $("#annuSpl").val(Math.round(Splvalue));
                $("#perAnnumSpl").val(Math.round(Splvalue));
                $("#perMonthSpl").val(Math.round(loadSplForMonth));
            }
            if (result == "NO") {
                $("#perAnnumMeal").val("");
                $("#annuMeal").val("");
                $("#perMonthMeal").val("");
                var Total = splAnnum + meal;
                var loadSplForAnnum = Total
                var loadSplForMonth = (loadSplForAnnum / 12).toFixed(2);
                $("#annuSpl").val(Math.round(loadSplForAnnum));
                $("#perAnnumSpl").val(Math.round(loadSplForAnnum));
                $("#perMonthSpl").val(Math.round(loadSplForMonth));
                $("#mealSpan").text("");
                $("#mealSpan").css("background-color", "lightgray");
            }
        }
        if (id == "convSelec") {
            var conv = parseFloat(21600);
            if (result == "YES") {
                if (peyearSpl < conv) {
                    $app.showAlert("Conveyance Less Then Special Allowance =" + peyearSpl, 3);
                    return false;
                }
                $("#perAnnumConv").val(conv);
                $("#annuConv").val(conv);
                $("#perMonthConv").val(conv / 12);
                $("#convSpan").text("Valid Input");
                $("#convSpan").css("background-color", "green");
                var displeSpl = splAnnum - conv
                $("#splCalcu").val(displeSpl);
                var loadSplForMonth = (displeSpl / 12).toFixed(2);
                $("#annuSpl").val(Math.round(displeSpl));
                $("#perAnnumSpl").val(Math.round(displeSpl));
                $("#perMonthSpl").val(Math.round(loadSplForMonth));
            }
            if (result == "NO") {
                $("#perAnnumConv").val("");
                $("#annuConv").val("");
                $("#perMonthConv").val("");
                var Total = splAnnum + conv;
                var loadSplForMonth = (Total / 12).toFixed(2);
                $("#annuSpl").val(Math.round(Total));
                $("#perAnnumSpl").val(Math.round(Total));
                $("#perMonthSpl").val(Math.round(loadSplForMonth));
                $("#convSpan").text("");
                $("#convSpan").css("background-color", "lightgray");
            }
        }
        if (id == "eduSelec") {
            var edu = parseFloat(2400);
            if (result == "YES") {
                if (peyearSpl < edu) {
                    $app.showAlert("Conveyance Less Then Special Allowance =" + peyearSpl, 3);
                    return false;
                }
                $("#perAnnumEdu").val(edu);
                $("#annuEdu").val(edu);
                $("#perMonthEdu").val(edu / 12);
                $("#eduSpan").text("Valid Input");
                $("#eduSpan").css("background-color", "green");
                var displeSpl = splAnnum - edu
                $("#splCalcu").val(displeSpl);
                var loadSplForMonth = (displeSpl / 12).toFixed(2);
                $("#annuSpl").val(Math.round(displeSpl));
                $("#perAnnumSpl").val(Math.round(displeSpl));
                $("#perMonthSpl").val(Math.round(loadSplForMonth));
            }
            if (result == "NO") {
                $("#perAnnumEdu").val("");
                $("#annuEdu").val("");
                $("#perMonthEdu").val("");
                var Total = splAnnum + edu;
                var loadSplForAnnum = Total
                var loadSplForMonth = (Total / 12).toFixed(2);
                $("#annuSpl").val(Math.round(loadSplForAnnum));
                $("#perAnnumSpl").val(Math.round(loadSplForAnnum));
                $("#perMonthSpl").val(Math.round(loadSplForMonth));
                $("#eduSpan").text("");
                $("#eduSpan").css("background-color", "lightgray");
            }
        }
    },
    calcuFlexi: function () {
        var Flaxi = Math.floor($("#flexible").val());
        var edu = Math.floor($("#perMonthEdu").val());
        var spl = Math.floor($("#perMonthSpl").val());
        var tele = Math.floor($("#perMonthTele").val());
        var nps = Math.floor($("#perMonthNps").val());
        var conv = Math.floor($("#perMonthConv").val());
        var meal = Math.floor($("#perMonthMeal").val());
        var gift = Math.floor($("#perMonthGift").val());
        var lta = Math.floor($("#perMonthLta").val());
        var hra = Math.floor($("#perMonthHra").val());
        if (!isNaN(edu) && edu !== 0) {
            $("#perAnnumEdu").val(Math.floor(edu * 12));
            $("#annuEdu").val(Math.floor(edu * 12));
            if (edu != 0) {
                $("#eduSelec").val("yes");
            }
        }
        if (Flaxi !== 0) {

        }
        else if (!isNaN(spl) && spl !== 0) {
            $("#perAnnumSpl").val(Math.floor(spl * 12));
            $("#annuSpl").val(Math.floor(spl * 12));
            if (spl != 0) {
                $("#splSelec").val("yes");
            }
        }
        if (!isNaN(tele) && tele !== 0) {
            $("#perAnnumTele").val(Math.floor(tele * 12));
            $("#annuTele").val(Math.floor(tele * 12));
            if (tele != 0) {
                $("#teleSelec").val("yes");
                $("#annuTele").prop("disabled", true);
            }
        }
        if (!isNaN(nps) && nps !== 0) {
            $("#perAnnumNps").val(Math.floor(nps * 12));
            $("#annumNps").val(Math.floor(nps * 12));
            if (nps != 0) {
                $("#npsSelec").val("yes");
                $("#annumNps").prop("disabled", true);
            }
        }
        if (!isNaN(conv) && conv !== 0) {
            $("#perAnnumConv").val(Math.floor(conv * 12));
            $("#annuConv").val(Math.floor(conv * 12));
            if (conv != 0) {
                $("#convSelec").val("yes")
            }
        }
        if (!isNaN(meal) && meal !== 0) {
            $("#perAnnumMeal").val(Math.floor(meal * 12));
            $("#annuMeal").val(Math.floor(meal * 12));
            if (meal != 0) {
                $("#mealSelec").val("yes");
            }
        }
        if (!isNaN(gift) && gift !== 0) {
            $("#perAnnumGift").val(Math.floor(gift * 12));
            $("#annuGift").val(Math.floor(gift * 12));
            if (gift != 0) {
                $("#giftSelec").val("yes");
            }
        }
        if (!isNaN(lta) && lta !== 0) {
            $("#perAnnumLta").val(Math.floor(lta * 12));
            $("#annuLta").val(Math.floor(lta * 12));
            if (lta != 0) {
                $("#ltaSelec").val("yes");
                $("#annuLta").prop("disabled", true);
            }
        }
        if (!isNaN(hra) && hra !== 0) {
            $("#perAnnumHra").val(Math.floor(hra * 12));
            $("#annuHra").val(Math.floor(hra * 12));
            if (hra) {
                $("#hraSelec").val("yes");
                $("#annuHra").prop("disabled", true);
            }
        }
    },
    //onKeyup function Use to SalaryMaster html
    checkValue: function (event, id) {


        var inputValue = $('#' + id).val();
        var pattern = /^[0-9]+$/; // Regular expression to allow only numbers

        if (!pattern.test(inputValue)) {
            // If the input contains non-numeric characters, prevent them
            var result = inputValue.replace(/[^0-9]+/g, '');
            $('#' + id).val(result);
        }


        var splValu = parseFloat($("#annuSpl").val());
        var splAnnumValu = splValu.toFixed(2);
        var value = parseFloat($("#" + id).val());
        var basicForHra = parseFloat($("#basic").val()) * 0.5;
        var basicForNps = parseFloat($("#basic").val()) * 0.1;
        var basicForLta = parseFloat($("#basic").val()) / 12;
        var basicForTelec = parseFloat($("#basic").val()) * 0.1;
        debugger
        if (id === "annuHra") {
            if (($("#hraSelec").val()) == "yes") {
                if (splAnnumValu < value) {
                    $app.showAlert("Maximum  of HRA Which ever is less Then=" + splAnnumValu, 3);
                    $("#" + id).val("");
                }
                $("#perAnnumSpl").val();

                if (isNaN(value)) {

                    $("#hraSpan").text("");
                    $("#hraSpan").css("background-color", "lightgray");
                } else if (value > basicForHra) {
                    $app.showAlert("Input value is Less than 50% of Basic Salary=" + basicForHra, 3);
                    $("#hraSpan").text("Invalid Input");
                    $("#hraSpan").css("background-color", "red");
                } else if (value < basicForHra) {
                    $("#hraSpan").text("Valid Input");
                    $("#hraSpan").css("background-color", "green");
                } else if (value == basicForHra) {
                    $("#hraSpan").text("Valid Input");
                    $("#hraSpan").css("background-color", "green");
                }
                else if (value == "") {
                    $("#hraSpan").text("");
                    $("#hraSpan").css("background-color", "lightgray");
                }
            }
            else {
                $app.showAlert("Please Select Yes", 3)
                $("#" + id).val("");
            }
        }
        if (id === "annumNps") {
            if (($("#npsSelec").val() == "yes")) {
                if (splAnnumValu < value) {
                    $app.showAlert("Maximum  of NPS Which ever is less Then=" + splAnnumValu, 3);
                    $("#" + id).val("");
                }

                if (isNaN(value)) {

                    $("#npsSpan").text("");
                    $("#npsSpan").css("background-color", "lightgray");
                } else if (value > basicForNps) {
                    $app.showAlert("10% of Basic Salary '" + basicForNps + "' or Balance in Special Allowance whichever is less", 3);
                    $("#npsSpan").text("Invalid Input");
                    $("#npsSpan").css("background-color", "red");
                } else if (value < basicForNps) {

                    $("#npsSpan").text("Valid Input");
                    $("#npsSpan").css("background-color", "green");
                }
                else if (value > 750000) {
                    $app.showAlert("Maximum  of NPS Which ever is less Then= 750000", 3);
                    $("#" + id).val("");
                }
                else if (value === basicForNps) {
                    $("#npsSpan").text("Valid Input");
                    $("#npsSpan").css("background-color", "green");
                }
                else if (value == "") {

                    $("#npsSpan").text(" ");
                    $("#npsSpan").css("background-color", "lightgray");
                }
            }
            else {
                $app.showAlert("Please Select Yes", 3)
                $("#" + id).val("");
            }
        }
        if (id === "annuLta") {
            if (($("#ltaSelec").val()) == "yes") {
                if (value > splAnnumValu) {
                    $app.showAlert("Maximum  of NPS Which ever is less Then=" + splAnnumValu, 3);
                    $("#" + id).val("");
                }

                if (isNaN(value)) {

                    $("#ltaSpan").text("");
                    $("#ltaSpan").css("background-color", "lightgray");
                } else if (value > basicForLta) {
                    $app.showAlert("Less Than 1 Month Basic Salary=" + basicForLta, 3);
                    $("#ltaSpan").text("Invalid Input");
                    $("#ltaSpan").css("background-color", "red");
                } else if (value < basicForLta) {
                    $("#ltaSpan").text("Valid Input");
                    $("#ltaSpan").css("background-color", "green");
                }
                else if (value == basicForLta) {
                    $("#ltaSpan").text("Valid Input");
                    $("#ltaSpan").css("background-color", "green");
                }
                else if (value == "") {
                    $("#ltaSpan").text(" ");
                    $("#ltaSpan").css("background-color", "lightgray");
                }
            }
            else {
                $app.showAlert("Please Select Yes", 3)
                $("#" + id).val("");
            }
        }
        if (id === "annuTele") {
            if (($("#teleSelec").val()) == "yes") {
                if (splAnnumValu < value) {
                    $app.showAlert("Maximum  of NPS Which ever is less Then=" + splAnnumValu, 3);
                    $("#" + id).val("");
                }
                debugger
                if (isNaN(value)) {

                    $("#teleSpan").text("");
                    $("#teleSpan").css("background-color", "lightgray");
                } else if (value > 60000 || value > basicForTelec) {
                    $app.showAlert("Maximum of 60000 INR Per Annum or 10% of Basic Salary Which ever is less=" + basicForTelec, 3);
                    $("#teleSpan").text("Invalid Input");
                    $("#teleSpan").css("background-color", "red");
                }
                else if (value < basicForTelec && value < 60000) {
                    $("#teleSpan").text("Valid Input");
                    $("#teleSpan").css("background-color", "green");
                }
                else if (value === basicForTelec) {
                    $("#teleSpan").text("Valid Input");
                    $("#teleSpan").css("background-color", "green");
                }
                else if (value == "") {
                    $("#teleSpan").text(" ");
                    $("#teleSpan").css("background-color", "lightgray");
                }
            }
            else {
                $app.showAlert("Please Select Yes", 3)
                $("#" + id).val("");
            }
        }


        

    }
    ,
    //onChange Event Use to SalaryMaster html
    changedval: function (id, event) {
        debugger;
        var value = Math.floor($("#" + id).val());
        var splvalu = Math.floor($("#perAnnumSpl").val());
        if (!isNaN(value)) {
            var total = Math.floor(splvalu - value);
            if (splvalu < value) {

            }
            $("#perAnnumSpl").val(Math.round(total));
            $("#perMonthSpl").val(Math.round(total / 12));

            if (id === "annuHra") {
                if ($("#hraSpan").text() === "Invalid Input") {
                    $("#hraSpan").text("");
                    $("#annuHra").val("");
                    return false;
                }
                $("#perMonthHra").val(Math.round(value / 12));
                $("#perAnnumHra").val(Math.round(value));
                $("#annuSpl").val(Math.round(total));
                $("#" + id).prop("disabled", true);
            }
            if (id === "annuLta") {
                if ($("#ltaSpan").text() === "Invalid Input") {
                    $("#ltaSpan").text("");
                    $("#annuLta").val("");
                    return false;
                }
                $("#perMonthLta").val(Math.round(value / 12));
                $("#perAnnumLta").val(Math.round(value));
                $("#annuSpl").val(Math.round(total));
                $("#" + id).prop("disabled", true);
            }
            if (id === "annumNps") {
                if ($("#npsSpan").text() === "Invalid Input") {
                    $("#npsSpan").text("");
                    $("#annumNps").val("");
                    return false;
                }
                $("#perMonthNps").val(Math.round(value / 12));
                $("#perAnnumNps").val(Math.round(value));
                $("#annuSpl").val(Math.round(total));
                $("#" + id).prop("disabled", true);
            }
            if (id === "annuTele") {
                if ($("#teleSpan").text() === "Invalid Input") {
                    $("#teleSpan").text("");
                    $("#annuTele").val("");
                    return false;
                }
                $("#perMonthTele").val(Math.round(value / 12));
                $("#perAnnumTele").val(Math.round(value));
                $("#annuSpl").val(Math.round(total));
                $("#" + id).prop("disabled", true);
            }
        }
        else {
        }
    },
    saveCheck: function () {
        var isValid = true; // Initialize a flag variable
        $(".validation-check").each(function () {
            var validLabel = $(this);
            if (validLabel.text() === "Invalid Input") {
                isValid = false; // Set the flag to false if any label has "Invalid Input"
                return false; // Exit the loop early since we found an invalid input
            }
        });
        if (isValid) {
            return true;
            // All labels are valid
        } else {
            // At least one label is invalid
        }

    },
    saveFlexiPay: function () {
        debugger
        var checkval = this.saveCheck();
        var valuesArray = [];
        var contex = $("#annuSpl").val();
        $("#flexible").val("0");
        $("[data-id]").each(function () {
            var input = $(this);
            if (input[0].tagName === "INPUT") {
                if (input[0].value == "") {
                    input[0].value = "0";
                }
                valuesArray.push({
                    Id: input.attr("data-id"),
                    value: input[0].value,
                    AttributeModelId: input.attr("data-id"),
                });
            }
        });
        valuesArray.push({
            Id: $("#basicMonth").attr("data-id"),
            value: $("#basicMonth").val(),
            AttributeModelId: $("#basicMonth").attr("data-id"),
        })
        var formData = {
            entityId: $("#tblFlexiPay").attr("data-entity"),
            entityModelId: $("#tblFlexiPay").attr("data-modelid"),
            EntityKeyValues: valuesArray
        };
        var refEntity = $("form").attr("id");
        var refEntityModel = "Employee";
        if (checkval) {
            $app.showAlert("Are You Sure Do you want Save", 1);
            $.ajax({
                url: $app.baseUrl + "Entity/SaveEntityFlexValue",
                data: JSON.stringify({ dataValue: formData, refEntityId: refEntity, refEntityModel: refEntityModel, saveMappingId: null }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    if (jsonResult.Status) {
                        $app.showAlert("Saved Successfuly ", 2)
                        $FlexiPay.calcuNetPay();
                        $("#btnSaveFlexi").hide();
                        $("#btnEditFlexi").show();
                        $(".opting-select").prop("disabled", true);
                        $('#annuHra').prop("disabled", true);
                        $('#annuLta').prop("disabled", true);
                        $('#annumNps').prop("disabled", true);
                        $('#annuTele').prop("disabled", true);
                    }
                    else {
                        $app.showAlert(jsonResult.Message, 4);
                    }
                },
                error: function () {

                }
            })
        }
        else {
            $app.showAlert("Some Invalid Input ", 4);
        }
    },
    calcuNetPay: function () {
        debugger
        $("#netSalaryCalcu").show();
        // Retrieve input values
        var basicSalary = isNaN(parseFloat($("#basic").val())) ? 0 : parseFloat($("#basic").val());
        var perMonthNps = isNaN(parseFloat($("#perMonthNps").val())) ? 0 : parseFloat($("#perMonthNps").val());
        var perMonthMeal = isNaN(parseFloat($("#perMonthMeal").val())) ? 0 : parseFloat($("#perMonthMeal").val());
        var perMonthGift = isNaN(parseFloat($("#perMonthGift").val())) ? 0 : parseFloat($("#perMonthGift").val());
        var perMonthHra = isNaN(parseFloat($("#perMonthHra").val())) ? 0 : parseFloat($("#perMonthHra").val());
        var perMonthLta = isNaN(parseFloat($("#perMonthLta").val())) ? 0 : parseFloat($("#perMonthLta").val());
        var perMonthTele = isNaN(parseFloat($("#perMonthTele").val())) ? 0 : parseFloat($("#perMonthTele").val());
        var perMonthEdu = isNaN(parseFloat($("#perMonthEdu").val())) ? 0 : parseFloat($("#perMonthEdu").val());
        var perMonthConv = isNaN(parseFloat($("#perMonthConv").val())) ? 0 : parseFloat($("#perMonthConv").val());
        var perMonthSpl = isNaN(parseFloat($("#perMonthSpl").val())) ? 0 : parseFloat($("#perMonthSpl").val());
        // Calculate deductions
        var basicSalaryMonth = Math.ceil(basicSalary / 12);
        var pfDeduction = (basicSalaryMonth) * 0.12
        var profTaxDeduction = Math.ceil(2500 / 12);
        var totalDeduction = pfDeduction + perMonthMeal + perMonthGift + perMonthNps + profTaxDeduction + 20;
        // Calculate earnings
        var earningGross = perMonthNps + perMonthMeal + perMonthGift + basicSalaryMonth +
            perMonthHra + perMonthLta + perMonthTele + perMonthEdu + perMonthConv + perMonthSpl;
        ;
        // Calculate net pay
        var netPay = earningGross - totalDeduction
        $("#netPay").val(Math.ceil(netPay));
        $("#PTAX").val(Math.ceil(profTaxDeduction));
        $("#PFtxt").val(Math.ceil(pfDeduction));
        $("#erGross").val(Math.ceil(earningGross));
        $("#totalDedcu").val(Math.ceil(totalDeduction));
        $("#txtGift").val(Math.ceil(perMonthGift));
        $("#txtMeal").val(Math.ceil(perMonthMeal));
        $("#txtNps").val(Math.ceil(perMonthNps));
    },
    loadDataCheck: function () {
        if ($('#convSelec').val() == "yes") {
            $("#convSpan").text("Valid Input");
            $("#convSpan").css("background-color", "green");
        }
        if ($("#giftSelec").val() == "yes") {
            $("#giftSpan").text("Valid Input");
            $("#giftSpan").css("background-color", "green");
        }
        if ($("#mealSelec").val = "yes") {
            $("#mealSpan").text("Valid Input");
            $("#mealSpan").css("background-color", "green");
        }
        if ($("#eduSelec").val = "yes") {
            $("#eduSpan").text("Valid Input");
            $("#eduSpan").css("background-color", "green");
        }

        if ($("#annuHra").val() != "" && $("#annuHra").val() != "0.00") {
            $("#hraSpan").text("Valid Input");
            $("#hraSpan").css("background-color", "green");
        }
        if ($("#annuLta").val() != "" && $("#annuLta").val() != "0.00") {
            $("#ltaSpan").text("Valid Input");
            $("#ltaSpan").css("background-color", "green");
        }
        if ($("#annuTele").val() != "" && $("#annuTele").val() != "0.00") {
            $("#teleSpan").text("Valid Input");
            $("#teleSpan").css("background-color", "green");
        }
        if ($("#annumNps").val() != "" && $("#annumNps").val() != "0.00") {
            $("#npsSpan").text("Valid Input");
            $("#npsSpan").css("background-color", "green");
        }
    },
    changeInput: function (sid, cotext) {
        debugger
        var selectedval = cotext.toUpperCase();
        var spl = isNaN(parseFloat($("#annuSpl").val())) ? 0 : parseFloat($("#annuSpl").val());
        if (sid == "npsSelec") {
            if (selectedval == "NO") {
                var nps = parseFloat($("#annumNps").val());
                if (!isNaN(nps) && !isNaN(spl)) {
                    var total = spl + nps;
                    $("#annuSpl").val(total);
                    $("#perMonthSpl").val(Math.round((total / 12).toFixed(2)));
                    $("#perAnnumSpl").val(total);
                    $("#annumNps").prop("disabled", false);
                    $("#npsSpan").text("");
                    $("#npsSpan").css("background-color", "lightgray");
                    $("#annumNps").val("");
                    $("#perMonthNps").val("");
                    $("#perAnnumNps").val("");
                }
            }
        }
        if (sid == "hraSelec") {

            if (selectedval == "NO") {
                var hra = parseFloat($("#annuHra").val());
                if (!isNaN(hra) && !isNaN(spl)) {
                    var total = spl + hra;
                    $("#annuSpl").val(total);
                    $("#perMonthSpl").val(Math.round((total / 12).toFixed(2)));
                    $("#perAnnumSpl").val(total);
                    $("#annuHra").prop("disabled", false);
                    $("#hraSpan").text("");
                    $("#hraSpan").css("background-color", "lightgray");
                    $("#annuHra").val("");
                    $("#perMonthHra").val("");
                    $("#perAnnumHra").val("");
                }
            }
        }
        if (sid == "teleSelec") {

            if (selectedval == "NO") {
                var tele = parseFloat($("#annuTele").val());
                if (!isNaN(tele) && !isNaN(spl)) {
                    var total = spl + tele;
                    $("#annuSpl").val(total);
                    $("#perMonthSpl").val(Math.round((total / 12).toFixed(2)));
                    $("#perAnnumSpl").val(total);
                    $("#annuTele").prop("disabled", false);
                    $("#teleSpan").text("");
                    $("#teleSpan").css("background-color", "lightgray");
                    $("#perMonthTele").val("");
                    $("#annuTele").val("");
                    $("#perAnnumTele").val("");
                }
            }
        }
        if (sid == "ltaSelec") {
            if (selectedval == "NO") {
                var lta = parseFloat($("#annuLta").val());
                if (!isNaN(lta) && !isNaN(spl)) {
                    var total = spl + lta;
                    $("#annuSpl").val(total);
                    $("#perMonthSpl").val(Math.round((total / 12).toFixed(2)));
                    $("#perAnnumSpl").val(total);
                    $("#annuLta").prop("disabled", false);
                    $("#annuLta").val("");
                    $("#ltaSpan").text("");
                    $("#ltaSpan").css("background-color", "lightgray");
                    $("#perAnnumLta").val("");
                    $("#perMonthLta").val("");
                }
            }
        }
    },
    LoadData: function () {
        debugger;
        var valuesArray = [];

        $("#tblFlexiPay [data-id]").each(function () {
            var dataId = $(this).data("id");
            valuesArray.push(dataId);
        });
        $.ajax({
            url: $app.baseUrl + "Entity/LoadFlexiPay",
            type: "POST",
            data: JSON.stringify({ Component: valuesArray }),
            dataType: "json",
            contentType: "application/json",
            success: function (jsonResult) {
                debugger;
                var responseData = jsonResult.result;
                responseData.forEach(function (item) {
                    var dataId = item.AttributeModelId.toString();
                    var value = Math.floor(item.Value);
                    var $targetInput = $("#tblFlexiPay [data-id='" + dataId + "']");

                    if (value !== "0.00" && value !== 0 && value !== "") {
                        // Enable the "select yes" option
                        var $selectElement = $targetInput.closest("tr").find(".opting-select");
                        $selectElement.val("yes");
                        // Update the input fields
                        $targetInput.val(value);
                    }
                });
                var a = $("#annuSpl").val();
                $FlexiPay.calcuFlexi();
                $FlexiPay.calcuNetPay();
                $("#tblFlexiPay").attr("data-entity", responseData[0].EntityId);
                $("#tblFlexiPay").attr("data-modelid", responseData[0].EntityModelId);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error(errorThrown);
            }
        })
    },
    editFlexiPay: function () {
        $(".opting-select").prop("disabled", false);

        debugger
        $("#splSelec").prop("disabled", true);
        $("#btnSaveFlexi").removeAttr("style");
    }
}
