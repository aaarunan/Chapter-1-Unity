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
        editMode = true;
    }
    private void OnEnable()
    {
        Actions.OnEditMode += OnEditMode;
    }




    void Update()
    {
        editModeHUD.gameObject.SetActive(editMode);
    }

    private void OnEditMode()
    {
        editMode = !editMode;
        print("Edit mode GUI " + editMode);
    }
}
