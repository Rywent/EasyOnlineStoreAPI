# EasyOnlineStoreAPI

## Endpoints
### Products

| Method | Endpoint | Description | Request model |
|:------|:---------|:---------|:----------|
| `GET` | `/api/products` | **pagination** | `page=1&pageSize=10` |
| `GET` | `api/products/all` | **all products** | - |
| `GET` | `api/products/{id}` | **by id** | `id: Guid` |
| `POST`| `api/products` | **Create** | `ProductCreate` |
| `PUT` | `api/products` | **Update** | `ProductUpdate` |
| `DELETE` | `api/products/{id}` | **Delete** | `id: Guid` |

#### Product Requests
ProductCreate
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
ProductUpdate
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
