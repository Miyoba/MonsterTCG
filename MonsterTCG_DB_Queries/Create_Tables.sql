CREATE TABLE credentials (
username VARCHAR(255) PRIMARY KEY, 
password VARCHAR(255), 
token VARCHAR(255)
);

CREATE TABLE cards (
id VARCHAR(255) PRIMARY KEY, 
name VARCHAR(255),
typ VARCHAR(255), 
element VARCHAR(255),
damage INT
);

CREATE TABLE user_cards (
id serial PRIMARY KEY,
username VARCHAR(255),
card_id VARCHAR(255),
CONSTRAINT fk_user_cards
      FOREIGN KEY(username) 
	  REFERENCES credentials(username),
	  FOREIGN KEY(card_id) 
	  REFERENCES cards(id)
);

