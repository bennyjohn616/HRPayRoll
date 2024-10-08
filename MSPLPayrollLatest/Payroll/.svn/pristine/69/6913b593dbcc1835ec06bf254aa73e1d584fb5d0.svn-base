//$('#ulPayMenu li>a').on("click", function (e) {
 
    
//    //$compView.GetCompanyData();
//    if (this.id == '') {
//        return false;
//    }
//    else {
        
//        if ($(window).width() < 1025)
//        {
         
//               $(this).parent('li').parent('.sub-menu').css("display", "none");
              
//               $(this).parent('li').parent('.sub-menu').siblings('a').children("span.title").addClass("hidden");
//               if ($(this).siblings().size()> 0) {
//                   $(this).children("span.title").removeClass("hidden");
//               }
//               else {
//                   $(this).children("span.title").toggleClass("hidden");
//                   $(this).css("width", "0px");
//               }
//        }
//        $payroll.CallControler(this);
//    }
//    //deActive all submenu
//    $('.wraplist .sub-menu li>a').each(function () {
//        $('#' + $(this).attr('id')).removeClass('active');
//    });

//    //active selected submenu
//    $('#' + this.id).addClass('active');


//});
//--------------
//$('#utopmenu li>a').on("click", function (e)
//{
//    
//    if (this.id == '') {
//        return false;
//    }
//    else {
//        $.ajax({
//            type: 'POST',
//            url: $app.baseUrl + 'Company/ClickModule',
//            contentType: 'application/json; charset=utf-8',
//            data: JSON.stringify({ actionVal: this.id }),
//            // dataType: 'json',
//            success: function (data) {
//                console.log(data);
//                if (data.Status) {

//                    window.location.href = data.result;
//                }

//            },
//            error: function (data) {
//                $('#dvError').removeClass('nodisp');
//                $('#lblError').text('There is some error.Please try again later.');
//            }
//        });
//    }
//    //deActive all submenu
//    $('.wraplist .sub-menu li>a').each(function () {
//        $('#' + $(this).attr('id')).removeClass('active');
//    });

//    //active selected submenu
//    $('#' + this.id).addClass('active');
//});
///testing


$('#topmenu li>a').on("click", function (e) {
    debugger;
//$('#topmenu').on("click", function (e) {
     debugger;
    if (this.id == '') {
        return false;
    }
    else {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl +'Company/DoModule',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ actionVal: this.id }),
            // dataType: 'json',
            success: function (data) {
                debugger;
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


$(document).ready(function () {
    $payroll.initDatetime();
   

});


var $payroll = {
    ImportTableName:'',
    click:0,
    initDatetime: function () {

        $('.datepicker').datepicker({
            calendarWeeks: true,
            changeYear: true,
            changeMonth: true,
            autoclose: true,
            todayHighlight: true,
            dateFormat: 'dd/M/yy',
            yearRange: "-120:+2",
            beforeShowMonth: function (date) {
                //                switch (date.getMonth()) {
                //                    case 8:
                //                        return false;
                //                }
            },
            //beforeShow: function (input, inst) {
            //    setTimeout(function () {
            //        inst.dpDiv.css({
            //            marginTop: (-textbox.offsetHeight) + 'px',
            //            marginLeft: textbox.offsetWidth + 'px'
            //        });
            //    }, 0);
            //},
            toggleActive: true
        });
      
         $('.datepickerDOB').datepicker({
            calendarWeeks: true,
            changeYear: true,
            changeMonth: true,
            autoclose: true,
            todayHighlight: true,
            dateFormat: 'dd/M/yy',
            yearRange: (parseInt(new Date().getFullYear()) - 120).toString() + ":" + (parseInt(new Date().getFullYear()) - 18).toString(),
            beforeShowMonth: function (date) {
                //                switch (date.getMonth()) {
                //                    case 8:
                //                        return false;
                //                }
            },
            toggleActive: true
        });
    },
    initMonthYear: function () {
        $('.Monthyearpicker').datepicker({
            calendarWeeks: false,
            autoclose: true,
            dateFormat: 'MM yy',
            beforeShowMonth: function (date) {

            },
            toggleActive: true
        });
    },
    GetFullMonthName: function (id) {
        var monthsName = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        return monthsName[id - 1];
    },
    GetMonthName: function (yearNumber) {
        var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
   'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        return months[yearNumber - 1];
    },
    CallControler: function (context) {
       
        //comment by mubarak in order to solve the issue in leave module
        //$compView.GetCompanyData();
        debugger;
        $app.showProgressModel();
        $payroll.ImportTableName = context.text;
        var id = context.id;
        var importModel = '';
        if (id.indexOf('Import') >= 0) {
            importModel = id;
            id = 'Import';
        }
        if (id.indexOf('DeclarationDataImp') >= 0) {
            importModel = id;
            id = 'TaxImport';
        }
        var booksDiv = $("#renderViews");
        $.ajax({
            cache: false,
            type: "GET",
            url: $app.baseUrl + "Company/Do",
            data: { "actionVal": id },
            success: function (data) {
                booksDiv.html('');
                booksDiv.html(data);
                $app.hideProgressModel();
               
                switch (id) {
                    case "Company":
                        $compView.loadInitial();
                        break;
                    case "Employee":
                        $employee.loadInitial();
                        break;
                    case "Category":
                        $category.LoadCategories();
                        break;
                    case "JoiningDocument":
                        $joinDoc.LoadJoinDoc();
                        break;
                    case "Popup":
                        $comPopup.init();
                        break;
                    case "Entitymodel":
                        $entitymodel.loadTableCategory();
                        break;
                    case "HrComponent":
                        $HRComponent.LoadHRComponents();
                        break;
                    case "DynamicEntity":
                        $dyanmicEntity.LoadEntityModelDrop();
                        break;
                    case "LoanEntry":
                        $LoanEntry.initpage();
                        break;
                    case "LoanMaster":
                        $LoanMaster.LoadLoanMaster();
                        break;
                    case "Separation":
                        $Separation.loadInitial();
                        break;
                    case "Release":
                        $Release.loadInitial();
                        break;
                    case "StopPayment":
                        $StopPayment.loadInitial();
                        break;
                    case "Role":
                        $Role.LoadRole();
                        break;
                    case "User":
                        $User.LoadUser();
                        break;
                    case "FormRights":
                        $formRights.loadInitial();
                        break;
                    case "PTax":
                        $PTax.loadInitial();
                        break;
                    case "Lwf":
                        $lwf.loadInitial();
                    case "Setting":
                        $setting.loadInitial(); //LoadEntityModelDrop();
                        break;
                    case "PayslipSetting":
                        $Payslipsetting.LoadPayslipsetting('Payslip');
                        break;
                    case "PremiumSetting":
                        $PremiumSetting.loadpremiumsetting();
                        break;
                    case "MonthlyInput":
                        $monthlyInput.LoadEntityModelDrop();
                        break;
                    case "PayrollProcess":
                        $payrollHistroy.LoadEntityModelDrop();
                        break;
                    case "tdsForm16PartB":
                        $payrollHistroy.LoadEntityModelDrop();
                        break;
                    case "Increment":
                        $increment.loadInitial();
                        break;
                    case "FullFinalSettlement":
                        $FullFinalSettlement.loadInitial();
                        break;                   
                    case "lopCredit":
                        $companyCom.loadCategory({ id: 'sltCategorylist' });
                        $('#renderViews .title').text('LOP Credit Days');
                        $lop.settingFor = "LOP";

                        break;
                    case "supplementaryDays":
                        $companyCom.loadCategory({ id: 'sltCategorylist' });
                        $('#divEmp').addClass('nodisp');
                        $('#renderViews .title').text('Supplementary Days');
                        $lop.settingFor = "SUPPLEMENTARY";

                        break;
                    case "RoleFormCammandSetting":
                        $RoleFormCommandSetting.loadInitial();
                        break;
                    case "Import":
                        break;
                    case "financeYear":
                        $financeYear.LoadfinanceYears();
                        break;
                    case "slab":
                        $Slab.loadInitial();
                        break;
                    case "Resignation":
                        $Resignation.loadInitial();
                        break;
                    default:

                        break;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $app.hideProgressModel();
                $app.showAlert('Failed to retrieve data.', 4);
            }
        });
      
    },
    loadMonth: function (dropControl) {
        var msg = [];
        msg.push({ id: 1, name: 'January' });
        msg.push({ id: 2, name: 'February' });
        msg.push({ id: 3, name: 'March' });
        msg.push({ id: 4, name: 'April' });
        msg.push({ id: 5, name: 'May' });
        msg.push({ id: 6, name: 'June' });
        msg.push({ id: 7, name: 'July' });
        msg.push({ id: 8, name: 'August' });
        msg.push({ id: 9, name: 'September' });
        msg.push({ id: 10, name: 'October' });
        msg.push({ id: 11, name: 'November' });
        msg.push({ id: 12, name: 'December' });
        $('#' + dropControl.id).html('');
        $.each(msg, function (index, blood) {
            $('#' + dropControl.id).append($("<option></option>").val(blood.id).html(blood.name));
        });
    },
    loadEntityBehaviorType: function (dropControl) {
        var msg = [];
        msg.push({ id: 1, name: 'Master Input' });
        msg.push({ id: 2, name: 'Monthly Input' });
        msg.push({ id: 3, name: 'Percentage' });
        msg.push({ id: 4, name: 'Conditional' });
        msg.push({ id: 5, name: 'Range' });
        $('#' + dropControl.id).html('');
        $.each(msg, function (index, blood) {
            $('#' + dropControl.id).append($("<option></option>").val(blood.id).html(blood.name));
        });
    },
    loadRounding: function (dropControl) {
        var msg = [];
        msg.push({ id: 1, name: 'NORMAL' });
        msg.push({ id: 2, name: '>1RUPEE' });
        msg.push({ id: 3, name: '<1RUPEE' });
        msg.push({ id: 4, name: '50 PAISE' });
        msg.push({ id: 5, name: '>50 PAISE' });
        msg.push({ id: 6, name: '<50 PAISE' });
        msg.push({ id: 7, name: '10 PAISE' });
        msg.push({ id: 8, name: '>10 PAISE' });
        msg.push({ id: 9, name: '5 PAISE' });
        msg.push({ id: 10, name: '>5 PAISE' });
        msg.push({ id: 11, name: '5 RUPEES' });
        msg.push({ id: 12, name: '>5 RUPEES' });
        $('#' + dropControl.id).html('');
        $.each(msg, function (index, blood) {
            $('#' + dropControl.id).append($("<option></option>").val(blood.id).html(blood.name));
        });
    },
    getTreeglyphOpts: function () {
        glyph_opts = {
            map: {
                doc: "glyphicon glyphicon-file",
                docOpen: "glyphicon glyphicon-file",
                checkbox: "glyphicon glyphicon-unchecked",
                checkboxSelected: "glyphicon glyphicon-check",
                checkboxUnknown: "glyphicon glyphicon-share",
                dragHelper: "glyphicon glyphicon-play",
                dropMarker: "glyphicon glyphicon-arrow-right",
                error: "glyphicon glyphicon-warning-sign",
                expanderClosed: "glyphicon glyphicon-plus-sign",
                expanderLazy: "glyphicon glyphicon-plus-sign",  // glyphicon-expand
                expanderOpen: "glyphicon glyphicon-minus-sign",  // glyphicon-collapse-down
                folder: "glyphicon glyphicon-folder-close",
                folderOpen: "glyphicon glyphicon-folder-open",
                loading: "glyphicon glyphicon-refresh glyphicon-spin"
            }
        };
        return glyph_opts;
    }

};
