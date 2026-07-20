/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandMenuManager : MonoBehaviour
   
{
    [SerializeField] private List<GameObject> Modalita= new List<GameObject>();
    [SerializeField] private GameObject Menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void AtrattoreActivate()
    {
        
        foreach (GameObject modalita in Modalita)
        {
            if (modalita != Modalita[0])
            {
                modalita.SetActive(false);
            }
        }
        Modalita[0].SetActive(true);
        if (Menu != null) if (Menu.activeSelf) Menu.SetActive(false);
        
        return;
    }

    public void MassScannerActivate()
    {
        
        foreach (GameObject modalita in Modalita)
        {
            if (modalita != Modalita[1])
            {
                modalita.SetActive(false);
            }
        }
        Modalita[1].SetActive(true);
        if (Menu != null) if (Menu.activeSelf) Menu.SetActive(false);

        return;
    }
}*/
