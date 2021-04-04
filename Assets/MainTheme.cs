using UnityEngine;

public class MainTheme : MonoBehaviour
{
    void Start()
    {
        if (FindObjectsOfType<MainTheme>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}