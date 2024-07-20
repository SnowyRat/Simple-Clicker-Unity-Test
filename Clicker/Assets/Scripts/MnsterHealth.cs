using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MnsterHealth : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI monsterNameText;
    private string monsterName;

    private void Start()
    {
        slider = GetComponent<Slider>();
        SetSliderVisibility(false);
    }

    public void SetMonsterName(string name)
    {
        monsterName = name;
    }

    public void UpdateSliderValue(double currentValue, double maxValue)
    {
        currentValue = Mathf.Max((float)currentValue, 0); // Ensure current value doesn't go below zero
        slider.value = (float)(currentValue / maxValue);
        hpText.text = currentValue.ToString("F0") + " / " + maxValue.ToString("F0");
        Debug.Log("Working1");
        Debug.Log(monsterName.ToString());
        monsterNameText.text = monsterName;
        Debug.Log("Working1");
        if (currentValue <= 0) // Check if enemy health is zero or below
        {
            Debug.Log("Working3");
            // SetSliderVisibility(false); // Hide the slider when the enemy dies
        }
        else if (!IsSliderVisible())
        {
            Debug.Log("Working2");
            SetSliderVisibility(true); // Show the slider if it's not active
        }
    }

    public void SetSliderVisibility(bool isVisible)
    {
        CanvasGroup canvasGroup = slider.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isVisible ? 1 : 0; // Set the alpha to show/hide the slider
            // canvasGroup.interactable = isVisible; // Enable/disable interaction with the slider
            // canvasGroup.blocksRaycasts = isVisible; // Enable/disable raycast interaction with the slider
        }
        else
        {
            slider.gameObject.SetActive(isVisible); // Fallback to set the slider game object active/inactive
        }
    }

    private bool IsSliderVisible()
    {
        CanvasGroup canvasGroup = slider.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            return canvasGroup.alpha > 0; // Return true if alpha is greater than 0 (visible)
        }
        else
        {
            return slider.gameObject.activeSelf; // Fallback to check if the slider game object is active
        }
    }
}
