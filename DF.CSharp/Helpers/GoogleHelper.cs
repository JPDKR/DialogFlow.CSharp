using Google.Apis.Auth.OAuth2;
using Google.Apis.Dialogflow.v2beta1;

namespace DF.CSharp.Helpers
{
    public class GoogleHelper(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public GoogleCredential? CredentialScope()
        {
            GoogleCredential credential;
            string currentDirectory = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(currentDirectory, _configuration["Google:ConfigPath"]!);

            try
            {
                using (Stream configStream = new FileStream(configPath, FileMode.Open))
                {
                    credential = GoogleCredential.FromStream(configStream);
                }

                var scope = credential.CreateScoped(DialogflowService.Scope.CloudPlatform);

                return scope;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
