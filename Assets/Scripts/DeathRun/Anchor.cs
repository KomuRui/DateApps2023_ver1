using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : GimmickBase
{
    private float anchorRotateLimit = 179;
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

    //��]�����ɖ߂�
    public void ReturnRotate()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutQuad).OnComplete(ReverseMovement);
    }

    //�����𔽑΂ɂ���
    public void ReverseMovement()
    {
        //�ʏ�̓���
        if (!isReverse)
        {
            transform.DOLocalRotate(new Vector3(0, 0, 360), 2f).SetEase(Ease.OutQuad).OnComplete(ReverseMovement);
        }
        //�t�̓���
        else
        {
            transform.DOLocalRotate(new Vector3(0, 0, -anchorRotateLimit), 2f).SetEase(Ease.OutQuad).OnComplete(ReverseMovement);
        }
        isReverse = !isReverse;
    }
}
