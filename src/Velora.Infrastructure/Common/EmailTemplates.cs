namespace Velora.Infrastructure.Common;

public static class EmailTemplates
{
    public static string ConfirmationEmailBody(string userName, string confirmationLink)
    {
        return $"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Confirm Your Email</title>
            </head>

            <body style="
                margin: 0;
                padding: 0;
                background-color: #f4f6f8;
                font-family: Arial, Helvetica, sans-serif;
                color: #1f2937;
            ">

                <table width="100%" cellpadding="0" cellspacing="0" border="0"
                       style="background-color: #f4f6f8; padding: 40px 20px;">
                    <tr>
                        <td align="center">

                            <table width="100%" cellpadding="0" cellspacing="0" border="0"
                                   style="
                                       max-width: 600px;
                                       background-color: #ffffff;
                                       border-radius: 12px;
                                       overflow: hidden;
                                       box-shadow: 0 4px 20px rgba(0,0,0,0.06);
                                   ">

                                <!-- Header -->
                                <tr>
                                    <td style="
                                        padding: 30px;
                                        text-align: center;
                                        background-color: #111827;
                                    ">
                                        <h1 style="
                                            margin: 0;
                                            color: #ffffff;
                                            font-size: 28px;
                                            font-weight: 700;
                                        ">
                                            Velora
                                        </h1>
                                    </td>
                                </tr>

                                <!-- Content -->
                                <tr>
                                    <td style="padding: 40px 35px;">

                                        <h2 style="
                                            margin: 0 0 20px;
                                            font-size: 24px;
                                            color: #111827;
                                        ">
                                            Confirm your email
                                        </h2>

                                        <p style="
                                            margin: 0 0 15px;
                                            font-size: 16px;
                                            line-height: 1.6;
                                            color: #4b5563;
                                        ">
                                            Hello <strong>{userName}</strong>,
                                        </p>

                                        <p style="
                                            margin: 0 0 30px;
                                            font-size: 16px;
                                            line-height: 1.6;
                                            color: #4b5563;
                                        ">
                                            Thanks for creating an account with Velora.
                                            Please confirm your email address to activate
                                            your account and get started.
                                        </p>

                                        <!-- Button -->
                                        <table cellpadding="0" cellspacing="0" border="0"
                                               style="margin: 0 auto 30px;">
                                            <tr>
                                                <td align="center"
                                                    style="
                                                        border-radius: 8px;
                                                        background-color: #111827;
                                                    ">
                                                    <a href="{confirmationLink}"
                                                       style="
                                                           display: inline-block;
                                                           padding: 14px 28px;
                                                           font-size: 16px;
                                                           font-weight: 600;
                                                           color: #ffffff;
                                                           text-decoration: none;
                                                           border-radius: 8px;
                                                       ">
                                                        Confirm Email
                                                    </a>
                                                </td>
                                            </tr>
                                        </table>

                                        <p style="
                                            margin: 0 0 10px;
                                            font-size: 14px;
                                            color: #6b7280;
                                        ">
                                            If the button doesn't work, copy and paste
                                            the following link into your browser:
                                        </p>

                                        <p style="
                                            margin: 0;
                                            padding: 15px;
                                            background-color: #f3f4f6;
                                            border-radius: 6px;
                                            word-break: break-all;
                                            font-size: 13px;
                                            line-height: 1.5;
                                        ">
                                            <a href="{confirmationLink}"
                                               style="
                                                   color: #4f46e5;
                                                   text-decoration: none;
                                               ">
                                                {confirmationLink}
                                            </a>
                                        </p>

                                    </td>
                                </tr>

                                <!-- Footer -->
                                <tr>
                                    <td style="
                                        padding: 25px 35px;
                                        background-color: #f9fafb;
                                        text-align: center;
                                        border-top: 1px solid #e5e7eb;
                                    ">
                                        <p style="
                                            margin: 0 0 8px;
                                            font-size: 13px;
                                            color: #6b7280;
                                        ">
                                            If you didn't create this account,
                                            you can safely ignore this email.
                                        </p>

                                        <p style="
                                            margin: 0;
                                            font-size: 12px;
                                            color: #9ca3af;
                                        ">
                                            © {DateTime.UtcNow.Year} Velora. All rights reserved.
                                        </p>
                                    </td>
                                </tr>

                            </table>

                        </td>
                    </tr>
                </table>

            </body>
            </html>
            """;
    }
}
