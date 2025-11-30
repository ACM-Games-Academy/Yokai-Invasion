using UnityEngine;

public class NPCAudio : MonoBehaviour
{
    private AudioSettings audioSettings;

    //public AK.Wwise.Event Play_Footstep_Farmer;
    //public AK.Wwise.Event Play_Footstep_Ashigaru;

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;
    }


    public void PlayFootstepFarmer()
    {
        //  [3] Play_Footstep_Farmer - Plays footstep, quieter than player's
        audioSettings.Events[3].Post(gameObject);
    }

    public void PlayFootstepAshigaru()
    {
        //  [2] Play_Footstep_Ashigaru - Plays footstep and occasionally armour noises, quieter than player's
        audioSettings.Events[2].Post(gameObject);
    }

    public void PlayAttackAshigaruLight()
    {
        //  [9] Play_Attack_Ashigaru_Light - Plays whoosh and metal ringing
        audioSettings.Events[9].Post(gameObject);
    }

    public void PlayAttackAshigaruHeavy()
    {
        //  [10] Play_Attack_Ashigaru_Heavy - Plays whoosh, metal ringing, and armour noises
        audioSettings.Events[10].Post(gameObject);
    }

    public void PlayAttackYokai()
    {
        //  [15] Play_Attack_Yokai - Plays whoosh sound
        audioSettings.Events[15].Post(gameObject);
    }

    public void PlayFootstepYokai()
    {
        //  [16] Play_Footstep_Yokai - Plays footsteps with higher and wider range random pitch
        audioSettings.Events[16].Post(gameObject);
    }
}
