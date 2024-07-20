using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class StageCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageCounter;
    private int stageCount=1;

    // Start is called before the first frame update
    void Start()
    {
        stageCounter.text = stageCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        stageCounter.text = "Stage \n" + stageCount.ToString();
    }
    public void GetGameStage(int stage){
        if(stageCount == stage){
            return;
        }
        stageCount = stage;
    }
}
