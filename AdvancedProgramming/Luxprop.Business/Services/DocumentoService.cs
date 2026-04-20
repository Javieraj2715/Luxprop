using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Luxprop.Data.Models;
using Luxprop.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth.OAuth2.Responses;
using System.Net.Http;
namespace Luxprop.Business.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly LuxpropContext _db;
        private readonly string _bucketName = "luxprop-3fc09.firebasestorage.app";
        private readonly IHistorialExpedienteRepository _historialRepository;

        public DocumentoService(LuxpropContext db, IHistorialExpedienteRepository historialRepository)
        {
            _db = db;
            _historialRepository = historialRepository;
        }

        private async Task<GoogleCredential> GetGoogleCredentialAsync()
        {
            // 1) Azure: JSON completo en App Settings
            var firebaseJson = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS_JSON");
            firebaseJson = string.IsNullOrWhiteSpace(firebaseJson) ? null : firebaseJson.Trim();

            if (!string.IsNullOrEmpty(firebaseJson))
                return GoogleCredential.FromJson(firebaseJson);

            // 2) Local: usa GOOGLE_APPLICATION_CREDENTIALS (ruta al .json) o ADC
            return await GoogleCredential.GetApplicationDefaultAsync();
        }
        private static ServiceAccountCredential GetServiceAccountCredential(GoogleCredential credential)
        {
            // Si viene como "GoogleCredential" envolviendo un "ServiceAccountCredential"
            if (credential.UnderlyingCredential is ServiceAccountCredential sa)
                return sa;

            throw new InvalidOperationException(
                "Signed URLs require a Service Account credential (JSON with private_key).");
        }

        public async Task<string> GetSignedDownloadUrlAsync(string fileUrlOrObjectName, int minutes = 10)
        {
            // 1) Obtener objectName (ej: documentos/uuid_archivo.pdf)
            string objectName;

            if (fileUrlOrObjectName.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                // Formato típico guardado por vos:
                // https://firebasestorage.googleapis.com/v0/b/<bucket>/o/<objectName>?alt=media
                var uri = new Uri(fileUrlOrObjectName);
                var parts = uri.AbsolutePath.Split("/o/");
                if (parts.Length < 2)
                    throw new InvalidOperationException("Invalid Firebase file URL format.");

                objectName = Uri.UnescapeDataString(parts[1]);
            }
            else
            {
                objectName = fileUrlOrObjectName;
            }

            // 2) Credencial (Azure JSON o local ADC)
            var credential = await GetGoogleCredentialAsync();

            // 3) Firmador: requiere ServiceAccountCredential
            var saCredential = GetServiceAccountCredential(credential);
            var signer = UrlSigner.FromServiceAccountCredential(saCredential);

            // 4) Firmar (usar POSICIONAL para evitar el error del nombre del parámetro)
            var signedUrl = signer.Sign(
                 _bucketName,
                 objectName,
                 TimeSpan.FromMinutes(minutes),
                 HttpMethod.Get
            );

            return signedUrl;
        }


        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var credential = await GetGoogleCredentialAsync();
            var storageClient = await StorageClient.CreateAsync(credential);

            var objectName = $"documentos/{Guid.NewGuid()}_{fileName}";

            await storageClient.UploadObjectAsync(_bucketName, objectName, contentType, fileStream);

            return $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";
        }



        public async Task DeleteFileAsync(string fileUrl, int? expedienteId, int usuarioId)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new ArgumentException("Invalid file URL");

            try
            {
                var uri = new Uri(fileUrl);
                var segments = uri.AbsolutePath.Split("/o/");
                if (segments.Length < 2)
                    throw new InvalidOperationException("Invalid Firebase file URL format.");

                var objectName = Uri.UnescapeDataString(
                    segments[1].Replace("?alt=media", string.Empty)
                );

                var credential = await GetGoogleCredentialAsync();
                var storageClient = await StorageClient.CreateAsync(credential);



                await storageClient.DeleteObjectAsync(_bucketName, objectName);

                Console.WriteLine($"Deleted file: {objectName}");

                if (expedienteId.HasValue)
                {
                    await _historialRepository.CrearHistorialAsync(
                        expedienteId.Value,
                        estadoNuevo: "Eliminado",
                        descripcion: $"El archivo '{objectName}' fue eliminado correctamente.",
                        usuarioId: usuarioId
                    );
                }
            }
            catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
            {
                Console.WriteLine("File not found in Firebase Storage.");

                if (expedienteId.HasValue)
                {
                    await _historialRepository.CrearHistorialAsync(
                        expedienteId.Value,
                        estadoNuevo: "No encontrado",
                        descripcion: "Se intentó eliminar el archivo pero no existía en Storage.",
                        usuarioId: usuarioId
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");

                if (expedienteId.HasValue)
                {
                    await _historialRepository.CrearHistorialAsync(
                        expedienteId.Value,
                        estadoNuevo: "Error",
                        descripcion: $"Error: {ex.Message}",
                        usuarioId: usuarioId
                    );
                }

                throw;
            }
        }

        public async Task<bool> UpdateDocumentStatusAsync(int documentoId, string newStatus, int usuarioId)
        {
            try
            {
                var documento = await _db.Documentos.FindAsync(documentoId);
                if (documento == null) return false;

                documento.Estado = newStatus;
                await _db.SaveChangesAsync();

                if (documento.ExpedienteId != null)
                {
                    await _historialRepository.CrearHistorialAsync(
                        expedienteId: documento.ExpedienteId.Value,
                        estadoNuevo: $"Documento {newStatus}",
                        descripcion: $"El documento '{documento.Nombre}' cambió a estado '{newStatus}'.",
                        usuarioId: usuarioId
                    );
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating document status: {ex.Message}");
                return false;
            }
        }

        public async Task UpdateExpedienteStatusAsync(int? expedienteId, int usuarioId)
        {
            if (expedienteId == null) return;

            try
            {
                var documentos = await _db.Documentos
                    .Where(d => d.ExpedienteId == expedienteId)
                    .ToListAsync();

                if (!documentos.Any()) return;

                string nuevoEstado;

                if (documentos.All(d => d.Estado == "Completado"))
                    nuevoEstado = "Completado";
                else if (documentos.All(d => d.Estado == "Archivado"))
                    nuevoEstado = "Archivado";
                else if (documentos.Any(d => d.Estado == "En revisión"))
                    nuevoEstado = "En revisión";
                else
                    nuevoEstado = "Activo";

                var expediente = await _db.Expedientes.FindAsync(expedienteId);
                if (expediente != null)
                {
                    string estadoAnterior = expediente.Estado!;

                    if (estadoAnterior != nuevoEstado)
                    {
                        expediente.Estado = nuevoEstado;
                        await _db.SaveChangesAsync();

                        await _historialRepository.CrearHistorialAsync(
                            expedienteId.Value,
                            estadoNuevo: nuevoEstado,
                            descripcion: $"El estado del expediente cambió de '{estadoAnterior}' a '{nuevoEstado}'.",
                            usuarioId: usuarioId
                        );

                        Console.WriteLine($"Expediente {expedienteId} actualizado a '{nuevoEstado}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating expediente status: {ex.Message}");
            }
        }


    }
}
