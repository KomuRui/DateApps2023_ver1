using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : GimmickBase
{
    private float anchorRotateLimit = 89;
    private bool isReverse = false; //���������΂ɂȂ��Ă��邩�ǂ���

    //����̃A�N�V�������N����
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

    //�����𔽑΂ɂ���
    public void ReverseMovement()
    {
        //�ʏ�̓���
        if (!isReverse)
        {
            transform.DORotate(new Vector3(0, 0, anchorRotateLimit), 2f).SetEase(Ease.OutQuint).OnComplete(ReverseMovement);
        }
        //�t�̓���
        else
        {
            transform.DORotate(new Vector3(0, 0, -anchorRotateLimit), 2f).SetEase(Ease.OutQuint).OnComplete(ReverseMovement);
        }
        isReverse = !isReverse;
    }
}
