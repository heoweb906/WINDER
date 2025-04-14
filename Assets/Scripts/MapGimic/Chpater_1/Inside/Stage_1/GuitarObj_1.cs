using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GuitarObj_1 : MonoBehaviour
{
    private CarriedObject carriedOj;
    private Rigidbody rigid;

    private void Awake()
    {
        carriedOj = GetComponent<CarriedObject>();
        rigid = GetComponent<Rigidbody>();
    }

    public void ChangeToFlyingState()
    {
        rigid.isKinematic = false;

        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        // ���� ���� ���� ���
        Vector3 upwardForce = Vector3.up * 50f;      // ���� ��
        Vector3 backwardForce = Vector3.back * 25f;  // ���� ��

        Vector3 finalForce = upwardForce + backwardForce;
        rigid.AddForce(finalForce, ForceMode.Impulse);

        Vector3 torque = new Vector3(Random.Range(0f, 0.2f), Random.Range(-0.1f, 0.1f), Random.Range(0f, 0.2f)) * 10f;
        rigid.AddTorque(torque, ForceMode.Impulse);

        carriedOj.enabled = true;


        Sequence scaleSeq = DOTween.Sequence();
        scaleSeq.PrependInterval(0.3f) // 0.3�� ������
                .Append(transform.DOScale(Vector3.one * 2.2f, 0.1f).SetEase(Ease.OutBack))
                .Append(transform.DOScale(Vector3.one * 0.45f, 0.15f).SetEase(Ease.InOutQuad));
    }
}
