# Rencana Pengembangan — Whispering Dolls: Ritual Terakhir

## Status Project
- Engine: Unity (PC Windows, Single Player)
- Bahasa: C#
- Timeline: 6 Minggu

---

## Arsitektur Script

```
Assets/
├── Scripts/
│   ├── Core/
│   │   └── GameManager.cs          ← singleton, state global game
│   ├── Player/
│   │   ├── PlayerController.cs     ← gerak, lari, jongkok
│   │   └── PlayerInteraction.cs    ← raycast interaksi objek
│   ├── Inventory/
│   │   ├── InventoryManager.cs     ← tambah/hapus/cek item
│   │   └── ItemData.cs             ← ScriptableObject data item
│   ├── Doll/
│   │   ├── DollData.cs             ← ScriptableObject: tipe boneka (baik/jahat)
│   │   └── DollPickup.cs           ← komponen di prefab boneka
│   ├── Enemy/
│   │   ├── GhostAI.cs              ← state machine: Patrol → Chase → Calm
│   │   └── GhostDetection.cs       ← FOV + radius dengar
│   ├── Ritual/
│   │   ├── RitualManager.cs        ← track progress ritual
│   │   └── FurnaceController.cs    ← interaksi tungku, bakar boneka
│   ├── Level/
│   │   ├── DoorController.cs       ← pintu terkunci/terbuka
│   │   └── LevelTrigger.cs         ← trigger event saat masuk area
│   ├── UI/
│   │   ├── UIManager.cs            ← tampilkan/sembunyi panel UI
│   │   └── NotificationUI.cs       ← popup teks notifikasi
│   ├── Audio/
│   │   └── AudioManager.cs         ← singleton, kelola semua audio
│   └── Ending/
│       └── EndingManager.cs        ← track keputusan → tentukan ending
├── ScriptableObjects/
│   ├── Items/                      ← data tiap item (boneka, bahan bakar)
│   └── Endings/                    ← data tiap ending
├── Prefabs/
│   ├── Player/
│   ├── Enemies/
│   ├── Items/
│   └── UI/
├── Scenes/
│   ├── MainMenu.unity
│   └── GameScene.unity
├── Materials/
├── Audio/
│   ├── Ambient/
│   ├── SFX/
│   └── Music/
└── UI/
    └── Sprites/
```

---

## Jadwal per Minggu

### MINGGU 1 — Setup & Player Movement
**Script yang dibuat Claude:**
- [ ] `GameManager.cs` — singleton, state (Playing/Paused/GameOver)
- [ ] `PlayerController.cs` — WASD/jalan/lari/jongkok, stamina lari
- [ ] `PlayerInteraction.cs` — raycast 2m ke depan, tombol E untuk interaksi

**Manual Unity (lihat MANUAL_UNITY.md → Bagian 1):**
- Buat Unity project baru
- Setup folder struktur di Assets
- Buat Scene GameScene
- Attach script ke Player GameObject
- Setup Input System / Input axis

---

### MINGGU 2 — Inventory & Sistem Interaksi
**Script yang dibuat Claude:**
- [ ] `ItemData.cs` — ScriptableObject: nama, sprite, tipe (DollGood/DollEvil/Fuel)
- [ ] `InventoryManager.cs` — List<ItemData>, max 6 slot, event saat berubah
- [ ] `DollData.cs` — ScriptableObject extend ItemData: tipe boneka
- [ ] `DollPickup.cs` — komponen pickup boneka di scene
- [ ] `FuelPickup.cs` — komponen pickup bahan bakar

**Manual Unity (lihat MANUAL_UNITY.md → Bagian 2):**
- Buat ScriptableObject untuk tiap item
- Buat prefab boneka dan bahan bakar
- Assign ScriptableObject ke prefab

---

### MINGGU 3 — Enemy AI & Sistem Boneka
**Script yang dibuat Claude:**
- [ ] `GhostAI.cs` — NavMeshAgent, state machine 3 state
- [ ] `GhostDetection.cs` — cone FOV + sphere hearing radius
- [ ] Logika: boneka baik → Ghost.Calm(duration), boneka jahat → tidak berpengaruh ke ghost

**Manual Unity (lihat MANUAL_UNITY.md → Bagian 3):**
- Bake NavMesh di scene
- Buat Ghost prefab + assign NavMeshAgent
- Buat waypoints patrol
- Set layer Player, Enemy, Item

---

### MINGGU 4 — Ritual System & Level Design
**Script yang dibuat Claude:**
- [ ] `RitualManager.cs` — track boneka jahat yang sudah dibakar, kondisi ritual selesai
- [ ] `FurnaceController.cs` — interaksi tungku, validasi: perlu fuel + boneka jahat
- [ ] `DoorController.cs` — pintu locked/unlocked, key item requirement
- [ ] `LevelTrigger.cs` — trigger cutscene/event/spawn

**Manual Unity (lihat MANUAL_UNITY.md → Bagian 4):**
- Design layout ruangan: Ruang Tamu, Kamar Tidur, Dapur, Basement
- Tempatkan tungku di Basement
- Tempatkan pintu-pintu dengan trigger

---

### MINGGU 5 — UI & Audio
**Script yang dibuat Claude:**
- [ ] `UIManager.cs` — tampilkan HUD, inventory panel, notifikasi
- [ ] `NotificationUI.cs` — coroutine: muncul 2 detik lalu fade out
- [ ] `AudioManager.cs` — singleton, PlayAmbient/PlaySFX/PlayMusic, fade in/out

**Manual Unity (lihat MANUAL_UNITY.md → Bagian 5):**
- Buat Canvas UI: HUD, inventory grid, panel notifikasi
- Import audio files (ambient, SFX, musik)
- Setup AudioMixer dengan channel Ambient/SFX/Music

---

### MINGGU 6 — Ending System, Testing & Polish
**Script yang dibuat Claude:**
- [ ] `EndingManager.cs` — counter: dollsSaved, dollsDestroyed → trigger ending
- [ ] Koneksi semua sistem
- [ ] Bug fix dari testing

**Manual Unity (lihat MANUAL_UNITY.md → Bagian 6):**
- Setup ending cutscene / screen
- Build settings untuk Windows
- Final testing

---

## Sistem Ending

| Kondisi | Ending |
|---------|--------|
| dollsSaved >= 3, dollsDestroyed == 0 | Good Ending |
| dollsDestroyed >= 3, dollsSaved == 0 | Bad Ending |
| Campuran keduanya | Neutral Ending |

---

## Dependency Antar Sistem

```
GameManager
    ↓
PlayerController → PlayerInteraction → InventoryManager
                                            ↓
                                        DollPickup / FuelPickup
                                            ↓
                                        RitualManager ← FurnaceController
                                            ↓
                                        EndingManager

GhostAI ← GhostDetection
GhostAI ← InventoryManager (cek boneka baik)

UIManager ← InventoryManager (update slot)
UIManager ← NotificationUI (trigger notif)
AudioManager (standalone singleton)
```
