using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
  // stored refs
  Rigidbody2D body;

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
}
