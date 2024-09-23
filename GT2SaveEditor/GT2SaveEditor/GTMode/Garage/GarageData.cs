using System;
using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.Garage
{
    public class GarageData
    {
        public GarageCar[] Cars { get; set; } = Array.Empty<GarageCar>();
        public uint Money { get; set; }
        public short CurrentCar { get; set; }
        public string PlayerName { get; set; } = "";

        public void ReadFromSave(Stream file)
        {
            uint carCount = file.ReadUInt();
            Cars = new GarageCar[carCount];
            long carsStart = file.Position;

            for (int i = 0; i < carCount; i++)
            {
                Cars[i] = new GarageCar();
                Cars[i].ReadFromSave(file);
            }

            file.Position = carsStart + 0xA4 * 100;
            Money = file.ReadUInt();
            CurrentCar = file.ReadShortFixed();
            file.Position += 0x1;
            long nameStart = file.Position;
            PlayerName = file.ReadCharacters();
            file.Position = nameStart + 13;
        }

        public void WriteToSave(Stream file)
        {
            file.WriteUInt((uint)Cars.Length);
            long carsStart = file.Position;

            foreach (GarageCar car in Cars)
            {
                car.WriteToSave(file);
            }

            while (file.Position < carsStart + 0xA4 * 100)
            {
                file.WriteByte(0);
            }
            file.WriteUInt(Money);
            file.WriteShortFixed(CurrentCar);
            file.Position += 0x1;
            long nameStart = file.Position;
            file.WriteCharacters(PlayerName);
            while (file.Position < nameStart + 13)
            {
                file.WriteByte(0);
            }
        }
    }
}