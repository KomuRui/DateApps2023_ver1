using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkPointStart : talkText
{
    [SerializeField] private GameObject talkSignBorad;
    [SerializeField] private GameObject mc;
    [SerializeField] private ScoreGenerationMetor scoreGenerationMetor;
    [SerializeField] private GameObject leftMetor;
    [SerializeField] private GameObject rightMetor;
    [SerializeField] private GameObject nextTalk;
    private bool isReturnPos = false;

    //�q���p�̃X�^�[�g
    public override void ChildStart() 
    { 
         StartTalk();
    }

    //�q���p�̍X�V
    public override void ChildUpdate()
    {
        if (scoreGenerationMetor.IsGeneratioonFinish() && !isReturnPos)
            ReturnPosGameObject();
    }

    //���ׂẲ�b�I�������Ƃ��̏���
    public override void AllTalkFinish()
    {
        //�A�j���[�V����
        leftMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        rightMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        talkSignBorad.transform.DOMoveY(25, 2.0f).SetEase(Ease.OutQuart);
        mc.transform.DOMoveZ(30, 2.0f).SetEase(Ease.OutQuart);

        //�|�C���g���Z�X�^�[�g
        scoreGenerationMetor.GenerationStart();
    }

    //���̈ʒu�ɖ߂�
    private void ReturnPosGameObject()
    {
        isReturnPos = true;
        this.gameObject.SetActive(false);
        leftMetor.transform.DOLocalMoveX(-20, 2.0f).SetEase(Ease.OutQuart);
        rightMetor.transform.DOLocalMoveX(20, 2.0f).SetEase(Ease.OutQuart);
        talkSignBorad.transform.DOMoveY(0.8f, 2.0f).SetEase(Ease.OutQuart);
        mc.transform.DOMoveZ(15.6f, 2.0f).SetEase(Ease.OutQuart).OnComplete(TalkStart);
    }

    //�b�X�^�[�g
    private void TalkStart()
    {
        nextTalk.SetActive(true);
        nextTalk.GetComponent<talkText>().StartTalk();
    }
}
