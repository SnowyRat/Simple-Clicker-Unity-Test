using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Camera mainCamera;
    private Animator animator;
    private int clickedStateHash;
    private bool isAnimationPlaying;

    [SerializeField] private double damageAmount; // Amount of damage to deal to the monster
    private double baseDamage = 5.0;
    private double upgradeQuant = 1.0;
    private int playerLevel = 1;
    public float critChance = 1f;


    private void Start()
    {
        damageAmount = baseDamage;
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        clickedStateHash = Animator.StringToHash("Player_clicked");

        Vector2 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = mousePosition;
    }

    public void ChangeDamage(int quantity)
    {
        damageAmount = baseDamage * Mathf.Pow(1.055f, quantity);
        upgradeQuant += quantity;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (isAnimationPlaying)
            {
                // Reset the animation state if "Player_clicked" is still playing
                animator.Play(clickedStateHash, 0, 0f);
                animator.SetBool("TooLong", false);
            }
            else
            {
                animator.SetTrigger("Clicked");
                animator.SetBool("TooLong", false);
            }

            // Cast a ray from the mouse cursor position into the scene
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            // Check if the ray hits a collider
            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                // Check if the hit object is a monster
                if (hitObject.CompareTag("Monster"))
                {
                    // Deal damage to the monster
                    MnsterTest monsterHealth = hitObject.GetComponent<MnsterTest>();
                    if (monsterHealth != null)
                    {
                        int roll = Random.Range(1, 101);
                        // Check if the roll is equal to or below the specified threshold
                        bool crit = roll <= critChance;

                        // Set the bool to true if the roll is below or equal to the threshold
                        if (crit)
                        {
                            monsterHealth.TakeDamage(damageAmount * 2, true);

                            // Use the bool as needed for further logic or conditions
                        }
                        else
                        {
                            monsterHealth.TakeDamage(damageAmount, false);
                        }

                    }
                }
                if (hitObject.CompareTag("MiniBoss"))
                {
                    // Deal damage to the monster
                    MiniBossTest miniBossHealth = hitObject.GetComponent<MiniBossTest>();
                    if (miniBossHealth != null)
                    {
                        int roll = Random.Range(1, 101);
                        // Check if the roll is equal to or below the specified threshold
                        bool crit = roll <= critChance;

                        // Set the bool to true if the roll is below or equal to the threshold
                        if (crit)
                        {
                            miniBossHealth.TakeDamage(damageAmount * 2, true);

                            // Use the bool as needed for further logic or conditions
                        }
                        else
                        {
                            miniBossHealth.TakeDamage(damageAmount, false);
                        }

                    }
                }
            }
        }
        else
        {
            animator.SetBool("TooLong", true);
        }

        isAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).fullPathHash == clickedStateHash;
    }
    public void IncreaseLevel(int newLevel)
    {
        playerLevel = newLevel;
    }
    public void UpdateDamage(double newDamage)
    {
        damageAmount = newDamage;
    }
}

