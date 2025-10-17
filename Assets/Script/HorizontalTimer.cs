using UnityEngine;
using UnityEngine.UI;

public class HorizontalTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float maxTime = 10f;      // durasi penuh timer
    private float timeLeft;

    [Header("UI Elements")]
    public Image timerBar;           // drag Image bar ke sini (harus type Filled)

    private bool isStopped = false;

    // === Properti publik agar bisa dibaca script lain ===
    public bool IsTimeOver => timeLeft <= 0f;
    public bool IsRunning => !isStopped;
    public float TimeRatio => Mathf.Clamp01(timeLeft / maxTime);

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (isStopped) return;

        timeLeft -= Time.deltaTime;

        if (timerBar)
            timerBar.fillAmount = TimeRatio;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            StopTimer();
        }
    }

    // === Fungsi kontrol ===
    public void ResetTimer()
    {
        timeLeft = maxTime;
        isStopped = false;

        if (timerBar)
        {
            timerBar.type = Image.Type.Filled;
            timerBar.fillMethod = Image.FillMethod.Horizontal;
            timerBar.fillOrigin = (int)Image.OriginHorizontal.Left;
            timerBar.fillAmount = 1f;
        }
    }

    public void StopTimer()
    {
        isStopped = true;
    }

    public void ResumeTimer()
    {
        isStopped = false;
    }
}
