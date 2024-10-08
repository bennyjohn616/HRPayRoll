$("#sltRCategorylist").change(function () {
    if ($("#sltRCategorylist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#dvemp").addClass('nodisp');
        $("#dvdetails").addClass('nodisp');
    }
    else {
        $Release.SelectedCatId = $("#sltRCategorylist").val();
        $companyCom.loadSelectiveEmployee({ id: 'sltREmployeelist', condi: 'Category-Release.' + $("#sltRCategorylist").val() });
        $("#dvemp").removeClass('nodisp');
        $("#sltREmployeelist").val("00000000-0000-0000-0000-000000000000");
        $("#dvdetails").addClass('nodisp');
    }
});
$("#sltREmployeelist").change(function () {
    if ($("#sltREmployeelist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#dvdetails").addClass('nodisp');
    }
    else {
        $Release.SelectedCatId = $("#sltRCategorylist").val();
        $Release.ReleaseEmpId = $("#sltREmployeelist").val();
        $("#dvdetails").removeClass('nodisp');
        $Release.LoadRelease({ Id: $Release.ReleaseEmpId, CatId: $Release.SelectedCatId });
    }
});
var $Release = {
    ReleaseId: '',
    ReleaseEmpId: '',
    SelectedCatId: '',
    loadInitial: function () {
        $payroll.initDatetime();
        $companyCom.loadCategory({ id: "sltRCategorylist" });
        //$companyCom.loadEmployee({ id: "sltREmployeelist" });
        $Separation.loadTypeOfSep({ id: "sltTypeOfSeparation" });
        $("#dvemprelease").addClass('nodisp');
    },
    LoadRelease: function (context) {
        $.ajax({
            url: $app.baseUrl + "Employee/GetReleaseData",
            data: JSON.stringify({ RelCatid: context.CatId, RelEmpId: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $Release.RenderData(p);
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
        var formData = document.forms["Release"];
        $Release.SelectedCatId = data.SelectedCatId;
        $Release.ReleaseEmpId = data.ReleaseEmpId;
        if (data.SepType == null) {
            $('#sltTypeOfSeparation').prop('readonly', true);
            $('#txtSeparationDate').prop('readonly', true);
            $('#txtReasonforSeparation').prop('readonly', true);
            $('#txtReleaseDate').prop('readonly', true);            
        }
        formData.elements["sltTypeOfSeparation"].value = data.SepType == null ? 1 : data.SepType;
        formData.elements["txtSeparationDate"].value = data.SepDate;
        formData.elements["txtReasonforSeparation"].value = data.SepReason;
        formData.elements["txtReleaseDate"].value = data.RelDate;
        
       
      
    },
    save: function () {
        $app.showProgressModel();
        var formData = document.forms["Release"];
        var data = {
            RelCatid: formData.elements["sltRCategorylist"].value,
            RelEmpId: formData.elements["sltREmployeelist"].value,    
            RelDate: formData.elements["txtReleaseDate"].value,
        };
        $.ajax({
            url: $app.baseUrl + "Employee/SaveRelease",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        //$('#AddUser').modal('toggle');
                        //$User.LoadUser();
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
};