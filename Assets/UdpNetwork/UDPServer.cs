using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;

namespace UdpNetwork
{
    class UDPServer : UDPBase
    {

        List<ClientStatus> clients = new List<ClientStatus>();
        public UDPServer(int setPort) : base(setPort)
        {
        }

        protected override void RecieveData(IPAddress address, int port, byte[] rcvBytes)
        {
            base.RecieveData(address, port, rcvBytes);

            Debug.Log("RecieveData(): id = " + rcvBytes[0]);

            ClientStatus sender = null;
            if (rcvBytes[0] != Message.IdSessionStart)
            {
                sender = clients.Where(client => client.address.ToString() == address.ToString() && client.port == port).First();
            }

            //データを文字列に変換する
            switch (rcvBytes[0])
            {
                case Message.IdSessionStart:
                    RecieveSessionStartClient(address, port, rcvBytes);
                    break;

                case Message.IdCreateNetworkObject:
                    SendClient(rcvBytes);
                    break;
                default:
                    //データを文字列に変換する
                    string rcvMsg = System.Text.Encoding.UTF8.GetString(rcvBytes);
                    Debug.Log("send text:" + rcvMsg);
                    SendClient(sender.nickName + ":" + rcvMsg);
                    break;
            }
        }


        private void RecieveSessionStartClient(IPAddress address, int port, byte[] rcvBytes)
        {
            ClientStatus newClient = null;
            using (var memory = new MemoryStream(rcvBytes))
            {
                using (BinaryReader binaryStream = new BinaryReader(memory, System.Text.Encoding.UTF8))
                {
                    byte id = binaryStream.ReadByte();
                    string name = binaryStream.ReadString();
                    Debug.Log(name);

                    newClient = new ClientStatus();
                    newClient.SetData(address, port, name);
                    newClient.SetNetworkId(clients.Count);
                    clients.Add(newClient);
                }
            }


            {   // networkidを返す

                SendNetworkId(newClient);
            }

            foreach (var client in clients)
            {
                string sendText = "" + newClient.nickName + " is join.";

                byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(sendText);

                Send(client, sendBytes);
            }
        }


        private void SendNetworkId(ClientStatus client)
        {

            using (var memory = new MemoryStream())
            {

                using (BinaryWriter binaryStream = new BinaryWriter(memory, System.Text.Encoding.UTF8))
                {
                    binaryStream.Write(Message.IdGetNetworkId);
                    binaryStream.Write(client.networkId);


                    Debug.Log("return msg: ---------------");
                    Debug.Log("nickName:" + client.nickName);
                    Debug.Log("address:" + client.address);
                    Debug.Log("port:" + client.port);
                    Debug.Log("networkId:" + client.networkId);
                    Debug.Log("---------------------------");
                }

                Send(client, memory.ToArray());
            }
        }

        private void SendClient(byte[] sendBytes)
        {
            foreach (var client in clients)
            {
                Send(client, sendBytes);
            }
        }

        private void SendClient(string sendText)
        {
            byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(sendText);
            foreach (var client in clients)
            {
                Debug.Log(client.nickName + " : " + sendText);
                Send(client, sendBytes);
            }
        }
    }
}
