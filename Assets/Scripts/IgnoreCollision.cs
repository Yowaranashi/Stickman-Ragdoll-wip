using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    private void Start()
    {
        var colliders = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for(int k = i + 1; k < colliders.Length; k++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[k]);
            }
        }
    }
}
