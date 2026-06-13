# Panduan Manual Unity — Whispering Dolls: Ritual Terakhir

> File ini berisi semua langkah yang HARUS kamu lakukan sendiri di Unity Editor.
> Setiap bagian sesuai dengan minggu pengerjaan.
> Claude akan buat semua script C#-nya, kamu tinggal ikuti instruksi di sini.

---

## BAGIAN 1 — Setup Project & Player (Minggu 1)

### 1.1 Buat Unity Project Baru
1. Buka Unity Hub
2. Klik **New Project**
3. Template: pilih **3D (URP)** — Universal Render Pipeline (bagus untuk efek horor)
4. Nama project: `WhisperingDolls`
5. Location: pilih folder `D:\01_Kuliah\Semester_6\Pemograman_game\whisperin_dolls\`
6. Klik **Create project**

### 1.2 Buat Struktur Folder di Assets
Di panel **Project** (bawah kiri Unity):
1. Klik kanan di folder `Assets` → **Create → Folder**
2. Buat folder-folder berikut satu per satu:
   - `Scripts`
   - `Scripts/Core`
   - `Scripts/Player`
   - `Scripts/Inventory`
   - `Scripts/Doll`
   - `Scripts/Enemy`
   - `Scripts/Ritual`
   - `Scripts/Level`
   - `Scripts/UI`
   - `Scripts/Audio`
   - `Scripts/Ending`
   - `ScriptableObjects`
   - `ScriptableObjects/Items`
   - `Prefabs`
   - `Prefabs/Player`
   - `Prefabs/Enemies`
   - `Prefabs/Items`
   - `Prefabs/UI`
   - `Scenes`
   - `Materials`
   - `Audio`
   - `Audio/Ambient`
   - `Audio/SFX`
   - `Audio/Music`
   - `UI`
   - `UI/Sprites`

### 1.3 Script Sudah Tersedia — Tunggu Unity Compile
Script sudah langsung ditulis ke folder yang benar oleh Claude. Kamu tidak perlu copy-paste apapun.
Yang perlu dilakukan:
1. Kembali ke Unity Editor (klik jendela Unity di taskbar)
2. Unity akan otomatis mendeteksi file baru dan compile — tunggu progress bar di pojok kanan bawah selesai
3. Pastikan panel **Console** (bawah) tidak ada tulisan merah (error)

### 1.4 Cek & Ganti Nama Scene
Scene default URP template bernama `SampleScene`. Rename saja:
1. Di panel **Project**, expand folder `Assets/Scenes`
2. Klik kanan `SampleScene` → **Rename** → ketik `GameScene` → Enter
3. Double-click `GameScene` untuk membukanya (jika belum terbuka)

### 1.5 Perbaiki Input System (PENTING!)
Script menggunakan Legacy Input (`Input.GetKey`, `Input.GetAxis`). Template URP mungkin set input ke "New Input System" saja. Cek ini:
1. Menu atas: **Edit → Project Settings**
2. Pilih **Player** di sidebar kiri
3. Scroll ke bagian **Other Settings**
4. Cari **Active Input Handling** — ubah ke **Both** (bukan "Input System Package (New)")
5. Unity akan restart — klik **Apply** jika diminta

### 1.6 Buat Player GameObject
1. Di panel **Hierarchy** (kiri), klik kanan area kosong → **3D Object → Capsule**
2. Rename jadi `Player` (klik sekali nama di Hierarchy → F2 → ketik Player → Enter)
3. Di panel **Inspector** (kanan), set Transform:
   - Position: X=`0`, Y=`1`, Z=`0`
4. Hapus komponen **Capsule Collider** yang ada (klik ⋮ → Remove Component)  
   _Kita akan pakai CharacterController sebagai pengganti_
5. Tambahkan komponen satu per satu lewat **Add Component**:
   - Cari `Character Controller` → klik
     - Set Height: `2`
     - Set Center Y: `1`
   - Cari `Player Controller` → klik _(script Minggu 1)_
   - Cari `Player Interaction` → klik _(script Minggu 1)_

### 1.7 Setup Kamera First-Person
Camera harus jadi **child** dari Player agar ikut bergerak:
1. Di Hierarchy, **drag** `Main Camera` ke dalam `Player` (jadi child)
2. Rename `Main Camera` jadi `PlayerCamera`
3. Set posisi `PlayerCamera` di Inspector:
   - Position: X=`0`, Y=`0.7`, Z=`0` _(setinggi kepala)_
   - Rotation: X=`0`, Y=`0`, Z=`0`

### 1.8 Assign Field di Inspector PlayerController
1. Di Hierarchy, pilih `Player`
2. Di Inspector, lihat komponen **Player Controller**
3. Drag `PlayerCamera` dari Hierarchy ke slot **Camera Transform**
4. Nilai default yang bisa kamu sesuaikan:
   - Walk Speed: `3`
   - Run Speed: `6`
   - Crouch Speed: `1.5`
   - Max Stamina: `5`
   - Mouse Sensitivity: `2`

### 1.9 Assign Field di Inspector PlayerInteraction
1. Masih di `Player`, lihat komponen **Player Interaction**
2. Drag `PlayerCamera` dari Hierarchy ke slot **Camera Transform**
3. **Interactable Layer**: klik dropdown → pilih `Interactable`
   _(layer ini perlu dibuat dulu — lihat langkah 1.10)_
4. Interact Range: `2` (sudah default)

### 1.10 Buat Layer "Interactable"
1. Menu atas: **Edit → Project Settings → Tags and Layers**
2. Di bagian **Layers**, cari slot kosong (misalnya User Layer 6)
3. Ketik `Interactable`
4. Ulangi untuk layer `Player` (Layer 7) dan `Enemy` (Layer 8)
5. Kembali ke Player GameObject → di pojok kanan atas Inspector, set **Layer** ke `Player`

### 1.11 Setup GameManager
1. Di Hierarchy, klik kanan area kosong → **Create Empty**
2. Rename jadi `GameManager`
3. Add Component → cari `Game Manager` → klik
4. Set posisi ke 0,0,0 (tidak penting, tapi rapi)

### 1.12 Test Pertama!
1. Klik tombol **Play ▶** di atas
2. Kursor akan terkunci — gerakkan mouse untuk lihat sekeliling
3. WASD = jalan, Shift = lari, Ctrl = jongkok, Esc = pause
4. Klik **Play ▶** lagi untuk berhenti
5. Jika ada error merah di Console, kirim pesan errornya ke Claude

---

## BAGIAN 2 — Inventory & Item (Minggu 2)

Script yang sudah dibuat Claude: `ItemData.cs`, `InventoryManager.cs`, `PickupItem.cs`

### 2.1 Tunggu Unity Compile
Kembali ke Unity — tunggu progress bar compile di pojok kanan bawah selesai.
Cek Console tidak ada error merah sebelum lanjut.

### 2.2 Tambahkan InventoryManager ke Scene
1. Di Hierarchy, klik **GameManager**
2. Di Inspector → **Add Component** → ketik `Inventory Manager` → klik
3. Set **Max Slots**: `6` (sudah default)

### 2.3 Buat ScriptableObject Data Item
Ini adalah "data" tiap item — bukan objek di scene, tapi file data di Project.

**Boneka Baik:**
1. Di panel Project, buka folder `Assets/ScriptableObjects/Items`
2. Klik kanan di area kosong folder → **Create → WhisperingDolls → ItemData**
3. Nama file: `Item_BonekaBaik` → Enter
4. Klik file tersebut, isi di Inspector:
   - **Item Name**: `Boneka Baik`
   - **Description**: `Boneka ini terasa hangat... mungkin bisa menenangkan sesuatu.`
   - **Item Type**: pilih `DollGood`
   - **Icon**: biarkan kosong dulu (isi nanti di Minggu 5)

**Boneka Jahat:**
1. Klik kanan di folder yang sama → **Create → WhisperingDolls → ItemData**
2. Nama file: `Item_BonekaJahat`
3. Isi Inspector:
   - **Item Name**: `Boneka Jahat`
   - **Description**: `Ada sesuatu yang gelap dari boneka ini. Harus dimusnahkan.`
   - **Item Type**: `DollEvil`

**Bahan Bakar (Kursi tua):**
1. Klik kanan → **Create → WhisperingDolls → ItemData**
2. Nama file: `Item_BahanBakar`
3. Isi Inspector:
   - **Item Name**: `Kursi Tua`
   - **Description**: `Kayu tua yang mudah terbakar.`
   - **Item Type**: `Fuel`

### 2.4 Buat Prefab Boneka Baik
1. Di Hierarchy, klik kanan → **3D Object → Cube** → rename: `BonekaBaik`
2. Set Transform di Inspector:
   - Scale: X=`0.3`, Y=`0.5`, Z=`0.2`
   - Posisi: taruh di suatu sudut lantai untuk testing
3. **Add Component** → `Pickup Item` → klik
4. Di Inspector komponen Pickup Item:
   - Drag file `Item_BonekaBaik` dari Project ke slot **Item Data**
5. Cek ada komponen **Box Collider** (otomatis ada di Cube) — tidak perlu centang Is Trigger
6. Set **Layer** object ini (pojok kanan atas Inspector): pilih `Interactable`
7. Buat material:
   - Di Project, klik kanan folder `Materials` → **Create → Material** → nama: `Mat_BonekaBaik`
   - Klik material → di Inspector, klik kotak warna **Base Map** → pilih warna pink
   - Drag material dari Project ke object `BonekaBaik` di Scene/Hierarchy
8. Drag `BonekaBaik` dari Hierarchy ke folder `Prefabs/Items` → jadi prefab (warna biru di Hierarchy)
9. Object di Hierarchy boleh dibiarkan untuk testing

### 2.5 Buat Prefab Boneka Jahat
Ulangi langkah 2.4 dengan perubahan:
- Nama: `BonekaJahat`
- Item Data: `Item_BonekaJahat`
- Material: `Mat_BonekaJahat` dengan warna merah gelap/hitam

### 2.6 Buat Prefab Bahan Bakar
Ulangi langkah 2.4 dengan perubahan:
- Nama: `BahanBakar`
- Item Data: `Item_BahanBakar`
- Scale: X=`0.8`, Y=`0.5`, Z=`0.4` (lebih besar, seperti kursi)
- Material: `Mat_BahanBakar` dengan warna coklat

### 2.7 Tes Pickup di Play Mode
1. Klik **Play ▶**
2. Dekati salah satu boneka/bahan bakar yang sudah ada di scene
3. Buka **Console** (menu Window → General → Console atau Ctrl+Shift+C)
4. Tekan `E` saat dekat objek
5. Di Console harus muncul: `[Inventory] Ditambah: Boneka Baik | Total: 1`
6. Jika tidak muncul teks apapun, kemungkinan penyebabnya:
   - Layer object tidak di-set ke `Interactable`
   - Slot **Interactable Layer** di PlayerInteraction belum di-assign (cek Inspector Player)

> **Cara assign Interactable Layer di PlayerInteraction:**  
> Pilih Player → Inspector → Player Interaction → klik dropdown **Interactable Layer** → centang `Interactable`

---

## BAGIAN 3 — Enemy AI (Minggu 3)

Script yang sudah dibuat Claude: `GhostAI.cs`, `GhostDetection.cs`

### 3.1 Install Package AI Navigation (WAJIB)
Unity 2022+ memerlukan package terpisah untuk NavMesh.

1. Menu atas: **Window → Package Manager**
2. Di pojok kiri atas dropdown, pilih **Unity Registry**
3. Cari `AI Navigation`
4. Klik → **Install** — tunggu selesai
5. Setelah install, menu **Window → AI → Navigation (Obsolete)** akan tersedia  
   _(pakai yang "Obsolete" dulu karena lebih mudah untuk pemula)_

### 3.2 Tag Player sebagai "Player" (WAJIB)
Script GhostAI mencari player lewat tag. Kalau belum di-set:
1. Klik objek `Player` di Hierarchy
2. Di Inspector pojok kiri atas, klik dropdown **Tag** → pilih `Player`
   - Kalau tidak ada tag Player: klik **Add Tag** → klik `+` → ketik `Player` → Save → ulangi langkah 2

### 3.3 Setup NavMesh (cara baru — NavMeshSurface)
Di AI Navigation package terbaru, baking dilakukan lewat komponen **NavMeshSurface**, bukan dari jendela Navigation.

1. Di Hierarchy, klik objek lantai (Plane/floor)
2. Inspector → **Add Component** → ketik `Nav Mesh Surface` → klik
3. Di komponen NavMeshSurface yang muncul:
   - **Agent Type**: Humanoid (default)
   - **Collect Objects**: All Game Objects (artinya semua objek di scene ikut dihitung)
   - **Include Layers**: Everything
4. Klik tombol **Bake** yang ada di dalam komponen itu
5. Tunggu selesai — lantai akan tampak overlay biru di Scene view

> Kalau ghost tetap tidak bisa jalan setelah Bake → coba pindahkan komponen NavMeshSurface ke Empty Object di root Hierarchy (bukan di lantai), lalu Bake ulang.

### 3.4 Tambahkan Layer "Enemy"
1. **Edit → Project Settings → Tags and Layers**
2. Cari slot kosong di Layers → ketik `Enemy`
3. Kembali ke scene

### 3.5 Buat Ghost di Scene
1. Hierarchy → klik kanan → **3D Object → Capsule** → rename: `Ghost`
2. Inspector → set Transform:
   - Position: taruh di sudut ruangan, jauh dari Player
   - Scale: X=`0.9`, Y=`1.1`, Z=`0.9`
3. Set **Layer** → `Enemy`
4. **Add Component** → `Nav Mesh Agent`
   - Stopping Distance: `0.5`
   - Speed biarkan default (diatur otomatis oleh GhostAI)
5. **Add Component** → `Ghost AI`
6. **Add Component** → `Ghost Detection`
7. Buat material ghost:
   - Project panel → klik kanan di `Materials` → **Create → Material** → nama: `Mat_Ghost`
   - Warna: abu-abu gelap
   - Drag material ke Ghost di scene

### 3.6 Buat Waypoints Patrol
Waypoints = titik-titik yang akan dipatroli ghost secara berurutan.

1. Hierarchy → klik kanan → **Create Empty** → rename: `PatrolPoints`
2. Klik kanan `PatrolPoints` → **Create Empty** → rename: `WP_01`
3. Ulangi buat `WP_02`, `WP_03`, `WP_04`
4. Posisikan tiap waypoint di sudut-sudut area (di Scene view: tekan **W** → drag panah)
   - Pastikan posisi waypoints di atas lantai (Y = `0`)
5. Klik `Ghost` di Hierarchy → Inspector → komponen **Ghost AI**
6. Di field **Waypoints**, set Size: `4`
7. Drag `WP_01` ke slot `Element 0`, `WP_02` ke `Element 1`, dst.

### 3.7 Setup GhostDetection di Inspector
1. Klik `Ghost` di Hierarchy → Inspector → komponen **Ghost Detection**
2. Set nilai:
   - **Sight Range**: `10`
   - **Fov Angle**: `90`
   - **Max Hearing Range**: `8`
   - **Sight Block Layer**: centang layer `Default` (dinding/objek yang menghalangi pandangan)

### 3.8 Test Ghost AI
1. Klik **Play ▶**
2. Ghost harus bergerak dari WP ke WP (patrol)
3. Dekati ghost dari arah depan → Console muncul `[Ghost] State → Chase`
4. Berlari menjauhi ghost → setelah 3 detik muncul `[Ghost] State → Patrol`
5. Saat dikejar, ambil boneka baik di inventory, dekati ghost, tekan **E** → muncul `[Ghost] State → Calm`

> **Ghost tidak bergerak?** Kemungkinan NavMesh belum di-bake. Cek overlay biru di lantai (Scene view).  
> **Ghost menembus dinding?** Dinding belum di-set Navigation Static → bake ulang NavMesh.

---

## BAGIAN 4 — Ritual System & Level Design (Minggu 4)

### 4.1 Buat Layout Ruangan
Buat ruangan dasar menggunakan Cube yang diratakan:

**Ruang Tamu (area awal):**
1. Buat Cube → scale X=10, Y=0.1, Z=10 → nama `Floor_RuangTamu`
2. Buat 4 Cube untuk dinding → scale tipis, tinggi
3. Posisikan membentuk ruangan

**Kamar Tidur:**
- Posisi: sebelah kiri ruang tamu
- Ukuran lebih kecil: 6x6

**Dapur:**
- Posisi: sebelah kanan ruang tamu

**Basement:**
- Di bawah ruang tamu (Y negatif)
- Ini area ritual utama
- Tambahkan tungku di tengah

> Tip: Gunakan **ProBuilder** (Window → Package Manager → cari ProBuilder → Install) untuk buat ruangan lebih mudah

### 4.2 Buat Tungku (Furnace)
1. Buat objek tungku dari beberapa Cube digabung, atau gunakan 1 Cube saja dulu
2. Rename: `Furnace`
3. Add Component → `FurnaceController` (script dari Claude)
4. Add Component → `Box Collider` (jika belum ada)
   - Centang **Is Trigger**
5. Tag objek ini: klik kanan di Tags → Add Tag: `Furnace`

### 4.3 Buat Pintu
1. Buat Cube → scale sesuai pintu → nama: `Door_01`
2. Add Component → `DoorController` (script dari Claude)
3. Add Component → `Box Collider` (Is Trigger: ON untuk area deteksi)

---

## BAGIAN 5 — UI & Audio (Minggu 5)

### 5.1 Buat Canvas UI
1. Di Hierarchy, klik kanan → **UI → Canvas**
2. Di Inspector Canvas, set:
   - Render Mode: **Screen Space - Overlay**
3. Di Canvas, tambahkan:

**Panel HUD:**
- Klik kanan Canvas → **UI → Panel** → nama: `HUD`
- Di dalam HUD, buat teks untuk notifikasi:
  - Klik kanan HUD → **UI → Text - TextMeshPro** → nama: `NotificationText`
  - Posisi: tengah atas layar

**Panel Inventory:**
- Klik kanan Canvas → **UI → Panel** → nama: `InventoryPanel`
- Isi dengan 6 `UI → Image` sebagai slot (nama: `Slot_01` sampai `Slot_06`)
- Default: `InventoryPanel` → SetActive(false) — disembunyikan

### 5.2 Setup UIManager
1. Pilih GameObject `GameManager`
2. Add Component → `UIManager`
3. Di Inspector UIManager, drag referensi:
   - `InventoryPanel` → slot InventoryPanel
   - `NotificationText` → slot NotificationText

### 5.3 Setup AudioMixer
1. Di panel Project, klik kanan → **Create → Audio Mixer** → nama: `GameAudioMixer`
2. Double-click untuk buka AudioMixer window
3. Buat 3 group (klik tombol "+" di Groups):
   - `Ambient`
   - `SFX`
   - `Music`

### 5.4 Import Audio
1. Cari audio gratis di [freesound.org](https://freesound.org) atau [pixabay.com/music](https://pixabay.com/music):
   - Ambient: suara angin, bisikan
   - SFX: langkah kaki, pintu, pickup item
   - Music: musik latar minimalis, tegang
2. Drag file audio (.mp3 atau .wav) ke folder `Audio/` yang sesuai
3. Di Inspector tiap AudioClip, set **Load Type** sesuai:
   - Ambient/Music: `Streaming` (hemat memori)
   - SFX: `Decompress On Load` (respons cepat)

### 5.5 Setup AudioManager
1. Di Hierarchy, buat Empty Object → nama: `AudioManager`
2. Add Component → 3 buah `Audio Source` (klik Add Component → AudioSource, 3 kali)
3. Add Component → `AudioManager` (script dari Claude)
4. Assign tiap AudioSource ke slot di Inspector AudioManager

---

## BAGIAN 6 — Ending & Final Testing (Minggu 6)

### 6.1 Setup Ending Manager
1. Pilih `GameManager` di Hierarchy
2. Add Component → `EndingManager`
3. Isi nilai di Inspector:
   - Min Dolls For Good Ending: `3`
   - Min Dolls For Bad Ending: `3`

### 6.2 Buat Ending Screen
1. Di Canvas, buat Panel baru → nama: `EndingPanel`
2. Di dalamnya:
   - TextMeshPro → nama: `EndingTitle`
   - TextMeshPro → nama: `EndingDescription`
   - Button → nama: `BtnMainMenu`
3. Default: SetActive(false)

### 6.3 Build Game
1. Menu **File → Build Settings**
2. Platform: **PC, Mac & Linux Standalone**
3. Target Platform: **Windows**
4. Architecture: **x86_64**
5. Klik **Add Open Scenes** (pastikan GameScene masuk)
6. Klik **Build**
7. Pilih folder output

---

## Tips Umum Unity untuk Pemula

| Situasi | Cara |
|---------|------|
| Script tidak muncul di Add Component | Cek ada error di Console (bawah), perbaiki dulu |
| Object tidak bergerak saat play | Pastikan script sudah di-attach ke GameObject yang benar |
| Ghost menembus dinding | Pastikan dinding sudah di-mark sebagai Navigation Static, bake ulang NavMesh |
| Klik E tapi tidak ada interaksi | Cek di PlayerInteraction: layer mask sudah benar, objek target punya tag "Interactable" |
| Game crash saat start | Cek Console untuk NullReferenceException — biasanya ada referensi yang belum di-assign di Inspector |

---

## Shortcut Penting Unity

| Shortcut | Fungsi |
|----------|--------|
| Ctrl+S | Save scene |
| Ctrl+Z | Undo |
| F | Focus ke objek yang dipilih |
| W/E/R | Mode Move/Rotate/Scale di scene view |
| Alt+klik drag | Orbit kamera scene |
| Ctrl+D | Duplicate objek |
| Play (▶) | Test play scene |
