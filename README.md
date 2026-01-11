# EasyOnlineStoreAPI

## Endpoints
| HTTP | Endpoint | Description | Options |
|:------|:---------|:---------|:----------|
| `GET` | `/api/products` | **pagination** | `page=1&pageSize=10` |
| `GET` | `api/products/all` | **all products** | - |
| `POST`| `api/products` | **➕ Create** | `ProductCreate` |
| `DELETE` | `api/products/{id}` | **🗑️ Delete** | `id: Guid` |
