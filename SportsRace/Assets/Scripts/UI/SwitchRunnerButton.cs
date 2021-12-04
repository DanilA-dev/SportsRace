using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchRunnerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;
     

    private SportType _switchType;
    private PlayerRunner _player;
    private Material _testMaterial;

    public SportType SwitchType { get => _switchType; set => _switchType = value; }
    public Material TestMaterial => _testMaterial;

    public void Init(PlayerRunner player, SportType newType, Material material)
    {
        _player = player;
        _switchType = newType;
        _testMaterial = material;
        text.text = _switchType.ToString();
        button.onClick.AddListener(() => _player.SwitchRunner(_switchType, _testMaterial));
    }

    

}
