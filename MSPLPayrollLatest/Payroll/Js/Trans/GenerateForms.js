$GenerateForms = {
    loadFinYrs: function () {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "FinanceYear/GetFinanceYears",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                debugger;
                var out = msg.result;
                console.log(out);
                $('#FinYears').html('');
                $('#FinYears').append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(out, function (index, finyr) {
                    $('#FinYears').append($("<option></option>").val(finyr.id).html(finyr.startDate + ' TO ' + finyr.EndDate));
                });
            },
            error: function (msg) {
            }
        });
    },
    generateForm24Q: function () {
        $app.showProgressModel();
        debugger;

        var FinYrId = '';
        var FinYrText = '';
        var Quaterly = '';

        FinYrId = $('#FinYears').val();
        FinYrText = $('#FinYears :selected').text();
        Quaterly = $('#ddQuaterly').val();
        $.ajax({
            url: $app.baseUrl + "DataWizard/GetForm24quaterly",
            data: JSON.stringify({ FinYrId: FinYrId, FinYrText: FinYrText, Quaterly: Quaterly }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                var oData = new Object();
                if (jsonResult.Status == true) {
                    oData.filePath = jsonResult.result;
                    $app.downloadSync('Download/DownloadPaySlip', oData);
                } else {
                    $app.showAlert(jsonResult.Message, 2);
                }


            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },
    generateForm24QA: function () {
        $app.showProgressModel();
        debugger;

        var FinYrId = '';
        var FinYrText = '';       
        FinYrId = $('#FinYears').val();
        FinYrText = $('#FinYears :selected').text();
        $.ajax({
            url: $app.baseUrl + "DataWizard/GetForm24QAnnenture",
            data: JSON.stringify({ FinYrId: FinYrId, FinYrText: FinYrText}),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                var oData = new Object();
                if (jsonResult.Status == true) {
                    oData.filePath = jsonResult.result;
                    $app.downloadSync('Download/DownloadPaySlip', oData);
                } else {
                    $app.showAlert(jsonResult.Message, 2);
                }


            },
            complete: function () {
                $app.hideProgressModel();

            }
        });

    },
}