create database BCStudents;
use BCStudents;

create table Student (
	Student_Number varchar(255) primary key,
	Student_Name_and_Surname varchar(255),
	Student_Image varchar(1023),
	DOB Date,
	Gender varchar(31),
	Phone varchar(15),
	Address varchar(255),
);

create table Module (
	Module_Code varchar(255) primary key,
	Module_Name varchar(255),
	Module_Description varchar(255)
);

create table StudentModule (
	Student_Number varchar(255) foreign key references Student(Student_Number),
	Module_Code varchar(255) foreign key references Module(Module_Code),
	primary key (Student_Number, Module_Code)
);

create table ModuleOnlineResource (
	Module_Code varchar(255) foreign key references Module(Module_Code),
	ResourceURL varchar(511),
	primary key (Module_Code, ResourceURL)
);

insert into Student 
(Student_Number,	Student_Name_and_Surname,	Student_Image,		DOB,			Gender, Phone,				Address) values
('BCS000001',		'Jack Jackson',				'BCS000001.jpg',	'1919/09/19',	'F',	'098 765 3124',		'THERE'),
('BCS000002',		'Able Ableson',				'BCS000002.jpg',	'1919/09/18',	'F',	'+27 78 987 2341',	'HERE'),
('BCS000003',		'Cameron Cameronson',		'BCS000003.jpg',	'1919/09/17',	'M',	'+1 78 0982 019',	'VOVER THAT WAY'),
('BCS000004',		'Dave Daveson',				'BCS000004.jpg',	'1919/09/16',	'F',	'0981231234',		'WAYYYY OVER THEIIIR');

insert into Module
(Module_Code,	Module_Name,			Module_Description) values
('PRG282',		'Programming part 2',	'You program stuff in c# using windows forms (for some modern reason'),
('WPG181',		'Web Programming',		'you make a websight. no js though thats too complicated'),
('ASS001',		'Marture i know',		'Im not stressed! YOu are calm down :('),
('POO123',		'Sample Data only?',	'so youve been reading sameple data have you? Why?');

insert into ModuleOnlineResource
(Module_Code, ResourceURL) values	
('PRG282', 'https://www.youtube.com/watch?v=dQw4w9WgXcQ'),
('WPG181', 'https://www.youtube.com/watch?v=dQw4w9WgXcQ'),
('POO123', 'https://www.google.com/search?q=When+Do+You+Read+Too+Much+Into+Things'),
('POO123', 'https://www.youtube.com/watch?v=dQw4w9WgXcQ'),

('PRG282', 'https://www.sanfransentinel.com/im-good-thanks.html'),
('POO123', 'https://www.google.com/search?q=When+Do+You+Read+Too+Much+Into+Things+Also');

insert into StudentModule
(Student_Number,	Module_Code) values
('BCS000001',		'PRG282'),
('BCS000002',		'WPG181'),
('BCS000003',		'POO123'),
('BCS000004',		'POO123'),

('BCS000001',		'WPG181'),
('BCS000002',		'POO123'),
('BCS000003',		'WPG181'),
('BCS000004',		'PRG282');