using UnityEngine;

namespace UdpNetwork
{
    public class UdpServerBehaviour : MonoBehaviour
    {
        private UDPServer _udpServer;
        // Start is called before the first frame update
        void Start()
        {
            _udpServer = new UDPServer(2001);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
