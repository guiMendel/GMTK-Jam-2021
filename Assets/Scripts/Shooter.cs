using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
  [SerializeField] GameObject projectilePrefab;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetButtonDown("Fire1")) Fire();
  }

  private void Fire()
  {
    // Safeguard
    if (!projectilePrefab) throw new Exception("No projectile prefab set for the shooter");

    // Instantiate
    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

    // Set projectile target
    projectile.GetComponent<Projectile>().SetTarget(Input.mousePosition);
  }
}
