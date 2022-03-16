using System.Threading.Tasks;

namespace DigitalSignEditor.GithubExport {

    public interface IFileCreator {

        Task<bool> Commit();

        Task<bool> CreateFile(string filename, byte[] byteArray);

        Task<bool> CreateFile(string filename, string contents);

        Task<bool> DeleteFile(string filename);
    }
}