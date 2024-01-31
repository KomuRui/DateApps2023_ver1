using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModeSelectManager : MonoBehaviour
{
    [SerializeField] private List<Vector3> pos;
    [SerializeField] private List<Vector3> rotate;
    [SerializeField] private List<Vector3> scale;
    [SerializeField] private List<GameObject> child;
    [SerializeField] private List<GameObject> mc;
    [SerializeField] private Fade fade;
    public GameObject talkImageCanvas;
    public GameObject nowTalkImageCanvas;

    private List<GameObject> gameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.Initializ();
        gameObjects.Add((GameObject)Resources.Load("Prefabs/ModeSelect/1P/" + PlayerManager.GetPlayerVisual(1)));
        gameObjects[0] = Instantiate(gameObjects[0], this.transform.position, Quaternion.identity);
        gameObjects[0].transform.position = pos[0];
        gameObjects[0].transform.localScale = scale[0];
        gameObjects[0].transform.localEulerAngles = rotate[0];
        child[0].transform.parent = gameObjects[0].transform;
        gameObjects[0].GetComponent<ModeSelectPlayer>().modeSelect = this;
        for (byte i = 1; i < PlayerManager.PLAYER_MAX; i++)
        {
            gameObjects.Add((GameObject)Resources.Load("Prefabs/ModeSelect/Other/" + PlayerManager.GetPlayerVisual((byte)(i + 1))));
            gameObjects[i] = Instantiate(gameObjects[i], this.transform.position, Quaternion.identity);
            gameObjects[i].transform.position = pos[i];
            gameObjects[i].transform.localScale = scale[i];
            gameObjects[i].transform.localEulerAngles = rotate[i];
            child[i].transform.parent = gameObjects[i].transform;
            gameObjects[i].GetComponent<ModeSelectPlayerNavMesh>().target = gameObjects[i - 1].transform;
        }

        //フェードが情報あるのなら
        if (fade)
            fade.FadeOut(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < mc.Count; i++)
        {
            // Y軸だけを向くように設定
            Vector3 targetPosition = gameObjects[0].transform.position;
            targetPosition.y = mc[i].transform.position.y;

            // ターゲットを向く
            mc[i].transform.LookAt(targetPosition);
        }
    }
}
