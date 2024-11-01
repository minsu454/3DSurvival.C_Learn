using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGo;
    private IInteractable curInteractable;

    public event Action<bool, IInteractable> promptTextEvent;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime <= checkRate)
            return;

        lastCheckTime = Time.time;

        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if (hit.collider.gameObject != curInteractGo)
            {
                curInteractGo = hit.collider.gameObject;
                curInteractable = hit.collider.GetComponent<IInteractable>();
                promptTextEvent?.Invoke(true, curInteractable);
            }
        }
        else
        {
            curInteractGo = null;
            curInteractable = null;
            promptTextEvent?.Invoke(false, curInteractable);
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGo = null;
            curInteractable = null;
            promptTextEvent?.Invoke(false, curInteractable);
        }
    }
}
