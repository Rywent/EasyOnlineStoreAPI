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
}
```
ProductUpdate
```json
{
}
```
