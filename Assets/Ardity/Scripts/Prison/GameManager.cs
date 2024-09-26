using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    SerialController serialController;
    bool startAgain;
    int numberOfPerson=0;
    [SerializeField]
    List<CallDetails> callDetails;
    int currentindex=0;
    // Start is called before the first frame update

    private void OnEnable()
    {
        GetMessage.OnMessageReceived += GetInputMessage;
    }

    private void OnDisable()
    {
        GetMessage.OnMessageReceived -= GetInputMessage;
    }
    void Start()
    {

        StartCoroutine(CallRingPhoneAfterDelay(StringData.ringC, 2f));
    }



    private IEnumerator CallRingPhoneAfterDelay(string ringTone, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for 2 seconds
        RingPhone(ringTone);                    // Call the RingPhone function
    }


    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

     public void RingPhone(string ringTone)
    {
        numberOfPerson = 1;
        Debug.Log("rang");
        serialController.SendSerialMessage(ringTone);
    }

    void GetInputMessage(string message)
    {
        if (message == "up")
        {
            Timer.Instance.StartCountdown(callDetails[currentindex].clipLength);
        }
        else if (message == StringData.down && numberOfPerson == 2)
        {
            Timer.Instance.StartCountdown(callDetails[currentindex].clipLength);
        }
    }

    public void playBeep()
    {
        numberOfPerson = 2;
        Debug.Log("Beeping");
    }

    



    public IEnumerator CallDecision(float delay)
    {
                                                //

        if (GetMessage.currentMessage == StringData.up)
        {
           //on hold to do
            playBeep();
            yield return new WaitForSeconds(delay);


        }
        else if (GetMessage.currentMessage == StringData.down)
        {
            RingPhone(StringData.ringA);
        }
    }
}
