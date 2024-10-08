//$("#sltSetting").change(function () {

//    if ($("#sltSetting option:selected").text() == "Employee Code Segment")
//    {
//        if($("input[id$='_PrefixEmpCode']").val().trim()!="")
//            $('#btnSave').prop('disabled', true);
//    }
//    else {
//        $('#btnSave').prop('disabled', false);
//    }

//});


$Empcodesetting = {
    canSave: false,
    settingId: '',
    currVal:null,
    HeadertableId: 'tblEmpCodeHeader',
    DetailtableId: 'tblEmpCodeDetail',
    headerformData: document.forms["frmEmpCodeheader"],
    detailformData: document.forms["frmEmpCodedetails"],
    CatloadData: '',
    loadInitial: function () {
        $Empcodesetting.headerdesignForm('dvEmpcodesettingHtml');
        $Empcodesetting.loadHeaderComponent();
    },
    AddInitialize: function () {
        $Empcodesetting.settingId = '';
        $Empcodesetting.loadpopUp()
        $('#dvcatname').addClass('nodisp');
        // var formData = document.forms["frmEmpCodedetails"];
        // $Empcodesetting.settingId = '';
        //formData.elements["sltddLoan"].value = "0";       

    },
    SetEmpCodeAutoMan: function () {

        $.ajax({
            url: $app.baseUrl + "Setting/SaveEmpcodeAutoManual",
            data: JSON.stringify({ AutoManual: $('#rbtnIsAutometic').prop('checked') == true ? 1 : 2 }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $app.showAlert(jsonResult.Message, 2);
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {
            }
        });
    },
    GetCodeAutoMan: function () {

        $.ajax({
            url: $app.baseUrl + "Setting/GetcodeAutoManual",
            data: null,
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                (jsonResult.result == '2') ? $("#rbtnIsManual").prop('checked', 'true') : $("#rbtnIsAutometic").prop('checked', 'true');
            },
            complete: function () {
            }
        });
    },
    loadpopUp: function () {

        $Empcodesetting.designDetailForm('dvEmpcodeHtml');
        $Empcodesetting.loadDetailComponent();
    },
    headerdesignForm: function (renderDiv) {

        var formrH = '<div> ';
        var formrH = '<div class="col-md-12 text-right" id="dvEmpCodeSeetingAdd"> ';
        formrH = formrH + '<div class="col-md-7">';
        formrH = formrH + '<div class="col-md-5">';
        formrH = formrH + '<input type="radio" name="txtIsAutoManual" id="rbtnIsAutometic" value="1" style="width: 30px;" />Autometic';
        formrH = formrH + '<input type="radio" name="txtIsAutoManual" id="rbtnIsManual" value="2" style="width: 30px;" />Manual';
        formrH = formrH + '</div>';
        formrH = formrH + '<div class="col-md-2">';
        formrH = formrH + '<input type="button" id="btnAddAutoManual" value="Save" class="btn custom-button marginbt7" onclick="$Empcodesetting.SetEmpCodeAutoMan();">'
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';
        formrH = formrH + '<div class="col-md-5">';
        formrH = formrH + '<input type="button" id="btnAddmpCodeSeeting" value="Add" class="btn custom-button marginbt7" data-toggle="modal" data-target="#EmpCodeSetting" onclick="$Empcodesetting.AddInitialize();" >'
        formrH = formrH + '</div>';
        formrH = formrH + '<div class="col-md-12"><div id="dvEmpcodesettingHeaderTable"></div></div>';//for table

        $('#' + renderDiv).html(formrH);//dvEmpcodeHeaderHtml
        $Empcodesetting.headerformData = document.forms["frmEmpCodeheader"];
    },
    designDetailForm: function (renderDiv) {

        var formrH = '<form id="frmEmpCodedetails" data-toggle="validator" role="form" action="javascript:$Empcodesetting.save()">';
        formrH = formrH + '<div class="modal-dialog">';
        formrH = formrH + '<div class="modal-content">'
        formrH = formrH + '<div class="modal-header">'
        formrH = formrH + ' <button type="button" class="close" data-dismiss="modal">&times;</button>'
        formrH = formrH + ' <h4 class="modal-title" id="H4">'
        formrH = formrH + 'Add/Edit Employee Code Setting'
        formrH = formrH + '</h4>'
        formrH = formrH + '</div>';//model headr closed
        formrH = formrH + '<div class="modal-body">'; //first

        formrH = formrH + '<div class="form-horizontal">';//start horizatal div
        //Name
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Name <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><input type="text" id="txtEmpStruct" class="form-control" placeholder="Enter the Name" required /></div>';
        formrH = formrH + '</div>';

        //PreFix
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">PreFix <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><input type="text" id="txtPreFix" class="form-control" placeholder="Enter the PreFix" required /></div>';
        formrH = formrH + '</div>';

        //Start Number
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Start Number <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><input type="text" id="txtSNum" onkeypress="return $validator.IsNumeric(event, this.id)" oncopy="return false" onpaste="return false" class="form-control" placeholder="Enter the Number" required /></div>';
        formrH = formrH + '</div>';

        //Category Name
        formrH = formrH + '<div class="form-group" id="dvcatname">';
        formrH = formrH + '<label class="control-label col-md-4">Selected Category</label>';
        formrH = formrH + '<div class="col-md-6"><a class="" href="#" id="hlnkcategory"></a></div>';
        formrH = formrH + '</div>';

        formrH = formrH + '</div>';//close horizontal div
        //Category
        formrH = formrH + '<div class="col-md-12"><div id="dvEmpcodesettingTable"></div></div>';//for table
        formrH = formrH + '</div>';//model body close

        formrH = formrH + '<div class="modal-footer">';
        formrH = formrH + '<button type="submit" id="btnSave" class="btn custom-button"> Save</button>';
        formrH = formrH + '<button type="button" class="btn custom-button" data-dismiss="modal">Close </button>';
        formrH = formrH + '</div>';//model footer close
        formrH = formrH + '</form>';//form end
        $('#' + renderDiv).html(formrH);//EmpCodedetailsHtml
        $Empcodesetting.detailformData = document.forms["frmEmpCodedetails"];
        $("#txtSNum").change(function () {
            debugger;
            if ($Empcodesetting.currVal >= $(this).val()) {
                $(this).val($Empcodesetting.currVal);
                $app.showAlert("Invalid Start Number ", 3);
            }
        })
    },
    loadHeaderComponent: function () {

        var gridObject = $Empcodesetting.HeaderEmpcodesettingGridObject();
        var tableid = { id: $Empcodesetting.HeadertableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvEmpcodesettingHeaderTable').html(modelContent);
        var data = null;
        $Empcodesetting.loadEmpcodesettingheaderGrid(data, gridObject, tableid);
    },
    HeaderEmpcodesettingGridObject: function () {

        var gridObject = [
                { tableHeader: "Name", tableValue: "Name", cssClass: '' },
                { tableHeader: "Prefix", tableValue: "PreFix", cssClass: '' },
                { tableHeader: "Next Number", tableValue: "SNumber", cssClass: '' },
                { tableHeader: "Category", tableValue: "CategoryName", cssClass: '' },
                { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }
        ];
        return gridObject;
    },
    loadEmpcodesettingheaderGrid: function (data, context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'checkbox') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="checkbox" id="chkbCategory"/>');
                        $(nTd).html(b);
                    }
                });

            }
            else if (context[cnt].cssClass == 'actionColumn') {
                columnDef.push(
                        {
                            "aTargets": [cnt],
                            "sClass": "actionColumn",
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
                                var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                                b.button();
                                b.on('click', function () {
                                    debugger;
                                    $Empcodesetting.currVal = oData.SNumber;
                                    $Empcodesetting.settingId = oData.Name;
                                    $Empcodesetting.loadpopUp();
                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    if (confirm('Are you sure ,do you want to delete?')) {
                                        $Empcodesetting.settingId = oData.Name;
                                        $Empcodesetting.Delete();
                                    }
                                    return false;
                                });
                                $(nTd).empty();
                                $(nTd).prepend(b, c);
                            }
                        }

                    ); //for action column
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
                    url: $app.baseUrl + "Setting/GetEmpcodeSetting",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        $Empcodesetting.GetCodeAutoMan();
                        var Rdata = jsonResult.result;
                        var out = Rdata;
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
    loadDetailComponent: function () {
        debugger;
        var gridObject = $Empcodesetting.DetailEmpcodesettingGridObject();
        var tableid = { id: $Empcodesetting.DetailtableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvEmpcodesettingTable').html(modelContent);

        $("#txtEmpStruct").change(function () {

            $("#tblEmpCodeHeader tbody tr").each(function () {

                if ($("#txtEmpStruct").val().toLowerCase() == $(this).find("td:nth-child(1)").html().toLowerCase()) {
                    if ($Empcodesetting.settingId.toLocaleLowerCase() != $(this).find("td:nth-child(1)").html().toLowerCase()) {
                        $app.showAlert("Already Exist " + $("#txtEmpStruct").val(), 4);
                        $("#txtEmpStruct").val('');
                        return false;
                    }
                }
            });
        });


        var data = null;
        $Empcodesetting.loadEmpcodesettingdetailGrid(data, gridObject, tableid);
    },
    DetailEmpcodesettingGridObject: function () {

        var gridObject = [
             { tableHeader: "Id", tableValue: "Id", cssClass: 'nodisp' },
             { tableHeader: '<input type="checkbox" id="CategoryAll" />', tableValue: 'Id', cssClass: 'checkbox' },
             { tableHeader: "Category", tableValue: "Name", cssClass: '' },
        ];
        return gridObject;
    },
    loadEmpcodesettingdetailGrid: function (data, context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass == 'checkbox') {
                columnDef.push({
                    "aTargets": [cnt],
                    //  "sClass": "checkbox",
                    "bSearchable": false,
                    //"bSort": false,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        debugger;
                        var b = "";
                        var test = [];
                        for (i = 0; i < $Empcodesetting.CatloadData.length; i++) {
                            test.push($Empcodesetting.CatloadData[i].catId);
                           
                        }
                        if (test.indexOf(sData) != -1) {
                            b = $('<input type="checkbox" id="chkbCategory" class="checkbox" checked>');
                        }
                        else {
                            b = $('<input type="checkbox" id="chkbCategory" class="checkbox">');
                        }
                      
                        $(nTd).html(b);
                    }
                });

            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var dtClientList = $('#' + tableId.id).DataTable({
            //'iDisplayLength': 10,
            'bPaginate': false,
            // 'sPaginationType': 'full',
            //'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Setting/GetEmpCodeSettingDetail",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ SettingName: $Empcodesetting.settingId }),
                    dataType: "json",
                    success: function (jsonResult) {
                        var Rdata = jsonResult.result;

                        var tmp = Rdata[0];
                        var out = tmp;
                        if ($Empcodesetting.settingId != '') {
                            $('#EmpCodeSetting').modal('toggle');
                            $Empcodesetting.RenderDetail(Rdata[1], Rdata[3]);
                            $('#dvEmpcodesettingTable').addClass('nodisp');
                            $Empcodesetting.CatloadData = Rdata[2];
                            $('#dvcatname').removeClass('nodisp');
                        }
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out//,
                                //recordsTotal: out.length,
                                //recordsFiltered: out.length
                            });

                        }, 50);

                        //if ($Empcodesetting.settingId != '') {
                        //    $Empcodesetting.RenderCategory(Rdata[2]);
                        //}
                    },
                    error: function (msg) {
                    },

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

        $('#CategoryAll').on('change', function () {
            $Empcodesetting.SelectAllchkbox($Empcodesetting.DetailtableId, "CategoryAll", "chkbCategory");
        });
        $('#hlnkcategory').on('click', function () {
            $('#dvEmpcodesettingTable').removeClass('nodisp');
            $Empcodesetting.RenderCategory($Empcodesetting.CatloadData);
            $('#dvcatname').addClass('nodisp');
        });




    },
    SelectAllchkbox: function (tblid, Headchkid, Childchkid) {
        $("#" + tblid + " tbody tr td").each(function () {
            $(this).find("#" + Childchkid).prop('checked', $('#' + Headchkid).is(":checked") == true ? true : false)
        });
    },
    save: function () {
        debugger;
        $Empcodesetting.canSave = false;
        $app.showProgressModel();
        var formData = $Empcodesetting.detailformData;

        var Categories = [];
        $("#" + $Empcodesetting.DetailtableId + " tbody tr").each(function () {
            if ($(this).find('#chkbCategory').is(":checked") == true) {
                Categories.push({ catId: $(this).find("td:nth-child(1)").html() })
            }
        });
        var data = {
            SidId: $Empcodesetting.settingId,
            SName: formData.elements["txtEmpStruct"].value,
            SPrefix: formData.elements["txtPreFix"].value,
            SlNo: formData.elements["txtSNum"].value,
            jsonCategories: Categories
        };

        $.ajax({
            url: $app.baseUrl + "Setting/SaveEmpcodeSetting",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $Empcodesetting.canSave = true;
                        var p = jsonResult.result;
                        $('#EmpCodeSetting').modal('toggle');
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $Empcodesetting.loadHeaderComponent();
                        break;
                    case false:
                        $Empcodesetting.canSave = true;
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {
                $Empcodesetting.canSave = true;
            }
        });
        $app.hideProgressModel();
    }, Delete: function () {

        $.ajax({
            url: $app.baseUrl + "Setting/DeleteEmpcodeSetting",
            data: JSON.stringify({ SettingName: $Empcodesetting.settingId }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $Empcodesetting.loadInitial();
                        $app.showAlert(jsonResult.Message, 2);
                        //alert(jsonResult.Message);
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
    RenderDetail: function (data, category) {

        var formData = $Empcodesetting.detailformData;
        formData.elements["txtEmpStruct"].value = data[0].SName;
        formData.elements["txtPreFix"].value = data[0].SPrefix;
        formData.elements["txtSNum"].value = data[0].SlNo;
        $("#hlnkcategory").text(category.trim());
    },
    RenderCategory: function (catdata) {

        $("#" + $Empcodesetting.DetailtableId + " tbody tr").each(function () {
            $(this).find("#chkbCategory").prop('checked', false)
        });

        $(catdata).each(function (i, obj) {
            var $this = $(this);
            $("#" + $Empcodesetting.DetailtableId + " tbody tr").each(function () {
                if ($(this).find("td:nth-child(1)").html() == $this[0].catId) {
                    $(this).find("#chkbCategory").prop('checked', true);
                    return false;
                }
            });

        });

    }

};