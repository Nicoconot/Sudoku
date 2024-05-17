using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuColors : MonoBehaviour
{
    public static Color transparent = new Color(0, 0, 0, 0);
    public static Color standard = new Color(.2f, .2f, .2f);
    public static Color correct = new Color(.3f, .6f, .1f);
    public static Color wrong = Color.red;
    public static Color completed = new Color(.2f, .2f, .2f, .5f);
    public static Color selectedText = new Color(.15f, .5f, .75f);
    public static Color playableSelectedText = new Color(.15f, .5f, .3f);
    public static Color selectedBg = new Color(.45f, .7f, .8f, .5f);
    public static Color highlightedBg = new Color(.7f, .8f, .85f, .3f);
    public static Color highlightedText = new Color(.2f, .2f, .2f);
}
