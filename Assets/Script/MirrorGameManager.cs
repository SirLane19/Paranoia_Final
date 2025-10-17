using UnityEngine;
using UnityEngine.UI;

public class MirrorGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image reflectionFace;
    public Image ghostSilhouette;
    public Image timerBar;
    public GameObject gameOverPanel;
    public GameObject survivePanel;

    [Header("Gameplay Settings")]
    public float gameDuration = 10f;        // durasi bertahan
    public float reflectionFadeRate = 0.1f; // refleksi makin gelap kalau diam
    public float reflectionRecover = 0.2f;  // refleksi makin terang saat klik
    public float ghostFadeInRate = 0.2f;   // kecepatan ghost muncul (konstan)
    public float ghostFadeOutRate = 0.5f;   // kecepatan ghost memudar saat klik
    public float fadeOutTimerDuration = 0.8f; // efek klik bertahan sedikit

    private float timeLeft;
    private float faceAlpha = 1f;
    private float ghostAlpha = 0f;
    private float ghostFadeOutTimer = 0f;
    private bool gameOver = false;

    void Start()
    {
        timeLeft = gameDuration;

        SetAlpha(reflectionFace, faceAlpha);
        SetAlpha(ghostSilhouette, ghostAlpha);

        if (timerBar) timerBar.fillAmount = 1f;
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (survivePanel) survivePanel.SetActive(false);
    }

    void Update()
    {
        if (gameOver) return;

        // TIMER
        timeLeft -= Time.deltaTime;
        if (timerBar)
            timerBar.fillAmount = timeLeft / gameDuration;

        // kalau waktu habis, cek kondisi akhir
        if (timeLeft <= 0f)
        {
            if (ghostAlpha <= 0f)
                EndGame(true);
            else
                EndGame(false);
            return;
        }

        // =====================
        // Ghost muncul konstan
        // =====================
        ghostAlpha += ghostFadeInRate * Time.deltaTime;
        ghostAlpha = Mathf.Clamp01(ghostAlpha);

        // refleksi makin gelap perlahan
        faceAlpha -= reflectionFadeRate * Time.deltaTime;
        faceAlpha = Mathf.Clamp01(faceAlpha);

        // klik: ghost mundur sedikit, refleksi terang
        if (Input.GetMouseButtonDown(0))
        {
            // Setelah klik, ghost langsung mulai muncul sedikit lebih cepat
            ghostAlpha += 0.002f;
            ghostAlpha = Mathf.Clamp01(ghostAlpha);

            ghostFadeOutTimer = fadeOutTimerDuration;
            faceAlpha += reflectionRecover;
            faceAlpha = Mathf.Clamp01(faceAlpha);
        }

        // efek klik: ghost memudar selama durasi singkat
        if (ghostFadeOutTimer > 0)
        {
            ghostFadeOutTimer -= Time.deltaTime;
            ghostAlpha -= ghostFadeOutRate * Time.deltaTime;
            ghostAlpha = Mathf.Clamp01(ghostAlpha);
        }

        // update visual
        SetAlpha(reflectionFace, faceAlpha);
        SetAlpha(ghostSilhouette, ghostAlpha);

        // kalah otomatis kalau ghost penuh
        if (ghostAlpha >= 1f)
            EndGame(false);
    }

    void SetAlpha(Image img, float a)
    {
        if (!img) return;
        Color c = img.color;
        c.a = a;
        img.color = c;
    }

    void EndGame(bool survived)
    {
        if (gameOver) return;
        gameOver = true;

        if (survived)
        {
            if (survivePanel)
                survivePanel.SetActive(true);
            Debug.Log("🎉 YOU SURVIVED! The reflection stayed pure!");
        }
        else
        {
            if (gameOverPanel)
                gameOverPanel.SetActive(true);
            Debug.Log("💀 The ghost consumed the reflection...");
        }
    }
}
