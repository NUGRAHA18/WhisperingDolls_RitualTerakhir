using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// Jalankan dari menu: WhisperingDolls > Buat Scene Main Menu
public static class MainMenuSceneBuilder
{
    // ── Palet Warna ───────────────────────────────────────────────
    static readonly Color C_BG        = Hex("080206");
    static readonly Color C_TITLE     = Hex("CC1A14");
    static readonly Color C_PARCHMENT = Hex("E6D4AA");
    static readonly Color C_ACCENT    = Hex("8B1010");
    static readonly Color C_ACCENT_LO = Hex("8B1010", 0.40f);
    static readonly Color C_FRAME     = new Color(0.05f, 0.02f, 0.05f, 0.90f);
    static readonly Color C_DIM       = new Color(0.46f, 0.40f, 0.32f, 0.75f);
    static readonly Color C_BTN_RED   = Hex("5C0806", 0.95f);
    static readonly Color C_BTN_DARK  = Hex("141020", 0.93f);
    static readonly Color C_BTN_BLACK = Hex("09070D", 0.93f);
    static readonly Color C_BTN_LBL   = Hex("F0E0C0");
    static readonly Color C_LORE_OVER = new Color(0f, 0f, 0f, 0.97f);
    static readonly Color C_LORE_BOX  = Hex("0C0610", 0.99f);
    static readonly Color C_LORE_TXT  = Hex("DCCFA0");

    [MenuItem("WhisperingDolls/Buat Scene Main Menu")]
    public static void Build()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // ── Camera ────────────────────────────────────────────────
        var camGO = new GameObject("Main Camera");
        camGO.tag = "MainCamera";
        var cam = camGO.AddComponent<Camera>();
        cam.clearFlags    = CameraClearFlags.SolidColor;
        cam.backgroundColor = C_BG;
        cam.depth         = -1;

        // ── EventSystem ───────────────────────────────────────────
        var esGO = new GameObject("EventSystem");
        esGO.AddComponent<EventSystem>();
        esGO.AddComponent<StandaloneInputModule>();

        // ── Canvas ────────────────────────────────────────────────
        var canvasGO = new GameObject("Canvas");
        var canvas   = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode          = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution  = new Vector2(1920, 1080);
        scaler.screenMatchMode      = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight   = 0.5f;
        canvasGO.AddComponent<GraphicRaycaster>();
        var ct = canvasGO.transform;

        // ── LAYER 1: Background ───────────────────────────────────
        StretchImg(ct, "BG_Base", C_BG);
        StretchImg(ct, "BG_Dim",  new Color(0f, 0f, 0f, 0.30f));

        // Garis merah tipis di tepi layar
        BoxImg(ct, "SBorder_T", C_ACCENT_LO, new Vector2(0,  527), new Vector2(1880,  2));
        BoxImg(ct, "SBorder_B", C_ACCENT_LO, new Vector2(0, -527), new Vector2(1880,  2));
        BoxImg(ct, "SBorder_L", C_ACCENT_LO, new Vector2(-933, 0), new Vector2(2,  1060));
        BoxImg(ct, "SBorder_R", C_ACCENT_LO, new Vector2( 933, 0), new Vector2(2,  1060));

        // ── LAYER 2: Content Frame ────────────────────────────────
        const float FW = 800f, FH = 760f;
        const float FX = FW / 2f, FY = FH / 2f;

        // Panel background
        BoxImg(ct, "Frame_BG", C_FRAME, Vector2.zero, new Vector2(FW, FH));

        // Border 4 sisi
        BoxImg(ct, "Frame_T", C_ACCENT, new Vector2(0,   FY), new Vector2(FW + 4, 2));
        BoxImg(ct, "Frame_B", C_ACCENT, new Vector2(0,  -FY), new Vector2(FW + 4, 2));
        BoxImg(ct, "Frame_L", C_ACCENT, new Vector2(-FX,  0), new Vector2(2, FH + 4));
        BoxImg(ct, "Frame_R", C_ACCENT, new Vector2( FX,  0), new Vector2(2, FH + 4));

        // Dekorasi sudut
        Corner(ct, "C_TL", new Vector2(-FX,  FY),  1f, -1f);
        Corner(ct, "C_TR", new Vector2( FX,  FY), -1f, -1f);
        Corner(ct, "C_BL", new Vector2(-FX, -FY),  1f,  1f);
        Corner(ct, "C_BR", new Vector2( FX, -FY), -1f,  1f);

        // ── LAYER 3: Judul ────────────────────────────────────────
        var titleGO = TMPText(ct, "Title",
            "WHISPERING DOLLS", new Vector2(0, 258), new Vector2(750, 108));
        ConfigTMP(titleGO, 68f, FontStyles.Bold, C_TITLE, TextAlignmentOptions.Center, 9f);

        var subGO = TMPText(ct, "Subtitle",
            "— Ritual Terakhir —", new Vector2(0, 192), new Vector2(620, 46));
        ConfigTMP(subGO, 25f, FontStyles.Italic, C_PARCHMENT, TextAlignmentOptions.Center);

        // Separator ganda
        BoxImg(ct, "Sep1", C_ACCENT,    new Vector2(0, 154), new Vector2(420, 2));
        BoxImg(ct, "Sep2", C_ACCENT_LO, new Vector2(0, 148), new Vector2(280, 1));

        // ── LAYER 4: Tombol ───────────────────────────────────────
        // PENTING: tombol dibuat SEBELUM LorePanel agar LorePanel dirender di atas tombol
        var btnPlay = BtnAccented(ct, "BtnPlay",
            "MULAI GAME", C_BTN_RED,   new Vector2(0,  65), new Vector2(400, 64));
        var btnCerita = BtnAccented(ct, "BtnCerita",
            "CERITA",     C_BTN_DARK,  new Vector2(0, -15), new Vector2(400, 64));
        var btnKeluar = BtnAccented(ct, "BtnKeluar",
            "KELUAR",     C_BTN_BLACK, new Vector2(0, -95), new Vector2(400, 64));

        // ── LAYER 5: Footer ───────────────────────────────────────
        BoxImg(ct, "FooterLine", C_ACCENT_LO, new Vector2(0, -162), new Vector2(500, 1));

        var tagGO = TMPText(ct, "Tagline",
            "Selesaikan ritual sebelum kegelapan menelanmu...",
            new Vector2(0, -188), new Vector2(680, 34));
        ConfigTMP(tagGO, 15f, FontStyles.Italic, C_DIM, TextAlignmentOptions.Center);

        var verGO = TMPText(ct, "Version",
            "v1.0  |  Pemrograman Game 2024",
            new Vector2(820, -505), new Vector2(300, 24));
        ConfigTMP(verGO, 12f, FontStyles.Normal,
            new Color(0.28f, 0.24f, 0.20f, 0.60f), TextAlignmentOptions.Right);

        // ── Manager ───────────────────────────────────────────────
        var mgrGO = new GameObject("MainMenuManager");
        var mgr   = mgrGO.AddComponent<MainMenuManager>();

        // ── LAYER 6: LorePanel — DIBUAT TERAKHIR agar ada di atas segalanya ──
        var lorePanel = BuildLorePanel(ct);
        lorePanel.SetActive(false);
        lorePanel.transform.SetAsLastSibling(); // jamin di atas tombol

        // Assign fields ke manager
        var so = new SerializedObject(mgr);
        so.FindProperty("gameSceneName").stringValue         = "FloodedGroundsGameScene";
        so.FindProperty("lorePanel").objectReferenceValue    = lorePanel;
        so.ApplyModifiedProperties();

        // Wire tombol tutup di lore panel
        var closeBtnT = lorePanel.transform.Find("Box/BtnClose");
        if (closeBtnT != null)
        {
            var cb = closeBtnT.GetComponent<Button>();
            if (cb != null)
                UnityEventTools.AddPersistentListener(cb.onClick, mgr.HideLore);
        }

        // Wire tombol utama
        UnityEventTools.AddPersistentListener(btnPlay.GetComponent<Button>().onClick,   mgr.PlayGame);
        UnityEventTools.AddPersistentListener(btnCerita.GetComponent<Button>().onClick, mgr.ShowLore);
        UnityEventTools.AddPersistentListener(btnKeluar.GetComponent<Button>().onClick, mgr.QuitGame);

        // ── Simpan + Build Settings (kedua scene) ─────────────────
        const string MAIN  = "Assets/Scenes/MainMenu.unity";
        const string GAME  = "Assets/Scenes/FloodedGroundsGameScene.unity";

        EditorSceneManager.SaveScene(scene, MAIN);
        AddToBuildSettings(MAIN,  0);
        AddToBuildSettings(GAME,  1);

        Debug.Log("[MainMenuBuilder] Scene disimpan: " + MAIN);
        EditorUtility.DisplayDialog("Berhasil!",
            "Scene MainMenu berhasil dibuat!\n\n" +
            "Build Settings diupdate:\n" +
            "  [0] MainMenu\n" +
            "  [1] FloodedGroundsGameScene\n\n" +
            "Buka Assets/Scenes/MainMenu.unity untuk preview.\n" +
            "Pastikan FloodedGroundsGameScene juga ada di Build Settings.", "OK");
    }

    // ── Lore Panel ────────────────────────────────────────────────
    static GameObject BuildLorePanel(Transform ct)
    {
        // Overlay gelap penuh layar — alpha hampir 1 agar tidak tembus
        var panelGO = StretchImg(ct, "LorePanel", C_LORE_OVER);
        var pT = panelGO.transform;

        // Kotak konten
        var boxGO = BoxImg(pT, "Box", C_LORE_BOX, new Vector2(0, 10), new Vector2(950, 810));
        var bT    = boxGO.transform;

        // Border 4 sisi
        BoxImg(bT, "BT", C_ACCENT, new Vector2(0,  383), new Vector2(894, 3));
        BoxImg(bT, "BB", C_ACCENT, new Vector2(0, -383), new Vector2(894, 3));
        BoxImg(bT, "BL", C_ACCENT, new Vector2(-450, 0), new Vector2(3, 766));
        BoxImg(bT, "BR", C_ACCENT, new Vector2( 450, 0), new Vector2(3, 766));

        Corner(bT, "C_TL", new Vector2(-450,  383),  1f, -1f);
        Corner(bT, "C_TR", new Vector2( 450,  383), -1f, -1f);
        Corner(bT, "C_BL", new Vector2(-450, -383),  1f,  1f);
        Corner(bT, "C_BR", new Vector2( 450, -383), -1f,  1f);

        // Judul
        var loreTitleGO = TMPText(bT, "LoreTitle",
            "K A L I M U T U ,  1 9 8 7",
            new Vector2(0, 335), new Vector2(840, 60));
        ConfigTMP(loreTitleGO, 28f, FontStyles.Bold, C_TITLE,
            TextAlignmentOptions.Center, 2f);

        BoxImg(bT, "LoreSep", C_ACCENT_LO, new Vector2(0, 298), new Vector2(700, 1));

        // Teks cerita
        string story =
            "Desa Kalimutu lenyap dalam satu malam.\n" +
            "Bukan bencana, bukan wabah — melainkan " +
            "<color=#CC1A14><b>kutukan</b></color>.\n\n" +
            "Seorang dukun tua menanamkan roh-roh jahat ke dalam empat boneka " +
            "kayu kuno yang tersebar di seluruh desa. Selama boneka-boneka itu ada, " +
            "hantu sang dukun akan terus menjaga dan membunuh siapapun yang mendekat.\n\n" +
            "<color=#553333>────────────────────────────────</color>\n\n" +
            "Tahun 2024. <b>Maya Prasasti</b>, mahasiswi arkeologi, datang sendirian\n" +
            "ke reruntuhan Kalimutu yang kini setengah terendam banjir.\n\n" +
            "Misinya:\n" +
            "<color=#CC9933>  >  Temukan 4 boneka jahat yang tersebar di bangunan desa\n" +
            "  >  Kumpulkan 2 kayu bakar\n" +
            "  >  Bakar semuanya di tungku gereja kuno\n" +
            "  >  Kabur sebelum hantu menemukanmu</color>\n\n" +
            "<i>Boneka baik — artefak pelindung warga yang lama pergi —\n" +
            "bisa menenangkan hantu... untuk sementara.</i>\n\n" +
            "<color=#553333>────────────────────────────────</color>\n\n" +
            "<color=#887766><size=85%>" +
            "[WASD] Gerak   [Shift] Sprint   [Ctrl] Jongkok\n" +
            "[Space] Lompat   [E] Interaksi   [Esc] Pause" +
            "</size></color>";

        var bodyGO = TMPText(bT, "LoreBody", story,
            new Vector2(0, -8), new Vector2(860, 570));
        var bodyTMP = bodyGO.GetComponent<TMP_Text>();
        bodyTMP.fontSize        = 18f;
        bodyTMP.color           = C_LORE_TXT;
        bodyTMP.alignment       = TextAlignmentOptions.TopLeft;
        bodyTMP.enableWordWrapping = true;

        BtnAccented(bT, "BtnClose", "TUTUP",
            new Color(0.38f, 0.06f, 0.06f, 0.94f),
            new Vector2(0, -348), new Vector2(200, 52));

        return panelGO;
    }

    // ── Factory Helpers ───────────────────────────────────────────

    static GameObject StretchImg(Transform parent, string name, Color color)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<Image>().color = color;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        return go;
    }

    static GameObject BoxImg(Transform parent, string name,
        Color color, Vector2 pos, Vector2 size)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<Image>().color = color;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta        = size;
        return go;
    }

    static GameObject TMPText(Transform parent, string name,
        string text, Vector2 pos, Vector2 size)
    {
        var go  = new GameObject(name);
        go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text               = text;
        tmp.color              = Color.white;
        tmp.alignment          = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = true;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta        = size;
        return go;
    }

    static void ConfigTMP(GameObject go, float fontSize, FontStyles style,
        Color color, TextAlignmentOptions align, float charSpacing = 0f)
    {
        var t = go.GetComponent<TMP_Text>();
        if (t == null) return;
        t.fontSize        = fontSize;
        t.fontStyle       = style;
        t.color           = color;
        t.alignment       = align;
        t.characterSpacing = charSpacing;
    }

    static GameObject BtnAccented(Transform parent, string name, string label,
        Color bgColor, Vector2 pos, Vector2 size)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<Image>().color = bgColor;

        var btn  = go.AddComponent<Button>();
        var cols = btn.colors;
        cols.normalColor      = bgColor;
        cols.highlightedColor = new Color(
            Mathf.Min(bgColor.r + 0.22f, 1f),
            Mathf.Min(bgColor.g + 0.10f, 1f),
            Mathf.Min(bgColor.b + 0.10f, 1f), bgColor.a);
        cols.pressedColor = new Color(
            bgColor.r * 0.55f, bgColor.g * 0.55f, bgColor.b * 0.55f, bgColor.a);
        cols.selectedColor = cols.normalColor;
        btn.colors = cols;

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta        = size;

        // Left accent bar (merah tipis di dalam sisi kiri tombol)
        var accGO  = new GameObject("Accent");
        accGO.transform.SetParent(go.transform, false);
        accGO.AddComponent<Image>().color = C_TITLE;
        var accRT = accGO.GetComponent<RectTransform>();
        accRT.anchorMin        = new Vector2(0f, 0.15f);
        accRT.anchorMax        = new Vector2(0f, 0.85f);
        accRT.anchoredPosition = new Vector2(10f, 0f);
        accRT.sizeDelta        = new Vector2(4f, 0f);

        // Label text
        var lblGO = new GameObject("Label");
        lblGO.transform.SetParent(go.transform, false);
        var tmp = lblGO.AddComponent<TextMeshProUGUI>();
        tmp.text              = label;
        tmp.fontSize          = 21f;
        tmp.fontStyle         = FontStyles.Bold;
        tmp.color             = C_BTN_LBL;
        tmp.alignment         = TextAlignmentOptions.Center;
        tmp.characterSpacing  = 4f;
        var lRT = lblGO.GetComponent<RectTransform>();
        lRT.anchorMin = Vector2.zero;
        lRT.anchorMax = Vector2.one;
        lRT.offsetMin = lRT.offsetMax = Vector2.zero;

        return go;
    }

    // Dekorasi sudut: dua garis L membentuk sudut
    // dirX: +1 ke kanan, -1 ke kiri | dirY: +1 ke atas, -1 ke bawah
    static void Corner(Transform parent, string name, Vector2 pos, float dirX, float dirY)
    {
        const float LEN = 36f, THK = 2f;

        var grp = new GameObject(name);
        grp.transform.SetParent(parent, false);
        var gRT = grp.AddComponent<RectTransform>();
        gRT.anchorMin = gRT.anchorMax = new Vector2(0.5f, 0.5f);
        gRT.anchoredPosition = pos;
        gRT.sizeDelta = Vector2.zero;

        // Lengan horizontal
        var h = new GameObject("H"); h.transform.SetParent(grp.transform, false);
        h.AddComponent<Image>().color = C_ACCENT;
        var hRT = h.GetComponent<RectTransform>();
        hRT.anchorMin = hRT.anchorMax = new Vector2(0.5f, 0.5f);
        hRT.anchoredPosition = new Vector2(dirX * (LEN * 0.5f), 0f);
        hRT.sizeDelta = new Vector2(LEN, THK);

        // Lengan vertikal
        var v = new GameObject("V"); v.transform.SetParent(grp.transform, false);
        v.AddComponent<Image>().color = C_ACCENT;
        var vRT = v.GetComponent<RectTransform>();
        vRT.anchorMin = vRT.anchorMax = new Vector2(0.5f, 0.5f);
        vRT.anchoredPosition = new Vector2(0f, dirY * (LEN * 0.5f));
        vRT.sizeDelta = new Vector2(THK, LEN);
    }

    static void AddToBuildSettings(string path, int idx)
    {
        // Cek apakah file scene ada
        string fullPath = Application.dataPath.Replace("Assets", "") + path;
        if (!System.IO.File.Exists(fullPath))
        {
            Debug.LogWarning("[MainMenuBuilder] Scene tidak ditemukan: " + path);
            return;
        }
        var list = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        list.RemoveAll(s => s.path == path);
        idx = Mathf.Clamp(idx, 0, list.Count);
        list.Insert(idx, new EditorBuildSettingsScene(path, true));
        EditorBuildSettings.scenes = list.ToArray();
    }

    static Color Hex(string hex, float a = 1f)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        c.a = a;
        return c;
    }
}
