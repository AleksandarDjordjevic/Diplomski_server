using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski_server
{
    public static class DataClient
    {

        public static ElasticClient client;

        public static ElasticClient getDataClient()
        {
            if (client == null)
            {
                var node = new Uri("http://localhost:9200");

                var settings = new ConnectionSettings(node);

                client = new ElasticClient(settings);
            }
            return client;

        }
    }
}
