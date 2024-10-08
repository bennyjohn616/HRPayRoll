﻿$('#txtHRAshareofinterest,#txtHRAInterestPayable,#txtHRAPreConstraction,#txtHRAReIncomePA,#txtHRAMuWaSetax,#txtHRALoanAmount,#sltHRAIstheProperty,#sltHRAPurposeofLoan,#sltHRAconstruction,#sltHRASelfoccupied,#sltHRALoanTakenBefore,#newtaxscheme').change(function () {

    $declaractionEntry.Totalinterest();
});

$('#Income1,#Income2,#Income3').change(function () {
    var income1 = $('#Income1').val();
    var income2 = $('#Income2').val();
    var income3 = $('#Income3').val();
    $('#total').val(parseFloat(income1) + parseFloat(income2) + parseFloat(income3));

});


$('input.invent').change(function () {

    $declaractionEntry.calculateRent();
});

$('input.invent').blur(function () {

    $declaractionEntry.calculateRent();
});


$('input.radioButton').change(function () {

    var Pancheck = $('input[name=pan]:checked').val();
    if (Pancheck == "true") {
        $declaractionEntry.Panshow();
    }
    else {
        $declaractionEntry.Panhide();
    }

    if (Pancheck == "false") {
        $declaractionEntry.aadharshow();
    }
    else {
        $declaractionEntry.aadharhide();
    }

    $declaractionEntry.radioButton();
});


$('input.radioButton').blur(function () {

    $declaractionEntry.radioButton();
});


$("#sltHRAIstheProperty").change(function () {

    if ($("#sltHRAIstheProperty").val() == 2) {
        $("#Lblinterest").hide();
        $('#txtHRAshareofinterest').removeClass("Reqrd");
        $('#txtHRAshareofinterest').val('0');
    }
    else {
        $("#Lblinterest").show();
        $('#txtHRAshareofinterest').addClass("Reqrd");
        $('#txtHRAshareofinterest').val('0');
    }
});
$("#sltHRASelfoccupied").change(function () {

    if ($("#sltHRASelfoccupied").val() == 2) {
        $("#dvSelfOccufied").hide();
        $("#dvSelfOccufieddrop").hide();
        $("#letoutproperty").show();
        $("#lblgrossrentalincome").show();
        $("#lblmunicipaltax").show();
        $('#sltHRALoanTakenBefore').removeClass("Reqrd");
        $('#sltHRALoanTakenBefore').val(null);
        $('#txtHRAReIncomePA').addClass("Reqrd");
        $('#txtHRAMuWaSetax').addClass("Reqrd");
        $('#txtHRAReIncomePA').val('0');
        $('#txtHRAMuWaSetax').val('0');
    }
    else {
        $("#dvSelfOccufied").show();
        $("#dvSelfOccufieddrop").show();
        $("#letoutproperty").hide();
        $("#lblgrossrentalincome").hide();
        $("#lblmunicipaltax").hide();
        $('#sltHRALoanTakenBefore').addClass("Reqrd");
        $('#sltHRALoanTakenBefore').val('0');
        $('#txtHRAReIncomePA').removeClass("Reqrd");
        $('#txtHRAMuWaSetax').removeClass("Reqrd");
        $('#txtHRAReIncomePA').val(null);
        $('#txtHRAMuWaSetax').val(null);
    }
});

$("#sltHRAPurposeofLoan").change(function () {

    if ($("#sltHRAPurposeofLoan").val() == 2) {
        $("#sltHRAconstruction").show();
        $('#sltHRAconstruction').addClass("Reqrd");
        $('#sltHRAconstruction').val('0');
        $("#txtHRAPreConstraction").show();
        $('#txtHRAPreConstraction').addClass("Reqrd");
        $('#txtHRAPreConstraction').val('0');
        $("#lblpreconstruction").show();
        $("#lblIncaseofconstruction").show();
    }
    else {
        $("#sltHRAconstruction").hide();
        $('#sltHRAconstruction').removeClass("Reqrd");
        $('#sltHRAconstruction').val(null);
        $("#txtHRAPreConstraction").hide();
        $('#txtHRAPreConstraction').removeClass("Reqrd");
        $('#txtHRAPreConstraction').val(null);
        $("#lblpreconstruction").hide();
        $("#lblIncaseofconstruction").hide();
    }
});



$declaractionEntry = {
    
    selectedloadCategory: 0,
    id: 0,
    entryType: "Single",
    formData: document.forms["frmDeclaration"],
    tableid: 'tblDeclaration',
    financeYear: $companyCom.getDefaultFinanceYear(),
    GridData: '',
    TxSectionId: '',
    MedicalTxSectionId: '',
    LicTxSectionId: '',
    TxRentSectionId: '',
    PropertyCount: '',
    ExistingPropertyCount: '',
    TxRentAmount: '',
    TxEmployeeId: '',
    LICData: null,
    INPdate: '',
    cutoffdate: '',
    payrollLockRelease: '',
    empobj: null,
    typeofpage: '',
    payhistory: '',
    financeYearDuration: function () {

        var year = " " + ":" + new Date($declaractionEntry.financeYear.startDate).getFullYear() + "-" + new Date($declaractionEntry.financeYear.EndDate).getFullYear().toString().substr(-2);
        return year;
    },

    GetDefaultfinyear: function () {
        debugger;
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            date2 = new Date($declaractionEntry.cutoffdate);
           $declaractionEntry.financeYear =  $companyCom.EmployeeDefaultFinanceYear(date2);
        }
        else {
           $declaractionEntry.financeYear =  $companyCom.getDefaultFinanceYear();
        }
    },

    GetPayHistory: function () {
        $app.showProgressModel();
        var FinanceYearId = $declaractionEntry.financeYear.id;
        var startdate = $declaractionEntry.financeYear.startDate;
        var enddate = $declaractionEntry.financeYear.EndDate;
        var EmployeeId = $('#ddlEmployee').val();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/GetPayhistory",
            data: JSON.stringify({ FinanceYearId: FinanceYearId, EmployeeId: EmployeeId, StartDate: startdate, EndDate: enddate }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $declaractionEntry.GetHistory(jsonResult.result);
                        $app.hideProgressModel();
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            }
        })
    },


    GetHistory: function (result) {
        $declaractionEntry.payhistory = '';
        if (result.length > 0) {
            $declaractionEntry.payhistory = result;
        }
    },


    Panhide: function () {
        $("#pan1").hide();
        $("#pan2").hide();
        $("#pan3").hide();
        $("#lblpan1").hide();
        $("#lblpan2").hide();
        $("#lblpan3").hide();
    },
    Panshow: function () {
        $("#pan1").show();
        $("#pan2").show();
        $("#pan3").show();
        $("#lblpan1").show();
        $("#lblpan2").show();
        $("#lblpan3").show();
    },

    aadharhide: function () {
        $("#aadhar1").hide();
        $("#aadhar2").hide();
        $("#aadhar3").hide();
        $("#lblaadhar1").hide();
        $("#lblaadhar2").hide();
        $("#lblaadhar3").hide();
    },
    aadharshow: function () {
        $('#noPan').removeClass('nodisp');
        $("#aadhar1").show();
        $("#aadhar2").show();
        $("#aadhar3").show();
        $("#lblaadhar1").show();
        $("#lblaadhar2").show();
        $("#lblaadhar3").show();
    },

    //Created by AjithPanner on 2/1/18
    radioButton: function () {

        var pan;
        var sum;
        $('input.radioButton').each(function () {

            pan = "p" + $('input[name=pan]:checked').val();
            if (pan == "ptrue") {
                $('#noPan').hide();
                $('input:radio[name=declaration]').each(function () {
                    $(this).removeAttr('checked');
                })
                $('#pan1').addClass('Reqrd');
                $('#pan2').addClass('Reqrd');
                $('#pan3').addClass('Reqrd');
            }
            else {
                $('#rd2').attr('checked', 'checked');
                /*$radi.filter('[value=false]').removeAttr('checked');*/
                $('#noPan').removeClass('nodisp');
                $('#noPan').show();
                $('#pan1').removeClass('Reqrd');
                $('#pan2').removeClass('Reqrd');
                $('#pan3').removeClass('Reqrd');
                $('#aadhar1').addClass('Reqrd');
                $('#aadhar2').addClass('Reqrd');
                $('#aadhar3').addClass('Reqrd');
            }
         /*   sum = "a" + $('input[name=pan]:checked').val() + $('input[name=declaration]:checked').val();
            if (sum == "afalsefalse") {
                $("#eligibleRent").val(0);
            }
            else {*/
                $declaractionEntry.calculateRent();
        });
    },

    
    
    //Created by AjithPanner on 29/12/17
    calculateRent: function () {
        debugger;
        var sum = 0;
        $('input.invent').each(function () {
            if (!isNaN(this.value) && this.value.length != 0) {
                sum += parseFloat(this.value);
            }
            else {
                $(this).val(0);
            }
        });

        $("#totalRent").val(sum.toFixed(2));
        $("#eligibleRent").val(sum.toFixed(2));
        if ($("#totalRent").val() >= 100000) {
            $("#moreThanOneLakh").show();
        }
        else {
            $("#moreThanOneLakh").hide();
        }

    },

    chkfunction : function () {
        debugger;
        if ($('#checky').is(':checked')) {
            $('#ntscheme').val(1);
        }
        else
        {
            $('#ntscheme').val(0);
        }

    },

    //Created by AjithPanner on 2/1/18
    retiveActualrent: function () {

        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;
        debugger
        if ($declaractionEntry.payrollLockRelease == 1) {
            $("#btnSave").hide();
        }
        else {
            if (InpDate >= SYSDat) {
                $("#btnSave").show();
            }
            else {
                $("#btnSave").hide();
            }
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $("#btnSave").hide();
            }
        }



        var FinanceYearId = $declaractionEntry.financeYear.id;
        var EmployeeId = $('#ddlEmployee').val();
        var effectiveDate = $('#ddMonth').val() + "/" + $('#txtYear').val();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/GetActualRentpaid",
            data: JSON.stringify({ FinanceYearId: FinanceYearId, EmployeeId: EmployeeId, EffectiveDate: effectiveDate, TxSecid: $declaractionEntry.TxRentSectionId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $declaractionEntry.renderActualRent(jsonResult.result);
                        $declaractionEntry.calculateRent();
                        $declaractionEntry.hidePan();
                        $declaractionEntry.radioButton();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },

        });

    },
    //Created by AjithPanner on 3/1/18
    renderActualRent: function (result) {
        if (result.length > 0) {
            $(result).each(function (index) {
                var monthMetro = "#" + result[index].Month + ".metro";
                var monthNonmetro = "#" + result[index].Month + ".nonmetro";
                $(monthMetro).val(result[index].MetroRent);
                $(monthNonmetro).val(result[index].NonMetroRent);

            });
        }
        else {

        }

    },
    alreadyExistPan: function (obj) {

        var allpan = [];
        var sum = 0;
        $('input:text[name=pannum]').each(function (index) {
            allpan[index] = $(this).val();
            if (allpan[index] == obj.value) {
                sum++;
            }
        });
        if (sum > 1) {
            $app.showAlert('Pan Number already exist ', 4);
            $('#' + obj.id).val('');
        }
    },

    alreadyExistaadhar: function (obj) {

        var allaadhar = [];
        var sum = 0;
        $('input:text[name=aadharnum]').each(function (index) {
            allaadhar[index] = $(this).val();
            if (allaadhar[index] == obj.value) {
                sum++;
            }
        });
        if (sum > 1) {
            $app.showAlert('Aadhar Number already exist ', 4);
            $('#' + obj.id).val('');
        }
    },


    fnValidateAADHAR: function (e) {
        if (e.value != "") {
            ObjVal = e.value;
            var aadharPat = /^([0-9]){12}?$/;
            var code = ObjVal;
            if (aadharPat.test(code) == false) {
                $app.showAlert("Invaild AADHAR No.", 4);
                ObjVal.focus();
                return false;
            }

            if (ObjVal.length < 12) {
                $app.showAlert("AADHAR No. must be 12 digits", 4);
                e.focus();
                return false;

            }

        }
    },



    //Created by AjithPanner on 2/1/18
    hidePan: function () {
        if ($("#eligibleRent").val() >= 100000) {
            $("#moreThanOneLakh").show();
        }
        else {
            $('#moreThanOneLakh').hide();
        }
    },
    submitRent: function () {

        if ($('#totalRent').val() >= 100000) {

            var Pancheck = $('input[name=pan]:checked').val();
            if (Pancheck == undefined) {
                $app.showAlert('Please Check PAN Or AADHAR', 4);
                return false;
            }
            if (Pancheck == undefined) {
                $app.showAlert('Please Check PAN', 4);
                return false;
            }

         /*   if ($('input[name=pan]:checked').val() == "false") {
                $('#rd3').attr('checked', 'checked');
                $radi.filter('[value=false]').removeAttr('checked');
            }*/
        }


        if ($('input[name=pan]:checked').val() == "false") {
            $("#pan1").val("");
            $("#pan2").val("");
            $("#pan3").val("");
        }


        if ($('input[name=pan]:checked').val() != undefined && $('input[name=pan]:checked').val() == "true") {
            var err = 0;
            $(".Reqrd").each(function () {
                if (this.id == "pan1") {

                    if (document.getElementById(this.id).value == "") {

                        $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                        err = 1;
                        return false;
                    }
                }
            });
            if (err == 1) {
                $app.hideProgressModel();
                return false;
            }
        }

    /*    if ($('input[name=declaration]:checked').val() == "false") {
            $("#aadhar1").val("");
            $("#aadhar2").val("");
            $("#aadhar3").val("");
        }*/

        if (($('#totalRent').val() >= 100000) && ($('input[name=pan]:checked').val() == "false")) {
            var err = 0;
            $(".Reqrd").each(function () {
                if (this.id == "aadhar1") {

                    if (document.getElementById(this.id).value == "") {

                        $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                        err = 1;
                        return false;
                    }
                }
            });
            if (err == 1) {
                $app.hideProgressModel();
                return false;
            }
        }


/*        if ($('input[name=pan]:checked').val() == "false" && $('input[name=declaration]:checked').val() == undefined) {
            $app.showAlert('Please select Whether Declaration received From Land Lord', 4);
            return false;
        }*/

        $declaractionEntry.submitEligibleRent();

    },
    //Created by AjithPanner on 2/1/18
    submitEligibleRent: function () {

        var data = [];
        var val = new Object();
        val.EmployeeId = $('#ddlEmployee').val();
        val.FinancialYear = $declaractionEntry.financeYear;
        val.SectionId = $declaractionEntry.TxRentSectionId;
        val.effectiveDate = $('#ddMonth').val() + "/" + $('#txtYear').val();
        val.value = $('#eligibleRent').val();
        val.hasPan = $('input[name=pan]:checked').val();
        val.panNumber = $('#pan1').val() + ',' + $('#pan2').val() + ',' + $('#pan3').val()
        if (val.panNumber == ',,' || $('input[name=pan]:checked').val() == undefined) {
            val.panNumber = null;
        } else {
            val.panNumber = val.panNumber.replace(/^,|,$/g, '');
            val.panNumber = val.panNumber.replace(/^,|,$/g, '');
        }

        var c1 = $('input[name=pan]:checked').val();
        if ( c1 == "false") {
            val.panNumber = $('#aadhar1').val() + ',' + $('#aadhar2').val() + ',' + $('#aadhar3').val()
        }


        val.hasDeclaration = $('input[name=declaration]:checked').val();
        val.landLordName = $('#landLordName').val().trim();
        val.landLordAddress = $('#landLordAddress').val().trim();
        val.EffectiveMonth = $('#ddMonth').val();
        val.EffectiveYear = $('#txtYear').val();
        val.Totalrent = $('#totalRent').val();
        $('#newrentamt').text(val.Totalrent);
        if ($('#totalRent').val() == 0 || $('#totalRent').val() < 100000) {
            val.panNumber = null;
            val.hasPan = null;
            val.hasDeclaration = null;
        }
        data.push(val);
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/SaveTaxDeclaration",
            data: JSON.stringify({ datavalue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $declaractionEntry.submitRentMonthly(jsonResult.result[0].sectionId, jsonResult.result[0].effectiveDate);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },

        });

    },
    //Created by AjithPanner on 2/1/18
    submitRentMonthly: function (txSecId, effectiveDate) {
        var data = [];
        var status = false;
        $('input.invent.metro').each(function (index) {
            var nonMetroInd = index;
            var dataNonMetro = [];
            var value = new Object();
            value.FinanceYearId = $declaractionEntry.financeYear.id;
            value.EmployeeId = $('#ddlEmployee').val();
            value.Month = $(this).attr('id');
            if (value.Month > 0) {
                value.MetroRent = $(this).val();
                $('input.invent.nonmetro').each(function (index) {
                    dataNonMetro[index] = $(this).val();
                });
                value.NonMetroRent = dataNonMetro[nonMetroInd]
                data.push(value);
            }
        });
        if ($('#totalRent').val() >= 100000) {
            var Pancheck = $('input[name=pan]:checked').val();
            var actualcheck = $('input[name=declaration]:checked').val();
            if (Pancheck != undefined) {
                status = true;
            }
        }
        else {
            status = true;
        }

        if (status == true) {
            $.ajax({
                url: $app.baseUrl + "TaxDeclaration/SaveActualRentpaid",
                data: JSON.stringify({ datavalue: data, TxSectionId: txSecId, employeeId: $('#ddlEmployee').val(), EffectiveDate: effectiveDate }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                async: false,
                success: function (jsonResult) {
                    switch (jsonResult.Status) {
                        case true:

                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 2);
                            var oData = new Object();
                            oData.filePath = jsonResult.result.filePath;
                            if ($('#totalRent').val() >= 100000) {
                                var Pancheck = $('input[name=pan]:checked').val();
                                var actualcheck = $('input[name=declaration]:checked').val();
                                if (Pancheck == "false" && actualcheck == "true") {
                                    $app.downloadSync('Download/DownloadPaySlip', oData);
                                    $app.hideProgressModel();
                                }
                            }
                            break;
                        case false:
                            $app.showAlert(jsonResult.Message, 4);
                            break;
                    }
                },

            });
        }
        else {
            $app.showAlert("Please check the PAN details", 4);

        }


    },
    LoadLock: function () {
        debugger
        var month = $('#ddMonth').val();
        var year = $('#txtYear').val();
        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxDeclaration/GetLockLoad",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                month: month,
                year: year
            }),
            dataType: "json",
            success: function (jsonResult) {
                debugger
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var out = jsonResult.result;
                        $declaractionEntry.payrollLockRelease = out.PayrollLock;
                        if (out.PayrollLock == 1) {
                            if (confirm(' You could not update your Entries, Please contact HR')) {
                            }
                            $("#btnSaveDeclaration").hide();
                        }
                        else {
                            $("#btnSaveDeclaration").show();
                        }
                        break;
                    case false:
                       // $("#btnSaveDeclaration").show();
                        $declaractionEntry.payrollLockRelease = "";
                        break;
                }

                var role = $('#hdnRoleName').val();
                if (role.toUpperCase() != "ADMIN") {
                    if (sysdate1 > cuofdate || month != cuofmonth) {
                        var cummyy = ($declaractionEntry.month_name(cuofdate) + "/" + cuofdate.getFullYear())
                       alert("You can update Entries only for " + cummyy + " upto "  +  cuofdate.getDate() + "/" + (cuofdate.getMonth() + 1) + "/" + cuofdate.getFullYear());
                        $("#btnSaveDeclaration").hide();
                    }
                }


            },
            error: function (msg) {
            }
        });

    },

    month_name: function(dt){
        mlist = [ "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" ];
        return mlist[dt.getMonth()];
    },

    loadProof: function () {
        if ($('#chkProof').is(':checked')) {
            $("#dvTax").addClass('nodisp');
        }
        else {
            $("#dvTax").removeClass('nodisp');
        }
    },

    loadMulti: function () {

        if ($('#chkMulti').is(':checked')) {
            $declaractionEntry.entryType = 'Multi';

            $("#dvEmp").addClass('nodisp');
            $("#btnAddSection").removeClass('nodisp');
            $("#btnViewSection").addClass('nodisp');
            $declaractionEntry.loadtaxDeclaration();
        }
        else {
            $declaractionEntry.entryType = 'Single';
            $("#btnViewSection").removeClass('nodisp');
            $("#dvEmp").removeClass('nodisp');
            $("#btnAddSection").addClass('nodisp');
            $companyCom.loadSelectiveEmployee({ condi: 'Category-DeclartionEntry.' + $("#ddlCategory").val() + '.' + $("#ddMonth").val() + '.' + $("#txtYear").val(), id: 'ddlEmployee' });
            $declaractionEntry.loadtaxDeclaration();
        }
    },

    loadEmployee: function () {
        debugger;

        if ($declaractionEntry.entryType == "Multi") {
            $declaractionEntry.applytaxDeclaration();
        } else {
           $declaractionEntry.loadSelectedEmployee();
        }
    },

    loadSelectedEmployee: function () {
        debugger;
        if ($("#ddlCategory").val() != "00000000-0000-0000-0000-000000000000" && $("#ddMonth").val() != "--Select---") {
            $declaractionEntry.loadSelectiveEmployee({ condi: 'Category-DeclartionEntry'+ '.'+ $("#ddlCategory").val() + '.' + $("#ddMonth").val() + '.' + $("#txtYear").val(), id: "ddlEmployee" });
        }
        else
        {
            $app.showAlert("Please select category/month", 3);
        }
      
    },
    loadSelectiveEmployee: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetSelectiveEmployees",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                Condition: dropControl.condi
            }),
            dataType: "json",
            async: false,
            success: function (data) {
               debugger
                var currempidstatus = data.result.employeeID;
                var msg = data.result.Jsondata;
                var empid = msg.empid;
                var out = msg;
               
                if (currempidstatus == "00000000-0000-0000-0000-000000000000") {

                    $('#' + dropControl.id).html('');
                    if ($('#hdnEmployeeId').val() == "00000000-0000-0000-0000-000000000000" && $declaractionEntry.typeofpage == "notAll")
                    {
                        $('#' + dropControl.id).append($("<option></option>").val($declaractionEntry.empobj.empid).html($declaractionEntry.empobj.empCode));
                    }
                    else {
                        $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                        $.each(msg, function (index, blood) {

                            $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                        });
                    }
                
                }
                else {
                    
                    $('#' + dropControl.id).empty();
                    
                    $.each(msg, function (index, blood) {

                        if (($('#hdnEmployeeId').val() == blood.empid)) {
                            debugger
                            $("#hdnEmployeeId").empty();
                            $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                        }

                    });
                }

            },
            error: function (msg) {
            }
        });
    },
    // Created by Babu.R as on 07-Dec-2017 for Income Tax Property Income Module
    HRAPremium: function (event) {

        var formData = document.forms["frmHRAProperty"];
        $declaractionEntry.HRAPremiumClear();
        $declaractionEntry.HRAGetdata();

        $("#btnHRASave").css("display", "");
        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;
        if ($declaractionEntry.payrollLockRelease == 1) {
           // $("#btnHRASave").hide();
        }
        else {
            if (InpDate >= SYSDat) {
            //    $("#btnHRASave").show();
            }
            else {
             //   $("#btnHRASave").hide();
            }
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
              //  $("#btnHRASave").hide();
              //  $("#btnHRADelete").hide();
              //  $("#btnHRAClear").hide();
            }
        }


    },
    //created by suriya
    HRAPropertyGetdata: function (Type) {
        debugger;
        var data = $declaractionEntry.HRAPremiumData();
      //  data.NoOfProperties
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/SaveHouseProperty",
            data: JSON.stringify({ dataValue: data, Type: Type }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $declaractionEntry.UpdateIFHP();
                        $app.showAlert(jsonResult.Message, 2);
                        return true;
                        break;
                    case false:
                        if (jsonResult.Message=="selfproperty") {
                            $app.showAlert("Only 2 properties are allowed to be claimed as Self Occupied Properties", 3);
                        }
                        else {
                            $app.showAlert("Please Config Employee Auto Number in App Setting Menu", 3);
                        }
                      
                        return false;
                        break;
                }
            }
        });
    },
    HRAGetdata: function () {

        var PropertyId = $('#lblPropertyID').text();
        var TxSectionId = $declaractionEntry.TxSectionId;
        var FinancialYear = $declaractionEntry.financeYear.id;
        var EmployeeId = $('#ddlEmployee').val();
        var EffectiveMonth = $('#ddMonth').val();
        var EffectiveYear = $('#txtYear').val();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/GetHouseProperty",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({ EmployeeId: EmployeeId, PropertyId: PropertyId, TxSectionId: TxSectionId, FinancialYear: FinancialYear, EffectiveMonth: EffectiveMonth, EffectiveYear: EffectiveYear }),
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                        var data = jsonResult.result;
                        var FinanYear = data[0].Financial_Year;
                        var FinanYearSplit = FinanYear.split('-');
                        var curFinYear = FinanYearSplit[1];
                        curFinYear = jQuery.trim(curFinYear);
                        var lableValue = 'In case of construction, whether the construction is completed or due for completion within 31-Mar-20' + curFinYear + '? [Completion certificate from builder/Self declaration to be attached]';
                        for (var i = 0; i < data.length; i++) {
                            $('#txtHRAEmployerName').val(data[i].CompanyName),
                            $('#txtHRAEmployee').val(data[i].EmployeeName),
                            $('#txtHRAEmployeeNo').val(data[i].EmployeeCode),
                            $('#txtHRAPAN').val(data[i].PANNumber),
                            $('#txtHRAFinancialYear').val(data[i].Financial_Year),
                            $('#lblTotalinterest').text(data[i].Financial_Year),
                            $('#lblmunicipal').text(data[i].Financial_Year),
                            $('#lblIncaseofconstruction').text(lableValue)
                            $declaractionEntry.HRARenderData(data[i]);
                        }
                        break;
                    case false:
                        $app.showAlert("Please Config Employee Auto Number in App Setting Menu", 3);
                        break;
                }
            }
        });
    },
    HRAPremiumClear: function () {
        $('#txtHRANameofOwner').val(''),
        $('#txtHRAAddressofProper').val(''),
        $('#txtHRALoanDate').val(''),

        $('#txtHRANameandAddress').val(''),
        $('#txtHRAName').val(''),
        $('#txtHRAAddress').val(''),
        $('#txtHRALenderPAN').val(''),
        $('#txtHRALenderType').val('1'),

        $('#txtHRALoanAmount').val(''),
        $('#sltHRAPurposeofLoan').val('1'),
        $('#sltHRAIstheProperty').val('1'),
        $('#txtHRAshareofinterest').val(''),
        $('#txtHRAInterestPayable').val(''),
        $('#txtHRAPreConstraction').val(''),
        $('#txtHRATotalInterest').val(''),
        $('#txtHRAInterest').val(''),
        $('#sltHRAconstruction').val('1'),
        $('#sltHRASelfoccupied').val('1'),
        $('#sltHRALoanTakenBefore').val('1'),
        $('#txtHRAReIncomePA').val(''),
        $('#txtHRAMuWaSetax').val(''),
        $('#txtHRAGroReIncome').val(''),
        $('#txtHRAMunicipalTaxes').val(''),
        $('#txtHRABalance').val(''),
        $('#txtHRAStdrdDedutn').val(''),
        $('#txtHRAIntHosLoan').val(''),
        $('#txtHRANetIncome').val('')
    },
    HRAClose: function () {
        $('#lblPropertyID').text('1'),
        $('#txtHRAPropertyRef').val('1'),
        $('#txtHRANameofOwner').val(''),
        $('#txtHRAAddressofProper').val(''),
        $('#txtHRALoanDate').val(''),

        $('#txtHRANameandAddress').val(''),
        $('#txtHRAName').val(''),
        $('#txtHRAAddress').val(''),
        $('#txtHRALenderPAN').val(''),
        $('#txtHRALenderType').val('1'),

        $('#txtHRALoanAmount').val(''),
        $('#sltHRAPurposeofLoan').val('1'),
        $('#sltHRAIstheProperty').val('1'),
        $('#txtHRAshareofinterest').val(''),
        $('#txtHRAInterestPayable').val(''),
        $('#txtHRAPreConstraction').val(''),
        $('#txtHRATotalInterest').val(''),
        $('#txtHRAInterest').val(''),
        $('#sltHRAconstruction').val('1'),
        $('#sltHRASelfoccupied').val('1'),
        $('#sltHRALoanTakenBefore').val('1'),
        $('#txtHRAReIncomePA').val(''),
        $('#txtHRAMuWaSetax').val(''),
        $('#txtHRAGroReIncome').val(''),
        $('#txtHRAMunicipalTaxes').val(''),
        $('#txtHRABalance').val(''),
        $('#txtHRAStdrdDedutn').val(''),
        $('#txtHRAIntHosLoan').val(''),
        $('#txtHRANetIncome').val('')
    },
    HRAPremiumData: function () {

        var err = 0
        var retObject = {
            EmployeeId: $('#ddlEmployee').val(),
            TxSectionId: $declaractionEntry.TxSectionId,
            NoOfProperties: $('#txtHRANoofProp').val(),
            PropertyId: $('#lblPropertyID').text(),
            FinancialYear: $declaractionEntry.financeYear,
            PropertyReference: $('#txtHRAPropertyRef').val(),
            Property_OwnersName: $('#txtHRANameofOwner').val(),
            PropertyAddress: $('#txtHRAAddressofProper').val(),
            PropertyLoanAmount: $('#txtHRALoanAmount').val(),
            DateofSanctionofLoan: $('#txtHRALoanDate').datepicker('getDate'),
            PurposeOfLoan: $('#sltHRAPurposeofLoan').val(),
            PropertyJointName: $('#sltHRAIstheProperty').val(),
            PropertyJointInterest: $('#txtHRAshareofinterest').val(),
            PayableHousingLoanPerYear: $('#txtHRAInterestPayable').val(),
            PreConstructionInterest: $('#txtHRAPreConstraction').val(),
            TotalInterestOfYear: $('#txtHRATotalInterest').val(),
            Interest_RestrictedtoEmployee: $('#txtHRAInterest').val(),
            ConstrutionIsCompleted: $('#sltHRAconstruction').val(),
            PropertySelfOccupied: $('#sltHRASelfoccupied').val(),
            HousingLoanTakenBefore_01_04_1999: $('#sltHRALoanTakenBefore').val(),
            GrossRentalIncome_PA: $('#txtHRAReIncomePA').val(),
            Municipal_Water_Sewerage_taxpaid: $('#txtHRAMuWaSetax').val(),
            GrossRentalIncome: $('#txtHRAGroReIncome').val(),
            LessMunicipalTaxes: $('#txtHRAMunicipalTaxes').val(),
            Balance: $('#txtHRABalance').val(),
            LessStandardDeduction: $('#txtHRAStdrdDedutn').val(),
            LessInterestOnHousingLoan: $('#txtHRAIntHosLoan').val(),
            HousePropertyNetIncome: $('#txtHRANetIncome').val(),

            LenderAddress: $('#txtHRANameandAddress').val(),
            LenderName: $('#txtHRAName').val(),
            LenderHRAAddress: $('#txtHRAAddress').val(),
            LenderPAN: $('#txtHRALenderPAN').val(),
            LenderType: $('#txtHRALenderType').val(),

            EffectiveMonth: $('#ddMonth').val(),
            EffectiveYear: $('#txtYear').val()
        };
        return retObject;
    },
    HRARenderData: function (data) {
        debugger
        var sanDate = "";
        if (data.DateofSancLoan != "01/Jan/0001") {
            sanDate = data.DateofSancLoan;
        }
        $declaractionEntry.PropertyCount = data.PropertyCount;
        $declaractionEntry.ExistingPropertyCount = data.NoOfProperties;
        var retObject = {
            NoOfProperties: $('#txtHRANoofProp').val(data.NoOfProperties),
            // PropertyId: $('#lblPropertyID').text(data.PropertyId),
            PropertyReference: $('#txtHRAPropertyRef').val(data.PropertyId),
            Property_OwnersName: $('#txtHRANameofOwner').val(data.Property_OwnersName),
            PropertyAddress: $('#txtHRAAddressofProper').val(data.PropertyAddress),
            PropertyLoanAmount: $('#txtHRALoanAmount').val(data.PropertyLoanAmount),
            DateofSanctionofLoan: $('#txtHRALoanDate').val(sanDate),
            PurposeOfLoan: $('#sltHRAPurposeofLoan').val(data.PurposeOfLoan),
            PropertyJointName: $('#sltHRAIstheProperty').val(data.PropertyJointName),
            PropertyJointInterest: $('#txtHRAshareofinterest').val(data.PropertyJointInterest),
            PayableHousingLoanPerYear: $('#txtHRAInterestPayable').val(data.PayableHousingLoanPerYear),
            PreConstructionInterest: $('#txtHRAPreConstraction').val(data.PreConstructionInterest),
            TotalInterestOfYear: $('#txtHRATotalInterest').val(data.TotalInterestOfYear),
            Interest_RestrictedtoEmployee: $('#txtHRAInterest').val(data.Interest_RestrictedtoEmployee),
            ConstrutionIsCompleted: $('#sltHRAconstruction').val(data.ConstrutionIsCompleted),
            PropertySelfOccupied: $('#sltHRASelfoccupied').val(data.PropertySelfOccupied),
            HousingLoanTakenBefore_01_04_1999: $('#sltHRALoanTakenBefore').val(data.HousingLoanTakenBefore_01_04_1999),
            GrossRentalIncome_PA: $('#txtHRAReIncomePA').val(data.GrossRentalIncome_PA),
            Municipal_Water_Sewerage_taxpaid: $('#txtHRAMuWaSetax').val(data.Municipal_Water_Sewerage_taxpaid),
            GrossRentalIncome: $('#txtHRAGroReIncome').val(data.GrossRentalIncome),
            LessMunicipalTaxes: $('#txtHRAMunicipalTaxes').val(data.LessMunicipalTaxes),
            Balance: $('#txtHRABalance').val(data.Balance),
            LessStandardDeduction: $('#txtHRAStdrdDedutn').val(data.LessStandardDeduction),
            LessInterestOnHousingLoan: $('#txtHRAIntHosLoan').val(data.LessInterestOnHousingLoan),
            HousePropertyNetIncome: $('#txtHRANetIncome').val(data.HousePropertyNetIncome),

            LenderAddress: $('#txtHRANameandAddress').val(data.LenderAddress),
            LenderName: $('#txtHRAName').val(data.LenderName),
            LenderHRAAddress: $('#txtHRAAddress').val(data.LenderHRAAddress),
            LenderPAN: $('#txtHRALenderPAN').val(data.LenderPAN),
            LenderType: $('#txtHRALenderType').val(data.LenderType),

        };
        if ($("#sltHRASelfoccupied").val() == 2) {
            $("#dvSelfOccufied").hide();
            $("#dvSelfOccufieddrop").hide();
            $("#letoutproperty").show();
            $("#lblgrossrentalincome").show();
            $("#lblmunicipaltax").show();
            $('#sltHRALoanTakenBefore').removeClass("Reqrd");
            $('#txtHRAReIncomePA').addClass("Reqrd");
            $('#txtHRAMuWaSetax').addClass("Reqrd");
        }
        else {
            $("#dvSelfOccufied").show();
            $("#dvSelfOccufieddrop").show();
            $("#letoutproperty").hide();
            $("#lblgrossrentalincome").hide();
            $("#lblmunicipaltax").hide();
            $('#sltHRALoanTakenBefore').addClass("Reqrd");
            $('#txtHRAReIncomePA').removeClass("Reqrd");
            $('#txtHRAMuWaSetax').removeClass("Reqrd");
        }
        if ($("#sltHRAIstheProperty").val() == 2) {
            $("#Lblinterest").hide();
            $('#txtHRAshareofinterest').removeClass("Reqrd");
        }
        else {
            $("#Lblinterest").show();
            $('#txtHRAshareofinterest').addClass("Reqrd");
        }
        if ($('#txtHRAPropertyRef').val() == 0) {
            $('#txtHRAPropertyRef').val($('#lblPropertyID').text());
        }
        if ($('#lblPropertyID').text() == 1 || $('#txtHRANoofProp').val() == 0) {
            $('#txtHRANoofProp').attr("readonly", false);
        }
        else {
            $('#txtHRANoofProp').attr("readonly", true);
        }

        if ($("#txtHRANoofProp").val() == $('#lblPropertyID').text()) {
            $("#btnHRANext").css("display", "none");
            $("#btnHRASave").css("display", "");
        }
        else if ($("#txtHRANoofProp").val() == 0) {
            $("#btnHRANext").css("display", "none");
            $("#btnHRASave").css("display", "");
        }
        else {
            $("#btnHRANext").css("display", "");
            $("#btnHRASave").css("display", "none");
        }
        if ($('#lblPropertyID').text() == 1) {
            $("#btnHRAprevious").css("display", "none");
        }
        if ($("#sltHRAPurposeofLoan").val() == 2) {
            $("#sltHRAconstruction").show();
            $('#sltHRAconstruction').addClass("Reqrd");
            $("#txtHRAPreConstraction").show();
            $('#txtHRAPreConstraction').addClass("Reqrd");
            $("#lblpreconstruction").show();
            $("#lblIncaseofconstruction").show();
        }
        else {
            $("#sltHRAconstruction").hide();
            $('#sltHRAconstruction').removeClass("Reqrd");
            $("#txtHRAPreConstraction").hide();
            $('#txtHRAPreConstraction').removeClass("Reqrd");
            $("#lblpreconstruction").hide();
            $("#lblIncaseofconstruction").hide();
        }
        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;

        if ($declaractionEntry.payrollLockRelease == 1) {
          //  $("#btnHRASave").hide();
        }
        else {
            if (InpDate >= SYSDat) {
             //   $("#btnHRASave").show();
            }
            else {
             //   $("#btnHRASave").hide();
            }
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
              //  $("#btnHRASave").hide();
            }
        }

        return retObject;
    },
    //Created by suriya
    Totalinterest: function () {

        //var Jointname = $('#sltHRAIstheProperty').val();
        //var Jointpropinterest = $('#txtHRAshareofinterest').val();
        //var YearlyInterest = $('#txtHRAInterestPayable').val();
        //var Preconstrinterest = $('#txtHRAPreConstraction').val();
        var data = $declaractionEntry.HRAPremiumData();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/Totalinterestcalculation",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                        var dataresult = jsonResult.result;
                        $('#txtHRATotalInterest').val(dataresult.TotalInterestOfYear),
                            $('#txtHRAInterest').val(dataresult.Interest_RestrictedtoEmployee),
                            $('#txtHRAGroReIncome').val(dataresult.GrossRentalIncome),
                            $('#txtHRAMunicipalTaxes').val(dataresult.LessMunicipalTaxes),
                            $('#txtHRABalance').val(dataresult.Balance),
                            $('#txtHRAStdrdDedutn').val(dataresult.LessStandardDeduction),
                            $('#txtHRAIntHosLoan').val(dataresult.LessInterestOnHousingLoan),
                            $('#txtHRANetIncome').val(dataresult.HousePropertyNetIncome),
                            $('#newhraamt').text(dataresult.HousePropertyNetIncome)
                            break;
                    case false:
                        $app.showAlert("Check the value", 4);
                        return false;
                        break;
                }
            }
        });
    },
    ///Modified By:arul
    applytaxDeclaration: function () {
        debugger
        var gridObject = $declaractionEntry.getGridObject();
        var rows = $("#tbltaxSections").dataTable().fnGetNodes();
        if ($declaractionEntry.entryType == "Multi") {
            for (var i = 0; i < rows.length; i++) {
                if ($(rows[i]).find(".chkTax").prop("checked")) {
                    gridObject.push({ tableHeader: $(rows[i]).find(".name").html() + '<span id="' + $(rows[i]).find('.chkTax').prop('id').toString() + '" class="nodisp"></span>', tableValue: $(rows[i]).find('.chkTax').prop('id').toString(), cssClass: 'declaration textbox' })
                }
            }
        }
        $declaractionEntry.GridData = gridObject;
        var tableid = { id: $declaractionEntry.tableid };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvEmpoyeeDetail').html(modelContent);
        $declaractionEntry.multiloadDetail(gridObject, tableid);

    },

    DownloadTaxDeclaration: function () {

        $.ajax({
            url: $app.baseUrl + "DataWizard/PrintTaxDeclaration",
            data: JSON.stringify({ financialyearId: $declaractionEntry.financeYear.id, employeeId: $('#ddlEmployee').val(), effectiveDate: $('#txtYear').val() + "-" + $('#ddMonth').val() + "-" + "01 00:00:00.000 ", parentId: '00000000-0000-0000-0000-000000000000', type: 'Declaration', EffectiveYear: $('#txtYear').val(), EffectiveMonth: $('#ddMonth').val() }),
            //data: JSON.stringify({ TaxData:$declaractionEntry.DeclarationList }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var oData = new Object();

                oData.filePath = jsonResult.result;
                $app.downloadSync('Download/DownloadPaySlip', oData);

            },
            complete: function () {
                $app.hideProgressModel();

            }
        });


    },
    DownloadTaxDeclarationpdf: function () {

        $.ajax({
            url: $app.baseUrl + "DataWizard/PrintDeclarationPdf",
            data: JSON.stringify({ financialyearId: $declaractionEntry.financeYear.id, employeeId: $('#ddlEmployee').val(), effectiveDate: $('#txtYear').val() + "-" + $('#ddMonth').val() + "-" + "01 00:00:00.000 ", parentId: '00000000-0000-0000-0000-000000000000', type: 'Declaration', EffectiveYear: $('#txtYear').val(), EffectiveMonth: $('#ddMonth').val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                var oData = new Object();

                oData.filePath = jsonResult.result;
                $app.downloadSync('Download/DownloadPaySlip', oData);

            },
            complete: function () {
                $app.hideProgressModel();

            }
        });


    },
    ///Modified By:Sharmila
    loadtaxDeclaration: function () {
        debugger;
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var EmployeeId = $('#ddlEmployee').val();
        if (INPMonth.toUpperCase() != '--SELECT--' && EmployeeId != null) {
            var gridObject = $declaractionEntry.getGridObject();
            var rows = $("#tbltaxSections").dataTable().fnGetNodes();
            if ($declaractionEntry.entryType == "Multi") {
                for (var i = 0; i < rows.length; i++) {
                    if ($(rows[i]).find(".chkTax").prop("checked")) {
                        gridObject.push({ tableHeader: $(rows[i]).find(".name").html() + '<span id="' + $(rows[i]).find('.chkTax').prop('id').toString() + '" class="nodisp"></span>', tableValue: $(rows[i]).find('.chkTax').prop('id').toString(), cssClass: 'declaration textbox' })
                    }
                }
            }
            var tableid = { id: $declaractionEntry.tableid };
            var modelContent = $screen.createTable(tableid, gridObject);
            $('#dvEmpoyeeDetail').html(modelContent);
            $declaractionEntry.GetPayHistory();
            $declaractionEntry.loadDetail(gridObject, tableid);
            var SYSDate = new Date();
            var SYSMonth = SYSDate.getMonth() + 1;
            var SYSDat = SYSDate.getDate();
            var SYSyear = SYSDate.getFullYear();
            var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
            var INPMonth = $('#ddMonth').val();
            var INPYear = $('#txtYear').val();
            var InpDate = $declaractionEntry.INPdate;
            var cuofdate = new Date($declaractionEntry.cutoffdate);
            var cuofmonth = cuofdate.getMonth() + 1;
            debugger;
            if ($declaractionEntry.payrollLockRelease == 1) {
                $("#btnSaveDeclaration").hide();
            }
            else {
                if (InpDate >= SYSDat) {
                    $("#btnSaveDeclaration").show();
                }
                else {
                    $("#btnSaveDeclaration").hide();
                }
            }

            var role = $('#hdnRoleName').val();
            if (role.toUpperCase() != "ADMIN") {
                if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                    $("#btnSaveDeclaration").hide();
                }
            }



            //var role = $('#hdnRoleName').val();
            //if (role.toUpperCase() == "EMPLOYEE") {
            //    if (SYSDate > cuofdate) {
            //        alert(SYSDate);
            //        alert(cuofdate);
            //        $("#btnSaveDeclaration").hide();
            //    }
            //}


        }
    },


    GetInpDate: function () {
        debugger;
        $.ajax({
            url: $app.baseUrl + "Setting/GetTDSdays",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async:false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $declaractionEntry.INPdate = jsonResult.result.TDSdays;
                        var locale = "en-us";
                        var cudate = new Date(parseInt(jsonResult.result.cutoffdate.replace(/(^.*\()|([+-].*$)/g, '')));
                        //cudate.setMonth(cudate.getMonth() + 1);
                        $declaractionEntry.cutoffdate = (cudate.getMonth() + 1) + "/" + cudate.getDate() + "/" + cudate.getFullYear();
                        //alert($declaractionEntry.cutoffdate);
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

    addTaxColumn: function () {
        $('#AddSection').modal('toggle');
    },

    loadInitial: function () {
        $declaractionEntry.GetInpDate();
        $declaractionEntry.GetDefaultfinyear();
        var gridObject = $declaractionEntry.getSectionGridObject();
        var tableid = { id: 'tbltaxSections' };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvtbltaxSections').html(modelContent);
        $declaractionEntry.multiloadSection(gridObject, tableid);
        $("#btnAddSection").addClass('nodisp');

    },
    getGridObject: function () {

        var gridObject;
        if ($declaractionEntry.entryType == "Multi") {
            gridObject = [
                    { tableHeader: "id", tableValue: "empid", cssClass: 'nodisp employeeId' },
                    { tableHeader: "Employee Code", tableValue: "empCode", cssClass: '' },
                    { tableHeader: "Employee Name", tableValue: 'empFName', cssClass: '' },

            ];
        } else {
            gridObject = [
                    { tableHeader: "id", tableValue: "id", cssClass: 'nodisp id' },
                    { tableHeader: "Parent Section", tableValue: "parentSection", cssClass: '' },
                    { tableHeader: "Section", tableValue: 'name', cssClass: '' },
                    { tableHeader: "Declared Value", tableValue: 'declaredValue', cssClass: 'textbox' },
                    { tableHeader: "orderNo", tableValue: 'disorderNo', cssClass: 'nodisp orderNo' },

            ];
        }
        return gridObject;

    },
    //Modified By:Sharmila

    loadtaxDeclarationSection: function () {
        var gridObject = $declaractionEntry.getGridObject();
        var rows = $("#tbltaxSections").dataTable().fnGetNodes();
        if ($declaractionEntry.entryType == "Multi") {
            for (var i = 0; i < rows.length; i++) {
                if ($(rows[i]).find(".chkTax").prop("checked")) {
                    gridObject.push({ tableHeader: $(rows[i]).find(".name").html() + '<span id="' + $(rows[i]).find('.chkTax').prop('id').toString() + '" class="nodisp taxcol"></span>', tableValue: $(rows[i]).find('.chkTax').prop('id').toString(), cssClass: 'declaration textbox' })
                }
            }
        }
        var tableid = { id: $declaractionEntry.tableid };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvEmpoyeeDetail').html(modelContent);
        $declaractionEntry.LoadSection(gridObject, tableid);

    },
    //testing 
    LoadSection: function (context, tableId) {
        debugger;
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {

            columnsValue.push({ "data": context[cnt].tableValue });

            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn ",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                        var b = $('<input type="checkbox"    id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            } else if (context[cnt].cssClass.includes('textbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "width": "20%",
                    "id": context[cnt].tableValue,
                    "sName": context[cnt].tableValue,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            if (oData.parentId != "00000000-0000-0000-0000-000000000000" || oData.sectionType == 'Declaration') {


                                if (oData.name) {
                                    var SecData = oData.name.split('[');
                                    if (SecData[0] == "LIC premium paid") {

                                        if (oData.declaredValue == 0) {
                                            var b = $("<div class='simple_link'><a href='javascript:void(0);' onclick='LICPremium()' data-toggle='modal' data-target='#AddLICPremium' '>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newlicamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</a></div>");
                                        }
                                        else {
                                            var b = $("<div class='simple_link'><a href='javascript:void(0);' onclick='LICPremium()' data-toggle='modal' data-target='#AddLICPremium' '>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newlicamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</a></div>");
                                        }

                                        $declaractionEntry.LICData = oData;
                                    }
                                    if (oData.name == "Income from house property") {
                                        var SecData = oData.name.split('[');
                                        $declaractionEntry.TxSectionId = oData.id.trim();
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#ShowHRAProperty' onclick='$declaractionEntry.HRAPremium(this)'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newhraamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</a></div>");
                                    }

                                    else {
                                        var b = $('<input type="text"  class="txtValue"  value="' + sData + '" onkeypress="return $validator.CheckNegativedecimal(event, 2)" />');
                                    }

                                    $(nTd).html(b);
                                  
                                }
                               
                              
                            }
                                
                           
                    }
                });
            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({ "aTargets": [cnt], "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                } else {
                    columnDef.push({ "aTargets": "", "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                }
                   }
        }


        var dtClientList = $('#' + tableId.id).dataTable({

            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            "order": [[4, "asc"]],
            //"aaData": data,
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxSection/GetTaxSection",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ financialyearId: $declaractionEntry.financeYear.id, parentId: '00000000-0000-0000-0000-000000000000', type: 'Declaration' }),
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

    loadDetail: function (context, tableId) {
        debugger;
        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;

        if ($declaractionEntry.payrollLockRelease == 1) {
            $("#btnSaveDeclaration").hide();
        }
        else {
            if (InpDate >= SYSDat) {
                $("#btnSaveDeclaration").show();
            }
            else {
                $("#btnSaveDeclaration").hide();
            }
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $("#btnSaveDeclaration").hide();
            }
        }

        var columnsValue = [];
        var columnDef = [];
      
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });

           

            if (context[cnt].cssClass == 'nodisp') {

                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }


            else if (context[cnt].cssClass.includes('checkbox')) {

                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn ",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                        var b = $('<input type="checkbox"    id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            } else if (context[cnt].cssClass.includes('textbox')) {

              

                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "width": "20%",
                    "id": context[cnt].tableValue,
                    "sName": context[cnt].tableValue,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                         
                        if (oData.parentId != "00000000-0000-0000-0000-000000000000" || oData.sectionType == "Declaration") //|| oData.name != null
                        {
                            if (oData.name) {
                                var SecData = oData.name.split('[');
                                if (SecData[0] == "LIC premium paid") {
                                    $declaractionEntry.LicTxSectionId = oData.id.trim();
                                    if (oData.declaredValue == 0) {
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' onclick='LICPremium()' data-toggle='modal' data-target='#AddLICPremium'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newlicamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</a></div>");
                                    }
                                    else {
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' onclick='LICPremium()' data-toggle='modal' data-target='#AddLICPremium'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newlicamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</a></div>");
                                    }
                                    $declaractionEntry.LICData = oData;
                                } else if (oData.name == "Income from house property") {

                                    var SecData = oData.name.split('[');
                                    if (oData.declaredValue == 0) {
                                        $declaractionEntry.TxSectionId = oData.id.trim();
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#ShowHRAProperty' onclick='$declaractionEntry.HRAPremium(this)'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newhraamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</a></div>");
                                    } else {
                                        $declaractionEntry.TxSectionId = oData.id.trim();
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#ShowHRAProperty' onclick='$declaractionEntry.HRAPremium(this)'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newhraamt' class='li1' style=padding:0.1em title='Click for Details'>"  + oData.declaredValue + "</a></div>");
                                    }
                                }
                                else if (oData.name == "Medical insurance premium") {
                                    var SecData = oData.name.split('[');
                                    if (oData.declaredValue == 0) {
                                        $declaractionEntry.MedicalTxSectionId = oData.id.trim();
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#ShowMedInsurance'  onclick='$medicalinsurance.ADDinitialize()'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newmedamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</label>"+ "</a></div>");
                                    } else {
                                        $declaractionEntry.MedicalTxSectionId = oData.id.trim();
                                        $('#newmedamt').val(oData.declaredValue);
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal'  data-target='#ShowMedInsurance'  onclick='$medicalinsurance.ADDinitialize()'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newmedamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</label>" + "</a></div>");
                                    }
                                }
                                else {
                                    var b = $('<input type="text"  class="txtValue"  value="' + sData + '" onkeypress="return $validator.CheckNegativedecimal(event, 2)" />');
                                }
                            } else if (oData.displayAs) {
                                if (oData.displayAs.toUpperCase() == "NEW TAX SCHEME") {
                                    var SecData = oData.displayAs.split('[');
                                    $declaractionEntry.TxSectionId = oData.id.trim();
                                    if ($declaractionEntry.payhistory.length > 0) {
                                        if (oData.declaredValue == 0) {
                                            var b = $('<div class=""> <input type="checkbox" id="checky" disabled="disabled" onclick="$declaractionEntry.chkfunction()" />  <input type="hidden" id="ntscheme" class ="txtValue" value="' + oData.declaredValue + '"/></div>');
                                        }
                                        else {
                                            var b = $('<div class=""> <input type="checkbox" checked id="checky" disabled="disabled" onclick="$declaractionEntry.chkfunction()" /> <input type="hidden" id="ntscheme" class ="txtValue" value="' + oData.declaredValue + '"/></div>');
                                        }
                                    }
                                    else {
                                        if (oData.declaredValue == 0) {
                                            var b = $('<div class=""> <input type="checkbox" id="checky" onclick="$declaractionEntry.chkfunction()" />  <input type="hidden" id="ntscheme" class ="txtValue" value="' + oData.declaredValue + '"/></div>');
                                        }
                                        else {
                                            var b = $('<div class=""> <input type="checkbox" checked id="checky" onclick="$declaractionEntry.chkfunction()" /> <input type="hidden" id="ntscheme" class ="txtValue" value="' + oData.declaredValue + '"/></div>');
                                        }
                                    }
                                }
                                else if (oData.displayAs.toUpperCase() == "ACTUAL RENT PAID") {
                                    var SecData = oData.displayAs.split('[');
                                    $declaractionEntry.TxRentSectionId = oData.id.trim();

                                    if (oData.declaredValue == "" || oData.declaredValue == null) {
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#rentPaid' onclick='$declaractionEntry.retiveActualrent()'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='rentamt' class='li1' style=padding:0.1em title='Click for Details'>"  + 0 + "</a></div>");
                                        $('#eligibleRent').val(0);
                                        $("#pan1").val("");
                                        $("#pan2").val("");
                                        $("#pan3").val("");
                                        $('input.invent').each(function () {
                                            $(this).val(0);
                                        });
                                        $("#landLordName").val("");
                                        $("#landLordAddress").val("");
                                        $declaractionEntry.calculateRent();
                                        $("span#fYear").text($declaractionEntry.financeYearDuration());
                                        var SYSDate = new Date();
                                        var SYSMonth = SYSDate.getMonth() + 1;
                                        var SYSDat = SYSDate.getDate();
                                        var SYSyear = SYSDate.getFullYear();
                                        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
                                        var INPMonth = $('#ddMonth').val();
                                        var INPYear = $('#txtYear').val();
                                        var InpDate = $declaractionEntry.INPdate;
                                        var cuofdate = new Date($declaractionEntry.cutoffdate);
                                        var cuofmonth = cuofdate.getMonth() + 1;
                                        debugger
                                        if ($declaractionEntry.payrollLockRelease == 1) {
                                            $("#btnSave").hide();
                                        }
                                        else {
                                            if (InpDate >= SYSDat) {
                                                $("#btnSave").show();
                                            }
                                            else {
                                                $("#btnSave").hide();
                                            }
                                        }

                                        var role = $('#hdnRoleName').val();
                                        if (role.toUpperCase() != "ADMIN") {
                                            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                                                $("#btnSave").hide();
                                            }
                                        }



                                    }
                                    else {
                                        var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#rentPaid' onclick='$declaractionEntry.retiveActualrent()'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newrentamt' class='li1' style=padding:0.1em title='Click for Details'>"  + oData.declaredValue + "</a></div>");
                                        $('#eligibleRent').val(oData.declaredValue);
                                        if (oData.LandLordName != null) {
                                            $('#landLordName').val(oData.LandLordName);
                                        } else {
                                            $('#landLordName').val("");
                                        }
                                        if (oData.LandLordAddress != null) {
                                            $('#landLordAddress').val(oData.LandLordAddress);
                                        } else {
                                            $('#landLordAddress').val("");
                                        }
                                        if (oData.PanNumber != null) {
                                            var str = oData.PanNumber;
                                            var strarray = str.split(',');
                                            for (var i = 0; i < strarray.length; i++) {
                                                var a = i + 1;
                                                if (oData.HasPan == true) {
                                                    var panId = "#pan" + a
                                                    $(panId).val(strarray[i]);
                                                }
                                                else
                                                {
                                                    var aadharId = "#aadhar" + a
                                                    $(aadharId).val(strarray[i]);
                                                }
                                            }
                                        }
                                        else {
                                            $("#pan1").val("");
                                            $("#pan2").val("");
                                            $("#pan3").val("");

                                        }
                                        var $radios = $('input:radio[name=pan]');
                                        var $radi = $('input:radio[name=declaration]')
                                        if (oData.HasPan == true) {
                                            $('#rd1').attr('checked', 'checked');
                                            $radios.filter('[value=false]').removeAttr('checked');
                                            $('#noPan').hide();

                                        }
                                        else 
                                        if (oData.HasPan == false) {
                                            $('#rd2').attr('checked', 'checked');
                                            $radios.filter('[value=true]').removeAttr('checked');
                                            $declaractionEntry.Panhide();
                                            $('#noPan').show();
                                        }

                                        /*       else {
                                                   $radios.filter('[value=true]').removeAttr('checked');
                                                   $radios.filter('[value=false]').removeAttr('checked');*/

/*                                        if (oData.HasDeclaration == true) {
                                            $('#rd3').attr('checked', 'checked');
                                            $radi.filter('[value=false]').removeAttr('checked');
                                        }
                                        else if (oData.HasDeclaration == false) {
                                            $('#rd4').attr('checked', 'checked');
                                            $radi.filter('[value=true]').removeAttr('checked');
                                        }
                                        else {
                                            $radi.filter('[value=true]').removeAttr('checked');
                                            $radi.filter('[value=false]').removeAttr('checked');
                                        }*/


                                        $("span#fYear").text($declaractionEntry.financeYearDuration());
                                        $declaractionEntry.TxRentAmount = oData.declaredValue;
                                        var SYSDate = new Date();
                                        var SYSMonth = SYSDate.getMonth() + 1;
                                        var SYSDat = SYSDate.getDate();
                                        var SYSyear = SYSDate.getFullYear();
                                        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
                                        var INPMonth = $('#ddMonth').val();
                                        var INPYear = $('#txtYear').val();
                                        var InpDate = $declaractionEntry.INPdate;
                                        var cuofdate = new Date($declaractionEntry.cutoffdate);
                                        var cuofmonth = cuofdate.getMonth() + 1;

                                        if ($declaractionEntry.payrollLockRelease == 1) {
                                            $("#btnSaveDeclaration").hide();
                                        }
                                        else {
                                            if (InpDate >= SYSDat) {
                                                $("#btnSaveDeclaration").show();
                                            }
                                            else {
                                                $("#btnSaveDeclaration").hide();
                                            }
                                        }

                                        var role = $('#hdnRoleName').val();
                                        if (role.toUpperCase() != "ADMIN") {
                                            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                                                $("#btnSaveDeclaration").hide();
                                            }
                                        }


                                    }
                                }
                                else {
                                    var b = $('<input type="text"  class="txtValue" onchange="$declaractionEntry.limitcheck(this.oData.limit)" onkeypress="return $validator.CheckNegativedecimal(event, 2)" value="' + sData + '"/>');
                                }
                            }
                            
                            $(nTd).html(b);
                        }

                        else if (oData.sectionType == "Others") {
                            if (oData.name == "Income from house property") {
                                var SecData = oData.name.split('[');
                                if (oData.declaredValue == 0) {
                                    $declaractionEntry.TxSectionId = oData.id.trim();
                                    var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#ShowHRAProperty' onclick='$declaractionEntry.HRAPremium(this)'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newhraamt' class='li1' style=padding:0.1em title='Click for Details'>" + 0 + "</a></div>");
                                } else {
                                    $declaractionEntry.TxSectionId = oData.id.trim();
                                    var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#ShowHRAProperty' onclick='$declaractionEntry.HRAPremium(this)'>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newhraamt' class='li1' style=padding:0.1em title='Click for Details'>"  + oData.declaredValue + "</a></div>");
                                }
                                $(nTd).html(b);
                            }
                            else {
                                var b = $('<input type="text"  class="txtValue"  value="' + sData + '" onkeypress="return $validator.CheckNegativedecimal(event, 2)"/>');
                                $(nTd).html(b);
                            }
                        }
                        else {
                            var b = '';
                            $(nTd).html(b);
                        }
                    }
                });
            }
            else {
                if (context[cnt].tableValue == 'parentSection') {

                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "word-wrap " + context[cnt].cssClass,
                        "bSearchable": true,
                        "width": "40%",
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            if (oData.parentId == "00000000-0000-0000-0000-000000000000" || oData.sectionType == "Declaration") //|| oData.name != null
                            {
                                if (oData.name) {
                                    var SecData = oData.name.split('[');
                                    if (SecData[0] == "LIC premium paid") {

                                        if (oData.declaredValue == 0) {
                                            var b = $("<div class='simple_link'><a href='javascript:void(0);' onclick='LICPremium()' data-toggle='modal' data-target='#AddLICPremium' '>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newlicamt' class='li1' style=padding:0.1em title='Click for Details'>" + 0 + "</a></div>");
                                        }
                                        else {
                                            var b = $("<div class='simple_link'><a href='javascript:void(0);' onclick='LICPremium()' data-toggle='modal' data-target='#AddLICPremium' '>" + "<span class='fa fa-hand-o-right'></span>" + "<label id='newlicamt' class='li1' style=padding:0.1em title='Click for Details'>" + oData.declaredValue + "</a></div>");
                                        }

                                        $declaractionEntry.LICData = oData;
                                    }
                                    if (oData.name == "Income from house property") {

                                        var SecData = oData.name.split('[');
                                        $declaractionEntry.TxSectionId = oData.id.trim();
                                        //var b = $("<div class='simple_link'><a href='javascript:void(0);' data-toggle='modal' data-target='#ShowHRAProperty' onclick='$declaractionEntry.HRAPremium(this)'>" + SecData[0] + "</a></div>");
                                        var b = oData.displayAs;
                                    }
                                    else {
                                        var b = oData.displayAs;//$('<input type="text"  class="txtValue"  value="' + sData + '"/>');
                                    }
                                }
                                $(nTd).html(b);

                            }
                            else {
                                var b = '';
                                $(nTd).html(b);
                            }

                            //else if (oData.orderNo == 1) {
                            //        var b = oData.parentSection; $(nTd).html(b);
                            //    }
                        }
                    });


                    //columnDef.push({ "aTargets": [cnt], "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                }
                else if (context[cnt].tableValue == 'name') {

                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "word-wrap " + context[cnt].cssClass,
                        "bSearchable": true,
                        "width": "30%",
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            if (oData.parentId != "00000000-0000-0000-0000-000000000000") //|| oData.name != null
                            {
                                var b = oData.name; //$('<input type="text"  class="txtValue"  value="' + sData + '"/>');
                                $(nTd).html(b);
                            } else {
                                var b = '';
                                $(nTd).html(b);
                            }
                        }
                    });


                    //columnDef.push({ "aTargets": [cnt], "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                }


                else {

                    columnDef.push({ "aTargets": [cnt], "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                }
            }
        }


        if ($declaractionEntry.entryType == "Multi") {
            var dtClientList = $('#' + tableId.id).dataTable({

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
                        url: $app.baseUrl + "Employee/GetSelectiveEmployees",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            Condition: "Category." + $('#ddlCategory').val()
                        }),
                        dataType: "json",
                        success: function (jsonResult) {
                            $app.clearSession(jsonResult);
                            switch (jsonResult.Status) {
                                case true:
                                    var out = jsonResult.result.Jsondata;
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
        }

        else {
            var dtClientList = $('#' + tableId.id).dataTable({

                //'iDisplayLength': 1000,
                'bPaginate': false,
                'sPaginationType': 'full',
                'sDom': '<"top">rt<"bottom"ip><"clear">',
                columns: columnsValue,
                "aoColumnDefs": columnDef,
                //"aaData": data,
                ajax: function (data, callback, settings) {
                    $.ajax({
                        type: 'POST',
                        url: $app.baseUrl + "TaxSection/GetTaxSectionId",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ financialyearId: $declaractionEntry.financeYear.id, employeeId: $('#ddlEmployee').val(), effectiveDate: $('#txtYear').val() + "-" + $('#ddMonth').val() + "-" + "01 00:00:00.000 ", parentId: '00000000-0000-0000-0000-000000000000', type: 'Declaration' }),
                        dataType: "json",
                        success: function (jsonResult) {
                            $app.clearSession(jsonResult);

                            switch (jsonResult.Status) {
                                case true:
                                    var out = jsonResult.result;
                                    $declaractionEntry.DeclarationList = out;
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
                    $('#tblDeclaration').DataTable().order([4, 'asc']).draw();
                },
                dom: "rtiS",
                "bDestroy": true,
                scroller: {
                    loadingIndicator: true
                }
            });
        }
      
    },
    getSectionGridObject: function () {

        var gridObject;

        gridObject = [
                { tableHeader: "Sectionid", tableValue: "id", cssClass: 'checkbox id' },
                { tableHeader: "Sections", tableValue: "name", cssClass: 'name' },
               // { tableHeader: "Sub Sections", tableValue: 'name', cssClass: 'name' },

        ];
        return gridObject;

    },
    checkselfproperty:function(){
        debugger;
        var data = $declaractionEntry.HRAPremiumData();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/checkselfproperty",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        


                        break;
                    case false:
                        
                        break;
                }
            }
        });


    },

    //Create by Babu.R as on 22-Nov-2017 for Income tax prtoperty income module
    nextbutton: function () {
        debugger;
        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;

        var NextPage = $('#lblPropertyID').text().trim();
        var textval = $('#txtHRANoofProp').val().trim();
        var err = 0;
        $(".Reqrd").each(function () {
            if (this.id == "sltHRAPurposeofLoan" || this.id == "sltHRAIstheProperty" || this.id == "txtHRALenderType" || this.id == "sltHRAconstruction" || this.id == "sltHRASelfoccupied" || this.id == "sltHRALoanTakenBefore") {
                if (document.getElementById(this.id).value == "0") {
                    $app.showAlert(this.id == "sltHRAPurposeofLoan" ? 'Please Select Purpose of loan' : this.id == "txtHRALenderType" ? 'Please Select Lender Type' : this.id == "sltHRAIstheProperty" ? 'Please Select Is the property held in Joint name by 2 or more persons?' : this.id == "sltHRAconstruction" ? 'Please Select In case of construction, whether the construction is completed' : this.id == "sltHRASelfoccupied" ? 'Please Select Is the Property Self Occupied or Let Out?' : 'Pelease Select Whether housing loan taken before 01/04/1999?', 4);
                    err = 1;
                    return false;
                }
            }
            else if (this.id == "txtHRANameofOwner" || this.id == "txtHRAAddressofProper" || this.id == "txtHRANameandAddress" || this.id == "txtHRAName" || this.id == "txtHRAAddress" || this.id == "txtHRALenderPAN" || this.id == "txtHRALoanAmount" || this.id == "txtHRALoanDate" || this.id == "txtHRAPreConstraction" || this.id == "txtHRAshareofinterest" || this.id == "txtHRAInterestPayable" || this.id == "txtHRAReIncomePA" || this.id == "txtHRAMuWaSetax") {

                if (document.getElementById(this.id).value == "") {

                    $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                    err = 1;
                    return false;
                }
            }
            else if (this.id == "txtHRANoofProp") {
                if (document.getElementById(this.id).value == "0") {

                    $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                    err = 1;
                    return false;
                }
            }
        });
        if (err == 1) {
            $app.hideProgressModel();
            return false;
        }
        var data = $declaractionEntry.HRAPremiumData();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/checkselfproperty",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $declaractionEntry.HRAPropertyGetdata("SaveOnly");
                        $declaractionEntry.HRAPremiumClear();
                        NextPage = Number(NextPage) + Number(1);

                        if (textval > NextPage) {
                            $('#lblPropertyID').text(NextPage);
                            $('#txtHRAPropertyRef').val(NextPage);
                            $declaractionEntry.HRAGetdata();

                            $('#txtHRAPropertyRef').val(NextPage);
                            $("#btnHRAprevious").css("display", "");
                            return;
                        }
                        else if (textval = NextPage) {

                            $('#lblPropertyID').text(NextPage);
                            $declaractionEntry.HRAGetdata();

                            $('#txtHRAPropertyRef').val(NextPage);

                            $("#btnHRANext").css("display", "none");
                            $("#btnHRAprevious").css("display", "");
                            $("#btnHRASave").css("display", "");
                            return;
                        }
                        else {
                            $declaractionEntry.HRAGetdata();
                            $("#btnHRANext").css("display", "none");
                            $("#btnHRASave").css("display", "none");
                        }


                        break;
                    case false:
                        $app.showAlert("Only 2 properties are allowed to be claimed as Self Occupied Properties", 3);
                        break;
                }
            }

        });
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $("#btnHRASave").hide();
            }
        }
     

   
    },

    Previousbutton: function () {

        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;

        var previousPage = $('#lblPropertyID').text();
        $declaractionEntry.HRAPremiumClear();
        previousPage = Number(previousPage) - Number(1);
        $('#lblPropertyID').text(previousPage);
        $('#txtHRAPropertyRef').val(previousPage);
        $declaractionEntry.HRAGetdata();
        if (previousPage > 1) {
            $("#btnHRAprevious").css("display", "");
            $("#btnHRANext").css("display", "");
        }
        else {
            $("#btnHRAprevious").css("display", "none");
            $("#btnHRANext").css("display", "");
            $("#btnHRASave").css("display", "");
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $("#btnHRASave").css("display","none");
            }
        }
    },
    //Modified By:Sharmila

    multiloadSection: function (context, tableId) {
        debugger;
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                        var b = $('<input type="checkbox" class="chkTax"   id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            } else if (context[cnt].cssClass.includes('textbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                        if (oData.parentId != "00000000-0000-0000-0000-000000000000" || oData.name != null) {
                            var b = $('<input type="text" class="txtValue" value="' + sData + '" onkeypress="return $validator.CheckNegativedecimal(event, 2)"/>');
                            $(nTd).html(b);
                        }
                    }
                });
            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({ "aTargets": [cnt], "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                } else {
                    columnDef.push({ "aTargets": "", "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                }
            }
        }

        var dtSections = $('#' + tableId.id).dataTable({
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
                    url: $app.baseUrl + "TaxSection/GetTaxSection",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ financialyearId: $declaractionEntry.financeYear.id, parentId: '00000000-0000-0000-0000-000000000000', type: 'Declaration' }),
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

    multiloadDetail: function (context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {

                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn ",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox"    id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            } else if (context[cnt].cssClass.includes('textbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "width": "20%",
                    "id": context[cnt].tableValue,
                    "sName": context[cnt].tableValue,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                        var b = $('<input type="text"  class="txtValue"  value="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            }
            else {
                if (context[cnt].tableValue == 'parentSection') {

                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "word-wrap " + context[cnt].cssClass,
                        "bSearchable": true,
                        "width": "40%",
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            if (oData.parentId == "00000000-0000-0000-0000-000000000000" || oData.sectionType == "Declaration") //|| oData.name != null
                            {
                                var b = oData.displayAs; //$('<input type="text"  class="txtValue"  value="' + sData + '"/>');
                                $(nTd).html(b);
                            }
                            else {
                                var b = '';
                                $(nTd).html(b);
                            }

                            //else if (oData.orderNo == 1) {
                            //        var b = oData.parentSection; $(nTd).html(b);
                            //    }
                        }
                    });


                    //columnDef.push({ "aTargets": [cnt], "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                }
                else if (context[cnt].tableValue == 'name') {

                    columnDef.push({
                        "aTargets": [cnt],
                        "sClass": "word-wrap " + context[cnt].cssClass,
                        "bSearchable": true,
                        "width": "30%",
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            if (oData.parentId != "00000000-0000-0000-0000-000000000000") //|| oData.name != null
                            {
                                var b = oData.name; //$('<input type="text"  class="txtValue"  value="' + sData + '"/>');
                                $(nTd).html(b);
                            } else {
                                var b = '';
                                $(nTd).html(b);
                            }
                        }
                    });


                    //columnDef.push({ "aTargets": [cnt], "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                }


                else {

                    columnDef.push({ "aTargets": [cnt], "sClass": 'word-wrap ' + context[cnt].cssClass, "bSearchable": true });
                }
            }
        }

        if ($declaractionEntry.entryType == "Multi") {
            var TaxCol = [];;
            for (var cnt = 3; cnt < $declaractionEntry.GridData.length; cnt++) {
                var newObj = new Object();
                newObj.tableHeader = $declaractionEntry.GridData[cnt].tableHeader,
                newObj.tableValue = $declaractionEntry.GridData[cnt].tableValue,
                TaxCol.push(newObj);
            }


            var dtClientList = $('#' + tableId.id).dataTable({

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
                        url: $app.baseUrl + "TaxSection/GetSelectiveData",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            Condition: "Category." + $('#ddlCategory').val(), dataCol: TaxCol, financialyearId: $declaractionEntry.financeYear.id, effectiveDate: $('#txtYear').val() + "-" + $('#ddMonth').val() + "-" + "01 00:00:00.000 "
                        }),
                        dataType: "json",
                        success: function (jsonResult) {

                            $app.clearSession(jsonResult);
                            switch (jsonResult.Status) {

                                case true:
                                    jsonObj = JSON.parse(jsonResult.result);
                                    var out = jsonObj;
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
        }
    },

    save: function () {
        debugger;
        $app.showProgressModel();
        var data = [];
        var rows = $('#tblDeclaration').dataTable().fnGetNodes();
        for (i = 0; i < rows.length; i++) {

            // if ($(rows[i]).find('id').html()) {
            if ($declaractionEntry.entryType == "Multi") {

                $(rows[i]).find('.declaration').each(function () {

                    if ($(this).find('.txtValue')) {
                        var newObj = new Object();
                        newObj.effectiveDate = $('#chkProof').is(':checked') ? "" : '1/' + $('#ddMonth').val() + "/" + $('#txtYear').val(),
                        newObj.Proof = $('#chkProof').is(':checked'),
                        newObj.id = '',
                        newObj.employeeId = $(rows[i]).find('.employeeId').html(),
                        newObj.sectionId = $(this).closest('table').find('th').eq($(this).prop('cellIndex')).find('span').prop('id');
                        newObj.value = $(this).find('.txtValue').val();
                        if (typeof newObj.value != "undefined" && newObj.value > 0) {
                            data.push(newObj);
                        }
                    }
                });

            } else {

                var newObj = new Object();
                newObj.effectiveDate = $('#chkProof').is(':checked') ? "" : '1/' + $('#ddMonth').val() + "/" + $('#txtYear').val(),
                newObj.Proof = $('#chkProof').is(':checked'),
                newObj.id = '',
                newObj.employeeId = $('#ddlEmployee').val();
                newObj.sectionId = $(rows[i]).find('.id').html();
                newObj.value = $(rows[i]).find('.txtValue').val();
                if (typeof newObj.value != "undefined") {
                    data.push(newObj);
                }
            }


             
        }

        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/SaveTaxDeclaration",

            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:


                        //var p = jsonResult.result;
                        //$txSection.selectedSectionId = p.id;

                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        //if ($txSection.selectedSectionparentId == '') {
                        //    $txSection.addInitializeSection();
                        //} else {
                        //    $txSection.addInitializeSubSection();
                        //}

                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        //$txSection.canSave = true;
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
                //$txSection.canSave = true;
            }
        });
    },

    SubmitHRA: function () {


        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;

        if ($('#txtHRALoanAmount').val() == 0) {
            //$('#sltHRALoanTakenBefore').addClass("Reqrd");
            $('#txtHRALenderType').removeClass("Reqrd");
            $('#sltHRAPurposeofLoan').removeClass("Reqrd");
            $('#txtHRALoanDate').removeClass("Reqrd");
            $('#txtHRAInterestPayable').removeClass("Reqrd");
        }
        else {

        }

        if (InpDate >= SYSDat) {
            var err = 0;
            $(".Reqrd").each(function () {
                if (this.id == "sltHRAPurposeofLoan" || this.id == "sltHRAIstheProperty" || this.id == "txtHRALenderType" || this.id == "sltHRAconstruction" || this.id == "sltHRASelfoccupied" || this.id == "sltHRALoanTakenBefore") {
                    if (document.getElementById(this.id).value == "0") {
                        $app.showAlert(this.id == "sltHRAPurposeofLoan" ? 'Please Select Purpose of loan' : this.id == "txtHRALenderType" ? 'Please Select Lender Type' : this.id == "sltHRAIstheProperty" ? 'Please Select Is the property held in Joint name by 2 or more persons?' : this.id == "sltHRAconstruction" ? 'Please Select In case of construction, whether the construction is completed' : this.id == "sltHRASelfoccupied" ? 'Please Select Is the Property Self Occupied or Let Out?' : 'Pelease Select Whether housing loan taken before 01/04/1999?', 4);
                        err = 1;
                        return false;
                    }
                }
                else if (this.id == "txtHRANameofOwner" || this.id == "txtHRAAddressofProper" || this.id == "txtHRANameandAddress" || this.id == "txtHRAName" || this.id == "txtHRAAddress" || this.id == "txtHRALenderPAN" || this.id == "txtHRALoanAmount" || this.id == "txtHRALoanDate" || this.id == "txtHRAPreConstraction" || this.id == "txtHRAshareofinterest" || this.id == "txtHRAInterestPayable" || this.id == "txtHRAReIncomePA" || this.id == "txtHRAMuWaSetax") {

                    if (document.getElementById(this.id).value == "") {

                        $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                        err = 1;
                        return false;
                    }
                }
                else if (this.id == "txtHRANoofProp") {
                    if (document.getElementById(this.id).value == "0") {

                        $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                        err = 1;
                        return false;
                    }
                }
            });
            if (this.id == "txtHRANoofProp") {
                if (document.getElementById(this.id).value == "0") {

                    $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                    err = 1;
                    return false;
                }
            }
            if (err == 1) {
                $app.hideProgressModel();
                return false;
            }

            if ($('#txtHRANoofProp').val() == $('#lblPropertyID').text()) {
                var data = $declaractionEntry.HRAPremiumData();
                $.ajax({
                    url: $app.baseUrl + "TaxDeclaration/checkselfproperty",
                    data: JSON.stringify({ dataValue: data }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {
                        switch (jsonResult.Status) {
                            case true:
                                $declaractionEntry.SubmitIFHP();
                                $declaractionEntry.HRAPropertyGetdata("Saveandsubmit");
                                $app.hideProgressModel();
                                $('#ShowHRAProperty').modal('toggle');
                                break;
                            case false:
                                $app.showAlert("Only 2 properties are allowed to be claimed as Self Occupied Properties", 3);
                                break;
                        }
                    }
                });

          
            }
            else {
                $app.showAlert("Please fill the all given No.Of Properties", 4);
            }
        }
        else {
            $app.showAlert("Declaration entry submitted date is expired", 4);
        }
    },

    DeleteHRA: function () {

        var data = $declaractionEntry.HRAPremiumData();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/DeleteHRA",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                        $declaractionEntry.UpdateIFHP();
                        $declaractionEntry.HRAPremiumClear();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },

    SubmitIFHP: function () {

        var data = [];
        var val = new Object();
        val.EmployeeId = $('#ddlEmployee').val();
        val.FinancialYear = $declaractionEntry.financeYear;
        val.SectionId = $declaractionEntry.TxSectionId;
        val.effectiveDate = $('#txtYear').val() + "-" + $('#ddMonth').val();
        val.EffectiveMonth = $('#ddMonth').val();
        val.EffectiveYear = $('#txtYear').val();
        data.push(val);

        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/SaveTaxDeclaration",
            data: JSON.stringify({ datavalue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },

        });

    },

    UpdateIFHP: function () {


        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/SubmitIFHP",
            data: JSON.stringify({ EmployeeId: $('#ddlEmployee').val(), FinancialYear: $declaractionEntry.financeYear.id, TxSectionId: $declaractionEntry.TxSectionId, EffectiveMonth: $('#ddMonth').val(), EffectiveYear: $('#txtYear').val(), EffectiveDate: $('#txtYear').val() + "-" + $('#ddMonth').val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:


                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },

        });

    },

    GetReport: function () {

        if ($('#ddMonth').find('option:selected').text() == "--Select---") {
            $app.showAlert("please select the effective month", 4);
            return;
        }
        if ($('#fromemployeeCode').find('option:selected').text() == "--Select--" || $('#toemployeeCode').find('option:selected').text() == "--Select--") {
            $app.showAlert("please select the FROM or TO employee code", 4);
            return;
        }
        $app.showProgressModel();
        $.ajax({
            timeout: 900000,
            url: $app.baseUrl + "DataWizard/PrintXlRent",
            data: JSON.stringify({ financialyearId: $declaractionEntry.financeYear.id, effectiveDate: $('#txtYear').val() + "-" + $('#ddMonth').val() + "-" + "01 00:00:00.000 ", EffectiveYear: $('#txtYear').val(), EffectiveMonth: $('#ddMonth').val(), sCode: $('#fromemployeeCode').find('option:selected').text(), eCode: $('#toemployeeCode').find('option:selected').text() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {

                if (jsonResult.Message == "Failure") {
                    $app.showAlert(jsonResult.result, 4)
                } else {
                    $app.hideProgressModel();
                    var oData = new Object();

                    oData.filePath = jsonResult.result;
                    $app.downloadSync('Download/DownloadPaySlip', oData);
                    $app.showAlert("Download is inprogress.Please wait some time", 1);
                }
            },
            error: function (x, t, m) {
                if (t === "timeout") {
                    alert("got timeout");
                } else {
                    alert(t);
                }
            },
            complete: function () {
                $app.hideProgressModel();

            }

        });

    },

    selectedEmployee: function () {
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmployeeData",
            data: JSON.stringify({ empId: $('#hdnEmpId').val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                debugger
                $declaractionEntry.empobj = jsonResult.result;
                if ($("#hdnRoleName").val() != "Employee") {
                    var hdnEmployeeId = $('#hdnEmployeeId').val();
                    $('#hdnEmployeeId').val($('#hdnEmpId').val());             
                    $("#ddlCategory").val($declaractionEntry.empobj.categoryId);
                    $declaractionEntry.loadEmployee();
                    $("#ddlEmployee").val($('#hdnEmpId').val());
                    $("#ddlEmployee").attr('disabled', true);
                    $("#ddlCategory").attr('disabled', true);
                    $('#hdnEmployeeId').val(hdnEmployeeId);
                }              
            },
            error: function (jsonResult) {

            },
            complete: function () {
                $app.hideProgressModel();
            }

        });
    },

    OthIncEntry: function () {
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var EmployeeId = $('#ddlEmployee').val();
        if (EmployeeId == '00000000-0000-0000-0000-000000000000') {
            EmployeeId = null;
        }
        if (INPMonth.toUpperCase() != '--SELECT--' && EmployeeId != null && EmployeeId.toUpperCase() != null) {
            var month = $('#ddMonth').val();
            var year = $('#txtYear').val();
            var FinanceYear = $declaractionEntry.financeYear.id;
            $.ajax({
                url: $app.baseUrl + "TaxDeclaration/GetProjIncome",
                data: JSON.stringify({ FinanceYear: FinanceYear, EmployeeId: EmployeeId, Month: month,Year:year}),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                async: false,
                success: function (jsonResult) {
                    switch (jsonResult.Status) {
                        case true:
                            $app.hideProgressModel();
                            $declaractionEntry.renderOthIncome(jsonResult.result)
                            break;
                        case false:
                            $app.showAlert(jsonResult.Message, 4);
                            break;
                    }
                },

            });
            $('#ShowOtherEntry').modal('show');
        }
    },

    OthIncEntry_Init: function () {
        $declaractionEntry.LoadLock();
        var INPMonth = $('#ddMonth').val();
        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;
        $("#btnOthSave").show();
        $('#msg2').show();
        if ($declaractionEntry.payrollLockRelease == 1) {
            $("#btnOthSave").hide();
            $('#msg2').hide();
        }
        else {
            if (InpDate >= SYSDat) {
                $("#btnOthSave").show();
                $('#msg2').show();
            }
            else {
                $("#btnOthSave").hide();
                $('#msg2').hide();
            }
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $("#btnOthSave").hide();
                $('#msg2').hide();
            }
        }
        $declaractionEntry.OthIncEntry();
    },

    renderOthIncome: function (result) {
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        $('#msg1').html('');
        $('#msg2').html('');
        if (result.length > 0) {
               $('#Income1').val(result[0].ProjIncome1);
               $('#Income2').val(result[0].ProjIncome2);
               $('#Income3').val(result[0].ProjIncome3);
            $('#total').val(result[0].ProjIncome1 + result[0].ProjIncome2 + result[0].ProjIncome3);

            if (result[0].MonthCol != INPMonth) {
                $('#msg1').html("Last Entry Details for the month of " + result[0].MonthCol + "/" + result[0].YearCol);
                $('#msg2').html("TO Save the Details 'Click' the Save button for Current Month " + INPMonth + "/" + INPYear);
            }
            else {
                $('#msg1').html("Details for the month of " + result[0].MonthCol + "/" + result[0].YearCol);
            }
        }
        if ($('#msg1').html().trim() == "" &&  ($('#msg2').html().trim() == "")) {
            $('#msg-head').hide();
        }
        if ($('#msg2').html().trim() == "") {
            $('#msg2').hide();
        }        

    },

    submitOthIncEntry: function () {
        var data = [];
        var status = false;
        var value = new Object();
        value.financeyear = $declaractionEntry.financeYear.id;
        value.month = $('#ddMonth').val();
        value.year = $('#txtYear').val();
        value.EmployeeId = $('#ddlEmployee').val();
        value.income1 = $('#Income1').val();
        value.income2 = $('#Income2').val();
        value.income3 = $('#Income3').val();
        data.push(value);
        status = true;
        if (status == true) {
            $.ajax({
                url: $app.baseUrl + "TaxDeclaration/SaveProjIncome",
                data: JSON.stringify({ datavalue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                async: false,
                success: function (jsonResult) {
                    switch (jsonResult.Status) {
                        case true:
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 2);
                            break;
                        case false:
                            $app.showAlert(jsonResult.Message, 4);
                            break;
                    }
                }
            });
        }
    }
}



