using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Floor : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI[] timeTextMeshPro; //制限時間テキスト

    //現在制限時間
    private float time = 30.0f;

    //スピード倍率
    private float speedRatio = 1.0f;

    //当たっているプレイヤーを保管
    private List<GameObject> hitPlayer = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = "30";
    }

    // Update is called once per frame
    void Update()
    {
        //時間計算・表示
        time -= Time.deltaTime * (speedRatio * hitPlayer.Count);
        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = ((int)time).ToString();

    }

    // コリジョンが発生したときに呼び出されるメソッド
    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーではないのならこの先処理しない
        if (!collision.gameObject.CompareTag("Player")) return;

        //すでに同じプレイヤーがいないかチェック
        for (int i = 0; i < hitPlayer.Count; i++)
            if (hitPlayer[i] == collision.gameObject) return;

        //追加
        hitPlayer.Add(collision.gameObject);
    }

    //コリジョンが離れた時に呼ばれる関数
    private void OnCollisionExit(Collision collision)
    {
        //プレイヤーではないのならこの先処理しない
        if (!collision.gameObject.CompareTag("Player")) return;

        //同じプレイヤーがいないかチェック
        for (int i = 0; i < hitPlayer.Count; i++)
        {
            if (hitPlayer[i] == collision.gameObject)
            {
                //削除
                hitPlayer.Remove(collision.gameObject);
                return;
            }
        }
       
    }
}
