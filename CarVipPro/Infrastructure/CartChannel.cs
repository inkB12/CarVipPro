namespace CarVipPro.APrenstationLayer.Infrastructure
{
    public static class CartChannel
    {
        public const string Key = "Cart.Channel";

        public static string EnsureChannel(ISession session)
        {
            var ch = session.GetString(Key);
            if (string.IsNullOrEmpty(ch))
            {
                ch = Guid.NewGuid().ToString("N");
                session.SetString(Key, ch);
            }
            return ch;
        }
    }
}
