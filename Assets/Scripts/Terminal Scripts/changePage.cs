using UnityEngine;

public class changePage : MonoBehaviour
{
    [SerializeField] private GameObject pageNext;
    [SerializeField] private GameObject pageActual;
    [SerializeField] private CanvasLoader canvasLoader;

    private void ChangePage(string command)
    {
        if(command == "next")
        {
            if (pageNext != null)
            {

                pageNext.SetActive(true);
                pageActual.SetActive(false);
                canvasLoader.ChengeCanvasAttivo(pageNext);
            }
        }
    }

    public void ChangePageNext() => ChangePage("next");


}
