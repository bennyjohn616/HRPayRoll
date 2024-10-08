
$("#btnSave").click(function () {
    debugger;
    if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000" || $('#allCategory').is(":checked")) {
        $tdsStatement.selectedCategoryId = $('#sltCategory').val();
        $tdsStatement.month = $('#sltMonth').val(),
        $tdsStatement.year = $('#sltYear').val(),
        $tdsStatement.type = "View"
        if ($('#allCategory').is(":checked")) {
            $tdsStatement.allctegory=true;
        }
        else {
            $tdsStatement.allctegory = false;
        }

        $tdsStatement.financeYear = $companyCom.getDefaultFinanceYear(),
        $tdsStatement.LoadData();
    }
    else {
        if (!$('#allCategory').is(":checked")) {
            $app.showAlert("select category", 3);
        }
    
    }
});
$('#allCategory').click(function () {
    debugger;
    if ($(this).is(":checked")) {
        debugger;
        $("#sltCategory").val('00000000-0000-0000-0000-000000000000');
     
    }
    else {
        $("#sltCategory").val('00000000-0000-0000-0000-000000000000');
      
        
    }

});
$('#allCategory').click(function () {

    $("#tabletds").addClass("nodisp");
    $("#preView").addClass("hidden");
});

$("#preView").click(function () {
    if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000" || $('#allCategory').is(":checked")) {
        $tdsStatement.selectedCategoryId = $('#sltCategory').val();
        $tdsStatement.month = $('#sltMonth').val(),
        $tdsStatement.year = $('#sltYear').val(),
        $tdsStatement.type = "Preview"
        if ($('#allCategory').is(":checked")) {
            $tdsStatement.allctegory = true;
        }
        else {
            $tdsStatement.allctegory = false;
        }
        $tdsStatement.financeYear = $companyCom.getDefaultFinanceYear(),
        $tdsStatement.LoadData();
    }
    else {
        if (!$('#allCategory').is(":checked")) {
            $app.showAlert("select category", 3);
        }
    }
});

$('#sltCategory').change(function () {
    $("#tabletds").addClass("nodisp");
    $("#preView").addClass("hidden");

    $('#allCategory').prop('checked', false);
    if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000") {
        $tdsStatement.month = $('#sltMonth').val();
        var sd = new Date($tdsStatement.financeYear.startDate);
        var ed = new Date($tdsStatement.financeYear.EndDate);
        if (ed.getMonth() + 1 >= $tdsStatement.month) {
            $('#sltYear').val(ed.getFullYear());
        }
        else {
            $('#sltYear').val(sd.getFullYear());
        }
        
        $tdsStatement.selectedCategoryId = $('#sltCategory').val();
        
    } 
    console.log($('#sltCategory').val());
});



$('#sltMonth').change(function () {
    debugger;
    $("#tabletds").addClass("nodisp");
    $("#preView").addClass("hidden");
    $tdsStatement.month = $('#sltMonth').val();
    var sd = new Date($tdsStatement.financeYear.startDate);
    var ed = new Date($tdsStatement.financeYear.EndDate);
    if (ed.getMonth() + 1 >= $tdsStatement.month) {
        $('#sltYear').val(ed.getFullYear());
    }
    else {
        $('#sltYear').val(sd.getFullYear());
    }
    if ($('#sltCategory').val() != "00000000-0000-0000-0000-000000000000") {
        
        $tdsStatement.selectedCategoryId = $('#sltCategory').val();
    }
    
});

$tdsStatement = {
    selectedCategoryId: null,
    type: null,
    allctegory:null,
    month: $('#sltMonth').val(),
    year: $('#sltYear').val(),
    financeYear: $companyCom.getDefaultFinanceYear(),
    LoadData: function () {
        debugger;
        var dtClientList = $('#tblTdsReport').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                { "data": "Id" },
                     {
                         "data": "EmployeeCode",
                     },
                     {
                         "data": "EmployeeName",
                     },
                     { "data": "TotalTax" },

                     { "data": "AlreadyDeducted" },

                     { "data": "BalanceTax" },

                      { "data": "TaxPercentage" },

                     { "data": "OneTimeTax" },

                     { "data": "Permonth" },

                     { "data": "NoOfMonths" },

                      { "data": "Thismonth" },
                    


            ],

            "aoColumnDefs": [
                 {
                     "aTargets": [0],
                     "sClass": "nodisp"
                 },
                  {
                      "aTargets": [1],
                      "sClass": "word-wrap"
                  },
                   {
                       "aTargets": [2],
                       "sClass": "word-wrap"
                   },
                    {
                        "aTargets": [3],
                        "sClass": "word-wrap"
                    },
                    {
                        "aTargets": [4],
                        "sClass": "word-wrap"
                    },
                     {
                         "aTargets": [5],
                         "sClass": "word-wrap"
                     },
                      {
                          "aTargets": [6],
                          "sClass": "word-wrap"
                      },
                       {
                           "aTargets": [7],
                           "sClass": "word-wrap"
                       },
                        {
                            "aTargets": [8],
                            "sClass": "word-wrap"
                        },
                          {
                            "aTargets": [9],
                            "sClass": "word-wrap"
                          },
                            {
                                "aTargets": [10],
                                "sClass": "word-wrap"
                            },



            ],
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxProcess/TdsStatement",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ selectedId: $tdsStatement.selectedCategoryId, year: $tdsStatement.year, month: $tdsStatement.month, financeYearId: $tdsStatement.financeYear.id, type: $tdsStatement.type, AllCategory: $tdsStatement.allctegory }),
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
                        var out = jsonResult.result.ltds;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {

                            case true:
                                
                                    setTimeout(function () {
                                        $("#tabletds").removeClass("nodisp");
                                        $("#preView").removeClass("hidden");
                                        callback({
                                            draw: data.draw,
                                            data: out,
                                            recordsTotal: out.length,
                                            recordsFiltered: out.length

                                        });

                                    }, 50);
                                    if (jsonResult.result.type == "Preview") {
                                        var oData = new Object();
                                        console.log(jsonResult.result.filePath);
                                        oData.filePath = jsonResult.result.filePath;
                                        $app.downloadSync('Download/DownloadPaySlip', oData);
                                        return false;
                                    }
                                
                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
};