$("#sltRolelist").change(function () {
    
    $formRights.selectedComponent = $("#sltRolelist").val();
    $formRights.LoadFormRights({ Id: $formRights.selectedComponent });
});
var $formRights = {
    UserId: '',
    selectedId: '',
    selectedComponent: '',
    loadInitial: function () {
        
        $companyCom.loadFormdata({ id: "sltRolelist" });
    },


    LoadFormRights: function (context) {      
        debugger;
       $('#tblFormRights').removeClass('nodisp');
        var dtClientList = $('#tblFormRights').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            "sSearch": "Search:",
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            dom: "Bfrtip",
            ajax: "../php/checkbox.php",

            columns: [
             { "data": "formid" },
             { "data": "formName" },
             { "data": "view" },
             { "data": "edit" },
                       { "data": "delete" },
              
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
         
             
                 render: function (data, type, row) {
                     if (type === 'display') {
                         var isChecked = (row.view) ? ' checked=' + row.view : '';
                         return '<input type="checkbox" onchange="$formRights.UpdateRights(\'' + row.formid + '\',this)" class="editor-active view"' + isChecked + '>';
                     }
                     return data;
                 }
             
         },

          {
              "aTargets": [3],
              render: function (data, type, row) {
                  if (type === 'display') {
                      var isChecked = (row.edit) ? ' checked=' + row.edit : '';
                      return '<input type="checkbox"  onchange="$formRights.UpdateRights(\'' + row.formid + '\',this)" class="editor-active edit"' + isChecked + '>';
                  }
                  return data;
              }

          },

          {
              "aTargets": [4],
              render: function (data, type, row) {
                  if (type === 'display') {
                      var isChecked = (row.delete) ? ' checked=' + row.delete : '';
                      return '<input type="checkbox"  onchange="$formRights.UpdateRights(\'' + row.formid + '\',this)" class="editor-active delete"' + isChecked + '>';
                  }
                  return data;
              }
          }

            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Admin/GetFormRightsData",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ role: context.Id }),
                    dataType: "json",
                    success: function (msg) {
                        
                        var out = msg.result;
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
                
                var r = $('#tblFormRights tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblFormRights thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

       
    },

    UpdateRights: function (context, cntrl) {
        
        var data = {
            FormId: context,
            RoleName: $("#sltRolelist").val(),
            ViewRights: $(cntrl).parent().parent().find('.view').is(":checked"),
            EditRights: $(cntrl).parent().parent().find('.edit').is(":checked"),
            DeleteRights: $(cntrl).parent().parent().find('.delete').is(":checked")
        };
        $.ajax({
            url: $app.baseUrl + "Admin/SaveRightsData",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        companyid = 0;
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            }
        });
    }
    , GetFormCommands: function () {
        debugger;
        $('#tblFormRights').removeClass('nodisp');
        var groupColumn = 5;
        var dtClientList = $('#tblFormRights').DataTable({
            'iDisplayLength': 30,
            'bPaginate': true,
            "sSearch": "Search:",
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            'drawCallback': function (settings) {
                debugger;
                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();
                var last = null;

                api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                    if (last !== group) {
                        $(rows).eq(i).before(
                            '<tr class="group" style="background-color:#94a8c1"><td colspan="5"><span"><font color="white" style="font-weight:bold">' + group + '</font></span></td></tr>'
                        );
                        last = group;
                    }
                });
            },
            dom: "Bfrtip",
            ajax: "../php/checkbox.php",

            columns: [
             { "data": "Id" },
             { "data": "Description" },
           
             { "data": "IsDefaultTransaction" },
             { "data": "IsDefaultRequired" },
             { "data": "IsDefaultApprovel" },
             { "data": "TableName" }
             
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
         
             
                 render: function (data, type, row) {
                     if (type === 'display') {
                         var isChecked = (row.IsDefaultTransaction) ? ' checked=' + row.IsDefaultTransaction : '';
                         return '<input type="checkbox" onchange="$formRights.UpdateFormCommands(\'' + row.Id + '\',this)" class="editor-active IsDefaultTransaction"' + isChecked + '>';
                     }
                     return data;
                 }
             
         },

          {
              "aTargets": [3],
              render: function (data, type, row) {
                  if (type === 'display') {
                      var isChecked = (row.IsDefaultRequired) ? ' checked=' + row.IsDefaultRequired : '';
                      return '<input type="checkbox"  onchange="$formRights.UpdateFormCommands(\'' + row.Id + '\',this)" class="editor-active IsDefaultRequired"' + isChecked + '>';
                  }
                  return data;
              }

          },
            {
                "aTargets": [4],
                render: function (data, type, row) {
                    if (type === 'display') {
                        var isChecked = (row.IsDefaultApprovel) ? ' checked=' + row.IsDefaultApprovel : '';
                        return '<input type="checkbox"  onchange="$formRights.UpdateFormCommands(\'' + row.Id + '\',this)" class="editor-active IsDefaultApprovel"' + isChecked + '>';
                    }
                    return data;
                }

            },
           {
               "aTargets": [5],
               "sClass": "nodisp"
           },

         

            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Admin/GetFormCommandsData",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ type:"Control"}),
                    dataType: "json",
                    success: function (msg) {
                        
                        var out = msg.result;
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
                
                var r = $('#tblFormRights tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblFormRights thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

       
    },
    UpdateFormCommands: function (context, cntrl) {
        debugger;
        var data = {
            Id: context,
            IsDefaultTransaction: $(cntrl).parent().parent().find('.IsDefaultTransaction').is(":checked"),
            IsDefaultRequired: $(cntrl).parent().parent().find('.IsDefaultRequired').is(":checked"),
            IsDefaultApprovel: $(cntrl).parent().parent().find('.IsDefaultApprovel').is(":checked")
        };
        $.ajax({
            url: $app.baseUrl + "Admin/UpdateFormCommands",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        companyid = 0;
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            }
        });
    }
}

