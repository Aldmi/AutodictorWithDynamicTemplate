using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using AutodictorBL.Entites;
using AutodictorBL.Settings.XmlSound;


namespace AutodictorBL.Sound
{
    public enum SoundPlayerStatus
    {
        Idle,
        Error,
        Stop,
        Playing,
        Paused,
    };

    public interface ISoundPlayer : IDisposable
    {
        bool PlayFile(ВоспроизводимоеСообщение soundMessage);
        void Pause();
        void Play();
        float GetDuration();
        int GetCurrentPosition();
        SoundPlayerStatus GetPlayerStatus();
        int GetVolume();
        void SetVolume(int volume);


        Task ReConnect();
        SoundPlayerType PlayerType { get; }
        bool IsConnect { get; }
        string GetInfo();
        string StatusString { get;}

         Subject<string> StatusStringChangeRx { get; }  //Изменение StatusString
         Subject<bool> IsConnectChangeRx { get; } //Изменение IsConnect
    }
}