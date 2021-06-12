using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
  [SerializeField] float jumpPower = 1000f;

  public void Jump(Controller controller)
  {
    controller.GetComponent<Rigidbody2D>().AddForce(
      Vector2.up * jumpPower * controller.GetTotalMass(),
      ForceMode2D.Impulse
    );
  }
}
