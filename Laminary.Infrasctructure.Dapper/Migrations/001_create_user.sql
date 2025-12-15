CREATE TABLE user(
    id UUID PRIMARY KEY,
    name VARCHAR(80) NOT NULL,
    register_date TIMESTAMP,
    password VARCHAR(120)
);