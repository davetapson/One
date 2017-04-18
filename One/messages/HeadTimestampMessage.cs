using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One.messages
{
    public class HeadTimestampMessage
    {
        public int ReqId { get; private set; }
        public string HeadTimestamp { get; private set; }

        public HeadTimestampMessage(int reqId, string headTimestamp)
        {
            this.ReqId = reqId;
            this.HeadTimestamp = headTimestamp;
        }
    }
}
