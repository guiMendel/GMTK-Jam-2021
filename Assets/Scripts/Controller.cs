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

  // state
  bool moving = false;

  public int GetPriority() { return mergePriority; }

  public bool IsMoving() { return moving; }

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
    if (Input.GetAxis("Horizontal") != 0) moving = Walk(Input.GetAxis("Horizontal"));
    else moving = false;

    // fire command
    if (Input.GetButtonDown("Fire1")) Fire();
  }

  private void Fire()
  {
    // find a shooter
    GetComponentInChildren<Shooter>()?.Fire();
  }

  private bool Walk(float movement)
  {
    // find a walker
    Walker walker;

    if (movement > 0) walker = GetComponentInChildren<RightWalker>();
    else walker = GetComponentInChildren<LeftWalker>();

    // there may not be walkers in this character
    if (walker)
    {
      walker.Walk(transform, movement);
      return true;
    }
    return false;
  }

  private void Jump()
  {
    // find a jumper
    Jumper jumper = GetComponentInChildren<Jumper>();

    // make sure characer is grounded
    if (jumper && IsGrounded()) jumper.Jump(body);
  }

  public bool IsGrounded()
  {
    // take all colliders
    Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

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

    // if at least one of the colliders is grounded, we return true
    return downmostColliders.Any((Collider2D collider) =>
    {
      // get the collider's size
      float colliderSize = collider.bounds.extents.y;

      // margin of distance between character and ground we accept
      float errorMargin = 0.1f;

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
    if (!HigherPriorityThan(otherController)) {
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

  private bool HigherPriorityThan(Controller otherController)
  {
    // grounded characters have higher priority
    bool selfGrounded = IsGrounded(), otherGrounded = otherController.IsGrounded();

    // if self isnt grounded but other is
    if (!selfGrounded && otherGrounded) return false;
    // if self is grounded and other isnt
    else if (selfGrounded && !otherGrounded) return true;

    // moving characters have second higher priority
    bool otherMoving = otherController.IsMoving();

    // if self isnt moving but other is
    if (!moving && otherMoving) return false;
    // if self is moving and other isnt
    else if (moving && !otherMoving) return true;

    // fallback to basic priorities
    if (otherController.GetPriority() > mergePriority)
    {
      return false;
    }
    return true;
  }
}
