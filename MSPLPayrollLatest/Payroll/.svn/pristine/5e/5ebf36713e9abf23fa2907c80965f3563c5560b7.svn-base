$("#txtInterestPercent").change(function () {
    var Percentage=parseFloat($('#txtInterestPercent').val());
    if (Percentage > 100.1) {
        $app.showAlert("Percentage should not be greater than 100 !!!", 4);
        $('#txtInterestPercent').val('');
    }    
});

$("#rbtnIsInterestYes ,#rbtnIsInterestNo").change(function () {
    if (!$("#rbtnIsInterestYes").prop('checked')) {
        $('#txtInterestPercent').val(0);
        $('#txtInterestPercent').prop('readonly', true);
    }
    else {
        $('#txtInterestPercent').prop('readonly', false);
    }

});


var $LoanMaster = {
    LoanMasterId: '',
    attributeModelList: null,
    AttributeModelId: '',
    LoadLoanMaster: function () {
        var dtClientList = $('#tblLoanMaster').DataTable({
            'iDisplayLength': 10,
            'bPaginate': true,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
             { "data": "loanid" },
                { "data": "attributeModelid" },
                    { "data": "loanCode" },
                       { "data": "loanDesc" },
                       { "data": "loanIsInterest" },
                       { "data": "loanInterestPercent" },
                       { "data": null }
            ],
            "aoColumnDefs": [

        {
            "aTargets": [0, 1],
            "sClass": "nodisp",
            "bSearchable": false
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
               "aTargets": [4],
               "sClass": "word-wrap"

           },
             {
                 "aTargets": [5],
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
                               //alert("begin GetloanEntry Data");
                               $LoanMaster.GetLoanMasterData({ Id: oData.loanid });
                               return false;
                           });
                           c.button();
                           c.on('click', function () {
                               if (confirm('Are you sure ,do you want to delete?')) {
                                   $LoanMaster.DeleteData({ Id: oData.loanid });
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
                    url: $app.baseUrl + "Loan/GetLoanMaster",
                    contentType: "application/json; charset=utf-8",
                    data: null,
                    dataType: "json",
                    success: function (jsonResult) {
                        $app.clearSession(jsonResult);
                        switch (jsonResult.Status) {
                            case true:
                                var out = jsonResult.result;
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
                                $app.showAlert(jsonResult.Message, 4);
                                //alert(jsonResult.Message);
                        }

                    },
                    error: function (msg) {
                    }
                });
            },
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblLoanMaster tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblLoanMaster thead').append(r);
                $('#search_0').css('text-align', 'center');
            },
            dom: "rtiS",
            "bDestroy": true,
            scroller: {
                loadingIndicator: true
            }
        });

    },
    save: function () {
        $app.showProgressModel();
        // var data = $employee.BuilEmployeObject();
        var formData = document.forms["frmLoanMaster"];
        var data = {
            loanid: $LoanMaster.LoanMasterId,
            attributeModelid: $LoanMaster.AttributeModelId,
            loanCode: formData.elements["txtLoanCode"].value,
            loanDesc: formData.elements["txtLoanDesc"].value,
            loanIsInterest: $(formData).find("#rbtnIsInterestYes").prop('checked') == true ? true : false,
            loanInterestPercent: formData.elements["txtInterestPercent"].value,
            loanEligComp: formData.elements["sltEligComponent"].value,

        };
        $.ajax({
            url: $app.baseUrl + "Loan/SaveLoanMasterData",
            data: JSON.stringify({ dataValue: data }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddLoanMaster').modal('toggle');
                        $LoanMaster.LoadLoanMaster();
                        $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 2);
                        // alert(jsonResult.Message);
                        var p = jsonResult.result;
                        companyid = 0;
                        break;
                    case false:
                       $app.hideProgressModel();
                        $app.showAlert(jsonResult.Message, 4);
                        //alert(jsonResult.Message);
                        break;
                }
            },
            complete: function () {
               $app.hideProgressModel();
            }
        });

    },
    AddInitialize: function () {
        var formData = document.forms["frmLoanMaster"];
        $LoanMaster.LoanMasterId = '';
        formData.elements["txtLoanCode"].value = "";
        formData.elements["txtLoanDesc"].value = "";
        formData.elements["rbtnIsInterestYes"].value = true;
        formData.elements["rbtnIsInterestNo"].value = false;
        formData.elements["txtInterestPercent"].value = "";
        formData.elements["sltEligComponent"].value = "0";
    },

    GetLoanMasterData: function (context) {
        $.ajax({
            url: $app.baseUrl + "Loan/GetLoanMasterData",
            data: JSON.stringify({ loanid: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $('#AddLoanMaster').modal('toggle');
                        var p = jsonResult.result;
                        $LoanMaster.RenderData(p);
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
            url: $app.baseUrl + "Loan/DeleteLoanMasterData",
            data: JSON.stringify({ id: context.Id }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $LoanMaster.LoadLoanMaster();
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
        debugger;
        var formData = document.forms["frmLoanMaster"];
        $LoanMaster.LoanMasterId = data.loanid;
        $LoanMaster.AttributeModelId = data.attributeModelid;
        formData.elements["txtLoanCode"].value = data.loanCode;
        formData.elements["txtLoanDesc"].value = data.loanDesc;
        (data.loanIsInterest == true) ? $(formData).find("#rbtnIsInterestYes").prop('checked', 'true') : $(formData).find("#rbtnIsInterestNo").prop('checked', 'true');
        formData.elements["txtInterestPercent"].value = data.loanInterestPercent;
        if (data.loanEligComp == "00000000-0000-0000-0000-000000000000") {
            formData.elements["sltEligComponent"].value = "0";
        }
        else {
            formData.elements["sltEligComponent"].value = data.loanEligComp;
        }
        
    },
    //LoadDynamicPopupDataJS: function (dropControl,temp,DisplayMember) {
    //     debugger;
         
    //     $.ajax({
    //         url: $app.baseUrl + "Loan/LoadDynamicPopupData",
    //         data: JSON.stringify({ popuptype: temp }),
    //         dataType: "json",
    //         //contentType: "json",
    //         contentType: "application/json",
    //         type: "POST",
    //         async: false,
    //         success: function (msg) {
    //             debugger;
    //             var out = msg.result;
    //             //var out = msg;
    //             switch (msg.Status) {
    //                 case true:
    //                     debugger;
    //                     $('#' + dropControl.id).empty();
    //                     $('#' + dropControl.id).append($("<option></option>").val('select').html('--Select--'));
    //                     $.each(out, function (index, blood) {
    //                         $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.DropdownComponent));
    //                     });
    //                     $('#lblSelectedPopupData').html(DisplayMember);
                         
    //                     break;
    //                 case false:
    //                     $('#' + dropControl.id).empty();
    //                     $('#lblSelectedPopupData').html(DisplayMember);
    //                     break;
    //             }
    //         },
    //         error: function () {
    //             debugger;
    //         }
    //     });

    //},

    LoadEligComponent: function (dropControl) {
        debugger;
    $.ajax({
            url: $app.baseUrl + "Loan/LoadEligblityComponent",
            data: null,
            dataType: "json",
            //contentType: "json",
            contentType: "application/json",
            type: "POST",
            async: false,
            success: function (msg) {
                debugger;
                var out = msg.result;
                //var out = msg;
                switch (msg.Status) {
                    case true:
                        debugger;
                        $('#' + dropControl.id).empty();
                        $('#' + dropControl.id).append($("<option></option>").val('0').html('--Select--'));
                        $.each(out, function (index, blood) {
                            $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.DisplayAs));
                        });
                        

                        break;
                    case false:
                        $('#' + dropControl.id).empty();
                        $app.showAlert(msg.result, 4);
                        break;
                }
            },
            error: function () {
                debugger;
            }
        });

    }
    //loadAttributeModelList: function () {

    //    $.ajax({
    //        //url: $app.baseUrl + "Entity/GetFormulaAttributeModelList",
    //        //data: JSON.stringify({ entityModelId: $formulaCreation.entityModelId }),
    //        url: $app.baseUrl + "Entity/GetPayrollmodels",
    //        data: JSON.stringify({ name: 'Salary' }),
    //        dataType: "json",
    //        contentType: "application/json",
    //        type: "POST",
    //        success: function (jsonResult) {
    //            $app.clearSession(jsonResult);
    //            switch (jsonResult.Status) {
    //                case true:
    //                    var p = jsonResult.result;
    //                    $LoanMaster.attributeModelList = p;
    //                    $LoanMaster.renderAttributeModelTree(p);
    //                    break;
    //                case false:
    //                    $app.showAlert(jsonResult.Message, 4);
    //                    break;
    //            }
    //        },
    //        complete: function () {

    //        }
    //    });
    //},
    //renderAttributeModelTree: function (data) {

    //    var treeData = [];
    //    for (var cnt = 0; cnt < data.length; cnt++) {
    //        if (data[cnt].AttributeModelList.length <= 0) {
    //            treeData.push({ key: data[cnt].Id, title: data[cnt].DisplayAs });
    //        }
    //        else {
    //            var treeChildData = [];
    //            for (var childcnt = 0; childcnt < data[cnt].AttributeModelList.length; childcnt++) {
    //                treeChildData.push({ key: data[cnt].AttributeModelList[childcnt].Id, title: data[cnt].AttributeModelList[childcnt].Name + ' [' + data[cnt].AttributeModelList[childcnt].DisplayAs + ']' });
    //            }
    //            treeData.push({ key: data[cnt].Id, title: data[cnt].DisplayAs, children: treeChildData });
    //        }
    //    }
    //    //initialize the tree
    //    $("#attributeModeltree").fancytree();
    //    //before load clean the tree nodes
    //    $("#attributeModeltree").fancytree("destroy");
    //    fancyTree = jQuery("#attributeModeltree").fancytree({
    //        extensions: ["contextMenu", "filter"],
    //        selectMode: 3,
    //        strings: {
    //            loading: "Loading..."
    //        },
    //        source: treeData,
    //        dblclick: function (event, data) {

    //            debugger;
    //            $formulaCreation.buildFormula(event, data);
    //        },
    //        filter: {
    //            // mode: "dimm"    
    //            mode: "hide"  // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
    //        },
    //        init: function (event, ctx) {
    //            ctx.tree.debug("init");
    //            ctx.tree.rootNode.fixSelection3FromEndNodes();
    //        },
    //        loadchildren: function (event, ctx) {
    //            ctx.tree.debug("loadchildren");
    //            ctx.node.fixSelection3FromEndNodes();
    //        },
    //        select: function (event, data) {
    //        }
    //        , contextMenu: {
    //            menu: function (node) {
    //                if (node.parent.title == 'root') {
    //                    return {
    //                        // 'AddFormula': { 'name': 'Add to Formula', 'icon': 'new', 'disabled': ((node.data.FileType === "2") ? true : false) }
    //                    }
    //                }
    //                else if ($formulaCreation.entityAttributeModel.AttributeModel.IsIncrement) {
    //                    return {
    //                        'AddFormula': { 'name': 'Add to Formula', 'icon': 'add' },
    //                        'SetArrear': { 'name': 'Set Arrear Field', 'icon': 'add' }
    //                    }
    //                }
    //                else {
    //                    return {
    //                        'AddFormula': { 'name': 'Add to Formula', 'icon': 'add' }

    //                    }
    //                }
    //            },
    //            actions: function (node, action, options) {
    //                if (action === "AddFormula") {
    //                    debugger;
    //                    $formulaCreation.buildFormula(null, { node: node }, null);
    //                }
    //                else if (action === "SetArrear") {
    //                    $formulaCreation.setArrearMatchField(node);
    //                }
    //                else {
    //                    return false;
    //                }
    //            }
    //        }
    //    });
    //}
};