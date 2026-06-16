# MANUAL UNITY — WEEK 5
# Whispering Dolls: Ritual Terakhir
# Main Menu + Cerita + Ritual System + Penempatan Item

---

## RINGKASAN FITUR BARU

| Script | Fungsi |
|--------|--------|
| `RitualManager.cs` | Cek & eksekusi ritual (4 boneka jahat + 2 kayu bakar) |
| `IncineratorController.cs` | Tungku interaktif di gereja |
| `StoryManager.cs` | Sistem narasi typewriter (antri, tampil otomatis) |
| `NarratorTrigger.cs` | Zone yang memunculkan teks cerita saat player masuk |
| `MainMenuManager.cs` | Tombol Play/Lore/Quit di Main Menu |
| `HUDManager.cs` | HUD: stamina bar + counter item + interact prompt |
| `EndingManager.cs` | Layar GameOver dan Good Ending (2 varian) |

---

## BAGIAN 1 — SCENE MAIN MENU

### 1.1 Buat Scene Baru

1. File → New Scene → pilih template **Basic (Built-in)** atau **Empty**
2. Simpan: File → Save As → nama `MainMenu`
3. Tambahkan ke Build Settings: File → Build Settings → **Add Open Scenes**

### 1.2 Buat Canvas Main Menu

**Hierarchy → klik kanan → UI → Canvas**

- Canvas component:
  - Render Mode: `Screen Space - Overlay`
  - UI Scale Mode: `Scale With Screen Size` → 1920 × 1080

**Di bawah Canvas, buat struktur ini:**
```
Canvas
├── Background (UI → Raw Image atau Image, warna hitam atau gambar horror)
├── TitleText (TextMeshPro - Text) → "WHISPERING DOLLS"
├── SubtitleText (TextMeshPro - Text) → "Ritual Terakhir"
├── ButtonPanel
│   ├── BtnPlay (Button - TextMeshPro)
│   ├── BtnLore (Button - TextMeshPro)
│   ├── BtnQuit (Button - TextMeshPro)
├── LorePanel (dinonaktifkan defaultnya)
│   ├── LoreBackground (Image, warna hitam semi-transparan)
│   ├── LoreText (TextMeshPro - Text)  ← salin teks cerita di bawah
│   └── BtnCloseLore (Button)
└── CreditsPanel (dinonaktifkan) [opsional]
```

### 1.3 Teks Lore / Cerita

Salin ke `LoreText`:

```
KALIMUTU, 1987

Desa Kalimutu lenyap dalam satu malam.
Bukan bencana, bukan wabah — melainkan kutukan.

Seorang dukun tua telah menanamkan roh-roh jahat ke dalam empat boneka
kayu kuno yang tersebar di seluruh desa. Selama boneka-boneka itu ada,
hantu sang dukun akan terus menjaga — dan membunuh siapapun yang mendekat.

Tahun 2024. Maya Prasasti, mahasiswi arkeologi, datang sendirian
ke reruntuhan desa yang kini setengah terendam banjir.

Tugasnya satu: temukan keempat boneka itu, kumpulkan kayu bakar,
dan bakar semuanya di tungku gereja kuno sebelum hantu menemukan Maya.

Boneka baik — artefak pelindung dari warga yang lama pergi —
bisa menenangkan hantu sementara. Tapi jangan berharap terlalu banyak.

Selamat datang di Kalimutu.
Kamu tidak akan sendirian lama.
```

### 1.4 Setup MainMenuManager

1. Hierarchy → buat **Empty Object** → rename `MainMenuManager`
2. Tambahkan komponen `MainMenuManager`
3. Di Inspector:
   - **Game Scene Name**: `FloodedGroundsGameScene`
   - **Lore Panel**: drag LorePanel dari Hierarchy
   - **Credits Panel**: drag CreditsPanel (jika ada)

### 1.5 Hubungkan Tombol

| Button | OnClick() → Function |
|--------|----------------------|
| BtnPlay | MainMenuManager → `PlayGame()` |
| BtnLore | MainMenuManager → `ShowLore()` |
| BtnCloseLore | MainMenuManager → `HideLore()` |
| BtnQuit | MainMenuManager → `QuitGame()` |

### 1.6 Atmosfer Main Menu (opsional tapi disarankan)

- **Background**: gunakan gambar gelap/berkabut dari asset Flooded Grounds
- **TitleText font**: pilih font horror (tersedia di Google Fonts: Creepster, Eater)
  - Import .ttf → klik kanan → Create → TextMeshPro → Font Asset
- **Particle fog**: tambahkan Particle System di belakang teks untuk efek kabut
- **Post Processing**: tambahkan Volume → Vignette + Color Grading (tint gelap)

---

## BAGIAN 2 — SCENE GAME: SETUP MANAGER BARU

Di scene `FloodedGroundsGameScene`:

### 2.1 Tambah RitualManager

1. Hierarchy → klik kanan Empty Object → rename `RitualManager`
2. Add Component → `RitualManager`

### 2.2 Tambah EndingManager

1. Hierarchy → klik kanan Empty Object → rename `EndingManager`
2. Add Component → `EndingManager`
3. Inspector:
   - **Main Menu Scene Name**: `MainMenu`
   - **Game Over Panel**: (akan diisi setelah buat Canvas — langkah 2.4)
   - **Good Ending Panel**: (sama)

### 2.3 Tambah StoryManager

1. Hierarchy → klik kanan Empty Object → rename `StoryManager`
2. Add Component → `StoryManager`
3. Inspector:
   - **Narrator Panel**: (isi setelah setup Canvas)
   - **Narrator Text**: (isi setelah setup Canvas)
   - **Typewriter Speed**: `0.03`
   - **Hold Duration**: `3.5`

### 2.4 Setup HUD Canvas

**Hierarchy → klik kanan → UI → Canvas** → rename `HUD_Canvas`

Buat struktur di bawah Canvas:
```
HUD_Canvas
├── StaminaBar (UI → Slider)
│   └── Atur min=0, max=1, interactable=false
├── ItemCounter (Panel / Group kosong)
│   ├── EvilDollText (TextMeshPro) → "Boneka Jahat: 0/4"
│   ├── FuelText (TextMeshPro) → "Kayu Bakar: 0/2"
│   └── GoodDollText (TextMeshPro) → "Boneka Baik: 0/4"
├── InteractPromptText (TextMeshPro) → posisi bawah tengah
│   (nonaktifkan defaultnya di Inspector dengan uncheck di sebelah nama)
│
├── NarratorPanel (Image hitam semi-transparan, bawah layar)
│   └── NarratorText (TextMeshPro, teks putih / krem)
│   [nonaktifkan NarratorPanel secara default]
│
├── GameOverPanel (Image hitam penuh, nonaktifkan default)
│   ├── TitleText → "GAME OVER"
│   ├── BodyText → (kosong, diisi EndingManager)
│   ├── BtnRestart (Button)
│   └── BtnMainMenu (Button)
│
└── GoodEndingPanel (Image gelap, nonaktifkan default)
    ├── TitleText → "RITUAL SELESAI"
    ├── BodyText → (kosong, diisi EndingManager)
    └── BtnMainMenu (Button)
```

### 2.5 Hubungkan HUDManager

1. Hierarchy → klik kanan Empty Object → rename `HUDManager`
2. Add Component → `HUDManager`
3. Di Inspector, drag/assign:
   - **Stamina Slider** → StaminaBar
   - **Evil Doll Text** → EvilDollText
   - **Fuel Text** → FuelText
   - **Good Doll Text** → GoodDollText
   - **Interact Prompt Text** → InteractPromptText
   - **Player Controller** → drag Player gameObject
   - **Player Interaction** → drag Player gameObject

### 2.6 Selesaikan referensi StoryManager & EndingManager

StoryManager Inspector:
- **Narrator Panel** → NarratorPanel
- **Narrator Text** → NarratorText

EndingManager Inspector:
- **Game Over Panel** → GameOverPanel
- **Good Ending Panel** → GoodEndingPanel
- **Game Over Body Text** → GameOverPanel/BodyText
- **Good Ending Body Text** → GoodEndingPanel/BodyText

### 2.7 Hubungkan tombol di panels

| Button | OnClick() → Function |
|--------|----------------------|
| BtnRestart (GameOver) | EndingManager → `RestartGame()` |
| BtnMainMenu (GameOver) | EndingManager → `ReturnToMainMenu()` |
| BtnMainMenu (GoodEnding) | EndingManager → `ReturnToMainMenu()` |

---

## BAGIAN 3 — TUNGKU RITUAL DI GEREJA

### 3.1 Setup Incinerator

1. Di scene, masuk ke bangunan gereja (Church) dari Flooded Grounds
2. Tambahkan Cube atau gunakan mesh gereja sebagai tungku → rename `Incinerator`
3. Add Component:
   - `IncineratorController`
   - Pastikan ada `Collider` (Box Collider cukup)
   - Set Layer → **Interactable** (layer yang di-raycast PlayerInteraction)
4. Opsional: tambahkan **Particle System** untuk efek api, drag ke field **Flame Effect**

---

## BAGIAN 4 — PENEMPATAN BONEKA DAN KAYU BAKAR

### 4.1 ScriptableObject Items (jika belum ada)

Project window → klik kanan → Create → WhisperingDolls → ItemData:

| Nama Asset | Item Name | Item Type |
|------------|-----------|-----------|
| `Item_BonekaJahat` | Boneka Jahat | DollEvil |
| `Item_BonekaBaik` | Boneka Baik | DollGood |
| `Item_KayuBakar` | Kayu Bakar | Fuel |

### 4.2 Buat Prefab Item

Untuk setiap jenis item:
1. Hierarchy → buat Cube (atau gunakan mesh yang ada)
2. Rename sesuai (misal `Prefab_BonekaJahat`)
3. Add Component → `PickupItem`
4. Assign field **Item Data** → drag ScriptableObject yang sesuai
5. Set Layer → **Interactable**
6. Drag ke folder Assets/Prefabs → jadikan Prefab

### 4.3 Lokasi Penempatan

**4 Boneka Jahat (DollEvil)** — satu per bangunan:

| # | Lokasi | Penempatan |
|---|--------|------------|
| 1 | **Cabin kecil** (dekat spawn player) | Di atas meja atau di sudut gelap |
| 2 | **Barn / Gudang besar** | Tersembunyi di balik jerami atau kotak |
| 3 | **Brick House** | Di lantai atas atau di lemari |
| 4 | **Industrial Building** | Area yang dijaga Ghost utama — paling sulit |

**4 Boneka Baik (DollGood)** — untuk menenangkan hantu sementara:

| # | Lokasi | Catatan |
|---|--------|---------|
| 1 | Luar Cabin #1 | Mudah ditemukan, untuk hint awal |
| 2 | Dekat Barn | Tersebar tidak jauh dari boneka jahat |
| 3 | Di jalan menuju gereja | Seperti sengaja ditinggalkan seseorang |
| 4 | Di dalam Gereja | Reward jika berani masuk gereja lebih awal |

**2 Kayu Bakar (Fuel)**:

| # | Lokasi |
|---|--------|
| 1 | Luar Barn / tumpukan kayu dekat gudang |
| 2 | Samping Gereja atau halaman gereja |

### 4.4 Tips Penempatan

- Pastikan semua item ada di atas NavMesh (tidak melayang atau tenggelam)
- Beri cahaya redup (Point Light kecil, warna kuning/oranye) dekat item penting
- Jangan taruh item di tempat yang terlalu mudah atau terlalu tersembunyi

---

## BAGIAN 5 — NARRATOR TRIGGER (TITIK CERITA)

Tambahkan `NarratorTrigger` di titik-titik penting:

### Cara Setup:
1. Hierarchy → klik kanan → Create Empty → rename `NarratorTrigger_[nama]`
2. Add Component → `NarratorTrigger` + `Box Collider`
3. Box Collider: centang **Is Trigger**, besarkan ukuran (misal 5×3×5 m)
4. Isi field **Narrative Text** di Inspector
5. Set **Trigger Once** = true (default)
6. Pastikan Player memiliki tag = **Player**

### Titik Cerita yang Disarankan:

**Trigger 1 — Spawn Point (delay 1.5 detik)**
Teks:
```
Desa Kalimutu... setengah terendam.
Tidak ada suara manusia. Hanya angin dan bisikan.

Carilah 4 boneka jahat yang tersebar di bangunan ini.
Bawa juga kayu bakar. Ritual harus dilakukan di gereja.

Dan hati-hati — kamu tidak sendirian.
```
Delay: `1.5`

**Trigger 2 — Mendekati Cabin pertama**
Teks:
```
Rumah itu terlihat ditinggalkan tergesa-gesa...
Piring masih di meja. Lilin masih menyala.
Sesuatu di sini terasa tidak beres.
```

**Trigger 3 — Mendekati Barn / Gudang**
Teks:
```
Gudang tua ini berbau tanah basah dan sesuatu yang lebih gelap.
Bisikan-bisikan makin jelas di sini...
```

**Trigger 4 — Mendekati Gereja**
Teks:
```
Gereja itu masih berdiri, meski dinding-dindingnya retak.
Di dalamnya — tungku yang sudah lama tidak digunakan.
Di sinilah ritual harus diselesaikan.
```

**Trigger 5 — Pintu Masuk Gereja**
Teks:
```
Satu langkah lagi...
Kalau kamu punya semua yang dibutuhkan,
tungku itu akan mengakhiri semuanya.
```

---

## BAGIAN 6 — BUILD SETTINGS & SCENE ORDER

File → Build Settings → pastikan urutan:
1. `MainMenu` (index 0)
2. `FloodedGroundsGameScene` (index 1)

---

## BAGIAN 7 — TEST CHECKLIST

### Main Menu
- [ ] Tombol Play membuka FloodedGroundsGameScene
- [ ] Tombol Lore membuka LorePanel dengan teks cerita
- [ ] Tombol Quit menutup game (di Editor: berhenti play mode)
- [ ] Cursor terlihat di main menu

### HUD
- [ ] Stamina bar turun saat sprint, naik saat jalan
- [ ] Counter "Boneka Jahat: 0/4" muncul dan update saat pickup
- [ ] Interact prompt muncul saat lihat objek interaktif
- [ ] Narrator Panel muncul saat masuk trigger zone

### Ritual
- [ ] Incinerator menampilkan "butuh X boneka jahat dan Y kayu bakar lagi"
- [ ] Saat punya 4 boneka jahat → narasi milestone muncul otomatis
- [ ] Saat punya semua item → Incinerator prompt berubah ke "Lakukan Ritual"
- [ ] Setelah ritual → GoodEndingPanel muncul
- [ ] Teks ending berbeda jika masih punya DollGood (★ ENDING TERBAIK ★)

### GameOver
- [ ] Hantu mengejar, menyentuh player → GameOverPanel muncul
- [ ] Tombol Restart mereset game ke awal
- [ ] Tombol Main Menu kembali ke scene MainMenu

### Cerita
- [ ] NarratorTrigger spawn area muncul setelah delay 1.5 detik
- [ ] Setiap bangunan memiliki trigger cerita
- [ ] Teks muncul karakter per karakter (typewriter effect)
- [ ] Panel narasi menghilang setelah 3.5 detik
```

---

## CATATAN TAMBAHAN

**Inventory Slots**: `maxSlots` di InventoryManager default = 6. Untuk bawa 4 DollEvil + 2 Fuel sekaligus tepat 6. Naikkan ke 10 jika ingin player bisa bawa semua item (termasuk DollGood) sekaligus.

**Ghost Patrol**: Taruh Ghost di Industrial Building dengan waypoints berputar di sekitar area tersebut. Ini area yang menjaga BonekaJahat ke-4, jadi paling menantang.

**Lighting Horror**: Di setiap bangunan, kurangi intensitas ambient light. Gunakan Point Light warna merah/oranye redup. Tambahkan Fog di Lighting Settings.
