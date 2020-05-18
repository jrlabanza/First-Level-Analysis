using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FLA.Controllers;

namespace FLA.Models
{
		public class FLAMod
		{
			//return value is the data type. ex: creates boolean = public boolean 

			public Boolean fla_form(string problem_description, string machine_model, string machine_no, string handler_id, string handler_pf ,string package, string details_of_the_problem, string machine_status_and_remarksdate, string capa_and_recommendations, string user, string product_name, string site, string loadboard_name, string loadboard_no)
			{
				string query = "INSERT INTO fla_form(problemDescription, machineModel, machineNo, handler_id, handler_model, package, problemDetails, machineStatus, CAPAandRecommendations,submitter,productName,site,loadboardName,loadboardNo) " +
                                "VALUES('" + problem_description + "', '" + machine_model + "','" + machine_no + "','" + handler_id + "','" + handler_pf + "','" + package + "','" + details_of_the_problem + "','" + machine_status_and_remarksdate + "','" + capa_and_recommendations + "','" + user + "','" + product_name + "','" + site + "','" + loadboard_name + "','" + loadboard_no + "')";

				Boolean results = Connection.ExecuteThisQuery(query, "Insert FLA Form", Connection.fla_connString);

				return results;
			}

            public string getLastInputID()
            {
                IDictionary<string, string> results = new Dictionary<string, string>();

                string query = "SELECT * FROM fla.fla_form WHERE isDeleted = 0 ORDER BY id DESC LIMIT 1;";

                results = Connection.GetDataArray(query, "Get last ID", Connection.fla_connString);

                if (results.Count > 0)
                {
                    return results["id"].ToString();
                }

                return "0";
            }
            public Boolean containment_action(string flaId, string what, string who, string when, string remarks)
            {
                string query = "INSERT INTO containment_action(flaID, what, who, fla_when, remarks) " +
                                "VALUES('" + flaId + "', '" + what + "','" + who + "','" + when + "','" + remarks + "')";

                Boolean results = Connection.ExecuteThisQuery(query, "Insert FLA Form", Connection.fla_connString);

                return results;
            }
            public Boolean root_cause_analysis(string flaId, string machine, string outflow, string systemic_root_cause)
            {
                string query = "INSERT INTO root_cause_analysis(flaID, machine, outflow, systemic_root_cause) " +
                                "VALUES('" + flaId + "','" + machine + "', '" + outflow + "','" + systemic_root_cause +"')";

                Boolean results = Connection.ExecuteThisQuery(query, "Insert FLA Form", Connection.fla_connString);

                return results;
            }
            public Boolean upload_image(string flaId, string pic_name, string hash_name)
            {
                string query = "INSERT INTO pictures_and_illustrations(flaID, pic_name, hash_name) " +
                                "VALUES('" + flaId + "','" + pic_name + "', '" + hash_name + "')";

                Boolean results = Connection.ExecuteThisQuery(query, "Insert FLA Form", Connection.fla_connString);

                return results;
            }

            public List<IDictionary<string, string>> get_all_fla_inputs()
            {
                List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

                string query = "SELECT * FROM fla.fla_form WHERE isDeleted = 0 ORDER BY fla_date DESC;";

                results = Connection.GetDataAssociateArray(query, "Get All Data", Connection.fla_connString);

                return results;
            }

            public IDictionary<string, string> add_open_count(int count, int id)
            {

                int newcount = count + 1;

                IDictionary<string, string> results = new Dictionary<string, string>();

                string query = "UPDATE fla.fla_form SET open_count = " + newcount + " WHERE id =" + id + " AND isDeleted = 0;";

                results = Connection.GetDataArray(query, "Get All Data", Connection.fla_connString);

                return results;
            }

            public IDictionary<string, string> show_fla_data(int id)
            {
                IDictionary<string, string> results = new Dictionary<string, string>();

                string query = "SELECT * FROM fla.fla_form WHERE id ="+ id +" AND isDeleted = 0;";

                results = Connection.GetDataArray(query, "Get All Data", Connection.fla_connString);

                return results;
            }

            public List<IDictionary<string, string>> show_containment_action(int id)
            {
                List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

                string query = "SELECT * FROM fla.containment_action WHERE flaID =" + id + ";";

                results = Connection.GetDataAssociateArray(query, "Get All Data", Connection.fla_connString);

                return results;
            }

            public List<IDictionary<string, string>> show_imgs(int id)
            {
                List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

                string query = "SELECT * FROM fla.pictures_and_illustrations WHERE flaID =" + id + ";";

                results = Connection.GetDataAssociateArray(query, "Get All Data", Connection.fla_connString);

                return results;
            }

            public List<IDictionary<string, string>> show_root_cause(int id)
            {
                List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

                string query = "SELECT * FROM fla.root_cause_analysis WHERE flaID =" + id + ";";

                results = Connection.GetDataAssociateArray(query, "Get All Data", Connection.fla_connString);

                return results;
            }
            // promis connection
            public List<IDictionary<string, string>> show_machine_model(int id)
            {
                List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

                string query = "SELECT DISTINCT TesterPF FROM p1_equipt_db.testers";

                results = Connection.GetDataAssociateArray(query, "Get All Machine Model", Connection.promis_connString);

                return results;
            }

            public List<IDictionary<string, string>> show_machine_no(int id)
            {
                List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

                string query = "SELECT DISTINCT id,TesterID FROM p1_equipt_db.testers";

                results = Connection.GetDataAssociateArray(query, "Get All Machine Model", Connection.promis_connString);

                return results;
            }

            public IDictionary<string, string> get_machine_pf_promis(string id)
            {
                IDictionary<string, string> results = new Dictionary<string, string>();

                string query = "SELECT TesterPF,pkg_type FROM p1_equipt_db.testers WHERE TesterID ='" + id + "';";

                results = Connection.GetDataArray(query, "Get Machine PF", Connection.promis_connString);

                return results;
            }

            public IDictionary<string, string> get_machine_pf_onload(string macID)
            {
                IDictionary<string, string> results = new Dictionary<string, string>();

                string query = "SELECT TesterPF FROM p1_equipt_db.testers WHERE Tester_ID ='" + macID + "';";

                results = Connection.GetDataArray(query, "Get Machine PF", Connection.promis_connString);

                return results;
            }

            public IDictionary<string, string> get_handler_pf(string handler_pf)
            {
                IDictionary<string, string> results = new Dictionary<string, string>();

                string query = "SELECT Equipt_Model FROM p1_equipt_db.equipt_list WHERE Equipt_ID ='" + handler_pf + "';";

                results = Connection.GetDataArray(query, "Get Machine PF", Connection.promis_connString);

                return results;
            }

            public IDictionary<string, string> get_handler_id(string handler_id)
            {
                IDictionary<string, string> results = new Dictionary<string, string>();

                string query = "SELECT Handler_ID,Status_Desc FROM p1_equipt_db.testers WHERE Tester_ID ='" + handler_id + "';";

                results = Connection.GetDataArray(query, "Get Machine PF", Connection.promis_connString);

                return results;
            }

            //public IDictionary<string, string> get_handler_promis(string handler_id)
            //{
            //    IDictionary<string, string> results = new Dictionary<string, string>();

            //    string query = "SELECT Handler_ID,Status_Desc FROM p1_equipt_db.testers WHERE Tester_ID ='" + handler_id + "';";

            //    results = Connection.GetDataArray(query, "Get Machine PF", Connection.promis_connString);

            //    return results;
            //}

            public List<IDictionary<string, string>> get_all_user()
            {
                List<IDictionary<string, string>> results = new List<IDictionary<string, string>>();

                string query = "select email_address from users ORDER BY first_name ASC";

                results = Connection.GetDataAssociateArray(query, "Get User", Connection.promis_connString);

                return results;
            }

            public IDictionary<string, string> check_user(string user, string pass)
            {
                IDictionary<string, string> results = new Dictionary<string, string>();

                string query = "select email_address,password from users WHERE email_address ='" + user + "'AND password ='" + pass + "'";

                results = Connection.GetDataArray(query, "Check User", Connection.promis_connString);

                return results;
            }
		}
}