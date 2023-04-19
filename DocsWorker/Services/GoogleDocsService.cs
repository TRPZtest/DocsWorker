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
    public class GoogleDocsService
    {
        private readonly DocsService _docsClient;
        private readonly DriveService _driveClient;

        public GoogleDocsService()
        {
            _docsClient = new DocsService();
            _driveClient = new Google.Apis.Drive.v3.DriveService();
        }

        public async Task GetDocumentsListAsync()
        {

            
            //var req = new Google.Apis.Docs.v1.DocumentsResource.GetRequest(_docsClient, "14")
            //{

            //};

            var req = new Google.Apis.Drive.v3.FilesResource.ListRequest(_driveClient);

            req.OauthToken = "562891167535-su3jddfdaijtp71vk51bgu4adjle8q10.apps.googleusercontent.com";



        }
    }
}
