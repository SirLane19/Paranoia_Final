using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    public Image cutsceneImage;
    public Sprite[] cutsceneSprites;
    public float imageDuration = 30f;
    public string nextSceneName = "Level1";

    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        if (cutsceneSprites.Length > 0)
        {
            cutsceneImage.sprite = cutsceneSprites[0];
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= imageDuration)
        {
            timer = 0f;
            currentIndex++;

            if (currentIndex < cutsceneSprites.Length)
            {
                cutsceneImage.sprite = cutsceneSprites[currentIndex];
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
