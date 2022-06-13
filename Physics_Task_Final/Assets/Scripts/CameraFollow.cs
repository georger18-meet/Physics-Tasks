using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ObjToFollow;
    public Vector3 Offset;
    public bool IsTrackingCam;
    public int FollowAfterX;
    public int FollowAfterY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTrackingCam)
        {
            transform.position = ObjToFollow.transform.position + Offset;
        }
        else
        {
            if (ObjToFollow.transform.position.y > FollowAfterY || ObjToFollow.transform.position.x > FollowAfterX)
            {
                if (GetComponent<Camera>().orthographicSize < 25000)
                {
                    GetComponent<Camera>().orthographicSize += 2;
                    transform.position += new Vector3(4, 2, 0);
                }
            }
        }
    }
}
