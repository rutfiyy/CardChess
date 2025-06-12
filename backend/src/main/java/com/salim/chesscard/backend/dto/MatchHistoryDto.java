package com.salim.chesscard.dto;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class MatchHistoryDto {
    private Long id;
    private Long player1Id;
    private Long player2Id;
    private String result;
    private LocalDateTime playedAt;
}