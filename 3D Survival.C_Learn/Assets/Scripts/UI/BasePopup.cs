using UnityEngine;

public class BasePopup : MonoBehaviour
{
    public GameObject window;

    protected virtual void Start()
    {
        window.SetActive(false);
    }

    public bool Toggle()
    {
        if (IsOpen())
        {
            window.SetActive(false);
            return false;
        }
        else
        {
            window.SetActive(true);
            return true;
        }
    }

    public bool IsOpen()
    {
        return window.activeInHierarchy;
    }
}
