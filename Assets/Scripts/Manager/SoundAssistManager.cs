using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SoundAssistManager : MonoBehaviour
{
    public static SoundAssistManager Instance { get; private set; }

    // 딕셔너리로 변경 (파일명을 키로 사용)
    public Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    public AudioMixer audioMixer_Master;


    [Header("AudioBlock_BGM")]
    public AudioSource audioSource_BGM;

    [Header("AudioBlock_SFX")]
    public int iPoolSize;
    public GameObject audioPlaterBlockPrefab; 
    public Queue<GameObject> audioPlayerBlockPool = new Queue<GameObject>();
    public List<AudioPlayerBlock> audioPlayerBlockList = new List<AudioPlayerBlock>();


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            
            LoadSounds("Sounds");
            InitializeAudioPlayerBlockPool(iPoolSize);

            AudioMixerSet();

            // Invoke("AudioMixerSet",0.1f);


            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            UnmuteMasterVolume();
            Destroy(gameObject); // 이미 존재하는 인스턴스가 있으면 현재 오브젝트 파괴
        }
        
        string currentSceneName = SceneManager.GetActiveScene().name;
        this.BGMChange(currentSceneName);

    }

    // 사운드 찾기
    #region 

    private void LoadSounds(string folderPath)
    {
        //string fullPath = Path.Combine(Application.dataPath, "Resources", folderPath);
        //string[] files = Directory.GetFiles(fullPath, "*.mp3", SearchOption.AllDirectories);  // MP3 파일만 로드

        //foreach (var file in files)
        //{
        //    string relativePath = "Sounds" + file.Substring(Application.dataPath.Length).Replace("\\", "/").Replace(Path.GetExtension(file), "");

        //    StartCoroutine(LoadMP3AudioClip(file));
        //}

        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderPath);
        foreach (AudioClip clip in clips)
        {
            soundDictionary[clip.name] = clip;
        }
    }

    private IEnumerator LoadMP3AudioClip(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    string clipName = Path.GetFileNameWithoutExtension(filePath);
                    soundDictionary[clipName] = clip;
                }
            }
            else
            {
                Debug.LogWarning($"Failed to load sound from {filePath}: {www.error}");
            }
        }
    }

    // #. 현재 딕셔너리에 담겨 있는 AudioClip Key / Value 출력
    public void DebugSoundDictionary()
    {
        foreach (var entry in soundDictionary)
        {
            string clipName = entry.Key;
            AudioClip clip = entry.Value;
            Debug.Log($"Key: {clipName}, Clip: {clip.name}, Length: {clip.length} seconds");
        }
    }
    #endregion


   



    // 풀링 관련
    #region

    // 오브젝트 풀 초기화: 지정된 수만큼 오브젝트를 미리 생성
    public void InitializeAudioPlayerBlockPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newAudioPlayerBlock = Instantiate(audioPlaterBlockPrefab);
            AudioPlayerBlock audioBlock  = newAudioPlayerBlock.GetComponent<AudioPlayerBlock>();
            newAudioPlayerBlock.transform.SetParent(transform, this);
            newAudioPlayerBlock.SetActive(false);  // 처음에는 비활성화
            audioPlayerBlockPool.Enqueue(newAudioPlayerBlock);

            audioPlayerBlockList.Add(audioBlock);
        }
    }


    // 오브젝트 풀에서 사운드 블럭 가져오기 (효과음)
    public GameObject GetSFXAudioBlock(string audioClipName, Transform objTransform, bool _bVoice = false)
    {
        if (audioClipName != null)
        {
            if (audioPlayerBlockPool.Count > 0)
            {
                GameObject audioPlayerBlock = audioPlayerBlockPool.Dequeue();
                audioPlayerBlock.transform.SetParent(objTransform);
                audioPlayerBlock.SetActive(true);  // 가져온 오브젝트 활성화

                AudioPlayerBlock audioBlock = audioPlayerBlock.GetComponent<AudioPlayerBlock>();
                if (soundDictionary.ContainsKey(audioClipName))
                {
                    audioBlock.audioSource.clip = soundDictionary[audioClipName];  // 오디오 클립 할당
                    audioBlock.bVoice = _bVoice;
                    audioBlock.PlayAudioClip();  // 오디오 클립 재생
                }
                else
                {
                    Debug.LogWarning($"Sound with name {audioClipName} not found in the dictionary.");
                }

                return audioPlayerBlock;
            }
            else
            {
                Debug.LogWarning("AudioPlayerBlock Pool is empty! Creating new AudioPlayerBlock.");
                GameObject newAudioPlayerBlock = Instantiate(audioPlaterBlockPrefab);
                newAudioPlayerBlock.transform.SetParent(objTransform);

                AudioPlayerBlock audioBlock = newAudioPlayerBlock.GetComponent<AudioPlayerBlock>();
                if (soundDictionary.ContainsKey(audioClipName))
                {
                    audioBlock.audioSource.clip = soundDictionary[audioClipName];  // 오디오 클립 할당
                    audioBlock.bVoice = _bVoice;
                    audioBlock.PlayAudioClip();  // 오디오 클립 재생
                }
                else
                {
                    Debug.LogWarning($"Sound with name {audioClipName} not found in the dictionary.");
                }

                audioPlayerBlockList.Add(audioBlock);

                return newAudioPlayerBlock;
            }
        }

        return null;
    }
    // 오브젝트 풀로 오브젝트 반환
    public void ReturnAudioPlayerBlock(GameObject audioPlayerBlock)
    {
        audioPlayerBlock.transform.SetParent(transform, this);
        audioPlayerBlock.SetActive(false);  // 오브젝트를 비활성화

        AudioPlayerBlock audioBlock = audioPlayerBlock.GetComponent<AudioPlayerBlock>();
        audioBlock.audioSource.clip = null;

        audioPlayerBlockPool.Enqueue(audioPlayerBlock);  // 풀로 되돌려보냄
    }



    #endregion





    // 사운드 조절 관련
    #region



    // #. 저장 데이터에서 사운드 값 불러오기
    public void AudioMixerSet()
    {
        float masterVolume = Mathf.Lerp(-80f, 0f, SaveData_Manager.Instance.GetMasterVolume());
        audioMixer_Master.SetFloat("Master", masterVolume);

        float bgmVolume = Mathf.Lerp(-80f, 0f, SaveData_Manager.Instance.GetBGMVolume());
        audioMixer_Master.SetFloat("BGM", bgmVolume);

        float sfxVolume = Mathf.Lerp(-80f, 0f, SaveData_Manager.Instance.GetSFXVolume());
        audioMixer_Master.SetFloat("SFX", sfxVolume);

        float voiceVolume = Mathf.Lerp(-80f, 0f, SaveData_Manager.Instance.GetVoiceVolume());
        audioMixer_Master.SetFloat("Voice", voiceVolume);

        Debug.Log("사운드 수치를 적용합니다");

        //bool isMasterMuted = masterVolume <= -30f;
        //audioMixer_Master.SetFloat("MasterMute", isMasterMuted ? 1f : 0f);

        //bool isBGMMuted = bgmVolume <= -50f;
        //.SetFloat("BGMMute", isBGMMuted ? 1f : 0f);

        //bool isSFXMuted = sfxVolume <= -50f;
        //audioMixer_Master.SetFloat("SFXMute", isSFXMuted ? 1f : 0f);

        // audioSource_BGM.Play();
    }


    public void MuteMasterVolume(float fMuteDuration = 4f)
    {
        Debug.Log("음소거");

        float muteVolume = -80f;

        // DOTween을 사용하여 볼륨을 2초 동안 서서히 -80f로 변경
        DOTween.To(() => {
            audioMixer_Master.GetFloat("Master", out float tempMaster);
            return tempMaster;
        },
            x => {
                audioMixer_Master.SetFloat("Master", x);
                // audioMixer_Master.SetFloat("MasterMute", x <= -30f ? 1f : 0f);  // 볼륨 값에 따라 뮤트 처리
            },
            muteVolume, fMuteDuration).SetUpdate(true);

    }

    public void UnmuteMasterVolume(float fUnMuteDuration = 4f)
    {
        if (!SaveData_Manager.Instance.GetBoolFristStart()) fUnMuteDuration += 2f;

        Debug.Log("음소거 롤백");

        // 저장된 원래 볼륨 값 가져오기 (0과 1 사이의 값)
        // float originalVolume = SaveData_Manager.Instance.GetMasterVolume();
        float adjustedVolume = Mathf.Lerp(-80f, 0f, SaveData_Manager.Instance.GetMasterVolume());
        audioMixer_Master.SetFloat("Master", -80f);



        // 실행 중인 Master 볼륨에 대한 DOTween 애니메이션만 중지
        DOTween.Kill(audioMixer_Master);

        DOTween.To(() => {
            audioMixer_Master.GetFloat("Master", out float tempMaster);
            return tempMaster;
        },
            x => {
                audioMixer_Master.SetFloat("Master", x);
                //audioMixer_Master.SetFloat("MasterMute", x <= -30f ? 1f : 0f);  // 볼륨 값에 따라 뮤트 처리
            },
            adjustedVolume, fUnMuteDuration).SetUpdate(true);
    }



    // #. 다른 코드들에서 호출하고 있음
    public void BGMChange(string sSceneName, string sCutSceneBGM = null)
    {
        // 컷씬용 음악 재생
        if(sCutSceneBGM != null)
        {
            switch (sCutSceneBGM)
            {
                case "BGM_CutScene1":
                    UnmuteMasterVolume(6f);

                    Debug.Log("BGM_CutScene1 _ 컷씬용 음악 재생");
                    audioSource_BGM.Stop();
                    audioSource_BGM.clip = soundDictionary["BGM_CutScene1"];
                    audioSource_BGM.Play();
                    break;


                default:
                    break;
            }


        }
        else
        {
            switch (sSceneName)
            {
                case "Chapter1_1_City":
                case "Chapter1_2_Subway":
                case "Chapter1_3_City":
                case "Chapter0_1_Alley":
                    if (audioSource_BGM.clip != soundDictionary["TestBGM"])
                    {

                    }
                    UnmuteMasterVolume(4f);
                    Debug.Log("TestBGM 재생");

                    audioSource_BGM.Stop();
                    audioSource_BGM.clip = soundDictionary["TestBGM"];
                    audioSource_BGM.Play();
                    break;

                case "MainMenu":
                    if (audioSource_BGM.clip != soundDictionary["The Last Campfire OST  Title Screen"])
                    {

                    }
                    UnmuteMasterVolume(4f);
                    Debug.Log("The Last Campfire OST  Title Screen");

                    audioSource_BGM.Stop();
                    audioSource_BGM.clip = soundDictionary["The Last Campfire OST  Title Screen"];
                    audioSource_BGM.Play();
                    break;



                default:
                    Debug.Log("일치하는 사운드가 없습니다");
                    audioSource_BGM.Stop();



                    break;
            }
        }


    }







    #endregion


}






