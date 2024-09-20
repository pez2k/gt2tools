using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Settings
{
    public class GlobalSettings
    {
        public ReplayInfoEnum ReplayInfo { get; set; }
        public CameraPositionEnum CameraPosition { get; set; }
        public ChaseViewEnum ChaseView { get; set; }
        public bool CourseMap { get; set; }
        public ViewAngleEnum ViewAngle { get; set; }
        public byte MusicVolume { get; set; }
        public byte SFXVolume { get; set; }

        public void ReadFromSave(Stream file)
        {
            ReplayInfo = (ReplayInfoEnum)file.ReadSingleByte();
            CameraPosition = (CameraPositionEnum)file.ReadSingleByte();
            ChaseView = (ChaseViewEnum)file.ReadSingleByte();
            CourseMap = file.ReadSingleByte() == 1;
            ViewAngle = (ViewAngleEnum)file.ReadSingleByte();
            MusicVolume = file.ReadSingleByte();
            SFXVolume = file.ReadSingleByte();
        }
    }
}