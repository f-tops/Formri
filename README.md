# Formri Project

## Overview

Formri is a web application built using .NET technologies, mainly Aspire. The application allows users to submit a contact form, and stores the submitted data in a Microsoft SQL Server database. The application includes additional features such as email notifications and user data caching.

## Features
- Contact form submission
- Storing user data in SQL Server
- Preventing duplicate submissions within one minute
- Fetching user details from an external API
- Sending email notifications with user data
- Displaying all users and users with ".biz" email addresses
- Complete Aspire dashboard used for analyzing container logs, performance and other infrastructure information

## Technologies Used
- .NET Core 8 Aspire
- Refit
- Microsoft SQL Server
- Blazor
- xUnit (for unit testing)
- Moq (for mocking in unit tests)
- IMemoryCache (for caching)

## Getting Started

### Prerequisites
- running SQL Server
- setup SQL Server connection string inside appsettings.Development.json file
