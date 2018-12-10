using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScaffoldPos { bottom, left, right }

public class Scaffold : MonoBehaviour {

    public Script entity;
    public ScaffoldPos pos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (pos)
        {
            case ScaffoldPos.bottom:
                entity.scaffold_col++;
                break;
            case ScaffoldPos.left:
                entity.left_col++;
                break;
            case ScaffoldPos.right:
                entity.right_col++;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (pos)
        {
            case ScaffoldPos.bottom:
                entity.scaffold_col--;
                break;
            case ScaffoldPos.left:
                entity.left_col--;
                break;
            case ScaffoldPos.right:
                entity.right_col--;
                break;
        }
    }
}
