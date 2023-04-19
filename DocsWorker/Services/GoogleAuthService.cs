using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsWorker.Services
{
    public class GoogleAuthService
    {
        public static UserCredential GetUserCredential(string userName, int timeout)
        {
            try
            {
                var clientId = "562891167535-su3jddfdaijtp71vk51bgu4adjle8q10.apps.googleusercontent.com";
                var secret = "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";
                //var userName = Environment.UserName;

                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(timeout));
                CancellationToken ct = cts.Token;

                string[] scopes = new string[] { DriveService.Scope.Drive, };

                // Requesting Authentication or loading previously stored authentication for userName
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets { ClientId = clientId, ClientSecret = secret },
                    scopes,
                    userName,
                    ct
                ).Result;
          
              


                return credential;
            }
            catch (Exception ex)
            {
                throw new Exception("Authentification error/timeout", ex);
            }          
        }
    }
}
