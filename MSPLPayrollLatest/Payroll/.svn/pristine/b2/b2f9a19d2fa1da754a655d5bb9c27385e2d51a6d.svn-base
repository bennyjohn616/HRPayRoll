


$("#preView").click(function () {

    if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000") {
        $('#tabletds').empty();
        $ArrearView.selectedCategoryId = $('#sltCategory').val();
        $ArrearView.month = new Date(parseInt($('#Period').val().substr(6))),
     
        $ArrearView.type = "Preview"
       
      
        $ArrearView.LoadDataTable();
    }
    else {
        $app.showAlert("select the category",3);
    }
   
});

$('#sltCategory').change(function () {
    debugger;
    $('#tabletds').empty();
    if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000") {
        $('#Period').html('');
        $ArrearView.selectedCategoryId = $('#sltCategory').val();
        $ArrearView.LoadPeriod();
    }
    console.log($('#sltCategory').val());
});

$('#Period').change(function () {
    $('#tabletds').empty();
});



$ArrearView = {
    selectedCategoryId: null,
    month: null,
    type:null,
  
    LoadPeriod: function () {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Transaction/ArrearViewPeriod",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ selectedId: $ArrearView.selectedCategoryId }),
            dataType: "json",
            success: function (jsonResult) {
                debugger;
                var out = jsonResult.result;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {

                    case true:
                        $('#periodDisplay').removeClass("nodisp");
                        $.each(out, function (data, value) {
                            debugger;
                            var date = value;
                            var nowDate = new Date(parseInt(date.substr(6)));
                            var month = ["January", "February", "March", "April", "May", "June",
                                "July", "August", "September", "October", "November", "December"][nowDate.getMonth()];

                            $("#Period").append($("<option></option>").val(value).html(month + " " + nowDate.getFullYear()));
                        })
               

                        break;
                    case false:

                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    LoadDataTable: function () {
        debugger;
        $app.hideProgressModel();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Transaction/ArrearView",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ selectedId: $ArrearView.selectedCategoryId, period:$ArrearView.month}),
            dataType: "json",
            success: function (jsonResult) {
                debugger;
                var out = jsonResult.result;
               
                switch (jsonResult.Status) {

                    case true:
                        $ArrearView.LoadDataTableData(out);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    LoadDataTableData: function (data) {
        debugger;
        $("#tabletds").append('<button id="btnExport" class="btn custom-button " onclick="fnExcelReport();"> EXPORT </button>');
        
        $("#tabletds").append('<table cellpadding="0" cellspacing="0" border="0" class="" id="tblTdsReport"></table>');
        var dataObject = eval('[{"COLUMNS":[],"DATA":[]}]');
        for (i = 0; i < data.rowheader.length; i++) {
            dataObject[0].COLUMNS[i] = { title: data.rowheader[i] }
        }
        for (i = 0; i < data.rows.length; i++) {
            dataObject[0].DATA[i] = data.rows[i];
        }
        dataObject[0].DATA[data.rows.length] = data.rowfooter;
        $('#tblTdsReport').dataTable({
            "data": dataObject[0].DATA,
            "columns": dataObject[0].COLUMNS,
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            pagingType: "full_numbers",
            searching: true,
            select: true,
            lengthChange: true,
            pageLength: 10,
            scrollX: true,
            scrollY: "550px",
            scrollCollapse: true,
            processing: true,
            //aaSorting: [[0, 'desc']],
            //dom: "rtiS",
   
       

        });





    },

};