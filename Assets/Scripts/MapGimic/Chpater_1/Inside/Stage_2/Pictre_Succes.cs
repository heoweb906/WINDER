using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pictre_Succes : MonoBehaviour
{
    private CarriedObject carriedOj;
    private Rigidbody rigid;


    private void Awake()
    {
        carriedOj = GetComponent<CarriedObject>();
        rigid = GetComponent<Rigidbody>();

        ChangeToColorObj_();
    }


    public void ChangeToColorObj_()
    {
        Invoke("ChangeToColorObj", 2f);
    }


    public void ChangeToColorObj()
    {
        rigid.isKinematic = false; // ���� ���� Ȱ��ȭ

        Sequence seq = DOTween.Sequence();

        Vector3 originalScale = transform.localScale; // ���� ũ�� ����

        seq.Append(transform.DOScale(originalScale * 1.2f, 0.1f).SetEase(Ease.OutBack)) // ���� ũ�� �������� 1.2�� Ŀ��
           .Append(transform.DOScale(originalScale * 0.23f, 0.15f).SetEase(Ease.InOutQuad)) // ���� ũ�� �������� 0.8�� �پ��
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
