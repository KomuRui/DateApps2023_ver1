using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float CAMERA_DISTANCE = 3.0f;       //一番早いプレイヤーからのカメラの距離
    public List<GameObject> playerList;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.nowMiniGameManager.IsFinish())
        {
            CameraMove();
        }
    }

    public void CameraMove()
    {
        //一番進んでいるプレイヤー
        float priorityDistance = -9999.0f;

        //プレイヤーの一番早いやつの位置
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] != null && playerList[i].transform.position.z >= priorityDistance)
            {
                priorityDistance = playerList[i].transform.position.z;
            }
        }

        //カメラとの距離を保つ
        transform.position = new Vector3(transform.position.x, transform.position.y, priorityDistance + CAMERA_DISTANCE);
    }
}
