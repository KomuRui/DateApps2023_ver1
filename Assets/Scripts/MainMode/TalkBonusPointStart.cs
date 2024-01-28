using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBonusPointStart : talkText
{

    [SerializeField] private Vector3 bonusGenerationPos; //�{�[�i�X�����ʒu
    [SerializeField] private List<Vector3> bonusMovePos; //�{�[�i�X�ړ��ʒu
    [SerializeField] private GameObject bonusPoint;      //�{�[�i�X�|�C���g
    [SerializeField] private GameObject mc;              //�i��

    //�q���p�̃X�^�[�g
    public override void ChildStart() 
    {
        isNextTalk.Clear();

        talk.Add("sajisjaisjiajsiajisajijsaij0");  //���āA�F����A�����ŃT�v���C�Y�ł��I
        talk.Add("sajisjaisjiajsiajisajijsaij1");  //�Ȃ�ƁA�Ō�ɏ��s��傫�����E���邩������Ȃ��{�[�i�X�|�C���g��p�ӂ��Ă��܂��I
        talk.Add("sajisjaisjiajsiajisajijsaij2");  //1���Љ�Ă����܂��I
        talk.Add("sajisjaisjiajsiajisajijsaij3");  //1�ڂ́��������̃{�[�i�X�|�C���g���l�������v���C���[�́A�A�A
        talk.Add("sajisjaisjiajsiajisajijsaij4");  //��P�ł��I�I
        talk.Add("sajisjaisjiajsiajisajijsaij5");  //2�ڂ́��������̃{�[�i�X�|�C���g���l�������v���C���[�́A�A�A
        talk.Add("sajisjaisjiajsiajisajijsaij6");  //��P�ł��I�I
        talk.Add("sajisjaisjiajsiajisajijsaij7");  //3�ڂ́��������̃{�[�i�X�|�C���g���l�������v���C���[�́A�A�A
        talk.Add("sajisjaisjiajsiajisajijsaij8");  //��P�ł��I�I
        talk.Add("sajisjaisjiajsiajisajijsaij9");  //4�ڂ́��������̃{�[�i�X�|�C���g���l�������v���C���[�́A�A�A
        talk.Add("sajisjaisjiajsiajisajijsaij10"); //��P�ł��I�I
        talk.Add("sajisjaisjiajsiajisajijsaij11"); //���āA�����ōŏI�I�Ȍ��ʂ𔭕\���܂��I
        talk.Add("sajisjaisjiajsiajisajijsaij12"); //�{�[�i�X�|�C���g�����Z���A�ŏI�I�ȏ��҂ƂȂ����̂�....�I

        //�~�܂�Ȃ��悤�ɂ���
        for (int i = 0; i < talk.Count; i++)
            isNextTalk[talk[i]] = true;

        //�g�[�N���I������^�C�~���O�ŉ��o�����݂�������false�ɂ���
        isNextTalk[talk[3]] = false;
        isNextTalk[talk[5]] = false;
        isNextTalk[talk[7]] = false;
        isNextTalk[talk[9]] = false;

        //�g�[�N�X�^�[�g
        StartTalk();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�e�g�[�N���I�������Ƃ��ɌĂ΂��֐�(���̉�b�ɂ����Ȃ��Ɛݒ肵�Ă���ꍇ����)
    public override void TalkFinish() 
    {
        //�|�C���g����
        GameObject point = Instantiate(bonusPoint, bonusGenerationPos, Quaternion.identity);
        point.transform.localScale = Vector3.zero;
        point.transform.DOMove(bonusMovePos[0], 1.0f).SetEase(Ease.OutCubic);
        point.transform.DOScale(Vector3.one * 0.55f, 1.0f).SetEase(Ease.OutCubic);
    }
}
