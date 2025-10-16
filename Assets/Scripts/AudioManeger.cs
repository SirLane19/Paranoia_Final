using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource music;

    [Header("Clips")]
    public AudioClip bgmCommon;        
    public AudioClip bgmCutsceneIntro; 

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);                                  
        music = GetComponent<AudioSource>();
        music.loop = true;
    }

    void OnEnable()  { SceneManager.activeSceneChanged += OnSceneChanged; } 
    void OnDisable() { SceneManager.activeSceneChanged -= OnSceneChanged; }

    void Start()
    {
        if (bgmCommon != null) PlayIfDifferent(bgmCommon);               
    }

    void OnSceneChanged(Scene oldSc, Scene newSc)
    {
        var name = newSc.name;
        if (name == "Cutscene_Intro")
        {
            PlayIfDifferent(bgmCutsceneIntro);                      
        }
        else
        {
            if (bgmCommon != null) PlayIfDifferent(bgmCommon);          
        }
    }

    void PlayIfDifferent(AudioClip clip)
    {
        if (music.clip == clip && music.isPlaying) return;              
        StartCoroutine(FadeSwap(clip, 0.5f));                           
    }

    System.Collections.IEnumerator FadeSwap(AudioClip next, float dur)
    {
        float t = 0f; float start = music.volume;
        while (t < dur) { t += Time.unscaledDeltaTime; music.volume = Mathf.Lerp(start, 0f, t/dur); yield return null; }
        music.clip = next; music.Play();
        t = 0f;
        while (t < dur) { t += Time.unscaledDeltaTime; music.volume = Mathf.Lerp(0f, start, t/dur); yield return null; }
    }
}
