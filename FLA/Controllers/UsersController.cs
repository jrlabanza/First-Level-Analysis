using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using FLA.Models;

namespace FLA.Controllers
{
    public class UsersController : BaseController
    {
 
        //public ActionResult Login()
        //{
        //    if (this.IsUserLoggedIn() == true)
        //    {
        //        return RedirectToAction("Dashboard", "Home");
        //    }
        //    else
        //    {
        //        ViewBag.page_id = "login_page";
        //        return View();
        //    }
        //}

        //public ActionResult Logout()
        //{
        //    Session[this.GV_sessionName_employee_data] = null;

        //    return RedirectToAction("Login", "Users");
        //}


        [HttpPost]
        [ValidateInput(true)]
        public JsonResult Login_employee(string ffID, string password)
        {

            IDictionary<string, string> results = new Dictionary<string, string>();
            IDictionary<string, string> employee_data = new Dictionary<string, string>();

            results["done"] = "FALSE";
            results["msg"] = "<strong class='error'>Can't connect to Active Directory server (LDAP)</strong>";

            try
            {
                string ldapAddress = "LDAP://ad.onsemi.com";
                DirectoryEntry directoryEntry = new DirectoryEntry(ldapAddress, ffID, password);

                DirectorySearcher ds = new DirectorySearcher(directoryEntry);

                ds.Filter = "(sAMAccountName=" + ffID + ")";
                ds.SearchScope = SearchScope.Subtree;
                SearchResult rs = ds.FindOne();

                if (rs.GetDirectoryEntry().Properties.Values.Count > 0)
                {
                    string ff_id = rs.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                    string first_name = rs.GetDirectoryEntry().Properties["givenName"].Value.ToString();
                    string last_name = rs.GetDirectoryEntry().Properties["sn"].Value.ToString();
                    string email = rs.GetDirectoryEntry().Properties["mail"].Value.ToString();

                    IDictionary<string, string> validUser = new Dictionary<string, string>();
                    validUser = userMod.Get_valid_user(ff_id);

                    string userType = "";

                    if (validUser.Count > 0)
                    {
                        userType = this.GV_userTypes_list["initiator"];
                    }
                    else
                    {
                        IDictionary<string, string> manager = new Dictionary<string, string>();
                        manager = userMod.Get_manager(ff_id);

                        if (manager.Count > 0)
                        {
                            userType = this.GV_userTypes_list["manager"];
                        }
                        else
                        {
                            IDictionary<string, string> approvers = new Dictionary<string, string>();
                            approvers = userMod.Get_process_approver(ff_id);

                            if (approvers.Count > 0)
                            {
                                userType = this.GV_userTypes_list["process_approver"];

                                // Process code of the approver
                                employee_data.Add(this.GV_session_apprvr_prcs_code, approvers["prcsCode"]);

                            }
                            else
                            {
                                userType = this.GV_userTypes_list["access_denied"]; // Access denied
                            }
                        }
                    }

                    if (userType == this.GV_userTypes_list["access_denied"] || userType == "")
                    {
                        results["msg"] = "<strong class='error'>Access denied</strong>";
                    }
                    else
                    {
                        // User Type
                        // -- 1 - Project Initiator
                        // -- 2 - Area Manager
                        // -- 3 - Process Approver
                        employee_data.Add(this.GV_session_userType, userType);

                        employee_data.Add(this.GV_session_ff_id, ff_id);
                        employee_data.Add(this.GV_session_first_name, first_name);
                        employee_data.Add(this.GV_session_last_name, last_name);
                        employee_data.Add(this.GV_session_email, email);
                        employee_data.Add(this.GV_session_isLoggedIn, "true");

                        Session[this.GV_sessionName_employee_data] = employee_data;

                        results["done"] = "TRUE";
                        results["msg"] = "<strong class='good'>Successfully logged in</strong>";
                    }
                }
            }
            catch (Exception ex)
            {
                results["done"] = "FALSE";
                results["msg"] = "<strong class='error'>" + ex.Message + "</strong>";

                this.LogTransactions("html", this.GV_error_log_file,
                    ex.Message, "Login_employee");
            }

            return Json(results);
        }

    }
}
