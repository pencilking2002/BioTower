using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace BioTower
{
public class WordAnimation : MonoBehaviour
{
    private TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    public void ShakeWord(string inputWord)
    {

        TMP_WordInfo[] wordArr = textMesh.textInfo.wordInfo;
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for(int w=0; w<wordArr.Length; w++)
        {
            try
            {
                var word = wordArr[w];
                //Debug.Log($"word: {word.GetWord().ToLower()}");
                
                if (string.Equals(word.GetWord().ToLower(), inputWord.ToLower()))
                {
                    Shake(word);
                    return;
                }
            }
            catch(Exception e)
            {
                
            }
            
        }
    }

    private void Shake(TMP_WordInfo word)
    {
        //Debug.Log($"Found word: {inputWord}");
        LeanTween.value(gameObject, 0,1, 5).setOnUpdate((float val) =>
        {
            for (int c=word.firstCharacterIndex; c<=word.lastCharacterIndex; c++)
            {
                Vector3 offset = Wobble(Time.time+c, new Vector2(100,10), 0.1f);
                int index = word.textComponent.textInfo.characterInfo[c].vertexIndex;
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);
        });
    }

    Vector2 Wobble(float time, Vector2 speed, float amplitude) {
        return new Vector2(amplitude * Mathf.Sin(time*speed.x), amplitude * Mathf.Cos(time*speed.y));
    }
}
}
