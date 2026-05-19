using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct SpawnListSelector
{
    public List<SpawnCard> spawnCards;
    public int currentIndex;
    public SpawnListSelector(List<SpawnCard> spawnCards)
    {
        this.spawnCards = spawnCards;
        this.currentIndex = 0;
    }
    public void NextSpawnCard()
    {
        if (currentIndex < spawnCards.Count)
        {
            currentIndex++;
            return;
        }
    }

    public void PreviousSpawnCard()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            return;
        }

    }

    public SpawnCard GetCurrentSpawnCard()
    {
        return spawnCards[currentIndex];
    }

    public bool HasSpawnCardNext()
    {
        return currentIndex < spawnCards.Count;
    }

    

    public bool HasSpawnCardPrevious()
    {
        return currentIndex > 0;
    }
    public bool HasSpawnCard() 
    { 
        return spawnCards[currentIndex] != null;
    }

    public int GetIndex()
    {
        return currentIndex;
    }
}

public class SequenceSpawnPlayer : MonoBehaviour
{
    [SerializeField] private List<SpawnCard> spawnCards;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private string[] titles;
    private SpawnListSelector spawnListSelector;

    private void Awake()
    {
        spawnListSelector = new SpawnListSelector(spawnCards);
    }

    public void NextSpawn()
    {
        if (spawnListSelector.HasSpawnCardNext())
        {
            spawnListSelector.NextSpawnCard();
            
            titleText.text = titles[spawnListSelector.GetIndex()]; 
        }
        else
        {
            Debug.Log("No more spawn cards available.");
        }
    }

    public void PreviousSpawn()
    {
        if (spawnListSelector.HasSpawnCardPrevious())
        {
            spawnListSelector.PreviousSpawnCard();
            titleText.text = titles[spawnListSelector.GetIndex()];
        }
        else
        {
            Debug.Log("No previous spawn cards available.");
        }
    }

    public void SpawnCurrentCard()
    {
        if (spawnListSelector.HasSpawnCard())
        {
            SpawnCard currentCard = spawnListSelector.GetCurrentSpawnCard();
            currentCard.SpawnFormulaCard();
        }
        else
        {
            Debug.Log("No spawn card available.");
        }
    }
}