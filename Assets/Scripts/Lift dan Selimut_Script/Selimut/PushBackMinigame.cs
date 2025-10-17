using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PushBackMinigame : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;
    public float moveSpeed = 0.5f;
    public float knockback = 0.3f;
    public float losePosition = 0.5f;
    public float winPosition = -2f;

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (targetObject == null) return;

        SpriteRenderer sr = targetObject.GetComponent<SpriteRenderer>();
        float halfWidth = sr != null ? sr.bounds.extents.x : 0f;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(winPosition, targetObject.position.y - 5f, 0),
        new Vector3(winPosition, targetObject.position.y + 5f, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(losePosition, targetObject.position.y - 5f, 0),
        new Vector3(losePosition, targetObject.position.y + 5f, 0));

    }
    #endif

    [Header("Visual & Audio")]
    public AudioSource audioSource;
    public AudioClip sfxClick;   ///klik tangannya
    public AudioClip sfxDrag;   ///kalau kalah
    public TimerScript_Selimut timer;

    //[Header("Transitions & Jumpsscare")]
    //public GameObject jumpscareObject;
    //public float jumpscareDuration = 1.5f;
    //public string nextSceneName;

    [Header("Ghost Animation")]
    public GhostSequence ghostSequence;

    private bool gameOver = false;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        targetObject.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        if (targetObject.position.x >= losePosition)
        {
            Kalah("Objek mencapai batas kanan!");
        }
    }

    public void OnObjectClicked()
    {
        if (gameOver) return;

        targetObject.Translate(Vector2.left * knockback);
        //if (audioSource != null && sfxClick != null)
        //    audioSource.PlayOneShot(sfxClick);

        if (targetObject.position.x < winPosition)
        {
            Menang();
        }
    }

    public void OnButtonPressed(Transform target)
    {
        if (gameOver) return;
        target.Translate(Vector2.left * knockback);
        //if (audioSource && sfxClick)
        //    audioSource.PlayOneShot(sfxClick);
    }

    public void OnTimeUp() //SYARAT KALAH PERTAMA--WAKTU HABIS
    {
        if (!gameOver)
        {
            //Debug.Log("WAktu habis, kalah");
            Kalah("waktu habis");
        }
    }

    void Menang()
    {
        if (gameOver) return;
        gameOver = true;

        if (timer != null) timer.StopTimer();


        Debug.Log("Menang");

        if (audioSource != null)
            audioSource.Stop();

        if (ghostSequence != null)
            ghostSequence.isActive = false;
        //if (audioSource != null && sfxClick != null)
        //    audioSource.PlayOneShot(sfxClick);

        //enih buat nganuin tahapan selanjutnya, pindah scene kah
        //kirim skor kah
        //SceneManager.LoadScene("NextMiniGameScene);
    }

    void Kalah(string reason) //SYARAT KALAH KEDUA- Hand-nya udah nyampe di target
    {
        if (gameOver) return;
        gameOver = true;


        if (timer != null) timer.StopTimer();

        Debug.Log("kalah: " + reason);

        if (audioSource != null)
            audioSource.Stop();

        if (ghostSequence != null)
            ghostSequence.isActive = false;
        //if (audioSource != null && sfxDrag != null)
        //    audioSource.PlayOneShot(sfxDrag);

        //if (jumpscareObject != null)
        //    StartCoroutine(ShowJumpscare());

        //GameBridge.OnMiniGameResult(false);
        //enih juga buat nganuin tindakan selanjutnya
    }

    //IEnumerator ShowJumpscare()
    //{
    //    if (jumpscareObject == null) yield break;

    //    jumpscareObject.SetActive(true);
    //    yield return new WaitForSeconds(jumpscareDuration);
    //    jumpscareObject.SetActive(false);
    //}

    public bool IsGameOver()
    {
        return gameOver;//ini buat timer biar mati
    }

}