$(document).ready(function () { //table

    var url = window.location.pathname;
    var getmachineID = url.substring(url.lastIndexOf('/') + 1);
    var getCurrent = url.substring(url.lastIndexOf('CreateFLAForm') + 0);
    var createFLAFormCheck = getCurrent.substring(0, 13);
    
    $('.customdate').daterangepicker({
        "singleDatePicker": true,
        "showDropdowns": true,
        "timePicker24Hour": true,
        "timePicker": true,
        "autoApply": true,

        locale: {
            format: "YYYY-MM-DD h:mm"
        }

    },
    function (start, end, label)
    {
        console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
    });

    if (createFLAFormCheck == "CreateFLAForm")
    {

        if (getmachineID != "CreateFLAForm")
        {
            $("input#machine_no").val(getmachineID);
            //alert(getmachineID);
            $.post(

                base_url + "FLA/get_machine_pf_promis",
                {

                    "id": getmachineID

                }, function (data) {

                    console.log(data.TesterPF);

                    $("input.mac_model").val(data.TesterPF);
                    $("input#package").val(data.pkg_type);
                }
            );

            $.post(
                base_url + "FLA/get_handler_id",
                {
                    "handler_id": getmachineID

                }, function (data) {
                    console.log(data.Handler_ID);
                    $('input#handler_id').val(data.Handler_ID);

                    $.post(
                        base_url + "FLA/get_handler_pf",
                        {
                            "handler_pf": data.Handler_ID

                        }, function (data) {
                            console.log(data.Equipt_Model);
                            $('input#handler_pf').val(data.Equipt_Model);
                        }
                     );
                }
             );
        }

        $.post(

               base_url + "FLA/show_machine_promis_no",
               //{
               //    "id": id
               //},
               function (data) {
                  // console.log(data);

                   var display2 = "";

                   $.each(data, function (j, FlaNo) {
                       display2 += "<option data-macID='" + FlaNo.TesterID + "' value='" + FlaNo.TesterID + "'>";
                   });

                   $("#mac_no").html(display2);
               }
        );

        $.post(
                    base_url + "FLA/get_user",
                    function (usr) {
                        var users = '';
                        $.each(usr, function (i, username) {
                            users += "<option class='user' value='" + username.email_address + "'>" + username.email_address + "</option>"
                        });

                        $("#user").append(users);

                    }
         );
    }
});

$(".mac_auto").on("keyup change select", function () {
    
    var selectedMacID = $(this).val().toUpperCase();
    var macID = $("#mac_no option[value='" + selectedMacID + "']").attr("data-macID");
    console.log(selectedMacID);
    $.post(

        base_url + "FLA/get_machine_pf_promis",
        {

            "id": selectedMacID

        }, function (data) {
            console.log(data);
            //var obj = JSON.parse(data);
            //console.log(obj.TesterPF);
            $("input.mac_model").val(data.TesterPF);
            $("input#package").val(data.pkg_type);

        }
    );

    $.post(
        base_url + "FLA/get_handler_id",
        {
            "handler_id": selectedMacID

        }, function (data) {
            console.log(data.Handler_ID);
            $('input#handler_id').val(data.Handler_ID);

            $.post(
                base_url + "FLA/get_handler_pf",
                {
                    "handler_pf": data.Handler_ID

                }, function (data) {
                    console.log(data.Equipt_Model);
                    $('input#handler_pf').val(data.Equipt_Model);
                }
             );
        }
     );

    

});

//$(document).on("click", "#problem_description", function () {
//    alert("test");

//});

$(document).on("click", ".submit_fla", function () {
        
    var problem_description = $("#problem_description").val();
    var machine_model = $("#machine_model").val();
    var machine_no = $("#machine_no").val().toUpperCase();
    var handler_id = $('#handler_id').val();
    var handler_pf = $('#handler_pf').val();
    var package = $("#package").val();
    var product_name = $("#product_name").val();
    var site = $("#site").val();
    var loadboard_name = $("#loadboard_name").val();
    var loadboard_no = $("#loadboard_no").val();
    var date = $("#date").val();
    var details_of_the_problem = $("#details_of_the_problem").val();
    var machine_status_and_remarks = $("#machine_status_and_remarks").val();
    var containmentActionsRowsLen = $(".containment_action_rows").length + 1;
    var containmentActionsDetails = {};
    var user = $("option:selected", "#user").text();
    var pass = $("#pass").val();
    var post_message = "";
    for (var i = 0; i < containmentActionsRowsLen; i++) {
        
        //var ai_no_tmp = "ai_no_" + i;
        var what_tmp = "what_" + i;
        var who_tmp = "who_" + i;
        var when_tmp = "when_" + i;
        var remarks_tmp = "remarks_" + i;
        //var ai_no = $("#" + ai_no_tmp).val();
        var what = $("#" + what_tmp).val();
        var who = $("#" + who_tmp).val();
        var when = $("#" + when_tmp).val();
        var remarks = $("#" + remarks_tmp).val();

        if (what !== "" && who !== ""  && remarks !== "")
        {
            var post_check = "SUCCESS";
            containmentActionsDetails[i] = {
                //"ai_no": ai_no,
                "what": what,
                "who": who,
                "when": when,
                "remarks": remarks
            };
        }
        else {
            var post_check = "FAILED";
            containmentActionsDetails[i] = {
                //"ai_no": "",
                "what": "",
                "who": "",
                "when": when,
                "remarks": ""
            };
        }
    }
   
    var rootCauseRowsLen = $(".root_cause_rows").length + 1;
    var rootCauseDetails = {};

    for (var i = 0; i < rootCauseRowsLen; i++) {

        var machine_tmp = "machine_" + i;
        var outflow_tmp = "outflow_" + i;
        var systemic_root_cause_tmp = "systemic_root_cause_" + i;
        var machine = $("#" + machine_tmp).val();
        var outflow = $("#" + outflow_tmp).val();
        var systemic_root_cause = $("#" + systemic_root_cause_tmp).val();

        if (machine != "" && outflow != "" && systemic_root_cause != "") {
            var post_check = "SUCCESS";
            rootCauseDetails[i] = {
                "machine": machine,
                "outflow": outflow,
                "systemic_root_cause": systemic_root_cause
            };

        }
        else {
            var post_check = "FAILED";
            rootCauseDetails[i] = {
                "machine": "",
                "outflow": "",
                "systemic_root_cause": ""
            };
        }
    }
    
    var capa_and_recommendations = $("#capa_and_recommendations").val();

    var FLA_IN_DATA = {
        "problem_description": problem_description,
        "machine_model": machine_model,
        "machine_no": machine_no,
        "handler_id": handler_id,
        "handler_pf": handler_pf,
        "date":date,
        "package": package,
        "product_name": product_name,
        "site": site,
        "loadboard_name": loadboard_name,
        "loadboard_no": loadboard_no,
        "details_of_the_problem": details_of_the_problem,
        "machine_status_and_remarks": machine_status_and_remarks,
        "capa_and_recommendations": capa_and_recommendations,
        "containmentActionsDetails": containmentActionsDetails,
        "rootCauseDetails": rootCauseDetails,
        "user": user
    };

    function zipFile(fileLocation, lastId) {

        var zipnotify = $.notify('<strong>Uploading Files</strong> Do not close this page...', {
            allow_dismiss: false,
            showProgressbar: false,
            delay: 0
        });
        var datetime = new Date;
        var zip = new JSZip();
        zip.file("Extract Files plz.txt", "sThank you for reading... now move along\n");

        for (var i = 0; i < $(fileLocation)[0].files.length; i++)

        {
            zip.file($(fileLocation)[0].files[i].name, $(fileLocation)[0].files[i], { base64: true });
        }

        zip.generateAsync({
            type: "blob",
            compression: "DEFLATE",
            compressionOptions: {
                level: 9
            }
        }).then(function (content)
        {
            var files = new File([content], "name.zip");

            var formData = new FormData();
            formData.append('fileZip', files);
            formData.append('lastId', lastId);

            $.ajax({
                data: formData,
                url: base_url + "FLA/testing",
                type: 'POST',
                processData: false,
                contentType: false,
                success: function (response) {
                    //alert("success");
                    console.log(response);
                    zipnotify.update({
                        title: '<strong>Success!</strong>',
                        message: "Data has been inputted. Returning to Dashboard",
                        type: 'success'
                    });
                    setTimeout(function () {
                        $.notifyClose('top-right');
                        window.location.href = base_url + "FLA/Index";
                    }, 3000);
                }
            });

        });
    }

   

    var fileLen = $("input#flaImageFile")[0].files.length;

    // Post 
    //if (fileLen != 0) {
        $.post(

            base_url + "FLA/hash_test",
            {
                "pass": pass
            },
            function (password) {

                $.post(
                    base_url + "FLA/check_user",
                    {
                        "user": user,
                        "pass": password
                    },
                    function (access) {

                        if (access.email_address != null) {

                            //if (track == "" || track == "N/A") {
                            //    alert('no track input');
                            //}
                            //else {
                            //    alert('track capable');
                            //    $.post(
                            //        base_url + "promis/track_lot",
                            //        FOL_DATA_UPDATE,
                            //        function (track) {
                            //            console.log(track);
                            //        }
                            //    );
                            //}

                            if (post_check == "SUCCESS") {

                                $.post(
                                    base_url + "FLA/InsertFLAData",
                                        FLA_IN_DATA,
                                        function (data) {
                                            console.log(data);
                                            var lastId = data.FLADataLastID;
                                            var fileLen = $("input#flaImageFile")[0].files.length;
                                            if (fileLen != 0) {
                                                zipFile("input#flaImageFile", lastId);
                                            }
                                            else {
                                                $.notify({
                                                    title: '<strong>Success!</strong>',
                                                    message: "Data has been inputted. Returning to Dashboard",
                                                    type: 'success'
                                                });
                                                setTimeout(function () {
                                                    $.notifyClose('top-right');
                                                    window.location.href = base_url + "FLA/Index";
                                                }, 3000);
                                            }

                                            //var formData = new FormData();
                                            //var fileLen = $("input#flaImageFile")[0].files.length;

                                            //if (fileLen > 0) {

                                            //    formData.append("fileLen", fileLen);

                                            //    for (var i = 0; i < fileLen; i++) {
                                            //        nameTmp = "fla_" + i;
                                            //        formData.append(nameTmp, $("input#flaImageFile")[0].files[i]);

                                            //    }
                                            //    formData.append("lastId", lastId);
                                            //}

                                            //var request = $.ajax({
                                            //    url: base_url + "FLA/UploadImage",
                                            //    type: "post",
                                            //    data: formData,
                                            //    contentType: false,
                                            //    cache: false,
                                            //    processData: false
                                            //});

                                            //request.done(function (data) {

                                            //    $.notify({
                                            //        title: '<strong>Success!</strong>',
                                            //        message: "Data has been inputted. Returning to Dashboard"
                                            //    }, {
                                            //        type: 'success'
                                            //    });
                                            //    setTimeout(function () {
                                            //        $.notifyClose('top-right');
                                            //        window.location.href = base_url + "FLA/Index";
                                            //    }, 3000);
                                            //});
                                        }

                                    );
                            }

                            else if (post_check == "FAILED") {
                                $.notify({
                                    title: '<strong>Warning!</strong>',
                                    message: "Complete Containment Action/Root Cause and Analysis"
                                }, {
                                    type: 'danger'
                                });
                                setTimeout(function () {
                                    $.notifyClose('top-right');
                                }, 3000);

                            }
                        }
                        else {
                            $.notify({
                                title: '<strong>Warning!</strong>',
                                message: "Invalid Credentials, Please Try Again!"
                            }, {
                                type: 'danger'
                            });
                            setTimeout(function () {
                                $.notifyClose('top-right');
                            }, 3000);
                        }
                    }
                )

            }
        )
    //}
    //else {
    //    $.notify({
    //        title: '<strong>Warning!</strong>',
    //        message: "PLEASE ATTACH FILE!"
    //    }, {
    //        type: 'danger'
    //    });
    //    setTimeout(function () {
    //        $.notifyClose('top-right');
    //    }, 3000);
    //}
    

});

//add extra columns
var containment_actions_row = 1;

$(document).on("click", "#add_form_containment_action", function (e) {
   
    e.preventDefault();

    var display = "";
    //var temp_ai_no = "ai_no_" + containment_actions_row;
    var temp_what = "what_" + containment_actions_row;
    var temp_who = "who_" + containment_actions_row;
    var temp_when = "when_" + containment_actions_row;
    var temp_remarks = "remarks_" + containment_actions_row;

    display += "<tr id='containment_action_rows_" + containment_actions_row + "' class='containment_action_rows'>";
    //display += "<td><input type='text' id='" + temp_ai_no + "' data-detail-row='" + containment_actions_row + "' class='ai_no form-control' autocomplete='off' required/></td>";
    display += "<td class='col-5'><textarea type='text' id='" + temp_what + "' class='what form-control'  autocomplete='off' rows='5' required></textarea></td>";
    display += "<td class='col-2'><input type='text' id='" + temp_who + "' class='who form-control'  autocomplete='off' required/></td>";
    display += "<td class='col-2'><input type='text' id='" + temp_when + "' class='when form-control customdate'  autocomplete='off' required/></td>";
    display += "<td class='col-2'><textarea type='text' id='" + temp_remarks + "' class='remarks form-control'  autocomplete='off' rows='5' required/></td>";
    display += "<td class='col-1'><button class='btn btn-primary btn-sm remove_form_containment_action' data-detail-row='" + containment_actions_row + "'><i class='fa fa-minus'></i></button></td>";
    display += "</td>";
    display += "</tr>";
    console.log(display);

    $("#append_containment_actions").append(display);

    //console.log(rack_in_lot_details_row);

    containment_actions_row += 1;

    $('.customdate').daterangepicker({
        "singleDatePicker": true,
        "showDropdowns": true,
        "timePicker24Hour": true,
        "timePicker": true,
        "autoApply": true,
        locale: {
            format: "YYYY-MM-DD h:mm"
        }
    }, function (start, end, label) {
        console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
    });

});

$(document).on("click", ".remove_form_containment_action", function (e) {

    e.preventDefault();

    console.log(containment_actions_row);

    var row = $(this).attr("data-detail-row");

    $("#append_containment_actions tr#containment_action_rows_" + row).remove();


    var rowsLen = $(".containment_action_rows").length;

    for (var i = 0; i <= rowsLen; i++) {

        var temp = i + 1;

        $(".containment_action_rows").eq(i).removeAttr("id").attr("id", "containment_action_rows_" + temp);
        //$(".ai_no").eq(temp).removeAttr("id").attr("id", "ai_no_" + temp);
        //$(".ai_no").eq(temp).removeAttr("data-detail-row").attr("data-detail-row", "ai_no_" + temp);
        $(".what").eq(temp).removeAttr("id").attr("id", "what_" + temp);
        $(".who").eq(temp).removeAttr("id").attr("id", "who_" + temp);
        $(".when").eq(temp).removeAttr("id").attr("id", "when_" + temp);
        $(".remarks").eq(temp).removeAttr("id").attr("id", "remarks_" + temp);
        $(".remove_form_containment_action").eq(i).removeAttr("data-detail-row").attr("data-detail-row", temp);

    }
    containment_actions_row = containment_actions_row - 1;

    //return false;

});

var root_cause_row = 1;

$(document).on("click", "#add_root_cause", function (e) {

    e.preventDefault();

    var display = "";

    var temp_machine = "machine_" + root_cause_row;
    var temp_outflow = "outflow_" + root_cause_row;
    var temp_systemic_root_cause_ = "systemic_root_cause_" + root_cause_row;

    display += "<tr id='root_cause_rows_" + root_cause_row + "' class='root_cause_rows'>";

    display += "<td><input type='text' id='" + temp_machine + "' data-detail-row='" + root_cause_row + "' class='machine form-control' autocomplete='off' required/></td>";
    display += "<td><input type='text' id='" + temp_outflow + "' class='outflow form-control'  autocomplete='off' required/></td>";
    display += "<td><input type='text' id='" + temp_systemic_root_cause_ + "' class='systemic_root_cause form-control'  autocomplete='off' required/></td>";
    display += "<td><button class='btn btn-primary btn-sm remove_form_root_cause' data-detail-row='" + root_cause_row + "'><i class='fa fa-minus'></i></button></td>";
    display += "</td>";
    display += "</tr>";
    console.log(display);

    $("#append_root_cause").append(display);

    //console.log(rack_in_lot_details_row);

    root_cause_row += 1;


});

$(document).on("click", ".remove_form_root_cause", function (e) {

    e.preventDefault();

    console.log(root_cause_row);

    var row = $(this).attr("data-detail-row");

    $("#append_root_cause tr#root_cause_rows_" + row).remove();


    var rowsLen = $(".root_cause_rows").length;

    for (var i = 0; i <= rowsLen; i++) {

        var temp = i + 1;

        $(".root_cause_rows").eq(i).removeAttr("id").attr("id", "root_cause_rows_" + temp);

        $(".machine").eq(temp).removeAttr("id").attr("id", "machine_" + temp);
        $(".machine").eq(temp).removeAttr("data-detail-row").attr("data-detail-row", "machine_" + temp);
        $(".outflow").eq(temp).removeAttr("id").attr("id", "outflow_" + temp);
        $(".systemic_root_cause").eq(temp).removeAttr("id").attr("id", "systemic_root_cause_" + temp);

        $(".remove_form_root_cause").eq(i).removeAttr("data-detail-row").attr("data-detail-row", temp);

    }


    root_cause_row = root_cause_row - 1;

    //return false;

});