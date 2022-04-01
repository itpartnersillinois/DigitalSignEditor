namespace DigitalSignEditor.Twitter {

    public interface ITwitterCache {

        bool AddCache(string username, string data);

        string GetCache(string username);
    }
}