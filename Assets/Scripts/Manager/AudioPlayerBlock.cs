using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayerBlock : MonoBehaviour
{
    public AudioSource audioSource;

    public bool bVoice = false;
    public AudioMixerGroup mixerGroupSFX;
    public AudioMixerGroup mixerGroupVoice;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip()
    {
        if (!bVoice) audioSource.outputAudioMixerGroup = mixerGroupSFX;
        else audioSource.outputAudioMixerGroup = mixerGroupVoice;

        audioSource.Play();
        StartCoroutine(WaitForAudioEnd());
    }

    // 오디오가 끝날 때까지 대기한 후, 풀로 반환
    private IEnumerator WaitForAudioEnd()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        SoundAssistManager.Instance.ReturnAudioPlayerBlock(gameObject);
    }
}
