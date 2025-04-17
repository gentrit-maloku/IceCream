# IceSync - The Ice Cream Company Sync App

## Setup Instructions

### To Run the Project Locally:

1. **Configure Database Connection**:
    - Open the `appsettings.json` file and update the `ConnectionString` to point to your local SQL Server instance.

2. **Database Migration**:
    - Open the **Package Manager Console** in Visual Studio and run the following command to apply database migrations:
      ```
      update-database
      ```

3. **Quartz Configuration**:
    - To ensure Quartz works as expected, execute the following SQL script in your SQL Server database:
      - [Quartz Tables SQL for SQL Server](https://github.com/quartznet/quartznet/blob/main/database/tables/tables_sqlServer.sql)

4. **Run the Project**:
    - After performing the above steps, you can run your project locally.
    - The application will begin by calling the API to retrieve workflows, display them on the UI, and synchronize data in the background every 30 minutes.

---

## Technologies Used

- **.NET 8.0**: Long term support of .NET for the Web App.
- **SQL Server**: Database to store workflows.
- **Quartz**: Background job scheduler to sync workflows periodically.
- **Refit**: A library for easier consumption of REST APIs, used to interact with the Universal Loader API.
- **Entity Framework Core**: ORM used to interact with the SQL Server database and manage migrations.

---

## Notes

- Ensure your SQL Server database is properly set up and accessible for the application.
- If you encounter any issues with the background job (Quartz), ensure that the Quartz tables are correctly created by running the provided SQL script.
- If you need to update the database schema, you can use Entity Framework Core migrations to apply changes.

---
