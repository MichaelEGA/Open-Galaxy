using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class NodeTypes
{
    #region generic nodes

    public static void Draw_NodeNotAvaible(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "node not avaible", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        NodeFunctions.DrawText(node, "This node does not exist yet. Check again in later versions to see if it has been added.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_CustomNode(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "custom_node", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawInputField(node, "Event Type", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "data1", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "data2", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "data3", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "data4", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "data5", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "data6", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "data7", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "data8", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "data9", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "data10", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "data11", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data12 = NodeFunctions.DrawInputField(node, "data12", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data13 = NodeFunctions.DrawInputField(node, "data13", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data14 = NodeFunctions.DrawInputField(node, "data14", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data15 = NodeFunctions.DrawInputField(node, "data15", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 1", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 2", 7, 5);

        drop -= 15;

        node.nextEvent3 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 3", 7, 5);

        drop -= 15;

        node.nextEvent4 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 4", 7, 5);

        drop -= 15 + 5;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

    #region pre load event nodes

    public static void Draw_PreLoad_LoadAsteroids(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadasteroids", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadasteroids", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawDropDownMenu(node, GetAsteroidList(), "type", "asteroid01", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "width", "15000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "height", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "length", "15000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "seed", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadEnvironment(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadenvironment", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadenvironment", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetEnvironmentList(), "type", "mountains01", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadPlanet(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadplanet", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadplanet", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "planetRotX", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "planetRotY", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "planetRotZ", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "pivotXRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "pivotYRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "pivotZRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "distance", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawDropDownMenu(node, GetPlanetList(), "planet", "habitable01", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetCloudList(), "clouds", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawDropDownMenu(node, GetAtmosphereList(), "atmosphere", "blue", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("none");
        options4.Add("ring01");

        node.data5 = NodeFunctions.DrawDropDownMenu(node, options4, "rings", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadMultipleShips(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadmultipleships", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadmultipleships", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("rectanglehorizontal");
        options3.Add("rectanglevertical");
        options3.Add("arrowhorizontal");
        options3.Add("arrowhorizontalinverted");
        options3.Add("linehorizontallongways");
        options3.Add("linehorizontalsideways");
        options3.Add("linevertical");
        options3.Add("randominsidecube");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options3, "pattern", "rectanglehorizontal", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "width", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "length", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "height", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "ships per line", "2", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "pos variance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("false");
        options4.Add("true");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options4, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("false");
        options5.Add("true");

        node.data13 = NodeFunctions.DrawDropDownMenu(node, options5, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data14 = NodeFunctions.DrawInputField(node, "player no.", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options6 = new List<string>();
        options6.Add("red");
        options6.Add("green");
        options6.Add("yellow");

        node.data15 = NodeFunctions.DrawDropDownMenu(node, options6, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadMultipleShipsOnGround(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadmultipleshipsonground", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadmultipleshipsonground", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "dsturrettall", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "width", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "length", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "above ground", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "ships per line", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "pos variance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("false");
        options4.Add("true");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options4, "always load", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("red");
        options5.Add("green");
        options5.Add("yellow");

        node.data13 = NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadSingleShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadsingleship", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadsingleship", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("false");
        options3.Add("true");

        node.data5 = NodeFunctions.DrawDropDownMenu(node, options3, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options4, "is AI", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("red");
        options5.Add("green");
        options5.Add("yellow");

        node.data7 = NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> optionsAR = new List<string>();
        optionsAR.Add("false");
        optionsAR.Add("true");

        node.data8 = NodeFunctions.DrawDropDownMenu(node, optionsAR, "run once", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadSingleShipOnGround(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadsingleshiponground", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadsingleshiponground", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data5 = NodeFunctions.DrawDropDownMenu(node, options3, "is AI", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "above ground", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "pos variance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("false");
        options4.Add("true");

        node.data8 = NodeFunctions.DrawDropDownMenu(node, options4, "always load", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("red");
        options5.Add("green");
        options5.Add("yellow");

        node.data9 = NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> optionsAR = new List<string>();
        optionsAR.Add("false");
        optionsAR.Add("true");

        node.data10 = NodeFunctions.DrawDropDownMenu(node, optionsAR, "run once", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_SetFogDistanceAndColor(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_setfogdistanceandcolor", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_setfogdistanceandcolor", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "Fog Start", "5000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "Fog End", "10000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "Fog Colour", "#919DA0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_SetLighting(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_setlighting", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_setlighting", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "Colour", "#E2EAF4", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("false");
        options.Add("true");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "Enable Sun", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "Sun Intensity", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "Sun Scale", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_SetHudColour(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_sethutcolour", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_sethudcolour", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "Colour", "#D63320", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_SetSceneRadius(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_setsceneradius", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_setsceneradius", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "Radius", "5000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_SetSkybox(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_setskybox", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_setskybox", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetSkyboxList(), "skybox", "space", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options2, "stars", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

    #region event nodes

    public static void Draw_ActivateDocking(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "activatedocking", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "activatedocking", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "target ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("true");
        options1.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options1, "activate docking", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "rot speed", "3", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "mov speed", "5", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ActivateHyperspace(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "activatehyperspace", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "activatehyperspace", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ActivateWaypointMarker(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "activatewaypointmarker", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "activatewaypointmarker", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "isActive", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "Title", " ", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_AddAITagToLargeShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "addaitagtolargeship", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "addaitagtolargeship", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("nochange");
        options.Add("fullspeed");
        options.Add("threequarterspeed");
        options.Add("halfspeed");
        options.Add("quarterspeed");
        options.Add("dynamicspeed");
        options.Add("nospeed");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "speed", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("nochange");
        options2.Add("fireweapons");
        options2.Add("noweapons");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "weapons", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("nochange");
        options3.Add("movetotargetrange");
        options3.Add("circletarget");
        options3.Add("movetowaypoint");
        options3.Add("norotation");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options3, "pattern", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_AddAITagToSmallShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "addaitagtosmallship", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "addaitagtosmallship", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("nochange");
        options1.Add("fullspeedwithboost");
        options1.Add("fullspeed");
        options1.Add("threequarterspeed");
        options1.Add("halfspeed");
        options1.Add("quarterspeed");
        options1.Add("dynamicspeed");
        options1.Add("nospeed");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options1, "speed", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("nochange");
        options2.Add("singlelaser");
        options2.Add("duallasers");
        options2.Add("alllasers");
        options2.Add("singleion");
        options2.Add("dualion");
        options2.Add("allion");
        options2.Add("singletorpedo");
        options2.Add("dualtorpedos");
        options2.Add("alltorpedos");
        options2.Add("dynamicweapons_single");
        options2.Add("dynamicweapons_dual");
        options2.Add("dynamicweapons_quad");
        options2.Add("noweapons");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "weapons", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("nochange");
        options3.Add("lowaccuracy");
        options3.Add("mediumaccuracy");
        options3.Add("highaccuracy");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options3, "accuracy", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("nochange");
        options4.Add("chase");
        options4.Add("chasewithdraw");
        options4.Add("strafewithdraw");
        options4.Add("movetowaypoint");
        options4.Add("patrolrandom");
        options4.Add("formationflying");
        options4.Add("norotation");

        node.data5 = NodeFunctions.DrawDropDownMenu(node, options4, "pattern", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("nochange");
        options5.Add("resetenergylevels");
        options5.Add("energytoshields");
        options5.Add("energytoengines");
        options5.Add("energytolasers");
        options5.Add("energyprotective");
        options5.Add("energyaggressive");
        options5.Add("energydynamic");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options5, "energy", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options6 = new List<string>();
        options6.Add("nochange");
        options6.Add("targetallprefsmall");
        options6.Add("targetallpreflarge");
        options6.Add("targetsmallshipsonly");
        options6.Add("targetlargeshipsonly");

        node.data7 = NodeFunctions.DrawDropDownMenu(node, options6, "targeting", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_CampaignInformation(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "campaigninformation", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "campaigninformation", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "Campaign", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data2 = NodeFunctions.DrawInputFieldLarge(node, "Information", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        drop -= 15;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ChangeLocation(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "changelocation", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "changelocation", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "exit pos x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "exit pos y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "exit pos z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "exit rot x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "exit rot y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "exit rot z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "Jump to", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ChangeLocationFade(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "changelocationfade", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "changelocationfade", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "exit pos x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "exit pos y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "exit pos z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "exit rot x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "exit rot y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "exit rot z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "new location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "fade colour", "#000000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_CreateLocation(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "createlocation", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "createlocation", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("false");
        options.Add("true");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "Start Here", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ExitMission(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "exitmission", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "exitmission", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ExitAndDisplayNextMissionMenu(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "exitanddisplaynext", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "exitanddisplaynextmissionmenu", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "Next Mission", "none", 7, 5, drop, 12.5f, 90, 5f);

        List<string> options01 = new List<string>();
        options01.Add("none");
        options01.Add("fx7");
        options01.Add("gonk");
        options01.Add("ito");
        options01.Add("med");
        options01.Add("mk4");
        options01.Add("probe");
        options01.Add("r2");
        options01.Add("r4");
        options01.Add("r5");
        options01.Add("wed");

        drop -= 15;

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options01, "model", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data3 = NodeFunctions.DrawInputFieldLarge(node, "debriefing", "", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.data4 = NodeFunctions.DrawInputField(node, "audio file", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options02 = new List<string>();
        options02.Add("true");
        options02.Add("false");

        node.data5 = NodeFunctions.DrawDropDownMenu(node, options02, "internal file", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options03 = new List<string>();
        options03.Add("true");
        options03.Add("false");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options03, "distortion", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "level", "0.5", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options04 = new List<string>();
        options04.Add("true");
        options04.Add("false");

        node.data8 = NodeFunctions.DrawDropDownMenu(node, options04, "nextMissionButton", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DeactivateShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "deactivateship", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "deactivateship", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayHint(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displayhint", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displayhint", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 2;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Hint", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.data2 = NodeFunctions.DrawInputField(node, "Fontsize", "15", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayTitle(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaytitle", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaytitle", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 2;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Title", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.data2 = NodeFunctions.DrawInputField(node, "Fontsize", "25", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "Colour", "#FFFFFF", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayMissionBriefing(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaymissionbriefing", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaymissionbriefing", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Briefing", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.data2 = NodeFunctions.DrawInputField(node, "audio file", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options01 = new List<string>();
        options01.Add("true");
        options01.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options01, "internal file", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options02 = new List<string>();
        options02.Add("true");
        options02.Add("false");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options02, "distortion", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "level", "0.5", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options03 = new List<string>();
        options03.Add("none");
        options03.Add("fx7");
        options03.Add("gonk");
        options03.Add("ito");
        options03.Add("med");
        options03.Add("mk4");
        options03.Add("probe");
        options03.Add("r2");
        options03.Add("r4");
        options03.Add("r5");
        options03.Add("wed");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options03, "model", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayMessage(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaymessage", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaymessage" + "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Message", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.data2 = NodeFunctions.DrawInputField(node, "audio file", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options01 = new List<string>();
        options01.Add("true");
        options01.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options01, "internal file", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options02 = new List<string>();
        options02.Add("true");
        options02.Add("false");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options02, "distortion", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "level", "0.5", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayMessageImmediate(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaymessageimmediate", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaymessageimmediate" + "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "Message", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

        node.data2 = NodeFunctions.DrawInputField(node, "audio file", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options01 = new List<string>();
        options01.Add("true");
        options01.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options01, "internal file", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options02 = new List<string>();
        options02.Add("true");
        options02.Add("false");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options02, "distortion", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "level", "0.5", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfObjectiveIsActive(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifobjectiveisactive", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifobjectiveisactive", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "objective", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipsHullIsLessThan(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshipshullislessthan", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshipshullislessthan", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "is less than", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipIsLessThanDistanceToOtherShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshipislessthandistancetoothership", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshipislessthandistancetoothership", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "shipA", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "shipB", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "is less than", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipIsLessThanDistanceToWaypoint(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshipislessthandistancetowaypoint", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshipislessthandistancetowaypoint", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "is less than", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipIsActive(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshipisactive", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshipisactive", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipHasBeenDisabled(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshiphasbeendisabled", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshiphasbeendisabled", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipHasntBeenDisabled(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshiphasntbeendisabled", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshiphasntbeendisabled", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipHasBeenScanned(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshiphasbeenscanned", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshiphasbeenscanned", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipHasntBeenScanned(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshiphasntbeenscanned", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshiphasntbeenscanned", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipOfAllegianceIsActive(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshipofallegianceisactive", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshipofallegianceisactive", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("allships");
        options2.Add("smallships");
        options2.Add("largeships");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options2, "mode", "allships", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_IfShipsSystemsAreLessThan(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "ifshipssystemsarelessthan", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "ifshipssystemsarelessthan", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "is less than", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "True", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "False", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadMultipleShips(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadmultipleships", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadmultipleships", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("rectanglehorizontal");
        options3.Add("rectanglevertical");
        options3.Add("arrowhorizontal");
        options3.Add("arrowhorizontalinverted");
        options3.Add("linehorizontallongways");
        options3.Add("linehorizontalsideways");
        options3.Add("linevertical");
        options3.Add("randominsidecube");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options3, "pattern", "rectanglehorizontal", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "width", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "length", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "height", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "ships per line", "2", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "pos variance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("false");
        options4.Add("true");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options4, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("false");
        options5.Add("true");

        node.data13 = NodeFunctions.DrawDropDownMenu(node, options5, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data14 = NodeFunctions.DrawInputField(node, "player no.", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options6 = new List<string>();
        options6.Add("red");
        options6.Add("green");
        options6.Add("yellow");

        node.data15 = NodeFunctions.DrawDropDownMenu(node, options6, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadMultipleShipsFromHangar(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadmultipleshipsfromhangar", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadmultipleshipsfromhangar", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "launch ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "hangarNo", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "delay", "5", 7, 5, drop, 12.5f, 90, 5f);

        List<string> options = new List<string>();
        options.Add("red");
        options.Add("green");
        options.Add("yellow");

        drop -= 15;

        node.data9 = NodeFunctions.DrawDropDownMenu(node, options, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadMultipleShipsOnGround(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadmultipleshipsonground", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadmultipleshipsonground", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "dsturrettall", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "width", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "length", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "above ground", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "ships per line", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "pos variance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options4, "always load", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("red");
        options5.Add("green");
        options5.Add("yellow");

        node.data13 = NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadSingleShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadsingleship", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadsingleship", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("false");
        options3.Add("true");

        node.data5 = NodeFunctions.DrawDropDownMenu(node, options3, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options4, "is AI", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("red");
        options5.Add("green");
        options5.Add("yellow");

        node.data7 = NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadSingleShipAtDistanceAndAngleFromPlayer(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadsingleshipatdistanceandanglefromplayer", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadsingleshipatdistanceandanglefromplayer", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "xAngle", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "yAngle", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "zAngle", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "distance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("false");
        options3.Add("true");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options3, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("red");
        options4.Add("green");
        options4.Add("yellow");

        node.data7 = NodeFunctions.DrawDropDownMenu(node, options4, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_LoadSingleShipOnGround(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "loadsingleshiponground", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "loadsingleshiponground", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetShipList(), "type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "name", "Alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "rebel", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data5 = NodeFunctions.DrawDropDownMenu(node, options3, "is AI", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "above ground", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "pos variance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("false");
        options4.Add("true");

        node.data8 = NodeFunctions.DrawDropDownMenu(node, options4, "always load", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("red");
        options5.Add("green");
        options5.Add("yellow");

        node.data9 = NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PlayMusicTrack(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "playmusictrack", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "playmusictrack", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawDropDownMenu(node, GetTrackList(), "track", "adventure", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options2, "loop", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PauseSequence(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "pausesequence", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "pausesequence", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetCargo(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setcargo", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setcargo", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetControlLock(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setcontrollock", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setcontrollock", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "locked", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetFollowTarget(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setfollowtarget", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setfollowtarget", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "follow", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("arrow");
        options.Add("linesingle");
        options.Add("linedual");
        options.Add("random");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options, "formation", "arrow", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetObjective(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setobjective", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setobjective", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("addobjective");
        options.Add("cancelobjective");
        options.Add("completeobjective");
        options.Add("clearobjective");
        options.Add("clearallobjectives");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "mode", "addobjective", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "objective", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetDontAttackLargeShips(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setdontattacklargeships", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setdontattacklargeships", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "dont attack", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipAllegiance(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshipallegiance", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshipallegiance", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawDropDownMenu(node, GetAllegianceList(), "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipLevels(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshiplevels", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshiplevels", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "hull", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "shield", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "systems", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "wep", "nochange", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipStats(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshipstats", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshipstats", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "acceleration", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "speed", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "maneuverability", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "hull", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "shield", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "laserfirerate", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "laserpower", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "WEP", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipTarget(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshiptarget", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshiptarget", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "target", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipTargetToClosestEnemy(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshiptargettoclosestenemy", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshiptargettoclosestenemy", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipToCannotBeDisabled(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshiptocannotbedisabled", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshiptocannotbedisabled", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "cannotbedisabled", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetShipToInvincible(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setshiptoinvincible", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setshiptoinvincible", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "invincible", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetTorpedoes(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "settorpedoes", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "settorpedoes", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawDropDownMenu(node, GetTorpedoList(), "type", "proton torpedo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data3 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetWayPoint(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setwaypoint", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setwaypoint", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "waypoint x", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "waypoint y", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "waypoint z", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetWayPointToShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setwaypointtoship", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setwaypointtoship", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "target ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SetWeaponSelectionOnPlayerShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setweaponselectiononplayership", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setweaponselectiononplayership", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options01 = new List<string>();
        options01.Add("lasers");
        options01.Add("ion");
        options01.Add("torpedos");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options01, "weapon", "lasers", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options02 = new List<string>();
        options02.Add("single");
        options02.Add("dual");
        options02.Add("all");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options02, "mode", "single", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options03 = new List<string>();
        options03.Add("true");
        options03.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options03, "prevent change", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_SplitEventSeries(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "spliteventseries", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "spliteventseries", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 1", 7, 5);

        drop -= 15;

        node.nextEvent2 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 2", 7, 5);

        drop -= 15;

        node.nextEvent3 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 3", 7, 5);

        drop -= 15;

        node.nextEvent4 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 4", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_StartEventSeries(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "starteventseries", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "starteventseries", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

    #region automatically generated lists for different dropdown menus

    public static List<string> GetAllegianceList()
    {
        //This gets the Json ship data
        TextAsset allegianceFile = Resources.Load(OGGetAddress.files + "Allegiances") as TextAsset;
        Allegiances allegiances = JsonUtility.FromJson<Allegiances>(allegianceFile.text);

        List<string> allegianceList = new List<string>();

        foreach (Allegiance allegiance in allegiances.allegianceData)
        {
            allegianceList.Add(allegiance.allegiance);
        }

        return allegianceList;
    }

    public static List<string> GetEnvironmentList()
    {
        List<string> environmentList = new List<string>();

        Object[] environments = Resources.LoadAll(OGGetAddress.environments, typeof(GameObject));

        foreach (Object environment in environments)
        {
            if (!environment.name.Contains("plane"))
            {
                environmentList.Add(environment.name);
            }
        }

        return environmentList;
    }

    public static List<string> GetShipList()
    {
        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        List<string> shipList = new List<string>();

        foreach (ShipType shipType in shipTypes.shipTypeData)
        {
            shipList.Add(shipType.type);
        }

        return shipList;
    }

    public static List<string> GetAsteroidList()
    {
        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        List<string> asteroidList = new List<string>();

        foreach (ShipType shipType in shipTypes.shipTypeData)
        {
            if (shipType.prefab.Contains("asteroid") || shipType.prefab.Contains("debris"))
            {
                string asteroidSetName = GetNameBeforeUnderscore(shipType.prefab);

                bool add = true;

                foreach(string name in asteroidList)
                {
                    if (name == asteroidSetName)
                    {
                        add = false;
                        break;
                    }
                }

                if (add == true)
                {
                    asteroidList.Add(asteroidSetName);
                }
            }
        }

        return asteroidList;
    }

    public static string GetNameBeforeUnderscore(string name)
    {
        int underscoreIndex = name.IndexOf('_');
        if (underscoreIndex > 0)
            return name.Substring(0, underscoreIndex);
        else
            return name;
    }

    public static List<string> GetTorpedoList()
    {
        //This gets the Json ship data
        TextAsset torpedoTypesFile = Resources.Load(OGGetAddress.files + "TorpedoTypes") as TextAsset;
        TorpedoTypes torpdedoTypes = JsonUtility.FromJson<TorpedoTypes>(torpedoTypesFile.text);

        List<string> torpedoList = new List<string>();

        foreach (TorpedoType torpedoType in torpdedoTypes.torpedoTypeData)
        {
            torpedoList.Add(torpedoType.name);
        }

        return torpedoList;
    }

    public static List<string> GetTrackList()
    {
        List<string> trackList = new List<string>();

        trackList.Add("none");

        Object[] musicTracks = Resources.LoadAll(OGGetAddress.musicclips, typeof(AudioClip));

        foreach (Object track in musicTracks)
        {
            trackList.Add(track.name);
        }

        return trackList;
    }

    public static List<string> GetPlanetList()
    {
        List<string> planetList = new List<string>();

        planetList.Add("deathstar");
        planetList.Add("deathstar2");

        Object[] planets = Resources.LoadAll(OGGetAddress.planets_planetmaterials, typeof(Material));

        foreach (Object planet in planets)
        {
            planetList.Add(planet.name);
        }

        return planetList;
    }

    public static List<string> GetAtmosphereList()
    {
        List<string> atmosphereList = new List<string>();

        atmosphereList.Add("none");

        Object[] atmospheres = Resources.LoadAll(OGGetAddress.planets_atmospherematerials, typeof(Material));

        foreach (Object atmosphere in atmospheres)
        {
            atmosphereList.Add(atmosphere.name);
        }

        return atmosphereList;
    }

    public static List<string> GetCloudList()
    {
        List<string> cloudList = new List<string>();

        cloudList.Add("none");

        Object[] clouds = Resources.LoadAll(OGGetAddress.planets_cloudmaterials, typeof(Material));

        foreach (Object cloud in clouds)
        {
            cloudList.Add(cloud.name);
        }

        return cloudList;
    }

    public static List<string> GetSkyboxList()
    {
        List<string> skyboxList = new List<string>();

        Object[] skyboxes = Resources.LoadAll(OGGetAddress.skyboxes, typeof(Material));

        foreach (Object skybox in skyboxes)
        {
            skyboxList.Add(skybox.name);
        }

        return skyboxList;
    }

    #endregion

}
