using UnityEngine;

public class CarrucolaController : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float baseSpeed;
    [SerializeField] private bool downToTop = false;
    [SerializeField] private bool topToDown = false;


    private bool isMoving = false;
    private GameObject carrucola;
   
    private float minSpeed = 0.1f;
    

    void Start() => carrucola = gameObject;

    /*vf = sqrt(2*(L-mgh)/m) => vf = sqrt(2*((1.12*mgh)-mgh)/m) => vf = sqrt(2*0.12*gh)
    La velocita finale della carrucola dipendera 
    solo dall'altezza e dal fattore di potenziamento nel lavoro rispetto all'energia potenziale.
    Questo è dato dal fatto che per salire il Lavoro in input deve essere maggiore 
    all'energia potenziale e la velocita sarà datadalla differenza tra le 2. 
    Sono partito da un fattore di 1.12 e ho ottenuto con altezza h = 8-2.4=5.6 calcolando la pedana di legno,
    una vf = 3.63*/

    private void Update()
    {
        if(downToTop)
        {
            float t01 = Vector3.Distance(carrucola.transform.position, endPoint.position) / 10f;
            float speed01 = Mathf.Lerp(minSpeed, baseSpeed, 1f - Mathf.Pow(1f - t01, 3f));
            carrucola.transform.position = Vector3.MoveTowards(carrucola.transform.position, endPoint.position, speed01 * Time.deltaTime);
            isMoving = true;
            if (Vector3.Distance(carrucola.transform.position, endPoint.position) < 0.1f)
            {
                t01 = 0f;
                speed01 = 0f;
                isMoving = false;
                downToTop = false;
            }
        }
        else if(topToDown)
        {
            float t02 = Vector3.Distance(carrucola.transform.position, startPoint.position) / 10f;
            float speed02 = Mathf.Lerp(minSpeed, baseSpeed, 1f - Mathf.Pow(1f - t02, 3f));
            carrucola.transform.position = Vector3.MoveTowards(carrucola.transform.position, startPoint.position, speed02 * Time.deltaTime);
            isMoving = true;

            if (Vector3.Distance(carrucola.transform.position, startPoint.position) < 0.1f)
            {
                t02 = 0f;
                speed02 = 0f;
                isMoving = false;
                topToDown = false;
            }
        }
    }
    public void MovetoTop()
    {
        if(!isMoving)
        {
            downToTop = true;
        }
    }
    public void MovetoDown()
    {
        if(!isMoving)
        {
            topToDown = true;
        }
    }
}
