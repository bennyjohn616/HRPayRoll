
$('#btnSave').click(function () {
    debugger;
    var fileUpload = $("#DocUpload").get(0);
    var files = fileUpload.files;
    if (files.length == 0 ) {
        msg = "No Files Selected";
        $SavgProof.warnmsg(msg);
        return false;
    }
    var desc = $('#Description').val();
    if (desc == '' || desc == null) {
        msg = "Description Should not be Blank"
        $SavgProof.warnmsg(msg);
        return false;
    }
    $SavgProof.saveProof();
    $('#DocUpload').val('');
    $('#Description').val('');
    

}),

    $('#EmpCode').change(function () {
        $('#data').addClass('nodisp');
        $('#table-disp').addClass('nodisp');
        $('#name').addClass('nodisp');
    })

    $('#tblSavgDoc').on('click','.printButton', function () {
        debugger;
        var currentRow = $(this).closest("tr");
        var Path = currentRow.find("td:eq(2)").text();
        var data = Path;
        $SavgProof.downloadFile(data);
    }),

    $('#tblSavgDoc').on('click', '.deleteButton', function () {
        debugger;
        var currentRow = $(this).closest("tr");
        var Path = currentRow.find("td:eq(2)").text();
        var SerialNo = currentRow.find("td:eq(0)").text();
        var data = {
            Path: Path,
            SerialNo: SerialNo,
            EmployeeId: $SavgProof.EmployeeId,
            FinanceYear: $SavgProof.financeYear.id
        }

        $SavgProof.delete(data);
    }),

    $('#tblSavgDoc').on('click','.Description', function () {
        debugger;
        var currentRow = $(this).closest("tr");
        var desc = currentRow.find("td:eq(1)").text();
        var rem = currentRow.find("td:eq(6)").text();
        var updt = currentRow.find("td:eq(4)").text();
        var status = currentRow.find("td:eq(5)").text();
        msg = "<p>Serial No : " + currentRow.find("td:eq(0)").text() + "</br>" + " Description : " + desc + "</br> Upload Date : " + updt +
            "</br> Status : " + status + "</br> Remarks : " + rem + "</p>";
        $SavgProof.infomsg(msg);
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
        $SavgProof.infomsg(msg);
    }),



$('#DocUpload').change(function (e) {
    var files = e.target.files[0].name;
    if (files.length > 0) {
        var file = this.files[0];
        fileName = file.name;
        size = file.size;
        if (size > 10485760) {
            msg = "Please upload file less than 10MB";
            $SavgProof.warnmsg(msg);
            $('#DocUpload').val('');
            $('#Description').val('');
            return false;
        }

        var validExtensions = ['jpg','jpeg','pdf'];
        var fileNameExt = fileName.substr(fileName.lastIndexOf('.') + 1);
        if ($.inArray(fileNameExt, validExtensions) == -1) {
            var msg = "Invalid file type";
            $SavgProof.warnmsg(msg);
            $('#DocUpload').val('');
            $('#Description').val('');
            return false;
        }


        type = file.type;
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append(files[x].name, files[x]);
            }
            $SavgProof.fileData = data;

        }
        else {
            var msg = "This browser doesn't support HTML5 file uploads!";
            $SavgProof.warnmsg(msg);
            $SavgProof.fileData = null;
            return false;
        }
    }
});



var $SavgProof = {
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

    LoadComponents: function () {
        debugger;
        var date1 = new Date($SavgProof.serverDate);
        $SavgProof.serverDate1 = (date1.getFullYear() * 10000) + ((date1.getMonth() + 1) * 100) + date1.getDate();
        $SavgProof.serverDate2 = date1.getDate();
        $SavgProof.GetInpDate();
        $SavgProof.GetDefaultfinyear();
        $SavgProof.AddInitialize();
        $SavgProof.GetEmployee();
        if ($SavgProof.serverDate != "") {
            $('#ld').html($SavgProof.cutoffdateDisp);
        }
    },

    AddInitialize: function () {
        $SavgProof.canSave = true;
        var formData = document.forms["frmSavgProof"];
    },


    LoadData: function () {
        debugger;
        $('#data').removeClass('nodisp');
        $('#table-disp').removeClass('nodisp');
        $('#name').removeClass('nodisp');
        var emp = $('#EmpCode option:selected').val();
        $SavgProof.EmployeeId = emp;
        $('#msg').html('');

        if ($SavgProof.cutoffdate < $SavgProof.serverDate1) {
            $('#data').addClass('nodisp');
            $('#btnSave').hide();
            var msg = "!Cannot Enter Data after cutoff date </br>" + " CUTOFF DATE : " + $SavgProof.cutoffdateDisp;
            this.warnmsg(msg);
            return false;
        }
        $SavgProof.LoadSavgProof(emp);
        var name = "Name : " + $SavgProof.datalist.result.emp.FirstName + "  " + $SavgProof.datalist.result.emp.LastName;
        $('#name').html(name);
        //$('#data').removeClass('nodisp');
    },

    GetDefaultfinyear: function () {
        debugger;
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            date2 = new Date($SavgProof.jsoncutoffdate);
            $SavgProof.financeYear = $companyCom.EmployeeDefaultFinanceYear(date2);
        }
        else {
            $SavgProof.financeYear = $companyCom.getDefaultFinanceYear();
        }
        var sdate = new Date($SavgProof.financeYear.startDate);
        var edate = new Date($SavgProof.financeYear.EndDate);
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
                        $SavgProof.INPdate = jsonResult.result.TDSdays;
                        var locale = "en-us";
                        jsonResult.result.proofcutoffdate = jsonResult.result.proofcutoffdate
                        var cudate = new Date(parseInt(jsonResult.result.proofcutoffdate.replace(/(^.*\()|([+-].*$)/g, '')));
                        $SavgProof.jsoncutoffdate = (cudate.getFullYear().toString() + "/" + (cudate.getMonth() + 1).toString() + "/" + cudate.getDate().toString());
                        $SavgProof.cutoffdate = (cudate.getFullYear() * 10000) + ((cudate.getMonth() + 1) * 100) + cudate.getDate();
                        $SavgProof.cutoffdateDisp = cudate.getDate() + "/" + (cudate.getMonth() + 1) + "/" + cudate.getFullYear();
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
        $SavgProof.datalist = '';
        debugger;
        var condi = "Savg" + ".";
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
                $SavgProof.datalist = data.result.Jsondata;
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
        var financeyear = $SavgProof.financeYear.id;
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
        //        //            $SavgProof.GetProof(oData);
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
                    async:false,
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
                        $SavgProof.RenderData(out);
                        $SavgProof.datalist = msg;
                        $SavgProof.AssignLastNo();
                    },
                    error: function (msg) {
                    }
                });
    },

    AssignLastNo: function () {
        debugger;
        if ($SavgProof.datalist.result.SavgProofList == null || $SavgProof.datalist.result.SavgProofList.length == 0) {
            $SavgProof.LastSerialNo = 0;
            $('#LastNo').val($SavgProof.LastSerialNo);
            
        }
        else {
            var index = $SavgProof.datalist.result.SavgProofList.length;
            $SavgProof.LastSerialNo = Number($SavgProof.datalist.result.SavgProofList[index - 1].SerialNo);
            $('#LastNo').val($SavgProof.LastSerialNo);
        }
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
            { tableHeader: "Action", tableValue: null},
        ];
        return gridObject;
    },




    RenderData: function (data) {
        debugger;
        var gridObject = $SavgProof.savgproofgridObject();
        $SavgProof.LoadSavgdocument(gridObject, data);
    },

    createSavgProofGrid: function () {
        var gridObject = $SavgProof.savgproofgridObject();
        var tableid = { id: $SavgProof.Savgdoctable };
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
                        var dt = $SavgProof.ToJavaScriptDate(value);
                        return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                    }
                })
            }
            else if (context[cnt].tableHeader == 'Action') {
                columnDef.push ({
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
            "bInfo":false,
            scroller: {
                loadingIndicator: true
            }
        });
    },


    saveProof: function () {
        if ($SavgProof.cutoffdate < $SavgProof.serverDate1) {
            msg = "Cannot update Data";
            this.warnmsg(msg);
            return false;
        }
        $app.showProgressModel();
        debugger;
        if ($SavgProof.LastSerialNo == 0 || $SavgProof.LastSerialNo == '') {
            $SavgProof.LastSerialNo = 0;
        }
        var formData = document.forms["frmSavgProof"];
        var fileUpload = $("#DocUpload").get(0);
        var files = fileUpload.files;

        for (var i = 0; i < files.length; i++) {
            $SavgProof.fileData.append(files[i].name, files[i]);
        }
        $SavgProof.LastSerialNo = Number($SavgProof.LastSerialNo + 1);
        var Description = $('#Description').val();
        var EmployeeCode = $('#EmpCode option:selected').text();
        var EmployeeId = $('#EmpCode option:selected').val();
        var financeyear = $SavgProof.financeYear.id;
        var finyearstart = new Date($SavgProof.financeYear.startDate);
        var Finyearref = finyearstart.getFullYear();
        var jsonSavgProof = JSON.stringify($SavgProof.datalist.result);
        $SavgProof.fileData.append('EmployeeCode', EmployeeCode);
        $SavgProof.fileData.append('EmployeeId', EmployeeId);
        $SavgProof.fileData.append('Description', Description);
        $SavgProof.fileData.append('LastNo', $SavgProof.LastSerialNo);
        $SavgProof.fileData.append('financeyear', financeyear);
        $SavgProof.fileData.append('Finyearref', Finyearref);
        $SavgProof.fileData.append('JsonList', jsonSavgProof);

        $.ajax({
            url: $app.baseUrl + "TaxSection/SaveProof",
            data: $SavgProof.fileData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $app.hideProgressModel();
                        //$SavgProof.LastSerialNo = Number(jsonResult.result.SerialNo);
                        $SavgProof.addRow(jsonResult.result);
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

    addRow: function (data) {
        debugger;
        var table = $('#tblSavgDoc').DataTable();
        table.row.add({
            "SerialNo": data.SerialNo,
            "Description": data.Description,
            "Filename": data.Filename, cssClass: 'nodisp',
            "File":
                ({
                    data: '<a href="#" class="printButton"> <img src="assets/plugins/TableTools-master/images/File.png"/>'
                }),
            "Uploaddate": data.Uploaddate,
                "render": function(value) {
                    if (new Date(value) == 'NaN') return "";
                    var dt = ToJavaScriptDate(value);
                    return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
            },
            "Status": data.Status,
            "Remarks": data.Remarks,
            "Action": null,
            "render": function (data) {
                if (trim(data.Status) == "") {
                    return '<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>';
                }
                return "";
            },
        }).draw();
        //$('#tblSavgDoc').dataTable('refresh');
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
        var gridObject = $SavgProof.savgproofgridObject();
        var tableid = { id: 'tblSavgDoc' };
        $SavgProof.LoadSavgdocument(gridObject, tableid);
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

    showFile: function () {

    },

    delete: function (data) {
        debugger;
        if ($SavgProof.cutoffdate < $SavgProof.serverDate1) {
            msg = "Cannot update Data";
            this.warnmsg(msg);
            return false;
        }

        if (data.Path == "" || data.EmployeeId == "" || data.SerialNo == "" || data.FinanceYear == "") {
            msg = "Error in Deletion";
            this.warnmsg(msg);
            return false;
        }
        $app.showProgressModel();
        $.ajax({
            url: $app.baseUrl + "TaxSection/DeleteProof",
            data: JSON.stringify({ EmployeeId: data.EmployeeId, SerialNo: data.SerialNo, FinanceYear: data.FinanceYear, Path: data.Path }),
            async: false,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger;
                var out = jsonResult;
                var result = jsonResult.Status;
                if (result == true) {
                    $SavgProof.RenderData(out);
                    $SavgProof.datalist = jsonResult;
                    $SavgProof.AssignLastNo();
                    $app.hideProgressModel();
                    $app.showAlert("Record Deleted Successfully", 2);
                    return false;
                }
                else {
                    $app.hideProgressModel();
                    $app.showAlert("Error in Deletion", 4);
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
