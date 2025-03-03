using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public MenuButton nowPlayerButton; // 현재 선택되어 있는 버튼
    public MenuButton lastButton;
    public MenuButton[] menuButtons;

    [SerializeField] private int nowPanelNum;

    public bool bNowSceneChange = false;

    private GameObject PanelNow;
    [Header("Main Panel")]
    public GameObject Panel_Main; // Panel Number = 0;

    [Header("Option Panel")]
    public AudioMixer audioMixer_Master;
    public GameObject Panel_Option; // Panel Number = 1;
    public Slider[] soundSliders;

    [Header("Other Panel")]
    public GameObject Panel_Other; // Panel Numebr 5;
    public GameObject Panel_Resolution; // Panel Numer 8;
    public GameObject Panel_Other_Production; // Panel Number 6;
    public GameObject Panel_Other_Sources; // Panel Number 7;
    public GameObject Panel_Warning; // Panel Number 9;
    


    [Header("UI 애니메이션 부드러움 수치")]
    private Vignette vignette;
    public GameObject image_FadeOut; // FadeOut에 사용할 Image
    public GameObject image_BlackBackGround;
    public bool bIsUIDoing; // UI가 뭔가 기능 중임
    public float duration; // 애니메이션 지속 시간
    public float maxScale; // 최대 크기 (커질 때의 크기)


    [Header("버튼 선택 시 생성되는 아이콘")]
    public GameObject objSelectIcon;
    private MenuButton lastPlayerButton = null; // 이전 nowPlayerButton 저장용
    private GameObject lastCreatedObject = null; // 마지막으로 생성한 빈 오브젝트 저장

    [Header("로고 이미지 관련")]
    public RectTransform ObjGameLogo;
    public RectTransform startTransform;
    public RectTransform targetTransform; // 목표 위치




    private void Start()
    {
        bIsUIDoing = true;
        FisrtFadeOutImageEffect();      // PanelChage() 포함
    }
    private void Update()
    {
        if(!bIsUIDoing)
        {
            InputKey();


            // 활성화된 버튼 옆 이미지 생성
            if (nowPlayerButton == null && lastCreatedObject != null)
            {
                Destroy(lastCreatedObject);
                lastCreatedObject = null;
                lastPlayerButton = null;
                return;
            }

            // nowPlayerButton이 변경되었고, null이 아닐 경우 처리
            if (nowPlayerButton != null && nowPlayerButton != lastPlayerButton)
            {
                // 이전에 생성한 오브젝트가 있다면 삭제
                if (lastCreatedObject != null)
                {
                    Destroy(lastCreatedObject);
                }

                if(nowPlayerButton.bCanSelectIcon)
                {
                    // objSelectIcon을 복제하여 nowPlayerButton의 자식으로 추가
                    lastCreatedObject = Instantiate(objSelectIcon);
                    lastCreatedObject.transform.SetParent(nowPlayerButton.transform, false); // 부모 설정
                }

                lastPlayerButton = nowPlayerButton;

            }
        }
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (nowPlayerButton != null) nowPlayerButton.ImplementButton();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FindClosestButton(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FindClosestButton(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FindClosestButton(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FindClosestButton(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && nowPanelNum != 0 && !bNowSceneChange)
        {
            if (nowPanelNum == 1)
            {
                Panel_Option.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(0, duration);

            }

            if (nowPanelNum == 5)
            {
                Panel_Other.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(0, duration);
            }

            if (nowPanelNum == 6)
            {
                Panel_Other_Production.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(5, duration);
            }
            if (nowPanelNum == 7)
            {
                Panel_Other_Sources.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(5, duration);
            }
            if (nowPanelNum == 8)
            {
                Panel_Resolution.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(1, duration);
            }
            if(nowPanelNum == 9)
            {
                Panel_Warning.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(0, duration);
            }


        }
    }

    // #. Arrow Key를 이용해서 현재 선택되어 있는 UI Button에서 가장 가까이에 있는 UI Button을 선택하도록 하는 함수
    private void FindClosestButton(Vector2 direction)
    {
        if (nowPlayerButton == null)
        {
            nowPlayerButton = lastButton;
            nowPlayerButton.SelectButtonOn();
            return;
        }

        float closestDistance = Mathf.Infinity;
        MenuButton closestButton = null;
        Vector2 currentPosition = nowPlayerButton.transform.position;

        foreach (MenuButton button in menuButtons)
        {
            if (button == nowPlayerButton) continue;

            Vector2 directionToButton = (Vector2)button.transform.position - currentPosition;

            // 정확한 방향 탐색: 지정된 방향과의 각도가 30도 이내일 때만 선택
            float angle = Vector2.Angle(direction, directionToButton);
            if (angle <= 45f) // 30도 범위로 제한
            {
                float distance = directionToButton.magnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestButton = button;
                }
            }
        }

        if (closestButton != null)
        {
            nowPlayerButton.SelectButtonOff();
            nowPlayerButton = closestButton;
            nowPlayerButton.SelectButtonOn();
        }
    }



    // #. Panel이 활성화 될 때 하위 버튼 항목들을 변경해줌
    public void PanelChage(int index, float fAnimSpeed = 1f, float fTextDealy = 0f)
    {
        image_FadeOut.SetActive(true);

        nowPlayerButton = null;

        switch (index)
        {
            case 0: // Main Panel 켜기
                // SaveData_Manager.Instance.SayveSettings();
                Panel_Main.SetActive(true);
                PanelNow = Panel_Main;
                OnOffBlackBackBoard(false);
                break;
            case 1: // Option Panel 켜기
                SliderValueSet();
                Panel_Option.SetActive(true);
                PanelNow = Panel_Option;
                OnOffBlackBackBoard(true);
                break;

            case 5: 
                Panel_Other.SetActive(true);
                PanelNow = Panel_Other;
                OnOffBlackBackBoard(true);
                break;

            case 6: 
                Panel_Other_Production.SetActive(true);
                PanelNow = Panel_Other_Production;
                break;

            case 7: 
                Panel_Other_Sources.SetActive(true);
                PanelNow = Panel_Other_Sources;
                break;

            case 8: 
                Panel_Resolution.SetActive(true);
                PanelNow = Panel_Resolution;
                break;

            case 9:
                Panel_Warning.SetActive(true);
                PanelNow = Panel_Warning;
                OnOffBlackBackBoard(true);
                break;


                

            case 999:
                return;
   
            default:
                break;
        }

        PanelOn(PanelNow, fAnimSpeed, fTextDealy);
        nowPanelNum = index;


        // Panel의 모든 하위 GameObject들을 가져옴
        Transform[] childTransforms = PanelNow.GetComponentsInChildren<Transform>(true);

        // MenuButton 스크립트를 상속받은 컴포넌트들을 찾아서 menuButtons 배열에 할당
        List<MenuButton> foundButtons = new List<MenuButton>();
        foreach (Transform childTransform in childTransforms)
        {
            // 하위 GameObject에서 MenuButton 스크립트를 상속받은 컴포넌트를 찾음
            MenuButton menuButton = childTransform.GetComponent<MenuButton>();
            if (menuButton != null)
            {
                foundButtons.Add(menuButton);
            }
        }

        // List를 배열로 변환하여 menuButtons에 할당
        menuButtons = foundButtons.ToArray();
        if (menuButtons.Length != 0) lastButton = menuButtons[0];
        lastButton = menuButtons[0];

      
    }

    public void PanelOff(int index)
    {
        // # 연출 1번
        PanelNow.SetActive(false);
        PanelChage(index, duration);

        // #.연출 2번
        //PanelNow.transform.DOScale(maxScale, duration / 2)
        //.SetEase(Ease.OutBack) // EaseOutBack 효과로 커짐
        //.OnComplete(() => // 커진 후에 크기를 0으로 줄임
        //{
        //    PanelNow.transform.DOScale(Vector3.zero, duration / 2)
        //        .SetEase(Ease.InBack)
        //        .OnComplete(() => // 크기가 0이 된 후에 SetActive(false) 실행
        //        {
        //            PanelNow.SetActive(false);
        //            PanelChage(index);
        //        });
        //});
    }
    public void PanelOn(GameObject ActivePanel, float fAnimSpeed = 1f, float fTextDelay = 0f)
    {
        // 연출 1번
        //ActivePanel.transform.localScale = Vector3.one * 0.5f;
        //ActivePanel.transform.DOScale(Vector3.one, duration / 2)
        //.SetEase(Ease.OutBack); // EaseOutBack 효과로 자연스럽게 커짐


        // 연출 2번
        Image[] images = ActivePanel.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] textMeshes = ActivePanel.GetComponentsInChildren<TextMeshProUGUI>();


        // 각 Image의 알파값을 0에서 1까지 서서히 변화
        foreach (Image img in images)
        {
            Color tempColor = img.color;
            tempColor.a = 0f;
            img.color = tempColor;

            // 알파값을 0에서 1로 1초 동안 서서히 올림
            DOTween.To(() => img.color.a, x => {
                tempColor.a = x;
                img.color = tempColor; 
                }, 1f, fAnimSpeed).SetEase(Ease.Linear).SetUpdate(true); // 1초 동안 알파값을 1로 만듦
        }


        // 각 TextMeshPro의 색상을 0에서 1까지 서서히 변화
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            // TextMeshPro의 초기 알파값을 0으로 설정
            Color textColor = textMesh.color;
            textColor.a = 0f;
            textMesh.color = textColor;

            // 텍스트 색상 알파값을 0에서 1로 1초 동안 서서히 올림
            DOTween.To(() => textMesh.color.a, x => {
                textColor.a = x;
                textMesh.color = textColor;
            }, 1f, fAnimSpeed).SetEase(Ease.Linear).SetUpdate(true)
            .SetDelay(fTextDelay)
            .OnComplete(() => {
                image_FadeOut.SetActive(false);
                bIsUIDoing = false;
            });
        }

    }




    /// <summary>
    /// 다른 씬과의 연결, 혹은 기타 작업들
    /// </summary>

    // #. Scene 전환 함수, 게임 시작 버튼에서 사용하지만, 버튼에는 버튼 관련 기능만 넣기 위해서
    public void StartNewGame(string sSceneSname = "Chapter0_1_Alley")
    {
        bNowSceneChange = true;

        // Vignette의 intensity를 현재 값에서 1로 서서히 변화
        //if (vignette != null)
        //{
        //    DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, 2.5f)
        //        .SetEase(Ease.OutQuad);

        //    Debug.Log("이부분이 실행중~~");

        //    // Vignette의 smoothness를 서서히 1로 변화, 마지막에 감속
        //    DOTween.To(() => vignette.smoothness.value, x => vignette.smoothness.value = x, 1f, 2.5f)
        //        .SetEase(Ease.OutQuad);
        //}


        image_FadeOut.SetActive(true);
        Image fadeoutImage = image_FadeOut.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;


        //SaveData_Manager.Instance.SetMasterVolume(soundSliders[0].value);
        //SaveData_Manager.Instance.SetBGMVolume(soundSliders[1].value);
        //SaveData_Manager.Instance.SetSFXVolume(soundSliders[2].value);

        SoundAssistManager.Instance.MuteMasterVolume();

        // 알파값을 서서히 1로, 마지막에 감속 후 씬 전환
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, 1f, 2.5f)
        .SetEase(Ease.OutQuad)
        .OnComplete(() => {
            // 애니메이션이 끝난 후 'Chapter_1' 씬으로 전환
            SoundAssistManager.Instance.BGMChange(sSceneSname);
            SceneManager.LoadScene(sSceneSname);
           
        });
    }



    // #. 사운드 슬라이더 함수
    private void SliderValueSet()
    {
        soundSliders[0].value = SaveData_Manager.Instance.GetMasterVolume();
        soundSliders[1].value = SaveData_Manager.Instance.GetBGMVolume();
        soundSliders[2].value = SaveData_Manager.Instance.GetSFXVolume();
        soundSliders[3].value = SaveData_Manager.Instance.GetVoiceVolume();
    }




    public void ControllSoundVolume_Master()
    {
        float adjustedVolume = Mathf.Lerp(-80f, 0f, soundSliders[0].value);
        audioMixer_Master.SetFloat("Master", adjustedVolume);

        //bool isMuted = adjustedVolume <= -30f;
        //audioMixer_Master.SetFloat("MasterMute", isMuted ? 1f : 0f);

        SaveData_Manager.Instance.SetMasterVolume(soundSliders[0].value);
    }
    public void ControllSoundVolume_BGM()
    {
        float adjustedVolume = Mathf.Lerp(-80f, 0f, soundSliders[1].value);
        audioMixer_Master.SetFloat("BGM", adjustedVolume);

        Debug.Log("버튼 함수 실행");
        Debug.Log(adjustedVolume);

        //bool isMuted = adjustedVolume <= -50f;
        //audioMixer_Master.SetFloat("BGMMute", isMuted ? 1f : 0f);

        SaveData_Manager.Instance.SetBGMVolume(soundSliders[1].value);
    }
    public void ControllSoundVolume_SFX()
    {
        float adjustedVolume = Mathf.Lerp(-80f, 0f, soundSliders[2].value);
        audioMixer_Master.SetFloat("SFX", adjustedVolume);

        //bool isMuted = adjustedVolume <= -50f;
        //audioMixer_Master.SetFloat("SFXMute", isMuted ? 1f : 0f);

        SaveData_Manager.Instance.SetSFXVolume(soundSliders[2].value);
    }
    public void ControllSoundVolume_Voice()
    {
        float adjustedVolume = Mathf.Lerp(-80f, 0f, soundSliders[3].value);
        audioMixer_Master.SetFloat("Voice", adjustedVolume);

        Debug.Log("버튼 함수 실행");
        Debug.Log(adjustedVolume);

        //bool isMuted = adjustedVolume <= -50f;
        //audioMixer_Master.SetFloat("SFXMute", isMuted ? 1f : 0f);

        SaveData_Manager.Instance.SetVoiceVolume(soundSliders[3].value);
    }



    // #. MainMenu Scene 최초 진입 시 페이드인 아웃
    private void FisrtFadeOutImageEffect()
    {
        image_FadeOut.SetActive(true);

        Image fadeoutImage = image_FadeOut.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;
        
        // 알파값을 서서히 1로, 마지막에 감속 후 씬 전환
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, 0f, 3.2f)
        .SetEase(Ease.OutQuad)
        .OnComplete(() => {
            // image_FadeOut.SetActive(false); 
        });

        PanelChage(0, 2.1f, 9.5f);


        // 로고 이미지 내려오는 효과
        if (ObjGameLogo != null && targetTransform != null)
        {
            ObjGameLogo.position = startTransform.position;

            ObjGameLogo.DOMove(targetTransform.position, 5.5f)
                .SetEase(Ease.OutQuad) // 부드러운 감속 효과
                .SetDelay(3.2f)
                .SetUpdate(true); // UI에서도 정상 작동
        }
    }
   

   

    // #. 설정창 등의 패널을 열 때 뒷배경에 검은 화면
    private void OnOffBlackBackBoard(bool bOnOff)
    {
        if(!image_BlackBackGround.activeSelf && bOnOff)
        {
            image_BlackBackGround.SetActive(bOnOff);

            Image fadeoutImage = image_BlackBackGround.GetComponent<Image>();
            Color fadeColor = fadeoutImage.color;
            fadeColor.a = 0f;

            DOTween.To(() => fadeColor.a, x =>
            {
                fadeColor.a = x;
                fadeoutImage.color = fadeColor;
            }, 1f, duration)
            .SetEase(Ease.OutQuad);
        }
        else if(image_BlackBackGround.activeSelf && !bOnOff)
        {
            Image fadeoutImage = image_BlackBackGround.GetComponent<Image>();
            Color fadeColor = fadeoutImage.color;
            fadeColor.a = 1f;

            DOTween.To(() => fadeColor.a, x =>
            {
                fadeColor.a = x;
                fadeoutImage.color = fadeColor;
            }, 0f, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                image_BlackBackGround.SetActive(bOnOff);

            });
        }




    }
   


}
