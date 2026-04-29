using System.Collections;
using UnityEngine;

public class YokaiGrunt : MonoBehaviour, Yokai
{
    [Header("Yokai Settings")]
    [Tooltip("Settings for the Yokai Grunt")]
    [SerializeField]
    private YokaiSettings yokaiSettings;
    YokaiSettings Yokai.yokaiSettings => yokaiSettings;

    private AudioSettings audioSettings;

    [Header("Grunt Stats")]
    [Tooltip("The current health of the Yokai Grunt")]
    private int currentHealth;

    private Yokai.States state = Yokai.States.Idle;     // Exposing this to editor means it may fail to update
    Yokai.States Yokai.state => state;

    private Collider[] targetsInRange;
    private float lastAttackTime;

    public Animator animator;


    private void OnEnable()
    {
        currentHealth = yokaiSettings.MaxHealth;
    }

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;
    }

    public void TakeDamage(int damageAmount)
    {
        //Debug.Log($"{yokaiSettings.YokaiName} took {damageAmount} damage.");
        currentHealth -= damageAmount;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            //  [11] Play_Damage_Yokai - Plays damage sound without armour scrape
            audioSettings.Events[11].Post(gameObject);

            state = Yokai.States.Fleeing;
        }
    }

    public void SetState(Yokai.States newState) => state = newState;

    private IEnumerator Die()
    {
        animator.SetBool("Dead", true);

        //  [27] Play_Death_Yokai - Plays yokai death voiceline
        audioSettings.Events[27].Post(gameObject);

        yield return new WaitForSeconds(3);

        // Debug.Log($"{yokaiSettings.YokaiName} has died.");

        //  [18] Play_Coin_Collect - Plays small coin sound
        audioSettings.Events[18].Post(gameObject);

        Overseer.Instance.GetManager<ResourceManager>().IncreaseGold(yokaiSettings.DropAmount);
        Overseer.Instance.GetManager<ObjectPooler>().ReturnPooledObject(gameObject);
    }
}
