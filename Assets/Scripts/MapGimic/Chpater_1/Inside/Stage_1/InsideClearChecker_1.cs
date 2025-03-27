using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideClearChecker_1 : MonoBehaviour
{
    public bool bIsObject; // ������Ʈ�� �ƴѰ�쿡�� Checker�ν� ����

    private bool bInObject;
    private bool bInPlayer;
    private bool bTriggerOn = false;

    [Header("���⿡ ����� �͵�")]
    private ColorObj colorObj;


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<InsideClearChecker_1>() != null)
        {
            InsideClearChecker_1 _checker = other.GetComponent<InsideClearChecker_1>();
            if(_checker.bIsObject == true && !bInObject)
            {
                bInObject = true;
                DirectMapChange();
            }
        }


        if (other.CompareTag("Player") && !bInPlayer)
        {
            bInPlayer = true;
            DirectMapChange();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ColorObj>() != null)
        {
            colorObj = other.GetComponent<ColorObj>();
        }
    }


    private void DirectMapChange()
    {
        if (bInObject == false || bInPlayer == false) return;
        if (bTriggerOn) return;
        bTriggerOn = true;

        StartCoroutine(DirectMapChange_());

       

    }

    IEnumerator DirectMapChange_()
    {
        yield return new WaitForSeconds(2f);

        Renderer renderer = colorObj.GetComponent<Renderer>();

        if (renderer != null)
        {
            Material mat = renderer.material;
            // Emission �ѱ�
            mat.SetFloat("_UseEmission", 1f);
            mat.EnableKeyword("_EMISSION");

            // 3�� ���� _EmissionColor�� (1.7f, 1.7f, 1.7f, 1f)�� Tween
            DOTween.To(() => mat.GetColor("_EmissionColor"),
                       x => mat.SetColor("_EmissionColor", x),
                       new Color(1.7f, 1.7f, 1.7f, 1f),
                       2f)
                   .SetEase(Ease.Linear);
        }


        yield return new WaitForSeconds(2.3f);

        if (renderer != null)
        {
            Material mat = renderer.material;

            // Transparent ������ ���� ��ȯ
            mat.SetFloat("_RenderingMode", 3f);
            mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetFloat("_ZWrite", 0f);
            mat.renderQueue = 3000; // Transparent ť�� ����

            // Base Color ���ĸ� 0���� Tween
            Color startColor = mat.GetColor("_BaseColor");
            Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            DOTween.To(() => mat.GetColor("_BaseColor"),
                       x => mat.SetColor("_BaseColor", x),
                       targetColor,
                       3.0f) 
                   .SetEase(Ease.Linear);
        }

        yield return new WaitForSeconds(3.1f);

        GameAssistManager.Instance.GetPlayerScript().RemoveCarryObject();






    }
    
}
