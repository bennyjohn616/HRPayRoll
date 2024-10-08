﻿$('#btnRequest').on('click', function () {

    if ($('#txtUserForget').val().trim() != "") {
        $login.forgetPassword();
    }
    else {
        $app.showAlert("Please Enter The Email ID", 4);
        $('#txtUserForget').focus();
    }
});
$('#txtCurPass').change(function () {

    if ($('#txtCurPass').val != "") {
        $login.ForgetPasswordCheck();
    }
});

$("#txtNewPassword").change(function () {

    var pwdvalue = $('#txtNewPassword').val();
    var paswd = /^(?=.*[0-9])(?=.*[A-Za-z])(?=.*[!@#$%&*.])[a-zA-Z0-9!@#$%^&*.]{6,12}$/;
    if (pwdvalue != "") {
        if (pwdvalue.match(paswd)) {

            return true;
        }
        else {
            $app.showAlert("Your password should be contain atleast length 6 to 12 Characters , one numeric digit and a special character  ", 4);
            $("#txtNewPassword").val('');
            $('#txtNewPassword').focus();
            return false;
        }
    } else {
        return true;
    }
});
//$("#txtConfirmPassword").change(function () {
//    
//    //var newpassword = $('#txtNewPassword').val();
//    //var confirmpass = $('#txtConfirmPassword').val();
//    //if(newpassword!=confirmpass)
//    //{
//    //    $app.showAlert("Your Password does not match with New Password", 4);
//    //    $("#txtConfirmPassword").val('');
//    //    $('#txtConfirmPassword').focus();
//    //    return false;
//    //}
//    //else
//    //{
//    //    return true;
//    //}

//});
$('#btnChangePass').on('click', function () {

    var err = 0;
    var newpassword = $('#txtNewPassword').val();
    var confirmpass = $('#txtConfirmPassword').val();
    var oldpassword = $('#txtCurPass').val();

    $(".Reqrd").each(function () {


        if (this.id == "txtCurPass" || this.id == "txtNewPassword" || this.id == "txtConfirmPassword") {
            if ($('#' + this.id).val() == "") {
                $app.showAlert('Please Enter ' + $(this).attr('placeholder'), 4);
                err = 1;
                $('#' + this.id).focus();
                return false;
            }
        }

    });

    if (newpassword != confirmpass) {
        $app.showAlert("Your Password does not match with New Password", 4);
        err = 1;
        $("#txtConfirmPassword").val('');
        $('#txtConfirmPassword').focus();
        return false;
    }
    if (err == 0) {
        $login.changePassword();
        return true;
    }
});


var $login = {
    doLogin: function () {
        
        if ($("#txtUsername").val() == "" || $("#txtPassword").val() == "") {
            return false;
        }
        else {
            var usr = { UserName: $("#txtUsername").val(), Password: $("#txtPassword").val() };
            $.ajax({
                type: 'POST',
                url: $app.baseUrl + 'Login/ValidateLogin',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ user: usr }),
                // dataType: 'json',
                success: function (data) {
                   
                    if (data.success) { 
                        window.location.href = data.result;
                        debugger
                    }
                    else {
                        $('#dvError').removeClass('nodisp');
                        $('#lblError').text(data.result);
                    }
                },
                error: function (data) {
                    $('#dvError').removeClass('nodisp');
                    $('#lblError').text('There is some error.Please try again later.');
                }
            });
        }
    },
    formload: function () {

        
        var BrowserResult = '';
        var is_chrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + 'Login/BrowserDetails',
            contentType: 'application/json; charset=utf-8',
            data: null,
            async: false,
            // dataType: 'json',
            success: function (data) {
                BrowserResult = data.result[0];
                
            },
            error: function (data) {
                $('#dvError').removeClass('nodisp');
                $('#lblError').text('There is some error.Please try again later.');
                $account.isAuthenticated = false;
            }
        });
        if (BrowserResult != 'Chrome') {
            if (BrowserResult != 'Firefox') {
                alert("sorry for inconvenience please use Chrome Browser")
                $(".login-box").addClass('nodisp');
                $(".Browser-support").removeClass('nodisp')
            }
            else {
                $(".login-box").removeClass('nodisp');
                $(".Browser-support").addClass('nodisp')
            }
        } else {
            $(".login-box").removeClass('nodisp');
            $(".Browser-support").addClass('nodisp')
        }
    },
    //--------------------
    ForgetPasswordCheck: function () {

        var data = {
            Password: $("#txtCurPass").val()
        };


        $.ajax({
            url: $app.baseUrl + "Login/Forgetpasswordchecking",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:


                        break;
                    case false:
                        $('#txtCurPass').val('');
                        $('#txtCurPass').focus();
                        $app.showAlert(jsonResult.Message, 4);
                        $app.hideProgressModel();

                        break;
                }

            }
        });
    },



    //----------------------
    //-------
    forgetPassword: function () {

        var data = {
            Email: $("#txtUserForget").val()
        };


        $.ajax({
            url: $app.baseUrl + "Login/ForgetPassword",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert("Mail sent Successfully");
                        $("#txtUserForget").val('');
                        $('#txtUserForget').focus();
                        break;

                    case false:


                        $app.showAlert("Mail id not found");
                        $("#txtUserForget").val('');
                        $('#txtUserForget').focus();
                        // window.location.href = data.result;
                        // return false;
                }

            },
            //complete: function () {
            //    $app.hideProgressModel();
            //}
        });
    },
    //----------
    changePassword: function () {

        var data = {
            Password: $("#txtNewPassword").val()
        };


        $.ajax({
            url: $app.baseUrl + "Login/ChangePassword",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (data) {

                if (data.success) {
                    alert("Your Password has been Changed")
                    window.location.href = data.result;
                }

                else {
                    $app.showAlert("Password does not Changed", 4);
                }

            },

        });

    }
}
