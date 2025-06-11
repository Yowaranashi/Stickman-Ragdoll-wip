using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    [SerializeField] private SceneSwitcher _sceneSwitcher;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            _sceneSwitcher.Restart();
        }
        else if(collision.TryGetComponent(out Enemy enemy))
        {
            Destroy(enemy.gameObject);
        }
    }
}
