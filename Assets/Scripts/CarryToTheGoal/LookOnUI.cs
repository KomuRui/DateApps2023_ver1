using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LookOnUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(0.055f, 0.5f).SetLoops(-1,LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
