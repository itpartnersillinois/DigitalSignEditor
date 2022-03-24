using System;

namespace DigitalSignEditor.GithubExport {

    public class FileCreatorFactory : IFileCreatorFactory {
        private string hostname;
        private string owner;
        private string repositoryName;
        private Func<byte[], byte[]> resizeAction;
        private string token;

        public FileCreatorFactory(string owner, string repositoryName, string token, string hostname, Func<byte[], byte[]> resizeAction) {
            this.owner = owner;
            this.repositoryName = repositoryName;
            this.token = token;
            this.hostname = hostname.TrimEnd('/');
            this.resizeAction = resizeAction;
        }

        public FileCreator Create() {
            return new FileCreator(owner, repositoryName, token, hostname, resizeAction);
        }
    }
}