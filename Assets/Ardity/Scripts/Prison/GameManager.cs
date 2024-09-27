using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

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

    [SerializeField]
    List<TextMeshPro> dialogues;
    [SerializeField]
    public TextMeshPro speakerName;
    [SerializeField]
    AudioSource yesNoAudio;
    [SerializeField]
    AudioClip yesClip;
    [SerializeField]
    AudioClip noClip;

    private Coroutine beepEnum;



    private void Awake()
    {
        Instance = this;
    
        audioSource = GetComponent<AudioSource>();

        storyInput = null;
    }

    void Start()
    {
        StartCoroutine(StartStoryCall());
        //StartCoroutine(CallRingPhoneAfterDelay(StringData.ringC, 2f));
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
    private void Update()
    {
        if (!isPhoneUp)
        {
            beepAudiosource.Stop();

        }
    }

    public void RingPhone(string ringTone)
    {
        NumberOfPerson = 1;
        SerialController.SendSerialMessage(ringTone);
    }

    private IEnumerator StartNextCall()
    {
        speakerName.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.1f);
        speakerName.text = "Customer";
        speakerName.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutSine);

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

    IEnumerator WaitForBeep(float wait) 
    {
        yield return new WaitForSeconds((wait));

        PlayBeep();
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

                //Timer.Instance.StartCountdown(currentActiveClip.timeBeforeNextCall + currentActiveClip.clip.length);
                Debug.Log("time : " + currentActiveClip.timeBeforeNextCall + currentActiveClip.clip.length);
                StartCoroutine(StartNextCall());
                beepEnum = StartCoroutine(WaitForBeep(currentActiveClip.clip.length - 0.3f));
            }
            else if (message == StringData.down && !isOnStoryCall)
            {
                //Timer.Instance.StartCountdown(currentActiveClip.timeBeforeNextCall + currentActiveClip.clip.length);
                Timer.Instance.StartCountdown(0.5f);
                Debug.Log("time : " + currentActiveClip.timeBeforeNextCall + currentActiveClip.clip.length);
                NumberOfPerson = 0;
                isSomeoneOnHold = false;

                beepAudiosource.Stop();
                audioSource.Stop();
                holdAudioSource.Stop();
                beepAudiosource.Stop();

                StopCoroutine(beepEnum);
            }
            else if (int.TryParse(message, out int messageInt) && !CallDetails[ActiveIndex].isCalldone && CallDetails[ActiveIndex].group.Contains(messageInt))
            {
                speakerName.DOColor(Color.green, 0.5f).SmoothRewind();

                Wins++;
                CallDetails[ActiveIndex].isCalldone = true;
                audioSource.Stop();
                holdAudioSource.Stop();

                StartCoroutine(WaitWinLose(() => yesNoAudio.PlayOneShot(yesClip)));

                beepAudiosource.Play();
            }

            else if (!CallDetails[ActiveIndex].isCalldone)
            {
                speakerName.DOColor(Color.red, 0.5f).SmoothRewind();
                
                Loses++;
                CallDetails[ActiveIndex].isCalldone = true;
                audioSource.Stop();
                holdAudioSource.Stop();

                StartCoroutine(WaitWinLose(() => yesNoAudio.PlayOneShot(noClip)));

                beepAudiosource.Play();
            }
        }

        else
        {
            storyInput = message;
            if (message == StringData.down)
            {
                storyInput = null;
                isOnStoryCall = false;

                StoryCalls.RemoveAt(StoryCallIndex);
                Currentindex--;
                ActiveIndex = Currentindex;

                Timer.Instance.StartCountdown(1.5f);
                NumberOfPerson = 0;
                isSomeoneOnHold = false;

                beepAudiosource.Stop();
                audioSource.Stop();
                holdAudioSource.Stop();
                beepAudiosource.Stop();

                StopCoroutine(beepEnum);
            }
      
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
            //PlayBeep();
            // TODO: Missed
        }

        else 
        {
            Currentindex++;
            Debug.Log(Currentindex);
            ActiveIndex = Currentindex;
            RingPhone(StringData.ringA);
        }
    }

    public IEnumerator StartStoryCall()
    {
        speakerName.DOColor(Color.clear, 0.2f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.2f);
        speakerName.text = "Friend";
        speakerName.DOColor(Color.white, 0.2f).SetEase(Ease.OutSine);

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
            if(i < story.talks.Count)
            {
                for (int j = 0; j < story.talks[i].Theoptions.Count; j++)
                {
                    dialogues[j].SetText(story.talks[i].Theoptions[j].ToString());

                    // Reset the scale to zero before applying the animation
                    RectTransform recT = dialogues[j].transform.GetComponent<RectTransform>();

                    // Animate the scale with a bounce effect
                    recT.DOAnchorPos(recT.anchoredPosition + Vector2.right * 10, 0.1f).SetEase(Ease.OutSine);

                    yield return new WaitForSeconds(0.1f);
                }
            }

            storyInput = null;
            yield return new WaitUntil(() => storyInput != null || i + 1 >= story.clip.Count);

            if(i < story.talks.Count)
            {
                for (int j = 0; j < story.talks[i].Theoptions.Count; j++)
                {
                    // Reset the scale to zero before applying the animation
                    RectTransform recT = dialogues[j].transform.GetComponent<RectTransform>();

                    recT.DOAnchorPos(recT.anchoredPosition + Vector2.left * 10, 0.1f).SetEase(Ease.OutSine);

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        beepEnum = StartCoroutine(WaitForBeep(0.1f));
    }


    private IEnumerator WaitForAudio(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
    }

    private IEnumerator WaitWinLose(Action action)
    {
        yield return new WaitForSeconds(0.2f);

        action();
    }
}
