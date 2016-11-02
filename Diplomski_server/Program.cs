using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alchemy;
using Alchemy.Classes;
using System.Collections.Concurrent;
using System.Threading;
using System.Net;
using System.IO;
using System.Collections;

namespace Diplomski_server
{
    public class Program
    {

        static void Main(string[] args)
        {
            /* Console.WriteLine("Sending GET request");

             string url = "http://192.168.0.102:3000/tweets/50";

             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
             HttpWebResponse response = (HttpWebResponse)request.GetResponse();
             Stream resStream = response.GetResponseStream();
             var encoding = ASCIIEncoding.UTF8;

             using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
             {
                 string responseText = reader.ReadToEnd();
                 Console.WriteLine(responseText);
             }*/
            // instantiate a new server - acceptable port and IP range,
            // and set up your methods.       
            
            WsServer server = new WsServer();
             server.start();
           

           // ArrayList tweets = ElasticService.getTweets("tvit", "prvi");
           // string response="";
           // foreach (Tweet t in tweets)
           // {
           //     response += t.created_at + " " + t.text + " " + t.followers_count+"/n";
           // }
           // Console.WriteLine(response);
           // tweets = ElasticService.getTweets("tvit", "drugi");
           //response = "";
           // foreach (Tweet t in tweets)
           // {
           //     response += t.created_at + " " + t.text + " " + t.followers_count + "/n";
           // }
           // Console.WriteLine(response);
           // Console.ReadLine();
         




        }
    }
}