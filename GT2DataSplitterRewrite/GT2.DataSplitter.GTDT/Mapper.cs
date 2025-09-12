using System.Reflection;

namespace GT2.DataSplitter.GTDT
{
    using ArcadeData;
    using Common;
    using GTModeData;
    using GTModeRace;
    using LicenseData;

    public static class Mapper
    {
        public static void MapToModel(TypedData[] data, object model, UnicodeStringTable strings, ASCIIStringTable asciiStrings)
        {
            foreach (TypedData typedData in data)
            {
                MapCommonTypes(typedData.Type, typedData.Structures, model, strings, asciiStrings);
                MapGTModeDataTypes(typedData.Type, typedData.Structures, model, strings);
                MapGTModeRaceTypes(typedData.Type, typedData.Structures, model, strings);
                MapArcadeDataTypes(typedData.Type, typedData.Structures, model, strings);
                MapLicenseDataTypes(typedData.Type, typedData.Structures, model, strings, asciiStrings);
            }
        }

        private static void MapCommonTypes(Type type, List<DataStructure> structures, object model, UnicodeStringTable strings, ASCIIStringTable asciiStrings)
        {
            if (type == typeof(ActiveStabilityControl) && TryGetCollection<Models.Common.ActiveStabilityControl>(model, out PropertyInfo? property))
            {
                property?.SetValue(model, structures.Cast<ActiveStabilityControl>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Brake) && TryGetCollection<Models.Common.Brake>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Brake>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(BrakeController) && TryGetCollection<Models.Common.BrakeController>(model, out property))
            {
                property?.SetValue(model, structures.Cast<BrakeController>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Chassis) && TryGetCollection<Models.Common.Chassis>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Chassis>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Clutch) && TryGetCollection<Models.Common.Clutch>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Clutch>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Computer) && TryGetCollection<Models.Common.Computer>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Computer>().Select(part => part.MapToModel<Models.Common.Computer>()).ToArray());
            }
            else if (type == typeof(Displacement) && TryGetCollection<Models.Common.Displacement>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Displacement>().Select(part => part.MapToModel<Models.Common.Displacement>()).ToArray());
            }
            else if (type == typeof(Drivetrain) && TryGetCollection<Models.Common.Drivetrain>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Drivetrain>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Engine) && TryGetCollection<Models.Common.Engine>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Engine>().Select(part => part.MapToModel(strings)).ToArray());
            }
            else if (type == typeof(EngineBalance) && TryGetCollection<Models.Common.EngineBalance>(model, out property))
            {
                property?.SetValue(model, structures.Cast<EngineBalance>().Select(part => part.MapToModel<Models.Common.EngineBalance>()).ToArray());
            }
            else if (type == typeof(Event) && TryGetCollection<Models.Common.Event>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Event>().Select(part => part.MapToModel(asciiStrings)).ToArray());
            }
            else if (type == typeof(Flywheel) && TryGetCollection<Models.Common.Flywheel>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Flywheel>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Gear) && TryGetCollection<Models.Common.Gear>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Gear>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Intercooler) && TryGetCollection<Models.Common.Intercooler>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Intercooler>().Select(part => part.MapToModel<Models.Common.Intercooler>()).ToArray());
            }
            else if (type == typeof(Lightweight) && TryGetCollection<Models.Common.Lightweight>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Lightweight>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(LSD) && TryGetCollection<Models.Common.LSD>(model, out property))
            {
                property?.SetValue(model, structures.Cast<LSD>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Muffler) && TryGetCollection<Models.Common.Muffler>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Muffler>().Select(part => part.MapToModel<Models.Common.Muffler>()).ToArray());
            }
            else if (type == typeof(NATune) && TryGetCollection<Models.Common.NATune>(model, out property))
            {
                property?.SetValue(model, structures.Cast<NATune>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(PortPolish) && TryGetCollection<Models.Common.PortPolish>(model, out property))
            {
                property?.SetValue(model, structures.Cast<PortPolish>().Select(part => part.MapToModel<Models.Common.PortPolish>()).ToArray());
            }
            else if (type == typeof(PropellerShaft) && TryGetCollection<Models.Common.PropellerShaft>(model, out property))
            {
                property?.SetValue(model, structures.Cast<PropellerShaft>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(RacingModify) && TryGetCollection<Models.Common.RacingModify>(model, out property))
            {
                property?.SetValue(model, structures.Cast<RacingModify>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Steer) && TryGetCollection<Models.Common.Steer>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Steer>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Suspension) && TryGetCollection<Models.Common.Suspension>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Suspension>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(TireCompound) && TryGetCollection<Models.Common.TireCompound>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TireCompound>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(TireForceVol) && TryGetCollection<Models.Common.TireForceVol>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TireForceVol>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(TiresFront) && TryGetCollection<Models.Common.TiresFront>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TiresFront>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(TireSize) && TryGetCollection<Models.Common.TireSize>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TireSize>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(TiresRear) && TryGetCollection<Models.Common.TiresRear>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TiresRear>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(TractionControlSystem) && TryGetCollection<Models.Common.TractionControlSystem>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TractionControlSystem>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(TurbineKit) && TryGetCollection<Models.Common.TurbineKit>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TurbineKit>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Wheel) && TryGetCollection<Models.Common.Wheel>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Wheel>().Select(part => part.MapToModel()).ToArray());
            }
        }

        private static void MapGTModeDataTypes(Type type, List<DataStructure> structures, object model, UnicodeStringTable strings)
        {
            if (type == typeof(Car) && TryGetCollection<Models.GTMode.Car>(model, out PropertyInfo? property))
            {
                property?.SetValue(model, structures.Cast<Car>().Select(part => part.MapToModel(strings)).ToArray());
            }
        }

        private static void MapGTModeRaceTypes(Type type, List<DataStructure> structures, object model, UnicodeStringTable strings)
        {
            if (type == typeof(EnemyCars) && TryGetCollection<Models.GTMode.EnemyCars>(model, out PropertyInfo? property))
            {
                property?.SetValue(model, structures.Cast<EnemyCars>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(Regulations) && TryGetCollection<Models.GTMode.Regulations>(model, out property))
            {
                property?.SetValue(model, structures.Cast<Regulations>().Select(part => part.MapToModel()).ToArray());
            }
        }

        private static void MapArcadeDataTypes(Type type, List<DataStructure> structures, object model, UnicodeStringTable strings)
        {
            if (type == typeof(CarArcadeDrift) && TryGetCollection<Models.Arcade.CarArcadeDrift>(model, out PropertyInfo? property))
            {
                property?.SetValue(model, structures.Cast<CarArcadeDrift>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(CarArcadeRacing) && TryGetCollection<Models.Arcade.CarArcadeRacing>(model, out property))
            {
                property?.SetValue(model, structures.Cast<CarArcadeRacing>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(EnemyCarsArcade) && TryGetCollection<Models.Arcade.EnemyCarsArcade>(model, out property))
            {
                property?.SetValue(model, structures.Cast<EnemyCarsArcade>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(TireCompoundArcade) && TryGetCollection<Models.Arcade.TireCompoundArcade>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TireCompoundArcade>().Select(part => part.MapToModel()).ToArray());
            }
        }

        private static void MapLicenseDataTypes(Type type, List<DataStructure> structures, object model, UnicodeStringTable strings, ASCIIStringTable asciiStrings)
        {
            if (type == typeof(CarLicense) && TryGetCollection<Models.License.CarLicense>(model, out PropertyInfo? property))
            {
                property?.SetValue(model, structures.Cast<CarLicense>().Select(part => part.MapToModel()).ToArray());
            }
            else if (type == typeof(EventLicense) && TryGetCollection<Models.License.EventLicense>(model, out property))
            {
                property?.SetValue(model, structures.Cast<EventLicense>().Select(part => part.MapToModel(asciiStrings)).ToArray());
            }
            else if (type == typeof(TireCompoundLicense) && TryGetCollection<Models.License.TireCompoundLicense>(model, out property))
            {
                property?.SetValue(model, structures.Cast<TireCompoundLicense>().Select(part => part.MapToModel()).ToArray());
            }
        }

        private static bool TryGetCollection<TModel>(object model, out PropertyInfo? property)
        {
            PropertyInfo? propertyCandidate = model.GetType().GetProperty(typeof(TModel).Name);
            if (propertyCandidate?.PropertyType == typeof(TModel[]))
            {
                property = propertyCandidate;
                return true;
            }
            property = null;
            return false;
        }
    }
}