GO
USE [TripAgency]

GO
INSERT INTO [dbo].[Employee] ([dbo].[Employee].[Surname], [dbo].[Employee].[Name], 
							  [dbo].[Employee].[Patronymic], [dbo].[Employee].[Phone])
	VALUES ('test1', 'test1', 'test1', '+375-29-556-06-66'),
		   ('test2', 'test2', 'test2', '+375-29-556-06-66'),
		   ('test3', 'test3', 'test3', '+375-29-556-06-66'),
		   ('test4', 'test4', 'test4', '+375-29-556-06-66'),
		   ('test5', 'test5', 'test5', '+375-29-556-06-66')

GO
INSERT INTO [dbo].[Client] ([dbo].[Client].[Surname], [dbo].[Client].[Name], 
							  [dbo].[Client].[Patronymic], [dbo].[Client].[Passport], [dbo].[Client].[Phone])
	VALUES ('test1', 'test1', 'test1', 'test1', '+375-29-556-06-66'),
		   ('test2', 'test2', 'test2', 'test2',  '+375-29-556-06-66'),
		   ('test3', 'test3', 'test3', 'test3', '+375-29-556-06-66'),
		   ('test4', 'test4', 'test4', 'test4', '+375-29-556-06-66'),
		   ('test5', 'test5', 'test5', 'test5', '+375-29-556-06-66')

GO
INSERT INTO [dbo].[Trip] ([dbo].[Trip].[Name], [dbo].[Trip].[Price], 
							  [dbo].[Trip].[DepartureDate], [dbo].[Trip].[CountNights], 
							  [dbo].[Trip].[CountKids], [dbo].[Trip].[Country], [dbo].[Trip].[TypeTrip], [dbo].[Trip].[Nutrition])
	VALUES ('test1', 1000, '2021-09-01', 11, 11, 'test1', 'test1', 'test1'),
		   ('test2', 2000, '2021-09-01', 22,  22, 'test2', 'test3', 'test2'),
		   ('test3', 3000, '2021-09-01', 33, 33, 'test3', 'test2', 'test1'),
		   ('test4', 4000, '2021-09-01', 44, 44, 'test4', 'test1', 'test2'),
		   ('test5', 5000, '2021-09-01', 55, 55, 'test5', 'test2', 'test1')

GO
INSERT INTO [dbo].[Sale] ([dbo].[Sale].[Data], [dbo].[Sale].[Count], 
							[dbo].[Sale].[ClientId], [dbo].[Sale].[EmployeeId], [dbo].[Sale].[TripId])
		VALUES ('2021-09-01', 5, 1, 1, 1), 
			   ('2021-09-01', 6, 2, 2, 2), 
			   ('2021-09-01', 7, 3, 3, 3), 
			   ('2021-09-01', 8, 4, 4, 4), 
			   ('2021-09-01', 9, 5, 5, 5)
