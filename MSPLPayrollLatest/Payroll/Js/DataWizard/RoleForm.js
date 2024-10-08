$Roleform = {
    EligibilityRole:null,
    tableId: 'TblRole',
    formData: document.forms["FrmRole"],
    designForm: function (renderDiv) {
        var formrH = '<form id="FrmRole">   <div class="row"> ';
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

        formrH = formrH + '<div class="col-md-12"><h4>Role Details</h4><div id="RoleTable"></div></div>';//for table
        formrH = formrH + '</form>';//form end
        $('#' + renderDiv).html(formrH);//DwcategoryHtml
        $Roleform.formData = document.forms["FrmRole"];
    },
    loadComponent: function () {
        debugger;
        var gridObject = $Roleform.RoleGridObject();
        var tableid = { id: $Roleform.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#RoleTable').html(modelContent);
        var data = null;
        $Roleform.RoleGrid(data, gridObject, tableid);
        
    },
    RoleGridObject: function () {
        var gridObject = [
                { tableHeader: "<input type='checkbox' id='cbRole' onchange=$ManagerEligiblity.SelectAll('" + $Roleform.tableId + "',this)>", tableValue: "RoleId", cssClass: 'checkbox' },
                { tableHeader: "Id", tableValue: "RoleId", cssClass: 'nodisp' },
                { tableHeader: "Roles", tableValue: "DisplayAs", cssClass: '' },
        ];
        return gridObject;
    }
    ,
    RoleGrid: function (data, context, tableId) {
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
                        debugger;
                        var elgdata = $Roleform.EligibilityRole;
                        var count =0;
                        if (elgdata.length>0 )
                        {
                            for(i=0;i<elgdata.length;i++)  
                            {
                                if (oData.RoleId == elgdata[i].RoleId)
                                {
                                    var b = $('<input type="checkbox" class="cbRole" id="' + sData + '" checked="checked"/>');
                                    count = count + 1;
                                }
                            }
                            if(count==0)
                            {
                                var b = $('<input type="checkbox" class="cbRole" id="' + sData + '"/>');
                            }
                        }
                        else
                        {
                            var b = $('<input type="checkbox" class="cbRole" id="' + sData + '"/>');
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
                    url: $app.baseUrl + "Admin/getCompanyrole",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    async: false,
                    dataType: "json",
                    success: function (jsonResult) {
                        debugger;
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
    dwcategoryRenderData: function (data) {
        var formData = document.forms["FrmRole"];
       

    }
}