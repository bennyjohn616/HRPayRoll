



$lwf = {
    selectedLocationId: 0,
    id: 0,
    loadInitial: function () {
        $companyCom.loadLocation({ id: "ddLocation" });
    },

    save: function () {
        var data = new Object();
        data.id = $lwf.selectedLocationId
        data.locationId = $('#ddLocation').val();
        data.applyMonth = $('#ddMonth').val();
        data.employeeAmt = $('#txtEmployeeAmt').val();
        data.employerAmt = $('#txtEmployerAmt').val();
        $.ajax({
            url: $app.baseUrl + "Company/Savelwf",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
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
    },
    get: function () {
        
        $.ajax({
            url: $app.baseUrl + "Company/GetLWFSetting",
            data: JSON.stringify({ locationId: $('#ddLocation').val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $lwf.selectedLocationId = out.locationId;
                        $lwf.id = out.id;
                        $('#txtEmployeeAmt').val(out.employeeAmt);
                        $('#txtEmployerAmt').val(out.employerAmt);
                        $('#ddMonth').val(out.applyMonth);
                        break;
                    case false:

                     
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

                $app.hideProgressModel();
            }
        });
    } 
}