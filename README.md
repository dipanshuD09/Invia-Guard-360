# Invia Guard 360


## Table of Contents

- [About](#about)
- [Demo](#demo)
- [Project Aspects](#projectaspects)
- [Features](#features)
- [License](#license)

## About

Welcome to **Invia Guard 360**! This project is a part of corporate training assignment at Invia, aiming to provide its users an enhanced analysis of their domains and subdomains with the help of a interactive dashboard.

### Demo

### Project Aspects

1. .Net core MVC
2. SQL(Stored Procedures, Functions & Scheduled Tasks)
3. Jquery AJAX (To GET POST data)
4. Bootstrap
5. Chart.js

### Features

1. Role-Based-Access: There are 2 roles Customer & Admin. Customers can view their profile and domains dashboard whereas Admins can add, edit, disable, delete user info.
2. Unit-of-Work Design Patter: Acts as an extra layer of abstraction or a Firewall between Repositories & the Database.
3. Implementation of SOLID Principles: Successfully implemented principles such as Open Close principle, Single responsibility principle, Interface Segregation Principle & Dependency Injection.
4. Loose Coupling & Seperation of Concern: Included the Repositories and Models in the project as class libraries by giving reference to achieve loose coupling.
5. Constructor Dependency Injection: Not allocating the memory of repositories repeatedly in the controller, rather using builder to allocate the memory for us thus achieving the Single responsibility principle. 
