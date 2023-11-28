create database mySite;

use mySite;

create table users(id bigint AUTO_INCREMENT primary key,Name varchar(20),Email varchar(40),password varchar(12),city char(15),dob varchar(12),gender tinyint,lookFor tinyint,about varchar(300),image1 mediumblob);

create table interest(id bigint,reading tinyint,trekking tinyint,hiking tinyint,singing tinyint,dancing tinyint,listenMusic tinyint,gardening tinyint,cooking tinyint,
fitness tinyint,foodie tinyint,travelling tinyint,art tinyint,photography tinyint,teaching tinyint,technology tinyint,coding tinyint,petCaring tinyint,outdoorGaming tinyint,
indoorGaming tinyint,fashion tinyint,nightLife tinyint,daylife tinyint,investment tinyint,business tinyint,movies tinyint,shopping tinyint,roadtrips tinyint,politics tinyint,chillatbar tinyint);

create table FriendRequest(profileId bigint,userId bigint);

create table DateRequest(profileId bigint,userId bigint);

CREATE INDEX Idx_Id ON users(id);

CREATE INDEX Idx_Email ON users(Name);

DELIMITER $$;
Create PROCEDURE Pro_UserEmail_Insert(in p_Name varchar(20),in p_Email varchar(40),in p_Password varchar(12))
BEGIN
insert into users(Name,Email,Password) values(p_Name,p_Email,p_Password);
END $$;

DELIMITER $$;
Create PROCEDURE Pro_User_Insert(in p_city char(15),in p_dob varchar(12),in p_gender tinyint,in p_lookFor tinyint,in p_about varchar(300),in p_image1 mediumblob,in p_id bigint)
BEGIN
update users set city=p_city,dob=p_dob,gender=p_gender,lookFor=p_lookFor,about=p_about,image1=p_image1 where id=p_id;
END $$;

DELIMITER $$;
Create PROCEDURE Pro_UserInterests_Insert(in p_reading tinyint,in p_trekking tinyint,in p_hiking tinyint,in p_singing tinyint,in p_dancing tinyint,in p_listenMusic tinyint,in p_gardening tinyint,in p_cooking tinyint,in p_fitness tinyint,in p_foodie tinyint,in p_travelling tinyint,in p_art tinyint,in p_photography tinyint,in p_teaching tinyint,in p_technology tinyint,in p_coding tinyint,in p_petCaring tinyint,in p_outdoorGaming tinyint,in p_indoorGaming tinyint,in p_fashion tinyint,in p_nightLife tinyint,in p_daylife tinyint,in p_investment tinyint,in p_business tinyint,IN p_movies TINYINT,IN p_shopping TINYINT,IN p_roadtrips TINYINT,IN p_politics TINYINT,IN p_chillatbar TINYINT,in p_id bigint)
BEGIN
update interest set reading=p_reading,trekking=p_trekking,hiking=p_hiking,singing=p_singing,dancing=p_dancing,listenMusic=p_listenMusic,gardening=p_gardening,cooking=p_cooking,fitness=p_fitness,foodie=p_foodie,travelling=p_travelling,art=p_art,photography=p_photography,teaching=p_teaching,technology=p_technology,coding=p_coding,petCaring=p_petCaring,outdoorGaming=p_outdoorGaming,indoorGaming=p_indoorGaming,fashion=p_fashion,nightLife=p_nightLife,daylife=p_daylife,investment=p_investment,business=p_business,movies=p_movies,shopping=p_shopping,roadtrips=p_roadtrips,politics=p_politics,chillatbar=p_chillatbar where id=p_id;
END $$;

DELIMITER $$;
Create PROCEDURE Pro_FriendRequests(in p_profileId bigint,in p_userId bigint)
BEGIN
insert into FriendRequest(profileId,userId) values(p_profileId,p_userId);
END $$;

DELIMITER $$;
Create PROCEDURE Pro_DateRequests(in p_profileId bigint,in p_userId bigint)
BEGIN
insert into DateRequest(profileId,userId) values(p_profileId,p_userId);
END $$;

DELIMITER $$;
Create PROCEDURE Pro_AcceptReject(in p_profileId bigint,in p_userId bigint,in P_acceptReject bigint,in P_type varchar(7))
BEGIN
if P_acceptReject=1 then
	if p_type="Date" then 
		update mySite.daterequest set accept=1 where profileid=p_profileId and userid=p_userId;
	else 
		update mySite.friendrequest set accept=1 where profileid=p_profileId and userid=p_userId;
	end if;
else
	if p_type="Date" then 
		delete from mySite.daterequest where profileid=p_profileId and userid=p_userId;
	else 
		delete from mySite.friendrequest where profileid=p_profileId and userid=p_userId;
	end if;
end if;
END $$;

DELIMITER $$;
Create PROCEDURE Pro_FriendDateRequests(in p_profileId bigint)
BEGIN
SELECT *
FROM (select * from (with cte as(SELECT u.id id,Name,IF(u.gender=1,'Male','Female') AS gender,(YEAR(curdate())-right(dob,4)) as age,city,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investing',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business 
FROM mySite.users u INNER JOIN mySite.interest i ON u.id = i.id)
SELECT id as senderId,d.userId as ReceiverId,Name,gender,age,city,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests
,case WHEN d.userid is not null THEN 'Date' ELSE 'Friend' end as RequestType,IF(d.accept=1,'Date Accepted',null)as accept FROM cte c inner join DateRequest d on c.id=d.profileId) as t1
union
select * from (with cte2 as(SELECT u.id id,Name,IF(u.gender=1,'Male','Female') AS gender,(YEAR(curdate())-right(dob,4)) as age,city,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investing',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business 
FROM mySite.users u INNER JOIN mySite.interest i ON u.id = i.id)
SELECT id as senderId,f.userid as ReceiverId,Name,gender,age,city,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests
,case WHEN f.userid is not null THEN 'Friend' ELSE 'Date' end as RequestType,IF(f.accept=1,'Friend Accepted',null)as accept FROM cte2 c2 inner join FriendRequest f on c2.id=f.profileId) as t2) AS result 
WHERE ReceiverId=p_profileId;
END $$;
-----------------------------------------------------------------------------------------------------------------
with cte as(SELECT u.id id,Name,email,IF(u.gender=1,'Male','Female') AS gender,(YEAR(curdate())-right(dob,4)) as age,city,IF(lookfor=2,'Friend or Date', IF(lookfor=1,'Date','Friend')) AS lookFor,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investing',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business FROM mySite.users u INNER JOIN mySite.interest i ON u.id = i.id) SELECT id,Name,email,gender,age,city,lookFor,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests FROM cte 
where id=121;

SELECT * FROM mySite.users order by 1 desc;

SELECT * FROM mySite.interest order by 1 desc;

SELECT * FROM mySite.friendrequest;

SELECT * FROM mySite.daterequest;

with cte as(
SELECT u.id id,Name,gender,city,lookFor,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investing',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business 
FROM mySite.users u
INNER JOIN mySite.interest i ON u.id = i.id)
SELECT id,Name,gender,city,lookFor,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests
FROM cte order by rand() limit 5;

SELECT table_schema, table_name, index_name, column_name FROM information_schema.statistics WHERE non_unique = 1 AND table_schema = "mySite";

SELECT table_schema AS "Database",ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS "SizeInMB" FROM information_schema.TABLES WHERE table_schema = "mysite" GROUP BY table_schema;

SELECT table_name AS "Table",ROUND(((data_length + index_length) / 1024 / 1024), 2) AS "SizeInMB" FROM information_schema.TABLES WHERE table_schema = "mysite" ORDER BY (data_length + index_length) DESC;