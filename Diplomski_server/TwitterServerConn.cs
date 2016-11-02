using Alchemy.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.ComponentModel;


namespace Diplomski_server
{

    public class TwitterServerConn
    {
        Client socket;
        private UserContext Context;
        WebClient webClient;
        private int count = 0;
        private string id = Constants.getUserID();
        private string searchword = "";
        private BackgroundWorker backgroundThread;
        private JArray a;

        public TwitterServerConn(UserContext user)
        {
            this.Context = user;
            webClient = new WebClient();
            prepareSocket();
            backgroundThread = new BackgroundWorker();
            this.backgroundThread.WorkerSupportsCancellation = true;
            backgroundThread.DoWork +=
               new DoWorkEventHandler(indexNewTweets);
            backgroundThread.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundThread_RunWorkerCompleted);
           // backgroundThread.ProgressChanged +=
             //   new ProgressChangedEventHandler(
           // backgroundThread_ProgressChanged);

        }

        public void start(string keyword, string searchword)
        {       
                this.searchword = searchword;
            JObject obj = new JObject();
            obj.Add("keyword", keyword);
                socket.Emit("filter", obj);
        }
       
        
        public void stop()
        {
            Console.WriteLine("Exiting service for client with ID " +id+"...");
            socket.Emit("stop", null);
            ElasticService.deleteIndex("index_" + id);
            Console.WriteLine("Client disconnected");          
        }

        public void renew()
        {
            socket.Emit("get_tweets", null);
        }
        private void indexNewTweets(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            indexTweets(worker, e);
        }
        private void indexTweets(BackgroundWorker sender, DoWorkEventArgs e)
        {

            int number = 0;
            if(a.Count>0)
                Console.WriteLine("Indexing for client with ID "+id+"...");
            foreach (JObject o in a.Children<JObject>())
            {
                if (sender.CancellationPending)
                {   e.Cancel = true;
                    Console.WriteLine("\nPartial indexing done with "+number+" tweets out of "+a.Count);
                    return;   }
            
                Tweet tvit = new Tweet();
                number++;
                Console.Write(number + "//" + a.Count+" ");
                foreach (JProperty p in o.Properties())
                {                
                    string name = p.Name;
                    string value = (string)p.Value;
                    if (name.Equals("text"))
                    {
                        value = value.Replace("\"", "");
                        tvit.text = value;
                    }
                    if (name.Equals("created_at"))
                        tvit.created_at = value;
                    if (name.Equals("screen_name"))
                        tvit.screen_name = value;
                    if (name.Equals("followers_count"))
                        tvit.followers_count = value;
                    if (name.Equals("id_str"))
                        tvit.id_str = value;
                }
                ElasticService.AddTweet(tvit, "index_" + id);
                
            }
            
            if(a.Count>0)
                Console.WriteLine("Indexing done. Indexed "+number+" tweets for client with ID "+id+"!");
        }

        public void SocketOpened(object sender, EventArgs e)
        {
            Console.WriteLine("Socket is now open");
        }
        public void SocketMessage(object sender, EventArgs e)
        {
            Console.WriteLine("New message from SocketIO");
        }
        public void SocketError(object sender, EventArgs e)
        {
            Console.WriteLine("Socket error");
        }
        public void SocketConnectionClosed(object sender, EventArgs e)
        {
            Console.WriteLine("Socket connection closed");
        }

        private void prepareSocket()
        {

            socket = new Client(Constants.SERVER_LOCATION);


            socket.Opened += SocketOpened;
            socket.Message += SocketMessage;
            socket.SocketConnectionClosed += SocketConnectionClosed;
            socket.Error += SocketError;

            socket.On("tweets", (fn) =>
            {
                       
                if (backgroundThread.IsBusy)
                    backgroundThread.CancelAsync();

                while (backgroundThread.IsBusy) ;
                dynamic data = JObject.Parse(fn.MessageText);
                dynamic my_data = data.args;

                a = JArray.Parse(JsonConvert.SerializeObject(my_data[0]));
                Console.WriteLine("I've just got " + a.Count + " tweets from Twitter server for client with ID " + id);
                count += a.Count;
                //indeksiranje tvitova pribavljenih od Tviter servera   
                backgroundThread.RunWorkerAsync();

            });

            socket.Connect();
                      
        }

        private void sendTweets()//sending tweets to client
        {
            ArrayList clientTweets = ElasticService.getTweets(searchword, "index_" + id);

            Console.WriteLine("Found " + clientTweets.Count + " tweets for client with ID " + id);
            JArray returnArray = new JArray();
            foreach (Tweet t in clientTweets)
            {
                var json = JsonConvert.SerializeObject(t);
                returnArray.Add(json);
            }
            if (returnArray.Count > 0)
                Context.Send(returnArray.ToString());
           
            Console.WriteLine("\n");
        }
        private void backgroundThread_RunWorkerCompleted(
             object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                Console.WriteLine(e.Error.Message);
            }
            else if (e.Cancelled)//handle cancelation
            {
                Console.WriteLine("Partial indexing done");
                sendTweets();
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                Console.WriteLine("All tweets indexed");
                sendTweets();
            }

        }

    }


}
