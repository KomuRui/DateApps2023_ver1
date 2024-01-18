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

        //�e���K�v�Ȃ���
        //PlayerManager.Initializ();
        ScoreManager.Initializ();
        PlayerInstantiate();
        AllMiniGameFinish();
        talkText[StageSelectManager.GetNowRound() - 1].SetActive(true);

        //�e�v���C���[�̃X�R�A�����݂̂ɑΉ�������
        for(int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            scoreText[i].text = ScoreManager.GetScore((byte)(i + 1)).ToString();

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
        //���ʔ��\���I������̂Ȃ�
        if (isResultFinish) 
            GoModeSelect();
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
        if (nextImageTime >= 0.60f)
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
}
