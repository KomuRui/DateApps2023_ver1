using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private List<Vector3> playerPosition = new List<Vector3>();
    [SerializeField] private List<Vector3> playerRotation = new List<Vector3>();
    [SerializeField] private List<Vector3> playerScale = new List<Vector3>();
    [SerializeField] private Fade fade;
    [SerializeField] private float fadeTime;
    [SerializeField] private  List<GameObject> stageImageObj;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject rankText;   
    [SerializeField] private GameObject mainModeWinPlayerCanvas;

    private bool isResultFinish;

    // Start is called before the first frame update
    void Start()
    {
        isResultFinish = false;

        PlayerManager.Initializ();

        //プレイヤー生成
        PlayerInstantiate();

        //ラウンド全て終了しているのなら
        if (StageSelectManager.isMainModeFinish)
        {
            roundText.text = "";
            mainModeWinPlayerCanvas = Instantiate(mainModeWinPlayerCanvas, new Vector3(0, 0, 0), Quaternion.identity);

            //1位のプレイヤを取得
            List<byte> a = ScoreManager.GetNominatePlayerRank(1);
            string text = "Win\n";
            for (int i = 0; i < a.Count; i++)
                text += a[i] + "P " + ScoreManager.GetScore(a[i]) + "point\n";

            //テキスト変更
            mainModeWinPlayerCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;

            //2秒後に開始
            StartCoroutine(ResultFinish(2.0f));

            return;
        }

        fade.FadeOut(fadeTime);
        roundText.text = StageSelectManager.GetNowRound() + "/4";

        //順位テキスト表示
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text
                = ScoreManager.GetRank((byte)(i + 1)).ToString() + "位 " + ScoreManager.GetScore((byte)(i + 1)).ToString() + "P";
        }
        Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //4秒後に開始
        StartCoroutine(StartSceneChange(4.0f));
    }

    // Update is called once per frame
    void Update()
    {
        //結果発表が終わったのなら
        if (isResultFinish) 
            GoModeSelect();
    }

    //プレイヤー生成
    private void PlayerInstantiate()
    {
        //プレイヤー生成
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
           GameObject player = ((GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(i)));
           player = Instantiate(player, this.transform.position, Quaternion.identity);
           player.transform.position = playerPosition[i - 1];
           player.transform.localScale = playerScale[i - 1];
           player.transform.localEulerAngles = playerRotation[i - 1];
        }
    }

    private void GoModeSelect()
    {
        if (Input.GetButtonDown("Abutton1")) 
            SceneManager.LoadScene("ModeSelect");
    }

    //結果発表終了
    IEnumerator ResultFinish(float delay)
    {
        yield return new WaitForSeconds(delay);

        //終わりに変更
        isResultFinish = true;
    }

    //シーン遷移開始
    IEnumerator StartSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);

        StageSelectManager.ChangeMiniGameScene();
    }
}
