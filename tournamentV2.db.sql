BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Game";
CREATE TABLE "Game" (
	"Id"	INTEGER,
	"Phase_id"	INTEGER,
	"Tournament_id"	INTEGER,
	"Group_id"	INTEGER,
	"Order"	INTEGER,
	"Date"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT),
	CONSTRAINT "Game_Group_FK" FOREIGN KEY("Group_id") REFERENCES "Group"("Id"),
	CONSTRAINT "Game_Phase_FK" FOREIGN KEY("Phase_id") REFERENCES "Phase"("Id"),
	CONSTRAINT "Game_Tournament_FK" FOREIGN KEY("Tournament_id") REFERENCES "Tournament"("Id")
);
DROP TABLE IF EXISTS "Group";
CREATE TABLE "Group" (
	"Id"	INTEGER,
	"Name"	TEXT,
	"Tournament_id"	INTEGER,
	PRIMARY KEY("Id" AUTOINCREMENT),
	CONSTRAINT "Group_Tournament_Fk" FOREIGN KEY("Tournament_id") REFERENCES "Tournament"("Id")
);
DROP TABLE IF EXISTS "Phase";
CREATE TABLE "Phase" (
	"Id"	INTEGER,
	"Name"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
DROP TABLE IF EXISTS "Player";
CREATE TABLE "Player" (
	"Id"	INTEGER,
	"Firstname"	TEXT NOT NULL,
	"Lastname"	TEXT NOT NULL,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
DROP TABLE IF EXISTS "PlayerGroup";
CREATE TABLE "PlayerGroup" (
	"Player_id"	INTEGER,
	"Group_id"	INTEGER,
	PRIMARY KEY("Player_id","Group_id"),
	CONSTRAINT "PG_Group_FK" FOREIGN KEY("Group_id") REFERENCES "Group"("Id"),
	CONSTRAINT "PG_Player_FK" FOREIGN KEY("Player_id") REFERENCES "Player"("Id")
);
DROP TABLE IF EXISTS "PlayerSet";
CREATE TABLE "PlayerSet" (
	"Player_id"	INTEGER,
	"Set_id"	INTEGER,
	"Points"	INTEGER,
	PRIMARY KEY("Player_id","Set_id"),
	CONSTRAINT "PS_Player_FK" FOREIGN KEY("Player_id") REFERENCES "Player"("Id"),
	CONSTRAINT "PS_Set_FK" FOREIGN KEY("Set_id") REFERENCES "Set"("Id")
);
DROP TABLE IF EXISTS "PlayerTournament";
CREATE TABLE "PlayerTournament" (
	"Player_id"	INTEGER,
	"Tournament_id"	INTEGER,
	PRIMARY KEY("Player_id","Tournament_id"),
	CONSTRAINT "PT_Player_FK" FOREIGN KEY("Player_id") REFERENCES "Player"("Id"),
	CONSTRAINT "PT_Tournament_FK" FOREIGN KEY("Tournament_id") REFERENCES "Tournament"("Id")
);
DROP TABLE IF EXISTS "Set";
CREATE TABLE "Set" (
	"Id"	INTEGER,
	"Setnumber"	INTEGER,
	"Game_id"	INTEGER,
	PRIMARY KEY("Id" AUTOINCREMENT),
	CONSTRAINT "Set_Game_FK" FOREIGN KEY("Game_id") REFERENCES "Game"("Id")
);
DROP TABLE IF EXISTS "Tournament";
CREATE TABLE "Tournament" (
	"Id"	INTEGER,
	"Name"	TEXT,
	"Phase_id"	INTEGER,
	"Created"	TEXT,
	"Finished"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT),
	CONSTRAINT "Tournament_Phase_FK" FOREIGN KEY("Phase_id") REFERENCES "Phase"("Id")
);
COMMIT;
