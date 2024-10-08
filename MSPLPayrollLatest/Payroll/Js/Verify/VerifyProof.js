$('#next').click(function () {
    debugger;
    $Verify.fetchData();
})

    //var currentRow = $(this).closest("tr");
    //$Verify.rowlength = $('#tblVerifyDoc tr').length;



$('#tblVerifyDoc').on('click','.printButton', function () {
    debugger;
    var currentRow = $(this).closest("tr");
    var table = $('#tblVerifyDoc');
    $Verify.rowlength = $('#tblVerifyDoc tr').length;
    var Path = currentRow.find("td:eq(2)").text();
    $Verify.rowIndex = $(this).closest('td').parent()[0].sectionRowIndex;
    $Verify.rowIndex = $Verify.rowIndex + 1;
    $Verify.SerialNo = currentRow.find("td:eq(0)").text();
    $Verify.slno = currentRow.find("td:eq(0)").text();
    $Verify.Description = currentRow.find("td:eq(1)").text();
    $Verify.Status = currentRow.find("td:eq(5)").text();
    $Verify.Remarks = currentRow.find("td:eq(6)").text();
    var data = Path;
    $Verify.rowClick = "Y";
    $Proof.getimage(data);
});

$('#submit').click(function () {
    debugger;
    var opt = $('input[name="opt"]:checked').val();
    if (opt != "yes" && opt != "no") {
        var msg = "Accept OR Reject Option Not Selected"
        $Verify.warnmsg(msg);
        return false;
    }
    var remarks = $('#remarks').val();
    if (opt == 'no' && remarks == '') {
        var msg = "Remarks should not be blank for Rejected cases";
        $Verify.warnmsg(msg);
        return false;
    }
    else {
        $Verify.VerifySave();
        $('#submit').hide();
        $('#ar-opt').hide();
        $('#remcol').hide();
        $('#msg-opt').hide();
        $('#next').show();
    }
}),



$Verify = {
    financeYear: '',
    empid: '',
    datalist: '',
    defaultyear: '',
    rowlength: '',
    SerialNo: '',
    rowIndex: '',
    slno: '',
    Description: '',
    startrow: '',
    companyMail: '',
    Status: '',
    Remarks: '',
    show:'Y',
    sendmail: '',
    rowClick:'',

    Alignment: function () {

        var a = $('.dataTables_scrollHeadInner table').first().css("margin-left");
        var b = $('.dataTables_scrollHeadInner table').first().css("width");
        $(".dataTables_scrollBody table").css({ 'margin-left': a, "width": b });
    },
    LoadInitial: function () {
        debugger;
        $Verify.financeYear = $companyCom.getDefaultFinanceYear();
        var sdate = new Date($Verify.financeYear.startDate);
        var edate = new Date($Verify.financeYear.EndDate);
        var syear = sdate.getFullYear();
        var eyear = edate.getFullYear();
        $('#FinYear').html(syear + '-' + eyear);
        $('#FinYear').attr('readonly', true);
        var data = 
        {
            id : 'CName',
        }
        $Verify.LoadCompany(data);
    },

    ScreenInit: function () {
        $('#Input1').addClass('nodisp');
        $('#CName').attr('disabled', false);
        $('#table-disp').addClass('nodisp');
        $('#btn-company').show();
    },

    LoadCompany: function (dropControl) {
        debugger;
        var finyear = $Verify.financeYear.id;
        var empid = $Verify.empid;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Verify/GetCompanyList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ finyear: finyear, empid: empid }),
            dataType: "json",
            success: function (jsonResult) {
                debugger;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                        $.each(out, function (index, object) {
                            var opt = "<option value= '" + object.DBConnectionId + "' data-companymail = '" + object.MailID + "'></option>";
                            $('#' + dropControl.id).append($(opt).html(object.CompanyName));
                        });
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });
    },

    EmpChange: function () {
        $Verify.show = "Y";
        $Verify.LoadEmployeeData();
    },

    LoadData: function () {
        debugger;
        $('#CName').attr('disabled', true);
        $('#btn-company').hide();
        $('#Input1').removeClass('nodisp');
        var DBConnection = $('#CName option:selected').val();
        $Verify.companyMail = $('#CName option:selected').data('companymail');
        if (DBConnection != '0') {
            $Verify.GetEmployee();
        }
    },


    GetEmployee: function () {
        $app.showProgressModel();
        $Verify.datalist = '';
        var sdate = new Date($Verify.financeYear.startDate);
        var edate = new Date($Verify.financeYear.EndDate);
        var DBConnection = $('#CName option:selected').val();
        var finyear = $Verify.financeYear.id;
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Verify/GetVerifyEmpList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({startdate:sdate,enddate:edate,DBConnection:DBConnection,finyear:finyear}),
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                debugger;
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var msg = jsonResult.result.jsonVerifyEmps;
                        $Verify.defaultyear = jsonResult.result.defaultyear;
                        $Verify.datalist = jsonResult.result.jsonVerifyEmps;
                        $('#EmpCode').html('');
                        $('#EmpCode').append($("<option></option>").val(0).html('--Select--'));
                        $.each(msg, function (index, blood) {
                            var opt = "<option value= '" + blood.Id + "' data-name = '" + blood.FirstName + "' data-email = '" + blood.Email + "'></option>";
                            $('#EmpCode').append($(opt).html(blood.EmployeeCode));
                        });
                        $app.hideProgressModel();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                }
            },
            
            error: function (msg) {
                $app.hideProgressModel();
            }
        });
    },

    LoadEmployeeData: function () {
        debugger;
        var name = $('#EmpCode option:selected').data('name');
        $('#name').html(name);
        $('#table-disp').removeClass('nodisp');
        var empid = $('#EmpCode option:selected').val();
        if (empid == '0') {
            return false;
        }
        $app.showProgressModel();
        var financeyear = $Verify.defaultyear;
        $.ajax({
            type: 'POST',
            async: false,
            url: $app.baseUrl + "Verify/GetVerifyProof",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                employeeId: empid, financeyear: financeyear
            }),
            dataType: "json",
            success: function (msg) {
                debugger;
                switch (msg.Status) {
                    case true:
                        debugger;
                        var out = msg;
                        $Verify.RenderData(out);
                        $Verify.datalist = msg;
                        $app.hideProgressModel();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(msg.Message, 4);
                        break;
                }
            },
            error: function (msg) {
                $app.hideProgressModel();
                $app.showAlert("Error in Fetching Data", 4);

            }
        });
    },

    proofgridObject: function () {
        gridObject = [];
        var gridObject = [
            { tableHeader: "SL.No", tableValue: "SerialNo" },
            { tableHeader: "Description", tableValue: "Description" },
            { tableHeader: "FileRef", tableValue: "Filename", cssClass: 'nodisp' },
            { tableHeader: "File", tableValue: null, cssClass: 'popup' },
            { tableHeader: "Upload Date", tableValue: "Uploaddate" },
            { tableHeader: "Status", tableValue: "Status" },
            { tableHeader: "Remarks", tableValue: "Remarks", cssClass: 'Remarks' },
        ];
        return gridObject;
    },

    RenderData: function (data) {
        debugger;
        var gridObject = $Verify.proofgridObject();
        $Verify.Loaddocument(gridObject, data);
    },

    createVerifyProofGrid: function () {
        var gridObject = $Verify.proofgridObject();
        var tableid = { id: $Verify.Verifydoctable };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvVerifyProof').html(modelContent);
    },


    Loaddocument: function (context, data) {
        debugger;
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'popup') {
                columnDef.push({
                    "aTargets": [cnt],
                    data: null,
                    "defaultContent": '<a href="#" class= "printButton" > <img src="assets/plugins/TableTools-master/images/File.png" />'
                }); //for action column

            }
            else if (context[cnt].tableHeader == 'Upload Date') {
                columnDef.push({
                    "aTargets": 4, "width": "100px",
                    "data": "Uploaddate",
                    "render": function (value) {
                        if (value == null) return "";
                        var dt = $Verify.ToJavaScriptDate(value);
                        return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                    }
                })
            }
            else if (context[cnt].tableHeader == 'Action') {
                columnDef.push({
                    "aTargets": 7,
                    "render": function (data) {
                        if ($.trim(data.Status) == "") {
                            return '<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>';
                        }
                        return "";
                    }
                })
            }
            else if (context[cnt].tableHeader == 'Remarks') {
                columnDef.push({
                    "aTargets": [cnt], "sClass": "Remarks ellipses", "width": "200px",
                })
            }
            else if (context[cnt].tableHeader == 'Description') {
                columnDef.push({
                    "aTargets": [cnt], "sClass": "Description word-wrap ellipses", "width": "200px",
                })
            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#tblVerifyDoc').DataTable({
            'iDisplayLength': 5,
            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data.result.VerifyProofList,
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblSavgDoc tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblVerifyDoc thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            "bInfo": false,
            scroller: {
                loadingIndicator: true
            }
        });
    },

    ToJavaScriptDate: function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        if (dt.getDate() == 1 && dt.getMonth() == 0 && dt.getFullYear() == 1) {
            return null;
        }
        return dt;
    },

    startRead: function () {
        debugger;
        $Verify.rowIndex = 0;
        $Verify.fetchData();
    },

    fetchData: function () {
        debugger;
        var table = document.getElementById('tblVerifyDoc');
        if (table.rows.length < 1) {
            var msg = "No Records";
            $Verify.warnmsg(msg);
            return false;
        }
        $Verify.rowIndex = $Verify.rowIndex + 1;
        if ($Verify.rowIndex <= (table.rows.length - 1)) {
            var i = $Verify.rowIndex;
            var cells = table.rows.item(i).cells;
            var Path = cells.item(2).innerHTML;
            $Verify.SerialNo = cells.item(0).innerHTML;
            $Verify.slno = cells.item(0).innerHTML;
            $Verify.Description = cells.item(1).innerHTML;
            $Verify.Status = cells.item(5).innerHTML;
            $Verify.Remarks = cells.item(6).innerHTML;
            var data = Path;
            $Proof.getimage(data);
        }
        else {
            debugger;
            $Verify.message();
        }
    },

    message: function () {
        var msg = "All Documents Viewed";
        alert(msg);
        $('#Viewpreview').modal('toggle');
        $Verify.LoadEmployeeData();
    },

    VerifySave: function () {
        $app.showProgressModel();
        debugger;
        var opt = $('input[name="opt"]:checked').val();
        var data = '';
        var remarks = $('#remarks').val();
        var status = "";
        var status = "";
        if (opt == "yes") {
            status = "Accepted";
        }
        else {
            status = "Rejected";
        }
        $('#tblVerifyDoc tr:eq(' + $Verify.rowIndex + ')').find('td:eq(5)').text(status);
        $('#tblVerifyDoc tr:eq(' + $Verify.rowIndex + ')').find('td:eq(6)').text(remarks);
        data = {
            EmployeeId: $('#EmpCode option:selected').val(),
            financeyear: $Verify.defaultyear,
            SerialNo: Number($Verify.SerialNo),
            VerifiedBy: $Verify.empid,
            Remarks: remarks,
            Status:opt,
        }

        $.ajax({
            url: $app.baseUrl + "Verify/VerifySave",
            data: JSON.stringify(data),
            contentType: 'application/json',
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $app.hideProgressModel();
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


    warnmsg: function (msg) {
        $.alert({
            boxWidth: '600px',
            useBootstrap: false,
            icon: 'fa fa-warning',
            title: "Warning!",
            content: msg,
            type: 'red',
            typeAnimated: true,
        });
    },

    SendMail: function () {
        debugger;
        $app.showProgressModel();
        var empid = $('#EmpCode option:selected').val();
        var finyear = $Verify.defaultyear;
        var email = $('#EmpCode option:selected').data('email');
        var name = $('#EmpCode option:selected').data('name');
        var companymail = $Verify.companyMail;
        $.ajax({
            url: $app.baseUrl + "Verify/SendVerifyMail",
            data: JSON.stringify({ employeeId: empid, financeyear: finyear,email:email,name:name,companymail:companymail }),
            contentType: 'application/json',
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $app.hideProgressModel();
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

}
