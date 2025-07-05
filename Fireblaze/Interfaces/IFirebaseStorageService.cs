using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFirebaseStorageService
{
    Task<List<string>> ListFilesInExportedLayouts();
}
