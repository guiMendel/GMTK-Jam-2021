using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Worm : MonoBehaviour
{
  [Tooltip("The range at which the worm follows characters")]
  [SerializeField] float range = 7f;

  [Tooltip("The speed at which it moves")]
  [SerializeField] float moveSpeed = 20f;

  // STATE

  // all the characters this worm can chase
  Transform[] characters;
  // the target its chasing now
  Transform currentTarget;

  // STORED REFS

  Animator animator;
  SpriteRenderer spriteRenderer;
  Rigidbody2D body;

  public void FlipSprite(bool flip)
  {
    spriteRenderer.flipX = flip;
  }

  private void Start()
  {
    // find all characters
    characters = FindObjectsOfType<Controller>().Select(controller => controller.transform).ToArray();

    // get refs
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    body = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    if (!currentTarget) FindTarget();
    else ChaseTarget();
  }

  private void FindTarget()
  {
    Transform target;
    float distance;

    try
    {
      // find the closest character
      (target, distance) =
        characters
        // get their distances
        .Select(character => (character, Vector3.Distance(transform.position, character.position)))
        // order by the distance
        .OrderBy(((Transform transform, float distance) character) => character.distance)
        .First();
    }
    // If one of them is dead, dont' bother
    catch (MissingReferenceException)
    {
      return;
    }

    if (distance > range) return;

    currentTarget = target;

    // set animation
    animator.SetBool("Walking", true);
    ChaseTarget();
  }

  private void ChaseTarget()
  {
    // safeguard
    if (!currentTarget) return;

    // stop if it's not within range
    if (Vector3.Distance(transform.position, currentTarget.position) > range) {
      StopChasing();
      return;
    }

    int targetDirection = 1;

    // flip sprite if necessary
    if (currentTarget.position.x > transform.position.x) FlipSprite(true);
    else
    {
      FlipSprite(false);
      targetDirection = -1;
    }

    // move towards it
    float displacement = moveSpeed * Time.fixedDeltaTime * targetDirection;

    // Apply movement
    body.velocity = new Vector2(displacement, body.velocity.y);
  }

  private void StopChasing()
  {
    animator.SetBool("Walking", false);
    currentTarget = null;
  }
}
