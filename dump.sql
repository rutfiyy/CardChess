-- Tabel User (account)
CREATE TABLE account (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);

-- Tabel Deck
CREATE TABLE deck (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    user_id BIGINT NOT NULL,
    card_list VARCHAR(1000) NOT NULL,
    deck_name VARCHAR(255),
    CONSTRAINT fk_deck_user FOREIGN KEY (user_id) REFERENCES account(id) ON DELETE CASCADE
);

-- Tabel Match History
CREATE TABLE match_history (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    player1_id BIGINT NOT NULL,
    player2_id BIGINT NOT NULL,
    result VARCHAR(50) NOT NULL,
    played_at DATETIME,
    CONSTRAINT fk_matchhistory_player1 FOREIGN KEY (player1_id) REFERENCES account(id) ON DELETE CASCADE,
    CONSTRAINT fk_matchhistory_player2 FOREIGN KEY (player2_id) REFERENCES account(id) ON DELETE CASCADE
);

-- Contoh data deck (opsional)
INSERT INTO deck (user_id, card_list, deck_name) VALUES
(1, '1,2,3,4,5', 'Starter Deck'),
(2, '2,3,4,5,6', 'Aggro Deck');

-- Contoh data match_history (opsional)
INSERT INTO match_history (player1_id, player2_id, result, played_at) VALUES
(1, 2, 'player1_win', NOW()),
(2, 1, 'draw', NOW());