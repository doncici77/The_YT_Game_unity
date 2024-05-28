using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChoiceGroup : MonoBehaviour
{
    public GameObject[] statChoices; // 선택지 오브젝트 배열

    public void ChooseStat(GameObject chosenStat)
    {
        foreach (var choice in statChoices)
        {
            if (choice != null)
            {
                Destroy(choice); // 모든 선택지 제거
            }
        }
        Destroy(gameObject); // 그룹 오브젝트 제거
    }
}
