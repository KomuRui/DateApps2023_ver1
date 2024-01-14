using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkMiniGameRandomStart : talkText
{
    //‚·‚×‚Ä‚Ì‰ï˜bI—¹‚µ‚½‚Æ‚«‚Ìˆ—
    public override void AllTalkFinish() 
    {
        SceneManager.LoadScene("Title");
    }
}
