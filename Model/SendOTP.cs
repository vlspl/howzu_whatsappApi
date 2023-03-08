using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using System.Data;
using System.Data.SqlClient;
using CrossPlatformAESEncryption.Helper;
using WhatsappWebAPI.Model;
using System.Data;
using System.Configuration;
using Twilio.Rest.Conversations.V1;
//using Twilio.Rest.Api.V2010.Account.Conference;
using Twilio.Rest.Conversations.V1.Conversation;
using MessageResource = Twilio.Rest.Conversations.V1.Conversation.MessageResource;
using System.IO;
 

namespace Howzu_API.Services
{
    public class SendOTP
    {
        DataAccessLayer DAL = new DataAccessLayer();

        const string accountSid = "AC33cdc44a09dbe7fc807ccf3bb48565dc";  //AC33cdc44a09dbe7fc807ccf3bb48565dc
         // public string authToken = DAL.getAuthToken;//"1182e90a83309a5c0848f110fa5db448"; //"70d189ea498c618137ef78b2df81fb36";
        public string authToken = string.Empty;
        public string sendOtpViaWatsapp(string mobileNo, string msgName, string Parameters)
        {
            authToken = DAL.getAuthString();
            try
            {
                int count;
                TwilioClient.Init(accountSid, authToken);

                SqlParameter[] param = new SqlParameter[]
               {
                    new SqlParameter("@msgName",msgName)
               };
                DataTable dt = DAL.ExecuteStoredProcedureDataTable("SP_GetWhatsappMsgDetails", param);
                string[] parametersListFrmModel = new string[200];
                string[] parametersListFrmDB = new string[200];

                parametersListFrmModel = Parameters.Split(',');

                string getParam = "", getBody = "", _getParamFrmDB = "";
                foreach (DataRow row in dt.Rows)
                {
                    getBody = row["body"].ToString();
                    parametersListFrmDB = row["paramList"].ToString().Split(',');

                    count = parametersListFrmDB.Count(); //int.Parse(row["noOfParameters"].ToString());
                    for (int i = 0; i < count; i++)
                    {
                        getParam = parametersListFrmModel[i];
                        _getParamFrmDB = parametersListFrmDB[i];
                        getBody = getBody.Replace("@" + _getParamFrmDB, getParam);
                        //  getBody = getBody.Replace("@br", "\n");
                    }



                }

                var message = Twilio.Rest.Api.V2010.Account.MessageResource.Create(
                    from: new Twilio.Types.PhoneNumber("whatsapp:+919922330701"),//whatsapp:14155238886 //7692468383  
                    body: getBody, //"Hi  , Your OTP is " + OTP + ".Please don't share it with anyone.",

                    to: new Twilio.Types.PhoneNumber("whatsapp:" + mobileNo)
                    );
                return "1";


            }
            catch (Exception ex)
            {
                //LogError.LoggerCatch(ex);

                return ex.ToString();//"0";
            }
        }


        public string twoWayCommunication(string mobileNo)
        {

            try
            {
                authToken = DAL.getAuthString();
                TwilioClient.Init(accountSid, authToken);
                //create Conversation


                var conversation = ConversationResource.Create(
                    friendlyName: "Friendly Conversation"
                );
                Console.WriteLine(conversation.Sid);

                //Fetch conversation
                var conversationFetch = ConversationResource.Fetch(
                 pathSid: conversation.Sid
      );

                Console.WriteLine(conversation.FriendlyName);


                //read multiple conversations
                var conversations = ConversationResource.Read(limit: 20);

                foreach (var record in conversations)
                {
                    Console.WriteLine(record.Sid);
                }

                // create whatsapp participant
                var participant = ParticipantResource.Create(
               messagingBindingAddress: "whatsapp:" + mobileNo,
               messagingBindingProxyAddress: "whatsapp:+17692468383",
               pathConversationSid: conversation.Sid //"SMdacbcb394f59485cb9dd4a22504acf56"
           );




                //Invite customer to Engage
                var message = MessageResource.Create(
               author: "whatsapp:+919561284894",
               body: "Hello Robert, your food delivery is almost there but Alicia (your rider) needs help finding your door. Are you willing to chat with them?",
               pathConversationSid: conversation.Sid
           );

                //Webhook
                var configurationFilters = new List<string> {
            "onMessageAdded"
        };

                var webhook = WebhookResource.Create(
                    configurationUrl: "https://mustard-earwig-8077.twil.io/", //"http://funny-dunkin-3838.twil.io/customer-optin",
                    configurationMethod: WebhookResource.MethodEnum.Post,
                    configurationFilters: configurationFilters,
                    target: WebhookResource.TargetEnum.Webhook,
                    pathConversationSid: conversation.Sid
                );


                return "1";


            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public string mutliMediaMsg(string mobileNo)
        {
            try
            {
                authToken = DAL.getAuthString();
                TwilioClient.Init(accountSid, authToken);


                //        var mediaUrl = new[] {
                //     new Uri("https://howzu.co.in/images/SignUpImage.jpeg")
                //}.ToList();

                var message = Twilio.Rest.Api.V2010.Account.MessageResource.Create(
                     body: "Please Check the above image to learn how to sign in",
                   // mediaUrl: mediaUrl, 
                   mediaUrl: new List<Uri> { new Uri("https://howzu.co.in/images/SignUpImage.jpeg") },
                    from: new Twilio.Types.PhoneNumber("whatsapp:+919922330701"),
                  to: new Twilio.Types.PhoneNumber("whatsapp:" + mobileNo)
                );

                Console.WriteLine(message.Sid);


                return "1";


            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string mutliMediaMsgWithVideo(string mobileNo)
        {
            try
            {
                authToken = DAL.getAuthString();
                TwilioClient.Init(accountSid, authToken);


                //        var mediaUrl = new[] {
                //     new Uri("https://howzu.co.in/images/SignUpImage.jpeg")
                //}.ToList();

                var message = Twilio.Rest.Api.V2010.Account.MessageResource.Create(
                   //  body: "Please Check this Video to learn how to sign in to Howzu App",
                   // mediaUrl: mediaUrl, 
                   mediaUrl: new List<Uri> { new Uri("https://howzu.co.in/images/WhatsAppVideo.mp4") },
                    //WhatsApp Video.mp4

                    from: new Twilio.Types.PhoneNumber("whatsapp:+919922330701"),
                  to: new Twilio.Types.PhoneNumber("whatsapp:" + mobileNo)
                );

                Console.WriteLine(message.Sid);


                return "1";


            }
            catch (Exception ex)
            {
                return "0";
            }
        }




    }
}
