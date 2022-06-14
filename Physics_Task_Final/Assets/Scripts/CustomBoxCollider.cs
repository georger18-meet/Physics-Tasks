using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBoxCollider : MonoBehaviour
{
    public bool DetectCollision = true;
    public bool WasTriggered;
    public CustomBoxCollider ObjCollidedWithRef;
    public Vector3 ColliderOffset;
    public Vector3 ColliderSize = new Vector3(1, 1, 1);

    private CollisionsManager collisionsManagerRef;

    //// Box Collider Edges List
    // Cube Corner Transform Front
    private Vector3 _frontTopRight;
    private Vector3 _frontBottomRight;
    private Vector3 _frontBottomLeft;
    private Vector3 _frontTopLeft;
    // Cube Corner Transform Back
    private Vector3 _backTopRight;
    private Vector3 _backBottomRight;
    private Vector3 _backBottomLeft;
    private Vector3 _backTopLeft;
    // Box Collider Edges Get Properties
    public Vector3 FTR
    {
        get
        {
            return _frontTopRight;
        }
    }
    public Vector3 FBR
    {
        get
        {
            return _frontBottomRight;
        }
    }
    public Vector3 FBL
    {
        get
        {
            return _frontBottomLeft;
        }
    }
    public Vector3 FTL
    {
        get
        {
            return _frontTopLeft;
        }
    }
    public Vector3 BTR
    {
        get
        {
            return _backTopRight;
        }
    }
    public Vector3 BBR
    {
        get
        {
            return _backBottomRight;
        }
    }
    public Vector3 BBL
    {
        get
        {
            return _backBottomLeft;
        }
    }
    public Vector3 BTL
    {
        get
        {
            return _backTopLeft;
        }
    }


    private void Awake()
    {
        collisionsManagerRef = FindObjectOfType<CollisionsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        BoxColliderEdges(ColliderOffset + transform.position);
        CheckCollisionsWithThisObj();
    }


    // Method For PinPointing The Transform of the Box Collider's Edges In World Space
    public void BoxColliderEdges(Vector3 positionInV3)
    {
        // FTR Location
        _frontTopRight = SingleEdgeCollision(positionInV3, _frontTopRight, "+", "+", "-");

        // FBR Location
        _frontBottomRight = SingleEdgeCollision(positionInV3, _frontBottomRight, "+", "-", "-");

        // FBL Location
        _frontBottomLeft = SingleEdgeCollision(positionInV3, _frontBottomLeft, "-", "-", "-");

        // FTL Location
        _frontTopLeft = SingleEdgeCollision(positionInV3, _frontTopLeft, "-", "+", "-");

        // BTR Location
        _backTopRight = SingleEdgeCollision(positionInV3, _backTopRight, "+", "+", "+");

        // BBR Location
        _backBottomRight = SingleEdgeCollision(positionInV3, _backBottomRight, "+", "-", "+");

        // BBL Location
        _backBottomLeft = SingleEdgeCollision(positionInV3, _backBottomLeft, "-", "-", "+");

        // BTL Location
        _backTopLeft = SingleEdgeCollision(positionInV3, _backTopLeft, "-", "+", "+");


        //Debug.Log($"{gameObject.name} FTR:{_frontTopRight}");
        //Debug.Log($"{gameObject.name} FBR:{_frontBottomRight}");
        //Debug.Log($"{gameObject.name} FBL:{_frontBottomLeft}");
        //Debug.Log($"{gameObject.name} FTL:{_frontTopLeft}");
        //Debug.Log($"{gameObject.name} BTR:{_backTopRight}");
        //Debug.Log($"{gameObject.name} BBR:{_backBottomRight}");
        //Debug.Log($"{gameObject.name} BBL:{_backBottomLeft}");
        //Debug.Log($"{gameObject.name} BTL:{_backTopLeft}");
    }

    private Vector3 SingleEdgeCollision(Vector3 positionInV3 ,Vector3 edge, string xOperation, string yOperation, string zOperation)
    {
        // Finding Edge xPos In World
        if (xOperation == "+")
        {
            edge.x = positionInV3.x + (ColliderSize.x / 2);
        }
        else
        {
            edge.x = positionInV3.x - (ColliderSize.x / 2);
        }

        // Finding Edge yPos In World
        if (yOperation == "+")
        {
            edge.y = positionInV3.y + (ColliderSize.y / 2);
        }
        else
        {
            edge.y = positionInV3.y - (ColliderSize.y / 2);
        }

        // Finding Edge zPos In World
        if (zOperation == "+")
        {
            edge.z = positionInV3.z + (ColliderSize.z / 2);
        }
        else
        {
            edge.z = positionInV3.z - (ColliderSize.z / 2);
        }

        return edge;
    }


    private void CheckCollisionsWithThisObj()
    {
        bool tempTrigger = false;
        // Loop Through All Objects With Custom Collider
        foreach (var boxCollider in collisionsManagerRef.boxCollidersList)
        {
            // Skip If The Collider Is "This"
            if (!boxCollider.Equals(this))
            {
                // Run The Collision Check Method
                if (collisionsManagerRef.CollisionCheck(this, boxCollider))
                {
                    if (!tempTrigger)
                    {
                        // Set Obj Collided With Ref
                        ObjCollidedWithRef = boxCollider;
                    }
                    tempTrigger = true;
                }
            }
        }
        WasTriggered = tempTrigger;

        // Reset Obj Collided With Ref If Collision Wasn't Triggered
        if (!WasTriggered)
        {
            ObjCollidedWithRef = null;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(ColliderOffset, ColliderSize);
    }
}
