$(document).on("click", "#showFDAData", function () {

    var flaDataId = $(this).attr("data-machine-id");
    var open_count = $(this).attr("data-open-count");

    $.post(

           base_url + "FLA/add_open_count",
           {
               "count": open_count,
               "id": flaDataId
           },
           function (open_count)
           {
               console.log(open_count);
               window.location = "ViewFLAForm/" + flaDataId;
           }
       );
});

$(document).ready(function () {
    //view
    var url = window.location.pathname;
   
    if (compare_url_view.substring(0, 11) == "ViewFLAForm") {
        
        var id = url.substring(url.lastIndexOf('/') + 1);
    
        $.post(

           base_url + "FLA/show_fla_data",
           {
               "id": id
           },
           function (data) {
               console.log(data);
               DisplayFLAData_Show(data);
               displayCont(data.id);
               displayImgs(data.id);
               displayRootCause(data.id);
           }
       );
    }
   


});




function DisplayFLAData_Show(data) {
    var display = "";
    console.log(data.fla_date);
    var newDate = moment(data.fla_date).format("YYYY-MM-DD");

    display = "<div class='row'>";
    display += "<div class='col-4'>";
    display += "<div>Problem Description</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Tester Model</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Tester No</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Package</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Submitter</div>";
    display += "</div>";
    display += "</div>";

    display += "<div class='row'>";
    display += "<div class='col-4'>";
    display += "<input id='problem_description' class='form-control form-control-sm' value='" + data.problemDescription + "' disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='machine_model' class='form-control form-control-sm' value='" + data.machineModel + "' disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='machine_no' class='form-control form-control-sm' value='" + data.machineNo + "' disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='package' class='form-control form-control-sm' value='" + data.package + "' disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='date' type='text' class='form-control form-control-sm' value='" + data.submitter + "'disabled/>";
    display += "</div>";
    display += "</div>";

    display += "<div class='row'>";
    display += "<div class='col-2'>";
    display += "<div>Handler ID</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Handler Model</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Product Name</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Site</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Loadboard Name</div>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<div>Loadboard No</div>";
    display += "</div>";
    display += "</div>";

    display += "<div class='row'>";
    display += "<div class='col-2'>";
    display += "<input id='handler_id' class='form-control form-control-sm' value='" + data.handler_id + "' disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='handler_pf' class='form-control form-control-sm' value='" + data.handler_model + "' disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='date' type='text' class='form-control form-control-sm' value='" + data.productName + "'disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='date' type='text' class='form-control form-control-sm' value='" + data.site + "'disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='date' type='text' class='form-control form-control-sm' value='" + data.loadboardName + "'disabled/>";
    display += "</div>";
    display += "<div class='col-2'>";
    display += "<input id='date' type='text' class='form-control form-control-sm' value='" + data.loadboardNo + "'disabled/>";
    display += "</div>";
    display += "</div>";
    display += "<div class='row'>";
    display += "<div class='col-2'>";
    display += "<div>Date Submitted</div>";
    display += "</div>";
    display += "</div>";
    display += "<div class='row'>";
    display += "<div class='col-2'>";
    display += "<input id='date' type='text' class='form-control form-control-sm' value='" + data.fla_date + "'disabled/>";
    display += "</div>";
    display += "</div>";
    display += "<hr>";
    display += "<div class='row'>";
    display += "<div class='col-6'>";
    display += "<div>Details of the Problem</div>";
    display += "<textarea class='form-control' name='details_of_the_problem' rows='5' cols='70' id='details_of_the_problem' disabled>" + data.problemDetails + "</textarea>";
    display += "</div>";
    display += "<div class='col-6'>";
    display += "<div>Machine Status and Remarks</div>";
    display += "<textarea class='form-control' name='machine_status_and_remarks' rows='5' cols='70' id='machine_status_and_remarks' disabled>" + data.machineStatus + "</textarea>";
    display += "</div>";
    display += "</div>";
    display += "<div class='show_cont_act'>";
    display += "</div>";
    display += "<div class='show_imgs row'>";
    display += "</div>";
    display += "<div class='show_root_cause'>";
    display += "</div>";
    display += "<div class='row'>"
    display += "<div class='col-6'>";
    display += "<div>Capa and Recommendations</div>";
    display += "<textarea class='form-control' id='capa_and_recommendations' cols='70' rows='5' disabled>" + data.CAPAandRecommendations + "</textarea>";
    display += "</div>";
    display += "</div>"

    $("#data-body").html(display);

    $('.customdate').daterangepicker({
        "singleDatePicker": true,
        "linkedCalendars": false,
        "showCustomRangeLabel": false,
    }, function (start, end, label) {
        console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
    });

}
function displayCont(id) {

        $.post(

           base_url + "FLA/show_containment_action",
           {
               "id": id
           },
           function (cont_act) {
               console.log(cont_act.length);
               //contTable(cont_act);
              
               var display = "";
               var counter = 1;
               display += "<div class='row'>";
               display += "<div class='col-12'>";
               display += "<div>Containment Actions</div>";
               display += "<table class='table table-hover table-bordered'>";
               display += "<thead>";
               display += "<tr>";
               display += "<th>AI#</th>";
               display += "<th>ACTION DONE</th>";
               display += "<th>WHO</th>";
               display += "<th>WHEN</th>";
               display += "<th>REMARKS</th>";
               display += "</tr>";
               
               $.each(cont_act, function (i, FlaData) {

                   display += "<tr>";
                   display += "<td><span id='ai_no_0' data-detail-row='0' class='ai_no' value='' disabled/>" + counter + "</span></td>";
                   display += "<td class='col-4'><textarea id='what_0' class='form-control form-control-sm what' value='" + FlaData.what + "' rows='5' disabled>" + FlaData.what + "</textarea></td>";
                   display += "<td class='col-2'><input id='who_0' class='form-control form-control-sm who' value='" + FlaData.who + "' disabled/></td>";
                   display += "<td class='col-2'><input id='when_0' type='text' class='form-control form-control-sm when customdate' value='" + FlaData.fla_when + "' disabled/></td>";
                   display += "<td class='col-3'><textarea id='remarks_0' class='form-control form-control-sm remarks' value='" + FlaData.remarks + "' rows='5' disabled>" + FlaData.remarks + "</textarea></td>";
                   display += "</tr>";

                   counter +=  1;
               });
               display += "</thead>";
               display += "<tbody id='append_containment_actions'></tbody>";
               display += "</table>";
               display += "</div>";


               $(".show_cont_act").append(display);
           }
       );

       

}

function displayImgs(id) {

    $.post(

       base_url + "FLA/show_imgs",
       {
           "id": id
       },
       function (imgs) {
           console.log(imgs);
           //contTable(cont_act);

           var display = "";

           display += "<div class='col-6'>";
           display += "<div class='card'>";
           display += "<div class='card-header'>Pictures and Illustrations</div>";
           display += "<div class='card-body'>";
           $.each(imgs, function (i, FlaData) {

               display += "<a class='col-2' target='_blank' href='"+ img_path + FlaData.pic_name + "'>"+ FlaData.hash_name +"</a></br>";

           });
           display += "</div>";
           display += "</div>";
           display += "</div>";


           $(".show_imgs").append(display);
       }
   );



}

function displayRootCause(id) {

    $.post(

       base_url + "FLA/show_root_cause",
       {
           "id": id
       },
       function (root_cause) {
           console.log(root_cause);
           //contTable(cont_act);

           var display = "";

           display += "<div class='row'>";
           display += "<div class='col-12'>";
           display += "<div>Root Cause and Analysis</div>";
           display += "<table class='table table-hover table-bordered'>";
           display += "<thead>";
           display += "<tr>";
           display += "<th>Machine</th>";
           display += "<th>Outflow</th>";
           display += "<th>Systemic Root Cause</th>";
           display += "</tr>";
           $.each(root_cause, function (i, FlaData) {
               display += "<tr>";
               display += "<td><input id='machine_0' class='form-control form-control-sm machine' value='" + FlaData.machine + "' disabled/></td>";
               display += "<td><input id='outflow_0' class='form-control form-control-sm outflow' value='" + FlaData.outflow + "' disabled/></td>";
               display += "<td><input id='systemic_root_cause_0' class='form-control form-control-sm systemic_root_cause' value='" + FlaData.systemic_root_cause + "' disabled/></td>";
               display += "</tr>";
           });
           display += "</thead>";
           display += "<tbody id='append_root_cause'></tbody>";
           display += "</table>";
           display += "</div>";

           display += "</div>";

           $(".show_root_cause").append(display);
       }
   );



}
