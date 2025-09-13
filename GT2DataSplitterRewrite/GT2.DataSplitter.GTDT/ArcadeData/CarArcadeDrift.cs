namespace GT2.DataSplitter.GTDT.ArcadeData
{
    using CarNameConversion;

    public class CarArcadeDrift : MappedDataStructure<CarArcadeRacing.Data, Models.Arcade.CarArcadeDrift>
    {
        public override Models.Arcade.CarArcadeDrift MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Arcade.CarArcadeDrift
            {
                CarId = data.CarId.ToCarName(),
                Brake = data.Brake,
                BrakeController = data.BrakeController,
                Steer = data.Steer,
                Chassis = data.Chassis,
                Lightweight = data.Lightweight,
                RacingModify = data.RacingModify,
                Engine = data.Engine,
                PortPolish = data.PortPolish,
                EngineBalance = data.EngineBalance,
                Displacement = data.Displacement,
                Computer = data.Computer,
                NATune = data.NATune,
                TurbineKit = data.TurbineKit,
                Drivetrain = data.Drivetrain,
                Flywheel = data.Flywheel,
                Clutch = data.Clutch,
                PropellerShaft = data.PropellerShaft,
                LSD = data.LSD,
                Gear = data.Gear,
                Suspension = data.Suspension,
                Intercooler = data.Intercooler,
                Muffler = data.Muffler,
                TiresFront = data.TiresFront,
                TiresRear = data.TiresRear,
                Unknown1 = data.Unknown1,
                Unknown2 = data.Unknown2,
                Unknown3 = data.Unknown3,
                Unknown4 = data.Unknown4
            };
    }
}