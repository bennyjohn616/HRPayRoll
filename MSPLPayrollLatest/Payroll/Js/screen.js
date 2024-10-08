﻿
$screen = {
    createPopUp: function (context) {
        var returnObject = '<form id="' + context.id + '" role="form">';
        returnObject = returnObject + ' <div class="modal-dialog">';
        returnObject = returnObject + ' <div class="modal-content">';
        returnObject = returnObject + '<div class="modal-header"><button type="button" class="close" data-dismiss="modal">&times;</button><h4 class="modal-title" id="H4">'+context.title+'</h4></div>';
        returnObject = returnObject + ' <div class="modal-body">';
        returnObject = returnObject + '{ModelBody}';
        returnObject = returnObject + '</div>';
        if (context.button != null) {
            returnObject = returnObject + '  <div class="modal-footer"><button type="submit" id="' + context.button.id + '" class="btn custom-button">Save</button><button type="button" class="btn custom-button" data-dismiss="modal">Close</button></div>';
        }
        return returnObject;
    },
    createTable: function (tableId, context) {
        debugger
        var grid = '<table id="' + tableId.id + '" class="table table-responsive table-striped table-hover table-condensed userTablehand dataTable no-footer">'
        grid = grid + '<thead><tr>'
        for (var cnt = 0; cnt < context.length; cnt++) {
            grid = grid + '<th class="' + context[cnt].cssClass + '">' + context[cnt].tableHeader + '</th>'
        }
        grid = grid + '</tr></thead>';
        grid = grid + '<tbody><tr>'
        for (var cnt = 0; cnt < context.length; cnt++) {//for action td 
            grid = grid + '<td></td>';
        }
        grid = grid + '</tr></tbody></table>';
        return grid;

    }
}