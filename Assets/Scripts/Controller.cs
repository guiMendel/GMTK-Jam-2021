using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
  [Tooltip("If on, character never dies")]
  [SerializeField] bool godMode = false;

  [Tooltip("Plays on death")]
  [SerializeField] GameObject deathVFX;

  [Tooltip("Duration of vfx")]
  [SerializeField] float vfxDuration = 1f;

  // stored refs
  Rigidbody2D body;

  // state
  float movement;
  // Update sets this and FixedUpdate reads from and resets it
  bool jumpInThisUpdate = false;

  public bool IsMoving() { return movement != 0; }

  public float GetTotalMass()
  {
    Rigidbody2D[] bodies = GetComponentsInChildren<Rigidbody2D>();

    return bodies
      .Select((Rigidbody2D body) => body.mass)
      .Aggregate((float mass1, float mass2) => mass1 + mass2);
  }

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
    if (Input.GetButtonDown("Jump")) jumpInThisUpdate = true;

    // walk command
    movement = Input.GetAxis("Horizontal");

    // fire command
    if (Input.GetButtonDown("Fire1")) Fire();
  }

  private void FixedUpdate()
  {
    Walk();
    if (jumpInThisUpdate)
    {
      Jump();
      jumpInThisUpdate = false;
    }
  }

  private void Fire()
  {
    // find a shooter
    GetComponentInChildren<Shooter>()?.Fire();
  }

  private void Walk()
  {
    if (movement >= 0) GetComponentInChildren<RightWalker>()?.Walk(body, movement);
    if (movement <= 0) GetComponentInChildren<LeftWalker>()?.Walk(body, movement);
  }

  private void Jump()
  {
    // find a jumper
    Jumper jumper = GetComponentInChildren<Jumper>();

    // make sure character is grounded
    if (jumper && IsGrounded()) jumper.Jump(this);
  }

  public bool IsGrounded()
  {
    // take all colliders
    Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

    // if at least one of the colliders is grounded, we return true
    return colliders.Any((Collider2D collider) =>
    {
      // get the collider's size
      float colliderSize = collider.bounds.extents.y;

      // margin of distance between character and ground we accept
      float errorMargin = 0.1f;

      // print("probe: " + (collider.Raycast(Vector2.down, new RaycastHit2D[1], colliderSize + errorMargin) > 0).ToString());

      // probe the ground
      return (collider.Raycast(Vector2.down, new RaycastHit2D[1], colliderSize + errorMargin) > 0);
    });
  }

  private void Die()
  {
    // vfx
    if (deathVFX)
    {
      GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
      Destroy(vfx, vfxDuration);
    }

    // deparent any other blobs
    foreach (var transform in transform.GetComponentsInChildren<Transform>())
    {
      transform.parent = null;
    }

    Destroy(gameObject);
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Hazard") && !godMode) Die();
  }
}
