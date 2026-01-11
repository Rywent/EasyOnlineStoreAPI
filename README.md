# EasyOnlineStoreAPI

## Endpoints
### Products

| Method | Endpoint | Description | Request model | Response |
|:------|:---------|:------------|:--------------|:---------|
| `GET` | `/api/products` | **Pagination** | `?page=1&pageSize=10` | `ProductResponse[]` |
| `GET` | `/api/products/all` | **All products** | - | `ProductResponse[]` |
| `GET` | `/api/products/{id}` | **By ID** | `id: Guid` | `ProductResponse` |
| `POST` | `/api/products` | **Create** | `ProductCreateRequest` | **201** `ProductResponse` |
| `PUT` | `/api/products/{id}` | **Update** | `ProductUpdateRequest` | `ProductResponse` |
| `DELETE` | `/api/products/{id}` | **Delete** | `id: Guid` | **204** |

#### Product details
ProductCreateRequest
```json
{
  "name": "string",
  "description": "string",
  "shortDescription": "string",
  "oldPrice": 0,
  "stock": 0,
  "price": 0,
  "images": [
    {
      "imageUrl": "string"
    }
  ],
  "warehouseId": "guid"
}
```
ProductUpdateRequest
```json
{
  "name": "string",
  "description": "string",
  "shortDescription": "string",
  "oldPrice": 0,
  "stock": 0,
  "price": 0,
  "warehouseId": "guid"
}
```
ProductResponse
```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "shortDescription": "string",
  "oldPrice": 0,
  "stock": 0,
  "sku": "string",
  "rating": 0,
  "price": 0,
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
