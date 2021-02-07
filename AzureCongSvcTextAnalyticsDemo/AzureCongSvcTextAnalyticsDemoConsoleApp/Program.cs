using System;
using System.Threading.Tasks;
using Azure;
using Azure.AI.TextAnalytics;

namespace AzureCongSvcTextAnalyticsDemoConsoleApp
{
    class Program
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("52e37d7db6b945b18c664ee534afed5a");
        private static readonly Uri endpoint = new Uri("https://ta-07022021-demo.cognitiveservices.azure.com/");

        static async Task Main()
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            // You will implement these methods later in the quickstart.
            SentimentAnalysisExample(client);
            LanguageDetectionExample(client);
            EntityRecognitionExample(client);
            EntityLinkingExample(client);
            KeyPhraseExtractionExample(client);

            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Determines the sentiment of the text
        /// </summary>
        /// <param name="client"></param>
        static void SentimentAnalysisExample(TextAnalyticsClient client)
        {
            //const string inputText = "I had the best day of my life. I wish you were there with me.";
            const string inputText = "I hate you.";


            DocumentSentiment documentSentiment = client.AnalyzeSentiment(inputText);
            Console.WriteLine($"Document sentiment: {documentSentiment.Sentiment}\n");

            foreach (var sentence in documentSentiment.Sentences)
            {
                Console.WriteLine($"\tText: \"{sentence.Text}\"");
                Console.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                Console.WriteLine($"\tPositive score: {sentence.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNegative score: {sentence.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tNeutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");
            }
        }

        /// <summary>
        /// Determines the language based on content
        /// </summary>
        /// <param name="client"></param>
        static void LanguageDetectionExample(TextAnalyticsClient client)
        {
            DetectedLanguage detectedLanguage = client.DetectLanguage("Ce document est rédigé en Français.");
            Console.WriteLine("Language:");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
        }

        /// <summary>
        /// Identifies the entity content from a phrase
        /// </summary>
        /// <param name="client"></param>
        static void EntityRecognitionExample(TextAnalyticsClient client)
        {
            var response = client.RecognizeEntities("I had a wonderful trip to Seattle last week.");
            Console.WriteLine("Named Entities:");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tText: {entity.Text},\tCategory: {entity.Category},\tSub-Category: {entity.SubCategory}");
                Console.WriteLine($"\t\tScore: {entity.ConfidenceScore:F2}\n");
            }
        }

        /// <summary>
        /// Links entities from a text phrase
        /// </summary>
        /// <param name="client"></param>
        private static void EntityLinkingExample(TextAnalyticsClient client)
        {
            var response = client.RecognizeLinkedEntities(
                "Microsoft was founded by Bill Gates and Paul Allen on April 4, 1975, " +
                "to develop and sell BASIC interpreters for the Altair 8800. " +
                "During his career at Microsoft, Gates held the positions of chairman, " +
                "chief executive officer, president and chief software architect, " +
                "while also being the largest individual shareholder until May 2014.");
            Console.WriteLine("Linked Entities:");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tName: {entity.Name},\tID: {entity.DataSourceEntityId},\tURL: {entity.Url}\tData Source: {entity.DataSource}");
                Console.WriteLine("\tMatches:");
                foreach (var match in entity.Matches)
                {
                    Console.WriteLine($"\t\tText: {match.Text}");
                    Console.WriteLine($"\t\tScore: {match.ConfidenceScore:F2}\n");
                }
            }
        }

        /// <summary>
        /// Extracts the key parts from a phrase
        /// </summary>
        /// <param name="client"></param>
        private static void KeyPhraseExtractionExample(TextAnalyticsClient client)
        {
            var response = client.ExtractKeyPhrases("My cat might need to see a veterinarian.");

            // Printing key phrases
            Console.WriteLine("Key phrases:");

            foreach (string keyphrase in response.Value)
            {
                Console.WriteLine($"\t{keyphrase}");
            }
        }

    }
}
