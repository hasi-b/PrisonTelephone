using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private SerialController SerialController;
    private int NumberOfPerson = 0;
    [SerializeField]
    private List<CallDetails> CallDetails;
    private int ActiveIndex = -1;
    private int Currentindex = -1;
    private int OnHoldIndex = -1;
    private int OnHoldTimeSample = 0;
    private int Wins = 0;
    private int Loses = 0;
    private bool isSomeoneOnHold = false;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(CallRingPhoneAfterDelay(StringData.ringC, 2f));
    }

    private void OnEnable()
    {
        GetMessage.OnMessageReceived += GetInputMessage;
    }

    private void OnDisable()
    {
        GetMessage.OnMessageReceived -= GetInputMessage;
    }

    private IEnumerator CallRingPhoneAfterDelay(string ringTone, float delay)
    {
        yield return new WaitForSeconds(delay);
        RingPhone(ringTone);
    }

    public void RingPhone(string ringTone)
    {
        NumberOfPerson = 1;
        SerialController.SendSerialMessage(ringTone);
    }

    private void StartNextCall()
    {
        Currentindex++;
        ActiveIndex = Currentindex;

        audioSource.timeSamples = 0;
        audioSource.PlayOneShot(CallDetails[ActiveIndex].clip);
    }

    void GetInputMessage(string message)
    {
        CallDetails callDetails = CallDetails[ActiveIndex];
        if (message == "up" && NumberOfPerson == 1)
        {
            Timer.Instance.StartCountdown(callDetails.timeBeforeNextCall);

            StartNextCall();
        }
        else if (message == StringData.down)
        {
            NumberOfPerson = 0;
            isSomeoneOnHold = false;
            if(NumberOfPerson == 2)
                Timer.Instance.StartCountdown(callDetails.timeBeforeNextCall);
        }
        else if(message == "*")
        {
            if(NumberOfPerson == 2)
            {
                if(!isSomeoneOnHold)
                {
                    OnHoldIndex = ActiveIndex;
                    isSomeoneOnHold = true;

                    StartNextCall();
                }

                else
                {
                    (ActiveIndex, OnHoldIndex) = (OnHoldIndex, ActiveIndex);

                    (audioSource.timeSamples, OnHoldTimeSample) = (OnHoldTimeSample, audioSource.timeSamples);

                    audioSource.Stop();
                    audioSource.PlayOneShot(CallDetails[ActiveIndex].clip);
                }
            }
        }

        else if(message == callDetails.group.ToString())
            Wins++;
        else
            Loses++;
    }

    public void PlayBeep()
    {
        NumberOfPerson = 2;
    }

    public IEnumerator CallDecision(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (GetMessage.currentMessage == StringData.up)
        {
            PlayBeep();
        }

        else if (GetMessage.currentMessage == StringData.down)
        {
            RingPhone(StringData.ringA);
        }
    }
}
