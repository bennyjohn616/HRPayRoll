$RoleFormCommandSetting = {
    canSave: false,
    roleId: '',
    formCommandId: '',
    id: '',
    UserId: '',
    selectedId: '',
    selectedComponent: '',
    roleFormCommandObject: null,

    //-- Modified by Keerthika on 09/05/2017
    loadInitial: function () {

        $companyCom.loadFormdata({ id: "sltFormRolelist" });
        $("#sltFormRolelist").change(function () {

            $RoleFormCommandSetting.canSave = true;
            //  $RoleFormCommandSetting.selectedComponent = $("#sltFormRolelist option:selected").text();
            $RoleFormCommandSetting.selectedComponent = $("#sltFormRolelist").val();
            //  $RoleFormCommandSetting.selectedId = $("#sltFormRolelist").val();
            $RoleFormCommandSetting.LoadFormRights({ Id: $RoleFormCommandSetting.selectedComponent });
        });
        $('#btnSave').on('click', function () {

            if (!$RoleFormCommandSetting.canSave) {
                return false;
            }
            $RoleFormCommandSetting.canSave = false;
            $RoleFormCommandSetting.Save();

        });
    },
    //Modified by Keerthika on 10/05/2017
    LoadFormRights: function (context) {

        $('#tblFormRights').removeClass('nodisp');
        var dtClientList = $('#tblFormRights').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'searching': true,
            'sDom': '<"top">rt<"bottom"ip><"clear">',

            dom: "Bfrtip",
            columns: [
          { "data": "id" },
          { "data": "formCommandId" },
          { "data": "roleId" },
          { "data": "commandName" },
           { "data": "commandType" },
          { "data": "isRead" },
          { "data": "isWrite" },
          { "data": "isDelete" },
          { "data": "isApproval" },
          { "data": "isRequired" },
          { "data": "isPayrollTransaction" }


            ],
            "aoColumnDefs": [
        {
            "aTargets": [0, 1, 2],
            "sClass": "nodisp",
            "bSearchable": false
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
             "aTargets": [5],//read
             render: function (sdata, type, oData, row) {

                 if (type === 'display') {
                     var isChecked = (oData.isRead) ? ' checked=' + oData.isRead : '';
                     //  var isdisable = (oData.readDisable) ? ' disabled' : '';
                     return '<input type="checkbox" onchange="$RoleFormCommandSetting.UpdateRights(\'' + oData.id + '\',\'' + oData.formCommandId + '\',\'' + oData.roleId + '\',\'read\',this)" class="editor-active view"' + isChecked + '>';
                 }
                 return sdata;
             }
         },
         {
             "aTargets": [6],//write
             render: function (sdata, type, oData, row) {

                 if (type === 'display') {
                     var isChecked = (oData.isWrite) ? ' checked=' + oData.isWrite : '';
                     //  var isdisable = (oData.writeDisable) ? ' disabled' : '';
                     return '<input type="checkbox" onchange="$RoleFormCommandSetting.UpdateRights(\'' + oData.id + '\',\'' + oData.formCommandId + '\',\'' + oData.roleId + '\',\'write\',this)" class="editor-active view"' + isChecked + '>';
                 }
                 return sdata;
             }
         },
          {
              "aTargets": [7],//delete
              render: function (sdata, type, oData, row) {

                  if (type === 'display') {
                      var isChecked = (oData.isDelete) ? ' checked=' + oData.isDelete : '';
                      // var isdisable = (oData.readDisable) ? ' disabled' : '';//need to modify
                      return '<input type="checkbox" onchange="$RoleFormCommandSetting.UpdateRights(\'' + oData.id + '\',\'' + oData.formCommandId + '\',\'' + oData.roleId + '\',\'delete\',this)" class="editor-active view"' + isChecked + '>';
                  }
                  return sdata;
              }
          },
          {
              "aTargets": [8],//approve
               "sClass": "nodisp",
              render: function (sdata, type, oData, row) {

                  if (type === 'display') {
                      var isChecked = (oData.isApproval) ? ' checked=' + oData.isApproval : '';
                      //  var isdisable = (oData.approvalDisable) ? ' disabled' : '';
                      return '<input type="checkbox" onchange="$RoleFormCommandSetting.UpdateRights(\'' + oData.id + '\',\'' + oData.formCommandId + '\',\'' + oData.roleId + '\',\'approve\',this)" class="editor-active view"' + isChecked + '>';
                  }
                  return sdata;
              }
          },
          {
              "aTargets": [9],//required
              "sClass": "nodisp",
              render: function (sdata, type, oData, row) {

                  if (type === 'display') {
                      var isChecked = (oData.isRequired) ? ' checked=' + oData.isRequired : '';
                      //    var isdisable = (oData.requireDisable) ? ' disabled' : '';
                      return '<input type="checkbox" onchange="$RoleFormCommandSetting.UpdateRights(\'' + oData.id + '\',\'' + oData.formCommandId + '\',\'' + oData.roleId + '\',\'required\',this)" class="editor-active view"' + isChecked + '>';
                  }
                  return sdata;
              }
          }
          ,
          {
              "aTargets": [10],//transaction
              "sClass": "nodisp",
              render: function (sdata, type, oData, row) {

                  if (type === 'display') {
                      var isChecked = (oData.isPayrollTransaction) ? ' checked=' + oData.isPayrollTransaction : '';
                      // var isdisable = (oData.tranDisable) ? ' disabled' : '';
                      return '<input type="checkbox" onchange="$RoleFormCommandSetting.UpdateRights(\'' + oData.id + '\',\'' + oData.formCommandId + '\',\'' + oData.roleId + '\',\'tran\',this)" class="editor-active view"' + isChecked + '>';
                  }
                  return sdata;
              }
          }
            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Setting/GetRoleFormSetting",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ roleId: $RoleFormCommandSetting.selectedComponent }),
                    dataType: "json",
                    success: function (msg) {

                        var out = msg.result;
                        $RoleFormCommandSetting.roleFormCommandObject = out;
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

        var table = $('#tblFormRights').DataTable();
        $('#myInput').keyup(function () {

            table.search($(this).val()).draw();
        })
    },
    AddInitialize: function () {

        $RoleFormCommandSetting.settingId = '';
        $RoleFormCommandSetting.loadpopUp()
        $('#dvcatname').addClass('nodisp');
        // var formData = document.forms["frmEmpCodedetails"];
        // $Empcodesetting.settingId = '';
        //formData.elements["sltddLoan"].value = "0";       

    },
    loadpopUp: function () {

        $RoleFormCommandSetting.designDetailForm('dvEmpcodeHtml');
        $RoleFormCommandSetting.loadDetailComponent();
    },
    Save: function () {


        $.ajax({
            url: $app.baseUrl + "Setting/SaveRoleFormSetting",
            data: JSON.stringify({ roleId: $RoleFormCommandSetting.selectedComponent, dataValue: $RoleFormCommandSetting.roleFormCommandObject }), //--
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $RoleFormCommandSetting.canSave = true;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $RoleFormCommandSetting.canSave = true;
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
                $RoleFormCommandSetting.canSave = true;
            }
        });
    },
    UpdateRights: function (id, formcommandId, roleId, type, elemnt) {


        $.each($RoleFormCommandSetting.roleFormCommandObject, function (ind, obj) {

            if (obj.id == id && obj.formCommandId == formcommandId && obj.roleId == roleId) {
                var ischeke = $(elemnt).prop('checked');
                if (type == 'read') {
                    $RoleFormCommandSetting.roleFormCommandObject[ind].isRead = ischeke;
                }
                if (type == 'write') {
                    $RoleFormCommandSetting.roleFormCommandObject[ind].isWrite = ischeke;
                }
                if (type == 'delete') {
                    $RoleFormCommandSetting.roleFormCommandObject[ind].isDelete = ischeke;
                }
                if (type == 'approve') {
                    $RoleFormCommandSetting.roleFormCommandObject[ind].isApproval = ischeke;
                }
                if (type == 'required') {
                    $RoleFormCommandSetting.roleFormCommandObject[ind].isRequired = ischeke;
                }
                if (type == 'tran') {
                    $RoleFormCommandSetting.roleFormCommandObject[ind].isPayrollTransaction = ischeke;
                }
            }
        });
    }


};