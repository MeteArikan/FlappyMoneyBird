using UnityEngine;

[CreateAssetMenu(fileName = "New Medal", menuName = "ScriptableObjects/MedalSO")]
public class MedalSO : ScriptableObject
{   
    [SerializeField] private Sprite _medalSprite;
    [SerializeField] private int _medalMinScore;

    public Sprite MedalSprite => _medalSprite;
    public int MedalMinScore => _medalMinScore;

}
