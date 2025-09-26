namespace Sentience
{
    [System.Serializable]
    public struct Quest
    {
        public PlayerData Player;
        public SentienceQuest QuestData;
        public int Stage;

        public Quest(PlayerData player, SentienceQuest questData, int stage)
        {
            Player = player;
            QuestData = questData;
            Stage = stage;
        }
    }
}