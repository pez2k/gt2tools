using System.Diagnostics.CodeAnalysis;
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
                if (!MapTypes(typedData.Type, typedData.Structures, model, strings, asciiStrings))
                {
                    throw new Exception($"Unmapped type {typedData.Type.Name}");
                }
            }
        }

        private static bool MapTypes(Type type, List<DataStructure> structures, object model, UnicodeStringTable strings, ASCIIStringTable asciiStrings) =>
            MapType<ActiveStabilityControl, Models.Common.ActiveStabilityControl>(type, structures, model, strings, asciiStrings) ||
            MapType<Brake, Models.Common.Brake>(type, structures, model, strings, asciiStrings) ||
            MapType<BrakeController, Models.Common.BrakeController>(type, structures, model, strings, asciiStrings) ||
            MapType<Chassis, Models.Common.Chassis>(type, structures, model, strings, asciiStrings) ||
            MapType<Clutch, Models.Common.Clutch>(type, structures, model, strings, asciiStrings) ||
            MapType<Computer, Models.Common.Computer>(type, structures, model, strings, asciiStrings) ||
            MapType<Displacement, Models.Common.Displacement>(type, structures, model, strings, asciiStrings) ||
            MapType<Drivetrain, Models.Common.Drivetrain>(type, structures, model, strings, asciiStrings) ||
            MapType<Engine, Models.Common.Engine>(type, structures, model, strings, asciiStrings) ||
            MapType<EngineBalance, Models.Common.EngineBalance>(type, structures, model, strings, asciiStrings) ||
            MapType<Event, Models.Common.Event>(type, structures, model, strings, asciiStrings) ||
            MapType<Flywheel, Models.Common.Flywheel>(type, structures, model, strings, asciiStrings) ||
            MapType<Gear, Models.Common.Gear>(type, structures, model, strings, asciiStrings) ||
            MapType<Intercooler, Models.Common.Intercooler>(type, structures, model, strings, asciiStrings) ||
            MapType<Lightweight, Models.Common.Lightweight>(type, structures, model, strings, asciiStrings) ||
            MapType<LSD, Models.Common.LSD>(type, structures, model, strings, asciiStrings) ||
            MapType<Muffler, Models.Common.Muffler>(type, structures, model, strings, asciiStrings) ||
            MapType<NATune, Models.Common.NATune>(type, structures, model, strings, asciiStrings) ||
            MapType<PortPolish, Models.Common.PortPolish>(type, structures, model, strings, asciiStrings) ||
            MapType<PropellerShaft, Models.Common.PropellerShaft>(type, structures, model, strings, asciiStrings) ||
            MapType<RacingModify, Models.Common.RacingModify>(type, structures, model, strings, asciiStrings) ||
            MapType<Steer, Models.Common.Steer>(type, structures, model, strings, asciiStrings) ||
            MapType<Suspension, Models.Common.Suspension>(type, structures, model, strings, asciiStrings) ||
            MapType<TireCompound, Models.Common.TireCompound>(type, structures, model, strings, asciiStrings) ||
            MapType<TireForceVol, Models.Common.TireForceVol>(type, structures, model, strings, asciiStrings) ||
            MapType<TiresFront, Models.Common.TiresFront>(type, structures, model, strings, asciiStrings) ||
            MapType<TireSize, Models.Common.TireSize>(type, structures, model, strings, asciiStrings) ||
            MapType<TiresRear, Models.Common.TiresRear>(type, structures, model, strings, asciiStrings) ||
            MapType<TractionControlSystem, Models.Common.TractionControlSystem>(type, structures, model, strings, asciiStrings) ||
            MapType<TurbineKit, Models.Common.TurbineKit>(type, structures, model, strings, asciiStrings) ||
            MapType<Wheel, Models.Common.Wheel>(type, structures, model, strings, asciiStrings) ||
            MapType<Car, Models.GTMode.Car>(type, structures, model, strings, asciiStrings) ||
            MapType<EnemyCars, Models.GTMode.EnemyCars>(type, structures, model, strings, asciiStrings) ||
            MapType<Regulations, Models.GTMode.Regulations>(type, structures, model, strings, asciiStrings) ||
            MapType<CarArcadeDrift, Models.Arcade.CarArcadeDrift>(type, structures, model, strings, asciiStrings) ||
            MapType<CarArcadeRacing, Models.Arcade.CarArcadeRacing>(type, structures, model, strings, asciiStrings) ||
            MapType<EnemyCarsArcade, Models.Arcade.EnemyCarsArcade>(type, structures, model, strings, asciiStrings) ||
            MapType<TireCompoundArcade, Models.Arcade.TireCompoundArcade>(type, structures, model, strings, asciiStrings) ||
            MapType<CarLicense, Models.License.CarLicense>(type, structures, model, strings, asciiStrings) ||
            MapType<EventLicense, Models.License.EventLicense>(type, structures, model, strings, asciiStrings) ||
            MapType<TireCompoundLicense, Models.License.TireCompoundLicense>(type, structures, model, strings, asciiStrings);

        private static bool MapType<TStructure, TModel>(Type type, List<DataStructure> structures, object model,
                                                        UnicodeStringTable unicode, ASCIIStringTable ascii) where TStructure : DataStructureWithModel<TModel>
        {
            if (type == typeof(TStructure) && TryGetCollection<TModel>(model, out PropertyInfo? property))
            {
                property.SetValue(model, structures.Cast<TStructure>().Select(part => part.MapToModel(unicode, ascii)).ToArray());
                return true;
            }
            return false;
        }

        private static bool TryGetCollection<TModel>(object model, [NotNullWhen(true)]out PropertyInfo? property)
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