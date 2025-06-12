package com.salim.chesscard.repository;

import com.salim.chesscard.model.MatchHistory;
import com.salim.chesscard.model.User;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface MatchHistoryRepository extends JpaRepository<MatchHistory, Long> {
    List<MatchHistory> findByPlayer1OrPlayer2(User player1, User player2);
}