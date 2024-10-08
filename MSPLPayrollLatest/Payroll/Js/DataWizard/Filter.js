﻿$PaySheetFilter = {
    dtFilter: null,
    masterFields: [],
    allowanceFields: [],
    deductionsFields: [],

    render: function () {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "DataWizard/GetMasterFields",
            data: JSON.stringify({
                "tableName": "\'Employee\',\'Emp_Address\',\'Emp_Bank\',\'Emp_Personal\',\'EntityAdditionalInfo\'",
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $PaySheetFilter.masterFields = jsonResult;
            }
        });
        this.dtFilter = $("#tblFilter").DataTable({
            "bPaginate": false,
            "bFilter": false,
            "bInfo": false,
            "bDestroy": true, "aoColumnDefs": [

            { "sClass": "hide_column", "aTargets": [3, 4] }
            ],
            fnDrawCallback: function () {
                $("#tblFilter").removeClass("sorting_asc");
                $('#tblFilter tbody').on('click', 'a .glyphicon-remove', function () {
                    $PaySheetFilter.dtFilter.row($(this).parents('tr')).remove().draw(false);
                });
                $('#tblFilter select[name="sltField"]').on('change', function () {
                    debugger;
                    type = $('input[name="Detail"]:checked').val();
                    var tr = $(this).closest('tr');
                    var field = $(tr).find("#sltField option:selected").html();
                    var jsonResult;
                    switch (type) {
                        case "Master":
                            jsonResult = $PaySheetFilter.masterFields;
                            break;
                        case "Earnings":
                            jsonResult = $PaySheetFilter.allowanceFields;
                            break;
                        case "Deductions":
                            jsonResult = $PaySheetFilter.deductionsFields;
                            break;
                    }
                    var type = '';

                    for (i = 0; i < jsonResult.length; i++) {

                        if (field == jsonResult[i].fieldName) {
                            $(tr).find('#sltddlst').html('');
                            $(tr).find('#sltoperation').attr('disabled', false);
                            $(tr).find('td:eq(3)').text(jsonResult[i].datatype);
                            var temp = field.split(" ");
                            if (temp[1] == "Category" || temp[1] == "BankName" || temp[1] == "CostCentre" || temp[1] == "Department"
                                || temp[1] == "Designation" || temp[1] == "ESILocation" || temp[1] == "PTLocation") {
                                $(tr).find('#sltoperation').val('=');
                                $(tr).find('#sltoperation').attr('disabled', true);
                                $.ajax({
                                    type: 'POST',
                                    url: $app.baseUrl + "Company/GetPopUpDatas",
                                    contentType: "application/json; charset=utf-8",
                                    data: JSON.stringify({ type: temp[1] }),
                                    dataType: "json",
                                    success: function (jsonResult) {
                                        debugger;
                                        $(tr).find('.dynamicdd').addClass('show').removeClass('hide');
                                        $(tr).find('#filterValue').addClass('hide').removeClass('show');
                                        var out = jsonResult.result;
                                        $(tr).find('#sltddlst').html('');
                                        $(tr).find('#sltddlst').append($("<option></option>").val('0').html('--Select--'));
                                        for (var i = 0; out.length > i; i++) {
                                            $(tr).find('#sltddlst').append($("<option value='" + out[i].popuplalue + "'>" + out[i].popuplalue + "</option>"));
                                        }
                                    },
                                    error: function (msg) {
                                    }
                                });
                            }
                            else {
                                $(tr).find('.dynamicdd').addClass('hide').removeClass('show');
                                $(tr).find('#filterValue').addClass('show').removeClass('hide');
                            }
                            break;
                        }
                    };

                });

            },
        });

    },
    addRow: function () {
        debugger;
        type = $('input[name="Detail"]:checked').val();

        var jsonResult = $PaySheetFilter.masterFields;
        var strOption = '<select id="sltField" name="sltField" class="filterField form-control"><option value=0>--Select--</option>';
       
        switch (type) {
            case "Master":

                for (var i = 0; jsonResult.length > i; i++) {
                    strOption = strOption + "<option value='" + jsonResult[i].tableName + "'>" + jsonResult[i].fieldName + "</option>";
                }
              
                break;
            case "Earnings":
                jsonResult = $PaySheetFilter.allowanceFields;
                for (var i = 0; jsonResult.length > i; i++) {
                    strOption = strOption + "<option value='" + jsonResult[i].fieldName + "'>" + jsonResult[i].displayAs + "</option>";
                }
                break;
            case "Deductions":
                jsonResult = $PaySheetFilter.deductionsFields;
                for (var i = 0; jsonResult.length > i; i++) {
                    strOption = strOption + "<option value='" + jsonResult[i].fieldName + "'>" + jsonResult[i].displayAs + "</option>";
                }
                break;
        }
        strOption = strOption + "</select>"
        strOperation = '<select id="sltoperation" class="operations form-control">';
        strOperation += '<option value="<"> < </option>';
        strOperation += '<option value=">"> >  </option>';
        strOperation += '<option value="="> = </option>';
        strOperation += '<option value="<="> <= </option>';
        strOperation += '<option value=">="> >= </option>';
        strOperation += '<option value="<>"> <> </option></select>';
        var strddlst = '<select id="sltddlst" class="filterValue dynamicdd form-control hide">';
     
        if (type == "Master") {
            $PaySheetFilter.dtFilter.row.add([strOption, strOperation, '<input type="text" id="filterValue"   class="filterValue form-control txtip" value="" />' + strddlst, '', type, '<a href="#" id="deletefilter" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>']).draw(false);
        }
        else {
            $PaySheetFilter.dtFilter.row.add([strOption, strOperation, '<input type="text" id="filterValue"  onkeyup="return $validator.moneyvalidation(this.id)" onkeypress="return $validator.moneyvalidation(this.id)" class="filterValue form-control" value="" />', '', type, '<a href="#" id="deletefilter" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>']).draw(false);
        }


    }

}
