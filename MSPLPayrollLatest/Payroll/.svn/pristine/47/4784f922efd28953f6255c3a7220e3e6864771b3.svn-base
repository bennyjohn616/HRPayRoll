var $validator = {

    alphanumericonly: function (e) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }

        e.preventDefault();
        return false;
    },
    //created by mubarak
    //In order to validate satae,city,country text box in company creation.
    alphaonly: function (e) {

        var regex = new RegExp("^[a-zA-Z ]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }

        e.preventDefault();
        return false;
    },
    fnValidatePAN: function (Obj) {
        if (Obj == null) Obj = window.event.srcElement;
        if (Obj.value != "") {
            ObjVal = Obj.value.toUpperCase();
            var panPat = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
            var code = /([P])/;
            var code_chk = ObjVal.substring(3, 4);
            if (ObjVal.search(panPat) == -1) {
                $app.showAlert("First 5 digits should be alphabets,\n Next 4 digits should be number,\n Last 1 digit should be alphabet,Length should be 10 Digits", 4);
                Obj.focus();
                return false;
            }
            if (code.test(code_chk) == false) {
                $app.showAlert("Invaild PAN Card No.Fourth Letter in PAN card number must be 'P'", 4);
                Obj.focus();
                return false;
            }
        }
    },

    

    // fnValidatePAN:function(Obj) {
    //     if (Obj == null) Obj = window.event.srcElement;
    //     if (Obj.value != "") {
    //         ObjVal = Obj.value;
    //         var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
    //         var code = /([C,P,H,F,A,T,B,L,J,G])/;
    //         var code_chk = ObjVal.substring(3,4);
    //         if (ObjVal.search(panPat) == -1) {             
    //             $app.showAlert('First 5 digits shall be alphabets,Next 4 digits shall be number,Last 1 digit shall be alphabet,Length should be 10 Digits', 4);
    //             Obj.val('');
    //             Obj.focus();          


    //             return false;
    //         }
    //         if (code.test(code_chk) == false) {
    //             $app.showAlert('Invaild PAN Card No', 4);                
    //             return false;
    //         }
    //     }
    //},
    pastePhoneNum: function (obj) {

        setTimeout(function () {

            var totalCharacterCount = obj.value;
            var strValidChars = "+0123456789";
            var strChar;
            var FilteredChars = "";
            for (i = 0; i < totalCharacterCount.length; i++) {
                strChar = totalCharacterCount.charAt(i);
                if (totalCharacterCount.charAt(i + 1) == "+") {
                    obj.value = "";
                    return false;
                }
                if (strValidChars.indexOf(strChar) != -1) {
                    FilteredChars = FilteredChars + strChar;
                }
            }
            obj.value = FilteredChars;
            return false;
        }, 100);

    },
    pasteNum: function (obj) {

        setTimeout(function () {

            var totalCharacterCount = obj.value;
            var strValidChars = "0123456789";
            var strChar;
            var FilteredChars = "";
            for (i = 0; i < totalCharacterCount.length; i++) {
                strChar = totalCharacterCount.charAt(i);
                if (strValidChars.indexOf(strChar) != -1) {
                    FilteredChars = FilteredChars + strChar;
                }
            }
            obj.value = FilteredChars;
            return false;
        }, 100);

    },
    IsNumeric: function (e, ID) {

        var keyCode = e.which ? e.which : e.keyCode
        value = document.getElementById(ID).value + String.fromCharCode(keyCode);
        if (keyCode >= 48 && keyCode <= 57) {

        }
        else
            return false;
    },
    IsPhoneCompanyCreate: function (e, ID) {


        var keyCode = e.which ? e.which : e.keyCode
        value = document.getElementById(ID).value + String.fromCharCode(keyCode);
        if (keyCode == 43) {
            if (value == "+") {

            }
            else {
                return false;
            }
        }
        else if (keyCode >= 47 && keyCode <= 57) {

        }
        else
            return false;
    },
    IsPhone: function (e, ID) {


        var keyCode = e.which ? e.which : e.keyCode
        value = document.getElementById(ID).value + String.fromCharCode(keyCode);
        if (keyCode == 43) {
            if (value == "+") {

            }
            else {
                return false;
            }
        }
        else if (keyCode >= 48 && keyCode <= 57) {

        }
        else
            return false;
    },


    //Validphone: function (e, ID) {
    //    
    //    //var keyCode = e.which ? e.which : e.keyCode
    //    inputtxt = document.getElementById(ID).value;
    //    var phoneno =  /^\+?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$/;  
    //    if((inputtxt.value.match(phoneno))  
    //    {  
    //        return true;  
    //    }  
    //    else  
    //    {  
    //        alert("Please enter valid phone number");  
    //        return false;  
    //    }  
    //    //if (keyCode >= 48 && keyCode <= 57) {

    //    //}
    //    //else
    //    //    return false;
    //},
    //Age: function (age) {

    //    var keyCode = e.which ? e.which : e.keyCode
    //    value = document.getElementById(age).value + String.fromCharCode(keyCode);
    //    if (keyCode >= 48 && keyCode <= 57) {
    //        if (this.age >= 100) {

    //        }

    //    }
    //    else

    //        return false;
    //},

    checkEmail: function (id) {

        var email = document.getElementById(id);
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

        if (!filter.test(email.value)) {
            $app.showAlert('Please provide a valid email address', 3);
            //alert('Please provide a valid email address');
            email.focus;
            return false;
        }
    },
    moneyvalidation: function (target) {

        var regex = /^\d{0,10}(\.\d{0,2})?$/;
        if (!regex.test($('#' + target).val())) {
            $('#' + target).val('');
            $app.showAlert('Please provide valid amount and provide two digit after a decimal point', 3);
            //alert("please provide valid amount and provide two digit after a decimal point");

        }
        //else {
        //    $this.css("border-color", "#FF0000");
        //}


    },
    Age: function (id) {
        var age = document.getElementById(id).value;
        if (age === "") {
            return true;
        }

        // convert age to a number
        age = parseInt(age, 10);
        if (isNaN(age)) {
            document.getElementById(id).value = '';
            return false;

            //}
            //check if age is a number or less than 1 or greater than 100
            if (isNaN(age) || age < 1 || age > 100) {
                document.getElementById(id).value = '';
                $app.showAlert('The age must be a number between 1 and 100', 3);
                return false;
            }
        }
    },

    minmax: function (id) {

        var idval = document.getElementById(id);
        var value = $('#' + id).val()
        var min = 0;
        var max = 31;
        if (parseInt(value) < min || isNaN(parseInt(value))) {
            document.getElementById(id).value = '';
            $app.showAlert('The days must be a number between 0 and 31', 3);
            return false;
        }
        else if (parseInt(value) > max) {
            document.getElementById(id).value = '';
            $app.showAlert('The days must be a number between 0 and 31', 3);
            return false;
        }
        else return true;
    },

    checkDecimal: function (evt, Digcount) {

        var e = window.event || evt;
        var charCode = e.which || e.keyCode;

        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            var parts = evt.target.value.split('.');
            if (parts.length == 1 && charCode == 46) {
                return true;
            } else if (charCode >= 37 && charCode <= 40) {
                return true;
            }
            else {
                if (charCode == 45) {

                    if (evt.target.value == "") {
                        return true;
                    } else {
                        return false;
                    }
                } else {

                    return false;
                }
            }
        }
        var period = evt.target.value.indexOf('.');
        if (period != -1) {
            var parts = evt.target.value.split('.');
            var Count = parts[1].length;
            var CharPos = evt.target.selectionStart;
            var dotPos = evt.target.value.indexOf(".");
            if (CharPos > dotPos && dotPos > -1 && (parts[1].length >= Digcount)) {
                if (charCode == 8 || charCode == 46) {
                    return true;
                } else {
                    return false;
                }
            }
            if (Digcount == 1) {
                if (charCode == 53 || charCode == 48)
                    return true;
                else
                    return false;
            }
        }
        return true;
    },
    CheckNegativedecimal: function (evt, Digcount) {

        var e = window.event || evt;
        var charCode = e.which || e.keyCode;
        if (charCode == 40 || charCode == 38 || charCode == 37 || charCode == 39) {
            return false;
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            var parts = evt.target.value.split('.');
            if (parts.length == 1 && charCode == 46) {
                return true;
            } else if (charCode >= 37 && charCode <= 40) {
                return true;
            }
            else {
                if (charCode == 45) {
                    return false;
                    //Comment by mubarak inorder to restrict minus(-) symbol typing in between the content.
                    //if (evt.target.value == "") {
                    //        return false;
                    //} 
                } else {

                    return false;
                }
            }
        }
        var period = evt.target.value.indexOf('.');
        if (period != -1) {
            var parts = evt.target.value.split('.');
            var Count = parts[1].length;
            var CharPos = evt.target.selectionStart;
            var dotPos = evt.target.value.indexOf(".");
            if (CharPos > dotPos && dotPos > -1 && (parts[1].length >= Digcount)) {
                if (charCode == 8 || charCode == 46) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        return true;
    },
    Percentagevalidate: function (id) {
        debugger;
        var returnval = true;
        var idval = document.getElementById(id);
        var x = $('#' + id).val()
        var parts = x.split(".");
        if (typeof parts[1] == "string" && (parts[1].length > 2))
            returnval= false;
        var n = parseFloat(x);
        if (isNaN(n))
            returnval= false;
        if (n < 0 || n > 100)
            returnval = false;

        var tem = (returnval == false ? '0' : x);
        $('#' + id).val(tem);
        return returnval;
    }
};
//function myFunction() {
//    var d = new Date("10.10.1988");
//    var T = new Date();
//    var n = (T.getFullYear() - d.getFullYear());
//    document.getElementById("demo").innerHTML = n;
//}
//$(function()
//{
//    $("#txtConfirmationPeriodfrom").datepicker(
//    {
//        minDate: new Date(),
//        changeMonth: true,
//        numberOfMonths: 1,
//        onClose: function( selectedDate )
//        {
//            $("#txtConfirmationPeriodto").datepicker("option", "minDate", selectedDate);
//        }
//    });

//    $("#txtConfirmationPeriodto").datepicker(
//    {
//        minDate: new Date(),
//        changeMonth: true,
//        numberOfMonths: 1,
//        onClose: function( selectedDate )
//        {
//            $("#txtConfirmationPeriodfrom").datepicker("option", "maxDate", selectedDate);
//        }
//    });
//});

