# Tablet Integration API

## Overview
The Sankay POS system can accept orders from tablets on the local network via TCP/IP connection.

## Configuration

### Enable Tablet Server
Update the Settings table in the database:
```sql
UPDATE Settings SET Value = 'true' WHERE Key = 'TabletServerEnabled';
```

### Default Settings
- **IP Address**: 192.168.1.35
- **Port**: 8080
- **Protocol**: TCP/IP
- **Data Format**: JSON

## Connection

### Connect to Server
Tablets should connect to: `tcp://192.168.1.35:8080`

### Send Order
Send a JSON message with the following format:

```json
{
  "TableId": 1,
  "Items": [
    {
      "ProductId": 1,
      "Quantity": 2
    },
    {
      "ProductId": 9,
      "Quantity": 1
    }
  ]
}
```

### Fields Description

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| TableId | integer | Yes | ID of the table for this order |
| Items | array | Yes | Array of order items |
| Items[].ProductId | integer | Yes | ID of the product from Products table |
| Items[].Quantity | integer | Yes | Number of units to order |

## Response

The server will process the order and:
1. Create or update the order for the specified table
2. Add the items to the order
3. Update the table status to "Occupied"
4. The order will appear in the POS system immediately

Currently, the server does not send a response back. Future versions may include:
- Order confirmation with Order ID
- Error messages for invalid data
- Current order status

## Example Client Code

### Python Example
```python
import socket
import json

def send_order(table_id, items):
    # Create order data
    order = {
        "TableId": table_id,
        "Items": items
    }
    
    # Convert to JSON
    message = json.dumps(order)
    
    # Connect and send
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect(('192.168.1.35', 8080))
    sock.sendall(message.encode('utf-8'))
    sock.close()

# Example usage
send_order(1, [
    {"ProductId": 1, "Quantity": 2},
    {"ProductId": 9, "Quantity": 1}
])
```

### JavaScript/Node.js Example
```javascript
const net = require('net');

function sendOrder(tableId, items) {
    const order = {
        TableId: tableId,
        Items: items
    };
    
    const client = new net.Socket();
    client.connect(8080, '192.168.1.35', () => {
        client.write(JSON.stringify(order));
        client.end();
    });
}

// Example usage
sendOrder(1, [
    { ProductId: 1, Quantity: 2 },
    { ProductId: 9, Quantity: 1 }
]);
```

### C# Example
```csharp
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

public async Task SendOrderAsync(int tableId, List<OrderItem> items)
{
    var order = new {
        TableId = tableId,
        Items = items
    };
    
    var json = JsonSerializer.Serialize(order);
    var data = Encoding.UTF8.GetBytes(json);
    
    using var client = new TcpClient();
    await client.ConnectAsync("192.168.1.35", 8080);
    
    using var stream = client.GetStream();
    await stream.WriteAsync(data, 0, data.Length);
}

public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
```

## Product IDs

To get available products and their IDs, query the database:
```sql
SELECT Id, Name, Price, Category FROM Products ORDER BY Name;
```

Or implement a REST API endpoint to retrieve products (future enhancement).

## Table IDs

To get available tables and their IDs:
```sql
SELECT Id, Name, Status FROM Tables ORDER BY Position;
```

Status values:
- 0 = Available
- 1 = Occupied

## Error Handling

### Common Issues

1. **Connection Refused**
   - Ensure TabletServerEnabled is set to 'true'
   - Verify the IP address is correct
   - Check firewall settings

2. **Invalid JSON**
   - Ensure proper JSON formatting
   - All required fields must be present

3. **Invalid Product ID**
   - Product ID must exist in the Products table
   - Query the database to verify IDs

4. **Invalid Table ID**
   - Table ID must exist in the Tables table

## Security Notes

⚠️ **Important**: The current implementation has no authentication or encryption.

For production use, consider:
- Adding API key authentication
- Using TLS/SSL for encrypted communication
- Implementing rate limiting
- Adding IP whitelisting
- Validating all input data

## Future Enhancements

Planned features for tablet integration:
- [ ] Real-time order status updates
- [ ] Two-way communication (confirmations, errors)
- [ ] Menu synchronization
- [ ] Table status queries
- [ ] Order modification/cancellation
- [ ] Authentication and security
- [ ] WebSocket support for real-time updates

## Testing

### Test Connection
Use telnet or netcat to test connectivity:
```bash
echo '{"TableId":1,"Items":[{"ProductId":1,"Quantity":2}]}' | nc 192.168.1.35 8080
```

### Debug Mode
Check the Debug console in Visual Studio to see:
- Server start/stop messages
- Incoming connections
- Received orders
- Processing errors
