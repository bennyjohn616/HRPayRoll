﻿ 
$("#sltEntityModel,#sltCategory").change(function () {
    debugger
    if ($("#sltEntityModel").val() != "0" && $("#sltCategory").val() != "00000000-0000-0000-0000-000000000000") {
        $PreviousComponentsSetting.renderFieldGrid();
        $PreviousComponentsSetting.LoadAttributeModels();
    }
    else {
        $app.showAlert('Please select salary grade and category', 4);
    }

});




$PreviousComponentsSetting = {
    selectedEntityModelId:null,
    Data:null,
    LoadEntityModel: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/GetMonthlyEntityList",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var out = jsonResult.result;
                        $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                        $.each(out, function (index, object) {
                            $('#' + dropControl.id).append($("<option></option>").val(object.Id).html(object.Name));
                            $PreviousComponentsSetting.selectedEntityModelId = object.EntityModelId;
                        });
                        break;
                    case false:

                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }

            },
            error: function (msg) {
            }
        });
        $('#' + dropControl.id).change(function () {
            if ($('#' + dropControl.id).val() == 0) {
                $("#dvDynamicEntity").html('');
            }
            else {
                
            }
        });
    },
    alreadyMapped: function (selectId) {
        debugger;
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
renderFieldGrid: function () {
        debugger;

        var grid = '<table id="tblsettingColumn" class="table table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer"><thead><tr><th class="nodisp"></th><th>Name</th><th>MappedColumn</th></tr>';
        grid = grid + '<thead><tbody><tr><td></td><td></td><td></td></tr></tbody></table>';


        $("#dvColoumn").html(grid);
},
save: function () {
    debugger;
    var rows = $("#tblsettingColumn").dataTable().fnGetNodes();
    var c = 0;
    for (var i = 0; i < rows.length; i++) {

        if ($(rows[i]).find(".column option:selected").text() == "--select--") {
            c++;
        }
     
    }
   
    if (c == rows.length) {
        $app.showAlert('Please select atleast one matching components', 4);
        return false;
    }
    else {
        debugger;
        var things = [];
        var rows = $("#tblsettingColumn").dataTable().fnGetNodes();
        for (var i = 0; i < rows.length; i++) {
            debugger;
            var radi="";
            if ($("#hide").prop('checked') == true) {
                debugger;
                radi="hide";
            }
            else if($("#se").prop('checked') == true){
                radi="se";
            }
            else if($("#sne").prop('checked') == true){
                radi="sne";
            }
            else{
                $app.showAlert('Please check hide or showandedit or showandnonedit components', 4);
                return false;
            }

            var data = {
                Id: $(rows[i]).find(".nodisp").text(),
                Name: $(rows[i]).find(".word-wrap").text(),
                MappedColumn: $(rows[i]).find(".column option:selected").text(),
                MappedId: $(rows[i]).find(".column option:selected").val(),
                radio:radi
            }

            things.push(data);
        }
        things = JSON.stringify({
            'things': things, entityId: $("#sltEntityModel").val(),
            categoryId: $("#sltCategory").val(),
            entitymodelId: $PreviousComponentsSetting.selectedEntityModelId
        });
        $app.showProgressModel();
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Entity/PreviousComponentsSettingSave",
            contentType: "application/json; charset=utf-8",
            data: things,
            dataType: "json",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        debugger;
                        $app.showAlert(jsonResult.Message, 2);
                        $PreviousComponentsSetting.renderFieldGrid();
                        $PreviousComponentsSetting.LoadAttributeModels();
                        $app.hideProgressModel();
                        break;
                    case false:

                        $app.showAlert(jsonResult.Message, 4);
                        $app.hideProgressModel();
                        break;
                }
            },
            error: function (msg) {
            }
        });
    }
},
loadSelect: function (oData) {
    debugger;
    oData.Name = oData.Name.replace(/ +/g, "");
    var data = $PreviousComponentsSetting.Data;
    var select = '<select id="' + oData.Name + '" onchange="$PreviousComponentsSetting.alreadyMapped(this.id)"><option value="00000000-0000-0000-0000-000000000000">--select--</option>'
    for (var cnt = 0; cnt < data.length; cnt++) {
        var selected = "";
        if (oData.MappedId == data[cnt].Id) {
            selected = "selected";
        }
        select = select + '<option ' + selected + ' value="' + data[cnt].Id + '">' + data[cnt].Name + '</option>'
    }
    for (var cnt = 0; cnt < data[0].attr.length; cnt++) {
    var selected = "";
    if (oData.MappedId == data[0].attr[cnt].Id) {
            selected = "selected";
        }
        select = select + '<option ' + selected + ' value="' + data[0].attr[cnt].Id + '">' + data[0].attr[cnt].Name + '</option>'
    }
    select = select + '</select>';
    return select;
},
loadradio: function (Name,col) {
    debugger;
    if (col == 3) {
        return '<input type="radio" id="hide" name='+Name+'>';
    }
    if (col == 4) {
        return '<input type="radio" id="se" name=' + Name + '>';
    }
    if (col == 5) {
        return '<input type="radio" id="sne" name=' + Name + '>';
    }
},

LoadAttributeModels: function () {
    debugger;
    var dtClientList = $('#tblsettingColumn').DataTable({
        'iDisplayLength': 18,
        'bPaginate': true,
        'sPaginationType': 'full',
        'sDom': '<"top">rt<"bottom"ip><"clear">',
        columns: [
            { "data": "Id" },
         { "data": "Name" },
         { "data": "MappedColumn" },
    


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
             debugger;
             var b = $($PreviousComponentsSetting.loadSelect(oData));
             $(nTd).empty();
             $(nTd).prepend(b);
         }

     },
       //{
       //    "aTargets": [3],
       //    "sClass": "column",

       //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
       //        debugger;
       //        var b = $($PreviousComponentsSetting.loadradio(sData,iCol));
       //        $(nTd).empty();
       //        $(nTd).prepend(b);
       //    }

       //},
       //  {
       //      "aTargets": [4],
       //      "sClass": "column",

       //      "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
       //          debugger;
       //          var b = $($PreviousComponentsSetting.loadradio(sData,iCol));
       //          $(nTd).empty();
       //          $(nTd).prepend(b);
       //      }

       //  },
       //    {
       //        "aTargets": [5],
       //        "sClass": "column",

       //        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
       //            debugger;
       //            var b = $($PreviousComponentsSetting.loadradio(sData,iCol));
       //            $(nTd).empty();
       //            $(nTd).prepend(b);
       //        }

       //    },
       

        ],
        ajax: function (data, callback, settings) {
            $.ajax({
                type: 'POST',
                url: $app.baseUrl + "Entity/GetPreviousComponents",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    entitymodelId: $PreviousComponentsSetting.selectedEntityModelId,
                    entityId: $("#sltEntityModel").val(),
                    categoryId: $("#sltCategory").val(),
                    month: 4,
                    year: 1997,
                    employeeId: "00000000-0000-0000-0000-000000000000"
                }),
                dataType: "json",
                success: function (msg) {
                    debugger;
                    var out = msg.result;
                    if (msg.Message!="") {
                      var past = msg.Message.split(',');
                    $("#month").val(past[0]);
                    $("#year").val(past[1]);
                    }
                    $("#"+out[0].radio).attr('checked',true);

                    $PreviousComponentsSetting.Data = msg.result;
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

        },
        dom: "rtiS",
        "bDestroy": true,
        scroller: {
            loadingIndicator: true
        }
    });



},

}