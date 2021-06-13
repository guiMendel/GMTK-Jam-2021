using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeJudge : MonoBehaviour
{
  public static float snapGap = 0.2f;
  [SerializeField] float nextLevelDelay = 2f;

  // STORED REFS
  LevelHandler levelHandler;

  // state

  List<(Attacher, Attacher)> awaitingMerge;
  // number of successful merges
  int mergeCount = 0;

  public void MergeRequest(Attacher source, Attacher target)
  {
    // Check to see if it matches a previous request
    (Attacher foundSource, Attacher foundTarget) = awaitingMerge.Find((
      (Attacher source, Attacher target) storedRequest
    ) =>
    {
      return
        GameObject.ReferenceEquals(source, storedRequest.target) &&
        GameObject.ReferenceEquals(target, storedRequest.source);
    });

    if (foundSource)
    {
      // merge them
      Merge(source, target);

      // remove from waiting to merge
      awaitingMerge.Remove((foundSource, foundTarget));
    }
    // if not, wait for a matching request
    else awaitingMerge.Add((source, target));
  }

  private void Merge(Attacher attacher1, Attacher attacher2)
  {
    // make sure theyre not already part of the same body
    if (GameObject.ReferenceEquals(attacher1.GetController(), attacher2.GetController())) return;

    // take the one with higher priority
    (Attacher merger, Attacher target) = SortByPriority(attacher1, attacher2);

    // attach the other one to it
    target.AttachTo(merger);

    // count the merge
    mergeCount++;
    if (mergeCount >= 3) StartCoroutine(CompleteLevel());
  }

  private IEnumerator CompleteLevel()
  {
    yield return new WaitForSeconds(nextLevelDelay);
    levelHandler.NextLevel();
  }

  private void Start()
  {
    // singleton
    if (FindObjectsOfType<MergeJudge>().Length > 1)
    {
      enabled = false;
      Destroy(gameObject);
      return;
    }
    else
    {
      DontDestroyOnLoad(gameObject);
    }

    awaitingMerge = new List<(Attacher, Attacher)>();
    levelHandler = FindObjectOfType<LevelHandler>();
  }

  private (Attacher merger, Attacher target) SortByPriority(Attacher attacher1, Attacher attacher2)
  {
    print("Collision between " + attacher1.GetPriority() + " and " + attacher2.GetPriority());
    // print("attacher 1: " + attacher1.GetPriority() + ", attacher 2: " + attacher2.GetPriority());

    // take controllers
    Controller controller1 = attacher1.GetController();
    Controller controller2 = attacher2.GetController();

    // characters below have higher priority
    float attacher1Height = controller1.transform.position.y, attacher2Height = controller2.transform.position.y;

    // print("height1: " + attacher1Height + ", height2: " + attacher2Height);

    if (attacher1Height > attacher2Height) return (attacher2, attacher1);
    else if (attacher1Height < attacher2Height) return (attacher1, attacher2);

    // moving characters have second higher priority
    bool attacher1Moving = controller1.IsMoving(), attacher2Moving = controller2.IsMoving();

    // print("attacher1Moving: " + attacher1Moving + ", attacher2Moving: " + attacher2Moving);

    if (!attacher1Moving && attacher2Moving) return (attacher2, attacher1);
    else if (attacher1Moving && !attacher2Moving) return (attacher1, attacher2);

    // fallback to basic priorities
    if (attacher1.GetPriority() > attacher2.GetPriority())
    {
      return (attacher1, attacher2);
    }
    return (attacher2, attacher1);
  }

}
