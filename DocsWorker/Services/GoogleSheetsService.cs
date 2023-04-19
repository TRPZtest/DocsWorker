using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace DocsWorker.Services
{
    public class GoogleSheetsService
    {  
        private readonly SheetsService _sheetsClient;

        public GoogleSheetsService(UserCredential credential)
        {
            _sheetsClient = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Docs worker"
            });
        }

        public async Task<string> CreateSheet(string documentName)
        {         
            var speadSheet = new Spreadsheet();
            speadSheet.Properties = new SpreadsheetProperties() { Title = documentName };


            _sheetsClient.Spreadsheets.Create(speadSheet);

            var request = _sheetsClient.Spreadsheets.Create(speadSheet);

            var response = await request.ExecuteAsync();

            return response.SpreadsheetId;
        }

        public async Task RewriteSheet(ValueRange valueRange, string range, string sheetId)
        {           
            SpreadsheetsResource.ValuesResource valuesResource = new SpreadsheetsResource.ValuesResource(_sheetsClient) { };
            valuesResource.Append(valueRange, sheetId, range);

            var clearRequest = _sheetsClient.Spreadsheets.Values.Clear(null, sheetId, range);
            await clearRequest.ExecuteAsync();

            var appendRequest = valuesResource.Append(valueRange, sheetId, range);
            appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.RAW;
            await appendRequest.ExecuteAsync();
        }

    }
}
