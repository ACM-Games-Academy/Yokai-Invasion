/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PAUSE_MUSIC = 2735935537U;
        static const AkUniqueID PLAY_AMBIENCE_DAY = 4201350233U;
        static const AkUniqueID PLAY_AMBIENCE_NIGHT = 944950941U;
        static const AkUniqueID PLAY_ATTACK_ASHIGARU_HEAVY = 2824991575U;
        static const AkUniqueID PLAY_ATTACK_ASHIGARU_LIGHT = 1211607766U;
        static const AkUniqueID PLAY_ATTACK_PLAYER = 3428542466U;
        static const AkUniqueID PLAY_ATTACK_YOKAI = 261420542U;
        static const AkUniqueID PLAY_BUILDING_COMPLETE = 4001521644U;
        static const AkUniqueID PLAY_BUILDING_IN_PROGRESS = 2224434550U;
        static const AkUniqueID PLAY_COINS_COLLECT = 2547541695U;
        static const AkUniqueID PLAY_COINS_SPEND = 1006601637U;
        static const AkUniqueID PLAY_COMMAND_ASHIGARU = 2640662738U;
        static const AkUniqueID PLAY_DAMAGE_ASHIGARU = 3828475318U;
        static const AkUniqueID PLAY_DAMAGE_BUILDING = 3362139180U;
        static const AkUniqueID PLAY_DAMAGE_YOKAI = 1263684625U;
        static const AkUniqueID PLAY_DEATH_ASHIGARU = 3888899281U;
        static const AkUniqueID PLAY_DEATH_YOKAI = 1760829336U;
        static const AkUniqueID PLAY_DUSK_SHAMISENS = 2141916437U;
        static const AkUniqueID PLAY_FOOTSTEP_ASHIGARU = 546028113U;
        static const AkUniqueID PLAY_FOOTSTEP_FARMER = 978628930U;
        static const AkUniqueID PLAY_FOOTSTEP_PLAYER = 4134779396U;
        static const AkUniqueID PLAY_FOOTSTEP_YOKAI = 2266542104U;
        static const AkUniqueID PLAY_NIGHT_MUSIC = 3279835868U;
        static const AkUniqueID PLAY_NIGHT_TRANSITION = 3265916758U;
        static const AkUniqueID PLAY_SCROLL_OPEN = 151111856U;
        static const AkUniqueID PLAY_SELECT_UNIT = 304693731U;
        static const AkUniqueID PLAY_WEAPON_SPIN = 3787597485U;
        static const AkUniqueID RESUME_MUSIC = 2940177080U;
        static const AkUniqueID STOP_NIGHT_MUSIC = 1932246634U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace GRUNTSACTIVE
        {
            static const AkUniqueID GROUP = 2986509750U;

            namespace STATE
            {
                static const AkUniqueID ACTIVE = 58138747U;
                static const AkUniqueID INACTIVE = 3163453698U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace GRUNTSACTIVE

        namespace ONISACTIVE
        {
            static const AkUniqueID GROUP = 1773585304U;

            namespace STATE
            {
                static const AkUniqueID ACTIVE = 58138747U;
                static const AkUniqueID INACTIVE = 3163453698U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace ONISACTIVE

        namespace PAUSED
        {
            static const AkUniqueID GROUP = 319258907U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PAUSED = 319258907U;
                static const AkUniqueID UNPAUSED = 1365518790U;
            } // namespace STATE
        } // namespace PAUSED

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID SHAMISENTRIGGERRATE = 3766713767U;
        static const AkUniqueID WINDINTENSITY = 1042517418U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MAIN_BUS = 3032847908U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC_BUS = 3127962312U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
