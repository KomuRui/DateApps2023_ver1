using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeSide_UI: MonoBehaviour
{
    [SerializeField] private GameObject player;
    public UnityEngine.Vector3 playerPos = UnityEngine.Vector3.zero;
    public const float SHIFT_VER_UI = 1.0f;
    public const float SHIFT_HOR_UI = -0.5f;
    [SerializeField] private int playerNum;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.nowMiniGameManager.threePlayerObj[this.GetComponent<PlayerNum>().playerNum - 2];

        //�ʒu��ς���
        SetPosition();
    }


    // Update is called once per frame
    void Update()
    {
        //�ʒu��ς���
        SetPosition();
    }

    //�ʒu��ς���
    void SetPosition()
    {
        UnityEngine.Vector3 myRectTfm;

        playerPos = player.transform.position;

        playerPos.x += SHIFT_HOR_UI;
        playerPos.y += SHIFT_VER_UI;

        myRectTfm = RectTransformUtility.WorldToScreenPoint(Camera.main, playerPos);

        // �J��������ɂ���^�[�Q�b�g�̃X�N���[�����W�́A��ʒ��S�ɑ΂���_�Ώ̂̍��W�ɂ���
        if (myRectTfm.z < 0.0f)
        {
            myRectTfm.x = -myRectTfm.x;
            myRectTfm.y = -myRectTfm.y;
        }

        this.transform.position = myRectTfm;
    }
}
