using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MirrorGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image reflectionFace;
    public Image ghostSilhouette;
    public Image timerBar;
    public GameObject gameOverPanel;
    public GameObject survivePanel;

    [Header("Gameplay Settings")]
    [Tooltip("Durasi waktu bertahan dalam detik")]
    public float gameDuration = 10f;
    [Tooltip("Refleksi makin gelap kalau diam")]
    public float reflectionFadeRate = 0.1f;
    [Tooltip("Refleksi makin terang saat klik")]
    public float reflectionRecover = 0.2f;
    [Tooltip("Kecepatan ghost muncul (konstan)")]
    public float ghostFadeInRate = 0.2f;
    [Tooltip("Kecepatan ghost memudar saat klik")]
    public float ghostFadeOutRate = 0.5f;
    [Tooltip("Durasi efek klik bertahan (detik)")]
    public float fadeOutTimerDuration = 0.8f;

    [Header("Scene Transition Settings")]
    [Tooltip("Jeda sebelum pindah ke scene berikutnya")]
    public float nextSceneDelay = 2.5f;
    [Tooltip("Nama scene tujuan setelah Mirror selesai")]
    public string nextSceneName = "Selimut";

    // internal variables
    private float timeLeft;
    private float faceAlpha = 1f;
    private float ghostAlpha = 0f;
    private float ghostFadeOutTimer = 0f;
    private bool gameOver = false;

    void Start()
    {
        // inisialisasi timer dan alpha
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

        // ====== TIMER ======
        timeLeft -= Time.deltaTime;
        if (timerBar)
            timerBar.fillAmount = timeLeft / gameDuration;

        // jika waktu habis, tentukan hasil akhir
        if (timeLeft <= 0f)
        {
            EndGame(ghostAlpha <= 0f);
            return;
        }

        // ====== LOGIKA GAMEPLAY ======
        ghostAlpha += ghostFadeInRate * Time.deltaTime;
        ghostAlpha = Mathf.Clamp01(ghostAlpha);

        faceAlpha -= reflectionFadeRate * Time.deltaTime;
        faceAlpha = Mathf.Clamp01(faceAlpha);

        // klik kiri → buat refleksi terang & ghost mundur
        if (Input.GetMouseButtonDown(0))
        {
            ghostAlpha += 0.002f;
            ghostAlpha = Mathf.Clamp01(ghostAlpha);

            ghostFadeOutTimer = fadeOutTimerDuration;

            faceAlpha += reflectionRecover;
            faceAlpha = Mathf.Clamp01(faceAlpha);
        }

        // efek ghost fade out sementara
        if (ghostFadeOutTimer > 0)
        {
            ghostFadeOutTimer -= Time.deltaTime;
            ghostAlpha -= ghostFadeOutRate * Time.deltaTime;
            ghostAlpha = Mathf.Clamp01(ghostAlpha);
        }

        // update visual
        SetAlpha(reflectionFace, faceAlpha);
        SetAlpha(ghostSilhouette, ghostAlpha);

        // kalau ghost sudah penuh → kalah otomatis
        if (ghostAlpha >= 1f)
            EndGame(false);
    }

    // ========== UTILITIES ==========
    void SetAlpha(Image img, float a)
    {
        if (!img) return;
        Color c = img.color;
        c.a = a;
        img.color = c;
    }

    // ========== GAME END LOGIC ==========
    void EndGame(bool survived)
    {
        if (gameOver) return;
        gameOver = true;

        if (survived)
        {
            if (survivePanel) survivePanel.SetActive(true);
            Debug.Log("🎉 YOU SURVIVED! The reflection stayed pure!");
        }
        else
        {
            if (gameOverPanel) gameOverPanel.SetActive(true);
            Debug.Log("💀 The ghost consumed the reflection...");
        }

        // jalankan transisi ke scene berikutnya
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(nextSceneDelay);

        // Pastikan scene tujuan sudah ada di Build Settings
        SceneManager.LoadScene(nextSceneName);
    }
}
