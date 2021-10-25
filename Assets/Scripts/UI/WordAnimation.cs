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
    Mesh initMesh;
    Vector3[] initVertices;
    Vector3[] vertices;

    [SerializeField] private bool isAnimating;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    public void ShakeWord(string inputWord, Vector3 speed, float amplitude)
    {
        isAnimating = true;
        TMP_WordInfo[] wordArr = textMesh.textInfo.wordInfo;
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        initMesh = mesh;
        vertices = mesh.vertices;
        initVertices = vertices;

        for(int w=0; w<wordArr.Length; w++)
        {
            try
            {
                var word = wordArr[w];                
                if (string.Equals(word.GetWord().ToLower(), inputWord.ToLower()))
                {
                    Shake(word, speed, amplitude);
                    return;
                }
            }
            catch(Exception e) {}
        }
    }

    private void Shake(TMP_WordInfo word, Vector2 speed, float amplitude)
    {
        //Debug.Log($"Found word: {inputWord}");
        LeanTween.value(gameObject, 0,1, 200000).setOnUpdate((float val) =>
        {
            if (!isAnimating)
            {
                LeanTween.cancel(gameObject);
                return;
            }

            for (int c=word.firstCharacterIndex; c<=word.lastCharacterIndex; c++)
            {
                Vector3 offset = Wobble(Time.time+c, new Vector2(speed.x, speed.y), amplitude);
                int index = word.textComponent.textInfo.characterInfo[c].vertexIndex;
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
            if (mesh.vertices.Length == mesh.triangles.Length * 3)
            {
                mesh.vertices = vertices;
                textMesh.canvasRenderer.SetMesh(mesh);
            }
        });
    }

    Vector2 Wobble(float time, Vector2 speed, float amplitude) 
    {
        return new Vector2(amplitude * Mathf.Sin(time*speed.x), amplitude * Mathf.Cos(time*speed.y));
    }

    private void OnAnimateText(string text, Vector2 speed, float amplitude)
    {
        ShakeWord(text, speed, amplitude);
    }

    private void OnTouchBegan(Vector3 pos)
    {
        var tutState = GameManager.Instance.currTutCanvas.tutState;

        if (isAnimating)
        { 
           
            isAnimating = false;
            LeanTween.cancel(gameObject);

            if (initMesh.vertices.Length == initMesh.triangles.Length * 3)
            {
                initMesh.vertices = initVertices;
                textMesh.canvasRenderer.SetMesh(initMesh);
                textMesh.ForceMeshUpdate();
            }
        }
    }

    private void OnEnable()
    {
        EventManager.Tutorials.onAnimateText += OnAnimateText;
        EventManager.Input.onTouchBegan += OnTouchBegan;

    }

    private void OnDisable()
    {
        EventManager.Tutorials.onAnimateText -= OnAnimateText;
        EventManager.Input.onTouchBegan -= OnTouchBegan;
    }
}
}
