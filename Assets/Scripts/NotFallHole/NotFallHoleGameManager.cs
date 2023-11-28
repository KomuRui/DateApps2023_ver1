using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotFallHoleGameManager : MiniGameManager
{
    //°‚ÌŠJ‚¢‚Ä‚é”
    public int openCount;
    public int kill = 0;

    public override void SceneStart()
    {
        openCount = 0;
    }

    public override void MiniGameUpdate()
    {
    }

    public int GetCount() { return openCount; }

    public bool AddCount() 
    {
        if (openCount < 6)
        {
            openCount++;
            return true;
        }
        else
            return false;
    }

    public void MinusCount() { openCount--; openCount = Math.Max(0, openCount); }
}
