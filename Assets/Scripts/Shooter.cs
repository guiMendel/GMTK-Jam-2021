using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
  [SerializeField] GameObject projectilePrefab;

  public void Fire()
  {
    // Safeguard
    if (!projectilePrefab) throw new Exception("No projectile prefab set for the shooter");

    // Instantiate
    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

    // Set projectile target
    projectile.GetComponent<Projectile>().SetTarget(MousePosition());
  }

  private static Vector3 MousePosition()
  {
    return Camera.main.ScreenToWorldPoint(Input.mousePosition);
  }
}
