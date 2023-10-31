using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LookOnTexture : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5.0f;          // �v���C���[�̈ړ����x
    [SerializeField] private GameObject Object;
    [SerializeField] private CarryBullet bullet;
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�

    private Vector3 beforePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //����
        Shoot();

        //�ړ�
        Move();

        //�e�N�X�`������t���邽�߃��C�L���X�g����
        TextureRayCast();
    }

    //����
    private void Shoot()
    {
        if(Input.GetButtonDown("Abutton" + playerNum))
            Instantiate(bullet, transform.position, Quaternion.identity);
    }

    //���C�L���X�g
    private void TextureRayCast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down); // Ray�𐶐�

        if (Physics.Raycast(ray, out hit, 10000))
        {
            if (hit.collider.gameObject.tag == "CarryStage")
                Object.transform.position = hit.point + new Vector3(0,1,0);
            else if (hit.collider.gameObject.tag == "Sea")
                transform.position = beforePos;
        }

    }

    //�ړ�
    private void Move()
    {
        //�ړ��O�̃|�W�V�������o���Ă���
        beforePos = transform.position;

        // ���͂��擾�p
        float horizontalInput = 0;
        float verticalInput = 0;

        // ���͂��擾
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + playerNum);

        //���͂��Ȃ��̂Ȃ�
        if (horizontalInput == 0 && verticalInput == 0)
            return;


        // �J�����̌�������Ƀv���C���[���ړ�
        Vector3 forwardDirection = Camera.main.transform.forward;
        Vector3 rightDirection = Camera.main.transform.right;
        forwardDirection.y = 0f; // Y��������0�ɂ��邱�ƂŐ��������ɐ���

        // �ړ��������v�Z
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // �ړ�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
