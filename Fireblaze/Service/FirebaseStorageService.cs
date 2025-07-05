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
}
