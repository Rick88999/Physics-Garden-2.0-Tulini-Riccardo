using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AddOnErrorDialogueOneWay : MonoBehaviour
{
    [SerializeField] private List<GameObject> ways;

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.tag != "Player" && other.gameObject.tag != "PlayerHand") return;
            else
            {
                if (ways != null && ways.Count > 0)
                {
                    foreach (GameObject way in ways)
                    {
                        if (way != null) way.SetActive(true);
                    }
                }
            }
        }
    }
}


