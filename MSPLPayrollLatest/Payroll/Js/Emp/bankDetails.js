var $empbank = {
    canSave: false,
    selectedEmployeeId: '',
    empBankid: '',
    emppersonalid: '',
    formData: document.forms["frmEmpBank"],
    bindEvent: function () {

        $('#frmEmpBank').on('submit', function () {

            $empbank.save();
            return false;
        });
        $("#txtMaritalStatus").change(function () {


            if ($("#txtMaritalStatus").val() == "Single") {
                $("#txtSpouseName").attr("disabled", "disabled");
                $("#txtSpouseName").val("");
            }
            else {
                $("#txtSpouseName").removeAttr("disabled");
            }


        });
    },
    get: function (employee) {
        $empbank.AddInitialize();
        $empbank.canSave = true;
        $empbank.bindEvent();
        $empbank.selectedEmployeeId = employee.id;
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpBankDetails",
            data: JSON.stringify({ employeeId: $empbank.selectedEmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $empbank.render(p);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpPersonalDetails",
            data: JSON.stringify({ employeeId: $empbank.selectedEmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var formData = $empbank.formData;
                        var data = jsonResult.result
                        $empbank.emppersonalid = data.empPersonalid;
                        formData.elements["txtPersonalMobileNo"].value = data.personalmobileno;
                        formData.elements["txtOfficeMobileNo"].value = data.officemobileno;
                        formData.elements["txtExtensionNo"].value = data.extensionno;
                        formData.elements["txtPersonalEmail"].value = data.personalmail;
                        formData.elements["txtOfficeEmail"].value = data.officemail;
                        formData.elements["ddBloodGroup"].value = data.bloodgroup;
                        formData.elements["txtPaySlipRemarks"].value = data.payslipremarks;
                        formData.elements["chkIsDisable"].checked = data.isdisable;
                        formData.elements["chkIsSeniorCitizen"].checked = data.iseniorcitizen;
                        formData.elements["chkPrintCheque"].checked = data.isprintcheque;
                        formData.elements["txtFatherName"].value = data.empfathername;
                        formData.elements["txtMaritalStatus"].value = data.maritalstatus;
                        formData.elements["txtSpouseName"].value = data.spousename;
                        formData.elements["txtNoOfChildren"].value = data.noofchildren;
                        formData.elements["txtPFNumber"].value = data.pfnumber;
                        formData.elements["txtPFUAN"].value = data.pfuan;
                        formData.elements["txtPFCofirmationDate"].value = data.pfconfirmationdate;
                        formData.elements["txtPANNumber"].value = data.pannumber;
                        formData.elements["txtESINumber"].value = data.esinumber;
                        formData.elements["txtAdharNumber"].value = data.adharnumber;
                        formData.elements["txtPensionElig"].value = data.PensionEligible;
                        $("#txtMaritalStatus").change();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    save: function () {
        debugger;
        if (!$empbank.canSave) {
            return false;
        }
        $empbank.canSave = false;
        $app.showProgressModel();
        var data = new Array();
        var formData = $empbank.formData;
        //personlDetails
        var personlDetails = {
            empPersonalid: $empbank.emppersonalid,
            employeeId: $empbank.selectedEmployeeId,
            personalmobileno: formData.elements["txtPersonalMobileNo"].value,
            officemobileno: formData.elements["txtOfficeMobileNo"].value,
            extensionno: formData.elements["txtExtensionNo"].value,
            personalmail: formData.elements["txtPersonalEmail"].value,
            officemail: formData.elements["txtOfficeEmail"].value,
            bloodgroup: formData.elements["ddBloodGroup"].value,
            payslipremarks: formData.elements["txtPaySlipRemarks"].value,
            isdisable: formData.elements["chkIsDisable"].checked,
            iseniorcitizen: formData.elements["chkIsSeniorCitizen"].checked,
            isprintcheque: formData.elements["chkPrintCheque"].checked,
            empfathername: formData.elements["txtFatherName"].value,
            maritalstatus: formData.elements["txtMaritalStatus"].value,
            spousename: formData.elements["txtSpouseName"].value,
            noofchildren: formData.elements["txtNoOfChildren"].value,
            pfnumber: formData.elements["txtPFNumber"].value,
            pfuan: formData.elements["txtPFUAN"].value,
            pfconfirmationdate: formData.elements["txtPFCofirmationDate"].value,
            pannumber: formData.elements["txtPANNumber"].value,
            esinumber: formData.elements["txtESINumber"].value,
            adharnumber: formData.elements["txtAdharNumber"].value,
            PensionEligible: formData.elements["txtPensionElig"].value
        };
        //var personlDetails = {
        //    Id: $empbank.emppersonalid,
        //    EmployeeId: $empbank.selectedEmployeeId,
        //    PersonalMobileNo: formData.elements["txtPersonalMobileNo"].value,
        //    OfficeMobileNo: formData.elements["txtOfficeMobileNo"].value,
        //    ExtensionNo: formData.elements["txtExtensionNo"].value,
        //    PersonalEmail: formData.elements["txtPersonalEmail"].value,
        //    OfficeEmail: formData.elements["txtOfficeEmail"].value,
        //    BloodGroup: formData.elements["ddBloodGroup"].value,
        //    PaySlipRemarks: formData.elements["txtPaySlipRemarks"].value,
        //    IsDisable: formData.elements["chkIsDisable"].checked,
        //    IsSeniorCitizen: formData.elements["chkIsSeniorCitizen"].checked,
        //    PrintCheque: formData.elements["chkPrintCheque"].checked,
        //    FatherName: formData.elements["txtFatherName"].value,
        //    MaritalStatus: formData.elements["txtMaritalStatus"].value,
        //    SpouseName: formData.elements["txtSpouseName"].value,
        //    NoOfChildren: formData.elements["txtNoOfChildren"].value,
        //    PFNumber: formData.elements["txtPFNumber"].value,
        //    PFUAN: formData.elements["txtPFUAN"].value,
        //    PFConfirmationDate: formData.elements["txtPFCofirmationDate"].value,
        //    PANNumber: formData.elements["txtPANNumber"].value,
        //    ESINumber: formData.elements["txtESINumber"].value,
        //    AADHARNumber: formData.elements["txtAdharNumber"].value
        //};

        //bankDetails
        var bankDetails = {
            empBankid: $empbank.empBankid,
            empBankEmployeeId: $empbank.selectedEmployeeId,
            bankId: formData.elements["ddBank"].value,
            ifsc: formData.elements["txtIFSC"].value,
            acctno: formData.elements["txtAcctNo"].value,
            branchName: formData.elements["txtBranchName"].value,
            address: formData.elements["txtAddress"].value,
            city: formData.elements["txtCity"].value,
            state: formData.elements["txtState"].value,
        };

        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpBank",
            data: JSON.stringify({ dataValue: bankDetails }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();

                        $empbank.canSave = true;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $empbank.canSave = true;
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
                $empbank.canSave = true;
            }
        });
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpPersonal",
            data: JSON.stringify({ dataValue: personlDetails }),
            dataType: "json",
            async: false,
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $empbank.canSave = true;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $empbank.canSave = true;
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
                $empbank.canSave = true;
            }
        });
        return false;
    },
    AddInitialize: function () {
        $empbank.empBankid = '';

        var formData = $empbank.formData;
        $empbank. detailsClear(formData);

    },
    detailsClear: function (formData) {
        //Permenant address
        formData.elements["ddBank"].value = '00000000-0000-0000-0000-000000000000';
        formData.elements["txtIFSC"].value = "";
        formData.elements["txtAcctNo"].value = "";
        formData.elements["txtBranchName"].value = "";
        formData.elements["txtAddress"].value = "";
        formData.elements["txtCity"].value = "";
        formData.elements["txtState"].value = "";
        formData.elements["txtPersonalMobileNo"].value = "";
        formData.elements["txtOfficeMobileNo"].value = "";
        formData.elements["txtExtensionNo"].value = "";
        formData.elements["txtPersonalEmail"].value = "";
        formData.elements["txtOfficeEmail"].value = "";
        formData.elements["ddBloodGroup"].value = 0;
        formData.elements["txtPaySlipRemarks"].value = "";
        formData.elements["chkIsDisable"].checked = "";
        formData.elements["chkIsSeniorCitizen"].checked = "";
        formData.elements["chkPrintCheque"].checked = "";
        formData.elements["txtFatherName"].value = "";
        formData.elements["txtMaritalStatus"].value = "";
        formData.elements["txtSpouseName"].value = "";
        formData.elements["txtNoOfChildren"].value = "";
        formData.elements["txtPFNumber"].value = "";
        formData.elements["txtPFUAN"].value = "";
        formData.elements["txtPFCofirmationDate"].value = "";
        formData.elements["txtPANNumber"].value = "";
        formData.elements["txtESINumber"].value = "";
        formData.elements["txtAdharNumber"].value = "";
    },


    render: function (datas) {
        var formData = $empbank.formData;    
        // $comPopup.selectedId = ''
        for (var cnt = 0; cnt < datas.length; cnt++) {
            var data = datas[cnt];

            //Permenant address
            $empbank.empBankid = data.empBankid;
            formData.elements["ddBank"].value = data.bankId;
            formData.elements["txtIFSC"].value = data.ifsc;
            formData.elements["txtAcctNo"].value = data.acctno;
            formData.elements["txtBranchName"].value = data.branchName;
            formData.elements["txtAddress"].value = data.address;
            formData.elements["txtCity"].value = data.city;
            formData.elements["txtState"].value = data.state;



        }
    },


};;