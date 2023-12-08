using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GimmickStart();
    }

    // Update is called once per frame
    void Update()
    {
        GimmickUpdate();

        //A�{�^����������ĂȂ��̂Ȃ炱�̐揈�����Ȃ�
        if (!Input.GetButtonDown("Abutton" + 1)) return;
        Action();
    }


    //����̃A�N�V�������N����
    public virtual void Action()
    {
    }

    public virtual void GimmickStart()
    {
    }

    public virtual void GimmickUpdate()
    {
    }

}
