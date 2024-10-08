$('#btnSaveLeaveopening').on('click', function () {
    
    if ($('#ddLeaveType').val() == "00000000-0000-0000-0000-000000000000") {
        $app.showAlert('Please Select Leave  Type', 4);
        $('#ddLeaveType').focus();
        return false;
    }
    else {
        $leaveOpening.SaveData();
    }
});


$('#ddLeaveType').change(function () {

    if ($('#ddLeaveType').find('option:selected').text() == "LOSS OF PAY DAYS") {
        $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
        $app.showAlert('You cannot set Leave openings for LOSS OF PAY DAYS', 4);
    }
    else if ($('#ddLeaveType').find('option:selected').text() == "ONDUTY") {
        $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
        $app.showAlert('You cannot set Leave openings for ONDUTY', 4);
    }
    else if ($('#ddLeaveType').find('option:selected').text() == "COMPOFF") {
        //$('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
        $('#btnSaveLeaveopening').addClass('nodisp');
        $app.showAlert('You cannot set Leave openings for COMPOFF', 4);
    }
    else {
        $('#btnSaveLeaveopening').removeClass('nodisp');
    }

});


var $leaveOpening = {
    canSave: true,
    selectedEmployeeId: null,
    employeeList: null,
    oDataCheckBal: null,
    LoadData: function () {
        
        var valueid = $('#ddLeaveType').val();
        var valuetext = $('#ddLeaveType').find('option:selected').text();
        if (valuetext.trim() == "LOSS OF PAY DAYS") //'199f5db2-14b7-46d3-a0e4-715d56682277'
        {
            $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
            $app.showAlert("You cannot set Leave openning for LOSS OF PAY DAYS", 4);
            return false;
        }
        if (valuetext.trim() == "ONDUTY") //'199f5db2-14b7-46d3-a0e4-715d56682277'
        {
            $('#ddLeaveType').val('00000000-0000-0000-0000-000000000000');
            $app.showAlert("You cannot set Leave openning for ONDUTY", 4);
            return false;
        }
        var dtClientList = $('#tblLeaveOpening').DataTable({

            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                     { "data": "empid" },
                     { "data": "empCode" },
                     { "data": "empName" },
                     { "data": "designation" },
                     { "data": "dateOfJoin" },
                     { "data": "leaveOpening" },
                     { "data": "leaveCredit" },
                     { "data": "leaveUsed" },
                     //{ "data": null }
            ],
            "aoColumnDefs": [
        {

            "aTargets": [0],
            "sClass": "nodisp empid",
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
             "aTargets": [5],
             "sClass": "actionColumn",

             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                 debugger;
                 var b;
                     if (oData.leaveopening=="Y")
                     {
                         b= $('<input type="text" class="txtLeaveOpenings" id="txtLeaveOpen*' + oData.empid + '" maxlength="5" onkeypress="return $validator.checkDecimal(event, 1)" onBlur="$leaveOpening.CheckLeaveUsed(this)"   value="' + sData + '"/>');

                     }
                     else
                     {
                         b = $('<input type="text" class="txtLeaveOpenings" id="txtLeaveOpen*' + oData.empid + '" maxlength="5" onkeypress="return $validator.checkDecimal(event, 1)" onBlur="$leaveOpening.CheckLeaveUsed(this)"   value="' + sData + '" readonly/>');
                     }
                     $(nTd).html(b);
                     //$leaveOpening.oDataCheckBal = oData;

                 }
             },
         {
            "aTargets": [6],
             "sClass": "actionColumn",
             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                 debugger;
                 var b;
                 if (oData.leaveopening == "Y")
                 {
                      b = $('<input type="text" class="txtLeaveCredit"  id="txtLeaveCredit*' + oData.empid + '" maxlength="5" onkeypress="return $validator.checkDecimal(event, 1)" onBlur="$leaveOpening.CheckLeaveUsed(this)"  value="' + sData + '"/>');
                 }
                 else
                 {
                      b = $('<input type="text" class="txtLeaveCredit"  id="txtLeaveCredit*' + oData.empid + '" maxlength="5" onkeypress="return $validator.checkDecimal(event, 1)" onBlur="$leaveOpening.CheckLeaveUsed(this)"  value="' + sData + '" readonly/>');
                 }
               
                 $(nTd).html(b);

             }

         },
            {
                "aTargets": [7],
                "sClass": "actionColumn",
                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    debugger;
                    var b = $('<input type="text" class="txtLeaveUsed"  id="txtLeaveUsed*' + oData.empid + '"    value="' + sData + '" readonly/>');
                    $(nTd).html(b);

                }

            }
      //,{
      //    "aTargets": [8],
      //    "sClass": "actionColumn",

      //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

      //        var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');

      //        c.button();
      //        c.on('click', function () {
      //            if (confirm('Are you sure ,do you want to delete?')) {
      //                $levFinanceYear.DeleteData(oData);
      //            }
      //            return false;
      //        });
      //        $(nTd).empty();
      //        $(nTd).prepend( c);
      //    }
      //}
            ], responsive: true,

            ajax: function (data, callback, settings) {
                
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Leave/GetLeaveOpenings",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ leavType: $("#ddLeaveType").val() }),
                    dataType: "json",
                    success: function (jsonResult) {
                        
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                var out = jsonResult.result;
                                $leaveOpening.employeeList = out;
                                setTimeout(function () {
                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        recordsTotal: out.length,
                                        recordsFiltered: out.length
                                    });



                                }, 50);
                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    searchData: function (fnType) {
        
        if ($('#sltLookup').val() != "0") {
            switch (fnType) {
                case "search":
                    var newDataSet = [];
                    var key = $('#sltLookup').val();
                    var value = $('#txtSearchData').val();
                    if (value != "") {
                        $.each($leaveOpening.employeeList, function (index, data) {
                            console.log(data);
                            
                            var re = new RegExp(value.toUpperCase() + ".*");
                            var dataVal = data[key].toUpperCase();
                            if (dataVal.match(re)) {
                                newDataSet.push(data);
                            }
                        });
                        $('#tblLeaveOpening').dataTable().fnClearTable();
                        if (newDataSet.length > 0) {
                            $('#tblLeaveOpening').dataTable().fnAddData(newDataSet);
                        }
                    } else {
                        $('#tblLeaveOpening').dataTable().fnClearTable();
                        $('#tblLeaveOpening').dataTable().fnAddData($employee.employeeList);
                    }
                    break;
                case "clear":
                    $('#sltLookup').val(0);
                    $('#txtSearchData').val('');
                    $('#tblLeaveOpening').dataTable().fnClearTable();
                    $('#tblLeaveOpening').dataTable().fnAddData($leaveOpening.employeeList);
                    break;
            }

        }
    },
    SaveData: function () {
        var rows = $("#tblLeaveOpening").dataTable().fnGetNodes();
        var dataValue = [];
        for (var i = 0; i < rows.length; i++) {

            var newattr = new Object();
            if ($(rows[i]).find(".txtLeaveOpenings").val() != "") {

                newattr.EmployeeId = $(rows[i]).find(".empid").html();
                newattr.LeaveOpening = $(rows[i]).find(".txtLeaveOpenings ").val();
                newattr.LeaveCredit = $(rows[i]).find(".txtLeaveCredit ").val();
                newattr.LeaveType = $("#ddLeaveType").val();


                dataValue.push(newattr);

            }

        }
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Leave/SaveLeaveOpenings",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dataValue: dataValue }),
            dataType: "json",
            success: function (jsonResult) {
                
                switch (jsonResult.Status) {
                    case true:
                        $leaveOpening.LoadData();
                        $app.showAlert('Saved Successfully', 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
                //$leaveOpening.LoadData();
                //$app.showAlert('Saved Successfully', 2);
            },
            complete: function () {

            }
        });

    },
    //CreditProcess: function () {
    //    $app.showProgressModel();
    //    var formData = document.forms["leaveCreditProcess"];
    //    var data = {
    //        CategoryId: formData.elements["sltRCategorylist"].value,
    //        LeaveTypeId: formData.elements["ddLeaveType"].value,
    //        ProcessDate: formData.elements["txtProcessDate"].value,
    //    };
    //    $.ajax({
    //        url: $app.baseUrl + "Leave/LeaveCreditProcess",
    //        data: JSON.stringify({ dataValue: data }),
    //        dataType: "json",
    //        contentType: "application/json",
    //        type: "POST",
    //        success: function (jsonResult) {
    //            $app.clearSession(jsonResult);
    //            switch (jsonResult.Status) {
    //                case true:
    //                    //$('#AddUser').modal('toggle');
    //                    //$User.LoadUser();
    //                    $app.hideProgressModel();
    //                    $app.showAlert(jsonResult.Message, 2);
    //                    //alert(jsonResult.Message);
    //                    var p = jsonResult.result;
    //                    companyid = 0;
    //                    break;
    //                case false:
    //                    $app.hideProgressModel();
    //                    $app.showAlert(jsonResult.Message, 4);
    //                    //alert(jsonResult.Message);
    //                    break;
    //            }
    //        },
    //        complete: function () {
    //            $app.hideProgressModel();
    //        }
    //    });

    //},
    CheckLeaveUsed: function (context) {
        
        var txtleavOpen, txtLeavCred, txtLeavUsed, txtleaveTotLevOpen
        var LevopnId = context.id;
        var Id = LevopnId.split("*");
        var EmpId = Id[1];
        $(context).closest('tr').find('input').each(function () {
            if ($(this).attr("id") == "txtLeaveOpen" + "*" + EmpId) txtleavOpen = parseFloat($(this).val());
            if ($(this).attr("id") == "txtLeaveCredit" + "*" + EmpId) txtLeavCred = parseFloat($(this).val());
            if ($(this).attr("id") == "txtLeaveUsed" + "*" + EmpId) txtLeavUsed = parseFloat($(this).val());

        });
        txtleaveTotLevOpen = txtleavOpen + txtLeavCred;
        if (txtleaveTotLevOpen < txtLeavUsed) {
            $app.showAlert("Please enter the LeaveOpening Value Greater Than LeaveUsed", 4);
            document.getElementById(LevopnId).value = ""
            $(context).focus();
        }
    },

    CheckDecimalValidate: function (evt) {
        
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
            return false;
        else {
            var len; var index;

            len = (evt.currentTarget.value + evt.key).length;
            index = (evt.currentTarget.value + evt.key).indexOf('.');

            if (index > 0 && charCode == 46) {
                return false;
            }
            if (index > 0) {
                charCode
                var CharAfterdot = (len + 1) - index;
                if (CharAfterdot > 3) {
                    return false;
                }
                if (charCode != 53) {
                    $app.showAlert('Invalid Data', 4);
                    evt.currentTarget.value = "";
                    evt.key = "";
                    return false;
                }
            }

        }
        return true;

    }

}
