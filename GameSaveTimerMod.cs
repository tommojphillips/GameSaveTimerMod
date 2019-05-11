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

        private const string SAVE_FILE_NAME = "gamesavetimermod.txt";
        private GameSaveTimerSaveData gstsd;
        private bool saving = false;

        public override void OnLoad()
        {
            // Written, 11.05.2019

            this.gstsd = this.LoadData();

            if (this.gstsd is null)
            {
                this.gstsd = new GameSaveTimerSaveData()
                {
                    startedFromNewGame = false,
                    sessions = 1
                };
            }

            ModConsole.Print(this.Name + ": Loaded");
        }
        public override void OnNewGame()
        {
            // Written, 11.05.2019

            this.gstsd = new GameSaveTimerSaveData()
            {
                startedFromNewGame = true,
                sessions = 1, 
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

            string _sessionMessage = "";
            if (this.gstsd.startedFromNewGame)
            {
                _sessionMessage += "| <color= green>started from new game</color>";
            }
            _sessionMessage = String.Format("{0} {1}", this.gstsd.sessions == 1 ? "First session" : String.Format("<b>#{0}</b> {1}", this.gstsd.sessions, TimeSpan.FromSeconds(GameSaveTimerSaveData.timePassedSession).ToString()), _sessionMessage);
            string _message = string.Format("<color=white>Time Passed: {0}\nSession: {1}</color>", TimeSpan.FromSeconds(this.gstsd.timePassed).ToString(), _sessionMessage);
            GUI.Label(new Rect(5, 5, 100, 25), _message, new GUIStyle() { fontSize = 18 });
        }

        private void saveData()
        {
            // Written, 11.05.2019

            try
            {
                this.saving = true;
                this.updateTime();
                this.gstsd.sessionStartTime = this.gstsd.timePassed;
                this.gstsd.sessions++;
                SaveLoad.SerializeSaveFile(this, this.gstsd, SAVE_FILE_NAME);
            }
            catch
            {
                ModConsole.Error("<b>[GameSaveTimerMod]</b> - an error occured while attempting to save info..");
            }
        }
        private void updateTime()
        {
            // Written, 11.05.2019

            this.gstsd.timePassed = float.Parse(Math.Round(Time.time - this.gstsd.gameStartTime).ToString());
            GameSaveTimerSaveData.timePassedSession = float.Parse(Math.Round(Time.time - this.gstsd.sessionStartTime).ToString());
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