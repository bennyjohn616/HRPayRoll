dtManagertbl = $('#tblManagerList').DataTable({
    'bPaginate': true,
    'sPaginationType': 'full',
    'sDom': '<"top">rt<"bottom"ip><"clear">'
});
dtEmployeetbl = $('#tblEmployeeList').DataTable({

    'bPaginate': true,
    'sPaginationType': 'full',
    'sDom': '<"top">rt<"bottom"ip><"clear">'
    , "columnDefs": [
    { "width": "20%", "targets": 1 },
     { "width": "20%", "targets": 2}
    ]
});
dtManagertbl.columns.adjust().draw();
dtEmployeetbl.columns.adjust().draw();

$("#sltMgrEmpCode").change(function () {
    debugger

    if ($('#sltMgrEmpCode').find('option:selected').text() != "--Select--") {

        $assignManager.managernameshow();
    }
    else {
        document.getElementById('lblMgrEmpCode').innerHTML ="";

    }


});
$("#sltChgMgrEmpCode").change(function () {
    debugger

    if ($('#sltChgMgrEmpCode').find('option:selected').text() != "--Select--") {

        $assignManager.changemanagernameshow();
    }
    else {

        document.getElementById('lblChgMgrEmpCode').innerHTML = "";
    }


});



var $assignManager = {
    savechangeassignmgr: function () {
        var savests = 0;
        
         if ($('#sltMgrEmpCode').find('option:selected').text() == "--Select--")
        {
             $app.showAlert("Please Select the Existing Manager code!!!", 4);
             savests = 1;
        }

        if ($('#sltChgMgrEmpCode').find('option:selected').text() == "--Select--")
        {
            $app.showAlert("Please Select the Change manager code!!!", 4);
            savests = 1;
        }
       if(savests == 0)
       {
           $.ajax({
               type: 'POST',
               url: $app.baseUrl + "LeaveRequest/SaveChangeManager",
               contentType: "application/json; charset=utf-8",
               data: JSON.stringify({ Exixtingmgrid: $("#sltMgrEmpCode").val(), Changemgrid: $("#sltChgMgrEmpCode").val() }),
               dataType: "json",
               success: function (jsonResult) {
                   var out = jsonResult.result;
                   switch (jsonResult.Status) {

                       case true:
                           
                           $app.hideProgressModel();
                           $app.showAlert(jsonResult.Message, 2);
                           break;
                       case false:
                           // $('#AddReason').modal('toggle');
                           $app.hideProgressModel();
                           $app.showAlert(jsonResult.Message, 4);
                           break;
                   }

               },
               error: function (msg) {
               }
           });

       }
       
    },
    addRowManager: function () {
         
        var valueid = $('#ddManager').val();
        var value = $('#ddManager option:selected').text();
        dtManagertbl.row.add([valueid, value, value]).draw(true);
    },
    addRowEmployee: function () {
        var valueid = $('#ddEmployee').val();
        var value = $('#ddEmployee option:selected').text();
        dtEmployeetbl.row.add([valueid, value, '', '', '', '']).draw(false);
    },
    
    managernameshow: function () {
        
        $app.showProgressModel();
      
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/Showmanagername",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EMPID: $("#sltMgrEmpCode").val() }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:
                        
                        $app.hideProgressModel();
                        document.getElementById('lblMgrEmpCode').innerHTML = out;
                        break;
                    case false:
                        // $('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
     changemanagernameshow: function () {
        
        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/Showmanagername",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EMPID: $("#sltChgMgrEmpCode").val() }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:
                        
                        $app.hideProgressModel();
                        document.getElementById('lblChgMgrEmpCode').innerHTML = out;
                        break;
                    case false:
                        // $('#AddReason').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },


    chgMgraddInitialize: function () {
        
        var formData = document.forms["frmChgAssMgr"];
        //var data = {
        //    id: $Role.RoleId,
        //    Name: formData.elements["txtName"].value,
        //    DisplayAs: formData.elements["txtDisplayAs"].value,
        //    Description: formData.elements["txtDescription"].value
        $($assignManager.formData).find('#sltMgrEmpCode').val('');
        $($assignManager.formData).find('#sltChgMgrEmpCode').val('');
    
}
}