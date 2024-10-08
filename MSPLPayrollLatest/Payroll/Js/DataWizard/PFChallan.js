$PFTemplate = {
    dtTemplate: null,
    masterFields: [],
    rowId: 0,
    renderTemplate: function () {

        this.dtTemplate = $("#tblPFTemplate").DataTable({
            "bPaginate": false,
            "bFilter": false,
            "bInfo": false,
            "aoColumnDefs": [{ "sClass": "hide_column", "aTargets": [0] }],
            "bDestroy": true, fnDrawCallback: function () {
                $("#tblPFTemplate").removeClass("sorting_asc");
                $('#tblPFTemplate tbody').on('click', 'a .glyphicon-remove', function () {
                    $PFTemplate.dtTemplate.row($(this).parents('tr')).remove().draw(false);
                });
                $('#tblPFTemplate tbody tr').on('click', function () {
                    $PFTemplate.rowId = $(this).find(":eq(0)").html();
                    $PFTemplate.get();
                });
                $('#tblPFTemplate tbody').on('click', 'a .glyphicon-remove', function () {
                    $PFTemplate.get();
                    $PFTemplate.Cancel();
                });
            }
        });
        $PFTemplate.get();
    },
    getColumns: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "PFChallan/GetColumns",
            dataType: "json",
            contentType: "application/json",
            async: false, //--
            data: JSON.stringify({ tableName: $('#sltTableName').val() }),

            success: function (jsonResult) {

                var out = jsonResult.result;
                $('#sltcolumnName').empty();
                $('#sltcolumnName').append($("<option value='0'>--Select--</option>"));
                for (var i = 0; out.length > i; i++) {
                    $('#sltcolumnName').append($("<option value='" + out[i].columnName + "'>" + out[i].displayAs + "</option>"));
                }
            }
        });
    },
    Save: function () {

        var data = new Object();
        data.id = $PFTemplate.rowId;
        data.tableName = $('#sltTableName').val();
        data.columnName = $('#sltcolumnName').val();
        data.fieldName = $('#sltcolumnName').val();
        data.displayAs = $('#txtDisplayAs').val();
        data.displayOrder = $('#txtDisplayOrder').val();
        $.ajax({

            type: 'POST',
            url: $app.baseUrl + "PFChallan/SavePFChallanTemplate",
            dataType: "json",
            contentType: "application/json",
            async: false,
            data: JSON.stringify({ dataValue: data }),

            success: function (jsonResult) {
                $PFTemplate.rowId = 0;
                // var out = jsonResult.result;
                //  $PFTemplate.get();
                //$('#tblPFTemplate').DataTable().ajax.reload();
                //if ($PFTemplate.rowId == 0) {
                //   for (var i = 0; out.length > i; i++) {
                //     $PFTemplate.dtTemplate.row.add([
                //          out.tableName,
                //       out.fieldName,
                //        out.displayAs,
                //      '<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" onClick="$PFTemplate.deleteTemplateRow(' + out.id + ')" class="glyphicon glyphicon-remove"></span></button>'])
                //     .draw(false);
                //  }
                // }
                $PFTemplate.get();
                $PFTemplate.Cancel();
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
        //$('#tblPFTemplate').DataTable().ajax.reload();
    }, // Modified by Keerthika  on 20/04/2017

    get: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "PFChallan/GetPFChallanTemplate",
            dataType: "json",

            contentType: "application/json",
            async: false,
            data: JSON.stringify({ id: $PFTemplate.rowId }),

            success: function (jsonResult) {

                var out = jsonResult.result;
                //---
                //   if (tableValue == "Salary" || tableValue == "SalaryBase") {
                // out[i].fieldName;
                // }
                // out.empty().append(jsonResult.result);
                if ($PFTemplate.rowId == 0) {

                    //---
                    $('#tblPFTemplate').dataTable().fnClearTable();
                    for (var i = 0; out.length > i; i++) {


                        $PFTemplate.dtTemplate.row.add([
                             out[i].id,
                             out[i].tableValue, //----
                             out[i].fieldName,
                             out[i].displayAs,
                              out[i].displayOrder,
                            '<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" onClick="$PFTemplate.deleteTemplateRow(' + out[i].id + ')" class="glyphicon glyphicon-remove"></span></a>'])
                            .draw(false);
                    }
                } else {
                    $('#sltTableName').val(out[0].tableName);
                    $PFTemplate.getColumns();
                    $('#sltcolumnName').val(out[0].columnName);
                    $('#txtDisplayAs').val(out[0].displayAs);
                    $('#txtDisplayOrder').val(out[0].displayOrder);
                }
            }
        });
    },
    deleteTemplateRow: function (id) {

        if (confirm('Are you sure,do you want to delete?')) {
            $.ajax({
                type: 'POST',
                url: $app.baseUrl + "PFChallan/DeleteRow",
                dataType: "json",
                contentType: "application/json",
                async: false,
                data: JSON.stringify({ id: id }),
                success: function (jsonResult) {
                }
            });
        }
        else {

            return false;

        }
    },
    GetPfChallan: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "PFChallan/GetPFChallan",
            dataType: "json",
            contentType: "application/json",
            async: false,
            data: JSON.stringify({ month: $('#sltMonth').val(), year: $('#sltYearPfChallan').val() }),

            success: function (jsonResult) {
                if (jsonResult.result.response != "")
                $app.showAlert(jsonResult.result.response+" Employees PAN No. empty", 1);
                var oData = new Object();
                oData.filePath = jsonResult.result.filepath;
                $app.downloadSync('Download/DownloadPaySlip', oData);
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },
    GetPfChallanXlsFormat: function () {
        $app.showProgressModel();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "PFChallan/GetPfChallanXlsFormat",
            dataType: "json",
            contentType: "application/json",
            async: false,
            data: JSON.stringify({ month: $('#sltMonth').val(), year: $('#sltYearPfChallan').val() }),
            
            success: function (jsonResult) {
                debugger;
                if (jsonResult.Status==true) {
                     var oData = new Object();
                oData.filePath = jsonResult.result.filepath;
                $app.downloadSync('Download/DownloadPaySlip', oData);
                }
                else
                {
                    $app.showAlert(jsonResult.result, 4);
                    $app.hideProgressModel();
                }
               
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
        $app.hideProgressModel();
    },
    GetESITemplateData: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "PFChallan/GetESIDataXlsFormat",
            dataType: "json",
            contentType: "application/json",
            async: false,
            data: JSON.stringify({ month: $('#sltMonth').val(), year: $('#sltYearPfChallan').val() }),

            success: function (jsonResult) {
                if (jsonResult.result.Message!="")
                $app.showAlert(jsonResult.result.Message + " Employees ESI No. empty", 1);
                var oData = new Object();
                oData.filePath = jsonResult.result.filepath;
                $app.downloadSync('Download/DownloadPaySlip', oData);
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },
    GetESIExtractData: function () {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "PFChallan/GetESIExtractData",
            dataType: "json",
            contentType: "application/json",
            async: false,
            data: JSON.stringify({ month: $('#sltMonth').val(), year: $('#sltYearPfChallan').val() }),

            success: function (jsonResult) {
                if (jsonResult.result.Message != "")
                    $app.showAlert(jsonResult.result.Message + " Employees ESI No. empty", 1);
                var oData = new Object();
                oData.filePath = jsonResult.result.filepath;
                $app.downloadSync('Download/DownloadPaySlip', oData);
            },
            complete: function () {
                $app.hideProgressModel();
            }
        });
    },
    Cancel: function () {
        $PFTemplate.rowId = 0;
        $('#sltTableName').val(0);
        $('#sltcolumnName').val(0);
        $('#txtDisplayAs').val('');
        $('#txtDisplayOrder').val('');
    }
}
