using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    public RectTransform canvas;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerManaBar;

    [SerializeField] private GameObject hud;
    
    [SerializeField] private GameObject pausePanel;            
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject gamewinPanel;

    [SerializeField] private Button firstButtonPause;
    [SerializeField] private Button firstButtonGameOver;
    [SerializeField] private Button firstButtonGameWin;

    [SerializeField] private TextMeshProUGUI dieByTimeMessageText;
    [SerializeField] private TextMeshProUGUI dieByDamageMessageText;

    private HealthSystem healthSystem;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        healthSystem = playerController.GetComponent<HealthSystem>();
    }

    private void Start()
    {
        playerController.PlayerOxygen.OnUpdateOxygen += UpdateOxygen;
        
        healthSystem.OnChangeHealth += UpdateLifeBar;

        GameManager.Instance.OnPauseStatusChange += UpdatePauseMenu;
        GameManager.Instance.OnGameOver += OpenGameoverMenu;
        GameManager.Instance.OnGameWin += OpenGamewinMenu;
    }

    private void OnDestroy()
    {
        playerController.PlayerOxygen.OnUpdateOxygen -= UpdateOxygen;
        
        healthSystem.OnChangeHealth -= UpdateLifeBar;

        GameManager.Instance.OnPauseStatusChange -= UpdatePauseMenu;
        GameManager.Instance.OnGameOver -= OpenGameoverMenu;
        GameManager.Instance.OnGameWin -= OpenGamewinMenu;
    }

    private void UpdateLifeBar(float current, float max)
    {
        playerHealthBar.fillAmount = current / max;
    }

    private void UpdateOxygen(float current, float max)
    {
        playerManaBar.fillAmount = current / max;
    }

    private void UpdatePauseMenu(bool value)
    {
        DisableAllMenus();

        firstButtonPause.Select();
        pausePanel.SetActive(value);
    }

    private void OpenGameoverMenu(bool dieByTime)
    {
        DisableAllMenus();

        firstButtonGameOver.Select();
     
        dieByTimeMessageText.gameObject.SetActive(dieByTime);
        dieByDamageMessageText.gameObject.SetActive(!dieByTime);
        
        gameoverPanel.SetActive(true);
    }

    private void OpenGamewinMenu()
    {
        DisableAllMenus();

        firstButtonGameWin.Select();
        gamewinPanel.SetActive(true);
    }

    private void DisableAllMenus()
    {
        pausePanel.SetActive(false);
        gameoverPanel.SetActive(false);
    }

    private void DisableHUD()
    {
        DisableAllMenus();
        hud.SetActive(false);
    }
}
