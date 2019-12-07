using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace UdpNetwork
{
    class UDPBase
    {
        protected int myPort = 2001;
        protected int sendPort = 2002;

        private UdpClient udpClient = null;


        public UDPBase(int setPort)
        {
            CreateReceive(setPort);
        }

        private void CreateReceive(int setPort)
        {
            Debug.Log("CreateReceive 000");
            myPort = setPort;
            //UdpClientを作成し、指定したポート番号にバインドする
            System.Net.IPEndPoint localEP = new System.Net.IPEndPoint(System.Net.IPAddress.Any, myPort);
            udpClient = new System.Net.Sockets.UdpClient(localEP);

            //非同期的なデータ受信を開始する
            udpClient.BeginReceive(ReceiveCallback, udpClient);
        }



        //データを受信した時
        private void ReceiveCallback(IAsyncResult ar)
        {
            System.Net.Sockets.UdpClient udp = (System.Net.Sockets.UdpClient)ar.AsyncState;

            //非同期受信を終了する
            System.Net.IPEndPoint remoteEP = null;
            byte[] rcvBytes;
            try
            {
                rcvBytes = udp.EndReceive(ar, ref remoteEP);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Debug.Log("受信エラー({0}/{1}) :" + ex.Message + " :" + ex.ErrorCode);
                return;
            }
            catch (ObjectDisposedException ex)
            {
                //すでに閉じている時は終了
                Debug.Log("Socketは閉じられています。:" + ex.ToString());
                return;
            }

            RecieveData(remoteEP.Address, remoteEP.Port, rcvBytes);


            //再びデータ受信を開始する
            udp.BeginReceive(ReceiveCallback, udp);
        }


        protected virtual void RecieveData(IPAddress address, int port, byte[] rcvBytes)
        {

        }


        public void Send(ClientStatus sentClient, byte[] sendBytes)
        {

            //UdpClientを作成する
            if (udpClient == null)
            {
                udpClient = new System.Net.Sockets.UdpClient();
            }

            //非同期的にデータを送信する
            udpClient.BeginSend(sendBytes, sendBytes.Length, sentClient.address.ToString(), sentClient.port, SendCallback, udpClient);
        }



        //データを送信した時
        private static void SendCallback(IAsyncResult ar)
        {
            System.Net.Sockets.UdpClient udp =
                (System.Net.Sockets.UdpClient)ar.AsyncState;

            //非同期送信を終了する
            try
            {
                udp.EndSend(ar);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Debug.Log("送信エラー({0}/{1})" + " :" + ex.Message + " :" + ex.ErrorCode);
            }
            catch (ObjectDisposedException ex)
            {
                //すでに閉じている時は終了
                Debug.Log("Socketは閉じられています。:" + ex.ToString());
            }
        }
    }
}
