using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
  [SerializeField] float speed = 4f;

  public void Walk(Rigidbody2D body, float movement)
  {
    float displacement = movement * speed * Time.fixedDeltaTime;

    // Apply movement
    body.velocity = new Vector2(displacement, body.velocity.y);
    // body.AddForce(displacement * Vector2.right);
    // body.MovePosition((Vector2)body.transform.position + displacement);
  }
}
