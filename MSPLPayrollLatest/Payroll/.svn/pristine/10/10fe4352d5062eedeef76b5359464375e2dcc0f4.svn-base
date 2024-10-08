$(document).ready(function() {
$('input[type=radio][name=LeaveCalender]').change(function () {
    
    if (this.value == '0') {
        $('#hidingid').addClass('nodisp');
        $Calendar.calenderload();
         }
    else if (this.value == '1') {

        $('#hidingid').removeClass('nodisp');
    }
});
$('#btncalendarsearch').on('click', function () {
    
    $Calendar.Load();
});
});




var $Calendar = {
    Data: null,
    formData: document.forms["frmLeaveDetails"],
    Load: function () {
        
        //var selectedvalue = $('input[name=LeaveCalender]:checked', '#myForm').val();
        //if (selectedvalue == null)
        //{
        //    selectedvalue = 0;
        //}
        //var fromdate = $('#txtFromDateid').val();
        //var todate =$('#txtToDateid').val();
        $.ajax({
           
            url: $app.baseUrl + "Leave/GetLeaveSchedule",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ datavalue:null }),
           // data: JSON.stringify({ LeaveCalender: selectedvalue, txtFromDateid: fromdate, txtToDateid: todate }),
            dataType: "json",
            type: 'POST',
            async: false,
            success: function (jsonResult) {
                //$app.clearSession(jsonResult);
                
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;



                        //$Calendar.calenderload();
                        for (i = 0; i < out.length; i++) {

                           
                            var source = {
                                events: [
                                    {
                                        //data




                                        className: out[i].className,
                                        end: out[i].LeaveDate,
                                        id: out[i].LeavetypeGUid,
                                        start: out[i].LeaveDate,
                                        title: out[i].title,
                                        userRole: out[i].Userrole,


                                       

                                    }
                                ]
                            };
                            $('#calendar').fullCalendar('addEventSource', source);
                            
                        }
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 'danger');
                        alert(jsonResult.Message);
                        break;
                }

            },
            complete: function () {
                
                $('#calendar').fullCalendar;


            }
        });

    },

    calenderload: function () {
        
        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },eventLimit:true,
            //defaultDate: '2016-06-12',        
            eventRender: function (event, element) {
                
                var myColors = ['Black', 'Red', 'Pink', 'Yellow', 'Brown', 'Purple', 'Blue', 'Green', 'Orange', 'SteelBlue', 'Lime'];
                element.css(
                    "background-color", event.className
                );
            },

            eventClick: function (event) {
                                
                //$Calendar.Data = event;
                if (event.userRole == "HOLIDAY") {

                }
                else {


                    $.ajax({
                        type: 'POST',
                        url: $app.baseUrl + 'Company/DoModule',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ actionVal: "LeaveCalendarDetailsRender", date: event.start._i, id: event.id }),
                        // dataType: 'json',
                        async: false,
                        success: function (data) {
                            
                            console.log(data);
                            if (data.Status) {


                                window.location.href = data.result;


                            }

                        },
                        error: function (data) {
                            $('#dvError').removeClass('nodisp');
                            $('#lblError').text('There is some error.Please try again later.');
                        }
                    });
                }
            }
        });
        $Calendar.Load();
    },

    






    LoadCalendarDetails: function (date,id) {
    
    
    
    var dtClientList = $('#tblCalDetails').DataTable({
        'iDisplayLength': 10,
        'bPaginate': true,
        'sPaginationType': 'full',
        'sDom': '<"top">rt<"bottom"ip><"clear">',
        columns: [
                { "data": "EmpId" },
                { "data": "Empcode" },
                { "data": "Name" },
                { "data": "UsedDays" },
                { "data": "AvailableDays" },
                { "data": "TotalDays" },
              
        ],
        "aoColumnDefs": [
    {
        "aTargets": [0],
        "sClass": "nodisp",
        "bSearchable": false
    },
    
     {
         "aTargets": [1, 2, 3, 4, 5],
         "sClass": "word-wrap"

     },
  
        ],
        ajax: function (data, callback, settings) {
            
            $.ajax({
                type: 'POST',
                cache:false,
               url: $app.baseUrl + "Leave/GetCalendardatedata",
              
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ LeaveDate: date, LeavetypeGUid: id }),
               
                async:false,
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
                            $Calendar.renderCalendar(out);
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

    LoadCalendarDetailsLOP: function (date, id) {
        


        var dtClientList = $('#tblCalDetailslop').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "EmpId" },
                    { "data": "Empcode" },
                    { "data": "Name" },
                    //{ "data": "UsedDays" },
                    //{ "data": "AvailableDays" },
                    //{ "data": "TotalDays" },

            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },

         {
             "aTargets": [1, 2],
             "sClass": "word-wrap"

         },

            ],
            ajax: function (data, callback, settings) {
                
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "Leave/GetCalendardatedataLOP",

                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ LeaveDate: date, LeavetypeGUid: id }),

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
                                $Calendar.renderCalendar(out);
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









    LoadEmployeeLeaveDashboard: function () {
        


        var dtClientList = $('#tblEmpLevDetails').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "LeaveTitle" },
                    { "data": "TotalDays" },
                    { "data": "UsedDays" },
                    //{ "data": "Debitdays" },
                    { "data": "AvailableDays" },
                    

            ],
            "aoColumnDefs": [
        

         {
             "aTargets": [0, 1, 2, 3],
             "sClass": "word-wrap"

         },

            ],
            ajax: function (data, callback, settings) {
                
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "Leave/GetEmpdashboarddata",

                    contentType: "application/json; charset=utf-8",
                    data:null,

                    async: false,
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        //console.log(jsonresult.result);
                        switch (jsonResult.Status) {
                            case true:
                                
                                var out = jsonResult.result;
                                var namemessage = jsonResult.Message;
                                setTimeout(function () {
                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length,
                                       
                                    });

                                }, 50);
                                $Calendar.renderempleavedashboard(out, namemessage);
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
                var r = $('#tblEmpLevDetails tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblEmpLevDetails thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });


    },







    /////////////////////////////////////////////



    LoadEmployeeHolidayDashboard: function () {
        


        var dtClientList = $('#tblEmpHolidayDetails').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    {
                        "data": "Holidaydate",

                        render: function (data) {
            
            var date = new Date(parseInt(data.replace(/(^.*\()|([+-].*$)/g, '')));
            var dateF = date.getDate() + '/' + $payroll.GetMonthName((date.getMonth() + 1)) + '/' + date.getFullYear();
            return dateF;
        }
                    },
                     { "data": "holidayDAY" },
                    { "data": "HolidayReason" },
                   

            ],
            "aoColumnDefs": [


         {
             "aTargets": [0, 1,2],
             "sClass": "word-wrap"

         },

            ],
            ajax: function (data, callback, settings) {
                
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: $app.baseUrl + "Leave/GetEmpHolidaylistdata",

                    contentType: "application/json; charset=utf-8",
                    data: null,

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
                               // $Calendar.renderempleavedashboard(out);
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
                var r = $('#tblEmpHolidayDetails tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblEmpHolidayDetails thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });


    },




    ////////////////////////////////////////////








    renderCalendar: function (data) {
        
       
        jQuery("label[for='myvalue']").html(data[0].LeaveTitle)
        
    },

    renderempleavedashboard: function (data,name) {
        

        jQuery("label[for='myvalue']").html(name)

    },



    //GETLOPIDANDONDUTYID: function () {
    //    
        
    //    if (id1 != "199f5db2-14b7-46d3-a0e4-715d56682277") {
    //        $('#accordion2').addClass('nodisp');
    //        $('#accordion1').removeClass('nodisp');
    //        $Calendar.LoadCalendarDetails(da, id1);
    //    }
    //    else {
    //        $('#accordion1').addClass('nodisp');
    //        $('#accordion2').removeClass('nodisp');
    //        $Calendar.LoadCalendarDetailsLOP(da, id1);
    //    }
    //},

    //GETDEFAULTCREATEDID: function () {
    //    

    //    $.ajax({
    //        url: $app.baseUrl + "Leave/GetLOPandONDUTYID",
    //        data: null,
    //        dataType: "json",
    //        contentType: "application/json",
    //        type: "POST",
    //        success: function (jsonResult) {
    //            
    //            $app.clearSession(jsonResult);
    //            switch (jsonResult.Status) {
    //                case true:
    //                    
    //                    var p = jsonResult.result;
    //                    //$mailConfiguration.render(p);
    //                    return
    //                    break;
    //                case false:
    //                    $app.showAlert(jsonResult.Message, 4);
    //                    break;
    //            }
    //        },
    //        complete: function () {

    //        }
    //    });
    //}



}
