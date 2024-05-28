using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f)
    {
        GameObject tempGO = new GameObject("TempAudio"); // 임시 오디오 오브젝트 생성
        tempGO.transform.position = position; // 위치 설정
        AudioSource audioSource = tempGO.AddComponent<AudioSource>(); // 오디오 소스 컴포넌트 추가
        audioSource.clip = clip;
        audioSource.spatialBlend = 0f; // 2D 사운드 설정
        audioSource.volume = volume; // 볼륨 설정
        audioSource.Play(); // 사운드 재생
        GameObject.Destroy(tempGO, clip.length); // 사운드 길이만큼 오브젝트 유지 후 파괴
    }
}
