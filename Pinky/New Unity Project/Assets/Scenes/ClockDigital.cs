using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockDigital : MonoBehaviour
{

    private Text textClock;
    // Start is called before the first frame update
    void Start()
    {
        textClock = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        DateTime time = DateTime.Now;
        var hour = LeadingZero(time.Hour);
        var minute = LeadingZero(time.Minute);
        var second = LeadingZero(time.Second);

        textClock.text = $"{hour}:{minute}:{second}";
    }

    string LeadingZero(int n) =>
        n.ToString().PadLeft(2, '0');
}

public static class NumberExtensions
{
    public static string ToLeadingZeroString(this int number) =>
        number.ToString().PadLeft(2, '0');
}