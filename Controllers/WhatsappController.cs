using Howzu_API.Services;
using ImageUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Http;
using WhatsappWebAPI.Model;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Hosting.Server;
using Twilio.AspNet.Core;
using Twilio.AspNet.Common;
using Twilio.TwiML;
using System.Data;
//using System.Web.Http;
//using System.Web.Script.Serialization;


namespace WhatsappWebAPI.Controllers
{

    public class WhatsappController : Controller
    {
       
        DataAccessLayer DAL = new DataAccessLayer();
        SqlConnection cn ;

        SendOTP _otp = new SendOTP();
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
       [Route("sendWatsappMsg/{mobNo}/{msgName}/{param}")]
        public string sendWatsappMsg(string mobNo,string msgName,string param)
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object
            string Msg = "";
            try
            {

                string returnValue = _otp.sendOtpViaWatsapp(mobNo, msgName, param);
                if (returnValue == "1")
                {
                    Result.Status = true;  //  Status Key 
                    Result.Msg = "Message Sent Successfully.";

                    JSONString = JsonConvert.SerializeObject(Result);//Add user details to array

                  //  return JSONString;
                }
                else
                {
                    Result.Status = false;  //  Status Key
                    Result.Msg = "Something went wrong,Please try again.";
                    JSONString = JsonConvert.SerializeObject(Result);
                   // return JSONString;
                }

            }
            catch (Exception ex)
            {
              //  LogError.LoggerCatch(ex);
                Result.Status = false;  //  Status Key
                Result.Msg = "Something went wrong,Please try again.";
                JSONString = JsonConvert.SerializeObject(Result);
               // return JSONString;
            }
            return JSONString;
        }


        //[AllowAnonymous]
        //[HttpPost]
        //[Route("webhook")]
        //public TwiMLResult webhook(SmsRequest request)
        //{
        //    var response = new MessagingResponse();
        //    response.Message("Hello World");
        //    return TwiML(response);
        //}


        [AllowAnonymous]
        [HttpPost]
        [Route("sendWatsappMsgModel")]
        public string sendWatsappMsgModel([FromBody] whatsappMsg model)
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object
            string Msg = "";
            try
            {

                string returnValue = _otp.sendOtpViaWatsapp(model.mobNo, model.msgName, model.parameters);
                if (returnValue == "1")
                {
                    Result.Status = true;  //  Status Key 
                    Result.Msg = "Message Sent Successfully.";

                    JSONString = JsonConvert.SerializeObject(Result);//Add user details to array

                    return JSONString;
                }
                else
                {
                    Result.Status = false;  //  Status Key
                    Result.Msg = returnValue; //"Something went wrong,Please try again.";
                    JSONString = JsonConvert.SerializeObject(Result);
                    return JSONString;
                }

            }
            catch (Exception ex)
            {
                //  LogError.LoggerCatch(ex);
                Result.Status = false;  //  Status Key
                Result.Msg = ex.ToString();//"Something went wrong,Please try again.";
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("createConversation")]
        public string createConversation(string mobNo)
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object
            string Msg = "";

            try
            {
               string returnValue= _otp.twoWayCommunication(mobNo);
                if (returnValue == "1")
                {
                    Result.Status = true;  //  Status Key 
                    Result.Msg = "Message Sent Successfully.";

                    JSONString = JsonConvert.SerializeObject(Result);//Add user details to array

                    return JSONString;
                }
                else
                {
                    Result.Status = false;  //  Status Key
                    Result.Msg = "Something went wrong,Please try again.";
                    JSONString = JsonConvert.SerializeObject(Result);
                    return JSONString;
                }
            }
            catch (Exception ex)
            {
                Result.Status = false;  //  Status Key
                Result.Msg = "Something went wrong,Please try again.";
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("MultiMedia")]
        public string MultiMedia(string mobNo)
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object
            string Msg = "";

            try
            {
                string returnValue = _otp.mutliMediaMsg(mobNo);
                _otp.mutliMediaMsgWithVideo(mobNo);
                if (returnValue == "1")
                {
                    Result.Status = true;  //  Status Key 
                    Result.Msg = "Message Sent Successfully.";

                    JSONString = JsonConvert.SerializeObject(Result);//Add user details to array

                    return JSONString;
                }
                else
                {
                    Result.Status = false;  //  Status Key
                    Result.Msg = "Something went wrong,Please try again.";
                    JSONString = JsonConvert.SerializeObject(Result);
                    return JSONString;
                }
            }
            catch (Exception ex)
            {
                Result.Status = false;  //  Status Key
                Result.Msg = "Something went wrong,Please try again.";
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }

       
        private IHostingEnvironment _hostingEnvironment;
        public WhatsappController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("userTwillioConverstionLog")]
        public string userTwillioConverstionLog([FromBody] UserReport model)
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object
            string Msg = "";
            try
            {

                SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@msgMasterId",model.msgMasterId),
                             new SqlParameter("@reportName",model.reportName),
                             new SqlParameter("@mobileNo",model.mobileNo),
                              new SqlParameter("@createdDate",model.createdDate),
                               new SqlParameter("@msgGroup",model.msgGroup),
                                new SqlParameter("@ReturnVal",SqlDbType.Int)

                        };
                int returnVal = DAL.ExecuteStoredProcedureRetnInt("SP_UserTwilioConversationLog", param);

                if (returnVal >= 1)
                {
                    Result.Status = true;  //  Status Key 
                    Result.Msg = "Record Added Successfully.";

                    JSONString = JsonConvert.SerializeObject(Result);//Add user details to array

                    return JSONString;
                }
                else
                {
                    Result.Status = false;  //  Status Key
                    Result.Msg = "Something went wrong,Please try again.";
                    JSONString = JsonConvert.SerializeObject(Result);
                    return JSONString;
                }

            }
            catch (Exception ex)
            {
                //  LogError.LoggerCatch(ex);
                Result.Status = false;  //  Status Key
                Result.Msg ="Something went wrong,Please try again.";
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("GetUserTwillioConverstionLog")]
        public string GetUserTwillioConverstionLog(string mobileNo,string msgGroup)
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object
            string msgText = "";
            try
            {

                SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@mobileNo",mobileNo),
                            new SqlParameter("@msgGroup",msgGroup)                             

                        };
                DataTable dt_GetTextMsg = DAL.ExecuteStoredProcedureDataTable("SP_GetTwilioNextMsg", param);

               if(dt_GetTextMsg.Rows.Count>0)
                {
                    
                    Result.Status = true;  //  Status Key 
                    Result.MsgText = dt_GetTextMsg.Rows[0]["msgText"].ToString();//
                    Result.MasterId = dt_GetTextMsg.Rows[0]["msgMasterID"].ToString();
                    Result.Msg = "Record Added Successfully.";

                    JSONString = JsonConvert.SerializeObject(Result);//Add user details to array

                    return JSONString;
                }
                else
                {
                    Result.Status = false;  //  Status Key
                    Result.Msg = "Something went wrong,Please try again.";
                    JSONString = JsonConvert.SerializeObject(Result);
                    return JSONString;
                }

            }
            catch (Exception ex)
            {
                //  LogError.LoggerCatch(ex);
                Result.Status = false;  //  Status Key
                Result.Msg = "Something went wrong,Please try again.";
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }
    }
}
