using System;
using System.Threading.Tasks;
using Octokit;

namespace DigitalSignEditor.GithubExport {

    public class FileCreator : IFileCreator {
        private const string branch = "staging";
        private const string headerValue = "itpartners-digital-sign";
        private string owner;
        private string repositoryName;
        private string token;

        public FileCreator(string owner, string repositoryName, string token) {
            this.owner = owner;
            this.repositoryName = repositoryName;
            this.token = token;
        }

        public async Task<bool> Commit() {
            var client = CreateClient();
            var pullRequest = await client.PullRequest.Create(owner, repositoryName, new NewPullRequest("Automated Commit", "staging", "main"));
            _ = await client.PullRequest.Merge(owner, repositoryName, pullRequest.Number, new MergePullRequest { CommitTitle = "Automated Commit Merge" });
            return true;
        }

        public async Task<bool> CreateFile(string filename, byte[] byteArray) {
            var client = CreateClient();
            var fileList = await client.Repository.Content.GetAllContents(owner, repositoryName, filename);
            if (fileList.Count > 0) {
                var sha = fileList[0].Sha;
                await client.Repository.Content.UpdateFile(owner, repositoryName, filename,
                    new UpdateFileRequest("Update " + filename, Convert.ToBase64String(byteArray), sha, branch, false));
            } else {
                _ = await client.Repository.Content.CreateFile(owner, repositoryName, filename,
                    new CreateFileRequest("Commit for " + filename, Convert.ToBase64String(byteArray), branch, false));
            }
            return true;
        }

        public async Task<bool> CreateFile(string filename, string contents) {
            var client = CreateClient();
            var fileList = await client.Repository.Content.GetAllContents(owner, repositoryName, filename);
            if (fileList.Count > 0) {
                var sha = fileList[0].Sha;
                await client.Repository.Content.UpdateFile(owner, repositoryName, filename,
                    new UpdateFileRequest("Update " + filename, contents, sha, branch));
            }
            _ = await client.Repository.Content.CreateFile(owner, repositoryName, filename,
                new CreateFileRequest("Commit for " + filename, contents, branch));
            return true;
        }

        public async Task<bool> DeleteFile(string filename) {
            var client = CreateClient();
            var fileList = await client.Repository.Content.GetAllContents(owner, repositoryName, filename);
            if (fileList.Count > 0) {
                var sha = fileList[0].Sha;
                await client.Repository.Content.DeleteFile(owner, repositoryName, filename,
                    new DeleteFileRequest("Delete " + filename, sha, branch));
            }
            return true;
        }

        private GitHubClient CreateClient() {
            var client = new GitHubClient(new ProductHeaderValue(headerValue));
            client.Credentials = new Credentials(token);
            return client;
        }
    }
}