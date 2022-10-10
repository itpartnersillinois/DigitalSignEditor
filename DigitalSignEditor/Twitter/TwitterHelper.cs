using System;
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

        public List<Tweet> Tweets { get; set; }

        public bool Update(string username, bool includeMentions) {
            if (!string.IsNullOrEmpty(username)) {
                if (username.Equals("education-convocation")) {
                    return Convocation();
                }
                return includeMentions ? Username(username) : UsernameSingleOnly(username);
            }
            return false;
        }

        private bool Convocation() {
            var auth = GenerateUser();
            var username = "edILLINOIS";
            var topic = "illinois2022";

            using var twitterCtx = new TwitterContext(auth);
            var tweets = twitterCtx.Status.Where(x => x.Type == StatusType.User && x.ScreenName == username && x.TweetMode == TweetMode.Extended).ToList();
            var correctUsername = tweets.Any() ? tweets.First().User?.ScreenNameResponse : username;
            var tweetsByMentions = twitterCtx.Status.Where(x => x.Type == StatusType.Mentions && x.TweetMode == TweetMode.Extended).ToList();
            var tweetsBySearch = twitterCtx.Search.Single(x => x.Type == SearchType.Search && x.TweetMode == TweetMode.Extended && x.Query == "\"@" + username + "\"").Statuses.ToList();
            var convocationSearch = twitterCtx.Search.Single(x => x.Type == SearchType.Search && x.TweetMode == TweetMode.Extended && x.Query == "\"#" + topic + "\"").Statuses.ToList();
            Tweets = convocationSearch.Union(tweets, new TweetComparer()).Union(tweetsBySearch, new TweetComparer()).Union(tweetsByMentions, new TweetComparer()).Distinct(new TweetComparer()).OrderByDescending(s => s.CreatedAt).Take(numberOfTweets * 5).Select(tweet => new Tweet(tweet, correctUsername)).Distinct(new TweetItemComparer()).Take(numberOfTweets).ToList();
            return true;
        }

        private SingleUserAuthorizer GenerateUser() => new SingleUserAuthorizer {
            CredentialStore = new SingleUserInMemoryCredentialStore {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                OAuthToken = oathToken,
                OAuthTokenSecret = oathTokenSecret
            }
        };

        private bool Username(string username) {
            var auth = GenerateUser();
            using var twitterCtx = new TwitterContext(auth);
            var tweets = twitterCtx.Status.Where(x => x.Type == StatusType.User && x.ScreenName == username && x.TweetMode == TweetMode.Extended).ToList();
            var correctUsername = tweets.Any() ? tweets.First().User?.ScreenNameResponse : username;
            var tweetsByMentions = twitterCtx.Status.Where(x => x.Type == StatusType.Mentions && x.TweetMode == TweetMode.Extended).ToList();
            var tweetsBySearch = twitterCtx.Search.Single(x => x.Type == SearchType.Search && x.TweetMode == TweetMode.Extended && x.Query == "\"@" + username + "\"").Statuses;
            Tweets = tweets.Union(tweetsBySearch, new TweetComparer()).Union(tweetsByMentions, new TweetComparer()).Distinct(new TweetComparer()).OrderByDescending(s => s.CreatedAt).Take(numberOfTweets * 5).Select(tweet => new Tweet(tweet, correctUsername)).Distinct(new TweetItemComparer()).Take(numberOfTweets).ToList();
            return true;
        }

        private bool UsernameSingleOnly(string username) {
            var auth = GenerateUser();
            using var twitterCtx = new TwitterContext(auth);
            var tweets = twitterCtx.Status.Where(x => x.Type == StatusType.User && x.ScreenName == username && x.TweetMode == TweetMode.Extended).ToList();
            var correctUsername = tweets.Any() ? tweets.First().User?.ScreenNameResponse : username;
            var tweetsByRetweet = twitterCtx.Search.Single(x => x.Type == SearchType.Search && x.TweetMode == TweetMode.Extended && x.Query == "\"@" + correctUsername + "\"").Statuses;
            var limitedTweetsByRetweet = tweetsByRetweet.Where(x => x.RetweetedStatus != null && x.RetweetedStatus.User != null && x.RetweetedStatus.User.ScreenNameResponse.Equals(username, StringComparison.OrdinalIgnoreCase));
            Tweets = tweets.Union(limitedTweetsByRetweet, new TweetComparer()).Distinct(new TweetComparer()).OrderByDescending(s => s.CreatedAt).Take(numberOfTweets * 5).Select(tweet => new Tweet(tweet, correctUsername)).Distinct(new TweetItemComparer()).Take(numberOfTweets).ToList();
            return true;
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