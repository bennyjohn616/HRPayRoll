
    $('#EmpCode').change(function () {
        $('#data').addClass('nodisp');
        $('#table-disp').addClass('nodisp');
        $('#name').addClass('nodisp');
    }),

$('#tblSavgDoc').on('click', '.printButton', function () {
    debugger;
    var currentRow = $(this).closest("tr");
    var Path = currentRow.find("td:eq(2)").text();
    var data = Path;
    var Path = currentRow.find("td:eq(2)").text();
    //$SavgProofView.rowIndex = $(this).closest('td').parent()[0].sectionRowIndex;
    //$SavgProofview.rowIndex = $Verify.rowIndex + 1;
    $SavgProofView.SerialNo = currentRow.find("td:eq(0)").text();
    $SavgProofView.slno = currentRow.find("td:eq(0)").text();
    $SavgProofView.Description = currentRow.find("td:eq(1)").text();
    $SavgProofView.Status = currentRow.find("td:eq(5)").text();
    $SavgProofView.Remarks = currentRow.find("td:eq(6)").text();
    var data = Path;
    $View1.getimage(data);
}),

    $('#tblSavgDoc').on('click', '.Description', function () {
        debugger;
        var currentRow = $(this).closest("tr");
        var desc = currentRow.find("td:eq(1)").text();
        var rem = currentRow.find("td:eq(6)").text();
        var updt = currentRow.find("td:eq(4)").text();
        var status = currentRow.find("td:eq(5)").text();
        msg = "<p>Serial No : " + currentRow.find("td:eq(0)").text() + "</br>" + " Description : " + desc + "</br> Upload Date : " + updt +
            "</br> Status : " + status + "</br> Remarks : " + rem + "</p>";
        $SavgProofView.infomsg(msg);
    }),

    $('#tblSavgDoc').on('click', '.Remarks', function () {
        debugger;
        var currentRow = $(this).closest("tr");
        var desc = currentRow.find("td:eq(1)").text();
        var rem = currentRow.find("td:eq(6)").text();
        var updt = currentRow.find("td:eq(4)").text();
        var status = currentRow.find("td:eq(5)").text();
        msg = "<p>Serial No : " + currentRow.find("td:eq(0)").text() + "</br>" + " Description : " + desc + "</br> Upload Date : " + updt +
            "</br> Status : " + status + "</br> Remarks : " + rem + "</p>";
        $SavgProofView.infomsg(msg);
    });



var $SavgProofView = {
    canSave: false,
    Savgdoctable: 'tblSavgDoc',
    tableId: 'tblSavgDoc',
    EmployeeId: null,
    payhistory: null,
    financeYear: null,
    INPdate: null,
    serverDate: null,
    jsoncutoffdate: null,
    cutoffdate: null,
    serverTime: null,
    serverDate1: null,
    serverDate2: null,
    cutoffdateDisp: null,
    datalist: null,
    fileData: null,
    LastSerialNo: null,
    rowIndex: '',
    SerialNo: '',
    slno: '',
    Description: '',
    Status: '',
    Remarks:'',

    LoadComponents: function () {
        debugger;
        var date1 = new Date($SavgProofView.serverDate);
        $SavgProofView.serverDate1 = (date1.getFullYear() * 10000) + ((date1.getMonth() + 1) * 100) + date1.getDate();
        $SavgProofView.serverDate2 = date1.getDate();
        $SavgProofView.GetInpDate();
        $SavgProofView.GetDefaultfinyear();
        $SavgProofView.AddInitialize();
        $SavgProofView.GetEmployee();
        if ($SavgProofView.serverDate != "") {
            $('#ld').html($SavgProofView.cutoffdateDisp);
        }
    },

    AddInitialize: function () {
        $SavgProofView.canSave = true;
        var formData = document.forms["frmSavgProof"];
    },


    LoadData: function () {
        debugger;
        $('#data').removeClass('nodisp');
        $('#table-disp').removeClass('nodisp');
        $('#name').removeClass('nodisp');
        var emp = $('#EmpCode option:selected').val();
        $SavgProofView.EmployeeId = emp;
        $('#msg').html('');

        $SavgProofView.LoadSavgProof(emp);
        var name = "Name : " + $SavgProofView.datalist.result.emp.FirstName + "  " + $SavgProofView.datalist.result.emp.LastName;
        $('#name').html(name);
        //$('#data').removeClass('nodisp');
    },

    GetDefaultfinyear: function () {
        debugger;
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            date2 = new Date($SavgProofView.jsoncutoffdate);
            $SavgProofView.financeYear = $companyCom.EmployeeDefaultFinanceYear(date2);
        }
        else {
            $SavgProofView.financeYear = $companyCom.getDefaultFinanceYear();
        }
        var sdate = new Date($SavgProofView.financeYear.startDate);
        var edate = new Date($SavgProofView.financeYear.EndDate);
        var syear = sdate.getFullYear();
        var eyear = edate.getFullYear();
        $('#txtYear').html(syear + '-' + eyear);
        $('#txtYear').attr('readonly', true);

    },

    GetInpDate: function () {
        $.ajax({
            url: $app.baseUrl + "Setting/GetTDSdays",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $SavgProofView.INPdate = jsonResult.result.TDSdays;
                        var locale = "en-us";
                        jsonResult.result.proofcutoffdate = jsonResult.result.proofcutoffdate
                        var cudate = new Date(parseInt(jsonResult.result.proofcutoffdate.replace(/(^.*\()|([+-].*$)/g, '')));
                        $SavgProofView.jsoncutoffdate = (cudate.getFullYear().toString() + "/" + (cudate.getMonth() + 1).toString() + "/" + cudate.getDate().toString());
                        $SavgProofView.cutoffdate = (cudate.getFullYear() * 10000) + ((cudate.getMonth() + 1) * 100) + cudate.getDate();
                        $SavgProofView.cutoffdateDisp = cudate.getDate() + "/" + (cudate.getMonth() + 1) + "/" + cudate.getFullYear();
                        $app.hideProgressModel();
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

    GetEmployee: function () {
        $SavgProofView.datalist = '';
        debugger;
        var condi = "SavgView" + ".";
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetSelectiveEmployees",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                Condition: condi
            }),
            dataType: "json",
            async: false,
            success: function (data) {
                debugger;
                var msg = data.result.Jsondata;
                $SavgProofView.datalist = data.result.Jsondata;
                $('#EmpCode').html('');
                $.each(msg, function (index, blood) {
                    $('#EmpCode').append($("<option></option>").val(blood.empid).html(blood.empCode));
                });

            },
            error: function (msg) {
            }
        });
    },



    LoadSavgProof: function (emp) {
        var financeyear = $SavgProofView.financeYear.id;
        //var dtClientList = $('#tblSavgDoc').DataTable({
        //    'iDisplayLength': 10,
        //    'bPaginate': true,
        //    'sPaginationType': 'full',
        //    'sDom': '<"top">rt<"bottom"ip><"clear">',
        //    columns: [
        //        { "data": "SerialNo" },
        //        { "data": "Description" },
        //        { "data": "Filename" },
        //        { "data": null },
        //        { "data": "Uploaddate" },
        //        { "data": "Status" },
        //        { "data": "Remarks" }
        //    ],
        //})

        //"columnDefs": [
        //    {
        //        "aTargets": [4],
        //        "data": "Uploaddate",
        //        "render": function (value) {
        //            if (value == null) return "";
        //            return (ToJavaScriptDate(value));
        //        }
        //    }
        //]
        //        {
        //            "aTargets": [1],
        //            "sClass": "word-wrap"

        //        },
        //        {
        //            "aTargets": [2],
        //            "sClass": "word-wrap"
        //        },
        //        {
        //            "aTargets": [3],
        //            "sClass": "word-wrap"
        //        },
        //        {
        //            "aTargets": [4],
        //            "sClass": "word-wrap"
        //        },
        //        //{
        //        //    "aTargets": [5],
        //        //    "sClass": "actionColumn",
        //        //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        //        //        var b = $('<a href="#"  data-toggle="modal" data-target="#filepreview" class="editeButton"><span>' + oData.Filename + '</span></button>');
        //        //        b.button();
        //        //        b.on('click', function () {
        //        //            $SavgProofView.GetProof(oData);
        //        //            return false;
        //        //        });
        //        //        $(nTd).empty();
        //        //        $(nTd).prepend(b);


        //        //    }
        //        //}
        //    ],
        //})
        //ajax: function () {
        $.ajax({
            type: 'POST',
            async: false,
            url: $app.baseUrl + "TaxSection/GetSavgProof",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                employeeId: emp, financeyear: financeyear
            }),
            dataType: "json",
            success: function (msg) {
                var out = msg;
                //setTimeout(function () {
                //    callback({
                //        draw: data.draw,
                //        data: out,
                //        recordsTotal: out.length,
                //        recordsFiltered: out.length
                //    });

                //}, 50);
                $SavgProofView.RenderData(out);
                $SavgProofView.datalist = msg;
            },
            error: function (msg) {
            }
        });
    },

    savgproofgridObject: function () {
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
        var gridObject = $SavgProofView.savgproofgridObject();
        $SavgProofView.LoadSavgdocument(gridObject, data);
    },

    createSavgProofGrid: function () {
        var gridObject = $SavgProofView.savgproofgridObject();
        var tableid = { id: $SavgProofView.Savgdoctable };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvSavgProof').html(modelContent);
    },


    LoadSavgdocument: function (context, data) {
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
                        var dt = $SavgProofView.ToJavaScriptDate(value);
                        return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
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
        var dtClientList = $('#tblSavgDoc').DataTable({
            'iDisplayLength': 5,
            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data.result.SavgProofList,
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblSavgDoc tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblSavgDoc thead').append(r);
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
        debugger;
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        if (dt.getDate() == 1 && dt.getMonth() == 0 && dt.getFullYear() == 1) {
            return null;
        }
        return dt;
    },

    rendersavgdoc: function (data) {
        var gridObject = $SavgProofView.savgproofgridObject();
        var tableid = { id: 'tblSavgDoc' };
        $SavgProofView.LoadSavgdocument(gridObject, tableid);
    },
    //need to check is this function required

    downloadFile: function (context) {
        debugger;
        $app.showProgressModel();
        var data = context;
        $.ajax({
            url: $app.baseUrl + "DownLoad/DownloadProof",
            data: JSON.stringify({ data: data }),
            async: false,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                var oData = new Object();
                oData.filePath = jsonResult.result.filePath;
                if (oData.filePath != "") {
                    $app.downloadSync('Download/DownloadPaySlip', oData);
                    return false;
                }
                else {
                    $app.hideProgressModel();
                    $app.showAlert("Error in Downloading", 4);
                }
            },


            complete: function () {
                $app.hideProgressModel();
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

    infomsg: function (msg) {
        $.alert({
            boxWidth: '600px',
            useBootstrap: false,
            icon: 'fa fa-info',
            title: "Message!",
            content: msg,
            type: 'green',
            typeAnimated: true,
        });
    },

};
