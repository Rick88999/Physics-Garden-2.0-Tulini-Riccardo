using UnityEngine;

public abstract class Handler<T> : MonoBehaviour
{
    [SerializeField] protected GameObject target;

    public abstract void Perform(T data);
}
