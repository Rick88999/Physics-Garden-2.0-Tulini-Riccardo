using TMPro;
using UnityEngine;

public abstract class CardBase : MonoBehaviour
{
    [SerializeField] protected string nome;
    [SerializeField] protected string details;
    [SerializeField] protected TextMeshProUGUI textTarghetta;

    public virtual string GetName() => nome;

    public virtual void SetName(string name) => nome = name;
    
    public virtual string GetDetails() => details;

    public virtual void SetDetails(string details) => this.details = details;
    
    public abstract void SetTextTarghetta();
}
