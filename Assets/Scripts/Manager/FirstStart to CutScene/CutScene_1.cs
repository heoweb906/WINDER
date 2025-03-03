using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene_1 : MonoBehaviour
{
    public GameObject imageBackGround;
    public GameObject[] imageCutscenes;

    private void Start()
    {
        if (SaveData_Manager.Instance.GetBoolFristStart()) return;

        Debug.Log("�ƾ��� ��û�� �̷��� �����ϴ�. 1�� �ƾ��� �����մϴ�");

        // ���߿� �ٽ� �� �������� ������� ��
        GameAssistManager.Instance.bCantInpuFunc = true;                    // �÷��̾� ���� ����
        InGameUIController.Instance.bIsUIDoing = true;                      // UI ���� ����
        SoundAssistManager.Instance.BGMChange(null, "BGM_CutScene1");       // CutScene�� BGM ���

        StartCoroutine(PlayCutscenes());
    }


    private IEnumerator PlayCutscenes()
    {
        // ��� Ȱ��ȭ �� ���̵� ��
        imageBackGround.SetActive(true);
        yield return StartCoroutine(FadeImage(imageBackGround, 1f, 1f, 0f));

        yield return new WaitForSecondsRealtime(1.5f); // 1.5�� ���

        foreach (GameObject obj in imageCutscenes)
        {
            obj.SetActive(true); // ���̵� �� ���� Ȱ��ȭ
            yield return StartCoroutine(FadeImage(obj, 0f, 1f, 1f)); // 1�� ���� ���̵� ��
            yield return new WaitForSecondsRealtime(1.5f);
            yield return StartCoroutine(FadeImage(obj, 1f, 0f, 1f)); // 1�� ���� ���̵� �ƿ�
            obj.SetActive(false); // ���̵� �ƿ� �� ��Ȱ��ȭ
            yield return new WaitForSecondsRealtime(1.5f);
        }

        // ��� ���̵� �ƿ� �� ��Ȱ��ȭ
        SoundAssistManager.Instance.MuteMasterVolume(2f);
        yield return StartCoroutine(FadeImage(imageBackGround, 1f, 0f, 1f));

        CutSceneEnd_GameStart();
    }
    // #. �̹��� Alpha�� ���� �Լ�
    private IEnumerator FadeImage(GameObject obj, float startAlpha, float endAlpha, float duration)
    {
        Image img = obj.GetComponent<Image>();
        if (img == null) yield break;

        float elapsedTime = 0f;
        Color color = img.color;
        color.a = startAlpha;
        img.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            img.color = color;
            yield return null;
        }

        color.a = endAlpha;
        img.color = color;
    }


    
    private void CutSceneEnd_GameStart()
    {
        imageBackGround.SetActive(false);
        SaveData_Manager.Instance.SetBoolFirstStart(true);          // �ƾ��� ����

        GameAssistManager.Instance.bCantInpuFunc = false;                    // �÷��̾� ���� ����
        GameAssistManager.Instance.PlayerInputLockOff();
        InGameUIController.Instance.bIsUIDoing = false;

        string currentSceneName = SceneManager.GetActiveScene().name;
        SoundAssistManager.Instance.BGMChange(currentSceneName);       // CutScene�� BGM ���
    }

}