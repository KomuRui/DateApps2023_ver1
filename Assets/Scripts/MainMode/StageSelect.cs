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
    [SerializeField] private GameObject rankText;   
    [SerializeField] private GameObject mainModeWinPlayerCanvas;
    [SerializeField] private MeshRenderer mainImage;
    [SerializeField] private List<MeshRenderer> subImage;
    [SerializeField] private List<Material> miniGameMaterial;
    [SerializeField] private List<GameObject> talkText;
    [SerializeField] private List<TextMeshProUGUI> scoreText;
    [SerializeField] private float nextImageTime;

    private int nowLookMaterialNum = 0;
    private bool isResultFinish;

    // Start is called before the first frame update
    void Start()
    {
        isResultFinish = false;

        //各自必要なこと
        //PlayerManager.Initializ();
        ScoreManager.Initializ();
        PlayerInstantiate();
        AllMiniGameFinish();
        talkText[StageSelectManager.GetNowRound() - 1].SetActive(true);

        //各プレイヤーのスコアを現在のに対応させる
        for(int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            scoreText[i].text = ScoreManager.GetScore((byte)(i + 1)).ToString();

        //フェード
        fade.FadeOut(fadeTime);

        ////順位テキスト表示
        //for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        //{
        //    rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text
        //        = ScoreManager.GetRank((byte)(i + 1)).ToString() + "位 " + ScoreManager.GetScore((byte)(i + 1)).ToString() + "P";
        //}
        //Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //4秒後に開始
        //StartCoroutine(StartSceneChange(4.0f));
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

    //モード選択に戻す
    private void GoModeSelect()
    {
        if (Input.GetButtonDown("Abutton1")) 
            SceneManager.LoadScene("ModeSelect");
    }

    //すべてのミニゲームが終了したタイミングで呼ばれる
    private void AllMiniGameFinish()
    {
        //ラウンド全て終了しているのなら
        if (StageSelectManager.isMainModeFinish)
        {
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
    }

    //ミニゲーム開始
    IEnumerator MiniGameStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Materialの元の名前を取得する
        string originalName = mainImage. material.name.Replace("(Instance)", "").Trim();
        SceneManager.LoadScene(originalName);
    }

    //画像のアニメーション
    IEnumerator ImageAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        //サブ画像を変更
        subImage[StageSelectManager.GetNowRound() - 1].material = mainImage.material;
        fade.FadeIn(fadeTime);
        StartCoroutine(MiniGameStart(fadeTime));
    }

    //ミニゲームをランダムにスタート
    public IEnumerator MiniGameRandom(float delay)
    {
        yield return new WaitForSeconds(delay);

        //マテリアルを新たに変更
        mainImage.material = miniGameMaterial[nowLookMaterialNum];
        nowLookMaterialNum += 1;

        //要素数をオーバーしているのなら0に戻す
        if (miniGameMaterial.Count <= nowLookMaterialNum)
            nowLookMaterialNum = 0;

        //次の画像に移る秒数を増やす
        nextImageTime += Random.Range(0.005f, 0.02f);

        //一定の速度にきたら終わり
        if (nextImageTime >= 0.60f)
            StartCoroutine(ImageAnimation(0.5f));
        else
            StartCoroutine(MiniGameRandom(nextImageTime));

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
