using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TerrorHammerThreePlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;          // �v���C���[�̈ړ����x
    [SerializeField] private float rotationSpeed = 180.0f;    // �v���C���[�̉�]���x
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩
    [SerializeField] public float startPosX;     // �X�^�[�g
    [SerializeField] public float checkPosX;     // �`�F�b�N
    [SerializeField] private float nowPosX;     // �v���C���[
    [SerializeField] private int point;     // �v���C���[
    [SerializeField] public TextMeshProUGUI pointText;       //�_���e�L�X�g
    [SerializeField] private GameObject HammerOb;  // �n���}�[
    
    private Quaternion initializeRotate;
    private Vector3 AttackRotate;
    private bool isAttack;
    private bool isSuper;
    

    private Rigidbody rBody;
    private Transform mainCameraTransform; // ���C���J������Transform

    // Start is called before the first frame update
    void Start()
    {
        nowPosX = startPosX;
        point = 0;
        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        //���W�b�g�{�f�B�擾
        rBody = this.GetComponent<Rigidbody>();

        //����
        initializeRotate = HammerOb.transform.rotation;
        AttackRotate = new Vector3(0, 0, 180);
        isAttack = false;
        isSuper = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.7f, 4.7f),transform.position.y, Mathf.Clamp(transform.position.z, -1.0f, 1.0f));//1.7f

        //�`�F�b�N
        if(nowPosX == startPosX && this.transform.position.x > checkPosX)
        {
            nowPosX = checkPosX;
        }

        if (nowPosX == checkPosX && this.transform.position.x < startPosX)
        {
            nowPosX = startPosX;
            point++;
            ((TerrorHammerGameManager)GameManager.nowMiniGameManager).goalPlayer.Add(this.GetComponent<PlayerNum>().playerNum);

            if (((TerrorHammerGameManager)GameManager.nowMiniGameManager).goalPlayer.Count >= 3)
                ((TerrorHammerGameManager)GameManager.nowMiniGameManager).SetMiniGameFinish();
        }

        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish() || point >= 1 || this.transform.localScale.y <= 0.5f)
            return;
        
        //����
        Move();

        //�U��
        if (Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum) && !HammerOb.GetComponent<ThreePlayerHammer>().isAttack)
        {
            HammerOb.GetComponent<ThreePlayerHammer>().Attack();
            //1.5�b��ɂ�����
            Invoke("HammerUp", 0.5f);
        }
    }

    //�ړ�
    private void Move()
    {
        //�U�����Ȃ炱�̐揈�����Ȃ�
        if (HammerOb.GetComponent<ThreePlayerHammer>().isAttack) return;

        // ���͂��擾�p
        float horizontalInput = 0;
        float verticalInput = 0;

        // ���͂��擾
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + this.GetComponent<PlayerNum>().playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + this.GetComponent<PlayerNum>().playerNum);

        //���͂��Ȃ��̂Ȃ�
        if (horizontalInput == 0 && verticalInput == 0)
        {
            //�ʏ��ԂɕύX
            //ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }
        
        // �J�����̌�������Ƀv���C���[���ړ�
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y��������0�ɂ��邱�ƂŐ��������ɐ���

        // �ړ��������v�Z
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // �ړ�
        rBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    public void HammerUp()
    {
        HammerOb.GetComponent<ThreePlayerHammer>().Return();
        //HammerOb.transform.DORotateQuaternion(initializeRotate, 0.5f).SetEase(Ease.InQuad).OnComplete(() => isAttack = false) ;
    }


    //�v���C���[�̃n���}�[�Ƀq�b�g
    public void HitPlayerHammer()
    {
        if (isSuper) return;

        this.transform.localScale = new Vector3(1, 0.3f, 1);
        isSuper = true;
        Invoke("MovePlayer", 2f);
        Invoke("WeakPlayer", 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "PlayerHammer" && collision.gameObject.GetComponent<TerrorHammerThreePlayer>().isAttack)
        {
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer"/* && isSuper == false*/)
        {
            this.transform.localScale = new Vector3(1, 0.3f, 1);
            isSuper = true;
            GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
            GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);
        }
    }

    void WeakPlayer()
    {
        isSuper = false;
    }

    void MovePlayer()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}