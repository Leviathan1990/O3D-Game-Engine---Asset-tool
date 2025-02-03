/*  SyntaxChecker.cs code
 *  Part of MapDataEditor tool V1.3 or above.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace MapDataEditor
{
    public class SyntaxChecker
    {
        private Dictionary<string, string> syntaxDictionary;

        public SyntaxChecker()
        {
            //  Available commands and syntaxes.
            syntaxDictionary = new Dictionary<string, string>
            {
                //  Scenes background color
                { "scene_bgcolor_red",  "scene_bgcolor_red(int red);\nSets the red component of the background color.\nHint: Goes to init.oms"},
                { "scene_bgcolor_blue", "scene_bgcolor_blue(int blue);\nSets the blue component of the background color.\nHint: Goes to init.oms"},
                { "scene_bgcolor_green","scene_bgcolor_green(int green);\nSets the green component of the background color.\nHint: Goes to init.oms"},

                //  Scenes ambinet
                {"scene_ambinet_blue",  "scene_ambinet_blue (int Color);\nSets the blue component of the ambinet light.\nHint: Goes to init.oms"},
                {"scene_ambinet_green", "scene_ambinet_green (int Color);\nSets the green component of the ambinet light.\nHint: Goes to init.oms"},
                {"scene_ambinet_red",   "scene_ambinet_red (int Color);\nSets the red component of the ambinet light.\nHint: Goes to init.oms"},

                //  Scenes sun
                {"scene_sun_blue",      "scene_sun_blue (int Color);\nSets the blue component of the color of the directional light."},
                {"scene_sun_green",     "scene_sun_green (int Color);\nSets the green component of the color of the directional light."},
                {"scene_sun_red",       "scene_sun_red (int Color);\nSets the red component of the color of the directional light."},
                {"scene_sun_pitch",     "scene_sun_pitch (float Pitch);\nSets the pitch of the directional light source."},
                {"scene_sun_heading",   "scene_sun_heading(float Heading);\nSets the heading of the directional light source."},

                //  Map information
                { "si_Map_Height",      "si_Map_Height(int Height);\nThe height of the selected map.\nHint: Goes to information.oms"},
                { "si_Map_Width",       "si_Map_Width(int Width);\nThe width of the selected map.\nHint: Goes to information.oms"},
                { "si_Map_Players",     "si_Map_Players(int NumPlayers);\nThe number of players this map was designed for. 1-8.\nHint: Goes to information.oms"},
                { "si_Map_Author",      "si_Map_Author(str AuthorName);\nAuthor of the map. Max 8 chars long!\nHint: Goes to information.oms"},
                { "si_Map_Description", "si_Map_Description(str Desc);\nA description of the map to play on.\nHint: Goes to information.oms"},

                //  View GameScene
                { "View_GameSceneCenterX","View_GameSceneCenterX(float Pos);\nSets the lookat position of the camera (X)."},
                { "View_GameSceneCenterY","View_GameSceneCenterY(float Pos);\nSets the lookat position of the camera (Y)."},
                
                //  Game logic
                { "M_SetGameSceneRect", "M_SetGameSceneRect(int x, int y);\nSets and allocates the game scene. Measuered in world units."},
                { "M_Resources",        "M_Resources(int Resources);\nSets the number of resources for the active player."},
                { "STARTMP3",           "STARTMP3(str fileName);\nStart a mp3 sound. See documentation for the available mp3 files."},
                { "M_SetStartPosition", "M_SetStartPosition(int PlayerID, float X, float Z);\nSets a start position of a player.,"},
                { "M_SetStartPositionByMouse", "M_SetStartPositionByMouse();\nSets the start position by mouse."},
                { "M_SetTimer",         "M_SetTimer(int seconds);\nSets the countdown timer."},
                { "M_DefineStartPositions","M_DefineStartPositions(int NumberOfPosition);\nDefines a number of start positions."},
                { "StartGame",          "StartGame();\nStarts up the game."},
                { "StopMP3",            "StopMP3();\nStops playing the playing .mp3."},
                { "Anim_Play",          "Anim_Play();\nPlays the supplied bink animation stretched to the whole Outforce window."},
                { "m_SetConditions",    "m_SetConditions [Number]\nSet a number of conditions."},
                { "m_Condition",        "m_Condition();\nDynamic arguments, see other documentation.\nHint: Goes to Objective.oms"},

                //  Map Logic   Used for Campaign maps including tutorial maps
                { "SetActiveCampaign",              "SetActiveCampaign(char MapName);\nSets the map name of the active campaign."},
                { "StartCampaignMission",           "StartCampaignMission(char MapName);\nStarts the specified map in the campaign set by SetActiveCampaign."},
                { "input_LastFile",                 "input_LastFile\nVariable for saving the last used bindings file."},
                { "bind",                           "Bind(str Event = Down, str PreKey = None, str Key, str CommandLine);\n\nBinds a command line the a key.\nEvent: Down, Up, Change, Repeat or Continuous.\nPreKey: Control | Shift | Alt.\nCommand: Any valid command sequence."},
                { "AddCustomizedControl",           "AddCustomizedControl [MAP_ID]\nAddCustomizedControl."},
                { "AddMoveOrder",                   "AddMoveOrder [MAP_ID] [X] [Y] [Z] [FORCEMOVE]\nAddMoveOrder."},
                { "AddPatrolOrder",                 "AddPatrolOrder [MAP_ID] [X] [Z]\nAddPatrolOrder."},
                { "g_SetCampaignMissionText",       "g_SetCampaignMissionText( char Text );\nSets the mission text shown in a GUI object."},
                { "g_SetCampaignObjectiveText",     "g_SetCampaignObjectiveText( char Text);\nSets the objective text shown in a GUI object."},
                { "g_SetCampaignBriefingAnimation", "g_SetCampaignBriefingAnimation( char Text);\ng_SetCampaignBriefingAnimation."},
                { "G_SetActiveGUI",                 "G_SetActiveGUI( int ID );\nThe specified ID tells which GUI to be shown."},
                { "m_BuildRestrictionMode",         "m_BuildRestrictionMode(int Value);\nEnable or disable Unit build restriction.\nHint: Can be 1 or 0. Default value is: 0."},
                { "m_NoBuildRestriction",           "m_NoBuildRestriction (str unit name, int Value);\nBuild restriction for a unit.\nHint: Can be 1 or 0. Default value is: 0."},
                { "AI_AddCommander",                "AI_AddCommander [int player, str race];\nAdds a new AI player\nHint: Can Be 'Xenons', 'Humans', 'Crions', 'Gobins'"}, //?
                { "AI_CommanderColor",              "AI_CommanderColor(int ObjectGroup, float r, float g, float b);\nSets the tint color of an commander"},
                { "AI_InitializeCommanders",        "AI_InitializeCommanders Description();"},
                { "exec",                           "Exec(str FileName);\nExecutes the supplied file"},
                { "RemoveBonds",                    "RemoveBonds();\nRemoves all keyboard bonds.\nHit:Lost.cfg"},
                { "se_KillLooping",                 "se_KillLooping(int Slot);\nKillss a looping sound.\nHint:Goes to Campaigns.cfg"}
            };
        }

        public List<string> GetMatchingCommands(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return syntaxDictionary.Keys.ToList();
            }

            return syntaxDictionary.Keys
                .Where(cmd => cmd.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public string GetSyntax(string command)
        {
            return syntaxDictionary.ContainsKey(command) ? syntaxDictionary[command] : null;
        }

        //  Available assets // A list of assets can be used for m_BuildRestriction

    }
}