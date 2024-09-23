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
        public bool Unknown { get; set; }

        public void ReadFromSave(Stream file)
        {
            ReplayInfo = (ReplayInfoEnum)file.ReadSingleByte();
            CameraPosition = (CameraPositionEnum)file.ReadSingleByte();
            ChaseView = (ChaseViewEnum)file.ReadSingleByte();
            CourseMap = file.ReadByteAsBool();
            ViewAngle = (ViewAngleEnum)file.ReadSingleByte();
            MusicVolume = file.ReadSingleByte();
            SFXVolume = file.ReadSingleByte();
            Unknown = file.ReadByteAsBool();
            file.Position += 0x2;
        }

        public void WriteToSave(Stream file)
        {
            file.WriteByte((byte)ReplayInfo);
            file.WriteByte((byte)CameraPosition);
            file.WriteByte((byte)ChaseView);
            file.WriteBoolAsByte(CourseMap);
            file.WriteByte((byte)ViewAngle);
            file.WriteByte(MusicVolume);
            file.WriteByte(SFXVolume);
            file.WriteBoolAsByte(Unknown);
            file.Position += 0x2;
        }
    }
}