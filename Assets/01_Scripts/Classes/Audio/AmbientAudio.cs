using System.Collections;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    private NightCycleSettings nightCycleSettings;
    private AudioSettings audioSettings;

    //public AK.Wwise.Event Play_Ambience_Day;
    //public AK.Wwise.Event Play_Ambience_Night;

    public float duskLengthSeconds;
    public bool nightMusicIsPlaying = false;


    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;
        nightCycleSettings = Overseer.Instance.Settings.NightCycleSettings;

        duskLengthSeconds = nightCycleSettings.DuskLengthSeconds;

        Overseer.Instance.GetManager<NightCycle>().DawnStarted += PlayAmbienceDay;
        Overseer.Instance.GetManager<NightCycle>().DawnStarted += StopNightMusic;
        Overseer.Instance.GetManager<NightCycle>().DuskStarted += PlayNightTransition;
        Overseer.Instance.GetManager<NightCycle>().NightStarted += PlayAmbienceNight;
        Overseer.Instance.GetManager<NightCycle>().NightStarted += PlayNightMusic;


        //  Initial DawnStarted action is called before this Start()
        PlayAmbienceDay();
    }

    public void PlayAmbienceDay()
    {
        //  [0] Play_Ambience_Day - Stops all nighttime ambience and plays all daytime ambience
        audioSettings.Events[0].Post(gameObject);
    }

    public void PlayAmbienceNight()
    {
        //  [1] Play_Ambience_Night - Stops all daytime ambience and plays all nighttime ambience
        audioSettings.Events[1].Post(gameObject);
    }

    public void PlayNightTransition()
    {
        StartCoroutine(NightTransitionMusic());
    }

    public void PlayNightMusic()
    {
        //  [7] Play_Night_Music - Plays the night music track and stops shamisens if still playing
        audioSettings.Events[7].Post(gameObject);
        nightMusicIsPlaying = true;
    }

    public void StopNightMusic()
    {
        //  [8] Stop_Night_Music - Fades out night music if it is still playing
        audioSettings.Events[8].Post(gameObject);
        nightMusicIsPlaying = false;
    }

    public IEnumerator NightTransitionMusic()
    {
        //  Reset [0] ShamisenTriggerRate Parameter
        audioSettings.RTPCs[0].SetGlobalValue(0);

        //  [5] Play_Dusk_Shamisens - Starts playing shamisens
        audioSettings.Events[5].Post(gameObject);

        //  Increase [0] ShamisenTriggerRate over time
        for (int i = 0; i < 100; i++)
        {
            audioSettings.RTPCs[0].SetGlobalValue(i);

            yield return new WaitForSeconds(duskLengthSeconds/100);

            //  Play night music transition 6 seconds before night happens
            if (!nightMusicIsPlaying && duskLengthSeconds >= 6 && duskLengthSeconds / 100 * i > duskLengthSeconds - 6)
            {
                //  [6] Play_Night_Transition - Plays 6-second transition segment and stops dusk shamisens after 6 seconds
                audioSettings.Events[6].Post(gameObject);
                nightMusicIsPlaying = true;
            }
        }
    }
}
