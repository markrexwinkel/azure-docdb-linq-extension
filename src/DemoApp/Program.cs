using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using InfoSupport.Azure.DocumentDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var authKey = ConfigurationManager.AppSettings["DocumentDbAuthKey"];
            var client = new DocumentClient(new Uri("https://[YOUR ACCOUNT].documents.azure.com:443/"), authKey);
            var documentCollectionSelfLink = "dbs/GWFXAA==/colls/GWFXAOumvQA=";

            
            Problem(client, documentCollectionSelfLink);
            // Before running Solution() first delete Problem documents from the database
            //Solution(client, documentCollectionSelfLink);

            Console.ReadKey();
        }

        
        static async void Problem(DocumentClient client, string documentCollectionSelfLink)
        {
            var newDrawing = new Drawing()
            {
                Name = "DrawingWithNoTypeInformation",
                Figures = new List<Figure>() {
                    new Circle() { Center = new Point() { X = 10.0, Y = 10.0}, Radius = 5.43 },
                    new Line() { Start = new Point() { X = 0.0, Y = 54.3 }, End = new Point() { X = 243.45, Y = 12.34 }},
                    new Path() { Points = new Point[] { new Point() { X = 0.0, Y = 0.0 }, new Point() { X = 10.0, Y = 10.0 }, new Point() { X = 20.0, Y = 35.5 }}}
                }
            };

            var response = await client.CreateDocumentAsync(documentCollectionSelfLink, newDrawing);
            Console.WriteLine("{0}: {1}\r\n", response.StatusCode, response.Resource.SelfLink);

            // Throws an exception because the JsonSerializer tries to create an instance of the abstsract Figure class
            try
            {
                var query = from drawing in client.CreateDocumentQuery<Drawing>(documentCollectionSelfLink)
                            select drawing;

                foreach (var drawing in query)
                {
                    Console.WriteLine(drawing.Name);
                }
            }
            catch (AggregateException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var inner in ex.Flatten().InnerExceptions)
                {
                    Console.WriteLine(inner.Message);
                }
            }
        }

        static async void Solution(DocumentClient client, string documentCollectionSelfLink)
        {
            var newDrawing = new Drawing()
            {
                Name = "DrawingWithTypeInformation",
                Figures = new List<Figure>() {
                    new Circle() { Center = new Point() { X = 10.0, Y = 10.0}, Radius = 5.43 },
                    new Line() { Start = new Point() { X = 0.0, Y = 54.3 }, End = new Point() { X = 243.45, Y = 12.34 }},
                    new Path() { Points = new Point[] { new Point() { X = 0.0, Y = 0.0 }, new Point() { X = 10.0, Y = 10.0 }, new Point() { X = 20.0, Y = 35.5 }}}
                }
            };

            var settings = new JsonSerializerSettings() { TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto };

            var response = await client.CreateDocumentAsync(documentCollectionSelfLink, newDrawing, settings);
            Console.WriteLine("{0}: {1}\r\n", response.StatusCode, response.Resource.SelfLink);

            try
            {
                var query = from drawing in client.CreateDocumentQuery<Drawing>(documentCollectionSelfLink, settings)
                            select drawing;

                foreach (var drawing in query)
                {
                    Console.WriteLine(drawing.Name);
                }
            }
            catch (AggregateException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var inner in ex.Flatten().InnerExceptions)
                {
                    Console.WriteLine(inner.Message);
                }
            }
        }
    }
}
