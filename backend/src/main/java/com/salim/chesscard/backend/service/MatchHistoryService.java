package com.salim.chesscard.service;

import com.salim.chesscard.model.MatchHistory;
import com.salim.chesscard.model.User;
import com.salim.chesscard.repository.MatchHistoryRepository;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class MatchHistoryService {
    private final MatchHistoryRepository matchHistoryRepository;

    public MatchHistoryService(MatchHistoryRepository matchHistoryRepository) {
        this.matchHistoryRepository = matchHistoryRepository;
    }

    public MatchHistory save(MatchHistory matchHistory) {
        return matchHistoryRepository.save(matchHistory);
    }

    public List<MatchHistory> getHistoryForUser(User user) {
        return matchHistoryRepository.findByPlayer1OrPlayer2(user, user);
    }
}