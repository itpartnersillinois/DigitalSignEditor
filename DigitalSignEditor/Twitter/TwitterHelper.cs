using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;

namespace DigitalSignEditor.Twitter {

    public class TwitterHelper {
        private const int numberOfTweets = 30;
        private string consumerKey;
        private string consumerSecret;
        private string oathToken;
        private string oathTokenSecret;

        public TwitterHelper() {
        }

        public TwitterHelper(string consumerKey, string consumerSecret, string oathToken, string oathTokenSecret) {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.oathToken = oathToken;
            this.oathTokenSecret = oathTokenSecret;
        }

        public bool IsSuccessful { get; set; }

        public List<Tweet> Tweets { get; set; }

        public void Update(string username) {
            if (numberOfTweets > 0) {
                var auth = new SingleUserAuthorizer {
                    CredentialStore = new SingleUserInMemoryCredentialStore {
                        ConsumerKey = consumerKey,
                        ConsumerSecret = consumerSecret,
                        OAuthToken = oathToken,
                        OAuthTokenSecret = oathTokenSecret
                    }
                };

                using (var twitterCtx = new TwitterContext(auth)) {
                    var tweets = twitterCtx.Status.Where(x => x.Type == StatusType.User && x.ScreenName == username && x.TweetMode == TweetMode.Extended);
                    var tweetsByMentions = twitterCtx.Status.Where(x => x.Type == StatusType.Mentions && x.TweetMode == TweetMode.Extended);
                    var tweetsBySearch = twitterCtx.Search.Single(x => x.Type == SearchType.Search && x.TweetMode == TweetMode.Extended && x.Query == "\"@" + username + "\"").Statuses;
                    Tweets = tweets.Union(tweetsBySearch, new TweetComparer()).Union(tweetsByMentions, new TweetComparer()).Distinct(new TweetComparer()).OrderByDescending(s => s.CreatedAt).Take(numberOfTweets * 5).Select(tweet => new Tweet(tweet)).Distinct(new TweetItemComparer()).Take(numberOfTweets).ToList();
                    IsSuccessful = true;
                }
            }
        }

        private class TweetComparer : IEqualityComparer<Status> {

            public bool Equals(Status x, Status y) {
                return x.CreatedAt == y.CreatedAt && x.User.ScreenNameResponse == y.User.ScreenNameResponse;
            }

            public int GetHashCode(Status model) {
                return model.CreatedAt.GetHashCode() + 7 * model.User.ScreenNameResponse.GetHashCode();
            }
        }

        private class TweetItemComparer : IEqualityComparer<Tweet> {

            public bool Equals(Tweet x, Tweet y) {
                return x.Text == y.Text;
            }

            public int GetHashCode(Tweet model) {
                return model.Text.GetHashCode();
            }
        }
    }
}