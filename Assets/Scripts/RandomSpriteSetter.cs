using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSpriteSetter : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _renderer;
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        var rndSpriteIndex = Random.Range(0, _sprites.Length);
        _renderer.sprite = _sprites[rndSpriteIndex];
    }
}
