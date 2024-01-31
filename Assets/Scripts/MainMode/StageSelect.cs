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
    [SerializeField] private List<Vector3> numberOnePlayerEffePos = new List<Vector3>();
    [SerializeField] private List<GameObject> numberOnePlayerEffe;
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
    [SerializeField] public List<GameObject> playerList = new List<GameObject>();

    private int nowLookMaterialNum = 0;
    private bool isResultFinish;

    // Start is called before the first frame update
    void Start()
    {
        isResultFinish = false;

        //�e���K�v�Ȃ���
        //PlayerManager.Initializ();
        //ScoreManager.Initializ();
        TutorialManager.isInitializOK = false;
        TutorialManager.isTutorialFinish = false;
        PlayerInstantiate();
        talkText[StageSelectManager.GetNowRound() - 1].SetActive(true);

        //�e�v���C���[�̃X�R�A�����݂̂ɑΉ�������
        for(int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            scoreText[i].text = ScoreManager.GetBeforeScore((byte)(i + 1)).ToString();

        //�v���C�����~�j�Q�[���̉摜�ɕύX
        for (int i = 0; i < StageSelectManager.GetNowRound() - 1; i++)
            subImage[i].material = StageSelectManager.playMaterial[i];

        //���łɃv���C�����~�j�Q�[�����̂���
        for(int i = 0; i < subImage.Count; i++)
        {
            for(int j = 0; j < miniGameMaterial.Count; j++)
            {
                if (subImage[i].material.mainTexture == miniGameMaterial[j].mainTexture)
                {
                    miniGameMaterial.RemoveAt(j);
                }
            }
        }

        //�t�F�[�h
        fade.FadeOut(fadeTime);

        ////���ʃe�L�X�g�\��
        //for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        //{
        //    rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text
        //        = ScoreManager.GetRank((byte)(i + 1)).ToString() + "�� " + ScoreManager.GetScore((byte)(i + 1)).ToString() + "P";
        //}
        //Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //4�b��ɊJ�n
        //StartCoroutine(StartSceneChange(4.0f));
    }

    // Update is called once per frame
    void Update()
    {

        //�e�v���C���[�̃X�R�A���~���ŕۑ�
        Dictionary<int, int> score = new Dictionary<int, int>();
        for (int i = 0; i < scoreText.Count; i++)
            score[i] = int.Parse(scoreText[i].text);

        //�\�[�g
        var sortedDictionary = score.OrderByDescending(pair => pair.Value);
        int scoreMax = score[0];
        int effectNum = 0;
        foreach (var item in sortedDictionary)
        {
            if (scoreMax == item.Value)
            {
                numberOnePlayerEffe[effectNum].SetActive(true);
                numberOnePlayerEffe[effectNum].transform.localPosition = numberOnePlayerEffePos[effectNum];
                effectNum++;
            }
        }

    }

    //�v���C���[����
    private void PlayerInstantiate()
    {
        //�v���C���[����
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
           GameObject player = ((GameObject)Resources.Load("Prefabs/" + PlayerManager.GetPlayerVisual(i)));
           player = Instantiate(player, this.transform.position, Quaternion.identity);
           player.transform.position = playerPosition[i - 1];
           player.transform.localScale = playerScale[i - 1];
           player.transform.localEulerAngles = playerRotation[i - 1];

           //�v���C���[�����X�g�ɒǉ�
           playerList.Add(player);
        }
    }

    //���[�h�I���ɖ߂�
    private void GoModeSelect()
    {
        if (Input.GetButtonDown("Abutton1")) 
            SceneManager.LoadScene("ModeSelect");
    }

    //���ׂẴ~�j�Q�[�����I�������^�C�~���O�ŌĂ΂��
    private void AllMiniGameFinish()
    {
        //���E���h�S�ďI�����Ă���̂Ȃ�
        if (StageSelectManager.isMainModeFinish)
        {
            mainModeWinPlayerCanvas = Instantiate(mainModeWinPlayerCanvas, new Vector3(0, 0, 0), Quaternion.identity);

            //1�ʂ̃v���C�����擾
            List<byte> a = ScoreManager.GetNominatePlayerRank(1);
            string text = "Win\n";
            for (int i = 0; i < a.Count; i++)
                text += a[i] + "P " + ScoreManager.GetScore(a[i]) + "point\n";

            //�e�L�X�g�ύX
            mainModeWinPlayerCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;

            //2�b��ɊJ�n
            StartCoroutine(ResultFinish(2.0f));

            return;
        }
    }

    //�~�j�Q�[���J�n
    IEnumerator MiniGameStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Material�̌��̖��O���擾����
        string originalName = mainImage. material.name.Replace("(Instance)", "").Trim();
        SceneManager.LoadScene(originalName);
    }

    //�摜�̃A�j���[�V����
    IEnumerator ImageAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�T�u�摜��ύX
        subImage[StageSelectManager.GetNowRound() - 1].material = mainImage.material;
        StageSelectManager.playMaterial.Add(mainImage.material);
        fade.FadeIn(fadeTime);
        StartCoroutine(MiniGameStart(fadeTime));
    }

    //�~�j�Q�[���������_���ɃX�^�[�g
    public IEnumerator MiniGameRandom(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�}�e���A����V���ɕύX
        mainImage.material = miniGameMaterial[nowLookMaterialNum];
        nowLookMaterialNum += 1;

        //�v�f�����I�[�o�[���Ă���̂Ȃ�0�ɖ߂�
        if (miniGameMaterial.Count <= nowLookMaterialNum)
            nowLookMaterialNum = 0;

        //���̉摜�Ɉڂ�b���𑝂₷
        nextImageTime += Random.Range(0.005f, 0.02f);

        //���̑��x�ɂ�����I���
        if (nextImageTime >= 0.5f)
            StartCoroutine(ImageAnimation(0.5f));
        else
            StartCoroutine(MiniGameRandom(nextImageTime));

    }

    //���ʔ��\�I��
    IEnumerator ResultFinish(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�I���ɕύX
        isResultFinish = true;
    }

    //�V�[���J�ڊJ�n
    IEnumerator StartSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);

        StageSelectManager.ChangeMiniGameScene();
    }

    public Fade GetFade()
    {
        return fade;
    }
}
