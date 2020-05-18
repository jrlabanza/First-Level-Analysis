using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using FLA.Models;

namespace FLA.Controllers
{

	public class BaseController : Controller
	{
		//FLA FORMS
		public string sanitizeInput(string input)
		{
			StringBuilder str_input = new StringBuilder();
			str_input.Append(HttpUtility.HtmlEncode(input));
			return str_input.ToString();
		}

		public FormCollection SanitizeFormCollection(IDictionary<string, string> dataKeys, FormCollection formData)
		{

			string inputName = "";

			foreach (KeyValuePair<string, string> data in dataKeys)
			{
				inputName = data.Key.ToString();

				formData[inputName] = this.sanitizeInput(formData[inputName]);
			}

			return formData;
		}

		public Tuple<Boolean, string> form_validation(IDictionary<string, string> dataKeys, FormCollection formData)
		{
			string errorMsg = "";
			string inputName = "";
			string inputDescription = "";

			Boolean isInputsComplete = true;

			foreach (KeyValuePair<string, string> data in dataKeys)
			{
				inputName = data.Key.ToString();
				inputDescription = data.Value.ToString();

				if (!formData.AllKeys.Contains(inputName))
				{
					errorMsg += "<strong>" + inputDescription + "</strong> is required <br/>";
					isInputsComplete = false;
					return Tuple.Create(isInputsComplete, errorMsg);
				}

				if (formData[inputName] == "")
				{
					errorMsg += "<strong class='error'>" + inputDescription + " is required</strong><br/>";
					isInputsComplete = false;
					return Tuple.Create(isInputsComplete, errorMsg);
				}
			}

			return Tuple.Create(isInputsComplete, errorMsg);

		}

		public string GetHashSHA512(string str)
		{
			string hashStr = "";

			byte[] byteSourceText = Encoding.ASCII.GetBytes(str);

			var SHA512Hast = new SHA512CryptoServiceProvider();

			byte[] byteHash = SHA512Hast.ComputeHash(byteSourceText);

			foreach (byte b in byteHash)
			{
				hashStr += b.ToString("x2");
				//return hashStr;
			}

			return hashStr;

		}

		public string GetHashMD5(string str)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(str);
			Byte[] encodedBytes = md5.ComputeHash(originalBytes);

			string hash = BitConverter.ToString(encodedBytes).Replace("-", "").ToUpper();

			return hash;
		}

		public string GetFileNewFileName(string fileName)
		{
			Random rnd = new Random();
			var nowTime = DateTime.Now.ToString("yyMMddHHmmssffftt");
			int randNum = rnd.Next(1, 1000);

			string newFileName = this.GetHashMD5(fileName + nowTime + randNum.ToString());

			return newFileName;
		}

		public string[] GetValidDocsFileExtensions()
		{
			string[] validExtensions = new string[] {
				"xls", "xlt", "xlm", "xlsx", "xlsm", "xltx",
				"xltm", "xlsb", "xla", "xlam", "xll", "xlw",
				"ppt", "pot", "pps", "pptx", "pptm", "potx",
				"potm", "ppam", "ppsx", "ppsm", "sldx", "sldm",
				"doc", "docm", "docx", "dot", "dotm", "dotx",
				"msg", "pdf", "csv", "txt"
			};

			return validExtensions;
		}

		public string[] GetValidImgFileExtensions()
		{
			string[] validExtensions = new string[] {
				"bmp", "dds", "gif", "jpg", "jpeg", "png",
				"pspimage", "tga", "thm", "tif", "tiff",
				"yuv", "jif", "jfif", "jp2", "jpx", "j2k",
				"j2c", "fpx", "pcd"
			};

			return validExtensions;
		}

		public string[] GetValidDocsMIMETypes()
		{
			string[] validMIMETypes = new string[]{
				"text/csv",
				"text/plain",
				"application/pdf",
				"application/msword",
				"application/vnd.ms-excel",
				"application/vnd.ms-powerpoint",
				"application/vnd.openxmlformats-officedocument.presentationml.presentation",
				"application/vnd.openxmlformats-officedocument.wordprocessingml.document",
				"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
			};

			return validMIMETypes;
		}

		public string[] GetValidImgsMIMETypes()
		{
			string[] validMIMETypes = new string[]{
				"image/png", "image/jpg", "image/jpeg", "image/gif", "image/bmp", "image/dds", "image/pspimage",
				"image/tga", "image/thm", "image/tif", "image/tiff", "image/yuv", "image/jif", "image/jfif",
				"image/jp2", "image/jpx", "image/j2k", "image/j2c", "image/fpx", "image/pcd"
			};

			return validMIMETypes;
		}



		[HttpPost]
		public IDictionary<string, string> UploadThisFile(HttpPostedFileBase file, string uploadPath, string expectedFile = "")
		{
			IDictionary<string, string> results = new Dictionary<string, string>();

			string[] validDocsExtension = this.GetValidDocsFileExtensions();
			string[] validImgsExtension = this.GetValidImgFileExtensions();

			string[] validDocsMIMETypes = this.GetValidDocsMIMETypes();
			string[] validImgsMIMETypes = this.GetValidImgsMIMETypes();

			if (file.ContentLength > 0)
			{
				var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
				var fileName = Path.GetFileName(file.FileName);
				var newFileName = this.GetFileNewFileName(fileName) + "." + fileExt;

				if (file.ContentLength <= 20000000)
				{
					if (expectedFile != "")
					{
						if (fileExt != expectedFile)
						{
							results["done"] = "false";
							results["msg"] = "Invalid document type (expected file is ." + expectedFile + ")";
							results["fileType"] = "";
							results["origfileName"] = "";
							results["newFileName"] = "";

							return results;
						}
					}

					if (validDocsExtension.Contains(fileExt.ToLower())) // DOCS UPLOAD
					{
						var fileContentType = file.ContentType;

						if (validDocsMIMETypes.Contains(fileContentType))
						{
							if (uploadPath != "")
							{
								var path = Path.Combine(Server.MapPath(uploadPath), newFileName);
								file.SaveAs(path);
							}


							results["done"] = "TRUE";
							results["msg"] = "Successfully uploaded!";
							results["fileType"] = "doc";
							results["origfileName"] = fileName;
							results["newFileName"] = newFileName;

						}
						else
						{
							results["done"] = "FALSE";
							results["msg"] = "Invalid document type (Not allowed to upload)";
							results["fileType"] = "";
							results["origfileName"] = fileName;
							results["newFileName"] = newFileName;
						}
					}
					else if (validImgsExtension.Contains(fileExt.ToLower())) // IMAGE UPLOAD
					{
						var fileContentType = file.ContentType;

						if (validImgsMIMETypes.Contains(fileContentType))
						{
							if (uploadPath != "")
							{
								var path = Path.Combine(Server.MapPath(uploadPath), newFileName);
								file.SaveAs(path);
							}

							results["done"] = "TRUE";
							results["msg"] = "Successfully uploaded!";
							results["fileType"] = "img";
							results["origfileName"] = fileName;
							results["newFileName"] = newFileName;

						}
						else
						{
							results["done"] = "FALSE";
							results["msg"] = "Invalid image type (Not allowed to upload)";
							results["fileType"] = "";
							results["origfileName"] = fileName;
							results["newFileName"] = newFileName;
						}
					}
					else
					{
						results["done"] = "FALSE";
						results["msg"] = "File is invalid (Not allowed to upload)";
						results["fileType"] = "";
						results["origfileName"] = fileName;
						results["newFileName"] = newFileName;
					}
				}
				else
				{
					results["done"] = "FALSE";
					results["msg"] = "Invalid file size";
					results["fileType"] = "";
					results["origfileName"] = fileName;
					results["newFileName"] = newFileName;
				}
			}

			return results;
		}


		[HttpPost]
		public JsonResult Validate_File_Attributes()
		{
			IDictionary<string, string> results = new Dictionary<string, string>();
			IDictionary<string, IDictionary<string, string>> resultsMsg = new Dictionary<string, IDictionary<string, string>>();
			IDictionary<string, string> upload_results = new Dictionary<string, string>();

			int filesLen = Request.Files.Count;

			for (int i = 0; i < filesLen; i++)
			{
				results = new Dictionary<string, string>();

				HttpPostedFileBase file = Request.Files[i];

				upload_results = this.UploadThisFile(file, "");

				if (upload_results["done"] == "TRUE")
				{
					results["done"] = "TRUE";
					results["msg"] = "Can upload";
					results["origfileName"] = upload_results["origfileName"];
					resultsMsg[i.ToString()] = results;
				}
				else
				{
					results["done"] = "FALSE";
					results["msg"] = upload_results["msg"];
					results["origfileName"] = upload_results["origfileName"];
					resultsMsg[i.ToString()] = results;
				}

			}

			return Json(resultsMsg);
		}


		public FileResult Download(string serverFileName, string categoryDIR, string fileName)
		{
			var FileVirtualPath = Server.MapPath("~/App_Data/" + categoryDIR + serverFileName);
			byte[] fileBytes = System.IO.File.ReadAllBytes(@FileVirtualPath);
			return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
		}

	}
		//FLA USER CONFIG






		

}
