﻿$('#LICPolicyno').change(function (value) {

    var policyno = document.getElementById('LICPolicyno').value
    for (var m = 0; m < $LICpremium.Policynolist.length; m++) {
        if (policyno == $LICpremium.Policynolist[m]) {
            $app.showAlert("Policyno already exist", 4);
            $('#LICPolicyno').val('');
        }
    }
});

$('#lic-btn-new').click(function () {
    $('#lic-new-entry').removeClass("nodisp");
    $('#lic-show-screen').addClass("nodisp");
    $('#lic-new-entry').modal("show");
    $('#lic-show-screen').modal("hide");

});


var $LICpremium = {
    FinancialyrId: null,
    LICSectionId: null,
    EmpCode: null,
    selectedEmployeeId: null,
    cansave: false,
    LifeInsuranceYrList: null,
    Policynolist: null,
    CurrLICid: "00000000-0000-0000-0000-000000000000",
    formData: document.forms["frmLICEntry"],
    Lifeinsurancepolicylist: null,
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
                $LICpremium.showbutton = "N";
            }
        }
        else {
            $LICpremium.showbutton = "Y";
        }
    },

    AddInitialize: function () {

        var formData = document.forms["frmLICEntry"];
        formData.elements["LICPolicyno"].value = "",
        formData.elements["LICDateofpolicy"].value = "",
        formData.elements["LIcpersonname"].value = "",
        formData.elements["LIcsumassured"].value = "",
        formData.elements["LIcprimiumamt"].value = "",
        formData.elements["LIcamount"].value = "",
        formData.elements["hiddenLICid"].value = ""
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
            $("#btnLICPremiumsave").hide();
        }
        else {
            if (InpDate >= SYSDat) {
                $("#btnLICPremiumsave").show();
            }
            else {
                $("#btnLICPremiumsave").hide();
            }
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $("#btnLICPremiumsave").hide();
            }
        }

        $LICpremium.SelectLifeinsurance();
      //  $LICpremium.BindLICdetails();
    },
    SelectLifeinsurance: function () {
        var decform = document.forms["frmDeclaration"];
        var Year = decform.elements["txtYear"].value;
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/GetLifeinsurancelist",
            data: JSON.stringify({ financialYearId: $LICpremium.FinancialyrId, year: Year }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {

                $LICpremium.LifeInsuranceYrList = jsonResult.result.InsuranceList;
                $LICpremium.Policynolist = jsonResult.result.Policylist;
            }

        });
    },

    findTotal: function () {
        debugger;
        var decform = document.forms["frmDeclaration"];
        var Month = decform.elements["ddMonth"].value; var Year = decform.elements["txtYear"].value;
        var premiumtotal = 0;

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "TaxDeclaration/GetLifeInsurance",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ employeeId: $LICpremium.selectedEmployeeId, financialYearId: $LICpremium.FinancialyrId, month: Month, year: Year, sectionId: $LICpremium.LICSectionId }),
            dataType: "json",
            success: function (msg) {
                var out = msg.result;
                for (var i = 0; i < out.length; i++) {
                    premiumtotal += parseFloat(out[i].annualPremium);
                }
                $('#newlicamt').text(premiumtotal);
            }
        })
    },

    BindLICdetails: function () {

        $app.applyseletedrow();
        $LICpremium.ButtonInit();
        if ($LICpremium.showbutton == "N") {
            $('#lic-btn-new').addClass("nodisp");
        }
        else {
            $('#lic-btn-new').removeClass("nodisp");
        }

        var decform = document.forms["frmDeclaration"];
        var Month = decform.elements["ddMonth"].value; var Year = decform.elements["txtYear"].value;
        var dtClientList = $('#LICPremiumtbl').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'responsive': true,
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            columns: [
                        { "data": "id" },
                        { "data": "policyNumber" },
                        { "data": "policyDate" },
                        { "data": "insuredPersonName" },
                        { "data": "relationship" },
                        { "data": "Sumassured" },
                        { "data": "premiumAmount" },
                        { "data": "premiumAmountFallingDueInFeb" },
                        { "data": "annualPremium" },
                        { "data": "Premiumdeduction" },
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
                 "sClass": "word-wrap premamt"

             },
          {
              "aTargets": [10],
              "sClass": "actionColumn"
                        ,
              "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                  if ($LICpremium.showbutton == "N") {
                      var b = $('<a href="#" class="nodisp editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                      var c = $('<a href="#" class="nodisp deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                  }
                  else {
                      var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                      var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                  }

                  b.button();
                  b.on('click', function () {

                      $LICpremium.EditLifeinsurance(oData);
                      $LICpremium.CurrLICid = oData.id;

                  });
                  c.button();
                  c.on('click', function () {
                      if (confirm("Are you sure, do you want to Delete?")) {
                          $LICpremium.DeleteLifeinsurance(oData);
                      }
                      return false;
                  });
                  $(nTd).empty();
                  $(nTd).append(b, c);

              }
          }


            ],
            ajax: function (data, callback, settings) {

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxDeclaration/GetLifeInsurance",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ employeeId: $LICpremium.selectedEmployeeId, financialYearId: $LICpremium.FinancialyrId, month: Month, year: Year, sectionId: $LICpremium.LICSectionId }),
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
                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {

                var r = $('#LICPremiumtbl tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#LICPremiumtbl thead').append(r);
                $('#search_0').css('text-align', 'center');

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


    save: function () {

        var err = 0
        $(".required").each(function () {


            if (this.id == "LICrelation" || this.id == "LICtreatment" || this.id == "LICdisability") {
                if ($('#' + this.id).val() == "") {
                    $app.showAlert(this.id == "LICrelation" ? 'Please Select Relation' : this.id == "LICtreatment" ? 'Please Select Treatment' : 'Please Select Disability', 4);
                    err = 1;
                    $('#' + this.id).focus();
                    return false;
                }
            }
            if (this.id == "LICDateofpolicy") {

                if ($('#' + this.id).val() == "") {
                    $app.showAlert('Please Select Commencement Date', 4);
                    err = 1;
                    //$('#' + this.id).focus();
                    return false;
                }
            }

            if (this.id == "LICPolicyno") {

                var currPolicyno = $('#' + this.id).val();
                for (var i = 0; i < $LICpremium.Policynolist.length; i++) {
                    if (currPolicyno == $LICpremium.Policynolist[i]) {
                        $app.showAlert('Policy number already exist', 4);
                        err = 1;
                    }
                    else {
                        return false;
                    }
                }
            }

        });

        var formData = document.forms["frmLICEntry"];
        var decform = document.forms["frmDeclaration"];
        var data = {
            financialYearId: $LICpremium.FinancialyrId,
            sectionid: $LICpremium.LICSectionId,
            employeecode: $LICpremium.EmpCode,
            employeeId: $LICpremium.selectedEmployeeId,
            policyNumber: formData.elements["LICPolicyno"].value,
            policyDate: formData.elements["LICDateofpolicy"].value,
            insuredPersonName: formData.elements["LIcpersonname"].value,
            Relationship: formData.elements["LICrelation"].value,
            isDisabilityPerson: formData.elements["LICdisability"].value,
            isPersonTakingTreatement: formData.elements["LICtreatment"].value,
            Sumassured: formData.elements["LIcsumassured"].value,
            premiumAmount: formData.elements["LIcprimiumamt"].value,
            premiumAmountFallingDueInFeb: formData.elements["LIcamount"].value,
            month: decform.elements["ddMonth"].value,
            year: decform.elements["txtYear"].value,
            annualPremium: 0
        };
        if (err == 0) {
            $.ajax({
                url: $app.baseUrl + "TaxDeclaration/SaveLifeInsurance",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                async: false,
                type: "POST",
                success: function (jsonResult) {

                    //$app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:
                            $LICpremium.BindLICdetails();
                            $LICpremium.findTotal();
                            $app.showAlert(jsonResult.Message, 2);
                            $LICpremium.AddInitialize();
                            $LICpremium.cansave = true;
                            if ($('#hiddenLICid').val() == "") {
                                $LICpremium.CurrLICid = jsonResult.result.id; $('#hiddenLICid').val(jsonResult.result.id);
                            }
                            else {
                                $LICpremium.CurrLICid = "00000000-0000-0000-0000-000000000000"; $('#hiddenLICid').val('');
                            }
                            companyid = 0;
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
        }
    },

    EditLifeinsurance: function (data) {

        var formData = document.forms["frmLICEntry"];
        formData.elements["hiddenLICid"].value = data.id,
        formData.elements["LICPolicyno"].value = data.policyNumber,
        formData.elements["LICDateofpolicy"].value = data.policyDate,
        formData.elements["LIcpersonname"].value = data.insuredPersonName,
        formData.elements["LICrelation"].value = data.relationship,
        formData.elements["LICdisability"].value = data.isDisabilityPerson,
        formData.elements["LICtreatment"].value = data.isPersonTakingTreatement,
        formData.elements["LIcsumassured"].value = data.Sumassured,
        formData.elements["LIcprimiumamt"].value = data.premiumAmount,
        formData.elements["LIcamount"].value = data.premiumAmountFallingDueInFeb
    },
    DeleteLifeinsurance: function (data) {

        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/DeleteLifeInsurance",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $LICpremium.BindLICdetails();
                        $LICpremium.findTotal();
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
    LICPremium: function (financialId, SectionId) {

        $LICpremium.FinancialyrId = financialId;
        $LICpremium.LICSectionId = SectionId;
        $LICpremium.EmpCode = ddlEmployee;
        $LICpremium.AddInitialize();
        $LICpremium.ButtonInit();
        $LICpremium.BindLICdetails();
        $LICpremium.findTotal();
    },


    ///LIC premium policy


    AddpremiumformInitialize: function () {

        var formData = document.forms["frmLICPremiumEntry"];
        var formPolicyData = document.forms["frmLICEntry"];
        formData.elements["LICpremiumPolicyno"].value = $('#LICPolicyno').val(),
         formData.elements["LICpremiumamt"].value = "",
         formData.elements["LICpremiumpolicydate"].value = "",
        $('#frmLICPremiumEntry').on("submit", function (e) {

            return false;
        });


        $LICpremium.SelectLifeinsurance();
    },
    Premiumformsave: function () {

    },
    BindLICPremiumdetails: function () {
        $app.applyseletedrow();
        $LICpremium.ButtonInit();
        if ($LICpremium.showbutton == "N") {
            $('#lic-btn-new').addClass("nodisp");
        }
        else {
            $('#lic-btn-new').removeClass("nodisp");
        }

        var decform = document.forms["frmLICPremiumEntry"];
        var dtClientList = $('#LICPolicyPremiumformtbl').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'responsive': true,
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            "aaSorting": [[1, "asc"]],
            "sSearch": "Search:",
            "bFilter": true,
            columns: [
                        { "data": "id" },
                        { "data": "premiumAmount" },
                        { "data": "PremiumDate" },
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
              "sClass": "actionColumn"
                        ,
              "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                  if ($LICpremium.showbutton == "N") {
                      var b = $('<a href="#" class="nodisp editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                      var c = $('<a href="#" class="nodisp deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                  }
                  else {
                      var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                      var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                  }


                  b.button();
                  b.on('click', function () {

                      $LICpremium.EditLifeinsurancepremium(oData);

                  });
                  c.button();
                  c.on('click', function () {
                      if (confirm("Are you sure, do you want to Delete?")) {
                          $LICpremium.DeleteLifeinsurancepremium(oData);
                      }
                      return false;
                  });
                  $(nTd).empty();
                  $(nTd).append(b, c);

              }
          }


            ],
            ajax: function (data, callback, settings) {
                var taxlicId = $('#hiddenLICid').val() == "" ? "00000000-0000-0000-0000-000000000000" : $('#hiddenLICid').val();

                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "TaxDeclaration/GetLifeInsurancePolicy",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ employeeId: $LICpremium.selectedEmployeeId, financialYearId: $LICpremium.FinancialyrId, TXLICid: taxlicId }),
                    dataType: "json",
                    success: function (msg) {

                        var out = msg.result;
                        $LICpremium.Lifeinsurancepolicylist = msg.result;
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length
                            });

                        }, 50);
                    },
                    error: function (msg) {
                    }
                });


            },
            fnInitComplete: function (oSettings, json) {

                var r = $('#LICPremiumtbl tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#LICPremiumtbl thead').append(r);
                $('#search_0').css('text-align', 'center');

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
    SaveLICPremiumpolicy: function () {

        var formData = document.forms["frmLICEntry"];
        var Premiumform = document.forms["frmLICPremiumEntry"];
        var decform = document.forms["frmDeclaration"];
        var Month = decform.elements["ddMonth"].value; var Year = decform.elements["txtYear"].value;
        if (Premiumform.elements["LICpremiumPolicyno"].value != "") {
            var LICID = formData.elements["hiddenLICid"].value;
            if (LICID == "") {
                $LICpremium.save();
                $('#hiddenLICid').val('');
            }

            var data = {
                LifeInsuranceId: $LICpremium.CurrLICid,
                financialYearId: $LICpremium.FinancialyrId,
                employeeId: $LICpremium.selectedEmployeeId,
                policyNumber: Premiumform.elements["LICpremiumPolicyno"].value,
                PremiumDate: Premiumform.elements["LICpremiumpolicydate"].value,
                premiumAmount: Premiumform.elements["LICpremiumamt"].value,
                annualPremium: 0,
                month: Month, year: Year
            };

            $LICpremium.saveLICPremium(data);

        }


    },
    //checkPolicyNo: function (PolicyNumber) {
    //    
    //    for (var m = 0; m < $LICpremium.Policynolist.length; m++) {
    //        if (PolicyNumber == $LICpremium.Policynolist[m])
    //        {
    //            return true;
    //        }
    //        else {
    //            $LICpremium.save();
    //            return true;
    //        }
    //    }
    //},
    saveLICPremium: function (data) {
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/SaveLifePremiumInsurance",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $LICpremium.BindLICdetails();
                        $LICpremium.findTotal();
                        $LICpremium.EditLifeinsurance(p[0]);
                        $LICpremium.BindLICPremiumdetails();
                        companyid = 0;
                        $('#LICpremiumamt').val(''); $('#LICpremiumpolicydate').val('');
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
    EditLifeinsurancepremium: function (data) {

        var formData = document.forms["frmLICPremiumEntry"];
        formData.elements["LICpremiumamt"].value = data.premiumAmount,
        formData.elements["LICpremiumpolicydate"].value = data.PremiumDate

    },
    DeleteLifeinsurancepremium: function (data) {
        var decform = document.forms["frmDeclaration"];
        var Month = decform.elements["ddMonth"].value; var Year = decform.elements["txtYear"].value;
        $.ajax({
            url: $app.baseUrl + "TaxDeclaration/DeleteLifeInsurancepolicy",
            data: JSON.stringify({ dataValue: data, month: Month, year: Year }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {

                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $LICpremium.BindLICPremiumdetails();
                        $LICpremium.BindLICdetails();
                        $LICpremium.findTotal();
                        var p = jsonResult.result;
                        $LICpremium.EditLifeinsurance(p[0]);
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

};