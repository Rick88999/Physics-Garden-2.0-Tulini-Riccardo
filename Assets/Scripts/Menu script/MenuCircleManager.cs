using UnityEngine;

public class MenuCircleManager : MonoBehaviour
{
    public GameObject HandMenusRight;
    public GameObject HandMenusLeft;
    public GameObject[] buttonsRight;
    public GameObject[] buttonsLeft;
    public void activateMenus() 
    {
        HandMenusRight.GetComponent<HandMenu>().updateButton();
        HandMenusLeft.GetComponent<HandMenu>().updateButton();
    }

    public void enableAttrattore()
    {
        buttonsRight[0].SetActive(true);
        buttonsLeft[0].SetActive(true);
    }
    
}

