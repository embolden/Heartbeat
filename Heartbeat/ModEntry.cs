using System;
using System.IO;
using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System.Media;

namespace Heartbeat
{
    public class ModEntry : Mod
    {
        private ModConfig Config;

        private SoundPlayer _player;

        private enum PlayingStatus : int
        {
            Not_Playing = 0,
            Playing = 1,
        }

        private PlayingStatus isCurrentlyPlaying;

        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();

            string path = Path.Combine(helper.DirectoryPath, "assets", "human-heartbeat-daniel_simon.wav");
            _player = new SoundPlayer(path);

            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
        }

        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady) { return; }

            if (!Config.HeartBeatEnabled) { return; }

            int playerHealth = Game1.player.health;
            int maxHealth = Game1.player.maxHealth;
            double currentHealth = Math.Round(((double)playerHealth / maxHealth * 100), 0);

            //Monitor.Log($"Current Health: {playerHealth}. \n" +
            //    $"Max Health: {maxHealth}. \n" +
            //    $"Health as percent: {currentHealth}. \n" +
            //    $"Configured Health: {Config.HeartBeatAlertPercent}", LogLevel.Debug);

            if (currentHealth <= Config.HeartBeatAlertPercent)
            {
                if (isCurrentlyPlaying == PlayingStatus.Playing) { return; }
                
                //Monitor.Log($"Current Health: {playerHealth}. \n" +
                //    $"Max Health: {maxHealth}. \n" +
                //    $"Health as percent: {currentHealth}. \n" +
                //    $"Configured Health: {Config.HeartBeatAlertPercent}", LogLevel.Debug);

                _player.PlayLooping();

                isCurrentlyPlaying = PlayingStatus.Playing;
            }
            else
            {
                if (isCurrentlyPlaying == PlayingStatus.Not_Playing) { return; }

                _player.Stop();
                isCurrentlyPlaying = PlayingStatus.Not_Playing;
            }
        }
    }
}

public interface GenericModConfigMenuAPI
{
    void RegisterModConfig(IManifest mod, Action revertToDefault, Action saveToFile);

    void RegisterLabel(IManifest mod, string labelName, string labelDesc);
    void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<bool> optionGet, Action<bool> optionSet);
    void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<int> optionGet, Action<int> optionSet);
    void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<float> optionGet, Action<float> optionSet);
    void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<string> optionGet, Action<string> optionSet);
    void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<SButton> optionGet, Action<SButton> optionSet);

    void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<int> optionGet, Action<int> optionSet, int min, int max);
    void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<float> optionGet, Action<float> optionSet, float min, float max);

    void RegisterChoiceOption(IManifest mod, string optionName, string optionDesc, Func<string> optionGet, Action<string> optionSet, string[] choices);

    void RegisterComplexOption(IManifest mod, string optionName, string optionDesc,
                               Func<Vector2, object, object> widgetUpdate,
                               Func<SpriteBatch, Vector2, object, object> widgetDraw,
                               Action<object> onSave);
}