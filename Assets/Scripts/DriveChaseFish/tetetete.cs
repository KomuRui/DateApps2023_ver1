using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tetetete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetPoolMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ƒS[ƒ‹‚ÉŒü‚©‚¤“®‚«‚ÉÝ’è
    public void SetPoolMove()
    {
        // ’†“_‚ð‹‚ß‚é
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos;
        endPos.x += 10;
        endPos.y = startPos.y;
        Vector3 half = endPos - startPos * 0.50f + startPos;
        half.y += Vector3.up.y + 15;
        StartCoroutine(LerpThrow(this.gameObject, startPos, half, endPos, 4500));
    }

    IEnumerator LerpThrow(GameObject target, Vector3 start, Vector3 half, Vector3 end, float duration)
    {
        float startTime = Time.timeSinceLevelLoad;
        float rate = 0f;
        while (true)
        {
            if (rate >= 1.0f)
                yield break;

            float diff = Time.timeSinceLevelLoad - startTime;
            rate = diff / (duration / 60f);
            target.transform.position = CalcLerpPoint(start, half, end, rate);

            yield return null;
        }
    }

    Vector3 CalcLerpPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        var a = Vector3.Lerp(p0, p1, t);
        var b = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(a, b, t);
    }
}
