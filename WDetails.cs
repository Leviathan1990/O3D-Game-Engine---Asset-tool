using System.Collections.Generic;
using System.Linq;

namespace WDetails
{
    public static class WordDetails
    {
        // A list to hold the keywords.
        private static List<string> keywords = new List<string>
        {
            "M_Resources",
            "M_SetGameSceneRect",
            "STARTMP3",
            "si_Map_Description",
            "si_Map_Author",
            "si_Map_Players",
            "si_Map_Width",
            "si_Map_Height",
            "m_EditMode",
            "m_Devmap",
            "m_DevMapMode",
            "M_SetStartPositionByMouse",
            "M_SetStartPosition",
            "M_DefineStartPositions",
            "m_SetTimer",
            "M_SetLayerPosition",
            "M_SelectLayer",
            "M_TeamMode",
        };

        /// <summary>
        /// Returns the keyword if it exists, otherwise a default message.
        /// </summary>
        public static string GetDetailedInfo(string searchTerm)
        {
            if (keywords.Contains(searchTerm))
            {
                return searchTerm;
            }
            return "No info available...";
        }

        /// <summary>
        /// Returns all the keywords.
        /// </summary>
        public static IEnumerable<string> GetAllKeywords()
        {
            return keywords;
        }
    }
}
