using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkMiniGameRandomStart : talkText
{
    //すべての会話終了したときの処理
    public override void AllTalkFinish() 
    {
        SceneManager.LoadScene("Title");
    }
}
