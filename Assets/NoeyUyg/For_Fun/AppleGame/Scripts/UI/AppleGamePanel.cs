using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGamePanel : MonoBehaviour
{
    public void OnShow()
    {
        this.gameObject.SetActive(true);
        ShowAfterAction();
    }

    public virtual void ShowAfterAction()
    {

    }

    public void OnHide()
    {
        HideBeforeAction();
        this.gameObject.SetActive(false);
    }

    public virtual void HideBeforeAction()
    {

    }
}
