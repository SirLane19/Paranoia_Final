using UnityEngine;
using UnityEngine.UI;

public class FrameAnimator : MonoBehaviour
{
    [Header("Frame Animation Settings")]
    public Sprite[] frames;          // kumpulan gambar frame by frame
    public float frameRate = 0.1f;   // waktu antar frame (semakin kecil = makin cepat)
    public bool loop = true;         // apakah animasi muter terus
    public bool playOnStart = true;  // auto play saat mulai
    public bool randomDelay = false; // jeda acak sebelum ulang

    private Image imageRenderer;     // buat UI Image
    private SpriteRenderer spriteRenderer; // buat SpriteRenderer 2D biasa
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = false;

    void Start()
    {
        imageRenderer = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playOnStart) Play();
    }

    void Update()
    {
        if (!isPlaying || frames.Length == 0) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop)
                {
                    currentFrame = 0;
                    if (randomDelay)
                        timer = Random.Range(0.5f, 2f); // jeda acak antar kedipan
                }
                else
                {
                    Stop();
                    return;
                }
            }

            SetFrame(frames[currentFrame]);
            timer = frameRate;
        }
    }

    void SetFrame(Sprite s)
    {
        if (imageRenderer)
            imageRenderer.sprite = s;
        else if (spriteRenderer)
            spriteRenderer.sprite = s;
    }

    public void Play()
    {
        isPlaying = true;
        currentFrame = 0;
        timer = frameRate;
        SetFrame(frames[currentFrame]);
    }

    public void Stop()
    {
        isPlaying = false;
    }
}
