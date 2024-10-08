//Modefied By Benny
$("#btnCategorySave").click(function () {
    
    var disordernum=  $("#txtDisOrder").val();
    if(disordernum==0)
    {
        $app.showAlert('Display order Cannot be 0', 4);
    }
    else{
        $category.save();
    }
});





$("#txtCategoryName").change(function () {
    
    var CatNameRowsDateCheck = $("#tblCategory").dataTable().fnGetNodes();
    for (i = 0; i < CatNameRowsDateCheck.length; i++)
    {
        if ($(CatNameRowsDateCheck[i]).find("td:nth-child(2)").html().trim().toLowerCase() == $("#txtCategoryName").val().trim().toLowerCase())
        {
            $app.showAlert("Already Exist " + $("#txtCategoryName").val(), 4);
            $("#txtCategoryName").val('');
            $("#txtCategoryName").focus();
            return false;
        }
    }
   
    //transRowsDateCheck.each(function () {
    //    if ($("#txtCategoryName").val().trim().toLowerCase()== $(this).find("td:nth-child(2)").html().trim().toLowerCase()) {
    //        $app.showAlert("Already Exist " + $("#txtCategoryName").val(), 4);
    //        $("#txtCategoryName").val('');
    //        return false;
    //    }
    //});
});
//Modefied By Benny

$("#txtDisOrder").change(function () {
    
    //var inp1 = parseInt($("#txtDisOrder").val().trim().toLowerCase());
    $("#tblCategory tbody tr").each(function () {
        if (parseInt($("#txtDisOrder").val().trim().toLowerCase()) == $(this).find("td:nth-child(3)").html().trim().toLowerCase()) {
            $app.showAlert("Already Exist " + $("#txtDisOrder").val(), 4);
            $("#txtDisOrder").val('');
            return false;
        }
    });
});
var $category = {
   
    categoryId: '',
    canSave: false,
    LoadCategories: function () {
        debugger;
        var dtClientList = $('#tblCategory').DataTable({

            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[2, "asc"]],
            columns: [
             { "data": "Id" },
                    { "data": "Name" },
                    { "data": "DisOrder" },
                       { "data": "CompanyId" },
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
                  $category.GetPopupData(oData);
                  $category.canSave = true;
                  return false;
              });
              c.button();
              c.on('click', function () {
                  
                  //$category.EntityCatagoryCheck({ Id: oData.Id, Name: oData.Name, companyid: oData.CompanyId });

                  if (confirm('Are you sure ,do you want to delete?')) {
                      $category.DeleteData(oData);
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
                    url: $app.baseUrl + "Company/GetCategories",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        
                        var out = jsonResult.result;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                debugger;
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

    },
    
    EntityCatagoryCheck: function (context) {
        
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetCatagoryMapping",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ entityId: context.Id, cmpid: context.CompanyId, Name: context.Name }),
            dataType: "json",
            success: function (jsonResult) {
                
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                    case false:
                        if (confirm('Are you sure,do you want to delete?')) {
                            $category.DeleteData(oData);
                        }

                        //$app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    save: function () {
        
        if (!$category.canSave) {
            return false;
        }
        $category.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmCategory"];
        var data = {
            id: $category.categoryId,
            popuplalue: formData.elements["txtCategoryName"].value,
            DisOrder: formData.elements["txtDisOrder"].value,
            type: "category"
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
                        $('#AddCategory').modal('toggle');
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
        var formData = document.forms["frmCategory"];
        $category.categoryId = ''
        formData.elements["txtCategoryName"].value = "";
        formData.elements["txtDisOrder"].value = "";        
        $category.canSave = true;
    },

    GetPopupData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Company/GetPopupData",
            data: JSON.stringify({ id: context.Id, type: "category" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddCategory').modal('toggle');
                        var p = jsonResult.result;
                        $category.RenderData(p);
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
            data: JSON.stringify({ id: context.Id, type: "category" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
             
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                   
                    case true:
                        $category.LoadCategories();
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
        var formData = document.forms["frmCategory"];
        $category.categoryId = data.Id;
        formData.elements["txtCategoryName"].value = data.popuplalue;
        formData.elements["txtDisOrder"].value = data.DisOrder;
    }


};