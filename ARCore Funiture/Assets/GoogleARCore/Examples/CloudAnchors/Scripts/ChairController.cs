//-----------------------------------------------------------------------
// <copyright file="AnchorController.cs" company="Google">
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

namespace GoogleARCore.Examples.CloudAnchors
{
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// A Controller for the Anchor object that handles hosting and resolving the Cloud Anchor.
    /// </summary>
    public class ChairController : MonoBehaviour
    {
        
        /// <summary>
        /// The anchor mesh object.
        /// In order to avoid placing the Anchor on identity pose, the mesh object should
        /// be disabled by default and enabled after hosted or resolved.
        /// </summary>
        private GameObject m_ChairMesh;

        /// <summary>
        /// The Cloud Anchors example controller.
        /// </summary>
        private CloudAnchorsExampleController m_CloudAnchorsExampleController;

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            m_CloudAnchorsExampleController =
                GameObject.Find("CloudAnchorsExampleController")
                    .GetComponent<CloudAnchorsExampleController>();
            m_ChairMesh = transform.Find("mesh_chair").gameObject;
            m_ChairMesh.SetActive(false);
        }



        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            if (m_ChairMesh.activeSelf)
            {
                return;
            }

            // Only sets the Star object's mesh after the origin is placed to avoid being placed
            // at identity pose.
            if (!m_CloudAnchorsExampleController.IsOriginPlaced)
            {
                return;
            }

            m_ChairMesh.SetActive(true);
        }
    }
}
