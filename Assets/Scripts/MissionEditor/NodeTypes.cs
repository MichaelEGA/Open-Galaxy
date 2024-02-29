using System.Collections;
using System.Collections.Generic;
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

        node.data3 = NodeFunctions.DrawInputField(node, "Image File", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

    #region pre load event nodes

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

        node.data1 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        NodeFunctions.DrawText(node, "Leave this as 'none' or '0' if you want the asteroid number to be set by the location seed.", 5, 5, drop, 20, 90);

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

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_PreLoad_LoadTerrain(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_preload_loadterrain", 8, 5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadterrain", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("Canyon01");
        options1.Add("Canyon02");
        options1.Add("Cliff01");
        options1.Add("Cliff02");
        options1.Add("Flat01");
        options1.Add("Flat02");
        options1.Add("Mountain01");
        options1.Add("Mountain02");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options1, "terrain", "Canyon01", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("Asteroid01");
        options2.Add("Asteroid02");
        options2.Add("Desert01");
        options2.Add("Desert02");
        options2.Add("Desert03");
        options2.Add("Forest01");
        options2.Add("Forest02");
        options2.Add("Snow01");
        options2.Add("Snow02");
        options2.Add("Snow03");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options2, "material", "Asteroid01", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "position", "-15000", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data2 = NodeFunctions.DrawInputField(node, "name", "tower", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");
        options2.Add("pirate");
        options2.Add("smuggler");
        options2.Add("hutt");
        options2.Add("hostile");
        options2.Add("corporatesector");
        options2.Add("independent");
        options2.Add("civilian");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data13= NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data2 = NodeFunctions.DrawInputField(node, "name", "alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");
        options2.Add("pirate");
        options2.Add("smuggler");
        options2.Add("hutt");
        options2.Add("hostile");
        options2.Add("corporatesector");
        options2.Add("independent");
        options2.Add("civilian");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("false");
        options3.Add("true");
    
        node.data5 = NodeFunctions.DrawDropDownMenu(node, options3, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("false");
        options4.Add("true");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options4, "is AI", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("red");
        options5.Add("green");

        node.data7 = NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> optionsAR = new List<string>();
        optionsAR.Add("false");
        optionsAR.Add("true");

        node.data8 = NodeFunctions.DrawDropDownMenu(node, optionsAR, "run once", "false", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data2 = NodeFunctions.DrawInputField(node, "name", "alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");
        options2.Add("pirate");
        options2.Add("smuggler");
        options2.Add("hutt");
        options2.Add("hostile");
        options2.Add("corporatesector");
        options2.Add("independent");
        options2.Add("civilian");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data10 = NodeFunctions.DrawInputField(node, "ships per line", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "pos variance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options4, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("true");
        options5.Add("false");

        node.data13 = NodeFunctions.DrawDropDownMenu(node, options5, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data14 = NodeFunctions.DrawInputField(node, "player no.", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options6 = new List<string>();
        options6.Add("red");
        options6.Add("green");

        node.data15 = NodeFunctions.DrawDropDownMenu(node, options6, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

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

        List<string> options1 = new List<string>();
        options1.Add("space_black");
        options1.Add("space_nebula01");
        options1.Add("space_nebula02");
        options1.Add("space_nebula03");
        options1.Add("sky_blue01");
        options1.Add("sky_blue02");
        options1.Add("sky_blue03");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options1, "mode", "space", 7, 5, drop, 12.5f, 90, 5f);

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

    public static void Draw_ClearAIOverride(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "clearaioverride", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "clearaioverride", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

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

    public static void Draw_DisplayDialogueBox(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaydialoguebox", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaydialoguebox", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        float multiplySize = 5;

        node.data1 = NodeFunctions.DrawInputFieldLarge(node, "message", "none", 7, 5, drop, 12.5f * multiplySize, 90);

        drop -= 15 * multiplySize;

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

        node.data2 = NodeFunctions.DrawInputField(node, "Fontsize", "12", 7, 5, drop, 12.5f, 90);

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

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options, "internal file", "true", 7, 5, drop, 12.5f, 90, 5f);

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

        List<string> options = new List<string>();
        options.Add("true");
        options.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options, "internal file", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

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

        node.data3 = NodeFunctions.DrawInputField(node, "is less than", "50", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data2 = NodeFunctions.DrawInputField(node, "is less than", "50", 7, 5, drop, 12.5f, 90, 5f);

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

        List<string> options1 = new List<string>();
        options1.Add("imperial");
        options1.Add("rebel");
        options1.Add("pirate");
        options1.Add("smuggler");
        options1.Add("hutt");
        options1.Add("hostile");
        options1.Add("corporatesector");
        options1.Add("independent");
        options1.Add("civilian");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options1, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data2 = NodeFunctions.DrawInputField(node, "name", "alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");
        options2.Add("pirate");
        options2.Add("smuggler");
        options2.Add("hutt");
        options2.Add("hostile");
        options2.Add("corporatesector");
        options2.Add("independent");
        options2.Add("civilian");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data5 = NodeFunctions.DrawDropDownMenu(node, options3, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options4, "is AI", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("red");
        options5.Add("green");

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

        node.data2 = NodeFunctions.DrawInputField(node, "name", "alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");
        options2.Add("pirate");
        options2.Add("smuggler");
        options2.Add("hutt");
        options2.Add("hostile");
        options2.Add("corporatesector");
        options2.Add("independent");
        options2.Add("civilian");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "cargo", "no cargo", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "distance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data6 = NodeFunctions.DrawDropDownMenu(node, options3, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("red");
        options4.Add("green");

        node.data7 = NodeFunctions.DrawDropDownMenu(node, options4, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data2 = NodeFunctions.DrawInputField(node, "name", "tower", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");
        options2.Add("pirate");
        options2.Add("smuggler");
        options2.Add("hutt");
        options2.Add("hostile");
        options2.Add("corporatesector");
        options2.Add("independent");
        options2.Add("civilian");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data13 = NodeFunctions.DrawDropDownMenu(node, options5, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event", 7, 5);

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

        node.data2 = NodeFunctions.DrawInputField(node, "name", "alpha", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");
        options2.Add("pirate");
        options2.Add("smuggler");
        options2.Add("hutt");
        options2.Add("hostile");
        options2.Add("corporatesector");
        options2.Add("independent");
        options2.Add("civilian");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

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

        node.data10 = NodeFunctions.DrawInputField(node, "ships per line", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "pos variance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options4, "exiting hyperspace", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options5 = new List<string>();
        options5.Add("true");
        options5.Add("false");

        node.data13 = NodeFunctions.DrawDropDownMenu(node, options5, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data14 = NodeFunctions.DrawInputField(node, "player no.", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options6 = new List<string>();
        options6.Add("red");
        options6.Add("green");

        node.data15 = NodeFunctions.DrawDropDownMenu(node, options6, "laser color", "red", 7, 5, drop, 12.5f, 90, 5f);

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

        List<string> options1 = new List<string>();
        options1.Add("traitor");
        options1.Add("deathstar");
        options1.Add("endor");
        options1.Add("exodus");
        options1.Add("beginning");
        options1.Add("hoth");
        options1.Add("lose");
        options1.Add("mosespa");
        options1.Add("taloraan");
        options1.Add("win");
        options1.Add("march01");
        options1.Add("march02");
        options1.Add("hope");
        options1.Add("last");
        options1.Add("adventure");
        options1.Add("battle");
        options1.Add("chase");
        options1.Add("none");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options1, "track", "first_strike", 7, 5, drop, 12.5f, 90, 5f);

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

    public static void Draw_SetAIOverride(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setaioverride", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setaioverride", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "ship", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options = new List<string>();
        options.Add("none");
        options.Add("MoveToWaypoint");
        options.Add("Patrol");
        options.Add("Stationary");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "override", "none", 7, 5, drop, 12.5f, 90, 5f);

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

        List<string> options = new List<string>();
        options.Add("imperial");
        options.Add("rebel");
        options.Add("pirate");
        options.Add("smuggler");
        options.Add("hutt");
        options.Add("hostile");
        options.Add("corporatesector");
        options.Add("independent");
        options.Add("civilian");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

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

    public static void Draw_SetWeaponsLock(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "setweaponslock", 8, 17.5f, -5, 12.5f, 65);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "setweaponslock", 7, 5, drop, 12.5f, 90);

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

    #endregion

    #region standard lists for different dropdown menus

    public static List<string> GetShipList()
    {
        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load("Data/Files/ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        List<string> shipList = new List<string>();

        foreach (ShipType shipType in shipTypes.shipTypeData)
        {
            shipList.Add(shipType.type);
        }

        return shipList;
    }

    #endregion

}
