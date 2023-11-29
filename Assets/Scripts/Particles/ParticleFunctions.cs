using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleFunctions
{
    //This instantiates an explosion at the laser point of impact
    public static void InstantiateExplosion(GameObject parentObject, Vector3 hitPosition, string explosionName = "explosion01", float explosionSize = 25, Audio audioManager = null)
    {
        Scene scene = SceneFunctions.GetScene();
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
            explosion.transform.position = hitPosition;
            explosion.transform.rotation = Quaternion.LookRotation(hitPosition);
            Vector3 scale = new Vector3(explosionSize, explosionSize, explosionSize); //This sets the explosion size
            explosion.transform.SetParent(parentObject.transform);
            explosion.transform.localScale = scale; //This reapplies the object scale after parenting
            explosion.SetActive(true);
            Task a = new Task(GameObjectUtils.DeactivateObjectAfterDelay(5, explosion));

            if (audioManager != null)
            {
                AudioFunctions.PlayAudioClip(audioManager, "impact01_laserhitshield", "External", hitPosition, 1, 1, 500, 0.6f);
            }
        }
    }
}
