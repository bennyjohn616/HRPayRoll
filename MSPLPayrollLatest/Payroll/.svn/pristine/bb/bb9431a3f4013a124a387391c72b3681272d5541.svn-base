$('input[type=radio][name=Matching]').change(function () {
    if (this.value == 'Cumulative') {
        $("#dvMatching").removeClass('show').addClass('hide');
        $("#dvCumulative").removeClass('hide').addClass('show');
    }
    else if (this.value == 'Matching') {
        $("#dvMatching").removeClass('hide').addClass('show');
        $("#dvCumulative").removeClass('show').addClass('hide');
    }
    else {
        $("#dvCumulative").removeClass('show').addClass('hide');
        $("#dvMatching").removeClass('show').addClass('hide');
    }
});




$payslipMatching = {
    Data: null,
    MatchingVal: null,
    LoadAttributeModels: function () {
        var attr = [];
        var allAttr = [];
        var rows = $("#tblEarnings").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {
            var newattr = new Object();
            newattr.fieldName = $(rows[i]).find(":eq(1)").html();
            // newattr.tableName = $(rows[i]).find(":eq(3)").html();
            newattr.type = "Earnings";
            newattr.isPhysicalTbl = true;
            newattr.hOrder = $(rows[i]).find(".txtHOrder input").val();
            newattr.fOrder = $(rows[i]).find(".txtFOrder input").val();
            newattr.eOrder = $(rows[i]).find(".txtEOrder input").val();
            newattr.displayAs = $(rows[i]).find(".txtDisplay input").val();
            newattr.dOrder = 0;
            newattr.Name = $(rows[i]).find(".txtDisplay input").val();
            newattr.Id = $(rows[i]).find(":eq(1)").html();
            newattr.MappedColumn = newattr.Name;
            newattr.MappedId = $(rows[i]).find(":eq(2)").html();
            allAttr.push(newattr);
            if ($(rows[i]).find(".txtEOrder input").val() != 0) {
                attr.push(newattr);
            }


        }
        $payslipMatching.Data = allAttr;
        var dtClientList = $('#tblsettingColumn').DataTable({
            'iDisplayLength': 18,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
                { "data": "Id" },
             { "data": "Name" },
             { "data": "MappedColumn" }
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
             "sClass": "column",

             "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                 var b = $($payslipMatching.loadSelect(oData));
                 $(nTd).empty();
                 $(nTd).prepend(b);
             }
         },


            ],
            data: attr,
            fnInitComplete: function (oSettings, json) {

            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    loadSelect: function (oData) {

        oData.Name = oData.Name.replace(/ +/g, "");
        var data = $payslipMatching.Data;
        var sel = "";

        var select = '<select id="' + oData.Name + '" onchange="$payslipMatching.alreadyMapped(this.id)"><option value="00000000-0000-0000-0000-000000000000">--select--</option>'
        for (var cnt = 0; cnt < data.length; cnt++) {
            var selected = "";
            if (oData.MappedId == data[cnt].Id) {
                selected = "selected";
            }
            select = select + '<option ' + selected + ' value="' + data[cnt].Id + '">' + data[cnt].Name + '</option>'
        }
        select = select + '</select>';
        return select;
    },
    renderFieldGrid: function () {
        var grid = '<table id="tblsettingColumn" class="table table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer"><thead><tr><th class="nodisp"></th><th>Name</th><th>MappedColumn</th></tr>';
        grid = grid + '<thead><tbody><tr><td></td><td></td><td></td></tr></tbody></table>';
        $("#dvColoumn").html(grid);
    },
    alreadyMapped: function (selectId) {
        var find = '#' + selectId + ' option:selected'
        var currentVal = $(".column").find(find).val();
        var count = 0;
        var rows = $("#tblsettingColumn").dataTable().fnGetNodes();
        var column = [];
        for (var i = 0; i < rows.length; i++) {
            column[i] = $(rows[i]).find(".column option:selected").val();
        }
        if (currentVal != 0) {
            for (var j = 0 ; j < column.length; j++) {

                if (currentVal == column[j]) {

                    count++;
                    if (count > 1) {
                        $app.showAlert('Already mapped', 3);
                        $(".column").find('#' + selectId).val('00000000-0000-0000-0000-000000000000');
                    }
                }

            }
        }

    },
    saveMatchingComp: function () {
        var rows = $("#tblsettingColumn").dataTable().fnGetNodes();
        var c = 0;
        for (var i = 0; i < rows.length; i++) {

            if ($(rows[i]).find(".column option:selected").text() == "--select--") {
                c++;
            }
        }
        if (c == rows.length) {
            $app.showAlert('Please select atleast one matching components', 4);
        }
        else {

            var things = [];
            var rows = $("#tblsettingColumn").dataTable().fnGetNodes();
            for (var i = 0; i < rows.length; i++) {

                var data = {
                    Id: $(rows[i]).find(".nodisp").text(),
                    Name: $(rows[i]).find(".word-wrap").text(),
                    MappedColumn: $(rows[i]).find(".column option:selected").text(),
                    MappedId: $(rows[i]).find(".column option:selected").val()
                }

                things.push(data);
            }
            $payslipMatching.MatchingVal = null;
            $payslipMatching.MatchingVal = things;
        }
    },
}