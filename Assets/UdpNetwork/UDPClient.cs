using System;
using System.IO;
using System.Net;

namespace UdpNetwork
{
    class UDPClient : UDPBase
    {
        ClientStatus status;
        ClientStatus surverStatus;
        public UDPClient(int setPort, int setServerPort, string name) : base(setPort)
        {

            // ホスト名を取得する
            string hostname = Dns.GetHostName();

            // ホスト名からIPアドレスを取得する
            IPAddress[] adrList = Dns.GetHostAddresses(hostname);

            surverStatus = new ClientStatus();
            surverStatus.address = adrList[3];
            surverStatus.port = setServerPort;
            surverStatus.nickName = "server";


            status = new ClientStatus();
            status.address = adrList[1];
            status.port = sendPort;
            status.nickName = name;

            Console.WriteLine("MyPort:" + myPort);
            Console.WriteLine("ServerPort:" + sendPort);

            SendSessionStart(surverStatus, status);
        }

        public void Test001()
        {
            SendSessionStart(surverStatus, status);
        }

        public void Test002()
        {
            Console.WriteLine("address:" + status.address);
            Console.WriteLine("port:" + status.port);
            Console.WriteLine("nickName:" + status.nickName);
            Console.WriteLine("networkId:" + status.networkId);
        }

        private void SendSessionStart(ClientStatus sendServer, ClientStatus clientStatus)
        {

            using (var memory = new MemoryStream())
            {

                using (BinaryWriter binaryStream = new BinaryWriter(memory, System.Text.Encoding.UTF8))
                {
                    binaryStream.Write(Message.IdSessionStart);
                    binaryStream.Write(clientStatus.nickName);
                }

                Send(sendServer, memory.ToArray());
            }
        }


        public void Send(byte[] sendBytes)
        {

            Send(surverStatus, sendBytes);
        }


        private void ReceiveNetworkId(IPAddress address, int port, byte[] rcvBytes)
        {

            using (var memory = new MemoryStream(rcvBytes))
            {
                using (BinaryReader binaryStream = new BinaryReader(memory, System.Text.Encoding.UTF8))
                {
                    byte id = binaryStream.ReadByte();
                    int networkId = binaryStream.ReadInt32();
                    Console.WriteLine("networkId:" + networkId);

                    status.networkId = networkId;
                }
            }
        }

        protected override void RecieveData(IPAddress address, int port, byte[] rcvBytes)
        {



            base.RecieveData(address, port, rcvBytes);



            //データを文字列に変換する
            switch (rcvBytes[0])
            {
                case Message.IdSessionStart:
                    break;
                case Message.IdGetNetworkId:
                    ReceiveNetworkId(address, port, rcvBytes);
                    break;
                default:
                    //データを文字列に変換する


                    //データを文字列に変換する
                    string rcvMsg = System.Text.Encoding.UTF8.GetString(rcvBytes);

                    //受信したデータと送信者の情報をRichTextBoxに表示する
                    string displayMsg = string.Format("[{0} ({1})] > {2}", address, port, rcvMsg);
                    //RichTextBox1.BeginInvoke(new Action<string>(ShowReceivedString), displayMsg);
                    Console.WriteLine(displayMsg);

                    break;
            }



        }
    }
}
