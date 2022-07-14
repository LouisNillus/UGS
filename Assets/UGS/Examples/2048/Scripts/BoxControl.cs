using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game_2048
{
    public class BoxControl : MonoBehaviour
    {
    
        public int value;
        public TextMeshProUGUI scoreText;
        public SpriteRenderer sr;
    
        UGS_Grid grid;
    
        // Start
        void Start()
        {
            InitialValue();
    
            grid = PlayerController.instance.grid;
        }
    
        public void Double()
        {
            value *= 2;
            BindValueToText();
            SetColorFromGradient();
        }
    
        public void BindValueToText()
        {
            scoreText.text = value.ToString();
        }
    
        public void InitialValue()
        {
            value = Random.Range(0, 100) > 90 ? 4 : 2;
            BindValueToText();
            SetColorFromGradient();
        }
    
        public void SetColorFromGradient()
        {
            int key = (int)(Mathf.Log(value) / Mathf.Log(2));
    
            sr.color = PlayerController.instance.boxesGradient.Evaluate(Mathf.InverseLerp(1, 11, key));
        }
    
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H)) Double();
        }
    
        public void Kill()
        {
            Destroy(this.gameObject);
        }
    
    
    }
}