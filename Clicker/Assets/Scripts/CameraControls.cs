using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private string monsterTag = "Monster";
    [SerializeField] private string miniBossTag = "MiniBoss";
    [SerializeField] private string bossTag = "Boss";
    private GameObject[] monsters;
    private GameObject miniBoss;
    private GameObject boss;
    private GameObject targetMonster;
    private bool bossAlive;
    private bool inCoro;
private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
        // Find all the monsters at the beginning
        miniBoss = GameObject.FindGameObjectWithTag(miniBossTag);
        boss = GameObject.FindGameObjectWithTag(bossTag);
        monsters = GameObject.FindGameObjectsWithTag(monsterTag);

        if (boss != null)
        {
            TargetBoss(boss);
        }
        else if (miniBoss != null)
        {
            TargetBoss(miniBoss);
        }
        else if (monsters.Length > 0)
        {
            // Set the closest monster as the initial target
            FindClosestMonster();
        }
    }

    private void Update()
    {
    miniBoss = GameObject.FindGameObjectWithTag(miniBossTag);
    boss = GameObject.FindGameObjectWithTag(bossTag);
    monsters = GameObject.FindGameObjectsWithTag(monsterTag);

    if (boss != null)
    {
        TargetBoss(boss);
    }
    else if (miniBoss != null)
    {
        TargetBoss(miniBoss);
    }
    else
    {
        // If the target monster is destroyed, find the next available monster
        if (targetMonster == null && monsters.Length > 0)
        {
                FindClosestMonster();
            
        }
    }

    // Move the camera towards the target monster or starting position
    if (targetMonster != null && (targetMonster != miniBoss || targetMonster != boss) && !bossAlive)
    {
        Vector3 targetPosition = targetMonster.transform.position;
        Vector3 cameraPosition = transform.position;

        float interpolationFactor = 2f;

        cameraPosition.x = Mathf.Lerp(cameraPosition.x, targetPosition.x - 1, Time.deltaTime * interpolationFactor);
        cameraPosition.y = Mathf.Lerp(cameraPosition.y, targetPosition.y, Time.deltaTime * interpolationFactor);
        transform.position = cameraPosition;
    }
    
    else if (targetMonster != null && (targetMonster == miniBoss || targetMonster == boss))
    {
        Vector3 targetPosition = targetMonster.transform.position;
        Vector3 cameraPosition = transform.position;

        float interpolationFactor = 2f;

        cameraPosition.x = Mathf.Lerp(cameraPosition.x, targetPosition.x, Time.deltaTime * interpolationFactor);
        cameraPosition.y = Mathf.Lerp(cameraPosition.y, targetPosition.y - 0.5f, Time.deltaTime * interpolationFactor);
        transform.position = cameraPosition;
    }
    else if (targetMonster != null && (targetMonster != miniBoss || targetMonster != boss) && bossAlive && !inCoro)
    {
        Vector3 cameraPosition = transform.position;
        float interpolationFactor = 4f;
        cameraPosition.x = Mathf.Lerp(cameraPosition.x, 0f, Time.deltaTime * interpolationFactor);
        cameraPosition.y = Mathf.Lerp(cameraPosition.y, 0f, Time.deltaTime * interpolationFactor);
        transform.position = cameraPosition;

        StartCoroutine(WaitAfterBoss());
    }
}

     private IEnumerator WaitAfterBoss(){
        inCoro = true;
        yield return new WaitForSeconds(1f);
        bossAlive = false;
        inCoro = false;
    }

    private void FindClosestMonster()
    {
        // Find all the monsters again
        monsters = GameObject.FindGameObjectsWithTag(monsterTag);

        if (monsters.Length > 0)
        {
            // Set the first monster as the initial closest one
            targetMonster = monsters[0];
            float closestDistance = Vector3.Distance(transform.position, targetMonster.transform.position);

            // Find the closest monster
            for (int i = 1; i < monsters.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, monsters[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetMonster = monsters[i];
                }
            }
        }
    }
private void TargetBoss(GameObject specialMonster)
    {
        bossAlive = true;
        targetMonster = specialMonster;
    }
}
