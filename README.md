# Chess Card

## Deskripsi
Proyek ini adalah game catur berbasis kartu dengan fitur multiplayer online (Photon) dan histori pertandingan. Backend menggunakan Spring Boot (Java) dan frontend/game client menggunakan Unity.

---

## Fitur

- **Registrasi & Login** (Spring Boot REST API)
- **Multiplayer Chess** (Photon Unity Networking)
- **Card System** (kartu dapat digunakan untuk mengubah jalannya permainan catur)

---

## Cara Menjalankan

### 1. Backend (Spring Boot)
- Pastikan Java & Maven sudah terinstall.
- Buat database MySQL/MariaDB, import struktur dari `dump.sql` (lihat di bawah).
- Edit `src/main/resources/application.properties` untuk konfigurasi database.
- Jalankan:
    ```
    cd backend
    ./mvnw spring-boot:run
    ```
- API akan berjalan di `http://localhost:8080/`

### 2. Game (Unity)
- Buka folder `Game` dengan Unity Editor (versi 2022.3.43f1 atau sesuai ProjectVersion.txt).
- Pastikan Photon sudah di-setup (AppId, region, dsb).
- Build & Run, atau tekan Play di Editor.

---

## API Endpoint Penting

- `POST /api/auth/register` — Registrasi user baru
- `POST /api/auth/login` — Login user
- `GET /api/decks/user/{userId}` — Ambil semua deck milik user
- `POST /api/decks` — Simpan deck baru
- `GET /api/match-history/user/{userId}` — Ambil histori pertandingan user
- `POST /api/match-history` — Simpan hasil pertandingan

---

## Struktur Database (MySQL/MariaDB)

Lihat file `dump.sql` berikut untuk struktur tabel:

```sql
CREATE TABLE account (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE deck (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    user_id BIGINT NOT NULL,
    card_list VARCHAR(1000) NOT NULL,
    deck_name VARCHAR(255),
    CONSTRAINT fk_deck_user FOREIGN KEY (user_id) REFERENCES account(id) ON DELETE CASCADE
);

CREATE TABLE match_history (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    player1_id BIGINT NOT NULL,
    player2_id BIGINT NOT NULL,
    result VARCHAR(50) NOT NULL,
    played_at DATETIME,
    CONSTRAINT fk_matchhistory_player1 FOREIGN KEY (player1_id) REFERENCES account(id) ON DELETE CASCADE,
    CONSTRAINT fk_matchhistory_player2 FOREIGN KEY (player2_id) REFERENCES account(id) ON DELETE CASCADE
);
```

---

## Dokumentasi
### Main Menu
![Screenshot 2025-06-12 231224](https://hackmd.io/_uploads/B1DTpuumee.png)

### Lobby
![Screenshot 2025-06-12 231254](https://hackmd.io/_uploads/SJv66uu7xx.png)

### Game
![Screenshot 2025-06-12 231426](https://hackmd.io/_uploads/BywTaOd7ge.png)

## Diagram
### Entity Relationship Diagram (ERD)
![image](https://hackmd.io/_uploads/rJJWf_OQgl.png)

### Use Case Diagram
![image](https://hackmd.io/_uploads/Sk1F8u_Qle.png)

### Class Diagram
#### Backend (Spring Boot, Java)
![Screenshot 2025-06-12 225001](https://hackmd.io/_uploads/Sk82wOdmll.png)

#### FrontEnd (C#)
![image](https://hackmd.io/_uploads/H18OP__7xl.png)
