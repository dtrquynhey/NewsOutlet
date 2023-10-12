# News Outlet Software

Winter 2023 LaSalle College - Data Structures - Final Project


News Outlet Software is a data-driven application designed for processing news data in JSON format. This software allows users to interact with a console-based interface for efficient news content management and user-friendly news consumption. It employs diverse data structures for streamlined data management.


## Table of Contents
- [Key Features](#key-features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
  - [Importing Mock Data](#importing-mock-data)
  - [Running the Application](#running-the-application)
  - [Commands](#commands)
- [Project Structure](#project-structure)
- [License](#license)
- [Acknowledgements](#acknowledgements)


## Key Features
<a name="key-features"></a>
- Advanced search functionality for news content.
- Efficient memory management for seamless operation.
- Diverse data structures for effective data organization.
- User-friendly news access and engagement.
- Seamless data storage and retrieval from JSON files.


## Getting Started
<a name="getting-started"></a>

### Prerequisites
<a name="prerequisites"></a>
- C# Programming Language
- JSON.NET (for JSON data handling)
  

### Installation
<a name="installation"></a>
1. Clone this repository to your local machine.
```
git clone <repository-url>
```
2. Ensure you have the necessary prerequisites installed.


## Usage
<a name="usage"></a>

### Importing Mock Data
<a name="importing-mock-data"></a>
To use the application with sample data, follow these steps:
1. Download the `.JSON` data file you want to use.
2. Place the `.JSON` data file in the `bin/Debug` directory of the project.

### Running the Application
<a name="running-the-application"></a>
1. Open your C# Integrated Development Environment (IDE) like Visual Studio.
2. Load the project within your IDE.
3. Compile and run the application using your IDE's built-in tools.
The application expects news data to be provided in JSON format. It will process this data and allow users to search and interact with news content.

### Commands
<a name="commands"></a>
Once the application is running, you can use the following commands in the command-line interface:
- `Show Recent News`: Displays recent news articles.
- `Show Trending News`: Displays trending news articles.
- `Select News`: Select a news article to read.
- `Go Back`: Return to the previously selected news article.
- `Display All News`: Shows all available news articles.
- `Set System Time`: Set the system time to simulate viewing articles published in the last 24 hours.
- `Exit`: Exit the application.
- 
When prompted, you can filter news articles by keywords. Separate multiple keywords with commas.


## Project Structure
<a name="project-structure"></a>
The core directories within this project include:
- `basisClasses/`: Directory containing core classes
  - `DataManagement.cs`: News data management class
  - `FilesProcess.cs`: File processing and data structures class
  - `News.cs`: News class
- `Program.cs`: Main program file


## License
<a name="license"></a>
This project is licensed under the MIT License - see the LICENSE file for details.


## Acknowledgments
<a name="acknowledgements"></a>
Special thanks to @SizarStass for their valuable contributions to this project.

