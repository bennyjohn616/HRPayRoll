
$("#txtYear, #ddMonth").change(function () {
    $('#hdnType').val($lop.settingFor);
    if ($('#txtYear').val() != ''&& $('#hdnType').val() != "LOP") {
        
        $supplementary.applyMonth = $('#ddMonth').val();
        $supplementary.appplyYear = $('#txtYear').val();
        $supplementary.loadComponent();
        $('#hdnType').val() = "Supp";
    }
});
$supplementary = {
    tableId: 'tblsupplementary',
    settingFor: '',
    applyMonth: 0,
    appplyYear: 0,
    loadComponent: function () {

        var gridObject = $supplementary.loadTable();
        var tableid = { id: $supplementary.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvtbl').html(modelContent);
        var data = null;
        $supplementary.loadData(data, gridObject, tableid);
    },
    loadTable: function () {
        
        var gridObject = [];
        gridObject = [
           { tableHeader: "Id", tableValue: "id", cssClass: 'id nodisp' },
           { tableHeader: "Code", tableValue: "employeeId", cssClass: 'employeeId nodisp' },
           { tableHeader: "Code", tableValue: "empCode", cssClass: 'empCode' },
           { tableHeader: "Name", tableValue: "employeename", cssClass: 'employeename' },
           { tableHeader: "Date Of Joining", tableValue: 'dateofjoining', cssClass: 'dateofjoining' },
           { tableHeader: "month", tableValue: 'month', cssClass: 'month nodisp' },
           { tableHeader: "year", tableValue: 'year', cssClass: 'year nodisp' },
           { tableHeader: "Paid Days", tableValue: 'paidDays', cssClass: 'textbox txtpaidDays' },
           { tableHeader: "Lop Days", tableValue: 'lopDays', cssClass: 'textbox txtlopDays' }
        ];
        return gridObject;
    },
    loadData: function (data, context, tableId) {
        
        var columnsValue = [];
        var columnDef = [];
        for (var cnt = 0; cnt < context.length; cnt++) {

            columnsValue.push({ "data": context[cnt].tableValue });
            if (context[cnt].cssClass.includes( 'nodisp')) {
                columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass, "bSearchable": false }); //for id column
            }
            else if (context[cnt].cssClass.includes('checkbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        
                        var b = $('<input type="checkbox" class="chk"  onchange="$PaySheet.displayAttrOrder(this)" id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });

            } 
            else if (context[cnt].cssClass.includes('textbox')) {
                columnDef.push({
                    "aTargets": [cnt],
                    "sClass": "actionColumn " + context[cnt].cssClass,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        
                        var b = $('<input type="text" class="txtValue" value="'+sData+'"   onkeypress="return $validator.IsNumeric(event, this.id)"  id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass + "word-wrap", "bSearchable": true });
                } else {
                    columnDef.push({ "aTargets": "", "sClass": context[cnt].cssClass + "word-wrap", "bSearchable": true });
                }
            }

        }
        var dttable = $('#' + tableId.id).DataTable({
            'iDisplayLength': 10,
            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            //    'scrollY': '50vh',
            'scrollCollapse': true,
            columns: columnsValue,
            "aoColumnDefs": columnDef,
            //"aaData": data,
            ajax: function (data, callback, settings) {
                
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + "Transaction/getSupplementaryDays", //Get Attributemodel List for Field values
                    data: JSON.stringify({
                        dataValue: {
                            applyMonth: $supplementary.applyMonth,
                            applyYear: $supplementary.appplyYear
                        }
                    }),
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST",
                    success: function (jsonResult) {
                        
                        var out = jsonResult.result;
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

            fnDrawCallback: function () {
                $("#"+$supplementary.tableId).removeClass("sorting_asc");
                var api = this.api();            
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });
    },
    saveCredit: function () {
        var lopCredits = []; 
        var taxableFileds = $("#" + $supplementary.tableId).dataTable().fnGetNodes();
       
        for (i = 0; i < taxableFileds.length; i++)
        {
            if ($(taxableFileds[i]).find('.txtlopDays input').val() != '')
            {
                
                var other = new Object();
                other.applyMonth = $("#ddMonth").val();
                other.applyYear = $("#txtYear").val();
                other.employeeId = $(taxableFileds[i]).find(".employeeId").html();
                other.id = $(taxableFileds[i]).find(".id").html();
                other.month = $(taxableFileds[i]).find('.month').html();
                other.year = $(taxableFileds[i]).find('.year').html();
                other.paidDays = $(taxableFileds[i]).find('.txtpaidDays input').val()
                other.lopDays = $(taxableFileds[i]).find('.txtlopDays input').val();
                other.type = "Supp";
                lopCredits.push(other);
            }
        }
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Transaction/SaveCreditDays", //Get Attributemodel List for Field values
            data: JSON.stringify({
                dataValue: lopCredits
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
            
                $app.showAlert(jsonResult.Message, 2);
                 return false;
            }
        });
    },
}