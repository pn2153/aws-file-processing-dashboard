# AWS File Processing Dashboard

## Project Overview

AWS File Processing Dashboard is a cloud-native ASP.NET Core MVC application that demonstrates file upload, event-driven processing, database integration, containerization, and cloud deployment using AWS services.

The application allows users to upload files through a web interface. Uploaded files are stored in Amazon S3, metadata is stored in Amazon RDS MySQL, and file upload events trigger downstream processing through Amazon SQS and AWS Lambda.

---

## Architecture

User Browser
     |
     v
Elastic Beanstalk (.NET 8 MVC Application)
     |
     +-------------------+
     |                   |
     v                   v
Amazon S3           Amazon RDS MySQL
     |
     v
S3 Event Notification
     |
     v
Amazon SQS
     |
     v
AWS Lambda
     |
     v
Amazon CloudWatch Logs

---

## AWS Services Used

| Service           | Purpose                    |
| ----------------- | -------------------------- |
| Elastic Beanstalk | Application Hosting        |
| Amazon S3         | File Storage               |
| Amazon RDS MySQL  | Metadata Storage           |
| Amazon SQS        | Event Queue                |
| AWS Lambda        | File Event Processing      |
| Amazon CloudWatch | Monitoring and Logging     |
| IAM Roles         | Secure AWS Resource Access |

---

## Features

### File Upload

* Upload files using a web interface
* Store files in Amazon S3
* Generate unique timestamp-based S3 object keys

### Metadata Storage

* Store uploaded file information in Amazon RDS MySQL
* Track:

  * File Name
  * S3 Object Key
  * Upload Timestamp

### Upload Dashboard

* Display upload history
* View recently uploaded files
* Retrieve data directly from RDS

### Event-Driven Processing

* S3 Object Created event triggers
* Event sent to Amazon SQS
* Lambda function processes queue messages
* Processing activity logged in CloudWatch

### Secure Authentication

* No AWS Access Keys stored in application code
* Elastic Beanstalk EC2 Instance Profile used for AWS authentication
* Temporary IAM credentials automatically provided by AWS

---

## Technology Stack

### Backend

* ASP.NET Core MVC (.NET 8)

### Database

* Amazon RDS MySQL

### Storage

* Amazon S3

### Messaging

* Amazon SQS

### Serverless

* AWS Lambda

### Hosting

* AWS Elastic Beanstalk

### Containerization

* Docker

---

## Project Structure

AwsAssignmentDemo
|
+-- Controllers
|    +-- HomeController.cs
|    +-- UploadController.cs
|
+-- Models
|    +-- UploadedFile.cs
|
+-- Services
|    +-- S3Service.cs
|    +-- DatabaseService.cs
|
+-- Views
|    +-- Home
|    +-- Upload
|
+-- wwwroot
|
+-- Program.cs
+-- appsettings.json
+-- Dockerfile
+-- .dockerignore

---

## Database Schema

### Database

fileprocessingdb

### Table

CREATE TABLE UploadedFiles
(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FileName VARCHAR(255),
    S3Key VARCHAR(500),
    UploadDate DATETIME
);

---

## Application Workflow

### Upload Process

1. User uploads a file
2. Application uploads file to Amazon S3
3. Metadata stored in Amazon RDS
4. S3 generates ObjectCreated event
5. Event sent to Amazon SQS
6. Lambda processes message
7. Processing logs written to CloudWatch

---

## Deployment

### Elastic Beanstalk

Application is deployed to:

* AWS Elastic Beanstalk
* .NET 8 running on Amazon Linux 2023

Deployment package generated using:

dotnet publish -c Release

---

## Docker Support

### Build Docker Image

docker build -t aws-assignment-demo .

### Run Container

docker run -d -p 8080:8080 --name aws-assignment-demo aws-assignment-demo

### Access Application
http://localhost:8080

---

## Docker Execution Notes

The application can be successfully built and executed using Docker.

The Amazon RDS instance is intentionally deployed inside a private subnet and is not publicly accessible.

As a result:

* Application starts successfully in Docker
* Dashboard and Upload pages load successfully
* Database operations are unavailable from local Docker containers
* Upload history may appear empty when running locally in Docker
* Full functionality is available when deployed inside AWS Elastic Beanstalk

This design follows cloud security best practices by preventing direct internet access to the database.

---

## Security

### IAM Authentication

Application uses EC2 Instance Profile:

app-elasticbeanstalk-ec2-role-01

Benefits:

* No hardcoded AWS credentials
* Temporary credentials managed by AWS
* Least-privilege access model

### Network Security

* RDS deployed in private subnet
* Security Group restricts database access
* Only Elastic Beanstalk instances can connect to MySQL

---

## Monitoring

### CloudWatch Logs

Lambda execution logs available in:

CloudWatch
 -> Log Groups
 -> Lambda Function Logs

### Elastic Beanstalk Logs

Application logs available through:

Elastic Beanstalk
 -> Logs

---

## Future Enhancements

* CI/CD using GitHub Actions
* Infrastructure as Code using Terraform
* File download functionality
* User authentication and authorization
* File processing status dashboard
* Application Insights and enhanced monitoring

---

## Author

Developed as part of an AWS Cloud Native Application Assignment demonstrating:

* Cloud Hosting
* Event-Driven Architecture
* Serverless Computing
* Database Integration
* Containerization
* Secure AWS Authentication
