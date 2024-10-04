using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    private float currentTime;
    private bool isCounting = false;

    private void Awake()
    {
        Instance = this;
    }

    public void StartCountdown(float time)
    {
        currentTime = time;
        isCounting = true;
    }

    private void Update()
    {
        if (isCounting)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                isCounting = false;
                currentTime = 0f;
                StartCoroutine(GameManager.Instance.CallDecision(2));
            }
        }
    }
}
