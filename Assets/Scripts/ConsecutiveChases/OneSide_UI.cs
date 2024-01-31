using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSide_UI : MonoBehaviour
{
    [SerializeField] public GameObject player;
    public UnityEngine.Vector3 playerPos = UnityEngine.Vector3.zero;
    [SerializeField] public const float SHIFT_VER_UI = 7.0f;
    [SerializeField] public const float SHIFT_HOR_UI = -4.0f;

    [SerializeField] private float SHIFT_POS_X = 7.0f;
    [SerializeField] private float SHIFT_POS_Y = -4.0f;
    [SerializeField] private float SHIFT_POS_Z = 0f;


    // Start is called before the first frame update
    void Start()
    {
        //�ʒu��ς���
        Setposition();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + SHIFT_POS_X, player.transform.position.y + SHIFT_POS_Y, player.transform.position.z + SHIFT_POS_Z);
        //�ʒu��ς���
        //Setposition();
    }

    void Setposition()
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
