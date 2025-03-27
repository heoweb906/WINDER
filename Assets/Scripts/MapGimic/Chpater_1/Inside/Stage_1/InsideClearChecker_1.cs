using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideClearChecker_1 : MonoBehaviour
{
    public bool bIsObject; // 오브젝트가 아닌경우에만 Checker로써 동작

    private bool bInObject;
    private bool bInPlayer;
    private bool bTriggerOn = false;

    [Header("연출에 사용할 것들")]
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
            // Emission 켜기
            mat.SetFloat("_UseEmission", 1f);
            mat.EnableKeyword("_EMISSION");

            // 3초 동안 _EmissionColor를 (1.7f, 1.7f, 1.7f, 1f)로 Tween
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

            // Transparent 렌더링 모드로 전환
            mat.SetFloat("_RenderingMode", 3f);
            mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetFloat("_ZWrite", 0f);
            mat.renderQueue = 3000; // Transparent 큐로 변경

            // Base Color 알파를 0으로 Tween
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
