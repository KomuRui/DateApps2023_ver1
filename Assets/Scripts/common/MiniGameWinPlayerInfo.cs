using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameWinPlayerInfo : MonoBehaviour
{

    //カメラ
    public Vector3 winThreeCameraPos;
    public Vector3 winThreeCameraRotate;
    public Vector3 winOneCameraPos;
    public Vector3 winOneCameraRotate;

    //プレイヤーの位置など
    public Vector3 winOnePlayerPos;
    public Vector3 winOnePlayerRotate;
    public Vector3 winOnePlayerScale;
    public List<Vector3> winThreePlayerPos = new List<Vector3>();
    public List<Vector3> winThreePlayerRotate = new List<Vector3>();
    public List<Vector3> winThreePlayerScale = new List<Vector3>();

    //専用のフェード
    public Fade fade;

    //勝利プレイヤー用のキャンバス
    public GameObject winPlayerCanves;

    //エフェクト
    public List<GameObject> effeOne;
    public List<GameObject> effeThree;

    //生成したオブジェクト保存用
    private GameObject winOneObj;
    private List<GameObject> winThreeObj = new List<GameObject>();

    //どっちが勝ったか
    private bool isWinOne;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //フェードイン
    public void FadeIn(bool isWinOnePlayer)
    {
        fade.FadeIn(2.0f);
        StartCoroutine(FadeOut(2.0f));
        StartCoroutine(WinPlayerDirecting(2.0f, isWinOnePlayer));

        //不要なUIを削除
        foreach (var obj in GameManager.nowMiniGameManager.killCanvas)
            obj.SetActive(false);
    }

    //フェードアウト
    IEnumerator FadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.nowMiniGameManager.endText.SetActive(false);
        fade.FadeOut(2.0f);
        StartCoroutine(WinPlayerCanvasGeneration(2.0f));

        if (GameManager.isSubMode)
            StartCoroutine(SubModeLoad(6.0f));
        else
            StartCoroutine(RankResult(6.0f));
    }

    //ランク発表移行
    IEnumerator RankResult(float delay)
    {
        yield return new WaitForSeconds(delay);
        fade.FadeIn(2.0f);
        StartCoroutine(RankResultPlayerGeneration(2.0f));
        StartCoroutine(WinPlayerDelete(2.0f));

    }

    //順位発表のプレイヤー生成
    IEnumerator RankResultPlayerGeneration(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.nowMiniGameManager.ChangeRankAnnouncement();
        fade.FadeOut(2.0f);
    }

    //勝利UI生成
    IEnumerator WinPlayerCanvasGeneration(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.nowMiniGameManager.endText = Instantiate(winPlayerCanves, new Vector3(0, 0, 0), Quaternion.identity);

        //1人側が勝ったのなら
        if (isWinOne)
        {
            for(int i = 0; i < effeOne.Count; i++)
            {
                effeOne[i].SetActive(true);
                effeOne[i].GetComponent<ParticleSystem>().Play();
            }

            winOneObj.GetComponent<MiniGameWinPlayer>().WinAnimation();
        }
        else
        {
            for (int i = 0; i < effeThree.Count; i++)
            {
                effeThree[i].SetActive(true);
                effeThree[i].GetComponent<ParticleSystem>().Play();
            }

            for (int i = 0; i < winThreeObj.Count; i++)
                winThreeObj[i].GetComponent<MiniGameWinPlayer>().WinAnimation();
        }
    }

    //勝利プレイヤーの演出
    IEnumerator WinPlayerDirecting(float delay,bool isWinOnePlayer)
    {
        yield return new WaitForSeconds(delay);
        MiniGameManager g = GameManager.nowMiniGameManager;
        isWinOne = isWinOnePlayer;

        //1人側が買ったのなら
        if (isWinOnePlayer)
        {
            //生成
            GameObject obj = (GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(g.onePlayer));
            obj = Instantiate(obj, winOnePlayerPos, Quaternion.identity);
            obj.transform.position = winOnePlayerPos;
            obj.transform.localScale = winOnePlayerScale;
            obj.transform.localEulerAngles = winOnePlayerRotate;
            winOneObj = obj;

            //カメラのもろもろ変更
            Camera.main.transform.position = winOneCameraPos;
            Camera.main.transform.localEulerAngles = winOneCameraRotate;
        }
        else
        {
            //新たにプレイヤー召喚
            int i = 0;
            foreach (var rank in g.threePlayer)
            {
                GameObject obj = (GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(rank.Key));
                obj = Instantiate(obj, winThreePlayerPos[i], Quaternion.identity);
                obj.transform.position = winThreePlayerPos[i];
                obj.transform.localScale = winThreePlayerScale[i];
                obj.transform.localEulerAngles = winThreePlayerRotate[i];
                winThreeObj.Add(obj);
                i++;
            }

            //カメラのもろもろ変更
            Camera.main.transform.position = winThreeCameraPos;
            Camera.main.transform.localEulerAngles = winThreeCameraRotate;
        }

        //プレイヤー削除
        g.PlayerAllDelete();

        //finishの文字を削除
        GameManager.nowMiniGameManager.endText.SetActive(false);
    }


    //勝利プレイヤー削除
    IEnumerator WinPlayerDelete(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isWinOne)
        {
            Destroy(winOneObj.gameObject);
        }
        else
        {
            for(int i = 0; i < winThreeObj.Count; i++)
            {
                Destroy(winThreeObj[i].gameObject);
            }
        }
    }

    IEnumerator SubModeLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("SubMode");
    }
}
