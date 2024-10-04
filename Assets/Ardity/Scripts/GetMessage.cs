using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetMessage : MonoBehaviour
{

    public static event Action<string> OnMessageReceived;
    public static string currentMessage; 
    [SerializeField]
    TextMeshProUGUI screenText;
    string msg = null;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        HandleNumberInput();
        HandleDirectionInput();
    }

    // Handle input for number keys 1 to 9
    private void HandleNumberInput()
    {
        // Check if any key was pressed
        if (!string.IsNullOrEmpty(Input.inputString))
        {
            // Check if the input is between '1' and '9'
            char inputChar = Input.inputString[0];

            if (inputChar >= '1' && inputChar <= '9')
            {
                msg = inputChar.ToString();
                OnMessageReceived?.Invoke(msg);
                Debug.Log("Key pressed: " + msg);
            }
        }

    }

    // Handle input for U (up) and I (down) keys
    private void HandleDirectionInput()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            msg = StringData.up;
            OnMessageReceived?.Invoke(msg);
            GameManager.Instance.isPhoneUp = true;

        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            msg = StringData.down;
            OnMessageReceived?.Invoke(msg);
            GameManager.Instance.isPhoneUp = false;

        }
    }

    //void OnMessageArrived(string msg)
    //{
    //    //screenText.text = msg;
    //    Debug.Log(msg);
    //    OnMessageReceived?.Invoke(msg);
    //    if (msg == StringData.up)
    //    {
    //        GameManager.Instance.isPhoneUp = true;
    //    }
    //    else if (msg == StringData.down)
    //    {
    //        GameManager.Instance.isPhoneUp = false;
    //    }

    //    currentMessage = msg;
    //}

    //// Invoked when a connect/disconnect event occurs. The parameter 'success'
    //// will be 'true' upon connection, and 'false' upon disconnection or
    //// failure to connect.
    //void OnConnectionEvent(bool success)
    //{
        
    //    Debug.Log  (success);
        
    //}
}
