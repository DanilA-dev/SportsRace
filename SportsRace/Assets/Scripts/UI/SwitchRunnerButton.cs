using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchRunnerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;
     

    private SportType _switchType;
    private PlayerRunner _player;
    private SwitchButtonInitializer _switchButtonController;

    public SportType SwitchType { get => _switchType; set => _switchType = value; }
    public Button SwitchButton { get => button; set => button = value; }


    public void Init(PlayerRunner player, SportType newType, SwitchButtonInitializer buttonController)
    {
        _player = player;
        _switchButtonController = buttonController;
        _switchType = newType;
        text.text = _switchType.ToString();
        button.onClick.AddListener(() => SwitchAndDisable(_switchType));
    }

    public void SwitchAndDisable(SportType type)
    {
        _player.SwitchRunner(_switchType);

        foreach (var button in _switchButtonController.Switches.Where(s => s.SwitchType != _player.Type))
            button.SwitchButton.interactable = true;

        button.interactable = false;
    }
    

}
