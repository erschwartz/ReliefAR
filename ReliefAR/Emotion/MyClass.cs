using System;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Emotion
{
    public class Emotion
    {
        public static EmotionScores MakeRequest(byte[] byteData)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "1c079b10797a49debd18b8089e08c621"); // 

            // NOTE: You must use the same region in your REST call as you used to obtain your subscription keys.
            //   For example, if you obtained your subscription keys from westcentralus, replace "westus" in the 
            //   URI below with "westcentralus".
            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                Task<HttpResponseMessage> postTask = client.PostAsync(uri, content);
                postTask.Wait();
                response = postTask.Result;
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            // A peak at the raw JSON response.
            Console.WriteLine(responseContent);

            // Processing the JSON into manageable objects.
            JToken rootToken = JArray.Parse(responseContent).First;

            // First token is always the faceRectangle identified by the API.
            JToken faceRectangleToken = rootToken.First;

            // Second token is all emotion scores.
            JToken scoresToken = rootToken.Last;

            // Show all face rectangle dimensions
            JEnumerable<JToken> faceRectangleSizeList = faceRectangleToken.First.Children();
            foreach (var size in faceRectangleSizeList)
            {
                Console.WriteLine(size);
            }

            // Show all scores
            JEnumerable<JToken> scoreList = scoresToken.First.Children();
            EmotionScores scores = new EmotionScores();
            foreach (var score in scoreList)
            {
                JProperty jProperty = score.Value<JProperty>();
                String name = jProperty.Name;
                double value = jProperty.Value.Value<double>();
                scores.set(name, value);
            }
            return scores;
        }
    }

    public class EmotionScores {
        public double Anger { get; set; }
        public double Contempt { get; set; }
        public double Disgust { get; set; }
        public double Fear { get; set; }
        public double Happiness { get; set; }
        public double Neutral { get; set; }
        public double Sadness { get; set; }
        public double Surprise { get; set; }

        public void set(string name, double value) {
            switch(name.ToLower()) {
                case "anger":
                    Anger = value;
                    break;
                case "contempt":
                    Contempt = value;
                    break;
                case "disgust":
                    Disgust = value;
                    break;
                case "fear":
                    Fear = value;
                    break;
                case "happiness":
                    Happiness = value;
                    break;
                case "neutral":
                    Neutral = value;
                    break;
                case "sadness":
                    Sadness = value;
                    break;
                case "surprise":
                    Surprise = value;
                    break;
            }
        }
    }
}
