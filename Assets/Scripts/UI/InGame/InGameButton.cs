using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected InGameUIController ingameUIController;
    public TMP_Text textButton;

    protected float fButtonAnimtionDelay = 0.35f;

    public bool bSelect = false;
    public bool bCanSelectIcon;

    private void Awake()
    {
        ingameUIController = FindObjectOfType<InGameUIController>();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (ingameUIController.nowPlayerButton != null)
        {
            ingameUIController.nowPlayerButton.SelectButtonOff();
        }

        if (ingameUIController.ingameButtons != null)
        {
            foreach (var item in ingameUIController.ingameButtons)
            {
                item.SelectButtonOff();
            }
        }

        SelectButtonOn();
    }


    public virtual void OnPointerExit(PointerEventData eventData)
    {
        SelectButtonOff();
    }

    // #. MenuButton을 상속 받은 버튼들의 실행 기능을 여기에 다 구현하는 거임
    public virtual void ImplementButton()
    {
        SoundAssistManager.Instance.GetSFXAudioBlock("POP Brust 08", ingameUIController.gameObject.transform);


        SelectButtonOff();
    }

    public virtual void SelectButtonOn()
    {
        DOTween.Kill(gameObject, true); 

        ingameUIController.nowPlayerButton = this;
    }

    // #. 버튼이 비활성화 되었을 때 취할 액션의 내용을 담을 함수
    // 각 버튼 별로 다른 효과를 줄 수 있으므로 내용은 자식에서 작성
    public virtual void SelectButtonOff()
    {
        ingameUIController.nowPlayerButton = null;
        ingameUIController.lastButton = this;
    }


}
