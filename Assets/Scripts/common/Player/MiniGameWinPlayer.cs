using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameWinPlayer : MonoBehaviour
{
    public float rotationDuration = 2f;
    public float jumpHeight = 3f;
    public float jumpDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���������̃A�j���[�V����
    public void WinAnimation()
    {
        //��]
        Vector3 angle = transform.localEulerAngles + new Vector3(0,360,0);
        transform.DORotate(angle, rotationDuration, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear);

        //�W�����v
        transform.DOLocalJump(transform.position, jumpHeight, 1, jumpDuration)
           .SetEase(Ease.OutQuad);
    }
}
