$("#btnUploadLevOpenings").on('click', function (event) {
    
    $import.uploadfile();
    return false;
});
$('#fUploadLevOpenings').on('change', function (e) {
    
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
                $('#fUploadLevOpenings').val('');
                $app.showAlert('Invalid file size, the maximum file size is 5 MB', 3);
                $import.fileData = null;
                return false;
            }
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append(files[x].name, files[x]);
                }
                $import.fileData = data;
                $("#btnUploadLevOpenings").show();
            }
            else {
                $('#fUploadLevOpenings').val('');
                $app.showAlert("This browser doesn't support HTML5 file uploads!", 3);
                $import.fileData = null;
                return false;
            }
        }
        else {
            $('#fUploadLevOpenings').val('');
            $app.showAlert('Invalid file, please select another file', 3);
            $import.fileData = null;
            return false;
        }
    }
});


$LeaveOpeningsimport = {



}