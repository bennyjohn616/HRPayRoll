﻿$('#btnSaveJoinDoc').on('click', function () {
    $joinDoc.AddInitialize();

});
$('#btnempjoiningdoc').on('click', function () {
    $('#sltempDocRequired').val('00000000-0000-0000-0000-000000000000');
    $('#fjoinDocUpload').val('');

});
$('#emp_joiningdocument').addClass('nodisp', function () {
    return false;
});
$('#frmJoinDoc').on('submit', function () {
    $joinDoc.save();
    return false;
});

$("#txtJoinDocName").change(function () {
    $("#tblJoinDoc tbody tr").each(function () {
        if ($("#txtJoinDocName").val().toLowerCase() == $(this).find("td:nth-child(2)").html().toLowerCase()) {
            $app.showAlert("Already Exist " + $("#txtJoinDocName").val(), 4);
            $("#txtJoinDocName").val('');
            return false;
        }
    });
});

$('#fjoinDocUpload').on('change', function (e) {
    debugger
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
            $joinDoc.fileData = data;

        }
        else {
            $app.showAlert("This browser doesn't support HTML5 file uploads!", 3);
            $joinDoc.fileData = null;
            return false;
        }

    }
});

var $joinDoc = {
    canSave: false,
    joinDocId: '',
    joindoctable: 'tblJoinDoc',
    tableId: 'tblempjoiningdoc',
    selectedEmployeeId: null,
    fileData: null,
    AddInitialize: function () {
        var formData = document.forms["frmJoinDoc"];
        $joinDoc.joinDocId = '';
        formData.elements["txtJoinDocName"].value = "";
        $joinDoc.canSave = true;

    },
    LoadJoinDoc: function () {
        var dtClientList = $('#tblJoinDoc').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "Id" },
                    { "data": "DocumentName" },
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
             "sClass": "nodisp"

         },
      {
          "aTargets": [3],
          "sClass": "actionColumn",
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  $joinDoc.GetPopupData(oData);
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $joinDoc.DeleteData({ Id: oData.Id });
                  }
                  return false;
              });
              $(nTd).empty();
              $(nTd).prepend(b, c);


          }
      }],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Company/GetJoiningDocuments",
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
                var r = $('#tblJoinDoc tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblJoinDoc thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    GetPopupData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Company/GetPopupData",
            data: JSON.stringify({ id: context.Id, type: "JoiningDocument" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddJoinDocument').modal('toggle');
                        var p = jsonResult.result;
                        $joinDoc.RenderData(p);
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
        $joinDoc.canSave = true;
        var formData = document.forms["frmJoinDoc"];
        $joinDoc.joinDocId = data.Id;
        formData.elements["txtJoinDocName"].value = data.popuplalue;
    },
    save: function () {
        if (!$joinDoc.canSave) {
            return false;
        }
        $joinDoc.canSave = false;
        $app.showProgressModel();
        var formData = document.forms["frmJoinDoc"];
        var data = {
            id: $joinDoc.joinDocId,
            popuplalue: formData.elements["txtJoinDocName"].value,
            type: "JoiningDocument"
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
                        $('#AddJoinDocument').modal('toggle');
                        $joinDoc.LoadJoinDoc();
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
    DeleteData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Company/DeletePopupData",
            data: JSON.stringify({ id: context.Id, type: "JoiningDocument" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $joinDoc.LoadJoinDoc();
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

    //Employee related code
    initiateFormEmployee: function (employeeId) {
        $joinDoc.selectedEmployeeId = employeeId.id;
        $joinDoc.createjoindocGrid();
        $companyCom.loadJoinDocdata({ id: "sltempDocRequired" })
    },
    createjoindocGrid: function () {
        debugger;
        var gridObject = $joinDoc.joindocumentGridObject();
        var tableid = { id: $joinDoc.joindoctable };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvEmpjoiningdocument').html(modelContent);
        $joinDoc.Loadjoindocument(gridObject, tableid);
    },
    joindocumentGridObject: function () {
        var gridObject = [
                { tableHeader: "id", tableValue: "joingDocumentId", cssClass: 'nodisp' },
                 { tableHeader: "Path", tableValue: "filePath", cssClass: 'nodisp' },
                { tableHeader: "Documents Required", tableValue: "documentName", cssClass: 'popup' },
                { tableHeader: "Status", tableValue: "status", cssClass: '' },
        ];
        return gridObject;
    },
    Loadjoindocument: function (context, tableId) {
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'popup') {
                columnDef.push(
                       {
                           "aTargets": [cnt],
                           "sClass": "actionColumn",
                           "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                               var b = $('<a href="#" class="editeButton"><span>' + sData + '</span></a>');
                               b.on('click', function () {
                                   if (oData.status == 'Given') {
                                       $app.downloadSync('Download/DownloadEmpJoinDoc', oData);
                                       return false;
                                   }
                                   else {
                                       $app.showAlert('File not given by you.', 1);
                                       return false;
                                   }
                                   return false;
                               });
                               $(nTd).html(b);
                           }
                       }

                   ); //for action column

            }
            else if (context[cnt].cssClass == 'actionColumn') {
                columnDef.push(
                        {
                            "aTargets": [cnt],
                            "sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');

                                b.button();
                                b.on('click', function () {
                                    $joinDoc.GetEmpJoiningDoc({ Id: oData.joinDocId });
                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    if (confirm('Are you sure ,do you want to delete?')) {
                                        $joinDoc.DeleteLoanEntryData({ Id: oData.joinDocId });
                                    }
                                    return false;
                                });
                                $(nTd).empty();
                                $(nTd).prepend(e, b, c);
                            }
                        }

                    ); //for action column
            }
            else if (context[cnt].cssClass == 'edit') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="text" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
                        $(nTd).html(b);
                    }
                });

            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetEmpJoiningDoc",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ 'employeeId': $joinDoc.selectedEmployeeId }),
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                var out = jsonResult.result;
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
                                $app.showAlert(jsonResult.Message, 4);

                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#' + tableId.id + ' tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + tableId.id + ' thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    saveEmpjoindoc: function () {
        debugger
        var formData = document.forms["frmEmpJoinDoc"];
        //var data = {
        //    id: $joinDoc.joinDocId,
        //    employeeId: $joinDoc.selectedEmployeeId,
        //    joingDocumentId: formData.elements["sltempDocRequired"].value

        //};
        $joinDoc.fileData.append('EmpJoiningDocId', '');// $joinDoc.joinDocId
        $joinDoc.fileData.append('employeeId', $joinDoc.selectedEmployeeId);
        $joinDoc.fileData.append('joingDocumentId', formData.elements["sltempDocRequired"].value);
        // $joinDoc.fileData.append('EmpJoiningDoc', data);
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpJoiningDoc",
            data: $joinDoc.fileData,//JSON.stringify({ dataValue: data }),//
            //  dataType: "json",
            // contentType: "application/json",
            processData: false,
            contentType: false,
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddEmp_JoiningDocument').modal('toggle');
                        $joinDoc.createjoindocGrid();
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

            }
        });
    },
    renderempjoindoc: function (data) {
        var gridObject = $joinDoc.joindocumentGridObject();
        var tableid = { id: 'tblempjoiningdoc' };
        var popup = $screen.createPopUp({ id: 'frmEmpJoinDoc', title: 'Employee Joining Documents' });//, button: { id: 'loanSave' }
        var modelContent = $screen.createTable(tableid, gridObject);
        document.getElementById("renderId").innerHTML = popup.replace('{ModelBody}', modelContent);
        $joinDoc.loadempjoindoc(data, gridObject, tableid);
    },
    //need to check is this function required
    edit: function (context) {
        $joinDoc.RenderData(context);

    },
    downloadFile: function (context) {

        $.ajax({
            url: $app.baseUrl + "Download/DownloadFile",
            data: JSON.stringify({ path: context.filePath }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $joinDoc.LoadJoinDoc();
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
    }
};
























