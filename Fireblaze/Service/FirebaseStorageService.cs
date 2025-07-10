using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirebaseStorageService : IFirebaseStorageService
{
    private readonly StorageClient _storageClient;
    private const string BucketName = "lutech-virtual-layout.firebasestorage.app";

    public FirebaseStorageService(string serviceAccountPath)
    {
        // Crea client autenticato
        _storageClient = StorageClient.Create(
            Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(serviceAccountPath));
    }

    public async Task<List<string>> ListFilesInExportedLayouts()
    {
        var files = new List<string>();

        // Lista tutti gli oggetti con prefisso "ExportedLayouts/"
        var storageObjects = _storageClient.ListObjectsAsync(BucketName, "ExportedLayouts/");

        await foreach (var obj in storageObjects)
        {
            files.Add(obj.Name);
        }

        return files;
    }

    public async Task<string> DownloadGlbFromFirebaseAndSaveLocal(string remotePath, string originalFileName)
    {
        var folderPath = Path.Combine("wwwroot", "temp");
        Directory.CreateDirectory(folderPath);

        var safeFileName = originalFileName.Replace(" ", "_");
        var tempPath = Path.Combine(folderPath, safeFileName);

        try
        {
            // Se il file esiste, controllo la dimensione
            if (File.Exists(tempPath))
            {
                var fileInfo = new FileInfo(tempPath);
                if (fileInfo.Length == 0)
                {
                    Console.WriteLine($"⚠️ File esistente ma vuoto. Elimino e riscarico: {safeFileName}");
                    File.Delete(tempPath);
                }
                else
                {
                    Console.WriteLine($"⚠️ File già esistente e valido: uso cache → {safeFileName}");
                    return $"/temp/{safeFileName}";
                }
            }

            // Scarica il file da Firebase
            using var outputStream = File.Create(tempPath);
            await _storageClient.DownloadObjectAsync(BucketName, remotePath, outputStream);

            // Verifica che il file scaricato non sia vuoto
            var downloadedFileInfo = new FileInfo(tempPath);
            if (downloadedFileInfo.Length == 0)
            {
                File.Delete(tempPath);
                throw new Exception($"Il file scaricato è vuoto: {safeFileName}");
            }

            Console.WriteLine($"✅ File scaricato da Firebase: {safeFileName}");
            return $"/temp/{safeFileName}";
        }
        catch (Google.GoogleApiException gex)
        {
            Console.WriteLine($"❌ Google API error durante download file '{safeFileName}': {gex.Message}");
            // eventualmente puoi loggare anche gex.Error?.Message o gex.Error?.Code
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Errore generico durante download file '{safeFileName}': {ex.Message}");
            throw;
        }
    }



    public async Task<List<string>> ListGlbFilesAsync(string prefix = "ExportedLayouts/")
    {
        var result = new List<string>();

        await foreach (var obj in _storageClient.ListObjectsAsync(BucketName, prefix))
        {
            if (obj.Name.EndsWith(".glb", StringComparison.OrdinalIgnoreCase))
            {
                result.Add(obj.Name);
            }
        }

        return result;
    }


}
