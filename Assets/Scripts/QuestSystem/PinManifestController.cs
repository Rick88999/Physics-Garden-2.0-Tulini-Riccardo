using System.Collections.Generic;
using UnityEngine;

public class PinManifestController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<RefQuest> questDescriptions;

    [System.Serializable]
    private struct RefQuest
    {
        public GameObject italianTitle;
        public GameObject englishTitle; 
       public Description description;
    }

    [System.Serializable]
    private struct Description
    {
        public List<GameObject> italianDescriptions;
        public List<GameObject> englishDescriptions;
    }
    
    [SerializeField] private GameObject defaultTitle;
    private Language actualLanguage;

    public void SetTitlePin(int index)
    {
        actualLanguage = gameManager.GetLanguage();
        if (actualLanguage == Language.Italian)
        {
            for (int i = 0; i < questDescriptions.Count; i++)
            {
                if(questDescriptions[i].italianTitle != null)
                    questDescriptions[i].italianTitle.SetActive(i == index);
                
                if(i == index) defaultTitle.SetActive(false);
            }
        }
        else if (actualLanguage == Language.English)
        {
            for (int i = 0; i < questDescriptions.Count; i++)
            {
                if(questDescriptions[i].englishTitle != null)
                    questDescriptions[i].englishTitle.SetActive(i == index);

                if(i == index) defaultTitle.SetActive(false);
            }

        }
    }

    public void SetDescriptionPin(int fatherIndex, int index)
    {
        actualLanguage = gameManager.GetLanguage();
        if (actualLanguage == Language.Italian)
        {
            for (int i = 0; i < questDescriptions[fatherIndex].description.italianDescriptions.Count; i++)
            {
                questDescriptions[fatherIndex].description.italianDescriptions[i].SetActive(i == index);
            }
        }
        else if (actualLanguage == Language.English)
        {
            for (int i = 0; i < questDescriptions[fatherIndex].description.englishDescriptions.Count; i++)
            {
                questDescriptions[fatherIndex].description.englishDescriptions[i].SetActive(i == index);
            }
        }
    }

    public void resetPin(int fatherIndex)
    {
        actualLanguage = gameManager.GetLanguage();
        if (actualLanguage == Language.Italian)
        {
            questDescriptions[fatherIndex].italianTitle.SetActive(false);
            for (int i = 0; i < questDescriptions[fatherIndex].description.italianDescriptions.Count; i++)
            {
                questDescriptions[fatherIndex].description.italianDescriptions[i].SetActive(false);
            }
            defaultTitle.SetActive(true);
        }
        else if (actualLanguage == Language.English)
        {
            questDescriptions[fatherIndex].englishTitle.SetActive(false);
            for (int i = 0; i < questDescriptions[fatherIndex].description.englishDescriptions.Count; i++)
            {
                questDescriptions[fatherIndex].description.englishDescriptions[i].SetActive(false);
            }
            defaultTitle.SetActive(true);
        }
    }
   


    
}
