public class FirebaseStorageServiceTest : IFirebaseStorageService
{
    public Task<List<string>> ListFilesInExportedLayouts()
    {
        return Task.FromResult(new List<string> { "file1.glb", "file2.glb" });
    }
}
