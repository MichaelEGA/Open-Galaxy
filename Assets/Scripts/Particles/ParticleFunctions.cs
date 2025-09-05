using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleFunctions
{
    //This instantiates an explosion at the given point
    public static ParticleSystem InstantiateExplosion(GameObject parentObject, Vector3 hitPosition, string explosionName = "explosion01", float explosionSize = 25, Audio audioManager = null, string explosionSound = "impact01_laserhitshield", float audioDistance = 500, string mixer = "External")
    {
        Scene scene = SceneFunctions.GetScene();
        ParticleSystem particleSystem = null;
        GameObject explosion = null;

        //This searches for any deactivated explosions and reuses them
        if (scene.particlesPool != null)
        {
            explosion = PoolUtils.FindInactiveGameObjectInPool(scene.particlesPool, explosionName);
        }

        //This instantiates a new explosion if there are none in the list
        if (explosion == null)
        {
            Object explosionObject = PoolUtils.FindPrefabObjectInPool(scene.particlePrefabPool, explosionName);

            if (explosionObject != null)
            {
                explosion = GameObject.Instantiate(explosionObject) as GameObject;
                scene.particlesPool = PoolUtils.AddToPool(scene.particlesPool, explosion);
            }
        }

        //This sets the position of the explosion and also sets it's deactivation time
        if (explosion != null)
        {
            //This sets the simulation space of the explosion to the scene
            particleSystem = explosion.GetComponent<ParticleSystem>();
            var main = particleSystem.main;
            main.simulationSpace = ParticleSystemSimulationSpace.Custom;
            main.customSimulationSpace = scene.transform;

            Vector3 scenePoint = scene.transform.InverseTransformPoint(hitPosition);

            explosion.transform.SetParent(scene.transform);
            explosion.transform.localPosition = scenePoint;
            explosion.transform.rotation = Quaternion.LookRotation(scenePoint);
            Vector3 scale = new Vector3(explosionSize, explosionSize, explosionSize); //This sets the explosion size
            explosion.transform.localScale = scale; //This reapplies the object scale after parenting

            explosion.SetActive(true);
            Task a = new Task(GameObjectUtils.DeactivateObjectAfterDelay(5, explosion));

            if (audioManager != null)
            {
                AudioFunctions.PlayAudioClip(audioManager, explosionSound, mixer, hitPosition, 1, 1, audioDistance, 0.6f);
            }
        }

        return (particleSystem);
    }

    //This instantiates an explosions at the given point without audio
    public static ParticleSystem InstantiatePersistantExplosion(GameObject parentObject, Vector3 hitPosition, string explosionName = "explosion01", float explosionSize = 25)
    {
        Scene scene = SceneFunctions.GetScene();
        ParticleSystem particleSystem = null;
        GameObject explosion = null;

        //This searches for any deactivated explosions and reuses them
        if (scene.particlesPool != null)
        {
            explosion = PoolUtils.FindInactiveGameObjectInPool(scene.particlesPool, explosionName);
        }

        //This instantiates a new explosion if there are none in the list
        if (explosion == null)
        {
            Object explosionObject = PoolUtils.FindPrefabObjectInPool(scene.particlePrefabPool, explosionName);

            if (explosionObject != null)
            {
                explosion = GameObject.Instantiate(explosionObject) as GameObject;
                scene.particlesPool = PoolUtils.AddToPool(scene.particlesPool, explosion);
            }
        }

        //This sets the position of the explosion and also sets it's deactivation time
        if (explosion != null)
        {
            //This sets the simulation space of the explosion to the scene
            particleSystem = explosion.GetComponent<ParticleSystem>();
            var main = particleSystem.main;
            main.simulationSpace = ParticleSystemSimulationSpace.Custom;
            main.customSimulationSpace = scene.transform;

            Vector3 scenePoint = scene.transform.InverseTransformPoint(hitPosition);

            explosion.transform.SetParent(scene.transform);
            explosion.transform.localPosition = scenePoint;
            explosion.transform.rotation = Quaternion.LookRotation(scenePoint);
            Vector3 scale = new Vector3(explosionSize, explosionSize, explosionSize); //This sets the explosion size
            explosion.transform.localScale = scale; //This reapplies the object scale after parenting

            explosion.SetActive(true);
        }

        return (particleSystem);
    }
}
