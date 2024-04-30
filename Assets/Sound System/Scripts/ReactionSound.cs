using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class ReactionSound : MonoBehaviour
{
    [System.Serializable]
    public enum AudioPlayMode {OneShot , OneShotRandom , Repeat , LoopEndless , RepeatRandom , RepeatOnRandomSource}
    [SerializeField]List<AudioClip> audioClips;
    [SerializeField]AudioPlayMode audioPlayMode;
    [HideIf("audioPlayMode" , AudioPlayMode.OneShot)][SerializeField]int repeatAmount;
    [SerializeField]List<AudioSource> audioSources;
    [SerializeField]UnityEvent onFinish;
    [SerializeField]Ease fadeEase = Ease.Linear;
    [SerializeField]float fadeDuration = 0f;
    [SerializeField]bool overrideCrossfadeDuration = false;
    [ShowIf("overrideCrossfadeDuration")][SerializeField]float crossfadeDuration = .5f;
    [SerializeField]bool playOnStart = false;
    [SerializeField]bool randomPitch;
    [ShowIf("randomPitch")][SerializeField][MinMaxSlider(-3,3)]Vector2 pitchRange = new Vector2(0.9f,1.1f);
    [SerializeField]bool delay;
    [ShowIf("delay")][SerializeField]float delayAmount;
    public bool originalPlayOnStartValue {get; private set;}
    Coroutine coroutine;
    Sequence currentSequence;
    List<float> startVolumes = new();
    AudioSource audioSourceCrossFade = new();
    List<AudioSource> audioSourcesCrossFades = new();

    float currentClipStemp; 
    private void Awake() 
    {
        originalPlayOnStartValue = playOnStart;
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent<AudioSource>(out AudioSource audioSource))
            {
                audioSources.Add(audioSource);
                startVolumes.Add(audioSource.volume);
            }
        }
        if(audioSources.Count > 0 && transform.TryGetComponent<AudioSource>(out AudioSource localAudioSource))
        {
            audioSources.Add(localAudioSource);
            startVolumes.Add(localAudioSource.volume);
        }
    }
    private void Start() {
        if(playOnStart)
        {
            PlaySound();
        }
    }
    public void SetPlayOnStart(bool state) => playOnStart = state;
    public void SetOriginalStartValue() => originalPlayOnStartValue = playOnStart;
    [Button]
    public void PlaySound()
    {
        StopSoundImmediately();
        switch (audioPlayMode)
        {
            case AudioPlayMode.OneShot:
                coroutine = StartCoroutine(PlayOneShot());
                break;
            case AudioPlayMode.Repeat:
                coroutine = StartCoroutine(PlayOnRepeat());
                break;
            case AudioPlayMode.LoopEndless:
                coroutine = StartCoroutine(PlayEndless());
                break;
            case AudioPlayMode.RepeatRandom:
                coroutine = StartCoroutine(PlayEndlessRandom());
                break;
            case AudioPlayMode.OneShotRandom:
                coroutine = StartCoroutine(PlayOneShotRandom());
                break;
            /*case AudioPlayMode.RepeatInOrder:
                break;*/
            case AudioPlayMode.RepeatOnRandomSource:
                coroutine = StartCoroutine(PlayRepeatOnRandomSource());    
                break;
        }
    }
    [Button]
    public void StopSound()
    {
        if(coroutine == null)
            return;
        StopCoroutine(coroutine);
        coroutine = null;
        
        currentSequence = DOTween.Sequence();

        foreach (var audioSource in audioSources)
        {
            currentSequence.Append(audioSource.DOFade(0f, fadeDuration)
                .SetEase(fadeEase))
                .AppendCallback(() => audioSource.Stop())
                .AppendCallback(() => audioSource.time = 0);
        }
        currentSequence.AppendCallback(() => currentSequence = null);
        onFinish?.Invoke();
    }
    [Button]
    public void StopSoundImmediately()
    {
        if(coroutine == null)
            return;
        StopCoroutine(coroutine);
        coroutine = null;
        
        currentSequence = DOTween.Sequence();

        foreach (var audioSource in audioSources)
        {
            audioSource.Stop();
            audioSource.time = 0;
            ResetAudioSource(audioSource);
        }
        audioSourceCrossFade = null;
        audioSourcesCrossFades.Clear();
        currentSequence = null;
        onFinish?.Invoke();
    }
    public void PauseSound()
    {   
        currentSequence = DOTween.Sequence();

        foreach (var audioSource in audioSources)
        {
            currentSequence.Append(audioSource.DOFade(0f, fadeDuration)
                .SetEase(fadeEase))
                .AppendCallback(() => audioSource.Pause())
                .AppendCallback(() => ResetAudioSource(audioSource));
        }
        currentSequence.AppendCallback(() => currentSequence = null);
        onFinish?.Invoke();
    }
    public void ResumeSound()
    {
        PlaySound();
        for(int i = 0;i<audioSources.Count;i++)
        {
            if(audioSources[i].clip == null)
                return;
            audioSources[i].time = currentClipStemp;
        }
    }
    IEnumerator PlayOneShot()
    {
        AudioClip currentClip = null;
        float pitch = Random.Range(pitchRange.x,pitchRange.y);
        for(int i = 0; i<audioSources.Count;i++)
        {
            if(randomPitch)
                audioSources[i].pitch = pitch;
            if(delay)
                yield return new WaitForSeconds(delayAmount);
            audioSources[i].clip = audioClips[0];
            audioSources[i].Play();
            currentClip = audioClips[0];
        }
        yield return new WaitForSeconds(audioClips[0].length);
        StopSoundImmediately();
    }
    IEnumerator PlayOneShotRandom()
    {
        AudioClip currentClip = null;
        currentClip = GetRandomClip(audioClips,currentClip);
        float pitch = Random.Range(pitchRange.x,pitchRange.y);
        for(int i = 0; i<audioSources.Count;i++)
        {   
            if(randomPitch)
                audioSources[i].pitch = pitch;
            if(delay)
                yield return new WaitForSeconds(delayAmount);
            audioSources[i].clip = currentClip;
            audioSources[i].Play();
        }
        yield return new WaitForSeconds(currentClip.length);
        StopSoundImmediately();
    }
    IEnumerator PlayOnRepeat()
    {
        int amount = 0;
        while(amount < repeatAmount)
        {
            for(int i = 0; i<audioSources.Count;i++)
            {
                audioSources[i].clip = audioClips[0];
                audioSources[i].Play();
            }
            yield return new WaitForSeconds(audioClips[0].length);
            amount++;
        }
        StopSoundImmediately();
    }
    IEnumerator PlayEndless()
    {
        while(true)
        {
            for(int i = 0; i<audioSources.Count;i++)
            {
                audioSources[i].clip = audioClips[0];
                audioSources[i].Play();
            }
            yield return new WaitForSeconds(audioClips[0].length);
        }
    }
    IEnumerator PlayEndlessRandom()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            AudioSource audioSourceCrossFade = audioSources[i].gameObject.AddComponent<AudioSource>();
            CopyAudioSourceSettings(audioSources[i], audioSourceCrossFade);
            audioSourcesCrossFades.Add(audioSourceCrossFade);
        }
        AudioClip lastClip = null;
        while (true)
        {
            if (audioSources[0].clip != null)
            {
                yield return new WaitForSeconds(audioSources[0].clip.length-crossfadeDuration);

                float currentAudioTime = audioSources[0].time;
                float currentCrossFadeTime = audioSourcesCrossFades[0].time;

                List<AudioSource> tempCurrentAudioSources = new List<AudioSource>(audioSources);
                audioSources = audioSourcesCrossFades;
                audioSourcesCrossFades = tempCurrentAudioSources;

                for (int i = 0; i < audioSources.Count; i++)
                {
                    audioSources[i].volume = 0f;
                    audioSources[i].DOFade(startVolumes[i], fadeDuration);
                }
                for (int i = 0; i < audioSourcesCrossFades.Count; i++)
                {
                    audioSourcesCrossFades[i].volume = audioSourcesCrossFades[i].volume;
                    audioSourcesCrossFades[i].DOFade(0f, fadeDuration);
                }

            }
            AudioClip randomClip = GetRandomClip(audioClips,lastClip);
            lastClip = randomClip;
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].clip = randomClip;
                audioSources[i].Play();
            }
        }
    }
    IEnumerator PlayRepeatOnRandomSource()
    {
        int randomIndex = Random.Range(0,audioSources.Count);
        AudioSource targetSource = audioSources[randomIndex];
        targetSource.clip = null;
        targetSource.time = 0;

        audioSourceCrossFade = audioSources[randomIndex].gameObject.AddComponent<AudioSource>();
        Debug.Log("other room");
        AudioClip targetClip = GetRandomClip(audioClips,null);
        
        CopyAudioSourceSettings(audioSources[randomIndex], audioSourceCrossFade);
        while (true)
        {
            if (audioSources[randomIndex].clip != null)
            {
                yield return new WaitForSeconds(targetClip.length-crossfadeDuration);

                float currentAudioTime = targetSource.time;
                float currentCrossFadeTime = audioSourceCrossFade.time;

                AudioSource tempCurrentAudioSources = targetSource;
                targetSource = audioSourceCrossFade;
                audioSourceCrossFade = tempCurrentAudioSources;

                audioSources[randomIndex].volume = 0f;
                audioSources[randomIndex].DOFade(startVolumes[randomIndex], fadeDuration);
            
                audioSourceCrossFade.volume = audioSourceCrossFade.volume;
                audioSourceCrossFade.DOFade(0f, fadeDuration);
            }
            audioSources[randomIndex].clip = targetClip;
            audioSources[randomIndex].Play();
        }
    }
    void CopyAudioSourceSettings(AudioSource source, AudioSource target)
    {
        target.volume = source.volume;
        target.pitch = source.pitch;
        target.spatialBlend = source.spatialBlend;
        target.panStereo = source.panStereo;
        target.loop = source.loop;
        target.maxDistance = source.maxDistance;
        target.minDistance = source.minDistance;
        target.spatialBlend = source.spatialBlend;

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
    void ResetAudioSource(AudioSource targetSource)
    {
        targetSource.clip = null;
    }
}
