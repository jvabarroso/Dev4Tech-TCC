<?php
class SenhasHash {
    const SALT_SIZE = 16; // 128 bits
    const HASH_SIZE = 32; // 256 bits
    const ITERATIONS = 100000;
    const ALGORITHM = 'sha1';

    public static function hashPassword($password) {
        // Gera um salt aleatório
        $salt = random_bytes(self::SALT_SIZE);
        
        // Cria o hash da senha usando PBKDF2
        $hash = hash_pbkdf2(
            self::ALGORITHM,
            $password,
            $salt,
            self::ITERATIONS,
            self::HASH_SIZE,
            true
        );
        
        // Combina salt e hash
        $hashBytes = $salt . $hash;
        
        // Converte para Base64
        return base64_encode($hashBytes);
    }

    public static function verificarSenha($password, $hashedPassword) {
        // Converte de volta para bytes
        $hashBytes = base64_decode($hashedPassword);
        
        // Extrai o salt (primeiros 16 bytes)
        $salt = substr($hashBytes, 0, self::SALT_SIZE);
        
        // Extrai o hash armazenado (próximos 32 bytes)
        $storedHash = substr($hashBytes, self::SALT_SIZE, self::HASH_SIZE);
        
        // Cria o hash da senha fornecida
        $computedHash = hash_pbkdf2(
            self::ALGORITHM,
            $password,
            $salt,
            self::ITERATIONS,
            self::HASH_SIZE,
            true
        );
        
        // Compara os hashes (comparação segura contra timing attacks)
        return hash_equals($storedHash, $computedHash);
    }
}
?>