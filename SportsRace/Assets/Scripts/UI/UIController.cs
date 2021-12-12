using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIPanelType
{
    None, Menu, Core, Settings, Store, Rewards, Win, Lose, Pause
}

public class UIController : MonoBehaviour
{
    public static UIController Instance;


    [SerializeField] private TMP_Text sessionCoins;
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
        sessionCoins.text = "";
        GameController.OnSessionScoreChange += UpdateSessionScore;
    }

    public void OnClickToMenu()
    {
        GameController.CurrentState = GameState.Menu;
        GameController.OnSessionScoreChange -= UpdateSessionScore;
    }

    private void UpdateSessionScore(int amount)
    {
        sessionCoins.SetText(amount.ToString());
    }

    public void OnStore()
    {
        TurnOnPanel(UIPanelType.Store);
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        TurnOnPanel(UIPanelType.Pause);
    }

    public void OnUnPause()
    {
        Time.timeScale = 1;
        TurnOnPanel(UIPanelType.Core);
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