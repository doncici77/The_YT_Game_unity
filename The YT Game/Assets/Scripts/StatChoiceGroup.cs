using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChoiceGroup : MonoBehaviour
{
    public GameObject[] statChoices;

    public void DisableChoices()
    {
        foreach (GameObject choice in statChoices)
        {
            choice.SetActive(false);
        }
    }
}
