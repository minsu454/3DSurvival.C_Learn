using UnityEngine;

public class BasePopup : MonoBehaviour
{
    public GameObject window;

    protected virtual void Start()
    {
        window.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            window.SetActive(false);
        }
        else
        {
            window.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return window.activeInHierarchy;
    }
}
