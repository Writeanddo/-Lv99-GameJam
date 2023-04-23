using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject hudIngame;
    [SerializeField] private GameObject hudPowerUP;
    [SerializeField] private GameObject hudWin;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button quitToMenuButton;

    public GameObject painelChoiceCards;


    /// <summary>
    /// Interface do sistema de vida
    /// </summary>
    private IDamageable playerHealthSystem;

    private void Start()
    {
        DisableAll();

        backgroundButton.onClick.AddListener(HandlePauseUIButton);
        tryAgainButton.onClick.AddListener(TryAgain);
        quitToMenuButton.onClick.AddListener(QuitToMenu);

        GameManager.Instance.OnPauseStatusChange += HandlePauseUI;
        //GameManager.Instance.OnDead += OpenDeathScreen;
        GameManager.Instance.OnGameWin += OpenGameWin;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPauseStatusChange -= HandlePauseUI;
        //GameManager.Instance.OnDead -= OpenDeathScreen;
        GameManager.Instance.OnGameWin -= OpenGameWin;
    }

    private void OpenGameWin()
    {
        DisableAll();
        hudWin.SetActive(true);
    }

    private void HandlePauseUIButton()
    {
        GameManager.Instance.PauseResume();
    }

    public void TryAgain()
    {
        SceneLoader.Instance.ReloadScene();
    }

    public void QuitToMenu()
    {
        SceneLoader.Instance.LoadTitle();
    }

    /// <summary>
    /// Desabilita ou habilita o painel de pause de acordo com o valor
    /// </summary>
    /// <param name="value">Estado de pausa</param>
    private void HandlePauseUI(bool value)
    {
        pausePanel.SetActive(value);

        if (value && backgroundButton)
            backgroundButton.Select();
    }

    /// <summary>
    /// Atualiza a barra de vida do player na UI
    /// </summary>
    /// <param name="current">Valor atual</param>
    /// <param name="max">Valor maximo</param>
    private void UpdateHealthBar(float current, float max)
    {
        var percent = current / max;
    }

    private void OpenDeathScreen()
    {
        OpenDeathScreen(null);
    }

    /// <summary>
    /// Abre o painel de morte
    /// </summary>
    private void OpenDeathScreen(IDamageable instance)
    {
        DisableAll();
        hudIngame.SetActive(false);
        deathPanel.SetActive(true);
    }


    /// <summary>
    /// Desativa todos os paineis
    /// </summary>
    private void DisableAll()
    {
        pausePanel.SetActive(false);
        deathPanel.SetActive(false);

    }
}
