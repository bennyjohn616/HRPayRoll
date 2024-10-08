




var $languageKnown = {
    canSave: false,
    selectedEmployeeId: '',
    selectedLangugeKnownId: '',
    formData: document.forms["frmEmpLanguageKnown"],
    bindEvent: function () {
        $('#btnAddLanguageKnown').on('click', function () {
            $languageKnown.addInitialize();
        });
        $('#btnLangKnownSend').on('click', function () {
            $languageKnown.save();
        });
    },
    LoadlanguageKnowns: function (employee) {
        $languageKnown.bindEvent();
        $languageKnown.selectedEmployeeId = employee.id;
        var dtlnguageKnown = $('#tblLanguageKnown').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "empLangKnownid" },
                    { "data": "empLangKnownEmployeeId" },
                    { "data": "languages" },
                     { "data": "empLangKnownIsSpeak" },
                      { "data": "empLangKnownIsRead" },
                       { "data": "empLangKnownIsWrite" },

                    { "data": null }
            ],
            "aoColumnDefs": [
        {
            "aTargets": [0, 1],
            "sClass": "nodisp",
            "bSearchable": false
        },

         {
             "aTargets": [2, 3, 4, 5],
             "sClass": "word-wrap"
             //"fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
             //    if (sData.empLangKnownIsSpeak == true)
             //        return "Yes";
             //    else
             //        return "No";
             //    if (sData.empLangKnownIsRead == true)
             //        return "Yes";
             //    else
             //        return "No";
             //    if (sData.empLangKnownIsWrite == true)
             //        return "Yes";
             //    else
             //        return "No";
             //}
         },
{
    "aTargets": [6],
    "sClass": "actionColumn"
                    ,
    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
        var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
        b.button();
        b.on('click', function () {
            $languageKnown.render(oData);
            return false;
        });
        c.button();
        c.on('click', function () {
            if (confirm('Are you sure,do you want to delete?')) {
                $languageKnown.deleteData({ Id: oData.empLangKnownid, empId: $languageKnown.selectedEmployeeId, type: "languageknown" });
            }
            return false;
        });
        $(nTd).empty();
        $(nTd).prepend(b, c);


    }
}


            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetEmpLanguageKnowns",
                    contentType: "application/json; charset=utf-8",
                    data: "{'employeeId':'" + employee.id + "'}",
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
                var r = $('#tblLanguageKnown tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblLanguageKnown thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    addInitialize: function () {
        $languageKnown.canSave = true;
        $($languageKnown.formData).find('#ddEmpLanguageName').val(0);
        $($languageKnown.formData).find('#chkEmpLanguageSpeak').prop('checked', false);
        $($languageKnown.formData).find('#chkEmpLanguageRead').prop('checked', false);
        $($languageKnown.formData).find('#chkEmpLanguageWrite').prop('checked', false);
        $languageKnown.selectedLangugeKnownId = '';
    },
    render: function (data) {

        $languageKnown.canSave = true;
        $('#AddLanguageKnown').modal('toggle');
        $($languageKnown.formData).find('#ddEmpLanguageName').val(data.languageId);
        data.empLangKnownIsSpeak == true ? $($languageKnown.formData).find('#chkEmpLanguageSpeak').prop('checked', true) : $($languageKnown.formData).find('#chkEmpLanguageSpeak').prop('checked', false);
        data.empLangKnownIsRead == true ? $($languageKnown.formData).find('#chkEmpLanguageRead').prop('checked', true) : $($languageKnown.formData).find('#chkEmpLanguageRead').prop('checked', false);
        data.empLangKnownIsWrite == true ? $($languageKnown.formData).find('#chkEmpLanguageWrite').prop('checked', true) : $($languageKnown.formData).find('#chkEmpLanguageWrite').prop('checked', false);
        $languageKnown.selectedLangugeKnownId = data.empLangKnownid;
    },
    get: function () {
        $.ajax({
            url: $app.baseUrl + "Employee/GetEmpData",
            data: JSON.stringify({ id: $academic.seletedEmployeeId, type: "academic" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $languageKnown.render(p);
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
        var isSpeak = $($languageKnown.formData).find('#chkEmpLanguageSpeak').prop('checked');
        var isRead = $($languageKnown.formData).find('#chkEmpLanguageRead').prop('checked');
        var isWrite = $($languageKnown.formData).find('#chkEmpLanguageWrite').prop('checked');
        if ($('#ddEmpLanguageName').val() == "0") {
            $app.hideProgressModel();
            $app.showAlert("Please select language", 4);
            return false;
        }
        if (isSpeak || isRead || isWrite) {
            if (!$languageKnown.canSave) {
                return false;
            }

            $languageKnown.canSave = false;

            $app.showProgressModel();
            //var formData = document.forms["frmEmpLanguageKnown"];

            var data = {
                empLangKnownid: $languageKnown.selectedLangugeKnownId,
                empLangKnownEmployeeId: $languageKnown.selectedEmployeeId,
                empLangKnownIsSpeak: $($languageKnown.formData).find('#chkEmpLanguageSpeak').prop('checked'),
                empLangKnownIsRead: $($languageKnown.formData).find('#chkEmpLanguageRead').prop('checked'),
                empLangKnownIsWrite: $($languageKnown.formData).find('#chkEmpLanguageWrite').prop('checked'),
                language: { id: $($languageKnown.formData).find('#ddEmpLanguageName').val(), name: "" }

            };



            $.ajax({
                url: $app.baseUrl + "Employee/SaveEmpLanguage",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    var p = jsonResult.result;
                    switch (jsonResult.Status) {
                        case true:
                            $('#AddLanguageKnown').modal('toggle');
                            $languageKnown.LoadlanguageKnowns({ id: $languageKnown.selectedEmployeeId });
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 2);
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
        } else {
            $app.hideProgressModel();
            $app.showAlert("Please select atleast one!!", 4);
        }
    },
    deleteData: function (data) {
        $languageKnown.selectedLangugeKnownId = data.Id;
        $.ajax({
            url: $app.baseUrl + "Employee/DeleteEmpData",
            data: JSON.stringify({ id: data.Id, empId: data.empId, type: data.type }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $languageKnown.LoadlanguageKnowns({ id: $languageKnown.selectedEmployeeId });
                        $app.showAlert(jsonResult.Message, 2);
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
    loadEmpContractDetail: function (employee) {
        debugger;
        var dtlnguageKnown = $('#tblContrDetail').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                    { "data": "empLangKnownid" },
                    { "data": "empLangKnownEmployeeId" },
                    { "data": "languages" },
                    { "data": "empLangKnownIsSpeak" },
                    { "data": null }
            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp",
            "bSearchable": false
        },

         {
             "aTargets": [1,2,3],
             "sClass": "word-wrap"
             
         },
{
    "aTargets": [4],
    "sClass": "actionColumn"
                    ,
    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        var b = $('<a href="#" class="editeButtonEmpContr" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
        var c = $('<a href="#" class="deleteButtonEmpContr" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
        b.button();
        b.on('click', function () {
           
            return false;
        });
        c.button();
        c.on('click', function () {
            
            return false;
        });
        $(nTd).empty();
        $(nTd).prepend(b, c);


    }
}


            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Employee/GetEmployeeContractDetail",
                    contentType: "application/json; charset=utf-8",
                    data: "{'employeeId':'" + employee.id + "'}",
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
                var r = $('#tblContrDetail tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblContrDetail thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    }

};