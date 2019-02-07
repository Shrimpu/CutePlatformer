using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Range(1, 6)]
    public int playerInputID = 1;
    [HideInInspector]
    public int playerID = 1;

    float xInput;
    string horizontal;

    void Start()
    {
        AssignInputKeys(playerInputID);
    }

    void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        xInput = Input.GetAxisRaw(horizontal);
    }

    void AssignInputKeys(int id) // makes it take in the id just so that its use isn't limited to this class
    {
        if (id == 1)
        {
            horizontal = InputManager.CustomInputs.P1Keyboard.horizontal;
        }
        if (id == 2)
        {
            horizontal = InputManager.CustomInputs.P2Keyboard.horizontal;
        }
        if (id == 3)
        {
            horizontal = InputManager.CustomInputs.P1.horizontal;
        }
        if (id == 4)
        {
            horizontal = InputManager.CustomInputs.P2.horizontal;
        }
        if (id == 5)
        {
            horizontal = InputManager.CustomInputs.P3.horizontal;
        }
        if (id == 6)
        {
            horizontal = InputManager.CustomInputs.P4.horizontal;
        }
    }
}
