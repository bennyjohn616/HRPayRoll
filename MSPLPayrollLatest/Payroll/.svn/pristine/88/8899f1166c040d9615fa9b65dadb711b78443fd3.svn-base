
$("#sltCategorylist").change(function () {
    
    if ($("#sltCategorylist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#dvEmpCode").addClass('nodisp');
        $("#dvLoanAdd").addClass('nodisp');
        $("#dvLoanTable").addClass('nodisp');
    }
    else {
        $("#dvEmpCode").removeClass('nodisp');
        $companyCom.loadSelectiveEmployee({ id: 'sltEmployeelist', condi: 'Category.' + $("#sltCategorylist").val() });
    }
});

$("#sltEmployeelist").change(function () {

    if ($("#sltEmployeelist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#dvLoanAdd").addClass('nodisp');
        $("#dvLoanTable").addClass('nodisp');
    }
    else {
        $("#dvLoanAdd").removeClass('nodisp');
        $("#dvLoanTable").removeClass('nodisp');
        $LoanEntry.selectedEmployeeId = $("#sltEmployeelist").val();
        //$LoanEntry.LoadLoanEntry();
        $LoanEntry.initiateFormEmployee($LoanEntry.selectedEmployeeId, 'transHtml');
    }
});
$(document).ready(function () {
    
    //$("#contentDiv").html(innerHtml);
    $('#txtInstallmentdate').datepicker();
    $('#txtInstallmentdate').datepicker("show");
});



var $LoanEntry = {
    LoanEntryId: '',
    LoanMasterId: '',
    IsInterestRate:'',
    selectedEmployeeId: '',
    loantable: 'tblLoanEntry',
    tableId: 'tblLoanTran',
    formData: document.forms["frmLoanEntry"],
    designForm: function (renderDiv) {
        var formrH = '<div class="col-sm-12"> ';
        var formrH = ' <div class="col-sm-12 text-right" id="dvLoanAdd"> <h4 class="pull-left">Loan </h4>';
        formrH = formrH + '<input type="button" id="btnAddLoanEntry" value="Add" class="btn custom-button marginbt7" data-toggle="modal" data-target="#AddLoanEntry" onclick="$LoanEntry.AddInitialize();">'
        formrH = formrH + '</div>';
        formrH = formrH + '<div class="col-sm-12 scrol table-responsive" id="dvLoanTable"></div>';

        formrH = formrH + '<div id="AddForeColse" class="modal fade" role="dialog" aria-hidden="true" data-backdrop="static"data-keyboard="false">'
        formrH = formrH + ' <div id="renderId"></div>';
        formrH = formrH + '</div>';
        //After clicking add  
        formrH = formrH + '<div id="AddLoanEntry" class="modal fade" role="dialog" aria-hidden="true" data-backdrop="static"data-keyboard="false">'
        //formrH = formrH + '
        formrH = formrH + '<form id="frmLoanEntry">';
        formrH = formrH + '<div class="modal-dialog">';
        formrH = formrH + '<div class="modal-content">'
        formrH = formrH + '<div class="modal-header">'
        formrH = formrH + ' <button type="button" class="close" data-dismiss="modal" onClick="$LoanEntry.Enablefields()">&times;</button>'
        formrH = formrH + ' <h4 class="modal-title" id="H4">'
        formrH = formrH + 'Add/Edit Loan Entry'
        formrH = formrH + '</h4>'
        formrH = formrH + '</div>';//model headr closed
        formrH = formrH + '<div class="modal-body">'; //first
        formrH = formrH + '<div class="form-horizontal">';//start horizontal div
        //Loan
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Loan <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-7"><select id="sltddLoan" class="form-control" placeholder="Enter the Loan" required ></select> </div>';
        formrH = formrH + '</div>';
        //Loan Date
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4"> Loan Date <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-7"><input type="text" id="txtLoanDate" class="form-control datepicker" placeholder="Enter the Date of loan" readonly required /> </div>';
        formrH = formrH + '</div>';
        //  Apply On(Month and Year)
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4"> Apply On(Month and Year) <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-7"><input type="text" id="txtMonthYear" class="form-control datepicker" onChange="$LoanEntry.ValidateMonth()" placeholder="Enter the Month and Year of Loan" readonly required /> </div>';
        formrH = formrH + '</div>';
        //  Loan Amount
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4"> Loan Amount <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-7"><input type="text" id="txtLoanAmt" onkeypress="return $validator.checkDecimal(event, 2)" oncopy="return false" onpaste="return false" maxlength="10" class="form-control" placeholder="Enter the Amount of Loan" required /> </div>';
        formrH = formrH + '</div>';
        //  No of Installments
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4"> No of Installments <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-7"><input type="text" id="txtnoofinstall" onkeypress="return $validator.IsNumeric(event, this.id)" oncopy="return false" onpaste="return false" maxlength="3" class="form-control" placeholder="Enter the Number of Installments" required /> </div>';
        formrH = formrH + '</div>';
        //  Per Month
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label id="lblpermonth" class="control-label col-md-4"> Per Month <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-7"><input type="text" id="txtpermonth" class="form-control" placeholder="Enter the Per Month of Amount" required /> </div>';
        formrH = formrH + '</div>';

        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';//model body close
        // formrH = formrH + '</div>';//row end 
        //button row start
        formrH = formrH + '<div class="modal-footer">';
        formrH = formrH + '<button type="submit" id="btnSave" class="btn custom-button"> Save</button>';
        formrH = formrH + '<button type="button" class="btn custom-button" data-dismiss="modal" onClick="$LoanEntry.Enablefields()">Close </button> </div>';
        //button row end
        // formrH = formrH + '<div class="col-md-12"><div id="dvTransTable"></div></div>';//for table       
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';
        formrH = formrH + '</form>';//form end
        formrH = formrH + '</div>';
        $('#' + renderDiv).html(formrH);//transHtml
        $LoanEntry.formData = document.forms["frmLoanEntry"];

    },
    loanGridObject: function () {
        
        var gridObject = [
                { tableHeader: "id", tableValue: "loanEntryid", cssClass: 'nodisp' },
                { tableHeader: "Description", tableValue: "loanName", cssClass: 'popup' },
                { tableHeader: "Date", tableValue: "loanDate", cssClass: '' },
                { tableHeader: "Apply Month and Year", tableValue: "loanApplyMonthYear", cssClass: '' },
                { tableHeader: "Loan Amount", tableValue: "loanAmt", cssClass: '' },
                { tableHeader: " No.of Months", tableValue: "NoofInstall", cssClass: '' },
                { tableHeader: "Per Month", tableValue: "Permonth", cssClass: '' },
                { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }
        ];
        return gridObject;
    },
    initiateFormEmployee: function (employeeId, renderDiv) {
        
        $LoanEntry.selectedEmployeeId = employeeId;
        $LoanEntry.IsInterest = false;
        $LoanEntry.IsInterestRate = 0;
        $LoanEntry.designForm(renderDiv);
        $payroll.loadMonth({ id: 'ddApplyMonth' });
        $companyCom.loadLoanMaster({ id: "sltddLoan" });
        $LoanEntry.createLoanGrid();

        //  $LoanEntry.loadComponent();
        $('#frmLoanEntry').on('submit', function (event) {
            if ($app.requiredValidate('btnLoanEntrySave', event)) {
                $LoanEntry.save();
                return false;
            }
            else {
                return false;
            }
        });
        $payroll.initDatetime();

        $("#txtnoofinstall,#txtLoanAmt").change(function () {
            $LoanEntry.ProcessLoanInstallmonth();
        });
        $("#txtpermonth").change(function () {
            $LoanEntry.ProcessNoOfMonth();
        });

        $("#sltddLoan").change(function () {
            $LoanEntry.LoanMasterId = $("#sltddLoan").val();
            if ($("#sltddLoan").val() == "00000000-0000-0000-0000-000000000000") {
                $LoanEntry.LoanMasterId = '';
                alert($("#sltddLoan").val());
            }
            else {
                $.ajax({
                    url: $app.baseUrl + "Loan/GetLoanMasterData",
                    data: JSON.stringify({ loanid: $LoanEntry.LoanMasterId }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                           
                            case true:
                                //$('#AddLoanEntry').modal('toggle');
                                var p = jsonResult.result;
                                
                                $LoanEntry.IsInterest = p.loanIsInterest;
                                console.log($LoanEntry.IsInterest + '*' + p.loanInterestPercent);
                                if ($LoanEntry.IsInterest) {
                                    $('#lblpermonth').text('EMI Per Month (' + p.loanInterestPercent + '%)')
                                    $LoanEntry.IsInterestRate = p.loanInterestPercent;

                                }
                                else {
                                    $('#lblpermonth').text('Per Month');
                                    $LoanEntry.IsInterestRate = 0;
                                }
                                $LoanEntry.ProcessLoanInstallmonth();
                                $LoanEntry.ProcessNoOfMonth();
                                break;
                            case false:
                                $app.showAlert("EMI Processing Error", 4);
                                break;
                        }
                    },
                    complete: function () {

                    }
                });
            }
        });
    },
    ProcessLoanInstallmonth: function () {
        
        var Loanamt = $("#txtLoanAmt").val();
        var Noofinstall = $("#txtnoofinstall").val();

        if (Loanamt != "" && Noofinstall != "" && Noofinstall != 0) {
            console.log(Noofinstall + ',' + Loanamt + ',' + $LoanEntry.IsInterestRate);
            if ($LoanEntry.IsInterestRate == 0) {
                $('#txtpermonth').val((Loanamt / Noofinstall).toFixed(2));
            } else {
                var EMIAmt = $LoanEntry.EMICalculation(Noofinstall, Loanamt, $LoanEntry.IsInterestRate);
                $('#txtpermonth').val(EMIAmt.toFixed(2));
            }
        }
        else {
            $('#txtpermonth').val(null);
        }
    },
    //Created By AjithPanner on 27/11/17
    ProcessNoOfMonth: function () {
        
        var Loanamt = $("#txtLoanAmt").val();
        var perMonthValue = $("#txtpermonth").val();
        if (Loanamt != "" && perMonthValue != "") {
            console.log(perMonthValue + ',' + Loanamt + ',' + $LoanEntry.IsInterestRate);
            if ($LoanEntry.IsInterestRate == 0) {
                var NoOfInstall = (Loanamt / perMonthValue);
                isNoOfInstallValid = NoOfInstall % 1;
                if (isNoOfInstallValid == 0) {
                    $("#txtnoofinstall").val(NoOfInstall);
                }
                else {
                    $app.showAlert("Loan Amount and Per month value Invalid", 4);
                    $("#txtnoofinstall").val(null);
                }
            }
            else {
                $LoanEntry.ProcessLoanInstallmonth();
            }
        }
        else {
            $('#txtnoofinstall').val(null);
        }


    },
    datepick: function () {
        

        $('#txtInstallmentdate').datepicker();
        $('#txtInstallmentdate').datepicker("show");

    },
    //creted by mubarak
    //In order to enable all the fields while clicking remove or close button in loan entry screen.
    Enablefields: function () {
        
        var formData1 = document.forms["frmLoanEntry"];
            formData1.elements.sltddLoan.disabled = false;
            formData1.elements.txtLoanDate.disabled = false;
            formData1.elements.txtMonthYear.disabled = false;
            formData1.elements.txtLoanAmt.disabled = false;
            formData1.elements.txtnoofinstall.disabled = false;
            formData1.elements.txtpermonth.disabled = false;
            formData1.elements.btnSave.disabled = false;

    },
    ValidateMonth: function () {
        
        var formData = document.forms["frmLoanEntry"];
        var loandate = new Date( formData.elements["txtLoanDate"].value);
        var applymonthyear = new Date(formData.elements["txtMonthYear"].value);
       // if (!(applymonthyear.getMonth()) >= loandate.getMonth() && !(applymonthyear.getFullYear()) >= loandate.getFullYear()) {
        if (applymonthyear < loandate) {
            $app.showAlert("Invalid Date");
            $("#txtMonthYear").val('');
            $("#txtMonthYear").focus();
            return false;
        }
    },
    pdf: function (context) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Loan/pdf",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ 'Id': context.Id}),
            dataType: "json",
            async: false,
            success: function (jsonResult) {
                var oData = new Object();
                oData.filePath = jsonResult.result.filePath;
                $app.downloadSync('Download/DownloadPaySlip', oData);
                return false;
            }
        });
    },
    datepickValue: function () {
        
        var transRowsDateCheck = $("#tblLoanTran").dataTable().fnGetNodes();

        for (i = 0; i < transRowsDateCheck.length; i++) {
            
            //if ($(transRowsDateCheck[i]).find('.status').html() == "Paid") {
            //    var transpaid = [];

            //    transpaid.appliedOn = $(transRowsDateCheck[i]).find('.appliedOn').html();
            //    transpaid.push(transpaid.appliedOn);
                
            //}
            //else 
            if ($(transRowsDateCheck[i]).find('.status').html() == "Paid" || $(transRowsDateCheck[i]).find('.txtAmount').prop('disabled') != true) {
               
                var applydate = new Date($("#txtInstallmentdate").val());
                var applyon = new Date($(transRowsDateCheck[i]).find('.appliedOn').html());

                if (applydate.getMonth() == applyon.getMonth() && applydate.getFullYear() == applyon.getFullYear())
                {
                    $app.showAlert("Invalid Date");
                    $("#txtInstallmentdate").val('');
                    return false;
                }

                //Check Paid month
                if($(transRowsDateCheck[i]).find('.status').html() == "Paid" && applydate.getMonth() <= applyon.getMonth() && applydate.getFullYear() <= applyon.getFullYear()){
                    $app.showAlert("apply date should not be lesser then paid date");
                    $("#txtInstallmentdate").val('');
                    return false;
                }
              
                //var transunpaid = [];
                //var transunpaid1 = new Object();
                //transunpaid1.appliedOn = $(transRowsDateCheck[i]).find('.appliedOn').html();
                //transunpaid.push(transunpaid1);
            }

        //    else
        //{
        //            var transunpaidDEL = new Object();

        //            transunpaidDEL.appliedOn = $(transRowsDateCheck[i]).find('.appliedOn').html();
        //        }

        }

        if ($("#txtInstallmentdate").val() != "") {
            var valDate = new Date($("#txtInstallmentdate").val());
            $("#txtInstallmentdate").val(valDate.getDate() + '/' + $payroll.GetMonthName((valDate.getMonth() + 1)) + '/' + valDate.getFullYear());
        }

        //var paiddate1 = new Date(transpaid.appliedOn);
        //var applydate = new Date($("#txtInstallmentdate").val());
        //applydate = $payroll.GetMonthName((applydate.getMonth() + 1)) + '/' + applydate.getFullYear();
        //if ($("#txtSeparationDate").val() != '' && $("#txtReleaseDate").val() != '') {
        //    if (dateto > datefrom) {

        //    }
        //    else {

        //        $app.showAlert('LastWorkingDate should not be less than Date of Joining !', 3);
        //        $("#txtReleaseDate").val('');
        //    }
        //}





       

    },
    EMICalculation: function (numberOfPayments, loanAmount, yearlyInterestRate) {
        
        var rate = parseFloat(yearlyInterestRate) / 100 / 12;
        var denaminator = Math.pow((1 + rate), parseFloat(numberOfPayments)) - 1;
        return (rate + (rate / denaminator)) * parseFloat(loanAmount);
    },
    createLoanGrid: function () {

        var gridObject = $LoanEntry.loanGridObject();
        var tableid = { id: $LoanEntry.loantable };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvLoanTable').html(modelContent);
        $LoanEntry.LoadLoanEntry(gridObject, tableid);
    },

    initpage: function () {
        $payroll.initDatetime();
        $payroll.initMonthYear();
        $companyCom.loadLoanMaster({ id: "sltddLoan" });
        $companyCom.loadCategory({ id: "sltCategorylist" });
        //$companyCom.loadEmployee({ id: "sltEmployeelist" });
        $("#dvEmpCode").addClass('nodisp');
        $("#dvLoanAdd").addClass('nodisp');
        $("#dvLoanTable").addClass('nodisp');
    },
    loadComponent: function () {
        var gridObject = $LoanEntry.loanTranGridObject();
        var tableid = { id: $LoanEntry.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvLoanTable').html(modelContent);
        var data = null;
        $LoanEntry.loadLoanTrans(data, gridObject, tableid);
    },
    LoadLoanEntry: function (context, tableId) {
        
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'popup') {
                columnDef.push(
                       {
                           "aTargets": [cnt],
                           "sClass": "actionColumn",
                           "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                               var b = $('<a href="#" class="editeButton"><span>' + sData + '</span></a>');
                               b.on('click', function () {
                                   $LoanEntry.renderLoanTran(oData);
                                   return false;
                               });
                               $(nTd).html(b);
                           }
                       }

                   ); //for action column

            }
            else if (context[cnt].cssClass == 'actionColumn') {
                columnDef.push(
                        {
                            "aTargets": [cnt],
                            "sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                                var d = $('<a href="#" class="dpdfButton" title="pdf"><span aria-hidden="true" class="glyphicon glyphicon-list-alt"></span></button>');
                                var forclosetext = 'Fore Close';
                                if (oData.foreclosed) {
                                    forclosetext = 'Reverse Fore Close';

                                }
                                var e = $('<button type="button" class="btn custom-button" data-dismiss="modal">' + forclosetext + '</button>');
                                b.button();
                                b.on('click', function () {
                                    
                                    $LoanEntry.GetLoanEntryData({ Id: oData.loanEntryid });
                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    if (confirm('Are you sure ,do you want to delete?')) {
                                        $LoanEntry.DeleteLoanEntryData({ Id: oData.loanEntryid });
                                    }
                                    return false;
                                });
                                e.button();
                                e.on('click', function () {
                                    if (confirm('Are you sure ,do you want to Foreclose?')) {
                                        $LoanEntry.ForeClose({ Id: oData.loanEntryid, Fclose: oData.foreclosed });
                                    }
                                    return false;
                                });
                                d.button();
                                d.on('click', function () {
                                    debugger;
                                    $LoanEntry.pdf({ Id: oData.loanEntryid });
                                });

                                $(nTd).empty();
                                $(nTd).prepend(e, b, c,d);
                            }
                        }

                    ); //for action column
            }
            else if (context[cnt].cssClass == 'edit') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="text" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
                        $(nTd).html(b);
                    }
                });

            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Loan/GetLoanEntry",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ 'employeeId': $LoanEntry.selectedEmployeeId }),
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
                                break;
                            case false:
                                $app.showAlert(jsonResult.Message, 4);
                                //alert(jsonResult.Message);
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#' + tableId.id + ' tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + tableId.id + ' thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    save: function () {
        
        $app.showProgressModel();
        var formData = document.forms["frmLoanEntry"];
      
        var data = {
            loanEntryid: $LoanEntry.LoanEntryId,
            employeeId: $LoanEntry.selectedEmployeeId,
            InterestRate:$LoanEntry.IsInterestRate,
            loanMasterId: formData.elements["sltddLoan"].value,
            loanDate: formData.elements["txtLoanDate"].value,
            loanApplyMonthYear: formData.elements["txtMonthYear"].value,
            loanAmt: formData.elements["txtLoanAmt"].value,
            NoofInstall: formData.elements["txtnoofinstall"].value,
            Permonth: formData.elements["txtpermonth"].value,
        };
        $.ajax({
            url: $app.baseUrl + "Loan/SaveLoanEntry",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddLoanEntry').modal('toggle');
                        $LoanEntry.createLoanGrid();//LoadLoanEntry();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        //alert(jsonResult.Message);
                        var p = jsonResult.result;
                        companyid = 0;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },

    AddInitialize: function () {
        
        var formData = document.forms["frmLoanEntry"];
        $LoanEntry.LoanEntryId = '';
        formData.elements["sltddLoan"].value = "0";
        formData.elements["txtLoanDate"].value = "";
        formData.elements["txtMonthYear"].value = "";
        formData.elements["txtLoanAmt"].value = "";
        formData.elements["txtnoofinstall"].value = "";
        formData.elements["txtpermonth"].value = "";

    },

    GetLoanEntryData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Loan/GetLoanEntryData",
            data: JSON.stringify({ loanEntryid: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddLoanEntry').modal('toggle');
                        var p = jsonResult.result;
                        $LoanEntry.RenderData(p);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });

    },
    DeleteLoanEntryData: function (context) {

        $.ajax({
            url: $app.baseUrl + "Loan/DeleteLoanEntryData",
            data: JSON.stringify({ id: context.Id, type: "LoanEntry" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        // $LoanEntry.LoadLoanEntry();
                        $LoanEntry.createLoanGrid();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    //DeleteLoanTransData: function (context) {
    //    
    //    $.ajax({
    //        url: $app.baseUrl + "Loan/DeleteLoanTrans",
    //        data: JSON.stringify({ id: context.id}),
    //        dataType: "json",
    //        contentType: "application/json",
    //        type: "POST",
    //        success: function (jsonResult) {
    //            $app.clearSession(jsonResult);
    //            switch (jsonResult.Status) {
    //                case true:
    //                    // $LoanEntry.LoadLoanEntry();
    //                    $LoanEntry.loadComponent();
    //                    break;
    //                case false:
    //                    $app.showAlert(jsonResult.Message, 4);
    //                    //alert(jsonResult.Message);
    //                    break;
    //            }
    //        },
    //        complete: function () {

    //        }
    //    });
    //},
    ForeClose: function (context) {

        var forclosetext = 'Fore Close';
        if (context.Fclose) {
            forclosetext = 'Reverse Fore Close';
        }
        var formAttribute = [];
        var returnval = '<form role="form" id="foreclose"><div class="modal-dialog"> '
        + '<div class="modal-content"> '
        + ' <div class="modal-header">  <button type="button" class="close" data-dismiss="modal">'
                + '  &times;</button>'
            + '  <h4 class="modal-title" id="H4">'
            + '    Add/Edit Fore Close</h4>'
        + ' </div>'
        + ' <div class="modal-body"> <div class="form-horizontal"> '
        + ' <div class="form-group"> <label class="control-label col-md-4">' + forclosetext + ' Date</label>'
                        + '<div class="col-md-6"> <input type="text" class="form-control datepicker"   id="' + forclosetext + '" '
        + 'placeholder="' + forclosetext + ' Date"> </div> </div> '

          + ' <div class="form-group"> <label class="control-label col-md-4">Reason</label>'
                        + '<div class="col-md-6"> <input type="text" class="form-control"   id="txtReason" '
        + 'placeholder="Enter Reason"> </div> </div>   </div></div>';
        returnval = returnval + ' <div class="modal-footer">'
            + ' <button type="submit" id="btnForcloseSave" class="btn custom-button">'
                + 'Save</button>'
            + ' <button type="button" class="btn custom-button" data-dismiss="modal">'
                + 'Close</button>'
        + ' </div> </div>';
        returnval = returnval + '</form>'
        //returnval = returnval.replace('{formelemnt}', formelemnt);

        document.getElementById("renderId").innerHTML = returnval;
        $payroll.initDatetime();
        $('#AddForeColse').modal('toggle');
        $("#btnForcloseSave").click(function (event) {
            if ($app.requiredValidate("foreclose", event)) {
                var formData = document.forms["foreclose"];
                var data = {
                    loanEntryid: context.Id,
                    foreclosed: context.Fclose,
                    reason: formData.elements["txtReason"].value,
                    forecloseOrReversedDate: formData.elements[forclosetext].value
                }
                $LoanEntry.ForeClosesave(data);

                return false;
            }
            else {
                return false;
            }
        });


    },

    ForeClosesave: function (data) {

        $.ajax({
            url: $app.baseUrl + "Loan/ForeCloseSaveLoanEntry",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddForeColse').modal('toggle');
                        $LoanEntry.createLoanGrid(); //$LoanEntry.LoadLoanEntry();
                        $app.showAlert(jsonResult.Message, 2);
                        //alert(jsonResult.Message);
                        var p = jsonResult.result;
                        companyid = 0;
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });
    },

    RenderData: function (data) {
        debugger;
        var formData = document.forms["frmLoanEntry"];
        console.log(data);
        $LoanEntry.LoanEntryId = data.loanEntryid;
        formData.elements["sltddLoan"].value = data.loanMasterId;
        formData.elements["txtLoanDate"].value = data.loanDate;
        formData.elements["txtMonthYear"].value = data.loanApplyMonthYear;
        formData.elements["txtLoanAmt"].value = data.loanAmt;
        formData.elements["txtnoofinstall"].value = data.NoofInstall;
        formData.elements["txtpermonth"].value = data.Permonth;
        for (i = 0; i < data.loanTrans.length; i++) {
            if (data.loanTrans[i].status == "Paid") {
                var formData1 = document.forms["frmLoanEntry"];
                formData1.elements.sltddLoan.disabled = true;
                formData1.elements.txtLoanDate.disabled = true;
                formData1.elements.txtMonthYear.disabled = true;
                formData1.elements.txtLoanAmt.disabled = true;
                formData1.elements.txtnoofinstall.disabled = true;
                formData1.elements.txtpermonth.disabled = true;
                formData1.elements.btnSave.disabled = true;

            }
           
        }
    },
    renderLoanTran: function (data) {
        debugger;
        // Total Loan Amount Calculation
        var AmounttoPay = 0;
        for (i = 0; i < data.loanTrans.length; i++)
        {
            var total = data.loanTrans[i].amtPaid;
            var amounttotal = parseFloat(AmounttoPay) + parseFloat(total);
            AmounttoPay = amounttotal;
        }
        var Loanamounttopay = AmounttoPay;


        //Math.round(parseFloat(AmounttoPay) + parseFloat(total) + parseFloat(total))
        var gridObject = $LoanEntry.loanTranGridObject();
        var tableid = { id: 'tblLoanTran' };
        //var data1 = data;
        var popup = $screen.createPopUp({ id: 'frmloanTran', title: 'Loan Transactions' });//, button: { id: 'loanSave' }
        var modelContent = '';
        $LoanEntry.LoanEntryId = data.loanEntryid;
        modelContent = modelContent + '<div id="addLoan" class="col-sm-12"> ';
        modelContent = modelContent + '<div class="col-sm-5"> ';
        //modelContent = modelContent + '<label class="control-label col-md-6">';
        modelContent = modelContent + '<input type="text" id="txtInstallmentdate" class="form-control datepicker" placeholder="Enter the  Date" onFocus="$LoanEntry.datepick()"  onchange="$LoanEntry.datepickValue()" readonly/></div>';
        //modelContent = modelContent + ' </label>';
        modelContent = modelContent + '<div class="col-sm-5"> ';
        // modelContent = modelContent + '<label class="control-label col-md-6">';
        modelContent = modelContent + '<input type="text" id="txtamount" class="form-control" onkeypress="return $validator.checkDecimal(event, 2)" placeholder="Enter the amount" maxlength="5" /></div>';
        //modelContent = modelContent + '</label></div></div>';
        modelContent = modelContent + '<div class="col-sm-2"><a href="#" class="editeButton"  title="Add"><span aria-hidden="true" class="glyphicon glyphicon-plus" onClick="$LoanEntry.checkInstallment()"></span></button></div></div>';

        modelContent = modelContent + '<div class="col-sm-12">  <label class="control-label nodisp" id="lbllastmonth">' + data.loanTrans[data.loanTrans.length - 1].appliedOn + '</label>';
        modelContent = modelContent + '<div class="col-sm-10"> <div class="form-group">';
        modelContent = modelContent + '<label class="control-label col-md-6">';
        modelContent = modelContent + 'Paid Amount: <label class="control-label" id="lblPaid">' + (data.amtPaid).toString() + '</label>';
        modelContent = modelContent + '</label>';
        modelContent = modelContent + '<label class="control-label col-md-6">';
        
        modelContent = modelContent + 'Amount to Pay: <label class="control-label" id="lblAmttoPay">' + (Loanamounttopay - parseFloat(data.amtPaid)).toString() + '</label>';
        modelContent = modelContent + '</label></div></div>';
        
        modelContent = modelContent + '<div class="col-sm-2"><div class="form-group"><input type="button" id="saveLoanTrans" value="Save" onClick="$LoanEntry.saveLoanTrans()"  class="btn custom-button pull-right"/></div></div></div>';
       

        modelContent = modelContent + $screen.createTable(tableid, gridObject);


        // modelContent = modelContent + '';
        //modelContent = modelContent + $screen.createTable(tableid, gridObject);
        document.getElementById("renderId").innerHTML = popup.replace('{ModelBody}', modelContent);
        $LoanEntry.getLoanTrans(data, gridObject, tableid);

        $('#AddForeColse').modal('toggle');
        //<div class="form-group">
        //                <label class="control-label col-md-4">
        //                    Last Working Date <label style="color:red;font-size: 13px">*</label>
        //                </label>
        //                <div class="col-md-6">
        //                    <input type="text" id="txtLastWorkingDate" onchange="DateValidate()" class="form-control datepicker" placeholder="Enter the Last Working Date"
        //                           readonly required />
        //                </div>
        //            </div>
    },
    getLoanTrans: function (data, gridObject, tableid) {

        $.ajax({
            url: $app.baseUrl + "Loan/GetLoanTrans",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                var out = jsonResult.result;
                switch (jsonResult.Status) {
                    case true:
                        $LoanEntry.loadLoanTrans(out, gridObject, tableid);

                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    loanTranGridObject: function () {

        var gridObject = [
                { tableHeader: "id", tableValue: "id", cssClass: 'nodisp id' },
                { tableHeader: "LoanEntryId", tableValue: "loanEntryId", cssClass: 'loanentryId nodisp' },
                { tableHeader: "Applied On", tableValue: "appliedOn", cssClass: 'appliedOn' },
                { tableHeader: "Amount", tableValue: "amtPaid", cssClass: 'amtPaid textbox' },
                { tableHeader: "InterestPercentage", tableValue: "interestAmt", cssClass: '' },
                { tableHeader: "Status", tableValue: "status", cssClass: 'status' },
                { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }
        ];
        return gridObject;
    },
    loadLoanTrans: function (data, context, tableId) {
        
        console.log(data);
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass.includes('nodisp')) {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('actionColumn')) {
                
                //var tableid = { id: 'tblLoanTran' };

                columnDef.push(
                        {
                            "aTargets": [cnt],
                            "sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                               
                                
                                if (oData.status == "UnPaid"&& oData.interestAmt==0) {
                                    
                                    //   var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                    //var c = $('<a href="#" class="editeButton" title="Add"><span aria-hidden="true" class="glyphicon glyphicon-plus"></span></button>');
                                    var d = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                                    //c.button();
                                    //c.on('click', function () {
                                    //    if (confirm('Are you sure ,do you want do Add installment?')) {
                                    //        //var tr = $(this).closest('tr');
                                    //        //$(tr).find('.txtAmount').prop('disabled', true);
                                    //        
                                    //        //var gridObject = $LoanEntry.loanTranGridObject();
                                    //        //var tableid = { id: $LoanEntry.tableId };
                                    //        $LoanEntry.checkInstallment();
                                    //        //$LoanEntry.loadLoanTrans(data, gridObject, "tblLoanTran");
                                    //        return false;
                                    //    }
                                    //});
                                    d.button();
                                    d.on('click', function () {

                                        if (confirm('Are you sure ,do you want to delete?')) {
                                            
                                            var tr = $(this).closest('tr');
                                            $(tr).find('.txtAmount').prop('disabled', true);
                                            $(tr).addClass('nodisp');
                                            //$(tr).remove();
                                            //$LoanEntry.checkInstallment();
                                            //if (oData.LoanEntryId != 0 && oData.id!=0) {
                                            //    $LoanEntry.DeleteLoanTransData({ oData });
                                            //}
                                            //else {
                                            //    $(this).parent().parent().remove();
                                            //}
                                        }
                                        else {

                                        }
                                        return false;
                                    });

                                    $(nTd).empty();
                                    $(nTd).prepend(d);

                                    $('#addLoan').removeClass('nodisp');
                                    $('#saveLoanTrans').prop('disabled', false);
                                    //$(nTd).prepend(d);
                                } else {
                                    $(nTd).empty();
                                    $('#addLoan').addClass('nodisp');
                                    $('#saveLoanTrans').prop('disabled', true);
                                    // $(nTd).prepend(c);
                                }
                            }
                        }

                    );//for action column

            } else if (context[cnt].cssClass.includes('textbox')) {
                
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var disabled = "";
                        if (oData.status == "Paid") {
                            disabled = "disabled";
                        }
                        var b = $('<input type="text" class="txtAmount"  ' + disabled + '  onkeypress="return $validator.checkDecimal(event, 2)" value="' + sData + '"  id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass + " word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "aaData": data,
            "aaSorting": [[ 2 ,"asc" ]],
            //"bSort": false,
            fnInitComplete: function (oSettings, json) {
                var r = $('#' + tableId.id + ' tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#' + tableId.id + ' thead').append(r);
                $('#search_0').css('text-align', 'center');


                //SSorting disable
                var oTable = $('#tblLoanTran').dataTable();
                // get settings object after table is initialized
                var oSettings = oTable.fnSettings();
                // disable sorting on column "1"
                oSettings.aoColumns[2].bSortable = false;
                oSettings.aoColumns[4].bSortable = false;
                oSettings.aoColumns[5].bSortable = false;

            },
            dom: "rtiS",
            scroller: {
                loadingIndicator: true
            }
        });
    },
    checkInstallment: function () {
        
        var transRows = $("#tblLoanTran").dataTable().fnGetNodes();
        var pendingAmt = 0;
        var loanentryId = 0;
        for (i = 0; i < transRows.length; i++) {
            
            if ($(transRows[i]).find('.status').html() != "Paid") {
                
                if (!$(transRows[i]).find('.txtAmount').prop('disabled')) {
                    pendingAmt = pendingAmt + parseFloat($(transRows[i]).find('.txtAmount').val());
                }
            }
        }
        var bal = pendingAmt;
        pendingAmt = Math.round(bal);
        if (pendingAmt <= parseFloat($('#lblAmttoPay').text())) {

            //pendingAmt = parseFloat($('#lblAmttoPay').text()) - pendingAmt;
            //lastmonth = new Date($('#lbllastmonth').text());//passed as yyyy,m,date
            //var lDate = new Date(lastmonth.setMonth(lastmonth.getMonth() + 1));
            //var c = $('<a href="#" class="editeButton" title="Add"><span aria-hidden="true" class="glyphicon glyphicon-plus"></span></button>');
            //var d = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
            var AppliedDate = document.getElementById('txtInstallmentdate').value;
            var AmountPaid = document.getElementById('txtamount').value;
            $("#tblLoanTran").DataTable().row.add({ id: 0, loanEntryId: loanentryId, appliedOn: AppliedDate, amtPaid: AmountPaid, interestAmt:0, status: "UnPaid" }).draw();
            $("#txtInstallmentdate").val('');
            $("#txtamount").val('');

            //$("#tblLoanTran").DataTable().row.add({ id: 0, loanEntryId: loanentryId, appliedOn: $.datepicker.formatDate('dd/M/yy',lDate), amtPaid: 0, interestAmt: 0, status: "Unpaid" }).draw();
            //$(nTd).empty();
            //$(nTd).prepend(c, d);
            //$('#lbllastmonth').text($.datepicker.formatDate('dd/M/yy', lDate));

        }

    },

    saveLoanTrans: function () {
        
        var transRows = $("#tblLoanTran").dataTable().fnGetNodes();
        var pendingAmt = 0;
        var loanTrans = [];
        var chkbal = 0;
        for (i = 0; i < transRows.length; i++) {
            
            if ($(transRows[i]).find('.txtAmount').prop('disabled') != true && $(transRows[i]).find('.status').html() != "Paid") {
                var amtpaid = $(transRows[i]).find('.txtAmount').val();
                chkbal = chkbal + parseFloat(amtpaid);
            }
        }
        var bal = chkbal;
        chkbal = Math.round(bal);
        if (chkbal > parseFloat($('#lblAmttoPay').text())) {
            $app.showAlert(" Payment amount" + " " + chkbal + "," + "Not tallying  Amount to Pay", 4);
            return false;
        }
        else if (chkbal < parseFloat($('#lblAmttoPay').text())) {
            $app.showAlert(" Payment amount" + " " + chkbal + "," + "Not tallying Amount to Pay", 4);
            return false;
        }
        for (i = 0; i < transRows.length; i++) {

            if ($(transRows[i]).find('.status').html() != "Paid") {
                var trans = new Object();

                trans.id = $(transRows[i]).find('.id').html();
                trans.loanentryId = $LoanEntry.LoanEntryId;
                trans.appliedOn = $(transRows[i]).find('.appliedOn').html();
                trans.amtPaid = $(transRows[i]).find('.txtAmount').val();
                trans.deleted = $(transRows[i]).find('.txtAmount').prop('disabled');
                loanTrans.push(trans);

            }

        }

        $.ajax({
            url: $app.baseUrl + "Loan/SaveLoanTrans",
            data: JSON.stringify({ dataValue: loanTrans }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddLoanEntry').modal('toggle');
                        //$LoanEntry.createLoanGrid();//LoadLoanEntry();
                        $app.hideProgressModel();
                        $LoanEntry.createLoanGrid();
                        $app.showAlert(jsonResult.Message, 2);

                        //alert(jsonResult.Message);
                        //var p = jsonResult.result;
                        //companyid = 0;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $LoanEntry.createLoanGrid();
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {

            }
        })

    }


};