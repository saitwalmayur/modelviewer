using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// This class is used to add shadow on the UI
    /// </summary>
    [AddComponentMenu("UI/Effects/DropShadow", 14)]
    public class DropShadow : BaseMeshEffect
    {
        [SerializeField]
        private Color shadowColor = new Color(0f, 0f, 0f, 0.5f);

        [SerializeField]
        private Vector2 shadowDistance = new Vector2(1f, -1f);

        [SerializeField]
        private bool m_UseGraphicAlpha = true;
        public int iterations = 5;
        public Vector2 shadowSpread = Vector2.one;

        protected DropShadow()
        { }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            EffectDistance = shadowDistance;
            base.OnValidate();
        }

#endif
        /// <summary>
        /// This method is used to set shadow color
        /// </summary>
        public Color effectColor
        {
            get { return shadowColor; }
            set
            {
                shadowColor = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        /// <summary>
        /// This method is used to set shadow spread 
        /// </summary>
        public Vector2 ShadowSpread
        {
            get { return shadowSpread; }
            set
            {
                shadowSpread = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        /// <summary>
        /// This method provides a way to get and set the value of iterations while also triggering on a graphic
        /// </summary>
        public int Iterations
        {
            get { return iterations; }
            set
            {
                iterations = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        /// <summary>
        /// This method is used to set effect distance for shadow 
        /// </summary>
        public Vector2 EffectDistance
        {
            get { return shadowDistance; }
            set
            {
                shadowDistance = value;

                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        /// <summary>
        /// The method is used to set the graphic alpha 
        /// </summary>
        public bool useGraphicAlpha
        {
            get { return m_UseGraphicAlpha; }
            set
            {
                m_UseGraphicAlpha = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        /// <summary>
        ///  This method applies a drop shadow effect to the vertices by modifying their positions and colors based on the provided parameters
        /// </summary>
        /// <param name="verts"></param>
        void DropShadowEffect(List<UIVertex> verts)
        {
            UIVertex vt;
            int count = verts.Count;

            List<UIVertex> vertsCopy = new List<UIVertex>(verts);
            verts.Clear();

            for (int i = 0; i < iterations; i++)
            {
                for (int v = 0; v < count; v++)
                {
                    vt = vertsCopy[v];
                    Vector3 position = vt.position;
                    float fac = (float)i / (float)iterations;
                    position.x *= (1 + shadowSpread.x * fac * 0.01f);
                    position.y *= (1 + shadowSpread.y * fac * 0.01f);
                    position.x += shadowDistance.x * fac;
                    position.y += shadowDistance.y * fac;
                    vt.position = position;
                    Color32 color = shadowColor;
                    color.a = (byte)((float)color.a / (float)iterations);
                    vt.color = color;
                    verts.Add(vt);
                }
            }

            for (int i = 0; i < vertsCopy.Count; i++)
            {
                verts.Add(vertsCopy[i]);
            }
        }

        /// <summary>
        /// This method retrieves the current vertex data of a UI element, applies the DropShadowEffect to modify the vertices, and updates the VertexHelper object with the modified vertex data
        /// </summary>
        /// <param name="vh"></param>
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            List<UIVertex> output = new List<UIVertex>();
            vh.GetUIVertexStream(output);

            DropShadowEffect(output);

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);
        }
    }
}