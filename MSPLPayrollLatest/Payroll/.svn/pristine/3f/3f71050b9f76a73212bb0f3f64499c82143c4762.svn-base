$("#btnLopSave").click(function () {
    

    if ($lop.settingFor == "LOP") {

        var taxableFileds = $("#" + $lop.tableId).dataTable().fnGetNodes();
    }
    else {
        var taxableFileds = $("#" + $supplementary.tableId).dataTable().fnGetNodes();
    }
    if (taxableFileds.length > 0) 
    {
                $lop.frmSubmit();
    }
    else { $app.showAlert("There is no processed data", 4) }
    });

$("#sltCategorylist").change(function () {
    if ($("#sltCategorylist").val() == "00000000-0000-0000-0000-000000000000") {
        $("#dvemp").addClass('nodisp');
        $("#dvdetails").addClass('nodisp');
    }
    else {
        $("#sltCategorylist").val();
        $companyCom.loadSelectiveEmployee({ id: 'sltEmployeelist', condi: 'Category.' + $("#sltCategorylist").val() });
        $("#dvemp").removeClass('nodisp');
        $("#sltEmployeelist").val("00000000-0000-0000-0000-000000000000");

    }
});






$("#sltEmployeelist,#txtYear, #ddMonth").change(function () {
    
    $('#hdnType').val($lop.settingFor);
    if ($('#txtYear').val() != '' && $('#hdnType').val() == "LOP" && $('#sltEmployeelist').val() != '00000000-0000-0000-0000-000000000000')
    {
        $lop.applyMonth = $('#ddMonth').val();
        $lop.appplyYear = $('#txtYear').val();
        $lop.loadComponent();
       
    }
});
$lop = {
    tableId: 'tbllopCredit',
    settingFor: '',
    applyMonth: 0,
    appplyYear: 0,
    loadComponent: function () {
        
        var gridObject = $lop.loadTable();
        var tableid = { id: $lop.tableId };
        var modelContent = $screen.createTable(tableid, gridObject);
        $('#dvtbl').html(modelContent);
        var data = null;
        $lop.loadData(data, gridObject, tableid);
    },
    loadTable: function () {
        
        var gridObject = [];
        gridObject = [
             { tableHeader: "id", tableValue: "id", cssClass: 'id nodisp' },
                { tableHeader: "Month", tableValue: "month", cssClass: 'month' },
                { tableHeader: "Year", tableValue: "year", cssClass: 'year' },
                { tableHeader: "Lop Days", tableValue: 'lopDays', cssClass: 'lopDays' },
                { tableHeader: "Lop Credit Days", tableValue: 'paidDays', cssClass: 'textbox txtlopCreditDays' },
               // { tableHeader: "Balance Lop Credit Days", tableValue: 'balanceDays', cssClass: 'balanceDays' }
        ];
        return gridObject;
    },
    loadData: function (data, context, tableId) {
        
        $lop.applyMonth = $('#ddMonth').val();
        $lop.appplyYear = $('#txtYear').val();
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
                        var b = $('<input type="text" class="txtValue" value="' + sData + '"   onkeypress="return $validator.IsNumeric(event, this.id)" onblur="$lop.checkLOP(' + oData.lopDays + ' , this.id)" onchange="$lop.checkLOP(' + oData.lopDays + ', this.id)"  id="' + sData + '"/>');
                        $(nTd).html(b);
                    }
                });
            }
            else {
                if (context[cnt].tableValue != '') {
                    columnDef.push({ "aTargets": [cnt], "sClass": context[cnt].cssClass + " word-wrap", "bSearchable": true });
                } else {
                    columnDef.push({ "aTargets": "", "sClass": context[cnt].cssClass + " word-wrap", "bSearchable": true });
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
                    url: $app.baseUrl + "Transaction/getLOPCreditDays", //Get Attributemodel List for Field values
                    data: JSON.stringify({
                        dataValue: {
                            applyMonth: $lop.applyMonth,
                            applyYear: $lop.appplyYear,
                            employeeId: $('#sltEmployeelist').val()
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
                $("#tbllopCredit").removeClass("sorting_asc");
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
        var taxableFileds = $("#" + $lop.tableId).dataTable().fnGetNodes();
        var i;
            for (i = 0; i < taxableFileds.length; i++) {
                if ($(taxableFileds[i]).find('.txtlopCreditDays input').val() != '' && parseInt($(taxableFileds[i]).find('.txtlopCreditDays input').val().trim()) <= $(taxableFileds[i]).find('.lopDays').html())
                {
                    var other = new Object();
                    other.employeeId = $("#sltEmployeelist").val();
                    other.applyMonth = $("#ddMonth").val();
                    other.applyYear = $("#txtYear").val();
                    other.id = $(taxableFileds[i]).find(".id").val();
                    other.month = $(taxableFileds[i]).find('.month').html();
                    other.year = $(taxableFileds[i]).find('.year').html();
                    other.lopDays = $(taxableFileds[i]).find('.lopDays').html();
                    other.paidDays = $(taxableFileds[i]).find('.txtlopCreditDays input').val();
                    other.type = "LOP";
                    lopCredits.push(other);


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
                }
                else {
                    $app.showAlert('Lop credit days must be less than Or equalto Lop days', 4);
                    $(taxableFileds[i]).find('.txtlopCreditDays input').val('0');
                }

            }   
    },
    checkLOP:function(lopdays,ID)
    {
        var creditdays = document.getElementById(ID).value;
        if(creditdays>lopdays)
        {
            $('#' + ID).val(0);
            $app.showAlert('Lop credit days must be less than Or equalto Lop days', 4);
            return false;
        }
    },

    frmSubmit: function () {
        
        
        if ($lop.settingFor == "LOP") {
            $lop.saveCredit();
        }
        else
        {
            $supplementary.saveCredit();
        }
    }
}