﻿


//-------------



var $app = {
    baseUrl: $rootUrl,
    secureformCommand: null,
    secureRoleformCommand: null,
    secureFormRights: null,
    topMenus: null,
    leftMenus: null,
    //  formRoleId:$("#hdnId").val(),
    showAlert: function (message, type) {

        var preMessage = '';
        switch (type) {
            case 1:
                type = 'info';
                preMessage = 'Info!';
                break;
            case 2:
                type = 'success';
                preMessage = 'Success!';
                break;
            case 3:
                type = 'warning';
                preMessage = 'Warning!';
                break;
            case 4:
                type = 'danger';
                preMessage = 'Failed!';
                break;
            default:
                type = 'info';
                break;
        }
        if ($("#alerts-container").length == 0) {
            // alerts-container does not exist, add it
            $("body").append($('<div id="alerts-container" class="modal-dialog" style="position: fixed;width: 50%; left: 30%; top: 0%; z-index:18000;">'));
        }
        $("#alerts-container").html('');
        // default to alert-info; other options include success, warning, danger
        //  type = type || "info";

        // create the alert div
        var alert = $('<div class="alert alert-' + type + ' fade in" role="alertdialog">').append(
            // $('<button type="button" class="close" data-dismiss="alert">').append("&times;")
            ' <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
        )
            .append('<strong>' + preMessage + ' </strong>' + message);

        // add the alert div to top of alerts-container, use append() to add to bottom
        $("#alerts-container").prepend(alert);

        // if closeDelay was passed - set a timeout to close the alert
        // if (closeDelay)
        window.setTimeout(function () {
            // alert.alert("close")
            $(".alert").fadeTo(5000, 0).slideUp(5000, function () {
                $(this).remove();
            });
        }, 5000);
    },
    clearSession: function (context) {

        if (context.StatusCode == 0) {
            window.location.href = $app.baseUrl;
        }
    },
    requiredValidate: function (target, event) {

        var isvalidated = true;
        $("#" + target + " .form-control").each(function () {
            if ($(this).attr('required') == "required") {
                if ($(this).val().trim() == '') {
                    $(this).val('');
                    $(this).focus();
                    isvalidated = false;
                    event.preventDefault();
                    return false;
                }
            }

        });
        return isvalidated;
    },
    clearControlValues: function (target) {

        $("#" + target + " .form-control").each(function () {
            if ($(this).prop('type') == "text") {
                $(this).val('');
            }
        });

    },
    applyseletedrow: function () {
        //  
        var table = $('.userTablehand').DataTable();
        $('.userTablehand tbody').on('click', 'tr', function () {
            $(".selected").removeClass("selected");
            $(this).toggleClass('selected');
        });
    },

    getrights: function (formname, operation) {

        var ret = false;
        $.each($app.secureFormRights, function (key, item) {
            //   
            if (item.formName == formname) {
                if (item[operation] == false)
                    ret = false;
                else
                    ret = true;
            }
        });
        return ret;
    },
    checkMenu: function () {

        $.each($('#ulPayMenu li>a'), function (cnt, elemnt) {
            //   
            var elId = elemnt.id;

            if (elId != '') {
                if ($app.getrights(elId, 'canVisible')) {

                    $('#ulPayMenu #' + elId).show();
                }
            }

        });
        debugger;
        $.each($('#topmenu li>a'), function (cnt, elemnt) {
            debugger;
            var elId = elemnt.id;

            if (elId != '') {
                if ($app.getrights(elId, 'canVisible')) {
                    $('#topmenu #' + elId).parent().hide();
                }
            }

        });


    },
    TopMenu: function () {
        var topmenu = "";
        $.each($app.topMenus, function (key, item) {
            debugger;
            var newItem = "<li class=navItem   navItem--domains'><a href='#' id='" + item.formName + "'><span>" + item.Description + " </span></a></li>";
            topmenu += newItem;
        });
        $("#topmenu").append($(topmenu));

        $('#topmenu li>a').on("click", function (e) {
            if (this.id == '') {
                return false;
            }
            
            else {
                debugger
                $.ajax({
                    type: 'POST',
                    url: $app.baseUrl + 'Company/DoModule',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ actionVal: this.id }),
                    // dataType: 'json',
                    success: function (data) {
                        console.log(data);
                        if (data.Status) {

                            window.location.href = data.result;
                        }

                    },
                    error: function (data) {
                        $('#dvError').removeClass('nodisp');
                        $('#lblError').text('There is some error.Please try again later.');
                    }
                });
            }
            //deActive all submenu
            $('.wraplist .sub-menu li>a').each(function () {
                $('#' + $(this).attr('id')).removeClass('active');
            });

            //active selected submenu
            $('#' + this.id).addClass('active');


        });
    },

    LeftMenus: function () {
        var MenuResult = $app.leftMenus;
        var MainMenu = [];
        var subMenu = [];
        MenuResult.forEach(function (item) {
            if (item.ParentMenu == 0) {
                MainMenu.push(item);
            }
            else {
                subMenu.push(item);
            }
        });
        MainMenu.forEach(function (item) {
            var ulStart = "<ul class='nav-left-dropdown sub-menu'>";
            var ulEnd = "</ul>"
            var liStart = "<li>";
            var liEnd = "</li>"
            var subTitle = "";
            var iconClass = "";
            var newItem = "";
            if (item.formName.indexOf("Master") != -1)
                newItem = "<a href='javascript:;' id='" + item.formName + "'> " +
                    " <i class='fa fa-bank'></i><span class='title'>" + item.Description + " </span> <span class='arrow '></span> "
                    + " </a>";
            if (item.formName.indexOf("Transaction") != -1)
                newItem = "<a href='javascript:;' id='" + item.formName + "'> " +
                    " <i class='fa fa-sliders'></i><span class='title'>" + item.Description + " </span> <span class='arrow '></span> "
                    + " </a>";
            if (item.formName.indexOf("Utilities") != -1 || item.formName == 'OpeningInsert')
                newItem = "<a href='javascript:;' id='" + item.formName + "'> " +
                    " <i class='fa fa-arrow-circle-o-down'></i><span class='title'>" + item.Description + " </span> <span class='arrow '></span> "
                    + " </a>";
            if (item.formName.indexOf("Settings") != -1)
                newItem = "<a href='javascript:;' id='" + item.formName + "'> " +
                    " <i class='fa fa-gear'></i><span class='title'>" + item.Description + " </span> <span class='arrow '></span> "
                    + " </a>";
            if (item.formName.indexOf("Admin") != -1)
                newItem = "<a href='javascript:;' id='" + item.formName + "'> " +
                    " <i class='fa fa-adn'></i><span class='title'>" + item.Description + " </span> <span class='arrow '></span> "
                    + " </a>";
            if (item.formName.indexOf("Report") != -1 || item.formName == 'UserReporTab')
                newItem = "<a href='javascript:;' id='" + item.formName + "'> " +
                    " <i class='fa fa-file'></i><span class='title'>" + item.Description + " </span> <span class='arrow '></span> "
                    + " </a>";
            if (item.formName.indexOf("MyInfo") != -1)
                newItem = "<a href='javascript:;' id='" + item.formName + "'> " +
                    " <i class='fa fa-user-circle'></i><span class='title'>" + item.Description + " </span> <span class='arrow '></span> "
                    + " </a>";
            if (item.formName.indexOf("Guidelines") != -1)
               
                newItem = "<a class='Guidelines' href='" + $app.baseUrl + "DataWizard/DownloadGuidelineFile' id='" + item.formName + "'> " +
                    " <i class='fa fa-map-marker'></i><span class='title'>" + item.Description + " </span>"
                    + " </a>";
            if (item.formName.indexOf("PayrollDashboard") != -1)
                newItem = "<a class='PayrollDashboard' href='javascript:;' id='" + item.formName + "'> " +
                    " <i class='fa fa-user-circle'></i><span class='title'>" + item.Description + " </span> <span class='arrow '></span> "
                    + " </a>";
            subMenu.forEach(function (subitem) {
                if (subitem.ParentMenu == item.formName) {
                    var id = subitem.formName;
                    var clsActive = "";
                    var newItem = "<li ><a class='" + clsActive + "' href='#' id='" + id + "'>" + subitem.Description + " </a></li>";
                    subTitle += newItem;
                }
            });

            $("#ulPayMenu").append($(liStart + newItem + ulStart + subTitle + ulEnd + liEnd));
            $('.nav-left-dropdown > li').each(function () {
                $(this).parent().slideUp("slow");
                $(this).parent().parent().addClass('active');
            });

        });
        $('.nav-left-menu > li > a').click(function () {

            $('.nav-left-menu > li').removeClass('active open');
            if ($(this).next('.nav-left-dropdown').is(":visible")) {
                $(this).next('.nav-left-dropdown').slideUp("slow");
            }
            else {
                $('.nav-left-dropdown').slideUp("slow");
                $(this).next('.nav-left-dropdown').slideDown("slow");
                $(this).parent().addClass('active open');
            }
        });

        $('.nav-left-dropdown li>a').on("click", function (e) {

            $('.sub-menu li >a').removeClass("active");
            $(this).addClass("active");
            $payroll.CallControler(this);
        });
        $('.PayrollDashboard').on("click", function (e) {
            $payroll.CallControler(this);
        });
    },

    secureFormRightsView: function () {
        var data = $app.secureFormRights;
        if (data != null) {
            $.each(data, function (da, t) {
                var cls = t.canVisible == true ? "show" : "hide";
                if (t.canVisible == false && t.CommandType == "tab") {
                    $('#' + t.formName).remove();
                }
                else if (t.canVisible == false && t.CommandType != "tab") {
                    $('#' + t.formName).addClass(cls);
                }
            });
        }
    },
    /* Progress bar execute, while loading an event in project */

    showProgressModel: function () {
        //comment by mubarak in order to solve the issue in leave module
        // $compView.GetCompanyData();
        debugger
        var appPath = $app.baseUrl;
        ModalPopups.Indicator("progressModelId", "Please wait", "<div style=''>" +
            "<div style='float:left;'><img src='" + appPath + "assets/images/spinner.gif'></div>" +
            "<div style='float:left; padding-left:10px;'>" +
            "Loading Page" +
            "<br/>" +
            "This may take few seconds." +
            "</div>",
            {
                width: 300,
                height: 100

            });

        if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
            var ieversion = new Number(RegExp.$1)
            if (ieversion >= 5) {//ieversion >= 9 || ieversion >= 8 || ieversion >= 7 || ieversion >= 6 ||
                setTimeout('ModalPopups.Close(\"progressModelId\");', 1200);
            }
        }
    },
    /* Close the progress model*/
    hideProgressModel: function () {
        ModalPopups.Close("progressModelId");
    },
    /* Progress bar execute, while loading an event in project */
    showDownloadProgressModel: function () {
        var appPath = $app.baseUrl;
        ModalPopups.Indicator("progressModelId", "Please wait", "<div style=''>" +
            "<div style='float:left;'><img src='" + appPath + "assets/images/spinner.gif'></div>" +
            "<div style='float:left; padding-left:10px;'>" +
            "Downloading" +
            "<br/>" +
            "The file is downloading, please wait..." +
            "</div>",
            {
                width: 300,
                height: 100

            });
        if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
            var ieversion = new Number(RegExp.$1)
            if (ieversion >= 5) {//ieversion >= 9 || ieversion >= 8 || ieversion >= 7 || ieversion >= 6 ||
                setTimeout('ModalPopups.Close(\"progressModelId\");', 1200);
            }
        }
    },

    downloadSync: function (url, data) {
        debugger;
        $app.showDownloadProgressModel();
        url = $app.baseUrl + url;
        $.fileDownload(url, {
            //preparingMessageHtml: "We are preparing your report, please wait...",
            //failMessageHtml: "There was a problem generating your report, please try again.",
            httpMethod: "POST",
            data: data//$(this).serialize()
            , successCallback: function (url) {

                $app.hideProgressModel();
            },
            failCallback: function (responseHtml, url) {
                $app.hideProgressModel();
                $app.showAlert('File not found', 4);

            }
        });
        // e.preventDefault(); //otherwise a normal form submit would occur
        return false;
    },
    downloadAsync: function (url, data) {
        url = $app.baseUrl + url;
        $app.showAlert('The File download has been started.', 1);
        $.fileDownload(url)
            .done(function () {
                $app.showAlert('The File has been download.', 2);
            })
            .fail(function () {
                $app.showAlert('The File has not been download.', 4);
            });

        return false; //this is critical to stop the click event which will trigger a normal file download
    },
    //----
    //Modified by Keerthika on 18/05/2017
    formRights: function () {

        $.ajax({
            url: $rootUrl + "Home/GetUserFormrights",
            data: JSON.stringify({ roleId: $("#hdnId").val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                //    
                /*debugger;*/
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {

                    case true:
                        var p = jsonResult.result;
                        $app.secureFormRights = p.formRights;
                        $app.topMenus = p.topMenus;
                        $app.leftMenus = p.leftMenus;
                        $app.TopMenu();
                        //$app.checkMenu();
                        $app.LeftMenus();
                        $app.secureFormRightsView();
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
    },//loadFormRights()
    //---
    //Modified by Keerthika on 11/05/2017
    formRoleCommand: function () {//need to work 


        $.ajax({
            url: $rootUrl + "Setting/GetRoleFormSetting",
            data: JSON.stringify({ roleId: $("#hdnId").val() }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                switch (jsonResult.Status) {
                    case true:

                        var p = jsonResult.result;
                        for (var i = 0; i < p.length; i++) { //------



                            //-- $app.secureFormCommand = p;
                            // --$app.applyFormCommand();
                            $app.secureRoleformCommand = p;
                            $app.applyFormroleCommand();
                            //  }
                        }
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
    },//loadFormRights()
    //------------
    //Modified by keerthika on 16/05/2017
    applyFormroleCommand: function () {

        $("*[data-rolecmd]").each(function (i, elmnt) {
            var cmdName = $(elmnt).attr("data-rolecmd");
            $.each($app.secureRoleformCommand, function (ind, obj) {
                if (obj.commandName.trim() == cmdName.trim()) {
                    if (!obj.isRequired) {
                        $(elmnt).prop('required', false);
                    }
                    if (obj.isRequired) {
                        $(elmnt).prop('required', true);
                    }
                    if (!obj.isWrite) {
                        $(elmnt).prop("disabled", true);
                        $(elmnt).prop("readonly", true);

                    } else {

                    }
                    //if (obj.isWrite) {      //--
                    //    $(elmnt).prop("disabled", false);
                    //    $(elmnt).prop("readonly", false);
                    //}
                    if (!obj.isRead) {
                        $(elmnt).addClass('hide');
                        $(elmnt).parent().parent().addClass('hide');
                    }
                }

            });

        });
    },



};
