using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WhatsappWebAPI.Model;

namespace WhatsappWebAPI.Controllers
{
    public class SelfAssessmentTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        DataAccessLayer DAL = new DataAccessLayer();
        SqlConnection cn;

        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserSelfAssessmentTestlist")]
        public string GetUserSelfAssessmentTestlist(string getselfTestName = "All")
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object

            try
            {

                SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@SelfTestName",getselfTestName)


                        };
                DataTable dt_getSelfTestLIst = DAL.ExecuteStoredProcedureDataTable("sp_getSelfAssessmentTestList", param);


                if (dt_getSelfTestLIst.Rows.Count > 0)
                {
                    Result.SelfTestList = new JArray() as dynamic;   // Create Array for Test Details

                    dynamic ObjSelfTestList = new JObject();

                    foreach (DataRow dr in dt_getSelfTestLIst.Rows)
                    {


                        ObjSelfTestList.assessmentGroupID = dr["assessmentGroupID"];
                        ObjSelfTestList.assessmentGroupName = dr["assessmentGroupName"];
                        ObjSelfTestList.assessmentGroupDescription = dr["assessmentGroupDescription"];

                        Result.SelfTestList.Add(ObjSelfTestList); //Add Test details to array

                    }

                    Result.Status = true;  //  Status Key                    
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
                Result.Msg = "Something went wrong,Please try again." + ex.ToString();
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserSelfAssessmentQuestions")]
        public string GetSelfAssessmentNextQuestion(string selfAssessmentGroup,int currentQuestionID=0)
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object

            try
            {

                SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@SelfTestGroup",selfAssessmentGroup),
                            new SqlParameter("@SelfTestQuestionID",currentQuestionID)

                        };
                DataTable dt_getSelfTestQuestion = DAL.ExecuteStoredProcedureDataTable("sp_getSelfAssessmentGetQuestions", param);


                if (dt_getSelfTestQuestion.Rows.Count > 0)
                {
                    Result.SelfTestQuestionOption = new JArray() as dynamic;   // Create Array for Test Details
                                      
                    Result.assessmentQuestionID= dt_getSelfTestQuestion.Rows[0]["assessmentMasterID"];
                    Result.assessmentQuestion = dt_getSelfTestQuestion.Rows[0]["assessmentQuestion"];
                    Result.assessmentDescription = dt_getSelfTestQuestion.Rows[0]["assessmentDescription"];
                    Result.assessmentFor = dt_getSelfTestQuestion.Rows[0]["assessmentFor"];
                    Result.islastQuestion = dt_getSelfTestQuestion.Rows[0]["islastQuestion"];

                    //get options for questions
                    dynamic ObjSelfTestQuestionOptions = new JObject();
                    foreach (DataRow dr in dt_getSelfTestQuestion.Rows)
                    {
                       
                        ObjSelfTestQuestionOptions.QuestionsOptions = dr["optionDetails"];
                        ObjSelfTestQuestionOptions.QuestionsOptionsID = dr["assessmentOptionID"];
                        Result.SelfTestQuestionOption.Add(ObjSelfTestQuestionOptions); //Add Test details to array
                    }

                   

                    Result.Status = true;  //  Status Key                    
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
                Result.Msg = "Something went wrong,Please try again." + ex.ToString();
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserSelfAssessmentUserTestResult")]
        public string GetSelfAssessmentUserTestResult(int userID,string selfTestGroup,string userSelfTestDate)
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object

            try
            {
                SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@userID",userID),
                            new SqlParameter("@selfTestGroup",selfTestGroup)  , //
                            new SqlParameter("@selfTestDate",userSelfTestDate)
                        };
                DataTable dt_getSelfuserTestResult = DAL.ExecuteStoredProcedureDataTable("sp_selfAssessmentUserTestResult", param);


                if (dt_getSelfuserTestResult.Rows.Count > 0)
                {
                    Result.SelfUserTestOuestion = new JArray() as dynamic;   // Create Array for Test Details

                    Result.userSelfAssessmentTestID = dt_getSelfuserTestResult.Rows[0]["userSelfAssessmentTestID"];
                    Result.assessmentGroup = dt_getSelfuserTestResult.Rows[0]["assessmentGroup"];
                    Result.selfAssessmentTestDate = dt_getSelfuserTestResult.Rows[0]["selfAssessmentTestDate"];
                    

                    //get options for questions
                    dynamic ObjSelfTestQuestionOptions = new JObject();
                    foreach (DataRow dr in dt_getSelfuserTestResult.Rows)
                    {

                        ObjSelfTestQuestionOptions.assessmentQuestion = dr["assessmentQuestion"];
                        ObjSelfTestQuestionOptions.UserSelectedOption = dr["optionDetails"];
                        Result.SelfUserTestOuestion.Add(ObjSelfTestQuestionOptions); //Add Test details to array
                    }

                    Result.Status = true;  //  Status Key                    
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
                Result.Msg = "Something went wrong,Please try again." + ex.ToString();
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("InsertSelfAssessmentUserTest")]
        public string InsertSelfAssessmentUserTest([FromBody] selfUserTest model )
        {
            string JSONString = string.Empty; // Create string object to return final output
            dynamic Result = new JObject();  //Create root JSON Object

            try
            {
                SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@userId",model.userID),
                            new SqlParameter("@assessmentMasterID",model.AssessmentMasterID),
                            new SqlParameter("@assessmentOptionID",model.AssessmentOptionID),
                            new SqlParameter("@userSelfTestDescription",model.userSelfTestDescription),
                            new SqlParameter("@selfAssessmentTestDate",model.selfTestDate),
                            new SqlParameter("@ReturnVal",SqlDbType.Int)

                        };
               int result = DAL.ExecuteStoredProcedureRetnInt("sp_selfAssessmentUserTest", param);


                if (result > 0)
                {
                   
                    Result.Status = true;  //  Status Key
                    Result.Msg = "User self assessment test added successfully..";
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
                Result.Msg = "Something went wrong,Please try again." + ex.ToString();
                JSONString = JsonConvert.SerializeObject(Result);
                return JSONString;
            }
        }


    }
}
