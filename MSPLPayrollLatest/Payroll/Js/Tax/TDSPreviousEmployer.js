$TDSPreviousEmployer = {
    
    financeYear: $companyCom.getDefaultFinanceYear(),
    LoadData: function () {
        debugger;
        var dtClientList = $('#tableTDSPreviousEmployer').DataTable({
           
            columns: [
                { "data": "EmployeeCode" },
                     {
                         "data": "Name",
                     },
                     {
                         "data": "DateOfJoining",
                     },
               {
                   "data": "Tax",
               },

            ],

            "aoColumnDefs": [
                 {
                     "aTargets": [0],
                     "sClass": "word-wrap"
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
                 



            ],
         
            ajax: function (data, callback, settings) {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Taxsection/TDSPreviousEmployer",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(),
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
                        var out = jsonResult.result;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {

                            case true:
                                $.each(out, function (data, value) {
                                    debugger;
                                    var date = value.DateOfJoining;
                                    var nowDate = new Date(parseInt(date.substr(6)));
                                    value.DateOfJoining = nowDate.getDate()+'/'+(parseInt(nowDate.getMonth())+1)+'/'+nowDate.getFullYear();
                                })
                                setTimeout(function () {
                                  
                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length

                                    });

                                }, 50);
                              

                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            dom: "rtiS",
          
        });
    },
};