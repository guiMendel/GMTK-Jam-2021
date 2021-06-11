using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
  [SerializeField] float jumpPower = 1000f;

  public void Jump(Rigidbody2D body)
  {
    body.AddForce(Vector2.up * jumpPower);
  }
}
