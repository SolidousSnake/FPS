using UnityEngine;

namespace _Project.Code.Runtime.Core.Utils
{
    public static class Constants
    {
        public const int DefaultCollectionCapacity = 32;

        public static class ConfigPath
        {
            private const string MainFolder = "Config/";

            public const string MapFile = MainFolder + "Gameplay/";
            public const string AiQuotes = MainFolder + "AI/Quotes/";
        }
        
        public static class Audio
        {
            public const float MuteValue = -80f;
            public const float OriginalValue = 0f;
            public const float MaxValue = 20f;
            
            public const string Master = "Master";
            public const string Music = "Music";
            public const string Sfx = "Sfx";
        }
        
        public static class Animation
        {
            public static class Weapon
            {
                public const string Walk = "Walk";
                public const string Run = "Rub";
                public const string Aim = "Aim";
                public const string Fire = "Fire";
                public const string Reload = "Reload";
                public const string EmptyMagazine = "EmptyMagazine";
                public const string Select = "Select";
                public const string Deselect = "Deselect";
            }
        }
        
        public static class Rotation
        {
            public const float RightAngle = 90f;
        }
    }
}