using LunaGames.Tutorial;

namespace LunaGames.Main
{
    public static class CORE
    {
        public static FXManager FX;
        public static UIManager UI;
        public static GameData DATA;
        public static Debugger DEBUG;
        public static LevelManager LEVELMANAGER;
        public static TutorialSM TUTORIAL;
        public static ResourceManager RESOURCES;
    }

    public enum GameStates { PreGame, Tutorial, OnGame, Win, Fail }
    public enum WorkerType
    {
        Apprentice, 
        Laborer,
        Master,
        Foreman,
        Engineer   
    }
}
