using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : GimmickBase
{
    private float anchorRotateLimit = 89;
    private bool isReverse = false; //動きが反対になっているかどうか

    //特定のアクションを起こす
    public override void Action()
    {
        ReverseMovement();
    }

    public override void GimmickStart()
    {
    }

    public override void GimmickUpdate()
    {
    }

    //動きを反対にする
    public void ReverseMovement()
    {
        //通常の動き
        if (!isReverse)
        {
            transform.DORotate(new Vector3(0, 0, anchorRotateLimit), 2f).SetEase(Ease.OutQuint).OnComplete(ReverseMovement);
        }
        //逆の動き
        else
        {
            transform.DORotate(new Vector3(0, 0, -anchorRotateLimit), 2f).SetEase(Ease.OutQuint).OnComplete(ReverseMovement);
        }
        isReverse = !isReverse;
    }
}
