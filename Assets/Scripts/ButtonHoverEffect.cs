using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Scale Settings")]
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    public float speed = 8f;

    private bool isHovered = false;

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            isHovered ? hoverScale : normalScale,
            Time.deltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}