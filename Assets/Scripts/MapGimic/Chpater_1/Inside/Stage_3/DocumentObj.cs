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
        rigid.isKinematic = false; // ���� ���� Ȱ��ȭ

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(Vector3.one * 3.8f, 0.1f).SetEase(Ease.OutBack)) // ��¦ Ŀ���ٰ�
           .Append(transform.DOScale(Vector3.one * 0.2f, 0.15f).SetEase(Ease.InOutQuad)) // Ȯ �پ��
           .OnComplete(() =>
           {
               gameObject.tag = "Untagged"; // �±� ����
               gameObject.layer = LayerMask.NameToLayer("Interactable"); // ���̾� ����

               // ���� + �ణ ������ �������� Ƣ�����
               Vector3 forceDir = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
               rigid.AddForce(forceDir * 10f, ForceMode.Impulse);

               // ������ ������ ȸ���ϴ� �� �߰�
               Vector3 torque = new Vector3(Random.Range(0f, 0.2f), 0, Random.Range(0f, 0.2f)) * 10f;
               rigid.AddTorque(torque, ForceMode.Impulse);

               carriedOj.enabled = true;
           });
    }

}
