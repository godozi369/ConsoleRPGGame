using System;
using NAudio.Wave;

namespace Game.Audio
{
    public class BgmPlayer : IDisposable
    {
        private IWavePlayer outputDevice;
        private AudioFileReader audioFile;
        private bool isLooping = true;

        public void Play(string filePath)
        {
            Stop(); // 이미 재생 중이면 정지

            audioFile = new AudioFileReader(filePath);
            outputDevice = new WaveOutEvent();

            var loop = new LoopStream(audioFile); // 커스텀 루프 스트림
            outputDevice.Init(loop);
            outputDevice.Play();
        }

        public void Stop()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
        }

        public void Dispose()
        {
            Stop();
        }
    }

    // LoopStream 클래스 (반복 재생용)
    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;
        public override long Length => sourceStream.Length;
        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = sourceStream.Read(buffer, offset, count);
            if (read == 0)
            {
                sourceStream.Position = 0; // 다시 처음으로
                read = sourceStream.Read(buffer, offset, count);
            }
            return read;
        }
    }
}