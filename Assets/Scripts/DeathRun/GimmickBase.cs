using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBase : MonoBehaviour
{

    private bool isAction = false;
    private bool isInput = false;
    // Start is called before the first frame update
    void Start()
    {
        GimmickStart();
    }

    // Update is called once per frame
    void Update()
    {
        GimmickUpdate();

        //if (Input.GetButtonDown("Abutton1"))
            //SetIsInput(true);

        //入力があったかアクションを終えていたらこの先処理しない
        if (!isInput || isAction) return;
        isAction = true;
        Action();
    }


    //特定のアクションを起こす
    public virtual void Action()
    {
    }

    public virtual void GimmickStart()
    {
    }

    public virtual void GimmickUpdate()
    {
    }

    public void SetIsInput(bool a)
    {
        isInput = a;
    }
}
