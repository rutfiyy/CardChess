package com.salim.chesscard.controller;

import com.salim.chesscard.model.Deck;
import com.salim.chesscard.model.User;
import com.salim.chesscard.repository.UserRepository;
import com.salim.chesscard.service.DeckService;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/decks")
@CrossOrigin(origins = "*")
public class DeckController {
    private final DeckService deckService;
    private final UserRepository userRepository;

    public DeckController(DeckService deckService, UserRepository userRepository) {
        this.deckService = deckService;
        this.userRepository = userRepository;
    }

    @PostMapping
    public Deck createDeck(@RequestBody Deck deck) {
        return deckService.save(deck);
    }

    @GetMapping("/user/{userId}")
    public List<Deck> getDecksByUser(@PathVariable Long userId) {
        User user = userRepository.findById(userId).orElseThrow();
        return deckService.getDecksByUser(user);
    }

    @DeleteMapping("/{id}")
    public void deleteDeck(@PathVariable Long id) {
        deckService.deleteDeck(id);
    }
}