using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
  [SerializeField] Sprite pressedSprite;
  [SerializeField] float startDelay = 0.5f;

  private void OnMouseDown()
  {
    if (!ReadyToGo()) return;
    StartCoroutine(Begin());
  }

  private bool ReadyToGo()
  {
    var placers = FindObjectsOfType<CharacterPlaceButton>();
    foreach (var placer in placers)
    {
      if (placer.GetPlacedBlob() == null) return false;
    }

    return true;
  }

  private IEnumerator Begin()
  {
    GetComponent<SpriteRenderer>().sprite = pressedSprite;

    yield return new WaitForSeconds(startDelay);

    // Instantiate characters
    var placers = FindObjectsOfType<CharacterPlaceButton>();
    foreach (var placer in placers) placer.PlaceCharacter();

    // Disable character picker
    Destroy(FindObjectOfType<CharacterPlacer>().gameObject);
  }
}
