namespace _Project.Code.Runtime.Core.Utils
{
    public static class AssetPath
    {
        public static class ConfigPath
        {
            private const string MainFolder = "Config/";

            public const string MapFile = MainFolder + "Gameplay/";
            public const string AiQuotes = MainFolder + "AI/Quotes/";
            public const string AiStats = MainFolder + "AI/Stats/";
        }

        public static class UIPath
        {
            private const string MainFolder = "Prefab/UI/";
            public const string BossLabel = MainFolder + "BossNameLabel";
            public const string HealthBar = MainFolder + "SedHealthBar";
            public const string ArmorBar = MainFolder + "SedArmorBar";
        }
    }
}