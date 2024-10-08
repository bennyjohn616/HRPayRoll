
$('#txtStartDate').change(function () {
    
    var srtDate = new Date($('#txtStartDate').val());
    srtDate.setFullYear(srtDate.getFullYear() + 1);
    srtDate.setDate(srtDate.getDate() - 1);
    locale = "en-us";
    var dd = srtDate.getDate();
    var mm = srtDate.toLocaleString(locale, { month: "short" });
    var y = srtDate.getFullYear();
    var edDate = dd + '/' + mm + '/' + y;
    $('#txtEndDate').val(edDate);
});


var $financeYear = {

    financeYearId: "00000000-0000-0000-0000-000000000000",
    canSave: false,
    dttblCess: null,
    LoadfinanceYears: function () {
        $payroll.initDatetime();

        $companyCom.loadEmployee({ id: 'sltEmployee' });
        var dtClientList = $('#tblFinanceYear').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "id" },
                    { "data": "startDate" },
                    { "data": "EndDate" },
                       { "data": "empName" },
                        { "data": "place" },
                        { "data": "defaultyear" },
                       {
                           "data": null
                       }
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
          "aTargets": [6],
          "sClass": "actionColumn",

          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
              var b = $('<a href="#" class="editeButton" title="Edit"><span aria-hidden="true" class="glyphicon glyphicon-pencil"></span></button>');
              var c = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {
                  // $financeYear.GetFinanceYear(oData);
                  $financeYear.RenderData(oData);
                  $financeYear.canSave = true;
                  return false;
              });
              c.button();
              c.on('click', function () {
                  if (confirm('Are you sure ,do you want to delete?')) {
                      $financeYear.DeleteData(oData);
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
                    url: $app.baseUrl + "FinanceYear/GetFinanceYears",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        var out = jsonResult.result;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
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

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    }
        ,
    save: function () {
        
        if (!$financeYear.canSave) {
            return false;
        }
        $financeYear.canSave = false;
        $app.showProgressModel();
    /*   var formData = document.forms["frmFinancialYear"];


        var otherExemption = [];
        var cess = $("#tblCess").dataTable().fnGetNodes();
        for (i = 0; i < cess.length; i++) {


            var other = new Object();

            other.Id = $(cess[i]).find('.Id').html();
            other.Name = $(cess[i]).find('.Name').html();
            other.Value = $(cess[i]).find('.txtValue').val();
            other.IsDeleted = $(cess[i]).find('.txtValue').prop('disabled');
            other.Type = "Cess"
            otherExemption.push(other);


        }
        var HRA = $("#tblHRAExemption").dataTable().fnGetNodes();
        for (i = 0; i < HRA.length; i++) {


            var other = new Object();

            other.Id = $(HRA[i]).find('.Id').html();
            other.Name = $(HRA[i]).find('.Name').html();
            other.Value = $(HRA[i]).find('.txtValue').val();

            other.Type = "HRA"
            otherExemption.push(other);


        }
        var taxableFileds = $("#tblTaxable").dataTable().fnGetNodes();
        for (i = 0; i < taxableFileds.length; i++) {
            if ($(taxableFileds[i]).find(".chkBasic").prop('checked')) {
                var other = new Object();
                other.Id = $(taxableFileds[i]).find(".txtexampId").val();
                other.Name = $(taxableFileds[i]).find('.Id').html();
                other.Type = "Taxable"
                otherExemption.push(other);
            }
        }*/
        var data = {
            id: $financeYear.financeYearId,
            startDate: $('#txtStartDate').val(),
            EndDate: $('#txtEndDate').val(),
            tanNo: $('#txtTanNo').val(),
            tdsCircle: $('#txtTdsCircle').val(),
            panNo: $('#txtPANno').val(),
            taxDedAccNo: $('#txtTaxDedAccNo').val(),
            employeeId: $('#sltEmployee').val(),
            place: $('#txtPlace').val(),
            defaultyear: $('#chkDefault').is(':checked')
            //,otherExemption: otherExemption
        };
        $.ajax({
            url: $app.baseUrl + "FinanceYear/SaveFinanceYear",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddFinancialYear').modal('toggle');
                        $financeYear.LoadfinanceYears();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        var p = jsonResult.result;
                        companyid = 0;
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

    AddInitialize: function () {
        var formData = document.forms["frmFinancialYear"];
        $financeYear.financeYearId = '00000000-0000-0000-0000-000000000000';
        $('#txtStartDate').val(''),
        $('#txtEndDate').val(''),
        $('#txtTanNo').val(''),
        $('#txtTdsCircle').val(''),
        $('#txtPANno').val(''),
        $('#txtTaxDedAccNo').val(''),
        $('#sltEmployee').val('00000000-0000-0000-0000-000000000000'),
        $('#txtPlace').val(''),
        $financeYear.canSave = true;
     //   $financeYear.LoadHRA();
     //   $financeYear.LoadCess();
     //   $financeYear.LoadTaxable();
    },

    GetFinanceYear: function (context) {
        $.ajax({
            url: $app.baseUrl + "FinanceYear/GetFinanceYear",
            data: JSON.stringify({ id: context.Id, type: "financeYear" }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddfinanceYear').modal('toggle');
                        var p = jsonResult.result;
                        $financeYear.RenderData(p);
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
    DeleteData: function (context) {
        
        $.ajax({
            url: $app.baseUrl + "FinanceYear/DeleteFinanceYear",
            data: JSON.stringify({ Data:context }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $financeYear.LoadfinanceYears();
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

    RenderData: function (data) {

        $('#AddFinancialYear').modal('toggle');
        var formData = document.forms["frmFinancialYear"];
        $financeYear.financeYearId = data.id;
        formData.elements["txtStartDate"].value = data.startDate;
        formData.elements["txtEndDate"].value = data.EndDate;
        formData.elements["txtTanNo"].value = data.tanNo;
        formData.elements["txtTdsCircle"].value = data.tdsCircle;
        formData.elements["txtPANno"].value = data.panNo;
        formData.elements["txtTaxDedAccNo"].value = data.taxDedAccNo;
        formData.elements["sltEmployee"].value = data.employeeId;
        formData.elements["txtPlace"].value = data.place;
        $("#chkDefault").prop('checked', data.defaultyear);
    //    $financeYear.LoadHRA();
    //    $financeYear.LoadCess();
        //
        //var rowsotherExemption = $("#tblTaxable").dataTable().fnGetNodes();
        //for (var i = 0; i < rowsotherExemption.length; i++) {
        //    $(rowsotherExemption[i]).find(".chkBasic").prop("checked", false);
        //    
        //    for (var j = 0; j < data.otherExemption.length; j++) {

        //        if (data.otherExemption[j].Type == 'Taxable' && data.otherExemption[j].Name == $(rowsotherExemption[i]).find('.Id').html()) {

        //            $(rowsotherExemption[i]).find(".chkBasic").prop("checked", true);
        //            $(rowsotherExemption[i]).find(".txtexampId").val(data.otherExemption[j].Id);
        //        }
        //    }
        //}

    },
    LoadHRA: function () {

        dttblHRAExemption = $('#tblHRAExemption').DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            // 'sPaginationType': 'full',
            //  'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "Id" },
                    { "data": "Name" },
                    { "data": "Value" },


            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "nodisp Id",
            "bSearchable": false
        }, {
            "aTargets": [1],
            "sClass": "word-wrap Name",
            "bSearchable": false
        }, {
            "aTargets": [2],
            "sClass": "actionColumn Value",
            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                var b = $('<input type="text"  id="' + iRow + '" class="txtValue" onkeypress="return $validator.IsNumeric(event, this.id)"   value="' + sData + '"/>');
                $(nTd).html(b);
            }
        },

            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "FinanceYear/GetOtherExamption",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ financeYearId: $financeYear.financeYearId, type: 'HRA' }),
                    dataType: "json",
                    async: false,
                    success: function (jsonResult) {
                        var out = jsonResult.result;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                setTimeout(function () {
                                    callback({
                                        draw: data.draw,
                                        data: out,
                                        // recordsTotal: out.length,
                                        // recordsFiltered: out.length
                                    });

                                }, 50);
                                break;
                            case false:

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                $('#tblHRAExemption').find('thead').remove();
                $('#frmFinancialYear .dataTables_info').remove();
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    LoadCess: function () {

        $financeYear.dttblCess = $('#tblCess').DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            // 'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "Id" },
                    { "data": "Name" },
                    { "data": "Value" },
                     { "data": 'Action' }

            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "Id nodisp",
            "bSearchable": false
        }, {
            "aTargets": [1],
            "sClass": "word-wrap Name",
            "bSearchable": false
        }, {
            "aTargets": [2],
            "sClass": "actionColumn Value",
            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                var b = $('<input type="text" id="' + iRow + '" class="txtValue" onkeypress="return $validator.IsNumeric(event, this.id)"   value="' + sData + '"/>');
                $(nTd).html(b);
            }

        }, {
            "aTargets": [3],
            "sClass": "actionColumn ",
            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                var b = $('<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
                b.button();
                $(nTd).empty();
                $(nTd).prepend(b);
                b.on('click', function () {
                    if (confirm('Are you sure ,do you want do delete?')) {
                        var tr = $(this).closest('tr');
                        $(tr).find('.txtValue').prop('disabled', true);

                        return false;
                    }
                });
            }
        },

            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "FinanceYear/GetOtherExamption",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ financeYearId: $financeYear.financeYearId, type: 'Cess' }),
                    dataType: "json",
                    async: false,
                    success: function (jsonResult) {
                        var out = jsonResult.result;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
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

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                $('#frmFinancialYear .dataTables_info').remove();
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    LoadTaxable: function () {

        dttblTaxable = $('#tblTaxable').DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            // 'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "Id" },
                    { "data": "Name" },
                    { "data": null }

            ],
            "aoColumnDefs": [
        {
            "aTargets": [0],
            "sClass": "actionColumn Id",
            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                var b = $('<input type="checkbox" class="chkBasic"/>');
                $(nTd).html(b);
            }

        }, {
            "aTargets": [2],
            "sClass": "actionColumn exampId",
            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                var b = $('<input type="text" class="txtexampId"/>');
                $(nTd).html(b);
            }
        }

            ],
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "FinanceYear/GetTaxable",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ financeYearid: null }),
                    dataType: "json",
                    async: false,
                    success: function (jsonResult) {
                        var out = jsonResult.result;
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
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

                                break;
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                $('#tblTaxable').find('thead').remove();
                $('#frmFinancialYear .dataTables_info').remove();
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    addCess: function () {
        var cessName = $('#txtCessName').val();
        var cessValue = $('#txtCessPercentage').val();
        $financeYear.dttblCess.row.add({ Id: 0, Name: cessName, Value: cessValue, Action: '<a href="#" class="addButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>' }).draw(false);
        $('#txtCessName').val('');
        $('#txtCessPercentage').val('');
    }
};