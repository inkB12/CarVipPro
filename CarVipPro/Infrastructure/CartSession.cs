using System.Text.Json;

namespace CarVipPro.APrenstationLayer.Infrastructure
{
    public static class CartSessionKeys
    {
        public const string Cart = "Cart.Items";
    }

    public class CartItem
    {
        public int ElectricVehicleId { get; set; }
        public string Name { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }
    }

    public class CartModel
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.LineTotal);
    }

    public static class CartSession
    {
        public static CartModel GetCart(this ISession session)
        {
            var json = session.GetString(CartSessionKeys.Cart);
            if (string.IsNullOrEmpty(json)) return new CartModel();
            return JsonSerializer.Deserialize<CartModel>(json) ?? new CartModel();
        }

        public static void SaveCart(this ISession session, CartModel cart)
        {
            var json = JsonSerializer.Serialize(cart);
            session.SetString(CartSessionKeys.Cart, json);
        }

        public static void ClearCart(this ISession session)
        {
            session.Remove(CartSessionKeys.Cart);
        }
    }
}
