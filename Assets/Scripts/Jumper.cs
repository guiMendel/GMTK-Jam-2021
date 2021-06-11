using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
  [SerializeField] float jumpPower = 4f;

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
    if (Input.GetButtonDown("Jump")) Jump();
  }

  private void Jump()
  {
    body.velocity = transform.up * jumpPower;
  }
}
