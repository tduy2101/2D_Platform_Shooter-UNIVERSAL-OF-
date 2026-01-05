using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    public static UiController Instance;

    [SerializeField] private Slider energySlider;
    [SerializeField] private TMP_Text energyText;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TMP_Text experienceText;

    public GameObject pausePanel;

    public void UpdateEnergy(float currentEnergy, float maxEnergy)
    {
        energySlider.value = currentEnergy;
        energySlider.maxValue = maxEnergy;
        energyText.text = $"{energySlider.value:F0} / {energySlider.maxValue:F0}";
    }

    public void UpdateHealth(float currentEnergy, float maxEnergy)
    {
        healthSlider.value = currentEnergy;
        healthSlider.maxValue = maxEnergy;
        healthText.text = $"{healthSlider.value:F0} / {healthSlider.maxValue:F0}";
    }

    public void UpdateExperience(float current, float max)
    {
        experienceSlider.value = current;
        experienceSlider.maxValue = max;
        experienceText.text = $"{experienceSlider.value:F0} / {experienceSlider.maxValue:F0}";
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
