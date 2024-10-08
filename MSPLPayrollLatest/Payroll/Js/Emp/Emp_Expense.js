$("#btnExpAssMgrClose").click(function () {
    $empExpens.clear();
});
$("#sltManager").change(function () {
    debugger
    var status = true;
    if ($('#sltManager').find('option:selected').text() != "--Select--") {
        for (i = 0; i < $empExpens.AssMgrDetails.length; i++) {
            var selectAssEmpid = $("#sltManager").val();
            if ($empExpens.AssMgrDetails[i].AssEmpId == selectAssEmpid) {
                status = false
            }
        }
        if (status == true) {
            $empExpens.managernameshow();
        }
        else {
            $app.showAlert("This Employee is already assigned", 4);
            var formData = document.forms["frmAssMgrPopup"];
            formData.elements["sltManager"].value = "00000000-0000-0000-0000-000000000000";
            formData.elements["txtMgrName"].value = "";
        }

    }
    else {


    }


});
$("#sltEmpCode").change(function () {
    debugger
    //LoadFields($("#sltEmpCode").val(), $('#sltEmpCode').find('option:selected').text());
    if ($('#sltEmpCode').find('option:selected').text() != "--Select--") {
        $empExpens.AssMgrDetails = [];
        $("#idassigntable").removeClass("nodisp");
        $empExpens.LoadAssignmanager();

        $empExpens.CheckforAssignedmanagereditordelete();

    }
    else {
        $empExpens.AssMgrDetails = [];
        $("#idassigntable").addClass("nodisp")

    }

});
$("#btnMLAssMgrSave").on('click', function () {
    debugger
    var savestatus = 0;
    var managercode = $("#sltManager").val();
    var appstatus = $("#sltAppStat").val();
    var prioritynum = $("#txtPrioNumb").val();
    var appcancelrights = $("#sltAppCanRigh").val();
    if (managercode == "00000000-0000-0000-0000-000000000000") {
        $app.showAlert('Please Select Manager Id', 4);
        savestatus = 1;
    }
    if (appstatus == "0") {
        $app.showAlert('Please Select Approval status', 4);
        savestatus = 1;

    }

    if (prioritynum == "") {
        $app.showAlert('Please Enter Priority number ', 4);
        savestatus = 1;

    }
    if (prioritynum == "0") {
        $app.showAlert('Priority number cannot be Zero', 4);
        savestatus = 1;

    }
    if (appcancelrights == "0") {
        $app.showAlert('Please Select Approval cancel rights ', 4);
        savestatus = 1;

    }
    if (savestatus == "0") {
        $empExpens.saveAssignMgr();
    }
});

var $empExpens = {
    expenseEntry: new FormData(),
    AssignMgrId:null,
    AssMgrDetails: null,
    formData: document.forms["frmExpense"],
    getFileExtension: function (filename) {
        // Get the file extension from the filename
        var parts = filename.split('.');
        return parts[parts.length - 1].toLowerCase();
    },
    expense: function () {
        debugger
        $("#tblempexpens").DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full', // Typo: 'sPaginationType' instead of 'sPagenationType'
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"], [2, "asc"]],
            columns: [

                { "data": "ID" },
                { "data": "EmployeeID" },
                { "data": "CostCenter" },
                { "data": "CostCenterMgr" },
                { "data": "PurposeForExpense" },
                { "data": "CategeroyOfExpense" },
                {
                    "data": "DateOfExpense",
                    render: function (data) {

                        var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                        return dateF;
                    }


                },
                { "data": "CostOfExpense" },
                { "data": "DescriptExpense" },

                {
                    "data": "SubmitDate",
                    render: function (data) {

                        var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                        return dateF;
                    }
                },
                { "data": "Status" },
                {
                    "data": "Attachment",
                    render: function (data) {
                        var fileExtension = $empExpens.getFileExtension(data);

                        if (fileExtension === 'jpg' || fileExtension === 'png' || fileExtension === 'gif') {

                            var image = '<a href="' + data + '" target="_blank" download>';
                            image += '<img src="' + data + '" style="max-width: 46px;" class="img-circle img-inline" alt="Lights" style="width:10%">   ';
                            image += ' </a>';
                            return image;
                        } else if (fileExtension === 'pdf') {

                            return '<a href="' + data + '" target="_blank" download><i class="fa fa-file-pdf-o"></i> PDF</a>';
                        } else if (fileExtension === 'xlsx' || fileExtension === 'xls') {

                            return '<a href="' + data + '" target="_blank" download><i class="fa fa-file-excel-o"></i> Excel</a>';
                        } else {

                            return '<a href="' + data + '" target="_blank" download>  Attachment </a>';
                        }
                    }


                },

                {
                    "data": null
                }
            ],
            'aoColumnDefs': [
                {
                    "aTargets": [0,1],
                    "sClass": "nodisp"
                },

                {
                    "aTargets": [   3, 4, 5, 7, 8, 9, 2, 10, 11],
                    "sClass": "word-wrap"
                },
                {
                    "aTargets": [12],
                    "sClass": "actionColumns",
                    "fnCreatedCell": function (nTd, row, oData) {
                        var d = $('<input type="button" value="Cancel" data-target="#empExpensesReason" data-toggle="modal" class="btn custom-button marginbt7">');

                        d.button();
                        d.on('click', function () {
                           


                        })

                        $(nTd).empty();
                        $(nTd).prepend(d);
                    }
                }
            ],
            responsive: true,
            ajax: function (data, callback, settings) {
                $.ajax({
                    url: $app.baseUrl + "Employee/GetEmployeeExpense",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
                        $app.clearSession(jsonResult);
                        var out = jsonResult.result;
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length
                            });

                        }, 50);
                    }
                });
            }

        })
    },
    save: function () {
        debugger
        var result = $empExpens.checkValue();
        if (result) {
            $app.showProgressModel();
            $empExpens.expenseEntry.append("EmpID", $("#sltEmpCode").val());
            $empExpens.expenseEntry.append("CostCenter", $("#txtCostCenter").val());
            $empExpens.expenseEntry.append("CostCenterMgr", $("#txt1stLevel").val());
            $empExpens.expenseEntry.append("PurposeForExpense", $("#txtPurpose").val());
            $empExpens.expenseEntry.append("CategeroyOfExpense", $("#txtCategory").val());
            $empExpens.expenseEntry.append("DescriptExpense", $("#txtDescrip").val());
            $empExpens.expenseEntry.append("DateOfExpense", $("#txtItemDate").val());
            $empExpens.expenseEntry.append("SubmitDate", $("#txtSubmitDate").val());
            $empExpens.expenseEntry.append("CostOfExpense", $("#txtCostExpense").val());


            $.ajax({
                url: $app.baseUrl + "Employee/ExpenseEntry",
                dataType: "json",
                type: "POST",
                data: $empExpens.expenseEntry,
                contentType: false,
                processData: false,
                success: function (jsonResult) {
                    debugger
                    if (jsonResult.Status) {
                        $("#empExpenses").modal("toggle");
                        var table = $("#tblempexpens").DataTable();
                        table.destroy();

                        $empExpens.expense();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                    }
                }

            })
        }
    },

    cancelExpenseRequest: function (contxt) {
        debugger;
        alert("You Clicked The Image" + contxt.ID);
    },
    clear: function () {
            // Clear all input elements by setting their values to an empty string
            $('#txtCostCenter').val('');
            $('#txtPurpose').val('');
            $('#txtItemDate').val('');
            $('#txtCostExpense').val('');
            $('#txtCostMgr').val('');
            $('#txtCategory').val('');
            $('#txtDescrip').val('');
            $('#txtSubmitDate').val('');
            $('#txtfile').val('');
     },
    checkValue: function () {
        if ($("#sltEmpCode").val() == "") {
            $app.showAlert("Must Enter The Employee ID", 3)
            return false;
        }
        if ($("#txtSubmitDate").val() == "") {
            $app.showAlert("Must Enter The Submitted Date", 3)
            return false;
        }
        if ($("#txtItemDate").val() == "") {
            $app.showAlert("Must Enter The   Date of Expense Item", 3)
            return false;
        }
        if ($("#txtCostExpense").val() == "") {
            $app.showAlert("Must Enter The Cost of Expense ", 3)
            return false;
        }
        
        return true;
    },
    saveAssignMgr: function () {
        debugger
        $app.showProgressModel();
        var Id = $empExpens.AssignMgrId;
        var AssEmpId = $("#sltManager").val();
        var ApprovMust = $("#sltAppStat").val();
        var MgrPriority = $("#txtPrioNumb").val();
        var AppCancelRights = $("#sltAppCanRigh").val();
        var EmployeeId = $("#sltEmpCode").val();
        var SaveAssignMgrValue = {
            Id: $empExpens.AssignMgrId,
            AssEmpId: $("#sltManager").val(),
            ApprovMust: $("#sltAppStat").val(),
            MgrPriority: $("#txtPrioNumb").val(),
            AppCancelRights: $("#sltAppCanRigh").val(),
            EmployeeId: $("#sltEmpCode").val()
        }

        $.ajax({
            url: $app.baseUrl + "Employee/SaveAssignManager",

            data: JSON.stringify({ SaveAssignMgrValue: SaveAssignMgrValue }),
            dataType: "json",
            contentType: "application/json ",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                debugger
                switch (jsonResult.Status) {

                    case true:
                        $('#AddAssignpopup').modal('toggle');
                        $empExpens.clear();
                        $empExpens.LoadAssignmanager();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $employee.canSave = true;
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    LoadAssignmanager: function () {

        var dtClientList = $('#tblAssignedmanager').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                { "data": "Id" },
                { "data": "MgrEmpCode" },
                { "data": "MgrEmpName" },
                { "data": "ApprovMustString" },
                { "data": "MgrPriority" },
                { "data": "AppCancelRightString" },
                { "data": null }
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
                    "sClass": "actionColumn"
                    ,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                        var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                        var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                        b.button();
                        b.on('click', function () {
                            debugger
                            if ($("#iddisplaystatus").text() == "") {
                                $empExpens.getAssignMgrData(oData);
                                return false;
                            }
                            else {
                                $app.showAlert("You Can't Edit this Manager,This Employee is UnderProcess", 4);
                            }
                        });
                        c.button();
                        c.on('click', function () {
                            if ($("#iddisplaystatus").text() == "") {
                                if (confirm('Are you sure ,do you want do delete?')) {
                                    $empExpens.deleteAssingMgr(oData);
                                    return false;
                                }
                            }
                            else {
                                $app.showAlert("You Can't delete this Manager,This Employee is UnderProcess", 4);
                            }
                        });
                        $(nTd).empty();
                        $(nTd).prepend(b, c);
                    }
                }],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetAssignManager",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EMPID: $("#sltEmpCode").val() }),
                    dataType: "json",
                    async: false,
                    success: function (jsonResult) {
                        debugger
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {        
                            case true:
                                console.log(jsonResult.result);
                                var out = jsonResult.result;
                                for (i = 0; i < out.length; i++) {
                                    var AssMgrObj = new Object();
                                    AssMgrObj.Id = out[i].Id;
                                    AssMgrObj.AssEmpId = out[i].AssEmpId;
                                    AssMgrObj.EmployeeId = out[i].EmployeeId;
                                    AssMgrObj.FinYear = out[i].FinYear;
                                    AssMgrObj.MgrEmpCode = out[i].MgrEmpCode;
                                    AssMgrObj.MgrEmpName = out[i].MgrEmpName;
                                    AssMgrObj.MgrPriority = out[i].MgrPriority;
                                    AssMgrObj.ApprovMust = out[i].ApprovMust;
                                    AssMgrObj.AppCancelRights = out[i].AppCancelRights;
                                    AssMgrObj.CompanyId = out[i].CompanyId;
                                    $empExpens.AssMgrDetails.push(AssMgrObj);
                                }
                                setTimeout(function () {
                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });
                                }, 500);
                                break;
                            case false:
                                $empExpens.AssMgrDetails = [];
                                $app.showAlert(jsonResult.Message, 4);
                        }
                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblAssignedmanager tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblAssignedmanager thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    deleteAssingMgr: function (context) {
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteAssignMgrData",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $empExpens.LoadAssignmanager();
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
    getAssignMgrData: function (data) {
        $.ajax({
            url: $app.baseUrl + "LeaveRequest/GetAssMgrSelectedData",
            data: JSON.stringify({ Id: data.Id, EmpId: data.EmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $empExpens.renderAssignMgr(data);
                        break;
                    case false:
                        alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {
            }
        });
    },
    renderAssignMgr: function (data) {
        $('#AddAssignpopup').modal('toggle');
        var formData = document.forms["frmAssMgrPopup"];
        $leave.AssignMgrId = data.Id;
        formData.elements["sltManager"].value = data.AssEmpId;
        formData.elements["txtMgrName"].value = data.MgrEmpName;
        formData.elements["sltAppStat"].value = data.ApprovMust;
        formData.elements["txtPrioNumb"].value = data.MgrPriority;
        formData.elements["sltAppCanRigh"].value = data.AppCancelRights;
    },
    getManager: function (context) {

        debugger
        $.ajax({
            url: $app.baseUrl + "Employee/GetAssignManage",
            data: JSON.stringify({ EMPLOYEEID: $("#hdnEmpId").val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var out = jsonResult.result;

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        $('#txt1stLevel').val(out[0].Email);
                        $('#txt1stlvlData').val(out[0].firstlevelData);
                        break;
                    case false:
                        $('#txt1stLevel').val('');
                        $('#txt1stlvlData').val('');
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });

    },
    loadEmployee: function (dropControl) {
        debugger
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetEmployees",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                debugger
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                });
            },
            error: function (msg) {
            }
        });
    },
    expenseApprove: function () {
        debugger
        $("#tblEmpexpensApprove").DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full', // Typo: 'sPaginationType' instead of 'sPagenationType'
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"], [2, "asc"]],
            columns: [

                { "data": "ID" },
                { "data": "EmployeeID" },
                { "data": "CostCenter" },
                { "data": "CostCenterMgr" },
                { "data": "PurposeForExpense" },
                { "data": "CategeroyOfExpense" },
                {
                    "data": "DateOfExpense",
                    render: function (data) {

                        var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                        return dateF;
                    }


                },
                { "data": "CostOfExpense" },
                { "data": "DescriptExpense" },

                {
                    "data": "SubmitDate",
                    render: function (data) {

                        var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                        var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                        return dateF;
                    }
                },
                { "data": "Status" },
                {
                    "data": "Attachment",
                    render: function (data) {
                        var fileExtension = $empExpens.getFileExtension(data);

                        if (fileExtension === 'jpg' || fileExtension === 'png' || fileExtension === 'gif') {

                            var image = '<a href="' + data + '" target="_blank" download>';
                            image += '<img src="' + data + '" style="max-width: 46px;" class="img-circle img-inline" alt="Lights" style="width:10%">   ';
                            image += ' </a>';
                            return image;
                        } else if (fileExtension === 'pdf') {

                            return '<a href="' + data + '" target="_blank" download><i class="fa fa-file-pdf-o"></i> PDF</a>';
                        } else if (fileExtension === 'xlsx' || fileExtension === 'xls') {

                            return '<a href="' + data + '" target="_blank" download><i class="fa fa-file-excel-o"></i> Excel</a>';
                        } else {

                            return '<a href="' + data + '" target="_blank" download>  Attachment </a>';
                        }
                    }


                },

                {
                    "data": null
                }
            ],
            'aoColumnDefs': [
                {
                    "aTargets": [0, 1],
                    "sClass": "nodisp"
                },

                {
                    "aTargets": [3, 4, 5, 7, 8, 9, 2, 10, 11],
                    "sClass": "word-wrap"
                },
                {
                    "aTargets": [12],
                    "sClass": "actionColumns",
                    "fnCreatedCell": function (nTd, row, oData) {
                        var d = $('<input type="button" value="Cancel"  class="btn custom-button marginbt7">');

                        d.button();
                        d.on('click', function () {
                            $empExpens.cancelExpenseRequest(oData);


                        })

                        $(nTd).empty();
                        $(nTd).prepend(d);
                    }
                }
            ],
            responsive: true,
            ajax: function (data, callback, settings) {
                $.ajax({
                    url: $app.baseUrl + "Employee/GetEmployeeExpense",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
                        $app.clearSession(jsonResult);
                        var out = jsonResult.result;
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length
                            });

                        }, 50);
                    }
                });
            }

        })


    },
    CheckforAssignedmanagereditordelete: function () {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/CheckforAssignedmanagereditordelete",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EmployeeId: $("#sltEmpCode").val() }),
            dataType: "json",
            success: function (jsonResult) {

                var out = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:

                        document.getElementById('iddisplaystatus').innerHTML = '';
                        $("#btnAddPopup").removeClass("nodisp");
                        break;
                    case false:

                        document.getElementById('iddisplaystatus').innerHTML = 'You cannot Edit or Delete,This Employee is UnderProcess!!!';
                        $("#btnAddPopup").addClass("nodisp");
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },
    managernameshow: function () {
        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/Showmanagername",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EMPID: $("#sltManager").val() }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:

                        $app.hideProgressModel();
                        $("#txtMgrName").val(out);
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
    rejectExpense: function () {
    
    },
}