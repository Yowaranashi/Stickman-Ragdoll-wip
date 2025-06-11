using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName ="New Skins Database", menuName = "Skins/New Skins Database")]
public class SkinsDatabase : ScriptableObject
{
    public int Money = 100;
    [SerializeField] private CharacterSkin[] _skins;
    public CharacterSkin[] Skins => _skins;
    public CharacterSkin GetChosenSkin()
    {
        CharacterSkin skin = _skins[0];
        if(_skins.Any(t => t.Chosen))
            skin = _skins.Where(s => s.Chosen).First();
        return skin;
    }
    public void SetKeys()
    {
        for (int i = 0; i < _skins.Length; i++)
        {
            _skins[i].Key = "skin" + i;
        }
        Debug.Log("KEYS SETTED!");
    }
}
