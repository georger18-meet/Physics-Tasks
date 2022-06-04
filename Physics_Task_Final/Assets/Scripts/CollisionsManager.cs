using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsManager : MonoBehaviour
{
    public CustomBoxCollider box1;
    public CustomBoxCollider box2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CollisionCheck(box1, box2));
    }

    // Collisen Checker Method
    public static bool CollisionCheck(CustomBoxCollider CBC1, CustomBoxCollider CBC2)
    {
        bool isCollision = false;


        if (CBC1.DetectCollision && CBC2.DetectCollision)
        {
            // Checking Collision Sequence #1
            if (CBC2.FBL.x <= CBC1.FTR.x && CBC2.FBL.y <= CBC1.FTR.y && CBC2.FTR.x >= CBC1.FTR.x && CBC2.FTR.y >= CBC1.FTR.y)
            {
                if (CBC2.BBL.z >= CBC1.FTR.z && CBC1.FTR.z >= CBC2.FBL.z)
                {
                    Debug.Log("FTRtoBBL");
                    isCollision = true;
                }
                else if (CBC2.BBL.z >= CBC1.BTR.z && CBC1.BTR.z >= CBC2.FBL.z)
                {
                    Debug.Log("BTRtoFBL");
                    isCollision = true;
                }
            }
            // Checking Collision Sequence #2
            else if (CBC2.FTL.x <= CBC1.FBR.x && CBC2.FTL.y >= CBC1.FBR.y && CBC2.FBR.x >= CBC1.FBR.x && CBC2.FBR.y <= CBC1.FBR.y)
            {
                if (CBC2.BTL.z >= CBC1.FBR.z && CBC1.FBR.z >= CBC2.FTL.z)
                {
                    Debug.Log("FBRtoBTL");
                    isCollision = true;
                }
                else if (CBC2.BTL.z >= CBC1.BBR.z && CBC1.BBR.z >= CBC2.FTL.z)
                {
                    Debug.Log("BBRtoFTL");
                    isCollision = true;
                }
            }
            // Checking Collision Sequence #3
            else if (CBC2.BBR.x >= CBC1.FTL.x && CBC2.FBR.y <= CBC1.FTL.y && CBC2.BTL.x <= CBC1.FTL.x && CBC2.FTL.y >= CBC1.FTL.y)
            {
                if (CBC2.BBR.z >= CBC1.FTL.z && CBC1.FTL.z >= CBC2.FBR.z)
                {
                    Debug.Log("FTLtoBBR");
                    isCollision = true;
                }
                else if (CBC2.BBR.z >= CBC1.BTL.z && CBC1.BTL.z >= CBC2.FBR.z)
                {
                    Debug.Log("BTLtoFBR");
                    isCollision = true;
                }
            }
            // Checking Collision Sequence #4

        }

        return isCollision;
    }
}
