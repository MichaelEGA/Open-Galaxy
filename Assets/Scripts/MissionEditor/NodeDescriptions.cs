using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeDescriptions
{ 
  public static string GetNodeDescription(string name)
    {
        string description = "No description availible.";

        if (name == "custom_node")
        {
            description =
            "Custom Node \n " +
            "\n " +
            "Use this node to access a custom mission function that has no node. \n" +
            "\n " +
            "JSON data\n " +
            "eventID -> empty \n " +
            "eventType -> empty \n " +
            "conditionTime -> empty \n " +
            "conditionLocation -> empty \n " +
            "x -> empty \n " +
            "y -> empty \n " +
            "z -> empty \n " +
            "data1 -> empty \n " +
            "data2 -> empty \n " +
            "data3 -> empty \n " +
            "data4 -> empty \n " +
            "data5 -> empty \n " +
            "data6 -> empty \n " +
            "data7 -> empty \n " +
            "data8 -> empty \n " +
            "data9 -> empty \n " +
            "data10 -> empty \n " +
            "nextEvent1 -> empty \n " +
            "nextEvent2 -> empty \n " +
            "nextEvent3 -> empty \n " +
            "nextEvent4 -> empty \n ";
        }
        else if (name == "loadscene")
        {
            description =
            "Load Scene \n " +
            "\n " +
            "This node is essential. It tells OG that you want to load a scene in a particular location. \n" +
            "\n " +
            "Extra Information \n" +
            "- If the location is not found (or you leave the location as *none*) OG will load a random location \n" +
            "- Look for a list of all avaible locations linked on the OG Github page. \n" +
            "- This node does not need to linked to any others. OG will automatically search for and run this event. \n" +
            "\n " +
            "JSON data\n " +
            "eventID -> empty \n " +
            "eventType -> empty \n " +
            "conditionTime -> empty \n " +
            "conditionLocation -> location name \n " +
            "x -> empty \n " +
            "y -> empty \n " +
            "z -> empty \n " +
            "data1 -> empty \n " +
            "data2 -> empty \n " +
            "data3 -> empty \n " +
            "data4 -> empty \n " +
            "data5 -> empty \n " +
            "data6 -> empty \n " +
            "data7 -> empty \n " +
            "data8 -> empty \n " +
            "data9 -> empty \n " +
            "data10 -> empty \n " +
            "nextEvent1 -> empty \n " +
            "nextEvent2 -> empty \n " +
            "nextEvent3 -> empty \n " +
            "nextEvent4 -> empty \n ";
        }
        else if (name == "preload_loadasteroids")
        {
            description = "No description availible.";
        }
        else if (name == "preload_loadplanet")
        {
            description = "No description availible.";
        }
        else if (name == "preload_loadtiles")
        {
            description = "No description availible.";
        }
        else if (name == "preload_loadmultipleshipsonground")
        {
            description = "No description availible.";
        }
        else if (name == "preload_loadsingleship")
        {
            description = "No description availible.";
        }
        else if (name == "preload_loadmultipleships")
        {
            description = "No description availible.";
        }
        else if (name == "preload_loadmultipleshipsbytype")
        {
            description = "No description availible.";
        }
        else if (name == "preload_setskybox")
        {
            description = "No description availible.";
        }
        else if (name == "startmission")
        {
            description = "No description availible.";
        }
        else if (name == "changemusicvolume")
        {
            description = "No description availible.";
        }
        else if (name == "clearaioverride")
        {
            description = "No description availible.";
        }
        else if (name == "displaylargemessagethenexit")
        {
            description = "No description availible.";
        }
        else if (name == "dialoguebox")
        {
            description = "No description availible.";
        }
        else if (name == "displaylargemessage")
        {
            description = "No description availible.";
        }
        else if (name == "displaylocation")
        {
            description = "No description availible.";
        }
        else if (name == "displaymissionbriefing")
        {
            description = "No description availible.";
        }
        else if (name == "iftypeofshipisactive")
        {
            description = "No description availible.";
        }
        else if (name == "ifshipisactive")
        {
            description = "No description availible.";
        }
        else if (name == "loadsingleship")
        {
            description = "No description availible.";
        }
        else if (name == "loadsingleshipatdistanceandanglefromplayer")
        {
            description = "No description availible.";
        }
        else if (name == "loadmultipleshipsonground")
        {
            description = "No description availible.";
        }
        else if (name == "loadmultipleships")
        {
            description = "No description availible.";
        }
        else if (name == "loadmultipleshipsbytype")
        {
            description = "No description availible.";
        }
        else if (name == "lockmainshipweapons")
        {
            description = "No description availible.";
        }
        else if (name == "message")
        {
            description = "No description availible.";
        }
        else if (name == "movetowaypoint")
        {
            description = "No description availible.";
        }
        else if (name == "playmusictype")
        {
            description = "No description availible.";
        }
        else if (name == "setaioverride")
        {
            description = "No description availible.";
        }
        else if (name == "setdontattacklargeships")
        {
            description = "No description availible.";
        }
        else if (name == "setshipallegiance")
        {
            description = "No description availible.";
        }
        else if (name == "setshiptarget")
        {
            description = "No description availible.";
        }
        else if (name == "setshiptargettoclosestenemy")
        {
            description = "No description availible.";
        }
        else if (name == "setshiptoinvincible")
        {
            description = "No description availible.";
        }
        else if (name == "setweaponslock")
        {
            description = "No description availible.";
        }
        else if (name == "shipshullislessthan")
        {
            description = "No description availible.";
        }
        else if (name == "shipislessthandistancetowaypoint")
        {
            description = "No description availible.";
        }

        return description;
    }
}
