using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;


public class Button_ResolutionNum : MenuButton
{
    public int iResolutionNum;
    public Button_ResolutionNum[] resolutionNumButtons;
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
        base.ImplementButton();

        SaveData_Manager.Instance.SetResolution(iResolutionNum);

        foreach (var item in resolutionNumButtons)
        {
            if (item.bButtonSelceted)
            {
                item.bButtonSelceted = false;
                item.SelectButtonOff();
            }
        }

        if (mainMenuController.bIsUIDoing) return;
        mainMenuController.bIsUIDoing = true;

        mainMenuController.PanelOff(1);
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



    private void OnEnable()
    {
        if (SaveData_Manager.Instance.GetResolutionIndex() == iResolutionNum)
        {
            bButtonSelceted = true;
            textButton.color = new Color(1f, 1f, 0f, 1f);

            foreach (var item in resolutionNumButtons)
            {
                if(item.bButtonSelceted)
                {
                    item.bButtonSelceted = false;
                    item.SelectButtonOff();
                }
            }

    
        }
    }



}
