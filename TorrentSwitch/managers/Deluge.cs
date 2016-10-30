using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookComputing.XmlRpc;


namespace TorrentSwitch.managers
{
    class Deluge
    {
        public struct SumAndDiffValue
        {
            public int sum;
            public int difference;
        }

        [XmlRpcUrl("10.0.0.123:8112/json")]
        public interface ISumAndDiff : IXmlRpcProxy
        {
            [XmlRpcMethod]
            SumAndDiffValue SumAndDifference(int x, int y);
        }
    }
}
