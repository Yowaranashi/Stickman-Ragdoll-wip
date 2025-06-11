using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    [SerializeField] private SkinsDatabase _skinsDatabase;
    [SerializeField] private SpriteRenderer _head, _body, _rightHand, _leftHand, _rightLeg, _leftLeg;
    private void Start()
    {
        var chosenSkin = _skinsDatabase.GetChosenSkin();
        ApplySkin(chosenSkin);
    }
    public void ApplySkin(CharacterSkin skin)
    {
        _head.sprite = skin.Head;
        _body.sprite = skin.Body;
        _leftHand.sprite = skin.HandLeft;
        _rightHand.sprite = skin.HandRight;
        _rightLeg.sprite = skin.LegRight;
        _leftLeg.sprite = skin.LegLeft;
    }
}
