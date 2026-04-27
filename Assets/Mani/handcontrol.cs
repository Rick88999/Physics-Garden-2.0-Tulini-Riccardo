using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandControl : MonoBehaviour
{
    public InputActionReference gripInput;
    public InputActionReference triggerInput;

    
    public GameObject rayInteractorObjectAttrattore;
    public GameObject rayInteractorObjectPointer;
    [SerializeField] private List<GameObject> Modalita = new List<GameObject>();

    private Animator animator;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisualAtrattore;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisualPointer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (rayInteractorObjectAttrattore != null && rayInteractorObjectPointer != null)
        {
            lineVisualAtrattore = rayInteractorObjectAttrattore.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>();
            lineVisualPointer = rayInteractorObjectPointer.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>();
        }
    }

    void Update()
    {
        if (!animator) return;

        float grip = gripInput.action.ReadValue<float>();
        float trigger = triggerInput.action.ReadValue<float>();

        animator.SetFloat("Grip", grip);
        animator.SetFloat("Trigger", trigger);

        if (Modalita[0].activeSelf)
        {
            if (lineVisualAtrattore != null) lineVisualAtrattore.enabled = (trigger < 0.1f);
                
        }

        if (Modalita[1].activeSelf)
        {

            Modalita[1].GetComponent<LineRenderer>().enabled = (trigger < 0.1f);

        }
        if(Modalita[2].activeSelf)
        {

            if (lineVisualPointer != null) lineVisualPointer.enabled = (trigger < 0.1f);

        }
    }
}