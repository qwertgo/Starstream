using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

enum BlinkingMode {Static , Blinking , SequenceSimultaneously, SequenceSuccessively, SequenceRandomNext , SequenceBuildUpBuildDown , Flicker}
public class BlinkingLights : MonoBehaviour
{
    [SerializeField]bool playOnStart;
    [SerializeField]BlinkingMode blinkingMode;
    [SerializeField]Material lampOffMaterial;
    [SerializeField]bool overrideMaterial;
    [ShowIf("overrideMaterial")][SerializeField]Material lampMaterial;
    [SerializeField]bool randomizeColors;
    [ShowIf("randomizeColors")][ColorUsage(true,true)][SerializeField]Color minColor;
    [ShowIf("randomizeColors")][ColorUsage(true,true)][SerializeField]Color maxColor;
    [SerializeField]List<MeshRenderer> lampLights;
    [SerializeField]bool locateChildrenAsLampLights = true;
    //[SerializeField][Range(0f,1f)] float EnableOnStartProbability = 0.7f;
    [SerializeField]float enablingDelay;
    [HideIf("blinkingMode", BlinkingMode.Static)][SerializeField]float inactiveTime = 1f;
    [HideIf("blinkingMode", BlinkingMode.Static)][SerializeField]float activeTime = 1f;
    [ShowIf("blinkingMode", BlinkingMode.Flicker)][SerializeField][MinMaxSlider(-10,10)] Vector2 intensityRange = new Vector2(-10,0);
    [ShowIf("blinkingMode", BlinkingMode.Flicker)][SerializeField][MinMaxSlider(0,10)] Vector2 timeRangeBetweenEachFlicker;
    [ShowIf("blinkingMode", BlinkingMode.Flicker)][SerializeField][MinMaxSlider(0,10)] Vector2 timeRangeForFlickerOnTime;
    [SerializeField]float cooldown;
    [SerializeField]int coolDownBetweenEach;
    [SerializeField]int repeatAmount;

    List<Material> lampMaterials = new();
    
    List<Tween> currentTweens;

    Coroutine coroutine;
    [SerializeField]bool disableAfterCycle;
    [SerializeField]bool lampOffOnStart;

    private void Start() 
    {
        if(locateChildrenAsLampLights)
        {
            lampLights.Clear();
            for(int i = 0;i<transform.childCount;i++)
            {
                lampLights.Add(transform.GetChild(i).GetComponent<MeshRenderer>());  
            }
        }
        else
            lampLights.Add(gameObject.GetComponent<MeshRenderer>());
        if(overrideMaterial)
        {
            for(int i = 0;i<lampLights.Count;i++)
            {
                lampLights[i].material = lampMaterial;
            }
        }
        if(randomizeColors&& !overrideMaterial)
        {
            for(int i = 0;i<lampLights.Count;i++)
            {
                
                Material randomMaterial = new(lampLights[i].material);
                
                randomMaterial.EnableKeyword("_EMISSION");
                Color randomColor = GetRandomColor();
                randomMaterial.SetColor("_EmissionColor", randomColor);
                randomMaterial.color = randomColor;
                lampLights[i].material = randomMaterial;
            }
        }
        
        for(int i = 0;i< lampLights.Count;i++)
        {
            if(lampLights[i].material != lampOffMaterial)
            {
                lampMaterials.Add(lampLights[i].material);
            }
            else
            {
                lampMaterials.Add(lampMaterial);
            }  
            if(lampOffOnStart)  
                lampLights[i].material = lampOffMaterial;
        }

        if(playOnStart)
            EnableLamp();
    }
    [Button]
    public void EnableLamp()
    {
        if(coroutine != null)
            return;
        disableAfterCycle = false;
        switch (blinkingMode)
        {
            case BlinkingMode.Static:
                coroutine = StartCoroutine(StartStaticLamnp());
                break;
            case BlinkingMode.Blinking:
                coroutine = StartCoroutine(StartBlinkingLamp());
                break;
            case BlinkingMode.SequenceSimultaneously:
                coroutine = StartCoroutine(StartSequenceSimultaneouslyLamp());
                break;
            case BlinkingMode.SequenceSuccessively:
                coroutine = StartCoroutine(StartSequenceSuccessivelyLamp());
                break;
            case BlinkingMode.SequenceRandomNext:
                coroutine = StartCoroutine(StartSequenceRandomNextyLamp());
                break;
            case BlinkingMode.SequenceBuildUpBuildDown:
                coroutine = StartCoroutine(StartSequenceBuildUpBuildDownLamp());
                break;
            case BlinkingMode.Flicker:
                coroutine = StartCoroutine(StartFlickerLamp());
                break;
        }
        Debug.Log("enabled lamps: "+lampLights.Count);
    }
    private Color GetRandomColor()
    {
        float randomR = Random.Range(Mathf.Min(minColor.r, maxColor.r), Mathf.Max(minColor.r, maxColor.r));
        float randomG = Random.Range(Mathf.Min(minColor.g, maxColor.g), Mathf.Max(minColor.g, maxColor.g));
        float randomB = Random.Range(Mathf.Min(minColor.b, maxColor.b), Mathf.Max(minColor.b, maxColor.b));

        return new Color(randomR, randomG, randomB);
    }
    [Button]
    public void DisableLamp()
    {
        if(coroutine == null)
            return;
        disableAfterCycle = true;
    }
    [Button]
    public void DisableLampImmediately()
    {
        if(coroutine == null)
            return;

        StopCoroutine(coroutine);
        coroutine = null;
        
        for(int i = 0; i< lampLights.Count; i++)
        {
            lampLights[i].material = lampOffMaterial;
        }
    }
    IEnumerator StartStaticLamnp()
    {
        yield return new WaitForSeconds(enablingDelay);
        for(int i = 0; i< lampLights.Count; i++)
        {
            lampLights[i].material = lampMaterials[i];
        }
        yield return null;
    }
    IEnumerator StartBlinkingLamp()
    {
        yield return new WaitForSeconds(enablingDelay);
        while(true)
        {
            for(int i = 0; i< lampLights.Count; i++)
            {
                lampLights[i].material = lampMaterials[i];
            }
            yield return new WaitForSeconds(activeTime);
            for(int i = 0; i< lampLights.Count; i++)
            {
                lampLights[i].material = lampOffMaterial;
            }
            yield return new WaitForSeconds(inactiveTime);

            if(disableAfterCycle)
                DisableLampImmediately();
        }
    }
    IEnumerator StartSequenceSimultaneouslyLamp()
    {
        yield return new WaitForSeconds(enablingDelay);
        int amount = 0;
        while(true)
        {
            if(repeatAmount == -1)
                repeatAmount = int.MaxValue;
            while(amount <= repeatAmount)
            {
                for(int i = 0; i< lampLights.Count; i++)
                {
                    lampLights[i].material = lampMaterials[i];
                }
                yield return new WaitForSeconds(activeTime);
                for(int i = 0; i< lampLights.Count; i++)
                {
                    lampLights[i].material = lampOffMaterial;
                }
                yield return new WaitForSeconds(inactiveTime);
                amount++;

                if(disableAfterCycle)
                    DisableLampImmediately();
                yield return new WaitForSeconds(coolDownBetweenEach);
            }
            if(repeatAmount != int.MaxValue)
                DisableLampImmediately();
            yield return new WaitForSeconds(cooldown);
            amount = 0;
        }
    }
    IEnumerator StartSequenceSuccessivelyLamp()
    {
        yield return new WaitForSeconds(enablingDelay);
        int amount = 0;
        while(true)
        {
            if(repeatAmount == -1)
                repeatAmount = int.MaxValue;
            while(amount <= repeatAmount)
            {
                for(int i = 0; i<lampLights.Count; i++)
                {
                    if(i == 0)
                        lampLights[lampLights.Count-1].material = lampOffMaterial;
                    else
                        lampLights[i-1].material = lampOffMaterial;
                    lampLights[i].material = lampMaterials[i];
                    
                    yield return new WaitForSeconds(activeTime);
                    
                    lampLights[i].material = lampOffMaterial;;

                    yield return new WaitForSeconds(inactiveTime);
                }
                amount++;

                if(disableAfterCycle)
                    DisableLampImmediately();
                yield return new WaitForSeconds(coolDownBetweenEach);
            }
            if(repeatAmount != int.MaxValue)
                DisableLampImmediately();
            yield return new WaitForSeconds(cooldown);
            amount = 0;
        }
    }
    IEnumerator StartSequenceRandomNextyLamp()
    {
        yield return new WaitForSeconds(enablingDelay);
        int amount = 0;
        MeshRenderer lastLamp = new();
        while(true)
        {
            if(repeatAmount == -1)
                repeatAmount = int.MaxValue;
            while(amount <= repeatAmount)
            {
                for(int i = 0; i<lampLights.Count; i++)
                {
                    if(lastLamp != null)
                        lastLamp.material = lampOffMaterial;
                    lastLamp = lampLights[Random.Range(0,lampLights.Count-1)];
                    lastLamp.material = lampMaterials[i];
                    
                    yield return new WaitForSeconds(activeTime);
                    
                    lastLamp.material = lampOffMaterial;

                    yield return new WaitForSeconds(inactiveTime);
                }
                amount++;

                if(disableAfterCycle)
                    DisableLampImmediately();
                yield return new WaitForSeconds(coolDownBetweenEach);
            }
            if(repeatAmount != int.MaxValue)
                DisableLampImmediately();
            yield return new WaitForSeconds(cooldown);
            amount = 0;
        }
    }
    IEnumerator StartSequenceBuildUpBuildDownLamp()
    {
        yield return new WaitForSeconds(enablingDelay);
        int amount = 0;
        bool lampsOn = false;
        while(true)
        {
            if(repeatAmount == -1)
                repeatAmount = int.MaxValue;
            while(amount <= repeatAmount)
            {
                for(int i = 0; i<lampLights.Count; i++)
                {
                    if(lampsOn)
                    {
                        lampLights[i].material = lampOffMaterial;
                        yield return new WaitForSeconds(inactiveTime);
                    }
                    else
                    {
                        lampLights[i].material = lampMaterials[i];
                        yield return new WaitForSeconds(activeTime);
                    }
                }
                amount++;
                lampsOn = !lampsOn;

                if(disableAfterCycle)
                    DisableLampImmediately();
                yield return new WaitForSeconds(coolDownBetweenEach);
            }
            if(repeatAmount != int.MaxValue)
                DisableLampImmediately();
            yield return new WaitForSeconds(cooldown);
            amount = 0;
        }
    }
    IEnumerator StartFlickerLamp()
    {
        yield return new WaitForSeconds(enablingDelay);
        int amount = 0;
        float intensity = 0;
        while(true)
        {
            if(repeatAmount == -1)
                repeatAmount = int.MaxValue;
            for(int i = 0; i<lampLights.Count; i++)
            {
                lampLights[i].material = lampMaterials[i];
            }
            while(amount <= repeatAmount)
            {
                intensity = Random.Range(intensityRange.x,intensityRange.y);
                for(int i = 0; i<lampLights.Count; i++)
                {
                    lampLights[i].material.SetColor("_EmissionColor", lampLights[i].material.color * intensity);
                }
                yield return new WaitForSeconds(Random.Range(timeRangeForFlickerOnTime.x,timeRangeForFlickerOnTime.y));

                intensity = Random.Range(intensityRange.x,intensityRange.y);
        
                for(int i = 0; i<lampLights.Count; i++)
                {
                    lampLights[i].material.SetColor("_EmissionColor", lampLights[i].material.color * intensity);
                }

                yield return new WaitForSeconds(Random.Range(timeRangeBetweenEachFlicker.x,timeRangeBetweenEachFlicker.y));
                amount++;

                if(disableAfterCycle)
                    DisableLampImmediately();
                yield return new WaitForSeconds(coolDownBetweenEach);
            }
            if(repeatAmount != int.MaxValue)
                DisableLampImmediately();
            yield return new WaitForSeconds(cooldown);
            amount = 0;
        }
    }
}
