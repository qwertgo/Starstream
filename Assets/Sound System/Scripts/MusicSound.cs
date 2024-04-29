using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class MusicSound : MonoBehaviour
{
    enum PlayMode {RepeatQueue , RepeatLastOfQueue}
    [SerializeField]PlayMode playMode;
    [SerializeField]AudioSource audioSource;
    float startVolume;
    [SerializeField]List<AudioClip> audioClips;
    Sequence currentSequence;
    [SerializeField]Ease fadeEase;
    [SerializeField]float fadeDuration;
    [SerializeField]bool playOnStart;
    [SerializeField]float dimedVolume = 0.02f;
    [SerializeField]float dimFadeDuration;
    AudioClip currentClip;
    Queue<AudioClip> clipQueue = new();
    Coroutine coroutine = null;
    private void Awake() 
    {
        startVolume = audioSource.volume;
    }
    private void Start() {
        if(playOnStart)
            StartAmbientSound();
    }
    public void StartAmbientSound()
    {
        audioSource.volume = 0f;
        PlayQueue();
        audioSource.Play();
        currentSequence.Append(audioSource.DOFade(startVolume,fadeDuration)
            .SetEase(fadeEase)).
        AppendCallback(() => currentSequence = null);
    }
    public void StopAmbientSound()
    {
        if(currentClip == null)
            return;
        currentSequence.Append(audioSource.DOFade(startVolume-startVolume,fadeDuration)
            .SetEase(fadeEase)).
        AppendCallback(() => audioSource.Stop()).
        AppendCallback(() => currentSequence = null).
        AppendCallback(() => StopCoroutine(coroutine)).
        AppendCallback(() => coroutine = null);
        audioSource.Stop();
        currentClip = null;
    }
    public void NextAmbientSound()
    {
        currentClip = audioClips[Random.Range(0,audioClips.Count)];
        audioSource.clip = currentClip;
        audioSource.volume = 0;
        audioSource.Play();
        currentSequence.Append(audioSource.DOFade(startVolume,fadeDuration)
            .SetEase(fadeEase)).
        AppendCallback(() => currentSequence = null);
    }
    public void AddClipToQueue(AudioClip targetClip) => clipQueue.Enqueue(targetClip);

    public void PlayQueue()
    {
        switch (playMode)
        {
            case PlayMode.RepeatQueue:
                coroutine = StartCoroutine(PlayRepeatQueue());
                break;
            case PlayMode.RepeatLastOfQueue:
                coroutine = StartCoroutine(PlayRepeatLastOfQueue());
                break;
        }
    }
    IEnumerator PlayRepeatLastOfQueue()
    {
        for(int i = 0;i<audioClips.Count;i++)
        {
            clipQueue.Enqueue(audioClips[i]);
        }
        while(true)
        {
            audioSource.clip = clipQueue.Peek();
            audioSource.Play();
            Debug.Log("Start of Music Clip");
            
            yield return new WaitForSeconds(clipQueue.Peek().length);
            Debug.Log("End of Music Clip");
            if(clipQueue.Count >1)
                clipQueue.Dequeue();
        }
    }
    IEnumerator PlayRepeatQueue()
    {
        while(true)
        {
            for(int i = 0;i<audioClips.Count;i++)
            {
                audioSource.clip =  audioClips[i];
                yield return new WaitForSeconds(audioClips[i].length);
                if(i >= audioClips.Count)
                    i = 0;
            }
            
        }
    }
    [Button]
    public void DimSound()
    {
        audioSource.DOFade(dimedVolume,dimFadeDuration);
    }
    [Button]
    public void ReDimSound()
    {
        audioSource.DOFade(startVolume,dimFadeDuration);
    }
}
