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
    [SerializeField]
    private List<StoryCalls> StoryCalls;
    private int ActiveIndex = 0;
    private int Currentindex = 0;
    private int OnHoldIndex = 0;
    private int OnHoldTimeSample = 0;
    bool hasGroupIsChosen;
    bool isBeeping;
    [SerializeField]
   
    private int Wins = 0;
    [SerializeField]
    private int Loses = 0;
    private bool isSomeoneOnHold = false;
    public bool isPhoneUp;
    private AudioSource audioSource;
    [SerializeField]
    AudioSource holdAudioSource;
    [SerializeField]
    AudioSource beepAudiosource;

    CallDetails currentActiveClip;

    int StoryCallIndex;
    int StoryCallPhase;
    bool isOnStoryCall;
    string storyInput;

    private void Awake()
    {
        Instance = this;
    
        audioSource = GetComponent<AudioSource>();

        storyInput = null;
    }

    void Start()
    {
        StartCoroutine(CallRingPhoneAfterDelay(StringData.ringC, 2f));
        currentActiveClip = CallDetails[0];
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
        if (holdAudioSource.volume == 0) {

            holdAudioSource.volume = 1;
            audioSource.volume = 0;

            holdAudioSource.clip = CallDetails[ActiveIndex].clip;
            currentActiveClip = CallDetails[ActiveIndex];
            if(!holdAudioSource.isPlaying)
                holdAudioSource.Play();
        }
        else if (audioSource.volume == 0)
        {
            holdAudioSource.volume = 0;
            audioSource.volume = 1;

            audioSource.clip = CallDetails[ActiveIndex].clip;
            currentActiveClip = CallDetails[ActiveIndex];
            if (!audioSource.isPlaying)
                audioSource.Play();
        } 
    }

    void GetInputMessage(string message)
    {
        if(!isOnStoryCall)
        {
            if (message == "up" && NumberOfPerson == 1 && !isOnStoryCall)
            {
                if (StoryCalls.Exists(x => x.afterThisCall == Currentindex))
                {
                    StartCoroutine(StartStoryCall());
                    return;
                }

                Timer.Instance.StartCountdown(currentActiveClip.timeBeforeNextCall + currentActiveClip.clip.length);
                Debug.Log("time : " + currentActiveClip.timeBeforeNextCall + currentActiveClip.clip.length);

                StartNextCall();
            }
            else if (message == StringData.down && !isOnStoryCall)
            {
                if (NumberOfPerson == 2)
                    Timer.Instance.StartCountdown(currentActiveClip.timeBeforeNextCall + currentActiveClip.clip.length);
                Debug.Log("time : " + currentActiveClip.timeBeforeNextCall + currentActiveClip.clip.length);
                NumberOfPerson = 0;
                isSomeoneOnHold = false;

                beepAudiosource.Stop();
                audioSource.Stop();
                holdAudioSource.Stop();
            }
            else if (message == "*" && !isOnStoryCall)
            {
                if (NumberOfPerson == 2)
                {
                    if (!isSomeoneOnHold)
                    {
                        OnHoldIndex = ActiveIndex;
                        isSomeoneOnHold = true;

                        Currentindex++;
                        ActiveIndex = Currentindex;

                        StartNextCall();
                    }

                    else
                    {
                        (ActiveIndex, OnHoldIndex) = (OnHoldIndex, ActiveIndex);

                        StartNextCall();
                    }
                }
            }

            else if (message == CallDetails[ActiveIndex].group.ToString() && !CallDetails[ActiveIndex].isCalldone)
            {
                Wins++;
                CallDetails[ActiveIndex].isCalldone = true;
            }

            else if (!CallDetails[ActiveIndex].isCalldone)
            {
                Loses++;
                CallDetails[ActiveIndex].isCalldone = true;
            }
        }

        else
        {
            storyInput = message;
            if (message == StringData.down) ;
                
        }
        
    }

    public void PlayBeep()
    {
        NumberOfPerson = 2;
        beepAudiosource.Play();
        isBeeping = true;

    }

    public IEnumerator CallDecision(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isPhoneUp)
        {
            PlayBeep();
        }

        else 
        {
            Currentindex++;
            ActiveIndex = Currentindex;
            RingPhone(StringData.ringA);
        }
    }

    public IEnumerator StartStoryCall()
    {
        StoryCalls story = StoryCalls[StoryCallIndex];
        for (int i = 0; i < story.clip.Count; i++)
        {
            isOnStoryCall = true;

            audioSource.volume = 1;
            holdAudioSource.volume = 0;

            audioSource.clip = story.clip[i];
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);

            // TODO Show the string

            yield return new WaitUntil(() => storyInput != null);

            storyInput = null;
        }

        isOnStoryCall = false;
    }
}
