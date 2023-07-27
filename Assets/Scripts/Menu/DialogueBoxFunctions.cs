using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public static class DialogueBoxFunctions
{
    //This display the exit menu
    public static void DisplayDialogueBox(bool isDisplaying, string dialogue)
    {
        DialogueBox dialogueBox = GameObject.FindObjectOfType<DialogueBox>(true);
        GameObject dialogueBoxGO = null;

        if (dialogueBox != null)
        {
            dialogueBoxGO = dialogueBox.gameObject;
        }

        if (isDisplaying == true)
        {

            if (dialogueBoxGO == null)
            {
                GameObject exitMenuPrefab = Resources.Load("Menu/DialogueBox") as GameObject;
                dialogueBoxGO = GameObject.Instantiate(exitMenuPrefab);
                dialogueBox = dialogueBoxGO.AddComponent<DialogueBox>();
                dialogueBoxGO.name = "DialogueBox";
            }

            if (dialogueBoxGO != null)
            {
                Time.timeScale = 0;

                Text textBox = dialogueBoxGO.GetComponentInChildren<Text>();
                textBox.text = dialogue;

                dialogueBox.loadTime = Time.unscaledTime; //This is used to prevent the dialogue box disappearing straight away if the player is pressing a button when it's loaded

                dialogueBoxGO.SetActive(true);
            }
        }
        else
        {
            Time.timeScale = 1;

            if (dialogueBoxGO != null)
            {
                Text textBox = dialogueBoxGO.GetComponentInChildren<Text>();
                textBox.text = "";
                dialogueBoxGO.SetActive(false);
            }

        }
    }

    public static void ReturnToGame(DialogueBox dialogueBox, GameObject dialogue)
    {
        if (dialogueBox.loadTime + 2.5f < Time.unscaledTime) //This is used to prevent the dialogue box disappearing straight away if the player is pressing a button when it's loaded
        {
            bool returnToGame = false;

            var gamepad = Gamepad.current;

            if (gamepad != null)
            {
                if (gamepad.dpad.left.isPressed == true) { returnToGame = true; }
                else if (gamepad.dpad.left.isPressed) { returnToGame = true; }
                else if (gamepad.dpad.up.isPressed) { returnToGame = true; }
                else if (gamepad.dpad.right.isPressed) { returnToGame = true; }
                else if (gamepad.dpad.down.isPressed) { returnToGame = true; }
                else if (gamepad.leftShoulder.isPressed) { returnToGame = true; }
                else if (gamepad.rightShoulder.isPressed) { returnToGame = true; }
                else if (gamepad.rightTrigger.isPressed) { returnToGame = true; }
                else if (gamepad.bButton.isPressed) { returnToGame = true; }
                else if (gamepad.aButton.isPressed) { returnToGame = true; }
                else if (gamepad.xButton.isPressed) { returnToGame = true; }
                else if (gamepad.yButton.isPressed) { returnToGame = true; }
                else if (gamepad.startButton.isPressed) { returnToGame = true; }
                else if (gamepad.selectButton.isPressed) { returnToGame = true; }
                else if (gamepad.rightStickButton.isPressed) { returnToGame = true; }
                else if (gamepad.leftStickButton.isPressed) { returnToGame = true; }
                else if (gamepad.leftTrigger.isPressed) { returnToGame = true; }
            }

            var keyboard = Keyboard.current;
            var mouse = Mouse.current;

            if (keyboard != null)
            {
                if (keyboard.anyKey.wasPressedThisFrame == true) { returnToGame = true; }
            }

            if (mouse != null)
            {
                if (mouse.leftButton.isPressed == true) { returnToGame = true; }
                else if (mouse.rightButton.isPressed == true) { returnToGame = true; }
            }

            if (returnToGame == true)
            {
                Time.timeScale = 1;
                dialogue.SetActive(false);
            }
        }
    }
}
