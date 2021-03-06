using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsManager : MonoBehaviour
{
    public static CollisionsManager instance;

    public CustomBoxCollider[] boxCollidersList;

    private void Awake()
    {
        // Only Have 1 Instance In Scene
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        boxCollidersList = FindObjectsOfType<CustomBoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Collisen Checker Method - (Should Be Called From CustomBoxCollider Script On An Object)
    public bool CollisionCheck(CustomBoxCollider CBC1, CustomBoxCollider CBC2)
    {
        bool isCollision = false;

        // Continue If Both Objects Can Detect Collision
        if (CBC1.DetectCollision && CBC2.DetectCollision)
        {
            // Checking Collision Sequence #1 - (CBC1 X&Y < CBC2 X&Y)
            if (CBC2.FBL.x <= CBC1.FTR.x && CBC2.FBL.y <= CBC1.FTR.y && CBC2.FTR.x >= CBC1.FTR.x && CBC2.FTR.y >= CBC1.FTR.y)
            {
                // CBC1 Is Behind CBC2 On Z Axis
                if (CBC2.BBL.z >= CBC1.FTR.z && CBC1.FTR.z >= CBC2.FBL.z)
                {
                    Debug.Log("FTRtoBBL");
                    isCollision = true;
                }
                // CBC1 Is Infront CBC2 On Z Axis
                else if (CBC2.BBL.z >= CBC1.BTR.z && CBC1.BTR.z >= CBC2.FBL.z)
                {
                    Debug.Log("BTRtoFBL");
                    isCollision = true;
                }
            }
            // Checking Collision Sequence #2 - (CBC1 X < CBC2 X) && (CBC1 Y > CBC2 Y)
            else if (CBC2.FTL.x <= CBC1.FBR.x && CBC2.FTL.y >= CBC1.FBR.y && CBC2.FBR.x >= CBC1.FBR.x && CBC2.FBR.y <= CBC1.FBR.y)
            {
                // CBC1 Is Behind CBC2 On Z Axis
                if (CBC2.BTL.z >= CBC1.FBR.z && CBC1.FBR.z >= CBC2.FTL.z)
                {
                    Debug.Log("FBRtoBTL");
                    isCollision = true;
                }
                // CBC1 Is Infront CBC2 On Z Axis
                else if (CBC2.BTL.z >= CBC1.BBR.z && CBC1.BBR.z >= CBC2.FTL.z)
                {
                    Debug.Log("BBRtoFTL");
                    isCollision = true;
                }
            }
            // Checking Collision Sequence #3 - (CBC1 X > CBC2 X) && (CBC1 Y < CBC2 Y)
            else if (CBC2.BBR.x >= CBC1.FTL.x && CBC2.FBR.y <= CBC1.FTL.y && CBC2.BTL.x <= CBC1.FTL.x && CBC2.FTL.y >= CBC1.FTL.y)
            {
                // CBC1 Is Behind CBC2 On Z Axis
                if (CBC2.BBR.z >= CBC1.FTL.z && CBC1.FTL.z >= CBC2.FBR.z)
                {
                    Debug.Log("FTLtoBBR");
                    isCollision = true;
                }
                // CBC1 Is Infront CBC2 On Z Axis
                else if (CBC2.BBR.z >= CBC1.BTL.z && CBC1.BTL.z >= CBC2.FBR.z)
                {
                    Debug.Log("BTLtoFBR");
                    isCollision = true;
                }
            }
            // Checking Collision Sequence #4 - (CBC1 X > CBC2 X) && (CBC1 Y > CBC2 Y)
            else if (CBC2.BTR.x >= CBC1.FBL.x && CBC2.BTR.y >= CBC1.FBL.y && CBC2.BTL.x <= CBC1.FTL.x && CBC2.FTL.y <= CBC1.FTL.y)
            {
                // CBC1 Is Behind CBC2 On Z Axis
                if (CBC2.BBR.z >= CBC1.FTL.z && CBC1.FTL.z >= CBC2.FBR.z)
                {
                    Debug.Log("FBLtoBTR");
                    isCollision = true;
                }
                // CBC1 Is Infront CBC2 On Z Axis
                else if (CBC2.BBR.z >= CBC1.BTL.z && CBC1.BTL.z >= CBC2.FBR.z)
                {
                    Debug.Log("BBLtoFTR");
                    isCollision = true;
                }
            }
        }

        return isCollision;
    }
}
