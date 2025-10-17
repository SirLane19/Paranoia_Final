using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // ✅ Tambahin ini biar bisa ganti scene
using System.Collections;

public class NeedleRotator : MonoBehaviour
{
    [Header("References")]
    public RectTransform needle;
    public Image successZone;
    public Image ghostSilhouette;
    public Image eyeBlinkImage;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Timer Reference")]
    public HorizontalTimer timer;   // drag script HorizontalTimer di inspector

    [Header("Gameplay Settings")]
    public int targetSuccess = 5;
    public int maxMisses = 3;
    public float rotationSpeed = 120f;
    public float zoneSize = 0.1f;

    [Header("Ghost & Eye Settings")]
    public float ghostFadeStep = 0.3f;
    public float ghostFadeOutSpeed = 1.5f;
    public float eyeCloseSpeed = 0.25f;
    public float eyeOpenSpeed = 1.2f;

    private float currentAngle = 0f;
    private float zoneStartAngle = 0f;
    private int successCount = 0;
    private int missCount = 0;
    private bool isActive = true;
    private float eyeAlpha = 1f;

    void Start()
    {
        SetRandomZone();

        if (ghostSilhouette)
        {
            Color g = ghostSilhouette.color;
            g.a = 0f;
            ghostSilhouette.color = g;
        }

        if (eyeBlinkImage)
        {
            Color e = eyeBlinkImage.color;
            e.a = 1f;
            eyeBlinkImage.color = e;
        }

        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    void Update()
    {
        if (!isActive) return;

        if (timer && timer.IsTimeOver)
        {
            timer.StopTimer();
            TriggerGameOver("⏰ Waktu habis!");
            return;
        }

        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;
        needle.rotation = Quaternion.Euler(0, 0, -currentAngle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }

        if (eyeBlinkImage && isActive)
        {
            eyeAlpha -= eyeCloseSpeed * Time.deltaTime;
            eyeAlpha = Mathf.Clamp01(eyeAlpha);
            SetAlpha(eyeBlinkImage, eyeAlpha);
        }
    }

    void CheckHit()
    {
        float zoneEndAngle = zoneStartAngle + (successZone.fillAmount * 360f);
        float needleAngle = currentAngle % 360f;

        bool inZone = false;
        if (zoneEndAngle < 360f)
            inZone = (needleAngle >= zoneStartAngle && needleAngle <= zoneEndAngle);
        else
            inZone = (needleAngle >= zoneStartAngle || needleAngle <= zoneEndAngle - 360f);

        if (inZone)
        {
            successCount++;
            Debug.Log($"✅ Perfect! {successCount}/{targetSuccess}");

            StopCoroutine("FadeEye");
            StartCoroutine(FadeEye(1f, eyeOpenSpeed));

            rotationSpeed += 25f;

            if (successCount >= targetSuccess)
            {
                WinGame();
                return;
            }

            StartCoroutine(NextRound());
        }
        else
        {
            missCount++;
            Debug.Log($"❌ Miss {missCount}/{maxMisses}");

            if (ghostSilhouette)
            {
                Color g = ghostSilhouette.color;
                g.a = Mathf.Clamp01(g.a + ghostFadeStep);
                ghostSilhouette.color = g;
            }

            if (missCount >= maxMisses)
            {
                TriggerGameOver("💀 Terlalu banyak miss!");
                return;
            }

            StartCoroutine(NextRound());
        }
    }

    IEnumerator NextRound()
    {
        isActive = false;
        yield return new WaitForSeconds(0.4f);
        SetRandomZone();
        isActive = true;
    }

    IEnumerator FadeEye(float targetAlpha, float speed)
    {
        if (!eyeBlinkImage) yield break;
        Color c = eyeBlinkImage.color;
        while (Mathf.Abs(c.a - targetAlpha) > 0.01f)
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * speed);
            eyeBlinkImage.color = c;
            yield return null;
        }
        c.a = targetAlpha;
        eyeBlinkImage.color = c;
    }

    void SetRandomZone()
    {
        zoneStartAngle = Random.Range(0f, 360f);
        successZone.fillAmount = zoneSize;
        successZone.fillMethod = Image.FillMethod.Radial360;
        successZone.fillClockwise = true;
        successZone.rectTransform.localRotation = Quaternion.Euler(0, 0, -zoneStartAngle);
    }

    void SetAlpha(Image img, float a)
    {
        if (!img) return;
        Color c = img.color;
        c.a = a;
        img.color = c;
    }

    void TriggerGameOver(string reason)
    {
        if (!isActive) return;
        isActive = false;
        Debug.Log(reason);

        if (timer)
            timer.StopTimer();

        if (ghostSilhouette)
        {
            Color g = ghostSilhouette.color;
            g.a = 1f;
            ghostSilhouette.color = g;
        }

        if (eyeBlinkImage)
            StopAllCoroutines();

        if (losePanel) losePanel.SetActive(true);
        if (winPanel) winPanel.SetActive(false);

        // ✅ Tambahan: otomatis lanjut ke scene "Mirror" setelah kalah
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    void WinGame()
    {
        if (!isActive) return;
        isActive = false;
        Debug.Log("🎉 YOU SURVIVED!");

        if (timer)
            timer.StopTimer();

        if (ghostSilhouette)
            StartCoroutine(FadeOutGhost());

        if (eyeBlinkImage)
            StopAllCoroutines();

        if (winPanel) winPanel.SetActive(true);
        if (losePanel) losePanel.SetActive(false);

        // ✅ Tambahan: otomatis lanjut ke scene "Mirror" setelah menang
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator FadeOutGhost()
    {
        Color g = ghostSilhouette.color;
        while (g.a > 0f)
        {
            g.a = Mathf.MoveTowards(g.a, 0f, Time.deltaTime * ghostFadeOutSpeed);
            ghostSilhouette.color = g;
            yield return null;
        }
    }

    // ✅ Tambahan fungsi baru buat ganti scene
    IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(2f); // delay 2 detik sebelum pindah
        SceneManager.LoadScene("Mirror");    // ganti ke nama scene tujuan kamu persis
    }
}
