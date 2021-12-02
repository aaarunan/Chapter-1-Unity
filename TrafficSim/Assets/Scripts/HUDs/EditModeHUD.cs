using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class EditModeHUD : MonoBehaviour
{
    public Canvas editModeHUD;
    private static bool editMode;
    // Start is called before the first frame update

    private void Start()
    {
        editMode = false;
        EditMode(editMode);
    }
    private void OnEnable()
    {
        EditMode(true);
        Actions.OnEditMode += EditMode;
        
    }

    private void OnDestroy()
    {
        Actions.OnEditMode -= EditMode;
    }
    

    private void EditMode(bool editMode)
    {
        gameObject.SetActive(editMode);
        
    }
}
