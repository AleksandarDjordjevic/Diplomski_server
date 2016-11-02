using Nest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski_server
{
   public class ElasticService
    {
        public static void AddTweet(Tweet tweet, string index)
        {
            ElasticClient client = DataClient.getDataClient();
            client.Index(tweet, i => i
              .Index(index)
              .Refresh()
              .Ttl("1m"));
        }
        public static ArrayList getTweets(string searchWord, string index)
        {
            ArrayList tweets = new ArrayList();
            ElasticClient client = DataClient.getDataClient();

            // var result = client.Search<Tweet>(s => s.Index(index).Query(q => q.QueryString(d => d.Query(searchWord))));
            var result = client.Search<Tweet>(s => s.Index(index).Size(50).Query(q => q.Term(p => p.text, searchWord))); 

            foreach (Tweet document in result.Documents)
            {
                tweets.Add(document);
            }
            return tweets;

        }
        public static void deleteIndex(string index)
        {
            ElasticClient client = DataClient.getDataClient();
            client.DeleteIndex(index);
        }
    }
}
