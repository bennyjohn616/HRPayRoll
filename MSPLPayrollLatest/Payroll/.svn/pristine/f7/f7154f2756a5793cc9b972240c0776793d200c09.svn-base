$("#txtBankName").change(function () {
    $("#tblBank tbody tr").each(function () {
        if ($("#txtBankName").val().toLowerCase() == $(this).find("td:nth-child(2)").html().toLowerCase()) {
            $app.showAlert("Already Exist " + $("#txtBankName").val(), 4);
            $("#txtBankName").val('');
            return false;
        }
    });
});



var $bank = {
    bankId: '',
    canSave: false,
    LoadBanks: function () {
        var dtClientList = $('#tblBank').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "Id" },
                    { "data": "Name" },
                    
                      
                       {
                           "data": null
                       }
            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
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
             "sClass": "nodisp"

         },
      {
          "aTargets": [4],
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  $bank.GetPopupData(oData);
                  $bank.canSave = true;
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $bank.DeleteData(oData);
                  }
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend(b, c);
          }
      }
            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Company/GetBanks",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (msg) {
                        var out = msg;
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length
                            });

                        }, 50);
                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblCompany tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblCompany thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    }
    ,
    save: function () {
        if (!$bank.canSave) {
            return false;
        }
        $bank.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmBank"];
        var data = {
            id: $bank.bankId,
            popuplalue: formData.elements["txtBankName"].value,
           
            type: "bank"
        };
        $.ajax({
            url: $app.baseUrl + "Company/SavePopup",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddBank').modal('toggle');
                        $bank.LoadBanks();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var p = jsonResult.result;
                        companyid = 0;
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

    AddInitialize: function () {
        var formData = document.forms["frmBank"];
        $bank.bankId = ''
        formData.elements["txtBankName"].value = "";
      
        $bank.canSave = true;
    },

    GetPopupData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Company/GetPopupData",
            data: JSON.stringify({ id: context.Id, type: "bank" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddBank').modal('toggle');
                        var p = jsonResult.result;
                        $bank.RenderData(p);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });

    },
    DeleteData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Company/DeletePopupData",
            data: JSON.stringify({ id: context.Id, type: "bank" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $bank.LoadCategories();
                        $app.showAlert(jsonResult.Message, 2);
                        //alert(jsonResult.Message);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });
    },

    RenderData: function (data) {
        var formData = document.forms["frmBank"];
        $bank.bankId = data.Id;
        formData.elements["txtBankName"].value = data.popuplalue;
     
    }


};