using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher : MonoBehaviour
{
  [Tooltip("Attachers with higher priority become parents when merging with other attachers. Has less precendence than movement priority.")]
  [SerializeField] int mergePriority = 0;

  // stored refs
  Controller controller;

  public int GetPriority() { return mergePriority; }

  public Controller GetController() { return controller; }

  // attach to the other object
  public void AttachTo(Attacher merger)
  {
    print("merging " + GetPriority() + " to " + merger.GetPriority());

    // disable controller
    controller.enabled = false;

    // use other's controller as own
    controller = merger.GetController();

    // use other as parent
    transform.parent = merger.transform;

    // disable rigidbody's physics
    Rigidbody2D body = GetComponent<Rigidbody2D>();
    body.velocity = Vector2.zero;
    GetComponent<Rigidbody2D>().isKinematic = true;

    // snap to it
    SnapTo(merger.transform);

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

    Vector3 desiredPosition = otherTransform.position + (Vector3)side * desiredDistance;

    // snap
    transform.position = desiredPosition;
  }

  private void Start()
  {
    controller = GetComponent<Controller>();
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
