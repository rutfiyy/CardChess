package com.salim.chesscard.controller;

import com.salim.chesscard.dto.RegisterRequest;
import com.salim.chesscard.model.User;
import com.salim.chesscard.service.UserService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

@RestController
@RequestMapping("/api/auth")
@CrossOrigin(origins = "*")
public class AuthController {

    private final UserService userService;
    public AuthController(UserService userService) {
        this.userService = userService;
    }

    @PostMapping("/register")
    public ResponseEntity<?> register(@RequestBody RegisterRequest request) {
        User user = userService.registerAndReturnUser(request);
        if (user != null) {
            Map<String, Object> response = new HashMap<>();
            response.put("user_id", user.getId());
            response.put("username", user.getUsername());
            return ResponseEntity.ok(response);
        } else {
            return ResponseEntity.badRequest().body("Username already exists");
        }
    }

    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody RegisterRequest request) {
        User user = userService.loginAndReturnUser(request);
        if (user != null) {
            Map<String, Object> response = new HashMap<>();
            response.put("user_id", user.getId());
            response.put("username", user.getUsername());
            return ResponseEntity.ok(response);
        } else {
            return ResponseEntity.status(401).body("Invalid credentials");
        }
    }
}