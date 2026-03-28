using UnityEngine;

public class ShowPath : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }
}