using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class SurfboardPlayer : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�
    [SerializeField] private float LIMIT_ROTATE = 90.0f;      //��]�̌��E
    private Rigidbody rb;
    private Vector3 pos;
    Quaternion rot;

    public float rotationSpeed = 10.0f;
    public float maxRotationAngle = 90.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rot = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // ���͂��擾�p
        float horizontalInput = 0;
        float verticalInput = 0;

        // ���͂��擾
        horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        verticalInput = -Input.GetAxis("L_Stick_V" + playerNum);

        // ���݂̉�]�p�x���擾
        Vector3 currentRotation = transform.localEulerAngles;

        // ��]�ɐ�����������
        currentRotation.x += horizontalInput * rotationSpeed;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxRotationAngle, maxRotationAngle);

        // �I�u�W�F�N�g����]
        transform.localEulerAngles = currentRotation;

        //�X�e�B�b�N�ɂ���ĉ�]
        //transform.Rotate(new Vector3(0, horizontalInput, 0));
        //transform.Rotate(new Vector3(verticalInput , 0, 0));

    }
}
