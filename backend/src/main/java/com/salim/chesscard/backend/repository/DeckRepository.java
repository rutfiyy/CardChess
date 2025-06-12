package com.salim.chesscard.repository;

import com.salim.chesscard.model.Deck;
import com.salim.chesscard.model.User;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface DeckRepository extends JpaRepository<Deck, Long> {
    List<Deck> findByUser(User user);
}