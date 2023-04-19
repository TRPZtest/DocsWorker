using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace DocsWorker.Services
{
    public class GoogleDriveService
    {        
        private readonly DriveService _driveClient;

        public GoogleDriveService(UserCredential credential)
        {          
            _driveClient = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Docs worker"
            });
        }

        public async Task<IEnumerable<Google.Apis.Drive.v3.Data.File>> GetAllFilesAsync()
        {
            try
            {    
                var request = _driveClient.Files.List();

                request.PageSize = 1000;                

                request.Q = "'me' in owners and mimeType != 'application/vnd.google-apps.folder'";

                var allFiles = new List<Google.Apis.Drive.v3.Data.File>();

                do
                {
                    var result = await request.ExecuteAsync();
                    request.PageToken = result.NextPageToken;

                    allFiles = allFiles.Concat(result.Files).ToList();
                }
                while (!string.IsNullOrEmpty(request.PageToken));

                return allFiles;
            }
            catch (Exception Ex)
            {
                throw new Exception("Request Files.List failed.", Ex);
            }
        }
    }
}
