using UnityEngine;

public class ChangePageStartMenu : MonoBehaviour
{
    [SerializeField] private GameObject pageNext;
    [SerializeField] private GameObject pageActual;
    public void ChangePage()
    {
        if (pageNext != null)
        {
            pageNext.SetActive(true);
            pageActual.SetActive(false);
                
        }
    }
}
