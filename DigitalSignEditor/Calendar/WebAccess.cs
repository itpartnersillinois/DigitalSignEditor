using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace DigitalSignEditor.Calendar {

    public static class WebAccess {

        public static string GetCalenderIcs(string url) {
            var client = new HttpClient(new HttpClientHandler());
            return client.GetStringAsync(url).Result;
        }

        public static IEnumerable<dynamic> GetCalenderJson(string url) {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            using var response = httpWebRequest.GetResponse();
            using var streamReader = new StreamReader(response.GetResponseStream() ?? new MemoryStream());
            var result = streamReader.ReadToEnd();
            return JArray.Parse(result);
        }
    }
}