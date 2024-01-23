using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class talkText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private List<string> talk;
    [SerializeField] private float interval;       //�C���^�[�o��
    [SerializeField] private GameObject nextImage; //���ւ̉摜
    private int nowLookTalkNum = 0;  //���݌��Ă����b�̗v�f�ԍ�
    private int nowLookTextNum = 0;  //���݌��Ă��镶���̗v�f�ԍ�
    private bool isTalkChangeWait = false;   //��b�ύX�ҋ@���邩

    // Start is called before the first frame update
    void Start()
    {
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
            nowLookTalkNum++;
            nextImage.SetActive(true);
            isTalkChangeWait = true;
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

    //�b���̃X�^�[�g
    public void StartTalk()
    {
        StartCoroutine(nextTextPrint(2));
    }

    //���ׂẲ�b�I�������Ƃ��̏���
    public virtual void AllTalkFinish() { }

    //�q���p�̃X�^�[�g
    public virtual void ChildStart() { }

    //�q���p�̍X�V
    public virtual void ChildUpdate() { }

}
