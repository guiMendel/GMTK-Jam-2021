using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BadMushroom : MonoBehaviour
{
  [Tooltip("How far the shroom can shoot")]
  [SerializeField] float range = 5f;

  [Tooltip("How often the shroom can shoot, in seconds")]
  [SerializeField] float cooldown = 1f;

  [Tooltip("The projectile this shroom shoots")]
  [SerializeField] Projectile projectilePrefab;

  [Tooltip("Where the projectiles come from")]
  [SerializeField] Transform shootFrom;

  [Tooltip("Plays on death")]
  [SerializeField] GameObject deathVFX;

  [Tooltip("VFX duration")]
  [SerializeField] float vfxDuration = 1f;

  // STATE

  // all the characters this shroom can shoot at
  Transform[] characters;
  // the target its shooting at right now
  Transform currentTarget;
  // if his cooldown is up
  bool readyToShoot = true;

  // STORED REFS

  Animator animator;
  SpriteRenderer spriteRenderer;

  public void Shoot()
  {
    // Safeguard
    if (!projectilePrefab) throw new Exception("No projectile prefab set for the mushroom");
    if (!currentTarget) return;

    // Instantiate
    Projectile projectile = Instantiate(projectilePrefab, shootFrom.position, Quaternion.identity);

    // Set projectile target
    projectile.SetTarget(currentTarget.position);

    // forget this target
    currentTarget = null;
  }

  public void FlipSprite(bool flip)
  {
    spriteRenderer.flipX = flip;
  }

  private void Start()
  {
    // find all characters
    characters = FindObjectsOfType<Controller>().Select(controller => controller.transform).ToArray();
    print(characters);

    // get refs
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    FindTarget();
  }

  private void FindTarget()
  {
    // if still on cooldown, stop
    if (!readyToShoot) return;

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

    // stop if it's not within range
    if (distance > range) return;

    AnimationShoot(target);
  }

  private void AnimationShoot(Transform target)
  {
    // shoot
    currentTarget = target;
    animator.SetTrigger("Shoot");

    // count cooldown
    StartCoroutine(CountCooldown());

    // flip sprite if necessary
    if (currentTarget.position.x > transform.position.x) FlipSprite(true);
    else FlipSprite(false);
  }

  private IEnumerator CountCooldown()
  {
    readyToShoot = false;

    // wait cooldown
    yield return new WaitForSeconds(cooldown);

    readyToShoot = true;
  }

  private void Die()
  {
    // vfx
    if (deathVFX)
    {
      GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
      Destroy(vfx, vfxDuration);
    }

    Destroy(gameObject);
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.GetComponent<Projectile>()) Die();
  }
}
