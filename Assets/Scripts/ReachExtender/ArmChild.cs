using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmChild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayerMove()
    {
        //ƒŒƒC‚ª“–‚½‚Á‚½î•ñ
        RaycastHit rayHit;

        //ƒŒƒC‚ğ”ò‚Î‚·•ûŒü
        Ray ray = new Ray(transform.position, transform.parent.gameObject.transform.position);

        if (UnityEngine.Physics.Raycast(ray, out rayHit, 9999))
        {
            transform.parent.GetComponent<Arm>().hitPlayer.GetComponent<ReachExtenderThreePlayer>().move = rayHit.normal * 100;
        }
    }
}
