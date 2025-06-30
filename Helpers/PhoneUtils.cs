namespace theUpSkilzAPI.Helpers
{
    public static class PhoneUtils
    {
        public static string NormalizePhone(string phone)
        {
            var cleaned = phone.Trim().Replace(" ", "").Replace("-", "");
            if (!cleaned.StartsWith("+91") && cleaned.Length == 10)
            {
                cleaned = $"+91{cleaned}";
            }
            return cleaned;
        }
    }
}
