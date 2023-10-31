using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TextureTarget : MonoBehaviour
{

    private GameObject targetObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RayCastTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000))
            PaintTexture(hit.point, hit.normal);
    }

    private void PaintTexture(Vector3 point, Vector3 normal)
    {
        if(targetObj == null)
        {
            targetObj = new GameObject();
            targetObj.name = "targetObject";
            targetObj.hideFlags = HideFlags.HideInHierarchy;
        }

        targetObj.transform.position = point;

        Vector3 leftVec = Vector3.Cross(normal, Vector3.up);
        if (leftVec.magnitude > 0.001f)
            targetObj.transform.rotation = Quaternion.LookRotation(leftVec, normal);
        else
            targetObj.transform.rotation = Quaternion.identity;

        Paint newPaint = new Paint();

    }
}
