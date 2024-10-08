$('#sltEmployeeRolelist').change(function () {
    
    $EmployeeReport.EmployeeId = $('#sltEmployeeRolelist').val(),
    $EmployeeReport.LoadDetail();
    
    var selectedtext = $('#sltEmployeeRolelist').find('option:selected').text();
    if (selectedtext != "--Select--")
       
    {
        $EmployeeReport.managernameshow();
    }
    else
    {
        //document.getElementById('Userrptmngname').innerHTML = "";
        $('#Userrptmngname').val('');
    }
    
});
$("#btnManagerReportView").on('click', function () {
    
    if ($("#txtMngrReportFromDate").val() == "" || $("#txtMngrReportToDate").val() == "") {
        $app.showAlert('Please Select the Dates!', 4);
    }
    else {
        $("#Mngrreportdivid").removeClass('nodisp')
        $EmployeeReport.LoadManagerReport();
    }

});
$EmployeeReport = {
    EmployeeId: null,
    loadInitial: function () {
        
        EmployeeId: $('#sltEmployeeRolelist').val(),
        //$EmployeeReport.loadEmployee({ id: "sltEmployeeRolelist" });
        $companyCom.loadFullManager({ id: 'sltEmployeeRolelist' });
        $EmployeeReport.EmployeeId = $('#sltEmployeeRolelist').val(),
        $EmployeeReport.LoadDetail();
    },
    loadEmployee: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/getuserrole",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.EmployeeCode + ' - ' + blood.Displayas));
                });
            },
            error: function (msg) {
            }
        });
    },
    //LoadDetail: function () {
    //    
    //   // $app.applyseletedrow();
    //    var dtClientList = $('#Usermanagertbl').DataTable({
    //        'iDisplayLength': 3,
    //        'bPaginate': true,
    //        'sPaginationType': 'full',
    //        'responsive': true,
    //        'sDom': '<"top">rt<"bottom"ip><"clear">',
    //        "aaSorting": [[1, "asc"]],
    //        "sSearch": "Search:",
    //        "bFilter": true,
    //        columns: [

    //                    { "data": "EmployeeCode" },
    //                    { "data": "FirstName" },
    //                    { "data": "ManagerPriority" },
    //        ],
    //        "aoColumnDefs": [

    //         {
    //             "aTargets": [0],
    //             "sClass": "word-wrap"

    //         },
    //         {
    //             "aTargets": [1],
    //             "sClass": "word-wrap"
    //         },
    //         {
    //             "aTargets": [2],
    //             "sClass": "word-wrap"
    //         },
    //        ],
    //        ajax: function (data, callback, settings) {
    //            
    //            var employee = $EmployeeReport.EmployeeId;
    //            $.ajax({
    //                type: 'POST',
    //                url: $app.baseUrl + "Leave/AssignedUserselect",
    //                contentType: "application/json; charset=utf-8",
    //                data: JSON.stringify({ EmployeeId: employee }),
    //                dataType: "json",
    //                success: function (msg) {
    //                    
    //                    var out = msg.result;
    //                    console.log(out);

    //                    setTimeout(function () {
    //                        callback({
    //                            draw: data.draw,
    //                            data: out,
    //                            recordsTotal: out.length,
    //                            recordsFiltered: out.length
    //                        });

    //                    }, 50);
    //                },
    //                error: function (msg) {
    //                }
    //            });
    //        },
    //        fnInitComplete: function (oSettings, json) {
    //            
    //            var r = $('#Usermanagertbl tfoot tr');
    //            r.find('th').each(function () {
    //                $('#Usermanagertbl').css('padding', 8);
    //            });
    //            $('#Usermanagertbl thead').append(r);
    //            $('#search_0').css('text-align', 'center');

    //        },
    //        "aaSorting": [[1, "asc"]],
    //        "sSearch": "Search:",
    //        "bFilter": true,
    //        dom: "rtiS",
    //        "bDestroy": true,


    //        scroller: {
    //            loadingIndicator: true
    //        }
    //    });
    //},



    LoadDetail: function () {
        
        
        var dtClientList = $('#Usermanagertbl').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                        { "data": "Id" },
                        { "data": "EmployeeCode" },
                        { "data": "FirstName" },
                        { "data": "ManagerPriority" },
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
            ],
            ajax: function (data, callback, settings) {
                
                var employee = $EmployeeReport.EmployeeId;
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/AssignedUserselect",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: employee }),
                    dataType: "json",
                    async: false,
                    success: function (jsonResult) {
                        

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

                                }, 500);
                                break;
                            case false:
                                $app.showAlert(jsonResult.Message, 4);
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            //fnInitComplete: function (oSettings, json) {
            //    var r = $('#Usermanagertbl tfoot tr');
            //    r.find('th').each(function () {
            //        $(this).css('padding', 8);
            //    });
            //    $('#Usermanagertbl thead').append(r);
            //    $('#search_0').css('text-align', 'center');
            //},
                    fnInitComplete: function (oSettings, json) {
                        
                        var r = $('#Usermanagertbl tfoot tr');
                        r.find('th').each(function () {
                            $('#Usermanagertbl').css('padding', 8);
                        });
                        $('#Usermanagertbl thead').append(r);
                        $('#search_0').css('text-align', 'center');

                    },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });


    },
    LoadManagerReport: function () {
        


        var dtClientList = $('#tblleavereport').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [

                    {
                        "data": "LeaveDate",
                        render: function (data) {
                            
                            var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
                            var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
                            return dateF;
                        }
                    },
                    { "data": "LeaveDay" },
                    { "data": "Duration" },
                    { "data": "LeaveType" },


            ],
            "aoColumnDefs": [


         {
             "aTargets": [0, 1, 2, 3],
             "sClass": "word-wrap"

         },

            ],


            ajax: function (data, callback, settings) {
                

                var fdate = $("#txtMngrReportFromDate").val();
                var Tdate = $("#txtMngrReportToDate").val();
                var Empcode = $("#sltmgrviewrptEmpCode").val();
                var Levstatus = $("#sltleavestatus").val();
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "Leave/GetManagerReport",

                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ FromDate: fdate, ToDate: Tdate, Employeecodeid: Empcode, Leaverptstatus: Levstatus }),
                    async: false,
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
                                //$EmployeeLeaveReport.renderReporttitle(out);
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
                var r = $('#tblCalDetails tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblCalDetails thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });


    },
    managernameshow: function () {
        
        $app.showProgressModel();

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "LeaveRequest/Showmanagername",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ EMPID: $("#sltEmployeeRolelist").val() }),
            dataType: "json",
            success: function (jsonResult) {
                var out = jsonResult.result;
                switch (jsonResult.Status) {

                    case true:
                        
                        $app.hideProgressModel();
                        //document.getElementById('Userrptmngname').innerHTML = out;
                        $("#Userrptmngname").val(out);
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
}