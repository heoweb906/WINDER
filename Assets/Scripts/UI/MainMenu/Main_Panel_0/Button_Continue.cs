using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Button_Continue : MenuButton
{
    private void Start()
    {
        if (string.IsNullOrEmpty(SaveData_Manager.Instance.GetStringSceneName()))
        {
            Debug.Log("이어하기 버튼을 지웁니다");
            RemoveFromMenuButtons();
            Destroy(gameObject);
        }     
    }

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


        if (!string.IsNullOrEmpty(SaveData_Manager.Instance.GetStringSceneName()))
        {
            mainMenuController.StartNewGame(SaveData_Manager.Instance.GetStringSceneName());
        }
           
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


    private void RemoveFromMenuButtons()
    {
        // MenuButton 배열이 있는지 확인
        if (mainMenuController.menuButtons != null)
        {
            List<MenuButton> buttonList = new List<MenuButton>(mainMenuController.menuButtons);
            buttonList.Remove(this);
            mainMenuController.menuButtons = buttonList.ToArray();
            mainMenuController.lastButton = mainMenuController.menuButtons[0];


        }
    }
}
