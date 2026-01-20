using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SankayPOS.Models;

namespace SankayPOS.Utils;

/// <summary>
/// TCP/IP server for tablet integration
/// Allows tablets on the local network to connect and send orders
/// </summary>
public class TabletServer
{
    private TcpListener? _listener;
    private bool _isRunning;
    private readonly int _port;
    private readonly string _ipAddress;
    
    public event EventHandler<OrderReceivedEventArgs>? OrderReceived;
    
    public TabletServer(string ipAddress, int port)
    {
        _ipAddress = ipAddress;
        _port = port;
    }
    
    public void Start()
    {
        if (_isRunning)
            return;
        
        try
        {
            var localAddr = IPAddress.Parse(_ipAddress);
            _listener = new TcpListener(localAddr, _port);
            _listener.Start();
            _isRunning = true;
            
            // Start accepting connections in background
            Task.Run(() => AcceptClients());
            
            System.Diagnostics.Debug.WriteLine($"Tablet server started on {_ipAddress}:{_port}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to start tablet server: {ex.Message}");
        }
    }
    
    public void Stop()
    {
        _isRunning = false;
        _listener?.Stop();
        System.Diagnostics.Debug.WriteLine("Tablet server stopped");
    }
    
    private async Task AcceptClients()
    {
        while (_isRunning)
        {
            try
            {
                if (_listener == null)
                    break;
                    
                var client = await _listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClient(client));
            }
            catch (Exception ex)
            {
                if (_isRunning)
                    System.Diagnostics.Debug.WriteLine($"Error accepting client: {ex.Message}");
            }
        }
    }
    
    private async Task HandleClient(TcpClient client)
    {
        try
        {
            using var stream = client.GetStream();
            var buffer = new byte[4096];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            
            if (bytesRead > 0)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                ProcessMessage(message);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }
    
    private void ProcessMessage(string message)
    {
        try
        {
            // Expected message format: JSON with order details
            // Example: {"TableId": 1, "Items": [{"ProductId": 1, "Quantity": 2}]}
            var orderData = JsonSerializer.Deserialize<TabletOrderData>(message);
            
            if (orderData != null)
            {
                OnOrderReceived(new OrderReceivedEventArgs { OrderData = orderData });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error processing message: {ex.Message}");
        }
    }
    
    protected virtual void OnOrderReceived(OrderReceivedEventArgs e)
    {
        OrderReceived?.Invoke(this, e);
    }
}

public class OrderReceivedEventArgs : EventArgs
{
    public TabletOrderData? OrderData { get; set; }
}

public class TabletOrderData
{
    public int TableId { get; set; }
    public List<TabletOrderItem> Items { get; set; } = new();
}

public class TabletOrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
