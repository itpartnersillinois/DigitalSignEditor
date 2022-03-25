using System;
using System.Linq;
using System.Threading;
using Octokit;

namespace DigitalSignEditor.GithubExport {

    // assuming that the all the branches, folders and the timeout.json file already exists -- if it doesn't, this will error out.
    // using Thread.Sleep(1000) to fix issues with response
    public class FileCreator : IFileCreator {
        private const string branch = "staging";
        private const string compressedImageFolder = "images_web";
        private const string dataFolder = "json";
        private const string dataFolderShared = "_data";
        private const string headerValue = "itpartners-digital-sign";
        private const string imageFolder = "images";
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

        public bool Commit() {
            Thread.Sleep(5000);
            var client = CreateClient();
            var fileList = client.Repository.Content.GetAllContentsByRef(owner, repositoryName, timeoutFilename, branch);
            if (fileList.Result.Count > 0) {
                var sha = fileList.Result[0].Sha;
                client.Repository.Content.UpdateFile(owner, repositoryName, timeoutFilename,
                    new UpdateFileRequest("Update to trigger reload", "{ \"time\": " + DateTime.Now.Ticks + " }", sha, branch));
                var pullRequest = client.PullRequest.Create(owner, repositoryName, new NewPullRequest("Automated Commit", "staging", "main"));
                client.PullRequest.Merge(owner, repositoryName, pullRequest.Result.Number, new MergePullRequest { CommitTitle = "Automated Commit Merge from Digital Sign Uploader" });
            }
            return true;
        }

        public bool CreateDataFile(string filename, string contents) {
            var newFilename = filename.ToLowerInvariant() + ".json";
            var client = CreateClient();
            var fileList = client.Repository.Content.GetAllContentsByRef(owner, repositoryName, dataFolder, branch);
            var action = "";
            try {
                if (fileList.Result.Any(x => x.Name == newFilename)) {
                    var sha = fileList.Result.First(x => x.Name == newFilename).Sha;
                    action = "Update with " + sha;
                    _ = client.Repository.Content.UpdateFile(owner, repositoryName, dataFolder + "/" + newFilename,
                        new UpdateFileRequest("Update " + newFilename, contents, sha, branch));
                } else {
                    action = "Create new file";
                    _ = client.Repository.Content.CreateFile(owner, repositoryName, dataFolder + "/" + newFilename,
                        new CreateFileRequest("Commit for " + newFilename, contents, branch));
                }
                Thread.Sleep(1000);
                return true;
            } catch (Exception e) {
                throw new Exception("Error (" + action + ") with filename " + filename, e);
            }
        }

        public bool CreateImageFiles(string folder, string filename, byte[] byteArray) {
            try {
                var newFilename = filename.ToLowerInvariant();
                var newFolder = folder.ToLowerInvariant();
                var client = CreateClient();
                var folderList = client.Repository.Content.GetAllContentsByRef(owner, repositoryName, imageFolder, branch);
                if (!folderList.Result.Any(x => x.Name.StartsWith(newFolder))) {
                    _ = client.Repository.Content.CreateFile(owner, repositoryName, imageFolder + "/" + newFolder + "/" + newFilename,
                        new CreateFileRequest("Commit for " + newFilename, Convert.ToBase64String(byteArray), branch, false));
                } else {
                    var fileList = client.Repository.Content.GetAllContentsByRef(owner, repositoryName, imageFolder + "/" + newFolder, branch);
                    if (fileList.Result.Any(x => x.Name == newFilename)) {
                        var sha = fileList.Result.First(x => x.Name == newFilename).Sha;
                        _ = client.Repository.Content.UpdateFile(owner, repositoryName, imageFolder + "/" + newFolder + "/" + newFilename,
                            new UpdateFileRequest("Update " + newFilename, Convert.ToBase64String(byteArray), sha, branch, false));
                    } else {
                        _ = client.Repository.Content.CreateFile(owner, repositoryName, imageFolder + "/" + newFolder + "/" + newFilename,
                            new CreateFileRequest("Commit for " + newFilename, Convert.ToBase64String(byteArray), branch, false));
                    }
                }
                Thread.Sleep(1000);
                var compressedByteArray = resizeAction(byteArray);
                var folderListCompressed = client.Repository.Content.GetAllContentsByRef(owner, repositoryName, compressedImageFolder, branch);
                if (!folderListCompressed.Result.Any(x => x.Name.StartsWith(newFolder))) {
                    _ = client.Repository.Content.CreateFile(owner, repositoryName, compressedImageFolder + "/" + newFolder + "/" + newFilename,
                        new CreateFileRequest("Commit for " + newFilename, Convert.ToBase64String(byteArray), branch, false));
                } else {
                    var fileListCompressed = client.Repository.Content.GetAllContentsByRef(owner, repositoryName, compressedImageFolder + "/" + newFolder, branch);
                    if (fileListCompressed.Result.Any(x => x.Name == newFilename)) {
                        var sha = fileListCompressed.Result.First(x => x.Name == newFilename).Sha;
                        _ = client.Repository.Content.UpdateFile(owner, repositoryName, compressedImageFolder + "/" + newFolder + "/" + newFilename,
                            new UpdateFileRequest("Update " + newFilename, Convert.ToBase64String(compressedByteArray), sha, branch, false));
                    } else {
                        _ = client.Repository.Content.CreateFile(owner, repositoryName, compressedImageFolder + "/" + newFolder + "/" + newFilename,
                            new CreateFileRequest("Commit for " + newFilename, Convert.ToBase64String(compressedByteArray), branch, false));
                    }
                }
                Thread.Sleep(1000);
                return true;
            } catch (Exception e) {
                throw new Exception("Image error with filename " + filename, e);
            }
        }

        public bool CreateSharedDataFile(string filename, string contents) {
            try {
                var newFilename = filename.ToLowerInvariant() + ".json";
                var client = CreateClient();
                var fileList = client.Repository.Content.GetAllContentsByRef(owner, repositoryName, dataFolderShared, branch);
                if (fileList.Result.Any(x => x.Name == newFilename)) {
                    var sha = fileList.Result.First(x => x.Name == newFilename).Sha;
                    _ = client.Repository.Content.UpdateFile(owner, repositoryName, dataFolderShared + "/" + newFilename,
                        new UpdateFileRequest("Update " + newFilename, contents, sha, branch));
                } else {
                    _ = client.Repository.Content.CreateFile(owner, repositoryName, dataFolderShared + "/" + newFilename,
                        new CreateFileRequest("Commit for " + newFilename, contents, branch));
                }
                Thread.Sleep(1000);
                return true;
            } catch (Exception e) {
                throw new Exception("Shared data file error with filename " + filename, e);
            }
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