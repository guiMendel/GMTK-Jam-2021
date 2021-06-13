using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  [SerializeField] float initialForce = 20f;
  [SerializeField] GameObject hitVFX;
  [SerializeField] float vfxDuration = 1f;

  // stored refs
  Rigidbody2D body;

  private void Awake()
  {
    body = GetComponent<Rigidbody2D>();
  }

  public void SetTarget(Vector3 target)
  {
    // Aim
    Vector3 thisPos = transform.position;
    target.x = target.x - thisPos.x;
    target.y = target.y - thisPos.y;
    float angle = Mathf.Atan2(target.y, target.x);
    // transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

    // Fire
    // body.AddForce(transform.up * initialForce);
    body.AddForce(new Vector2(Mathf.Cos(angle) * initialForce, Mathf.Sin(angle) * initialForce));
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    // vfx
    if (hitVFX)
    {
      var vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
      Destroy(vfx, vfxDuration);
    }

    // vanish
    Destroy(gameObject);
  }
}
