
# PiggyBank - The app for tracking financial state

***Disclaimer: this is not a commercial application; it is purely used to test skills and practise***

## The application has the following dependencies all in the latest 6.0.* verions

- `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore`
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.AspNetCore.Identity.UI`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.VisualStudio.Web.CodeGeneration.Design`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `SendGrid`

In addition, you need to have a [SendGrid](https://sendgrid.com/free/) account to use a cloud-based email marketing (email confirmation) and your API key, as well as [PostgreSQL](https://www.postgresql.org/download/) server available on your machine. You can learn how to generate a SendGrid API [here](https://docs.sendgrid.com/ui/sending-email/senders).

## To run the app on your local machine follow these steps:

1. Clone the repository  

```
 git clone https://github.com/ali-alo/PiggyBank.git
```

2. Navigate to the project folder

```
cd PiggyBank
cd PiggyBank
```

3. Restore dependencies

```
dotnet restore
```

4. Generate user secrets to connect to PostgreSQL

```
dotnet user-secrets set "ConnectionStrings:DbConnectionString" "Host=localhost; Database=PiggyBank; Username=[yourPostgreSQLUsername]; Password=[yourPostgreSQLPassword]"
```

5. Generate a secret to set up SendGrid API key

```
dotnet user-secrets set "EmailApi:SendGridKey" "[yourSendGridApiKey]"
```

6. Navigate to line 35 in Services/EmailSender.cs and change email to the email address you specified when creating a SendGrid API key.

    *Line 35 in Services/EmailSender.cs*
```
From = new EmailAddress("[yourEmailAddress]"),
```

7. Update database to the latest migration

```
dotnet ef database update
```

8. Run the app

```
dotnet run
```
