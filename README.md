# EF Core API

A hands-on ASP.NET Core Web API project built to practice Entity Framework Core, LINQ, and SQL Server through a real-world training center management scenario.

The project focuses on building RESTful API endpoints, working with relational data, querying related entities, and applying EF Core data-access patterns in a practical environment.

> **Note:** This is a learning-focused project. Authentication, authorization, and production-level security are intentionally outside the current scope.

---

## Overview

The API is built around a training center management system containing students, courses, instructors, enrollments, and student profiles.

The project was developed to gain practical experience with:

- Entity Framework Core
- ASP.NET Core Web API
- LINQ and database querying
- Relational data and entity relationships
- Asynchronous database operations
- RESTful API design
- SQL Server integration

---

## Domain Model

The system contains the following main entities:

- **Students**
- **Student Profiles**
- **Courses**
- **Instructors**
- **Enrollments**

### Relationships

- A student can have multiple enrollments.
- A course can have multiple enrollments.
- A student has a related profile.
- An instructor can teach multiple courses.
- An enrollment connects a student with a course and stores enrollment-specific information such as progress, grade, and status.

---

## Features

### Student Management

- Retrieve all students
- Retrieve graduated students
- Update student status
- Delete students
- Search and query student data
- Retrieve student enrollments and related courses

### Course & Enrollment Queries

- Retrieve students enrolled in a specific course
- Retrieve completed courses for a student
- Update enrollment status and final grade
- Calculate course statistics
- Calculate average grades
- Count students per course

### Reporting & Custom Queries

The project also includes custom queries for practicing LINQ and EF Core, such as:

- Average grade per course
- Student enrollment ordering
- Student status statistics
- Course enrollment statistics
- Student completion percentage
- Instructor-related reports
- Related-data projections

---

## EF Core Concepts Practiced

This project was primarily built to practice the following EF Core concepts:

- `DbContext` and `DbSet`
- Database-First workflow
- Scaffolded EF Core models
- Entity relationships
- Navigation properties
- Projection with `Select()`
- Filtering with `Where()`
- Sorting with `OrderBy()` / `OrderByDescending()`
- Aggregation with `Count()` / `Average()`
- Grouping with `GroupBy()`
- `Any()` / `AnyAsync()`
- Asynchronous queries
- `AsNoTracking()`
- Working with nullable values
- Querying related entities
- Mapping relational data into API responses

---

## API Endpoints

The project exposes RESTful endpoints for working with students, courses, enrollments, and reports.

Example operations include:

GET    /api/Main/Get-All-Students
GET    /api/Main/Get-Graduated-Students
GET    /api/Main/Get-Student-Enrollments/{student_id}
GET    /api/Main/Get-Students-By-Course/{course_id}
GET    /api/Main/Get-Student-Profile/{student_id}

PUT    /api/Main/Update-Student-Status/{student_id}
PUT    /api/Main/Update-Enrollment/{student_id}/{course_code}

DELETE /api/Main/Delete-Student/{student_id}

POST   /api/Main/Enroll-Student