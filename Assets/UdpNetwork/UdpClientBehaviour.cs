using System.IO;
using System.Net;
using UdpNetwork;
using UnityEngine;

public class UdpClientBehaviour : MonoBehaviour
{
    public static int MyUsePort = 2002;
    bool Owner = false;

    public static void CreateNetworkGameObject(bool owner, Vector3 pos)
    {
        var createNetworkObject = Instantiate(Resources.Load("Cube", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
        createNetworkObject.name = "name_" + Random.value;

        var client = createNetworkObject.GetComponent<UdpClientBehaviour>();
        client.Owner = owner;

    }

    private UDPClient udpClient;

    // Start is called before the first frame update
    void Start()
    {
        udpClient = new UDPClient(MyUsePort, 2001, gameObject.name);

        // 他のクライアントに通知する
        if (Owner)
        {
            SendCreateNetworkObject(transform.position);
        }

        udpClient.CreateNetworkObject = new UDPClient.ReceiveCreateNetworkObject(ReceiveCreateNetworkObjectCallBack);
    }


    public const byte Id = Message.IdCreateNetworkObject;
    public class CreateType
    {
        public const byte UdpClientBehaviour = 0;
    }

    void SendCreateNetworkObject(Vector3 sendVec)
    {

        using (var memory = new MemoryStream())
        {

            using (BinaryWriter binaryStream = new BinaryWriter(memory, System.Text.Encoding.UTF8))
            {
                binaryStream.Write(Message.IdCreateNetworkObject);
                binaryStream.Write(CreateType.UdpClientBehaviour);
                binaryStream.Write(sendVec.x);
                binaryStream.Write(sendVec.y);
                binaryStream.Write(sendVec.z);
            }

            udpClient.Send(memory.ToArray());
        }
    }

    void ReceiveCreateNetworkObjectCallBack(IPAddress address, int port, byte[] rcvBytes)
    {

        using (var memory = new MemoryStream(rcvBytes))
        {
            using (BinaryReader binaryStream = new BinaryReader(memory, System.Text.Encoding.UTF8))
            {
                byte id = binaryStream.ReadByte();
                byte createType = binaryStream.ReadByte();

                Vector3 pos = new Vector3();
                switch (createType)
                {
                    case CreateType.UdpClientBehaviour:

                        pos.x = binaryStream.ReadSingle();
                        pos.y = binaryStream.ReadSingle();
                        pos.z = binaryStream.ReadSingle();

                        break;
                }

                CreateNetworkGameObject(false, pos);
            }
        }
    }

}
