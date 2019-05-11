using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TommoJProductions.GameSaveTimerMod
{
    /// <summary>
    /// Represents mod save data.
    /// </summary>
    public class GameSaveTimerSaveData
    {
        // Written, 11.05.2019

        /// <summary>
        /// Represents the time passed in the current session.
        /// </summary>
        public static float timePassedSession { get; set; }
        /// <summary>
        /// Represents the game start time.
        /// </summary>
        public static float gameStartTime { get; set; }
        /// <summary>
        /// Represents if the game save timer has been started from a new game.
        /// </summary>
        public bool startedFromNewGame { get; set; }
        /// <summary>
        /// Represents the time passed in the game save.
        /// </summary>
        public float timePassed { get; set; }
        /// <summary>
        /// Represents the next sessions in the game save.
        /// </summary>
        public int nextSession { get; set; } = 1;
    }
}
