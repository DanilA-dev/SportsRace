using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIPanelType
{
    None, Menu, Core, Settings, Store, Rewards, Win, Lose
}

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private TMP_Text playerSpeedText;
    [SerializeField] private List<UIPanel> panels = new List<UIPanel>();

    public void Awake()
    {
        #region Singleton

        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);

        #endregion
    }

    private void Start()
    {
        SpeedChange(GameController.Instance.Runners[0].DefaultSpeed);
    }
    private void SpeedChange(float obj)
    {
        playerSpeedText.SetText("Speed : " + obj);
    }

    public static void TurnOnPanel(UIPanelType type)
    {
        for (int i = 0; i < Instance.panels.Count; i++)
        {
            if (Instance.panels[i].Type == type)
                Instance.panels[i].Toogle(true);
            else
                Instance.panels[i].Toogle(false);
        }
    }

    public static void TurnOffPanel(UIPanelType type)
    {
        for (int i = 0; i < Instance.panels.Count; i++)
        {
            if (Instance.panels[i].Type == type)
                Instance.panels[i].Toogle(false);
        }
    }

    public void OnClickToCore()
    {
        GameController.CurrentState = GameState.Core;
        PlayerRunner.OnSpeedChange += SpeedChange;
    }

    public void OnClickToMenu()
    {
        GameController.CurrentState = GameState.Menu;
        PlayerRunner.OnSpeedChange -= SpeedChange;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

[System.Serializable]
public class UIPanel
{
    [SerializeField] private string name;
    [SerializeField] private UIPanelType type;
    [SerializeField] private GameObject panelObj;
    public UIPanelType Type => type;

    public void Toogle(bool isOn)
    {
        panelObj.SetActive(isOn);
    }
}