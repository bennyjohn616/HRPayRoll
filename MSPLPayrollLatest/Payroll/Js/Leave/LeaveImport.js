$("#btnTemplateLevOpen").on('click', function (event) {
    debugger;
    $Leaveimport.DownloadTemplate();
    return false;
});
$("#btnUploadLevOpenings").on('click', function (event) {
    debugger;
    $Leaveimport.uploadfile();
    return false;
});
$('#btnLeaveImport').on('click', function (e) {

    $Leaveimport.importProcess();
    return false;

});
$('#fUploadLevOpeningsLevOpenings').on('change', function (e) {
    debugger;
    var files = e.target.files;
    if (files.length > 0) {
        var file = this.files[0];
        fileName = file.name;
        var re = /\..+$/;
        var ext = fileName.match(re);
        $("#btnUploadLevOpenings").removeClass('hide');
        $("#btnUploadLevOpenings").hide();
        size = file.size;

        if (ext[0] == '.xls' || ext[0] == '.xlsx') {
            if (size > 5242880) {
                $('#fUpload').val('');
                $app.showAlert('Invalid file size, the maximum file size is 5 MB', 3);
                $Leaveimport.fileData = null;
                return false;
            }
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append(files[x].name, files[x]);
                }
                $Leaveimport.fileData = data;
                $("#btnUploadLevOpenings").show();
            }
            else {
                $('#fUpload').val('');
                $app.showAlert("This browser doesn't support HTML5 file uploads!", 3);
                $Leaveimport.fileData = null;
                return false;
            }
        }
        else {
            $('#fUpload').val('');
            $app.showAlert('Invalid file, please select another file', 3);
            $Leaveimport.fileData = null;
            return false;
        }
    }
});
$Leaveimport = {
    
    fileResponseData: null,
    fileData: null,
    filepath: null,
    LoadData: function () {
        $companyCom.loadLeaveType({ id: 'ddLeaveType' });
    },
    uploadfile: function () {
        debugger;
        $('#dvErrorMsg').html('');
        $.ajax({
            type: "POST",
            url: $app.baseUrl + 'Util/UploadFile',
            contentType: false,
            processData: false,
            data: $Leaveimport.fileData,
            success: function (result) {
                debugger;
                $Leaveimport.fileResponseData = result;
                $Leaveimport.filepath = result[0].Filepath;
                $("#btnLeaveImport").removeClass('hide');
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
                console.log(err);
            }
        });
    },
    DownloadTemplate: function () {
        debugger;
        $.ajax({
            type: "GET",
            url: $app.baseUrl + 'Util/DownLoadTemp',
            contentType: false,
            processData: false,
            data: null,
           
            success: function (JsonResult) {
                debugger;
                switch (JsonResult.Status) {
                    case true:
                        debugger;
                        var obj = new Object();
                        obj = JsonResult.result;
                        $app.downloadSync('Download/DownLevOpenTemp', obj);
                        $app.showAlert("Template Downloaded Successfully", 2);
                       
                        break;
                    case false:
                        debugger;
                        $app.showAlert("There is some Error occured While Downloading",4);
                        break;
                }
            },
        });
    },
    importProcess: function () {
        debugger;
        if ($("#ddLeaveType").val() == "00000000-0000-0000-0000-000000000000")
        {
            $app.showAlert("Plz select Leave Type", 4);
            return false;
        }
        else
        {
            $app.showProgressModel();
            $.ajax({
                url: $app.baseUrl + "Util/ImportLeaveData",
                data: JSON.stringify({ FilePath: $Leaveimport.filepath, LeaveType: $("#ddLeaveType").val() }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    debugger;
                    switch (jsonResult.Status) {
                        case true:
                            debugger;
                            if (jsonResult.length == 0) {
                                $app.showAlert(jsonResult.Message, 2);
                            }
                            $app.hideProgressModel();
                            $app.showAlert("Leave Imported Successfully", 2);
                            break;
                        case false:
                            debugger;
                            var errshow = "";
                            if (jsonResult.result.length != 0) {
                                for (i = 0; i <= jsonResult.result.length; i++)
                                {
                                    var err = jsonResult.result[i];
                                    var show = err;
                                    errshow = errshow + err + '\n';
                                }
                            }                           
                            alert(errshow);
                            $app.hideProgressModel();
                            break;
                    }
                },                
            });
        }
    },
    
};