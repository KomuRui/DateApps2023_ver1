using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FinishAnimation : MonoBehaviour
{
    [SerializeField] private Image finish;
    [SerializeField] private Image finishBackGround;
    [SerializeField] private Vector3 initializScale;
    private Vector3 beforeScale;

    // Start is called before the first frame update
    void Start()
    {
        //大きさなど設定
        beforeScale = this.transform.localScale;
        this.transform.localScale = initializScale;

        //透明に
        Color c = finish.color;
        Color c1 = finishBackGround.color;
        c.a = 0.0f;
        c1.a = 0.0f;
        finish.color = c;
        finishBackGround.color = c1;

        //アニメーション
        this.transform.DOScale(beforeScale, 0.5f).SetEase(Ease.OutBack);
        finish.DOFade(1, 0.5f).SetEase(Ease.OutQuad);
        finishBackGround.DOFade(1, 0.5f).SetEase(Ease.OutQuad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
