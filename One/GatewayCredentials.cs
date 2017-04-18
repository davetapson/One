using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One
{
    class GatewayCredentials
    {
        public GatewayCredentials()
        {
            DefaultCredentials();
        }

        public GatewayCredentials(string[] args)
        {
            try
            {
                if (args == null ||
                    args.Length == 0)
                {
                    DefaultCredentials();
                }
                else
                {
                    Host = args[0];
                    Port = Convert.ToInt32(args[1]);
                    ClientId = Convert.ToInt32(args[2]);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid argument/s passed to GatewayCredentials\n" + e.Message);
            }
        }

        private void DefaultCredentials()
        {
            // todo - db params
            Host = "127.0.0.1";
            Port = 4002;
            ClientId = 0;
        }

        public int ClientId { get; internal set; }
        public string Host { get; internal set; }
        public int Port { get; internal set; }
    }
}
