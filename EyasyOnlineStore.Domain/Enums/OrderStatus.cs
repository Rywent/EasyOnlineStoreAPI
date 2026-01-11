namespace EasyOnlineStore.Domain.Enums;

public enum OrderStatus
{
    Pending = 1,            // ожидание
    Paid = 2,               // оплачен
    Delivering = 3,         // в пути
    ReadyForPickup = 4,     // Готов к выдаче
    Completed = 5,          // Завершен
    Cancelled = 6           // Отменен
}
