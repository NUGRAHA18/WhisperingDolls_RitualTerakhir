# MANUAL UNITY — WEEK 4: MIGRASI KE FLOODED GROUNDS
Game: Whispering Dolls: Ritual Terakhir

---

## BAGIAN 1 — Import Flooded Grounds & Fix URP

### 1.1 Import Asset
1. Buka Unity → Window → Package Manager → pilih tab "My Assets"
2. Cari "Flooded Grounds" → klik Download → klik Import
3. Pada dialog Import, centang semua → klik Import
4. Tunggu hingga selesai — JANGAN klik Play dulu

### 1.2 Fix Material URP (WAJIB sebelum Play)
```
Edit → Rendering → Materials → Convert All Built-in Materials to URP
```
Klik "Proceed" jika ada konfirmasi. Tunggu proses selesai.

### 1.3 Verifikasi Tidak Ada Error Merah
- Buka Console (Window → General → Console)
- Pastikan tidak ada error merah sebelum lanjut
- Warning kuning boleh diabaikan
- Jika error "Shader not found": ulangi langkah 1.2

### 1.4 Cek Input System
```
Edit → Project Settings → Player → Other Settings → Active Input Handling
```
Pastikan nilainya = "Both" (bukan "New" saja)

---

## BAGIAN 2 — Setup Scene

### 2.1 Buka Scene Flooded Grounds
1. Di Project window, cari folder "Flooded Grounds" → Scenes
2. Double-click scene demo-nya untuk membuka
3. Atau: buat scene baru → File → New Scene → Basic (Built-in)

### 2.2 Jika Buat Scene Baru
1. Drag prefab environment Flooded Grounds ke Hierarchy
2. Atur posisi di (0, 0, 0)
3. File → Save As → simpan sebagai "GameScene" di Assets/Scenes/

### 2.3 Setup Lighting & Fog
```
Window → Rendering → Lighting
```
- Tab Environment:
  - Skybox Material: pilih yang gelap/overcast dari Flooded Grounds
  - Ambient Color: warna abu gelap (#1A1A1A)
- Tab Environment lagi, scroll bawah:
  - Fog: centang Enable
  - Fog Color: abu kebiruan gelap (#2B3040)
  - Fog Mode: Exponential Squared
  - Fog Density: 0.015

### 2.4 Tentukan 5 Bangunan yang Dipakai
Dari modul Flooded Grounds, pilih dan tandai bangunan berikut:

| Bangunan | Fungsi | Boneka |
|---|---|---|
| Church/Bangunan besar | Lokasi Ritual (tungku di sini) | - |
| Cabin #1 | Start area player | Boneka #1 |
| Barn | Area eksplorasi tengah | Boneka #2 |
| Brick House | Area eksplorasi luar | Boneka #3 |
| Industrial Building | Ghost sering patrol di sini | - |

---

## BAGIAN 3 — Setup Player GameObject

### 3.1 Buat Player
1. Hierarchy → klik kanan → Create Empty → rename "Player"
2. Atur posisi Player: (X, Y, Z) sesuai titik awal di dalam Cabin #1
3. Tambah komponen berikut (Inspector → Add Component):
   - `Character Controller`
   - `PlayerController` (script)
   - `PlayerInteraction` (script)

### 3.2 Setting Character Controller
Di komponen Character Controller:
- Height: 2
- Radius: 0.4
- Center Y: 1 (setengah dari height)

### 3.3 Buat Camera Child
1. Klik kanan pada "Player" di Hierarchy → Create Empty → rename "CameraHolder"
2. Set posisi CameraHolder: (0, 1.7, 0) — setinggi mata
3. Klik kanan pada CameraHolder → Camera → rename "Main Camera"
4. Pastikan tag Main Camera = "MainCamera"
5. Tambah komponen AudioListener ke Main Camera

### 3.4 Assign CameraTransform — INI YANG MENYEBABKAN BUG KAMERA
1. Klik GameObject "Player" di Hierarchy
2. Di Inspector, lihat komponen PlayerController
3. Field "Camera Transform": drag GameObject "CameraHolder" dari Hierarchy ke field ini
4. JANGAN drag "Main Camera" — yang di-drag adalah "CameraHolder"

### 3.5 Setting PlayerController di Inspector
- Walk Speed: 3
- Run Speed: 6
- Crouch Speed: 1.5
- Max Stamina: 5
- Mouse Sensitivity: 2
- Max Look Angle: 80
- Jump Force: 4

### 3.6 Kontrol Player (Ringkasan)
| Tombol | Aksi |
|---|---|
| WASD | Gerak |
| Shift | Lari (pakai stamina) |
| Ctrl | Jongkok (toggle) |
| Space | Lompat |
| E | Interaksi |
| Esc | Pause |

---

## BAGIAN 4 — Setup GameManager

### 4.1 Buat GameManager Object
1. Hierarchy → klik kanan → Create Empty → rename "GameManager"
2. Posisi: (0, 0, 0)
3. Tambah komponen:
   - `GameManager` (script)
   - `InventoryManager` (script)

### 4.2 Buat ScriptableObject Item (jika belum ada)
Klik kanan di Assets/ScriptableObjects/ → Create:
- Create → WhisperingDolls → ItemData → rename "Item_BonekaBaik"
  - Item Type: DollGood
  - Display Name: Boneka Baik
- Buat lagi → rename "Item_BonekaJahat"
  - Item Type: DollEvil
  - Display Name: Boneka Jahat
- Buat lagi → rename "Item_BahanBakar"
  - Item Type: Fuel
  - Display Name: Bahan Bakar

---

## BAGIAN 5 — Setup Ghost AI

### 5.1 Pastikan Package AI Navigation Terinstall
```
Window → Package Manager → Unity Registry → cari "AI Navigation" → Install
```

### 5.2 Buat Ghost GameObject
1. Hierarchy → klik kanan → Create Empty → rename "Ghost"
2. Tambah komponen:
   - `Capsule Collider` (Height: 2, Radius: 0.4)
   - `Nav Mesh Agent`
   - `GhostAI` (script)
   - `GhostDetection` (script)
3. Tag: biarkan default (Untagged)

### 5.3 Setting NavMeshAgent di Inspector
- Speed: 3.5
- Angular Speed: 360
- Acceleration: 8
- Stopping Distance: 0.5
- Radius: 0.4
- Height: 2

### 5.4 Buat Waypoints Ghost (Outdoor, Antar Bangunan)
1. Buat 10 GameObject kosong, rename "Waypoint_01" s/d "Waypoint_10"
2. Kelompokkan dalam satu parent: Create Empty → rename "GhostWaypoints"
3. Tempatkan waypoint di:
   - 2 titik di area outdoor depan Church
   - 2 titik di jalan utama antar bangunan
   - 2 titik dekat Barn
   - 2 titik dekat Brick House
   - 2 titik dekat Industrial Building

### 5.5 Assign Waypoints ke GhostAI
1. Klik Ghost → Inspector → GhostAI
2. Field "Waypoints": isi size = 10
3. Drag Waypoint_01 s/d Waypoint_10 ke tiap slot

### 5.6 Setting GhostAI & GhostDetection di Inspector
GhostAI:
- Sight Range: 20 (lebih luas karena outdoor)
- Chase Speed: 5
- Calm Distance: 8

GhostDetection:
- Sight Range: 20
- FOV Angle: 90
- Max Hearing Range: 25 (lebih luas outdoor)
- Player Layer: pilih layer "Player"

---

## BAGIAN 6 — Bake NavMesh

### 6.1 Setup NavMesh Surface
1. Klik pada GameObject environment Flooded Grounds di Hierarchy
2. Add Component → NavMesh Surface
3. Di komponen NavMesh Surface:
   - Agent Type: Humanoid
   - Collect Objects: All Game Objects
   - Include Layers: Everything (kecuali layer yang tidak perlu)

### 6.2 Bake
1. Klik tombol "Bake" di bawah komponen NavMesh Surface
2. Tunggu proses selesai (bisa 1–3 menit untuk map besar)
3. Area biru muda = area yang bisa dilewati Ghost
4. Pastikan area outdoor + interior bangunan terwakili

### 6.3 Jika Area Indoor Tidak Ter-bake
- Masuk ke dalam bangunan, tandai floor sebagai Navigation Static
- Klik floor object → Inspector → kanan atas ada dropdown Static → centang "Navigation Static"
- Bake ulang

---

## BAGIAN 7 — Setup Interactable Props

### 7.1 Buat Prefab Boneka
1. Buat GameObject 3D sederhana (Capsule kecil atau model boneka)
2. Rename "Prefab_BonekaBaik"
3. Tambah komponen:
   - `Box Collider` (Is Trigger: OFF)
   - `PickupItem` (script)
4. Di PickupItem → assign "Item_BonekaBaik" ScriptableObject
5. Drag ke Assets/Prefabs/ untuk jadi Prefab
6. Ulangi untuk "Prefab_BonekaJahat"

### 7.2 Letakkan Boneka di Scene
- Drag Prefab_BonekaBaik ke dalam Cabin #1 (di atas meja atau sudut ruangan)
- Drag Prefab_BonekaBaik ke dalam Barn
- Drag Prefab_BonekaJahat ke dalam Brick House

### 7.3 Layer Setup
1. Edit → Project Settings → Tags and Layers
2. Tambah Layer baru: "Interactable" (User Layer 6)
3. Set semua prefab boneka ke layer "Interactable"
4. Di PlayerInteraction → Layer Mask: pilih "Interactable"

---

## BAGIAN 8 — Setup Lokasi Ritual (Church)

### 8.1 Tandai Church sebagai Ritual Room
1. Di dalam Church, buat GameObject kosong: "RitualPoint"
2. Posisikan di tengah ruangan church
3. Ini akan dipakai RitualManager.cs (Week 4 script)

### 8.2 Placeholder Tungku/Incinerator
Sementara tunggu script Week 4:
1. Buat Cube 3D → resize jadi berbentuk kotak besar (0.8 x 1.2 x 0.8)
2. Rename "Furnace_Placeholder"
3. Letakkan di dalam Church
4. Tambah tag "Furnace"

---

## BAGIAN 9 — Optimasi Performa

### 9.1 Occlusion Culling
```
Window → Rendering → Occlusion Culling → tab Bake → klik Bake
```
Ini mencegah rendering objek yang tidak terlihat kamera.

### 9.2 Camera Far Clip
1. Klik Main Camera → Inspector
2. Far: ubah dari 1000 → 150

### 9.3 Quality Settings
```
Edit → Project Settings → Quality
```
Pilih level "Medium" untuk development. Naikkan ke "High" saat build final.

---

## BAGIAN 10 — CHECKLIST TEST AKHIR

Jalankan Play Mode dan verifikasi satu per satu:

### Player
- [ ] WASD bisa gerak ke 4 arah
- [ ] Mouse bisa putar kamera kiri-kanan 360 derajat bebas
- [ ] Mouse bisa putar kamera atas-bawah (terbatas -80 s/d 80)
- [ ] Shift = lari, stamina berkurang, otomatis jalan saat stamina habis
- [ ] Ctrl = jongkok (toggle), badan mengecil
- [ ] Space = lompat, tidak bisa lompat saat jongkok
- [ ] Esc = pause / resume

### Interaksi
- [ ] Dekati boneka → muncul prompt "Tekan E"
- [ ] Tekan E → boneka hilang dari scene
- [ ] Console menampilkan log pickup (cek Window → General → Console)

### Ghost AI
- [ ] Ghost bergerak mengikuti waypoints secara berurutan
- [ ] Ghost mendeteksi player saat dalam jarak dan sudut pandang
- [ ] Saat terdeteksi, Ghost mengejar player
- [ ] Jika player memberi boneka baik (E saat dikejar) → Ghost berhenti

### Environment
- [ ] Fog terlihat di kejauhan
- [ ] Tidak ada objek melayang atau terbenam ke tanah
- [ ] NavMesh ter-bake (area biru muda terlihat di Scene view saat Play)

---

## RINGKASAN KONTROL FINAL

| Tombol | Fungsi |
|---|---|
| WASD | Gerak |
| Mouse | Putar kamera (360 horizontal, ±80 vertikal) |
| Shift | Lari |
| Ctrl | Jongkok (toggle) |
| Space | Lompat |
| E | Interaksi / Beri boneka ke Ghost |
| Esc | Pause |

---

*Manual dibuat untuk Whispering Dolls: Ritual Terakhir — Week 4 Development*
