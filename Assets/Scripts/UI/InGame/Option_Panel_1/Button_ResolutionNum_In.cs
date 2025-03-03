using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Button_ResolutionNum_In : InGameButton
{
    public int iResolutionNum;
    public Button_ResolutionNum_In[] resolutionNumButtons;
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

        if (ingameUIController.bIsUIDoing) return;
        ingameUIController.bIsUIDoing = true;

        ingameUIController.PanelOff(1);
    }


    public override void SelectButtonOn()
    {
        base.SelectButtonOn();

        if (textButton != null)
        {
            DOTween.Kill(gameObject, true);
            textButton.DOFontSize(24f, fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true); ;
            textButton.DOColor(new Color(0.58f, 1f, 1f, 1f), fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true); ;
        }
    }

    public override void SelectButtonOff()
    {
        base.SelectButtonOff();

        if (textButton != null && !bButtonSelceted)
        {
            textButton.DOFontSize(20f, fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true); ;
            textButton.DOColor(new Color(1f, 1f, 1f, 1f), fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true); ;
        }
        else
        {
            textButton.DOFontSize(20f, fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true); ;
            textButton.DOColor(new Color(1f, 1f, 0f, 1f), fButtonAnimtionDelay).SetEase(Ease.OutCirc).SetUpdate(true); ;
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
                if (item.bButtonSelceted)
                {
                    item.bButtonSelceted = false;
                    item.SelectButtonOff();
                }
            }


        }
    }
}
