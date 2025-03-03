using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Button_FullScreenOn_In : InGameButton
{
    public Button_FullScreenOff_in otherButton;
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
            textButton.DOFontSize(24f, fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
            textButton.DOColor(new Color(0.58f, 1f, 1f, 1f), fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
        }
    }

    public override void SelectButtonOff()
    {
        base.SelectButtonOff();

        if (textButton != null && !bButtonSelceted)
        {
            textButton.DOFontSize(20f, fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
            textButton.DOColor(new Color(1f, 1f, 1f, 1f), fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
        }
        else
        {
            textButton.DOFontSize(20f, fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
            textButton.DOColor(new Color(1f, 1f, 0f, 1f), fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
        }

    }




    public void ButtonSelceted()
    {
        if (!bButtonSelceted)
        {
            bButtonSelceted = true;
            textButton.DOFontSize(20f, fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
            textButton.DOColor(new Color(1f, 1f, 0f, 1f), fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
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
