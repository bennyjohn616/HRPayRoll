
var $MiscEntry =
{

    Initial: function () {
        var SYSDate = new Date();
        var SYSMonth = SYSDate.getMonth() + 1;
        var SYSDat = SYSDate.getDate();
        var SYSyear = SYSDate.getFullYear();
        var sysdate1 = new Date(SYSyear, SYSDate.getMonth(), SYSDat);
        var INPMonth = $('#ddMonth').val();
        var INPYear = $('#txtYear').val();
        var InpDate = $declaractionEntry.INPdate;
        var cuofdate = new Date($declaractionEntry.cutoffdate);
        var cuofmonth = cuofdate.getMonth() + 1;

        if ($declaractionEntry.payrollLockRelease == 1) {
            $("#btnSave").hide();
        }
        else {
            if (InpDate >= SYSDat) {
                $("#btnSave").show();
            }
            else {
                $("#btnSave").hide();
            }
        }

        var role = $('#hdnRoleName').val();
        if (role.toUpperCase() != "ADMIN") {
            if (sysdate1 > cuofdate || INPMonth != cuofmonth) {
                $("#btnSave").hide();
            }
        }
    }
}