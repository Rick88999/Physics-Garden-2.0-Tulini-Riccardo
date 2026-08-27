using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilanciaDuePiatti : MonoBehaviour
{
    [Header("Riferimenti Piatti")]
    [SerializeField] private Transform leftPlate;
    [SerializeField] private Transform rightPlate;

    [Header("Impostazioni Peso")]
    [SerializeField] private float leftPlateWeight;
    [SerializeField] private float rightPlateWeight = 0f;

    [Header ("Impostazioni Led")]
    [SerializeField] private GameObject led;
    [SerializeField] private Material red;
    [SerializeField] private Material green;

    [Header("Impostazioni Movimento")]
    [SerializeField] private float minY = 0.56f;
    [SerializeField] private float maxY = 1.6f;
   
    [SerializeField] private float speed = 3f;                 

    private Vector3 leftInitialPos;
    private Vector3 rightInitialPos;

    
    private List<GameObject> objectsOnRightPlate = new List<GameObject>();

    private void Start()
    {
        if (leftPlate != null) leftInitialPos = leftPlate.localPosition;
        if (rightPlate != null) rightInitialPos = rightPlate.localPosition;
        AggiornaPosizionePiatti();
    }

    public void ObjectEnter(GameObject obj)
    {
        AddObject(obj);
        AggiornaPosizionePiatti();
    }

    public void ObjectExit(GameObject obj)
    {
        RemoveObject(obj);
        AggiornaPosizionePiatti();

    }
    

    private void AddObject(GameObject obj)
    {
        if (!objectsOnRightPlate.Contains(obj))
        {
            objectsOnRightPlate.Add(obj);
            RecalculateRightWeight();
        }
    }

    private void RemoveObject(GameObject obj)
    {
        if (objectsOnRightPlate.Contains(obj))
        {
            objectsOnRightPlate.Remove(obj);
            RecalculateRightWeight();
        }
    }

    private void RecalculateRightWeight()
    {
        rightPlateWeight = 0f;
        foreach (GameObject obj in objectsOnRightPlate)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rightPlateWeight += rb.mass;
            }
        }
    }
    void Update()
    {
        bool result = AggiornaPosizionePiatti();
        if (result && led != null)
        {
            led.GetComponent<Renderer>().material = green;
        }
        else if (led != null)
        {
            led.GetComponent<Renderer>().material = red;
        }
    }

    private bool AggiornaPosizionePiatti()
    {
        float weightDifference = rightPlateWeight - leftPlateWeight;
        float t = Mathf.InverseLerp(-leftPlateWeight, leftPlateWeight, weightDifference);

        float rightTargetY = Mathf.Lerp(minY, maxY, 1 - t);
        float leftTargetY = Mathf.Lerp(minY, maxY, t);

        Vector3 targetRightPos = new Vector3(rightInitialPos.x, rightTargetY, rightInitialPos.z);
        Vector3 targetLeftPos = new Vector3(leftInitialPos.x, leftTargetY, leftInitialPos.z);

        rightPlate.localPosition = Vector3.Lerp(rightPlate.localPosition, targetRightPos, Time.deltaTime * speed);
        leftPlate.localPosition = Vector3.Lerp(leftPlate.localPosition, targetLeftPos, Time.deltaTime * speed);

        if(Mathf.Approximately(weightDifference, 0f))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }


}

