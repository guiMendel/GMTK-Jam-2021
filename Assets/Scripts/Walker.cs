using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
  [SerializeField] float speed = 4f;
  [SerializeField] bool walksRight = false;
  [SerializeField] bool walksLeft = false;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Walk();
  }

  private void Walk()
  {
    // Get the movement
    float movement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

    // Limit the movement direction
    // Right movement
    if (!walksRight && movement > 0) return;

    // Left movement
    if (!walksLeft && movement < 0) return;

    // Apply movement
    transform.Translate(movement, 0, 0);
  }
}
