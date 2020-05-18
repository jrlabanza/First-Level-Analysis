using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FLA.Models
{
    public class UsersMod
    {

        public IDictionary<string, string> Get_valid_user(string ff_id)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            string query = "SELECT * FROM ValidUsers WHERE isDeleted=0 AND ffID='" + ff_id + "'";

            results = Connection.GetDataArray(query, "Get valid user", Connection.fla_connString);

            return results;
        }

        public IDictionary<string, string> Get_manager(string ff_id)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            string query = "SELECT * FROM Managers WHERE isDeleted=0 AND ffID='" + ff_id + "'";

            results = Connection.GetDataArray(query, "Get valid user", Connection.fla_connString);

            return results;
        }

        public IDictionary<string, string> Get_process_approver(string ff_id)
        {
            IDictionary<string, string> results = new Dictionary<string, string>();

            string query = "SELECT * FROM ProcessApprovers WHERE isDeleted=0 AND ffID='" + ff_id + "' ORDER BY id DESC LIMIT 1";

            results = Connection.GetDataArray(query, "Get process approvers", Connection.fla_connString);

            return results;
        }

    }
}