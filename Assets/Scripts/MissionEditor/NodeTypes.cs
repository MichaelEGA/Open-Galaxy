using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeTypes
{
    //This generates the test node
    public static void DrawTestNode(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawNodeLink(node, 7.5f, -12f, 10, 10, "female");

        NodeFunctions.DrawText(node, "Test Node", 8, 17.5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        NodeFunctions.DrawInputField(node, "Event Type", "none", 7, 5, -25, 12.5f, 90, 5f);

        NodeFunctions.DrawDropDownMenu(node, "Ship Type", "none", 7, 5, -40, 12.5f, 90, 5f);

        NodeFunctions.DrawNodeLink(node, 87.5f, -62.5f, 10, 10, "male");
    }

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

        node.conditionTime = NodeFunctions.DrawInputField(node, "Time", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.conditionLocation = NodeFunctions.DrawInputField(node, "Location", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.x = NodeFunctions.DrawInputField(node, "x", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.y = NodeFunctions.DrawInputField(node, "y", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.z = NodeFunctions.DrawInputField(node, "z", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.xRotation = NodeFunctions.DrawInputField(node, "xRot", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.yRotation = NodeFunctions.DrawInputField(node, "yRot", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        node.zRotation = NodeFunctions.DrawInputField(node, "zRot", "none", 7, 5, drop, 12.5f, 90, 5f);

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

    public static void DrawPreLoad_LoadAsteroids(Node node)
    {
        NodeFunctions.DrawNodeBase(node);

        NodeFunctions.DrawText(node, "pl_loadasteroids", 8, 5f, -5, 12.5f, 90);

        NodeFunctions.DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        NodeFunctions.DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        float drop = -25;

        node.data1 = NodeFunctions.DrawInputField(node, "No. of Ast...", "none", 7, 5, drop, 12.5f, 90, 5f);

        drop -= 15;

        NodeFunctions.DrawText(node, "Leave this as 'none' if you want the asteroid number to be set by the planet seed.", 5, 5, drop, 20, 90);

        drop -= 30;

        NodeFunctions.SetNodeSize(node, 100, Mathf.Abs(drop));
    }
}
