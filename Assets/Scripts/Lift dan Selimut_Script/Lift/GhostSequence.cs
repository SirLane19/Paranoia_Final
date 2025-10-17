using UnityEngine;

public class GhostSequence : MonoBehaviour
{
    public SpriteRenderer ghostRenderer;
    public Sprite[] ghostSprites;
    public Vector3[] ghostPositions;
    public Vector3[] ghostScales; // tambahkan ini
    public float totalDuration = 10f;

    private float elapsedTime = 0f;
    private int currentIndex = 0;

    [HideInInspector]
    public bool isActive = true;


    void Start()
    {
        if (ghostSprites.Length > 0)
        {
            ghostRenderer.sprite = ghostSprites[0];
            if (ghostPositions.Length > 0)
                ghostRenderer.transform.localPosition = ghostPositions[0];
            if (ghostScales.Length > 0)
                ghostRenderer.transform.localScale = ghostScales[0];
        }
    }

    void Update()
    {
        if (!isActive) return;
        if (currentIndex >= ghostSprites.Length) return;

        elapsedTime += Time.deltaTime;
        float segment = totalDuration / ghostSprites.Length;

        if (elapsedTime >= (currentIndex + 1) * segment)
        {
            currentIndex++;
            if (currentIndex < ghostSprites.Length)
            {
                ghostRenderer.sprite = ghostSprites[currentIndex];

                if (ghostPositions.Length > currentIndex)
                    ghostRenderer.transform.localPosition = ghostPositions[currentIndex];
                if (ghostScales.Length > currentIndex)
                    ghostRenderer.transform.localScale = ghostScales[currentIndex];
            }
        }
    }

}
