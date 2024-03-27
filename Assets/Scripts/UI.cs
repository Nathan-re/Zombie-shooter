using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    /// <summary>
    /// Le conteneurs de balles restantes
    /// </summary>
    public RectTransform healthBarContainer;
    private float healthBarContainerWidth;
    private RectTransform healthBar;

    /// <summary>
    /// Conteneur contenant les TextMeshPro d'information sur les ennemis
    /// </summary>
    public GameObject ennemiesInfoContainer;
    private TextMeshProUGUI waveNoTmp;
    private TextMeshProUGUI killsTmp;
    private TextMeshProUGUI remainingTmp;

    /// <summary>
    /// TextMeshPro contenant l'indicateur de balles restantes
    /// </summary>
    public GameObject bulletsTmpContainer;
    private TextMeshProUGUI bulletsTmp;

    public float health = 0;
    public float maxHealth = 0;

    public int waveNo = 0;
    public int kills = 0;
    public int ennemiesRemaining = 0;
    
    public int bullets = 0;
    public int maxBullets = 0;


    void Start()
    {
        healthBarContainerWidth = healthBarContainer.rect.width;
        healthBar = healthBarContainer.transform.Find("Health").GetComponent<RectTransform>();
        waveNoTmp = ennemiesInfoContainer.transform.Find("WaveNo").GetComponent<TextMeshProUGUI>();
        killsTmp = ennemiesInfoContainer.transform.Find("Kills").GetComponent<TextMeshProUGUI>();
        bulletsTmp = bulletsTmpContainer.GetComponent<TextMeshProUGUI>();
        remainingTmp = ennemiesInfoContainer.transform.Find("Remaining").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UpdateInfo();
    }

    void UpdateInfo()
    {
        Debug.Log($"{this.health} {this.maxHealth} {this.health/this.maxHealth*500f}");
        // Adapter la taille de la barre en fonction de la vie restante
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health / maxHealth * 500f);

        // Compteur de balles
        bulletsTmp.text = $"{bullets}/{maxBullets}";
        if (bullets == 0) bulletsTmp.color = Color.red;
        else bulletsTmp.color = Color.white;

        // Ennemis
        waveNoTmp.text = $"Vague n°{waveNo+1}";
        killsTmp.text = $"{kills} kills";
        remainingTmp.text = $"{ennemiesRemaining} restant";
    }
}
