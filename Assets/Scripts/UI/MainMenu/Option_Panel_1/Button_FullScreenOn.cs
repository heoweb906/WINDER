using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Button_FullScreenOn : MenuButton
{
    public Button_FullScreenOff otherButton;
    public bool bButtonSelceted;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    public override void ImplementButton()
    {
        if (!SaveData_Manager.Instance.GetBoolFullScreen())
        {
            if (otherButton.bButtonSelceted)
            {
                otherButton.bButtonSelceted = false;
                otherButton.SelectButtonOff();
            }

            SaveData_Manager.Instance.SetFullScreen(true);
            ButtonSelceted();
        }

        base.ImplementButton();

    }

    public override void SelectButtonOn()
    {
        base.SelectButtonOn();

        if (textButton != null)
        {
            textButton.DOFontSize(24f, fButtonAnimationDelay).SetEase(Ease.OutCirc);
            textButton.DOColor(new Color(0.58f, 1f, 1f, 1f), fButtonAnimationDelay).SetEase(Ease.OutCirc);
        }
    }

    public override void SelectButtonOff()
    {
        base.SelectButtonOff();

        if (textButton != null && !bButtonSelceted)
        {
            textButton.DOFontSize(20f, fButtonAnimationDelay).SetEase(Ease.OutCirc);
            textButton.DOColor(new Color(1f, 1f, 1f, 1f), fButtonAnimationDelay).SetEase(Ease.OutCirc);
        }
        else
        {
            textButton.DOFontSize(20f, fButtonAnimationDelay).SetEase(Ease.OutCirc);
            textButton.DOColor(new Color(1f, 1f, 0f, 1f), fButtonAnimationDelay).SetEase(Ease.OutCirc);
        }
    
    }




    public void ButtonSelceted()
    {
        if(!bButtonSelceted)
        {
            bButtonSelceted = true;
            textButton.DOFontSize(20f, fButtonAnimationDelay).SetEase(Ease.OutCirc);
            textButton.DOColor(new Color(1f, 1f, 0f, 1f), fButtonAnimationDelay).SetEase(Ease.OutCirc);
        }
    }   

    private void OnEnable()
    {
       
        if (SaveData_Manager.Instance.GetBoolFullScreen())
        {
            bButtonSelceted = true;
            textButton.color = new Color(1f, 1f, 0f, 1f);

            if (otherButton.bButtonSelceted)
            {
                otherButton.bButtonSelceted = false;
                otherButton.SelectButtonOff();
            }
        }
    }


}
