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

  // state

  // all the characters this shroom can shoot at
  Transform[] characters;
  // if his cooldown is up
  bool readyToShoot = true;

  private void Start()
  {
    // find all characters
    characters = FindObjectsOfType<Controller>().Select(controller => controller.transform).ToArray();
    print(characters);
  }

  private void Update()
  {
    FindTarget();
  }

  private void FindTarget()
  {
    // if still on cooldown, stop
    if (!readyToShoot) return;

    // find the closest character
    (Transform target, float distance) =
      characters
      // get their distances
      .Select(character => (character, Vector3.Distance(transform.position, character.position)))
      // order by the distance
      .OrderBy(((Transform transform, float distance) character) => character.distance)
      .First();

    // stop if it's not within range
    if (distance > range) return;

    // shoot
    ShootAt(target);
  }

  private void ShootAt(Transform target)
  {
    // Safeguard
    if (!projectilePrefab) throw new Exception("No projectile prefab set for the mushroom");

    // Instantiate
    Projectile projectile = Instantiate(projectilePrefab, shootFrom.position, Quaternion.identity);

    // Set projectile target
    projectile.SetTarget(target.position);

    // count cooldown
    StartCoroutine(CountCooldown());
  }

  private IEnumerator CountCooldown()
  {
    readyToShoot = false;

    // wait cooldown
    yield return new WaitForSeconds(cooldown);

    readyToShoot = true;
  }
}
