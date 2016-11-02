using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski_server
{

    public class Tweet
    {
        public String created_at { get; set; }
        public String text { get; set; }
        public String screen_name { get; set; }
        public String followers_count { get; set; }
        public String id_str { get; set; }
        public Tweet()
        { }
        public Tweet(String created_at, String text, String screen_name, String followers_count)
        {
            this.created_at = created_at;
            this.text = text;
            this.followers_count = followers_count;
            this.screen_name = screen_name;

        }
    }
}
