$(document).ready(function () {
    
    if (compare_url == "Index" || compare_url == "FLA" || compare_url == "") {
        
        $.post(
            base_url + "FLA/get_all_fla_inputs",
            function (data) {

                console.log(data);
                DisplayFLAData(data);

                $(document).ready(function () {
                    $('.custom').DataTable({
                        paging: true,
                        info: false,
                        searching: true,
                        fixedHeader: true,
                        deferRender: true,
                        "order": [],
                        "columnDefs": [
                        { "type": "date", "targets": 0 }
                        ]
                    });
                });
            }
        );
        

    }
	
});


function DisplayFLAData(data) {
	var display = "";
	var check_num = 0;
	var check_loop = 0;

    
	$.each(data, function (i, FlaData) {
        
	    display += "<tr>";
	    display += "<td>" + FlaData.fla_date + "</td>";
	    display += "<td>" + FlaData.machineNo + "</td>";
	    display += "<td>" + FlaData.machineModel + "</td>";
	    display += "<td>" + FlaData.handler_id + "</td>";
	    display += "<td>" + FlaData.handler_model + "</td>";
	    display += "<td>" + FlaData.problemDetails + "</td>";
	    //display += "<td id='machineStatus_" + check_loop + "'></td>";
	    display += "<td>" + FlaData.CAPAandRecommendations + "</td>";
	    display += "<td id='submitter'>" + FlaData.submitter + "</td>";
	    display += "<td id='open_count'>" + FlaData.open_count + "</td>";
	    display += "<td><a class='btn btn-primary mr-1' id='showFDAData' data-open-count=" + FlaData.open_count + " data-machine-id=" + FlaData.id + " type='button'><i class='fa fa-eye'></i></a>";
	    display += "<a class='btn btn-primary' id='showFDAData' data-machine-id=" + FlaData.id + " type='button' hidden><i class='fa fa-paperclip'></i></a></td>";
	    //display += "<td><a class='btn btn-primary' href='ViewFLAForm' type='button'><i class='fa fa-eye'></i></a></td>";
	    display += "</tr>";
	    

		//$.post(
        //    base_url + "FLA/get_handler_id",
        //    {
        //        "handler_id": FlaData.machineNo
        //    },
        //    function (handlerid) {
        //        $('td#machineStatus_' + check_num).html(handlerid.Status_Desc);
        //        console.log(handlerid);
        //        console.log('td#machineStatus_' + check_num);
        //        check_num += 1;
        //    }
        //);
		//console.log("<td id='machineStatus_" + check_loop + "'></td>");
		//check_loop += 1
		

	});
	$("#fla_list_body").html(display);

}
