using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManifestManager : MonoBehaviour
{
    [SerializeField] private List<Page> manifestPages;
    [SerializeField] private GameObject anyQuestPage;
    [SerializeField] private PinManifestController pinManifestController;

    private Page currentPage;

    [System.Serializable]
    private struct CheckField
    {
        public int idStep;
        public GameObject checkField;

        public bool isYallowAtStart;
    }
    [System.Serializable]
    private struct Page
    {
        public int idQuest;
        public GameObject page;

        public List<CheckField> checkFields;

    }


    public void questStarted(int idQuest)
    {
        foreach (Page page in manifestPages) 
            if (page.idQuest == idQuest)
            {
                currentPage = page;
                anyQuestPage.SetActive(false);
                page.page.SetActive(true);
                foreach (CheckField check in page.checkFields)
                {
                    if(check.idStep == 0)
                    {
                        Image image = check.checkField.GetComponent<Image>();
                        if(!check.isYallowAtStart)
                            image.color = Color.yellow;
                        pinManifestController.SetTitlePin(page.idQuest);
                        pinManifestController.SetDescriptionPin(page.idQuest, check.idStep);
                    }
                }
            }
    }

    public void stepCompleted(int idQuest, int idStep)
    {
        foreach (Page page in manifestPages)
        {
            if (page.idQuest == idQuest)
            {
                foreach (CheckField check in page.checkFields)
                {
                if (check.idStep == idStep)
                    {
                        Image image = check.checkField.GetComponent<Image>();
                        image.color = Color.green;
                    }
                    if (page.checkFields.Count > idStep + 1)
                    {
                        if(check.idStep == idStep + 1)
                        {
                            Image image = check.checkField.GetComponent<Image>();
                            image.color = Color.yellow;
                            pinManifestController.SetDescriptionPin(page.idQuest, check.idStep);
                        }
                    } 
                }   
            }
        }
    }

    public void questCompleted()
    {
        currentPage.page.SetActive(false);
        anyQuestPage.SetActive(true);
        pinManifestController.resetPin(currentPage.idQuest);
    }
}
