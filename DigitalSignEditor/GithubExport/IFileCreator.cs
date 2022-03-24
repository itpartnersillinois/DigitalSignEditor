namespace DigitalSignEditor.GithubExport {

    public interface IFileCreator {

        bool Commit();

        bool CreateDataFile(string filename, string contents);

        bool CreateImageFiles(string folder, string filename, byte[] byteArray);

        bool CreateSharedDataFile(string filename, string contents);

        string GetCompressedUrl(string folder, string file);

        string GetFullUrl(string folder, string file);

        string GetUrl(string folder, string file);
    }
}