using UnityEngine;

public class TriggerStep : MonoBehaviour
{
    private bool alreadyTriggered = false;
    private enum StepAction
    {
        StartNewStep,
        EndCurrentStep
    }
    [SerializeField] private Step step;
    [SerializeField] private string triggerTag;
    [SerializeField] private StepAction stepAction;
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag(triggerTag))
      {
         if (step != null && !alreadyTriggered)
         {
            alreadyTriggered = true;
            switch (stepAction)
            {
               case StepAction.StartNewStep:
                  step.startStep();
                  break;
               case StepAction.EndCurrentStep:
                  step.endStep();
                  break;
               default:
                  Debug.LogError("TriggerStep: Both startNewStep and endCurrentStep are set to the same value. Please set one to true and the other to false.");
                  break;
            }
         }
      }
   }
}
