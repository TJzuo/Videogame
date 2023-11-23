using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
   private LineRenderer lr;
   private Vector3 grapplePoint;
   public LayerMask whatIsGrappleable;
   public Transform gunTip, gameCamera, player;
   private float maxDistance = 100f;
   private SpringJoint joint;
   public LayerMask whatIsGround;
   void Awake () 
   {
       lr = GetComponent<LineRenderer>();
   }

   void Update()
   {
       if (Input.GetMouseButtonDown(0)) {
           StartGrapple();
       }
       else if (Input.GetMouseButtonUp(0)) {
           StopGrapple();
       }
   }
   void LateUpdate()
   {
     DrawRope();
   }

   void StartGrapple() {
       RaycastHit hit;
       if (Physics.Raycast(gameCamera.transform.position, gameCamera.transform.forward, out hit, maxDistance, whatIsGround)) {
           grapplePoint = hit.point;
           joint = player.gameObject.AddComponent<SpringJoint>();
           joint.autoConfigureConnectedAnchor = false;
           joint.connectedAnchor = grapplePoint;

           float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
           joint.maxDistance = distanceFromPoint * 0.2f;
           joint.minDistance = distanceFromPoint * 0f;

           joint.spring = 4.5f;
           joint.damper = 7f;
           joint.massScale = 4.5f;

           lr.positionCount = 2;
       }
   }

   void DrawRope(){
       
       if (!joint) return;

       lr.SetPosition (0, gunTip.position);
       lr.SetPosition (1, grapplePoint);
   }

   void StopGrapple() {
       lr.positionCount = 0;
       Destroy(joint);

   }

   public bool IsGrappling(){
       return joint != null;
   }

   public Vector3 GetGrapplePoint(){
       return grapplePoint;
   }

}
