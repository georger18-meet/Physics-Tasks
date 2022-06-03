using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBoxCollider : MonoBehaviour
{
    public bool UseSpriteForSize;
    public bool DetectCollision = true;
    public bool IsTrigger;
    public Vector3 ColliderOffset;
    public Vector3 ColliderSize = new Vector3(1, 1, 1);

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
        if (TryGetComponent(out SpriteRenderer theSprite) && UseSpriteForSize)
        {
            ColliderSize = new Vector3(theSprite.size.x, theSprite.size.y, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BoxColliderEdges(ColliderOffset + transform.position);
    }


    // Method For PinPointing The Transform of the Box Collider's Edges
    public void BoxColliderEdges(Vector3 positionInV3)
    {
        //// Clearing The List
        //BCEdges.Clear();

        float colliderSizeHalfX = ColliderSize.x / 2;
        float colliderSizeHalfY = ColliderSize.y / 2;
        float colliderSizeHalfZ = ColliderSize.z / 2;

        // FTR Location
        _frontTopRight.x = positionInV3.x + colliderSizeHalfX;
        _frontTopRight.y = positionInV3.y + colliderSizeHalfY;
        _frontTopRight.z = positionInV3.z - colliderSizeHalfZ;
        //Debug.Log($"{gameObject.name} FTR:{_frontTopRight}");

        // FBR Location
        _frontBottomRight.x = positionInV3.x + colliderSizeHalfX;
        _frontBottomRight.y = positionInV3.y - colliderSizeHalfY;
        _frontBottomRight.z = positionInV3.z - colliderSizeHalfZ;
        //Debug.Log($"{gameObject.name} FBR:{_frontBottomRight}");

        // FBL Location
        _frontBottomLeft.x = positionInV3.x - colliderSizeHalfX;
        _frontBottomLeft.y = positionInV3.y - colliderSizeHalfY;
        _frontBottomLeft.z = positionInV3.z - colliderSizeHalfZ;
        //Debug.Log($"{gameObject.name} FBL:{_frontBottomLeft}");

        // FTL Location
        _frontTopLeft.x = positionInV3.x - colliderSizeHalfX;
        _frontTopLeft.y = positionInV3.y + colliderSizeHalfY;
        _frontTopLeft.z = positionInV3.z - colliderSizeHalfZ;
        //Debug.Log($"{gameObject.name} FTL:{_frontTopLeft}");

        // BTR Location
        _backTopRight.x = positionInV3.x + colliderSizeHalfX;
        _backTopRight.y = positionInV3.y + colliderSizeHalfY;
        _backTopRight.z = positionInV3.z + colliderSizeHalfZ;
        //Debug.Log($"{gameObject.name} BTR:{_backTopRight}");

        // BBR Location
        _backBottomRight.x = positionInV3.x + colliderSizeHalfX;
        _backBottomRight.y = positionInV3.y - colliderSizeHalfY;
        _backBottomRight.z = positionInV3.z + colliderSizeHalfZ;
        //Debug.Log($"{gameObject.name} BBR:{_backBottomRight}");

        // BBL Location
        _backBottomLeft.x = positionInV3.x - colliderSizeHalfX;
        _backBottomLeft.y = positionInV3.y - colliderSizeHalfY;
        _backBottomLeft.z = positionInV3.z + colliderSizeHalfZ;
        //Debug.Log($"{gameObject.name} BBL:{_backBottomLeft}");

        // BTL Location
        _backTopLeft.x = positionInV3.x - colliderSizeHalfX;
        _backTopLeft.y = positionInV3.y + colliderSizeHalfY;
        _backTopLeft.z = positionInV3.z + colliderSizeHalfZ;
        //Debug.Log($"{gameObject.name} BTL:{_backTopLeft}");
    }


    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(ColliderOffset, ColliderSize);
    }
}
