
$('#btnSave').on("click", function () {
    
    var satus = 0;
    if ($('#txtIpAddress').val() == "") {
        satus = 1;
        $app.showAlert("please enter the IP address ", 4);
        return false;
    }
    if ($('#txtPortNo').val() == "") {
        satus = 1;
        $app.showAlert("please enter the Port ", 4);
        return false;
    }
    if($('#txtFromAddress').val()=="")
    {
        satus = 1;
        $app.showAlert("please enter the From address ", 4);
        return false;
    }
    if ($('#txtFromAddress').val() == "") {
        satus = 1;
        $app.showAlert("please enter the From address ", 4);
        return false;
    }
    if ($('#txtFrompassword').val() == "")
    {
        satus = 1;
        $app.showAlert("please enter the Password ", 4);
        return false;
    }
    if ($('#txtURL').val() == "") {
        satus = 1;
        $app.showAlert("please enter the Application Url ", 4);
        return false;
    }
    if(satus ==0)
    {
        $mailConfiguration.mailConfigSave();
    }
});



$("#sltAutheticated").change(function () 
{
    
    if($("#sltAutheticated").val()==0)
    {
        $('#txtSmtpPwd').prop('disabled', true);
    }
    else
    {
        $('#txtSmtpPwd').prop('disabled', false);
    }
});
var $mailConfiguration =
    {
        
        selectedConfigId: '',
        mailConfigSave: function () {
            
            var checkEnable;
            var authEmail;
            if ($('#chkEnableSSL').is(":checked")) {
                checkEnable = 1;
            }
            else
            {
                checkEnable=0;
            }
            //if ($('#sltAutheticated').val() == 1)
            //{
            //    authEmail = true;
            //}
            //else {
            //    authEmail = false;
            //}
            var data = {
                Id:$mailConfiguration.selectedConfigId,
                IPAddress: $('#txtIpAddress').val(),
                PortNo: $('#txtPortNo').val(),
                FromEmail: $('#txtFromAddress').val(),
                mailpassword: $('#txtFrompassword').val(),
                EnableSSL: checkEnable,
                CCMail: $('#txtCC').val(),
                mailapproval: $('#txtURL').val()
                //AuthenEmail: authEmail,
                //AuthenSMTPUser: $('#txtSmtpUser').val(),
                //AuthenSMTPPwd: $('#txtSmtpPwd').val(),

            };
            //  LastName: $($User.formData).find('#txtLastName').val(),
            console.log(data);
            $.ajax({
                url: $app.baseUrl + "Leave/SaveMailConfig",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    
                    $app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:
                            

                            $app.hideProgressModel();
                            $mailConfiguration.get();
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

        },

        get: function () {
            
           
            $.ajax({
                url: $app.baseUrl + "Leave/GetMailConfig",
                data: null,
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    
                    $app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:
                            var p = jsonResult.result;
                            $mailConfiguration.render(p);
                            return 
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
       
        render: function (data) {
            
            var authEmail;
            if (data.AuthenEmail =="True") {
                authEmail = 1;
            }
            else {
                authEmail = 0;
            }
            if (data.EnableSSL == true)
            {
                $('#chkEnableSSL').prop('checked', true);
                }
          
            else {
                $('#chkEnableSSL').prop('checked', false);
            }
               $mailConfiguration.selectedConfigId=data.Id;
               $('#txtIpAddress').val(data.IPAddress),
               $('#txtPortNo').val(data.PortNo),
               $('#txtFromAddress').val(data.FromEmail),
               $('#chkEnableSSL').prop('checked', data.EnableSSL),
               $('#sltAutheticated').val(authEmail),
               $('#txtSmtpUser').val(data.AuthenSMTPUser),
               $('#txtSmtpPwd').val(data.AuthenSMTPPwd),
               $('#txtFrompassword').val(data.MailPassword),
               $('#txtCC').val(data.CCMail),
               $('#txtURL').val(data.mailapproval)
        },
        sendTestMail: function () {
            
            var smtpServer = $('#txtIpAddress').val();
            var portNo = $('#txtPortNo').val();
            var fromMail = $('#txtFromAddress').val();
            var toMail = $('#txtEmailAddr').val();
            //var mailpassword = $('#txtSmtpPwd').val();
            var mailpassword = $('#txtFrompassword').val();
            var authon = $('#sltAutheticated').val();
            
            var sslchecl = null;
            if ($('#chkEnableSSL').is(":checked")) {
                sslchecl = true;
            }
            else {
                sslchecl = false;
            }
            
            $.ajax({
                url: $app.baseUrl + "Leave/SendTestMail",
                data: JSON.stringify({toMail: toMail}),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    
                    $app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:
                          
                            $app.showAlert(jsonResult.Message, 2);

                            return
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
        GETDEFAULTCREATEDID: function (date,viewbagid) {
        

        $.ajax({
            url: $app.baseUrl + "Leave/GetLOPandONDUTYID",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        
                        var p = jsonResult.result;
                        //$mailConfiguration.render(p);
                       

                        if (viewbagid != p.ONDUTYId && viewbagid !=p.LOPId) {
                            $('#accordion2').addClass('nodisp');
                            $('#accordion1').removeClass('nodisp');
                            $Calendar.LoadCalendarDetails(date, viewbagid);
                        }
                        else {
                            $('#accordion1').addClass('nodisp');
                            $('#accordion2').removeClass('nodisp');
                            $Calendar.LoadCalendarDetailsLOP(date, viewbagid);
                        }

                        return
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
        


    }