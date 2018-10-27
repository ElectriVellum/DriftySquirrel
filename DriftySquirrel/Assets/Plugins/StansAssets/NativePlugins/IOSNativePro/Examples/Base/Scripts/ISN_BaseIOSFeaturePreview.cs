////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA.iOS.Examples
{

    public class ISN_BaseIOSFeaturePreview : MonoBehaviour
    {

        protected GUIStyle style;

        protected int buttonWidth = 200;
        protected int buttonHeight = 75;
        protected float StartY = 20;
        protected float StartX = 10;


        protected float XStartPos = 10;
        protected float YStartPos = 10;

        protected float XButtonStep = 220;
        protected float YButtonStep = 90;

        protected float YLableStep = 50;

        protected virtual void InitStyles() {
            style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 16;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.alignment = TextAnchor.UpperLeft;
            style.wordWrap = true;
        }

        public virtual void Start() {
            InitStyles();
        }

        public void UpdateToStartPos() {
            StartY = YStartPos;
            StartX = XStartPos;
        }

        public void LoadLevel(string levelName) {
            SceneManager.LoadScene(levelName);
        }
    }
}