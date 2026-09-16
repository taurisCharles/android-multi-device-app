using UnityEngine;

namespace SchoolYardArea.Runtime
{
    public sealed class GameAudio : MonoBehaviour
    {
        private static GameAudio instance;

        [SerializeField] private float musicVolume = 0.42f;
        [SerializeField] private float sfxVolume = 0.9f;

        private AudioSource musicSource;
        private AudioSource sfxSource;
        private AudioClip punchClip;
        private AudioClip specialClip;
        private AudioClip hitClip;
        private AudioClip healClip;
        private AudioClip winClip;
        private AudioClip loseClip;

        private void Awake()
        {
            instance = this;
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.volume = sfxVolume;

            punchClip = CreateTone("Punch", 190f, 0.09f, Wave.Square, 0.45f);
            specialClip = CreateSweep("Special", 260f, 620f, 0.32f, 0.45f);
            hitClip = CreateNoiseHit("Hit", 0.12f, 0.34f);
            healClip = CreateArpeggio("Heal", new[] { 523.25f, 659.25f, 783.99f }, 0.36f, 0.35f);
            winClip = CreateArpeggio("Win", new[] { 523.25f, 659.25f, 783.99f, 1046.5f }, 0.52f, 0.4f);
            loseClip = CreateArpeggio("Lose", new[] { 392f, 329.63f, 261.63f }, 0.48f, 0.34f);

            musicSource.clip = CreateMusicLoop();
            musicSource.Play();
            Play(healClip, 0.7f);
        }

        public static void PlayPunch() => instance?.Play(instance.punchClip, 0.75f);
        public static void PlaySpecial() => instance?.Play(instance.specialClip, 0.95f);
        public static void PlayHit() => instance?.Play(instance.hitClip, 0.7f);
        public static void PlayHeal() => instance?.Play(instance.healClip, 0.82f);
        public static void PlayWin() => instance?.Play(instance.winClip, 0.95f);
        public static void PlayLose() => instance?.Play(instance.loseClip, 0.75f);

        private void Play(AudioClip clip, float volumeScale)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip, volumeScale);
            }
        }

        private static AudioClip CreateMusicLoop()
        {
            const int sampleRate = 22050;
            const float duration = 8f;
            var sampleCount = Mathf.CeilToInt(sampleRate * duration);
            var data = new float[sampleCount];
            var notes = new[] { 261.63f, 329.63f, 392f, 329.63f, 293.66f, 349.23f, 440f, 349.23f };

            for (var i = 0; i < sampleCount; i++)
            {
                var time = i / (float)sampleRate;
                var beat = Mathf.FloorToInt(time * 2f) % notes.Length;
                var local = time * 2f - Mathf.Floor(time * 2f);
                var envelope = Mathf.Exp(-local * 3.2f);
                var melody = Mathf.Sin(2f * Mathf.PI * notes[beat] * time) * envelope * 0.23f;
                var harmony = Mathf.Sin(2f * Mathf.PI * (notes[beat] * 1.5f) * time) * envelope * 0.06f;
                var bass = Mathf.Sin(2f * Mathf.PI * (notes[beat] * 0.5f) * time) * 0.12f;
                data[i] = melody + harmony + bass;
            }

            var clip = AudioClip.Create("Schoolyard Loop", sampleCount, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static AudioClip CreateTone(string name, float frequency, float duration, Wave wave, float volume)
        {
            const int sampleRate = 22050;
            var sampleCount = Mathf.CeilToInt(sampleRate * duration);
            var data = new float[sampleCount];
            for (var i = 0; i < sampleCount; i++)
            {
                var time = i / (float)sampleRate;
                var raw = wave == Wave.Square ? Mathf.Sign(Mathf.Sin(2f * Mathf.PI * frequency * time)) : Mathf.Sin(2f * Mathf.PI * frequency * time);
                var envelope = Mathf.Clamp01(1f - time / duration);
                data[i] = raw * envelope * volume;
            }

            return ClipFromData(name, data, sampleRate);
        }

        private static AudioClip CreateSweep(string name, float startFrequency, float endFrequency, float duration, float volume)
        {
            const int sampleRate = 22050;
            var sampleCount = Mathf.CeilToInt(sampleRate * duration);
            var data = new float[sampleCount];
            for (var i = 0; i < sampleCount; i++)
            {
                var t = i / (float)sampleCount;
                var time = i / (float)sampleRate;
                var frequency = Mathf.Lerp(startFrequency, endFrequency, t);
                var envelope = Mathf.Sin(Mathf.PI * t);
                data[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * envelope * volume;
            }

            return ClipFromData(name, data, sampleRate);
        }

        private static AudioClip CreateNoiseHit(string name, float duration, float volume)
        {
            const int sampleRate = 22050;
            var sampleCount = Mathf.CeilToInt(sampleRate * duration);
            var data = new float[sampleCount];
            uint seed = 12345;
            for (var i = 0; i < sampleCount; i++)
            {
                seed = seed * 1103515245 + 12345;
                var noise = ((seed / 65536u) % 32768) / 16384f - 1f;
                var envelope = Mathf.Clamp01(1f - i / (float)sampleCount);
                data[i] = noise * envelope * volume;
            }

            return ClipFromData(name, data, sampleRate);
        }

        private static AudioClip CreateArpeggio(string name, float[] notes, float duration, float volume)
        {
            const int sampleRate = 22050;
            var sampleCount = Mathf.CeilToInt(sampleRate * duration);
            var data = new float[sampleCount];
            for (var i = 0; i < sampleCount; i++)
            {
                var t = i / (float)sampleCount;
                var noteIndex = Mathf.Min(notes.Length - 1, Mathf.FloorToInt(t * notes.Length));
                var time = i / (float)sampleRate;
                var envelope = Mathf.Clamp01(1f - t * 0.55f);
                data[i] = Mathf.Sin(2f * Mathf.PI * notes[noteIndex] * time) * envelope * volume;
            }

            return ClipFromData(name, data, sampleRate);
        }

        private static AudioClip ClipFromData(string name, float[] data, int sampleRate)
        {
            var clip = AudioClip.Create(name, data.Length, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private enum Wave
        {
            Sine,
            Square
        }
    }
}
