using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Floor : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI[] timeTextMeshPro; //制限時間テキスト
    [SerializeField] private float flashingTime;                //点滅時間

    //現在制限時間
    private float time =10.0f;

    //スピード倍率
    private float speedRatio = 1.0f;

    //当たっているプレイヤーを保管
    private List<GameObject> hitPlayer = new List<GameObject>();

    //Dotween用
    private Tweener tweener;

    //揺らしているか
    private bool isShake = false;

    //赤色に変えるか
    private bool isChangeRedColor = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = "10";
    }

    // Update is called once per frame
    void Update()
    {
        //時間計算・表示
        TimeCalcPrint();

        //制限時間が来たら揺らす
        Shake();

        //色更新
        ColorUpdate();
    }

    //時間計算・表示
    private void TimeCalcPrint()
    {
        time -= Time.deltaTime * (speedRatio * hitPlayer.Count);
        time = Mathf.Max(time, 0);
        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = ((int)time).ToString();
    }

    //揺らす
    private void Shake()
    {
        if (time != 0 || isShake) return;

        //点滅止める
        tweener.Restart();
        tweener.Pause();

        //赤色にしておく
        GetComponent<MeshRenderer>().material.color = Color.red;

        //揺らす
        transform.DOShakePosition(1f, 1f, 5, 1, false, true);
        isShake = true;

        //落とす
        StartCoroutine(Drop(1.0f));
    }

    //更新
    private void ColorUpdate()
    {
        if (((int)time) >= 4 || isChangeRedColor) return;

        //文字を赤色
        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].color = Color.red;

        //メッシュレンダラーを取得(点滅)
        MeshRenderer r = GetComponent<MeshRenderer>();
        tweener = r.material.DOColor(Color.red, flashingTime).SetLoops(-1, LoopType.Yoyo);

        isChangeRedColor = true;
    }

    //落とす
    IEnumerator Drop(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.DOMove(new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), 1f).SetEase(Ease.InOutQuart);

        //元に戻す
        StartCoroutine(Undo(4.0f));
    }

    //元に戻す
    IEnumerator Undo(float delay)
    {
        yield return new WaitForSeconds(delay);

        //白色にしておく
        GetComponent<MeshRenderer>().material.color = Color.white;
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), 1f).SetEase(Ease.InOutQuart);

        //初期化
        StartCoroutine(Initializ(1.0f));
    }


    //初期化
    IEnumerator Initializ(float delay)
    {
        yield return new WaitForSeconds(delay);

        time = 10.0f;
        isShake = false;
        isChangeRedColor = false;

        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].color = Color.white;
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
