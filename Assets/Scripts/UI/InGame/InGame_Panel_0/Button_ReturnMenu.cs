using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_ReturnMenu : InGameButton
{
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
        base.ImplementButton();

        SaveData_Manager.Instance.SaveSettings();
        ingameUIController.bNowSceneChange = true;
        ingameUIController.ChangeScene("MainMenu");

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

        if (textButton != null)
        {
            textButton.DOFontSize(20f, fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
            textButton.DOColor(new Color(1f, 1f, 1f, 1f), fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true);
        }
    }
}
