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
    [SerializeField] private Image selectBorder;
     

    private SportType _switchType;
    private PlayerRunner _player;
    private SwitchButtonInitializer _switchButtonController;
    private SwitchButtonData _buttonData;

    public SportType SwitchType { get => _switchType; set => _switchType = value; }
    public Button SwitchButton { get => button; set => button = value; }
    public Image SelectBorder { get => selectBorder; set => selectBorder = value; }


    public void Init(PlayerRunner player, SportType newType, SwitchButtonInitializer buttonController, SwitchButtonData data)
    {
        _player = player;
        _switchButtonController = buttonController;
        _switchType = newType;
        _buttonData = data;
        text.text = _switchType.ToString();
        button.onClick.AddListener(() => SwitchAndDisable(_switchType));
    }

    public void SwitchAndDisable(SportType type)
    {
        _player.SwitchRunner(_switchType);

        foreach (var button in _switchButtonController.Switches.Where(s => s.SwitchType != _player.Type))
        {
            button.SwitchButton.enabled = true;
            button.SelectBorder.gameObject.SetActive(false);
        }

        selectBorder.gameObject.SetActive(true);
        button.enabled = false;
    }
    

}
