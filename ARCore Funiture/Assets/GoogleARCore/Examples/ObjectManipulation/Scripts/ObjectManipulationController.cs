//-----------------------------------------------------------------------
// <copyright file="ObjectManipulationController.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.ObjectManipulation
{

    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloObjectManipulation example.
    /// </summary>
    public class ObjectManipulationController : MonoBehaviour
    {
        public static GameObject SelectedModel = null;
        public static bool move = false;

        public GameObject EditPanel;
        public GameObject SelectPanel;
        public GameObject ColorSliders;
        public GameObject Button;

        int counter;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();
            Button.SetActive(true);

            // If the player has not touched the screen then the update is complete.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            RaycastHit hit2;
            Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(raycast, out hit2, Mathf.Infinity))
            {
                SelectedModel = hit2.transform.gameObject;
                SelectedModel.transform.gameObject.SetActive(true);
                EnableEditing();
            }
        }

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("ARCore");
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        public void ShowHidePanel()
        {
            Vector3 pos = Button.transform.position;
            counter++;
            if (counter % 2 == 1)
            {
                SelectPanel.SetActive(true);
                GameObject.Find("Button").GetComponentInChildren<Text>().text = "-";
                pos.y += 150f;
                Button.transform.position = pos;
            }
            else
            {
                SelectPanel.SetActive(false);
                GameObject.Find("Button").GetComponentInChildren<Text>().text = "+";
                pos.y -= 150f;
                Button.transform.position = pos;
            }
        }

        public void EnableEditing()
        {
            EditPanel.SetActive(true);
            ColorSliders.SetActive(false);
        }

        public void QuitEditing()
        {
            ColorSliders.SetActive(false);
            EditPanel.SetActive(false);
        }

        public void DeleteObject()
        {
            Destroy(SelectedModel.transform.parent.gameObject);
            SelectedModel = null;
            EditPanel.SetActive(false);
            ColorSliders.SetActive(false);
        }

        public void EnableColor()
        {
            ColorSliders.SetActive(true);

            Material mat = new Material(Shader.Find("Standard"));
            GameObject mod = SelectedModel.transform.Find("model").gameObject;
            GameObject ss = mod.transform.Find("color").gameObject;
            foreach (Transform child in ss.transform)
            {
                Renderer rend = child.gameObject.GetComponent<Renderer>();
                rend.material = mat;
                rend.material.SetColor("_Color", new Color(0, 0, 0));
                // Debug.Log("color" + ;
            }
        }

        public void ChangeRed(Slider s)
        {
            foreach (Transform child in SelectedModel.transform.Find("model").gameObject.transform.Find("color").gameObject.transform)
            {
                Renderer rend = child.gameObject.GetComponent<Renderer>();
                rend.material.SetColor("_Color", new Color(s.value, rend.material.color[1], rend.material.color[2]));
            }
        }

        public void ChangeGreen(Slider s)
        {
            foreach (Transform child in SelectedModel.transform.Find("model").gameObject.transform.Find("color").gameObject.transform)
            {
                Renderer rend = child.gameObject.GetComponent<Renderer>();
                rend.material.SetColor("_Color", new Color(rend.material.color[0], s.value, rend.material.color[2]));
            }
        }

        public void ChangeBlue(Slider s)
        {
            foreach (Transform child in SelectedModel.transform.Find("model").gameObject.transform.Find("color").gameObject.transform)
            {
                Renderer rend = child.gameObject.GetComponent<Renderer>();
                rend.material.SetColor("_Color", new Color(rend.material.color[0], rend.material.color[1], s.value));
            }
        }

    }
}
