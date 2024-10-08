$("#sltEmpCodeSP").change(function () {
    debugger

    if ($('#sltEmpCodeSP').find('option:selected').text() != "--Select--") {

        $Showpass.showPassword();
    }
    else {
        
        $("#usernamesp").val('');
        $("#lblusernamesp").addClass("nodisp");
        $("#usernamesp").addClass("nodisp");
        $("#empnamesp").val('');
        $("#lblempnamesp").addClass("nodisp");
        $("#empnamesp").addClass("nodisp");
        $("#emppasswordsp").val('');
        $("#lblemppasswordsp").addClass("nodisp");
        $("#emppasswordsp").addClass("nodisp");
    }


});
var $Showpass = {


    showPassword: function () {
        debugger;
        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/ShowPassword",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EMPID: $("#sltEmpCodeSP").val() }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:
                        debugger;
                        $app.hideProgressModel();
                        $("#usernamesp").val(out[0].Username);
                        $("#lblusernamesp").removeClass("nodisp");
                        $("#usernamesp").removeClass("nodisp");
                        $("#empnamesp").val(out[0].FirstName);
                        $("#lblempnamesp").removeClass("nodisp");
                        $("#empnamesp").removeClass("nodisp");
                        $("#emppasswordsp").val(out[0].Password);
                        $("#lblemppasswordsp").removeClass("nodisp");
                        $("#emppasswordsp").removeClass("nodisp");
                        break;
                    case false:
                        // $('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $("#usernamesp").val('');
                        $("#lblusernamesp").addClass("nodisp");
                        $("#usernamesp").addClass("nodisp");
                        $("#empnamesp").val('');
                        $("#lblempnamesp").addClass("nodisp");
                        $("#empnamesp").addClass("nodisp");
                        $("#emppasswordsp").val('');
                        $("#lblemppasswordsp").addClass("nodisp");
                        $("#emppasswordsp").addClass("nodisp");
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },





}