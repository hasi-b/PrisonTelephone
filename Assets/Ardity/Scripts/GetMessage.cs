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
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMessageArrived(string msg)
    {
        screenText.text = msg;
        Debug.Log(msg);
        OnMessageReceived?.Invoke(msg);
        currentMessage = msg;
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        
        Debug.Log  (success);
        
    }
}
