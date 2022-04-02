using System;
using System.Linq;
using LinqToTwitter;

namespace DigitalSignEditor.Twitter {

    public class Tweet {

        public Tweet() {
        }

        public Tweet(Status status, string username) {
            DatePulled = DateTime.Now.ToShortTimeString();
            Handle = status.User.ScreenNameResponse;
            IsFromUsername = username.Equals(status.User.ScreenNameResponse ?? "", StringComparison.OrdinalIgnoreCase);
            IsRetweeted = status.Retweeted;
            Image = status.RetweetedStatus?.Entities?.MediaEntities.FirstOrDefault()?.MediaUrlHttps;
            Text = (status.RetweetedStatus?.FullText ?? status.RetweetedStatus?.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(Image)) {
                Image = status.Entities?.MediaEntities.FirstOrDefault()?.MediaUrlHttps;
            }
            if (string.IsNullOrWhiteSpace(Text)) {
                Text = (status.FullText ?? status.Text ?? string.Empty).Trim();
            }
            var quote = (status.QuotedStatus.FullText ?? status.Text ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(quote)) {
                Text = Text + ": " + quote;
                Image = status.QuotedStatus?.Entities?.MediaEntities.FirstOrDefault()?.MediaUrlHttps;
            }
            Time = GetTweetTime(status.CreatedAt);
            UserImage = status.User.ProfileImageUrlHttps;
            Username = status.User.Name;
        }

        public string DatePulled { get; set; }
        public string Handle { get; set; }

        public string Image { get; set; }
        public bool IsFromUsername { get; set; }
        public bool IsRetweeted { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public string UserImage { get; set; }

        public string Username { get; set; }

        private string GetTweetTime(DateTime date) {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            if (ts.Hours == 0) {
                return ts.Minutes <= 5 ? "recently" : ts.Minutes + " minutes ago";
            }
            if (ts.Days == 0) {
                return ts.Hours == 1 ? "an hour ago" : ts.Hours + " hours ago";
            }
            if (ts.Days < 30) {
                return ts.Days == 1 ? "yesterday" : ts.Days + " days ago";
            }
            if (ts.Days < 365) {
                var months = Convert.ToInt32(Math.Floor((double) ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            return "over a year old";
        }
    }
}