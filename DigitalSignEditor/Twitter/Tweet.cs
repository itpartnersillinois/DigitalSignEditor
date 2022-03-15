using System.Linq;
using LinqToTwitter;

namespace DigitalSignEditor.Twitter {

    public class Tweet {

        public Tweet() {
        }

        public Tweet(Status status) {
            Handle = status.User.ScreenNameResponse;
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
                Text = this.Text + ": " + quote;
                Image = status.QuotedStatus?.Entities?.MediaEntities.FirstOrDefault()?.MediaUrlHttps;
            }
            UserDescription = status.User.Description;
            UserImage = status.User.ProfileImageUrlHttps;
            Username = status.User.Name;
        }

        public string Handle { get; set; }

        public string Image { get; set; }

        public Tweet SecondTweet { get; set; }
        public string Text { get; set; }

        public Tweet ThirdTweet { get; set; }
        public string UserDescription { get; set; }

        public string UserImage { get; set; }

        public string Username { get; set; }
    }
}