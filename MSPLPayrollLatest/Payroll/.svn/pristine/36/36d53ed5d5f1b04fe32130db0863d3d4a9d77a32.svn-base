//$(document).ready(function () {
//    $increment.loadInitial();
//});
$("#sltCategorylist").change(function () {

    if ($("#sltCategorylist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#sltEmployeelist").val() = "00000000-0000-0000-0000-000000000000";
        $("#dvEmpCode").addClass('nodisp');
    }
    else {
        $companyCom.loadSelectiveEmployee({ id: 'sltEmployeelist', condi: 'Category.' + $("#sltCategorylist").val() });
        $("#dvEmpCode").removeClass('nodisp');
    }
});

$("#sltEmployeelist").change(function () {

    if ($("#sltEmployeelist").val() == "00000000-0000-0000-0000-000000000000") {
        $('#transHtml').html(null);
    }
    else {
        $increment.initiateForm($("#sltEmployeelist").val(), 'dvIncrement');
    }
});

$("#btnDeleteCat").click(function () {
    debugger;
    if ($("#sltCategorylist").val() == "00000000-0000-0000-0000-000000000000") {
        $app.showAlert("Please select category", 4);
    }
    else {
        if (confirm("Are you sure, do you want to Delete?")) {
            $increment.deleteIncrementCategory();
        }
       
    }

});



$increment = {
    MIlopdays: '',
    selectedEmployeeId: '',//E5F95082-E525-41E2-91CF-3887244EF41A
    selectedIncrementId: '00000000-0000-0000-0000-000000000000',
    tableId: 'tblIncrement',
    lastEntryId: '00000000-0000-0000-0000-000000000000',
    incrementTable: 'tblIncrementMaster',
    formData: document.forms["frmIncrement"],
    canSave: false,
    disableField: '',
    designIncrementMaster: function (renderDiv) {

        var htm = '<h4>Increment</h4> <div class="row"> <div id="dvIncrementAdd" class ="col-md-12 text-right"><input type="button" id="btnAddIncrement" value="Add" class="btn custom-button btnRight marginbt7" data-toggle="modal" data-target="#AddIncrement"><input type="button" id="btnDeleteCat" value="Delete All from This category" onclick="$increment.deleteIncrementCategory();" class="btn custom-button btnRight marginbt7" style="margin-right: 10px;"></div>'
            + '<div class="col-md-12 table-responsive" id="dvIncrements"></div>'
        + '<div id="AddIncrement" class="modal fade" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false"> <form id="frmIncrement"><div class="modal-dialog">'
        + '<div class="modal-content"><div class="modal-header"> <button type="button" class="close" data-dismiss="modal">×</button><h4 class="modal-title" id="H4">Add/Edit Increment</h4></div>'
        + '<div class="modal-body"><div class="col-md-12 scrol" id="transHtml"></div></div>'
        + '<div class="modal-footer"><button type="submit" id="btnincSave" class="btn custom-button">Save</button><button type="button" class="btn custom-button" data-dismiss="modal">Close</button></div></div></div>'
            + '</form></div></div>'
        $('#' + renderDiv).html(htm);//transHtml
    },
    loadIncrementMaster: function () {

        var gridObject = $increment.incrementMasterGridObject();
        var tableid = { id: $increment.incrementTable };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvIncrements').html(modelContent);
        var data = null;
        $increment.loadincrementMasterGrid(data, gridObject, tableid);
    },
    incrementMasterGridObject: function () {

        var gridObject = [
                { tableHeader: "Id", tableValue: "id", cssClass: 'nodisp' },
                { tableHeader: "Effective Date", tableValue: "effDate", cssClass: '' },
                { tableHeader: "Apply Month", tableValue: "month", cssClass: '' },
                { tableHeader: "Apply Year", tableValue: "year", cssClass: '' },
                 { tableHeader: "Status", tableValue: "status", cssClass: '' },
                { tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }
        ];
        return gridObject;
    },
    loadincrementMasterGrid: function (data, context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
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
                                    if (oData.status == 'Processed') {
                                        $increment.disableField = "disabled";
                                        $increment.canSave = false;
                                        // $app.showAlert('You can not Edit Processed Increment', 3);
                                    } else {
                                        $increment.disableField = '';
                                        $increment.canSave = true;
                                    }
                                    $increment.selectedIncrementId = oData.id;
                                    $increment.incrementPopUp();
                                    $('#AddIncrement').modal('toggle');

                                    return false;
                                });
                                c.button();
                                c.on('click', function () {
                                    if (oData.status == 'Processed') {
                                        $app.showAlert('You can not Delete Processed Increment', 3);
                                    } else {
                                        if (confirm('Aru you sure,do you want to delete this increment')) {
                                            $increment.selectedIncrementId = oData.id;
                                            $increment.deleteIncrement();
                                        }
                                    }
                                    return false;
                                });
                                $(nTd).empty();
                                $(nTd).prepend(b, c);
                            }
                        }

                    ); //for action column
            }
            else if (context[cnt].cssClass == 'edit') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="text" onkeyup="return $validator.moneyvalidation(this.id)" ' + $increment.disableField + ' onkeypress="return $validator.moneyvalidation(this.id)" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
                        $(nTd).html(b);
                    }
                });

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
                    url: $app.baseUrl + "Transaction/GetEmployeeIncrements",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ employeeId: $increment.selectedEmployeeId }),
                    dataType: "json",
                    success: function (jsonResult) {
                        var out = jsonResult.result.data;
                        // $increment.IncrementRenderData(Rdata);
                        // var out = jsonResult.result.incrementDatails;
                        if (jsonResult.result.lastEntry) {
                            $increment.lastEntryId = jsonResult.result.lastEntry.id;
                        } else {
                            $increment.lastEntryId = '00000000-0000-0000-0000-000000000000';

                        }
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
    designForm: function (renderDiv) {

        var formrH = '';//<div class="row">
        formrH = formrH + ' <div class="col-md-12">'; //first
        formrH = formrH + '<div class="form-horizontal">';//start horizatal div
        //Apply on
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Apply Month <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><select id="ddApplyMonth"  ' + $increment.disableField + ' class="form-control" placeholder="Enter the Apply Month" required ></select> </div>';
        formrH = formrH + '</div>';
        //Apply year
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Apply Year <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><select id="txtApplyYear"  class="form-control" autofocus required></select> </div>';
        formrH = formrH + '</div>';
        // effective date
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Effective Date <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><input type="text" id="txtIncrementEffectiveDate"  ' + $increment.disableField + ' class="form-control datepicker" placeholder="Enter the Effective Date" required /> </div>';
        formrH = formrH + '</div>';

        formrH = formrH + '<div id="dvEffectiveDate"> <div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Before LOP <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><input type="text" id="txtBeforeLop"  ' + $increment.disableField + ' class="form-control" onkeypress="return $validator.checkDecimal(event, 1)" oncopy="return false" onpaste="return false" placeholder="Enter the Before LOP days"  /> </div>';
        formrH = formrH + '</div>';

        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">After LOP <label style="color:red;font-size: 13px">*</label></label>';
        formrH = formrH + '<div class="col-md-6"><input type="text" id="txtAfterLop" class="form-control"  ' + $increment.disableField + ' onkeypress="return $validator.checkDecimal(event, 1)" oncopy="return false" onpaste="return false" placeholder="Enter the After LOP days"  /> </div>';
        formrH = formrH + '</div> </div>';

        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';
        //  formrH = formrH + '</div>';//row end

        //button row start
        /*
        formrH = formrH + '<div class="row">';
        formrH = formrH + '<div class="col-md-6">';
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<button type="submit" id="btnincSave" class="btn custom-button">Save</button>';
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';*/
        //button row end
        formrH = formrH + '<div class="col-md-12"><div id="dvTransTable"></div></div>';//for table
        formrH = formrH + '<div class="form-group">';
        formrH = formrH + '<label class="control-label col-md-4">Total Value </label>';
        formrH = formrH + '<div class="col-md-4"><label class="control-label col-md-4" id="lblCurrentValue"> </label> </div>';
        formrH = formrH + '<div class="col-md-4"><label class="control-label col-md-4" id="lblNewValue"> </label> </div>';
        formrH = formrH + '</div> </div>';

        //formH = formH + '<div class="col-sm-12" id="dvTotal"> <table id="tblTotal"> <tr><td>Total Value</td><td><label class="control-label col-md-4" id="lblCurrentValue"></label></td>';
        //formH = formH + ' <td> <label class="control-label col-md-4" id="lblNewValue"></label> </td> </tr></table><div>'
        // formrH = formrH + '</form>';//form end

        $('#' + renderDiv).html(formrH);//transHtml
        $increment.formData = document.forms["frmIncrement"];

    },
    initiateForm: function (employeeId, renderDiv) {

        $increment.selectedEmployeeId = employeeId;
        $increment.designIncrementMaster(renderDiv);
        $increment.loadIncMaster();
    },
    loadIncMaster: function () {

        $increment.loadIncrementMaster();
        $('#btnAddIncrement').on('click', function () {

            $increment.canSave = true;
            $increment.disableField = '';
            //  $increment.selectedEmployeeId = '00000000-0000-0000-0000-000000000000'
            $increment.selectedIncrementId = '00000000-0000-0000-0000-000000000000';
            $increment.incrementPopUp();

        });
    },
    incrementPopUp: function () {

        $increment.designForm('transHtml');
        $companyCom.loadYear({ id: "txtApplyYear" });
        $payroll.loadMonth({ id: 'ddApplyMonth' });
        $increment.loadComponent();
        $('#frmIncrement').on('submit', function (event) {
            if ($app.requiredValidate('btnincSave', event)) {
                $increment.save();
                return false;
            }
            else {
                return false;
            }
        });
        $("#txtIncrementEffectiveDate").change(function () {

            var CheckIncrementEffectiveDate = new Date($("#txtIncrementEffectiveDate").val());
            $increment.SetVisible(CheckIncrementEffectiveDate);
            $('#btnincSave').prop('disabled', false);
            //check with joining day of employee // created by Ajithpanner on 11/20/2017
            if (CheckIncrementEffectiveDate.getDate() >= 1) {
                $.ajax({
                    url: $app.baseUrl + "Transaction/GetEmployeeJoiningDate",
                    data: JSON.stringify({ employeeId: $increment.selectedEmployeeId, date: CheckIncrementEffectiveDate }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {
                        switch (jsonResult.Status) {
                            case true:
                                $app.showAlert(jsonResult.Message, 2);
                                break;
                            case false:
                                $('#btnincSave').prop('disabled', true);
                                $app.showAlert(jsonResult.Message, 4);
                                break;
                        }
                    }
                });

                //Get Lop days as per Selected effective of month
                if (CheckIncrementEffectiveDate.getDate() > 1) {
                    $.ajax({
                        url: $app.baseUrl + "Transaction/GetLopDays",
                        data: JSON.stringify({ employeeId: $increment.selectedEmployeeId, Month: CheckIncrementEffectiveDate.getMonth() + 1, Year: CheckIncrementEffectiveDate.getFullYear() }),
                        dataType: "json",
                        contentType: "application/json",
                        type: "POST",
                        success: function (jsonResult) {
                            switch (jsonResult.Status) {
                                case true:
                                    $increment.MIlopdays = jsonResult.result;

                                    $("#txtAfterLop").val($increment.MIlopdays);
                                    $("#txtBeforeLop").val(0);
                                    break;
                                case false:
                                    $('#btnincSave').prop('disabled', true);
                                    $app.showAlert(jsonResult.Message, 4);
                                    break;
                            }
                        }
                    });
                }
            }
        });
        $("#txtBeforeLop,#txtAfterLop").change(function (e) {
            $increment.CheckLopdays(e, $('#txtIncrementEffectiveDate').datepicker('getDate').getDate() - 1);
        });
        $("#txtBeforeLop,#txtAfterLop").blur(function (e) {
            $increment.CheckLopdays(e, $('#txtIncrementEffectiveDate').datepicker('getDate').getDate() - 1);
        });

        //$("#txtBeforeLop,#txtAfterLop").keyup(function (e) {
        //    $increment.CheckLopdays(e, $('#txtIncrementEffectiveDate').datepicker('getDate').getDate()-1);
        //});

        $payroll.initDatetime();
        $("#dvEffectiveDate").addClass('nodisp');

    },
    CheckLopdays: function (e, effday) {
        debugger;

        var EffectiveDay = parseFloat(effday);
        var BfLop = parseFloat("0.0");
        var AfLop = parseFloat("0.0");
        var MILop = parseFloat($increment.MIlopdays);
        if ($("#txtBeforeLop").val().length > 0)
            BfLop = parseFloat($("#txtBeforeLop").val());

        if ($("#txtAfterLop").val().length > 0)
            var AfLop = parseFloat($("#txtAfterLop").val());


       
        var min = Math.min(EffectiveDay, MILop);

        var x = new Array(2);
        for (var i = 0; i <2; i++) {
            x[i] = new Array(min+1);

        }

        for (var i = 0; i <= min ; i++) {
            x[0][i] = i;

        }
        var max = MILop + 1;
        for (var i = 0; i <= min ; i++) {
            max--
            x[1][i] = max;

        }

        if (EffectiveDay > MILop) {
            var ed = x[1];
            var af = x[0];
        }
        else if (MILop >= EffectiveDay) {
            var ed = x[0];
            var af = x[1];
        }

        if (e.target.id == "txtAfterLop") {
            if (af.indexOf(AfLop) != -1) {
                $("#txtBeforeLop").val(ed[af.indexOf(AfLop)]);
                $("#txtAfterLop").val(AfLop);
            }
            else {
                $("#txtBeforeLop").val(0);
                $("#txtAfterLop").val(MILop);
            }
        }
        if (e.target.id == "txtBeforeLop") {

            if (ed.indexOf(BfLop) != -1) {
                $("#txtBeforeLop").val(BfLop);
                $("#txtAfterLop").val(af[ed.indexOf(BfLop)]);
            }
            else {
                $("#txtBeforeLop").val(0);
                $("#txtAfterLop").val(MILop);
            }
        }





       

        //if (BfLop > MILop || BfLop > EffectiveDay)
        //    BfLop = 0.0;
        //if (AfLop > MILop)
        //    AfLop = 0.0;
        //if (e.target.id == "txtAfterLop") {
        //    var diff = parseFloat(MILop - AfLop)
        //    $("#txtBeforeLop").val(diff);
        //    $("#txtAfterLop").val(MILop - diff);
        //}
        //if (e.target.id == "txtBeforeLop") {

        //    var diff = parseFloat(MILop - BfLop)
        //    $("#txtAfterLop").val(diff);
        //    $("#txtBeforeLop").val(MILop - diff);
        //}

    },
    loadInitial: function () {
        $companyCom.loadCategory({ id: "sltCategorylist" });
        //$companyCom.loadEmployee({ id: "sltEmployeelist" });
        $("#dvEmpCode").addClass('nodisp');
    },
    loadComponent: function () {
        var gridObject = $increment.incrementGridObject();
        var tableid = { id: $increment.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvTransTable').html(modelContent);
        var data = null;
        $increment.loadincrementGrid(data, gridObject, tableid);
    },
    incrementGridObject: function () {

        var gridObject = [
                { tableHeader: "attributeModId", tableValue: "attributeModId", cssClass: 'nodisp' },
                { tableHeader: "Component Name", tableValue: "attrModelName", cssClass: '' },
                { tableHeader: "Current Value", tableValue: "currentVal", cssClass: '' },
                { tableHeader: "New Value", tableValue: "newVal", cssClass: 'edit' },
                //{ tableHeader: "Action", tableValue: null, cssClass: 'actionColumn' }
        ];
        return gridObject;
    },
    loadincrementGrid: function (data, context, tableId) {

        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {
            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass == 'nodisp') {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
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

                    ); //for action column

            }
            else if (context[cnt].cssClass == 'edit') {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        var b = $('<input type="text" onkeyup="return $validator.moneyvalidation(this.id)" ' + $increment.disableField + ' onkeypress="return $validator.moneyvalidation(this.id)" id="txtNewVal_' + oData.attributeModId + '" value="' + oData.newVal + '" />');
                        $(nTd).html(b);
                    }
                });

            }
            else {
                columnDef.push({ "aTargets": [cnt], "sClass": "word-wrap", "bSearchable": true });
            }
        }
        var newEntry = true;
        if ($increment.selectedIncrementId != '00000000-0000-0000-0000-000000000000') {
            newEntry = false;
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
                    url: $app.baseUrl + "Transaction/GetIncrement",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ employeeId: $increment.selectedEmployeeId, incrementId: $increment.selectedIncrementId != '00000000-0000-0000-0000-000000000000' ? $increment.selectedIncrementId : $increment.lastEntryId, isnew: newEntry }),
                    dataType: "json",
                    success: function (jsonResult) {
                        var Rdata = jsonResult.result;
                        $increment.IncrementRenderData(Rdata);
                        var out = jsonResult.result.incrementDatails;
                        setTimeout(function () {
                            callback({
                                draw: data.draw,
                                data: out,
                                recordsTotal: out.length,
                                recordsFiltered: out.length
                            });

                        }, 50);
                        switch (jsonResult.Status) {
                            case false:
                                $app.hideProgressModel();
                                $app.showAlert(jsonResult.Message, 4);
                                break;
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
    save: function () {
        debugger;
        if (!$increment.canSave) {
            return false;
        }
        $increment.canSave = false;
        $app.showProgressModel();
        var keyvalues = [];
        var RowsDateCheck = $('#' + $increment.tableId).dataTable().fnGetNodes();
        $(RowsDateCheck).each(function (index, data) {
            var cnt = 0;
            var curVal = 0;
            var newVal = 0;
            var attrId = '';
            $(data).find('td').each(function (ind, tmp) {
                if (cnt == 2) {
                    curVal = $(tmp).text();
                }
                else {
                    var input = $(tmp).find('input');
                    if (input != null) {
                        var id = $(input).prop('id');
                        if (id != null) {
                            id = id.replace('txtNewVal_', '');
                            attrId = id;
                            newVal = $(input).val();
                        }
                    }
                }
                cnt = cnt + 1;
            });
            keyvalues.push({ 'attributeModId': attrId, 'currentVal': curVal, 'newVal': newVal });

        });
        var formData = {

            'id': $increment.selectedIncrementId == '00000000-0000-0000-0000-000000000000' ? '' : $increment.selectedIncrementId,
            'employeeId': $increment.selectedEmployeeId,
            'month': $($increment.formData).find('#ddApplyMonth').val(),
            'year': $($increment.formData).find('#txtApplyYear').val(),
            'effDate': $($increment.formData).find('#txtIncrementEffectiveDate').val(),
            'beforeLop': $($increment.formData).find('#txtBeforeLop').val(),
            'afterLop': $($increment.formData).find('#txtAfterLop').val(),
            'incrementDatails': keyvalues
        };
        $.ajax({
            url: $app.baseUrl + "Transaction/SaveIncrement",
            data: JSON.stringify({ dataValue: formData }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $increment.loadIncMaster();
                        $('#AddIncrement').modal('toggle');
                        break;
                    case false:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {
                $increment.canSave = true;
                $app.hideProgressModel();
            }
        });
    },
    //Modified by Keerthika on 30/06/2017
    IncrementRenderData: function (data) {

        $increment.selectedEmployeeId = data.employeeId;
        var formData = document.forms["frmIncrement"];
        if ($increment.selectedEmployeeId == "00000000-0000-0000-0000-000000000000") {
            formData.elements["ddApplyMonth"].value = "";
            formData.elements["txtApplyYear"].value = "";
            formData.elements["txtIncrementEffectiveDate"].value = "";
            formData.elements["txtBeforeLop"].value = "";
            formData.elements["txtAfterLop"].value = "";
        }
        else {
            if (!$increment.canSave) {
              $("#btnincSave").addClass("nodisp");
            }
    else{
                $("#btnincSave").removeClass("nodisp");
        }
          
            formData.elements["ddApplyMonth"].value = data.month;
            formData.elements["txtApplyYear"].value = data.year;
            formData.elements["txtIncrementEffectiveDate"].value = data.effDate;
            var CheckIncrementEffectiveDate = new Date(data.effDate);
            $increment.SetVisible(CheckIncrementEffectiveDate);
            formData.elements["txtBeforeLop"].value = data.beforeLop;
            formData.elements["txtAfterLop"].value = data.afterLop;
            var cv = 0;
            var nv = 0;
            for (var i = 0; i < data.incrementDatails.length; i++) {
                cv = cv + data.incrementDatails[i].currentVal;
                $("#lblCurrentValue").text(cv);
                nv = nv + data.incrementDatails[i].newVal;
                $("#lblNewValue").text(nv);
            }
        }

    },
    SetVisible: function (selectedData) {
        if (selectedData.getDate() == 1) {
            $("#dvEffectiveDate").addClass('nodisp');
        }
        else {
            $("#dvEffectiveDate").removeClass('nodisp');
        }
    },
    deleteIncrement: function () {
        $app.showProgressModel();
        var formData = {
            'id': $increment.selectedIncrementId == '00000000-0000-0000-0000-000000000000' ? '' : $increment.selectedIncrementId,
            'employeeId': $increment.selectedEmployeeId
        };
        $.ajax({
            url: $app.baseUrl + "Transaction/DeleteIncrement",
            data: JSON.stringify({ dataValue: formData }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        $increment.loadIncMaster();
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
    deleteIncrementCategory: function () {
        debugger;
        if ($("#sltCategorylist").val() == "00000000-0000-0000-0000-000000000000") {
            $app.showAlert("Please select category", 4);
        }
        else {
            if (confirm("Are you sure, do you want to Delete?")) {
                $app.showProgressModel();

                $.ajax({
                    url: $app.baseUrl + "Transaction/DeleteCtegoryIncrements",
                    data: JSON.stringify({ categoryid: $("#sltCategorylist").val() }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {
                        switch (jsonResult.Status) {
                            case true:
                                $app.hideProgressModel();
                                $app.showAlert(jsonResult.Message, 2);
                                $increment.loadIncMaster();
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
            }
        }
     
    },
};