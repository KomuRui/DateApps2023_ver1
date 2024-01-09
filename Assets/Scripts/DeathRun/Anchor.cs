using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : GimmickBase
{
    private float anchorRotateLimit = 179;
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

    //回転を元に戻す
    public void ReturnRotate()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutQuad).OnComplete(ReverseMovement);
    }

    //動きを反対にする
    public void ReverseMovement()
    {
        //通常の動き
        if (!isReverse)
        {
            transform.DOLocalRotate(new Vector3(0, 0, 360), 2f).SetEase(Ease.OutQuad).OnComplete(ReverseMovement);
        }
        //逆の動き
        else
        {
            transform.DOLocalRotate(new Vector3(0, 0, -anchorRotateLimit), 2f).SetEase(Ease.OutQuad).OnComplete(ReverseMovement);
        }
        isReverse = !isReverse;
    }
}
