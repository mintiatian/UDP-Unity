using System.Collections.Generic;
using System.Net;

namespace UdpNetwork
{
    class ClientStatus
    {
        public IPAddress address;
        public int port;
        public string nickName;
        public int networkId;

        public List<byte[]> syncDatas;

        public void SetData(IPAddress address, int port, string nickName)
        {
            this.address = address;
            this.port = port;
            this.nickName = nickName;

            syncDatas = new List<byte[]>();
        }

        public void SetNetworkId(int networkId)
        {
            this.networkId = networkId;
        }
    }
}
