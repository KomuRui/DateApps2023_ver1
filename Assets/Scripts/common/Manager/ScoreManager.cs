using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    //プレイヤースコア(key : プレイヤー番号)
    private static Dictionary<byte, int> score;

    //初期化
    public static void Initializ()
    {
        Dictionary<byte, int> player = new Dictionary<byte, int>();
    }
}
