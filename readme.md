
# Space Mission

Space... the final frontier. You the captain and sole crew member of the U.S.S. Andr II on a mission to survey a planet to determine if it can
sustain human life. You’ve been tasked with capturing sensor data while orbiting the planet and relaying it back to Earth. The Earth Space
Agency wanted to make sure you did not become bored on your trip, so they also tasked you to write all the code to support this mission.

## Features

1. **API Development**: Use .Net Core to develop the API with endpoints for earning points.
2. **Logging**: Integrate Serilog for logging.
3. **Database**: Use EFCore for database access with migrations.
4. **Validation**: Use FluentValidation for validating incoming requests.
5. **Containerization**: Containerize the application using Docker.

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

- [Docker](https://www.docker.com/get-started)

### Applications

# SpaceMissionMQTTService

 is the MQTT broker service responsible for receiving and processing temperature data from the U.S.S. Andr II. This service is designed as a background worker that runs continuously to ensure real-time data transmission and handling.

## Features

- **MQTT Broker**: Acts as an intermediary to facilitate the communication between the spaceship and the Earth Space Agency.
- **Service Worker**: Operates as a background service to ensure uninterrupted data processing.
- **Docker Support**: Easily deployable using Docker for consistency and scalability.
- **Keep Data In a Linkedlist**: when the connection lost it copies data in a linkedlist and send data to earth when see broker

## Running the Service

To start the **SpaceMissionMQTTService**, follow these steps:

1. Ensure Docker is installed on your machine.
2. Navigate to the root directory of the project.
3. Execute the following command:
    docker-compose up --build

This command will build the Docker image and start the service, allowing it to begin processing incoming temperature data.

# SpaceMissionPublisher

- **Message Publishing**: Publishes temperature readings and other sensor data at specified intervals.
- **Configurable**: Easily configurable using the `config.txt` file for customized operation.
- **Service Worker**: Operates as a background service to ensure continuous data transmission.

## Configuration

To set up the **SpaceMissionPublisher**, you need to configure the `config.txt` file located in the root directory of the project. The configuration file should include settings such as the publishing interval and MQTT broker details.

## Running the Service

To start the **SpaceMissionPublisher**, follow these steps:

1. Ensure Docker is installed on your machine.
2. Navigate to the root directory of the project.
3. Ensure the `config.txt` file is properly set up.
4. Execute the following command:

    docker-compose up --build

### SpaceMissionAPI

**SpaceMissionAPI** is the core of the project's backend, built using the MVC framework. It provides endpoints for saving and retrieving temperature data.

**SpaceMissionModels** contains the data models used across the project. These models define the structure of the data being captured and processed.

**SpaceMissionServices** includes the business logic and services that handle data processing, storage, and retrieval. This layer ensures the separation of concerns and modularity.

**SpaceMissionShared** houses common variables and functions shared across different applications in the project. This module promotes code reuse and maintainability.

# SpaceMissionExcel

**SpaceMissionExcel** is a service worker responsible for generating Excel files containing temperature data retrieved from the database. This service can be configured using a `config.txt` file located in the root folder.

## Features

- **Excel File Generation**: Creates Excel files with temperature data retrieved from the database.
- **Configurable**: Setup and configuration are managed through the `config.txt` file.
- **Service Worker**: Runs as a background service.

## Configuration

To set up the **SpaceMissionExcel**, configure the `config.txt` file located in the root directory of the project. The configuration file should include settings such as database connection details and file generation parameters.

## Running the Service

To start the **SpaceMissionExcel**, follow these steps:

1. Ensure Docker is installed on your machine.
2. Navigate to the root directory of the project.
3. Ensure the `config.txt` file is properly configured.
4. Execute the following command:

    docker-compose up --build
