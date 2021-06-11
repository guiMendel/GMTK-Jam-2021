using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
  [SerializeField] float speed = 4f;

  public void Walk(Transform target, float movement)
  {
    float displacement = movement * speed * Time.deltaTime;
    
    // Apply movement
    target.Translate(displacement, 0, 0);
  }
}
