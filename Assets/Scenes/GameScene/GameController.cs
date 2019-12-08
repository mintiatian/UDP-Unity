using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject networkGameObject;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(networkGameObject, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
