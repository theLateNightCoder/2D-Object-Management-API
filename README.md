# 2D Environment & Object Management API

## Overview
This project is a backend API built for a Unity application.  
It allows users to create and manage 2D environments and objects within those environments.

The system is designed using a **vertical slice architecture**, where each feature is organized end-to-end (controller, service, repository, and data access).

---

## Tech Stack
- ASP.NET Core Web API
- Dapper (lightweight ORM for database access)
- Identity Core (authentication & authorization)
- HTTPS (secure communication)
- C#

---

## Architecture
This project follows a **vertical slice architecture**, meaning each feature (Environment, Object2D, etc.) contains its own complete logic instead of being separated by technical layers only.

This improves:
- Maintainability
- Scalability
- Feature independence

---

## Features

### Environment Management
- Create environments (max 5 per user)
- Get all environments for authenticated user
- Get environment by name
- Delete environment
- Validation for environment rules (name length, uniqueness, limits)

### Object Management (2D Objects)
- Add objects to an environment
- Retrieve all objects in an environment
- Get specific object by ID
- Objects include position, scale, rotation, and sorting layer

---

## Security
- Uses **ASP.NET Identity Core**
- All endpoints require authentication
- User-based data isolation (each user only accesses their own data)
- Secure communication via HTTPS

---

## Database Access
- Uses **Dapper** for fast and lightweight SQL operations
- Manual mapping for better performance and control

---

## Purpose
This backend is designed to support a **Unity-based application**, where users can visually create and manipulate 2D environments and objects in real time.

---

## Author
Individual school project – focused on backend development, API design, and Unity integration.
