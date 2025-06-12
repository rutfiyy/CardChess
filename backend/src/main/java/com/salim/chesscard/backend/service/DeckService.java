package com.salim.chesscard.service;

import com.salim.chesscard.model.Deck;
import com.salim.chesscard.model.User;
import com.salim.chesscard.repository.DeckRepository;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class DeckService {
    private final DeckRepository deckRepository;

    public DeckService(DeckRepository deckRepository) {
        this.deckRepository = deckRepository;
    }

    public Deck save(Deck deck) {
        return deckRepository.save(deck);
    }

    public List<Deck> getDecksByUser(User user) {
        return deckRepository.findByUser(user);
    }

    public void deleteDeck(Long id) {
        deckRepository.deleteById(id);
    }
}