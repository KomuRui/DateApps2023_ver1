using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TalkMiniGameRandomStart : talkText
{
    [SerializeField] private GameObject mainMiniGameBoard;
    [SerializeField] private GameObject talkSignBorad;
    [SerializeField] private GameObject mc;

    //���ׂẲ�b�I�������Ƃ��̏���
    public override void AllTalkFinish() 
    {
        mainMiniGameBoard.transform.DOMoveY(-0.2f, 2.0f).SetEase(Ease.OutQuart);
        talkSignBorad.transform.DOMoveY(25, 2.0f).SetEase(Ease.OutQuart);
        mc.transform.DOMoveZ(30, 2.0f).SetEase(Ease.OutQuart);
    }
}
