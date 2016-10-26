﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rampastring.XNAUI
{
    /// <summary>
    /// A sound player that manages prioritized sounds, with the aim of preventing
    /// insignificant sounds from overriding important sounds while still
    /// playing insignificant sounds if there's no risk of them cutting off
    /// important sounds.
    /// </summary>
    public class SoundPlayer : GameComponent
    {
        public SoundPlayer(Game game) : base(game)
        {
            soundList = new List<PrioritizedSoundInstance>();
        }

        private static List<PrioritizedSoundInstance> soundList;

        public static float Volume { get; private set; }

        public void SetVolume(float volume)
        {
            Volume = volume;
            MediaPlayer.Volume = volume;
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        public static void Play(PrioritizedSound sound)
        {
            if (soundList == null)
                return;

            Play(Volume, sound);
        }

        /// <summary>
        /// Plays a sound with the specified volume.
        /// </summary>
        /// <param name="volume">The volume that the sound will be played at.</param>
        /// <param name="sound">The sound to play.</param>
        public static void PlayWithVolume(float volume, PrioritizedSound sound)
        {
            Play(volume, sound);
        }

        private static void Play(float volume, PrioritizedSound sound)
        {
            foreach (var psi in soundList)
            {
                if (psi.Priority > sound.Priority)
                    return;
            }

            var soundInstance = sound.CreateSoundInstance();
            soundInstance.Volume = volume;

            var prioritizedSoundInstance = new PrioritizedSoundInstance(
                soundInstance, sound.Priority, sound.PriorityDecayRate);

            soundInstance.Play();
            soundList.Add(prioritizedSoundInstance);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < soundList.Count; i++)
            {
                var sound = soundList[i];
                if (!sound.Update(gameTime))
                {
                    sound.Dispose();
                    soundList.RemoveAt(i);
                    i--;
                }
            }

            base.Update(gameTime);
        }
    }
}
