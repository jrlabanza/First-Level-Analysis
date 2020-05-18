using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FLA.Models;
using System.IO.Compression;
using System.IO;

namespace FLA.Controllers
{
    public class FLA_IN_DATA //POST DATA
    {    
        public string problem_description { get; set; }

        public string machine_model { get; set; }
        public string machine_no { get; set; }
        public string handler_id { get; set; }
        public string handler_pf { get; set; }
        public string package { get; set; }
        public string product_name { get; set; }
        public string site { get; set; }
        public string loadboard_name { get; set; }
        public string loadboard_no { get; set; }
        public string date { get; set; }
        public string user { get; set; }
        public string details_of_the_problem { get; set; }
        public string machine_status_and_remarks { get; set; }
        public string capa_and_recommendations { get; set; }

        public Dictionary<string, Dictionary<string, string>> containmentActionsDetails { get; set; } //if getting a array variable
        public Dictionary<string, Dictionary<string, string>> rootCauseDetails { get; set; }
    }

    public class GET_FORM_ID //POST DATA
    {
        public int id { get; set; }
        public string macID { get; set; }

    }

    public class FLAController : BaseController
    {
        //public string GV_sessionName_employee_data = "employee_data";

        //public string GV_session_ff_id = "ff_id";
        //public string GV_session_first_name = "first_name";
        //public string GV_session_last_name = "last_name";
        //public string GV_session_email = "email";
        //public string GV_session_isLoggedIn = "isLoggedIn";
        //public string GV_session_userType = "userType";
        //public string GV_session_apprvr_prcs_code = "prcsCode";

        //public string GV_log_dir = "~/Views/SysLogs/";
        //public string GV_error_log_filename = DateTime.Now.ToString("yyyy_MM_dd") + ".cshtml";
        //public string GV_error_log_file = "";


        //public IDictionary<string, string> GV_userTypes_list = new Dictionary<string, string>();
        //public IDictionary<string, string> GV_overall_status_list = new Dictionary<string, string>();

        //public ProjectMod projMod = new ProjectMod();

        //public BaseController()
        //{
        //    this.GV_userTypes_list["initiator"] = "1";
        //    this.GV_userTypes_list["manager"] = "2";
        //    this.GV_userTypes_list["process_approver"] = "3";
        //    this.GV_userTypes_list["access_denied"] = "4";

        //    this.GV_overall_status_list["project_saved"] = "0";
        //    this.GV_overall_status_list["project_approval"] = "1";
        //    this.GV_overall_status_list["project_in_progress"] = "2";
        //    this.GV_overall_status_list["project_active"] = "3";
        //    this.GV_overall_status_list["project_release"] = "4";

        //    this.GV_error_log_file = this.GV_log_dir + "" + this.GV_error_log_filename;
        //}

        [HttpPost] //Posting in C#
        [ValidateInput(true)] // Checks if inputs are true
        public JsonResult testing(FormCollection data)
        {
            string timestamp = DateTime.Now.ToString("hh.mm.ss.ffffff");
            string newfilename = timestamp + ".zip";
            int filesLen = Request.Files.Count;
            ////var files = Request.Files[0];
            for (int i = 0; i < filesLen; i++)
            {
                HttpPostedFileBase file = Request.Files[i];

                //var upload_results = this.UploadThisFile(file, "~/Uploads/");
                var path = Path.Combine(Server.MapPath("~/Uploads/"), newfilename);
                file.SaveAs(path);
                FLAObject.upload_image(data["lastId"], newfilename, newfilename);
            }

            return Json(filesLen);

            //return Json(Request.Files, JsonRequestBehavior.AllowGet);
        }


        FLAMod FLAObject = new FLAMod(); //Object for FLA quesry use
        //
        // GET: /FLA/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateFLAForm()
        {
            return View();
        }

        public ActionResult ViewFLAForm()
        {
            return View();
        }

        [HttpPost] //Posting in C#
        [ValidateInput(true)] // Checks if inputs are true

        public JsonResult InsertFLAData(FLA_IN_DATA data)  //backend process
        {
            
            IDictionary<string, string> results = new Dictionary<string, string>(); //for checking if success

            results["done"] = "FALSE";
            results["FLADataLastID"] = "0";
            results["msg"] = "<strong class='error'>You have no permission to update this item (Please login)</strong>";

            if (data.problem_description != "" && data.machine_model != "" && data.machine_no != "" && data.package != "" && data.details_of_the_problem != "" && data.machine_status_and_remarks != "" && data.capa_and_recommendations != "")
            { //if field is populated = true

                //DateTime dateValue = DateTime.Parse(data.date.ToString());
                //string formatForMySql = dateValue.ToString("yyyy-MM-dd HH:mm:ss");

                if (FLAObject.fla_form(data.problem_description, data.machine_model, data.machine_no, data.handler_id, data.handler_pf, data.package, data.details_of_the_problem, data.machine_status_and_remarks, data.capa_and_recommendations, data.user, data.product_name, data.site, data.loadboard_name, data.loadboard_no)) //query push
                {


                    string LastId = FLAObject.getLastInputID(); //get last ID

                    IDictionary<string, string> ArrayData = new Dictionary<string, string>(); //create object ot store array
                    for (int i = 0; i < data.containmentActionsDetails.Count(); i++)
                    {
                        ArrayData = data.containmentActionsDetails[i.ToString()]; //push array to object

                         //post data from jquery and only applicable for array type objects
                        string what = ArrayData["what"];
                        string who = ArrayData["who"];
                        string when = ArrayData["when"];
                        string remarks = ArrayData["remarks"];

                        DateTime dateValue2 = DateTime.Parse(when.ToString());
                        string formatForMySql2 = dateValue2.ToString("yyyy-MM-dd HH:mm:ss");

                        FLAObject.containment_action(LastId, what, who, formatForMySql2, remarks);

                    }

                    for (int i = 0; i < data.rootCauseDetails.Count(); i++)
                    {
                        ArrayData = data.rootCauseDetails[i.ToString()];

                        string machine = ArrayData["machine"];
                        string outflow = ArrayData["outflow"];
                        string systemic_root_cause = ArrayData["systemic_root_cause"];


                        FLAObject.root_cause_analysis(LastId, machine, outflow, systemic_root_cause);

                    }

                    results["done"] = "TRUE";
                    results["FLADataLastID"] = LastId.ToString();
                    results["msg"] = "<strong class='error'>Inserted</strong>";
                    
                }


            }

            return Json(results);
        }

        [HttpPost] //Posting in C#
        [ValidateInput(true)] // Checks if inputs are true
        public JsonResult UploadImage(FormCollection data)
        {
            int filesLen = Request.Files.Count;

            for (int i = 0; i < filesLen; i++)
            {

            HttpPostedFileBase file = Request.Files[i];

            var upload_results = this.UploadThisFile(file, "~/Uploads/");

                if (upload_results["done"] == "TRUE")
                {
                    FLAObject.upload_image(data["lastId"], upload_results["newFileName"], upload_results["origfileName"]);

                }
                else
                {

                }
            }
            return Json(data);
        }

        public JsonResult add_open_count(int count, int id)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            results = FLAObject.add_open_count(count,id);

            return Json(results);
        }

        public JsonResult get_all_fla_inputs()
        {
            List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

            results = FLAObject.get_all_fla_inputs();
            
            return Json(results);
        }

        public JsonResult show_fla_data(GET_FORM_ID data)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            results = FLAObject.show_fla_data(data.id);

            return Json(results);
        }

        public JsonResult show_containment_action(GET_FORM_ID data)
        {
            List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

            results = FLAObject.show_containment_action(data.id);

            return Json(results);
        }

        public JsonResult show_imgs(GET_FORM_ID data)
        {
            List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

            results = FLAObject.show_imgs(data.id);

            return Json(results);
        }

        public JsonResult show_root_cause(GET_FORM_ID data)
        {
            List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

            results = FLAObject.show_root_cause(data.id);

            return Json(results);
        }

        public JsonResult show_machine_promis_model(GET_FORM_ID data)
        {
            List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

            results = FLAObject.show_machine_model(data.id);

            return Json(results);
        }

        public JsonResult show_machine_promis_no(GET_FORM_ID data)
        {
            List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

            results = FLAObject.show_machine_no(data.id);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_machine_pf_promis(string id)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            results = FLAObject.get_machine_pf_promis(id);

            return Json(results);
        }

        public JsonResult get_machine_pf_onload(GET_FORM_ID data)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            results = FLAObject.get_machine_pf_onload(data.macID);

            return Json(results);
        }

        public JsonResult get_handler_id(string handler_id)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            results = FLAObject.get_handler_id(handler_id);

            return Json(results);
        }

        public JsonResult get_handler_pf(string handler_pf)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            results = FLAObject.get_handler_pf(handler_pf);

            return Json(results);
        }

        //Validation Only
        public JsonResult get_user()
        {
            List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

            results = FLAObject.get_all_user();

            return Json(results);
        }

        public JsonResult hash_test(string pass)
        {

            var results = this.GetHashMD5(pass);
            return Json(results);
        }

        public JsonResult check_user(string user, string pass)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            results = FLAObject.check_user(user, pass);

            return Json(results);

        }
    }
}

        //IGNORE BELOW

        //[HttpPost]
        //public JsonResult Insert_npd_worksheets_files(FormCollection data)
        //{
        //    IDictionary<string, string> results = new Dictionary<string, string>();
        //    IDictionary<string, IDictionary<string, string>> resultsMsg = new Dictionary<string, IDictionary<string, string>>();
        //    IDictionary<string, string> upload_results = new Dictionary<string, string>();

        //    IDictionary<string, string> dataKeys = new Dictionary<string, string>();

        //    try
        //    {
        //        //this.IsUserLoggedIn() == true

        //        dataKeys["basicInfoID"] = "Basic information id";

        //        Tuple<Boolean, string> form_validation_result = this.form_validation(dataKeys, data);

        //        if (form_validation_result.Item1 == false)
        //        {
        //            results["done"] = "FALSE";
        //            results["msg"] = form_validation_result.Item2.ToString();
        //            resultsMsg["0"] = results;
        //        }
        //        else
        //        {
        //            int filesLen = Request.Files.Count;

        //            int basicInfoID = 0;

        //            Int32.TryParse(data["basicInfoID"], out basicInfoID);

        //            for (int i = 0; i < filesLen; i++)
        //            {
        //                results = new Dictionary<string, string>();

        //                HttpPostedFileBase file = Request.Files[i];

        //                upload_results = this.UploadThisFile(file, "~/App_Data/Uploads/NPDWorksheets");

        //                if (upload_results["done"] == "TRUE")
        //                {
        //                    if (projMod.Insert_NPD_checksheets(basicInfoID, upload_results["newFileName"], upload_results["origfileName"]))
        //                    {
        //                        results["done"] = "TRUE";
        //                        results["msg"] = "<strong class='good'>" + upload_results["msg"] + "</strong>";
        //                        results["origfileName"] = upload_results["origfileName"];
        //                        resultsMsg[i.ToString()] = results;
        //                    }
        //                    else
        //                    {
        //                        results["done"] = "FALSE";
        //                        results["msg"] = "<strong class='error'>Error uploading..., Please contact your admin</strong>";
        //                        results["origfileName"] = upload_results["origfileName"];
        //                        resultsMsg[i.ToString()] = results;
        //                    }
        //                }
        //                else
        //                {
        //                    results["done"] = "FALSE";
        //                    results["msg"] = "<strong class='error'>" + upload_results["msg"] + "</strong>";
        //                    results["origfileName"] = upload_results["origfileName"];
        //                    resultsMsg[i.ToString()] = results;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        results["done"] = "FALSE";
        //        results["msg"] = ex.ToString();

        //        this.LogTransactions("html", this.GV_error_log_file,
        //            ex.Message, "Insert_npd_worksheets_files");
        //    }

        //    return Json(resultsMsg);
        //}

    //}



