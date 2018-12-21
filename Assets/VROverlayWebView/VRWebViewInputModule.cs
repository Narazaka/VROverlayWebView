using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using sh_akira.VROverlay;
using ICODES.STUDIO.WWebView;

namespace VROverlayWebView {
    public class VRWebViewInputModule : BaseInputModule {
        public OVRControllerAction controllerAction;
        public List<VRGUIOverlay> guiOverlays = new List<VRGUIOverlay>();
        public float scrollDeltaParam;

        bool initialized = false;
        bool IsDrag = false;
        bool IsKeyDown = false;
        bool IsKeyUp = false;
        Vector2 previousPosition = Vector2.zero;
        bool hasPreviousPosition = false;

        /// <summary>
        /// Update
        /// </summary>
        public override void Process()
        {
            if (controllerAction == null) return;

            if (initialized == false)
            {
                initialized = true;
                controllerAction.KeyDownEvent += Controller_KeyDown;
                controllerAction.KeyUpEvent += Controller_KeyUp;
            }

            foreach (var guiOverlay in guiOverlays) {
                if (!guiOverlay.showCursor) continue;

                var webView = guiOverlay.gameObject.GetComponent<WWebView>();
                if (webView != null) {
                    var position = guiOverlay.cursorPosition;
                        var x1 = (int)(webView.GetActualWidth() * position.x);
                        var y1 = (int)(webView.GetActualHeight() * position.y);
                    if (IsKeyDown)
                    {
                        var x = (int)(webView.GetActualWidth() * position.x);
                        var y = (int)(webView.GetActualHeight() * position.y);
                        webView.InputEvent(1, 0, x, y);
                    }
                    if (IsKeyUp)
                    {
                        var x = (int)(webView.GetActualWidth() * position.x);
                        var y = (int)(webView.GetActualHeight() * position.y);
                        webView.InputEvent(2, 0, x, y);
                    }
                    if (IsDrag && hasPreviousPosition)
                    {
                        var delta = position.y - previousPosition.y;
                        var x = (int)(webView.GetActualWidth() * position.x);
                        var y = (int)(webView.GetActualHeight() * position.y);
                        webView.InputEvent(3, (int)(webView.GetActualHeight() * delta * scrollDeltaParam), x, y);
                    }
                    previousPosition = position;
                    hasPreviousPosition = true;
                }
            }

            IsKeyDown = false;
            IsKeyUp = false;
        }

        void Controller_KeyDown(object sender, OVRKeyEventArgs args)
        {
            if (args.ButtonId == Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)
            {
                IsDrag = true;
                IsKeyDown = true;
            }
        }

        void Controller_KeyUp(object sender, OVRKeyEventArgs args)
        {
            if (args.ButtonId == Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)
            {
                IsDrag = false;
                IsKeyUp = true;
            }
        }
    }
}
