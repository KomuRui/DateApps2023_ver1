using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class Fade : MonoBehaviour
{
    IFade fade;

    void Start()
    {
        
    }

    float cutoutRange;

    void Init()
    {
        fade = GetComponent<IFade>();
    }

    void OnValidate()
    {
        Init();
        fade.Range = cutoutRange;
    }

    IEnumerator FadeoutCoroutine(float time, System.Action action)
    {
        float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);

        var endFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
        {
            cutoutRange = (endTime - Time.timeSinceLevelLoad) / time;
            fade.Range = cutoutRange;
            yield return endFrame;
        }
        cutoutRange = 0;
        fade.Range = cutoutRange;

        if (action != null)
        {
            action();
        }
    }

    IEnumerator FadeinCoroutine(float time, System.Action action)
    {
        float endTime = Time.timeSinceLevelLoad + time * (1 - cutoutRange);

        var endFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
        {
            cutoutRange = 1 - ((endTime - Time.timeSinceLevelLoad) / time);
            fade.Range = cutoutRange;
            yield return endFrame;
        }
        cutoutRange = 1;
        fade.Range = cutoutRange;

        if (action != null)
        {
            action();
        }
    }

    public Coroutine FadeOut(float time, System.Action action)
    {

        StopAllCoroutines();
        return StartCoroutine(FadeoutCoroutine(time, action));
    }

    public Coroutine FadeOut(float time)
    {
        Init();
        cutoutRange = 1;
        fade.Range = cutoutRange;
        return FadeOut(time, null);
    }

    public Coroutine FadeIn(float time, System.Action action)
    {
        StopAllCoroutines();
        return StartCoroutine(FadeinCoroutine(time, action));
    }

    public Coroutine FadeIn(float time)
    {
        Init();
        fade.Range = cutoutRange;
        return FadeIn(time, null);
    }
}