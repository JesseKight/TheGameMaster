using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherController : MonoBehaviour
{

    Rigidbody rigidbodyR;
    public LayerMask groundedMask;
    public LineRenderer line;
    public Transform startPoint, cam, player;
    public Vector3 endPoint, beginning;
    public float maxDistance;
    SpringJoint joint;


    public RaycastHit hit;

    Vector3 place;


    // Start is called before the first frame update
    void Start()
    {
        rigidbodyR = GetComponent<Rigidbody>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

      
            if (Input.GetMouseButtonDown(1))
            {
                line.enabled = true;
                startTether();
                
            }
            if (Input.GetMouseButtonUp(1))
            {
                stopTether();
                line.enabled = false;
            }

        beginning = startPoint.position;
    }

    private void LateUpdate()
    {
        DrawTether();
    }

    void startTether()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, groundedMask))
        {
            
            endPoint = hit.point;
            
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = endPoint;

            float distanceFromPoint = Vector3.Distance(player.position, endPoint);

            joint.maxDistance = distanceFromPoint * .8f;
            joint.maxDistance = distanceFromPoint * .25f;


            joint.spring = 1.5f;
            joint.damper = 4f;
            joint.massScale = 10f;

            line.positionCount = 2;



        }
    }
    void stopTether()
    {
        line.positionCount = 0;

        Destroy(joint);
    }

    void DrawTether()
    {
        line.SetPosition(0, beginning);
        line.SetPosition(1, endPoint);
    }
}
