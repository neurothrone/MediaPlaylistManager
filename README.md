# Media Playlist Manager

## Overview

Media Playlist Manager is a cross-platform application designed for managing and playing audio playlists on Windows, macOS, iOS, and Android. It uses a 5-tier layer architecture to ensure modularity and maintainability.

## Architecture

The project is structured into the following layers:

- **Presentation Layer**:
  - User Interface built with MAUI.
- **Service Layer**:
  - Manages communication between the UI and the business logic.
- **DTO (Data Transfer Object) Layer**:
  - Defines the objects used for data transfer between layers.
- **Business Layer**:
  - Contains the core business logic for audio playback and playlist management.
- **Data Access Layer**:
  - Handles data storage and retrieval.

## Technologies

- **MAUI**
- **.NET 8.0**
- **C# 12.0**