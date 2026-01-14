# EasyOnlineStoreAPI

## 📍 Endpoints
### 🛍️ Products

| Method | Endpoint | Description | Request model | Response |
|:------|:---------|:------------|:--------------|:---------|
| `GET` | `/api/products` | **Pagination** | `?page=1&pageSize=10` |**200** `ProductResponse[]` |
| `GET` | `/api/products/all` | **All products** | - | **200** `ProductResponse[]` |
| `GET` | `/api/products/{id}` | **By ID** | `id: Guid` |**200** `ProductResponse` |
| `POST` | `/api/products` | **Create** | `ProductCreateRequest` | **201** `ProductResponse` |
| `PATCH` | `/api/products/{id}` | **Update** | `ProductUpdateRequest` |**200** `ProductResponse` |
| `DELETE` | `/api/products/{id}` | **Delete** | `id: Guid` | **204** |

### Product Images

| Method | Endpoint | Description | Request model | Response |
|:------|:---------|:------------|:--------------|:---------|
| `GET` | `/api/products/{productId}/images` | **Get images** | - | `ProductImageResponse[]` |
| `POST` | `/api/products/{productId}/images` | **Add image** | `ProductImageRequest` | **201** `ProductImageResponse` |
| `DELETE` | `/api/products/{productId}/images/{imageId}` | **Delete image** | `imageId: Guid` `productId: Guid` | **204** |

### ℹ️ Product details
**ProductCreateRequest**
```json
{
  "name": "string",
  "description": "string",
  "shortDescription": "string",
  "oldPrice": 0.0,
  "stock": 0,
  "price": 0.0,
  "images": [
    {
      "imageUrl": "string"
    }
  ],
  "categoryId" : "guid",
  "warehouseId": "guid"

}
```
**ProductUpdateRequest**
```json
{
  "name": "string",
  "description": "string",
  "shortDescription": "string",
  "oldPrice": 0.0,
  "stock": 0,
  "price": 0.0,
  "categoryId" : "guid",
  "warehouseId": "guid"
}
```
**ProductResponse**
```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "shortDescription": "string",
  "oldPrice": 0.0,
  "stock": 0,
  "sku": "string",
  "rating": 0.0,
  "price": 0.0,
  "createdAt": "string",
  "updatedAt": "string",
  "images": [
    {
      "id": "guid",
      "imageUrl": "string"
    }
  ],
  "warehouseId": "guid"
}
```
**ProductImageRequest**
```json
{
  "imageUrl": "string"
}
```
**ProductImageResponse**
```json
{
  "id": "guid",
  "imageUrl": "string"
}
```

### 🛒 Carts 

| Method | Endpoint | Description | Request model | Response |
|:------|:---------|:------------|:--------------|:---------|
| `GET` | `/api/carts` | **All carts** | `-` | **200** `CartResponse[]` |
| `GET` | `/api/carts/{id}` | **By ID** | `id: Guid` | **200** `CartResponse` |
| `POST` | `/api/carts` | **Create an empty cart** | `-` | **201** `CartResponse` |
| `POST` | `/api/carts/{cartId}/items` | **Add item to cart** | `cartId: Guid` `CartAddItemRequest` |**200** `CartResponse` |
| `POST` | `/api/carts/{cartId}/items/clear` | **Clear cart** | `-` | **200** `CartResponse` |
| `PATCH` | `/api/carts/{cartId}/items` | **Update item quantity** | `cartId: Guid` `CartItemUpdateRequest` | **200** `CartResponse` |
| `DELETE` | `/api/carts/{cartId}/items/{itemId}` | **Delete item from cart** | `cartId: Guid` `itemId: Guid` | **204** |
| `DELETE` | `/api/carts/{id}` | **Delete cart** | `id: Guid` | **204** |

### ℹ️ Cart details
**CartAddItemRequest**
```json
{
  "productId": "guid",
  "quantity": 0
}
```
**CartItemUpdateRequest**
```json
{
  "productId": "guid",
  "quantity": 0
}
```
**CartResponse**
```json
{
  "cartId": "guid",
  "cartItems": [
    {
      "productId": "guid",
      "productName": "string",
      "quantity": 0,
      "unitPrice": 0.0,
      "subTotal": 0.0
    }
  ],
  "totalPrice": 0.0
}
```

### 📦 Order 

| Method | Endpoint | Description | Request model | Response |
|:------|:---------|:------------|:--------------|:---------|
| `GET` | `/api/orders` | **Pagination** | `?page=1&pageSize=10` |**200** `OrderResponse[]` |
| `GET` | `/api/orders/all` | **All orders** | `-` | **200** `OrderResponse[]` |
| `GET` | `/api/orders/{id}` | **By ID** | `id: Guid` | **200** `OrderResponse` |
| `POST` | `/api/orders/{cartId}` | **Create an order from the cart** | `-` | **201** `OrderResponse` |
| `PATCH` | `/api/orders/{orderId}` | **Edit order status** | `?status=statusNumber` |**200** `OrderResponse` |
| `PUT` | `/api/orders/cancel/{id}` | **Cancel order** | `id: Guid` | **200** |
| `DELETE` | `/api/orders/{id}` | **Delete order** | `id: Guid` | **204** |

### ℹ️ Order details

**Order statuses**
- **Pending** (1) 
- **Paid** (2)
- **Delivering** (3)
- **ReadyForPickup** (4)
- **Completed** (5)
- **Cancelled** (6)

**OrderResponse**
```json
{
  "id": "guid",
  "orderNumber": "string",
  "createdDate": "0001-01-01T00:00:00",
  "status": 1,
  "items": [
    {
      "id": "guid",
      "productId": "guid",
      "productName": "string",
      "quantity": 0,
      "unitPrice": 0.0,
      "warehouseId": "guid"
    }
  ],
  "totalPrice": 0.0
}
```

### 🏭 Warehouse 

| Method | Endpoint | Description | Request model | Response |
|:------|:---------|:------------|:--------------|:---------|
| `GET` | `/api/warehouses` | **Pagination** | `?page=1&pageSize=10` |**200** `WarehouseShortResponse[]` |
| `GET` | `/api/warehouses/{id}` | **By ID** | `id: Guid` | **200** `WarehouseResponse` |
| `POST` | `/api/warehouses` | **Create new warehouse* | `WarehouseCreateRequest` | **201** `WarehouseResponse` |
| `PATCH` | `/api/warehouses/{id}` | **Update** | `WarehouseUpdateRequest` |**200** `WarehouseResponse` |
| `DELETE` | `/api/warehouses/{id}` | **Delete warehouse** | `id: Guid` | **204** |

### ℹ️ Warehouse details

**WarehouseCreateRequest**
```json
{
  "name": "string",
  "location": "string",
  "adress": "string",
  "phone": "string",
  "isActive": true,
  "deliveryCost": 0
}
```

**WarehouseUpdateRequest**
```json
{
  "name": "string",
  "location": "string",
  "adress": "string",
  "phone": "string",
  "isActive": true,
  "deliveryCost": 0
}
```

**WarehouseShortResponse**
```json
[
  {
    "id": "guid",
    "name": "string",
    "location": "string"
  }
]
```

**WarehouseResponse**
```json
{
  "id": "guid",
  "name": "string",
  "location": "string",
  "adress": "string",
  "phone": "string",
  "isActive": true,
  "deliveryCost": 0.0,
  "likesCount": 0,
  "createdAt": "0001-01-01T00:00:00",
  "products": [
    {
      "id": "guid",
      "name": "string",
      "description": "string",
      "shortDescription": "string",
      "price": 0.0,
      "stock": 0,
      "sku": "string"
    }
  ]
}
```



