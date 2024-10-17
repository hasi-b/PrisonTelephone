using System.Collections;
using Assets.Scripts.TypewriterEffects;
using TMPro;
using UnityEngine;

public class HoursController : MonoBehaviour
{
    public static HoursController Instance;
    public int CurrentHour;
    public float CurrentMins;

    public int CurrentDay;

    public int DayStartHour;
    public int DayEndHour;

    public bool IsIncreaseTimeWithRealTime;
    public float MinsPerSec;

    //TODO: Change here
    public TextMeshPro Day;
    public TextMeshPro Hours;
    public TextMeshPro Mins;

    public GameObject Game;
    public GameObject NextDayGO;

    public GameObject Day1;
    public GameObject Day2;
    public GameObject Day3;
    public GameObject Day4;
    public GameObject Day5;
    public GameObject Day6;

    public Typewriter Goal1;
    public Typewriter Goal2;
    public Typewriter Goal3;
    public Typewriter Goal4;
    public Typewriter Goal5;

    public Typewriter ExtraGoal1;
    public Typewriter ExtraGoal2;

    private void Awake()
    {
        Instance = this;

        SolveMinutes();
    }
    
    public IEnumerator StartHours()
    {
        Goal1.Animate();

        yield return new WaitForSeconds(0.2f);

        Goal2.gameObject.SetActive(true);
        Goal2.Animate();

        yield return new WaitForSeconds(0.2f);

        ExtraGoal1.gameObject.SetActive(true);
        ExtraGoal1.Animate();

        yield return new WaitForSeconds(0.3f);

        ExtraGoal2.gameObject.SetActive(true);
        ExtraGoal2.Animate();
    }

    public void SolveMinutes()
    {
        CurrentHour += Mathf.FloorToInt(CurrentMins) / 60;
        CurrentMins %= 60;
    }

    private void AddMins(float mins)
    {
        CurrentMins += mins;

        SolveMinutes();

        if(CurrentHour > DayEndHour)
            StartCoroutine(NextDay());
    }

    public void AddMinsIfNotAlreadyAddedViaRealTime(int mins)
    {
        if(IsIncreaseTimeWithRealTime)
            return;
        
        AddMins(mins);
    }

    public IEnumerator NextDay()
    {
        CurrentHour = DayStartHour;
        CurrentMins = 0;

        CurrentDay++;

        Game.SetActive(false);
        NextDayGO.SetActive(true);

        if(CurrentDay == 2)
        {
            Day1.SetActive(true);
        }
        else if(CurrentDay == 3)
        {
            Day1.GetComponent<Animator>().enabled = false;
            Day2.SetActive(true);
        }
        else if(CurrentDay == 4)
        {
            Day2.GetComponent<Animator>().enabled = false;
            Day3.SetActive(true);
        }
        else if (CurrentDay == 5)
        {
            Day3.GetComponent<Animator>().enabled = false;
            Day4.SetActive(true);
        }
        else if (CurrentDay == 6)
        {
            Day4.GetComponent<Animator>().enabled = false;
            Day5.SetActive(true);
        }
        else if (CurrentDay == 7)
        {
            Day5.GetComponent<Animator>().enabled = false;
            Day6.SetActive(true);
        }
        
        yield return new WaitForSeconds(3);
        Game.SetActive(true);
        NextDayGO.SetActive(false);

        Timer.Instance.StartCountdown(1.5f);

        if(CurrentDay == 2)
        {
            Goal3.gameObject.SetActive(true);

            Goal3.Animate();
        }
        else if(CurrentDay == 3)
        {
            Goal4.gameObject.SetActive(true);
            Goal5.gameObject.SetActive(true);

            Goal4.Animate();
        yield return new WaitForSeconds(0.3f);
            Goal5.Animate();
        }
    }

    private float deltaUntilSec;
    private void Update()
    {
        deltaUntilSec += Time.deltaTime;

        if(IsIncreaseTimeWithRealTime && deltaUntilSec >= 1)
        {
            AddMins(MinsPerSec);

            deltaUntilSec = 0;
        }

        SetText();
    }

    private void SetText()
    {
        Day.text = CurrentDay.ToString("0");
        Hours.text = CurrentHour.ToString("00");
        Mins.text = CurrentMins.ToString("00");
    }
}
