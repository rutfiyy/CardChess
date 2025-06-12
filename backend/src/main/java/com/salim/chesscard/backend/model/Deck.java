package com.salim.chesscard.model;

import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "deck")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Deck {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    @Column(nullable = false, length = 1000)
    private String cardList; // e.g. comma-separated card IDs

    private String deckName;
}