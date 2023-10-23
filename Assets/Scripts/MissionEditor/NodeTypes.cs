using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeTypes
{
    #region generic nodes

    //This generates the test node
    public static void DrawTestNode(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "Test Node", 8, 17.5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        NodeFunctions.DrawInputField(node, "Event Type", "none", 7, 5, -25, 12.5f, 90, 5f);

        List<string> options = new List<string>();
        options.Add("x-wing");
        options.Add("y-wing");
        options.Add("a-wing");

        NodeFunctions.DrawDropDownMenu(node, options, "Ship Type", "none", 7, 5, -40, 12.5f, 90, 5f);

        NodeFunctions.DrawNodeLink(node, 87.5f, -62.5f, 10, 10, "male");
    }

    //This generates dummy node with no input
    public static void DrawNodeNotAvaible(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "node not avaible", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        NodeFunctions.DrawText(node, "This node does not exist yet. Check again in later versions to see if it has been added.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    //This draws a custom node that can be used to implement a custom function
    public static void DrawCustomNode(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "custom_node", 8, 17.5f, -5, 12.5f, 90);

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

    public static void DrawLoadScene(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadscene", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadscene", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        NodeFunctions.DrawText(node, "Leave this as 'none' if you want the game to randomly select a location each time.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void DrawPreLoad_LoadAsteroids(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadasteroids", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadasteroids", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        NodeFunctions.DrawText(node, "Leave this as 'none' or '0' if you want the asteroid number to be set by the location seed.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void DrawPreLoad_LoadPlanet(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadplanet", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadplanet", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        NodeFunctions.DrawText(node, "This node loads the planet set at the location set in the 'Load Scene' node.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void DrawPreLoad_LoadTiles(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadtiles", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadtiles", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "tile type", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x distance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y distance", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "tile size", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "seed", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void DrawPreLoad_LoadMultipleShipsOnGround(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadmultipleshipsonground", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadmultipleshipsonground", 7, 5, drop, 12.5f, 90);

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

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("x-wing");
        options.Add("y-wing");
        options.Add("a-wing");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "ship name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("imperial");
        options2.Add("rebel");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "no. per length", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "length", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "width", "1000", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "height", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data10 = NodeFunctions.DrawDropDownMenu(node, options3, "randomise", "true", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options4 = new List<string>();
        options4.Add("true");
        options4.Add("false");

        node.data11 = NodeFunctions.DrawDropDownMenu(node, options4, "always load", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void DrawPreLoad_LoadShip(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadship", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadship", 7, 5, drop, 12.5f, 90);

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

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("x-wing");
        options.Add("y-wing");
        options.Add("a-wing");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("true");
        options1.Add("false");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options1, "is AI", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data3 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("imperial");
        options3.Add("rebel");

        node.data4 = NodeFunctions.DrawDropDownMenu(node, options3, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void DrawPreLoad_LoadShipsByName(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadshipsbyname", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadshipsbyname", 7, 5, drop, 12.5f, 90);

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

        List<string> options = new List<string>();
        options.Add("tiefighter");
        options.Add("x-wing");
        options.Add("y-wing");
        options.Add("a-wing");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "tiefighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data2 = NodeFunctions.DrawInputField(node, "number", "1", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "groups of", "4", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "ship distance", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "group distance", "250", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data8 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data9 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data10 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data11 = NodeFunctions.DrawDropDownMenu(node, options3, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data12 = NodeFunctions.DrawInputField(node, "player no.", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void DrawPreLoad_LoadShipsByTypeAndAllegiance(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadshipsbytype", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_loadshipsbytypeandallegiance", 7, 5, drop, 12.5f, 90);

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

        List<string> options = new List<string>();
        options.Add("fighter");
        options.Add("bomber");
        options.Add("capital");
        options.Add("station");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options, "ship type", "fighter", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options1 = new List<string>();
        options1.Add("imperial");
        options1.Add("rebel");

        node.data2 = NodeFunctions.DrawDropDownMenu(node, options1, "allegiance", "imperial", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data4 = NodeFunctions.DrawInputField(node, "number", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data5 = NodeFunctions.DrawInputField(node, "groups of", "4", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data6 = NodeFunctions.DrawInputField(node, "ship distance", "50", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data7 = NodeFunctions.DrawInputField(node, "group distance", "250", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data9 = NodeFunctions.DrawInputField(node, "variance", "10", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("true");
        options2.Add("false");

        node.data10 = NodeFunctions.DrawDropDownMenu(node, options2, "randomise", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data11 = NodeFunctions.DrawInputField(node, "name", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        List<string> options3 = new List<string>();
        options3.Add("true");
        options3.Add("false");

        node.data12 = NodeFunctions.DrawDropDownMenu(node, options3, "include player", "false", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data13 = NodeFunctions.DrawInputField(node, "player no", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void DrawPreLoad_SetSkybox(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_setskybox", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventType = NodeFunctions.DrawText(node, "preload_setskybox", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        List<string> options2 = new List<string>();
        options2.Add("space");
        options2.Add("sky");

        node.data1 = NodeFunctions.DrawDropDownMenu(node, options2, "mode", "space", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

    #region event nodes

    public static void Draw_ChangeMusicVolume(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "changemusicvolume", 8, 17.5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "changemusicvolume", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "volume", "100", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 1", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_ClearAIOverride(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "clearaioverride", 8, 17.5f, -5, 12.5f, 90);

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

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 1", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    public static void Draw_DisplayLargeMessageThenExit(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "displaylargemessagethenexit", 8, 17.5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.eventID = NodeFunctions.DrawText(node, "", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.eventType = NodeFunctions.DrawText(node, "displaylargemessagethenexit", 7, 5, drop, 12.5f, 90);

        drop -= 15;

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "0", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.data1 = NodeFunctions.DrawInputField(node, "message", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.nextEvent1 = NodeFunctions.DrawNodeLink(node, 5, drop, 12.5f, 90, "male", "Next Event 1", 7, 5);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }

    #endregion

}
