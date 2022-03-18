using System;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace DigitalSignEditor.GithubExport {

    // assuming that the all the branches, folders and the timeout.json file already exists -- if it doesn't, this will error out.
    public class FileCreator : IFileCreator {
        private const string branch = "staging";
        private const string compressedImageFolder = "image_web";
        private const string dataFolder = "json";
        private const string dataFolderShared = "_data";
        private const string headerValue = "itpartners-digital-sign";
        private const string imageFolder = "image";
        private const string timeoutFilename = "json/timeout.json";
        private string hostname;
        private string owner;
        private string repositoryName;
        private Func<byte[], byte[]> resizeAction;
        private string token;

        public FileCreator(string owner, string repositoryName, string token, string hostname, Func<byte[], byte[]> resizeAction) {
            this.owner = owner;
            this.repositoryName = repositoryName;
            this.token = token;
            this.hostname = hostname.TrimEnd('/');
            this.resizeAction = resizeAction;
        }

        public async Task<bool> Commit() {
            var client = CreateClient();
            var fileList = await client.Repository.Content.GetAllContentsByRef(owner, repositoryName, timeoutFilename, branch);
            if (fileList.Count > 0) {
                var sha = fileList[0].Sha;
                _ = await client.Repository.Content.UpdateFile(owner, repositoryName, timeoutFilename,
                    new UpdateFileRequest("Update to trigger reload", "{ \"time\": " + DateTime.Now.Ticks + " }", sha, branch));
            }

            var pullRequest = await client.PullRequest.Create(owner, repositoryName, new NewPullRequest("Automated Commit", "staging", "main"));
            _ = await client.PullRequest.Merge(owner, repositoryName, pullRequest.Number, new MergePullRequest { CommitTitle = "Automated Commit Merge from Digital Sign Uploader" });
            return true;
        }

        public async Task<bool> CreateDataFile(string filename, string contents) {
            filename = filename.ToLowerInvariant() + ".json";
            var client = CreateClient();
            var fileList = await client.Repository.Content.GetAllContentsByRef(owner, repositoryName, dataFolder, branch);
            if (fileList.Any(x => x.Name == filename)) {
                var sha = fileList.First(x => x.Name == filename).Sha;
                _ = await client.Repository.Content.UpdateFile(owner, repositoryName, dataFolder + "/" + filename,
                    new UpdateFileRequest("Update " + filename, contents, sha, branch));
            } else {
                _ = await client.Repository.Content.CreateFile(owner, repositoryName, dataFolder + "/" + filename,
                    new CreateFileRequest("Commit for " + filename, contents, branch));
            }
            return true;
        }

        public async Task<bool> CreateImageFiles(string folder, string filename, byte[] byteArray) {
            filename = filename.ToLowerInvariant();
            folder = folder.ToLowerInvariant();
            var client = CreateClient();
            var folderList = await client.Repository.Content.GetAllContentsByRef(owner, repositoryName, imageFolder, branch);
            if (!folderList.Any(x => x.Name.StartsWith(folder))) {
                _ = await client.Repository.Content.CreateFile(owner, repositoryName, imageFolder + "/" + folder + "/" + filename,
                    new CreateFileRequest("Commit for " + filename, Convert.ToBase64String(byteArray), branch, false));
            } else {
                var fileList = await client.Repository.Content.GetAllContentsByRef(owner, repositoryName, imageFolder + "/" + folder, branch);
                if (fileList.Any(x => x.Name == filename)) {
                    var sha = fileList.First(x => x.Name == filename).Sha;
                    _ = await client.Repository.Content.UpdateFile(owner, repositoryName, imageFolder + "/" + folder + "/" + filename,
                        new UpdateFileRequest("Update " + filename, Convert.ToBase64String(byteArray), sha, branch, false));
                } else {
                    _ = await client.Repository.Content.CreateFile(owner, repositoryName, imageFolder + "/" + folder + "/" + filename,
                        new CreateFileRequest("Commit for " + filename, Convert.ToBase64String(byteArray), branch, false));
                }
            }
            var compressedByteArray = resizeAction(byteArray);
            var folderListCompressed = await client.Repository.Content.GetAllContentsByRef(owner, repositoryName, compressedImageFolder, branch);
            if (!folderListCompressed.Any(x => x.Name.StartsWith(folder))) {
                _ = await client.Repository.Content.CreateFile(owner, repositoryName, compressedImageFolder + "/" + folder + "/" + filename,
                    new CreateFileRequest("Commit for " + filename, Convert.ToBase64String(byteArray), branch, false));
            } else {
                var fileListCompressed = await client.Repository.Content.GetAllContentsByRef(owner, repositoryName, compressedImageFolder + "/" + folder, branch);
                if (fileListCompressed.Any(x => x.Name == filename)) {
                    var sha = fileListCompressed.First(x => x.Name == filename).Sha;
                    _ = await client.Repository.Content.UpdateFile(owner, repositoryName, compressedImageFolder + "/" + folder + "/" + filename,
                        new UpdateFileRequest("Update " + filename, Convert.ToBase64String(compressedByteArray), sha, branch, false));
                } else {
                    _ = await client.Repository.Content.CreateFile(owner, repositoryName, compressedImageFolder + "/" + folder + "/" + filename,
                        new CreateFileRequest("Commit for " + filename, Convert.ToBase64String(compressedByteArray), branch, false));
                }
            }
            return true;
        }

        public async Task<bool> CreateSharedDataFile(string filename, string contents) {
            filename = filename.ToLowerInvariant() + ".json";
            var client = CreateClient();
            var fileList = await client.Repository.Content.GetAllContentsByRef(owner, repositoryName, dataFolderShared, branch);
            if (fileList.Any(x => x.Name == filename)) {
                var sha = fileList.First(x => x.Name == filename).Sha;
                _ = await client.Repository.Content.UpdateFile(owner, repositoryName, dataFolderShared + "/" + filename,
                    new UpdateFileRequest("Update " + filename, contents, sha, branch));
            } else {
                _ = await client.Repository.Content.CreateFile(owner, repositoryName, dataFolderShared + "/" + filename,
                    new CreateFileRequest("Commit for " + filename, contents, branch));
            }
            return true;
        }

        public string GetCompressedUrl(string folder, string file) => hostname + "/" + compressedImageFolder + "/" + folder.ToLowerInvariant() + "/" + file.ToLowerInvariant();

        public string GetFullUrl(string folder, string file) => hostname + GetUrl(folder, file);

        public string GetUrl(string folder, string file) => "/" + imageFolder + "/" + folder.ToLowerInvariant() + "/" + file.ToLowerInvariant();

        private GitHubClient CreateClient() {
            var client = new GitHubClient(new ProductHeaderValue(headerValue));
            client.Credentials = new Credentials(token);
            return client;
        }
    }
}