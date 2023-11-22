using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagUpGameManager : MiniGameManager
{
    public bool isAloneTurn;
    public bool isStop;
    public int roundNum;

    // Start is called before the first frame update
    public override void SceneStart()
    {
        isAloneTurn = true;
        isStop = false;
        roundNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
