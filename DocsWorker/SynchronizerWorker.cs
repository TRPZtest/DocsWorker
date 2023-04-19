using DocsWorker.Services;
using Google.Apis.Sheets.v4.Data;

namespace DocsWorker
{
    public class SynchronizerWorker : BackgroundService
    {
        const string SHEET_NAME = "List of files";

        private readonly ILogger<SynchronizerWorker> _logger;

        public SynchronizerWorker(ILogger<SynchronizerWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {   
            try
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var credential = GoogleAuthService.GetUserCredential();

                var sheetsService = new GoogleSheetsService(credential);
                var driveService = new GoogleDriveService(credential);

                var allFiles = await driveService.GetAllFilesAsync();

                var listOfFiles = allFiles.Where(x => x.Name == SHEET_NAME);

                var listOfFilesId = allFiles.Where(x => x.Name == SHEET_NAME && x.MimeType == "application/vnd.google-apps.spreadsheet").FirstOrDefault()?.Id;

                if (string.IsNullOrEmpty(listOfFilesId))
                {
                    listOfFilesId = await sheetsService.CreateSheet(SHEET_NAME);
                }

                List<IList<object>> rangeValues = new();

                foreach (var item in allFiles)
                {
                    var row = new List<object>();
                    row.Add(item.Name);
                    row.Add(item.Id);
                    rangeValues.Add(row);
                }

                await sheetsService.RewriteSheet(new ValueRange() { Values = rangeValues }, $"!A:B", listOfFilesId);
               
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
           
        }


    }
}