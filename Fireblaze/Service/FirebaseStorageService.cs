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

        var safeFileName = originalFileName.Replace(" ", "_"); // sostituisci gli spazi!
        var tempPath = Path.Combine(folderPath, safeFileName);

        using var outputStream = File.Create(tempPath);
        await _storageClient.DownloadObjectAsync(BucketName, remotePath, outputStream);

        return $"/temp/{safeFileName}"; // <-- questo path funziona sempre
    }


    public async Task<List<string>> ListGlbFilesAsync(string prefix = "ExportedLayouts/")
    {
        var result = new List<string>();

        await foreach (var obj in _storageClient.ListObjectsAsync(BucketName, prefix))
        {
            if (obj.Name.EndsWith(".glb", StringComparison.OrdinalIgnoreCase))
            {
                result.Add(obj.Name); // es: "ExportedLayouts/Test 123.glb"
            }
        }

        return result;
    }


}
