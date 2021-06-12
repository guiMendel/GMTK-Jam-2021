using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
  [Tooltip("Controllers with higher priority become parents when merging with other controllers. Has less precendence than movement priority.")]
  [SerializeField] int mergePriority = 0;

  // stored refs
  Rigidbody2D body;

  public int GetPriority() { return mergePriority; }

  // Start is called before the first frame update
  void Start()
  {
    body = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    // take commands
    // jump command
    if (Input.GetButtonDown("Jump")) Jump();
    // walk command. Registers if character is walking or not
    if (Input.GetAxis("Horizontal") != 0) Walk(Input.GetAxis("Horizontal"));
    if (Input.GetButtonDown("Fire1")) Fire();
  }

  private void Fire()
  {
    // find a shooter
    GetComponentInChildren<Shooter>()?.Fire();
  }

  private void Walk(float movement)
  {
    // find a walker
    if (movement > 0) GetComponentInChildren<RightWalker>()?.Walk(transform, movement);
    else GetComponentInChildren<LeftWalker>()?.Walk(transform, movement);
  }

  private void Jump()
  {
    // find a jumper
    Jumper jumper = GetComponentInChildren<Jumper>();

    // make sure characer is grounded
    if (jumper && IsGrounded()) jumper.Jump(body);
  }

  private bool IsGrounded()
  {
    // take all colliders
    Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

    print("all colliders length: " + colliders.Length.ToString());

    // minimum height
    float height = float.MaxValue;

    // pick the downmost ones
    List<Collider2D> downmostColliders = new List<Collider2D>();

    foreach (Collider2D collider in colliders)
    {
      float colliderHeight = collider.transform.position.y;

      if (colliderHeight < height)
      {
        height = colliderHeight;

        // flush all previous results
        downmostColliders.Clear();

        downmostColliders.Add(collider);
      }
      else if (colliderHeight == height) downmostColliders.Add(collider);
    }

    print("filtered colliders length: " + downmostColliders.Count.ToString());


    // if at least one of the colliders is grounded, we return true
    return downmostColliders.Any((Collider2D collider) =>
    {
      // get the collider's size
      float colliderSize = collider.bounds.extents.y;

      // margin of distance between character and ground we accept
      float errorMargin = 0.1f;

      print("collider size: " + colliderSize.ToString());
      print("probe: " + (collider.Raycast(Vector2.down, new RaycastHit2D[1], colliderSize + errorMargin) > 0).ToString());

      // probe the ground
      return (collider.Raycast(Vector2.down, new RaycastHit2D[1], colliderSize + errorMargin) > 0);
    });
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    // merge if its another controller
    if (other.gameObject.GetComponent<Controller>()) Merge(other.gameObject);
  }

  private void Merge(GameObject otherObject)
  {
    // get other controller
    Controller otherController = otherObject.GetComponent<Controller>();

    // stops if the other has higher priority
    if (otherController.GetPriority() > mergePriority)
    {
      enabled = false;
      return;
    }

    // Set this as its parent
    otherObject.transform.parent = transform;

    // disable its sprite renderer
    otherObject.GetComponent<SpriteRenderer>().enabled = false;

    // disable its rigidbody
    otherObject.GetComponent<Rigidbody2D>().isKinematic = true;

    // disable its collider
    otherObject.GetComponent<BoxCollider2D>().enabled = false;
  }
}
