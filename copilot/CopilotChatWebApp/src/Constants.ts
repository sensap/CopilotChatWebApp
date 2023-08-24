import botIcon1 from './assets/bot-icons/bot-icon-1.png';

export const Constants = {
    app: {
        name: 'Copilot',
        updateCheckIntervalSeconds: 60 * 5,
    },
    msal: {
        method: 'redirect', // 'redirect' | 'popup'
        auth: {
            clientId: process.env.REACT_APP_AAD_CLIENT_ID as string,
            authority: process.env.REACT_APP_AAD_AUTHORITY as string,
        },
        cache: {
            cacheLocation: 'localStorage',
            storeAuthStateInCookie: false,
        },
       // semanticKernelScopes: ['openid', 'offline_access', 'profile' ,'api://8553f7a2-6c1c-4ae3-bbf8-6a297bc1d2ae/access_as_user'],
       semanticKernelScopes: ['openid', 'offline_access', 'profile' ,'api://6a09384b-16cb-4634-a1ac-770aafc15e34/access_as_user'],
        // MS Graph scopes required for loading user information
        msGraphAppScopes: ['User.ReadBasic.All'],
    },
    bot: {
        profile: {
            id: 'bot',
            fullName: 'Copilot',
            emailAddress: '',
            photo: botIcon1,
        },
        fileExtension: 'skcb',
        typingIndicatorTimeoutMs: 5000,
    },
    debug: {
        root: 'sk-chatbot',
    },
    sk: {
        service: {
            defaultDefinition: 'int',
        },
        // Reserved context variable names
        reservedWords: ['server_url', 'server-url'],
    },
    // For a list of Microsoft Graph permissions, see https://learn.microsoft.com/en-us/graph/permissions-reference.
    // Your application registration will need to be granted these permissions in Azure Active Directory.
    msGraphPluginScopes: ['Calendars.Read', 'Mail.Read', 'Mail.Send', 'Tasks.ReadWrite', 'User.Read'],
    adoScopes: ['vso.work'],
    BATCH_REQUEST_LIMIT: 20,
};
