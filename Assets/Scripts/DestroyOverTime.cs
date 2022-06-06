using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] float lifetime;

    private void OnEnable() => Destroy(gameObject, lifetime);
}
