using System.Collections;
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

    private void Awake()
    {
        Instance = this;

        SolveMinutes();
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

        if(CurrentDay == 1)
            Day1.SetActive(true);
        else if(CurrentDay == 2)
            Day2.SetActive(true);
        else if(CurrentDay == 3)
            Day3.SetActive(true);
        else if (CurrentDay == 4)
            Day4.SetActive(true);
        
        yield return new WaitForSeconds(4);
        Game.SetActive(true);
        NextDayGO.SetActive(false);
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
        Day.text = CurrentDay.ToString("00");
        Hours.text = CurrentHour.ToString("00");
        Mins.text = CurrentMins.ToString("00");
    }
}
