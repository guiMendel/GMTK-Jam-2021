using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlacer : MonoBehaviour
{
  GameObject selectedBlobPrefab;

  public void SelectBlob(GameObject blob) { selectedBlobPrefab = blob; }

  public GameObject GetSelectedBlob()
  {
    GameObject blob = selectedBlobPrefab;

    selectedBlobPrefab = null;
    RemoveHighlight();

    return blob;
  }

  private void RemoveHighlight()
  {
    var buttons = FindObjectsOfType<CharacterSelectButton>();

    foreach (var button in buttons)
    {
      button.GetComponent<SpriteRenderer>().color = new Color32(87, 87, 87, 255);
    }
  }

}
