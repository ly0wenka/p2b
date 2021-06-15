using UnityEngine;
using UnityEngine.UI;

public class DigitalCountdown : MonoBehaviour
{
    private Text textClock;

    private float countdownTimerDuration;
    private float countdownTimerStartTime;

    void Start()
    {
        textClock = GetComponent<Text>();
        CountdownTimerReset(30.0f);
    }

    void Update()
    {
        var timerMessage = "Countdown has finished";
        var timeLeft = (int)CountdownTimerSecondsRemaining();
        if (timeLeft > 0)
            timerMessage = $"Countdown seconds remaining = {timeLeft}";

        textClock.text = timerMessage;
    }

    private void CountdownTimerReset(float delayInSeconds)
    {
        countdownTimerDuration = delayInSeconds;
        countdownTimerStartTime = Time.time;
    }

    private float CountdownTimerSecondsRemaining()
    {
        var elapsedSeconds = Time.time - countdownTimerStartTime;
        var timeLeft = countdownTimerDuration - elapsedSeconds;
        return timeLeft;
    }
}
