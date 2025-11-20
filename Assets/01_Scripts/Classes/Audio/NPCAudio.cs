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
}
