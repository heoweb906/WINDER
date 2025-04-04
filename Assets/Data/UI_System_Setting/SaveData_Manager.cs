using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;


// 관리할 데이터 
[System.Serializable]
public class SettingsData
{
    // #. UI, 설정 관련 데이터
    public VolumeSettings Volume = new VolumeSettings();
    public bool bFullScreen; // FullScreen 상태인지 아닌지
    public ResolutionSettings Resolution = new ResolutionSettings();


    // #. 게임 진척도 관련 데이터
    public int iClearStageNum;
    public string sSceneName;
    public int iTransformRespawn;
    public int iCameraNum;
    public bool bFirstStart;
}

[System.Serializable]
public class VolumeSettings
{
    public float Master;
    public float BGM;
    public float Effect;
    public float Voice;
}

[System.Serializable]
public class ResolutionSettings
{
    public int Width;
    public int Height;

    public static List<ResolutionSettings> AvailableResolutions = new List<ResolutionSettings>
    {
        new ResolutionSettings { Width = 720, Height = 480 },
        new ResolutionSettings { Width = 1280, Height = 720 },
        new ResolutionSettings { Width = 1920, Height = 1080 },
        new ResolutionSettings { Width = 2560, Height = 1440 }
    };

    public string GetResolutionName()
    {
        return $"{Width}x{Height}";
    }
}

//===================================================================================================


public class SaveData_Manager : MonoBehaviour
{
    public static SaveData_Manager Instance { get; private set; }

    private SettingsData settingsData = new SettingsData();
    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 인스턴스가 있으면 현재 오브젝트 파괴
        }

        filePath = Path.Combine(Application.persistentDataPath, "UISetting.json");
        LoadSettings();
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) SaveSettings();
        if (Input.GetKeyDown(KeyCode.C)) ClearData();
        if (Input.GetKeyDown(KeyCode.P)) PrintData();
    }

    //===================================================================================================
    // Volume setters
    public void SetMasterVolume(float value)
    {
        settingsData.Volume.Master = value;
        SaveSettings();
    }

    public void SetBGMVolume(float value)
    {
        settingsData.Volume.BGM = value;
        SaveSettings();
    }

    public void SetSFXVolume(float value)
    {
        settingsData.Volume.Effect = value;
        SaveSettings();
    }

    public void SetVoiceVolume(float value)
    {
        settingsData.Volume.Voice = value;
        SaveSettings();
    }
    //===================================================================================================
    // Volume getters
    public float GetMasterVolume()
    {
        return settingsData.Volume.Master;
    }

    public float GetBGMVolume()
    {
        return settingsData.Volume.BGM;
    }

    public float GetSFXVolume()
    {
        return settingsData.Volume.Effect;
    }

    public float GetVoiceVolume()
    {
        return settingsData.Volume.Voice;
    }

    //===================================================================================================
    // FullScreen setters and getters
    public void SetFullScreen(bool isFullScreen)
    {
        settingsData.bFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
        SaveSettings();
    }

    public bool GetBoolFullScreen()
    {
        return settingsData.bFullScreen;
    }
    //===================================================================================================
    // Resolution setters and getters
    public void SetResolution(int index)
    {
        if (index >= 0 && index < ResolutionSettings.AvailableResolutions.Count)
        {
            settingsData.Resolution = ResolutionSettings.AvailableResolutions[index];
            // 실제로 화면 해상도를 변경합니다.
            Screen.SetResolution(settingsData.Resolution.Width, settingsData.Resolution.Height, settingsData.bFullScreen);
            SaveSettings();
        }
        else
        {
            Debug.LogWarning("Invalid resolution index.");
        }
    }

    public ResolutionSettings GetResolution()
    {
        return settingsData.Resolution;
    }
    public int GetResolutionIndex()
    {
        for (int i = 0; i < ResolutionSettings.AvailableResolutions.Count; i++)
        {
            // 현재 해상도가 AvailableResolutions 목록의 항목과 일치하는지 확인
            if (settingsData.Resolution.Width == ResolutionSettings.AvailableResolutions[i].Width &&
                settingsData.Resolution.Height == ResolutionSettings.AvailableResolutions[i].Height)
            {
                return i; // 인덱스를 반환
            }
        }
        Debug.LogWarning("Current resolution not found in available resolutions.");
        return -1;
    }
    //===================================================================================================
    public void SetIntClearStageNum(int _iClearStageNum)
    {
        settingsData.iClearStageNum = _iClearStageNum;
    }
    public int GetIntClearStageNum()
    {
        return settingsData.iClearStageNum;
    }

    //===================================================================================================
    public void SetStringSceneName(string _sSceneName)
    {
        settingsData.sSceneName = _sSceneName;
    }
    public string GetStringSceneName()
    {
        return settingsData.sSceneName;
    }
    //===================================================================================================
    public void SetIntTransformRespawn(int _iTransformRespawn)
    {
        settingsData.iTransformRespawn = _iTransformRespawn;
    }
    public int GetIntTransformRespawn()
    {
        return settingsData.iTransformRespawn;
    }
    //===================================================================================================
    public void SetIntCameraNum(int _iCameraNum)
    {
        settingsData.iCameraNum = _iCameraNum;
    }
    public int GetIntCameraNum()
    {
        return settingsData.iCameraNum;
    }
    //===================================================================================================

    public void SetBoolFirstStart(bool bbb)
    {
        settingsData.bFirstStart = bbb;
    }
    public bool GetBoolFristStart()
    {
        return settingsData.bFirstStart;
    }
    //===================================================================================================
    // Save and load methods
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(settingsData, true);
        File.WriteAllText(filePath, json);
    }

    private void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            settingsData = JsonUtility.FromJson<SettingsData>(json);

  
            Debug.Log("저장되었던 정보를 불러옵니다.");
        }
        else
        {
            // Default values
            settingsData.Volume.Master = 0.7f;
            settingsData.Volume.BGM = 0.7f;
            settingsData.Volume.Effect = 0.7f;
            settingsData.Volume.Voice = 0.7f;

            SetFullScreen(true);
            SetResolution(3);


            settingsData.sSceneName = null;
            settingsData.iTransformRespawn = 0;
            settingsData.iCameraNum = 0;


            SaveSettings();

            Debug.Log("불러올 데이터가 없습니다. 초기 세팅값을 설정합니다.");
        }
    }



    private void ClearData()
    {
        settingsData.Volume.Master = 0.7f;
        settingsData.Volume.BGM = 0.7f;
        settingsData.Volume.Effect = 0.7f;
        settingsData.Volume.Voice = 0.7f;

        SetFullScreen(true);
        SetResolution(3);

        settingsData.iClearStageNum = 0;
        settingsData.sSceneName = null;
        settingsData.iTransformRespawn = 0;
        settingsData.iCameraNum = 0;

        SaveSettings();
    }


    public void PrintData()
    {
        Debug.Log("=== 저장된 설정 정보 ===");

        // Volume 정보 출력
        Debug.Log($"Master Volume: {settingsData.Volume.Master}");
        Debug.Log($"BGM Volume: {settingsData.Volume.BGM}");
        Debug.Log($"Effect Volume: {settingsData.Volume.Effect}");
        Debug.Log($"Voice Volume: {settingsData.Volume.Voice}");

        // FullScreen 상태 출력
        Debug.Log($"Full Screen: {settingsData.bFullScreen}");

        // Resolution 정보 출력
        Debug.Log($"Resolution: {settingsData.Resolution.Width}x{settingsData.Resolution.Height}");

        // SceneName 출력
        Debug.Log($"Scene Name: {settingsData.sSceneName}");

        // TransformRespawn, CameraNum, FirstStart 출력
        Debug.Log($"Clear Stage Number: {settingsData.iClearStageNum}");
        Debug.Log($"Transform Respawn: {settingsData.iTransformRespawn}");
        Debug.Log($"Camera Number: {settingsData.iCameraNum}");
        Debug.Log($"First Start: {settingsData.bFirstStart}");

        Debug.Log("=====================");
    }



    public void GameClearDataReset()
    {
        settingsData.iClearStageNum = 0;
        settingsData.sSceneName = null;
        settingsData.iTransformRespawn = 0;
        settingsData.iCameraNum = 0;
    }
        
}
