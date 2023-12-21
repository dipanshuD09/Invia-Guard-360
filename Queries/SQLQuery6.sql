INSERT INTO Plans (Id,Plan_Name, Plan_Validity, Plan_Amount, Max_Rounds)
VALUES (1,'Basic', 90, 19.99, 1),
       (2,'Standard', 180, 49.99, 3),
       (3,'Premium', 365, 99.99, 6);


INSERT INTO Customers (Email, First_Name, Last_Name, City, Country, Address, Current_Plan, TransactionId, PlanId)
VALUES ('customer1@email.com', 'John', 'Smith', 'New York', 'USA', '123 Main St, Apt 101', 1, 1, 1),
       ('customer2@email.com', 'Alice', 'Johnson', 'Los Angeles', 'USA', '456 Elm St, Suite 201', 2, 2, 2),
       ('customer3@email.com', 'Bob', 'Lee', 'London', 'UK', '789 Oak St, Unit 301', 3, 3, 3);

INSERT INTO Transactions (Transaction_Id, Plan_Start, Plan_End, Current_Round, IsLatest, CustomerId, PlanId)
VALUES (101, '2023-01-15 00:00:00', '2023-04-15 00:00:00', 1, 1, 1, 1),
       (102, '2023-03-20 00:00:00', '2023-06-20 00:00:00', 2, 1, 2, 2),
       (103, '2023-02-10 00:00:00', '2023-05-10 00:00:00', 3, 1, 3, 3);


INSERT INTO Users (Username, Password, IsCustomer, IsAdmin, IsEmployee, CustomerId, IsActive)
VALUES ('munna', 'admin123', 0, 1, 1,0, 1),
       ('neeraj', 'admin123', 0, 1, 1, 0, 1)


	   -- Inserting entries into the Enquires table
INSERT INTO Enquires (Enquiry_Date, isResolved, Message, CustomerId, PlanId)
VALUES ('2023-10-20 10:00:00', 0, 'I have a question about the Basic Plan', 1, 1),
       ('2023-10-21 14:30:00', 0, 'I need information about the Premium Plan', 2, 3),
       ('2023-10-22 12:15:00', 1, 'I am satisfied with the Standard Plan', 3, 2),
	   ('2023-10-20 10:00:00', 1, 'I need information about your Standard Plan', 2, 2);





	   
select d.Customer_Id, t.Current_Round,d.IpAddress,d.Name,d.Title,p.Plan_Name from Domains d
join Transactions t on d.Customer_Id = t.CustomerId
join Plans p on t.PlanId = p.Id
where d.Customer_Id = 35 and t.IsLatest = 1


select * from Domains d
join Transactions t on t.CustomerId = d.Customer_Id
where d.Customer_Id = 35 and t.IsLatest = 1 and d.Name like '%domain%'
