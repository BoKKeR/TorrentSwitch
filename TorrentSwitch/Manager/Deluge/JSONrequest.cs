using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentSwitch.managers.Deluge.JSONrequest
{




    public class JSONrequest
    {

        public JProperty method { get; set; }
        public JProperty parameters { get; set; }
        public JProperty id { get; set; }


    public static JObject initializeJSON()
        {
            JArray jarrayObj = new JArray();
            //jarrayObj.Add();

            //if (/*isTorrentRequest*/)
            //{
                //JObject torrentExtraObject = new JObject();
                //jarrayObj.Add(torrentExtraObject);
            //}

            JObject X = new JObject(
                                new JProperty("method", ""),
                                new JProperty("params", jarrayObj),
                                new JProperty("id", "1"));

        return X; 
        }
    }
}
