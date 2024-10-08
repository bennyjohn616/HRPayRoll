$('[name=reportname]').change(function () {

    switch ($(this).val()) {
        case "pfChallan":
            $('#HeadPanel').text('PF Challan');
            $('#divSingleMonth').removeClass('nodisp');
            $('#divpfChallan').removeClass('nodisp');
            $('#divPeriod').addClass('nodisp');
            $('#divCategory').addClass('nodisp');
            $('#divempcode').addClass('nodisp');
            break;
        case "pfExtract":
            $('#HeadPanel').text('PF Extract ');
            $('#divSingleMonth').removeClass('nodisp');
            $('#divCategory').removeClass('nodisp');
            $('#divPeriod').addClass('nodisp');
            $('#divempcode').addClass('nodisp');
            $('#divpfChallan').addClass('nodisp');
            break;
        case "pfForm3A":
            $('#HeadPanel').text('Form 3A ');
            $('#divSingleMonth').addClass('nodisp');
            $('#divPeriod').removeClass('nodisp');
            $('#divempcode').removeClass('nodisp');
            $('#divCategory').removeClass('nodisp');
            $('#divpfChallan').addClass('nodisp');
            break;
        default:
            $('#divSingleMonth').addClass('nodisp');
            $('#divPeriod').removeClass('nodisp');
            $('#divempcode').removeClass('nodisp');
            $('#divCategory').removeClass('nodisp');
            $('#divpfChallan').addClass('nodisp');
            break;
    }
});
$pfesiReport = {
    rpttitle: null,
    init: function () {
        
    },
    GetReport: function () {
        
        dataValue = $pfesiReport.buildRptObject();
        
        $.ajax({
            url: dataValue.url,
            data: dataValue.data,
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {
                var oData = new Object();
               
                oData.filePath = jsonResult.result.filePath;
                $app.downloadSync('Download/DownloadPaySlip', oData);
                
            }
        });
    },

    buildRptObject: function () {
        
        var data = new Object();
        var report = document.querySelector('input[name="reportname"]:checked').value;
        var url = '';
        var categories = '';
        var attrFilter = [];
        //Get Selected Category
        var rows = $("#tbldwCat").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            if ($(rows[i]).find(".cbCategory").prop("checked")) {
                categories = categories + $(rows[i]).find(":eq(2)").html() + ',';

            }
        } 
        
        switch (report) {
            case "pfChallan":
                //data.category = categories;
                data.smonth = $('#sltMonth').val();
                data.syear = $('#sltYear').val();
                //data.Chqdate = $('#dtpChqDate').val();
                //data.PayDate = $('#dtpPayDate').val();
                //data.ChqNumber = $('#txtChqNo').val();
                url = $app.baseUrl + "Reports/getPFChallan";
                break;
            case "pfExtract":
                data.smonth = $('#sltMonth').val();
                data.syear = $('#sltYear').val();
                data.category = categories;
                url = $app.baseUrl + "Reports/getPFExtact";
                break;
            case "pfForm3A":
                var rows = $("#tblFilter").dataTable().fnGetNodes();
                for (var i = 0; i < rows.length; i++) {

                    var newattr = new Object();

                    if ($(rows[i]).find(".filterField").val() != 0) {

                        newattr.type = $(rows[i]).find('td:eq(4)').text();
                        if (newattr.type == "Master") {
                            newattr.fieldName = $(rows[i]).find(".filterField option:selected").html();
                        } else {
                            newattr.fieldName = $(rows[i]).find(".filterField").val();
                        }
                        newattr.displayAs = $(rows[i]).find(".filterField option:selected").html();
                        newattr.tableName = $(rows[i]).find(".filterField").val();

                        newattr.datatype = $(rows[i]).find('td:eq(3)').text();
                        newattr.operation = $(rows[i]).find(".operations").val();
                        newattr.order = 0;
                        newattr.value = $(rows[i]).find(".filterValue").val();
                        attrFilter.push(newattr);
                    }
                }
                data.category = categories;
                data.title = "";
                data.syear = $('#sltSYear').val();
                data.smonth = 4;
                data.nYear = $('#sltEYear').val();
                data.nMonth = 5;
                data.filters = attrFilter;
                data.Empcode = $('#txtEmpcode').val();
                url = $app.baseUrl + "Reports/getPFForm3A";
                break;
            case "esiChallan":
                data.category = categories;
                data.smonth = $('#sltMonth').val();
                data.syear = $('#sltYear').val();
                url = $app.baseUrl + "Reports/getEsiChallan";
                break;
            case "esiExtract":
                data.smonth = $('#sltMonth').val();
                data.syear = $('#sltYear').val();
                data.category = categories;
                url = $app.baseUrl + "Reports/getESIExtract";
                break;
            default:
                break;

        }
        return {data:JSON.stringify(data),url:url};
    }
}