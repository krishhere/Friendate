create database mySite;

use mySite;

create table users(id bigint AUTO_INCREMENT primary key,Name varchar(20),Email varchar(40),password varchar(12),city char(15),dob varchar(12),gender tinyint,lookFor tinyint,about varchar(300),image1 mediumblob);

create table interest(id int,reading tinyint,trekking tinyint,hiking tinyint,singing tinyint,dancing tinyint,listenMusic tinyint,gardening tinyint,cooking tinyint,
gym tinyint,foodie tinyint,travelling tinyint,art tinyint,photography tinyint,teaching tinyint,technology tinyint,coding tinyint,petCaring tinyint,outdoorGaming tinyint,indoorGaming tinyint,fashion tinyint,nightLife tinyint,daylife tinyint);

CREATE INDEX Idx_Id ON users(id);

CREATE INDEX Idx_Email ON users(Email);

DELIMITER $$;
Create PROCEDURE Pro_User_Insert(in p_Name varchar(20),in p_Email varchar(40),in p_Password varchar(12),in p_city char(15),in p_dob varchar(12),in p_gender tinyint,in p_lookFor tinyint,in p_about varchar(300),in p_image1 mediumblob)
BEGIN
insert into users(Name,Email,Password,city,dob,gender,lookFor,about,image1) values(p_Name,p_Email,p_Password,p_city,p_dob,p_gender,p_lookFor,p_about,p_image1);
END $$;

DELIMITER $$;
Create PROCEDURE Pro_UserInterests_Insert(in p_id tinyint,in p_reading tinyint,in p_trekking tinyint,in p_hiking tinyint,in p_singing tinyint,in p_dancing tinyint,in p_listenMusic tinyint,in p_gardening tinyint,in p_cooking tinyint,in p_gym tinyint,in p_foodie tinyint,in p_travelling tinyint,in p_art tinyint,in p_photography tinyint,in p_teaching tinyint,in p_technology tinyint,in p_coding tinyint,in p_petCaring tinyint,in p_outdoorGaming tinyint,in p_indoorGaming tinyint,in p_fashion tinyint,in p_nightLife tinyint,p_daylife tinyint)
BEGIN
insert into interest(id,reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife)
values(p_id,p_reading,p_trekking,p_hiking,p_singing,p_dancing,p_listenMusic,p_gardening,p_cooking,p_gym,p_foodie,p_travelling,p_art,p_photography,p_teaching,p_technology,p_coding,p_petCaring,p_outdoorGaming,p_indoorGaming,p_fashion,p_nightLife,p_daylife);
END $$;

SELECT * FROM mySite.users;

SELECT * FROM mySite.interest;

SELECT table_schema, table_name, index_name, column_name FROM information_schema.statistics WHERE non_unique = 1 AND table_schema = "mySite";

with cte as(
SELECT u.id id,Name,gender,city,lookFor,about,image1,
	IF(i.reading = 1, 'Reading', NULL) AS reading,
	IF(i.trekking = 1, 'Trekking', NULL) AS trekking,
    IF(i.hiking = 1, 'Hiking', NULL) AS hiking,
	IF(i.singing = 1, 'Singing', NULL) AS singing,
	IF(i.dancing = 1, 'Dancing', NULL) AS dancing,
    IF(i.listenMusic = 1, 'Music lover', NULL) AS listenMusic,
	IF(i.gardening = 1, 'Gardening', NULL) AS gardening,
	IF(i.cooking = 1, 'Cooking', NULL) AS cooking,
    IF(i.gym = 1, 'Gym', NULL) AS gym,
	IF(i.foodie = 1, 'Foodie', NULL) AS foodie,
	IF(i.travelling = 1, 'travelling', NULL) AS travelling,
    IF(i.art = 1, 'Artist', NULL) AS art,
	IF(i.photography = 1, 'Photography', NULL) AS photography,
	IF(i.teaching = 1, 'Teaching', NULL) AS teaching,
    IF(i.technology = 1, 'Technology', NULL) AS technology,
	IF(i.coding = 1, 'Coding', NULL) AS coding,
	IF(i.petCaring = 1, 'Pet caring', NULL) AS petCaring,
    IF(i.outdoorGaming = 1, 'Outdoor gaming', NULL) AS outdoorGaming,
	IF(i.indoorGaming = 1, 'Indoor gaming', NULL) AS indoorGaming,
	IF(i.fashion = 1, 'Fashion', NULL) AS fashion,
    IF(i.nightLife = 1, 'Night life', NULL) AS nightLife,
	IF(i.daylife = 1, 'Day life', NULL) AS daylife
FROM mySite.users u
INNER JOIN mySite.interest i ON u.id = i.id)
SELECT id,Name,gender,city,lookFor,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife) AS interests
FROM cte;