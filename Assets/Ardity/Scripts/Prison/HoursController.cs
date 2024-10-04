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
            NextDay();
    }

    public void AddMinsIfNotAlreadyAddedViaRealTime(int mins)
    {
        if(IsIncreaseTimeWithRealTime)
            return;
        
        AddMins(mins);
    }

    public void NextDay()
    {
        CurrentHour = DayStartHour;
        CurrentMins = 0;

        CurrentDay++;
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
