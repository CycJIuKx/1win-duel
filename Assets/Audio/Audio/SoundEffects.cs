using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public enum Effects {none, echo, lowPass, slow };
    static SoundEffects instance;
    void Start()
    {
        instance = this;

    }
    [ContextMenu("Lowpass test")]
    public void testAllPass()
    {
        SoundEffects.AddEffectsAllAudio(SoundEffects.Effects.lowPass, 1,0.3f);
       // SoundEffects.AddEffectsAllAudio(SoundEffects.Effects.echo, 2, 0.3f);
    }

    public static void AddEffectsAllAudio(Effects e, float time, float valueFactor = 1, float innerSpeedFactor = 1)
    {
        foreach (var item in GameObject.FindObjectsOfType<AudioSource>())
        {
            instance.StartCoroutine(instance.addCor(item.gameObject, e, time, valueFactor, innerSpeedFactor));
        }
       
    }
    public static void Add(GameObject target, Effects e, float time, float valueFactor=1, float innerSpeedFactor = 1)
    {
        instance.StartCoroutine(instance.addCor(target, e, time,valueFactor,innerSpeedFactor));
    }
    IEnumerator addCor(GameObject target, Effects e, float time, float factor, float innerSpeedFactor )
    {

        if (e == Effects.echo)
        {
            if (target.GetComponent<AudioEchoFilter>()) yield break;
            var echo = target.AddComponent<AudioEchoFilter>();
            echo.dryMix = 0.4f;
            echo.decayRatio = 0.25f;
            echo.delay = 600;
            yield return new WaitForSecondsRealtime(time);
            while (true)
            {
                if (target)
                {
                    echo.wetMix = Mathf.MoveTowards(echo.wetMix, 0, Time.unscaledDeltaTime / 2*innerSpeedFactor);
                    // echo.wetMix -= Time.unscaledDeltaTime / 2;

                    if (echo.wetMix == 0) break;
                    yield return null;
                }
                else break;
            }
            if (target) Destroy(target.GetComponent<AudioEchoFilter>());

        }
        if (e == Effects.lowPass)
        {
            if (target.GetComponent<AudioLowPassFilter>()) yield break;
            var echo = target.AddComponent<AudioLowPassFilter>();
            echo.lowpassResonanceQ = 3*factor;
            echo.cutoffFrequency = 20000;
            while (true)
            {
                if (target)
                {
                    echo.cutoffFrequency = Mathf.MoveTowards(echo.cutoffFrequency, 800, Time.unscaledDeltaTime * 55000 * innerSpeedFactor);
                    if (echo.cutoffFrequency == 800) break;
                    yield return null;
                }
                else break;
            }
            yield return new WaitForSecondsRealtime(time);
            while (true)
            {
                if (target)
                {
                    echo.cutoffFrequency = Mathf.MoveTowards(echo.cutoffFrequency, 22000, Time.unscaledDeltaTime * 7000);
                    if (echo.cutoffFrequency == 22000) break;
                    yield return null;
                }
                else break;
            }
            if (target)
            {
                Destroy(target.GetComponent<AudioLowPassFilter>());
            }
            

        }
        if (e == Effects.slow)
        {
            var echo = target.GetComponent<AudioSource>();
            if (echo.pitch != 1) yield break;
            while (true)
            {
                if (target)
                {
                    echo.pitch = Mathf.MoveTowards(echo.pitch, factor/2, Time.unscaledDeltaTime / 2 * innerSpeedFactor);
                    // echo.wetMix -= Time.unscaledDeltaTime / 2;

                    if (echo.pitch == factor/2) break;
                    yield return null;
                }
                else break;
            }
            yield return new WaitForSecondsRealtime(time);
            while (true)
            {
                if (target)
                {
                    echo.pitch = Mathf.MoveTowards(echo.pitch, 1, Time.unscaledDeltaTime / 2);
                    // echo.wetMix -= Time.unscaledDeltaTime / 2;

                    if (echo.pitch == 1) break;
                    yield return null;
                }
                else break;
            }
            if (target) Destroy(target.GetComponent<AudioEchoFilter>());

        }

    }
}
