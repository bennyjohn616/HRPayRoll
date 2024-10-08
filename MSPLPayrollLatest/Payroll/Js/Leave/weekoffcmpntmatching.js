var $wkoffcmpntmatching = {
    wkoffcmpntmatchingId:'',
    canSave: true,  
    oDataCheckBal: null,
    LoadData: function () {
        
        var dtClientList = $('#tblLeaveOpening').DataTable({

            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                     { "data": "id" },
                     { "data": "lvcategory" },
                     { "data": "lvcomponent" },
                     { "data": null }
            ],
            "aoColumnDefs": [
        {

            "aTargets": [0],
            "sClass": "nodisp id",
            "bSearchable": false
        },
        {
            "aTargets": [1],
            "sClass": "word-wrap"

        },
        {
            "aTargets": [2],
            "sClass": "word-wrap"

        }
      ,{
          "aTargets": [3],
          "sClass": "actionColumn",

          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              //var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              //b.button();
              //b.on('click', function () {
                  //$benefitComponent.render(oData);
                 // return false;
              //});
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      // $levFinanceYear.DeleteData(oData);
                  }
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend( c);
          }
      }
            ], responsive: true,

            ajax: function (data, callback, settings) {
                
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetComponentMatching",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
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
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    save: function () {
        
        if (!$wkoffcmpntmatching.canSave) {
            return false;
        }
        $wkoffcmpntmatching.canSave = false;
        $app.showProgressModel();
        
        var data = {
            id: $wkoffcmpntmatching.wkoffcmpntmatchingId,
            leavecategory
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
                        $category.LoadCategories();
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
        $('#ddLeavecategory').val('00000000-0000-0000-0000-000000000000');
        $('#ddLeavecomponent').val('00000000-0000-0000-0000-000000000000');
        $wkoffcmpntmatching.canSave = true;
    },

}
$companyCom.loadLeaveType({ id: 'ddLeavecategory' })