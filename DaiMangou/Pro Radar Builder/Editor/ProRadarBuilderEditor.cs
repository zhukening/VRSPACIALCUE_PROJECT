using UnityEngine;
using System.Collections;
using UnityEditor;
using DaiMangou.ProRadarBuilder.Editor;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DaiMangou.ProRadarBuilder
{
    [System.Serializable]
    public class ProRadarBuilderEditor : EditorWindow
    {

        [MenuItem("Tools/DaiMangou/Radar/Pro Radar Builder")]
        private static void Init()
        {
            EditorWindow win = EditorWindow.GetWindow(typeof(ProRadarBuilderEditor));
            win.minSize = new Vector2(600, 250);

        }
        #region Variables
        private Sprite DefaultRadarSprite;
        private string[] Tabs = { "Design", "Blips", "Create" };
        [SerializeField]
        private _2DRadar Current_Selected2D_Radar;
        [SerializeField]
        private _3DRadar Current_Selected3D_Radar;
        [SerializeField]
        private int Tab_Selection = 0;
        private GameObject _Selection()
        {
            try { return Selection.activeGameObject; }
            catch { return null; }

        }
        private Vector2 RadarBlipScrollPosition;
        private GameObject RadarObject;
        private bool MakeNewRadar, DoRepaint, RepaintOnce;
        private string RadarName = "Radar";
        private string AddedInfo;
        private int AmountOfRadars = 1;
        private Texture2D BackgroundImage, LinkBroken, LinkMade;
        [NonSerialized]
        private int ID_Count;
        private Rect rect = new Rect();
        public RadarAction action;





        internal Sprite defaultRadarSpite
        {
            get
            {
                return Resources.Load("Default2DRadar", typeof(Sprite)) as Sprite;
            }


        }
        Sprite PixelSprite
        {
            get
            {
                return Sprite.Create(DrawPixelTexture.txture(Color.white), new Rect(), new Vector2(10, 10));

            }


        }
        void OnDisable()
        {
            GetResource.CleanUpTextures();

            DestroyImmediate(PixelSprite);

        }

        #endregion
        void OnEnable()
        {
            this.titleContent.image = GetResource.LoadDllResource("Resources", "RadarBuilder", "png", true, 16, 16);
            this.titleContent.text = "Radar Builder";
            BackgroundImage = GetResource.LoadDllResource("Resources", "RadarBuilderLogo", "png", false, 400, 200) as Texture2D;
            // LinkBroken = GetResource.LoadDllResource("GeneralImageResources", "LinkDark", "png", false, 10, 15);
            //  LinkMade = GetResource.LoadDllResource("GeneralImageResources", "LinkMade", "png", false, 10, 15);
            if (!DefaultRadarSprite)
                DefaultRadarSprite = Resources.Load("DefaultRadarSprite", typeof(Sprite)) as Sprite;


        }

        public void OnGUI()
        {
            #region Undo
            if (Current_Selected2D_Radar != null)
            {
                Undo.RecordObject(Current_Selected2D_Radar, "vala");
                EditorUtility.SetDirty(Current_Selected2D_Radar);
            }
            if(Current_Selected3D_Radar != null)
            {
                Undo.RecordObject(Current_Selected3D_Radar, "valb");
                EditorUtility.SetDirty(Current_Selected3D_Radar);
            }

            if (Event.current.type == EventType.ValidateCommand)
            {
                switch (Event.current.commandName)
                {
                    case "UndoRedoPerformed":

                        DoRepaint = true;
                        break;
                }
            }
            #endregion

            #region Create New Radar UI
            if (!RadarObject || MakeNewRadar)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height).ToUpperLeft(BackgroundImage.width, BackgroundImage.height), BackgroundImage);

                if (!RadarObject) EditorGUILayout.HelpBox("There is no Radar selected , you will need to add one or select an existing radar. Thanks a bunch", MessageType.Info);

                GUILayout.BeginArea(new Rect(position.width / 2 - 125, 100, 250, 140));
                GUILayout.Space(35);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name :");
                RadarName = GUILayout.TextArea(RadarName, 21);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Amount of Radars :" + AmountOfRadars.ToString());
                AmountOfRadars = (int)GUILayout.HorizontalSlider(AmountOfRadars, 1, 5, GUILayout.Width(90));
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("2D Radar", GUILayout.Width(115)))
                {
                    Create2DRadar();
                }
                GUILayout.Space(15);
                if (GUILayout.Button("3D Radar", GUILayout.Width(115)))
                {
                    Create3DRadar();
                }
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Info| Contact| Make Suggestion", GUILayout.Width(245)))
                {
                    Application.OpenURL("http://forum.unity3d.com/threads/released-pro-radar-builder-2d-3d-tracking-system-and-radar-creation-without-coding.382801/");
                }
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            if (Tab_Selection != 2)
            {
                if (MakeNewRadar) MakeNewRadar = false;

            }
            #endregion

            #region 2D UI
            if (RadarObject && RadarObject.GetComponent<_2DRadar>() != null)
            {
                Tab_Selection = GUILayout.SelectionGrid(Tab_Selection, Tabs, Tabs.Length);
                Current_Selected2D_Radar = RadarObject.GetComponent<_2DRadar>();
                switch (Tab_Selection)
                {
                    #region Radar Design and Settings
                    case 0:
                        Current_Selected2D_Radar._RadarDesign.Scrollposition = GUILayout.BeginScrollView(Current_Selected2D_Radar._RadarDesign.Scrollposition, false, false);
                        if (Current_Selected2D_Radar._RadarDesign.RadarTexture == null) { Current_Selected2D_Radar._RadarDesign.RadarTexture = defaultRadarSpite; }


                        #region Design Section 1

                        Current_Selected2D_Radar._RadarDesign.DesignSection1 = EditorGUILayout.Foldout(Current_Selected2D_Radar._RadarDesign.DesignSection1, "UI Design");

                        if (Current_Selected2D_Radar._RadarDesign.DesignSection1)
                        {
                            GUILayout.BeginHorizontal();

                            try
                            {
                                GUILayout.Box(Current_Selected2D_Radar._RadarDesign.RadarTexture.texture, GUILayout.Height((float)Current_Selected2D_Radar._RadarDesign.RadarDiameter), GUILayout.Width((float)Current_Selected2D_Radar._RadarDesign.RadarDiameter));
                            }
                            catch { }

                            if (CurrentEvent.evt().type == EventType.repaint || CurrentEvent.evt().type == EventType.layout) { rect = GUILayoutUtility.GetLastRect(); }

                            try
                            {
                                GUI.DrawTexture(rect, Current_Selected2D_Radar._RadarDesign.OptionalTexture.texture);
                            }
                            catch { }
                            GUILayout.BeginVertical();
                            GUILayout.Label("Primary Radar Image");
                            try
                            {
                                Current_Selected2D_Radar._RadarDesign.RadarTexture = (Sprite)EditorGUILayout.ObjectField(Current_Selected2D_Radar._RadarDesign.RadarTexture, typeof(Sprite), false);
                            }
                            catch { }
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("secondary Radar Image");
                            Current_Selected2D_Radar._RadarDesign.CanRotate = GUILayout.Toggle(Current_Selected2D_Radar._RadarDesign.CanRotate, (Current_Selected2D_Radar._RadarDesign.CanRotate) ? "Will rotate" : "Will not rotate");
                            GUILayout.EndHorizontal();
                            try { Current_Selected2D_Radar._RadarDesign.OptionalTexture = (Sprite)EditorGUILayout.ObjectField(Current_Selected2D_Radar._RadarDesign.OptionalTexture, typeof(Sprite), false); }
                            catch { }

                            Current_Selected2D_Radar._RadarDesign.RadarZoom = EditorGUILayout.Slider("Radar Zoom", Current_Selected2D_Radar._RadarDesign.RadarZoom, 0, 5);

                            Current_Selected2D_Radar._RadarDesign.RadarDiameter = EditorGUILayout.Slider("Radar Size", Current_Selected2D_Radar._RadarDesign.RadarDiameter, 1, 250, GUILayout.MaxWidth(200));

                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();
                        }

                        #endregion

                        EditorGUILayout.Separator();

                        #region Design Section 2
                        Current_Selected2D_Radar._RadarDesign.DesignSection2 = EditorGUILayout.Foldout(Current_Selected2D_Radar._RadarDesign.DesignSection2, "Radar Behavior");
                        if (Current_Selected2D_Radar._RadarDesign.DesignSection2)
                        {
                            GUILayout.BeginVertical();



                            Current_Selected2D_Radar._RadarDesign.action = (RadarAction)EditorGUILayout.EnumPopup("Radar Animation", Current_Selected2D_Radar._RadarDesign.action);

                            Current_Selected2D_Radar._RadarDesign.radarPositioning = (RadarPositioning)EditorGUILayout.EnumPopup("Radar Positioning", Current_Selected2D_Radar._RadarDesign.radarPositioning);

                            switch (Current_Selected2D_Radar._RadarDesign.radarPositioning)
                            {
                                case RadarPositioning.Manual:
                                    Current_Selected2D_Radar._RadarDesign.RadarRect.position = EditorGUILayout.Vector2Field(" ", Current_Selected2D_Radar._RadarDesign.RadarRect.position);

                                    break;
                                case RadarPositioning.Snap:
                                    Current_Selected2D_Radar._RadarDesign.snapPosition = (SnapPosition)EditorGUILayout.EnumPopup(Current_Selected2D_Radar._RadarDesign.snapPosition);
                                    break;

                            }


                            Current_Selected2D_Radar._RadarDesign.radarFront = (FrontIs)EditorGUILayout.EnumPopup("Front is " + AddedInfo, Current_Selected2D_Radar._RadarDesign.radarFront);

                            switch (Current_Selected2D_Radar._RadarDesign.radarFront)
                            {
                                case FrontIs.North: Current_Selected2D_Radar._RadarDesign.trackingVal = 0; AddedInfo = ""; break;
                                case FrontIs.East: Current_Selected2D_Radar._RadarDesign.trackingVal = 90; AddedInfo = "(Best For 2D)"; break;
                                case FrontIs.South: Current_Selected2D_Radar._RadarDesign.trackingVal = 180; AddedInfo = ""; break;
                                case FrontIs.West: Current_Selected2D_Radar._RadarDesign.trackingVal = -90; AddedInfo = "(Best For 2D)"; break;

                            }

                            Current_Selected2D_Radar._RadarDesign.UseScreenRes = GUILayout.Toggle(Current_Selected2D_Radar._RadarDesign.UseScreenRes, "Use Screen Resolution");

                            if (Current_Selected2D_Radar._RadarDesign.UseScreenRes)
                            {
                                GUILayout.BeginHorizontal();
                                Current_Selected2D_Radar._RadarDesign.ScaleFactor = EditorGUILayout.Slider("ScaleFactor", Current_Selected2D_Radar._RadarDesign.ScaleFactor, 3, 15);
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndVertical();


                        }
                        #endregion

                        EditorGUILayout.Separator();


                        try { if (GUILayout.Button((Current_Selected2D_Radar._RadarDesign.ShowRadar ? "Radar is " + "Active" : "Radar  is " + "Inactive"), GUILayout.Width(150))) Current_Selected2D_Radar._RadarDesign.ShowRadar = !Current_Selected2D_Radar._RadarDesign.ShowRadar; }
                        catch { }

                        GUILayout.EndScrollView();
                        EditorGUILayout.Separator();
                        break;
                    #endregion

                    #region Blip Design UI
                    case 1:


                        GUILayout.BeginHorizontal();
                        Current_Selected2D_Radar._RadarDesign.Count = Mathf.Clamp(EditorGUILayout.IntField("Number of Blips types", Current_Selected2D_Radar._RadarDesign.Count, GUILayout.MaxWidth(200)), 0, 8000);

                        if (GUILayout.Button("Apply", GUILayout.MaxWidth(50)) || CurrentEvent.evt().keyCode == KeyCode.Return)
                        {

                            Selection.activeGameObject = Current_Selected2D_Radar.gameObject;
                            ListExt.Resize(Current_Selected2D_Radar._RadarBlips, Current_Selected2D_Radar._RadarDesign.Count);
                            Repaint();
                        }

                        GUILayout.EndHorizontal();



                        RadarBlipScrollPosition = EditorGUILayout.BeginScrollView(RadarBlipScrollPosition, false, false);


                        Current_Selected2D_Radar._RadarCenterObject2D.State = (Current_Selected2D_Radar._RadarCenterObject2D.IsActive ? Current_Selected2D_Radar._RadarCenterObject2D.PlayerTag + " is " + " Active" : Current_Selected2D_Radar._RadarCenterObject2D.PlayerTag + " is " + " Inactive");
                        Current_Selected2D_Radar._RadarCenterObject2D.IsShowing = EditorGUILayout.Foldout(Current_Selected2D_Radar._RadarCenterObject2D.IsShowing, Current_Selected2D_Radar._RadarCenterObject2D.State);

                        if (Current_Selected2D_Radar._RadarCenterObject2D.IsShowing)
                        {
                            GUILayout.BeginHorizontal();

                            try
                            {
                                GUILayout.Box(Current_Selected2D_Radar._RadarCenterObject2D.RadarCenterSprite.texture, GUILayout.Height(50), GUILayout.Width(50));
                            }
                            catch { }

                            GUILayout.BeginVertical();
                            Current_Selected2D_Radar._RadarCenterObject2D.RadarCenterSprite = (Sprite)EditorGUILayout.ObjectField(Current_Selected2D_Radar._RadarCenterObject2D.RadarCenterSprite, typeof(Sprite), false);


                            Current_Selected2D_Radar._RadarCenterObject2D.PlayerTag = EditorGUILayout.TagField("Tag ", Current_Selected2D_Radar._RadarCenterObject2D.PlayerTag);

                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                            if (GUILayout.Button(Current_Selected2D_Radar._RadarCenterObject2D.State)) Current_Selected2D_Radar._RadarCenterObject2D.IsActive = !Current_Selected2D_Radar._RadarCenterObject2D.IsActive;

                            Current_Selected2D_Radar._RadarCenterObject2D.playerBlipSize = (int)EditorGUILayout.Slider(Current_Selected2D_Radar._RadarCenterObject2D.PlayerTag + " Blip Size", Current_Selected2D_Radar._RadarCenterObject2D.playerBlipSize, 0, 30);
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();
                        }
                        EditorGUILayout.Separator();

                        foreach (var _Blip in Current_Selected2D_Radar._RadarBlips.Select((x, i) => new { Value = x, Index = i }))
                        {

                            try
                            {
                                _Blip.Value.State = _Blip.Value.IsActive ? _Blip.Value.Tag + " is " + "Active" : _Blip.Value.Tag + " is " + "Inactive";
                                _Blip.Value.IsShowing = EditorGUILayout.Foldout(_Blip.Value.IsShowing, _Blip.Value.State);

                                if (_Blip.Value.IsShowing)
                                {
                                    GUILayout.BeginHorizontal();

                                    try
                                    {
                                        GUILayout.Box(_Blip.Value.icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                                    }
                                    catch { }

                                    GUILayout.BeginVertical();
                                    _Blip.Value.icon = (Sprite)EditorGUILayout.ObjectField(_Blip.Value.icon, typeof(Sprite), false);


                                    _Blip.Value.Tag = EditorGUILayout.TagField("Tag ", _Blip.Value.Tag);

                                    GUILayout.EndVertical();

                                    GUILayout.BeginVertical();
                                    GUILayout.BeginHorizontal();
                                    if (GUILayout.Button(_Blip.Value.State)) _Blip.Value.IsActive = !_Blip.Value.IsActive;
                                    string state = (Current_Selected2D_Radar._RadarBlips[_Blip.Index].IsAlwaysShow) ? "" : "not";
                                    Current_Selected2D_Radar._RadarBlips[_Blip.Index].IsAlwaysShow = GUILayout.Toggle(Current_Selected2D_Radar._RadarBlips[_Blip.Index].IsAlwaysShow, "and will " + state + " Always show");
                                    GUILayout.EndHorizontal();

                                    _Blip.Value.BlipSize = (int)EditorGUILayout.Slider(_Blip.Value.Tag + " Blip Size", _Blip.Value.BlipSize, 0, 30);
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                }
                            }
                            catch { }
                            EditorGUILayout.Separator();
                        }
                        EditorGUILayout.EndScrollView();

                        break;
                    #endregion

                    #region Make New Radar
                    case 2:
                        if (!MakeNewRadar)
                            MakeNewRadar = true;

                        break;
                    #endregion
                }

            }
            #endregion

            #region 3D UI


            if (RadarObject && RadarObject.GetComponent<_3DRadar>() != null)
            {


                Tab_Selection = GUILayout.SelectionGrid(Tab_Selection, Tabs, 3);
                Current_Selected3D_Radar = RadarObject.GetComponent<_3DRadar>();
                switch (Tab_Selection)
                {
                    #region Radar Design and Settings
                    case 0:



                        Rect SidebarRect = new Rect(0, 40, PublicVariables.ScreenRect.width, PublicVariables.ScreenRect.height);
                        GUILayout.BeginArea(SidebarRect);

                        #region Design Section 1
                        try
                        {
                            Current_Selected3D_Radar.RadarDesign.DesignSection1 = EditorGUILayout.Foldout(Current_Selected3D_Radar.RadarDesign.DesignSection1, "Design Section A");
                            if (Current_Selected3D_Radar.RadarDesign.DesignSection1)
                            {

                                GUILayout.BeginHorizontal();
                                Current_Selected3D_Radar.RadarDesign.RadarDiameter = EditorGUILayout.FloatField("Radar Diameter", Current_Selected3D_Radar.RadarDesign.RadarDiameter);

                                Current_Selected3D_Radar.RadarDesign.UseLocalScale = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign.UseLocalScale, "Use Local Scale");
                                GUILayout.EndHorizontal();

                                Current_Selected3D_Radar.RadarDesign.SceneScale = EditorGUILayout.FloatField("Scene Scale", Current_Selected3D_Radar.RadarDesign.SceneScale);

                                Current_Selected3D_Radar.RadarDesign.InnerRangeScale = EditorGUILayout.FloatField("Inner Culling Zone", Current_Selected3D_Radar.RadarDesign.InnerRangeScale);

                                Current_Selected3D_Radar.RadarDesign.RangeScale = EditorGUILayout.FloatField("Tracking Bounds", Current_Selected3D_Radar.RadarDesign.RangeScale);

                                Current_Selected3D_Radar.RadarDesign.Visualize = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign.Visualize, "Visualize");

                            }
                        }
                        catch { }
                        #endregion

                        #region Design Section 2
                        /*Current_Selected3D_Radar.RadarDesign.DesignSection2 = EditorGUILayout.Foldout(Current_Selected3D_Radar.RadarDesign.DesignSection2, "Design Section 2");
                        if (Current_Selected3D_Radar.RadarDesign.DesignSection1)
                        {

                        }
                        */
                        #endregion
                        GUILayout.EndArea();




                        break;
                    #endregion

                    #region Blip Design UI
                    case 1:


                        RadarBlipScrollPosition = EditorGUILayout.BeginScrollView(RadarBlipScrollPosition, false, false);

                        #region Setting and creating Blips
                        GUILayout.BeginHorizontal();

                        Current_Selected3D_Radar.RadarDesign.Count = Mathf.Clamp(EditorGUILayout.IntField("Number of Blips types", Current_Selected3D_Radar.RadarDesign.Count, GUILayout.MaxWidth(200)), 0, 8000);

                        if (GUILayout.Button("Apply", GUILayout.MaxWidth(50)) || CurrentEvent.evt().keyCode == KeyCode.Return)
                        {
                            if (Current_Selected3D_Radar.Blips.Count < Current_Selected3D_Radar.RadarDesign.Count)
                                Current_Selected3D_Radar.Blips.AddRange(Enumerable.Repeat(default(RadarBlips3D), Current_Selected3D_Radar.RadarDesign.Count));
                            else
                                Current_Selected3D_Radar.Blips.RemoveRange(Current_Selected3D_Radar.RadarDesign.Count, Current_Selected3D_Radar.Blips.Count - Current_Selected3D_Radar.RadarDesign.Count);


                            Repaint();
                        }
                        GUILayout.EndHorizontal();
                        #endregion

                        #region Setting PlayerBlip Info

                        Current_Selected3D_Radar._RadarCenterObject3D.State = Current_Selected3D_Radar._RadarCenterObject3D.IsActive ? Current_Selected3D_Radar._RadarCenterObject3D.Tag + " is " + " Active" : Current_Selected3D_Radar._RadarCenterObject3D.Tag + " is " + " Inactive";

                        Current_Selected3D_Radar._RadarCenterObject3D.IsShowing = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.IsShowing, Current_Selected3D_Radar._RadarCenterObject3D.State);

                        if (Current_Selected3D_Radar._RadarCenterObject3D.IsShowing)
                        {
                            #region sprite
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();

                            Current_Selected3D_Radar._RadarCenterObject3D.IsSprite = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.IsSprite, Current_Selected3D_Radar._RadarCenterObject3D.Tag + " Sprite");
                            if (Current_Selected3D_Radar._RadarCenterObject3D.IsSprite)
                            {
                                #region SpriteBlipDesign
                                GUILayout.BeginHorizontal();

                                try
                                {
                                    GUILayout.Box(Current_Selected3D_Radar._RadarCenterObject3D.icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                                }
                                catch { }


                                GUILayout.BeginVertical();
                                Current_Selected3D_Radar._RadarCenterObject3D.icon = (Sprite)EditorGUILayout.ObjectField(Current_Selected3D_Radar._RadarCenterObject3D.icon, typeof(Sprite), false);

                                Current_Selected3D_Radar._RadarCenterObject3D.SpriteMaterial = (Material)EditorGUILayout.ObjectField("", Current_Selected3D_Radar._RadarCenterObject3D.SpriteMaterial, typeof(Material), true);

                                Current_Selected3D_Radar._RadarCenterObject3D.colour = EditorGUILayout.ColorField("Colour", Current_Selected3D_Radar._RadarCenterObject3D.colour);



                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            #endregion

                            #region Mesh
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();

                            Current_Selected3D_Radar._RadarCenterObject3D.IsMesh = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.IsMesh, Current_Selected3D_Radar._RadarCenterObject3D.Tag + " Mesh");
                            if (Current_Selected3D_Radar._RadarCenterObject3D.IsMesh)
                            {
                                #region MeshBlipDesign
                                GUILayout.BeginHorizontal();

                                #region 1
                                Current_Selected3D_Radar._RadarCenterObject3D.mesh = (Mesh)EditorGUILayout.ObjectField(Current_Selected3D_Radar._RadarCenterObject3D.mesh, typeof(Mesh), false);


                                Current_Selected3D_Radar._RadarCenterObject3D.MatCount = Mathf.Clamp(EditorGUILayout.IntField(" Material Count", Current_Selected3D_Radar._RadarCenterObject3D.MatCount, GUILayout.MaxWidth(200)), 0, 8000);

                                if (CurrentEvent.evt().keyCode == KeyCode.Return)
                                {
                                    Array.Resize(ref Current_Selected3D_Radar._RadarCenterObject3D.MeshMaterials, Current_Selected3D_Radar._RadarCenterObject3D.MatCount);
                                    Repaint();
                                }
                                GUILayout.BeginVertical();
                                for (int i = 0; i < Current_Selected3D_Radar._RadarCenterObject3D.MeshMaterials.Count(); i++)
                                {

                                    try
                                    {
                                        Current_Selected3D_Radar._RadarCenterObject3D.MeshMaterials[i] = (Material)EditorGUILayout.ObjectField(Current_Selected3D_Radar._RadarCenterObject3D.MeshMaterials[i], typeof(Material), false);
                                    }
                                    catch { }

                                }
                                GUILayout.EndVertical();

                                #endregion
                                GUILayout.EndHorizontal();
                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            #endregion

                            #region Prefab

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();
                            Current_Selected3D_Radar._RadarCenterObject3D.IsPrefab = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.IsPrefab, Current_Selected3D_Radar._RadarCenterObject3D.Tag + " Prefab");
                            if (Current_Selected3D_Radar._RadarCenterObject3D.IsPrefab)
                            {
                                #region PrefabBLipDesign
                                #region 1
                                Current_Selected3D_Radar._RadarCenterObject3D.prefab = (Transform)EditorGUILayout.ObjectField("Prefab", Current_Selected3D_Radar._RadarCenterObject3D.prefab, typeof(Transform), true);
                                #endregion
                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();
                            #endregion

                            #region Universal Settings

                            GUILayout.BeginHorizontal();

                            Current_Selected3D_Radar._RadarCenterObject3D.IsTrackRotation = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.IsTrackRotation, "Track Rotation");

                            Current_Selected3D_Radar._RadarCenterObject3D.BlipSize = EditorGUILayout.FloatField(Current_Selected3D_Radar._RadarCenterObject3D.Tag + " Size :", Current_Selected3D_Radar._RadarCenterObject3D.BlipSize);

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Freeze");
                            Current_Selected3D_Radar._RadarCenterObject3D.lockX = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.lockX, "X");
                            Current_Selected3D_Radar._RadarCenterObject3D.lockY = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.lockY, "Y");
                            Current_Selected3D_Radar._RadarCenterObject3D.lockZ = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.lockZ, "Z");
                            GUILayout.EndHorizontal();

                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button(Current_Selected3D_Radar._RadarCenterObject3D.State, GUILayout.MaxWidth(200))) Current_Selected3D_Radar._RadarCenterObject3D.IsActive = !Current_Selected3D_Radar._RadarCenterObject3D.IsActive;

                            Current_Selected3D_Radar._RadarCenterObject3D._CreateBlipAs = (CreateBlipAs)EditorGUILayout.EnumPopup(Current_Selected3D_Radar._RadarCenterObject3D._CreateBlipAs, GUILayout.Width(100));

                            GUILayout.BeginHorizontal(); GUILayout.Label("Tag"); Current_Selected3D_Radar._RadarCenterObject3D.Tag = EditorGUILayout.TagField(Current_Selected3D_Radar._RadarCenterObject3D.Tag); GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal(); GUILayout.Label("Layer"); Current_Selected3D_Radar._RadarCenterObject3D.Layer = EditorGUILayout.LayerField(Current_Selected3D_Radar._RadarCenterObject3D.Layer); GUILayout.EndHorizontal();

                            GUILayout.EndHorizontal();
                            #endregion


                        }
                        #endregion

                        #region Setting and creating All other Blips
                        EditorGUILayout.Separator();

                        foreach (var _Blip in Current_Selected3D_Radar.Blips.Select((x, i) => new { Value = x as RadarBlips3D, Index = i }))
                        {
                            try
                            {
                                _Blip.Value.State = _Blip.Value.IsActive ? _Blip.Value.Tag + " is " + "Active" : _Blip.Value.Tag + " is " + "Inactive";

                                _Blip.Value.IsShowing = EditorGUILayout.Foldout(_Blip.Value.IsShowing, _Blip.Value.State);



                                if (_Blip.Value.IsShowing)
                                {
                                    #region Sprite
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();
                                    _Blip.Value.IsSprite = EditorGUILayout.Foldout(_Blip.Value.IsSprite, _Blip.Value.Tag + " Sprite");
                                    if (_Blip.Value.IsSprite)
                                    {



                                        GUILayout.BeginHorizontal();

                                        #region sprite Design

                                        try
                                        {
                                            GUILayout.Box(_Blip.Value.icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                                        }
                                        catch { }
                                        GUILayout.BeginVertical();
                                        _Blip.Value.icon = (Sprite)EditorGUILayout.ObjectField(_Blip.Value.icon, typeof(Sprite), false);


                                        Current_Selected3D_Radar.Blips[_Blip.Index].SpriteMaterial = (Material)EditorGUILayout.ObjectField(Current_Selected3D_Radar.Blips[_Blip.Index].SpriteMaterial, typeof(Material), true);


                                        Current_Selected3D_Radar.Blips[_Blip.Index].colour = EditorGUILayout.ColorField("Colour", Current_Selected3D_Radar.Blips[_Blip.Index].colour);

                                        GUILayout.EndVertical();
                                        #endregion

                                        GUILayout.EndHorizontal();


                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                    #endregion

                                    #region Mesh
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();

                                    _Blip.Value.IsMesh = EditorGUILayout.Foldout(_Blip.Value.IsMesh, _Blip.Value.Tag + " Mesh");
                                    if (_Blip.Value.IsMesh)
                                    {
                                        #region MeshBlipDesign
                                        GUILayout.BeginHorizontal();

                                        #region 1
                                        GUILayout.BeginVertical();
                                        _Blip.Value.mesh = (Mesh)EditorGUILayout.ObjectField(_Blip.Value.mesh, typeof(Mesh), false);
                                        #region LOD area

                                        string statement = (_Blip.Value.UseLOD) ? " is on" : " is off";
                                        _Blip.Value.UseLOD = EditorGUILayout.Foldout(_Blip.Value.UseLOD, _Blip.Value.Tag + " LOD" + statement);
                                        if (_Blip.Value.UseLOD)
                                        {

                                            GUILayout.BeginVertical();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("LOW");
                                            _Blip.Value.Low = (Mesh)EditorGUILayout.ObjectField(_Blip.Value.Low, typeof(Mesh), false);
                                            _Blip.Value.LowDistance = EditorGUILayout.FloatField(_Blip.Value.LowDistance);
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("MEDIUM");
                                            _Blip.Value.Medium = (Mesh)EditorGUILayout.ObjectField(_Blip.Value.Medium, typeof(Mesh), false);
                                            _Blip.Value.MediumDistance = EditorGUILayout.FloatField(_Blip.Value.MediumDistance);
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("HIGH");
                                            _Blip.Value.High = (Mesh)EditorGUILayout.ObjectField(_Blip.Value.High, typeof(Mesh), false);
                                            _Blip.Value.HighDistance = EditorGUILayout.FloatField(_Blip.Value.HighDistance);
                                            GUILayout.EndHorizontal();

                                            GUILayout.EndVertical();


                                        }

                                        #endregion
                                        GUILayout.EndVertical();

                                        #region Define how many materials the mesh uses
                                        _Blip.Value.MatCount = Mathf.Clamp(EditorGUILayout.IntField(" Material Count", _Blip.Value.MatCount, GUILayout.MaxWidth(200)), 0, 8000);
                                        #endregion

                                        if (CurrentEvent.evt().keyCode == KeyCode.Return)
                                        {
                                            Array.Resize(ref _Blip.Value.MeshMaterials, _Blip.Value.MatCount);
                                            Repaint();
                                        }
                                        GUILayout.BeginVertical();
                                        for (int i = 0; i < _Blip.Value.MeshMaterials.Count(); i++)
                                        {

                                            try
                                            {
                                                _Blip.Value.MeshMaterials[i] = (Material)EditorGUILayout.ObjectField(_Blip.Value.MeshMaterials[i], typeof(Material), false);
                                            }
                                            catch { }

                                        }
                                        GUILayout.EndVertical();

                                        #endregion
                                        GUILayout.EndHorizontal();
                                        #endregion


                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    #endregion

                                    #region prefab
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();
                                    _Blip.Value.IsPrefab = EditorGUILayout.Foldout(_Blip.Value.IsPrefab, _Blip.Value.Tag + " Prefab");
                                    if (_Blip.Value.IsPrefab)
                                    {
                                        #region PrefabBLipDesign

                                        _Blip.Value.prefab = (Transform)EditorGUILayout.ObjectField("Prefab", _Blip.Value.prefab, typeof(Transform), true);

                                        #endregion
                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                    #endregion

                                    #region Tracking line

                                    #region Tracking Line Design
                                    /* GUILayout.BeginVertical();
                                try
                                {
                                    GUILayout.Box(_Blip.Value.TrackingLine.texture, GUILayout.Height(50), GUILayout.Width(50));

                                }
                                catch { }

                               _Blip.Value.TrackingLine = (Sprite)EditorGUILayout.ObjectField(_Blip.Value.TrackingLine, typeof(Sprite), false); 

                                Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineMaterial = (Material)EditorGUILayout.ObjectField("", Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineMaterial, typeof(Material), true);


                                Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineColour = EditorGUILayout.ColorField("Colour",Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineColour);
                              

                               Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineDimention = EditorGUILayout.FloatField("Size",Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineDimention);
                               

                                GUILayout.EndVertical(); */
                                    #endregion

                                    #endregion

                                    #region Universal Settings

                                    GUILayout.BeginHorizontal();


                                    Current_Selected3D_Radar.Blips[_Blip.Index].IsTrackRotation = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].IsTrackRotation, "Track Rotation");


                                    Current_Selected3D_Radar.Blips[_Blip.Index].BlipSize = EditorGUILayout.FloatField(_Blip.Value.Tag + " Size", Current_Selected3D_Radar.Blips[_Blip.Index].BlipSize);

                                    //Current_Selected3D_Radar.Blips[_Blip.Index].BlipCanScleBasedOnDistance = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].BlipCanScleBasedOnDistance, "Scale by distance");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Freeze");
                                    Current_Selected3D_Radar.Blips[_Blip.Index].lockX = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].lockX, "X");
                                    Current_Selected3D_Radar.Blips[_Blip.Index].lockY = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].lockY, "Y");
                                    Current_Selected3D_Radar.Blips[_Blip.Index].lockZ = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].lockZ, "Z");
                                    GUILayout.EndHorizontal();

                                    GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal();
                                    if (GUILayout.Button((_Blip.Value.State), GUILayout.MaxWidth(200))) _Blip.Value.IsActive = !_Blip.Value.IsActive;

                                    _Blip.Value._CreateBlipAs = (CreateBlipAs)EditorGUILayout.EnumPopup(_Blip.Value._CreateBlipAs, GUILayout.Width(100));

                                    GUILayout.BeginHorizontal(); GUILayout.Label("Tag"); _Blip.Value.Tag = EditorGUILayout.TagField(_Blip.Value.Tag); GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal(); GUILayout.Label("Layer"); _Blip.Value.Layer = EditorGUILayout.LayerField(_Blip.Value.Layer); GUILayout.EndHorizontal();

                                    GUILayout.EndHorizontal();


                                    #endregion
                                }

                                EditorGUILayout.Separator();
                            }
                            catch { }
                        }
                        #endregion

                        EditorGUILayout.EndScrollView();


                        break;
                    #endregion

                    #region Make New Radar
                    case 2:
                        if (!MakeNewRadar)
                            MakeNewRadar = true;

                        break;
                    #endregion
                }
            }

            #endregion

        }



        void OnInspectorUpdate()
        {

            if (GUI.changed) DoRepaint = true;

            if (_Selection())
            {
                if (RepaintOnce) RepaintOnce = false;

                if (_Selection().GetComponent<_2DRadar>() != null)
                {
                    if (RadarObject != _Selection())
                    {
                        RadarObject = _Selection();
                        DoRepaint = true;
                    }

                }
                if (_Selection().GetComponent<_3DRadar>() != null)
                {
                    if (RadarObject != _Selection())
                    {
                        RadarObject = _Selection();
                        DoRepaint = true;
                    }

                }
            }

            if (!_Selection() && !RepaintOnce)
            {
                DoRepaint = true;
                RepaintOnce = true;
            }
            if (DoRepaint) Repaint(); DoRepaint = false;
        }

        void Create2DRadar()
        {
            ID_Count += 1;
            for (int i = 0; i < AmountOfRadars; i++)
            {
                GameObject RadarInstance = new GameObject(RadarName);
                RadarInstance.AddComponent<_2DRadar>();
                Selection.activeGameObject = RadarInstance;

            }
            MakeNewRadar = false;
            Tab_Selection = 0;

        }
        void Create3DRadar()
        {
            for (int i = 0; i < AmountOfRadars; i++)
            {
                GameObject RadarInstance = new GameObject(RadarName);
                RadarInstance.AddComponent<_3DRadar>();
                Selection.activeGameObject = RadarInstance;
                GameObject DesignsParent = new GameObject("Designs");
                DesignsParent.transform.parent = RadarInstance.transform;
                GameObject DefaultDesign = new GameObject("DefaultRadarSprite");
                DefaultDesign.transform.parent = DesignsParent.transform;
                DefaultDesign.transform.Rotate(new Vector2(90, 0), Space.World);
                DefaultDesign.AddComponent<SpriteRenderer>();
                if (DefaultRadarSprite)
                    DefaultDesign.GetComponent<SpriteRenderer>().sprite = DefaultRadarSprite;

            }

            MakeNewRadar = false;
            Tab_Selection = 0;
        }


    }
}
