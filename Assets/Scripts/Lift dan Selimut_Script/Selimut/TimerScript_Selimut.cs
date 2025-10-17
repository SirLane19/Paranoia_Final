using UnityEngine;
using UnityEngine.UI;

public class TimerScript_Selimut : MonoBehaviour
{

    private Image timerBar;
    public float maxTime = 5f;
    float timeLeft;
    public PushBackMinigame gameController;

    
    // Start is called before the first frame update
    void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }

        else
        {
            if (gameController != null && !gameController.IsGameOver())
            {
                gameController.OnTimeUp();
            }
            else
            {
                Debug.Log("Times up");
            }
            enabled = false;
        }
    }

        public void StopTimer()
    {
        enabled = false;
        Debug.Log("Timer stopped manually");
    }
}
