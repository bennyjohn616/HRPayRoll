var $ManagerEligiblity = {
    loadinitilaize: function () {
        debugger;
        $Roleform.designForm('RoleTable');
        $Roleform.loadComponent();
        $ManagerEligiblity.GetManagerEligiblity();

       
    },
    SelectAll: function (tblid, chkid) {
        var checkboxCount = $('#' + tblid + ' tbody tr').length;
        var isCheckAll;
        var rows = $("#" + tblid + "").dataTable().fnGetNodes();
        if (chkid.checked == true) {
            isCheckAll = true;
        } else {
            isCheckAll = false;
        }
        for (var i = 0; i < rows.length; i++) {
            $(rows[i]).find("." + chkid.id).prop("checked", isCheckAll);
        }
    },
    save: function () {
        debugger;
        var attr = [];
        //Get Selected Category
        var rows = $("#TblRole").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            if ($(rows[i]).find(".cbRole").prop("checked")) {
                newattr.Id = $(rows[i]).find(":eq(2)").html();
                newattr.FieldName = $(rows[i]).find(":eq(3)").html();
                attr.push(newattr);
            }
        }
        if (attr.length != 0) {
            

            $.ajax({
                url: $app.baseUrl + "Leave/Savemanager",
                data: JSON.stringify({ attr: attr }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    switch (jsonResult.Status) {
                        case true:
                            $app.showAlert('Data saved successfully', 2);
                            break;
                        case false:
                            $app.showAlert('Data not saved', 4);
                            break;
                    }
                },
                complete: function () {
                    $app.hideProgressModel();
                }
            });
        }
        else {
            $app.showAlert('Please Select The Role before Saving!!!', 4)
        }
    },    
    GetManagerEligiblity: function () {
        $.ajax({
            url: $app.baseUrl + "Leave/GetManagerEligiblity",
            data: JSON.stringify({ }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async:false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        var data = jsonResult.result;
                        var rows = $("#TblRole").dataTable().fnGetNodes();
                        $Roleform.EligibilityRole = null;
                        $Roleform.EligibilityRole = jsonResult.result;
                        //for (var i = 0; i < data.length; i++) {
                        //    debugger;
                        //    for (var j = 0; j < elgdata.length; j++) {

                        //        if (data[i].RoleId == elgdata[j].RoleID) {
                        //            $(rows[i]).find(".cbRole").prop("checked", true);
                        //        }
                        //    }
                        //}                       
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    }
}