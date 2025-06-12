package com.salim.chesscard.service;

import com.salim.chesscard.dto.RegisterRequest;
import com.salim.chesscard.model.User;
import com.salim.chesscard.repository.UserRepository;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class UserService {
    private final UserRepository userRepository;
    private final BCryptPasswordEncoder passwordEncoder = new BCryptPasswordEncoder();

    public UserService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    public boolean register(RegisterRequest request) {
        if (userRepository.findByUsername(request.getUsername()).isPresent()) return false;

        User user = new User();
        user.setUsername(request.getUsername());
        user.setPassword(passwordEncoder.encode(request.getPassword()));
        userRepository.save(user);
        return true;
    }

    public boolean login(RegisterRequest request) {
        Optional<User> user = userRepository.findByUsername(request.getUsername());
        return user.isPresent() && passwordEncoder.matches(request.getPassword(), user.get().getPassword());
    }
}
