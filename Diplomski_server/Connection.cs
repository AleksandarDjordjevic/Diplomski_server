using Alchemy.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski_server
{
    public class Connection
    {
        public UserContext Context { get; set; }
        TwitterServerConn tsc;
        public Connection(UserContext user)
        {
            Context = user;
             tsc = new TwitterServerConn(Context);
            
        }
        
        public void parseMessage()
        {
            Console.WriteLine("Message parsed!");
            if (Context.DataFrame.ToString().Equals("renew"))
                tsc.renew();
            else
            {
                dynamic data = JObject.Parse(Context.DataFrame.ToString());

                Console.WriteLine("Keyword: " + data.keyword + " Searchword: " + data.searchword);
                tsc.start((string)data.keyword, ((string)data.searchword));
            }
        }
        public void stop()
        {
            this.tsc.stop();
        }

      /*  private void TimerCallback(object state)
        {
            try
            {
                // Sending Data to the Client
                Context.Send("[" + Context.ClientAddress.ToString() + "] " + System.DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }*/
    }
}
