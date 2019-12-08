using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //public GameObject networkGameObject;
    public Dropdown settingIp;

    // Start is called before the first frame update
    public void Create()
    {

        UdpClientBehaviour.MyUsePort += settingIp.value;

        Debug.Log("MyUsePort:" + UdpClientBehaviour.MyUsePort);
        UdpClientBehaviour.CreateNetworkGameObject(true, new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5)));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
