var $empAddress = {
    canSave: false,
    selectedEmployeeId: '',
    selectedAddressId1: '',
    selectedAddressId2: '',
    formData: document.forms["frmEmpAddress"],
    bindEvent: function () {
        $("#chkSamePermanant").change(function () {
            var formData = document.forms["frmEmpAddress"];
            if ($('#chkSamePermanant').is(":checked")) {
                $empAddress.CopyAddress(formData);
            }
            else {
                $empAddress.CommAddrClear(formData);
            }
        });
        $('#frmEmpAddress').on('submit', function (event) {
            $empAddress.save();
            event.preventDefault();
        });
    },
    get: function (employee) {
        $empAddress.canSave = true;
        $empAddress.bindEvent();
        $empAddress.selectedEmployeeId = employee.id;
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpAddress",
            data: JSON.stringify({ employeeId: $empAddress.selectedEmployeeId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $empAddress.render(p);
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
        if (!$empAddress.canSave) {
            return false;
        }
        $empAddress.canSave = false;
        $app.showProgressModel();
        var data = new Array();
        var formData = $empAddress.formData;
        //Permenant address
        var permenant = {
            empAddid: $empAddress.selectedAddressId1,
            empAddEmployeeId: $empAddress.selectedEmployeeId,
            empAddressLine1: formData.elements["txtPerAddressline1"].value,
            empAddressLine2: formData.elements["txtPerAddressLine2"].value,
            empCity: formData.elements["txtPerCity"].value,
            empState: formData.elements["txtPerState"].value,
            empPinCode: formData.elements["txtPerPinCode"].value,
            empCountry: formData.elements["txtPerCountry"].value,
            empPhone: formData.elements["txtPerPhoneNumber"].value,
            empAddressType: 1
        };
        data.push(permenant);
        //communication address
        if ($('#chkSamePermanant').is(":checked")) {
            var communication = {
                empAddid: $empAddress.selectedAddressId2,
                empAddEmployeeId: $empAddress.selectedEmployeeId,
                empAddressLine1: formData.elements["txtComAddressline1"].value,
                empAddressLine2: formData.elements["txtComAddressLine2"].value,
                empCity: formData.elements["txtComCity"].value,
                empState: formData.elements["txtComState"].value,
                empPinCode: formData.elements["txtComPinCode"].value,
                empCountry: formData.elements["txtComCountry"].value,
                empPhone: formData.elements["txtComPhoneNumber"].value,
                empAddressType: 2
            };
            data.push(communication);
        }
        $.ajax({
            url: $app.baseUrl + "Employee/SaveEmpAddress",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $empAddress.canSave = true;
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        $empAddress.canSave = true;
                        break;
                }
            },
            complete: function () {
                $app.hideProgressModel();
                $empAddress.canSave = true;
            }
        });

    },
    AddInitialize: function () {
        $empAddress.selectedAddressId1 = '';
        $empAddress.selectedAddressId2 = '';
        var formData = $empAddress.formData;
        PerAddrClear(formData);
        CommAddrClear(formData);
    },
    PerAddrClear: function (formData) {
        //Permenant address
        formData.elements["txtPerAddressline1"].value = "";
        formData.elements["txtPerAddressLine2"].value = "";
        formData.elements["txtPerCity"].value = "";
        formData.elements["txtPerState"].value = "";
        formData.elements["txtPerPinCode"].value = "";
        formData.elements["txtPerCountry"].value = "";
        formData.elements["txtPerPhoneNumber"].value = "";
    },
    CommAddrClear: function (formData) {
        //communication address       
        formData.elements["txtComAddressline1"].value = "";
        formData.elements["txtComAddressLine2"].value = "";
        formData.elements["txtComCity"].value = "";
        formData.elements["txtComState"].value = "";
        formData.elements["txtComPinCode"].value = "";
        formData.elements["txtComCountry"].value = "";
        formData.elements["txtComPhoneNumber"].value = "";
    },
    CopyAddress: function (formDate) {
        //Copy address from Permanent
        $('#txtComAddressline1').val($('#txtPerAddressline1').val());
        $('#txtComAddressLine2').val($('#txtPerAddressLine2').val());
        $('#txtComCity').val($('#txtPerCity').val());
        $('#txtComState').val($('#txtPerState').val());
        $('#txtComPinCode').val($('#txtPerPinCode').val());
        $('#txtComCountry').val($('#txtPerCountry').val());
        $('#txtComPhoneNumber').val($('#txtPerPhoneNumber').val());



    },
    render: function (datas) {
        var formData = $empAddress.formData;
        // $comPopup.selectedId = ''
        if (datas.length == 0) {
            $empAddress.PerAddrClear(document.forms["frmEmpAddress"]);
            $empAddress.CommAddrClear(document.forms["frmEmpAddress"]);
        }
        for (var cnt = 0; cnt < datas.length; cnt++) {
            var data = datas[cnt];
            if (data.empAddressType == 1) {
                //Permenant address
                $empAddress.selectedAddressId1 = data.empAddid;
                formData.elements["txtPerAddressline1"].value = data.empAddressLine1;
                formData.elements["txtPerAddressLine2"].value = data.empAddressLine2;
                formData.elements["txtPerCity"].value = data.empCity;
                formData.elements["txtPerState"].value = data.empState;
                formData.elements["txtPerPinCode"].value = data.empPinCode;
                formData.elements["txtPerCountry"].value = data.empCountry;
                formData.elements["txtPerPhoneNumber"].value = data.empPhone;
            }
            if (data.empAddressType == 2) {
                //communication address
                $empAddress.selectedAddressId2 = data.empAddid;
                $('#chkSamePermanant').prop('checked', true);
                formData.elements["txtComAddressline1"].value = data.empAddressLine1;
                formData.elements["txtComAddressLine2"].value = data.empAddressLine2;
                formData.elements["txtComCity"].value = data.empCity;
                formData.elements["txtComState"].value = data.empState;
                formData.elements["txtComPinCode"].value = data.empPinCode;
                formData.elements["txtComCountry"].value = data.empCountry;
                formData.elements["txtComPhoneNumber"].value = data.empPhone;
            }
        }
    }
};;