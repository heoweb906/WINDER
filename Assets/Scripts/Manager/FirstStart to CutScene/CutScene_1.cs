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

        Debug.Log("컷씬을 시청한 이력이 없습니다. 1차 컷씬을 진행합니다");

        // 나중에 다시 다 정상으로 돌려줘야 함
        GameAssistManager.Instance.bCantInpuFunc = true;                    // 플레이어 조작 막기
        InGameUIController.Instance.bIsUIDoing = true;                      // UI 조작 막기
        SoundAssistManager.Instance.BGMChange(null, "BGM_CutScene1");       // CutScene용 BGM 재생

        StartCoroutine(PlayCutscenes());
    }


    private IEnumerator PlayCutscenes()
    {
        // 배경 활성화 후 페이드 인
        imageBackGround.SetActive(true);
        yield return StartCoroutine(FadeImage(imageBackGround, 1f, 1f, 0f));

        yield return new WaitForSecondsRealtime(1.5f); // 1.5초 대기

        foreach (GameObject obj in imageCutscenes)
        {
            obj.SetActive(true); // 페이드 인 전에 활성화
            yield return StartCoroutine(FadeImage(obj, 0f, 1f, 1f)); // 1초 동안 페이드 인
            yield return new WaitForSecondsRealtime(1.5f);
            yield return StartCoroutine(FadeImage(obj, 1f, 0f, 1f)); // 1초 동안 페이드 아웃
            obj.SetActive(false); // 페이드 아웃 후 비활성화
            yield return new WaitForSecondsRealtime(1.5f);
        }

        // 배경 페이드 아웃 후 비활성화
        SoundAssistManager.Instance.MuteMasterVolume(2f);
        yield return StartCoroutine(FadeImage(imageBackGround, 1f, 0f, 1f));

        CutSceneEnd_GameStart();
    }
    // #. 이미지 Alpha값 조절 함수
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
        SaveData_Manager.Instance.SetBoolFirstStart(true);          // 컷씬을 봤음

        GameAssistManager.Instance.bCantInpuFunc = false;                    // 플레이어 조작 막기
        GameAssistManager.Instance.PlayerInputLockOff();
        InGameUIController.Instance.bIsUIDoing = false;

        string currentSceneName = SceneManager.GetActiveScene().name;
        SoundAssistManager.Instance.BGMChange(currentSceneName);       // CutScene용 BGM 재생
    }

}