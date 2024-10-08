﻿$('#txtcomprofile').on('change', function (e) {
    
    var files = e.target.files;
    if (files.length > 0) {
        var file = this.files[0];
        fileName = file.name;
        size = file.size;
        type = file.type;
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append(files[x].name, files[x]);
            }
            
            $company.fileUploadData = data;

        }
        else {
            $app.showAlert("This browser doesn't support HTML5 file uploads!", 3);
            
            $company.fileUploadData = data;
            return false;
        }

    }
});
document.addEventListener('DOMContentLoaded', function () {
    $company.loadInitial();

    if ($("#hdnName").val().toLowerCase() != "admin") {
        $('#btnAddCompany').prop('disabled', true).hide();
        $('#btnCreateCompany').prop('disabled', true).hide();
    }
});



$DBCreate = {

    CreateDB: function () {
        var formAttribute = [];
        var returnval = '<form rle="form" id="frmCreateDB"><div class="modal-dialog"> '
        + '<div class="modal-content"> '
        + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
              + '  &times;</button>'
          + '  <h4 class="modal-title" id="H4">'
            + '    Create New DataBase</h4>'
       + ' </div>'
       + ' <div class="modal-body"> <div class="form-horizontal"> '
       + ' <div class="form-group"> <label class="control-label col-md-4">DataBase Name</label>'
                     + '<div class="col-md-6"> <input type="text" class="form-control"   id="txtDBName" '
        + 'placeholder="Enter the DataBase Name"> </div> </div>  </div></div>';
        returnval = returnval + ' <div class="modal-footer">'
           + ' <button type="submit" id="btnDBSave" class="btn custom-button">'
              + '  Save</button>'
           + ' <button type="button" class="btn custom-button" data-dismiss="modal">'
             + '   Close</button>'
       + ' </div> </div>';
        returnval = returnval + '</form>'
        //returnval = returnval.replace('{formelemnt}', formelemnt);
        document.getElementById("CreateDB").innerHTML = returnval;

        $("#frmCreateDB").on('submit', function (event) {
            $DBCreate.DBsave();//{ id: $("#txtDBName").val() }
            return false;
        });
    },
    DBsave: function () {

        $app.showProgressModel();
        var formData = document.forms["frmCreateDB"];
        var data = {
            CConnString: formData.elements["txtDBName"].value
        };
        $.ajax({
            url: $app.baseUrl + "Company/SaveCreateNewCompany",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#CreateCompany').modal('toggle');
                        BindClient();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $app.showAlert(id.id + 'DataBase Created Successfully', 2);

                        //alert(jsonResult.Message);
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


        return false;
    }
}

$company = {
    companyid: '',
    canSave: false,
    filePath: '',
    fileUploadData: null,
    loadInitial: function () {
        
        $company.BindClient();
        $('#frmCompany').on('submit', function () {

            $company.save();
        });
        $('#btnAddCompany').on('click', function () {

            $company.AddInitialize();
        });
        $('#btnCreateCompany').on('click', function () {

            $DBCreate.CreateDB();
        });

        //        $(function () {
        //
        //if ($("#hdnName").val().toLowerCase()!="admin") {
        //    $('#btnAddCompany').prop('disabled', true).hide();
        //    $('#btnCreateCompany').prop('disabled', true).hide();
        //}
        //});

    },
    AddInitialize: function () {

        $company.canSave = true;
        var formData = document.forms["frmCompany"];
        $company.companyid = 0;
        formData.elements["txtCompanyname"].value = "";
        formData.elements["txtAddressline1"].value = "";
        formData.elements["txtAdressline2"].value = "";
        formData.elements["txtCity"].value = "";
        formData.elements["txtState"].value = "";
        formData.elements["txtCountry"].value = "";
        formData.elements["txtPinCode"].value = "";
        formData.elements["txtPhone"].value = "";
        formData.elements["txtEMail"].value = "";
        formData.elements["txtcompanylogo"].value = "";
        formData.elements["cutoffdate"].value = "";
        formData.elements["proofcutoffdate"].value = "";
        formData.elements["proofrscutoffdate"].value = "";
        formData.elements['#imgCompanylogoView'].attr('src', '');

    },
    save: function () {
        debugger;
        if (!$company.canSave) {
            return false;
        }
        if ($company.fileUploadData != null) {
            $company.saveCompanyProfileImage();
        }
        $company.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmCompany"];
        var data = {
            Id: $company.companyid,
            CompanyName: formData.elements["txtCompanyname"].value,
            AddressLine1: formData.elements["txtAddressline1"].value,
            AddressLine2: formData.elements["txtAdressline2"].value,
            City: formData.elements["txtCity"].value,
            State: formData.elements["txtState"].value,
            Country: formData.elements["txtCountry"].value,
            PinCode: formData.elements["txtPinCode"].value,
            Phone: formData.elements["txtPhone"].value,
            EMail: formData.elements["txtEMail"].value,
            cutoffdate: new Date(formData.elements["cutoffdate"].value),
            proofcutoffdate: new Date(formData.elements["proofcutoffdate"].value),
            proofrscutoffdate: new Date(formData.elements["proofrscutoffdate"].value),
            IsActive: true,
            Companylogo: $company.filePath
        };
        $.ajax({
            url: $app.baseUrl + "Company/SaveCompany",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddCompany').modal('toggle');
                        $company.BindClient();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var p = jsonResult.result;
                        $company.companyid = 0;
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
    GetCompanyData: function (context) {
        
        $.ajax({
            url: $app.baseUrl + "Company/GetCompanyData",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddCompany').modal('toggle');
                        var p = jsonResult.result;
                        $company.RenderCompany(p);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });

    },
    DeleteData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Company/DeleteCompany",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $company.BindClient();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    RenderCompany: function (data) {
        debugger;
        var formData = document.forms["frmCompany"];
        $company.companyid = data.Id;
        formData.elements["txtCompanyname"].value = data.CompanyName;
        formData.elements["txtAddressline1"].value = data.AddressLine1;
        formData.elements["txtAdressline2"].value = data.AddressLine2;
        formData.elements["txtCity"].value = data.City;
        formData.elements["txtState"].value = data.State;
        formData.elements["txtCountry"].value = data.Country;
        formData.elements["txtPinCode"].value = data.PinCode;
        formData.elements["txtPhone"].value = data.Phone;
        formData.elements["txtEMail"].value = data.EMail;
        var locale = "en-us";
        var cudate = new Date(parseInt(data.cutoffdate.replace(/(^.*\()|([+-].*$)/g, '')));
        data.cutoffdate = cudate.getDate() + "/" + cudate.toLocaleString(locale, { month: "short" }) + "/" + cudate.getFullYear();
        formData.elements["cutoffdate"].value = data.cutoffdate;

        var proofcutoffdate = new Date(parseInt(data.proofcutoffdate.replace(/(^.*\()|([+-].*$)/g, '')));
        data.proofcutoffdate = proofcutoffdate.getDate() + "/" + proofcutoffdate.toLocaleString(locale, { month: "short" }) + "/" + proofcutoffdate.getFullYear();
        formData.elements["proofcutoffdate"].value = data.proofcutoffdate;

        var proofrscutoffdate = new Date(parseInt(data.proofrscutoffdate.replace(/(^.*\()|([+-].*$)/g, '')));
        data.proofrscutoffdate = proofrscutoffdate.getDate() + "/" + proofrscutoffdate.toLocaleString(locale, { month: "short" }) + "/" + proofrscutoffdate.getFullYear();
        formData.elements["proofrscutoffdate"].value = data.proofrscutoffdate;


        $company.filePath = data.Companylogo;
        $('#imgCompanylogoView').attr('src', data.Companylogo.replace('~', '../..')).width(150).height(150)
    },

    saveCompanyProfileImage: function () {
        
        $.ajax({
            url: $app.baseUrl + "Company/SaveUserImage",
            data: $company.fileUploadData,
            processData: false,
            contentType: false,
            type: "POST",
            async: false,
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        
                        $app.hideProgressModel();
                        var p = jsonResult.result;
                        $company.filePath = p;
                        $company.fileUploadData = null;
                        // $company.save();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    BindClient: function () {

        $app.applyseletedrow();
        var dtClientList = $('#tblCompany').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'responsive': true,
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            columns: [
                 { "data": "Id" },
                        { "data": "CompanyName" },
                           { "data": "AddressLine1" },
                           { "data": "City" },
                           { "data": "State" },
                            { "data": "Phone" },
                            { "data": "EMail" },
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
              "sClass": "actionColumn"
                        ,
              "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                  var a = $('<a href="' + $app.baseUrl + 'Home/LoadCompany?id=' + oData.Id + '")" class=""><span> Load</span></a>');
                  var b = "";
                  var c = "";
                  var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                  var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                      a.button();
                      b.button();
                      b.on('click', function () {

                          $company.GetCompanyData(oData);
                          $company.canSave = true;
                          return false;
                      });
                      c.button();
                      c.on('click', function () {
                          if (confirm("Are you sure, do you want to Delete?"))
                              $company.DeleteData(oData);
                          return false;
                      });
                      $(nTd).empty();
                      var role = $('#hdnRoleName').val();
                      if (role.toUpperCase() != "EMPLOYEE")
                      {
                          $(nTd).append(a, b, c);
                      }
                      else
                      {
                          $(nTd).append(a);
                      }
                  

              }
          }


            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Company/GetCompany",
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
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            dom: "rtiS",
            "bDestroy": true,

            scroller: {
                loadingIndicator: true
            }
        });

        var table = $('#tblCompany').DataTable();
        $('#myInput').keyup(function () {
            table.search($(this).val()).draw();
        })

    }

}









