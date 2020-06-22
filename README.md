---
page_type: sample
languages:
- csharp
products:
- .NET Core 3.1
description: "The user approval system to approve user registration"
urlFragment: ""
---

# Custom User Approval API

The User approval system acts as a bridge between signup user flow and user creation. The self-service sign-up user flow collects user data during the sign-up process and passes it to the approval system. The approval system user can then view/approve/deny the request.

## Contents

| File/folder         | Description                                |
|---------------------|--------------------------------------------|
| `CustomApproval.sln`| Sample solution.                           |
| `.gitignore`        | Define what to ignore at commit time.      |
| `CHANGELOG.md`      | List of changes to the sample.             |
| `CONTRIBUTING.md`   | Guidelines for contributing to the sample. |
| `README.md`         | This README file.                          |
| `LICENSE`           | The license for the sample.                |

## Prerequisites

-   IDE which supports .NET Core 3.1 (VS 2019 preferred)

-   An [Azure Active Directory tenant.]

-   An application registered in the AAD tenant with user read & write permissions (Learn more about [registering an app in AAD.])

-   A SendGrid account having an API Key (Learn more about [creating a SendGrid account.])

## Setup

Update the values in **appsettings.json**

**AppSettings:ParentAppRedirectUrl** -- The redirect URL present in the email received to the user after their request gets approved.

**AppSettings:DefaultLocale** -- The default locale to be used if there is no localization identifier present in the request.

**BasicAuth:ApiUsername** -- The Approvals API username

**BasicAuth:ApiPassword** -- The approvals API password

**GraphApi:Tenant** -- The AAD tenant name.

**GraphApi:ClientId** -- The application ID for the AAD app.

**GraphApi:ClientSecret** -- The client secret for the AAD app.

## Running the sample

Load the project in Visual Studio, update the values in **appsettings.json**, and then build and run the app.

## Key concepts

### Create Request

The API connectors (setup in the AAD tenant) will use the ***checkstatus*** & ***submit*** endpoints in **UserApprovalController** to communicate with the approval system. The ***checkstatus*** endpoint is to check whether the request is allowed to create and the ***submit*** endpoint is for creating a new approval request.

### Approval
In the case of an Azure AD user signing up, the approval system will create an invitation against the email id in the approval request. The recipient should accept the invitation to complete the sign-up process and gain access to the application.

In the case of a federated social user, the approval system will create a guest user account in the AAD and send a notification to the email id in the approval request.

Contributing
------------

This project welcomes contributions and suggestions. Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the \[Microsoft Open Source Code of Conduct\] (https://opensource.microsoft.com/codeofconduct/).

For more information see the \[Code of Conduct FAQ\] (https://opensource.microsoft.com/codeofconduct/faq/) or contact \[opencode\@microsoft.com\](mailto:opencode\@microsoft.com) with any additional questions or comments.

  [registering an app in AAD.]: https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app
  [creating a SendGrid account.]: https://docs.microsoft.com/en-us/azure/sendgrid-dotnet-how-to-send-email#create-a-sendgrid-account
  [Azure Active Directory tenant.]: https://docs.microsoft.com/azure/active-directory/develop/quickstart-create-new-tenant  