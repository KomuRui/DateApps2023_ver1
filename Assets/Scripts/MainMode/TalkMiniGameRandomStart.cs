using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkMiniGameRandomStart : talkText
{
    //���ׂẲ�b�I�������Ƃ��̏���
    public override void AllTalkFinish() 
    {
        SceneManager.LoadScene("Title");
    }
}
