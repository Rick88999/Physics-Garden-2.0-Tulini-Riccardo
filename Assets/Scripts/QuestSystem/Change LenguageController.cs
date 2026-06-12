using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLenguageController : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] private List<GameObject> italianTexts;
    [SerializeField] private List<GameObject> engTexts;

    private void Start()
    {
        gm = GetComponent<GameManager>();
    }

    private void setArray(List<GameObject> array, bool value)
    {
        foreach (GameObject text in array)
        {
            text.SetActive(value);
        }
    }

    private void SetLenguage(Lenguage lenguage)
    {
        switch (lenguage)
        {
            case Lenguage.Italian:
                setArray(italianTexts, true);
                setArray(engTexts, false);
                gm.SetLenguage(Lenguage.Italian);
                break;
            case Lenguage.English:
                setArray(italianTexts, false);
                setArray(engTexts, true);
                gm.SetLenguage(Lenguage.English);
                break;
        }
    }

    public void SetItalian() => SetLenguage(Lenguage.Italian);
    public void SetEnglish() => SetLenguage(Lenguage.English);
}
