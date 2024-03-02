using System.Collections;
using System.Collections.Generic;


namespace Game
{
    public enum TimeTense
    {
        PRESENT,
        PAST
    }

    // TODO(keyi): read keys from unity, do not need hard code
    public static class KeyMapping 
    {
        public const string LEFT = "A";
        public const string RIGHT = "D";
        public const string JUMP = "Space";

        // our customized keystrokes, in "edit" - "setting" - "input manager"
        public const string TIME_SWITCH = "Q";
        public const string LASER_ROTATE = "F";
    }
}
