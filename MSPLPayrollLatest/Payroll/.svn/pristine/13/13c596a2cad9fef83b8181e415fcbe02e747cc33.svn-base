$dwcategory = {
    tableId: 'tbldwCat',
    formData: document.forms["frmDwcategory"],
    designForm: function (renderDiv) {

        var formrH = '<form id="frmDwcategory">   <div class="row"> ';
        formrH = formrH + ' <div class="col-md-6">'; //first
        formrH = formrH + '<div class="form-horizontal">';//start horizatal divF
        //Categories
        //formrH = formrH + '<div class="form-group">';
        //formrH = formrH + '<label class="radio-inline"><input type="radio" id="rbtnIsCategoriesall" name="Categories" />All Categories</label>';
        //formrH = formrH + '<label class="radio-inline"><input type="radio" id="rbtnIsCategoriesparti"name="Categories"/>Particular Category</label>';
        //formrH = formrH + '</div>';

        formrH = formrH + '</div>';//close horizontal div
        formrH = formrH + '</div>';
        formrH = formrH + '</div>';//row end

        formrH = formrH + '<div class="col-md-12 table-responsive"><h4>Category Details</h4><div id="dvDwcategoryTable"></div></div>';//for table
        formrH = formrH + '</form>';//form end
        $('#' + renderDiv).html(formrH);//DwcategoryHtml
        $dwcategory.formData = document.forms["frmDwcategory"];
    },
    loadComponent: function () {
        
        var gridObject = $dwcategory.dwcategoryGridObject();
        var tableid = { id: $dwcategory.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvDwcategoryTable').html(modelContent);
        var data = null;
        $dwcategory.loadDwcategoryGrid(data, gridObject, tableid);
    },
    loadFinYrs: function () {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "FinanceYear/GetFinanceYears",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                debugger;
                var out = msg.result;
                console.log(out);
                $('#FinYears').html('');
                $('#FinYears').append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(out, function (index, finyr) {
                    $('#FinYears').append($("<option></option>").val(finyr.id).html(finyr.startDate + ' TO ' + finyr.EndDate));
                });
            },
            error: function (msg) {
            }
        });
    },
    dwcategoryGridObject: function () {
        var gridObject = [
                { tableHeader: "<input type='checkbox' id='cbCategory' onchange=$Payslipsetting.SelectAll('" + $dwcategory.tableId + "',this)>", tableValue: "Id", cssClass: 'checkbox' },
                { tableHeader: "Id", tableValue: "Id", cssClass: 'nodisp' },
                { tableHeader: "Category", tableValue: "Name", cssClass: '' },
        ];
        return gridObject;
    }
    ,
    loadDwcategoryGrid: function (data, context, tableId) {
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
                        var b = $('<input type="checkbox" class="cbCategory" id="' + sData + '"/>');
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
            "paging":   false,
            //"ordering": false,
            "info":     false,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Company/GetCategories",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
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
            //fnInitComplete: function (oSettings, json) {
            //    var r = $('#' + tableId.id + ' tfoot tr');
            //    r.find('th').each(function () {
            //        $(this).css('padding', 8);
            //    });
            //    $('#' + tableId.id + ' thead').append(r);
            //    $('#search_0').css('text-align', 'center');
            //},

            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
        
    },
    dwcategoryRenderData: function (data) {
        var formData = document.forms["frmDwcategory"];
       

    }
}