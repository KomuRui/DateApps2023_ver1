using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSurfboardPlayer : MonoBehaviour
{
    [SerializeField] private float rotateSpeed  = 0.3f;                   //�X������
    [SerializeField] private float kajiRotateSpeed = 32.0f;                   //1�b�Ԃőǂ��X���p�x
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�
    [SerializeField] private GameObject kaji;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //�}���ł��邽�߃R�����g�ꎞ�I�ɂȂ�
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish()) 
        {
            Move();
        }
    }

    public void Move()
    {
        // ���͂��擾�p
        float horizontalInput = 0;

        // ���͂��擾
        horizontalInput = Input.GetAxis("L_Stick_H" + this.GetComponent<PlayerNum>().playerNum) * rotateSpeed;

        //�ǂ𓮂���
        KajiMove(horizontalInput);

        foreach (var item in GameManager.nowMiniGameManager.threePlayerObj)
        {
            if(item == null || item.transform.parent == null || item.transform.parent.gameObject.GetComponent<SurfboardPlayer>() == null) continue;
            
            SurfboardPlayer p = item.transform.parent.gameObject.GetComponent<SurfboardPlayer>();

            if (p.sumRotateX + horizontalInput <= p.LIMIT_ROTATE && p.sumRotateX + horizontalInput >= -p.LIMIT_ROTATE && !p.isDead)
            {
                p.sumRotateX += horizontalInput;

                //�X�e�B�b�N�ɂ���ĉ�]
                p.transform.Rotate(new Vector3(0, horizontalInput, 0));
            }
        }
    }

    //�ǂ𓮂����v���O����
    public void KajiMove(float input)
    {
        float isPlus = 1;
        if (input < 0)
        {
            isPlus *= -1;
        }
        if (input != 0) 
        {
            //�ǂ���]������
            kaji.transform.Rotate(kajiRotateSpeed * Time.deltaTime * 10 * isPlus, 0, 0);
        }
    }
}
