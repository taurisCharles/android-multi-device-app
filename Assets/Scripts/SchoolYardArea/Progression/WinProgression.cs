using UnityEngine;

namespace SchoolYardArea.Progression
{
    public sealed class WinProgression
    {
        private const string WinsKey = "SchoolYardArea.TotalWins";

        public int TotalWins => PlayerPrefs.GetInt(WinsKey, 0);

        public void AddWin()
        {
            PlayerPrefs.SetInt(WinsKey, TotalWins + 1);
            PlayerPrefs.Save();
        }

        public bool IsUnlocked(int requiredWins)
        {
            return TotalWins >= requiredWins;
        }
    }
}
