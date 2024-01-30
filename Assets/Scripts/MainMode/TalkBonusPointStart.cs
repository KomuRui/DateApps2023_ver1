using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBonusPointStart : talkText
{

    [SerializeField] private Vector3 bonusGenerationPos; //�{�[�i�X�����ʒu
    [SerializeField] private Vector3 bonusMoveInitializPos;  //�{�[�i�X�ړ��揉���ʒu
    [SerializeField] private List<Vector3> bonusPlayerMovePos; //�{�[�i�X�ړ��ʒu
    [SerializeField] private List<Vector3> playerPos;          //�v���C���[�ʒu
    [SerializeField] private GameObject bonusPoint;      //�{�[�i�X�|�C���g
    [SerializeField] private GameObject mc;              //�i��
    [SerializeField] private GameObject leftMetor;
    [SerializeField] private GameObject rightMetor;
    [SerializeField] private GameObject talkSignBorad;
    [SerializeField] private ScoreGenerationMetor scoreGenerationMetor;
    private GameObject generatipnBonusObj; //���������{�[�i�X�I�u�W�F

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
        isNextTalk[talk[4]] = false;
        isNextTalk[talk[5]] = false;
        isNextTalk[talk[6]] = false;
        isNextTalk[talk[7]] = false;
        isNextTalk[talk[8]] = false;
        isNextTalk[talk[9]] = false;
        isNextTalk[talk[10]] = false;
        isNextTalk[talk[12]] = false;

        //�g�[�N�X�^�[�g
        StartTalk();
    }

    //�e�g�[�N���I�������Ƃ��ɌĂ΂��֐�(���̉�b�ɂ����Ȃ��Ɛݒ肵�Ă���ꍇ����)
    public override void TalkFinish() 
    {
        //�����Ȃ�v���C���[�̂��Ƃֈړ�
        if (nowLookTalkNum == 12)
            StartCoroutine(AllPointAddStart(1.0f));
        else if (nowLookTalkNum % 2 == 0)
            StartCoroutine(BonusToPlayerMpve(1.0f));
        else
            //�|�C���g����
            StartCoroutine(BonusGeneration(1.0f));
    }

    //����
    IEnumerator BonusGeneration(float delay)
    {
        yield return new WaitForSeconds(delay);
        generatipnBonusObj = Instantiate(bonusPoint, bonusGenerationPos, Quaternion.identity);
        generatipnBonusObj.transform.localScale = Vector3.zero;
        generatipnBonusObj.transform.localEulerAngles = new Vector3(-90,0,0);
        generatipnBonusObj.transform.DOMove(bonusMoveInitializPos, 1.0f).SetEase(Ease.OutCubic);
        generatipnBonusObj.transform.DOScale(Vector3.one * 0.55f, 1.0f).SetEase(Ease.OutCubic).OnComplete(() => NextImageActive());
    }

    //�{�[�i�X���v���C���[�ֈړ�
    IEnumerator BonusToPlayerMpve(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        var sequence = DOTween.Sequence(); 
        sequence.Append(generatipnBonusObj.transform.DOMove(bonusPlayerMovePos[0], 1.0f).SetEase(Ease.OutCubic))
            �@�@.AppendInterval(0.5f)
            �@�@.Append(generatipnBonusObj.transform.DOMove(playerPos[0], 1.0f).SetEase(Ease.OutCubic).OnComplete(() => NextImageActive()))
                .Join(generatipnBonusObj.transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.OutCubic));
    }

    //�|�C���g���Z�J�n
    IEnumerator AllPointAddStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�A�j���[�V����
        leftMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        rightMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        talkSignBorad.transform.DOMoveY(25, 2.0f).SetEase(Ease.OutQuart);
        mc.transform.DOMoveZ(30, 2.0f).SetEase(Ease.OutQuart);

        //�|�C���g������
        for(int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            scoreGenerationMetor.ScoreMetorSet(i, 0, ScoreManager.GetScore((byte)(i + 1)));

        //�|�C���g���Z�X�^�[�g
        scoreGenerationMetor.GenerationStart();
    }
}
