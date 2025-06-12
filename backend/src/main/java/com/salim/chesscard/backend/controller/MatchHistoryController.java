package com.salim.chesscard.controller;

import com.salim.chesscard.model.MatchHistory;
import com.salim.chesscard.model.User;
import com.salim.chesscard.repository.UserRepository;
import com.salim.chesscard.service.MatchHistoryService;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/match-history")
@CrossOrigin(origins = "*")
public class MatchHistoryController {
    private final MatchHistoryService matchHistoryService;
    private final UserRepository userRepository;

    public MatchHistoryController(MatchHistoryService matchHistoryService, UserRepository userRepository) {
        this.matchHistoryService = matchHistoryService;
        this.userRepository = userRepository;
    }

    @PostMapping
    public MatchHistory createMatchHistory(@RequestBody MatchHistory matchHistory) {
        return matchHistoryService.save(matchHistory);
    }

    @GetMapping("/user/{userId}")
    public List<MatchHistory> getHistoryForUser(@PathVariable Long userId) {
        User user = userRepository.findById(userId).orElseThrow();
        return matchHistoryService.getHistoryForUser(user);
    }
}