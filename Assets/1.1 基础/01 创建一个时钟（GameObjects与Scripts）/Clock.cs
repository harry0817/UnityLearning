using System;
using UnityEngine;

public class Clock : MonoBehaviour {

    public Transform hoursTransform;
    public Transform minutesTransform;
    public Transform secondsTransform;

    const float degreesPerHour = 30f;
    const float degreesPerMinute = 6f;
    const float degreesPerSecond = 6f;

    public bool continuous;

    private void Update() {
        if (continuous) {
            UpdateContinuous();
        } else {
            UpdateDiscrete();
        }
    }

    void UpdateContinuous() {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalHours * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalMinutes * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalSeconds * degreesPerSecond, 0f);
        Debug.Log(time.TotalSeconds);
    }

    void UpdateDiscrete() {
        DateTime time = DateTime.Now;
        hoursTransform.localRotation = Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);
    }

}
