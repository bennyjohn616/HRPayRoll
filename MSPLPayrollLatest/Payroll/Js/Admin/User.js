﻿//( function () {
//    
//    $User.LoadPopupforRights($(".sltRights").val());
//});
//$('#ddcomponent').on('change', function () {
//    $User.LoadPopupforRights($("#ddcomponent").val());
//});
//// ---Modified By Keerthika on 29/04/2017---

//$("#btnAddUser").on('click', function (event) {
//    
//    $User.AddInitialize();
//});
//Created by Keerthika on 10/06/2017

$("#txtEmail").change(function () {

    if ($("#txtEmail").val().length > 0)
        $User.mailcheck();

});
$("#sltRolelist").change(function () {

    $User.EmployeeAction = $("#sltRolelist :selected").text().trim();
    $User.LoadCompTable();
});
//$('#sltEmployeeId').change(function () {
//    
//    var EmployeeId = $("#sltEmployeeId").val();
//    $User.GetRegData(EmployeeId);
//});




//$("#btnComponentSave").on('click', function (event) {
//    
//    var error = 0;
//    $(".Reqrd").each(function () {
//        
//        if (document.getElementById(this.id).value == "") {
//            
//            $app.showAlert('Please ' + $(this).attr('placeholder'), 'danger');
//            error = 1;
//            return false;
//        }

//        if ($("#txtPassword").val().trim() != $("#txtCNFPassword").val().trim()) {

//            
//            $app.showAlert('Password And Confirm Password Should be Same', 4);
//            $("#txtCNFPassword").focus();
//            error = 1;
//            return false;
//        }





//    });

//    if (error == 0) {
//        
//        $User.save();
//    }





//});




//$("#txtUsername").change(function () {


//    var value = $('#txtUsername').val();
//    var userlist = $User.UserAll;
//    for (var i = 0; i < userlist.length; i++) {
//        if (value.toLowerCase().trim() == "admin") {
//            $app.showAlert("You Cannot create Username as Admin", 4);
//            $("#txtUsername").val('');
//            $('#txtUsername').focus();
//        }
//        else if (userlist[i].Username.toLowerCase().trim() == value.toLowerCase().trim()) {
//            //var companylist = userlist[i].userCompanyMapping;
//            //for (var j = 0; j < companylist.length; j++) {
//            //    if (companylist[j].CompanyId == $('#hdnCompId').val())
//            //   {

//                    $app.showAlert("Username Already Exist ", 4);
//                    $("#txtUsername").val('');
//                    $('#txtUsername').focus();
//            //        break;
//            //    }
//            //}

//        }
//    }

//});

$("#txtPassword,#txtCNFPassword").change(function () {

    var paswd = /^(?=.*[0-9])(?=.*[A-Za-z])(?=.*[!@#$%&*.])[a-zA-Z0-9!@#$%^&*.]{6,12}$/;
    if (document.getElementById(this.id).value != "") {

        if (document.getElementById(this.id).value.match(paswd)) {

            if (this.id == "txtCNFPassword" && $("#txtCNFPassword").val().trim() != "") {


                if ($("#txtPassword").val() != "") {

                    if ($("#txtPassword").val().trim() != $("#txtCNFPassword").val().trim()) {


                        $app.showAlert('Password And Confirm Password Should be Same', 4);
                        $("#txtCNFPassword").val('');
                        $("#txtCNFPassword").focus();
                        return false;
                    }
                }
            }

            return true;
        }
        else {

            $app.showAlert("Your password should be contain atleast length 6 to 12 Characters ,It should contain atleast one numeric digit  one special character and one alphabet  ", 4);
            $('#' + this.id).val('');
            $('#' + this.id).focus();
            return false;
        }
    }



});

$("#btnSave").on('click', function (event) {

    var error = 0;
    $(".Reqrd").each(function () {

        if (document.getElementById(this.id).value == "") {

            $app.showAlert('Please ' + $(this).attr('placeholder'), 'danger');
            error = 1;
            return false;
        }

        if ($("#txtPassword").val().trim() != $("#txtCNFPassword").val().trim()) {


            $app.showAlert('Password And Confirm Password Should be same', 4);
            $("#txtCNFPassword").val('');
            $("#txtCNFPassword").focus();
            return false;
        }





    });


    if (error == 0) {

        $User.UserRegistration();
    }





});
//$("#frmUser").on('submit', function (event) {
//            if ($app.requiredValidate("frmUser", event)) {
//               // $User.save();
//                return false;
//            }
//        });
var $User = {
    UserId: '',
    filePath: '',
    formData: document.forms["frmUser"],
    formRegData: document.forms["frmUserReg"],
    rightsvalue: '00000000-0000-0000-0000-000000000000',
    UserAll: '',
    EmployeeAction: '',
    edit:null,
    datalength:null,
    fileUploadData: null,
    bindEvent: function () {


        $('#txtprofile').on('change', function (e) {
            debugger;
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
                    $User.fileUploadData = data;

                }
                else {
                    $app.showAlert("This browser doesn't support HTML5 file uploads!", 3);
                    $User.fileUploadData = data;
                    return false;
                }

            }
        });

    },

    LoadUser: function () {

        $User.allUser();
        $User.bindEvent();
        $companyCom.loadFormdatafullrole({ id: "sltRolelist" });
        // $('#tblUser').dataTable().fnClearTable();
        var dtClientList = $('#tblUser').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                       { "data": "Id" },
                       { "data": "Username" },
                       { "data": "Password" },
                       { "data": "FirstName" },
                       { "data": "LastName" },
                       { "data": "Email" },
                       { "data": "Phone" },
                      { "data": "UserRole" },
                       { "data": "RoleName" },
                       { "data": "ProfileImage" },
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
             "sClass": "word-wrap",
             render: function (nTd, sData, oData, iRow, iCol) {                   
                 var color = 'green';
                 var title='Active User'
                 if (oData.IsActive==false) {
                     color = 'red';
                     title = 'InActive User'
                 }
                
                 return '<span title='+title+' style="color:' + color + '">' + oData.Username + '</span>';
             }


         },
          {
              "aTargets": [2],
              "sClass": "nodisp"

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
                   "sClass": "nodisp"

               },
               {
                   "aTargets": [8],
                   "sClass": "word-wrap"

               },
                {
                    "aTargets": [9],
                    "sClass": "word-wrap",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        
                        var path = oData.ProfileImage.replace('~/', '');
                        var imgpath =path==""?"": $rootUrl + path;// path.substring(0, 1) == '/' ? path.substring(1) : path;
                        var b = $('<img id="imgUserProfileView" src="' + imgpath + '" class="img-circle img-inline" style="max-height:50px;max-width:50px;" />');
                        $(nTd).empty();
                        $(nTd).prepend(b);
                    }
                },


      {
          "aTargets": [10],
          "sClass": "actionColumn"
                    ,
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  // $User.AddInitialize();
                  $User.edit = true;
                  $User.GetUserData(oData);
                  document.getElementById('txtUsername').readOnly = true;
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $User.DeleteData({ Id: oData.Id });
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
                    url: $app.baseUrl + "Admin/GetUser",
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
                var r = $('#tblUser tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblUser thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

        $User.LoadCompTable();
    },

    mailcheck: function () {

        var email = {
            id: $User.UserId,
            Username: $($User.formData).find('#txtUsername').val(),
            Email: $($User.formData).find('#txtEmail').val().trim()
        };
        $.ajax({
            url: $app.baseUrl + "Admin/EmailCheck",
            data: JSON.stringify({ dataValue: email }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $('#txtEmail').val('');
                        $('#txtEmail').focus();
                        break;
                }
            },

        });
    },





    // Modified By Keerthika on 23/05/2017
    save: function () {

        debugger;
        if ($User.fileUploadData != null) {
            var ext = $('#txtprofile').val().split('.').pop().toLowerCase();
            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                $app.showAlert('invalid image Format!', 4);
                return false;
            }
            $User.saveProfileImage();
            return false;
        }
        var rows = $('#tblCompanygrid').dataTable().fnGetNodes();
        var company = '';
        var rightson = '';
        var rightsonvalue = '';
        var Myvariables = [];

        for (var i = 0; i < rows.length; i++) {

            if ($(rows[i]).find(".cbCompany").prop("checked") == true) {

                var myvariables = new Object();
                //  company = company + $(rows[i]).find(":eq(0)").html() + (',');
                company = $(rows[i]).find(":eq(0)").html()

                myvariables = {
                    companyId: $(rows[i]).find(":eq(0)").html(),
                    rightson: $("#cmbR" + company).val(),
                    rightsonvalue: $("#cmbV" + company).val()
                };
                Myvariables.push(myvariables);
            }

        }

        $app.showProgressModel();
        var isActive = false;
        if ($($User.formData).find('#rdUserActive').prop('checked') == true) {
            isActive = true;
        }
        var data1 = {

            id: $User.UserId,
            Username: $($User.formData).find('#txtUsername').val(),
            Password: $($User.formData).find('#txtPassword').val(),
            FirstName: $($User.formData).find('#txtFirstName').val(),
            LastName: $($User.formData).find('#txtLastName').val(),
            Email: $($User.formData).find('#txtEmail').val(),
            Phone: $($User.formData).find('#txtPhone').val(),
            UserRole: $($User.formData).find('#sltRolelist').val(),
            ConfirmPassword: $($User.formData).find('#txtCNFPassword').val(),
            ProfileImage: $User.filePath,
            IsActive: isActive,
            //  CompanyId:company,
            // $($User.formData).find('txtprofile').val()

            //   userCompanyMapping: { RightsOn: $("#ddcomponent").val(), RightsOnValue: $("#ddData").val() },
            // mappingcount: Myvariables.length,
            userCompanyMappingset: Myvariables,
            EmployeeId: $($User.formData).find('#sltEmployeeId').val() //---
        };
        //var data2 = {
        //    mappingcount: Myvariables.length,


        //};

        $.ajax({
            url: $app.baseUrl + "Admin/SaveUserData",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ userValue: data1 }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddUser').modal('toggle');
                        // $User.LoadUser();
                        $app.hideProgressModel();

                        $app.showAlert("User Registered Sucessfully", 2);

                        $User.LoadUser();
                        $User.AddInitialize();//---------added by Keerthika on 29/04/2017-----
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

        return false;

    },

    //Created by Keerthika on 10/06/2017
    allUser: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetAllUser",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: $('#hdnCompId').val() }),
            dataType: "json",
            success: function (msg) {

                var out = msg;
                $User.UserAll = out;
                //var value = $('#txtUsername').val();
                //for(var i=0;i<out.length;i++)
                //{
                //    if(out[i].Username.toLowerCase().trim()==value.toLowerCase().trim()&&out[i].Username.toLowerCase().trim()=="admin")
                //    {
                //        $app.showAlert("Already Exist " + $("#txtUsername").val(), 4);
                //        $("#txtUsername").val('');
                //        //$('#txtUsername').val("");
                //        //$('#txtUsername').focus();
                //        //alert("UserName Already Exist");


                //    }
                //}

            },
            error: function (msg) {
            }
        });
    },
    //---------------
    saveEmpProfileImage: function () {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Admin/SaveUserImage",
            data: $User.fileUploadData,
            processData: false,
            contentType: false,
            type: "POST",
            async: false,
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $app.hideProgressModel();
                        var p = jsonResult.result;
                        $User.filePath = p;
                        $User.fileUploadData = null;
                        $User.UserRegistration();
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
    }
    ,
    //---------------
    //Created by Keerthika on 19/05/2017
    LoadCompTable: function () {

        //  $User.bindEvent();
        var dtCompanyList = $('#tblCompanygrid').DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            'bDestroy': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [


            { "data": "Id" },
                {
                    "data": null
                },
              {
                  // "data":"Id",
                  "data": "CompanyName"

              },
             {
                 "data": null
             },
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
                "sClass": "actionColumn",
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    debugger;
                    var a = "";
                    if ($User.edit && $User.datalength == 1)
                    {
                       a = $('<input type="checkbox" class="cbCompany" id="chk' + oData.Id + '" checked/>');
                    }
                    else {
                        a = $('<input type="checkbox" class="cbCompany" id="chk' + oData.Id + '"/>');
                    }
                   
                    $(nTd).html(a);
                }

            },
            {
                "aTargets": [2],
                "sClass": "word-wrap"

            },

            {
                "aTargets": [3],
                "sClass": "actionColumn",
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                    var b = $('<select  class="sltRights" id="cmbR' + oData.Id + '"  onchange=" $User.LoadPopupforRights( ' + oData.Id + ' );"/> ')

                    $(nTd).html(b);
                }

            },

              {
                  "aTargets": [4],
                  "sClass": "actionColumn",
                  "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                      var c = $('<select class="sltRightsValue"  id="cmbV' + oData.Id + '"  /> ');
                      $(nTd).html(c);
                  }

              },






            ]
        ,
            //"aaData": data,
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Company/GetCompany",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ getempaction: $User.EmployeeAction }),
                    dataType: "json",
                    success: function (jsonResult) {

                        var Rdata = jsonResult;
                        var out = Rdata;
                        $User.datalength = Rdata.length;
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


                var result =
                 [{ Key: "CategoryId", Value: "Category" },
                     { Key: "Branch", Value: "Branch" },
                     { Key: "Designation", Value: "Designation" },
                     { Key: "CostCentre", Value: "CostCentre" },
                                 { Key: "ESILocation", Value: "ESILocation" },

                                   { Key: "Grade", Value: "Grade" },
                                    { Key: "Department", Value: "Department" },
                                                         { Key: "ESIDespensary", Value: "ESIDespensary" },
                                                    { Key: "Location", Value: "Location" },
                                                    { Key: "PTLocation", Value: "PTLocation" },
                                                     { Key: "Bank", Value: "Bank" },

                 ]
                $('#tblCompanygrid tbody tr').each(function () {

                    var id = $(this).find(' .sltRights').attr('id');
                    //       var dept = $(this).find('td:nth-child(1)').html();

                    $('#' + id).html('');
                    $('#' + id).append($("<option></option>").val(0).html('ALL'));
                    // $('#' + id).append($("<option></option>").val(1).html('Category'));
                    $.each(result, function (index, item) {

                        $('#' + id).append($("<option></option>").val(item.Key).html(item.Value));
                    });

                    //if ($('#' + id).attr('class').replace('form-control ', '').length > 0) {
                    //    $("#" + id + " option:contains(" + '(' + $('#' + id).attr('class').replace('form-control ', '') + ')' + ")").attr('selected', true);
                    //}

                });

            }

        });

    },

    //Created by Keerthika on 05/06/2017
    //---------
    imageChange: function (e) {
        debugger;
        var files = e.target.files;
        if (files.length > 0) {
            var file = files[0];
            fileName = file.name;
            size = file.size;
            type = file.type;
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append(files[x].name, files[x]);
                }
                $User.fileUploadData = data;

            }
            else {
                $app.showAlert("This browser doesn't support HTML5 file uploads!", 3);
                $User.fileUploadData = data;
                return false;
            }

        }
    },
    //-----------
    UserRegistration: function () {
        debugger;

        if ($User.fileUploadData != null) {
            var ext = $('#txtprofile').val().split('.').pop().toLowerCase();
            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                $app.showAlert('invalid image Format!', 4);
                return false;
            }
            $User.saveEmpProfileImage();
            return false;
        }
       



        //  company = company + $(rows[i]).find(":eq(0)").html() + (',');




        $app.showProgressModel();
        var data1 = {
            id: $User.UserId,
            Username: $('#txtUsername').val(),
            Password: $('#txtPassword').val(),

            //  LastName: $($User.formData).find('#txtLastName').val(),
            Email: null,
            Phone: $('#txtPhone').val(),

            ProfileImage: $User.filePath,
            //  CompanyId:company,
            // $($User.formData).find('txtprofile').val()

            //   userCompanyMapping: { RightsOn: $("#ddcomponent").val(), RightsOnValue: $("#ddData").val() },
            userCompanyMapping: [{ CompanyId: $("#hdnCompId").val(), RightsOn: 0 }],
            EmployeeId: $("#hdnid").val()

        };
        $.ajax({
            url: $app.baseUrl + "Admin/SaveUserEmpData",
            data: JSON.stringify({ dataValue: data1 }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        $app.hideProgressModel();

                        $app.showAlert("User Registered Successfully", 2);
                        $('#btnSave').prop('disabled', true);
                        $('#txtUsername').val('');
                        $('#txtPassword').val('');
                        $("#txtCNFPassword").val('');
                        $('#txtPhone').val('');
                        $('#txtUsername').val().prop('disabled', true);
                        $('#txtPassword').val().prop('disabled', true);
                        $('#txtPhone').val().prop('disabled', true);
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
        return false;

    },
    //--------------------------

    AddInitialize: function () {

        $User.UserId = '';
        document.getElementById('txtUsername').readOnly = false;
         $($User.formData).find('#txtUsername').val('').prop('disabled', false);
        $($User.formData).find('#txtUsername').val('');
        $($User.formData).find('#txtPassword').val('');
         $($User.formData).find('#txtFirstName').val('').prop('disabled', false);
        $($User.formData).find('#txtLastName').val('').prop('disabled', false);
        $($User.formData).find('#txtFirstName').val('');
        $($User.formData).find('#txtLastName').val('');
        $($User.formData).find('#txtEmail').val('').prop('disabled', true);
        $($User.formData).find('#txtPhone').val('').prop('disabled', true);
        $($User.formData).find('#sltRolelist').val("0");
        $($User.formData).find('#txtCNFPassword').val('');
        $($User.formData).find('#sltEmployeeId').val("00000000-0000-0000-0000-000000000000").prop('disabled', false);
        $($User.formData).find('#txtprofile').val('');
        $($User.formData).find('#imgUserProfileView').attr('src', '');
        $('.cbCompany').prop("checked", false);
        $User.EmployeeAction = "";
        $User.LoadCompTable();
        $User.filePath = '';
        $('#txtprofile').val('');
    },
    GetUserData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Admin/GetUserData",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddUser').modal('toggle');
                        var p = jsonResult.result;

                        if (p.UserRole == 2) {

                            $User.EmployeeAction = "Employee";
                            $User.LoadCompTable();
                        }

                        $User.RenderData(p);
                        $User.renderCompanyTable(p);
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
    GetRegData: function () {
        $('#txtUsername').prop('disabled', false);
        var Id = $('#sltEmployeeId').val();
        $.ajax({
            url: $app.baseUrl + "Admin/GetRegisterData",
            data: JSON.stringify({ id: Id }),
            dataType: "json",
            contentType: "application/json",

            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
                        if (p.Id != 0) {
                            $User.RenderData(p);
                            $('#txtUsername').prop('disabled', true);
                            $app.showAlert("This User Was Already get Registered", 3);
                            $User.EmployeeAction = $("#sltRolelist :selected").text().trim();
                            $User.LoadCompTable();
                        }
                        else {
                            $User.GetEmployeeData();
                            $app.showAlert("Welcome, Kindly Proceed the Registration Process", 2);
                            // $("#txtUsername").attr("disabled", "disabled");
                        }
                        break;
                    case false:
                        //$User.GetEmployeeData();
                        break;
                }
            },
            complete: function () {

            }
        });

    },
    //-----------Created By Keerthika S on 24/04/2017-----
    GetEmployeeData: function () {

        var sltEmpId = $('#sltEmployeeId').val();
        var selectedText = $('#sltEmployeeId').find("option:selected").text();
        //if (selectedText != "--Select--") {
        //    $('#txtUsername').val(selectedText).prop('disabled', true);
        //}
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmployeeData",
            data: JSON.stringify({ empId: sltEmpId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {


                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                  
                    case true:
                        debugger;
                        //   $('#sltEmployeeId').modal('toggle');
                        var p = jsonResult.result;
                        // $User.RenderData(p);
                        if (selectedText != "--Select--") {
                            // ('#txtPassword').val(data.Password);
                            if (p.empcreationtype == true) {
                                //  $('#txtUsername').val(p.empEmail).prop('disabled', true);
                            }
                            else {
                                // $('#txtUsername').val(selectedText).prop('disabled', true);
                            }

                            $('#txtFirstName').val(p.empFName).prop('disabled', true);
                            $('#txtLastName').val(p.empLName).prop('disabled', true);
                            $('#txtEmail').val(p.empEmail).prop('disabled', true);
                            $('#txtPhone').val(p.empPhone).prop('disabled', true);
                            $($User.formData).find('#imgUserProfileView').attr('src', p.EmployeeImage);
                        }
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
    //---------------------------
    DeleteData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Admin/DeleteUserData",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $User.LoadUser();
                        $app.showAlert(jsonResult.Message, 2);
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
    RenderData: function (data) {
        debugger;
        
        $User.UserId = data.Id;
        $($User.formData).find('#txtUsername').val(data.Username);
        $($User.formData).find('#txtPassword').val(data.Password);
        $($User.formData).find('#txtFirstName').val(data.FirstName).prop('disabled', true);
        $($User.formData).find('#txtLastName').val(data.LastName).prop('disabled', true);
        $($User.formData).find('#txtEmail').val(data.Email).prop('disabled', true);
        $($User.formData).find('#txtPhone').val(data.Phone).prop('disabled', true);
        $($User.formData).find('#sltEmployeeId').val(data.EmployeeId).prop('disabled', true);
        // $('#txtCNFPassword').val(data.ConfirmPassword);
        // $($User.formData).find('#txtprofile').val(data.ProfileImage);
        $($User.formData).find('#txtCNFPassword').val(data.Password);
        $User.filePath = data.ProfileImage;
        $($User.formData).find('#imgUserProfileView').attr('src', '');
        var path = data.ProfileImage.replace('~/', '');
        var imgpath = path == "" ? "" : $rootUrl + path;
        $($User.formData).find('#imgUserProfileView').attr('src', imgpath);
        //  $($User.formData).find('#txtprofile').val(data.ProfileImage);
        $($User.formData).find('#sltRolelist').val(data.UserRole);
        // $($User.formData).find('#txtUsername').prop('disabled', false);

        //  $('#lblCNF').hide();
        //  $($User.formData).find('#ddcomponent').val(data.userCompanyMapping.RightsOn);
        // $User.rightsvalue = data.userCompanyMapping.RightsOnValue;
        //  $User.LoadCompTable(); //--
        if (data.IsActive) {
            $($User.formData).find('#rdUserActive').prop('checked', true);
        }
        else {
            $($User.formData).find('#rdUserInActive').prop('checked', true);
        }
        //   $User.LoadPopupforRights(data.userCompanyMapping.RightsOn); 
        $('.cbCompany').prop("checked", false);

    },
    renderCompanyTable: function (data) {

        var rows = $('#tblCompanygrid').dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            $(rows[i]).find($("#chk" + rows[i].CompanyId)).prop("checked", false);

            for (var j = 0; j < data.userCompanyMapping.length; j++) {

                if (data.userCompanyMapping[j].CompanyId == $(rows[i]).find(":eq(0)").html()) {

                    $(rows[i]).find($("#chk" + data.userCompanyMapping[j].CompanyId)).prop("checked", true);
                    $(rows[i]).find($("#cmbR" + data.userCompanyMapping[j].CompanyId)).val(data.userCompanyMapping[j].RightsOn);

                    $User.LoadPopupforRights(data.userCompanyMapping[j].CompanyId);
                    $(rows[i]).find($("#cmbV" + data.userCompanyMapping[j].CompanyId)).val(data.userCompanyMapping[j].RightsOnValue);

                }

            }
        }
    },
    saveProfileImage: function () {

        $.ajax({
            url: $app.baseUrl + "Admin/SaveUserImage",
            //data: $User.fileUploadData,
            processData: false,
            contentType: false,
            type: "POST",
            async: false,
            success: function (jsonResult) {
                debugger;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        $app.hideProgressModel();
                        var p = jsonResult.result;
                        $User.filePath = p;
                        $User.fileUploadData = null;
                        $User.save();
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
    }
    ,
    //----- created by Keerthika on 31/05/2017--
    loadCategoryComp: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetCategoriesComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg.result;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(out, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.Name));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadBranchComp: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetBranchesComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.BranchName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadCostCentreComp: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetCostcentresComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.CostCentreName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadDesignationComp: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetDesignationsComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.DesignationName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadDepartmentComp: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetDepartmentsComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.DepartmentName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadLocationComp: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetLocationComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, Loc) {
                    $('#' + dropControl.id).append($("<option></option>").val(Loc.Id).html(Loc.LocationName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadEsiLocationComp: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetEsilocationComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, Loc) {
                    $('#' + dropControl.id).append($("<option></option>").val(Loc.Id).html(Loc.LocationName));
                });
            },
            error: function (msg) {

            }
        });

    },
    loadPTLocationComp: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetPTLocationsComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {

                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, PTLoc) {
                    $('#' + dropControl.id).append($("<option></option>").val(PTLoc.Id).html(PTLoc.PTLocationName));
                });
            },
            error: function (msg) {
                //alert("al");
            }
        });

    },
    loadESIDespensaryComp: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetESIDespensarysComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.ESIDespensary));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadGradeComp: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetGradesComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ companyId: dropControl.compid }),
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.GradeName));
                });
            },
            error: function (msg) {
            }
        });

    },
    //-----

    LoadPopupforRights: function (id) {

        popup = $("#cmbR" + id).val().toLowerCase();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetPopUpDataComp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ type: popup, companyId: id }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        switch ($("#cmbR" + id).val()) {
                            case "CategoryId":
                                $User.loadCategoryComp({ id: "cmbV" + id, compid: id });


                                break;
                            case "Branch":
                                $User.loadBranchComp({ id: "cmbV" + id, compid: id });

                                break;
                            case "Designation":
                                $User.loadDesignationComp({ id: "cmbV" + id, compid: id });

                                break;

                            case "CostCentre":
                                $User.loadCostCentreComp({ id: "cmbV" + id, compid: id });

                                break;
                            case "ESILocation":
                                $User.loadEsiLocationComp({ id: "cmbV" + id, compid: id });

                                break;
                            case "Grade":
                                $User.loadGradeComp({ id: "cmbV" + id, compid: id });

                                break;
                            case "ESIDespensary":
                                $User.loadESIDespensaryComp({ id: "cmbV" + id, compid: id });

                                break;
                            case "Department":
                                $User.loadDepartmentComp({ id: "cmbV" + id, compid: id });

                                break;
                            case "Location":
                                $User.loadLocationComp({ id: "cmbV" + id, compid: id });

                                break;

                            case "PTLocation":
                                $User.loadPTLocationComp({ id: "cmbV" + id, compid: id });

                                break;

                            case "Bank":
                                $companyCom.loadBank({ id: "cmbV" + id });

                                break;
                            default:
                                $("#cmbV" + id).val('');
                                $("#cmbV" + id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('All'));
                                break;
                        }
                        document.getElementById("cmbV" + id).options[0].innerHTML = "All";
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);

                }

            },
            error: function (msg) {
            }
        });

    },
    CheckAlreadyExist: function (data) {

        var count = 0;
        var rows = $("#tblEmpCode").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            if (data.val().trim().toLowerCase() == $(rows[i]).find(".selectxlColumn").val().trim().toLowerCase()) {

                count++;
                if (count > 1) {

                    data.val(data.closest('tr').find(":eq(1)").html().trim());
                    $app.showAlert("Employee Code Already Exist", 4);
                    return false;
                }
            }
        }

        if (data.val().trim() == "") {
            data.val(data.closest('tr').find(":eq(1)").html().trim());
        }



    },
    SaveEmployeeCode: function () {

        var data = [];


        var rows = $("#tblEmpCode").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {



            var datum = new Object();

            datum.empid = $(rows[i]).find(":eq(0)").html().trim();
            datum.empCode = $(rows[i]).find(".selectxlColumn").val().trim();
            datum.oldCode = $(rows[i]).find(":eq(1)").html().trim();

            data.push(datum)

        }

        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmployeeCode",
            contentType: "application/json",
            data: JSON.stringify({ attr: data }),
            dataType: "json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $User.LoadEmployeeCode();
                        $app.hideProgressModel();
                        var p = jsonResult.result;
                        $app.showAlert(jsonResult.Message, 2);
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
    LoadEmployeeCode: function () {

            var dtClientList = $('#tblEmpCode').DataTable({
                'iDisplayLength': 10,
                'bPaginate': true,
                'sPaginationType': 'full',
                'sDom': '<"top">rt<"bottom"ip><"clear">',
                columns: [

                         { "data": "empid" },
                          { "data": "empCode" },
                         { "data": "empFName" },
                         { "data": "empCode" }
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
                            "aTargets": [3],
                            "sClass": "word-wrap"
                        },


               {
                   "aTargets": [3],
                   "sClass": "actionColumn",

                   "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                       var c = $('<input type="text" class="selectxlColumn" onchange="$User.CheckAlreadyExist($(this))" onkeypress="return $validator.alphanumericonly(event, this.id)">');
                       c.button();
                       c.val(sData);
                       c.on('click', function () {
                           //$LeaveReport.LoadPendingStatPopup(sData);
                           //$leave.intializereason(data = 'Cancel');

                       });

                       $(nTd).empty();
                       $(nTd).prepend(c);
                   }
               }],
                ajax: function (data, callback, settings) {

                    $.ajax({
                        type: 'POST',
                        url: $app.baseUrl + "Employee/GetEmployees",
                        contentType: "application/json; charset=utf-8",
                        data: null,
                        dataType: "json",
                        success: function (jsonResult) {

                            var out = jsonResult.result;
                            $app.clearSession(jsonResult);
                            switch (jsonResult.Status) {

                                case true:
                                    setTimeout(function () {
                                        callback({
                                            draw: data.draw,
                                            data: out,

                                        });

                                    }, 50);
                                    break;
                                case false:
                                    $app.showAlert(jsonResult.Message, jsonResult.StatusCode);
                                    break;
                            }

                        },
                        error: function (msg) {
                        }
                    });
                },
                "order": [[1, "asc"], [2, 'asc']],
                fnInitComplete: function (oSettings, json) {

                },
                dom: "rtiS",
                "bDestroy": true,
                scroller: {
                    loadingIndicator: true
                }
            });
        }

};