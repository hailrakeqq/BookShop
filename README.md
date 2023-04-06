# BookShop

Requirement for build:

- `.NET 7.0`
- `NodeJS`
- `MongoDB`

## :floppy_disk: Installation

*The following commands should be executed in a CMD, Bash or Powershell window. To do this, go to a folder on your computer, click in the folder path at the top and type CMD, then press enter.*

1. Clone the repository:
For this step you need Git installed, but you can just download the zip file instead by clicking the button at the top of this page ☝️

  `git clone https://github.com/hailrakeqq/BookShop.git`


2. Navigate to the project directory:
*(Type this into your CMD window, you're aiming to navigate the CMD window to the repository you just downloaded)*

  `cd 'BookShop'`

3. Build with docker compose;
Run: 

  `docker compose -f "docker-compose.yml" up -d --build`

<br>If build was successfully you can see next: 

![image](https://user-images.githubusercontent.com/102614143/230348235-33f05c82-f7cf-4a47-b066-72f53eb6ca72.png)

After build you can get access to services by next URL's:

  `http://localhost:8080` - it's our frontend

  `http://localhost:8080` - it's our backend API

  `mongodb://mongo:27020` - connection string to db

You can connect to db using `MongoDB Compass` or any another DBMS for MongoDB


<br>For stop docker compose work you can use next command:

  `docker compose -f "docker-compose.yml" down`
