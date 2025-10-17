using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    public bool triggerOnDifferentObject = false;
    public Transform targetToPush;

    public PushBackMinigame controller;


    void OnMouseDown()
    {
        if (!triggerOnDifferentObject)
            controller.OnObjectClicked();
        else
            controller.OnButtonPressed(targetToPush);
    }
    

}
