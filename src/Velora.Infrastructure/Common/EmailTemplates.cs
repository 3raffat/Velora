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

    public static string CouponBody(
        string customerName,
        string couponCode,
        decimal discount,
        DateTime? expiryDate = null,
        string storeName = "Velora",
        string shopUrl = "https://velora.com",
        string title = "A Special Offer For You 🎉",
        string description = "Enjoy this special discount on your next order."
    )
    {
        var expiryText = expiryDate.HasValue
            ? $"Valid until <strong>{expiryDate:yyyy-MM-dd}</strong>"
            : "No expiry date";
        return $""" <!DOCTYPE html> <html> <body style=" margin:0; padding:0; background-color:#f5f5f5; font-family:Arial, Helvetica, sans-serif; "> <table width="100%" cellpadding="0" cellspacing="0" border="0"> <tr> <td align="center" style="padding:40px 20px;"> <table width="600" cellpadding="0" cellspacing="0" border="0" style=" background-color:#ffffff; border-radius:12px; padding:35px; text-align:center; "> <tr> <td> <h1 style=" margin:0 0 20px; color:#111827; font-size:28px; "> {title} </h1> <p style=" margin:0 0 15px; color:#4b5563; font-size:16px; "> Hi {customerName}, </p> <p style=" margin:0; color:#4b5563; font-size:16px; line-height:1.6; "> {description} </p> <div style=" margin:30px 0; padding:25px; background-color:#f3f4f6; border-radius:10px; "> <p style=" margin:0 0 10px; color:#6b7280; font-size:14px; "> Your Coupon Code </p> <h2 style=" margin:0 0 15px; color:#111827; font-size:28px; letter-spacing:4px; "> {couponCode} </h2> <p style=" margin:0; color:#16a34a; font-size:20px; font-weight:bold; "> {discount}% OFF </p> </div> <p style=" margin:0; color:#6b7280; font-size:14px; "> {expiryText} </p> <a href="{shopUrl}" style=" display:inline-block; margin-top:25px; padding:14px 35px; background-color:#111827; color:#ffffff; text-decoration:none; border-radius:8px; font-weight:bold; "> Shop Now </a> <p style=" margin:30px 0 0; color:#9ca3af; font-size:13px; "> Thank you for choosing {storeName} ❤️ </p> </td> </tr> </table> </td> </tr> </table> </body> </html> """;
    }

    public static string BirthdayCouponBody(
        string firstName,
        string couponCode,
        decimal discount,
        DateTime expiryDate,
        string shopUrl = "https://velora.com"
    )
    {
        return $$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Happy Birthday!</title>
            </head>

            <body style="
                margin: 0;
                padding: 0;
                background-color: #f4f6f8;
                font-family: Arial, Helvetica, sans-serif;
                color: #1f2937;
            ">

                <table width="100%" cellpadding="0" cellspacing="0" border="0"
                       style="background-color: #f4f6f8; padding: 40px 16px;">

                    <tr>
                        <td align="center">

                            <table width="100%" cellpadding="0" cellspacing="0" border="0"
                                   style="
                                       max-width: 600px;
                                       background-color: #ffffff;
                                       border-radius: 12px;
                                       overflow: hidden;
                                   ">

                                <!-- Header -->
                                <tr>
                                    <td align="center"
                                        style="
                                            padding: 36px 30px;
                                            background-color: #111827;
                                            color: #ffffff;
                                        ">

                                        <div style="
                                            font-size: 42px;
                                            line-height: 1;
                                            margin-bottom: 12px;
                                        ">
                                            🎂
                                        </div>

                                        <h1 style="
                                            margin: 0;
                                            font-size: 28px;
                                            font-weight: 700;
                                        ">
                                            Happy Birthday!
                                        </h1>

                                        <p style="
                                            margin: 10px 0 0;
                                            font-size: 15px;
                                            color: #d1d5db;
                                        ">
                                            A little birthday surprise from Velora
                                        </p>

                                    </td>
                                </tr>

                                <!-- Content -->
                                <tr>
                                    <td style="padding: 36px 32px;">

                                        <p style="
                                            margin: 0 0 16px;
                                            font-size: 17px;
                                        ">
                                            Hi <strong>{{firstName}}</strong>,
                                        </p>

                                        <p style="
                                            margin: 0 0 24px;
                                            font-size: 15px;
                                            line-height: 1.7;
                                            color: #4b5563;
                                        ">
                                            Happy Birthday from everyone at Velora! 🎉
                                            We hope you have an amazing day.
                                        </p>

                                        <p style="
                                            margin: 0 0 24px;
                                            font-size: 15px;
                                            line-height: 1.7;
                                            color: #4b5563;
                                        ">
                                            To celebrate with you, we've prepared a
                                            special birthday discount just for you.
                                        </p>

                                        <!-- Discount -->
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0"
                                               style="
                                                   background-color: #f9fafb;
                                                   border: 1px solid #e5e7eb;
                                                   border-radius: 10px;
                                                   margin-bottom: 24px;
                                               ">

                                            <tr>
                                                <td align="center" style="padding: 28px 20px;">

                                                    <div style="
                                                        font-size: 32px;
                                                        font-weight: 700;
                                                        color: #111827;
                                                        margin-bottom: 8px;
                                                    ">
                                                        {{discount}} JOD OFF
                                                    </div>

                                                    <div style="
                                                        font-size: 14px;
                                                        color: #6b7280;
                                                    ">
                                                        Your birthday gift from Velora
                                                    </div>

                                                </td>
                                            </tr>

                                        </table>

                                        <!-- Coupon -->
                                        <p style="
                                            margin: 0 0 8px;
                                            text-align: center;
                                            font-size: 13px;
                                            color: #6b7280;
                                        ">
                                            Your coupon code
                                        </p>

                                        <table width="100%" cellpadding="0" cellspacing="0" border="0"
                                               style="
                                                   background-color: #111827;
                                                   border-radius: 8px;
                                                   margin-bottom: 24px;
                                               ">

                                            <tr>
                                                <td align="center" style="padding: 18px;">

                                                    <span style="
                                                        color: #ffffff;
                                                        font-size: 22px;
                                                        font-weight: 700;
                                                        letter-spacing: 3px;
                                                    ">
                                                        {{couponCode}}
                                                    </span>

                                                </td>
                                            </tr>

                                        </table>

                                        <!-- Expiry -->
                                        <p style="
                                            margin: 0;
                                            text-align: center;
                                            font-size: 13px;
                                            color: #6b7280;
                                        ">
                                            Valid until
                                            <strong style="color: #374151;">
                                                {{expiryDate}}
                                            </strong>
                                        </p>

                                        <!-- CTA -->
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0"
                                               style="margin-top: 30px;">

                                            <tr>
                                                <td align="center">

                                                    <a href="{{shopUrl}}"
                                                       style="
                                                           display: inline-block;
                                                           padding: 14px 28px;
                                                           background-color: #111827;
                                                           color: #ffffff;
                                                           text-decoration: none;
                                                           border-radius: 7px;
                                                           font-size: 15px;
                                                           font-weight: 600;
                                                       ">
                                                        Start Shopping
                                                    </a>

                                                </td>
                                            </tr>

                                        </table>

                                    </td>
                                </tr>

                                <!-- Footer -->
                                <tr>
                                    <td align="center"
                                        style="
                                            padding: 24px 30px;
                                            border-top: 1px solid #e5e7eb;
                                            background-color: #f9fafb;
                                        ">

                                        <p style="
                                            margin: 0 0 8px;
                                            font-size: 13px;
                                            color: #6b7280;
                                        ">
                                            Thank you for being part of Velora.
                                        </p>

                                        <p style="
                                            margin: 0;
                                            font-size: 12px;
                                            color: #9ca3af;
                                        ">
                                            © {{DateTime.UtcNow.Year}} Velora. All rights reserved.
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
