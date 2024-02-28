-- Überprüfen, ob die Datenbank existiert, und falls nicht, erstellen
IF NOT EXISTS (
    SELECT name 
    FROM sys.databases 
    WHERE name = N'BaselCoinDB'
)
BEGIN
    CREATE DATABASE BaselCoinDB;
END
GO

-- Verbindung zur Datenbank herstellen
USE BaselCoinDB;
GO

-- Tabellen, falls vorhanden, löschen (Reihenfolge beachten wegen Abhängigkeiten)
IF OBJECT_ID('dbo.user_sessions', 'U') IS NOT NULL
    DROP TABLE dbo.user_sessions;
IF OBJECT_ID('dbo.app_logs', 'U') IS NOT NULL
    DROP TABLE dbo.app_logs;
IF OBJECT_ID('dbo.users', 'U') IS NOT NULL
    DROP TABLE dbo.users;
GO

-- Benutzertabelle
CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(255) NOT NULL UNIQUE,
    password_hash CHAR(60) NOT NULL,
    role VARCHAR(10) NOT NULL CHECK (role IN ('admin', 'user')),
    balance DECIMAL(10, 2) NOT NULL DEFAULT 0.00
);
GO

-- Log-Tabelle für Applikationsereignisse
CREATE TABLE app_logs (
    id INT IDENTITY(1,1) PRIMARY KEY,
    event_date DATETIME DEFAULT GETDATE(),
    event_type VARCHAR(50) NOT NULL,
    user_id INT,
    action VARCHAR(255) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id)
);
GO

-- Session-Tabelle
CREATE TABLE user_sessions (
    session_id VARCHAR(255) PRIMARY KEY,
    user_id INT NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    last_accessed DATETIME DEFAULT GETDATE(),
    expires_at DATETIME NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id)
);
GO




INSERT INTO users (username, password_hash, role, balance) 
VALUES 
('Benutzername1', 'PasswortHash1', 'admin', 100), 
('Benutzername2', 'PasswortHash2', 'user', 50),
('Benutzername3', 'PasswortHash3', 'user', 75);
