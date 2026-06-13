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

    private void SetLenguage(Language lenguage)
    {
        switch (lenguage)
        {
            case Language.Italian:
                setArray(italianTexts, true);
                setArray(engTexts, false);
                gm.SetLanguage(Language.Italian);
                break;
            case Language.English:
                setArray(italianTexts, false);
                setArray(engTexts, true);
                gm.SetLanguage(Language.English);
                break;
        }
    }

    public void SetItalian() => SetLenguage(Language.Italian);
    public void SetEnglish() => SetLenguage(Language.English);
}
