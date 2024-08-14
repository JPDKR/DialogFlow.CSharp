using DF.CSharp.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Dialogflow.v2beta1;
using Google.Apis.Dialogflow.v2beta1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;

namespace DF.CSharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("GetChatAsync")]
        public dynamic Chat(string message)
        {
            string currentSession = Guid.NewGuid().ToString();
            GoogleCredential? credential = new GoogleHelper(_configuration).CredentialScope();
            string projectId = _configuration["Google:ProjectId"]!;

            var response = new DialogflowService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = projectId
            }).Projects.Agent.Sessions.DetectIntent(
                new GoogleCloudDialogflowV2beta1DetectIntentRequest
                {
                    QueryInput = new GoogleCloudDialogflowV2beta1QueryInput
                    {
                        Text = new GoogleCloudDialogflowV2beta1TextInput
                        {
                            Text = message,
                            LanguageCode = "en-US"
                        }
                    }
                },
                "projects/" + projectId + "/agent/sessions/" + currentSession).Execute();
            dynamic queryResult = response.QueryResult;
            return queryResult;
        }
    }
}
