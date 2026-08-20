using OrderService.Application.Features.Orders.Dtos;

namespace OrderService.Infrastructure.Common;

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
                                            Thanks for creating an account with OrderService.
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
                                            © {DateTime.UtcNow.Year} OrderService. All rights reserved.
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
        string shopUrl = "https://OrderService.com",
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
        string shopUrl = "https://OrderService.com"
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
                                                        {{discount}} % OFF
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
                                            Thank you for being part of OrderService.
                                        </p>

                                        <p style="
                                            margin: 0;
                                            font-size: 12px;
                                            color: #9ca3af;
                                        ">
                                            © {{DateTime.UtcNow.Year}} OrderService. All rights reserved.
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

    public static string ToEmailHtml(this OrderDetailDto order)
    {
        var orderItemsHtml = string.Join(
            "",
            order.Items.Select(item =>
                $"""
                    <tr>
                        <td style="padding:12px 8px; border-bottom:1px solid #e5e7eb;">
                            {item.ProductName}
                        </td>

                        <td align="center"
                            style="padding:12px 8px; border-bottom:1px solid #e5e7eb;">
                            {item.Quantity}
                        </td>

                        <td align="right"
                            style="padding:12px 8px; border-bottom:1px solid #e5e7eb;">
                            ${item.UnitPrice:N2}
                        </td>

                        <td align="right"
                            style="padding:12px 8px; border-bottom:1px solid #e5e7eb;">
                            ${item.Discount:N2}
                        </td>

                        <td align="right"
                            style="padding:12px 8px; border-bottom:1px solid #e5e7eb;">
                            ${item.TotalPrice:N2}
                        </td>
                    </tr>
                    """
            )
        );

        var billingAddress = $"""
        {order.BillingAddress.AddressLine1}<br>
        {(!string.IsNullOrWhiteSpace(order.BillingAddress.AddressLine2)
            ? $"{order.BillingAddress.AddressLine2}<br>"
            : "")}
        {order.BillingAddress.City}, {order.BillingAddress.State}<br>
        {order.BillingAddress.Country}
        """;

        var shippingAddress = $"""
        {order.ShippingAddress.AddressLine1}<br>
        {(!string.IsNullOrWhiteSpace(order.ShippingAddress.AddressLine2)
            ? $"{order.ShippingAddress.AddressLine2}<br>"
            : "")}
        {order.ShippingAddress.City}, {order.ShippingAddress.State}<br>
        {order.ShippingAddress.Country}
        """;

        return $$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Order Confirmation</title>
            </head>

            <body style="
                margin:0;
                padding:0;
                background:#f4f4f5;
                font-family:Arial,Helvetica,sans-serif;
                color:#18181b;
            ">

                <table width="100%" cellpadding="0" cellspacing="0" border="0"
                       style="background:#f4f4f5;padding:30px 15px;">

                    <tr>
                        <td align="center">

                            <table width="650" cellpadding="0" cellspacing="0" border="0"
                                   style="
                                        width:100%;
                                        max-width:650px;
                                        background:#ffffff;
                                        border-radius:12px;
                                        overflow:hidden;
                                   ">

                                <!-- Header -->
                                <tr>
                                    <td style="
                                        background:#111827;
                                        padding:32px;
                                        text-align:center;
                                    ">
                                        <h1 style="
                                            margin:0;
                                            color:#ffffff;
                                            font-size:28px;
                                        ">
                                            Order Confirmed
                                        </h1>

                                        <p style="
                                            margin:10px 0 0;
                                            color:#d1d5db;
                                            font-size:14px;
                                        ">
                                            Thank you for your order!
                                        </p>
                                    </td>
                                </tr>


                                <!-- Order Info -->
                                <tr>
                                    <td style="padding:30px;">

                                        <table width="100%"
                                               cellpadding="0"
                                               cellspacing="0">

                                            <tr>

                                                <td width="50%">
                                                    <div style="
                                                        font-size:12px;
                                                        color:#71717a;
                                                    ">
                                                        ORDER NUMBER
                                                    </div>

                                                    <div style="
                                                        margin-top:5px;
                                                        font-size:16px;
                                                        font-weight:bold;
                                                    ">
                                                        {{order.OrderNumber}}
                                                    </div>
                                                </td>


                                                <td width="50%" align="right">
                                                    <div style="
                                                        font-size:12px;
                                                        color:#71717a;
                                                    ">
                                                        ORDER DATE
                                                    </div>

                                                    <div style="
                                                        margin-top:5px;
                                                        font-size:16px;
                                                        font-weight:bold;
                                                    ">
                                                        {{order.OrderDate:yyyy-MM-dd HH:mm}}
                                                    </div>
                                                </td>

                                            </tr>


                                            <tr>
                                                <td colspan="2" style="padding-top:20px;">

                                                    <div style="
                                                        font-size:12px;
                                                        color:#71717a;
                                                    ">
                                                        ORDER STATUS
                                                    </div>

                                                    <div style="margin-top:5px;">
                                                        <span style="
                                                            display:inline-block;
                                                            padding:6px 12px;
                                                            border-radius:20px;
                                                            background:#dcfce7;
                                                            color:#166534;
                                                            font-size:13px;
                                                            font-weight:bold;
                                                        ">
                                                            {{order.Status}}
                                                        </span>
                                                    </div>

                                                </td>
                                            </tr>

                                        </table>


                                        <!-- Order Items -->
                                        <h2 style="
                                            margin:35px 0 15px;
                                            font-size:18px;
                                        ">
                                            Order Items
                                        </h2>


                                        <table width="100%"
                                               cellpadding="0"
                                               cellspacing="0"
                                               style="border-collapse:collapse;">

                                            <thead>

                                                <tr style="background:#f4f4f5;">

                                                    <th align="left"
                                                        style="
                                                            padding:12px 8px;
                                                            font-size:12px;
                                                        ">
                                                        Product
                                                    </th>

                                                    <th align="center"
                                                        style="
                                                            padding:12px 8px;
                                                            font-size:12px;
                                                        ">
                                                        Qty
                                                    </th>

                                                    <th align="right"
                                                        style="
                                                            padding:12px 8px;
                                                            font-size:12px;
                                                        ">
                                                        Unit Price
                                                    </th>

                                                    <th align="right"
                                                        style="
                                                            padding:12px 8px;
                                                            font-size:12px;
                                                        ">
                                                        Discount
                                                    </th>

                                                    <th align="right"
                                                        style="
                                                            padding:12px 8px;
                                                            font-size:12px;
                                                        ">
                                                        Total
                                                    </th>

                                                </tr>

                                            </thead>


                                            <tbody>
                                                {{orderItemsHtml}}
                                            </tbody>

                                        </table>


                                        <!-- Totals -->
                                        <table width="100%"
                                               cellpadding="0"
                                               cellspacing="0"
                                               style="margin-top:25px;">

                                            <tr>
                                                <td style="
                                                    padding:8px 0;
                                                    color:#71717a;
                                                ">
                                                    Subtotal
                                                </td>

                                                <td align="right">
                                                    ${{order.TotalBaseAmount:N2}}
                                                </td>
                                            </tr>


                                            <tr>
                                                <td style="
                                                    padding:8px 0;
                                                    color:#71717a;
                                                ">
                                                    Discount
                                                </td>

                                                <td align="right"
                                                    style="color:#dc2626;">
                                                    -${{order.TotalDiscountAmount:N2}}
                                                </td>
                                            </tr>


                                            <tr>
                                                <td style="
                                                    padding:8px 0;
                                                    color:#71717a;
                                                ">
                                                    Shipping
                                                </td>

                                                <td align="right">
                                                    ${{order.ShippingCost:N2}}
                                                </td>
                                            </tr>


                                            <tr>
                                                <td colspan="2"
                                                    style="
                                                        padding-top:15px;
                                                        border-top:1px solid #e5e7eb;
                                                    ">
                                                </td>
                                            </tr>


                                            <tr>
                                                <td style="
                                                    font-size:18px;
                                                    font-weight:bold;
                                                ">
                                                    Total
                                                </td>

                                                <td align="right"
                                                    style="
                                                        font-size:20px;
                                                        font-weight:bold;
                                                    ">
                                                    ${{order.TotalAmount:N2}}
                                                </td>
                                            </tr>

                                        </table>


                                        <!-- Addresses -->
                                        <table width="100%"
                                               cellpadding="0"
                                               cellspacing="0"
                                               style="margin-top:35px;">

                                            <tr>

                                                <td width="48%" valign="top">

                                                    <h3 style="
                                                        margin:0 0 10px;
                                                        font-size:15px;
                                                    ">
                                                        Billing Address
                                                    </h3>

                                                    <div style="
                                                        padding:15px;
                                                        background:#f9fafb;
                                                        border-radius:8px;
                                                        font-size:14px;
                                                        line-height:1.6;
                                                    ">
                                                        {{billingAddress}}
                                                    </div>

                                                </td>


                                                <td width="4%"></td>


                                                <td width="48%" valign="top">

                                                    <h3 style="
                                                        margin:0 0 10px;
                                                        font-size:15px;
                                                    ">
                                                        Shipping Address
                                                    </h3>

                                                    <div style="
                                                        padding:15px;
                                                        background:#f9fafb;
                                                        border-radius:8px;
                                                        font-size:14px;
                                                        line-height:1.6;
                                                    ">
                                                        {{shippingAddress}}
                                                    </div>

                                                </td>

                                            </tr>

                                        </table>

                                    </td>
                                </tr>


                                <!-- Footer -->
                                <tr>
                                    <td style="
                                        padding:25px;
                                        text-align:center;
                                        background:#f9fafb;
                                        color:#71717a;
                                        font-size:12px;
                                    ">
                                        Thank you for shopping with us.
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

    public static string CancellationConfirmationBody(
        OrderDetailDto order,
        CancellationDto cancellation
    )
    {
        var chargesHtml = cancellation.CancellationCharges.HasValue
            ? $"""
                <tr>
                    <td style="
                        padding:8px 0;
                        color:#71717a;
                    ">
                        Cancellation Charges
                    </td>

                    <td align="right"
                        style="color:#dc2626;">
                        ${cancellation.CancellationCharges:N2}
                    </td>
                </tr>
                """
            : "";

        var refundHtml = cancellation.Refund is not null
            ? $"""
                <tr>
                    <td colspan="2"
                        style="
                            padding-top:15px;
                            border-top:1px solid #e5e7eb;
                        ">
                    </td>
                </tr>

                <tr>
                    <td style="
                        font-size:16px;
                        font-weight:bold;
                        color:#166534;
                    ">
                        Refund Amount
                    </td>

                    <td align="right"
                        style="
                            font-size:18px;
                            font-weight:bold;
                            color:#166534;
                        ">
                        ${cancellation.Refund.Amount:N2}
                    </td>
                </tr>

                <tr>
                    <td style="
                        padding:8px 0;
                        color:#71717a;
                    ">
                        Refund Status
                    </td>

                    <td align="right">
                        <span style="
                            display:inline-block;
                            padding:4px 10px;
                            border-radius:12px;
                            background:#fef3c7;
                            color:#92400e;
                            font-size:12px;
                            font-weight:bold;
                        ">
                            {cancellation.Refund.Status}
                        </span>
                    </td>
                </tr>

                <tr>
                    <td style="
                        padding:8px 0;
                        color:#71717a;
                    ">
                        Refund Method
                    </td>

                    <td align="right">
                        {cancellation.Refund.RefundMethod}
                    </td>
                </tr>
                """
            : "";

        return $$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Order Cancellation</title>
            </head>

            <body style="
                margin:0;
                padding:0;
                background:#f4f4f5;
                font-family:Arial,Helvetica,sans-serif;
                color:#18181b;
            ">

                <table width="100%" cellpadding="0" cellspacing="0" border="0"
                       style="background:#f4f4f5;padding:30px 15px;">

                    <tr>
                        <td align="center">

                            <table width="650" cellpadding="0" cellspacing="0" border="0"
                                   style="
                                        width:100%;
                                        max-width:650px;
                                        background:#ffffff;
                                        border-radius:12px;
                                        overflow:hidden;
                                   ">

                                <!-- Header -->
                                <tr>
                                    <td style="
                                        background:#111827;
                                        padding:32px;
                                        text-align:center;
                                    ">
                                        <div style="
                                            font-size:36px;
                                            line-height:1;
                                            margin-bottom:12px;
                                        ">
                                            ❌
                                        </div>

                                        <h1 style="
                                            margin:0;
                                            color:#ffffff;
                                            font-size:28px;
                                        ">
                                            Order Cancelled
                                        </h1>

                                        <p style="
                                            margin:10px 0 0;
                                            color:#d1d5db;
                                            font-size:14px;
                                        ">
                                            Your cancellation request has been approved
                                        </p>
                                    </td>
                                </tr>


                                <!-- Order Info -->
                                <tr>
                                    <td style="padding:30px;">

                                        <table width="100%"
                                               cellpadding="0"
                                               cellspacing="0">

                                            <tr>
                                                <td width="50%">
                                                    <div style="
                                                        font-size:12px;
                                                        color:#71717a;
                                                    ">
                                                        ORDER NUMBER
                                                    </div>

                                                    <div style="
                                                        margin-top:5px;
                                                        font-size:16px;
                                                        font-weight:bold;
                                                    ">
                                                        {{order.OrderNumber}}
                                                    </div>
                                                </td>


                                                <td width="50%" align="right">
                                                    <div style="
                                                        font-size:12px;
                                                        color:#71717a;
                                                    ">
                                                        CANCELLED ON
                                                    </div>

                                                    <div style="
                                                        margin-top:5px;
                                                        font-size:16px;
                                                        font-weight:bold;
                                                    ">
                                                        {{cancellation.ProcessedAt?.ToString("yyyy-MM-dd HH:mm") ?? "—"}}
                                                    </div>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td colspan="2" style="padding-top:20px;">

                                                    <div style="
                                                        font-size:12px;
                                                        color:#71717a;
                                                    ">
                                                        ORDER STATUS
                                                    </div>

                                                    <div style="margin-top:5px;">
                                                        <span style="
                                                            display:inline-block;
                                                            padding:6px 12px;
                                                            border-radius:20px;
                                                            background:#fee2e2;
                                                            color:#991b1b;
                                                            font-size:13px;
                                                            font-weight:bold;
                                                        ">
                                                            Cancelled
                                                        </span>
                                                    </div>

                                                </td>
                                            </tr>

                                        </table>


                                        <!-- Cancellation Reason -->
                                        <div style="
                                            margin-top:25px;
                                            padding:20px;
                                            background:#fef2f2;
                                            border:1px solid #fecaca;
                                            border-radius:10px;
                                        ">

                                            <div style="
                                                font-size:12px;
                                                color:#991b1b;
                                                font-weight:bold;
                                                margin-bottom:8px;
                                            ">
                                                CANCELLATION REASON
                                            </div>

                                            <p style="
                                                margin:0;
                                                font-size:15px;
                                                line-height:1.6;
                                                color:#7f1d1d;
                                            ">
                                                {{cancellation.Reason}}
                                            </p>

                                            {{(cancellation.Remarks is not null
                                                ? $"""
                                                    <div style="
                                                        margin-top:12px;
                                                        font-size:12px;
                                                        color:#991b1b;
                                                        font-weight:bold;
                                                    ">
                                                        REMARKS
                                                    </div>
                                                    <p style="
                                                        margin:4px 0 0;
                                                        font-size:14px;
                                                        color:#7f1d1d;
                                                    ">
                                                        {cancellation.Remarks}
                                                    </p>
                                                    """
                                                : "")}}

                                        </div>


                                        <!-- Financial Summary -->
                                        <h2 style="
                                            margin:30px 0 15px;
                                            font-size:18px;
                                        ">
                                            Financial Summary
                                        </h2>

                                        <table width="100%"
                                               cellpadding="0"
                                               cellspacing="0"
                                               style="border-collapse:collapse;">

                                            <tr>
                                                <td style="
                                                    padding:8px 0;
                                                    color:#71717a;
                                                ">
                                                    Order Amount
                                                </td>

                                                <td align="right">
                                                    ${{cancellation.OrderAmount:N2}}
                                                </td>
                                            </tr>

                                            {{chargesHtml}}

                                            {{refundHtml}}

                                        </table>

                                    </td>
                                </tr>


                                <!-- Footer -->
                                <tr>
                                    <td style="
                                        padding:25px;
                                        text-align:center;
                                        background:#f9fafb;
                                        border-top:1px solid #e5e7eb;
                                    ">
                                        <p style="
                                            margin:0 0 8px;
                                            font-size:14px;
                                            color:#4b5563;
                                        ">
                                            If you have any questions, please contact our support team.
                                        </p>

                                        <p style="
                                            margin:0;
                                            font-size:12px;
                                            color:#9ca3af;
                                        ">
                                            © {{DateTime.UtcNow.Year}} OrderService. All rights reserved.
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

    public static string RefundConfirmationBody(OrderDetailDto order, RefundDto refund)
    {
        var statusColor = refund.Status.ToString() switch
        {
            "Completed" => "background:#dcfce7;color:#166534;",
            "Approved" => "background:#dbeafe;color:#1e40af;",
            "Rejected" => "background:#fee2e2;color:#991b1b;",
            "Failed" => "background:#fee2e2;color:#991b1b;",
            _ => "background:#fef3c7;color:#92400e;",
        };

        var transactionHtml = refund.TransactionId is not null
            ? $"""
                <tr>
                    <td style="
                        padding:8px 0;
                        color:#71717a;
                    ">
                        Transaction ID
                    </td>

                    <td align="right"
                        style="
                            font-family:monospace;
                            font-size:13px;
                        ">
                        {refund.TransactionId}
                    </td>
                </tr>
                """
            : "";

        var reasonHtml = refund.RefundReason is not null
            ? $"""
                <div style="
                    margin-top:25px;
                    padding:20px;
                    background:#f3f4f6;
                    border:1px solid #e5e7eb;
                    border-radius:10px;
                ">
                    <div style="
                        font-size:12px;
                        color:#6b7280;
                        font-weight:bold;
                        margin-bottom:8px;
                    ">
                        REFUND REASON
                    </div>

                    <p style="
                        margin:0;
                        font-size:15px;
                        line-height:1.6;
                        color:#374151;
                    ">
                        {refund.RefundReason}
                    </p>
                </div>
                """
            : "";

        return $$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Refund Update</title>
            </head>

            <body style="
                margin:0;
                padding:0;
                background:#f4f4f5;
                font-family:Arial,Helvetica,sans-serif;
                color:#18181b;
            ">

                <table width="100%" cellpadding="0" cellspacing="0" border="0"
                       style="background:#f4f4f5;padding:30px 15px;">

                    <tr>
                        <td align="center">

                            <table width="650" cellpadding="0" cellspacing="0" border="0"
                                   style="
                                        width:100%;
                                        max-width:650px;
                                        background:#ffffff;
                                        border-radius:12px;
                                        overflow:hidden;
                                   ">

                                <!-- Header -->
                                <tr>
                                    <td style="
                                        background:#111827;
                                        padding:32px;
                                        text-align:center;
                                    ">
                                        <div style="
                                            font-size:36px;
                                            line-height:1;
                                            margin-bottom:12px;
                                        ">
                                            💰
                                        </div>

                                        <h1 style="
                                            margin:0;
                                            color:#ffffff;
                                            font-size:28px;
                                        ">
                                            Refund Update
                                        </h1>

                                        <p style="
                                            margin:10px 0 0;
                                            color:#d1d5db;
                                            font-size:14px;
                                        ">
                                            Order #{{order.OrderNumber}}
                                        </p>
                                    </td>
                                </tr>


                                <!-- Refund Details -->
                                <tr>
                                    <td style="padding:30px;">

                                        <!-- Status Badge -->
                                        <div style="text-align:center;margin-bottom:25px;">
                                            <span style="
                                                display:inline-block;
                                                padding:8px 20px;
                                                border-radius:20px;
                                                {{statusColor}}
                                                font-size:14px;
                                                font-weight:bold;
                                            ">
                                                Refund {{refund.Status}}
                                            </span>
                                        </div>


                                        <!-- Refund Amount -->
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0"
                                               style="
                                                    background:#f9fafb;
                                                    border:1px solid #e5e7eb;
                                                    border-radius:10px;
                                                    margin-bottom:25px;
                                               ">

                                            <tr>
                                                <td align="center" style="padding:28px 20px;">

                                                    <div style="
                                                        font-size:14px;
                                                        color:#6b7280;
                                                        margin-bottom:8px;
                                                    ">
                                                        Refund Amount
                                                    </div>

                                                    <div style="
                                                        font-size:32px;
                                                        font-weight:700;
                                                        color:#111827;
                                                    ">
                                                        ${{refund.Amount:N2}}
                                                    </div>

                                                </td>
                                            </tr>

                                        </table>


                                        <!-- Details Table -->
                                        <h2 style="
                                            margin:0 0 15px;
                                            font-size:18px;
                                        ">
                                            Refund Details
                                        </h2>

                                        <table width="100%"
                                               cellpadding="0"
                                               cellspacing="0"
                                               style="border-collapse:collapse;">

                                            <tr>
                                                <td style="
                                                    padding:8px 0;
                                                    color:#71717a;
                                                ">
                                                    Refund Method
                                                </td>

                                                <td align="right"
                                                    style="font-weight:bold;">
                                                    {{refund.RefundMethod}}
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="
                                                    padding:8px 0;
                                                    color:#71717a;
                                                ">
                                                    Status
                                                </td>

                                                <td align="right">
                                                    <span style="
                                                        display:inline-block;
                                                        padding:4px 10px;
                                                        border-radius:12px;
                                                        {{statusColor}}
                                                        font-size:12px;
                                                        font-weight:bold;
                                                    ">
                                                        {{refund.Status}}
                                                    </span>
                                                </td>
                                            </tr>

                                            {{transactionHtml}}

                                        </table>


                                        {{reasonHtml}}

                                    </td>
                                </tr>


                                <!-- Footer -->
                                <tr>
                                    <td style="
                                        padding:25px;
                                        text-align:center;
                                        background:#f9fafb;
                                        border-top:1px solid #e5e7eb;
                                    ">
                                        <p style="
                                            margin:0 0 8px;
                                            font-size:14px;
                                            color:#4b5563;
                                        ">
                                            If you have any questions, please contact our support team.
                                        </p>

                                        <p style="
                                            margin:0;
                                            font-size:12px;
                                            color:#9ca3af;
                                        ">
                                            © {{DateTime.UtcNow.Year}} OrderService. All rights reserved.
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
