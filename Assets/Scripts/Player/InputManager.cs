using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    public Action keyaction = null;
    public Action FixedKeyaction = null;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 200;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(keyaction != null)
        {
            keyaction.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (FixedKeyaction != null)
        {
            FixedKeyaction.Invoke();
        }
    }

}
