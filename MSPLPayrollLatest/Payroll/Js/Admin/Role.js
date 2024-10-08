$("#frmRole").on('submit', function (event) {
    event.preventDefault();
    if ($app.requiredValidate("frmRole", event)) {
        if ($("#txtDisplayAs").val().toLowerCase() == "employee" || $("#txtName").val().toLowerCase() == "employee") {
            $app.showAlert("You Can't Create Employee Role", 3);
            return false;
        }
        $Role.save();
    }
});
var $Role = {
    RoleId: '',
    LoadRole: function () {
        var dtClientList = $('#tblRole').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "Id" },
                    { "data": "Name" },
                       { "data": "DisplayAs" },
                       { "data": "Description" },
                       {
                           "data": null
                       }
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
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  
                  if (oData.Id == "1" || oData.Id == "2" || oData.Id == "3") {
                      $app.showAlert("You Can't Edit  " + oData.DisplayAs, 4);
                  }
                  else {
                      $Role.GetRoleData(oData);
                  }

                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      if (oData.Id == "1" || oData.Id == "2" || oData.Id == "3") {
                          $app.showAlert("You Can't Delete  " + oData.DisplayAs, 4);
                      }
                      else {
                          $Role.DeleteData({ Id: oData.Id });
                      }

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
                    url: $app.baseUrl + "Admin/GetRole",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (msg) {

                        var out = msg; // $.parseJSON(msg.d);
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
                var r = $('#tblRole tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblRole thead').append(r);
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

        $app.showProgressModel();
        var formData = document.forms["frmRole"];
        var data = {
            id: $Role.RoleId,
            Name: formData.elements["txtName"].value,
            DisplayAs: formData.elements["txtDisplayAs"].value,
            Description: formData.elements["txtDescription"].value
        };
        $.ajax({

            url: $app.baseUrl + "Admin/SaveRoleData",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddRole').modal('toggle');
                        $Role.LoadRole();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        //alert(jsonResult.Message);
                        var p = jsonResult.result;
                        companyid = 0;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },

    AddInitialize: function () {

        var formData = document.forms["frmRole"];
        $Role.RoleId = "";
        formData.elements["txtName"].value = "";
        formData.elements["txtDisplayAs"].value = "";
        formData.elements["txtDescription"].value = "";
        $('#txtName').attr("readonly", false);

    },

    GetRoleData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Admin/GetRoleData",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddRole').modal('toggle');
                        var p = jsonResult.result;
                        $Role.RenderData(p);
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
    // Modified by Keerthika on 17/05/2017
    DeleteData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Admin/DeleteRoleData",
            data: JSON.stringify({ id: context.Id, type: "Role" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $Role.LoadRole();
                        $app.showAlert(jsonResult.Message, 2); //--
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
        
        var formData = document.forms["frmRole"];

        $Role.RoleId = data.Id;
        formData.elements["txtName"].value = data.Name;
        formData.elements["txtDisplayAs"].value = data.DisplayAs;
        formData.elements["txtDescription"].value = data.Description;
        $('#txtName').attr("readonly", "readonly");

    }


};