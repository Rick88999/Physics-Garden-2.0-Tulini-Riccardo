using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryButton> formulaButtons;
    [SerializeField] private List<InventoryButton> constantButtons;
    [SerializeField] private List<InventoryButton> toolsButtons;

    public void SetActiveFormulaButtons(SaveInventoryButton[] oldState)
    {
        for (int i = 0; i < formulaButtons.Count; i++)
        {
            formulaButtons[i].SetActive(oldState[i].state);
        }
    }

    public void SetActiveConstantButtons(SaveInventoryButton[] oldState)
    {
        for (int i = 0; i < constantButtons.Count; i++)
        {
            constantButtons[i].SetActive(oldState[i].state);
        }
    }

    public void SetActiveToolsButtons(SaveInventoryButton[] oldState)
    {
        for (int i = 0; i < toolsButtons.Count; i++)
        {
            toolsButtons[i].SetActive(oldState[i].state);
        }
    }

    public SaveInventoryButton[] SaveFormulaButtons()
    {         
        SaveInventoryButton[] saveButtons = new SaveInventoryButton[formulaButtons.Count];
        for (int i = 0; i < formulaButtons.Count; i++)
        {
            saveButtons[i] = new SaveInventoryButton
            {
                index = i,
                state = formulaButtons[i].GetState()
            };
        }
        return saveButtons;
    }

    public SaveInventoryButton[] SaveConstantButtons()
    {
        SaveInventoryButton[] saveButtons = new SaveInventoryButton[constantButtons.Count];
        for (int i = 0; i < constantButtons.Count; i++)
        {
            saveButtons[i] = new SaveInventoryButton
            {
                index = i,
                state = constantButtons[i].GetState()
            };
        }
        return saveButtons;
    }

    public SaveInventoryButton[] SaveToolsButtons()
    {
        SaveInventoryButton[] saveButtons = new SaveInventoryButton[toolsButtons.Count];
        for (int i = 0; i < toolsButtons.Count; i++)
        {
            saveButtons[i] = new SaveInventoryButton
            {
                index = i,
                state = toolsButtons[i].GetState()
            };
        }
        return saveButtons;
    }

    public void AwakeButtons()
    {
        foreach (InventoryButton button in formulaButtons)
        {
            button.AwakeBotton();
        }
        foreach (InventoryButton button in constantButtons)
        {
            button.AwakeBotton();
        }
        foreach (InventoryButton button in toolsButtons)
        {
            button.AwakeBotton();
        }
    }
}
