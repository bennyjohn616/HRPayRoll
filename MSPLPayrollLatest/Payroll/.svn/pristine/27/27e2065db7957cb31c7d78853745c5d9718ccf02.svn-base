










$("#btnPtaxSave").click(function () {

    var err = 0;
    $(".Reqrd").each(function () {


        if (this.id == "sltPTLocation" || this.id == "sltPTRepFormat") {
            if ($('#' + this.id).val() == "00000000-0000-0000-0000-000000000000") {
                $app.showAlert(this.id == "sltPTLocation" ? 'Please Select Location' : 'Please Select Report Format', 4);
                err = 1;
                $('#' + this.id).focus();
                return false;

            }
        } if (this.id == "sltCalculationMethod" || this.id == "sltPTRepFormat") {


            if ($('#' + this.id).val() == null) {
                $app.showAlert(this.id == "sltCalculationMethod" ? 'Please Select CalculationMethod' : 'Please Select Report Format', 4);
                err = 1;
                $('#' + this.id).focus();
                return false;
            }
        }
    });

    if (err == 0) {
        $PTax.save();
    }

});

$("#sltPTLocation").change(function () {
    if ($("#sltPTLocation").val() == "00000000-0000-0000-0000-000000000000") {
        $("#PTaxDisplay").addClass('nodisp');
        $PTax.PTaxSelectedLocationId = '';
        $PTax.PTaxCalcPeriod = '';
    }
    else {
        $PTax.canSave = true;
        $("#PTaxDisplay").removeClass('nodisp');
        $PTax.PTaxSelectedLocationId = $("#sltPTLocation").val();
        $PTax.PTaxCalcPeriod = $("#sltCalculationMethod").val();
        $PTax.GetPtaxData();
    }
});
$("#sltCalculationMethod").change(function () {
    $PTax.PTaxCalcPeriod = $("#sltCalculationMethod").val();
    $PTax.SetVisible($("#sltCalculationMethod").val());

});
//$("#btnPtaxSave").click(function () {
//    $PTax.save();
//});


$("#txt_RangeFrom,#txt_RangeTo").change(function () {
    $("#tblPTax tbody tr").each(function () {
    });
});



$("#slt1stPartOnceinSixmonths1").change(function () {
    $PTax.MonthLoad();
});

var $PTax = {
    canSave: false,
    PTaxId: '',
    PTaxRangeId: '',
    PTaxSelectedLocationId: '',
    PTaxCalcPeriod: '',
    tableId: '',
    ptaxdeductionmonth1: '',
    ptaxdeductionmonth2: '',
    formData: document.forms["frmPtaxNew"],
    loadInitial: function () {
        $companyCom.loadPTLocation({ id: "sltPTLocation" });
        $companyCom.loadPTLocation({ id: "sltPTRepFormat" });
        $("#PTaxLocGrid").addClass('nodisp');
        $("#slt1stPartOnceinSixmonths1").val(4);
        $("#slt1stPartOnceinSixmonths2").val(9);
        $("#slt2ndPartOnceinSixmonths1").val(10);
        $("#slt2ndPartOnceinSixmonths2").val(3);
        $PTax.MonthLoad();
        $("#slt1stPartPTaxDed").val(8);
        $("#slt2ndPartPTaxDed").val(2);

    },

    MonthLoad: function () {
        $("#slt1stPartOnceinSixmonths2").val($("#slt1stPartOnceinSixmonths1").val() != 12 ? (parseInt($("#slt1stPartOnceinSixmonths1").val()) + 5) > 12 ? parseInt($("#slt1stPartOnceinSixmonths1").val()) + 5 - 12 : parseInt($("#slt1stPartOnceinSixmonths1").val()) + 5 : 5);
        $("#slt2ndPartOnceinSixmonths1").val($("#slt1stPartOnceinSixmonths2").val() != 12 ? (parseInt($("#slt1stPartOnceinSixmonths2").val()) + 1) > 12 ? parseInt($("#slt1stPartOnceinSixmonths2").val()) + 1 - 12 : parseInt($("#slt1stPartOnceinSixmonths2").val()) + 1 : 1);
        $("#slt2ndPartOnceinSixmonths2").val($("#slt2ndPartOnceinSixmonths1").val() != 12 ? (parseInt($("#slt2ndPartOnceinSixmonths1").val()) + 5) > 12 ? parseInt($("#slt2ndPartOnceinSixmonths1").val()) + 5 - 12 : parseInt($("#slt2ndPartOnceinSixmonths1").val()) + 5 : 5);

        $('#slt1stPartPTaxDed').html('');
        var cnt = 1;
        for (i = parseInt($("#slt1stPartOnceinSixmonths1").val()) ; i <= 12; i++) {
            $('#slt1stPartPTaxDed').append($("<option></option>").val($("#slt1stPartOnceinSixmonths1 option:nth-child(" + i + ")").val()).html($("#slt1stPartOnceinSixmonths1 option:nth-child(" + i + ")").text()));
            if (i == 12) {
                i = 0;
            }

            if (cnt == 6) {
                break;
            }
            cnt = cnt + 1;
        }


        $('#slt2ndPartPTaxDed').html('');
        cnt = 1;
        for (i = parseInt($("#slt2ndPartOnceinSixmonths1").val()) ; i <= 12; i++) {
            //var Mvalue = $("#slt1stPartOnceinSixmonths1 option:nth-child(" + i + ")").val();
            //var MText = $("#slt1stPartOnceinSixmonths1 option:nth-child(" + i + ")").val();
            $('#slt2ndPartPTaxDed').append($("<option></option>").val($("#slt2ndPartOnceinSixmonths1 option:nth-child(" + i + ")").val()).html($("#slt2ndPartOnceinSixmonths1 option:nth-child(" + i + ")").text()));
            if (i == 12) {
                i = 0;
            }

            if (cnt == 6) {
                break;
            }
            cnt = cnt + 1;
        }
    },
    SetVisible: function (selectedData) {
        if (selectedData == "Monthly" || selectedData == "SixMont") {
            $("#PTaxDeductionLastMonths").removeClass('nodisp');
            $("#OnceinSixmonths").removeClass('nodisp');
        }
        else {
            $("#PTaxDeductionLastMonths").addClass('nodisp');
            $("#OnceinSixmonths").addClass('nodisp');
        }
    },
    LoadPTaxRange: function (data, isEdit) {
        var delrow = 1;
        var k = 0;
        if (data == null || data.length <= 0 || isEdit) {
            var rowOne = { ptrangeid: '', ptaxid: '', ptrangeRangeFrom: '', ptrangeRangeTo: '', ptrangeRangeAmt: '' };
            data.push(rowOne);
        }
        var dtClientList = $('#tblPTax').DataTable({

            'bPaginate': false,
            'sPaginationType': 'full',
            'sDom': '<"top">rt<"bottom"ip><"clear">',
            columns: [
               { "data": null },
               { "data": "ptrangeRangeFrom" },
               { "data": "ptrangeRangeTo" },
               { "data": "ptrangeRangeAmt" },
               { "data": "ptrangeid" },
               { "data": "ptaxid" },
               { "data": null }
            ],
            "aoColumnDefs": [

                {
                    "aTargets": [0],
                    "sClass": "nodisp",
                    "bSearchable": false,
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html(iRow);
                    }
                },


        {
            "aTargets": [1],
            "sClass": "",
            "bSearchable": false,
            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                var txt = $('<input type="text"  onkeyup="return $validator.moneyvalidation(this.id)" class="Reqrd" onkeypress="return $validator.moneyvalidation(this.id)" id="txt_RangeFrom' + oData.ptrangeid + '" value="' + oData.ptrangeRangeFrom + '" />');
                $(nTd).html(txt);

            }
        },
{
    "aTargets": [2],
    "sClass": "",
    "bSearchable": false,
    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        var txt = $('<input type="text"  onkeyup="return $validator.moneyvalidation(this.id)" class="Reqrd" onkeypress="return $validator.moneyvalidation(this.id)" id="txt_RangeTo' + oData.ptrangeid + '" value="' + oData.ptrangeRangeTo + '" />');
        $(nTd).html(txt);

    }
},
{
    "aTargets": [3],
    "sClass": "",
    "bSearchable": false,
    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        var txt = $('<input type="text"  onkeyup="return $validator.moneyvalidation(this.id)" class="Reqrd" onkeypress="return $validator.moneyvalidation(this.id)" id="txt_RangeAmt' + oData.ptrangeid + '" value="' + oData.ptrangeRangeAmt + '" />');
        $(nTd).html(txt);

    }
},
 {
     "aTargets": [4, 5],
     "sClass": "nodisp",
     "bSearchable": false
 },
      {
          "aTargets": [6],
          "sClass": "actionColumn",
          "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

              var b = $('<a href="#" class="editeButton" id=' + iRow + ' title="Add"><span aria-hidden="true" class="glyphicon glyphicon-plus"></span></button>');
              var c = $('<a href="#" class="deleteButton" id=' + iRow + ' title="Delete"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button>');
              b.button();
              b.on('click', function () {

                  var Errorflag = 0;
                  var rowcount = 0;
                  var RangeFrom = 0.0;
                  var RangeTo = 0.0;
                  var PreAmount = 0.0;
                  $("#tblPTax tbody tr").each(function () {

                      rowcount = rowcount + 1;
                      RangeFrom = parseFloat($(this).find("[id^='txt_RangeFrom']").val());
                      RangeTo = parseFloat($(this).find("[id^='txt_RangeTo']").val());
                      RangeAmount = parseFloat($(this).find("[id^='txt_RangeAmt']").val());
                      PreAmount = parseFloat($(this).prev().find("[id^='txt_RangeAmt']").val());;
                      if (isNaN(RangeFrom) || isNaN(RangeTo) || isNaN(RangeAmount)) {

                          $app.showAlert("Please provied all values!!!", 4);
                          Errorflag = 1;
                      }
                      if (RangeFrom == 0 || RangeTo == 0) {
                          $app.showAlert("Please provied greater than zero value in Slab range!!!", 4);
                          Errorflag = 1;
                      }
                      if (!isNaN(PreAmount)) {
                          if (PreAmount >= RangeAmount) {
                              $app.showAlert("Invalid Ptax Amount", 4);
                              $(this).find("[id^='txt_RangeAmt']").val('');
                              Errorflag = 1;
                          }
                      }


                      if (RangeFrom >= RangeTo) {
                          $app.showAlert("Invalid Slab Range", 4);
                          $(this).find("[id^='txt_RangeTo']").val('');
                          Errorflag = 1;
                      }
                      if (RangeFrom <= parseFloat($(this).prev().find("[id^='txt_RangeTo']").val())) {
                          $app.showAlert("Invalid Slab Range", 4);
                          $app.hideProgressModel();
                          $(this).find("[id^='txt_RangeFrom']").val('');
                          Errorflag = 1;

                      }
                  });

                  if (Errorflag == 0) {
                      var ranges = [];
                      $('#tblPTax tbody tr').each(function (index, rowelemt) {

                          var cnt = 0;
                          var tmpPtax = { ptaxid: $PTax.PTaxId };
                          $(rowelemt).find('td').each(function (ind, tmp) {
                              if (cnt == 4) {
                                  tmpPtax.ptrangeid = $(tmp).text()
                              }
                              else if (cnt == 1) {
                                  tmpPtax.ptrangeRangeFrom = $($(tmp).find('input')).val();

                              }
                              else if (cnt == 2) {
                                  tmpPtax.ptrangeRangeTo = $($(tmp).find('input')).val();


                              }
                              else if (cnt == 3) {
                                  tmpPtax.ptrangeRangeAmt = $($(tmp).find('input')).val();

                              }
                              cnt++;

                          });
                          ranges.push(tmpPtax);


                      });
                      $PTax.LoadPTaxRange(ranges, true);
                      return false;
                  }
              });
              c.button();
              c.on('click', function () {




                  var transRows = $("#tblPTax").dataTable().fnGetNodes();
                  var availablerow = 0;

                  for (i = 0; i < transRows.length; i++) {

                      //if ($(transRows[i]).hasClass('nodisp'));
                      if (delrow < transRows.length) {
                          availablerow = availablerow + 1;
                      }


                  }

                  if (availablerow > 1) {
                      if (confirm('Are you sure ,do you want to delete?')) {

                          //$(this).parent().parent().remove();
                          var tr = $(this).closest('tr');
                          $(tr).find('input').prop('disabled', true);
                          // $(tr).addClass('nodisp');
                          delrow = delrow + 1;

                          if ($(this).closest('td').find('.editeButton').attr('class') == "editeButton") {
                              var row = $(this).parent().parent();
                              var prev = row.prev();
                              prev.find('.editeButton').removeClass('nodisp');
                              $(this).closest('tr').remove();
                          }

                      }
                      return false;
                  }
                  else {
                      $app.showAlert('MINIMUM ONE ROW IS REQUIRED', 4);

                  }


              });
              $(nTd).empty();
              $(nTd).prepend(c, b)
              if (iRow != data.length - 1) {

                  b.addClass('nodisp')
                  $(nTd).prepend(c, b)
              }



          }
      }
            ],
            'aaData': data,
            fnInitComplete: function (oSettings, json) {
                var r = $('#tblPTax tfoot tr');
                r.find('th').each(function () {
                    $(this).css('padding', 8);
                });
                $('#tblPTax thead').append(r);
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

        $PTax.PTaxCalcPeriod = $("#sltCalculationMethod").val();
        var Errorflag = 0;
        var rowcount = 0;
        var RangeFrom = 0.0;
        var RangeTo = 0.0;
        var PreAmount = 0.0;
        $("#tblPTax tbody tr").each(function () {

            rowcount = rowcount + 1;
            RangeFrom = parseFloat($(this).find("[id^='txt_RangeFrom']").val());
            RangeTo = parseFloat($(this).find("[id^='txt_RangeTo']").val());
            RangeAmount = parseFloat($(this).find("[id^='txt_RangeAmt']").val());
            PreAmount = parseFloat($(this).prev().find("[id^='txt_RangeAmt']").val());
            if (isNaN(RangeFrom) || isNaN(RangeTo) || isNaN(RangeAmount)) {

                $app.showAlert("Please provied all  The Range values!!!", 4);
                Errorflag = 1;
            }
            if (RangeAmount > RangeTo) {
                $app.showAlert("You Can't Enter the Professional TAx Amount Greater Than Earning Rage To Amount!!!", 4);
                $(this).find("[id^='txt_RangeAmt']").val('');
                $(this).find("[id^='txt_RangeAmt']").focus();
                Errorflag = 1;
            }
            if (RangeFrom == 0 || RangeTo == 0) {
                $app.showAlert("Please provied greater than zero value in Slab range!!!", 4);
                Errorflag = 1;
            }
            if (!isNaN(PreAmount)) {
                if (PreAmount >= RangeAmount) {
                    $app.showAlert("Invalid Ptax Amount", 4);
                    $(this).find("[id^='txt_RangeAmt']").val('');
                    Errorflag = 1;
                }
            }


            if (RangeFrom >= RangeTo) {
                $app.showAlert("Invalid Slab Range", 4);
                $(this).find("[id^='txt_RangeTo']").val('');
                Errorflag = 1;
            }
            if (RangeFrom <= parseFloat($(this).prev().find("[id^='txt_RangeTo']").val())) {
                $app.showAlert("Invalid Slab Range", 4);
                $app.hideProgressModel();
                $(this).find("[id^='txt_RangeFrom']").val('');
                Errorflag = 1;

            }

        });

        if (Errorflag == 0) {

            if (!$PTax.canSave) {
                return false;
            }
            $PTax.canSave = false;
            $app.showProgressModel();
            var formData = document.forms["frmPtax"];
            if ($PTax.PTaxCalcPeriod == "Monthly" || $PTax.PTaxCalcPeriod == "SixMont") {
                $PTax.ptaxdeductionmonth1 = $("#slt1stPartPTaxDed").val() + "_" + $("#slt1stPartOnceinSixmonths1").val() + "_" + $("#slt1stPartOnceinSixmonths2").val();
                $PTax.ptaxdeductionmonth2 = $("#slt2ndPartPTaxDed").val() + "_" + $("#slt2ndPartOnceinSixmonths1").val() + "_" + $("#slt2ndPartOnceinSixmonths2").val();
            }
            else {
                $PTax.ptaxdeductionmonth1 = '';
                $PTax.ptaxdeductionmonth2 = '';
            }
            var ranges = [];
            $('#tblPTax tbody tr').each(function (index, rowelemt) {

                var cnt = 0;
                var tmpPtax = { ptaxid: $PTax.PTaxId };
                $(rowelemt).find('td').each(function (ind, tmp) {

                    if (cnt == 4) {
                        tmpPtax.ptrangeid = $(tmp).text()
                    }
                    else if (cnt == 1) {
                        tmpPtax.ptrangeRangeFrom = $($(tmp).find('input')).val();
                        tmpPtax.deleted = $($(tmp).find('input')).prop('disabled');
                    }
                    else if (cnt == 2) {
                        tmpPtax.ptrangeRangeTo = $($(tmp).find('input')).val();
                        tmpPtax.deleted = $($(tmp).find('input')).prop('disabled');

                    }
                    else if (cnt == 3) {
                        tmpPtax.ptrangeRangeAmt = $($(tmp).find('input')).val();
                        tmpPtax.deleted = $($(tmp).find('input')).prop('disabled');
                    }
                    //else if (cnt == 5) {
                    //    
                    //    tmpPtax.deleted = $($(tmp).find('input')).prop('disabled');
                    //}
                    cnt++;

                });

                ranges.push(tmpPtax);


            });

            //var ranges = [];
            //$('#tblPTax tbody tr').each(function (index, rowelemt) {
            //    var cnt = 0;
            //    var tmpPtax = { ptaxid: $PTax.PTaxId };
            //    $(rowelemt).find('td').each(function (ind, tmp) {
            //        if (cnt == 4) {
            //            tmpPtax.ptrangeid = $(tmp).text()
            //        }
            //        else if (cnt == 1) {
            //            tmpPtax.ptrangeRangeFrom = $($(tmp).find('input')).val();
            //        }
            //        else if (cnt == 2) {
            //            tmpPtax.ptrangeRangeTo = $($(tmp).find('input')).val();

            //        }
            //        else if (cnt == 3) {
            //            tmpPtax.ptrangeRangeAmt = $($(tmp).find('input')).val();
            //        }
            //        cnt++;

            //    });
            //    ranges.push(tmpPtax);

            //});


            var data = {

                ptaxid: $PTax.PTaxId,
                ptaxptlocation: $PTax.PTaxSelectedLocationId,
                ptaxCalculationtype: $PTax.PTaxCalcPeriod,
                ptaxdeductionmonth1: $PTax.ptaxdeductionmonth1,
                ptaxdeductionmonth2: $PTax.ptaxdeductionmonth2,
                ptaxRegCertificateNo: $("#txtRegCertificateNo").val(),
                ptaxPTOCircleNo: $("#txtPTOCircleNo").val(),
                ptaxpFormNo: $("#sltPTRepFormat").val(),
                jsonPTaxRange: ranges
            };

            $.ajax({
                url: $app.baseUrl + "Company/SavePTax",
                data: JSON.stringify({ dataValue: data }),
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                success: function (jsonResult) {
                    //$app.clearSession(jsonResult);
                    switch (jsonResult.Status) {
                        case true:
                            $PTax.canSave = true;
                            var p = jsonResult.result;

                            $app.showAlert(jsonResult.Message, 2);
                            $app.hideProgressModel();
                            break;
                        case false:
                            $PTax.canSave = true;
                            $app.hideProgressModel();
                            $app.showAlert(jsonResult.Message, 4);
                            break;
                    }
                },
                complete: function () {
                    $PTax.canSave = true;
                }
            });
        } else {
            $app.hideProgressModel();
        }
    },
    //AddInitialize: function () {
    //    var formData = document.forms["frmPtaxRange"];
    //    formData.elements["txtRangeFrom"].value = "";
    //    formData.elements["txtRangeTo"].value = "";
    //    formData.elements["txtProffAmount"].value = "";

    //},
    GetPtaxData: function () {
        $.ajax({
            url: $app.baseUrl + "Company/GetPTaxData",
            data: JSON.stringify({
                'ptaxptlocation': $PTax.PTaxSelectedLocationId
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        var p = jsonResult.result;
                        $PTax.RenderData(p);
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });

    },
    DeleteData: function (data) {
        $.ajax({
            url: $app.baseUrl + "Company/DeletePtaxData",
            data: JSON.stringify({
                'ptrangeid': data.Id
            }),
            dataType: "json",
            contentType: "application/json",
            type: "POST",
            success: function (jsonResult) {
                $app.clearSession(jsonResult);
                switch (jsonResult.Status) {
                    case true:
                        $PTax.LoadPTax();
                        break;
                    case false:
                        $app.showAlert(jsonResult.Message, 4);
                        break;
                }
            },
            complete: function () {

            }
        });
    },
    RenderData: function (data) {
        $PTax.PTaxId = data.ptaxid;
        $($PTax.formData).find('#sltCalculationMethod').val(data.ptaxCalculationtype);
        $($PTax.formData).find('#txtRegCertificateNo').val(data.ptaxRegCertificateNo);
        $($PTax.formData).find('#txtPTOCircleNo').val(data.ptaxPTOCircleNo);
        $($PTax.formData).find('#sltPTRepFormat').val(data.ptaxpFormNo);
        if (data.ptaxCalculationtype == "Monthly" || data.ptaxCalculationtype == "SixMont") {

            var mon1 = data.ptaxdeductionmonth1.split('_');
            var mon2 = data.ptaxdeductionmonth2.split('_');
            if (mon1) {
                $($PTax.formData).find('#slt1stPartPTaxDed').val(mon1[0]);
                $($PTax.formData).find('#slt1stPartOnceinSixmonths1').val(mon1[1]);
                $($PTax.formData).find('#slt1stPartOnceinSixmonths2').val(mon1[2]);
            }
            if (mon2) {
                $($PTax.formData).find('#slt2ndPartPTaxDed').val(mon2[0]);
                $($PTax.formData).find('#slt2ndPartOnceinSixmonths1').val(mon2[1]);
                $($PTax.formData).find('#slt2ndPartOnceinSixmonths2').val(mon2[2]);
            }

        }
        $("#PTaxLocGrid").removeClass('nodisp');
        $PTax.LoadPTaxRange(data.jsonPTaxRange);
        $PTax.SetVisible(data.ptaxCalculationtype);
    }
};