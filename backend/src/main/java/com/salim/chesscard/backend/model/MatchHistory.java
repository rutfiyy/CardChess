package com.salim.chesscard.model;

import jakarta.persistence.*;
import lombok.*;
import java.time.LocalDateTime;

@Entity
@Table(name = "match_history")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class MatchHistory {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "player1_id", nullable = false)
    private User player1;

    @ManyToOne
    @JoinColumn(name = "player2_id", nullable = false)
    private User player2;

    @Column(nullable = false)
    private String result; // "player1_win", "player2_win", "draw"

    private LocalDateTime playedAt;
}