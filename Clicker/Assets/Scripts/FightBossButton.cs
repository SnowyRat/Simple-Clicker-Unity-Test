using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightBossButton : MonoBehaviour
{
    [SerializeField] private Button fightBossButton;
    private GameManag _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManag>();
        //fightBossButton = GetComponent<Button>();
        fightBossButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the bossKilled variable is false
        if (!_gameManager.BossKilled()&&!_gameManager.BossAlive())
        {
            fightBossButton.gameObject.SetActive(true); // Enable the button
        }
        else
        {
            fightBossButton.gameObject.SetActive(false); // Disable the button
        }
    }
    public void Enable()
    {
        fightBossButton.gameObject.SetActive(true);
    }

    public void Disable()
    {
        fightBossButton.gameObject.SetActive(false);
    }

    public void onClick()
    {
        _gameManager.FightBossPressed();
        Disable();
    }
}
