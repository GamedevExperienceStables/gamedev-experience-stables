using UnityEditor;
using UnityEngine;


namespace Yapp
{
    public class BrushComponent
    {
        public enum BrushMode
        {
            None,
            ShiftKey,
            ShiftCtrlKey,
            ShiftPressed,
            ShiftCtrlPressed,
            ShiftDrag, // used eg for adding prefabs
            ShiftCtrlDrag // used eg for removing prefabs
        }

        private bool mousePressed = false;

        public bool DrawBrush(BrushSettings brushSettings, out BrushMode brushMode, out RaycastHit mouseHit)
        {
            brushMode = BrushMode.None;
            mouseHit = new RaycastHit();

            float radius = brushSettings.brushSize / 2f;

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            // TODO: raycast hit against layer
            //       see https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity))
            {
                mouseHit = hit;

                ///
                /// process mouse events
                ///

                // brush size & rotation: control key pressed
                if (Event.current.control)
                {
                    // mouse wheel up/down changes the radius
                    if (Event.current.type == EventType.ScrollWheel)
                    {
                        // ctrl + shift + scroll = brush rotation
                        if (Event.current.shift)
                        {
                            int rotationStepSize = 10;
                            int rotationMin = 0; // TODO: find out of to get that from Range
                            int rotationMax = 360; // TODO: find out of to get that from Range

                            // scroll up
                            if (Event.current.delta.y > 0)
                            {
                                brushSettings.brushRotation += rotationStepSize;
                                if (brushSettings.brushRotation > rotationMax)
                                {
                                    brushSettings.brushRotation = rotationMin + rotationStepSize;
                                }
                                Event.current.Use();
                            }
                            // scroll down
                            else if (Event.current.delta.y < 0)
                            {
                                brushSettings.brushRotation -= rotationStepSize;
                                if (brushSettings.brushRotation < rotationMin)
                                {
                                    brushSettings.brushRotation = rotationMax - rotationStepSize;
                                }
                                Event.current.Use();
                            }
                        }
                        // ctrl + scroll = brush size
                        else
                        {
                            // scroll up
                            if (Event.current.delta.y > 0)
                            {
                                brushSettings.brushSize++;
                                Event.current.Use();
                            }
                            // scroll down
                            else if (Event.current.delta.y < 0)
                            {
                                brushSettings.brushSize--;

                                // TODO: slider
                                if (brushSettings.brushSize < 1)
                                    brushSettings.brushSize = 1;

                                Event.current.Use();
                            }
                        }



                    }

                }

                // default: nothing pressed
                brushMode = BrushMode.None;

                // mouse pressed state: unity editor acts only on events, so we need to keep track of the click state
                if( Event.current.isMouse && Event.current.button == 0)
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        mousePressed = true;
                    }
                    else if(Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseLeaveWindow)
                    {
                        mousePressed = false;
                    }

                }

                // keyboard only case
                if (Event.current.shift)
                {
                    brushMode = BrushMode.ShiftKey;

                    if (Event.current.control)
                    {
                        brushMode = BrushMode.ShiftCtrlKey;
                    }
                }

                // check if mouse button is being pressed without dragging; 
                if (mousePressed)
                {
                    if (Event.current.shift)
                    {
                        brushMode = BrushMode.ShiftPressed;

                        if (Event.current.control)
                        {
                            brushMode = BrushMode.ShiftCtrlPressed;
                        }
                    }
                }

                // keyboard + mouse case
                if (Event.current.isMouse)
                {
                    // left button = 0; right = 1; middle = 2
                    if (Event.current.button == 0)
                    {
                        // drag case
                        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                        {
                            if (Event.current.shift)
                            {
                                brushMode = BrushMode.ShiftDrag;

                                if (Event.current.control)
                                {
                                    brushMode = BrushMode.ShiftCtrlDrag;
                                }
                            }

                        }
                    }
                }

                // draw brush gizmo
                DrawBrush( brushSettings, hit.point, hit.normal, radius, brushMode);

            }

            return brushMode != BrushMode.None;

        }

        public void Layout( string[] guiInfo)
        {
            int controlId = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(controlId);
            }


            // examples about how to show ui info
            // note: Handles.BeginGUI and EndGUI are important, otherwise the default gizmos aren't drawn
            Handles.BeginGUI();

            /* disabling this, never needed it so far
            if (raycastHitValid)
            {
                ShowHandleInfo(raycastHit.point);
            }
            */

            PrefabPainterEditor.ShowGuiInfo(guiInfo);

            Handles.EndGUI();

        }


        private void DrawBrush(BrushSettings brushSettings, Vector3 position, Vector3 normal, float radius, BrushMode brushMode)
        {
            // set default colors
            Color innerColor = GUIStyles.BrushNoneInnerColor;
            Color outerColor = GUIStyles.BrushNoneOuterColor;

            // set colors depending on brush mode
            switch (brushMode)
            {
                case BrushMode.None:
                    innerColor = GUIStyles.BrushNoneInnerColor;
                    outerColor = GUIStyles.BrushNoneOuterColor;
                    break;
                case BrushMode.ShiftKey:
                case BrushMode.ShiftPressed:
                case BrushMode.ShiftDrag:
                    innerColor = GUIStyles.BrushAddInnerColor;
                    outerColor = GUIStyles.BrushAddOuterColor;
                    break;
                case BrushMode.ShiftCtrlKey:
                case BrushMode.ShiftCtrlPressed:
                case BrushMode.ShiftCtrlDrag:
                    innerColor = GUIStyles.BrushRemoveInnerColor;
                    outerColor = GUIStyles.BrushRemoveOuterColor;
                    break;
            }



            // consider distribution
            switch (brushSettings.distribution)
            {
                case BrushSettings.Distribution.Center: // fallthrough
                case BrushSettings.Distribution.Poisson_Any: // fallthrough
                case BrushSettings.Distribution.Poisson_Terrain:
                    // inner disc
                    Handles.color = innerColor;
                    Handles.DrawSolidDisc(position, normal, radius);

                    // outer circle
                    Handles.color = outerColor;
                    Handles.DrawWireDisc(position, normal, radius);

                    // center line / normal
                    float lineLength = radius * 0.5f;
                    Vector3 lineStart = position;
                    Vector3 lineEnd = position + normal * lineLength;
                    Handles.DrawLine(lineStart, lineEnd);

                    break;

                case BrushSettings.Distribution.FallOff:

                    // use same curve for x and z
                    AnimationCurve fallOffCurve = brushSettings.fallOffCurve;
                    DrawCurveBrushSamplePoints( brushSettings, position, normal, innerColor, outerColor, fallOffCurve, fallOffCurve);

                    // alternate version: draw rings
                    // DrawCurveBrushSampleRings(position, normal, radius, innerColor, outerColor);

                    break;

                case BrushSettings.Distribution.FallOff2d:
                    AnimationCurve fallOff2dCurveX = brushSettings.fallOff2dCurveX;
                    AnimationCurve fallOff2dCurveZ = brushSettings.fallOff2dCurveZ;
                    //DrawCurveBrushSamplePoints( position, normal, innerColor, outerColor, fallOff2dCurveX, fallOff2dCurveZ);
                    DrawCurveBrushSamplePointsAsGrid(brushSettings, position, normal, innerColor, outerColor, fallOff2dCurveX, fallOff2dCurveZ);
                    break;
            }


        }



        /// <summary>
        /// Draw rings with alpha value set to the curve value
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <param name="radius"></param>
        /// <param name="innerColor"></param>
        /// <param name="outerColor"></param>
        private void DrawCurveBrushSampleRings(BrushSettings brushSettings, Vector3 position, Vector3 normal, float radius, Color innerColor, Color outerColor)
        {
            // number of sample points in 1 direction, i. e. there will be n * n sample points
            int samplePointsPerRow = brushSettings.curveSamplePoints;

            // the sample point distance on a [0,1] range, i. e. for 10 the distance will be 0.1
            float samplePointDistanceNormalized = 1f / samplePointsPerRow;

            AnimationCurve curve = brushSettings.fallOffCurve;
            for (var t = 0f; t <= 1f; t += samplePointDistanceNormalized)
            {
                float curvePoint = curve.Evaluate(t);

                // ensure value is [0,1]
                curvePoint = Mathf.Clamp01(curvePoint);

                Handles.color = new Color(innerColor.r, innerColor.g, innerColor.b, curvePoint);

                Handles.DrawWireDisc(position, normal, radius * t);

            }
        }

        // TODO: just a testing function with discs
        private void DrawCurveBrushSamplePoints(BrushSettings brushSettings, Vector3 position, Vector3 normal, Color innerColor, Color outerColor, AnimationCurve curveX, AnimationCurve curveZ)
        {
            // number of sample points in 1 direction, i. e. there will be n * n sample points
            int samplePointsPerRow = brushSettings.curveSamplePoints;

            // the sample point distance on a [0,1] range, i. e. for 10 the distance will be 0.1
            float samplePointDistanceNormalized = 1f / samplePointsPerRow;

            for (var x = 0f; x <= 1f; x += samplePointDistanceNormalized)
            {
                for (var z = 0f; z <= 1f; z += samplePointDistanceNormalized)
                {
                    float curvePointX = curveX.Evaluate(x);
                    float curvePointZ = curveZ.Evaluate(z);

                    // ensure value is [0,1]
                    curvePointX = Mathf.Clamp01(curvePointX);
                    curvePointZ = Mathf.Clamp01(curvePointZ);

                    float discSize = brushSettings.brushSize * x; // is same as y

                    Handles.color = new Color(innerColor.r, innerColor.g, innerColor.b, curvePointX * curvePointZ);

                    float radius = brushSettings.brushSize * samplePointDistanceNormalized * 0.5f;

                    // TODO: align depending on brush size
                    float xPosition = position.x - brushSettings.brushSize * (x - 0.5f) - radius;
                    float zPosition = position.z - brushSettings.brushSize * (z - 0.5f) - radius;

                    // high enough offset for y, in case the terrain below the brush aligned in it's normal direction isn't flat
                    // otherwise parts might be above terrain while others might be below it; another way would be to do an additional up raycast
                    float yRaystOffset = 3000f;
                    float yPosition = position.y + yRaystOffset;

                    // individual disc position, but with y offset
                    Vector3 discPosition = new Vector3(xPosition, yPosition, zPosition);

                    // y via raycast down
                    // TODO: raycast hit against layer
                    //       see https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
                    RaycastHit hit;
                    if (Physics.Raycast(discPosition, Vector3.down, out hit, Mathf.Infinity))
                    {
                        // set y position depending on the terrain
                        discPosition.y = hit.point.y;

                        // set the normal depending on the terrain
                        normal = hit.normal;

                    }

                    // y via height sampling
                    // discPosition.y = Terrain.activeTerrain.SampleHeight(discPosition);

                    Handles.DrawSolidDisc(discPosition, normal, radius);

                }

            }
        }

        // TODO: just a testing function with rectangles
        private void DrawCurveBrushSamplePointsAsGrid(BrushSettings brushSettings, Vector3 position, Vector3 normal, Color innerColor, Color outerColor, AnimationCurve curveX, AnimationCurve curveZ)
        {
            // number of sample points in 1 direction, i. e. there will be n * n sample points
            int samplePointsPerRow = brushSettings.curveSamplePoints;

            // the sample point distance on a [0,1] range, i. e. for 10 the distance will be 0.1
            float samplePointDistanceNormalized = 1f / samplePointsPerRow;

            Vector3[,] v = new Vector3[samplePointsPerRow, samplePointsPerRow];
            Color[,] c = new Color[samplePointsPerRow, samplePointsPerRow];

            int i;
            int j;
            for (i = 0; i < samplePointsPerRow; i++)
            {
                for (j = 0; j < samplePointsPerRow; j++)
                {

                    float x = i * samplePointDistanceNormalized;
                    float z = j * samplePointDistanceNormalized;

                    float curvePointX = curveX.Evaluate(x);
                    float curvePointZ = curveZ.Evaluate(z);

                    // ensure value is [0,1]
                    curvePointX = Mathf.Clamp01(curvePointX);
                    curvePointZ = Mathf.Clamp01(curvePointZ);

                    float discSize = brushSettings.brushSize * x; // is same as y

                    Handles.color = new Color(innerColor.r, innerColor.g, innerColor.b, curvePointX * curvePointZ);

                    float radius = brushSettings.brushSize * samplePointDistanceNormalized * 0.5f;

                    // TODO: align depending on brush size
                    float xPosition = position.x - brushSettings.brushSize * (x - 0.5f) - radius;
                    float zPosition = position.z - brushSettings.brushSize * (z - 0.5f) - radius;

                    // high enough offset for y, in case the terrain below the brush aligned in it's normal direction isn't flat
                    // otherwise parts might be above terrain while others might be below it; another way would be to do an additional up raycast
                    float yRaystOffset = 3000f;
                    float yPosition = position.y + yRaystOffset;

                    // individual disc position, but with y offset
                    Vector3 discPosition = new Vector3(xPosition, yPosition, zPosition);

                    // rotate around y world axis
                    float angle = brushSettings.brushRotation;
                    discPosition -= position; // move to origin
                    discPosition = Quaternion.Euler(0, angle, 0) * discPosition; // rotate around world axis
                    discPosition += position; // move back to position

                    // y via raycast down
                    // TODO: raycast hit against layer
                    //       see https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
                    RaycastHit hit;
                    if (Physics.Raycast(discPosition, Vector3.down, out hit, Mathf.Infinity))
                    {
                        // set y position depending on the terrain
                        discPosition.y = hit.point.y;

                        // set the normal depending on the terrain
                        normal = hit.normal;

                    }

                    // y via height sampling
                    // discPosition.y = Terrain.activeTerrain.SampleHeight(discPosition);

                    v[i, j] = discPosition;
                    c[i, j] = Handles.color;

                    // slope
                    float slopeAngle = Vector3.Angle(normal.normalized, new Vector3(0, 1, 0));
                    //Handles.Label(discPosition, new GUIContent("angle: " + slopeAngle));

                    // if brush area isn't inside the slope range, make the color almost transparent
                    if (slopeAngle < brushSettings.slopeMin || slopeAngle > brushSettings.slopeMax)
                    {
                        c[i, j].a = 0.05f;
                    }
                }
            }


            for (i = 0; i < v.GetLength(0) - 1; i++)
            {
                for (j = 0; j < v.GetLength(1) - 1; j++)
                {

                    Vector3[] verts = new Vector3[]
                    {
                                    v[i,j],
                                    v[i,j+1],
                                    v[i+1,j+1],
                                    v[i+1,j],
                    };

                    Handles.DrawSolidRectangleWithOutline(verts, c[i, j], new Color(0, 0, 0, c[i, j].a));
                }
            }
        }

        /* disabling this, never needed it so far
        private void ShowHandleInfo(Vector3 position)
        {
            if (debug)
            {
                // example about how to show info at the gizmo
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.blue;
                string text = "Mouse Postion: " + position;
                text += "\n";
                text += "Children: " + editor.getContainerChildren().Length;
                Handles.Label(position, text, style);
            }
        }
        */
    }
}
