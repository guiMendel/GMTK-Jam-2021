using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher : MonoBehaviour
{
  [Tooltip("Attachers with higher priority become parents when merging with other attachers. Has less precendence than movement priority.")]
  [SerializeField] int mergePriority = 0;

  // state
  // used to keep distance relative to parent always locked
  Vector3 lockedPosition;

  // controller getter
  public Func<Controller> GetController;

  public int GetPriority() { return mergePriority; }

  // attach to the other object
  public void AttachTo(Attacher merger)
  {
    print("merging " + GetPriority() + " to " + merger.GetPriority());

    // we need to snap this attacher to the merger's attacher

    // // disable collision between the two
    // Physics2D.IgnoreCollision(GetComponent<Collider2D>(), merger.GetComponent<Collider2D>());

    // snap to it
    SnapTo(merger.transform);

    // Now we need to set this attacher's main parent as a child of the merger's main parent

    // attach this controller's game object as a child to the merger's controller
    GameObject mainParent = GetController().gameObject;

    // disable current controller
    GetController().enabled = false;

    // make the mainParent's controller getter point to the new controller
    mainParent.GetComponent<Attacher>().GetController = merger.GetController;

    // use other as parent
    mainParent.transform.parent = merger.transform;

    // disable rigidbody's physics
    // Rigidbody2D body = mainParent.GetComponent<Rigidbody2D>();
    // body.velocity = Vector2.zero;

    // GetComponent<Collider2D>().enabled = false;

    // GetComponent<SpriteRenderer>().enabled = false;

  }

  private void SnapTo(Transform otherTransform)
  {
    float yDistance = otherTransform.position.y - transform.position.y;
    float xDistance = otherTransform.position.x - transform.position.x;

    // the side to which we will snap
    Vector2 side;

    // snap to the sides
    if (Mathf.Abs(yDistance) < Mathf.Abs(xDistance))
    {
      side = xDistance < 0 ? Vector2.right : Vector2.left;
    }
    // snap above or below
    else
    {
      side = yDistance < 0 ? Vector2.up : Vector2.down;
    }

    // adjust the bigger distance to be exactly on the edge of the collider
    float otherExtent = otherTransform.GetComponent<Collider2D>().bounds.extents.x;
    float desiredDistance = otherExtent + GetComponent<Collider2D>().bounds.extents.x;
    desiredDistance += MergeJudge.snapGap;

    lockedPosition = otherTransform.position + (Vector3)side * desiredDistance;

    // now that we know where this object should be, we translate the parent so that it gets there
    Vector3 displacement = lockedPosition - transform.position;

    // snap
    GetController().transform.Translate(displacement);

    // create a joint between the two
    CreateJointWith(otherTransform);
  }

  private void CreateJointWith(Transform otherTransform)
  {
    FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();

    // prevent collision
    joint.enableCollision = false;

    // use default anchors
    joint.autoConfigureConnectedAnchor = true;

    // adjust springyness
    joint.dampingRatio = 1;

    // connect to other rigid body
    joint.connectedBody = otherTransform.GetComponent<Rigidbody2D>();
  }

  private void Start()
  {
    // initially, just returns own controller
    GetController = GetComponent<Controller>;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    // merge if its another attacher
    Attacher otherAttacher = other.gameObject.GetComponent<Attacher>();
    if (otherAttacher)
    {
      // when the other attacher request a merge too, the mergeJudge will merge them both
      FindObjectOfType<MergeJudge>().MergeRequest(this, otherAttacher);
    }
  }
}
