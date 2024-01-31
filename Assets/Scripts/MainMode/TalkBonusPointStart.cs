using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    [SerializeField] private StageSelect stageSelectInfo;

    [SerializeField] private List<Transform> resultTrans = new List<Transform>();

    [SerializeField] private List<TextMeshProUGUI> resultTextList = new List<TextMeshProUGUI>();


    private GameObject generatipnBonusObj; //���������{�[�i�X�I�u�W�F
    private bool isTalkFinish = false;


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
        isNextTalk[talk[3]] = true;
        isNextTalk[talk[4]] = true;
        isNextTalk[talk[5]] = true;
        isNextTalk[talk[6]] = true;
        isNextTalk[talk[7]] = true;
        isNextTalk[talk[8]] = true;
        isNextTalk[talk[9]] = true;
        isNextTalk[talk[10]] = true;
        isNextTalk[talk[12]] = false;

        //�g�[�N�X�^�[�g
        StartTalk();
    }

    //�q���p��Update
    public override void ChildUpdate()
    {
        //��b���I�����ĂȂ���ΏI���
        if (!isTalkFinish) return;

        //�{�^���������Ă��Ȃ�������I���
        if (!Input.GetButtonDown("Abutton1")) return;

            //�t�F�[�h����񂠂�̂Ȃ�
            if (stageSelectInfo.GetFade())
            stageSelectInfo.GetFade().FadeOut(1.0f);

        //�V�[���ύX
        StartCoroutine(ChangeScene("ModeSelect", 1f));
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

        ////�A�j���[�V����
        //leftMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        //rightMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        //talkSignBorad.transform.DOMoveY(25, 2.0f).SetEase(Ease.OutQuart);
        //mc.transform.DOMoveZ(30, 2.0f).SetEase(Ease.OutQuart);

        ////�|�C���g������
        //for(int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        //    scoreGenerationMetor.ScoreMetorSet(i, 0, ScoreManager.GetScore((byte)(i + 1)));

        ////�|�C���g���Z�X�^�[�g
        //scoreGenerationMetor.GenerationStart();

        //�ŏI���\�̃A�j���[�V����
        ResultAnimation();
    }

    //�ŏI���\�̃A�j���[�V����
    void ResultAnimation()
    {
        //���ʍČv�Z
        ScoreManager.ReCalcRank();

        //1�ʂ̃v���C���[���擾
        List<byte> topPlayerList = ScoreManager.GetNominatePlayerRank(1);

        int val = Factorial(topPlayerList.Count) - topPlayerList.Count;

        int kaisu = 0;
        //�A�j���[�V����
        foreach (byte playerNum in topPlayerList)
        {
            //���݂̊p�x��ۑ�
            Vector3 tmp = stageSelectInfo.playerList[playerNum - 1].transform.localEulerAngles;

            //���U���g�̈ʒu������
            stageSelectInfo.playerList[playerNum - 1].transform.LookAt(
                new Vector3 (resultTrans[val + kaisu].position.x, stageSelectInfo.playerList[playerNum - 1].transform.position.y, resultTrans[val + kaisu].position.x));

            //�p�x��ۑ�
            Vector3 goalTrans = stageSelectInfo.playerList[playerNum - 1].transform.localEulerAngles;

            //�ŏ��̊p�x�ɖ߂�
            stageSelectInfo.playerList[playerNum - 1].transform.localEulerAngles = tmp;

            //��]������Ɉړ�����
            stageSelectInfo.playerList[playerNum - 1].transform.DOLocalRotate(new Vector3(goalTrans.x, goalTrans.y, goalTrans.z), 1f).SetEase(Ease.OutQuad);
            StartCoroutine(ResultAnimationMove((byte)(playerNum - 1), resultTrans[val + kaisu].position, 1f));

            kaisu++;
        }
    }

    //���ʔ��\�̉�]�A�j���[�V����
    void ResultAnimationRotate(byte num)
    {
            stageSelectInfo.playerList[num].transform.DOLocalRotate(new Vector3(0, 180, 0), 1f).SetEase(Ease.OutQuad).OnComplete(() => ResultAnimationText());
    }

    //���ʔ��\�̈ړ��A�j���[�V����
    IEnumerator ResultAnimationMove(byte num, Vector3 targetPos, float delay)
    {
        yield return new WaitForSeconds(delay);
        //�ړ��A�j���[�V�����̌�ɉ�]
        stageSelectInfo.playerList[num].transform.DOMove(targetPos, 2.0f).SetEase(Ease.OutCubic).OnComplete(() => ResultAnimationRotate(num));
    }
    
    public int Factorial(int n)
    {
        if (n == 0)
            return 0;
        return n + Factorial(n - 1);
    }

    //���ʔ��\�̃e�L�X�g�A�j���[�V����
    void ResultAnimationText()
    {
        //1�ʂ̃v���C���[���擾
        List<byte> topPlayerList = ScoreManager.GetNominatePlayerRank(1);

        //�������v���C���[��\��
        string winPlayerText = "";
        for (int i = 0; i < topPlayerList.Count; i++)
        {
            //�������v���C���[��1�l����Ȃ�������
            if (i != 0)
                winPlayerText += "&";

            winPlayerText += topPlayerList[i] + "P";
        }
        resultTextList[0].SetText(winPlayerText);
        resultTextList[1].SetText("Win!");

        //�g��
        Vector3 afterScale = transform.localScale + new Vector3(0.45f, 0.45f, 0.45f);
        resultTextList[1].transform.DOScale(afterScale, 1f).SetEase(Ease.OutBounce).OnComplete(() => SetIsTalkFinish(true, 1f));

        isTalkFinish = true;
    }

    //�V�[���ύX
    public IEnumerator ChangeScene(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    public IEnumerator SetIsTalkFinish(bool a, float delay)
    {
        yield return new WaitForSeconds(delay);
        isTalkFinish = a;
    }
}
