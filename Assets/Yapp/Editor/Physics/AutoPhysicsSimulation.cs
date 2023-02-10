using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yapp
{
    public class AutoPhysicsSimulation
    {
        public static void ApplyPhysics(PhysicsSettings physicsSettings, List<GameObject> objects, SpawnSettings.AutoSimulationType autoSimulationType)
        {
            Transform[] containerChildren = objects.ConvertAll(x => x.transform).ToArray();

            ApplyPhysics(physicsSettings, containerChildren, autoSimulationType);

        }

        public static void ApplyPhysics(PhysicsSettings physicsSettings, GameObject container, SpawnSettings.AutoSimulationType autoSimulationType)
        {
            Transform[] containerChildren = PrefabUtils.GetContainerChildren(container);

            ApplyPhysics(physicsSettings, containerChildren, autoSimulationType);

        }

        public static void ApplyPhysics(PhysicsSettings physicsSettings, Transform[] objects, SpawnSettings.AutoSimulationType autoSimulationType)
        {
            PhysicsSimulationGroup physicsSimulation = ScriptableObject.CreateInstance<PhysicsSimulationGroup>();
            physicsSimulation.ApplySettings(physicsSettings);

            if (autoSimulationType == SpawnSettings.AutoSimulationType.Continuous)
            {
                physicsSimulation.StartSimulation(objects);

                PhysicsSimulator.SetSimulationTime(physicsSettings.simulationTime);
                PhysicsSimulator.SetSimulationSteps(physicsSettings.simulationSteps);

                PhysicsSimulator.Activate();

                // consume the event or otherwise we'd get an error in PhysicsSimulator because of the UI display of the phyiscs timer
                // TODO: set flag and invoke in different pass
                Event.current.Use(); 
            }

        }
    }
}
