﻿$('#MedType,#medRelationship,#medSeniorcitizen,#medExpenceincurred,#txtAmountPremium,#medcash').change(function () {

    $medicalinsurance.Declarevalue();
});

$('#btn-new').click(function () {
    $('#new-entry-screen').removeClass("nodisp");
    $('#show-screen').addClass("nodisp");
    $('#new-entry-screen').modal("show");
    $('#show-screen').modal("hide");

});

$medicalinsurance = {
    financeYear: $companyCom.getDefaultFinanceYear(),
    selectedId: null,
    showbutton: null,

    ButtonInit: function () {
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
        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $medicalinsurance.showbutton = "N";
            }
        }
        else {
            $medicalinsurance.showbutton = "Y";
        }
    },

    ADDinitialize: function () {
        formData: document.forms["frmMedicalInsurance"],
        $medicalinsurance.LoadDetail();
        $medicalinsurance.renderDetail();
        $medicalinsurance.medicalInsuranceCalculation();
        $medicalinsurance.Clear();
        $('#medExpenceincurred').val(2);
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
            $("#btnSaveMedInsurance").hide();
            $("#btnClearMedInsurance").hide();
        }
        else {
            if (InpDate >= SYSDat) {
                $("#btnSaveMedInsurance").show();
            }
            else {
                $("#btnSaveMedInsurance").hide();
                $("#btnClearMedInsurance").hide();
            }
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $medicalinsurance.showbutton = "N";
                $("#btnSaveMedInsurance").hide();
                $("#btnClearMedInsurance").hide();
            }
        }
    },

    Declarevalue: function () {

        var data = $medicalinsurance.MedicalData();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/MedicalPremiumcal",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                        var dataresult = jsonResult.result;
                        $('#txtEligibleDeduction').val(dataresult.EligibleDeductionforthepolicy);
                        break;
                    case false:
                        $app.showAlert("Check the value", 4);
                        return false;
                        break;
                }
            }
        });
    },
    MedicalData: function () {

        var err = 0
        var retObject = {
            EmployeeId: $('#ddlEmployee').val(),
            TxSectionId: $declaractionEntry.MedicalTxSectionId,
            FinYearId: $medicalinsurance.financeYear.id,
            EffectiveMonth: $('#ddMonth').val(),
            EffectiveYear: $('#txtYear').val(),
            InsuranceType: $('#MedType').val(),
            PolicyNo: $('#txtPolicyNum').val(),
            DateofCommencofpolicy: $('#txtmedloandate').val(),
            InsuredPersonName: $('#txtInsuredName').val(),
            RelationshipoftheInsuredperson: $('#medRelationship').val(),
            CoveredinthepolicyisSeniorCitizen: $('#medSeniorcitizen').val(),
            IncurredinrespectofVerySeniorCitizen: $('#medExpenceincurred').val(),
            AmountofpremiumorExpense: $('#txtAmountPremium').val(),
            PayMode: $('#medcash').val(),
            EligibleDeductionforthepolicy: $('#txtEligibleDeduction').val(),
            SelfSpouseChildOveralldeduction: $('#lblSpousechild').val(),
            ParentOveralldeduction: $('#lblparents').val(),
            TotalDeduction: $('#lblTotaldeduction').val()
        };
        return retObject;
    },
    LoadDetail: function () {

        $app.applyseletedrow();
        $medicalinsurance.ButtonInit();
        if ($medicalinsurance.showbutton == "N") {
            $('#btn-new').addClass("nodisp");
        }
        else {
            $('#btn-new').removeClass("nodisp");
        }
        var decform = document.forms["frmDeclaration"];
        var Month = decform.elements["ddMonth"].value;
        var Year = decform.elements["txtYear"].value;
        var dtClientList = $('#MEDPremiumtbl').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'responsive': true,
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            columns: [
                        { "data": "Id" },
                        { "data": "InsuranceType" },
                        { "data": "PolicyNo" },
                        { "data": "PolicyDate" },
                        { "data": "InsuredPersonName" },
                        { "data": "RelationshipoftheInsuredperson" },
                        { "data": "CoveredinthepolicyisSeniorCitizen" },
                        { "data": "IncurredinrespectofVerySeniorCitizen" },
                        { "data": "AmountofpremiumorExpense" },
                        { "data": "PayMode" },
                        { "data": "EligibleDeductionforthepolicy" },
                        { "data": null },
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
                 "sClass": "word-wrap"

             },
              {
                  "aTargets": [7],
                  "sClass": "word-wrap"

              },
              {
                  "aTargets": [8],
                  "sClass": "word-wrap"

              },
             {
                 "aTargets": [9],
                 "sClass": "word-wrap"

             },
             {
                 "aTargets": [10],
                 "sClass": "word-wrap"

             },
          {
              "aTargets": [11],
              "sClass": "actionColumn"
                        ,
              "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                  if ($medicalinsurance.showbutton == "N") {
                      var b = $('<a href="#" class="nodisp editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                      var c = $('<a href="#" class="nodisp deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                  }
                  else {
                      var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                      var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                  }
                      b.button();
                      b.on('click', function () {

                          $medicalinsurance.EditMedicalInsurance(oData);
                          $medicalinsurance.medicalInsuranceCalculation();


                      });
                      c.button();
                      c.on('click', function () {
                          if (confirm("Are you sure, do you want to Delete?")) {
                              $medicalinsurance.DeleteMedicalInsurance(oData);
                              $medicalinsurance.medicalInsuranceCalculation();
                          }
                          return false;
                      });
                      $(nTd).empty();
                      $(nTd).append(b, c);
              },
          }


            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxDeclaration/GetMedicalInsurance",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ EmployeeId: $('#ddlEmployee').val(), FinancialYear: $medicalinsurance.financeYear.id, Month: Month, Year: Year, TXSection: $declaractionEntry.MedicalTxSectionId, Fieldname: 'Grid' }),
                    dataType: "json",
                    success: function (msg) {
                        var out = msg.result;
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length
                            });

                        }, 50);

                        // $medicalinsurance.renderDetail();
                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {

                var r = $('#MEDPremiumtbl tfoot tr');
                r.find('th').each(function () {
                    $('#MEDPremiumtbl').css('padding', 8);
                });
                $('#MEDPremiumtbl thead').append(r);
                $('#search_0').css('text-align', 'center');
                $medicalinsurance.selectedId = null;

            },
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            dom: "rtiS",
            "bDestroy": true,


            scroller: {
                loadingIndicator: true
            }
        });
    },
    Save: function () {
        var err = 0;
        $(".Reqrd").each(function () {
            if (this.id == "MedType" || this.id == "medRelationship" || this.id == "medSeniorcitizen" || this.id == "medExpenceincurred" || this.id == "medcash") {
                if (document.getElementById(this.id).value == "0") {
                    $app.showAlert(this.id == "MedType" ? 'Please Select Type' : this.id == "medRelationship" ? 'Please Select the Relationship' : this.id == "medSeniorcitizen" ? 'Please Select Seniorcitizen' : this.id == "medExpenceincurred" ? 'Please Select incurred in respect of Very Senior citizen(More than 80 years age) and no insurance has been taken for that person' : 'Pelease Select CashMode', 4);
                    err = 1;
                    return false;
                }
            }
            else if (this.id == "txtAmountPremium" || this.id == "txtPolicyNum" || this.id == "txtInsuredName") {

                if (document.getElementById(this.id).value == "") {

                    $app.showAlert('Please ' + $(this).attr('placeholder'), 4);
                    err = 1;
                    return false;
                }
            }
        });
        if (err == 1) {
            return false;
        }

        var data = $medicalinsurance.renderdata();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/SaveMedicalInsurance",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $medicalinsurance.renderDetail();
                        $medicalinsurance.LoadDetail();
                        $medicalinsurance.medicalInsuranceCalculation();
                        $medicalinsurance.SubmitMedicalInsurance();
                        $medicalinsurance.Clear();
                        $app.showAlert("Data Saved Successfully", 2);
                        return true;
                        break;
                    case false:
                        $app.showAlert("Data not saved check Policy Date", 4);
                        return false;
                        break;
                }
            }
        });

        $('#new-entry-screen').addClass("nodisp");
        $('#new-entry-screen').modal("hide");
        $('#show-screen').removeClass("nodisp");
        $('#show-screen').modal("show");
        $('#btnClearMedInsurance').removeClass("nodisp");
    },
    renderdata: function () {

        var err = 0
        var retObject = {
            EmployeeId: $('#ddlEmployee').val(),
            TxSectionId: $declaractionEntry.MedicalTxSectionId,
            FinYearId: $medicalinsurance.financeYear.id,
            id: $medicalinsurance.selectedId,
            EffectiveMonth: $('#ddMonth').val(),
            EffectiveYear: $('#txtYear').val(),
            InsuranceType: $('#MedType').val(),
            PolicyNo: $('#txtPolicyNum').val(),
            DateofCommencofpolicy: $('#txtmedloandate').datepicker('getDate'),
            InsuredPersonName: $('#txtInsuredName').val(),
            RelationshipoftheInsuredperson: $('#medRelationship').val(),
            CoveredinthepolicyisSeniorCitizen: $('#medSeniorcitizen').val(),
            IncurredinrespectofVerySeniorCitizen: $('#medExpenceincurred').val(),
            AmountofpremiumorExpense: $('#txtAmountPremium').val(),
            PayMode: $('#medcash').val(),
            EligibleDeductionforthepolicy: $('#txtEligibleDeduction').val(),
            SelfSpouseChildOveralldeduction: $('#lblSpousechild').val(),
            ParentOveralldeduction: $('#lblparents').val(),
            TotalDeduction: $('#lblTotaldeduction').val()
        };
        return retObject;
    },
    EditMedicalInsurance: function (data) {

        var policydate = data.PolicyDate;
        var InsureType = data.InsuranceType == "Medical Insurance" ? "1" : data.InsuranceType == "CGHS contribution" ? "2" : data.InsuranceType == "Preventive Health Check up" ? "3" : "4";
        var RelationshipInsuredperson = data.RelationshipoftheInsuredperson == "Self Spouse & Child" ? "1" : "2";
        var CoveredpolicyisSeniorCitizen = data.CoveredinthepolicyisSeniorCitizen == "Senior Citizen" ? "1" : "2";
        var IncurredrespectofVerySeniorCitizen = data.IncurredinrespectofVerySeniorCitizen == "Yes" ? "1" : data.IncurredinrespectofVerySeniorCitizen == "NO" ? "2" : "3";
        var pay = data.PayMode == "Cash" ? "1" : "2";
        var formData = document.forms["frmMedicalInsurance"];
        $medicalinsurance.selectedId = data.Id,
        formData.elements["MedType"].value = InsureType,
        formData.elements["txtPolicyNum"].value = data.PolicyNo,
        formData.elements["txtmedloandate"].value = policydate,
        formData.elements["txtInsuredName"].value = data.InsuredPersonName,
        formData.elements["medRelationship"].value = RelationshipInsuredperson,
        formData.elements["medSeniorcitizen"].value = CoveredpolicyisSeniorCitizen,
        formData.elements["medExpenceincurred"].value = IncurredrespectofVerySeniorCitizen,
        formData.elements["txtAmountPremium"].value = data.AmountofpremiumorExpense,
        formData.elements["medcash"].value = pay,
            formData.elements["txtEligibleDeduction"].value = data.EligibleDeductionforthepolicy
        $('#new-entry-screen').removeClass("nodisp");
        $('#new-entry-screen').modal("show");
        $('#show-screen').modal("hide");
        $('#btnClearMedInsurance').addClass("nodisp");
    },
    DeleteMedicalInsurance: function (data) {
        debugger
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/DeleteMedicalInsurance",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $medicalinsurance.renderDetail();
                        $medicalinsurance.LoadDetail();
                        $medicalinsurance.medicalInsuranceCalculation();
                        $medicalinsurance.SubmitMedicalInsurance();
                        $medicalinsurance.Clear();
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
    renderDetail: function () {
        debugger
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/GetMedicalInsurance",
            data: JSON.stringify({ EmployeeId: $('#ddlEmployee').val(), FinancialYear: $medicalinsurance.financeYear.id, Month: $('#ddMonth').val(), Year: $('#txtYear').val(), TXSection: $declaractionEntry.MedicalTxSectionId, Fieldname: 'Fields' }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:

                        var dataresult = jsonResult.result;
                        $('#txtEmployerName').val(dataresult[0].CompanyName),
                        $('#txtEmployeeNumber').val(dataresult[0].EmployeeCode),
                        $('#txtEmployeeName').val(dataresult[0].Employeename),
                        $('#txtPAN').val(dataresult[0].PANNumber),
                        $('#txtFinancialyear').val(dataresult[0].Financial_Year)
                        // $app.showAlert(jsonResult.Message, 2);
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
    Clear: function () {
        $medicalinsurance.selectedId = null,
       $('#MedType').val(1),
       $('#txtPolicyNum').val(""),
       $('#txtmedloandate').val(""),
       $('#txtInsuredName').val(""),
       $('#medRelationship').val(1),
       $('#medSeniorcitizen').val(1),
       $('#medExpenceincurred').val(1),
       $('#txtAmountPremium').val(""),
       $('#medcash').val(1),
       $('#txtEligibleDeduction').val("")
       $('#new-entry-screen').addClass("nodisp");
       $('#new-entry-screen').modal("hide");
       $('#show-screen').removeClass("nodisp");
       $('#show-screen').modal("show");
    },

    medicalInsuranceCalculation: function () {
        var data = $medicalinsurance.MedicalData();
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/medicalInsuranceCalculation",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                        var dataresult = jsonResult.result;
                        $('#txtTotalAmount').val(dataresult[0].TotalAmount);
                        $('#txtTotalEligibleDed').val(dataresult[0].TotalEligibleDeduction);
                        $('#newmedamt').text(dataresult[0].TotalDeduction);
                        $('#lblparents').text(dataresult[0].ParentOveralldeduction);
                        $('#lblSpousechild').text(dataresult[0].SelfSpouseChildOveralldeduction);
                        $('#lblTotaldeduction').text(dataresult[0].TotalDeduction);
                        //$('#txtEligibleDeduction').val(dataresult.EligibleDeductionforthepolicy);
                        break;
                    case false:
                        $app.showAlert("Check the value", 4);
                        return false;
                        break;
                }
            }
        });
    },
    SubmitMedicalInsurance: function () {

        var data = [];
        var val = new Object();
        val.EmployeeId = $('#ddlEmployee').val();
        val.FinancialYear = $medicalinsurance.financeYear.id;
        val.SectionId = $declaractionEntry.MedicalTxSectionId;
        val.effectiveDate = $('#txtYear').val() + "-" + $('#ddMonth').val();
        val.value = $('#lblTotaldeduction').text();
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

                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },

        });

    }
};
