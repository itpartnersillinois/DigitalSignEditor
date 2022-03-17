using System.Threading.Tasks;

namespace DigitalSignEditor.GithubExport {

    public interface IFileCreator {

        Task<bool> Commit();

        Task<bool> CreateDataFile(string filename, string contents);

        Task<bool> CreateImageFiles(string folder, string filename, byte[] byteArray);

        Task<bool> CreateSharedDataFile(string filename, string contents);

        string GetUrl(string folder, string file);
    }
}