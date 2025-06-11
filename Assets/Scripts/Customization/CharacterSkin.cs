using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skins/New Skin")]
public class CharacterSkin : ScriptableObject
{
    public string Key;
    [SerializeField] private Sprite _head, _body, _handLeft, _handRight, _legLeft, _legRight;
    [SerializeField] private bool _openForAd;
    [SerializeField] private int _cost = 100;
    public bool Opened, Chosen;
    public bool OpenForAd => _openForAd;
    public int Cost => _cost;
    public Sprite Head => _head;
    public Sprite Body => _body;
    public Sprite HandLeft => _handLeft;
    public Sprite HandRight => _handRight;
    public Sprite LegLeft => _legLeft;
    public Sprite LegRight => _legRight;
}
