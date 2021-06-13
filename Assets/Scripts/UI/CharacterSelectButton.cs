using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectButton : MonoBehaviour
{
  [SerializeField] GameObject blobPrefab;

  CharacterPlacer characterPlacer;

  private void Start()
  {
    characterPlacer = FindObjectOfType<CharacterPlacer>();
  }

  private void OnMouseDown()
  {
    print(blobPrefab.name);
    characterPlacer.SelectBlob(blobPrefab);
    Highlight();
  }

  private void Highlight()
  {
    var others = FindObjectsOfType<CharacterSelectButton>();

    foreach (var other in others)
    {
      other.GetComponent<SpriteRenderer>().color = new Color32(87, 87, 87, 255);
    }

    GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
  }
}
