using System.Collections.Concurrent;

public static class OtpStore
{
    private static ConcurrentDictionary<string, string> otpData = new();

    public static void SetOtp(string phone, string otp) => otpData[phone] = otp;
    public static string? GetOtp(string phone) => otpData.TryGetValue(phone, out var otp) ? otp : null;
    public static void ClearOtp(string phone) => otpData.TryRemove(phone, out _);
}

