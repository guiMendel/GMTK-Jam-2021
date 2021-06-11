using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
  [Tooltip("Controllers with higher priority become parents when merging with other controllers")]
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
    if (Input.GetButtonDown("Jump")) Jump();
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
    GetComponentInChildren<Jumper>()?.Jump(body);
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
    if (otherController.GetPriority() > mergePriority) {
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
