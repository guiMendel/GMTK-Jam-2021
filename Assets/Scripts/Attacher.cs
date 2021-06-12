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
    GetComponent<Rigidbody2D>().isKinematic = true;

    // GetComponent<Collider2D>().enabled = false;

    // GetComponent<SpriteRenderer>().enabled = false;

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
