using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator current;
    public GameObject prefab;
    public float spawnRadius = 0.5f;
    public float reducedSpawnRadius = 0.2f;

    void Awake()
    {
        current = this;
    }

    public void CreatePopUp(Vector3 position, string text, Color color, bool isCriticalHit)
    {
        // Generate a random offset within the specified range
        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.1f, 0.1f), 0f);

        // Calculate the spawn position by adding the random offset to the provided position
        Vector3 spawnPosition = position + randomOffset;

        var popup = Instantiate(prefab, spawnPosition, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        // Access the DamagePopUpAnimation component
        DamagePopUpAnimation damagePopUpAnimation = popup.GetComponent<DamagePopUpAnimation>();

        // Apply the appropriate scaleCurve based on isCriticalHit
        if (isCriticalHit)
        {
            damagePopUpAnimation.SetCriticalHit(true);
        }
        else
        {
            damagePopUpAnimation.SetCriticalHit(false);
        }

        Destroy(popup, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
