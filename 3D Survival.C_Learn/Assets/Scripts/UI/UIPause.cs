using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class UIPause : BasePopup
{
    // Start is called before the first frame update
    protected override void Start()
    {
        CharacterManager.Instance.Player.controller.pause += Toggle;
        base.Start();
    }
}
