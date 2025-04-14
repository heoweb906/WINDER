using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentObj : MonoBehaviour
{
    private CarriedObject carriedOj;
    private Rigidbody rigid;

    private void Awake()
    {
        carriedOj = GetComponent<CarriedObject>();
        rigid = GetComponent<Rigidbody>();
    }

  
    public void ChangeToColorObj_()
    {
        Invoke("ChangeToColorObj", 2f);
    }
    public void ChangeToColorObj()
    {
        rigid.isKinematic = false; // 물리 적용 활성화

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(Vector3.one * 3.8f, 0.1f).SetEase(Ease.OutBack)) // 살짝 커졌다가
           .Append(transform.DOScale(Vector3.one * 0.2f, 0.15f).SetEase(Ease.InOutQuad)) // 확 줄어듦
           .OnComplete(() =>
           {
               gameObject.tag = "Untagged"; // 태그 변경
               gameObject.layer = LayerMask.NameToLayer("Interactable"); // 레이어 변경

               // 위쪽 + 약간 랜덤한 방향으로 튀어오름
               Vector3 forceDir = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
               rigid.AddForce(forceDir * 10f, ForceMode.Impulse);

               // 랜덤한 축으로 회전하는 힘 추가
               Vector3 torque = new Vector3(Random.Range(0f, 0.2f), 0, Random.Range(0f, 0.2f)) * 10f;
               rigid.AddTorque(torque, ForceMode.Impulse);

               carriedOj.enabled = true;
           });
    }

}
