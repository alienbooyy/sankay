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
public class TabletServer : IDisposable
{
    private TcpListener? _listener;
    private bool _isRunning;
    private readonly int _port;
    private readonly string _ipAddress;
    private CancellationTokenSource? _cancellationTokenSource;
    
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
            _cancellationTokenSource = new CancellationTokenSource();
            
            // Start accepting connections in background
            _ = Task.Run(() => AcceptClients(_cancellationTokenSource.Token));
            
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
        _cancellationTokenSource?.Cancel();
        _listener?.Stop();
        System.Diagnostics.Debug.WriteLine("Tablet server stopped");
    }
    
    private async Task AcceptClients(CancellationToken cancellationToken)
    {
        while (_isRunning && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (_listener == null)
                    break;
                    
                var client = await _listener.AcceptTcpClientAsync(cancellationToken);
                _ = Task.Run(() => HandleClient(client), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when stopping
                break;
            }
            catch (Exception ex)
            {
                if (_isRunning && !cancellationToken.IsCancellationRequested)
                    System.Diagnostics.Debug.WriteLine($"Error accepting client: {ex.Message}");
            }
        }
    }
    
    private async Task HandleClient(TcpClient client)
    {
        try
        {
            using (client)
            {
                using var stream = client.GetStream();
                var buffer = new byte[4096];
                var messageBuilder = new StringBuilder();
                int bytesRead;
                
                // Read data in chunks to handle large messages
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    
                    // Check if we have a complete JSON message
                    var message = messageBuilder.ToString();
                    if (IsCompleteJson(message))
                    {
                        ProcessMessage(message);
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error handling client: {ex.Message}");
        }
    }
    
    private static bool IsCompleteJson(string message)
    {
        try
        {
            // Simple check: count opening and closing braces
            int openBraces = message.Count(c => c == '{');
            int closeBraces = message.Count(c => c == '}');
            return openBraces > 0 && openBraces == closeBraces;
        }
        catch
        {
            return false;
        }
    }
    
    private void ProcessMessage(string message)
    {
        try
        {
            // Validate and deserialize with safe settings
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                MaxDepth = 10 // Prevent deep nesting attacks
            };
            
            var orderData = JsonSerializer.Deserialize<TabletOrderData>(message, options);
            
            if (orderData != null && ValidateOrderData(orderData))
            {
                OnOrderReceived(new OrderReceivedEventArgs { OrderData = orderData });
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid order data received");
            }
        }
        catch (JsonException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error parsing JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error processing message: {ex.Message}");
        }
    }
    
    private static bool ValidateOrderData(TabletOrderData orderData)
    {
        // Basic validation
        if (orderData.TableId <= 0)
            return false;
        
        if (orderData.Items == null || orderData.Items.Count == 0)
            return false;
        
        foreach (var item in orderData.Items)
        {
            if (item.ProductId <= 0 || item.Quantity <= 0)
                return false;
        }
        
        return true;
    }
    
    protected virtual void OnOrderReceived(OrderReceivedEventArgs e)
    {
        OrderReceived?.Invoke(this, e);
    }
    
    public void Dispose()
    {
        Stop();
        _cancellationTokenSource?.Dispose();
        GC.SuppressFinalize(this);
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
