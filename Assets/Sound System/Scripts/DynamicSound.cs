using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSound : MonoBehaviour
{
    [SerializeField]List<AudioClip> audioClips;
    [SerializeField]DynamicSoundSetting defaultSetting;
    [SerializeField]List<AudioSource> audioSources;
    [SerializeField]float entranceDelay;
    DynamicSoundSetting currentSetting;
    Coroutine coroutine;
    List<float> startVolumes = new();
    private void Awake() {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent<AudioSource>(out AudioSource audioSource))
            {
                audioSources.Add(audioSource);
                startVolumes.Add(audioSource.volume);
            }  
        }
        currentSetting = defaultSetting;
    }
    public void ChangeModeSetting(DynamicSoundSetting targetDynamicSoundSetting)
    {
        currentSetting = targetDynamicSoundSetting;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        if(targetDynamicSoundSetting.repeatCoolDown <= 0)
            return;
        coroutine = StartCoroutine(StartDynamicSound());
    }
    IEnumerator StartDynamicSound()
    {
        Debug.Log("Started Dynamic Audio With Setting: "+currentSetting);
        AudioClip randomClip = null;
        yield return new WaitForSeconds(entranceDelay);
        while(true)
        {
            randomClip = GetRandomClip(audioClips,randomClip);
            for(int i = 0;i<audioSources.Count;i++)
            {
                audioSources[i].clip = randomClip;
                audioSources[i].Play();
            }
            yield return new WaitForSeconds(randomClip.length + currentSetting.repeatCoolDown);
            for(int i = 0;i<audioSources.Count;i++)
            {
                audioSources[i].Stop();
            }
        }
    }
    AudioClip GetRandomClip(List<AudioClip> clips, AudioClip lastClip)
    {
        AudioClip randomClip = lastClip;
        

        while (randomClip == lastClip)
        {
            randomClip = clips[Random.Range(0, clips.Count)];
        }
        return randomClip;
    }
}

