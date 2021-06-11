using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  [SerializeField] float initialForce = 20f;

  // stored refs
  Rigidbody2D body;

  private void Awake()
  {
    body = GetComponent<Rigidbody2D>();
  }

  public void SetTarget(Vector3 target)
  {
    // Aim

    Vector3 mouse_pos = Input.mousePosition;
    mouse_pos.z = 5.23f; //The distance between the camera and object
    Vector3 object_pos = Camera.main.WorldToScreenPoint(target);
    mouse_pos.x = mouse_pos.x - object_pos.x;
    mouse_pos.y = mouse_pos.y - object_pos.y;
    float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    // Fire
    body.AddForce(transform.forward * initialForce);
  }
}
