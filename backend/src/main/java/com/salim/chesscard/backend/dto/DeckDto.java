package com.salim.chesscard.dto;

import lombok.Data;

@Data
public class DeckDto {
    private Long id;
    private Long userId;
    private String cardList;
    private String deckName;
}