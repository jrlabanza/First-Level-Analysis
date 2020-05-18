using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Data.SqlClient;
using FLA.Controllers;

namespace FLA.Models
{
	public class Connection
	{
		//public static BaseController baseControll = new BaseController();

        //public static string fla_connString = "Database=fla; Data Source=localhost;User Id=root;Password=;Min Pool Size=0;Max Pool Size=500;Connection Timeout=90;";
        public static string fla_connString = ConfigurationManager.ConnectionStrings["fla_connString"].ConnectionString;

        public static string promis_connString = ConfigurationManager.ConnectionStrings["promis_connString"].ConnectionString;

		//public static string Get_valid_datetime_format(string inputDate)
		//{
		//    DateTime oDate = DateTime.ParseExact(inputDate, "yyyy-MM-dd HH:mm tt", null);
		//    return oDate.ToString();
		//}

		//public static string Get_valid_date_format(string inputDate)
		//{
		//    DateTime oDate = DateTime.ParseExact(inputDate, "yyyy-MM-dd", null);
		//    return oDate.ToString();
		//}

		public static long Get_last_inserted_id(string query, string connString)
		{
			IDictionary<string, string> results = new Dictionary<string, string>();

			results = GetDataArray(query, "Get last inserted id", connString);

			long id = 0;
			if (results.Count > 0)
			{
				long.TryParse(results["id"].ToString(), out id);
			}

			return id;
		}

        public static Boolean TestConnection(string connString)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connString))
				{
					conn.Open();
					if (conn.State == ConnectionState.Open)
					{
						conn.Close();
						return true;
					}
					conn.Close();
				}
			}
			catch (Exception ex)
			{
				throw new System.ArgumentException(ex.Message);
				//return ex.Message;
			}

			return false;
		}

        public static Boolean ExecuteThisQuery(string query, string funcName, string connString)
		{
			if (query != "")
			{
				using (MySqlConnection conn = new MySqlConnection(connString))
				{
					using (MySqlCommand command = new MySqlCommand(query, conn))
					{
						try
						{
							conn.Open();
							if (conn.State == ConnectionState.Open)
							{
								int rowsAffected = command.ExecuteNonQuery();

								if (rowsAffected > 0)
								{
									return true;
								}
							}
							else
							{
								throw new System.ArgumentException("Connection failed");
							}
						}
						catch (MySqlException ex)
						{
							throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong><br /><br /> [Func]: " + funcName);
						}
						catch (Exception ex)
						{
							throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong> <br /><br />[Func]: " + funcName);
						}
						finally
						{
							conn.Close();
						}
					}
				}
			}
			return false;
		}

        public static IDictionary<string, string> GetDataArray(string query, string funcName, string connString)
		{
			IDictionary<string, string> records = new Dictionary<string, string>();

			if (query != "")
			{
				using (MySqlConnection conn = new MySqlConnection(connString))
				{
					using (MySqlCommand command = new MySqlCommand(query, conn))
					{
						try
						{
							conn.Open();

							if (conn.State == ConnectionState.Open)
							{
								MySqlDataReader reader = command.ExecuteReader();

								try
								{
									while (reader.Read())
									{
										int numFld = reader.FieldCount;

										for (int i = 0; i < numFld; i++)
										{
											string val = (!reader.IsDBNull(i)) ? reader.GetString(i) : "";
											string key = reader.GetName(i);

											records[key] = val;
										}
										return records;
									}
								}
								catch (MySqlException ex)
								{
									throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong> <br /><br />[Func]: " + funcName);
								}
								catch (Exception ex)
								{
									throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong> <br /><br />[Func]: " + funcName);
								}
								finally
								{
									reader.Close();
									conn.Close();
								}
							}
							else
							{
								throw new System.ArgumentException("Connection failed [Func]: " + funcName);
							}
						}
						catch (MySqlException ex)
						{
							throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong><br /><br /> [Func]: " + funcName);
						}
						catch (Exception ex)
						{
							throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong> <br /><br />[Func]: " + funcName);
						}
					}
				}
			}
			return records;
		}

        public static List<IDictionary<string, string>> GetDataAssociateArray(string query, string funcName, string connString)
		{
			List<IDictionary<string, string>> records = new List<IDictionary<string, string>>();

			if (query != "")
			{
				using (MySqlConnection conn = new MySqlConnection(connString))
				{
					using (MySqlCommand command = new MySqlCommand(query, conn))
					{
						try
						{
							conn.Open();

							if (conn.State == ConnectionState.Open)
							{
								MySqlDataReader reader = command.ExecuteReader();

								try
								{
									while (reader.Read())
									{
										int numFld = reader.FieldCount;

										IDictionary<string, string> rowData = new Dictionary<string, string>();

										for (int i = 0; i < numFld; i++)
										{
											string val = (!reader.IsDBNull(i)) ? reader.GetString(i) : "";
											string key = reader.GetName(i);

											rowData[key] = val;
										}

										records.Add(rowData);
									}
								}
								catch (MySqlException ex)
								{
									throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong><br /><br /> [Func]: " + funcName);
								}
								catch (Exception ex)
								{
									throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong><br /><br /> [Func]: " + funcName);
								}
								finally
								{
									reader.Close();
									conn.Close();
								}
							}
							else
							{
								throw new System.ArgumentException("Connection failed [Func]: " + funcName);
							}
						}
						catch (MySqlException ex)
						{
							throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong><br /><br /> [Func]: " + funcName);
						}
						catch (Exception ex)
						{
							throw new System.ArgumentException(ex.Message + " <br /><br /><strong style='color:red'>[SQL]: " + query + " </strong><br /><br /> [Func]: " + funcName);
						}
					}
				}
			}

			return records;
		}

		//public static string GetHashPassSHA512(string password)
		//{
		//	string hastPass = "";


		//	byte[] byteSourceText = Encoding.ASCII.GetBytes(password);

		//	var SHA512Hast = new SHA512CryptoServiceProvider();

		//	byte[] byteHash = SHA512Hast.ComputeHash(byteSourceText);

		//	foreach (byte b in byteHash)
		//	{
		//		hastPass += b.ToString("x2");
		//	}

		//	return hastPass;
		//}
	}
}