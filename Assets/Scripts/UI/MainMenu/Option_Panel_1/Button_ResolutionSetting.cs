using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class Button_ResolutionSetting : MenuButton
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

        if (mainMenuController.bIsUIDoing) return;
        mainMenuController.bIsUIDoing = true;

        mainMenuController.PanelOff(8);
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
        if (textButton != null)
        {
            textButton.DOFontSize(20f, fButtonAnimationDelay).SetEase(Ease.OutCirc);
            textButton.DOColor(new Color(1f, 1f, 1f, 1f), fButtonAnimationDelay).SetEase(Ease.OutCirc);
        }
    }


    private void OnEnable()
    {
        int ddd = SaveData_Manager.Instance.GetResolutionIndex();

        switch (ddd)
        {
            case 0:
                textButton.text = "720 x 480";
                break;
            case 1:
                textButton.text = "1280 x 720";
                break;
            case 2:
                textButton.text = "1920 x 1080";
                break;
            case 3:
                textButton.text = "2560 x 1440";
                break;

            default: break;
 
        }


    }
}
