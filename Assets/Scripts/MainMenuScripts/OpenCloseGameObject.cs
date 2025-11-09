using UnityEngine;

public class OpenCloseGameObject : MonoBehaviour
{
    public GameObject obj;

    public void open()
    {

        obj.SetActive(true);
    
    }

    public void close()
    {

        obj.SetActive(false);

    }
}
