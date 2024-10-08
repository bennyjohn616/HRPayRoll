$PaySheetGrouping = {
    dtGrouping: null,
    masterFields: [],
    renderGroup: function () {

        this.dtGrouping = $("#tblGrouping").DataTable({
            "bPaginate": false,
            "bFilter": false,
            "bInfo": false,
            "bDestroy": true, fnDrawCallback: function () {
                $("#tblGrouping").removeClass("sorting_asc");
                $('#tblGrouping tbody').on('click', 'a .glyphicon-remove', function () {
                    $PaySheetGrouping.dtGrouping.row($(this).parents('tr')).remove().draw(false);
                });
            }
        });
        jsonResult: $PaySheetGrouping.masterFields;
    },
    addRow: function () {

        var jsonResult = $PaySheetGrouping.masterFields;
        var strOption = '<select  class="groupField form-control"><option value=0>--Select--</option>';
        for (var i = 0; jsonResult.length > i; i++) {


            strOption = strOption + "<option value='" + jsonResult[i].tableName + "'>" + jsonResult[i].fieldName + "</option>";

        }
        strOption = strOption + "</select>"
        $PaySheetGrouping.dtGrouping.row.add([strOption, '<a href="#" class="deleteButton" title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>']).draw(false);
    }

}
