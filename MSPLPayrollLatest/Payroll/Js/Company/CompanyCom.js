var $companyCom = {

    loadLanguge: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetLanguages",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.LangId).html(blood.Name));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadRelationship: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetRealtionShips",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                for (var cnt = 0; cnt < dropControl.length; cnt++) {
                    $('#' + dropControl[cnt].id).html('');
                    $('#' + dropControl[cnt].id).append($("<option></option>").val(0).html('--Select--'));
                    $.each(msg, function (index, blood) {
                        $('#' + dropControl[cnt].id).append($("<option></option>").val(blood.id).html(blood.name));
                    });
                }

            },
            error: function (msg) {
            }
        });

    },
    loadBloodGroup: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetBloodGroup",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.BloodGroupName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadEsiLocation: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetEsilocation",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.LocationName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadGrade: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetGrades",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.GradeName));
                });
            },
            error: function (msg) {
            }
        });

    },
    // Created by Ajithpanner on 23/12/2017
    loadYear: function (year) {
        
        var currentYear = (new Date).getFullYear();
        var count;
        $('#' + year.id).html('');
        for (count = -2; count < 5; count++) {
            $('#' + year.id).append($("<option></option>").val(currentYear + count).html(currentYear + count));
        }
        $('#' + year.id).val(currentYear);
    },
    loadCategory: function (dropControl) {
        debugger;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetCategories",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var empid = $('#hdnEmployeeId').val();
                var sessionempid = msg.Message;
                var out = msg.result;
                
                if (sessionempid == "00000000-0000-0000-0000-000000000000") {
                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                    $.each(out, function (index, blood) {
                        $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.Name));
                    });
                }
                else {
                    $.each(out, function (index, blood) {
                        $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.Name));
                    });
                }

            },
            error: function (msg) {
            }
        });

    },
    //------
    loadCompany: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetCompanies",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var out = msg.result;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('0').html('--Select--'));
                $.each(out, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.Name));
                });
            },
            error: function (msg) {
            }
        });

    },
    //-----
    loadManagerEmployee: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetManagerEmployees",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                debugger
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                });
            },
            error: function (msg) {
            }
        });
    },

    loadEmployee: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetEmployees",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                debugger
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                });
            },
            error: function (msg) {
            }
        });
    },
    loadEmployeeWithName: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetEmployeesWithName",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                debugger
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.EmpcodeName));
                });
            },
            error: function (msg) {
            }
        });
    },

    loadFullManager: function (dropControl) {
        debugger
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetUNResignedEmployees",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.EmployeeCode));
                });
            },
            error: function (msg) {
            }
        });
    },

    loadResignedEmployee: function (dropControl) {
        
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetResignedEmployees",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    // $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.EmployeeCode));
                });
            },
            error: function (msg) {
            }
        });
    },
    loadUNResignedEmployee: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetUNResignedEmployees",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    //$('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.EmployeeCode));
                });
            },
            error: function (msg) {
            }
        });
    },
    loadSeparatedEmployee: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetSeparatedEmployees",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg.result, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                });
            },
            error: function (msg) {
            }
        });
    },
    loadSelectiveEmployee: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Employee/GetSelectiveEmployees",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                Condition: dropControl.condi
            }),
            dataType: "json",
            async: false,
            success: function (data) {

                var currempidstatus = data.result.employeeID;
                var msg = data.result.Jsondata;
                var empid = msg.empid;
                var out = msg;
                if (currempidstatus == "00000000-0000-0000-0000-000000000000") {

                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                    $.each(msg, function (index, blood) {

                        $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                    });
                }
                else {
                    $.each(msg, function (index, blood) {

                        if (($('#hdnEmployeeId').val() == blood.empid)) {
                            $('#' + dropControl.id).append($("<option></option>").val(blood.empid).html(blood.empCode));
                        }

                    });
                }

            },
            error: function (msg) {
            }
        });
    },
    /////////testing//////////
    //DeclareValueEmployee: function (data) {
    //    
    //    $.ajax({
    //        type: 'POST',
    //        url: $app.baseUrl + "TaxSection/GetDeclareValue",
    //        contentType: "application/json; charset=utf-8",
    //        data: JSON.stringify({ employeeId: data.Employee, effectiveDate: data.Year+"-"+ data.Month+"-"+"01 00:00:00.000 "}),
    //        dataType: "json",
    //        success: function (jsonResult) {
    //            $app.clearSession(jsonResult);
    //            switch (jsonResult.Status) {
    //                case true:
    //                    var out = jsonResult.result;

    //                    break;
    //                case false:
    //                    $app.showAlert(jsonResult.Message, 4);
    //                    //alert(jsonResult.Message);
    //            }

    //        },
    //        error: function (msg) {
    //        }
    //    });
    //},

    loadBranch: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetBranchs",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.BranchName));
                });
            },
            error: function (msg) {
            }
        });

    },

    loadCostCentre: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetCostcentres",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.CostCentreName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadDesignation: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetDesignations",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.DesignationName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadDepartment: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetDepartments",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.DepartmentName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadLocation: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetLocation",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, Loc) {
                    $('#' + dropControl.id).append($("<option></option>").val(Loc.Id).html(Loc.LocationName));
                });
            },
            error: function (msg) {
            }
        });

    },

    loadAttribute: function () {
        var out = '';
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetAttribute",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                out = msg;
            },
            error: function (msg) {
            }
        });
        return out;
    },


    loadESILocation: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetEsilocation",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, Loc) {
                    $('#' + dropControl.id).append($("<option></option>").val(Loc.Id).html(Loc.LocationName));
                });
            },
            error: function (msg) {

            }
        });

    },


    loadPTLocation: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetPTLocations",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, PTLoc) {
                    $('#' + dropControl.id).append($("<option></option>").val(PTLoc.Id).html(PTLoc.PTLocationName));
                });
            },
            error: function (msg) {
                //alert("al");
            }
        });

    },
    loadESIDespensary: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetESIDespensarys",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.ESIDespensary));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadLoanMaster: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Loan/GetLoanMaster",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                $app.clearSession(msg);
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                switch (msg.Status) {
                    case true:
                        $.each(msg.result, function (index, loan) {
                            $('#' + dropControl.id).append($("<option></option>").val(loan.loanid).html(loan.loanDesc));
                        });
                        break;
                    case false:
                        alert(msg.Message);
                        break;

                }

            },
            error: function (msg) {
            }
        });

    },
    loadFormdata: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetRole",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (msg) {

                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                $.each(msg, function (index, Role) {
                    $('#' + dropControl.id).append($("<option></option>").val(Role.Id).html(Role.DisplayAs));
                });
            },
            error: function (msg) {
                //alert("al");
            }
        });

    },
    loadFormdatafullrole: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Admin/GetFULLRoleList",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (msg) {

                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val(0).html('--Select--'));
                $.each(msg, function (index, Role) {
                    $('#' + dropControl.id).append($("<option></option>").val(Role.Id).html(Role.DisplayAs));
                });
            },
            error: function (msg) {
                //alert("al");
            }
        });

    },
    loadJoinDocdata: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetJoiningDocuments",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {

                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, JoinDoc) {
                    $('#' + dropControl.id).append($("<option></option>").val(JoinDoc.Id).html(JoinDoc.DocumentName));
                });
            },
            error: function (msg) {
                //alert("al");
            }
        });

    },
    loadBank: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetBanks",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, bank) {
                    $('#' + dropControl.id).append($("<option></option>").val(bank.Id).html(bank.BankName));
                });
            },
            error: function (msg) {
                //alert("al");
            }
        });

    },
    loadlopcrdyscmpnent: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "PremiumSetting/GetlopcreditComponents",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (msg) {
                var out = msg.result;

                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(out, function (index, jcmpnt) {
                    $('#' + dropControl.id).append($("<option></option>").val(jcmpnt.Id).html(jcmpnt.displayAs));
                });
            },
            error: function (msg) {
                //alert("al");
            }
        });

    },
    loadPreviousPayrollProcessMonthYear: function (dropControl) {

        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetPreviousPayrollProcessMonthYear",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                Condition: dropControl.condi
            }),
            dataType: "json",
            success: function (msg) {
                var out = msg;
                
                if (dropControl.condi == "Year") {
                    var d = new Date();
                    var Currentyear = parseInt(d.getFullYear());
                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option></option>").val(Currentyear - 1).html(Currentyear - 1));
                    $('#' + dropControl.id).append($("<option></option>").val(Currentyear).html(Currentyear));
                    $('#' + dropControl.id).append($("<option></option>").val(Currentyear + 1).html(Currentyear + 1));
                }

                if (dropControl.condi == "Payroll") {
                    var d = new Date();
                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option selected></option>").val(0).html('--Select--'));
                    if (msg != "")
                        $('#' + dropControl.id).append($("<option></option>").val(msg).html(msg));
                    $('#' + dropControl.id).append($("<option></option>").val("Single Employee").html("Single Employee"));
                    $('#' + dropControl.id).append($("<option></option>").val("All Employees").html("All Employees"));               //    

                    $payrollHistroy.selectedpayrollType = msg;
                }

                if (dropControl.condi == "Month") {
                     $('#' + dropControl.id).val(msg);
                    }


                if (dropControl.condi == "Incometax") {
                    var d = new Date();
                    $('#' + dropControl.id).html('');
                    if ($('#hdnRoleName').val().toUpperCase() != "ADMIN" && dropControl.type == "employee" && dropControl.EmpId != '00000000-0000-0000-0000-000000000000') {
                        $('#' + dropControl.id).append($("<option selected></option>").val("Single Employee").html("Single Employee"));
                        msg = "Single Employee";
                        $taxHistory.selectedpayrollType = msg;
                        
                    }
                    else {
                        $('#' + dropControl.id).append($("<option selected></option>").val(0).html('--Select--'));
                        if (msg != "")
                            $('#' + dropControl.id).append($("<option></option>").val(msg).html(msg));
                        $('#' + dropControl.id).append($("<option></option>").val("Single Employee").html("Single Employee"));
                        $('#' + dropControl.id).append($("<option></option>").val("All Employees").html("All Employees"));
                        $taxHistory.selectedpayrollType = msg;
                    }
                }
                
                if (dropControl.condi != "Payroll") {
                    var selectedVal = msg == "" ? 0 : msg;
                    $('#' + dropControl.id).val(selectedVal);
                }

                if (dropControl.condi == "Incometax") {
                    var selectedVal = msg == "" ? 0 : msg;
                    $('#' + dropControl.id).val(selectedVal);
                }


            },
            error: function (msg) {
                //alert("al");
            }
        });

    },
    loadHRComponent: function (dropControl) {
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetHRComponents",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, HRComp) {
                    $('#' + dropControl.id).append($("<option></option>").val(HRComp.Id).html(HRComp.Name));
                });
            },
            error: function (msg) {
            }
        });

    },
    getDefaultFinanceYear: function () {

        var currentYear;
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "FinanceYear/GetDefaultFinaceYear",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (jsonResult) {

                currentYear = jsonResult.result;
            },
            error: function (msg) {
            }
        });
        return currentYear;
    },

    EmployeeDefaultFinanceYear: function (date2) {
        debugger;
        var currentYear;
        date1 = new Date(date2);
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "FinanceYear/EmployeeDefaultFinaceYear",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ date1 : date1 }),
            dataType: "json",
            async: false,
            success: function (jsonResult) {

                currentYear = jsonResult.result;
            },
            error: function (msg) {
            }
        });
        return currentYear;
    },


    loadSlabDetail: function (dropControl) {


        $.ajax({
            type: 'POST',
            async: false,
            url: $app.baseUrl + "Company/GetPopUpDatas",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ type: "slab" }),
            dataType: "json",
            success: function (result) {
                var out = result;

                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(result.result, function (index, slab) {
                    $('#' + dropControl.id).append($("<option></option>").val(slab.Id).html(slab.popuplalue));
                });
            },
            error: function (msg) {
            }
        });
    },
    loadSection: function (dropControl, parentId, type) {
        var FinancialYrId = $txSection.financeYear;
        if (FinancialYrId != null) {
            $.ajax({
                type: 'POST',
                async: false,
                url: $app.baseUrl + "TaxSection/GetTaxSection",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ financialyearId: FinancialYrId.id, parentId: parentId, type: type }),
                dataType: "json",
                success: function (result) {
                    var out = result;

                    $('#' + dropControl.id).html('');
                    $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                    $.each(result.result, function (index, value) {
                        $('#' + dropControl.id).append($("<option></option>").val(value.id).html(value.name));
                    });
                },
                error: function (msg) {
                }
            });
        }
        else {
            $app.showAlert("Set default Financial Year", 4);
        }
    },
    loadEarningDeduction: function (dropControl, type, takFPF) {
        

        $.ajax({
            type: 'POST',
            async: false,
            url: $app.baseUrl + "Company/GetAttributeModelList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ type: type, takFPF: takFPF }),
            dataType: "json",
            success: function (result) {
                
                var out = result;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                //$.each(result.result[0].AttributeModelList, function (index, value) {
                for (i = 0; i < result.result[0].AttributeModelList.length; i++) {

                    if (result.result[0].AttributeModelList[i].IsIncrement == true || result.result[0].AttributeModelList[i].Name == "FPF") {
                        $('#' + dropControl.id).append($("<option></option>").val(result.result[0].AttributeModelList[i].Id).html(result.result[0].AttributeModelList[i].Name));
                    }
                }
                //});
            },
            error: function (msg) {
            }
        });
    },
    loadOtherExamption: function (dropControl, type) {


        $.ajax({
            type: 'POST',
            async: false,
            url: $app.baseUrl + "FinanceYear/GetOtherExamption",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ type: type }),
            dataType: "json",
            success: function (result) {
                
                var out = result;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(result.result, function (index, value) {
                    $('#' + dropControl.id).append($("<option></option>").val(value.Id).html(value.Name));
                });
            },
            error: function (msg) {
            }
        });
    },
    loadLeaveType: function (dropControl) {
        debugger;
        //$('#FsltDay').prop("disabled", true);
        //$('#TsltDay').prop("disabled", true);
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetLeaveType",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.LeaveTypeName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadMasterLeaveType: function (dropControl) {
        //$('#FsltDay').prop("disabled", true);
        //$('#TsltDay').prop("disabled", true);
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetMasterLeaveType",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.LeaveTypeName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadLeaveTypeforleaverequest: function (dropControl) {
        //$('#FsltDay').prop("disabled", true);
        //$('#TsltDay').prop("disabled", true);
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetLeaveType",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                // $('#' + dropControl.id).append($("<option></option>").val('199F5DB2-14B7-46D3-A0E4-715D56682277').html('Loss of Pay'));
                $.each(msg, function (index, blood) {
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.LeaveTypeName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadLeaveTypeforcompoff: function (dropControl) {
        debugger;
        //$('#FsltDay').prop("disabled", true);
        //$('#TsltDay').prop("disabled", true);
        $.ajax({
            type: 'POST',
            url: $app.baseUrl + "Company/GetLeaveType",
            contentType: "application/json; charset=utf-8",
            data: null,
            dataType: "json",
            async: false,
            success: function (msg) {
                var out = msg;
                $('#' + dropControl.id).html('');
                $('#' + dropControl.id).append($("<option></option>").val('00000000-0000-0000-0000-000000000000').html('--Select--'));
                // $('#' + dropControl.id).append($("<option></option>").val('199F5DB2-14B7-46D3-A0E4-715D56682277').html('Loss of Pay'));
                $.each(msg, function (index, blood) {
                    if(blood.LeaveTypeName=="Comp Off")
                    $('#' + dropControl.id).append($("<option></option>").val(blood.Id).html(blood.LeaveTypeName));
                });
            },
            error: function (msg) {
            }
        });

    },
    loadatozSelect: function () {

        var alpha = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF"];
        var select = '<select><option value=0>--select--</option>'
        for (var cnt = 0; cnt < alpha.length; cnt++) {
            select = select + '<option value=' + alpha[cnt] + '>' + alpha[cnt] + '</option>'
        }
        select = select + '</select>';
        return select;
    }

};