using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
//Cambio
namespace Luxprop.Business.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly string _bucketName = "luxprop-3fc09.firebasestorage.app";

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var credential = await GoogleCredential.GetApplicationDefaultAsync();
            var storageClient = await StorageClient.CreateAsync(credential);
            var objectName = $"documentos/{Guid.NewGuid()}_{fileName}";

            await storageClient.UploadObjectAsync(_bucketName, objectName, contentType, fileStream);

            string publicUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";

            return publicUrl;
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new ArgumentException("Invalid file URL");

            try
            {
                var uri = new Uri(fileUrl);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

               
                var segments = uri.AbsolutePath.Split("/o/");
                if (segments.Length < 2)
                    throw new InvalidOperationException("Invalid Firebase file URL format.");

                
                var objectName = Uri.UnescapeDataString(segments[1].Replace("?alt=media", string.Empty));

                var storageClient = await StorageClient.CreateAsync();
                await storageClient.DeleteObjectAsync(_bucketName, objectName);

                Console.WriteLine($"Deleted file: {objectName}");
            }
            catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
            {
                Console.WriteLine("File not found in Firebase Storage.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                throw;
            }
        }
    }
}
