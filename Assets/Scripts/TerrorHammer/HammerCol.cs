using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCol : MonoBehaviour
{
    private GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Floor")
        {
            //エフェクトを衝突位置に
            GameObject effect = GetHitEffect();
            if (effect != null)
            {
                effect.transform.position = new Vector3(other.contacts[0].point.x, other.contacts[0].point.y + 0.1f, other.contacts[0].point.z);
                effect.SetActive(true);
                effect.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private GameObject GetHitEffect()
    {
        GameObject ef = ((TerrorHammerGameManager)GameManager.nowMiniGameManager).hitEffectParent;

        for (int i = 0; i < ef.transform.childCount; i++)
        {
            if (!ef.transform.GetChild(i).gameObject.activeSelf)
                return ef.transform.GetChild(i).gameObject;
        }
        return null;
    }
}
