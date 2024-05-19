using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance { get; private set; }

    public bool IsPressedPrimaryButton = false;
    public bool IsPressedSecondaryButton = false;
    public bool IsPressedWheelButton = false;

    public static KeyCode Key_Jump = KeyCode.Space;
    public static KeyCode Key_Dash = KeyCode.LeftShift;


    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        IsPressedPrimaryButton = Input.GetMouseButtonDown(0);
        IsPressedSecondaryButton = Input.GetMouseButtonDown(1);
        IsPressedWheelButton = Input.GetMouseButtonDown(2);
    }
}
