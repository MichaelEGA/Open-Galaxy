using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AvoidCollisionsFunctions
{
    //Starts avoid collision running
    public static void RunAvoidCollision()
    {
        Scene scene = SceneFunctions.GetScene();
        scene.runAvoidCollision = true;
    }

    //Stops avoid collision running
    public static void StopAvoidCollision()
    {
        Scene scene = SceneFunctions.GetScene();
        scene.runAvoidCollision = false;
    }

    //The main function that calls the other two functions
    public static void AvoidCollision(Scene scene)
    {
        if (scene != null & Time.time > scene.loadTime + 10 & scene.runAvoidCollision == true)
        {
            if (scene.objectPool != null)
            {
                if (scene.avoidSmallObjectsRunning == false)
                {
                    Task a = new Task(AvoidSmallObjects(scene));
                }

                if (scene.avoidLargeObjectsRunning == false)
                {
                    Task a = new Task(AvoidLargeObjects(scene));
                }

            }
        }      
    }

    //This avoids collisions between small ships and also checks whether the ship need to evade b/c of laser damage
    public static IEnumerator AvoidSmallObjects(Scene scene)
    {
        scene.avoidSmallObjectsRunning = true;

        foreach (GameObject shipA in scene.objectPool.ToArray())
        {
            if (shipA != null)
            {
                if (shipA.activeSelf == true & shipA.GetComponent<SmallShip>() != null)
                {
                    foreach (GameObject shipB in scene.objectPool.ToArray())
                    {
                        if (shipB.activeSelf == true & shipB.GetComponent<SmallShip>() != null & shipB != shipA)
                        {

                            //This checks if two ships need to avoid a collision
                            SmallShip smallShipA = shipA.GetComponent<SmallShip>();
                            SmallShip smallShipB = shipB.GetComponent<SmallShip>();

                            Vector3 shipATargetPosition = shipB.transform.position - shipA.transform.position;
                            float shipAForward = Vector3.Dot(shipA.transform.forward, shipATargetPosition.normalized);
                            float shipARight = Vector3.Dot(shipA.transform.right, shipATargetPosition.normalized);
                            float shipAUp = Vector3.Dot(shipA.transform.up, shipATargetPosition.normalized);

                            Vector3 shipBTargetPosition = shipA.transform.position - shipB.transform.position;
                            float shipBForward = Vector3.Dot(shipB.transform.forward, shipBTargetPosition.normalized);
                            float shipBRight = Vector3.Dot(shipB.transform.right, shipBTargetPosition.normalized);
                            float shipBUp = Vector3.Dot(shipB.transform.up, shipBTargetPosition.normalized);

                            float distance = Vector3.Distance(shipB.transform.position, shipA.transform.position);

                            int direction = 0;
                            bool avoidCollisionA = false;
                            bool avoidCollisionB = false;

                            if (distance < 500 & shipAForward > 0.8f & shipBForward > 0.8f || distance < 200 & shipAForward > 0.5f & shipBForward > 0.5f || shipAForward > 0.25f & shipBForward > 0.25f & distance < 100)
                            {
                                avoidCollisionA = true;
                                avoidCollisionB = true;
                            }

                            if (distance < 15 & shipAForward > 0.8f)
                            {
                                avoidCollisionA = true;
                            }

                            if (distance < 15 & shipBForward > 0.8f)
                            {
                                avoidCollisionB = true;
                            }

                            if (avoidCollisionA == true & smallShipA.aiEvade == false)
                            {
                                direction = 0; //Right = 0, Left = 1, Up = 2, Down, 3, RollRight 4, RollLeft 5, Fly Forward = 6 
                                if (shipARight > 0) { direction = 1; } else { direction = 0; }
                                if (shipAUp > 0) { direction = 3; } else { direction = 2; }

                                if (smallShipA.isAI == true)
                                {
                                    Task a = new Task(SmallShipAIFunctions.Evade(smallShipA, 2, "avoidCollision", direction));
                                }
                            }

                            if (avoidCollisionB == true & smallShipB.aiEvade == false)
                            {
                                direction = 0; //Right = 0, Left = 1, Up = 2, Down, 3, RollRight 4, RollLeft 5, Fly Forward = 6 
                                if (shipBRight > 0) { direction = 1; } else { direction = 0; }
                                if (shipBUp > 0) { direction = 3; } else { direction = 2; }

                                if (smallShipB.aiEvade == false & smallShipB.isAI == true)
                                {
                                    Task b = new Task(SmallShipAIFunctions.Evade(smallShipB, 2, "avoidCollision", direction));
                                }
                            }

                            //This checks if the ship needs to evade enemy fire
                            float healthTotalA = smallShipA.shieldLevel + smallShipA.hullLevel;
                            float healthTotalB = smallShipB.shieldLevel + smallShipB.hullLevel;
                            direction = Random.Range(0, 6);

                            if (smallShipA.healthSave > healthTotalA + 10)
                            {
                                smallShipA.healthSave = healthTotalA;

                                if (avoidCollisionA == false & smallShipA.isAI == true)
                                {
                                    Task a = new Task(SmallShipAIFunctions.Evade(smallShipA, 2, "evadeAttack", direction));
                                }

                            }

                            if (smallShipB.healthSave > healthTotalB + 10)
                            {
                                smallShipB.healthSave = healthTotalB;

                                if (avoidCollisionB == false & smallShipB.isAI == true)
                                {
                                    Task a = new Task(SmallShipAIFunctions.Evade(smallShipB, 2, "evadeAttack", direction));
                                }

                            }

                        }
                    }
                }

                if (scene.runAvoidCollision == false)
                {
                    break;
                }

                yield return null;
            }
        }

        scene.avoidSmallObjectsRunning = false;
    }

    //This avoids collisions between small and large and large and large
    public static IEnumerator AvoidLargeObjects(Scene scene)
    {
        scene.avoidLargeObjectsRunning = true;

        foreach (GameObject ship in scene.objectPool.ToArray())
        {
            if (ship != null)
            {
                if (ship.activeSelf == true)
                {
                    Vector3 forwardRaycast = ship.transform.position + (ship.transform.forward * 10);
                    Vector3 backwardRaycast = ship.transform.position + (ship.transform.forward * -10);
                    SmallShip smallShip = ship.GetComponent<SmallShip>();
                    LargeShip largeShip = ship.GetComponent<LargeShip>();

                    if (largeShip != null)
                    {
                        if (largeShip.castPoint != null)
                        {
                            forwardRaycast = largeShip.castPoint.position;
                        }
                    }

                    RaycastHit hit;
                    RaycastHit hitSave;

                    int direction = 0;

                    if (Physics.SphereCast(forwardRaycast, 50, ship.transform.TransformDirection(Vector3.forward), out hit, 500))
                    {
                        direction = 0;
                        hitSave = hit;

                        if (Physics.SphereCast(forwardRaycast, 50, ship.transform.TransformDirection(Vector3.right), out hit, 500))
                        {
                            direction = 1;
                            hitSave = hit;

                            if (Physics.SphereCast(forwardRaycast, 50, ship.transform.TransformDirection(Vector3.left), out hit, 500))
                            {
                                direction = 2;
                                hitSave = hit;

                                if (Physics.SphereCast(forwardRaycast, 50, ship.transform.TransformDirection(Vector3.up), out hit, 500))
                                {
                                    direction = 3;
                                    hitSave = hit;

                                    if (Physics.SphereCast(forwardRaycast, 50, ship.transform.TransformDirection(Vector3.down), out hit, 500))
                                    {
                                        direction = 1;
                                        hitSave = hit;
                                    }
                                }
                            }
                        }

                        if (hitSave.transform.GetComponentInParent<SmallShip>() != true)
                        {
                            if (smallShip != null)
                            {
                                if (smallShip.isAI == true)
                                {
                                    Task a = new Task(SmallShipAIFunctions.Evade(smallShip, 2, "avoidCollision", direction));
                                }
                            }
                            else if (largeShip != null)
                            {
                                Task a = new Task(LargeShipAIFunctions.Evade(largeShip, 2, "avoidCollision", direction));
                            }
                        }
                    }
                    else if (Physics.SphereCast(backwardRaycast, 50, ship.transform.TransformDirection(Vector3.back), out hit, 500) & smallShip != null)
                    {
                        direction = 6;

                        if (smallShip != null)
                        {
                            if (smallShip.isAI == true & hit.transform.GetComponentInParent<SmallShip>() != true)
                            {
                                Task a = new Task(SmallShipAIFunctions.Evade(smallShip, 6, "avoidCollision", direction));
                            }
                        }
                    }

                    yield return null;
                }

                if (scene.runAvoidCollision == false)
                {
                    break;
                }
            }
        }

        yield return null;

        scene.avoidLargeObjectsRunning = false;

    }
}
