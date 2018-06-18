# SonarWiper
You can opt-out collecting usage statistics of every Adobe product (ie. Adobe Photoshop) with SonarWiper.

## What we know (about Sonar folder):

- It is located at path `%AppData%\Adobe\Sonar`.
- Sonar folder contains XML file for each installed Adobe product that is runned at least once.
- Inside each XML file is stored timestamp when the first start of application was made by user and frequency of use (every each start and end of the application + number of sessions performed in application).

## How to opt-out

- [ ] **1a)** Download the latest relese.
- [ ] **1b)** Clone this repo and compile C# code (using Visual Studio or alternative .NET IDE or using C# compiler).
- [ ] **2)** Run downloaded/compiled executable called `sonarwiper.exe` with `/stay` argument.
- [ ] **3)** Now SonarWiper is watching Sonar folder and each time statistics are created and written, the program will delete Sonar folder immediately (after short 50 ms period of inactivity).

*You can also configure a sheduled task that run SonarWiper with `/stay` argument when user logs in.*
