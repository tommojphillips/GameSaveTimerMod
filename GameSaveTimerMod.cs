using System;
using UnityEngine;
using MSCLoader;

namespace TommoJProductions.GameSaveTimerMod
{
    public class GameSaveTimerMod : Mod
    {
        // Written, 11.05.2019

        public override string ID => "GameSaveTimer";
        public override string Name => "Game Save Timer";
        public override string Version => "0.1";
        public override string Author => "tommojphillips";

        /// <summary>
        /// Represents the save file name.
        /// </summary>
        private const string SAVE_FILE_NAME = "gamesavetimermod.txt";
        /// <summary>
        /// Represents the save data.
        /// </summary>
        private GameSaveTimerSaveData timerSaveData;
        /// <summary>
        /// Represents if the game is saving.
        /// </summary>
        private bool saving = false;

        public override void OnLoad()
        {
            // Written, 11.05.2019

            this.timerSaveData = this.LoadData();

            if (this.timerSaveData is null)
            {
                this.timerSaveData = new GameSaveTimerSaveData()
                {
                    startedFromNewGame = false
                };
            }
            GameSaveTimerSaveData.gameStartTime = this.timerSaveData.timePassed;

            ModConsole.Print(this.Name + ": Loaded");
        }
        public override void OnNewGame()
        {
            // Written, 11.05.2019

            this.timerSaveData = new GameSaveTimerSaveData()
            {
                startedFromNewGame = true
            };
        }
        public override void OnSave()
        {
            // Written, 11.05.2019

            this.saveData();
        }
        public override void Update()
        {
            // Written, 11.05.2019

            if (!saving)
                this.updateTime();
        }
        public override void OnGUI()
        {
            // Written, 11.05.2019

            string _message;
            if (!saving)
            {
                string _sessionMessage = this.timerSaveData.nextSession == 1 ? "First session" : String.Format("<b>#{0}</b> {1}", this.timerSaveData.nextSession, TimeSpan.FromSeconds(GameSaveTimerSaveData.timePassedSession).ToString());
                if (this.timerSaveData.startedFromNewGame)
                    _sessionMessage += " | <color= green>started from new game</color>";
                _message = string.Format("<color=white>Time Passed: {0}\nSession: {1}</color>", TimeSpan.FromSeconds(this.timerSaveData.timePassed).ToString(), _sessionMessage);
            }
            else
                _message = String.Format("Session finished at, {0}\nTotal Time, {1}", TimeSpan.FromSeconds(GameSaveTimerSaveData.timePassedSession).ToString(), TimeSpan.FromSeconds(this.timerSaveData.timePassed).ToString());
            GUI.Label(new Rect(5, 5, 100, 25), _message, new GUIStyle() { fontSize = 18 });
        }

        private void saveData()
        {
            // Written, 11.05.2019

            try
            {
                this.saving = true;
                this.updateTime();
                this.timerSaveData.nextSession++;
                SaveLoad.SerializeSaveFile(this, this.timerSaveData, SAVE_FILE_NAME);
            }
            catch
            {
                ModConsole.Error("<b>[GameSaveTimerMod]</b> - an error occured while attempting to save info..");
            }
        }
        private void updateTime()
        {
            // Written, 11.05.2019

            this.timerSaveData.timePassed = float.Parse(Math.Round(Time.timeSinceLevelLoad + GameSaveTimerSaveData.gameStartTime).ToString());
            GameSaveTimerSaveData.timePassedSession = float.Parse(Math.Round(Time.timeSinceLevelLoad).ToString());
        }
        private GameSaveTimerSaveData LoadData()
        {
            // Written, 11.05.2019

            try
            {
                return SaveLoad.DeserializeSaveFile<GameSaveTimerSaveData>(this, SAVE_FILE_NAME);
            }
            catch
            {
                return null;
            }
        }
    }
}