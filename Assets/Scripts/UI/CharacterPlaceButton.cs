using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlaceButton : MonoBehaviour
{
  // STATE
  GameObject placedBlob;
  Sprite originalSprite;

  // STORED REFS
  CharacterPlacer characterPlacer;
  SpriteRenderer spriteRenderer;

  public GameObject GetPlacedBlob() { return placedBlob; }

  public void RemoveBlob()
  {
    placedBlob = null;
    spriteRenderer.sprite = originalSprite;
  }

  public void PlaceCharacter()
  {
    Instantiate(placedBlob, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }

  private void Start()
  {
    characterPlacer = FindObjectOfType<CharacterPlacer>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    originalSprite = spriteRenderer.sprite;
  }

  private void OnMouseDown()
  {
    var blob = characterPlacer.GetSelectedBlob();

    // switch to this sprite
    if (blob)
    {
      RemoveBlobFromOtherPlaces(blob);
      placedBlob = blob;
      UseSpriteFrom(placedBlob);
    }
  }

  private void RemoveBlobFromOtherPlaces(GameObject blob)
  {
    var others = FindObjectsOfType<CharacterPlaceButton>();

    foreach (var place in others)
    {
      if (GameObject.ReferenceEquals(place.GetPlacedBlob(), blob))
      {
        place.RemoveBlob();
      }
    }
  }

  private void UseSpriteFrom(GameObject blob)
  {
    print(blob);
    spriteRenderer.sprite = blob.GetComponent<SpriteRenderer>().sprite;
  }
}
