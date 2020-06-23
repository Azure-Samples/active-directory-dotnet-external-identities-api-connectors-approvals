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

- IDE which supports .NET Core 3.1 (VS 2019 preferred)
- An [Azure Active Directory tenant.]
- An application registered in the AAD tenant with user read & write permissions (Learn more about [registering an app in AAD.])
- A SendGrid account having an API Key (Learn more about [creating a SendGrid account.])

## Setup

Update the values in **appsettings.json**

- **AppSettings:ParentAppRedirectUrl** -- The redirect URL present in the email received to the user after their request gets approved.
- **AppSettings:DefaultLocale** -- The default locale to be used if there is no localization identifier present in the request.
- **BasicAuth:ApiUsername** -- The Approvals API username
- **BasicAuth:ApiPassword** -- The approvals API password
- **GraphApi:Tenant** -- The AAD tenant name.
- **GraphApi:ClientId** -- The application ID for the AAD app.
- **GraphApi:ClientSecret** -- The client secret for the AAD app.

## Deploy the sample

Load the project in Visual Studio, update the values in **appsettings.json**, and then build and run the app. To make this sample work with API connectors, you must also deploy the application to Azure. Follow these instructions to publish it from Visual Studio 2019 to an [Azure App Service.]

## Key concepts

### Create Request

The API connectors (setup in the AAD tenant) will use the ***checkstatus*** & ***submit*** endpoints in **UserApprovalController** to communicate with the approval system. The ***checkstatus*** endpoint is to check whether the request is allowed to create and the ***submit*** endpoint is for creating a new approval request.

### Approval

In the case of an Azure AD user signing up, the approval system will create an invitation against the email id in the approval request. The recipient should accept the invitation to complete the sign-up process and gain access to the application.

In the case of a federated social user, the approval system will create a guest user account in the AAD and send a notification to the email id in the approval request.

## Integrate the Custom Approvals app with External Identities self-service sign up

### Configure a self-service sign up user flow

[Create a self-service sign up user flow] for registering external users to your Azure Active Directory tenant.

Under **User attributes**, the following must be selected to collect the information from the user signing up:

![User attributes](/Images/user-flow-attributes.png "User attributes selected")

### Create an API connector

After the Azure AD tenant has been configured for use with External Identities self-service sign up, [create an API connector] for both checking approval status and submitting an approval request.

#### Check status

- **Display Name**: Choose a name, such as 'Check approval status'
- **Endpoint URL**: Use the URL created when publishing the Custom Approvals app (/approvals/checkstatus)
- **Username**: Username defined in the **appsettings.json** file (BasicAuth:ApiUsername)
- **Password**: Password defined in the **appsettings.json** file (BasicAuth:ApiPassword)
- **Claims to send**:
  - Email address
  - Identity collection

The API connector configuration should look like the following:

![Check status](/Images/api-connector-check-status.png "Check status API connector")

#### Submit approval request

- **Display Name**: Choose a name, such as 'Submit approval request'
- **Endpoint URL**: Use the URL created when publishing the Custom Approvals app (/approvals/submit)
- **Username**: Username defined in the **appsettings.json** file (BasicAuth:ApiUsername)
- **Password**: Password defined in the **appsettings.json** file (BasicAuth:ApiPassword)
- **Claims to send**:
  - Email Address
  - Identity collection
  - Display name
  - Given name
  - Surname

The API connector configuration should look like the following:

![Submit approval](/Images/api-connector-submit.png "Submit approval API connector")

### Enable the API connectors in the user flow

You now need to enable the API connectors you configured in the user flow. Navigate back to **User flows (Preview)**, clikc the user flow you created, and click on **API connectors**. From here, select the drop-down menu for **After signing in with an identity provider** and select the API connector for ***Check approval status***. Then, select the drop-down menu for **Before creating the user** and select the API connector for ***Submit approval request***.

![User flow API connectors](/Images/user-flow-api-connectors.png "User flow API connectors")

### End user experience

Your self-service sign up user flow should now be calling out to the API when a user signs up. This will check the status of an approval request after a user signs in, and if one does not exist, submit a new one once they provide the information requested in the user flow. After submitting the information, the user will be informed that their request to access the application has been submitted. This will provision an approval request in the Custom Approvals application that can be approved or denied.

If the request is approved, the Custom Approvals app will provision the user account in your Azure Active Directory tenant and email the user to inform them that they can now access the application.

Contributing
------------

This project welcomes contributions and suggestions. Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the \[Microsoft Open Source Code of Conduct\] (https://opensource.microsoft.com/codeofconduct/).

For more information see the \[Code of Conduct FAQ\] (https://opensource.microsoft.com/codeofconduct/faq/) or contact \[opencode\@microsoft.com\](mailto:opencode\@microsoft.com) with any additional questions or comments.

  [registering an app in AAD.]: https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app
  [creating a SendGrid account.]: https://docs.microsoft.com/azure/sendgrid-dotnet-how-to-send-email#create-a-sendgrid-account
  [Azure Active Directory tenant.]: https://docs.microsoft.com/azure/active-directory/develop/quickstart-create-new-tenant
  [Azure App Service.]: https://docs.microsoft.com/visualstudio/deployment/quickstart-deploy-to-azure?view=vs-2019
  [Create a self-service sign up user flow]: https://docs.microsoft.com/azure/active-directory/b2b/self-service-sign-up-user-flow
  [create an API connector]: https://docs.microsoft.com/azure/active-directory/b2b/self-service-sign-up-add-api-connector#create-an-api-connector
