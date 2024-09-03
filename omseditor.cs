/* OMSEDITOR highlight
 * 
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact:
 */

using System.Security.Cryptography;
using System.Text;

namespace omseditor
{
                    //Highlight colors for OMSEDITOR (RichTextBox component)

    public static class WordsToColorBlue                            // Functions
    {
        public static string[] KeywordsB = { 
        "Create",
        "cost",
        "bind",
        "stopmp3",
        "disconnect",
        "se_killlooping",
        "SetActiveCampaign",
        "StartCampaignMission",
        "anim_play",
        "RemoveBonds", 
        "input_LastFile",
        "exec",
        "AddCustomizedControl",
        "AI_InitializeCommanders",
        "AddMoveOrder",
        "startgame",
        "AI_AddCommander",
        "ai_commandercolor",
        "se_playlooping",
        "AddPatrolOrder",
        "BriefingScrollReset",
        "g_SetCampaignMissionText",
        "g_SetCampaignObjectiveText",
        "g_SetCampaignBriefingAnimation",
        "g_SetActiveGui"
        };
    }

    public static class WordsToColorGreen                           // Objects on map (used by visual.oms file)
    {
        public static string[] KeywordsG = { 
        "asteroid_Chrystal_",
        "asteroid_Chrystal_1",
        "asteroid_Chrystal_2",
        "asteroid_Chrystal_3",
        "asteroid_Chrystal_4",
        "asteroid_Chrystal_5",
        "asteroid_Chrystal_6",
        "Asteroid_size6_green",
        "Asteroid_size6",
        "b_tutorial",
        "B_Skri",
        "Game_Resource_Spot",
        "Stone1",
        "Stone2",
        "Stone3", 
        "b_mission",
        "b_tutorial",
        "planet",
        "MH_",
        "HM_",
        "Debris",
        "ssm_toroid_size_" 
        };
    }

    public static class WordToColorOrange                           // Game logic
    {
        public static string[] KeywordsO =
        {
        "TEAM_IS_DEAD",
        "TIME_IS",
        "WIN",
        "LOST",
        "Neutral",
        "IS_DEAD",
        "ID",
        "IS_IN_AREA"
        };
    }

    //  FOR UNITS /  OTHER BUILDABLE STRUCTURES

    public static class WordsToColorPurple {

        public static string[] KeywordsP =
        {
                                                                    // FOR RACE "CRIONS" (CM/CP/CS)
        "Cm_LightMissileLauncher",
        "Cm_lightlaser",
        "Cm_extractor",
        "Cm_MetalWork",
        "Cm_solarplant",
        "CF_Zadorec",
        "CF_Djexo",
        "CF_Tixer",
        "Cs_Scout",
        "Cs_Towship",
        "Cs_Collector",
        "Cm_HeavyDefence",
        "Cm_MissileLauncher",
        "Cm_HeavyLaserTurret",
        "Cm_FusionPowerPlant",
        "Cs_Cloaker",
        "Cp_Fighter",
        "Cm_AntiWarp",
        "Cm_shieldlo",
        "Cm_AssaultWeapon",
        "Cm_TurbineLaser",
        "Cm_Radaradv",
        "Cm_WarpStation",
        "Cp_constructor",
        "Cm_phaserUCM",
        "Cs_SelfDestructor",
        "CD_Litter",
        "CD_Evron",
        "Cm_UCMAdv",
        "Cm_ExtractorAdv",
        "Cm_UCM",
        "Cs_constructor",
        "Cm_central",
        "Cm_shieldhi",
        "Cm_WarpNuke",
        "Cm_AntiMatterPlant",
        "CC_Loctor",
        "CC_Kegger",
        "Cs_constructorAdv",
        "Cm_NeutronWeapon",
        "Cp_SelfDestructor",                                         
                                                                    //  FOR RACE "GOBINS" (GM/GS/GF/Gm/GP/GD)
        "GM_LightMissileLauncher",
        "GM_LightLaserTurret",
        "Gm_extractor",
        "GM_MetalWork",
        "GM_solarplant",
        "GS_Scout",
        "GS_towship",
        "GS_collector",
        "GF_Jandrow",
        "GF_RazorFish",
        "GF_Gerdla",
        "GS_Cloaker",
        "GP_fighter",
        "Gm_AntiWarp",
        "GM_WarpNuke",
        "Gm_AssaultModule",
        "GM_TurbineLaser",
        "GM_warpStation",
        "Gm_antimatterPlant",
        "GM_shieldlo",
        "GM_radarAdv",
        "GP_constructor",
        "GM_PhaserUCM",
        "GM_shieldHi",
        "GM_HeavyDefence",
        "GM_MissileLauncher",
        "GM_HeavyLaserTurret",
        "GM_ExtractorAdv",
        "GM_FusionPowerplant",
        "GS_selfDestruct",
        "GC_IntoRoth",
        "GC_Wontar",
        "GD_onath",
        "GD_Zidra",
        "GM_UCMAdv",
        "GS_constructorAdv",
        "GM_UCM",
        "GS_constructor",
        "Gm_Central",                                                
                                                                 // FOR RACE "HUMANS" (HM/HF/HS/HP)
        "HM_lightMissileLauncher",
        "HM_LightLaserTurret",
        "HM_extractor",
        "HM_MetalWork",
        "HM_SolarPlant",
        "HF_Hawk",
        "HF_fox",
        "HF_Drone",
        "HS_Scout",
        "HS_Towship",
        "HS_Collector",
        "HM_shieldhi",
        "HM_HeavyDefence",
        "HM_MissileLauncher",
        "HM_HeavyLaserTurret",
        "HM_extractoradv",
        "HM_FusionPowerPlant",
        "HM_FusionPowerPlat",
        "HS_Cloaker",
        "HP_fighter",
        "HM_AntiWarp",
        "HM_WarpNuke",
        "HM_shieldLo",
        "HM_AssaultModule",
        "HM_TurbineLaser",
        "HM_RadarAdv",
        "HS_WarpStation",
        "HM_AntiMatterPlant",
        "HP_constructor",
        "HM_PhaserUCM",
        "HS_SelfDestruct",
        "HP_SelfDestruct",
        "HC_NBC",
        "HC_Centa",
        "HD_Raptor",
        "HD_Commander",
        "HM_UCMAdv",
        "HS_ConstructorAdv",
        "HM_UCM",
        "HS_Constructor",
        "HM_Central",
                                                                  // FOR RACE "MISC" (M_)
        "M_LongRangeRadar",
        "M_Gm_AssaultModule",
        "M_Cm_AssaultWeapon",
        "M_Cm_HeavyDefence",
        "M_GM_HeavyDefence",
        "M_USS_Harioken",
        "HD_Raptor_Special",
        "User_Random_EnterPrise",
        "1_Warhammer_Darkone",
        "HF_Drone_Special",                                                   
                                                                 // FOR RACE "XENONS" (Xeno_) Hidden race
        "Xeno_clanmember_FlatMange",
        "Xeno_NewBase_BigSidecanR",
        "Xeno_clone_gerdla",
        "Xeno_Clanmember_b ngan",
        "Xeno_clone_Evron",
        "Xeno_Clanmember_Tolmero",
        "Xeno_NewBase_center",
        "Xeno_NewBase_BigSidecanL",
        "Xeno_NewBase_BackDef",
        "Xeno_NewBase_FrontDef"

        };
    
    }

    public static class WordsToColorRed                         // For Editor
    {

        public static string[] KeywordsR = 
        { 
        "M_DefineStartPositions",
        "M_SetLayerPosition",
        "M_SelectLayer",
        "M_SetStartPosition",
        "m_condition",
        "m_setconditions",
        "M_Resources",
        "m_settimer",
        "m_BuildRestrictionMode",
        "m_NoBuildRestriction",
        "M_SetGameSceneRect "

        };
    }


}