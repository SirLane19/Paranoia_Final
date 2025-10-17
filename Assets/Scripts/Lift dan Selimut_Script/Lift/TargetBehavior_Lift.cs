using UnityEngine;

public class TargetBehavior_Lift : MonoBehaviour
{
    // Start is called before the first frame update

    public bool triggerOnDifferentObject = false;
    public Transform targetToPush;

    public PushBackMinigame_Lift controller;


    void OnMouseDown()
    {
        if (!triggerOnDifferentObject)
            controller.OnObjectClicked();
        else
            controller.OnButtonPressed(targetToPush);
    }
    

}
