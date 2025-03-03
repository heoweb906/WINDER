using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Button_GameEnd : MenuButton
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

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
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
}
