using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class talkText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] public List<string> talk = new List<string>();
    [SerializeField] private float interval;       //�C���^�[�o��
    [SerializeField] private GameObject nextImage; //���ւ̉摜
    protected int nowLookTalkNum = 0;  //���݌��Ă����b�̗v�f�ԍ�
    protected int nowLookTextNum = 0;  //���݌��Ă��镶���̗v�f�ԍ�
    protected bool isTalkChangeWait = false;   //��b�ύX�ҋ@���邩
    protected Dictionary<string, bool> isNextTalk = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        //�~�܂�Ȃ��悤�ɂ���
        for(int i = 0; i < talk.Count; i++)
            isNextTalk[talk[i]] = true;

        ChildStart();
    }

    // Update is called once per frame
    void Update()
    {
        //������b�ύX�ҋ@����A�{�^���������ꂽ�̂Ȃ玟�̉�b��
        if (isTalkChangeWait && Input.GetButtonDown("Abutton1"))
        {
            nextImage.SetActive(false);
            NextTalk();
        }

        ChildUpdate();
    }
    
    //���̕����\��
    IEnumerator nextTextPrint(float delay)
    {
        yield return new WaitForSeconds(delay);
     
        //��b�I������̂Ȃ�
        if (nowLookTextNum >= talk[nowLookTalkNum].Length)
        {
            if (isNextTalk[talk[nowLookTalkNum]])
            {
                NextImageActive();
            }
            else
                TalkFinish();
        }
        else
            AddNextText();

    }

    //�e�L�X�g��ɂ���
    IEnumerator TextClear(float delay)
    {
        yield return new WaitForSeconds(delay);
        text.text = "";
    }

    //���̉�b��
    private void NextTalk()
    {
        //���ׂẲ�b�I��������
        if (nowLookTalkNum >= talk.Count)
        {
            isTalkChangeWait = false;
            AllTalkFinish();
            return;
        }

        isTalkChangeWait = false;
        nowLookTextNum = 0;
        StartCoroutine(TextClear(0));
        StartCoroutine(nextTextPrint(0));
    }

    //���̕����ǉ�
    private void AddNextText()
    {
        text.text += talk[nowLookTalkNum][nowLookTextNum];
        nowLookTextNum++;
        StartCoroutine(nextTextPrint(interval));
    }

    //���ւ̉摜���A�N�e�B�u��
    public void NextImageActive()
    {
        nowLookTalkNum++;
        nextImage.SetActive(true);
        isTalkChangeWait = true;
    }

    //�b���̃X�^�[�g
    public void StartTalk()
    {
        StartCoroutine(nextTextPrint(2));
    }

    //���ׂẲ�b�I�������Ƃ��̏���
    public virtual void AllTalkFinish() { }

    //�e�g�[�N���I�������Ƃ��ɌĂ΂��֐�(���̉�b�ɂ����Ȃ��Ɛݒ肵�Ă���ꍇ����)
    public virtual void TalkFinish() { }

    //�q���p�̃X�^�[�g
    public virtual void ChildStart() { }

    //�q���p�̍X�V
    public virtual void ChildUpdate() { }

}
